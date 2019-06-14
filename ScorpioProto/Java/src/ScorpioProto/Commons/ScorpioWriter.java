package ScorpioProto.Commons;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
public class ScorpioWriter {
	private static final int MAX_LENGTH = 1024 * 1024;
	ByteBuffer writer;
	public ScorpioWriter() {
		writer = ByteBuffer.allocate(MAX_LENGTH).order(ByteOrder.LITTLE_ENDIAN);
	}
	public void WriteBool(boolean value) {
		writer.put(value ? (byte) 1 : (byte) 0);
	}
	public void WriteInt8(byte value) {
		writer.put(value);
	}
	public void WriteInt16(short value) {
		writer.putShort(value);
	}
	public void WriteInt32(int value) {
		writer.putInt(value);
	}
	public void WriteInt64(long value) {
		writer.putLong(value);
	}
	public void WriteFloat(float value) {
		writer.putFloat(value);
	}
	public void WriteDouble(double value) {
		writer.putDouble(value);
	}
	public void WriteString(String value) {
		try {
			if (value == null || value == "") {
				writer.put((byte) 0);
			} else {
				writer.put(value.getBytes("utf-8"));
				writer.put((byte) 0);
			}
		} catch (Exception e) {
		}
	}
	public void WriteBytes(byte[] value) {
		writer.putInt((int) value.length);
		writer.put(value);
	}
	public byte[] ToArray() {
		writer.flip();
		writer.rewind();
		byte[] bytes = new byte[writer.limit()];
		writer.get(bytes);
		return bytes;
	}
	public void Close() {
	}
}