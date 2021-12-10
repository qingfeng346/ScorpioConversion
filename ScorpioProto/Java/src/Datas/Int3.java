package Datas;
//本文件为自动生成，请不要手动修改
import java.util.*;
import ScorpioProto.Commons.*;
import ScorpioProto.Table.*;

public class Int3 implements IData {
    
    private List<Int2> _Value1;
    /**   默认值() */
    public List<Int2> getValue1() { return _Value1; }
    private Integer _Value2;
    /**   默认值() */
    public Integer getValue2() { return _Value2; }
    
    public Object GetData(String key) {
        if ("Value1".equals(key)) return _Value1;
        if ("Value2".equals(key)) return _Value2;
        return null;
    }
    
    public static Int3 Read(String fileName, IScorpioReader reader) {
        Int3 ret = new Int3();
        {
            List<Int2> list = new ArrayList<Int2>();
            int number = reader.ReadInt32();
            for (int i = 0; i < number; ++i) { list.add(Int2.Read(fileName, reader)); }
            ret._Value1 = Collections.unmodifiableList(list);
        }
        ret._Value2 = reader.ReadInt32();
        return ret;
    }
    
    public void Set(Int3 value) {
        this._Value1 = value._Value1;
        this._Value2 = value._Value2;
    }
    
    @Override
    public String toString() {
        return "Value1:" + _Value1 + "," + "Value2:" + _Value2;
    }
}
