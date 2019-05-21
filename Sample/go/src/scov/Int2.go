package scov
import (
    "scorpioproto"
)

// Int2 类名
type Int2 struct {
    
    _Value1 int32
    _Value2 int32
}


// ID   默认值()
func (data *Int2) ID() int32 { return data._Value1; }

// GetValue1   默认值()
func (data *Int2) GetValue1() int32 { return data._Value1; }

// GetValue2   默认值()
func (data *Int2) GetValue2() int32 { return data._Value2; }


// GetData 获取数据
func (data *Int2) GetData(key string) interface{} {
    if "Value1" == key {
        return data._Value1;
    }
    if "Value2" == key {
        return data._Value2;
    }
    return nil;
}


// Set 设置数据
func (data *Int2) Set(value *Int2) {
    data._Value1 = value._Value1;
    data._Value2 = value._Value2;
}


// Read 读取数据
func (data *Int2) Read(fileName string, reader scorpioproto.IScorpioReader) {
    data._Value1 = reader.ReadInt32();
    data._Value2 = reader.ReadInt32();
}

// Int2Read 读取数据
func Int2Read(fileName string, reader scorpioproto.IScorpioReader) *Int2 {
    ret := new(Int2)
    ret.Read(fileName, reader)
    return ret
}
