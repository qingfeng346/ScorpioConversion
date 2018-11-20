using System.Text;
public class GenerateEnumCSharp : IGenerate
{
    public GenerateEnumCSharp() : base(PROGRAM.CSharp) { }
    protected override string Generate_impl()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append($@"//本文件为自动生成，请不要手动修改
namespace {m_Package} {{
public enum {m_ClassName} {{");
        foreach (var info in m_Enums)
        {
            builder.Append($@"
    {info.Name} = {info.Index},");
        }
        builder.Append(@"
}
}");
        return builder.ToString();
    }
}
