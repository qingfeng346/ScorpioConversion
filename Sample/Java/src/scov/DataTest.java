
package scov;

import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import ScorpioProto.Commons.*;
import ScorpioProto.Table.*;
@SuppressWarnings("unused")

public class DataTest implements IData {
    private boolean m_IsInvalid;
    
    private Integer _TestID;
    /**   默认值() */
    public Integer getTestID() { return _TestID; }
    public Integer ID() { return _TestID; }
    private TestEnum _testEnum;
    /**   默认值() */
    public TestEnum gettestEnum() { return _testEnum; }
    private List<Int3> _TestDate;
    /**   默认值() */
    public List<Int3> getTestDate() { return _TestDate; }
    
    public Object GetData(String key ) {
        if ("TestID".equals(key)) return _TestID;
        if ("testEnum".equals(key)) return _testEnum;
        if ("TestDate".equals(key)) return _TestDate;
        return null;
    }
    
    public boolean IsInvalid() { return m_IsInvalid; }
    private boolean CheckInvalid() {
        if (!TableUtil.IsInvalid(this._TestID)) return false;
        if (!TableUtil.IsInvalid(this._testEnum)) return false;
        if (!TableUtil.IsInvalid(this._TestDate)) return false;
        return true;
    }
    
    public static DataTest Read(String fileName, IScorpioReader reader) {
        DataTest ret = new DataTest();
        ret._TestID = reader.ReadInt32();
        ret._testEnum = TestEnum.valueOf(reader.ReadInt32());
        {
            ArrayList<Int3> list = new ArrayList<Int3>();
            int number = reader.ReadInt32();
            for (int i = 0; i < number; ++i) { list.add(Int3.Read(fileName, reader)); }
            ret._TestDate = Collections.unmodifiableList(list);
        }
        ret.m_IsInvalid = ret.CheckInvalid();
        return ret;
    }
    
    public void Set(DataTest value) {
        this._TestID = value._TestID;
        this._testEnum = value._testEnum;
        this._TestDate = value._TestDate;
    }
    
    @Override
    public String toString() {
        return "{ " + 
            "TestID : " +  _TestID + "," + 
            "testEnum : " +  _testEnum + "," + 
            "TestDate : " +  ScorpioUtil.ToString(_TestDate) + 
            " }";
    }
}