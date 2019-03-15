using System;
using System.Collections.Generic;
using System.Text;

public class TemplateScorpio {
    public const string Table = @"
ScorpioSerializer = import_type(""ScorpioProto.Commons.ScorpioSerializer"")
__TableName = {
    m_count = 0,
    m_dataArray = {},
    function Initialize(fileName, reader) {
        this.m_dataArray = ScorpioSerializer.ReadDatas(_SCRIPT, fileName, reader, this.m_dataArray, ""__DataName"", ""__KeyName"", ""__MD5"")
        this.m_count = table.count(this.m_dataArray)
        return this
    }
    function GetValue(ID) {
        return this.m_dataArray[ID]
    }
    function Contains(ID) {
        return table.containskey(this.m_dataArray, ID)
    }
    function Count() {
        return this.m_count
    }
    function Datas() {
        return this.m_dataArray
    }
}";
}

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