using System;
using System.Collections.Generic;
using System.Text;

//一个数据库数据
public class DatabaseTable
{
    public string key;                  //主键
    public string comment;              //表注释
    public List<DatabaseField> fields;  //表字段数组
}
//一个表字段的数据
public class DatabaseField
{
    public string name;                 //名字
    public string type;                 //类型
    public long auto_increment = -1;    //自动增长值
    public string @default;             //默认值
    public string @class;               //转为指定类
    public bool array;                  //是否为数组
    public string comment;              //注释
}
public static class DatabaseUtil
{
    private static readonly Dictionary<string, string> DatabaseTypes = new Dictionary<string, string>() {
        {"char", "String"},
        {"text", "String"},
        {"float", "Float"},
        {"double", "Double"},
        {"decimal", "Double"},
        {"bigint", "Long"},
        {"bit", "Integer"},
        {"int", "Integer"},
        {"timestamp", "Timestamp"},
        {"datetime", "Timestamp"},
        {"date", "Date"},
        {"blob", "byte[]"},
    };
    public static bool IsClass(string @class)
    {
        return Util.Script.HasValue("class_" + @class);
    }
    /// <summary> 根据数据库类型 转换成java类型 </summary>
    public static String GetDatabaseType(DatabaseField info)
    {
        string type = info.type.ToLower();
        foreach (var pair in DatabaseTypes) {
            if (type.Contains(pair.Key))
                return pair.Value;
        }
        return "Invalid";
    }
    /// <summary> 获得最终字段的类型 </summary>
    public static String GetFinishType(DatabaseField info, bool checkArray)
    {
        String className = info.@class;
        if (!string.IsNullOrEmpty(className))
            return info.array && checkArray ? string.Format("List<{0}>", className) : className;
        return GetDatabaseType(info);
    }
    public static String AssignOperate(bool array, string @class)
    {
        return AssignOperate(array, @class, "this", "value");
    }
    public static String AssignOperate(bool array, string @class, string source, string target)
    {
        string str = "";
        if (array)
            str = "__source.__FieldName = com.commons.util.Utility.cloneList(__target.__FieldName)";
        else
            str = IsClass(@class) ? "__source.__FieldName = new __FieldType(__target.__FieldName)" : "__source.__FieldName = __target.__FieldName";
        str = str.Replace("__source", source);
        str = str.Replace("__target", target);
        return str;
    }
}

