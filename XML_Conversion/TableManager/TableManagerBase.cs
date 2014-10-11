using System;
using System.Collections.Generic;
using System.Text;


public partial class TableManager
{
    public void CreateBaseCS()
    {
        PROGRAM program = PROGRAM.CS;
        string strDataBase = @"using System;
public abstract class MT_DataBase {
    public abstract bool IsInvalid();
    public abstract object GetDataByString(string str);
    public T GetDataByStringTemplate<T>(string str)
    {
        object obj = GetDataByString(str);
        return (T)obj;
    }
}
";
        FileUtil.CreateFile("MT_DataBase.cs", strDataBase, true, Util.GetProgramInfo(program).CodeDirectory.Split(';'));

        string strTableBase = @"using System;
public abstract class MT_TableBase
{
    public virtual bool Contains(Int32 ID) { return false; }
    public virtual bool Contains(string ID) { return false; }
    public virtual MT_DataBase GetValue(int key) { return null; }
    public virtual MT_DataBase GetValue(string key) { return null; }
    public abstract Int32 Count();
}
";
        FileUtil.CreateFile("MT_TableBase.cs", strTableBase, true, Util.GetProgramInfo(program).CodeDirectory.Split(';'));

        Config config = new Config("CustomLayout.ini", true);
        string strUtilBase = @"using System;
using System.IO;
public class TableUtil
{
    public interface ITableUtil
    {
        string ReadString(BinaryReader reader);
        string ReadFString(BinaryReader reader);
        string ReadLString(BinaryReader reader, string fileName, string lineName, int id);
        string ReadLString(BinaryReader reader, string fileName, string lineName, string id);
        byte[] GetBuffer(string resource);
        void Warning(string str);
        void Error(string str);
    }
    public const int CLASS_VALUE = __ClassValue;
    private static ITableUtil util = null;
    public static void SetUtil(ITableUtil iutil) {
        util = iutil;
    }
    public static string ReadString(BinaryReader reader)
    {
        return util.ReadString(reader);
    }
    public static string ReadFString(BinaryReader reader)
    {
        return util.ReadFString(reader);
    }
    public static string ReadLString(BinaryReader reader, string fileName, string lineName, int id)
    {
        ReadString(reader);
        return util.ReadLString(reader, fileName, lineName, id);
    }
    public static string ReadLString(BinaryReader reader, string fileName, string lineName, string id)
    {
        ReadString(reader);
        return util.ReadLString(reader, fileName, lineName, id);
    }
    public static byte[] GetBuffer(string resource)
    {
        return util.GetBuffer(resource);
    }
    public static void Warning(string str)
    {
        util.Warning(str);
    }
    public static void Error(string str)
    {
        util.Error(str);
    }

    private const Byte INVALID_BYTE = Byte.MaxValue;
    private const Int16 INVALID_INT16 = Int16.MaxValue;
    private const UInt16 INVALID_UINT16 = UInt16.MaxValue;
    private const Int32 INVALID_INT32 = Int32.MaxValue;
    private const UInt32 INVALID_UINT32 = UInt32.MaxValue;
    private const Int64 INVALID_INT64 = Int64.MaxValue;
    private const UInt64 INVALID_UINT64 = UInt64.MaxValue;
    private const float INVALID_FLOAT = -1.0f;
    private const double INVALID_DOUBLE = -1.0;

    public static bool IsInvalidByte(Byte val)
    {
        return (val == INVALID_BYTE);
    }
    public static bool IsInvalidInt16(Int16 val)
    {
        return (val == INVALID_INT16);
    }
    public static bool IsInvalidUint16(UInt16 val)
    {
        return (val == INVALID_UINT16);
    }
    public static bool IsInvalidInt32(Int32 val)
    {
        return (val == INVALID_INT32);
    }
    public static bool IsInvalidUint32(UInt32 val)
    {
        return (val == INVALID_UINT32);
    }
    public static bool IsInvalidInt64(Int64 val)
    {
        return (val == INVALID_INT64);
    }
    public static bool IsInvalidUint64(UInt64 val)
    {
        return (val == INVALID_UINT64);
    }
    public static bool IsInvalidFloat(float val)
    {
        return (Math.Abs(INVALID_FLOAT - val) < 0.001f);
    }
    public static bool IsInvalidDouble(double val)
    {
        return (Math.Abs(INVALID_DOUBLE - val) < 0.001f);
    }
    public static bool IsInvalidString(string val)
    {
        return string.IsNullOrEmpty(val); ;
    }
    public static bool IsInvalid(object val)
    {
        if (val == null) return false;
        if (val.GetType() == typeof(byte))
            return TABLE.IsInvalidByte((byte)val);
        else if (val.GetType() == typeof(Int16))
            return TABLE.IsInvalidInt16((Int16)val);
        else if (val.GetType() == typeof(UInt16))
            return TABLE.IsInvalidUint16((UInt16)val);
        else if (val.GetType() == typeof(Int32))
            return TABLE.IsInvalidInt32((Int32)val);
        else if (val.GetType() == typeof(UInt32))
            return TABLE.IsInvalidUint32((UInt32)val);
        else if (val.GetType() == typeof(Int64))
            return TABLE.IsInvalidInt64((Int64)val);
        else if (val.GetType() == typeof(UInt64))
            return TABLE.IsInvalidUint64((UInt64)val);
        else if (val.GetType() == typeof(float))
            return TABLE.IsInvalidFloat((float)val);
        else if (val.GetType() == typeof(double))
            return TABLE.IsInvalidDouble((double)val);
        else if (val.GetType() == typeof(string))
            return TABLE.IsInvalidString((string)val);
        if (!val.GetType().IsSubclassOf(typeof(MT_DataBase)))
        {
            Error(""错误的判断类型 "" + val.GetType().FullName);
            return false;
        }
        MT_DataBase data = (MT_DataBase)val;
        return data.IsInvalid();
    }
}
";
        strUtilBase = strUtilBase.Replace("__ClassValue", ((int)ElementType.CLASS).ToString());
        FileUtil.CreateFile("TableUtil.cs", strUtilBase, true, Util.GetProgramInfo(program).CodeDirectory.Split(';'));
    }
    public void CreateBaseJAVA()
    {
        PROGRAM program = PROGRAM.JAVA;
        string strDataBase = @"package table;
public abstract class MT_DataBase {
	public abstract boolean IsInvalid();
	public abstract Object GetDataByString(String str) throws Exception;
}
";
        FileUtil.CreateFile("MT_DataBase.java", strDataBase, false, Util.GetProgramInfo(program).CodeDirectory.Split(';'));

        string strTableBase = @"package table;
public abstract class MT_TableBase {
    public MT_DataBase GetValue(Integer key) { return null; }
    public MT_DataBase GetValue(String key) { return null; }
    public Boolean Contains(Integer ID) { return false; }
    public Boolean Contains(String ID) { return false; }
    public abstract Integer Count();
}
";
        FileUtil.CreateFile("MT_TableBase.java", strTableBase, false, Util.GetProgramInfo(program).CodeDirectory.Split(';'));

        string strUtilBase = @"package table;
import java.nio.ByteBuffer;
public class TableUtil
{
    public interface ITableUtil
    {
        String ReadString(ByteBuffer reader);
        String ReadFString(ByteBuffer reader);
        String ReadLString(ByteBuffer reader, String fileName, String lineName, int id);
        String ReadLString(ByteBuffer reader, String fileName, String lineName, String id);
        byte[] GetBuffer(String resource);
        void Warning(String str);
        void Error(String str, Exception e);
    }
    public static final int CLASS_VALUE = __ClassValue;
    private static ITableUtil util = null;
    public static void SetUtil(ITableUtil iutil) {
        util = iutil;
    }
    public static String ReadString(ByteBuffer reader)
    {
        return util.ReadString(reader);
    }
    public static String ReadFString(ByteBuffer reader)
    {
        return util.ReadFString(reader);
    }
    public static String ReadLString(ByteBuffer reader, String fileName, String lineName, int id)
    {
        ReadString(reader);
        return util.ReadLString(reader, fileName, lineName, id);
    }
    public static String ReadLString(ByteBuffer reader, String fileName, String lineName, String id)
    {
        ReadString(reader);
        return util.ReadLString(reader, fileName, lineName, id);
    }
    public static byte[] GetBuffer(String resource)
    {
        return util.GetBuffer(resource);
    }
    public static void Warning(String str)
    {
        util.Warning(str);
    }
    public static void Error(String str, Exception e)
    {
        util.Error(str, e);
    }

    private static final Byte INVALID_BYTE = Byte.MAX_VALUE;
    private static final Short INVALID_INT16 = Short.MAX_VALUE;
    private static final Integer INVALID_INT32 = Integer.MAX_VALUE;
    private static final Long INVALID_INT64 = Long.MAX_VALUE;
    private static final Float INVALID_FLOAT = -1.0f;
    private static final Double INVALID_DOUBLE = -1.0;

    public static boolean IsInvalid(Boolean val)
    {
    	return (val == false);
    }
    public static boolean IsInvalid(Byte val)
    {
        return (val.equals(INVALID_BYTE));
    }
    public static boolean IsInvalid(Short val)
    {
        return (val.equals(INVALID_INT16));
    }
    public static boolean IsInvalid(Integer val)
    {
        return (val.equals(INVALID_INT32));
    }
    public static boolean IsInvalid(Long val)
    {
        return (val.equals(INVALID_INT64));
    }
    public static boolean IsInvalid(Float val)
    {
        return (Math.abs(INVALID_FLOAT - val) < 0.001f);
    }
    public static boolean IsInvalid(Double val)
    {
        return (Math.abs(INVALID_DOUBLE - val) < 0.001f);
    }
    public static boolean IsInvalid(String val)
    {
        return (val == null || val.isEmpty());
    }
    public static <T extends MT_DataBase> boolean IsInvalid(T val)
    {
        if (val == null) return false;
        return val.IsInvalid();
    }
}
";
        strUtilBase = strUtilBase.Replace("__ClassValue", ((int)ElementType.CLASS).ToString());
        FileUtil.CreateFile("TableUtil.java", strUtilBase, false, Util.GetProgramInfo(program).CodeDirectory.Split(';'));
    }
    public void CreateBasePHP()
    {

    }
}

