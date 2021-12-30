//本文件为自动生成，请不要手动修改
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Scorpio.Conversion;

namespace Datas {
public partial class Int2 : IData {
    
    private int _Value1;
    /* <summary>   默认值() </summary> */
    public int getValue1() { return _Value1; }
    private int _Value2;
    /* <summary>   默认值() </summary> */
    public int getValue2() { return _Value2; }
    
    public object GetData(string key) {
        if ("Value1".Equals(key)) return _Value1;
        if ("Value2".Equals(key)) return _Value2;
        return null;
    }
    
    public static Int2 Read(string fileName, IReader reader) {
        var ret = new Int2();
        ret._Value1 = reader.ReadInt32();
        ret._Value2 = reader.ReadInt32();
        return ret;
    }
    
    public void Set(Int2 value) {
        this._Value1 = value._Value1;
        this._Value2 = value._Value2;
    }
    
    public override string ToString() {
        return $"Value1:{_Value1}, Value2:{_Value2}, ";
    }
}
}
