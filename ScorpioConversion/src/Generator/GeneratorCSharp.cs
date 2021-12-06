using System;
using System.Collections.Generic;
using System.Text;


public class GenerateDataCSharp : IGenerator {
    public const string Head = @"using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ScorpioProto.Commons;
using ScorpioProto.Table;
";
    public GenerateDataCSharp() : base("cs") { }
    string GetLanguageType(ClassField field) {
        if (field.IsBasic) {
            switch (field.BasicType.Index) {
                case BasicEnum.BOOL: return "bool";
                case BasicEnum.INT8: return "sbyte";
                case BasicEnum.UINT8: return "byte";
                case BasicEnum.INT16: return "short";
                case BasicEnum.UINT16: return "ushort";
                case BasicEnum.INT32: return "int";
                case BasicEnum.UINT32: return "uint";
                case BasicEnum.INT64: return "long";
                case BasicEnum.UINT64: return "ulong";
                case BasicEnum.FLOAT: return "float";
                case BasicEnum.DOUBLE: return "double";
                case BasicEnum.STRING: return "string";
                case BasicEnum.DATETIME: return "DateTime";
                case BasicEnum.BYTES: return "byte[]";
                default: throw new Exception("未知的基础类型");
            }
        } else {
            return field.Type;
        }
    }
    public override string GenerateTableClass(string packageName, string tableClassName, string dataClassName, string suffix, string fileMD5, PackageClass packageClass) {
        var keyType = GetLanguageType(packageClass.Fields[0]);
        return $@"
{Head}
namespace {packageName} {{
public partial class {tableClassName} : ITable {{
	const string FILE_MD5_CODE = ""__MD5"";
    private int m_count = 0;
    private Dictionary<{keyType}, {dataClassName}> m_dataArray = new Dictionary<{keyType}, {dataClassName}>();
    public {tableClassName} Initialize(string fileName, IScorpioReader reader) {{
        var iRow = TableUtil.ReadHead(reader, fileName, FILE_MD5_CODE);
        for (var i = 0; i < iRow; ++i) {{
            var pData = {dataClassName}.Read(fileName, reader);
            if (m_dataArray.TryGetValue(pData.ID(), out var value))
                value.Set(pData);
            else
                m_dataArray[pData.ID()] = pData;
        }}
        m_count = m_dataArray.Count;
        return this;
    }}
    public {dataClassName} GetValue({keyType} ID) {{
        if (m_dataArray.TryGetValue(ID, out var value))
            return value;
        TableUtil.Warning(""{tableClassName} key is not exist "" + ID);
        return null;
    }}
    public bool Contains({keyType} ID) {{
        return m_dataArray.ContainsKey(ID);
    }}
    public Dictionary<__KeyType, {dataClassName}> Datas() {{
        return m_dataArray;
    }}
    
    public IData GetValueObject(object ID) {{
        return GetValue(({keyType})ID);
    }}
    public bool ContainsObject(object ID) {{
        return Contains(({keyType})ID);
    }}
    public IDictionary GetDatas() {{
        return Datas();
    }}
    public int Count() {{
        return m_count;
    }}
}}
}}";
    }

    public override string GenerateDataClass(string packageName, string tableClassName, string dataClassName, string suffix, string fileMD5, PackageClass packageClass) {
        throw new NotImplementedException();
    }
}
//public class GenerateTableCSharp : IGenerate {
//    protected override string Generate_impl() {
//        return $@"
//{TemplateCSharp.Head}
//namespace {PackageName} {{
//{TemplateCSharp.Table}
//}}";
//    }
//}
//public class GenerateEnumCSharp : IGenerate {
//    protected override string Generate_impl() {
//        var builder = new StringBuilder();
//        builder.Append($@"//本文件为自动生成，请不要手动修改
//namespace {PackageName} {{
//public enum {ClassName} {{");
//        foreach (var info in Enums.Fields) {
//            builder.Append($@"
//    {info.Name} = {info.Index},");
//        }
//        builder.Append(@"
//}
//}");
//        return builder.ToString();
//    }
//}