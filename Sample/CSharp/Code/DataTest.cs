//本文件为自动生成，请不要手动修改
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ScorpioProto.Commons;
using ScorpioProto.Table;

namespace Datas {
public partial class DataTest : IData {
    
    private int _TestID;
    /* <summary> 注释  默认值() </summary> */
    public int getTestID() { return _TestID; }
    public int ID() { return _TestID; }
    private TestEnum _testEnum;
    /* <summary>   默认值() </summary> */
    public TestEnum gettestEnum() { return _testEnum; }
    private ReadOnlyCollection<Int3> _TestDate;
    /* <summary>   默认值() </summary> */
    public ReadOnlyCollection<Int3> getTestDate() { return _TestDate; }
    private DateTime _TestDateTime;
    /* <summary>   默认值() </summary> */
    public DateTime getTestDateTime() { return _TestDateTime; }
    private int _TestInt;
    /* <summary>   默认值(999) </summary> */
    public int getTestInt() { return _TestInt; }
    
    public object GetData(string key) {
        if ("TestID".Equals(key)) return _TestID;
        if ("testEnum".Equals(key)) return _testEnum;
        if ("TestDate".Equals(key)) return _TestDate;
        if ("TestDateTime".Equals(key)) return _TestDateTime;
        if ("TestInt".Equals(key)) return _TestInt;
        return null;
    }
    
    public static DataTest Read(string fileName, IScorpioReader reader) {
        var ret = new DataTest();
        ret._TestID = reader.ReadInt32();
        ret._testEnum = (TestEnum)reader.ReadInt32();
        {
            var list = new List<Int3>();
            var number = reader.ReadInt32();
            for (int i = 0; i < number; ++i) { list.Add(Int3.Read(fileName, reader)); }
            ret._TestDate = list.AsReadOnly();
        }
        ret._TestDateTime = reader.ReadDateTime();
        ret._TestInt = reader.ReadInt32();
        return ret;
    }
    
    public void Set(DataTest value) {
        this._TestID = value._TestID;
        this._testEnum = value._testEnum;
        this._TestDate = value._TestDate;
        this._TestDateTime = value._TestDateTime;
        this._TestInt = value._TestInt;
    }
    
    public override string ToString() {
        return $"TestID:{_TestID}, testEnum:{_testEnum}, TestDate:{_TestDate}, TestDateTime:{_TestDateTime}, TestInt:{_TestInt}, ";
    }
}
}
