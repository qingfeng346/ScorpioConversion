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
        //    var builder = new StringBuilder();
        //    builder.Append($@"
        //public {ClassName} Read(TableManager tableManager, String fileName, ScorpioReader reader) {
        //    __ClassName ret = new __ClassName();");
        //    foreach (var field in m_Fields) {
        //        string str = "";
        //        if (field.Array) {
        //            str = @"
        //    {
        //        int number = reader.ReadInt32();
        //        List<__Type> list = new List<__Type> ();
        //        for (int i = 0;i < number; ++i) { list.Add(__FieldRead); }
        //        ret.___Name = list.AsReadOnly();
        //    }";
        //        } else {
        //            str = @"
        //    ret.___Name = __FieldRead;";
        //        }
        //        if (field.Attribute != null && field.Attribute.GetValue("Language").LogicOperation()) {
        //            str = @"
        //    reader.ReadString();
        //    ret.___Name = __FieldRead;";
        //            str = str.Replace("__FieldRead", string.Format("tableManager.getLanguageText(fileName +  \"_{0}_\" + ret._ID)", field.Name));
        //        } else {
        //            str = str.Replace("__FieldRead", field.IsBasic ? "reader.__Read()" : (field.Enum ? "(__Type)reader.ReadInt32()" : "__Type.Read(tableManager, fileName, reader)"));
        //        }
        //        str = str.Replace("__Read", field.IsBasic ? field.Info.ReadFunction : "");
        //        str = str.Replace("__Type", GetCodeType(field.Type));
        //        str = str.Replace("__Index", field.Index.ToString());
        //        str = str.Replace("__Name", field.Name);
        //        builder.Append(str);
        //    }
        //    builder.Append(@"
        //    ret.m_IsInvalid = ret.IsInvalid_impl();
        //    return ret;
        //}");
        //    return builder.ToString();
        return "";
    }
}
public class GenerateTableCSharp : IGenerate {
    protected override string Generate_impl() {
        return "";
    }
}
