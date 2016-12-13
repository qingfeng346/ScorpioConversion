package Scorpio.Table;
import java.util.List;
import Scorpio.Commons.*;
public class TableUtil
{
	public interface ITableUtil {
        byte[] GetBuffer(String resource);
        void Warning(String str);
    }
    private static ITableUtil IUtil = null;
	public static void SetTableUtil(ITableUtil util) {
		IUtil = util;
	}
	public static byte[] GetBuffer(String resource) {
		return IUtil != null ? IUtil.GetBuffer(resource) : null;
	}
	public static void Warning(String str) {
		if (IUtil != null)
			IUtil.Warning(str);
	}
    public static Scorpio.ScriptTable ReadDatas(Scorpio.Script script, String fileName, String dataName, String keyName, String MD5) throws Exception
    {
    	Scorpio.ScriptTable ret = script.CreateTable();
        ScorpioReader reader = new ScorpioReader(GetBuffer(fileName));
        int iRow = ReadHead(reader, fileName, MD5);
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
    public static int ReadHead(ScorpioReader reader, String fileName, String MD5) 
    {
    	int iRow = reader.ReadInt32();          //行数   
        if (!reader.ReadString().equals(MD5))   //验证文件MD5(检测结构是否改变)
            throw new RuntimeException("文件" + fileName + "版本验证失败");
        int i,j,number;
        {
            number = reader.ReadInt32();        //字段数量
            for (i = 0; i < number; ++i) {
                if (reader.ReadInt8() == 0) {   //基础类型
                    reader.ReadInt8();          //基础类型索引
                    reader.ReadBool();          //是否是数组
                } else {                        //自定义类
                    reader.ReadString();        //自定义类名称
                    reader.ReadBool();          //是否是数组
                }
            }
        }
        int customNumber = reader.ReadInt32();  //自定义类数量
        for (i = 0; i < customNumber; ++i) {
            reader.ReadString();                //读取自定义类名字
            number = reader.ReadInt32();        //字段数量
            for (j = 0; j < number; ++j) {
                if (reader.ReadInt8() == 0) {   //基础类型
                    reader.ReadInt8();          //基础类型索引
                    reader.ReadBool();          //是否是数组
                } else {                        //自定义类
                    reader.ReadString();        //自定义类名称
                    reader.ReadBool();          //是否是数组
                }
            }
        }
        return iRow;
    }
	private static final Byte INVALID_INT8 = Byte.MAX_VALUE;
	private static final Short INVALID_INT16 = Short.MAX_VALUE;
	private static final Integer INVALID_INT32 = Integer.MAX_VALUE;
	private static final Long INVALID_INT64 = Long.MAX_VALUE;
	private static final Float INVALID_FLOAT = -1.0f;
	private static final Double INVALID_DOUBLE = -1.0;
	public static boolean IsInvalidInt8(byte val) {
		return (val == INVALID_INT8);
	}
	public static boolean IsInvalidInt16(short val) {
		return (val == INVALID_INT16);
	}
	public static boolean IsInvalidInt32(int val) {
		return (val == INVALID_INT32);
	}
	public static boolean IsInvalidInt64(long val) {
		return (val == INVALID_INT64);
	}
	public static boolean IsInvalidFloat(float val) {
		return (Math.abs(INVALID_FLOAT - val) < 0.001f);
	}
	public static boolean IsInvalidDouble(double val) {
		return (Math.abs(INVALID_DOUBLE - val) < 0.001f);
	}
	public static boolean IsInvalidString(String val) {
		return (val == null || val.isEmpty());
	}
	public static boolean IsInvalidList(List<?> val) {
		return val.size() == 0;
	}
	public static boolean IsInvalidData(IData val) {
		return val.IsInvalid();
	}
    public static boolean IsInvalid(Object val) {
        if (val instanceof Byte)
            return IsInvalidInt8((Byte)val);
        else if (val instanceof Short)
            return IsInvalidInt16((Short)val);
        else if (val instanceof Integer)
            return IsInvalidInt32((Integer)val);
        else if (val instanceof Long)
            return IsInvalidInt64((Long)val);
        else if (val instanceof Float)
            return IsInvalidFloat((Float)val);
        else if (val instanceof Double)
            return IsInvalidDouble((Double)val);
        else if (val instanceof String)
            return IsInvalidString((String)val);
        else if (val instanceof List<?>)
        	return IsInvalidList((List<?>)val);
        else if (val instanceof IData)
        	return IsInvalidData((IData)val);
        return false;
    }
}
