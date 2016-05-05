using System.Text;
public class GenerateTableJava : IGenerate {
    public GenerateTableJava() : base(PROGRAM.Java) { }
    protected override string Generate_impl() {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("package " + m_Package + ";");
        builder.Append(TemplateJava.Head);
        builder.Append(TemplateJava.Table);
        return builder.ToString();
    }
}
