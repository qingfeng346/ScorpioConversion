
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;

public partial class TableBuilder {
    private const string EnumCheckError = "__EnumCheckError";
    private const string BoolCheckError = "__BoolCheckError";
    private const string EnumConditional = "N(\"__EnumConditional\")";
    private const string BoolConditional = "N(\"__BoolConditional\")";
    private const string COPY_SHEET_NAME = "!__CopySheet";
    public bool Reset(IWorkbook workbook, ISheet sheet, HashSet<string> enumSheets) {
        mParser = Config.Parser;
        LoadLayout(sheet.AsDataSet());
        return ResetData(workbook, sheet, enumSheets);
    }
    string CreateEnumSheet(IWorkbook workbook, string enumType) {
        var enumSheetName = $"!{enumType}";
        var sheet = workbook.GetSheet(enumSheetName);
        if (sheet == null) { sheet = workbook.CreateSheet(enumSheetName); }
        var enumList = mParser.GetEnumList(enumType);
        for (var i = 0; i < enumList.Length; ++i) {
            var row = sheet.GetRow(i);
            if (row == null) { row = sheet.CreateRow(i); }
            row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(enumList[i]);
        }
        for (var i = enumList.Length; i < sheet.LastRowNum; ++i) {
            var row = sheet.GetRow(i);
            if (row != null) { sheet.RemoveRow(row); }
        }
        return enumSheetName;
    }
    bool ResetData(IWorkbook workbook, ISheet sheet, HashSet<string> enumSheets) {
        //不存在枚举
        if (!Fields.Exists((field) => field.IsValid && !field.IsArray && (field.IsEnum || field.IsBool))) { return false; }
        var start = -1;
        for (var i = sheet.FirstRowNum; i <= sheet.LastRowNum; ++i) {
            var row = sheet.GetRow(i);
            if (row == null) { continue; }
            var keyCell = row.GetCellString(0);
            if (keyCell == KEYWORD_BEGIN) {
                start = i;
                break;
            }
        }
        //没有有效行 计算出 首个 /Begin 
        if (start == -1) { return false; }

        workbook.RemoveSheet(COPY_SHEET_NAME);
        var sheetIndex = workbook.GetSheetIndex(sheet);
        var sheetName = sheet.SheetName;
        var copySheet = workbook.CreateSheet(COPY_SHEET_NAME);
        var columNumber = 0;
        for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; ++i) {
            var row = sheet.GetRow(i);
            if (row == null) continue;
            var copyRow = copySheet.CreateRow(i);
            columNumber = Math.Max(columNumber, row.LastCellNum);
            for (int j = 0; j < row.LastCellNum; ++j) {
                var cell = row.GetCell(j);
                if (cell == null) { continue; }
                var copyCell = copyRow.CreateCell(j);
                if (cell.CellStyle != null) { copyCell.CellStyle = cell.CellStyle; }
                if (cell.CellComment != null) { copyCell.CellComment = cell.CellComment; }
                if (cell.Hyperlink != null) { copyCell.Hyperlink = cell.Hyperlink; }
                copyCell.SetCellType(cell.CellType);
                switch (cell.CellType) {
                    case CellType.Numeric: copyCell.SetCellValue(cell.NumericCellValue); break;
                    case CellType.String: copyCell.SetCellValue(cell.RichStringCellValue); break;
                    case CellType.Formula: copyCell.SetCellFormula(cell.CellFormula); break;
                    case CellType.Blank: copyCell.SetCellValue(cell.StringCellValue); break;
                    case CellType.Boolean: copyCell.SetCellValue(cell.BooleanCellValue); break;
                    case CellType.Error: copyCell.SetCellErrorValue(cell.ErrorCellValue); break;
                }
            }
        }
        for (int i = 0; i < columNumber; ++i) {
            copySheet.SetColumnWidth(i, sheet.GetColumnWidth(i));
        }
        //复制数据有效性验证 排除Enum 创建的数据有效性
        sheet.GetDataValidations().ForEach((data) => { if (data.ErrorBoxTitle != EnumCheckError && data.ErrorBoxTitle != BoolCheckError) { copySheet.AddValidationData(data); } });
        //复制条件格式 排除Enum 创建的数据有效性
        for (int i = 0; i < sheet.SheetConditionalFormatting.NumConditionalFormattings; ++i) {
            var formatting = sheet.SheetConditionalFormatting.GetConditionalFormattingAt(i);
            var generateConditional = false;
            for (var j = 0; j < formatting.NumberOfRules; ++j) {
                var formula = formatting.GetRule(j).Formula1;
                if (formula.EndsWith(EnumConditional) || formula.EndsWith(BoolConditional)) {
                    generateConditional = true;
                    break;
                }
            }
            if (!generateConditional) {
                copySheet.SheetConditionalFormatting.AddConditionalFormatting(formatting);
            }
        }

        var dataValidationHelper = copySheet.GetDataValidationHelper();
        //查找枚举列
        for (var i = 0; i < Fields.Count; ++i) {
            var field = Fields[i];
            if (!field.IsValid || field.IsArray || (!field.IsEnum && !field.IsBool)) { continue; }
            var cellRange = new CellRangeAddressList(start, 65535, i + 1, i + 1);
            if (field.IsEnum) {
                var enumSheetName = CreateEnumSheet(workbook, field.Type);
                enumSheets.Add(enumSheetName);
                //添加下拉框
                {
                    var constraint = dataValidationHelper.CreateExplicitListConstraint(new string[] { "" });
                    constraint.Formula1 = $"='{enumSheetName}'!$A:$A";
                    var dataValidation = dataValidationHelper.CreateValidation(constraint, cellRange);
                    dataValidation.SuppressDropDownArrow = true;
                    dataValidation.CreateErrorBox(EnumCheckError, "");
                    copySheet.AddValidationData(dataValidation);
                }
                //添加值验证
                {
                    var lineName = (i + 1).GetLineName() + (start + 1);
                    var rule = $"=NOT(OR(COUNTIF('{enumSheetName}'!$A:$A,{lineName})<>0,{lineName}=\"\"))+{EnumConditional}";
                    var conditional = copySheet.SheetConditionalFormatting.CreateConditionalFormattingRule(rule);
                    var pattern = conditional.CreatePatternFormatting();
                    pattern.FillBackgroundColor = IndexedColors.Red.Index;
                    pattern.FillPattern = FillPattern.SolidForeground;
                    copySheet.SheetConditionalFormatting.AddConditionalFormatting(cellRange.CellRangeAddresses, conditional);
                }
            } else if (field.IsBool) {
                //添加下拉框
                {
                    var dataValidation = dataValidationHelper.CreateValidation(dataValidationHelper.CreateExplicitListConstraint(new string[] { "TRUE", "FALSE" }), cellRange);
                    dataValidation.SuppressDropDownArrow = true;
                    dataValidation.CreateErrorBox(BoolCheckError, "");
                    copySheet.AddValidationData(dataValidation);
                }
                //添加值验证
                {
                    var lineName = (i + 1).GetLineName() + (start + 1);
                    var conditionalList = new string[] { "TRUE", "FALSE", "\"TRUE\"", "\"FALSE\"", "\"\"" };
                    for (var j = 0; j < conditionalList.Length; ++j) { conditionalList[j] = $"{lineName}<>{conditionalList[j]}"; }
                    var conditional = copySheet.SheetConditionalFormatting.CreateConditionalFormattingRule(
                        $"=AND({string.Join(',',conditionalList)})+{BoolConditional}");
                    var pattern = conditional.CreatePatternFormatting();
                    pattern.FillBackgroundColor = IndexedColors.Red.Index;
                    pattern.FillPattern = FillPattern.SolidForeground;
                    copySheet.SheetConditionalFormatting.AddConditionalFormatting(cellRange.CellRangeAddresses, conditional);
                }
            }
        }
        workbook.RemoveSheetAt(sheetIndex);
        workbook.SetSheetOrder(copySheet.SheetName, sheetIndex);
        workbook.SetSheetName(sheetIndex, sheetName);

        //var dataValidations = sheet.GetDataValidations();
        //var dataValidationHelper = sheet.GetDataValidationHelper();

        //for (var i = 0; i < mFields.Count; ++i) {
        //    var field = mFields[i];
        //    if (!field.IsValid || field.IsArray || !field.IsEnum) { continue; }
        //    var enumKey = $"{EnumCheckError}_{field.Name}";
        //    var enumList = mParser.GetEnumList(field.Type);
        //    var index = dataValidations.FindIndex(_ => _.ErrorBoxText == enumKey);
        //    if (index >= 0) {
        //        var dataValidation = dataValidations[index];
        //        if (enumList.StringArrayEqual(dataValidation.ValidationConstraint.ExplicitListValues)) { continue; }
        //        //dataValidations.RemoveAt(index);
        //        //dataValidation.ValidationConstraint.ExplicitListValues = enumList;
        //        //continue;
        //    }
        //    //作用空间
        //    var cellRange = new CellRangeAddressList(start, 32767, i + 1, i + 1);
        //    //添加下拉框
        //    {
        //        var dataValidation = dataValidationHelper.CreateValidation(dataValidationHelper.CreateExplicitListConstraint(enumList), cellRange);
        //        dataValidation.SuppressDropDownArrow = true;
        //        dataValidation.CreateErrorBox("", enumKey);
        //        sheet.AddValidationData(dataValidation);
        //    }
        //    //添加值验证
        //    //{
        //    //    var lineName = (i + 1).GetLineName();
        //    //    var ands = mParser.GetEnumList(fieled.Type).Select(_ => $"{lineName}{start + 1}<>\"{_}\"").ToArray();
        //    //    var conditional = copySheet.SheetConditionalFormatting.CreateConditionalFormattingRule(
        //    //        $"=AND({string.Join(",", ands)}, {lineName}{start + 1} <> \"\")+{EnumConditional}");

        //    //    var pattern = conditional.CreatePatternFormatting();
        //    //    pattern.FillBackgroundColor = IndexedColors.Red.Index;
        //    //    pattern.FillPattern = FillPattern.SolidForeground;
        //    //    copySheet.SheetConditionalFormatting.AddConditionalFormatting(cellRange.CellRangeAddresses, conditional);
        //    //}
        //}
        //}
        //workbook.RemoveSheet(COPY_SHEET_NAME);
        //var sheetIndex = workbook.GetSheetIndex(sheet);
        //var sheetName = sheet.SheetName;
        //var copySheet = workbook.CreateSheet(COPY_SHEET_NAME);
        //var columNumber = 0;
        //for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; ++i) {
        //    var row = sheet.GetRow(i);
        //    if (row == null) continue;
        //    var copyRow = copySheet.CreateRow(i);
        //    columNumber = Math.Max(columNumber, row.LastCellNum);
        //    for (int j = 0; j < row.LastCellNum; ++j) {
        //        var cell = row.GetCell(j);
        //        if (cell == null) { continue; }
        //        var copyCell = copyRow.CreateCell(j);
        //        if (cell.CellStyle != null) { copyCell.CellStyle = cell.CellStyle; }
        //        if (cell.CellComment != null) { copyCell.CellComment = cell.CellComment; }
        //        if (cell.Hyperlink != null) { copyCell.Hyperlink = cell.Hyperlink; }
        //        copyCell.SetCellType(cell.CellType);
        //        switch (cell.CellType) {
        //            case CellType.Numeric: copyCell.SetCellValue(cell.NumericCellValue); break;
        //            case CellType.String: copyCell.SetCellValue(cell.RichStringCellValue); break;
        //            case CellType.Formula: copyCell.SetCellFormula(cell.CellFormula); break;
        //            case CellType.Blank: copyCell.SetCellValue(cell.StringCellValue); break;
        //            case CellType.Boolean: copyCell.SetCellValue(cell.BooleanCellValue); break;
        //            case CellType.Error: copyCell.SetCellErrorValue(cell.ErrorCellValue); break;
        //        }
        //    }
        //}
        //for (int i = 0; i < columNumber; ++i) {
        //    copySheet.SetColumnWidth(i, sheet.GetColumnWidth(i));
        //}
        //sheet.GetDataValidations().ForEach((data) => { if (data.ErrorBoxTitle != EnumCheckError) { copySheet.AddValidationData(data); } });
        //for (int i = 0; i < sheet.SheetConditionalFormatting.NumConditionalFormattings; ++i) {
        //    var formatting = sheet.SheetConditionalFormatting.GetConditionalFormattingAt(i);
        //    var had = false;
        //    for (var j = 0; j < formatting.NumberOfRules; ++j) {
        //        var rule = formatting.GetRule(j);
        //        if (rule.Formula1.EndsWith(EnumConditional)) {
        //            had = true;
        //            break;
        //        }
        //    }
        //    if (!had) {
        //        copySheet.SheetConditionalFormatting.AddConditionalFormatting(formatting);
        //    }
        //}
        //for (var i = 0; i < mFields.Count; ++i) {
        //    var field = mFields[i];
        //    if (field.IsValid && !field.IsArray && field.IsEnum) {
        //        var cellRange = new CellRangeAddressList(start, 65535, i + 1, i + 1);
        //        //添加下拉框
        //        {
        //            var helper = sheet.GetDataValidationHelper();
        //            var enumList = mParser.GetEnumList(field.Type);
        //            var constraint = helper.CreateExplicitListConstraint(enumList);
        //            var dataValidation = helper.CreateValidation(constraint, cellRange);
        //            dataValidation.SuppressDropDownArrow = true;
        //            dataValidation.CreateErrorBox(EnumCheckError, "");
        //            Logger.info(constraint.Formula1.Length + "    " + constraint.Formula1);
        //            var str = string.Join(",", enumList);
        //            Logger.info(str.Length + "    " + str);
        //            copySheet.AddValidationData(dataValidation);
        //        }
        //        //添加值验证
        //        //{
        //        //    var lineName = (i + 1).GetLineName();
        //        //    var ands = mParser.GetEnumList(field.Type).Select(_ => $"{lineName}{start + 1}<>\"{_}\"").ToArray();
        //        //    var conditional = copySheet.SheetConditionalFormatting.CreateConditionalFormattingRule(
        //        //        $"=AND({string.Join(",", ands)}, {lineName}{start + 1} <> \"\")+{EnumConditional}");

        //        //    var pattern = conditional.CreatePatternFormatting();
        //        //    pattern.FillBackgroundColor = IndexedColors.Red.Index;
        //        //    pattern.FillPattern = FillPattern.SolidForeground;
        //        //    copySheet.SheetConditionalFormatting.AddConditionalFormatting(cellRange.CellRangeAddresses, conditional);
        //        //}
        //    }
        //}
        //workbook.RemoveSheetAt(sheetIndex);
        //workbook.SetSheetOrder(copySheet.SheetName, sheetIndex);
        //workbook.SetSheetName(sheetIndex, sheetName);
        return true;
    }
    public static void Format(IWorkbook workbook, ISheet sheet) {
        FormatData(workbook, sheet);
    }
    private static void FormatData(IWorkbook workbook,ISheet sheet) {
        //标题字，黑字
        var headerFont = workbook.CreateFont();
        headerFont.FontName = "宋体";
        headerFont.Color = IndexedColors.Black.Index;
        headerFont.FontHeightInPoints = 11;
        //标识字，黄斜粗字
        var keyFont = workbook.CreateFont();
        keyFont.FontName = "宋体";
        keyFont.Color = IndexedColors.Gold.Index;
        keyFont.FontHeightInPoints = 11;
        keyFont.IsItalic = true;
        //列标题字，黄字
        var columnFont = workbook.CreateFont();
        columnFont.FontName = "宋体";
        columnFont.Color = IndexedColors.Yellow.Index;
        columnFont.FontHeightInPoints = 11;
        //标题样式，黑底黄字
        var headerStyle = workbook.CreateCellStyle();
        headerStyle.FillForegroundColor = IndexedColors.Black.Index;
        headerStyle.FillPattern = FillPattern.SolidForeground;
        headerStyle.SetFont(columnFont);
        //注释样式，灰底黑字
        var commentStyle = workbook.CreateCellStyle();
        commentStyle.FillForegroundColor = IndexedColors.Grey25Percent.Index;
        commentStyle.FillPattern = FillPattern.SolidForeground;
        commentStyle.SetFont(headerFont);
        //标识样式，蓝底黄字
        var keyStyle = workbook.CreateCellStyle();
        keyStyle.FillForegroundColor = IndexedColors.Indigo.Index;
        keyStyle.FillPattern = FillPattern.SolidForeground;
        keyStyle.SetFont(keyFont);
        //各行标识位设定
        var columnNumber = 0;
        string[] rowTypeList = new string[sheet.LastRowNum+1];
        bool[] contentTypeList = new bool[sheet.LastRowNum+1];
        for(var i = 0; i < sheet.LastRowNum+1; ++i){
            contentTypeList[i] = false;
        }
        var lastKeyRowNum = -1;
        var firstBeginNum = -1;
        var isContent = false;
        //计算并确认各行的样式
        for (var i = sheet.FirstRowNum; i <= sheet.LastRowNum; ++i) {
            var row = sheet.GetRow(i);
            if (row == null) {row = sheet.CreateRow(i);}
            //计算整体表格最宽的表格
            columnNumber = Math.Max(columnNumber, row.LastCellNum);
            //划定标识行
            var keyCell = row.GetCellString(0);
            switch(keyCell) {
                case KEYWORD_NAME: rowTypeList[i] = "Name"; lastKeyRowNum = Math.Max(lastKeyRowNum, i); break;
                case KEYWORD_COMMENT: rowTypeList[i] = "Comment"; lastKeyRowNum = Math.Max(lastKeyRowNum, i); break;
                case KEYWORD_DEFAULT: rowTypeList[i] = "Comment"; lastKeyRowNum = Math.Max(lastKeyRowNum, i); break;
                case KEYWORD_TYPE: rowTypeList[i] = "Comment"; lastKeyRowNum = Math.Max(lastKeyRowNum, i); break;
                case KEYWORD_BEGIN:
                    rowTypeList[i] = "";
                    if(firstBeginNum < 0) {firstBeginNum = i;};
                    contentTypeList[i] = true;
                    isContent = true;
                    break;
                case KEYWORD_END:
                    rowTypeList[i] = "";
                    contentTypeList[i] = true;
                    isContent = false;
                    break;
                default: 
                    rowTypeList[i] = "";
                    if(keyCell != null & firstBeginNum < 0){
                        if(keyCell.Length >=1) {
                            if(keyCell.Substring(0,1).Equals("/")){
                                lastKeyRowNum = Math.Max(lastKeyRowNum, i);
                            }
                        }
                    }
                    if(isContent) {
                        contentTypeList[i] = true;
                    }
                    break;
            }
        }
        //涂行样式
        for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; ++i) {
            var row = sheet.GetRow(i);
            if(row == null) continue;
            for (int j = 0; j < columnNumber; ++j) {
                var cell = row.GetCell(j);
                if(cell == null) cell = row.CreateCell(j);
                switch(rowTypeList[i]){
                    case "Name": cell.CellStyle = headerStyle; break;
                    case "Comment": cell.CellStyle = commentStyle; break;
                    default: break;
                }
            }
        }
        //涂第一列的表头样式
        for (int i = sheet.FirstRowNum; i <= lastKeyRowNum; ++i) {
            var row = sheet.GetRow(i);
            if(row == null) { row = sheet.CreateRow(i); }
            var cell = row.GetCell(0);
            if(cell == null) { cell = row.CreateCell(0); }
            if(rowTypeList[i] == "") {
                cell.CellStyle = keyStyle;
            }
        }
        for (var i = 0; i < sheet.LastRowNum + 1; i++) {
            if (contentTypeList[i]){
                var row = sheet.GetRow(i);
                if(row == null) { row = sheet.CreateRow(i); }
                var cell = row.GetCell(0);
                if(cell == null) { cell = row.CreateCell(0); }
                cell.CellStyle = keyStyle;
            }
        }
        //设置冻结单元格
        sheet.CreateFreezePane(1, lastKeyRowNum+1, 1, lastKeyRowNum+1);
    }
}