using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using Scorpio;


public static partial class Util
{
    public static string CurrentDirectory { get { return AppDomain.CurrentDomain.BaseDirectory; } }

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
    public static void ParseStructure(string dir, ref Dictionary<string, List<PackageField>> customClass, ref Dictionary<string, List<PackageEnum>> customEnum)
    {
        customClass.Clear();
        customEnum.Clear();
        dir = Path.Combine(CurrentDirectory, dir);
        Script script = new Script();
        script.LoadLibrary();
        List<ScriptObject> GlobalBasic = new List<ScriptObject>();
        {
            var itor = script.GetGlobalTable().GetIterator();
            while (itor.MoveNext()) GlobalBasic.Add(itor.Current.Value);
        }
        string[] files = System.IO.Directory.GetFiles(dir, "*.js", SearchOption.AllDirectories);
        foreach (var file in files) {
            script.LoadFile(file);
        }
        {
            var itor = script.GetGlobalTable().GetIterator();
            while (itor.MoveNext()) {
                if (GlobalBasic.Contains(itor.Current.Value)) continue;
                string name = itor.Current.Key as string;
                ScriptTable table = itor.Current.Value as ScriptTable;
                if (name == null || table == null) continue;
                if (name.StartsWith("enum_")) {
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
                        enums.Sort((m1, m2) => { return m1.Index.CompareTo(m2.Index); });
                        customEnum[name] = enums;
                    }
                } else {
                    List<PackageField> fields = new List<PackageField>();
                    var tItor = table.GetIterator();
                    while (tItor.MoveNext()) {
                        string fieldName = tItor.Current.Key as string;
                        ScriptString val = tItor.Current.Value as ScriptString;
                        if (string.IsNullOrEmpty(fieldName) || val == null) throw new Exception(string.Format("Message:{0} Field:{1} 参数出错 参数模版 \"索引,类型,是否数组=false,注释\"", name, fieldName));
                        string[] infos = val.Value.Split(',');
                        if (infos.Length < 2) throw new Exception(string.Format("Message:{0} Field:{1} 参数出错 参数模版 \"索引,类型,是否数组=false,注释\"", name, fieldName));
                        bool array = infos.Length > 2 && infos[2] == "true";
                        string note = infos.Length > 3 ? infos[3] : "";
                        fields.Add(new PackageField() {
                            Index = int.Parse(infos[0]),
                            Type = infos[1],
                            Name = fieldName,
                            Array = array,
                            Note = note,
                        });
                    }
                    fields.Sort((m1, m2) => { return m1.Index.CompareTo(m2.Index); });
                    customClass[name] = fields;
                }
            }
        }
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
        List<byte> sb = new List<byte>();
        byte ch;
        while ((ch = reader.ReadByte()) != 0)
            sb.Add(ch);
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
}