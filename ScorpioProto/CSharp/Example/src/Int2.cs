//本文件为自动生成，请不要手动修改
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Scorpio.Conversion;

namespace Datas {
public partial class Int2 : IData {
    
    /* <summary>   默认值() </summary> */
    public int Value1 { get; private set; }
    /* <summary>   默认值() </summary> */
    public int Value2 { get; private set; }
    
    public Int2(string fileName, IReader reader) {
        this.Value1 = reader.ReadInt32();
        this.Value2 = reader.ReadInt32();
    }
    
    public object GetData(string key) {
        if ("Value1".Equals(key)) return Value1;
        if ("Value2".Equals(key)) return Value2;
        return null;
    }
    
    public void Set(Int2 value) {
        this.Value1 = value.Value1;
        this.Value2 = value.Value2;
    }
    
    public override string ToString() {
        return $"Value1:{Value1}, Value2:{Value2}, ";
    }
}
}
