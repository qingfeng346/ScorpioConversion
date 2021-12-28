//using System.Text;
//public class TemplateScorpio2 {
//    public const string Table = @"
//ScorpioSerializer = import_type(""ScorpioProto.Commons.ScorpioSerializer"")
//class __TableName {
//    constructor() {
//        this.m_count = 0
//        this.m_dataArray = {}
//    }
//    Initialize(fileName, reader) {
//        this.m_dataArray = ScorpioSerializer.ReadDatas(fileName, reader, this.m_dataArray, ""__DataName"", ""__KeyName"", ""__MD5"")
//        this.m_count = this.m_dataArray.length()
//        return this
//    }
//    GetValue(ID) {
//        return this.m_dataArray[ID]
//    }
//    Contains(ID) {
//        return this.m_dataArray.containsKey(ID)
//    }
//    Count() {
//        return this.m_count 
//    }
//    Datas() {
//        return this.m_dataArray
//    }
//    ""()""(ID) {
//        return this.m_dataArray[ID]
//    }
//}";
//}

//public class GenerateDataScorpio2 : IGenerate {
//    protected override string Generate_impl() {
//        var builder = new StringBuilder();
//        builder.Append($@"//本文件为自动生成，请不要手动修改
//{ClassName} = [");
//        foreach (var field in Fields) {
//            var languageType = field.GetLanguageType(Language);
//            languageType = field.IsEnum ? BasicUtil.GetType(BasicEnum.INT32).Name : languageType;
//            builder.Append($@"
//    /* {field.Comment}  默认值({field.Default}) */
//    {{ Index : {field.Index}, Name : ""{field.Name}"", Type : ""{languageType}"", Array : {field.IsArray.ToString().ToLower()}, Attribute : {field.AttributeString} }},
//");
//        }
//        builder.Append(@"
//]");
//        return builder.ToString();
//    }
//}
//public class GenerateTableScorpio2 : IGenerate {
//    protected override string Generate_impl() {
//        return TemplateScorpio2.Table;
//    }
//}
//public class GenerateEnumScorpio2 : GenerateEnumScorpio {
//}