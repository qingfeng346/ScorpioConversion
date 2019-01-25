using System;
using System.Collections.Generic;
using System.Reflection;
using Scorpio.Commons;

public class LanguageInfo : Attribute {
    public string extension;
    public bool bom;
    public LanguageInfo(string extension, bool bom) {
        this.extension = extension;
        this.bom = bom;
    }
}
//支持的语言列表
public enum Language {
    [LanguageInfo("sco", false)]
    Scorpio,
    [LanguageInfo("cs", false)]
    CSharp,
    [LanguageInfo("java", false)]
    Java,
}
//基础类型列表
public enum BasicEnum {
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
public class BasicType {
    public string Key { get; private set; }                                 //类型Key
    public string Name { get; private set; }                                //类型名字
    public BasicEnum Index { get; private set; }                            //类型索引
    public Dictionary<Language, string> Languages { get; private set; }     //各语言名字
    private MethodInfo WriteMethod;     //Write函数
    private MethodInfo ReadMethod;      //Read函数
    public BasicType(string name, BasicEnum index, Dictionary<Language, string> languages) {
        this.Key = name;
        this.Name = name;
        this.Index = index;
        this.Languages = languages ?? new Dictionary<Language, string>();
        this.WriteMethod = typeof(TableWriter).GetMethod("Write" + Util.ToOneUpper(name), new Type[] { typeof(string) });
        this.ReadMethod = typeof(TableReader).GetMethod("Read" + Util.ToOneUpper(name));
    }
    public BasicType SetKey(string key) {
        this.Key = key;
        return this;
    }
    public string GetLanguageType(Language language) {
        return Languages.ContainsKey(language) ? Languages[language] : Name;
    }
    public void WriteValue(TableWriter writer, string value) {
        WriteMethod.Invoke(writer, new object[] { value });
    }
    public object ReadValue(TableReader reader) {
        return ReadMethod.Invoke(reader, null);
    }
}
class BasicUtil {
    private static List<BasicType> BasicTypes = new List<BasicType>() {
        new BasicType("bool", BasicEnum.BOOL, new Dictionary<Language, string>() { { Language.Java, "Boolean" } }),
        new BasicType("Int8", BasicEnum.INT8, new Dictionary<Language, string>() { { Language.Java, "Byte" } }),
        new BasicType("Int16", BasicEnum.INT16, new Dictionary<Language, string>() { { Language.Java, "Short" } }),
        new BasicType("Int32", BasicEnum.INT32, new Dictionary<Language, string>() { { Language.Java, "Integer" } }),
        new BasicType("Int64", BasicEnum.INT64, new Dictionary<Language, string>() { { Language.Java, "Long" } }),
        new BasicType("float", BasicEnum.FLOAT, new Dictionary<Language, string>() { { Language.Java, "Float" } }),
        new BasicType("double", BasicEnum.DOUBLE, new Dictionary<Language, string>() { { Language.Java, "Double" } }),
        new BasicType("String", BasicEnum.STRING, new Dictionary<Language, string>() { }),
        new BasicType("byte[]", BasicEnum.BYTES, new Dictionary<Language, string>() { }),
    };
    static BasicUtil() {
        BasicTypes.Add(GetType(BasicEnum.BOOL).SetKey("Boolean"));
        BasicTypes.Add(GetType(BasicEnum.INT8).SetKey("byte"));
        BasicTypes.Add(GetType(BasicEnum.INT16).SetKey("short"));
        BasicTypes.Add(GetType(BasicEnum.INT32).SetKey("int"));
        BasicTypes.Add(GetType(BasicEnum.INT64).SetKey("long"));
        BasicTypes.Add(GetType(BasicEnum.BYTES).SetKey("bytes"));
    }
    public static bool HasType(string key) {
        return BasicTypes.Exists(_ => _.Key.ToLower() == key.ToLower());
    }
    public static BasicType GetType(string key) {
        return BasicTypes.Find(_ => _.Key.ToLower() == key.ToLower());
    }
    public static BasicType GetType(BasicEnum index) {
        return BasicTypes.Find(_ => _.Index == index);
    }
}
