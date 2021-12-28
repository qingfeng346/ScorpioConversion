using System.IO;
using System.Text;
using System.Collections.Generic;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

public class TableDecompile {
    class TableEnumElement {
        public string name;
        public int value;
        public override string ToString() { return $"{name}({value})"; }
    }
    class TableEnum {
        public List<TableEnumElement> elements = new List<TableEnumElement>();
        public string Get(int value) {
            return elements.Find(_ => _.value == value).name;
        }
        public int Get(string name) {
            return elements.Find(_ => _.name == name).value;
        }
    }
    class TableField {
        public bool array;
        public string name;
        public virtual string type { get; set; }
    }
    class TableFieldBasic : TableField {
        public BasicType basicType;
        public override string type { get => basicType.Key; set { } }
        public override string ToString() { return $"{name}-({basicType.Key})" + (array ? "[]" : ""); }
    }
    class TableFieldEnum : TableField {
        public override string ToString() { return $"{name}-Enum({type})" + (array ? "[]" : ""); }
    }
    class TableFieldClass : TableField {
        public override string ToString() { return $"{name}-Class({type})" + (array ? "[]" : ""); }
    }
    class TableClass {
        public List<TableField> Fields = new List<TableField>();
    }
    private Dictionary<string, TableEnum> customEnums = new Dictionary<string, TableEnum>();
    private Dictionary<string, TableClass> customClasses = new Dictionary<string, TableClass>();
    private TableClass tableClass = null;
    TableClass ReadClass(TableReader reader) {
        var tableClass = new TableClass();
        var number = reader.ReadInt32();
        for (var i = 0; i < number; ++i) {
            TableField field = null;
            switch (reader.ReadInt8()) {
                case 0: field = new TableFieldBasic() { basicType = BasicUtil.GetType((BasicEnum)reader.ReadInt8()) }; break;
                case 1: field = new TableFieldEnum() { type = reader.ReadString() }; break;
                case 2: field = new TableFieldClass() { type = reader.ReadString() }; break;
            }
            field.array = reader.ReadBool();
            field.name = reader.ReadString();
            tableClass.Fields.Add(field);
        }
        return tableClass;
    }
    TableEnum ReadEnum(TableReader reader) {
        var tableEnum = new TableEnum();
        var number = reader.ReadInt32();
        for (var i = 0; i < number; ++i) {
            tableEnum.elements.Add(new TableEnumElement() { name = reader.ReadString(), value = reader.ReadInt32() });
        }
        return tableEnum;
    }
    public void Decompile(string file, string name, string output) {
        customEnums.Clear();
        customClasses.Clear();
        var reader = new TableReader(File.ReadAllBytes(file));
        var rowNumber = reader.ReadInt32();
        reader.ReadString();        //MD5
        tableClass = ReadClass(reader);
        var customNumber = reader.ReadInt32();
        for (var i = 0; i < customNumber; ++i) {
            var typeName = reader.ReadString();
            if (reader.ReadInt8() == 1) {
                customEnums[typeName] = ReadEnum(reader);
            } else {
                customClasses[typeName] = ReadClass(reader);
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
                row.CreateCell(j + 1, CellType.String).SetCellValue(ReadField(reader, field));
            }
        }
        using (var fileStream = new FileStream($"{output}/{name}.xlsx", FileMode.Create, FileAccess.ReadWrite)) {
            workbook.Write(fileStream);
        }
        if (customNumber > 0) {
            var builder = new StringBuilder();
            foreach (var pair in customEnums) {
                builder.Append($@"enum_{pair.Key} = {{");
                foreach (var element in pair.Value.elements) {
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
                    builder.Append($@"
    {field.name} = `{i},{field.type},{field.array.ToString().ToLower()}`,");
                }
                builder.Append(@"
}
");
            }
            File.WriteAllBytes($"{output}/{name}.sco", Encoding.UTF8.GetBytes(builder.ToString()));
        }
    }
    string ReadField(TableReader reader, TableField field) {
        if (field.array) {
            var values = new List<string>();
            var number = reader.ReadInt32();
            for (var i = 0; i < number; ++i) {
                var value = ReadOneField(reader, field);
                if (field is TableFieldClass) {
                    values.Add($"[{value}]");
                } else {
                    values.Add(value);
                }
            }
            return string.Join(';', values);
        } else {
            return ReadOneField(reader, field);
        }
    }
    string ReadOneField(TableReader reader, TableField field) {
        if (field is TableFieldBasic) {
            return (field as TableFieldBasic).basicType.ReadValue(reader).ToString();
        } else if (field is TableFieldEnum) {
            return customEnums[field.type].Get(reader.ReadInt32());
        } else if (field is TableFieldClass) {
            var values = new List<string>();
            var customClass = customClasses[field.type];
            foreach (var cField in customClass.Fields) {
                var value = ReadField(reader, cField);
                if (cField is TableFieldClass) {
                    values.Add($"[{value}]");
                } else {
                    values.Add(value);
                }
            }
            return string.Join(';', values);
        }
        throw new System.Exception("不支持的字段类型 : " + field);
    }
}
