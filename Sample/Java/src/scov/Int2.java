
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

public class Int2 implements IData {
    private boolean m_IsInvalid;
    
    private Integer _Value1;
    /**   默认值() */
    public Integer getValue1() { return _Value1; }
    public Integer ID() { return _Value1; }
    private Integer _Value2;
    /**   默认值() */
    public Integer getValue2() { return _Value2; }
    
    public Object GetData(String key ) {
        if ("Value1".equals(key)) return _Value1;
        if ("Value2".equals(key)) return _Value2;
        return null;
    }
    
    public boolean IsInvalid() { return m_IsInvalid; }
    private boolean CheckInvalid() {
        if (!TableUtil.IsInvalid(this._Value1)) return false;
        if (!TableUtil.IsInvalid(this._Value2)) return false;
        return true;
    }
    
    public static Int2 Read(String fileName, IScorpioReader reader) {
        Int2 ret = new Int2();
        ret._Value1 = reader.ReadInt32();
        ret._Value2 = reader.ReadInt32();
        ret.m_IsInvalid = ret.CheckInvalid();
        return ret;
    }
    
    public void Set(Int2 value) {
        this._Value1 = value._Value1;
        this._Value2 = value._Value2;
    }
    
    @Override
    public String toString() {
        return "{ " + 
            "Value1 : " +  _Value1 + "," + 
            "Value2 : " +  _Value2 + 
            " }";
    }
}