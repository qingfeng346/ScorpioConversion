using System.Collections.Generic;
namespace Scorpio.Conversion {
    public class ClassField {
        private PackageParser mParser;
        public ClassField(PackageParser parser) { mParser = parser; }

        public int Index { get; set; }                   //字段索引
        public string Name { get; set; }                 //字段名字
        public string Type { get; set; }                 //字段类型
        public string Default { get; set; }              //字段默认值
        public string Comment { get; set; }              //字段注释
        public bool IsL10N { get; set; } = false;        //需要翻译的字段 
        public ScriptMap Attribute { get; set; }         //字段属性,字段配置
        public string MaxValue { get; set; }             //最大值（用于校验数据）
        public string MinValue { get; set; }             //最小值（用于校验数据）
        public bool IsArray { get; set; } = false;       //是否是数组
        public bool IsInvalid { get; set; } = false;     //是否是无效字段 !
        public bool IsValid => !IsInvalid && !Name.IsEmptyString() && !Type.IsEmptyString();            //字段是否有效
        public BasicType BasicType => BasicUtil.GetType(Type);      //基本数据类型
        public bool IsBasic => BasicType != null;                   //是否是基本数据

        public bool IsDateTime => IsBasic && BasicType.Index == BasicEnum.DATETIME;     //是否是时间
        public bool IsString => IsBasic && BasicType.Index == BasicEnum.STRING;         //是否是string类型
        public bool IsBool => IsBasic && BasicType.Index == BasicEnum.BOOL;             //是否是bool类型
        public bool IsEnum => mParser != null && mParser.Enums.ContainsKey(Type);       //是否是枚举

        public int GetEnumValue(string value) {
            if (mParser == null)
                throw new System.Exception($"Parser 为空 EnumType : {Type}  Value : {value}");
            return mParser.GetEnumValue(Type, value);
        }

        public PackageEnum CustomEnum {
            get {
                if (mParser == null)
                    throw new System.Exception($"Parser 为空 CustomEnum : {Type}");
                return mParser.GetEnum(Type);
            }
        }
        public PackageClass CustomType {
            get {
                if (mParser == null)
                    throw new System.Exception($"Parser 为空 CustomType : {Type}");
                return mParser.GetClasses(Type);
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
                throw new System.Exception($"当前类型不是基础类型 : {Type}");
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
                        if (list[i] is ValueList) {
                            WriteCustom(writer, list[i] as ValueList, false);
                        } else {
                            var valueString = list[i] as ValueString;
                            //数据为空
                            if (valueString.value.IsEmptyString()) {
                                WriteCustom(writer, null, false);
                            } else {
                                throw new System.Exception($"数据填写错误,{valueString.value}");
                            }
                        }
                    }
                }
            } else {
                if (list.IsEmptyValue()) {
                    for (var i = 0; i < type.Fields.Count; ++i)
                        type.Fields[i].Write(writer, new ValueString(""));
                } else {
                    var count = list.Count;
                    for (var i = 0; i < type.Fields.Count; ++i) {
                        var field = type.Fields[i];
                        if (i < count) {
                            field.Write(writer, list[i]);
                        } else if (field.Default == null) {
                            throw new System.Exception($"字段数量与{type.Name}需求数量不一致 需要:{type.Fields.Count} 填写数量:{count} ");
                        } else {
                            field.Write(writer, new ValueString(field.Default));
                        }
                    }
                }
            }
        }
        public override string ToString() {
            return (IsArray ? "Array" : "") + Name;
        }
    }
    public class PackageClass : IPackage {
        public string Name { get; set; }
        public List<ClassField> Fields = new List<ClassField>();
        public override string ToString() {
            return Name;
        }
    }
}