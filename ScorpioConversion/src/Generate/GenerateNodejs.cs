using System;
using System.Collections.Generic;
using System.Text;

public class GenerateDataNodejs : IGenerate {
    protected override string Generate_impl() {
        return $@"
const ScorpioReader = require(`./ScorpioReader`)
class {ClassName} {{
    {AllFields()}
    {FuncGetData()}
    {FuncIsInvalid()}
    {FuncRead()}
    {FuncSet()}
    {FuncToString()}
}}";
    }
    string AllFields() {
        var builder = new StringBuilder();
        var first = true;
        foreach (var field in Fields) {
            builder.Append($@"
    /* <summary> {field.Comment}  默认值({field.Default}) </summary> */
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
        return "";
    }
    string FuncRead() {
        return "";
    //    var builder = new StringBuilder();
    //    builder.Append($@"
    //public static {ClassName} Read(Dictionary<string, string> l10n, string fileName, ScorpioReader reader) {{
    //    var ret = new {ClassName}();");
    //    foreach (var field in Fields) {
    //        var languageType = field.GetLanguageType(Language);
    //        var fieldRead = "";
    //        if (field.Attribute != null && field.Attribute.GetValue("Language").LogicOperation()) {
    //            fieldRead = $@"TableUtil.Readl10n(l10n, fileName + ""_{field.Name}_"" + ret.ID(), reader)";
    //        } else if (field.IsBasic){
    //            fieldRead = $"reader.Read{field.BasicType.Name}()";
    //        } else if (field.IsEnum) {
    //            fieldRead = $"({languageType})reader.ReadInt32()";
    //        } else {
    //            fieldRead = $"{languageType}.Read(l10n, fileName, reader)";
    //        }
    //        if (field.Array) {
    //            builder.Append($@"
    //    {{
    //        List<{languageType}> list = new List<{languageType}>();
    //        int number = reader.ReadInt32();
    //        for (int i = 0; i < number; ++i) {{ list.Add({fieldRead}); }}
    //        ret._{field.Name} = list.AsReadOnly();
    //    }}");
    //        } else {
    //            builder.Append($@"
    //    ret._{field.Name} = {fieldRead};");
    //        }
    //    }
    //    builder.Append(@"
    //    ret.m_IsInvalid = ret.CheckInvalid();
    //    return ret;
    //}");
    //    return builder.ToString();
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
}
public class GenerateTableNodejs : IGenerate {
    protected override string Generate_impl() {
        return $@"
{TemplateCSharp.Head}
namespace {PackageName} {{
{TemplateCSharp.Table}
}}";
    }
}
