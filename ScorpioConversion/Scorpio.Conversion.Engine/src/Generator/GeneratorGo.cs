using System.Text;
namespace Scorpio.Conversion.Engine {
    [AutoGenerator("go")]
    public class GeneratorGo : IGenerator {
        public const string Head = @"//本文件为自动生成，请不要手动修改
import ""github.com/qingfeng346/ScorpioConversion/Scorpio.Conversion.Runtime/Go/ScorpioConversionRuntime""";
        static string GetLanguageType(ClassField field) {
            if (field.IsBasic) {
                switch (field.BasicType.Index) {
                    case BasicEnum.BOOL: return "bool";
                    case BasicEnum.INT8: return "int8";
                    case BasicEnum.UINT8: return "uint8";
                    case BasicEnum.INT16: return "int16";
                    case BasicEnum.UINT16: return "uint16";
                    case BasicEnum.INT32: return "int32";
                    case BasicEnum.UINT32: return "uint32";
                    case BasicEnum.INT64: return "int64";
                    case BasicEnum.UINT64: return "uint64";
                    case BasicEnum.FLOAT: return "float32";
                    case BasicEnum.DOUBLE: return "float64";
                    case BasicEnum.STRING: return "string";
                    case BasicEnum.DATETIME: return "time.Time";
                    case BasicEnum.BYTES: return "[]byte";
                    default: throw new System.Exception("未知的基础类型");
                }
            } else {
                return field.Type;
            }
        }
        public override string GenerateTableClass(string packageName, string tableClassName, string dataClassName, string fileMD5, PackageClass packageClass) {
            var keyType = GetLanguageType(packageClass.Fields[0]);
            return $@"package {packageName}
{Head}
import ""errors""
type {tableClassName} struct {{
    count int
    dataArray map[{keyType}]*{dataClassName}
}}
func (table *{tableClassName}) Initialize(fileName string, reader ScorpioConversionRuntime.IReader) {{
    if table.dataArray == nil {{
		table.dataArray = map[{keyType}]*{dataClassName}{{}}
	}}
    var row = reader.ReadInt32();
    var layoutMD5 = reader.ReadString();
    if (layoutMD5 != ""{fileMD5}"") {{
        return errors.New(""File schemas do not match [{tableClassName}] : "" + fileName)
    }}
    ScorpioConversionRuntime.ReadHead(reader);
    for i := 0; i < row; i++ {{
        var pData = New{dataClassName}(fileName, reader);
        if table.Contains(pData.ID()) {{
            table.dataArray[pData.ID()].Set(pData)
        }} else {{
            table.dataArray[pData.ID()] = pData;
        }}
    }}
    table.count = len(table.dataArray)
}}
// Contains 是否包含数据
func (table *{tableClassName}) Contains(ID {keyType}) bool {{
    if _, ok := table.dataArray[ID]; ok {{
        return true
    }}
    return false
}}
// GetValue 获取数据
func (table *{tableClassName}) GetValue(ID {keyType}) *{dataClassName} {{
    if table.Contains(ID) {{
        return table.dataArray[ID];
    }}
    return nil;
}}
func (table *{tableClassName}) GetValueObject(ID interface{{}}) IData {{
    return table.GetValue(({keyType})ID)
}}
func (table *{tableClassName}) ContainsObject(ID interface{{}}) bool {{
   if _, ok := table.dataArray[ID]; ok {{
       return true
   }}
   return false
}}
func (table *{tableClassName}) Count() int {{
    return table.count;
}}";
        }
        public override string GenerateDataClass(string packageName, string className, PackageClass packageClass, bool createID) {
            return $@"package {packageName}
{Head}
namespace {packageName} {{
public partial class {className} : IData {{
    {AllFields(packageClass, createID)}
    {FunctionConstructor(packageClass, className)}
    {FunctionGetData(packageClass)}
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
                if (first) {
                    first = false;
                    if (createID && field.Name != "ID") {
                        builder.Append($@"
    public {languageType} ID => {field.Name};");
                    }
                }
                builder.Append($@"
    /* <summary> {field.Comment}  默认值({field.Default}) </summary> */
    public {languageType} {field.Name} {{ get; private set; }}");

            }
            return builder.ToString();
        }
        string FunctionConstructor(PackageClass packageClass, string dataClassName) {
            var builder = new StringBuilder();
            builder.Append($@"
    public {dataClassName}(string fileName, IReader reader) {{");
            foreach (var field in packageClass.Fields) {
                var languageType = GetLanguageType(field);
                string fieldRead;
                if (field.Attribute != null && field.Attribute.GetValue("Language").IsTrue) {
                    fieldRead = $@"reader.ReadL10N(fileName + "".{field.Name}."" + this.ID)";
                } else if (field.IsBasic) {
                    fieldRead = $"reader.Read{field.BasicType.Name}()";
                } else if (field.IsEnum) {
                    fieldRead = $"({languageType})reader.ReadInt32()";
                } else {
                    fieldRead = $"new {languageType}(fileName, reader)";
                }
                if (field.IsArray) {
                    builder.Append($@"
        {{
            var list = new List<{languageType}>();
            var number = reader.ReadInt32();
            for (int i = 0; i < number; ++i) {{ list.Add({fieldRead}); }}
            this.{field.Name} = list.AsReadOnly();
        }}");
                } else {
                    builder.Append($@"
        this.{field.Name} = {fieldRead};");
                }
            }
            builder.Append(@"
    }");
            return builder.ToString();
        }
        string FunctionGetData(PackageClass packageClass) {
            var builder = new StringBuilder();
            builder.Append(@"
    public object GetData(string key) {");
            foreach (var field in packageClass.Fields) {
                builder.Append($@"
        if (""{field.Name}"".Equals(key)) return {field.Name};");
            }
            builder.Append(@"
        return null;
    }");
            return builder.ToString();
        }
        string FunctionSet(PackageClass packageClass, string dataClassName) {
            var builder = new StringBuilder();
            builder.Append($@"
    public void Set({dataClassName} value) {{");
            foreach (var field in packageClass.Fields) {
                builder.Append($@"
        this.{field.Name} = value.{field.Name};");
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
                builder.AppendFormat("{0}:{1}, ", field.Name, $"{{{field.Name}}}");
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
}
