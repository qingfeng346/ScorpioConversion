using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
//变量类型
public enum BasicEnum
{
    NONE = -1,      //无类型
    BOOL,           //bool类型
    INT8,           //int8类型
    INT16,          //int16类型
    INT32,          //int32类型
    INT64,          //int64类型
    FLOAT,          //float类型
    DOUBLE,         //double类型
    STRING,         //string类型
    BYTES,          //byte[]类型
}
//基本类型
public class BasicType
{
    public string ScorpioName;          //脚本名称
    public BasicEnum BasicIndex;        //类型索引
    public string WriteFunction;        //Write函数
    public string ReadFunction;         //Readh函数
    private List<string> Codes;         //程序变量
    private MethodInfo WriteMethod;     //Write函数
    private MethodInfo ReadMethod;      //Read函数
    public BasicType(string con, BasicEnum ben, string write, string read, string[] codes)
    {
        this.ScorpioName = con;
        this.BasicIndex = ben;
        this.WriteFunction = write;
        this.ReadFunction = read;
        this.Codes = new List<string>(codes);
        this.WriteMethod = typeof(TableWriter).GetMethod(WriteFunction, new Type[] { typeof(string) });
        this.ReadMethod = typeof(TableReader).GetMethod(ReadFunction);
    }
    public string GetCode(PROGRAM program)
    {
        int index = (int)program;
        return Codes.Count > index ? Codes[index] : "";
    }
    public void WriteValue(TableWriter writer, string value)
    {
        WriteMethod.Invoke(writer, new object[] { value });
    }
    public object ReadValue(TableReader reader)
    {
        return ReadMethod.Invoke(reader, null);
    }
}
//单个变量
public class PackageField
{
    public int Index;           //字段索引
    public string Comment;      //字段注释
    public string Name;         //字段名字
    public Scorpio.ScriptTable Attribute;    //字段属性 字段配置
    public string Default;      //字段默认值
    public string Type;         //字段类型
    public bool Enum = false;   //是否是枚举
    public bool Array = false;  //是否是数组
    public bool IsBasic {       //是否是基本数据
        get { return BasicUtil.HasType(Type); }
    }
    public BasicType Info {     //返回基本数据信息
        get { return BasicUtil.GetType(Type); }
    }
}
//单个枚举
public class PackageEnum
{
    public int Index;       //枚举值
    public string Name;     //枚举类型
}
//常量值
public class PackageConst
{
    public string Name;     //常量名字
    public BasicEnum Type;  //常量类型 目前只有 int32 int64 string
    public string Value;    //常量值
}
//单个数据库的配置
public class PackageDatabase {
    public Dictionary<string, DatabaseTable> tables = new Dictionary<string, DatabaseTable>();      //所有table列表
}
public static class BasicUtil
{
    private static readonly List<BasicType> BasicTypes = new List<BasicType>()
    {
        new BasicType( "bool", BasicEnum.BOOL, "WriteBool", "ReadBool", 
            new string[] {"bool", "Boolean", "", "bool"}) ,

        new BasicType( "int8", BasicEnum.INT8, "WriteInt8", "ReadInt8", 
            new string[] {"sbyte", "Byte", "", "__int8"}) ,

        new BasicType( "int16", BasicEnum.INT16, "WriteInt16", "ReadInt16", 
            new string[] {"short", "Short", "", "__int16"}) ,

        new BasicType( "int32", BasicEnum.INT32, "WriteInt32", "ReadInt32", 
            new string[] {"int", "Integer", "", "__int32"}) ,

        new BasicType( "int64", BasicEnum.INT64, "WriteInt64", "ReadInt64", 
            new string[] {"long", "Long", "", "__int64"}) ,

        new BasicType( "float", BasicEnum.FLOAT, "WriteFloat", "ReadFloat", 
            new string[] {"float", "Float", "", "float"}) ,

        new BasicType( "double", BasicEnum.DOUBLE, "WriteDouble", "ReadDouble", 
            new string[] {"double", "Double", "", "double"}) ,

        new BasicType( "string", BasicEnum.STRING, "WriteString", "ReadString", 
            new string[] {"string", "String", "", "char *"}) ,

        new BasicType( "bytes", BasicEnum.BYTES, "WriteBytes", "ReadBytes", 
            new string[] {"byte[]", "byte[]", "", "char * "}) ,

        new BasicType( "int", BasicEnum.INT32, "WriteInt32", "ReadInt32", 
            new string[] {"int", "Integer", "", "__int32"}) ,
    };
    public static bool HasType(string type)
    {
        foreach (var info in BasicTypes) {
            if (info.ScorpioName == type)
                return true;
        }
        return false;
    }
    public static BasicType GetType(string type)
    {
        foreach (var info in BasicTypes) {
            if (info.ScorpioName == type)
                return info;
        }
        return null;
    }
    public static bool HasType(BasicEnum type)
    {
        foreach (var info in BasicTypes) {
            if (info.BasicIndex == type)
                return true;
        }
        return false;
    }
    public static BasicType GetType(BasicEnum type)
    {
        foreach (var info in BasicTypes) {
            if (info.BasicIndex == type)
                return info;
        }
        return null;
    }
}

