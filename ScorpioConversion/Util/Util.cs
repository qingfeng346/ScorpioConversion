using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;


public static partial class Util
{
    public const string ReturnString        = "\r\n";
    public const string EmptyString         = "####";
    private const string TabString          = "\t";

    /// <summary> 数组关键字 </summary>
    public const string ArrayString         = "array";

    public const bool INVALID_BOOL          = false;
    public const sbyte INVALID_INT8         = sbyte.MaxValue;
    public const short INVALID_INT16        = Int16.MaxValue;
    public const int INVALID_INT32          = Int32.MaxValue;
    public const long INVALID_INT64         = Int64.MaxValue;
    public const float INVALID_FLOAT        = -1.0f;
    public const double INVALID_DOUBLE      = -1.0;
    public const string INVALID_STRING      = "";
    //获得几个数量的tab
    public static string GetTab(int num)
    {
        string str = "";
        for (int i = 0; i < num; ++i)
            str += TabString;
        return str;
    }
    //根据字符串获得基本类型 (不包含 class )
    private static BasicType GetType(string key)
    {
        foreach (var info in BasicTypes)
        {
            if (info.ScorpioName == key)
                return info;
        }
        return null;
    }
    //是不是非法字符串 ####
    public static bool IsEmptyString(string str)
    {
        return str == EmptyString || string.IsNullOrEmpty(str);
    }
    //写入一个 字符串
    public static void WriteString(BinaryWriter writer, string str)
    {
        if (Util.IsEmptyString(str)) {
            writer.Write((byte)0);
        } else {
            writer.Write(Encoding.UTF8.GetBytes(str));
            writer.Write((byte)0);
        }
    }
    //读取一个 字符串
    public static string ReadString(BinaryReader reader)
    {
        Stack<byte> sb = new Stack<byte>();
        byte ch;
        while ((ch = reader.ReadByte()) != 0)
            sb.Push(ch);
        return Encoding.UTF8.GetString(sb.ToArray());
    }
    //根据数字 获得 AA Excel列名字
    public static string GetLineName(int line)
    {
        --line;
        StringBuilder stringBuilder = new StringBuilder();
        if (line < 26)
        {
            stringBuilder.Append((char)('A' + line));
        }
        else if (line < 27 * 26)
        {
            stringBuilder.Append((char)('A' + line / 26 - 1));
            stringBuilder.Append((char)('A' + line % 26));
        }
        return stringBuilder.ToString();
    }
    //读取一个单元格的内容
    public static string ReadCellString(ICell cell)
    {
        if (cell == null) return "";
        cell.SetCellType(CellType.String);
        return cell.StringCellValue;
    }
    //获得反转自定义类结构
    public static string GetRollbackClassData(Dictionary<string, List<int>> typeList)
    {
//        StringBuilder builder = new StringBuilder();
//        foreach (KeyValuePair<string, List<int>> pair in typeList)
//        {
//            string str = @"
//public class __ClassName {";
//            str = str.Replace("__ClassName", pair.Key);
//            List<int> fields = pair.Value;
//            for (int i = 0; i < fields.Count;++i )
//            {
//                str += @"
//    public __FieldType __FieldName;";
//                Element element = GetElement((BasicEnum)fields[i]);
//                str = str.Replace("__FieldType", element.GetVariable(PROGRAM.CS));
//                str = str.Replace("__FieldName", "field" + (i+1));
//            }
//            str += @"
//}";
//            builder.Append(str);
//        }
//        return builder.ToString();
        return "";
    }
}