using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Scorpio.Commons;
using Scorpio.Table;
namespace ScorpioProtoTest {
public class DataTest : IData {
    private bool m_IsInvalid;
    private int _ID;
    /* <summary> 测试ID 此值必须唯一 而且必须为int型  默认值() </summary> */
    public int getID() { return _ID; }
    public int ID() { return _ID; }
    private int _TestInt;
    /* <summary> int类型
测试注释回车  默认值(20) </summary> */
    public int getTestInt() { return _TestInt; }
    private string _TestString;
    /* <summary> string类型  默认值(aaa) </summary> */
    public string getTestString() { return _TestString; }
    private bool _TestBool;
    /* <summary> bool类型  默认值() </summary> */
    public bool getTestBool() { return _TestBool; }
    private Int2 _TestInt2;
    /* <summary> 自定义类型 根据ExcelConfig下 table.sco文件定义的Int2解析 类型为table_后面的名字 格式为 , 隔开  默认值() </summary> */
    public Int2 getTestInt2() { return _TestInt2; }
    private TestEnum _TestEnumName;
    /* <summary> 自定义枚举  默认值() </summary> */
    public TestEnum getTestEnumName() { return _TestEnumName; }
    private ReadOnlyCollection<int> _TestArray;
    /* <summary> array类型 以逗号隔开  默认值() </summary> */
    public ReadOnlyCollection<int> getTestArray() { return _TestArray; }
    private ReadOnlyCollection<Int2> _TestArray2;
    /* <summary> array类型 自定义类型 每一个中括号为一个单位  默认值() </summary> */
    public ReadOnlyCollection<Int2> getTestArray2() { return _TestArray2; }
    private Int3 _TestInt3;
    /* <summary> 嵌套类型  默认值() </summary> */
    public Int3 getTestInt3() { return _TestInt3; }
    public object GetData(string key ) {
        if (key == "ID") return _ID;
        if (key == "TestInt") return _TestInt;
        if (key == "TestString") return _TestString;
        if (key == "TestBool") return _TestBool;
        if (key == "TestInt2") return _TestInt2;
        if (key == "TestEnumName") return _TestEnumName;
        if (key == "TestArray") return _TestArray;
        if (key == "TestArray2") return _TestArray2;
        if (key == "TestInt3") return _TestInt3;
        return null;
    }
    public bool IsInvalid() { return m_IsInvalid; }
    private bool IsInvalid_impl() {
        if (!TableUtil.IsInvalid(this._ID)) return false;
        if (!TableUtil.IsInvalid(this._TestInt)) return false;
        if (!TableUtil.IsInvalid(this._TestString)) return false;
        if (!TableUtil.IsInvalid(this._TestBool)) return false;
        if (!TableUtil.IsInvalid(this._TestInt2)) return false;
        if (!TableUtil.IsInvalid(this._TestEnumName)) return false;
        if (!TableUtil.IsInvalid(this._TestArray)) return false;
        if (!TableUtil.IsInvalid(this._TestArray2)) return false;
        if (!TableUtil.IsInvalid(this._TestInt3)) return false;
        return true;
    }
    public static DataTest Read(ScorpioReader reader) {
        DataTest ret = new DataTest();
        ret._ID = reader.ReadInt32();
        ret._TestInt = reader.ReadInt32();
        ret._TestString = reader.ReadString();
        ret._TestBool = reader.ReadBool();
        ret._TestInt2 = Int2.Read(reader);
        ret._TestEnumName = (TestEnum)reader.ReadInt32();
        {
            int number = reader.ReadInt32();
            List<int> list = new List<int> ();
            for (int i = 0;i < number; ++i) { list.Add(reader.ReadInt32()); }
            ret._TestArray = list.AsReadOnly();
        }
        {
            int number = reader.ReadInt32();
            List<Int2> list = new List<Int2> ();
            for (int i = 0;i < number; ++i) { list.Add(Int2.Read(reader)); }
            ret._TestArray2 = list.AsReadOnly();
        }
        ret._TestInt3 = Int3.Read(reader);
        ret.m_IsInvalid = ret.IsInvalid_impl();
        return ret;
    }
}
}