using System;
using System.Collections.Generic;
using System.Text;

public class TemplateCSharp {
    public const string Head = @"using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Scorpio.Commons;
using Scorpio.Table;
";
    public const string Table = @"public class __TableName : TableBase<__KeyType, __DataName> {
	const string FILE_MD5_CODE = ""__MD5"";
    private int m_count = 0;
    private Dictionary<__KeyType, __DataName> m_dataArray = new Dictionary<__KeyType, __DataName>();
    public __TableName Initialize(Dictionary<string, string> l10n, string fileName, byte[] buffer) {
        using (var reader = new ScorpioReader(buffer)) {
            var iRow = TableUtil.ReadHead(reader, fileName, FILE_MD5_CODE);
            for (var i = 0; i < iRow; ++i) {
                var pData = __DataName.Read(l10n, fileName, reader);
                if (m_dataArray.ContainsKey(pData.ID())) {
                    m_dataArray[pData.ID()].Set(pData);
                } else {
                    m_dataArray.Add(pData.ID(), pData);
                }
            }
            m_count = m_dataArray.Count;
            return this;
        }
    }
    public __DataName GetValue(__KeyType ID) {
        if (m_dataArray.ContainsKey(ID)) return m_dataArray[ID];
        TableUtil.Warning(""__DataName key is not exist "" + ID);
        return null;
    }
    public bool Contains(__KeyType ID) {
        return m_dataArray.ContainsKey(ID);
    }
    public Dictionary<__KeyType, __DataName> Datas() {
        return m_dataArray;
    }
    
    public IData GetValueObject(object ID) {
        return GetValue((__KeyType)ID);
    }
    public bool ContainsObject(object ID) {
        return Contains((__KeyType)ID);
    }
    public IDictionary GetDatas() {
        return Datas();
    }
    public int Count() {
        return m_count;
    }
}
";
}

public class GenerateDataCSharp : IGenerate {
    protected override string Generate_impl() {
        return $@"{TemplateCSharp.Head}
namespace {PackageName} {{
public class {ClassName} : IData {{
    private bool m_IsInvalid;
    {AllFields()}
    {FuncGetData()}
    {FuncIsInvalid()}
    {FuncRead()}
    {FuncSet()}
    {FucToString()}
}}
}}";
    }
    string AllFields() {
        var builder = new StringBuilder();
        var first = true;
        foreach (var field in Fields) {
            var languageType = field.GetLanguageType(Language);
            if (field.Array) { languageType = $"ReadOnlyCollection<{languageType}>"; }
            builder.Append($@"
    private {languageType} _{field.Name};
    /* <summary> {field.Comment}  默认值({field.Default}) </summary> */
    public {languageType} get{field.Name}() {{ return _{field.Name}; }}");
            if (first && (bool)Parameter) {
                first = false;
                builder.Append($@"
    public {languageType} ID() {{ return _{field.Name}; }}");
            }
        }
        return builder.ToString();
    }
    string FuncGetData() {
        var builder = new StringBuilder();
        builder.Append(@"
    public object GetData(string key) {");
        foreach (var field in Fields) {
            builder.Append($@"
        if (""{field.Name}"".Equals(key)) return _{field.Name};");
        }
        builder.Append(@"
        return null;
    }");
        return builder.ToString();
    }
    string FuncIsInvalid() {
        var builder = new StringBuilder();
        builder.Append(@"
    public bool IsInvalid() { return m_IsInvalid; }
    private bool CheckInvalid() {");
        foreach (var field in Fields) {
            builder.Append($@"
        if (!TableUtil.IsInvalid(this._{field.Name})) return false;");
        }
        builder.Append(@"
        return true;
    }");
        return builder.ToString();
    }
    string FuncRead() {
        var builder = new StringBuilder();
        builder.Append($@"
    public static {ClassName} Read(Dictionary<string, string> l10n, string fileName, ScorpioReader reader) {{
        var ret = new {ClassName}();");
        foreach (var field in Fields) {
            var languageType = field.GetLanguageType(Language);
            var fieldRead = "";
            if (field.Attribute != null && field.Attribute.GetValue("Language").LogicOperation()) {
                fieldRead = $@"TableUtil.Readl10n(l10n, fileName + ""_{field.Name}_"" + ret.ID(), reader)";
            } else if (field.IsBasic){
                fieldRead = $"reader.Read{field.BasicType.Name}()";
            } else if (field.IsEnum) {
                fieldRead = $"({languageType})reader.ReadInt32()";
            } else {
                fieldRead = $"{languageType}.Read(l10n, fileName, reader)";
            }
            if (field.Array) {
                builder.Append($@"
        {{
            List<{languageType}> list = new List<{languageType}>();
            int number = reader.ReadInt32();
            for (int i = 0; i < number; ++i) {{ list.Add({fieldRead}); }}
            ret._{field.Name} = list.AsReadOnly();
        }}");
            } else {
                builder.Append($@"
        ret._{field.Name} = {fieldRead};");
            }
        }
        builder.Append(@"
        ret.m_IsInvalid = ret.CheckInvalid();
        return ret;
    }");
        return builder.ToString();
    }
    string FuncSet() {
        var builder = new StringBuilder();
        builder.Append($@"
    public void Set({ClassName} value) {{");
        foreach (var field in Fields) {
            builder.Append($@"
        this._{field.Name} = value._{field.Name};");
        }
        builder.Append(@"
    }");
        return builder.ToString();
    }
    string FucToString() {
        var builder = new StringBuilder();
        builder.Append(@"
    public override string ToString() {
        return ""{ """);
        var count = Fields.Count;
        for (int i = 0; i < count; ++i) {
            var field = Fields[i];
            var toString = field.Array ? $"ScorpioUtil.ToString(_{field.Name})" : $"_{field.Name}";
            builder.Append($@" + 
            ""{field.Name} : "" +  {toString}");
            if (i != count - 1) {
                builder.Append(" + \",\"");
            }
        }
        builder.Append(@" + 
            "" }"";
    }");
        return builder.ToString();
    }
}
public class GenerateTableCSharp : IGenerate {
    protected override string Generate_impl() {
        return $@"
{TemplateCSharp.Head}
namespace {PackageName} {{
{TemplateCSharp.Table}
}}";
    }
}
