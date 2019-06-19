using System;
using System.Collections.Generic;
using System.Text;

public class TemplateCSharp {
    public const string Head = @"using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ScorpioProto.Commons;
using ScorpioProto.Table;
";
    public const string Table = @"public partial class __TableName : ITable {
	const string FILE_MD5_CODE = ""__MD5"";
    private int m_count = 0;
    private Dictionary<__KeyType, __DataName> m_dataArray = new Dictionary<__KeyType, __DataName>();
    public __TableName Initialize(string fileName, IScorpioReader reader) {
        var iRow = TableUtil.ReadHead(reader, fileName, FILE_MD5_CODE);
        for (var i = 0; i < iRow; ++i) {
            var pData = __DataName.Read(fileName, reader);
            __DataName value;
            if (m_dataArray.TryGetValue(pData.ID(), out value)) {
                value.Set(pData);
            } else {
                m_dataArray[pData.ID()] = pData;
            }
        }
        m_count = m_dataArray.Count;
        return this;
    }
    public __DataName GetValue(__KeyType ID) {
        __DataName value;
        if (m_dataArray.TryGetValue(ID, out value)) {
            return value;
        }
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
public partial class {ClassName} : IData {{
    {AllFields()}
    {FuncGetData()}
    {FuncSet()}
    {FucToString()}
    {FuncRead()}
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
    string FuncRead() {
        var builder = new StringBuilder();
        builder.Append($@"
    public static {ClassName} Read(string fileName, IScorpioReader reader) {{
        var ret = new {ClassName}();");
        foreach (var field in Fields) {
            var languageType = field.GetLanguageType(Language);
            var fieldRead = "";
            if (field.Attribute != null && field.Attribute.GetValue("Language").IsTrue) {
                fieldRead = $@"TableUtil.Readl10n(l10n, fileName + ""_{field.Name}_"" + ret.ID(), reader)";
            } else if (field.IsBasic) {
                fieldRead = $"reader.Read{field.BasicType.Name}()";
            } else if (field.IsEnum) {
                fieldRead = $"({languageType})reader.ReadInt32()";
            } else {
                fieldRead = $"{languageType}.Read(fileName, reader)";
            }
            if (field.Array) {
                builder.Append($@"
        {{
            var list = new List<{languageType}>();
            var number = reader.ReadInt32();
            for (int i = 0; i < number; ++i) {{ list.Add({fieldRead}); }}
            ret._{field.Name} = list.AsReadOnly();
        }}");
            } else {
                builder.Append($@"
        ret._{field.Name} = {fieldRead};");
            }
        }
        builder.Append(@"
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
        return string.Format(");
        var format = "";
        var args = "";
        var count = Fields.Count;
        for (int i = 0; i < count; ++i) {
            var field = Fields[i];
            if (i != 0) {
                format += ", ";
                args += ", ";
            }
            format += $"{field.Name} : {{{i}}}";
            args += $"_{field.Name}";
        }
        builder.Append($@"""{format}"", {args});
    }}");
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
public class GenerateEnumCSharp : IGenerate {
    protected override string Generate_impl() {
        var builder = new StringBuilder();
        builder.Append($@"//本文件为自动生成，请不要手动修改
namespace {PackageName} {{
public enum {ClassName} {{");
        foreach (var info in Enums.Fields) {
            builder.Append($@"
    {info.Name} = {info.Index},");
        }
        builder.Append(@"
}
}");
        return builder.ToString();
    }
}