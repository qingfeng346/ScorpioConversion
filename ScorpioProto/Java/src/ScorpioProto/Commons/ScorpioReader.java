package ScorpioProto.Commons;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.util.Calendar;
public class ScorpioReader implements IScorpioReader {
	ByteBuffer reader;
	public ScorpioReader(byte[] buffer) {
		reader = ByteBuffer.wrap(buffer).order(ByteOrder.LITTLE_ENDIAN);
	}
	public int ReadHead(String fileName, String MD5) throws Exception {
		int iRow = ReadInt32();          //行数
		if (ReadString() != MD5)         //验证文件MD5(检测结构是否改变)
			throw new Exception("文件[" + fileName + "]版本验证失败");
		{
			var number = ReadInt32();        //字段数量
			for (var i = 0; i < number; ++i) {
				if (ReadInt8() == 0) {   //基础类型
					ReadInt8();          //基础类型索引
				} else {                        //自定义类
					ReadString();        //自定义类名称
				}
				ReadBool();          //是否是数组
			}
		}
		{
			var customNumber = ReadInt32();  //自定义类数量
			for (var i = 0; i < customNumber; ++i) {
				ReadString();                //读取自定义类名字
				var number = ReadInt32();        //字段数量
				for (var j = 0; j < number; ++j) {
					if (ReadInt8() == 0) {   //基础类型
						ReadInt8();          //基础类型索引
					} else {                        //自定义类
						ReadString();        //自定义类名称
					}
					ReadBool();          //是否是数组
				}
			}
		}
		return iRow;
	}
	public boolean ReadBool() {
		return ReadInt8() == (byte) 1;
	}
	public byte ReadInt8() {
		return reader.get();
	}
	public byte ReadUInt8() {
		return reader.get();
	}
	public short ReadInt16() {
		return reader.getShort();
	}
	public short ReadUInt16() {
		return reader.getShort();
	}
	public int ReadInt32() {
		return reader.getInt();
	}
	public int ReadUInt32() {
		return reader.getInt();
	}
	public long ReadInt64() {
		return reader.getLong();
	}
	public long ReadUInt64() {
		return reader.getLong();
	}
	public float ReadFloat() {
		return reader.getFloat();
	}
	public double ReadDouble() {
		return reader.getDouble();
	}
	public String ReadString() {
		try {
			int length = ReadUInt16();
			byte[] bytes = new byte[length];
			reader.get(bytes);
			return new String(bytes, "utf-8");
		} catch (Exception e) { }
		return "";
	}
	public Calendar ReadDateTime() {
		Calendar calendar = Calendar.getInstance();
		calendar.setTimeInMillis(ReadInt64());
		return calendar;
	}
	public byte[] ReadBytes() {
		int length = ReadUInt16();
		byte[] bytes = new byte[length];
		reader.get(bytes);
		return bytes;
	}
}