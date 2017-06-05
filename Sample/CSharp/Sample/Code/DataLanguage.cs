using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Scorpio.Commons;
using Scorpio.Table;
namespace ScorpioProtoTest {
public class DataLanguage : IData {
    private bool m_IsInvalid;
    private int _Index;
    /* <summary> 索引  默认值() </summary> */
    public int getIndex() { return _Index; }
    public int ID() { return _Index; }
    private string _Key;
    /* <summary> 关键字  默认值() </summary> */
    public string getKey() { return _Key; }
    private string _Text;
    /* <summary> 文字  默认值() </summary> */
    public string getText() { return _Text; }
    public object GetData(string key ) {
        if (key == "Index") return _Index;
        if (key == "Key") return _Key;
        if (key == "Text") return _Text;
        return null;
    }
    public bool IsInvalid() { return m_IsInvalid; }
    private bool IsInvalid_impl() {
        if (!TableUtil.IsInvalid(this._Index)) return false;
        if (!TableUtil.IsInvalid(this._Key)) return false;
        if (!TableUtil.IsInvalid(this._Text)) return false;
        return true;
    }
    public override string ToString() {
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
}