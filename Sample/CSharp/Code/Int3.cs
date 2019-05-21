using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ScorpioProto.Commons;
using ScorpioProto.Table;

namespace scov {
public partial class Int3 : IData {
    private bool m_IsInvalid;
    
    private ReadOnlyCollection<Int2> _Value1;
    /* <summary>   默认值() </summary> */
    public ReadOnlyCollection<Int2> getValue1() { return _Value1; }
    public ReadOnlyCollection<Int2> ID() { return _Value1; }
    private int _Value2;
    /* <summary>   默认值() </summary> */
    public int getValue2() { return _Value2; }
    
    public object GetData(string key) {
        if ("Value1".Equals(key)) return _Value1;
        if ("Value2".Equals(key)) return _Value2;
        return null;
    }
    
    public bool IsInvalid() { return m_IsInvalid; }
    private bool CheckInvalid() {
        if (!TableUtil.IsInvalid(this._Value1)) return false;
        if (!TableUtil.IsInvalid(this._Value2)) return false;
        return true;
    }
    
    public static Int3 Read(string fileName, IScorpioReader reader) {
        var ret = new Int3();
        {
            List<Int2> list = new List<Int2>();
            int number = reader.ReadInt32();
            for (int i = 0; i < number; ++i) { list.Add(Int2.Read(fileName, reader)); }
            ret._Value1 = list.AsReadOnly();
        }
        ret._Value2 = reader.ReadInt32();
        ret.m_IsInvalid = ret.CheckInvalid();
        return ret;
    }
    
    public void Set(Int3 value) {
        this._Value1 = value._Value1;
        this._Value2 = value._Value2;
    }
    
    public override string ToString() {
        return "{ " + 
            "Value1 : " +  ScorpioUtil.ToString(_Value1) + "," + 
            "Value2 : " +  _Value2 + 
            " }";
    }
}
}