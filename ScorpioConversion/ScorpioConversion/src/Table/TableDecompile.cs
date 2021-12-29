using System.IO;
using System.Text;
using System.Collections.Generic;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

public class TableDecompile {
//    public void Decompile(string file, string name, string output) {
//        customEnums.Clear();
//        customClasses.Clear();
//        var reader = new TableReader(File.ReadAllBytes(file));
//        var rowNumber = reader.ReadInt32();
//        reader.ReadString();        //MD5
//        tableClass = ReadClass(reader);
//        var customNumber = reader.ReadInt32();
//        for (var i = 0; i < customNumber; ++i) {
//            var typeName = reader.ReadString();
//            if (reader.ReadInt8() == 1) {
//                customEnums[typeName] = ReadEnum(reader);
//            } else {
//                customClasses[typeName] = ReadClass(reader);
//            }
//        }
//        var workbook = new XSSFWorkbook();
//        var sheet = workbook.CreateSheet(name);
//        {
//            var row = sheet.CreateRow(0);
//            row.CreateCell(0, CellType.String).SetCellValue("/Name");
//            for (var i = 0; i < tableClass.Fields.Count; ++i) {
//                var field = tableClass.Fields[i];
//                row.CreateCell(i + 1, CellType.String).SetCellValue(field.name);
//            }
//        }
//        {
//            var row = sheet.CreateRow(1);
//            row.CreateCell(0, CellType.String).SetCellValue("/Type");
//            for (var i = 0; i < tableClass.Fields.Count; ++i) {
//                var field = tableClass.Fields[i];
//                row.CreateCell(i + 1, CellType.String).SetCellValue((field.array ? "array" : "") + field.type);
//            }
//        }
//        for (var i = 0; i < rowNumber; ++i) {
//            var row = sheet.CreateRow(i + 2);
//            if (i == 0) { row.CreateCell(0, CellType.String).SetCellValue("/Begin"); }
//            for (var j = 0; j < tableClass.Fields.Count; ++j) {
//                var field = tableClass.Fields[j];
//                row.CreateCell(j + 1, CellType.String).SetCellValue(ReadField(reader, field));
//            }
//        }
//        using (var fileStream = new FileStream($"{output}/{name}.xlsx", FileMode.Create, FileAccess.ReadWrite)) {
//            workbook.Write(fileStream);
//        }
//        if (customNumber > 0) {
//            var builder = new StringBuilder();
//            foreach (var pair in customEnums) {
//                builder.Append($@"enum_{pair.Key} = {{");
//                foreach (var element in pair.Value.elements) {
//                    builder.Append($@"
//    {element.name} = {element.value},");
//                }
//                builder.Append(@"
//}
//");
//            }
//            foreach (var pair in customClasses) {
//                builder.Append($@"table_{pair.Key} = {{");
//                for (var i = 0; i < pair.Value.Fields.Count; ++i) {
//                    var field = pair.Value.Fields[i];
//                    builder.Append($@"
//    {field.name} = `{i},{field.type},{field.array.ToString().ToLower()}`,");
//                }
//                builder.Append(@"
//}
//");
//            }
//            File.WriteAllBytes($"{output}/{name}.sco", Encoding.UTF8.GetBytes(builder.ToString()));
//        }
//    }
//    string ReadField(TableReader reader, TableField field) {
//        if (field.array) {
//            var values = new List<string>();
//            var number = reader.ReadInt32();
//            for (var i = 0; i < number; ++i) {
//                var value = ReadOneField(reader, field);
//                if (field is TableFieldClass) {
//                    values.Add($"[{value}]");
//                } else {
//                    values.Add(value);
//                }
//            }
//            return string.Join(';', values);
//        } else {
//            return ReadOneField(reader, field);
//        }
//    }
//    string ReadOneField(TableReader reader, TableField field) {
//        if (field is TableFieldBasic) {
//            return (field as TableFieldBasic).basicType.ReadValue(reader).ToString();
//        } else if (field is TableFieldEnum) {
//            return customEnums[field.type].Get(reader.ReadInt32());
//        } else if (field is TableFieldClass) {
//            var values = new List<string>();
//            var customClass = customClasses[field.type];
//            foreach (var cField in customClass.Fields) {
//                var value = ReadField(reader, cField);
//                if (cField is TableFieldClass) {
//                    values.Add($"[{value}]");
//                } else {
//                    values.Add(value);
//                }
//            }
//            return string.Join(';', values);
//        }
//        throw new System.Exception("不支持的字段类型 : " + field);
//    }
}
