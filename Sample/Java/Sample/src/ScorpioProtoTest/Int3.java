package ScorpioProtoTest;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import Scorpio.Commons.*;
import Scorpio.Table.*;
@SuppressWarnings("unused")
public class Int3 implements IData {
    private boolean m_IsInvalid = false;
    private List<Int2> _Value1;
    /** () */
    public List<Int2> getValue1() { return _Value1; }
    private Integer _Value2;
    /** () */
    public Integer getValue2() { return _Value2; }
    public Object GetData(String key ) {
        if (key.equals("Value1")) return _Value1;
        if (key.equals("Value2")) return _Value2;
        return null;
    }
    public boolean IsInvalid() { return m_IsInvalid; }
    private boolean IsInvalid_impl() {
        if (!TableUtil.IsInvalid(this._Value1)) return false;
        if (!TableUtil.IsInvalid(this._Value2)) return false;
        return true;
    }
    public static Int3 Read(ScorpioReader reader) {
        Int3 ret = new Int3();
        {
            int number = reader.ReadInt32();
            ArrayList<Int2> list = new ArrayList<Int2>();
            for (int i = 0;i < number; ++i) { list.add(Int2.Read(reader)); }
            ret._Value1 = Collections.unmodifiableList(list);
        }
        ret._Value2 = reader.ReadInt32();
        ret.m_IsInvalid = ret.IsInvalid_impl();
        return ret;
    }
}