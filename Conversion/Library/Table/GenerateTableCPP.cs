using System.Text;
public class GenerateTableCPP : IGenerate {
    public GenerateTableCPP() : base(PROGRAM.CPP) { }
    protected override string Generate_impl() {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(@"#ifndef ____TableName_H__
#define ____TableName_H__");
        builder.AppendLine(TemplateCPP.Head);
        builder.AppendLine("#include \"__DataName.h\"");
        string[] packages = m_Package.Split('.');
        foreach (var package in packages) {
            builder.AppendLine("namespace " + package + "{");
        }
        builder.AppendLine(TemplateCPP.Table);
        foreach (var package in packages) {
            builder.AppendLine("}");
        }
        builder.AppendLine("#endif");
        return builder.ToString();
    }
}
