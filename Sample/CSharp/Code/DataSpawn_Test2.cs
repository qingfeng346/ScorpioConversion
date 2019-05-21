using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ScorpioProto.Commons;
using ScorpioProto.Table;

namespace scov {
public partial class DataSpawn_Test2 : IData {
    private bool m_IsInvalid;
    
    private int _ID;
    /* <summary> 测试ID 此值必须唯一 而且必须为int型  默认值() </summary> */
    public int getID() { return _ID; }
    public int ID() { return _ID; }
    private int _TestInt;
    /* <summary> int类型  默认值() </summary> */
    public int getTestInt() { return _TestInt; }
    private string _TestString;
    /* <summary> string类型  默认值() </summary> */
    public string getTestString() { return _TestString; }
    private string _TestLanguage;
    /* <summary> 测试多国语言  默认值() </summary> */
    public string getTestLanguage() { return _TestLanguage; }
    private bool _TestBool;
    /* <summary> bool类型  默认值() </summary> */
    public bool getTestBool() { return _TestBool; }
    private Int2 _TestInt2;
    /* <summary> 自定义类型 根据ExcelConfig下 table.sco文件定义的Int2解析 类型为table_后面的名字 格式为 , 隔开  默认值() </summary> */
    public Int2 getTestInt2() { return _TestInt2; }
    private TestEnum _TestEnumName;
    /* <summary> 自定义枚举  默认值() </summary> */
    public TestEnum getTestEnumName() { return _TestEnumName; }
    
    public object GetData(string key) {
        if ("ID".Equals(key)) return _ID;
        if ("TestInt".Equals(key)) return _TestInt;
        if ("TestString".Equals(key)) return _TestString;
        if ("TestLanguage".Equals(key)) return _TestLanguage;
        if ("TestBool".Equals(key)) return _TestBool;
        if ("TestInt2".Equals(key)) return _TestInt2;
        if ("TestEnumName".Equals(key)) return _TestEnumName;
        return null;
    }
    
    public bool IsInvalid() { return m_IsInvalid; }
    private bool CheckInvalid() {
        if (!TableUtil.IsInvalid(this._ID)) return false;
        if (!TableUtil.IsInvalid(this._TestInt)) return false;
        if (!TableUtil.IsInvalid(this._TestString)) return false;
        if (!TableUtil.IsInvalid(this._TestLanguage)) return false;
        if (!TableUtil.IsInvalid(this._TestBool)) return false;
        if (!TableUtil.IsInvalid(this._TestInt2)) return false;
        if (!TableUtil.IsInvalid(this._TestEnumName)) return false;
        return true;
    }
    
    public static DataSpawn_Test2 Read(string fileName, IScorpioReader reader) {
        var ret = new DataSpawn_Test2();
        ret._ID = reader.ReadInt32();
        ret._TestInt = reader.ReadInt32();
        ret._TestString = reader.ReadString();
        ret._TestLanguage = reader.ReadString();
        ret._TestBool = reader.ReadBool();
        ret._TestInt2 = Int2.Read(fileName, reader);
        ret._TestEnumName = (TestEnum)reader.ReadInt32();
        ret.m_IsInvalid = ret.CheckInvalid();
        return ret;
    }
    
    public void Set(DataSpawn_Test2 value) {
        this._ID = value._ID;
        this._TestInt = value._TestInt;
        this._TestString = value._TestString;
        this._TestLanguage = value._TestLanguage;
        this._TestBool = value._TestBool;
        this._TestInt2 = value._TestInt2;
        this._TestEnumName = value._TestEnumName;
    }
    
    public override string ToString() {
        return "{ " + 
            "ID : " +  _ID + "," + 
            "TestInt : " +  _TestInt + "," + 
            "TestString : " +  _TestString + "," + 
            "TestLanguage : " +  _TestLanguage + "," + 
            "TestBool : " +  _TestBool + "," + 
            "TestInt2 : " +  _TestInt2 + "," + 
            "TestEnumName : " +  _TestEnumName + 
            " }";
    }
}
}