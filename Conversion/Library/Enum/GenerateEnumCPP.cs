using System.Text;
public class GenerateEnumCPP : IGenerate
{
    public GenerateEnumCPP() : base(PROGRAM.CPP) { }
    protected override string Generate_impl()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"//本文件为自动生成，请不要手动修改
namespace __Package {
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
