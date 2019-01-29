using System;
using System.Collections.Generic;
using System.Text;

public class GenerateDataCSharp : IGenerate {
    private List<FieldClass> fields;
    protected override string Generate_impl() {
        fields = (Package as PackageClass).Fields;
        var builder = new StringBuilder();
        builder.Append(TemplateCSharp.Head);
        builder.Append($@"
namespace {PackageName} {{
public class {ClassName} : IData {{
    private bool m_IsInvalid;
    {AllFields()}
    {FuncGetData()}
    {FuncIsInvalid()}
    {FuncRead()}
    {FuncSet()}
    {fields.Builder()}
}}
}}");
        return builder.ToString();
    }
    string AllFields() {
        var builder = new StringBuilder();
        var first = true;
        foreach (var field in fields) {
            var languageType = field.GetLanguageType(Language);
            if (field.Array) { languageType = $"ReadOnlyCollection<{languageType}>"; }
            builder.Append($@"
    private {languageType} _{field.Name};
    /* <summary> {field.Comment}  默认值({field.Default}) </summary> */
    public {languageType} get{field.Name}() {{ return _{field.Name}; }}");
            if (first && (bool)Parameter) {
                first = false;
                builder.Append($@"
    public {languageType} ID() {{ return _{field.Name}; }}");
            }
        }
        return builder.ToString();
    }
    string FuncGetData() {
        var builder = new StringBuilder();
        builder.Append(@"
    public object GetData(string key ) {");
        foreach (var field in fields) {
            builder.Append($@"
        if (key == ""{field.Name}"") return _{field.Name};");
        }
        builder.Append(@"
        return null;
    }");
        return builder.ToString();
    }
    string FuncIsInvalid() {
        var builder = new StringBuilder();
        builder.Append(@"
    public bool IsInvalid() { return m_IsInvalid; }
    private bool CheckInvalid() {");
        foreach (var field in fields) {
            builder.Append($@"
        if (!TableUtil.IsInvalid(this._{field.Name})) return false;");
        }
        builder.Append(@"
        return true;
    }");
        return builder.ToString();
    }
    string FuncRead() {
        var builder = new StringBuilder();
        builder.Append($@"
    public static {ClassName} Read(Dictionary<string, string> l10n, string fileName, ScorpioReader reader) {{
        var ret = new {ClassName}();");
        foreach (var field in fields) {
            var fieldRead = "";
            if (field.Attribute != null && field.Attribute.GetValue("Language").LogicOperation()) {
                fieldRead = $@"TableUtil.Readl10n(l10n, fileName + ""_{field.Name}_"" + ret.ID(), reader)";
            } else if (field.IsBasic){
                fieldRead = $"reader.Read{field.BasicType.Name}()";
            } else if (field.IsEnum) {
                fieldRead = $"({field.Type})reader.ReadInt32()";
            } else {
                fieldRead = $"{field.Type}.Read(l10n, fileName, reader)";
            }
            if (field.Array) {
                builder.Append($@"
        {{
            var list = new List<{field.Type}> ();
            var number = reader.ReadInt32();
            for (var i = 0; i < number; ++i) {{ list.Add({fieldRead}); }}
            ret._{field.Name} = list.AsReadOnly();
        }}");
            } else {
                builder.Append($@"
        ret._{field.Name} = {fieldRead};");
            }
        }
        builder.Append(@"
        ret.m_IsInvalid = ret.CheckInvalid();
        return ret;
    }");
        return builder.ToString();
    }
    string FuncSet() {
        var builder = new StringBuilder();
        builder.Append($@"
    public void Set({ClassName} value) {{");
        foreach (var field in fields) {
            builder.Append($@"
        this._{field.Name} = value._{field.Name};");
        }
        builder.Append(@"
    }");
        return builder.ToString();
    }
}
public class GenerateTableCSharp : IGenerate {
    protected override string Generate_impl() {
        var builder = new StringBuilder();
        builder.Append(TemplateCSharp.Head);
        builder.AppendLine("namespace " + PackageName + " {");
        builder.Append(TemplateCSharp.Table);
        builder.AppendLine("}");
        return builder.ToString();
    }
}
