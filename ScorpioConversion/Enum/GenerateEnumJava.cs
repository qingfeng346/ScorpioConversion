using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
public class GenerateEnumJava : IGenerate
{
    public GenerateEnumJava() : base(PROGRAM.Java) { }
    protected override string Generate_impl()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"package __Package;
public enum __EnumName {");
        foreach (var info in m_Enums)
        {
            string str = @"
    __FieldName(__FieldValue),";
            str = str.Replace("__FieldName", info.Name);
            str = str.Replace("__FieldValue", info.Index.ToString());
            builder.Append(str);
        }
        
        builder.Append(@"
    private final int value;
    private __EnumName(int value) { this.value = value; }
    public final int getValue() { return this.value; }
    public static __EnumName valueOf(int value) {
        switch (value) {");
        foreach (var info in m_Enums)
        {
            string str = @"
            case __FieldValue: return __FieldName;";
            str = str.Replace("__FieldName", info.Name);
            str = str.Replace("__FieldValue", info.Index.ToString());
            builder.Append(str);
        }
        builder.Append(@"
            default: return null;
        }
    }
}");
        builder.Replace("__EnumName", m_ClassName);
        builder.Replace("__Package", m_Package);
        return builder.ToString();
    }
}
