using System;
using System.Collections.Generic;
using System.Reflection;
using Scorpio.Commons;

//基础类型列表
public enum BasicEnum {
    BOOL,           //bool类型
    INT8,           //int8类型
    UINT8,          //uint8类型
    INT16,          //int16类型
    UINT16,         //uint16类型
    INT32,          //int32类型
    UINT32,         //uint32类型
    INT64,          //int64类型
    UINT64,         //uint64类型
    FLOAT,          //float类型
    DOUBLE,         //double类型
    STRING,         //string类型
    DATETIME,       //datetime日期时间
    BYTES,          //byte[]类型
}
//基本类型
public class BasicType {
    public string Key { get; private set; }                                 //类型Key
    public string Name { get; private set; }                                //类型名字
    public BasicEnum Index { get; private set; }                            //类型索引
    private MethodInfo WriteMethod;     //Write函数
    private MethodInfo ReadMethod;      //Read函数
    private BasicType(BasicType basicType, string key) {
        this.Key = key;
        this.Name = basicType.Name;
        this.Index = basicType.Index;
        this.WriteMethod = basicType.WriteMethod;
        this.ReadMethod = basicType.ReadMethod;
    }
    public BasicType(string name, BasicEnum index) {
        this.Key = name;
        this.Name = name;
        this.Index = index;
        this.WriteMethod = typeof(TableWriter).GetMethod("Write" + ScorpioUtil.ToOneUpper(name), new Type[] { typeof(string) });
        this.ReadMethod = typeof(TableReader).GetMethod("Read" + ScorpioUtil.ToOneUpper(name));
    }
    public BasicType SetKey(string key) {
        this.Key = key;
        return this;
    }
    public void WriteValue(TableWriter writer, string value) {
        try {
            WriteMethod.Invoke(writer, new object[] { value });
        } catch (Exception e) {
            throw new Exception($"写入数据失败,Value:{value} : {e}");
        }
    }
    public object ReadValue(TableReader reader) {
        return ReadMethod.Invoke(reader, null);
    }
    public BasicType Clone(string key) {
        return new BasicType(this, key);
    }
}
class BasicUtil {
    private readonly static DateTime BaseTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

    private static List<BasicType> BasicTypes = new List<BasicType>() {
        new BasicType("Bool", BasicEnum.BOOL),
        new BasicType("Int8", BasicEnum.INT8),
        new BasicType("UInt8", BasicEnum.UINT8),
        new BasicType("Int16", BasicEnum.INT16),
        new BasicType("UInt16", BasicEnum.UINT16),
        new BasicType("Int32", BasicEnum.INT32),
        new BasicType("UInt32", BasicEnum.UINT32),
        new BasicType("Int64", BasicEnum.INT64),
        new BasicType("UInt64", BasicEnum.UINT64),
        new BasicType("Float", BasicEnum.FLOAT),
        new BasicType("Double", BasicEnum.DOUBLE),
        new BasicType("String", BasicEnum.STRING),
        new BasicType("DateTime", BasicEnum.DATETIME),
        new BasicType("Bytes", BasicEnum.BYTES),
    };
    static BasicUtil() {
        BasicTypes.Add(GetType(BasicEnum.BOOL).Clone("bool"));
        BasicTypes.Add(GetType(BasicEnum.BOOL).Clone("boolean"));
        
        BasicTypes.Add(GetType(BasicEnum.INT8).Clone("sbyte"));
        BasicTypes.Add(GetType(BasicEnum.INT16).Clone("short"));
        BasicTypes.Add(GetType(BasicEnum.INT32).Clone("int"));
        BasicTypes.Add(GetType(BasicEnum.INT64).Clone("long"));

        BasicTypes.Add(GetType(BasicEnum.UINT8).Clone("byte"));
        BasicTypes.Add(GetType(BasicEnum.UINT16).Clone("ushort"));
        BasicTypes.Add(GetType(BasicEnum.UINT32).Clone("uint"));
        BasicTypes.Add(GetType(BasicEnum.UINT64).Clone("ulong"));

        BasicTypes.Add(GetType(BasicEnum.FLOAT).Clone("float32"));
        BasicTypes.Add(GetType(BasicEnum.DOUBLE).Clone("float64"));

        BasicTypes.Add(GetType(BasicEnum.BYTES).Clone("bytes"));
    }
    public static BasicType GetType(string key) {
        return BasicTypes.Find(_ => _.Key.ToLower() == key.ToLower());
    }
    public static BasicType GetType(BasicEnum index) {
        return BasicTypes.Find(_ => _.Index == index);
    }
    public static long GetTimeSpan(DateTime time) {
        if (time.Kind == DateTimeKind.Local) {
            time = TimeZoneInfo.ConvertTime(time, TimeZoneInfo.Utc);
        }
        return Convert.ToInt64((time - BaseTime).TotalMilliseconds);
    }
}
