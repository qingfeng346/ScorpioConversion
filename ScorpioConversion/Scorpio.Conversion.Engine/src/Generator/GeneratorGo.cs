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
            } else if (field.IsEnum) {
                return "int32";
            } else {
                return "*" + field.Type;
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
func (table *{tableClassName}) Initialize(fileName string, reader ScorpioConversionRuntime.IReader) error {{
    if table.dataArray == nil {{
		table.dataArray = map[{keyType}]*{dataClassName}{{}}
	}}
    row := int(reader.ReadInt32());
    layoutMD5 := reader.ReadString();
    if (layoutMD5 != ""{fileMD5}"") {{
        return errors.New(""File schemas do not match [{tableClassName}] : "" + fileName)
    }}
    ScorpioConversionRuntime.ReadHead(reader);
    for i := 0; i < row; i++ {{
        var pData = New{dataClassName}(fileName, reader);
        if table.Contains(pData.GetID()) {{
            table.dataArray[pData.GetID()].Set(pData)
        }} else {{
            table.dataArray[pData.GetID()] = pData;
        }}
    }}
    table.count = len(table.dataArray)
    return nil
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
func (table *{tableClassName}) Datas() map[{keyType}]*{dataClassName} {{
    return table.dataArray
}}
func (table *{tableClassName}) GetValueObject(ID interface{{}}) ScorpioConversionRuntime.IData {{
    return table.GetValue(ID.({keyType}))
}}
func (table *{tableClassName}) ContainsObject(ID interface{{}}) bool {{
   return table.Contains(ID.({keyType}))
}}
func (table *{tableClassName}) Count() int {{
    return table.count;
}}";
        }
        public override string GenerateDataClass(string packageName, string className, PackageClass packageClass, bool createID) {
            return $@"package {packageName}
{Head}
import ""fmt""
{GetHead(packageClass)}
type {className} struct {{
   {AllFields(packageClass)}
}}
{AllGetFields(className, packageClass, createID)}
{FunctionNew(className, packageClass)}
{FunctionGetData(className, packageClass)}
{FunctionSet(className, packageClass)}
{FunctionToString(className, packageClass)}
";
        }
        string GetHead(PackageClass packageClass) {
            var builder = new StringBuilder();
            foreach (var field in packageClass.Fields) {
                if (field.IsArray) {
                    builder.Append(@"
import ""container/list""");
                    break;
                }
            }
            foreach (var field in packageClass.Fields) {
                if (field.IsDateTime) {
                    builder.Append(@"
import ""time""");
                    break;
                }
            }
            return builder.ToString();
        }
        string AllFields(PackageClass packageClass) {
            var builder = new StringBuilder();
            foreach (var field in packageClass.Fields) {
                var languageType = GetLanguageType(field);
                if (field.IsArray) { languageType = "*list.List"; }
                builder.Append($@"
    {field.Name} {languageType}");
            }
            return builder.ToString();
        }
        string AllGetFields(string className, PackageClass packageClass, bool createID) {
            var builder = new StringBuilder();
            {
                var field = packageClass.Fields[0];
                if (createID && field.Name != "ID") {
                var languageType = GetLanguageType(field);
                builder.Append($@"
// GetID {field.Comment}  默认值({field.Default})
func (data *{className}) GetID() {languageType} {{ 
    return data.{field.Name};
}}");
                }
            }
            foreach (var field in packageClass.Fields) {
                var languageType = GetLanguageType(field);
                if (field.IsArray) { languageType = $"*list.List"; }
                builder.Append($@"
// Get{field.Name} {field.Comment}  默认值({field.Default})
func (data *{className}) Get{field.Name}() {languageType} {{ 
    return data.{field.Name};
}}");
            }
            return builder.ToString();
        }
        string FunctionNew(string className, PackageClass packageClass) {
            var builder = new StringBuilder();
            builder.Append($@"
func New{className}(fileName string, reader ScorpioConversionRuntime.IReader) *{className} {{
    data := &{className}{{}}");
            foreach (var field in packageClass.Fields) {
                var languageType = GetLanguageType(field);
                string fieldRead;
                if (field.Attribute != null && field.Attribute.GetValue("Language").IsTrue) {
                    fieldRead = $@"reader.ReadL10N(fileName + "".{field.Name}."" + this.ID)";
                } else if (field.IsBasic) {
                    fieldRead = $"reader.Read{field.BasicType.Name}()";
                } else if (field.IsEnum) {
                    fieldRead = $"reader.ReadInt32()";
                } else {
                    fieldRead = $"New{languageType.Substring(1)}(fileName, reader)";
                }
                if (field.IsArray) {
                    builder.Append($@"
    {{
        data.{field.Name} = list.New();
        number := int(reader.ReadInt32());
        for i := 0; i < number; i++ {{ 
            data.{field.Name}.PushBack({fieldRead});
        }}
    }}");
                } else {
                    builder.Append($@"
    data.{field.Name} = {fieldRead};");
                }
            }
            builder.Append(@"
    return data
}");
            return builder.ToString();
        }
        string FunctionGetData(string className, PackageClass packageClass) {
            var builder = new StringBuilder();
            builder.Append($@"
func (data *{className}) GetData(key string) interface{{}} {{");
        foreach (var field in packageClass.Fields) {
            builder.Append($@"
    if key == ""{field.Name}"" {{
        return data.{field.Name};
    }}");
        }
            builder.Append(@"
    return nil;
}");
            return builder.ToString();
        }
        string FunctionSet(string className, PackageClass packageClass) {
            var builder = new StringBuilder();
            builder.Append($@"
func (data *{className}) Set(value *{className}) {{");
        foreach (var field in packageClass.Fields) {
            builder.Append($@"
    data.{field.Name} = value.{field.Name};");
        }
            builder.Append(@"
}");
            return builder.ToString();
        }
        string FunctionToString(string className, PackageClass packageClass) {
            var builder = new StringBuilder();
            builder.Append($@"
func (data *{className}) String() string {{
    return fmt.Sprintf(""");
            var first = true;
            foreach (var field in packageClass.Fields) {
                if (first == false) {
                    builder.Append(" , ");
                }
                first = false;
                builder.Append($@"{field.Name} [%v]");
            }
            builder.Append("\"");
            foreach (var field in packageClass.Fields) {
                builder.Append($", data.{field.Name}");
            }
            builder.Append(@");
}");
            return builder.ToString();
        }
        public override string GenerateEnumClass(string packageName, string className, PackageEnum packageEnum) {
            var builder = new StringBuilder();
            builder.Append($@"package {packageName}
//本文件为自动生成，请不要手动修改
type {className} int32
const (");
            foreach (var info in packageEnum.Fields) {
                builder.Append($@"
    {className}_{info.Name} {className} = {info.Index}");
            }
            builder.Append(@"
)");
            return builder.ToString();
        }
    }
}
