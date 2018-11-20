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
        m_Consts.ForEach(info => {
            builder.Append($@"
        static const {GetCodeType(info.Type)} {info.Name} = {info.Value};");
        });
        builder.Append(@"
}");
        builder.Replace("__ConstName", m_ClassName);
        builder.Replace("__Package", m_Package);
        return builder.ToString();
    }
}

