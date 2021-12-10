package Datas;
//本文件为自动生成，请不要手动修改
import java.util.*;
import ScorpioProto.Commons.*;
import ScorpioProto.Table.*;

public class DataTest implements IData {
    
    private Integer _TestID;
    /** 注释  默认值() */
    public Integer getTestID() { return _TestID; }
    public Integer ID() { return _TestID; }
    private TestEnum _testEnum;
    /**   默认值() */
    public TestEnum gettestEnum() { return _testEnum; }
    private List<Int3> _TestDate;
    /**   默认值() */
    public List<Int3> getTestDate() { return _TestDate; }
    private Calendar _TestDateTime;
    /**   默认值() */
    public Calendar getTestDateTime() { return _TestDateTime; }
    private Integer _TestInt;
    /**   默认值(999) */
    public Integer getTestInt() { return _TestInt; }
    
    public Object GetData(String key) {
        if ("TestID".equals(key)) return _TestID;
        if ("testEnum".equals(key)) return _testEnum;
        if ("TestDate".equals(key)) return _TestDate;
        if ("TestDateTime".equals(key)) return _TestDateTime;
        if ("TestInt".equals(key)) return _TestInt;
        return null;
    }
    
    public static DataTest Read(String fileName, IScorpioReader reader) {
        DataTest ret = new DataTest();
        ret._TestID = reader.ReadInt32();
        ret._testEnum = TestEnum.valueOf(reader.ReadInt32());
        {
            List<Int3> list = new ArrayList<Int3>();
            int number = reader.ReadInt32();
            for (int i = 0; i < number; ++i) { list.add(Int3.Read(fileName, reader)); }
            ret._TestDate = Collections.unmodifiableList(list);
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
    
    @Override
    public String toString() {
        return "TestID:" + _TestID + "," + "testEnum:" + _testEnum + "," + "TestDate:" + _TestDate + "," + "TestDateTime:" + _TestDateTime + "," + "TestInt:" + _TestInt;
    }
}
