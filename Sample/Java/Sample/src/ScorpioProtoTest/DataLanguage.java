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
public class DataLanguage implements IData {
    private boolean m_IsInvalid = false;
    private Integer _Index;
    /** 索引  默认值() */
    public Integer getIndex() { return _Index; }
    public Integer ID() { return _Index; }
    private String _Key;
    /** 关键字  默认值() */
    public String getKey() { return _Key; }
    private String _Text;
    /** 文字  默认值() */
    public String getText() { return _Text; }
    public Object GetData(String key ) {
        if (key.equals("Index")) return _Index;
        if (key.equals("Key")) return _Key;
        if (key.equals("Text")) return _Text;
        return null;
    }
    public boolean IsInvalid() { return m_IsInvalid; }
    private boolean IsInvalid_impl() {
        if (!TableUtil.IsInvalid(this._Index)) return false;
        if (!TableUtil.IsInvalid(this._Key)) return false;
        if (!TableUtil.IsInvalid(this._Text)) return false;
        return true;
    }
    @Override
    public String toString() {
        return "{ " + 
                "Index : " + _Index + "," + 
                "Key : " + _Key + "," + 
                "Text : " + _Text + 
                " }";
    }
    public static DataLanguage Read(TableManager tableManager, String fileName, ScorpioReader reader) {
        DataLanguage ret = new DataLanguage();
        ret._Index = reader.ReadInt32();
        ret._Key = reader.ReadString();
        ret._Text = reader.ReadString();
        ret.m_IsInvalid = ret.IsInvalid_impl();
        return ret;
    }
}