//本文件为自动生成，请不要手动修改
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Scorpio.Conversion.Runtime;

namespace Datas {
public partial class DataSpawn : IData {
    
    /* <summary> 测试ID 此值必须唯一 而且必须为int型  默认值() </summary> */
    public int ID { get; private set; }
    /* <summary> int类型  默认值() </summary> */
    public int TestInt { get; private set; }
    /* <summary> string类型  默认值() </summary> */
    public string TestString { get; private set; }
    /* <summary> 测试多国语言  默认值() </summary> */
    public string TestLanguage { get; private set; }
    /* <summary> bool类型  默认值() </summary> */
    public bool TestBool { get; private set; }
    /* <summary> 自定义类型 根据ExcelConfig下 table.sco文件定义的Int2解析 类型为table_后面的名字 格式为 , 隔开  默认值() </summary> */
    public Int2 TestInt2 { get; private set; }
    /* <summary> 自定义枚举  默认值() </summary> */
    public TestEnum TestEnumName { get; private set; }
    
    public DataSpawn(string fileName, IReader reader) {
        this.ID = reader.ReadInt32();
        this.TestInt = reader.ReadInt32();
        this.TestString = reader.ReadString();
        this.TestLanguage = reader.ReadString();
        this.TestBool = reader.ReadBool();
        this.TestInt2 = new Int2(fileName, reader);
        this.TestEnumName = (TestEnum)reader.ReadInt32();
    }
    
    public object GetData(string key) {
        if ("ID".Equals(key)) return ID;
        if ("TestInt".Equals(key)) return TestInt;
        if ("TestString".Equals(key)) return TestString;
        if ("TestLanguage".Equals(key)) return TestLanguage;
        if ("TestBool".Equals(key)) return TestBool;
        if ("TestInt2".Equals(key)) return TestInt2;
        if ("TestEnumName".Equals(key)) return TestEnumName;
        return null;
    }
    
    public void Set(DataSpawn value) {
        this.ID = value.ID;
        this.TestInt = value.TestInt;
        this.TestString = value.TestString;
        this.TestLanguage = value.TestLanguage;
        this.TestBool = value.TestBool;
        this.TestInt2 = value.TestInt2;
        this.TestEnumName = value.TestEnumName;
    }
    
    public override string ToString() {
        return $"ID:{ID}, TestInt:{TestInt}, TestString:{TestString}, TestLanguage:{TestLanguage}, TestBool:{TestBool}, TestInt2:{TestInt2}, TestEnumName:{TestEnumName}, ";
    }
}
}
