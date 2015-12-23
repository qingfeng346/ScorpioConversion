using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
public class GenerateTableScorpio : IGenerate
{
    public GenerateTableScorpio() : base(PROGRAM.Scorpio) { }
    protected override string Generate_impl()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"//本文件为自动生成，请不要手动修改
__ClassName = [");
        foreach (var field in m_Fields)
        {
            string str = @"
    { Index = __Index, Name = ""__Name"", Type = ""__Type"", Array = __Array },     //__Note(__Default)";
            str = str.Replace("__Index", field.Index.ToString());
            str = str.Replace("__Name", field.Name);
            str = str.Replace("__Type", field.Enum ? BasicUtil.GetType(BasicEnum.INT32).ScorpioName : field.Type);
            str = str.Replace("__Note", field.Comment);
            str = str.Replace("__Default", field.Default);
            str = str.Replace("__Array", field.Array ? "true" : "false");
            builder.Append(str);
        }
        builder.Append(@"
]");
        builder = builder.Replace("__ClassName", m_ClassName);
        return builder.ToString();
    }
}
