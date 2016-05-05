using System.Collections.Generic;
using System.Text;
public class GenerateDataCPP : IGenerate
{
    public GenerateDataCPP() : base(PROGRAM.CPP) { }
    protected override string Generate_impl()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(@"#ifndef ____ClassName_H__
#define ____ClassName_H__");
        builder.AppendLine(TemplateCPP.Head);
        builder.AppendLine(GenerateMessageInclude());
        string[] packages = m_Package.Split('.');
        foreach (var package in packages) {
            builder.AppendLine("namespace " + package + "{");
        }
        builder.Append(@"class __ClassName : public IData {
    private: bool m_IsInvalid;");
        builder.Append(GenerateMessageFields());
        builder.Append(GenerateMessageGetData());
        builder.Append(GenerateMessageIsInvalid());
        builder.Append(GenerateMessageRead());
        builder.Append(@"
};
");
        builder.Replace("__ClassName", m_ClassName);
        foreach (var package in packages) {
            builder.AppendLine("}");
        }
        builder.AppendLine("#endif");
        return builder.ToString();
    }
    string GenerateMessageInclude() {
        StringBuilder builder = new StringBuilder();
        List<string> types = new List<string>();
        foreach (var field in m_Fields) {
            if (!field.IsBasic && !types.Contains(field.Type)) {
                types.Add(field.Type);
                builder.AppendLine(string.Format("#include \"{0}.h\"", GetCodeType(field.Type)));
            }
        }
        return builder.ToString();
    }
    string GenerateMessageGetData() {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"
    public: void* GetData(char * key ) {");
        foreach (var field in m_Fields) {
            builder.Append(@"
        if (strcmp(key, ""__Key"") == 0) return &___Key;".Replace("__Key", field.Name));
        }
        builder.Append(@"
        return nullptr;
    }");
        return builder.ToString();
    }
    string GenerateMessageFields()
    {
        StringBuilder builder = new StringBuilder();
        bool first = true;
        foreach (var field in m_Fields) {
            string str = "";
            if (field.Array) {
                str = @"
    private: std::vector<__Type> ___Name;
    /// <summary> __Note(__Default) </summary>
    public: std::vector<__Type> get__Name() { return ___Name; }";
            } else {
                str = @"
    private: __Type ___Name;
    /// <summary> __Note(__Default) </summary>
    public: __Type get__Name() { return ___Name; }";
                if (first && (bool)m_Parameter) {
                    first = false;
                    str += @"
    public: __Type ID() { return ___Name; }";
                }
            }
            str = str.Replace("__Name", field.Name);
            str = str.Replace("__Note", field.Comment);
            str = str.Replace("__Default", field.Default);
            var type = GetCodeType(field.Type);
            str = str.Replace("__Type", (field.IsBasic || field.Enum) ? type : (type + " *"));
            builder.Append(str);
        }
        return builder.ToString();
    }
    string GenerateMessageIsInvalid()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"
    public: bool IsInvalid() { return m_IsInvalid; }
    private: bool IsInvalid_impl() {");
        foreach (var field in m_Fields) {
            string str = @"
        if (!TableUtil::IsInvalid(___Name)) return false;";
            str = str.Replace("__Name", field.Name);
            builder.Append(str);
        }
        builder.Append(@"
        return true;
    }");
        return builder.ToString();
    }
    string GenerateMessageRead()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"
    public: static __ClassName * Read(ScorpioReader * reader) {
        __ClassName * ret = new __ClassName();");
        foreach (var field in m_Fields) {
            string str = "";
            if (field.Array) {
                str = @"
        {
            int number = reader->ReadInt32();
            for (int i = 0;i < number; ++i) { ret->___Name.push_back(__FieldRead); }
        }";
            } else {
                str = @"
        ret->___Name = __FieldRead;";
            }
            str = str.Replace("__FieldRead", field.IsBasic ? "reader->__Read()" : (field.Enum ? "(__Type)reader->ReadInt32()" : "__Type::Read(reader)"));
            str = str.Replace("__Read", field.IsBasic ? field.Info.ReadFunction : "");
            str = str.Replace("__Type", GetCodeType(field.Type));
            str = str.Replace("__Index", field.Index.ToString());
            str = str.Replace("__Name", field.Name);
            builder.Append(str);
        }
        builder.Append(@"
        ret->m_IsInvalid = ret->IsInvalid_impl();
        return ret;
    }");
        return builder.ToString();
    }
}
