using System;
using System.Collections.Generic;
using System.Text;
public class TemplateGo {
    public const string Head = @"import (
    ""scorpioproto""
)";
    public const string Table = @"

// __TableName 类名
type __TableName struct {
    count int
    dataArray map[__KeyType]*__DataName
}

// Initialize 初始化
func (table *__TableName) Initialize(fileName string, reader scorpioproto.IScorpioReader) {
    if table.dataArray == nil {
		table.dataArray = map[__KeyType]*__DataName{}
	}
    iRow := scorpioproto.TableUtilReadHead(reader, fileName, ""__MD5"");
    for i := 0; i < iRow; i++ {
        pData := __DataNameRead(fileName, reader)
        if table.Contains(pData.ID()) {
            table.dataArray[pData.ID()].Set(pData)
        } else {
            table.dataArray[pData.ID()] = pData;
        }
    }
    table.count = len(table.dataArray)
}

// Contains 是否包含数据
func (table *__TableName) Contains(ID __KeyType) bool {
    if _, ok := table.dataArray[ID]; ok {
        return true
    }
    return false
}

// GetValue 获取数据
func (table *__TableName) GetValue(ID __KeyType) *__DataName {
    if table.Contains(ID) {
        return table.dataArray[ID];
    }
    //TableUtilWarning(""__DataName key is not exist "" + ID);
    return nil;
}

// Datas 所有数据
func (table *__TableName) Datas() map[__KeyType]*__DataName {
    return table.dataArray
}

// Count 数量
func (table *__TableName) Count() int {
    return table.count
}";
}
public class GenerateDataGo : IGenerate {
    protected override string Generate_impl() {
        return $@"package {PackageName}
{GetHead()}

// {ClassName} 类名
type {ClassName} struct {{
    {AllFields()}
}}
{AllGetFields()}
{FuncGetData()}
{FuncSet()}
{FuncRead()}

// {ClassName}Read 读取数据
func {ClassName}Read(fileName string, reader scorpioproto.IScorpioReader) *{ClassName} {{
    ret := new({ClassName})
    ret.Read(fileName, reader)
    return ret
}}
";
    }
    string GetHead() {
        string head = @"import (
    ""scorpioproto""";
        foreach (var field in Fields) {
            if (field.Array) {
                head += @"
    ""container/list""";
            }
        }
        foreach (var field in Fields) {
            if (field.IsDateTime) {
                head += @"
    ""time""";
            }
        }
        head +=@"
)";
        return head;
    }
    string AllFields() {
        var builder = new StringBuilder();
        foreach (var field in Fields) {
            var languageType = field.GetLanguageType(Language);
            if (field.Array) { languageType = $"*list.List"; }
            builder.Append($@"
    _{field.Name} {languageType}");
        }
        return builder.ToString();
    }
    string AllGetFields() {
        var builder = new StringBuilder();
        {
            var field = Fields[0];
            var languageType = field.GetLanguageType(Language);
            builder.Append($@"

// ID {field.Comment}  默认值({field.Default})
func (data *{ClassName}) ID() {languageType} {{ return data._{field.Name}; }}");
        }

        foreach (var field in Fields) {
            var languageType = field.GetLanguageType(Language);
            if (field.Array) { languageType = $"*list.List"; }
            builder.Append($@"

// Get{field.Name} {field.Comment}  默认值({field.Default})
func (data *{ClassName}) Get{field.Name}() {languageType} {{ return data._{field.Name}; }}");
        }
        return builder.ToString();
    }
    string FuncGetData() {
        var builder = new StringBuilder();
        builder.Append($@"

// GetData 获取数据
func (data *{ClassName}) GetData(key string) interface{{}} {{");
        foreach (var field in Fields) {
            builder.Append($@"
    if ""{field.Name}"" == key {{
        return data._{field.Name};
    }}");
        }
        builder.Append(@"
    return nil;
}");
        return builder.ToString();
    }
    string FuncSet() {
        var builder = new StringBuilder();
        builder.Append($@"

// Set 设置数据
func (data *{ClassName}) Set(value *{ClassName}) {{");
        foreach (var field in Fields) {
            builder.Append($@"
    data._{field.Name} = value._{field.Name};");
        }
        builder.Append(@"
}");
        return builder.ToString();
    }
    string FuncRead() {
        var builder = new StringBuilder();
        builder.Append($@"

// Read 读取数据
func (data *{ClassName}) Read(fileName string, reader scorpioproto.IScorpioReader) {{");
        foreach (var field in Fields) {
            var languageType = field.GetLanguageType(Language);
            var fieldRead = "";
            if (field.Attribute != null && field.Attribute.GetValue("Language").LogicOperation()) {
                fieldRead = $@"TableUtil.Readl10n(l10n, fileName + ""_{field.Name}_"" + ret.ID(), reader)";
            } else if (field.IsBasic) {
                fieldRead = $"reader.Read{field.BasicType.Name}()";
            } else if (field.IsEnum) {
                fieldRead = $"{languageType}(reader.ReadInt32())";
            } else {
                //languageType.Substring(1) 去除 类型签名的 *
                fieldRead = $"{languageType.Substring(1)}Read(fileName, reader)";
            }
            if (field.Array) {
                builder.Append($@"
    {{
        data._{field.Name} = list.New();
        number := int(reader.ReadInt32());
        for i := 0; i < number; i++ {{ 
            data._{field.Name}.PushBack({fieldRead}); 
        }}
    }}");
            } else {
                builder.Append($@"
    data._{field.Name} = {fieldRead};");
            }
        }
        builder.Append(@"
}");
        return builder.ToString();
    }
}
public class GenerateTableGo : IGenerate {
    protected override string Generate_impl() {
        return $@"package {PackageName}
{TemplateGo.Head}
{TemplateGo.Table}
";
    }
}
public class GenerateEnumGo : IGenerate {
    protected override string Generate_impl() {
        var builder = new StringBuilder();
        builder.Append($@"package {PackageName}
//本文件为自动生成，请不要手动修改
type {ClassName} int32
const (");
        foreach (var info in Enums.Fields) {
            builder.Append($@"
    {ClassName}_{info.Name} {ClassName} = {info.Index}");
        }
        builder.Append(@"
)");
        return builder.ToString();
    }
}