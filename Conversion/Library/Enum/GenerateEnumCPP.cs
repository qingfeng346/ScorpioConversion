using System.Text;
public class GenerateEnumCPP : IGenerate
{
    public GenerateEnumCPP() : base(PROGRAM.CPP) { }
    protected override string Generate_impl()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(@"#ifndef ____EnumName_H__
#define ____EnumName_H__");
        string[] packages = m_Package.Split('.');
        foreach (var package in packages) {
            builder.AppendLine("namespace " + package + "{");
        }
        builder.Append(@"//本文件为自动生成，请不要手动修改
enum __EnumName {");
        foreach (var info in m_Enums)
        {
            string str = @"
    __EnumName___FieldName = __FieldValue,";
            str = str.Replace("__FieldName", info.Name);
            str = str.Replace("__FieldValue", info.Index.ToString());
            builder.Append(str);
        }
        builder.Append(@"
};
");
        builder.Replace("__EnumName", m_ClassName);
        foreach (var package in packages) {
            builder.AppendLine("}");
        }
        builder.Append("#endif");
        return builder.ToString();
    }
}
