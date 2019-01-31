using System;
using System.Collections.Generic;
using System.Text;

public class GenerateDataScorpio : IGenerate {
    protected override string Generate_impl() {
        var builder = new StringBuilder();
        builder.Append($@"//本文件为自动生成，请不要手动修改
{ClassName} = [");
        foreach (var field in Fields) {
            var languageType = field.GetLanguageType(Language);
            builder.Append($@"
    /* {field.Comment}  默认值({field.Default}) */
    {{ Index = {field.Index}, Name = ""{field.Name}"", Type = ""{languageType}"", Array = {field.Array.ToString().ToLower()}, Attribute = {field.AttributeString} }},
");
        }
        builder.Append(@"
]");
        return builder.ToString();
    }
}
public class GenerateTableScorpio : IGenerate {
    protected override string Generate_impl() {
        return TemplateScorpio.Table;
    }
}