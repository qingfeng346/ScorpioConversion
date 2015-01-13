using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
public class GenerateEnumCSharp : IGenerate
{
    public GenerateEnumCSharp() : base(PROGRAM.CSharp) { }
    protected override string Generate_impl()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"namespace __Package {
public enum __EnumName {");
        foreach (var info in m_Enums)
        {
            string str = @"
    __FieldName = __FieldValue,";
            str = str.Replace("__FieldName", info.Name);
            str = str.Replace("__FieldValue", info.Index.ToString());
            builder.Append(str);
        }
        builder.Append(@"
}
}");
        builder.Replace("__EnumName", m_ClassName);
        builder.Replace("__Package", m_Package);
        return builder.ToString();
    }
}
