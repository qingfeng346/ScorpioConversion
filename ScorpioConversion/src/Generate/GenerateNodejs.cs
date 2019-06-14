using System;
using System.Collections.Generic;
using System.Text;
public class TemplateNodejs {
    public const string Table = @"class __TableName {
    constructor() {
        this.m_count = 0
        this.m_dataArray = {}
    }
    Initialize(fileName, reader) {
        let iRow = TableUtil.ReadHead(reader, fileName, ""__MD5"");
        for (let i = 0; i < iRow; ++i) {
            let pData = __DataName.Read(fileName, reader);
            if (this.Contains(pData.ID())) {
                this.m_dataArray[pData.ID()].Set(pData);
            } else {
                this.m_dataArray[pData.ID()] = pData;
            }
        }
        this.m_count = Object.getOwnPropertyNames(this.m_dataArray).length;
        return this;
    }
    GetValue(ID) {
        if (this.Contains(ID)) return this.m_dataArray[ID];
        //TableUtil.Warning(""__DataName key is not exist "" + ID);
        return null;
    }
    Contains(ID) {
        return this.m_dataArray[ID] != null;
    }
    Datas() {
        return this.m_dataArray;
    }
    GetValueObject(ID) {
        return this.GetValue(ID);
    }
    ContainsObject(ID) {
        return this.Contains(ID);
    }
    GetDatas() {
        return this.Datas();
    }
    Count() {
        return this.m_count;
    }
}
exports.__TableName = __TableName;
";
}
public class GenerateDataNodejs : IGenerate {

    protected override string Generate_impl() {
        return $@"
{AllImports()}
class {ClassName} {{
    {AllFields()}
    {FuncGetData()}
    {FuncIsInvalid()}
    {FuncSet()}
    {FuncToString()}
    {FuncRead()}
}}
exports.{ClassName} = {ClassName};
";
    }
    string AllImports() {
        var builder = new StringBuilder();
        foreach (var field in Fields) {
            if (!field.IsBasic) {
                builder.AppendLine($"const {field.Type} = require('./{field.Type}').{field.Type}");
            }
        }
        return builder.ToString();
    }
    string AllFields() {
        var builder = new StringBuilder();
        var first = true;
        foreach (var field in Fields) {
            builder.Append($@"
    /* {field.Comment}  默认值({field.Default}) */
    get{field.Name}() {{ return this._{field.Name}; }}");
            if (first && (bool)Parameter) {
                first = false;
                builder.Append($@"
    ID() {{ return this._{field.Name}; }}");
            }
        }
        return builder.ToString();
    }
    string FuncGetData() {
        var builder = new StringBuilder();
        builder.Append(@"
    GetData(key) {");
        foreach (var field in Fields) {
            builder.Append($@"
        if (""{field.Name}"" == key) return this._{field.Name};");
        }
        builder.Append(@"
        return null;
    }");
        return builder.ToString();
    }
    string FuncIsInvalid() {
        return @"
    IsInvalid() { return false; }";
    }
    string FuncSet() {
        var builder = new StringBuilder();
        builder.Append($@"
    Set(value) {{");
        foreach (var field in Fields) {
            builder.Append($@"
        this._{field.Name} = value._{field.Name};");
        }
        builder.Append(@"
    }");
        return builder.ToString();
    }
    string FuncToString() {
        var builder = new StringBuilder();
        builder.Append(@"
    ToString() {
        return `{");
        var count = Fields.Count;
        for (int i = 0; i < count; ++i) {
            var field = Fields[i];
            var toString = field.Array ? $"ScorpioUtil.ToString(_{field.Name})" : $"_{field.Name}";
            builder.Append($@"
            {field.Name} : ${{{toString}}}");
            if (i != count - 1) {
                builder.Append(", ");
            }
        }
        builder.Append(@"
            }`;
    }");
        return builder.ToString();
    }
    string FuncRead() {
        var builder = new StringBuilder();
        builder.Append($@"
    static Read(fileName, reader) {{
        let ret = new {ClassName}();");
        foreach (var field in Fields) {
            var languageType = field.GetLanguageType(Language);
            var fieldRead = "";
            if (field.Attribute != null && field.Attribute.GetValue("Language").IsTrue) {
                fieldRead = $@"TableUtil.Readl10n(l10n, fileName + ""_{field.Name}_"" + ret.ID(), reader)";
            } else if (field.IsBasic) {
                fieldRead = $"reader.Read{field.BasicType.Name}()";
            } else if (field.IsEnum) {
                fieldRead = $"reader.ReadInt32()";
            } else {
                fieldRead = $"{languageType}.Read(fileName, reader)";
            }
            if (field.Array) {
                builder.Append($@"
        {{
            let list = new Array();
            let number = reader.ReadInt32();
            for (let i = 0; i < number; ++i) {{ list.push({fieldRead}); }}
            ret._{field.Name} = list;
        }}");
            } else {
                builder.Append($@"
        ret._{field.Name} = {fieldRead};");
            }
        }
        builder.Append(@"
        return ret;
    }");
        return builder.ToString();
    }
}
public class GenerateTableNodejs : IGenerate {
    protected override string Generate_impl() {
        return $@"
const TableUtil = require(""../ScorpioProto/TableUtil"")
const __DataName = require(""./__DataName"").__DataName
{TemplateNodejs.Table}";
    }
}
public class GenerateEnumNodejs : IGenerate {
    protected override string Generate_impl() {
        var builder = new StringBuilder();
        builder.Append($@"//本文件为自动生成，请不要手动修改
exports.{ClassName} = {{");
        foreach (var info in Enums.Fields) {
            builder.Append($@"
    {info.Name} : {info.Index},");
        }
        builder.Append(@"
}");
        return builder.ToString();
    }
}
