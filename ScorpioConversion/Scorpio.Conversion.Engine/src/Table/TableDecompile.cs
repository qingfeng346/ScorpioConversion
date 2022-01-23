using System.IO;
using System.Text;
using System.Collections.Generic;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Scorpio.Conversion {
    using TableClass = Extend.TableClass;
    using TableEnum = Extend.TableEnum;
    public class TableDecompile {
        private Dictionary<string, TableEnum> customEnums = new Dictionary<string, TableEnum>();
        private Dictionary<string, TableClass> customClasses = new Dictionary<string, TableClass>();
        private TableClass tableClass = null;
        public void Decompile(string file, string name, string output, string readerName) {
            customEnums.Clear();
            customClasses.Clear();
            using (var stream = File.OpenRead(file)) {

                var reader = ReaderManager.Instance.Get(readerName);
                reader.Initialize(stream);
                var rowNumber = reader.ReadInt32();
                reader.ReadString();        //MD5
                tableClass = reader.ReadClass();
                var customNumber = reader.ReadInt32();
                for (var i = 0; i < customNumber; ++i) {
                    var typeName = reader.ReadString();
                    if (reader.ReadInt8() == 1) {
                        customEnums[typeName] = reader.ReadEnum();
                    } else {
                        customClasses[typeName] = reader.ReadClass();
                    }
                }
                var workbook = new XSSFWorkbook();
                var sheet = workbook.CreateSheet(name);
                {
                    var row = sheet.CreateRow(0);
                    row.CreateCell(0, CellType.String).SetCellValue("/Name");
                    for (var i = 0; i < tableClass.Fields.Count; ++i) {
                        var field = tableClass.Fields[i];
                        row.CreateCell(i + 1, CellType.String).SetCellValue(field.name);
                    }
                }
                {
                    var row = sheet.CreateRow(1);
                    row.CreateCell(0, CellType.String).SetCellValue("/Type");
                    for (var i = 0; i < tableClass.Fields.Count; ++i) {
                        var field = tableClass.Fields[i];
                        row.CreateCell(i + 1, CellType.String).SetCellValue((field.array ? "array" : "") + field.type);
                    }
                }
                for (var i = 0; i < rowNumber; ++i) {
                    var row = sheet.CreateRow(i + 2);
                    if (i == 0) { row.CreateCell(0, CellType.String).SetCellValue("/Begin"); }
                    for (var j = 0; j < tableClass.Fields.Count; ++j) {
                        var field = tableClass.Fields[j];
                        row.CreateCell(j + 1, CellType.String).SetCellValue(reader.ReadField(field, customEnums, customClasses));
                    }
                }
                using (var fileStream = new FileStream($"{output}/{name}.xlsx", FileMode.Create, FileAccess.ReadWrite)) {
                    workbook.Write(fileStream);
                }
                if (customNumber > 0) {
                    var builder = new StringBuilder();
                    foreach (var pair in customEnums) {
                        builder.Append($@"enum_{pair.Key} = {{");
                        foreach (var element in pair.Value.Elements) {
                            builder.Append($@"
    {element.name} = {element.value},");
                        }
                        builder.Append(@"
}
");
                    }
                    foreach (var pair in customClasses) {
                        builder.Append($@"table_{pair.Key} = {{");
                        for (var i = 0; i < pair.Value.Fields.Count; ++i) {
                            var field = pair.Value.Fields[i];
                            if (field.fieldType == TableClass.FieldType.ENUM || field.fieldType == TableClass.FieldType.CLASS) {
                                builder.Append($@"
    {field.name} = `{i},{field.type},{field.array.ToString().ToLower()}`,");
                            } else {
                                var type = BasicUtil.GetType((BasicEnum)field.fieldType).Name;
                                builder.Append($@"
    {field.name} = `{i},{type},{field.array.ToString().ToLower()}`,");
                            }
                        }
                        builder.Append(@"
}
");
                    }
                    File.WriteAllBytes($"{output}/{name}.sco", Encoding.UTF8.GetBytes(builder.ToString()));
                }
            }
        }
    }
}