using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
public class GenerateJava : IGenerate
{
    public GenerateJava() : base(PROGRAM.Java) { }
    protected override string Generate_impl()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"
public class __ClassName extends IData {");
        builder.Append(GenerateMessageFields());
        builder.Append(GenerateMessageGetData());
        builder.Append(GenerateMessageRead());
        builder.Append(@"
}");
        builder.Replace("__ClassName", m_ClassName);
        return builder.ToString();
    }
    string GenerateMessageFields()
    {
        StringBuilder builder = new StringBuilder();
        bool first = true;
        foreach (var field in m_Fields)
        {
            string str = "";
            if (field.Array) {
                str = @"
    private List<__Type> ___Name;
    /** __Note */
    public List<__Type> get__Name() { return ___Name; }";
            } else {
                str = @"
    private __Type ___Name;
    /** __Note */
    public __Type get__Name() { return ___Name; }";
                if (first && m_ConID) {
                    first = false;
                    str += @"
    public __Type ID() { return ___Name; }";
                }
            }
            str = str.Replace("__Name", field.Name);
            str = str.Replace("__Note", field.Note);
            str = str.Replace("__Type", GetCodeType(field.Type));
            builder.Append(str);
        }
        return builder.ToString();
    }
    string GenerateMessageGetData()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"
    @Override
    public Object GetData(String key ) {");
        foreach (var field in m_Fields)
        {
            builder.Append(@"
        if (key == ""__Key"") return ___Key;".Replace("__Key", field.Name));
        }
        builder.Append(@"
        return null;
    }");
        return builder.ToString();
    }
    string GenerateMessageRead()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"
    public static __ClassName Read(ScorpioReader reader) {
        __ClassName ret = new __ClassName();");
        foreach (var field in m_Fields)
        {
            string str = "";
            if (field.Array)
            {
                str = @"
        {
            int number = reader.ReadInt32();
            ArrayList<__Type> list = new ArrayList<__Type>();
            for (int i = 0;i < number; ++i) { list.add(__FieldRead); }
            ret.___Name = Collections.unmodifiableList(list);
        }";
            }
            else
            {
                str = @"
        ret.___Name = __FieldRead;";
            }
            str = str.Replace("__FieldRead", !field.IsBasic ? "__Type.Read(reader)" : "reader.__Read()");
            str = str.Replace("__Read", field.IsBasic ? field.Info.ReadFunction : "");
            str = str.Replace("__Type", GetCodeType(field.Type));
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
