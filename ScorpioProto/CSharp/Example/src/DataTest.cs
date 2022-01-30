//本文件为自动生成，请不要手动修改
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Scorpio.Conversion.Runtime;

namespace Datas {
public partial class DataTest : IData {
    
    public int ID => TestID;
    /* <summary> 注释  默认值() </summary> */
    public int TestID { get; private set; }
    /* <summary>   默认值() </summary> */
    public TestEnum testEnum { get; private set; }
    /* <summary>   默认值() </summary> */
    public ReadOnlyCollection<Int3> TestDate { get; private set; }
    /* <summary>   默认值() </summary> */
    public DateTime TestDateTime { get; private set; }
    /* <summary>   默认值(999) </summary> */
    public int TestInt { get; private set; }
    
    public DataTest(string fileName, IReader reader) {
        this.TestID = reader.ReadInt32();
        this.testEnum = (TestEnum)reader.ReadInt32();
        {
            var list = new List<Int3>();
            var number = reader.ReadInt32();
            for (int i = 0; i < number; ++i) { list.Add(new Int3(fileName, reader)); }
            this.TestDate = list.AsReadOnly();
        }
        this.TestDateTime = reader.ReadDateTime();
        this.TestInt = reader.ReadInt32();
    }
    
    public object GetData(string key) {
        if ("TestID".Equals(key)) return TestID;
        if ("testEnum".Equals(key)) return testEnum;
        if ("TestDate".Equals(key)) return TestDate;
        if ("TestDateTime".Equals(key)) return TestDateTime;
        if ("TestInt".Equals(key)) return TestInt;
        return null;
    }
    
    public void Set(DataTest value) {
        this.TestID = value.TestID;
        this.testEnum = value.testEnum;
        this.TestDate = value.TestDate;
        this.TestDateTime = value.TestDateTime;
        this.TestInt = value.TestInt;
    }
    
    public override string ToString() {
        return $"TestID:{TestID}, testEnum:{testEnum}, TestDate:{TestDate}, TestDateTime:{TestDateTime}, TestInt:{TestInt}, ";
    }
}
}
