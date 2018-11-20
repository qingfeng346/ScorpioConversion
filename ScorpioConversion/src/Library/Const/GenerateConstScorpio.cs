using System;
using System.Collections.Generic;
using System.Text;
public class GenerateConstScorpio : IGenerate
{
    public GenerateConstScorpio() : base(PROGRAM.Scorpio) { }
    protected override string Generate_impl()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"//本文件为自动生成，请不要手动修改
__ConstName = {");
        foreach (var info in m_Consts) {
            string str = @"
    __FieldName = __FieldValue,";
            str = str.Replace("__FieldName", info.Name);
            str = str.Replace("__FieldValue", info.Value);
            builder.Append(str);
        }
        builder.Append(@"
}");
        builder = builder.Replace("__ConstName", m_ClassName);
        return builder.ToString();
    }
}

