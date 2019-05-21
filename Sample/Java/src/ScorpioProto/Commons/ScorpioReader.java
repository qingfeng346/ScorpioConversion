package ScorpioProto.Commons;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.util.Calendar;
public class ScorpioReader implements IScorpioReader {
	ByteBuffer reader;
	public ScorpioReader(byte[] buffer) {
		reader = ByteBuffer.wrap(buffer).order(ByteOrder.LITTLE_ENDIAN);
	}
	public boolean ReadBool() {
		return ReadInt8() == (byte) 1;
	}
	public byte ReadInt8() {
		return reader.get();
	}
	public short ReadInt16() {
		return reader.getShort();
	}
	public int ReadInt32() {
		return reader.getInt();
	}
	public long ReadInt64() {
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
			int length = ReadInt32();
			byte[] bytes = new byte[length];
			reader.get(bytes);
			return new String(bytes, "utf-8");
		} catch (Exception e) {
			
		}
		return "";
	}
	public Calendar ReadDateTime() {
		Calendar calendar = Calendar.getInstance();
		calendar.setTimeInMillis(ReadInt64());
		return calendar;
	}
	public byte[] ReadBytes() {
		int length = reader.getInt();
		byte[] bytes = new byte[length];
		reader.get(bytes);
		return bytes;
	}
}