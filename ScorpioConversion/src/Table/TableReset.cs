
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
public partial class TableBuilder {
    private const string EnumCheckError = "__EnumCheckError";
    private const string EnumConditional = "N(\"__EnumConditional\")";
    public void Reset(IWorkbook workbook, ISheet sheet, PackageParser parser) {
        mParser = parser;
        LoadLayout(sheet);
        ResetData(workbook, sheet);
    }
    void ResetData(IWorkbook workbook,ISheet sheet) {
        var sheetIndex = workbook.GetSheetIndex(sheet);
        var sheetName = sheet.SheetName;
        var copySheet = workbook.CreateSheet("copySheet");
        var columNumber = 0;
        for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; ++i) {
            var row = sheet.GetRow(i);
            if (row == null) continue;
            var copyRow = copySheet.CreateRow(i);
            columNumber = System.Math.Max(columNumber, row.LastCellNum);
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
        sheet.GetDataValidations().ForEach((data) => { if (data.ErrorBoxTitle != EnumCheckError) { copySheet.AddValidationData(data); } });
        for (int i = 0; i < sheet.SheetConditionalFormatting.NumConditionalFormattings; ++i) {
            var formatting = sheet.SheetConditionalFormatting.GetConditionalFormattingAt(i);
            var had = false;
            for (var j = 0; j < formatting.NumberOfRules; ++j) {
                var rule = formatting.GetRule(j);
                if (rule.Formula1.EndsWith(EnumConditional)) {
                    had = true;
                    break;
                }
            }
            if (!had) {
                copySheet.SheetConditionalFormatting.AddConditionalFormatting(formatting);
            }
        }
        var start = -1;
        for (var i = copySheet.FirstRowNum; i <= copySheet.LastRowNum; ++i) {
            var row = copySheet.GetRow(i);
            if (row == null) { continue; }
            var keyCell = row.GetCellString(0);
            if (keyCell == KEYWORD_BEGIN) {
                start = i;
                break;
            }
        }
        if (start == -1) { return; }
        for (var i = 0; i < mFields.Count; ++i) {
            var field = mFields[i];
            if (field.IsValid && !field.IsArray && field.IsEnum) {
                var cellRange = new CellRangeAddressList(start, 65535, i + 1, i + 1);
                //添加下拉框
                {
                    var helper = sheet.GetDataValidationHelper();
                    var constraint = helper.CreateExplicitListConstraint(mParser.GetEnumList(field.Type));
                    var dataValidation = helper.CreateValidation(constraint, cellRange);
                    dataValidation.SuppressDropDownArrow = true;
                    dataValidation.CreateErrorBox(EnumCheckError, "");
                    copySheet.AddValidationData(dataValidation);
                }
                //添加值验证
                {
                    var lineName = (i + 1).GetLineName();
                    var ands = new List<string>();
                    Array.ForEach(mParser.GetEnumList(field.Type), (value) => { ands.Add($"{lineName}{start + 1}<>\"{value}\""); });
                    var conditional = copySheet.SheetConditionalFormatting.CreateConditionalFormattingRule(string.Format("=AND({0})+{1}", string.Join(",", ands.ToArray()), EnumConditional));
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
    }
}
