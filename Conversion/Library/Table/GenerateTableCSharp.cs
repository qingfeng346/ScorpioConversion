using System.Text;

public class GenerateTableCSharp : IGenerate {
    public GenerateTableCSharp() : base(PROGRAM.CSharp) { }
    protected override string Generate_impl() {
        StringBuilder builder = new StringBuilder();
        builder.Append(TemplateCSharp.Head);
        builder.AppendLine("namespace " + m_Package + " {");
        builder.Append(TemplateCSharp.Table);
        builder.AppendLine("}");
        return builder.ToString();
    }
}
