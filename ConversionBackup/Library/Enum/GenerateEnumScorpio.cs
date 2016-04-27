using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
public class GenerateEnumScorpio : IGenerate
{
    public GenerateEnumScorpio() : base(PROGRAM.Scorpio) { }
    protected override string Generate_impl()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"//本文件为自动生成，请不要手动修改
__EnumName = {");
        foreach (var info in m_Enums)
        {
            string str = @"
    __FieldName = __FieldValue,";
            str = str.Replace("__FieldName", info.Name);
            str = str.Replace("__FieldValue", info.Index.ToString());
            builder.Append(str);
        }
        builder.Append(@"
}");
        builder = builder.Replace("__EnumName", m_ClassName);
        return builder.ToString();
    }
}
