using System;
using System.Collections.Generic;
using System.Text;
using Scorpio;
using Scorpio.Table;
namespace Scorpio.Commons
{
    public class ScorpioSerializer {
        private class Invalid { public bool value; }
        const string Index = "Index";       //索引
        const string Name = "Name";         //名字
        const string Type = "Type";         //数据类型
        const string Array = "Array";       //是否是数组

        const string BoolType = "bool";
        const string Int8Type = "int8";
        const string Int16Type = "int16";
        const string Int32Type = "int32";
        const string Int64Type = "int64";
        const string FloatType = "float";
        const string DoubleType = "double";
        const string StringType = "string";
        const string BytesType = "bytes";
        const string IntType = "int";

        public static ScriptTable Deserialize(Script script, byte[] data, string name, bool hasSign)
        {
            return Read(script, new ScorpioReader(data), name, hasSign);
        }
        public static ScriptTable Read(Script script, ScorpioReader reader, string tableName, bool hasSign)
        {
            ScriptTable table = script.CreateTable();
            ScriptArray layout = (ScriptArray)script.GetValue(tableName);
            bool isInvalid = true;
            int sign = hasSign ? reader.ReadInt32() : 0;
            for (int i = 0; i < layout.Count(); ++i) {
                ScriptObject config = layout.GetValue(i);
                if (!hasSign || ScorpioUtil.HasSign(sign, ScorpioUtil.ToInt32(config.GetValue(Index).ObjectValue))) {
                    string name = (string)config.GetValue(Name).ObjectValue;
                    string type = (string)config.GetValue(Type).ObjectValue;
                    bool array = (bool)config.GetValue(Array).ObjectValue;
                    Invalid invalid = new Invalid();
                    if (array) {
                        int count = reader.ReadInt32();
                        ScriptArray value = script.CreateArray();
                        for (int j = 0; j < count;++j ) {
							value.Add(ReadObject(script, reader, type, hasSign, invalid));
                        }
						table.SetValue(name, value);
                        if (count > 0) isInvalid = false;
                    } else {
                        table.SetValue(name, ReadObject(script, reader, type, hasSign, invalid));
                        if (!invalid.value) isInvalid = false;
                    }
                }
            }
            table.SetValue("IsInvalid", script.CreateBool(isInvalid));
            return table;
        }
        private static ScriptObject ReadObject(Script script, ScorpioReader reader, string type, bool hasSign, Invalid invalid)
        {
            object value = ReadField(reader, type);
            if (value != null) {
                invalid.value = TableUtil.IsInvalid(value);
                return script.CreateObject(value);
            } else {
                ScriptTable ret = Read(script, reader, type, hasSign);
                invalid.value = (bool)ret.GetValue("IsInvalid").ObjectValue;
                return ret;
            }
        }
        private static object ReadField(ScorpioReader reader, string type)
        {
            if (type == BoolType) {
                return reader.ReadBool();
            } else if (type == Int8Type) {
                return reader.ReadInt8();
            } else if (type == Int16Type) {
                return reader.ReadInt16();
            } else if (type == Int32Type || type == IntType) {
                return reader.ReadInt32();
            } else if (type == Int64Type) {
                return reader.ReadInt64();
            } else if (type == FloatType) {
                return reader.ReadFloat();
            } else if (type == DoubleType) {
                return reader.ReadDouble();
            } else if (type == StringType) {
                return reader.ReadString();
            } else if (type == BytesType) {
                return reader.ReadBytes();
            }
            return null;
        }

        public static byte[] Serialize(Script script, ScriptTable table, string name)
        {
            ScorpioWriter writer = new ScorpioWriter();
            Write(script, writer, table, name);
            return writer.ToArray();
        }
        private static void Write(Script script, ScorpioWriter writer, ScriptTable table, string tableName)
        {
            ScriptArray layout = (ScriptArray)script.GetValue(tableName);
            int sign = 0;
            for (int i = 0; i < layout.Count(); ++i)
            {
                ScriptObject config = layout.GetValue(i);
                if (table != null && table.HasValue(config.GetValue(Name).ObjectValue))
                    sign = ScorpioUtil.AddSign(sign, ScorpioUtil.ToInt32(config.GetValue(Index).ObjectValue));
            }
            writer.WriteInt32(sign);
            for (int i = 0; i < layout.Count(); ++i)
            {
                ScriptObject config = layout.GetValue(i);
                string name = (string)config.GetValue(Name).ObjectValue;
                if (table != null && table.HasValue(name)) {
                    string type = (string)config.GetValue(Type).ObjectValue;
                    bool array = (bool)config.GetValue(Array).ObjectValue;
                    if (array) {
                        ScriptArray arr = table.GetValue(name) as ScriptArray;
                        writer.WriteInt32(arr.Count());
                        for (int j = 0; j < arr.Count(); ++j) {
                            WriteObject(script, writer, type, arr.GetValue(j));
                        }
                    } else {
                        WriteObject(script, writer, type, table.GetValue(name));
                    }
                }
            }
        }
        private static void WriteObject(Script script, ScorpioWriter writer, string type, ScriptObject value)
        {
            if (!WriteField(writer, type, value.ObjectValue))
                Write(script, writer, (ScriptTable)value, type);
        }
        private static bool WriteField(ScorpioWriter write, string type, object value)
        {
            if (type == BoolType) {
                write.WriteBool((bool)value);
            } else if (type == Int8Type) {
                write.WriteInt8(ScorpioUtil.ToInt8(value));
            } else if (type == Int16Type) {
                write.WriteInt16(ScorpioUtil.ToInt16(value));
            } else if (type == Int32Type || type == IntType) {
                write.WriteInt32(ScorpioUtil.ToInt32(value));
            } else if (type == Int64Type) {
                write.WriteInt64(ScorpioUtil.ToInt64(value));
            } else if (type == FloatType) {
                write.WriteFloat(ScorpioUtil.ToFloat(value));
            } else if (type == DoubleType) {
                write.WriteDouble(ScorpioUtil.ToDouble(value));
            } else if (type == StringType) {
                write.WriteString((string)value);
            } else if (type == BytesType) {
                write.WriteBytes((byte[])value);
            } else {
                return false;
            }
            return true;
        }
    }
}
