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
    public int Index;           //变量索引
    public string Name;         //变量名字
    public string Type;         //类型
    public bool Array;          //是否是数组
    public string Note;         //注释
    public bool IsBasic {       //是否是基本数据
        get { return BasicUtil.HasType(Type); }
    }
    public BasicType Info {     //返回基本数据信息
        get { return BasicUtil.GetType(Type); }
    }
}
public static class BasicUtil
{
    private static readonly List<BasicType> BasicTypes = new List<BasicType>()
    {
        new BasicType( "bool", BasicEnum.BOOL, "WriteBool", "ReadBool", new string[] {"bool", "Boolean"}) ,
        new BasicType( "int8", BasicEnum.INT8, "WriteInt8", "ReadInt8", new string[] {"sbyte", "Byte"}) ,
        new BasicType( "int16", BasicEnum.INT16, "WriteInt16", "ReadInt16", new string[] {"short", "Short"}) ,
        new BasicType( "int32", BasicEnum.INT32, "WriteInt32", "ReadInt32", new string[] {"int", "Integer"}) ,
        new BasicType( "int64", BasicEnum.INT64, "WriteInt64", "ReadInt64", new string[] {"long", "Long"}) ,
        new BasicType( "float", BasicEnum.FLOAT, "WriteFloat", "ReadFloat", new string[] {"float", "Float"}) ,
        new BasicType( "double", BasicEnum.DOUBLE, "WriteDouble", "ReadDouble", new string[] {"double", "Double"}) ,
        new BasicType( "string", BasicEnum.STRING, "WriteString", "ReadString", new string[] {"string", "String"}) ,
        new BasicType( "bytes", BasicEnum.BYTES, "WriteBytes", "ReadBytes", new string[] {"byte[]", "Byte[]"}) ,
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

