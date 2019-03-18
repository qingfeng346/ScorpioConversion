using System;
using System.Collections.Generic;
using System.Text;

public class TemplateJava {
    public const string Head = @"
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import ScorpioProto.Commons.*;
import ScorpioProto.Table.*;
@SuppressWarnings(""unused"")
";
    public const string Table = @"public class __TableName implements ITable<__KeyType, __DataName> {
	final String FILE_MD5_CODE = ""__MD5"";
    private int m_count = 0;
    private HashMap<__KeyType, __DataName> m_dataArray = new HashMap<__KeyType, __DataName>();
    public __TableName Initialize(String fileName, IScorpioReader reader) {
        int iRow = TableUtil.ReadHead(reader, fileName, FILE_MD5_CODE);
        for (int i = 0; i < iRow; ++i) {
            __DataName pData = __DataName.Read(fileName, reader);
            if (m_dataArray.containsKey(pData.ID())) {
                m_dataArray.get(pData.ID()).Set(pData);
            } else {
                m_dataArray.put(pData.ID(), pData);
            }
        }
        m_count = m_dataArray.size();
        return this;
    }
    
    public __DataName GetValue(__KeyType ID) {
        if (m_dataArray.containsKey(ID)) return m_dataArray.get(ID);
        TableUtil.Warning(""__DataName key is not exist "" + ID);
        return null;
    }
    public boolean Contains(__KeyType ID) {
        return m_dataArray.containsKey(ID);
    }
    public final HashMap<__KeyType, __DataName> Datas() {
        return m_dataArray;
    }
    
    public IData GetValueObject(Object ID) {
        return GetValue((__KeyType)ID);
    }
    public boolean ContainsObject(Object ID) {
        return Contains((__KeyType)ID);
    }
    public int Count() {
        return m_count;
    }
}
";
}


public class GenerateDataJava : IGenerate {
    protected override string Generate_impl() {
        return $@"
package {PackageName};
{TemplateJava.Head}
public class {ClassName} implements IData {{
    private boolean m_IsInvalid;
    {AllFields()}
    {FuncGetData()}
    {FuncIsInvalid()}
    {FuncRead()}
    {FuncSet()}
    {FucToString()}
}}";
    }
    string AllFields() {
        var builder = new StringBuilder();
        var first = true;
        foreach (var field in Fields) {
            var languageType = field.GetLanguageType(Language);
            if (field.Array) { languageType = $"List<{languageType}>"; }
            builder.Append($@"
    private {languageType} _{field.Name};
    /** {field.Comment}  默认值({field.Default}) */
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
    public Object GetData(String key ) {");
        foreach (var field in Fields) {
            builder.Append($@"
        if (""{field.Name}"".equals(key)) return _{field.Name};");
        }
        builder.Append(@"
        return null;
    }");
        return builder.ToString();
    }
    string FuncIsInvalid() {
        var builder = new StringBuilder();
        builder.Append(@"
    public boolean IsInvalid() { return m_IsInvalid; }
    private boolean CheckInvalid() {");
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
    public static {ClassName} Read(String fileName, IScorpioReader reader) {{
        {ClassName} ret = new {ClassName}();");
        foreach (var field in Fields) {
            var languageType = field.GetLanguageType(Language);
            var fieldRead = "";
            if (field.Attribute != null && field.Attribute.GetValue("Language").LogicOperation()) {
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
            ArrayList<{languageType}> list = new ArrayList<{languageType}>();
            int number = reader.ReadInt32();
            for (int i = 0; i < number; ++i) {{ list.add({fieldRead}); }}
            ret._{field.Name} = Collections.unmodifiableList(list);
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
    @Override
    public String toString() {
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
public class GenerateTableJava : IGenerate {
    protected override string Generate_impl() {
        return $@"package {PackageName};
{TemplateJava.Head}
{TemplateJava.Table}";
    }
}
