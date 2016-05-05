using System.Text;
public class GenerateTableScorpio : IGenerate {
    public GenerateTableScorpio() : base(PROGRAM.Scorpio) { }
    protected override string Generate_impl() {
        StringBuilder builder = new StringBuilder();
        builder.Append(TemplateScorpio.Table);
        return builder.ToString();
    }
}
