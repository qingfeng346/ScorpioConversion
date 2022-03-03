package Datas;
//本文件为自动生成，请不要手动修改
import java.util.*;
import Scorpio.Conversion.Runtime.*;

public class DataSpawn implements IData {
    
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
    private Boolean _TestBool;
    /** bool类型  默认值() */
    public Boolean getTestBool() { return _TestBool; }
    private Int2 _TestInt2;
    /** 自定义类型 根据ExcelConfig下 table.sco文件定义的Int2解析 类型为table_后面的名字 格式为 , 隔开  默认值() */
    public Int2 getTestInt2() { return _TestInt2; }
    private TestEnum _TestEnumName;
    /** 自定义枚举  默认值() */
    public TestEnum getTestEnumName() { return _TestEnumName; }
    
    public DataSpawn(String fileName, IReader reader) throws Exception {
        this._ID = reader.ReadInt32();
        this._TestInt = reader.ReadInt32();
        this._TestString = reader.ReadString();
        this._TestLanguage = reader.ReadString();
        this._TestBool = reader.ReadBool();
        this._TestInt2 = new Int2(fileName, reader);
        this._TestEnumName = TestEnum.valueOf(reader.ReadInt32());
    }
    
    public Object GetData(String key) {
        if ("ID".equals(key)) return _ID;
        if ("TestInt".equals(key)) return _TestInt;
        if ("TestString".equals(key)) return _TestString;
        if ("TestLanguage".equals(key)) return _TestLanguage;
        if ("TestBool".equals(key)) return _TestBool;
        if ("TestInt2".equals(key)) return _TestInt2;
        if ("TestEnumName".equals(key)) return _TestEnumName;
        return null;
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
        return "ID:" + _ID + "," + "TestInt:" + _TestInt + "," + "TestString:" + _TestString + "," + "TestLanguage:" + _TestLanguage + "," + "TestBool:" + _TestBool + "," + "TestInt2:" + _TestInt2 + "," + "TestEnumName:" + _TestEnumName;
    }
}
