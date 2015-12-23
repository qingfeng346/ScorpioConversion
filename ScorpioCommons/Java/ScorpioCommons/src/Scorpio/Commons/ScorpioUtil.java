package Scorpio.Commons;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.List;

public final class ScorpioUtil {
    public static boolean HasSign(int sign, int index) {
        return (sign & (1 << index)) != 0;
    }
    public static int AddSign(int sign, int index) {
        if ((sign & (1 << index)) == 0) {
            sign |= (1 << index);
        }
        return sign;
    }
    public static void WriteString(ByteBuffer writer, String value)
    {
    	try {
            if (value == null || value == "")  {
            	writer.put((byte)0);
            } else {
            	writer.put(value.getBytes("utf-8"));
            	writer.put((byte)0);
            }
    	} catch (Exception e) { }
    }
    public static String ReadString(ByteBuffer reader)
    {
    	try	{
            List<Byte> sb = new ArrayList<Byte>();
            byte ch;
            while ((ch = reader.get()) != 0)
            	sb.add(ch);
            byte[] bytes = new byte[sb.size()];
            for (int i=0;i<sb.size();++i)
            	bytes[i] = sb.get(i);
            return new String(bytes, "utf-8");
    	} catch (Exception e) {}
    	return "";
    }
    public static byte ToInt8(Object value) {
    	return ((Number)value).byteValue();
    }
    public static short ToInt16(Object value) {
    	return ((Number)value).shortValue();
    }
    public static int ToInt32(Object value) {
    	return ((Number)value).intValue();
    }
    public static long ToInt64(Object value) {
    	return ((Number)value).longValue();
    }
    public static float ToFloat(Object value) {
    	return ((Number)value).floatValue();
    }
    public static double ToDouble(Object value) {
    	return ((Number)value).doubleValue();
    }
}