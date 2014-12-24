using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
public class GenerateMessageCS : IGenerate
{
    public GenerateMessageCS(string className, string package, List<PackageField> fields) : 
        base(className, package, fields, Code.CSharp)
    { }
    public override string Generate()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"
public class __ClassName : IData {");
        builder.Append(GenerateMessageFields());
        builder.Append(GenerateMessageRead());
        builder.Append(@"
}");
        builder.Replace("__ClassName", m_ClassName);
        builder.Replace("__Package", m_Package);
        return builder.ToString();
    }
    string GenerateMessageFields()
    {
        StringBuilder builder = new StringBuilder();
        foreach (var field in m_Fields)
        {
            string str = "";
            if (field.Array) {
                str = @"
    private ReadOnlyCollection<__Type> ___Name;
    public ReadOnlyCollection<__Type> get__Name() { return ___Name; }";
            } else {
                str = @"
    private __Type ___Name;
    public __Type get__Name() { return ___Name; }";
            }
            str = str.Replace("__Name", field.Name);
            str = str.Replace("__Type", GetCodeType(field.Type));
            builder.Append(str);
        }
        return builder.ToString();
    }
    string GenerateMessageRead()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"
    public static __ClassName Read(ScorpioReader reader) {
        __ClassName ret = new __ClassName();");
        foreach (var field in m_Fields) {
            string str = "";
            if (field.Array) {
                str = @"
        {
            List<__TypeName> list = new List<__TypeName> ();
            for (int i = 0;i < reader.ReadInt32(); ++i) { list.Add(__FieldRead); }
            ret.___Name = list.AsReadOnly();
        }";
            } else {
                str = @"
        ret.___Name = __FieldRead;";
            }
            str = str.Replace("__FieldRead", !field.IsBasic ? "__TypeName.Read(reader)" : "reader.__Read()");
            str = str.Replace("__Read", field.IsBasic ? field.Info.ReadFunction : "");
            str = str.Replace("__TypeName", GetCodeType(field.Type));
            str = str.Replace("__Index", field.Index.ToString());
            str = str.Replace("__Name", field.Name);
            builder.Append(str);
        }
        builder.Append(@"
        return ret;
    }");
        return builder.ToString();
    }
}
