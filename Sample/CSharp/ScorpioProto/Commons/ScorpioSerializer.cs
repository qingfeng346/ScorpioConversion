//// #if SCORPIO_PROTO_SCO
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Scorpio;
//using ScorpioProto.Table;
//namespace ScorpioProto.Commons {
//    public class ScorpioSerializer {
//        private class Invalid { public bool value; }
//        const string Index = "Index";           //索引
//        const string Name = "Name";             //名字
//        const string Type = "Type";             //数据类型
//        const string Array = "Array";           //是否是数组

//        const string Attribute = "Attribute";	//字段属性
//        const string Language = "Language";		//该字段是否有多国语言

//        const string BoolType = "bool";
//        const string Int8Type = "int8";
//        const string Int16Type = "int16";
//        const string Int32Type = "int32";
//        const string Int64Type = "int64";
//        const string FloatType = "float";
//        const string DoubleType = "double";
//        const string StringType = "string";
//        const string DateTimeType = "datetime";
//        const string BytesType = "bytes";

//        //解析一个网络协议
//        // public static ScriptTable Deserialize(Script script, byte[] data, string layoutTableName) {
//        //     return Read(script, new ScorpioReader(data), null, "", layoutTableName, true);
//        // }

//        /// <summary>
//        /// 脚本 读取excel文件数据内容
//        /// </summary>
//        /// <param name="script"></param>
//        /// <param name="fileName"></param>
//        /// <param name="reader"></param>
//        /// <param name="dataArray"></param>
//        /// <param name="layoutTableName"></param>
//        /// <param name="keyName"></param>
//        /// <param name="MD5"></param>
//        /// <returns></returns>
//        public static ScriptTable ReadDatas(Script script, string fileName, IScorpioReader reader, ScriptTable dataArray, string layoutTableName, string keyName, string MD5) {
//            var iRow = TableUtil.ReadHead(reader, fileName, MD5);
//            for (var i = 0; i < iRow; ++i) {
//                var data = Read(script, reader, layoutTableName, false);
//                data.SetValue("ID", data.GetValue(keyName));
//                var key = data.GetValue(keyName).ObjectValue;
//                if (dataArray.HasValue(key)) {
//                    var value = dataArray.GetValue(key) as ScriptTable;
//                    var itor = data.GetIterator();
//                    while (itor.MoveNext()) {
//                        value.SetValue(itor.Current.Key, itor.Current.Value);
//                    }
//                } else {
//                    dataArray.SetValue(key, data);
//                }
//            }
//            return dataArray;
//        }
//        /// <summary>
//        /// 读取一个网络协议或者一行table数据
//        /// </summary>
//        /// <param name="script">脚本引擎对象</param>
//        /// <param name="reader">读取类</param>
//        /// <param name="tableManager">tableManager类</param>
//        /// <param name="fileName">table用文件名字</param>
//        /// <param name="layoutTableName">结构名字</param>
//        /// <param name="message">是否有标识</param>
//        private static ScriptTable Read(Script script, IScorpioReader reader, string layoutTableName, bool message) {
//            var table = script.CreateTable();                               //返回的具体数据
//            var layout = (ScriptArray)script.GetValue(layoutTableName);     //数据结构
//            var sign = message ? reader.ReadInt32() : 0;
//            var isInvalid = true;
//            //string id = null;
//            for (int i = 0; i < layout.Count(); ++i) {
//                var config = layout.GetValue(i);        //单个数据的定义
//                if (message && !ScorpioUtil.HasSign(sign, ScorpioUtil.ToInt32(config.GetValue(Index).ObjectValue))) { continue; }
//                var name = config.GetValue(Name).ToString();             //字段名字
//                var type = config.GetValue(Type).ToString();             //字段类型
//                var array = config.GetValue(Array).LogicOperation();     //是否是数组
//                var invalid = new Invalid();                             //本行是否是无效行
//                if (array) {
//                    var count = reader.ReadInt32();                      //读取元素个数
//                    var value = script.CreateArray();
//                    for (var j = 0; j < count; ++j) {
//                        value.Add(ReadObject(script, reader, type, message, invalid));
//                    }
//                    table.SetValue(name, value);
//                    if (count > 0) isInvalid = false;
//                } else {
//                    if (message) {
//                        table.SetValue(name, ReadObject(script, reader, type, message, invalid));
//                    } else {
//                        //var attribute = config.GetValue(Attribute) as ScriptTable;
//                        //if (attribute.Count() == 0) {
//                            table.SetValue(name, ReadObject(script, reader, type, message, invalid));
//                            if (!invalid.value) isInvalid = false;
//                            //if (string.IsNullOrEmpty(id)) {
//                            //    id = ScorpioUtil.ToInt32(obj.ObjectValue).ToString();
//                            //}
//                        //} else {
//                        //    ReadObject(script, reader, tableManager, fileName, type, message, invalid);     //先读取一个值  如果是多国语言 生成数据的时候会写入一个空字符串
//                        //    table.SetValue(name, script.CreateObject(tableManager.GetValue("getValue").call(attribute, fileName, name, id)));
//                        //}
//                    }
//                }
//            }
//            if (!message) table.SetValue("IsInvalid", script.CreateBool(isInvalid));
//            return table;
//        }
//        private static ScriptObject ReadObject(Script script, IScorpioReader reader, string type, bool message, Invalid invalid) {
//            object value = ReadField(reader, type);
//            if (value != null) {
//                invalid.value = TableUtil.IsInvalid(value);
//                return script.CreateObject(value);
//            } else {
//                var ret = Read(script, reader, type, message);
//                if (!message) invalid.value = ret.GetValue("IsInvalid").LogicOperation();
//                return ret;
//            }
//        }
//        private static object ReadField(IScorpioReader reader, string type) {
//            switch (type.ToLower()) {
//                case BoolType: return reader.ReadBool();
//                case Int8Type: return reader.ReadInt8();
//                case Int16Type: return reader.ReadInt16();
//                case Int32Type: return reader.ReadInt32();
//                case Int64Type: return reader.ReadInt64();
//                case FloatType: return reader.ReadFloat();
//                case DoubleType: return reader.ReadDouble();
//                case StringType: return reader.ReadString();
//                case DateTimeType: return reader.ReadDateTime();
//                case BytesType: return reader.ReadBytes();
//                default: return null;
//            }
//        }

//        /// <summary>
//        /// 序列化一个网络协议
//        /// </summary>
//        /// <param name="script">脚本引擎</param>
//        /// <param name="table">协议内容</param>
//        /// <param name="name">协议结构</param>
//        /// <returns></returns>
//        public static byte[] Serialize(Script script, ScriptTable table, string name) {
//            //ScorpioWriter writer = new ScorpioWriter();
//            //Write(script, writer, table, name);
//            //return writer.ToArray();
//            return null;
//        }
//        //private static void Write(Script script, ScorpioWriter writer, ScriptTable table, string tableName) {
//        //    ScriptArray layout = (ScriptArray)script.GetValue(tableName);
//        //    int sign = 0;
//        //    for (int i = 0; i < layout.Count(); ++i) {
//        //        ScriptObject config = layout.GetValue(i);
//        //        if (table != null && table.HasValue(config.GetValue(Name).ObjectValue))
//        //            sign = ScorpioUtil.AddSign(sign, ScorpioUtil.ToInt32(config.GetValue(Index).ObjectValue));
//        //    }
//        //    writer.WriteInt32(sign);
//        //    for (int i = 0; i < layout.Count(); ++i) {
//        //        ScriptObject config = layout.GetValue(i);
//        //        string name = (string)config.GetValue(Name).ObjectValue;
//        //        if (table != null && table.HasValue(name)) {
//        //            string type = (string)config.GetValue(Type).ObjectValue;
//        //            bool array = (bool)config.GetValue(Array).ObjectValue;
//        //            if (array) {
//        //                ScriptArray arr = table.GetValue(name) as ScriptArray;
//        //                writer.WriteInt32(arr.Count());
//        //                for (int j = 0; j < arr.Count(); ++j) {
//        //                    WriteObject(script, writer, type, arr.GetValue(j));
//        //                }
//        //            } else {
//        //                WriteObject(script, writer, type, table.GetValue(name));
//        //            }
//        //        }
//        //    }
//        //}
//        //private static void WriteObject(Script script, ScorpioWriter writer, string type, ScriptObject value) {
//        //    if (!WriteField(writer, type, value.ObjectValue))
//        //        Write(script, writer, (ScriptTable)value, type);
//        //}
//        //private static bool WriteField(ScorpioWriter write, string type, object value) {
//        //    if (type == BoolType) {
//        //        write.WriteBool((bool)value);
//        //    } else if (type == Int8Type) {
//        //        write.WriteInt8(ScorpioUtil.ToInt8(value));
//        //    } else if (type == Int16Type) {
//        //        write.WriteInt16(ScorpioUtil.ToInt16(value));
//        //    } else if (type == Int32Type || type == IntType) {
//        //        write.WriteInt32(ScorpioUtil.ToInt32(value));
//        //    } else if (type == Int64Type) {
//        //        write.WriteInt64(ScorpioUtil.ToInt64(value));
//        //    } else if (type == FloatType) {
//        //        write.WriteFloat(ScorpioUtil.ToFloat(value));
//        //    } else if (type == DoubleType) {
//        //        write.WriteDouble(ScorpioUtil.ToDouble(value));
//        //    } else if (type == StringType) {
//        //        write.WriteString((string)value);
//        //    } else if (type == BytesType) {
//        //        write.WriteBytes((byte[])value);
//        //    } else {
//        //        return false;
//        //    }
//        //    return true;
//        //}
//    }
//}
//// #endif