using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;

//变量类型
public enum ElementType
{
    NONE = 0,           //无类型
    BOOL,               //bool类型
    INT8,               //int8类型
    INT16,              //int16类型
    INT32,              //int32类型
    INT64,              //int64类型
    FLOAT,              //float类型
    DOUBLE,             //double类型
    STRING,             //string类型
    LSTRING,            //lstring类型
    CLASS,              //class类型（自定义类）
    FSTRING,            //stringXXX类型(格式化字符串类型 字符串需要处理后赋值)
    CUSTOMLAYOUT,       //自定义格式
}
public static partial class Util
{
    public static readonly Element BOOL_ELEMENT = GetElement(ElementType.BOOL);
    public static readonly Element BYTE_ELEMENT = GetElement(ElementType.INT8);
    public static readonly Element INT_ELEMENT = GetElement(ElementType.INT32);
    public static readonly Element STRING_ELEMENT = GetElement(ElementType.STRING);
    

    private static readonly Type TYPE_BOOL = typeof(bool);
    private static readonly Type TYPE_SBYTE = typeof(sbyte);
    private static readonly Type TYPE_BYTE = typeof(byte);
    private static readonly Type TYPE_SHORT = typeof(short);
    private static readonly Type TYPE_USHORT = typeof(ushort);
    private static readonly Type TYPE_INT = typeof(int);
    private static readonly Type TYPE_UINT = typeof(uint);
    private static readonly Type TYPE_LONG = typeof(long);
    private static readonly Type TYPE_ULONG = typeof(ulong);
    private static readonly Type TYPE_FLOAT = typeof(float);
    private static readonly Type TYPE_DOUBLE = typeof(double);
    private static readonly Type TYPE_STRING = typeof(string);


    public const string ReturnString        = "\r\n";
    public const string EmptyString         = "####";
    private const string TabString          = "\t";

    private const string BoolString         = "bool";
    private const string BooleanString      = "boolean";

    private const string SByteString        = "sbyte";
    private const string ShortString        = "short";
    private const string IntString          = "int";
    private const string LongString         = "long";

    private const string ByteString         = "byte";
    private const string UShortString       = "ushort";
    private const string UIntString         = "uint";
    private const string ULongString        = "ulong";

    private const string Int8String         = "int8";
    private const string Int16String        = "int16";
    private const string Int32String        = "int32";
    private const string Int64String        = "int64";

    private const string UInt8String        = "uint8";
    private const string UInt16String       = "uint16";
    private const string UInt32String       = "uint32";
    private const string UInt64String       = "uint64";

    private const string FloatString        = "float";
    private const string SingleString       = "single";
    private const string DoubleString       = "double";
    private const string StringString       = "string";
    /// <summary> lstring关键字 多语言关键字 写入data文件的时候会自动把 此字符串写成[""]节省文件大小 真正的字符串会通过 TableUtil.ReadLString 函数获得 </summary>
    private const string LStringString      = "lstring";
    /// <summary> 数组关键字 </summary>
    public const string ArrayString         = "array";

    public const bool INVALID_BOOL          = false;
    public const sbyte INVALID_INT8         = sbyte.MaxValue;
    public const short INVALID_INT16        = short.MaxValue;
    public const int INVALID_INT32          = int.MaxValue;
    public const long INVALID_INT64         = long.MaxValue;
    public const float INVALID_FLOAT        = -1.0f;
    public const double INVALID_DOUBLE      = -1.0;
    public const string INVALID_STRING      = "";

    //各种类型数组
    private static Dictionary<ElementType, Type> mArrayElement;
    private static bool bInitialize = false;
    //初始化数据
    private static void Initialize()
    {
        if (bInitialize == false) {
            mArrayElement = new Dictionary<ElementType, Type>();
            mArrayElement.Add(ElementType.BOOL, typeof(BoolElement));
            mArrayElement.Add(ElementType.INT8, typeof(Int8Element));
            mArrayElement.Add(ElementType.INT16, typeof(Int16Element));
            mArrayElement.Add(ElementType.INT32, typeof(Int32Element));
            mArrayElement.Add(ElementType.INT64, typeof(Int64Element));
            mArrayElement.Add(ElementType.FLOAT, typeof(FloatElement));
            mArrayElement.Add(ElementType.DOUBLE, typeof(DoubleElement));
            mArrayElement.Add(ElementType.STRING, typeof(StringElement));
            mArrayElement.Add(ElementType.LSTRING, typeof(LStringElement));
            mArrayElement.Add(ElementType.CLASS, typeof(ClassElement));

            mArrayElement.Add(ElementType.FSTRING, typeof(FStringElement));
            bInitialize = true;
        }
    }
    //获得几个数量的tab
    public static string GetTab(int num)
    {
        string str = "";
        for (int i = 0; i < num; ++i)
            str += TabString;
        return str;
    }
    //根据字符串获得基本类型 (不包含 class )
    private static ElementType GetVariableIndex(string str)
    {
        str = str.ToLower();
        switch (str)
        {
            case BoolString:
            case BooleanString:
                return ElementType.BOOL;
            case Int8String:
            case SByteString:
            case UInt8String:
            case ByteString:
                return ElementType.INT8;
            case Int16String:
            case ShortString:
            case UInt16String:
            case UShortString:
                return ElementType.INT16;
            case Int32String:
            case IntString:
            case UInt32String:
            case UIntString:
                return ElementType.INT32;
            case Int64String:
            case LongString:
            case UInt64String:
            case ULongString:
                return ElementType.INT64;
            case FloatString:
            case SingleString:
                return ElementType.FLOAT;
            case DoubleString:
                return ElementType.DOUBLE;
            case StringString:
                return ElementType.STRING;
            case LStringString:
                return ElementType.LSTRING;
        }
        return ElementType.NONE;
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
    //读取一个单元格的内容
    public static string ReadCellString(ICell cell)
    {
        if (cell == null) return "";
        cell.SetCellType(CellType.String);
        return cell.StringCellValue;
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
    //根据枚举获得类型
    public static Element GetElement(ElementType index)
    {
        Initialize();
        if (mArrayElement.ContainsKey(index))
            return (Element)System.Activator.CreateInstance(mArrayElement[index]);
        return null;
    }
    //根据字符串获得类型
    public static Element GetElement(string str)
    {
        Element element = GetElement(GetVariableIndex(str));
        if (element != null)
            return element;
        Type type = CodeProvider.GetInstance().GetType(str);
        if (type == null)
            throw new Exception(string.Format("class 【{0}】 is not exist", str));
        ClassElement classElement = GetElement(ElementType.CLASS) as ClassElement;
        classElement.Initialize(type);
        return classElement;
    }
    //获得原始的数据 不包含特殊数据 例如 LString FString Class Json
    public static Element GetPrimitiveElement(Type type)
    {
        Initialize();
        ElementType index = ElementType.NONE;
        if (type == TYPE_BOOL)
            index = ElementType.BOOL;
        else if (type == TYPE_BYTE || type == TYPE_SBYTE)
            index = ElementType.INT8;
        else if (type == TYPE_SHORT || type == TYPE_USHORT)
            index = ElementType.INT16;
        else if (type == TYPE_INT || type == TYPE_UINT)
            index = ElementType.INT32;
        else if (type == TYPE_LONG || type == TYPE_ULONG)
            index = ElementType.INT64;
        else if (type == TYPE_FLOAT)
            index = ElementType.FLOAT;
        else if (type == TYPE_DOUBLE)
            index = ElementType.DOUBLE;
        else if (type == TYPE_STRING)
            index = ElementType.STRING;
        if (index == ElementType.NONE)
            throw new System.Exception("没有找到合适的类型 " + type.Name);
        return GetElement(index);
    }
    //根据类型 获得 所有子类型
    public static Type[] GetElementTypes(Type type)
    {
        List<Type> types = new List<Type>();
        FieldInfo[] fieldInfos = type.GetFields();
        foreach (FieldInfo fieldInfo in fieldInfos) {
            Element element = GetElement(fieldInfo.FieldType.Name);
            if (element is ClassElement) {
                types.AddRange(GetElementTypes((element as ClassElement).GetClassType()));
            } else {
                types.Add(fieldInfo.FieldType);
            }
        }
        return types.ToArray();
    }
    //获得类的MD5 验证结构
    public static string GetDataMD5Code(List<Variable> variables)
    {
        string strMD5 = "";
        strMD5 += variables.Count;
        foreach (Variable var in variables)
            strMD5 += var.strFieldType;
        return FileUtil.GetMD5FromString(strMD5);
    }
    //获得反转自定义类结构
    public static string GetRollbackClassData(Dictionary<string, List<int>> typeList)
    {
        StringBuilder builder = new StringBuilder();
        foreach (KeyValuePair<string, List<int>> pair in typeList)
        {
            string str = @"
public class __ClassName {";
            str = str.Replace("__ClassName", pair.Key);
            List<int> fields = pair.Value;
            for (int i = 0; i < fields.Count;++i )
            {
                str += @"
    public __FieldType __FieldName;";
                Element element = GetElement((ElementType)fields[i]);
                str = str.Replace("__FieldType", element.GetVariable(PROGRAM.CS));
                str = str.Replace("__FieldName", "field" + (i+1));
            }
            str += @"
}";
            builder.Append(str);
        }
        return builder.ToString();
    }
}