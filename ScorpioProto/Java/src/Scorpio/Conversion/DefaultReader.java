package Scorpio.Conversion;
import java.io.EOFException;
import java.io.IOException;
import java.io.InputStream;
import java.util.Date;
public class DefaultReader implements IReader {
	private byte readBuffer[] = new byte[8];
	private InputStream stream;
	private boolean closeStream;
	public DefaultReader(InputStream stream, boolean closeStream) {
		this.stream = stream;
		this.closeStream = closeStream;
	}
	private final void readFully(byte b[], int off, int len) throws IOException {
		int n = 0;
        while (n < len) {
            int count = stream.read(b, n, len - n);
            if (count < 0)
                throw new EOFException();
            n += count;
        }
    }
	public boolean ReadBool() throws Exception {
		return ReadInt8() == (byte) 1;
	}
	public byte ReadInt8() throws Exception {
		return (byte) stream.read();
	}
	public int ReadUInt8() throws Exception {
		return stream.read();
	}
	public short ReadInt16() throws Exception {
		return (short)ReadUInt16();
	}
	public int ReadUInt16() throws Exception {
		int ch1 = stream.read();
        int ch2 = stream.read();
        return (ch2 << 8) + (ch1 << 0);
	}
	public int ReadInt32() throws Exception {
		int ch1 = stream.read();
        int ch2 = stream.read();
        int ch3 = stream.read();
        int ch4 = stream.read();
		return ((ch4 << 24) + (ch3 << 16) + (ch2 << 8) + (ch1 << 0));
	}
	public int ReadUInt32() throws Exception {
		return ReadInt32();
	}
	public long ReadInt64() throws Exception {
		readFully(readBuffer, 0, 8);
        return (((long)readBuffer[7] << 56) +
                ((long)(readBuffer[6] & 255) << 48) +
                ((long)(readBuffer[5] & 255) << 40) +
                ((long)(readBuffer[4] & 255) << 32) +
                ((long)(readBuffer[3] & 255) << 24) +
                ((readBuffer[2] & 255) << 16) +
                ((readBuffer[1] & 255) <<  8) +
                ((readBuffer[0] & 255) <<  0));
	}
	public long ReadUInt64() throws Exception {
		return ReadInt64();
	}
	public float ReadFloat() throws Exception {
		return Float.intBitsToFloat(ReadInt32());
	}
	public double ReadDouble() throws Exception {
		return Double.longBitsToDouble(ReadInt64());
	}
	public String ReadString() throws Exception {
		int length = ReadUInt16();
		if (length == 0) { return ""; }
		byte[] bytes = new byte[length];
		readFully(bytes, 0, length);
		return new String(bytes, "utf-8");
	}
	public Date ReadDateTime() throws Exception {
		return new Date(ReadInt64());
	}
	public byte[] ReadBytes() throws Exception {
		int length = ReadInt32();
		byte[] bytes = new byte[length];
		readFully(bytes, 0, length);
		return bytes;
	}
	public void Close() throws Exception {
		if (closeStream) {
			this.stream.close();
		}
	}
}