package Scorpio.Commons;

import Scorpio.*;
import Scorpio.Table.TableUtil;

public class ScorpioSerializer {
	private static class Invalid {
		public boolean value;
	}

	private static final String Index = "Index"; // 索引
	private static final String Name = "Name"; // 名字
	private static final String Type = "Type"; // 数据类型
	private static final String Array = "Array"; // 是否是数组

	private static final String BoolType = "bool";
	private static final String Int8Type = "int8";
	private static final String Int16Type = "int16";
	private static final String Int32Type = "int32";
	private static final String Int64Type = "int64";
	private static final String FloatType = "float";
	private static final String DoubleType = "double";
	private static final String StringType = "string";
	private static final String BytesType = "bytes";
	private static final String IntType = "int";

	/** 解析一个网络协议 */
	public static ScriptTable Deserialize(Script script, byte[] data, String name) throws Exception {
		return Read(script, new ScorpioReader(data), name, true);
	}

	/** 读取excel文件数据内容 */
	public static ScriptTable ReadDatas(Script script, ScriptTable tableManager, String fileName, String dataName, String keyName, String MD5) throws Exception {
		Scorpio.ScriptTable ret = script.CreateTable();
		ScorpioReader reader = new ScorpioReader(TableUtil.GetBuffer(fileName));
		int iRow = TableUtil.ReadHead(reader, fileName, MD5);
		Scorpio.ScriptTable data = null;
		double key = 0;
		for (int i = 0; i < iRow; ++i) {
			data = ScorpioSerializer.Read(script, reader, dataName, false);
			key = ScorpioUtil.ToDouble(data.GetValue(keyName).getObjectValue());
			if (ret.HasValue(key))
				throw new Exception("文件[" + fileName + "]有重复项 ID : " + key);
			ret.SetValue(key, data);
		}
		return ret;
	}

	/**
	 * 读取一个网络协议或者一行table数据
	 * 
	 * @param script
	 *            脚本引擎对象
	 * @param reader
	 *            读取类
	 * @param tableName
	 *            结构名字
	 * @param message
	 *            是否是网络协议
	 */
	private static ScriptTable Read(Script script, ScorpioReader reader, String tableName, boolean message) throws Exception {
		ScriptTable table = script.CreateTable();
		ScriptArray layout = (ScriptArray) script.GetValue(tableName);
		boolean isInvalid = true;
		int sign = message ? reader.ReadInt32() : 0;
		for (int i = 0; i < layout.Count(); ++i) {
			ScriptObject config = layout.GetValue(i);
			if (!message || ScorpioUtil.HasSign(sign, ScorpioUtil.ToInt32(config.GetValue(Index).getObjectValue()))) {
				String name = config.GetValue(Name).toString();
				String type = config.GetValue(Type).toString();
				boolean array = config.GetValue(Array).LogicOperation();
				Invalid invalid = new Invalid();
				if (array) {
					int count = reader.ReadInt32();
					ScriptArray value = script.CreateArray();
					for (int j = 0; j < count; ++j) {
						value.Add(ReadObject(script, reader, type, message, invalid));
					}
					table.SetValue(name, value);
					if (count > 0)
						isInvalid = false;
				} else {
					table.SetValue(name, ReadObject(script, reader, type, message, invalid));
					if (!invalid.value)
						isInvalid = false;
				}
			}
		}
		if (!message)
			table.SetValue("IsInvalid", script.CreateBool(isInvalid));
		return table;
	}

	private static ScriptObject ReadObject(Script script, ScorpioReader reader, String type, boolean message, Invalid invalid) throws Exception {
		Object value = ReadField(reader, type);
		if (value != null) {
			invalid.value = TableUtil.IsInvalid(value);
			return script.CreateObject(value);
		} else {
			ScriptTable ret = Read(script, reader, type, message);
			if (!message)
				invalid.value = ret.GetValue("IsInvalid").LogicOperation();
			return ret;
		}
	}

	private static Object ReadField(ScorpioReader reader, String type) {
		if (BoolType.equals(type)) {
			return reader.ReadBool();
		} else if (Int8Type.equals(type)) {
			return reader.ReadInt8();
		} else if (Int16Type.equals(type)) {
			return reader.ReadInt16();
		} else if (Int32Type.equals(type) || IntType.equals(type)) {
			return reader.ReadInt32();
		} else if (Int64Type.equals(type)) {
			return reader.ReadInt64();
		} else if (FloatType.equals(type)) {
			return reader.ReadFloat();
		} else if (DoubleType.equals(type)) {
			return reader.ReadDouble();
		} else if (StringType.equals(type)) {
			return reader.ReadString();
		} else if (BytesType.equals(type)) {
			return reader.ReadBytes();
		}
		return null;
	}

	/**
	 * 序列化一个网络协议
	 * 
	 * @param script
	 *            脚本引擎
	 * @param table
	 *            协议内容
	 * @param name
	 *            协议结构
	 */
	public static byte[] Serialize(Script script, ScriptTable table, String name) throws Exception {
		ScorpioWriter writer = new ScorpioWriter();
		Write(script, writer, table, name);
		return writer.ToArray();
	}

	private static void Write(Script script, ScorpioWriter writer, ScriptTable table, String tableName) throws Exception {
		ScriptArray layout = (ScriptArray) script.GetValue(tableName);
		int sign = 0;
		for (int i = 0; i < layout.Count(); ++i) {
			ScriptObject config = layout.GetValue(i);
			if (table.HasValue(config.GetValue(Name).getObjectValue())) {
				sign = ScorpioUtil.AddSign(sign, ScorpioUtil.ToInt32(config.GetValue(Index).getObjectValue()));
			}
		}
		writer.WriteInt32(sign);
		for (int i = 0; i < layout.Count(); ++i) {
			ScriptObject config = layout.GetValue(i);
			String name = (String) config.GetValue(Name).getObjectValue();
			if (table.HasValue(name)) {
				String type = (String) config.GetValue(Type).getObjectValue();
				Boolean array = (Boolean) config.GetValue(Array).getObjectValue();
				if (array) {
					ScriptArray arr = (ScriptArray) table.GetValue(name);
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

	private static void WriteObject(Script script, ScorpioWriter writer, String type, ScriptObject value) throws Exception {
		if (!WriteField(writer, type, value.getObjectValue())) {
			Write(script, writer, (ScriptTable) value, type);
		}
	}

	private static boolean WriteField(ScorpioWriter write, String type, Object value) {
		if (BoolType.equals(type)) {
			write.WriteBool(((Boolean) value).booleanValue());
		} else if (Int8Type.equals(type)) {
			write.WriteInt8(ScorpioUtil.ToInt8(value));
		} else if (Int16Type.equals(type)) {
			write.WriteInt16(ScorpioUtil.ToInt16(value));
		} else if (Int32Type.equals(type) || IntType.equals(type)) {
			write.WriteInt32(ScorpioUtil.ToInt32(value));
		} else if (Int64Type.equals(type)) {
			write.WriteInt64(ScorpioUtil.ToInt64(value));
		} else if (FloatType.equals(type)) {
			write.WriteFloat(ScorpioUtil.ToFloat(value));
		} else if (DoubleType.equals(type)) {
			write.WriteDouble(ScorpioUtil.ToDouble(value));
		} else if (StringType.equals(type)) {
			write.WriteString((String) value);
		} else if (BytesType.equals(type)) {
			write.WriteBytes((byte[]) value);
		} else {
			return false;
		}
		return true;
	}
}