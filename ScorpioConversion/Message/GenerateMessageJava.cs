using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
public class GenerateMessageJava : IGenerate
{
    public GenerateMessageJava() : base(PROGRAM.Java) { }
    protected override string Generate_impl()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"package __Package;
import java.util.List;
import java.util.ArrayList;
import Scorpio.Commons.*;
import Scorpio.Message.*;
public class __ClassName extends IMessage {");
        builder.Append(GenerateMessageFields());
        builder.Append(GenerateMessageWrite());
        builder.Append(GenerateMessageRead());
        builder.Append(GenerateMessageDeserialize());
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
            if (field.Array)
            {
                str = @"
    private List<__Type> ___Name;
    public List<__Type> get__Name() { return ___Name; }
    public void set__Name(List<__Type> value) { ___Name = value; AddSign(__Index); } ";
            }
            else
            {
                str = @"
    private __Type ___Name;
    public __Type get__Name() { return ___Name; }
    public void set__Name(__Type value) { ___Name = value; AddSign(__Index); } ";
            }
            str = str.Replace("__Index", field.Index.ToString());
            str = str.Replace("__Name", field.Name);
            str = str.Replace("__Type", GetCodeType(field.Type));
            builder.Append(str);
        }
        return builder.ToString();
    }
    string GenerateMessageWrite()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"
    @Override
    public void Write(ScorpioWriter writer) {
        writer.WriteInt32(__Sign);");
        foreach (var field in m_Fields)
        {
            string str = "";
            if (field.Array)
            {
                str = @"
        if (HasSign(__Index)) {
            writer.WriteInt32(___Name.size());
            for (int i = 0;i < ___Name.size(); ++i) { __FieldWrite; }
        }";
            } else {
                str = @"
        if (HasSign(__Index)) { __FieldWrite; }";
            }
            string write = "";
            if (field.Array) {
                write = !field.IsBasic ? "___Name.get(i).Write(writer)" : "writer.__Write(___Name.get(i))";
            } else {
                write = !field.IsBasic ? "___Name.Write(writer)" : "writer.__Write(___Name)";
            }
            str = str.Replace("__FieldWrite", write);
            str = str.Replace("__Write", field.IsBasic ? field.Info.WriteFunction : "");
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
    public static __ClassName Read(ScorpioReader reader) {
        __ClassName ret = new __ClassName();
        ret.__Sign = reader.ReadInt32();");
        foreach (var field in m_Fields)
        {
            string str = "";
            if (field.Array)
            {
                str = @"
        if (ret.HasSign(__Index)) {
            int number = reader.ReadInt32();
            ret.___Name = new ArrayList<__TypeName>();
            for (int i = 0;i < number; ++i) { ret.___Name.add(__FieldRead); }
        }";
            }
            else
            {
                str = @"
        if (ret.HasSign(__Index)) { ret.___Name = __FieldRead; }";
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
    static string GenerateMessageDeserialize()
    {
        return @"
    public static __ClassName Deserialize(byte[] data) {
        return Read(new ScorpioReader(data));
    }";
    }
}
