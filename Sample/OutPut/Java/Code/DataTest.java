package scorpiogame.proto;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import Scorpio.Commons.*;
import Scorpio.Table.*;
@SuppressWarnings("unused")
public class DataTest implements IData {
    private boolean m_IsInvalid = false;
    private Integer _ID;
    /** 测试ID 此值必须唯一 而且必须为int型() */
    public Integer getID() { return _ID; }
    public Integer ID() { return _ID; }
    private Integer _TestInt;
    /** int类型() */
    public Integer getTestInt() { return _TestInt; }
    private String _TestString;
    /** string类型() */
    public String getTestString() { return _TestString; }
    private Boolean _TestBool;
    /** bool类型() */
    public Boolean getTestBool() { return _TestBool; }
    private Int2 _TestInt2;
    /** 自定义类型 根据ExcelConfig下 table.sco文件定义的Int2解析 类型为table_后面的名字 格式为 , 隔开() */
    public Int2 getTestInt2() { return _TestInt2; }
    private TestEnum _TestEnumName;
    /** 自定义枚举() */
    public TestEnum getTestEnumName() { return _TestEnumName; }
    public Object GetData(String key ) {
        if (key.equals("ID")) return _ID;
        if (key.equals("TestInt")) return _TestInt;
        if (key.equals("TestString")) return _TestString;
        if (key.equals("TestBool")) return _TestBool;
        if (key.equals("TestInt2")) return _TestInt2;
        if (key.equals("TestEnumName")) return _TestEnumName;
        return null;
    }
    public boolean IsInvalid() { return m_IsInvalid; }
    private boolean IsInvalid_impl() {
        if (!TableUtil.IsInvalid(this._ID)) return false;
        if (!TableUtil.IsInvalid(this._TestInt)) return false;
        if (!TableUtil.IsInvalid(this._TestString)) return false;
        if (!TableUtil.IsInvalid(this._TestBool)) return false;
        if (!TableUtil.IsInvalid(this._TestInt2)) return false;
        if (!TableUtil.IsInvalid(this._TestEnumName)) return false;
        return true;
    }
    public static DataTest Read(ScorpioReader reader) {
        DataTest ret = new DataTest();
        ret._ID = reader.ReadInt32();
        ret._TestInt = reader.ReadInt32();
        ret._TestString = reader.ReadString();
        ret._TestBool = reader.ReadBool();
        ret._TestInt2 = Int2.Read(reader);
        ret._TestEnumName = TestEnum.valueOf(reader.ReadInt32());
        ret.m_IsInvalid = ret.IsInvalid_impl();
        return ret;
    }
}