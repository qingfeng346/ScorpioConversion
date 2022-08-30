using System.Text;
namespace Scorpio.Conversion.Engine {
    [AutoGenerator("c++")]
    public class GeneratorCPP : IGenerator {
        public const string Head = @"//本文件为自动生成，请不要手动修改
#include <IReader.h>
#include <IData.h>
#include <ITable.h>
#include <ConversionUtil.h>
#include <map>
#include <vector>
#include <string>
using namespace std;
using namespace Scorpio::Conversion::Runtime;";
        static string GetLanguageType(ClassField field) {
            if (field.IsBasic) {
                switch (field.BasicType.Index) {
                    case BasicEnum.BOOL: return "bool";
                    case BasicEnum.INT8: return "__int8";
                    case BasicEnum.UINT8: return "unsigned __int8";
                    case BasicEnum.INT16: return "__int16";
                    case BasicEnum.UINT16: return "unsigned __int16";
                    case BasicEnum.INT32: return "__int32";
                    case BasicEnum.UINT32: return "unsigned __int32";
                    case BasicEnum.INT64: return "__int64";
                    case BasicEnum.UINT64: return "unsigned __int64";
                    case BasicEnum.FLOAT: return "float";
                    case BasicEnum.DOUBLE: return "double";
                    case BasicEnum.STRING: return "string";
                    case BasicEnum.DATETIME: return "__int64";
                    case BasicEnum.BYTES: return "char*";
                    default: throw new System.Exception("未知的基础类型");
                }
            } else if (field.IsEnum) {
                return "__int32";
            } else {
                return field.Type + "*";
            }
        }
        public override string GenerateTableClass(string packageName, string tableClassName, string dataClassName, string fileMD5, PackageClass packageClass) {
            var keyType = GetLanguageType(packageClass.Fields[0]);
            return $@"#ifndef ____{tableClassName}_H__
#define ____{tableClassName}_H__
{Head}
#include ""{dataClassName}.h""
namespace {packageName} {{
    class {tableClassName} : public ITable {{
        const char * FILE_MD5_CODE = ""{fileMD5}"";
    private:
        int m_count = 0;
        map<{keyType}, {dataClassName}*>* m_dataArray = new map<{keyType}, {dataClassName}*>();
    public:
        {tableClassName} * Initialize(string fileName, IReader * reader) {{
            int row = reader->ReadInt32();
            if (reader->ReadString() != FILE_MD5_CODE) {{
                throw new exception(""File schemas do not match [{tableClassName}]"");
            }}
            ConversionUtil::ReadHead(reader);
            for (int i = 0; i < row; ++i) {{
                {dataClassName} * pData = new {dataClassName}(fileName, reader);
                if (Contains(pData->GetID())) {{
                    (*m_dataArray)[pData->GetID()]->Set(pData);
                    delete pData;
                    pData = nullptr;
                }} else {{
                    (*m_dataArray)[pData->GetID()] = pData;
                }}
            }}
            m_count = m_dataArray->size();
            return this;
        }}
        {dataClassName} * GetValue({keyType} ID) {{
            if (m_dataArray->find(ID) != m_dataArray->end())
                return (*m_dataArray)[ID];
            throw new exception(""{tableClassName} not found data : {{ID}}"");
        }}
        bool Contains({keyType} ID) {{
            return m_dataArray->find(ID) != m_dataArray->end();
        }}
        IData * GetValueObject(void * ID) {{
            return GetValue(*({keyType}*)ID);
        }}
        bool ContainsObject(void * ID) {{
            return Contains(*({keyType}*)ID);
        }}
        map<{keyType}, {dataClassName}*>* Datas() {{
            return m_dataArray;
        }}
        int Count() {{
            return m_count;
        }}
    }};
}}
#endif";
        }
        public override string GenerateDataClass(string packageName, string className, PackageClass packageClass, bool createID) {
            return $@"#ifndef ____{className}_H__
#define ____{className}_H__
{Head}
{ImportList(packageClass, className, createID)}
namespace {packageName} {{
class {className} : public IData {{
    {AllFields(packageClass, createID)}
    {FunctionConstructor(packageClass, className)}
    {FunctionGetData(packageClass)}
    {FunctionSet(packageClass, className)}
    {FunctionToString(packageClass)}
}};
}}
#endif
";
        }
        string ImportList(PackageClass packageClass, string dataClassName, bool createID) {
            var builder = new StringBuilder();
            foreach (var field in packageClass.Fields) {
                if (field.IsClass) {
                    builder.Append($@"
#include ""{field.Type}.h""
");
                }
            }
            return builder.ToString();
        }
        string AllFields(PackageClass packageClass, bool createID) {
            var builder = new StringBuilder();
            builder.Append(@"
    private:");
            foreach (var field in packageClass.Fields) {
                var languageType = GetLanguageType(field);
                if (field.IsArray) { languageType = $"vector<{languageType}>*"; }
                builder.Append($@"
        {languageType} {field.Name};");
            }
            builder.Append(@"
    public:");
            var first = true;
            foreach (var field in packageClass.Fields) {
                var languageType = GetLanguageType(field);
                if (field.IsArray) { languageType = $"vector<{languageType}>*"; }
                if (first) {
                    first = false;
                    if (createID && field.Name != "ID") {
                        builder.Append($@"
        {languageType} GetID() {{ return {field.Name}; }}");
                    }
                }
                builder.Append($@"
        /* <summary> {field.Comment}  默认值({field.Default}) </summary> */
        {languageType} Get{field.Name}() {{ return {field.Name}; }}");
            }
            return builder.ToString();
        }
        string FunctionConstructor(PackageClass packageClass, string dataClassName) {
            var builder = new StringBuilder();
            builder.Append($@"
        {dataClassName}(string fileName, IReader * reader) {{");
            foreach (var field in packageClass.Fields) {
                var languageType = GetLanguageType(field);
                string fieldRead;
                if (field.IsL10n) {
                    fieldRead = $@"reader->ReadL10n(fileName + "".{field.Name}."" + this.ID)";
                } else if (field.IsBasic) {
                    fieldRead = $"reader->Read{field.BasicType.Name}()";
                } else if (field.IsEnum) {
                    fieldRead = $"reader->ReadInt32()";
                } else {
                    fieldRead = $"new {field.Type}(fileName, reader)";
                }
                if (field.IsArray) {
                    builder.Append($@"
            {{
                vector<{languageType}>* list = new vector<{languageType}>();
                int number = reader->ReadInt32();
                for (int i = 0; i < number; ++i) {{ 
                    list->push_back({fieldRead}); 
                }}
                this->{field.Name} = list;
            }}");
                } else {
                    builder.Append($@"
            this->{field.Name} = {fieldRead};");
                }
            }
            builder.Append(@"
        }");
            return builder.ToString();
        }
        string FunctionGetData(PackageClass packageClass) {
            var builder = new StringBuilder();
            builder.Append(@"
        void * GetData(string key) {");
            foreach (var field in packageClass.Fields) {
                builder.Append($@"
            if (key == ""{field.Name}"") return &{field.Name};");
            }
            builder.Append(@"
            return nullptr;
        }");
            return builder.ToString();
        }
        string FunctionSet(PackageClass packageClass, string dataClassName) {
            var builder = new StringBuilder();
            builder.Append($@"
        void Set({dataClassName} * value) {{");
            foreach (var field in packageClass.Fields) {
                builder.Append($@"
            this->{field.Name} = value->{field.Name};");
            }
            builder.Append(@"
        }");
            return builder.ToString();
        }
        string FunctionToString(PackageClass packageClass) {
            var builder = new StringBuilder();
            builder.Append(@"
        string ToString() {
            return """);
            // foreach (var field in packageClass.Fields) {
            //     builder.AppendFormat("{0}:{1}, ", field.Name, $"{{{field.Name}}}");
            // }
            builder.Append(@""";
        }");
            return builder.ToString();
        }
        public override string GenerateEnumClass(string packageName, string className, PackageEnum packageEnum) {
            var builder = new StringBuilder();
            builder.Append($@"//本文件为自动生成，请不要手动修改
namespace {packageName} {{
    enum {className} {{");
            foreach (var info in packageEnum.Fields) {
                builder.Append($@"
        {info.Name} = {info.Index},");
            }
            builder.Append(@"
    };
}");
            return builder.ToString();
        }
    }
}
