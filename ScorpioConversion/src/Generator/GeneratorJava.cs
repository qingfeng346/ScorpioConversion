using System;
using System.IO;
using System.Text;
using Scorpio.Commons;
public class GenerateDataJava : IGenerator {
    public const string Head = @"//本文件为自动生成，请不要手动修改
import java.util.*;
import ScorpioProto.Commons.*;
import ScorpioProto.Table.*;
";
    public GenerateDataJava() : base("java") { }
    string GetLanguageType(ClassField field) {
        if (field.IsBasic) {
            switch (field.BasicType.Index) {
                case BasicEnum.BOOL: return "Boolean";
                case BasicEnum.INT8: return "Byte";
                case BasicEnum.UINT8: return "Byte";
                case BasicEnum.INT16: return "Short";
                case BasicEnum.UINT16: return "Short";
                case BasicEnum.INT32: return "Integer";
                case BasicEnum.UINT32: return "Integer";
                case BasicEnum.INT64: return "Long";
                case BasicEnum.UINT64: return "Long";
                case BasicEnum.FLOAT: return "Float";
                case BasicEnum.DOUBLE: return "Double";
                case BasicEnum.STRING: return "String";
                case BasicEnum.DATETIME: return "Calendar";
                case BasicEnum.BYTES: return "byte[]";
                default: throw new Exception("未知的基础类型");
            }
        } else {
            return field.Type;
        }
    }
    public override string GetCodePath(LanguageInfo languageInfo, string name) {
        return Path.Combine(ScorpioUtil.CurrentDirectory, languageInfo.codeOutput, languageInfo.package.Replace(".", "/"), $"{name}.{languageInfo.codeSuffix}");
    }
    public override string GenerateTableClass(string packageName, string tableClassName, string dataClassName, string fileMD5, PackageClass packageClass) {
        var keyType = GetLanguageType(packageClass.Fields[0]);
        return $@"package {packageName};
{Head}
public class {tableClassName} implements ITable<{keyType}, {dataClassName}> {{
    final String FILE_MD5_CODE = ""{fileMD5}"";
    private int m_count = 0;
    private HashMap<{keyType}, {dataClassName}> m_dataArray = new HashMap<{keyType}, {dataClassName}>();
    public {tableClassName} Initialize(String fileName, IScorpioReader reader) throws Exception {{
        int iRow = reader.ReadHead(fileName, FILE_MD5_CODE);
        for (int i = 0; i < iRow; ++i) {{
            {dataClassName} pData = {dataClassName}.Read(fileName, reader);
            if (m_dataArray.containsKey(pData.ID()))
                m_dataArray.get(pData.ID()).Set(pData);
            else
                m_dataArray.put(pData.ID(), pData);
        }}
        m_count = m_dataArray.size();
        return this;
    }}
    public {dataClassName} GetValue({keyType} ID) throws Exception {{
        if (m_dataArray.containsKey(ID)) 
            return m_dataArray.get(ID);
        throw new Exception(""{tableClassName} not found data : "" + ID);
    }}
    public boolean Contains({keyType} ID) {{
        return m_dataArray.containsKey(ID);
    }}
    public final HashMap<{keyType}, {dataClassName}> Datas() {{
        return m_dataArray;
    }}
    public IData GetValueObject(Object ID) throws Exception {{
        return GetValue(({keyType})ID);
    }}
    public boolean ContainsObject(Object ID) {{
        return Contains(({keyType})ID);
    }}
    public int Count() {{
        return m_count;
    }}
}}";
    }
    public override string GenerateDataClass(string packageName, string className, PackageClass packageClass, bool createID) {
        return $@"package {packageName};
{Head}
public class {className} implements IData {{
    {AllFields(packageClass, createID)}
    {FunctionGetData(packageClass)}
    {FunctionRead(packageClass, className)}
    {FunctionSet(packageClass, className)}
    {FunctionToString(packageClass)}
}}
";
    }
    string AllFields(PackageClass packageClass, bool createID) {
        var builder = new StringBuilder();
        var first = true;
        foreach (var field in packageClass.Fields) {
            var languageType = GetLanguageType(field);
            if (field.IsArray) { languageType = $"List<{languageType}>"; }
            builder.Append($@"
    private {languageType} _{field.Name};
    /** {field.Comment}  默认值({field.Default}) */
    public {languageType} get{field.Name}() {{ return _{field.Name}; }}");
            if (first && createID) {
                first = false;
                builder.Append($@"
    public {languageType} ID() {{ return _{field.Name}; }}");
            }
        }
        return builder.ToString();
    }
    string FunctionGetData(PackageClass packageClass) {
        var builder = new StringBuilder();
        builder.Append(@"
    public Object GetData(String key) {");
        foreach (var field in packageClass.Fields) {
            builder.Append($@"
        if (""{field.Name}"".equals(key)) return _{field.Name};");
        }
        builder.Append(@"
        return null;
    }");
        return builder.ToString();
    }
    string FunctionRead(PackageClass packageClass, string dataClassName) {
        var builder = new StringBuilder();
        builder.Append($@"
    public static {dataClassName} Read(String fileName, IScorpioReader reader) {{
        {dataClassName} ret = new {dataClassName}();");
        foreach (var field in packageClass.Fields) {
            var languageType = GetLanguageType(field);
            string fieldRead;
            if (field.Attribute != null && field.Attribute.GetValue("Language").IsTrue) {
                fieldRead = $@"reader.ReadL10n(fileName + ""_{field.Name}_"" + ret.ID())";
            } else if (field.IsBasic) {
                fieldRead = $"reader.Read{field.BasicType.Name}()";
            } else if (field.IsEnum) {
                fieldRead = $"{languageType}.valueOf(reader.ReadInt32())";
            } else {
                fieldRead = $"{languageType}.Read(fileName, reader)";
            }
            if (field.IsArray) {
                builder.Append($@"
        {{
            List<{languageType}> list = new ArrayList<{languageType}>();
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
        return ret;
    }");
        return builder.ToString();
    }
    string FunctionSet(PackageClass packageClass, string dataClassName) {
        var builder = new StringBuilder();
        builder.Append($@"
    public void Set({dataClassName} value) {{");
        foreach (var field in packageClass.Fields) {
            builder.Append($@"
        this._{field.Name} = value._{field.Name};");
        }
        builder.Append(@"
    }");
        return builder.ToString();
    }
    string FunctionToString(PackageClass packageClass) {
        var builder = new StringBuilder();
        builder.Append(@"
    @Override
    public String toString() {
        return ");
        var first = true;
        foreach (var field in packageClass.Fields) {
            if (first == false) {
                builder.Append(" + \",\" + ");
            }
            first = false;
            builder.Append($"\"{field.Name}:\" + _{field.Name}");
        }
        builder.Append(@";
    }");
        return builder.ToString();
    }
    public override string GenerateEnumClass(string packageName, string className, PackageEnum packageEnum) {
        var builder = new StringBuilder();
        builder.Append($@"//本文件为自动生成，请不要手动修改
package {packageName};
public enum {className} {{");
        foreach (var info in packageEnum.Fields) {
            builder.Append($@"
    {info.Name}({info.Index}),");
        }
        builder.Append($@"
    ;
    private final int value;
    private {className}(int value) {{
        this.value = value;    
    }}
    public final int getValue() {{
        return this.value;
    }}
    public static {className} valueOf(int value) {{
        switch (value) {{");
        foreach (var info in packageEnum.Fields) {
            builder.Append($@"
            case {info.Index}: return {info.Name};");
        }
        builder.Append($@"
            default: return null;
        }}
    }}
}}");
        return builder.ToString();
    }
}