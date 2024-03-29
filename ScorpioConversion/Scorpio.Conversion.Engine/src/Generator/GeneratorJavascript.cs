﻿using System.Text;
namespace Scorpio.Conversion.Engine {
    [AutoGenerator("javascript")]
    public class GeneratorJavascript : IGenerator {
        public const string Head = @"//本文件为自动生成，请不要手动修改
";
        public override string GenerateTableClass(string packageName, string tableClassName, string dataClassName, string fileMD5, PackageClass packageClass) {
            return $@"{Head}
const {dataClassName} = require(""./{dataClassName}"")
class {tableClassName} {{
    constructor() {{
        this.m_count = 0;
        this.m_dataArray = new Map();
    }}
    Initialize(fileName, reader) {{
        let row = reader.ReadInt32();
        if (""{fileMD5}"" != reader.ReadString()) {{
            throw new Error(""File schemas do not match [{tableClassName}] : ${{fileName}}"");
        }}
        reader.ReadHead();
        for (let i = 0; i < row; i ++) {{
            let pData = new {dataClassName}(fileName, reader);
            if (this.m_dataArray.has(pData.ID)) {{
                this.m_dataArray.get(pData.ID).Set(pData);
            }} else {{
                this.m_dataArray.set(pData.ID, pData);
            }}
        }}
        this.m_count = this.m_dataArray.size;
        return this;
    }}
    GetValue(ID) {{
        return this.m_dataArray.get(ID)
    }}
    Contains(ID) {{
        return this.m_dataArray.has(ID)
    }}
    Datas() {{
        return this.m_dataArray
    }}
    GetValueObject(ID) {{
        return this.GetValue(ID)
    }}
    ContainsObject(ID) {{
        return this.Contains(ID)
    }}
    Count() {{
        return this.m_count;
    }}
}}
module.exports = {tableClassName};";
        }
        public override string GenerateDataClass(string packageName, string className, PackageClass packageClass, bool createID = false) {
            return $@"{Head}
{AllImports(packageClass)}
class {className} {{
    {FunctionConstructor(packageClass, className, createID)}
    {FunctionGetData(packageClass)}
    {FunctionSet(packageClass, className)}
    {FunctionToString(packageClass)}
}}
module.exports = {className};
";
        }
        string AllImports(PackageClass packageClass) {
            var builder = new StringBuilder();
            foreach (var field in packageClass.Fields) {
                if (!field.IsBasic && !field.IsEnum) {
                    builder.AppendLine($"const {field.Type} = require('./{field.Type}')");
                }
            }
            return builder.ToString();
        }
        string FunctionConstructor(PackageClass packageClass, string dataClassName, bool createID) {
            var first = true;
            var builder = new StringBuilder();
            builder.Append($@"
    constructor(fileName, reader) {{");
            foreach (var field in packageClass.Fields) {
                string fieldRead;
                if (field.IsL10n) {
                    fieldRead = $@"reader.ReadL10n(fileName + "".{field.Name}."" + this.ID)";
                } else if (field.IsBasic) {
                    fieldRead = $"reader.Read{field.BasicType.Name}()";
                } else if (field.IsEnum) {
                    fieldRead = $"reader.ReadInt32()";
                } else {
                    fieldRead = $"new {field.Type}(fileName, reader)";
                }
                if (field.IsArray) {
                    builder.Append($@"
        {{
            let list = []
            let number = reader.ReadInt32()
            for (let i = 0; i < number; i++) {{ 
                list.push({fieldRead})
            }}
            this.{field.Name} = list
        }}");
                } else {
                    builder.Append($@"
        this.{field.Name} = {fieldRead}");
                }
                if (first) {
                    first = false;
                    if (createID && field.Name != "ID") {
                        builder.Append($@"
        this.ID = this.{field.Name}");
                    }
                }
            }
            builder.Append(@"
    }");
            return builder.ToString();
        }
        string FunctionGetData(PackageClass packageClass) {
            var builder = new StringBuilder();
            builder.Append(@"
    GetData(key) {");
            foreach (var field in packageClass.Fields) {
                builder.Append($@"
        if (""{field.Name}"" == key) {{ return this.{field.Name} }}");
            }
            builder.Append(@"
        return null;
    }");
            return builder.ToString();
        }
        string FunctionSet(PackageClass packageClass, string dataClassName) {
            var builder = new StringBuilder();
            builder.Append($@"
    Set(value) {{");
            foreach (var field in packageClass.Fields) {
                builder.Append($@"
        this.{field.Name} = value.{field.Name}");
            }
            builder.Append(@"
    }");
            return builder.ToString();
        }
        string FunctionToString(PackageClass packageClass) {
            var builder = new StringBuilder();
            builder.Append(@"
    toString() {
        return ");
            var first = true;
            foreach (var field in packageClass.Fields) {
                if (first == false) {
                    builder.Append(" + \",\" + ");
                }
                first = false;
                builder.Append($"\"{field.Name}:\" + this.{field.Name}");
            }
            builder.Append(@"
    }");
            return builder.ToString();
        }

        public override string GenerateEnumClass(string packageName, string className, PackageEnum packageEnum) {
            var builder = new StringBuilder();
            builder.Append($@"{Head}
module.exports = {{");
            foreach (var info in packageEnum.Fields) {
                builder.Append($@"
    {info.Name} = {info.Index},");
            }
            builder.Append(@"
}");
            return builder.ToString();
        }
    }
}