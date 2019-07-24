using System;
using System.Collections.Generic;
using System.Text;
using Scorpio;

public class FieldClass {
    private PackageParser mParser;
    public FieldClass(PackageParser parser) { mParser = parser; }
    public int Index;                   //字段索引
    public string Name;                 //字段名字
    public string Comment;              //字段注释
    public ScriptMap Attribute;         //字段属性,字段配置
    public string Default = "";         //字段默认值
    public string Type;                 //字段类型

    //是否是数组
    public bool IsArray { get; set; } = false;
    //字段是否有效
    public bool IsValid { get; set; } = true;
    //是否是基本数据
    public bool IsBasic { get { return BasicUtil.HasType(Type); } }
    //基本数据类型
    public BasicType BasicType { get { return BasicUtil.GetType(Type); } }
    //是否是时间
    public bool IsDateTime { get { return IsBasic && BasicType.Index == BasicEnum.DATETIME; } }
    //是否是枚举
    public bool IsEnum { get { return mParser != null && mParser.Enums.ContainsKey(Type); } }
    public string AttributeString { get { return Attribute != null ? Attribute.ToJson(false) : "{}"; } }
    
    public int GetEnumValue(string value) {
        if (mParser == null)
            throw new Exception($"Parser 为空 EnumType : {Type}  Value : {value}");
        return mParser.GetEnumValue(Type, value);
    }
    public PackageClass CustomType {
        get {
            if (mParser == null)
                throw new Exception($"Parser 为空 CustomType : {Type}");
            return mParser.GetClasses(Type);
        }
    }
    public string GetLanguageType(Language language) {
        if (IsBasic) {
            return BasicType.GetLanguageType(language);
        } else if (language == Language.Go) {
            return IsEnum ? Type : $"*{Type}";
        } else {
            return Type;
        }
    }

    public void Write(TableWriter writer, string value) {
        value = value.IsEmptyString() ? Default : value;
        if (!IsArray && (IsBasic || IsEnum)) {
            Write(writer, new ValueString(value));
        } else {
            Write(writer, new ValueParser($"[{value}]").GetObject() as ValueList);
        }
    }
    void Write(TableWriter writer, IValue value) {
        if (IsBasic || IsEnum) {
            if (IsArray) {
                var list = value as ValueList;
                if (list.IsEmptyValue()) {
                    writer.WriteInt32(0);
                } else {
                    writer.WriteInt32(list.Count);
                    for (var i = 0; i < list.Count; ++i) {
                        WriteBasic(writer, list[i].Value);
                    }
                }
            } else {
                WriteBasic(writer, value.Value);
            }
        } else {
            WriteCustom(writer, value as ValueList, IsArray);
        }
    }
    void WriteBasic(TableWriter writer, string value) {
        if (IsBasic) {
            BasicType.WriteValue(writer, value);
        } else if (IsEnum) {
            writer.WriteInt32(GetEnumValue(value));
        } else {
            throw new Exception($"当前类型不是基础类型 : {Type}");
        }
    }

    void WriteCustom(TableWriter writer, ValueList list, bool array) {
        var type = CustomType;
        if (array) {
            if (list.IsEmptyValue()) {
                writer.WriteInt32(0);
            } else {
                writer.WriteInt32(list.Count);
                for (var i = 0; i < list.Count; ++i) {
                    WriteCustom(writer, list[i] as ValueList, false);
                }
            }
        } else {
            if (list.IsEmptyValue()) {
                for (var i = 0; i < type.Fields.Count; ++i)
                    type.Fields[i].Write(writer, new ValueString(""));
            } else {
                var count = list.Count;
                if (count != type.Fields.Count)
                    throw new Exception($"字段数量与{type.Name}需求数量不一致 需要:{type.Fields.Count} 填写数量:{count} ");
                for (var i = 0; i < count; ++i)
                    type.Fields[i].Write(writer, list[i]);
            }
        }
    }
}
public class PackageClass : IPackage {
    public string Name;
    public List<FieldClass> Fields = new List<FieldClass>();
}