using System.Text;

public class TemplateScorpioFun {
    public const string Table = @"
class __TableName {
    constructor() {
        this.m_count = 0
        this.m_dataArray = {}
    }
    Initialize(fileName, reader) {
        this.m_dataArray = TableSerializer.ReadDatas(fileName, reader, this.m_dataArray, ""__DataName"", ""__KeyName"", ""__MD5"")
        this.m_count = this.m_dataArray.length()
        return this
    }
    GetValue(ID) {
        return this.m_dataArray[ID]
    }
    Contains(ID) {
        return this.m_dataArray.containsKey(ID)
    }
    Count() {
        return this.m_count 
    }
    Datas() {
        return this.m_dataArray
    }
}
__TableName[""()""] = function(ID) {
    return this.m_dataArray[ID]
}";
}

public class GenerateDataScorpioFun : IGenerate {
    protected override string Generate_impl() {
        var builder = new StringBuilder();
        builder.Append($@"//本文件为自动生成，请不要手动修改
$G[""{ClassName}""] = [");
        foreach (var field in Fields) {
            var languageType = field.GetLanguageType(Language);
            languageType = field.IsEnum ? BasicUtil.GetType(BasicEnum.INT32).Name : languageType;
            builder.Append($@"
    /* {field.Comment}  默认值({field.Default}) */
    {{ Index : {field.Index}, Name : ""{field.Name}"", Type : ""{languageType}"", Array : {field.Array.ToString().ToLower()}, Attribute : {field.AttributeString} }},
");
        }
        builder.Append(@"
]");
        return builder.ToString();
    }
}
public class GenerateTableScorpioFun : IGenerate {
    protected override string Generate_impl() {
        return TemplateScorpioFun.Table;
    }
}
public class GenerateEnumScorpioFun : GenerateEnumScorpio {
}