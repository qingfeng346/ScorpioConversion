using System;
using System.IO;
using System.Text;
public class GenerateDataJava : IGenerator {
    public const string Head = @"//本文件为自动生成，请不要手动修改
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import ScorpioProto.Commons.*;
import ScorpioProto.Table.*;
";
    public GenerateDataJava() : base("java") { }
    string GetLanguageType(ClassField field) {
        if (field.IsBasic) {
            switch (field.BasicType.Index) {
                case BasicEnum.BOOL: return "boolean";
                case BasicEnum.INT8: return "byte";
                case BasicEnum.UINT8: return "byte";
                case BasicEnum.INT16: return "short";
                case BasicEnum.UINT16: return "short";
                case BasicEnum.INT32: return "int";
                case BasicEnum.UINT32: return "int";
                case BasicEnum.INT64: return "long";
                case BasicEnum.UINT64: return "long";
                case BasicEnum.FLOAT: return "float";
                case BasicEnum.DOUBLE: return "double";
                case BasicEnum.STRING: return "String";
                case BasicEnum.DATETIME: return "Calendar";
                case BasicEnum.BYTES: return "byte[]";
                default: throw new Exception("未知的基础类型");
            }
        } else {
            return field.Type;
        }
    }
    public override string GenerateTableClass(string packageName, string tableClassName, string dataClassName, string fileMD5, PackageClass packageClass) {
        var keyType = GetLanguageType(packageClass.Fields[0]);
        return $@"{Head}
package {packageName};
public class {tableClassName} implements ITable<{keyType}, {dataClassName}> {{
    final String FILE_MD5_CODE = ""{fileMD5}"";
    private int m_count = 0;
    private HashMap<{keyType}, {dataClassName}> m_dataArray = new HashMap<{keyType}, {dataClassName}>();
    public {tableClassName} Initialize(String fileName, IScorpioReader reader) {{
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
    public {dataClassName} GetValue({keyType} ID) {{
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
    public IData GetValueObject(Object ID) {{
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
        return $@"{Head}
namespace {packageName} {{
public partial class {className} : IData {{
    {AllFields(packageClass, createID)}
    {FunctionGetData(packageClass)}
    {FunctionRead(packageClass, className)}
    {FunctionSet(packageClass, className)}
    {FunctionToString(packageClass)}
}}
}}
";
    }
    string AllFields(PackageClass packageClass, bool createID) {
        var builder = new StringBuilder();
        var first = true;
        foreach (var field in packageClass.Fields) {
            var languageType = GetLanguageType(field);
            if (field.IsArray) { languageType = $"ReadOnlyCollection<{languageType}>"; }
            builder.Append($@"
    private {languageType} _{field.Name};
    /* <summary> {field.Comment}  默认值({field.Default}) </summary> */
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
    public object GetData(string key) {");
        foreach (var field in packageClass.Fields) {
            builder.Append($@"
        if (""{field.Name}"".Equals(key)) return _{field.Name};");
        }
        builder.Append(@"
        return null;
    }");
        return builder.ToString();
    }
    string FunctionRead(PackageClass packageClass, string dataClassName) {
        var builder = new StringBuilder();
        builder.Append($@"
    public static {dataClassName} Read(string fileName, IScorpioReader reader) {{
        var ret = new {dataClassName}();");
        foreach (var field in packageClass.Fields) {
            var languageType = GetLanguageType(field);
            string fieldRead;
            if (field.Attribute != null && field.Attribute.GetValue("Language").IsTrue) {
                fieldRead = $@"TableUtil.Readl10n(l10n, fileName + ""_{field.Name}_"" + ret.ID(), reader)";
            } else if (field.IsBasic) {
                fieldRead = $"reader.Read{field.BasicType.Name}()";
            } else if (field.IsEnum) {
                fieldRead = $"({languageType})reader.ReadInt32()";
            } else {
                fieldRead = $"{languageType}.Read(fileName, reader)";
            }
            if (field.IsArray) {
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
    public override string ToString() {
        return $""");
        foreach (var field in packageClass.Fields) {
            builder.AppendFormat("{0}:{1}, ", field.Name, $"{{_{field.Name}}}");
        }
        builder.Append(@""";
    }");
        return builder.ToString();
    }
    public override string GenerateEnumClass(string packageName, string className, PackageEnum packageEnum) {
        var builder = new StringBuilder();
        builder.Append($@"//本文件为自动生成，请不要手动修改
namespace {packageName} {{
    public enum {className} {{");
        foreach (var info in packageEnum.Fields) {
            builder.Append($@"
        {info.Name} = {info.Index},");
        }
        builder.Append(@"
    }
}");
        return builder.ToString();
    }
}