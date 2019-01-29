using System;
using System.Collections.Generic;
using System.Text;

public class GenerateDataJava : IGenerate {
    private List<FieldClass> fields;
    protected override string Generate_impl() {
        fields = (Package as PackageClass).Fields;
        var builder = new StringBuilder();
        builder.Append($@"
package {PackageName};
{TemplateJava.Head}
public class {ClassName} implements IData {{
    private boolean m_IsInvalid;
    {AllFields()}
    {FuncGetData()}
    {FuncIsInvalid()}
    {FuncRead()}
    {FuncSet()}
    {fields.ToJavaString()}
}}");
        return builder.ToString();
    }
    string AllFields() {
        var builder = new StringBuilder();
        var first = true;
        foreach (var field in fields) {
            var languageType = field.GetLanguageType(Language);
            if (field.Array) { languageType = $"List<{languageType}>"; }
            builder.Append($@"
    private {languageType} _{field.Name};
    /** {field.Comment}  默认值({field.Default}) */
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
    public Object GetData(String key ) {");
        foreach (var field in fields) {
            builder.Append($@"
        if (""{field.Name}"".equals(key)) return _{field.Name};");
        }
        builder.Append(@"
        return null;
    }");
        return builder.ToString();
    }
    string FuncIsInvalid() {
        var builder = new StringBuilder();
        builder.Append(@"
    public boolean IsInvalid() { return m_IsInvalid; }
    private boolean CheckInvalid() {");
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
    public static {ClassName} Read(Map<String, String> l10n, String fileName, ScorpioReader reader) {{
        {ClassName} ret = new {ClassName}();");
        foreach (var field in fields) {
            var languageType = field.GetLanguageType(Language);
            var fieldRead = "";
            if (field.Attribute != null && field.Attribute.GetValue("Language").LogicOperation()) {
                fieldRead = $@"TableUtil.Readl10n(l10n, fileName + ""_{field.Name}_"" + ret.ID(), reader)";
            } else if (field.IsBasic) {
                fieldRead = $"reader.Read{field.BasicType.Name}()";
            } else if (field.IsEnum) {
                fieldRead = $"({languageType})reader.ReadInt32()";
            } else {
                fieldRead = $"{languageType}.Read(l10n, fileName, reader)";
            }
            if (field.Array) {
                builder.Append($@"
        {{
            ArrayList<{languageType}> list = new ArrayList<{languageType}>();
            int number = reader.ReadInt32();
            for (int i = 0; i < number; ++i) {{ list.Add({fieldRead}); }}
            ret._{field.Name} = Collections.unmodifiableList(list);
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
public class GenerateTableJava : IGenerate {
    protected override string Generate_impl() {
        var builder = new StringBuilder();
        builder.Append(TemplateCSharp.Head);
        builder.AppendLine("namespace " + PackageName + " {");
        builder.Append(TemplateCSharp.Table);
        builder.AppendLine("}");
        return builder.ToString();
    }
}
