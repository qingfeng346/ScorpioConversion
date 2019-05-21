
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

public class DataSpawn implements IData {
    private boolean m_IsInvalid;
    
    private Integer _ID;
    /** 测试ID 此值必须唯一 而且必须为int型  默认值() */
    public Integer getID() { return _ID; }
    public Integer ID() { return _ID; }
    private Integer _TestInt;
    /** int类型  默认值() */
    public Integer getTestInt() { return _TestInt; }
    private String _TestString;
    /** string类型  默认值() */
    public String getTestString() { return _TestString; }
    private String _TestLanguage;
    /** 测试多国语言  默认值() */
    public String getTestLanguage() { return _TestLanguage; }
    private boolean _TestBool;
    /** bool类型  默认值() */
    public boolean getTestBool() { return _TestBool; }
    private Int2 _TestInt2;
    /** 自定义类型 根据ExcelConfig下 table.sco文件定义的Int2解析 类型为table_后面的名字 格式为 , 隔开  默认值() */
    public Int2 getTestInt2() { return _TestInt2; }
    private TestEnum _TestEnumName;
    /** 自定义枚举  默认值() */
    public TestEnum getTestEnumName() { return _TestEnumName; }
    
    public Object GetData(String key ) {
        if ("ID".equals(key)) return _ID;
        if ("TestInt".equals(key)) return _TestInt;
        if ("TestString".equals(key)) return _TestString;
        if ("TestLanguage".equals(key)) return _TestLanguage;
        if ("TestBool".equals(key)) return _TestBool;
        if ("TestInt2".equals(key)) return _TestInt2;
        if ("TestEnumName".equals(key)) return _TestEnumName;
        return null;
    }
    
    public boolean IsInvalid() { return m_IsInvalid; }
    private boolean CheckInvalid() {
        if (!TableUtil.IsInvalid(this._ID)) return false;
        if (!TableUtil.IsInvalid(this._TestInt)) return false;
        if (!TableUtil.IsInvalid(this._TestString)) return false;
        if (!TableUtil.IsInvalid(this._TestLanguage)) return false;
        if (!TableUtil.IsInvalid(this._TestBool)) return false;
        if (!TableUtil.IsInvalid(this._TestInt2)) return false;
        if (!TableUtil.IsInvalid(this._TestEnumName)) return false;
        return true;
    }
    
    public static DataSpawn Read(String fileName, IScorpioReader reader) {
        DataSpawn ret = new DataSpawn();
        ret._ID = reader.ReadInt32();
        ret._TestInt = reader.ReadInt32();
        ret._TestString = reader.ReadString();
        ret._TestLanguage = reader.ReadString();
        ret._TestBool = reader.ReadBool();
        ret._TestInt2 = Int2.Read(fileName, reader);
        ret._TestEnumName = TestEnum.valueOf(reader.ReadInt32());
        ret.m_IsInvalid = ret.CheckInvalid();
        return ret;
    }
    
    public void Set(DataSpawn value) {
        this._ID = value._ID;
        this._TestInt = value._TestInt;
        this._TestString = value._TestString;
        this._TestLanguage = value._TestLanguage;
        this._TestBool = value._TestBool;
        this._TestInt2 = value._TestInt2;
        this._TestEnumName = value._TestEnumName;
    }
    
    @Override
    public String toString() {
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