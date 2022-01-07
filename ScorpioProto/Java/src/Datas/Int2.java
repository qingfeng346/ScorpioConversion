package Datas;
//本文件为自动生成，请不要手动修改
import java.util.*;
import Scorpio.Conversion.*;

public class Int2 implements IData {
    
    private Integer _Value1;
    /**   默认值() */
    public Integer getValue1() { return _Value1; }
    private Integer _Value2;
    /**   默认值() */
    public Integer getValue2() { return _Value2; }
    
    public Object GetData(String key) {
        if ("Value1".equals(key)) return _Value1;
        if ("Value2".equals(key)) return _Value2;
        return null;
    }
    
    public static Int2 Read(String fileName, IReader reader) throws Exception {
        Int2 ret = new Int2();
        ret._Value1 = reader.ReadInt32();
        ret._Value2 = reader.ReadInt32();
        return ret;
    }
    
    public void Set(Int2 value) {
        this._Value1 = value._Value1;
        this._Value2 = value._Value2;
    }
    
    @Override
    public String toString() {
        return "Value1:" + _Value1 + "," + "Value2:" + _Value2;
    }
}
