using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
public class GenerateMessageCPP : IGenerate
{
    public GenerateMessageCPP() : base(PROGRAM.CPP) {}
    protected override string Generate_impl()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(@"#ifndef ____ClassName_H__
#define ____ClassName_H__");
        builder.Append(TemplateCPP.Head);
        builder.AppendLine(GenerateMessageInclude());
        string[] packages = m_Package.Split('.');
        foreach (var package in packages) {
            builder.AppendLine("namespace " + package + "{");
        }
        builder.Append(@"//本文件为自动生成，请不要手动修改
class __ClassName : public IMessage {");
        builder.Append(GenerateMessageFields());
        builder.Append(GenerateMessageRelease());
        builder.Append(GenerateMessageWrite());
        builder.Append(GenerateMessageRead());
        builder.Append(GenerateMessageDeserialize());
        builder.Append(@"
};
");
        builder.Replace("__ClassName", m_ClassName);
        builder.Replace("__Package", m_Package);
        foreach (var package in packages) {
            builder.AppendLine("}");
        }
        builder.Append("#endif");
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
    string GenerateMessageFields()
    {
        StringBuilder builder = new StringBuilder();
        foreach (var field in m_Fields)
        {
            string str = "";
            if (field.Array) {
                str = @"
    private: std::vector<__Type> ___Name;
    public: std::vector<__Type> get__Name() { return ___Name; }
    public: __ClassName * set__Name(std::vector<__Type> value) { ___Name = value; AddSign(__Index); return this; } ";
            } else {
                str = @"
    private: __Type ___Name;
    public: __Type get__Name() { return ___Name; }
    public: __ClassName * set__Name(__Type value) { ___Name = value; AddSign(__Index); return this; } ";
            }
            str = str.Replace("__Index", field.Index.ToString());
            str = str.Replace("__Name", field.Name);
            var type = GetCodeType(field.Type);
            str = str.Replace("__Type", (field.IsBasic || field.Enum) ? type : (type + " *"));
            builder.Append(str);
        }
        return builder.ToString();
    }
    string GenerateMessageRelease() {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"
    public: ~__ClassName() {");
        foreach (var field in m_Fields) {
            string str = "";
            if (field.Array && !field.IsBasic && !field.Enum) {
                str = @"
        for (size_t i = 0;i < ___Name.size(); ++i) {
            delete ___Name[i];
        }
        ___Name.clear();";
            } else if (!field.IsBasic && !field.Enum){
                str = @"
        delete ___Name; ___Name = nullptr; ";
            }
            str = str.Replace("__Index", field.Index.ToString());
            str = str.Replace("__Name", field.Name);
            var type = GetCodeType(field.Type);
            str = str.Replace("__Type", (field.IsBasic || field.Enum) ? type : (type + " *"));
            builder.Append(str);
        }
        builder.Append(@"
    }");
        return builder.ToString();
    }
    string GenerateMessageWrite()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"
    public: void Write(ScorpioWriter * writer) {
        writer->WriteInt32(__Sign);");
        foreach (var field in m_Fields)
        {
            string str = "";
            if (field.Array) {
                str = @"
        if (HasSign(__Index)) {
            writer->WriteInt32(static_cast<__int32>(___Name.size()));
            for (size_t i = 0;i < ___Name.size(); ++i) { __FieldWrite; }
        }";
            } else {
                str = @"
        if (HasSign(__Index)) { __FieldWrite; }";
            }
            string name = field.Array ? "___Name[i]" : "___Name";
            string write = field.IsBasic ? "writer->__Write(___Name)" : (field.Enum ? "writer->__Write((int)___Name)" : "___Name->Write(writer)");
            write = write.Replace("___Name", name);
            str = str.Replace("__FieldWrite", write);
            str = str.Replace("__Write", field.IsBasic ? field.Info.WriteFunction : (field.Enum ? "WriteInt32" : ""));
            str = str.Replace("__Index", field.Index.ToString());
            str = str.Replace("__Name", field.Name);
            builder.Append(str);
        }
        builder.Append(@"
    }");
        return builder.ToString();
    }
    string GenerateMessageRead()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"
    public: static __ClassName * Read(ScorpioReader * reader) {
        __ClassName * ret = new __ClassName();
        ret->__Sign = reader->ReadInt32();");
        foreach (var field in m_Fields) {
            string str = "";
            if (field.Array) {
                str = @"
        if (ret->HasSign(__Index)) {
            int number = reader->ReadInt32();
            ret->___Name = std::vector<__Type>();
            for (int i = 0;i < number; ++i) { ret->___Name.push_back(__FieldRead); }
        }";
            } else {
                str = @"
        if (ret->HasSign(__Index)) { ret->___Name = __FieldRead; }";
            }
            str = str.Replace("__FieldRead", field.IsBasic ? "reader->__Read()" : (field.Enum ? "(__Type)reader->ReadInt32()" : "__PriType::Read(reader)"));
            str = str.Replace("__Read", field.IsBasic ? field.Info.ReadFunction : "");
            var type = GetCodeType(field.Type);
            str = str.Replace("__Type", (field.IsBasic || field.Enum) ? type : (type + " *"));
            str = str.Replace("__PriType", type);
            str = str.Replace("__Index", field.Index.ToString());
            str = str.Replace("__Name", field.Name);
            builder.Append(str);
        }
        builder.Append(@"
        return ret;
    }");
        return builder.ToString();
    }
    static string GenerateMessageDeserialize()
    {
        return @"
    public: static __ClassName * Deserialize(char * data) {
        return Read(new ScorpioReader(data));
    }";
    }
}
