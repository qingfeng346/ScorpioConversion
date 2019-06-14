// #if SCORPIO_PROTO_SCO
using System;
using System.Collections.Generic;
using System.Text;
using Scorpio;
using ScorpioProto.Table;
namespace ScorpioProto.Commons {
    public class ScorpioSerializer {
        const string TypeBool = "bool";
        const string TypeInt8 = "int8";
        const string TypeUInt8 = "uint8";
        const string TypeInt16 = "int16";
        const string TypeUInt16 = "uint16";
        const string TypeInt32 = "int32";
        const string TypeUInt32 = "uint32";
        const string TypeInt64 = "int64";
        const string TypeUInt64 = "uint64";
        const string TypeFloat = "float";
        const string TypeDouble = "double";
        const string TypeString = "string";
        const string TypeDateTime = "datetime";
        const string TypeBytes = "bytes";



        const string Index = "Index";           //索引
        const string Name = "Name";             //名字
        const string Type = "Type";             //数据类型
        const string Array = "Array";           //是否是数组

        const string Attribute = "Attribute";	//字段属性
        const string Language = "Language";		//该字段是否有多国语言



        //解析一个网络协议
        // public static ScriptTable Deserialize(Script script, byte[] data, string layoutTableName) {
        //     return Read(script, new ScorpioReader(data), null, "", layoutTableName, true);
        // }

        /// <summary>
        /// 脚本 读取excel文件数据内容
        /// </summary>
        /// <param name="script">脚本引擎</param>
        /// <param name="fileName">文件名字</param>
        /// <param name="reader">文件读取</param>
        /// <param name="dataArray">数据集合</param>
        /// <param name="layoutTableName">布局</param>
        /// <param name="keyName">主key名字</param>
        /// <param name="MD5">文件结构MD5</param>
        /// <returns></returns>
        public static ScriptMap ReadDatas(string fileName, IScorpioReader reader, ScriptMap dataArray, string layoutTableName, string keyName, string MD5) {
            using (reader) {
                var script = dataArray.getScript();
                var iRow = TableUtil.ReadHead(reader, fileName, MD5);                       //数据行数
                var layout = script.Global.GetValue(layoutTableName).Get<ScriptArray>();    //数据结构
                for (var i = 0; i < iRow; ++i) {
                    var data = Read(script, reader, layout);     //读取一行数据
                    var keyValue = data.GetValue(keyName);              //获取key值
                    data.SetValue("ID", keyValue);
                    var key = keyValue.Value;
                    if (dataArray.ContainsKey(key)) {
                        var value = dataArray.GetValue(key).Get<ScriptMap>();
                        foreach (var pair in data) {
                            value.SetValue(pair.Key, pair.Value);
                        }
                    } else {
                        dataArray.SetValue(key, new ScriptValue(data));
                    }
                }
                return dataArray;
            }
        }
        /// <summary>
        /// 读取一个网络协议或者一行table数据
        /// </summary>
        /// <param name="script">脚本引擎对象</param>
        /// <param name="reader">读取类</param>
        /// <param name="tableManager">tableManager类</param>
        /// <param name="fileName">table用文件名字</param>
        /// <param name="layout">数据结构</param>
        private static ScriptMap Read(Script script, IScorpioReader reader, ScriptArray layout) {
            var table = script.CreateMap();                             //返回的具体数据
            for (int i = 0; i < layout.Length(); ++i) {
                var config = layout.GetValue(i);                        //单个数据的定义
                var name = config.GetValue(Name).ToString();            //字段名字
                var type = config.GetValue(Type).ToString();            //字段类型
                var array = config.GetValue(Array).IsTrue;              //是否是数组
                if (array) {
                    var count = reader.ReadInt32();                     //读取元素个数
                    var value = script.CreateArray();
                    for (var j = 0; j < count; ++j) {
                        value.Add(ReadObject(script, reader, type));
                    }
                    table.SetValue(name, new ScriptValue(value));
                } else {
                    table.SetValue(name, ReadObject(script, reader, type));
                }
            }
            return table;
        }
        private static ScriptValue ReadObject(Script script, IScorpioReader reader, string type) {
            object value = ReadField(reader, type);
            if (value != null) {
                return ScriptValue.CreateObject(value);
            } else {
                return new ScriptValue(Read(script, reader, script.Global.GetValue(type).Get<ScriptArray>()));
            }
        }
        private static object ReadField(IScorpioReader reader, string type) {
            switch (type.ToLower()) {
                case TypeBool: return reader.ReadBool();
                case TypeInt8: return reader.ReadInt8();
                case TypeUInt8: return reader.ReadUInt8();
                case TypeInt16: return reader.ReadInt16();
                case TypeUInt16: return reader.ReadUInt16();
                case TypeInt32: return reader.ReadInt32();
                case TypeUInt32: return reader.ReadUInt32();
                case TypeInt64: return reader.ReadInt64();
                case TypeUInt64: return reader.ReadUInt64();
                case TypeFloat: return reader.ReadFloat();
                case TypeDouble: return reader.ReadDouble();
                case TypeString: return reader.ReadString();
                case TypeDateTime: return reader.ReadDateTime();
                case TypeBytes: return reader.ReadBytes();
                default: return null;
            }
        }

        /// <summary>
        /// 序列化一个网络协议
        /// </summary>
        /// <param name="script">脚本引擎</param>
        /// <param name="table">协议内容</param>
        /// <param name="name">协议结构</param>
        /// <returns></returns>
        // public static byte[] Serialize(Script script, ScriptTable table, string name) {
        //     //ScorpioWriter writer = new ScorpioWriter();
        //     //Write(script, writer, table, name);
        //     //return writer.ToArray();
        //     return null;
        // }
        //private static void Write(Script script, ScorpioWriter writer, ScriptTable table, string tableName) {
        //    ScriptArray layout = (ScriptArray)script.GetValue(tableName);
        //    int sign = 0;
        //    for (int i = 0; i < layout.Count(); ++i) {
        //        ScriptObject config = layout.GetValue(i);
        //        if (table != null && table.HasValue(config.GetValue(Name).ObjectValue))
        //            sign = ScorpioUtil.AddSign(sign, ScorpioUtil.ToInt32(config.GetValue(Index).ObjectValue));
        //    }
        //    writer.WriteInt32(sign);
        //    for (int i = 0; i < layout.Count(); ++i) {
        //        ScriptObject config = layout.GetValue(i);
        //        string name = (string)config.GetValue(Name).ObjectValue;
        //        if (table != null && table.HasValue(name)) {
        //            string type = (string)config.GetValue(Type).ObjectValue;
        //            bool array = (bool)config.GetValue(Array).ObjectValue;
        //            if (array) {
        //                ScriptArray arr = table.GetValue(name) as ScriptArray;
        //                writer.WriteInt32(arr.Count());
        //                for (int j = 0; j < arr.Count(); ++j) {
        //                    WriteObject(script, writer, type, arr.GetValue(j));
        //                }
        //            } else {
        //                WriteObject(script, writer, type, table.GetValue(name));
        //            }
        //        }
        //    }
        //}
        //private static void WriteObject(Script script, ScorpioWriter writer, string type, ScriptObject value) {
        //    if (!WriteField(writer, type, value.ObjectValue))
        //        Write(script, writer, (ScriptTable)value, type);
        //}
        //private static bool WriteField(ScorpioWriter write, string type, object value) {
        //    if (type == BoolType) {
        //        write.WriteBool((bool)value);
        //    } else if (type == Int8Type) {
        //        write.WriteInt8(ScorpioUtil.ToInt8(value));
        //    } else if (type == Int16Type) {
        //        write.WriteInt16(ScorpioUtil.ToInt16(value));
        //    } else if (type == Int32Type || type == IntType) {
        //        write.WriteInt32(ScorpioUtil.ToInt32(value));
        //    } else if (type == Int64Type) {
        //        write.WriteInt64(ScorpioUtil.ToInt64(value));
        //    } else if (type == FloatType) {
        //        write.WriteFloat(ScorpioUtil.ToFloat(value));
        //    } else if (type == DoubleType) {
        //        write.WriteDouble(ScorpioUtil.ToDouble(value));
        //    } else if (type == StringType) {
        //        write.WriteString((string)value);
        //    } else if (type == BytesType) {
        //        write.WriteBytes((byte[])value);
        //    } else {
        //        return false;
        //    }
        //    return true;
        //}
    }
}
// #endif