using System;
using System.Collections.Generic;
using System.Text;
public class GenerateConstCPP : IGenerate
{
    public GenerateConstCPP() : base(PROGRAM.CPP) { }
    protected override string Generate_impl()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"//本文件为自动生成，请不要手动修改
class __ConstName {
    public:");
        foreach (var info in m_Consts) {
            string str = @"
        static const __FieldType __FieldName = __FieldValue;";
            str = str.Replace("__FieldName", info.Name);
            str = str.Replace("__FieldType", GetCodeType(info.Type));
            str = str.Replace("__FieldValue", info.Value);
            builder.Append(str);
        }
        builder.Append(@"
}");
        builder.Replace("__ConstName", m_ClassName);
        builder.Replace("__Package", m_Package);
        return builder.ToString();
    }
}

