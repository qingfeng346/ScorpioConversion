using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ScorpioProto.Commons;
using ScorpioProto.Table;

namespace scov {
public partial class DataTest : IData {
    private bool m_IsInvalid;
    
    private int _TestID;
    /* <summary>   默认值() </summary> */
    public int getTestID() { return _TestID; }
    public int ID() { return _TestID; }
    private TestEnum _testEnum;
    /* <summary>   默认值() </summary> */
    public TestEnum gettestEnum() { return _testEnum; }
    private ReadOnlyCollection<Int3> _TestDate;
    /* <summary>   默认值() </summary> */
    public ReadOnlyCollection<Int3> getTestDate() { return _TestDate; }
    
    public object GetData(string key) {
        if ("TestID".Equals(key)) return _TestID;
        if ("testEnum".Equals(key)) return _testEnum;
        if ("TestDate".Equals(key)) return _TestDate;
        return null;
    }
    
    public bool IsInvalid() { return m_IsInvalid; }
    private bool CheckInvalid() {
        if (!TableUtil.IsInvalid(this._TestID)) return false;
        if (!TableUtil.IsInvalid(this._testEnum)) return false;
        if (!TableUtil.IsInvalid(this._TestDate)) return false;
        return true;
    }
    
    public static DataTest Read(string fileName, IScorpioReader reader) {
        var ret = new DataTest();
        ret._TestID = reader.ReadInt32();
        ret._testEnum = (TestEnum)reader.ReadInt32();
        {
            List<Int3> list = new List<Int3>();
            int number = reader.ReadInt32();
            for (int i = 0; i < number; ++i) { list.Add(Int3.Read(fileName, reader)); }
            ret._TestDate = list.AsReadOnly();
        }
        ret.m_IsInvalid = ret.CheckInvalid();
        return ret;
    }
    
    public void Set(DataTest value) {
        this._TestID = value._TestID;
        this._testEnum = value._testEnum;
        this._TestDate = value._TestDate;
    }
    
    public override string ToString() {
        return "{ " + 
            "TestID : " +  _TestID + "," + 
            "testEnum : " +  _testEnum + "," + 
            "TestDate : " +  ScorpioUtil.ToString(_TestDate) + 
            " }";
    }
}
}