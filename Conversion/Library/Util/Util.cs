using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using Scorpio;
using Scorpio.Variable;
using Scorpio.Compiler;
public class IValue { }
public class ValueString : IValue {
    public string value;
    public ValueString(string value) {
        this.value = value;
    }
}
public class ValueList : IValue {
    public List<IValue> values = new List<IValue>();
}
public static partial class Util
{
    private const double KB_LENGTH = 1024;              //1KB 的字节数
    private const double MB_LENGTH = 1048576;           //1MB 的字节数
    private const double GB_LENGTH = 1073741824;		//1GB 的字节数

    public static string CurrentDirectory { get { return AppDomain.CurrentDomain.BaseDirectory; } }


    public static Script Script;
    public const string EmptyString         = "####";
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
    private const string TABLE_KEYWORD      = "table_";         //Table自定义类关键字
    private const string DATABASE_CLASS_KEYWORD = "class_";     //数据库自定义类关键字

    private const string ENUM_KEYWORD       = "enum_";          //枚举类型关键字
    private const string CONST_KEYWORD      = "const_";         //常量类型关键字
    private const string DATABASE_KEYWORD   = "database_";      //数据库格式关键字
    public static string GetMemory(long by) {
        if (by < MB_LENGTH)
            return string.Format("{0:f2} KB", Convert.ToDouble(by) / KB_LENGTH);
        else if (by < GB_LENGTH)
            return string.Format("{0:f2} MB", Convert.ToDouble(by) / MB_LENGTH);
        else
            return string.Format("{0:f2} MB", Convert.ToDouble(by) / GB_LENGTH);
    }
    public static void ParseStructure(string dir, 
        Dictionary<string, List<PackageField>> customClass, 
        Dictionary<string, List<PackageEnum>> customEnum,
        Dictionary<string, List<PackageField>> customTable, 
        Dictionary<string, Dictionary<string, DatabaseTable>> customDatabase,
        Dictionary<string, List<PackageField>> customDatabaseClass,
        Dictionary<string, List<PackageConst>> customConst)
    {
        if (customClass != null) customClass.Clear();
        if (customEnum != null) customEnum.Clear();
        if (customTable != null) customTable.Clear();
        if (customDatabase != null) customDatabase.Clear();
        if (customDatabaseClass != null) customDatabaseClass.Clear();
        if (customConst != null) customConst.Clear();
        dir = Path.Combine(CurrentDirectory, dir);
        Script = new Script();
        Script.LoadLibrary();
        List<ScriptObject> GlobalBasic = new List<ScriptObject>(); {
            var itor = Script.GetGlobalTable().GetIterator();
            while (itor.MoveNext()) GlobalBasic.Add(itor.Current.Value);
        }
        string[] files = System.IO.Directory.Exists(dir) ? System.IO.Directory.GetFiles(dir, "*.sco", SearchOption.AllDirectories) :  new string[0];
        foreach (var file in files) { Script.LoadFile(file); } {
            var itor = Script.GetGlobalTable().GetIterator();
            while (itor.MoveNext()) {
                if (GlobalBasic.Contains(itor.Current.Value)) continue;
                string name = itor.Current.Key as string;
                ScriptTable table = itor.Current.Value as ScriptTable;
                if (name == null || table == null) continue;
                if (name.StartsWith(ENUM_KEYWORD)) {
                    if (customEnum != null)                     {
                        List<PackageEnum> enums = new List<PackageEnum>();
                        var tItor = table.GetIterator();
                        while (tItor.MoveNext()) {
                            string fieldName = tItor.Current.Key as string;
                            ScriptNumber val = tItor.Current.Value as ScriptNumber;
                            if (string.IsNullOrEmpty(fieldName) || val == null) throw new Exception(string.Format("Enum:{0} Field:{1} 参数出错", name, fieldName));
                            enums.Add(new PackageEnum() {
                                Index = Convert.ToInt32(val.ObjectValue),
                                Name = fieldName,
                            });
                        }
                        enums.Sort((m1, m2) => { return m1.Index.CompareTo(m2.Index); });
                        customEnum[name.Substring(ENUM_KEYWORD.Length)] = enums;
                    }
                } else if (name.StartsWith(CONST_KEYWORD)) {
                    if (customConst != null) {
                        List<PackageConst> consts = new List<PackageConst>();
                        var tItor = table.GetIterator();
                        while (tItor.MoveNext()) {
                            string fieldName = tItor.Current.Key as string;
                            if (string.IsNullOrEmpty(fieldName)) throw new Exception(string.Format("Const:{0} Field:{1} 参数出错", name, fieldName));
                            PackageConst pack = new PackageConst();
                            pack.Name = fieldName;
                            ScriptObject value = tItor.Current.Value;
                            if (value is ScriptNumberDouble) {
                                pack.Type = BasicEnum.INT32;
                                pack.Value = ((ScriptNumberDouble)value).ToInt32().ToString();
                            } else if (value is ScriptNumberLong) {
                                pack.Type = BasicEnum.INT64;
                                pack.Value = ((ScriptNumberLong)value).ToLong().ToString() + "L";
                            } else if (value is ScriptString) {
                                pack.Type = BasicEnum.STRING;
                                pack.Value = "\"" + value.ToString() + "\"";
                            } else {
                                throw new Exception("不支持此常量类型 " + value.Type);
                            }
                            consts.Add(pack);
                        }
                        customConst[name.Substring(CONST_KEYWORD.Length)] = consts;
                    }
                } else if (name.StartsWith(DATABASE_KEYWORD)) {
                    if (customDatabase != null) {
                        Dictionary<string, DatabaseTable> tables = new Dictionary<string, DatabaseTable>();
                        var tItor = table.GetIterator();
                        while (tItor.MoveNext()) {
                            string tableName = tItor.Current.Key as string;
                            if (tableName.StartsWith(DATABASE_KEYWORD)) continue;
                            tables.Add(tableName, JsonUtil.JsonToObject<DatabaseTable>(tItor.Current.Value.ToJson()));
                        }
                        customDatabase[name.Substring(ENUM_KEYWORD.Length)] = tables;
                    }
                } else {
                    List<PackageField> fields = new List<PackageField>();
                    var tItor = table.GetIterator();
                    while (tItor.MoveNext()) {
                        string fieldName = tItor.Current.Key as string;
                        ScriptString val = tItor.Current.Value as ScriptString;
                        if (string.IsNullOrEmpty(fieldName) || val == null) throw new Exception(string.Format("Class:{0} Field:{1} 参数出错 参数模版 \"索引,类型,是否数组=false,注释\"", name, fieldName));
                        string[] infos = val.Value.Split(',');
                        if (infos.Length < 2) throw new Exception(string.Format("Class:{0} Field:{1} 参数出错 参数模版 \"索引,类型,是否数组=false,注释\"", name, fieldName));
                        bool array = infos.Length > 2 && infos[2] == "true";
                        string note = infos.Length > 3 ? infos[3] : "";
                        var packageField = new PackageField() {
                            Index = int.Parse(infos[0]),
                            Type = infos[1],
                            Name = fieldName,
                            Array = array,
                            Comment = note,
                        };
                        if (!packageField.IsBasic) {
                            if (Script.HasValue(ENUM_KEYWORD + packageField.Type)) {
                                packageField.Enum = true;
                            } else if ( !Script.HasValue(packageField.Type) &&                              //判断网络协议自定义类
                                        !Script.HasValue(TABLE_KEYWORD + packageField.Type) &&              //判断Table内嵌类
                                        !Script.HasValue(DATABASE_CLASS_KEYWORD + packageField.Type)        //判断数据库内嵌类
                                       ) {
                                throw new Exception(string.Format("Class:{0} Field:{1} 未知类型:{2}", name, fieldName, packageField.Type));
                            }
                        }
                        fields.Add(packageField);
                    }
                    fields.Sort((m1, m2) => { return m1.Index.CompareTo(m2.Index); });
                    if (name.StartsWith(DATABASE_CLASS_KEYWORD)) {
                        if (customDatabaseClass != null)
                            customDatabaseClass[name.Substring(DATABASE_CLASS_KEYWORD.Length)] = fields;
                    } else if (name.StartsWith(TABLE_KEYWORD)) {
                        if (customTable != null)
                            customTable[name.Substring(TABLE_KEYWORD.Length)] = fields;
                    } else {
                        if (customClass != null)
                            customClass[name] = fields;
                    }
                }
            }
        }
    }
    //是不是非法字符串 ####
    public static bool IsEmptyString(string str)
    {
        return str == EmptyString || string.IsNullOrEmpty(str);
    }
    //是否是非法值
    public static bool IsEmptyValue(ValueList value)
    {
        if (value.values.Count == 1) {
            ValueString val = value.values[0] as ValueString;
            if (val != null && IsEmptyString(val.value))
                return true;
        }
        return false;
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
    //字符串转bool
    public static bool ToBoolean(string str, bool def)
    {
        if (string.IsNullOrEmpty(str)) return def;
        switch (str.ToLower()) {
            case "1":
            case "true":
                return true;
            case "0":
            case "false":
                return false;
            default:
                throw new Exception("字符串不能转换为bool " + str);
        }
    }
    //读取Value
    public static IValue ReadValue(string value)
    {
        value = "[" + value + "]";
        return new ValueParser(null, new ScriptLexer(value, "").GetTokens(), "").GetObject();
    }
    //获得一个单元格的字符串
    public static string GetCellString(ICell cell)
    {
        if (cell == null) return "";
        cell.SetCellType(CellType.String);
        return cell.StringCellValue;
    }
}