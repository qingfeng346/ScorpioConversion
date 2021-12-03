using System.Text;

public class TemplateScorpioFun {
    public const string Table = @"/*
From : __FileName - __SheetName
*/
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
        return this.m_dataArray[ID] ?? log.warn(""__FileName - __SheetName - __TableName not found ID : "" + ID)
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
    return this.m_dataArray[ID] ?? log.warn(""__FileName - __SheetName - __TableName not found ID : "" + ID)
}";
}

public class GenerateDataScorpioFun : IGenerate {
    protected override string Generate_impl() {
        var builder = new StringBuilder();
        builder.Append($@"//本文件为自动生成，请不要手动修改
{ClassName} = [");
        foreach (var field in Fields) {
            var languageType = field.GetLanguageType(Language);
            languageType = field.IsEnum ? BasicUtil.GetType(BasicEnum.INT32).Name : languageType;
            builder.Append($@"
    /* {field.Comment}  默认值({field.Default}) */
    {{ Index : {field.Index}, Name : ""{field.Name}"", Type : ""{languageType}"", Array : {field.IsArray.ToString().ToLower()}, L10N : {field.IsL10N.ToString().ToLower()}, Attribute : {field.AttributeString} }},
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
    protected override string Generate_impl() {
        var builder = new StringBuilder();
        var stringBuilder = new StringBuilder();
        builder.Append($@"//本文件为自动生成，请不要手动修改
{ClassName} = {{");
        foreach (var info in Enums.Fields) {
            builder.Append($@"
    {info.Name} : {info.Index},");
            stringBuilder.Append($@"
            case {info.Index}: return '{info.Name}'; ");
        }
        builder.Append($@"
    GetString : function(id) {{
        switch (id) {{{stringBuilder.ToString()}
            default: return toString(id)
        }}
    }}
}}");
        return builder.ToString();
    }
}