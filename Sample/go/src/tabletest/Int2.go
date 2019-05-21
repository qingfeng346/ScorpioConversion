package tabletest
import (
    "scorpioproto"
)

// Int2 类名
type Int2 struct {
    
    _value1 int32
    _value2 int32
}


// ID   默认值()
func (data *Int2) ID() int32 { return data._value1; }

// Getvalue1   默认值()
func (data *Int2) Getvalue1() int32 { return data._value1; }

// Getvalue2   默认值()
func (data *Int2) Getvalue2() int32 { return data._value2; }


// GetData 获取数据
func (data *Int2) GetData(key string) interface{} {
    if "value1" == key {
        return data._value1;
    }
    if "value2" == key {
        return data._value2;
    }
    return nil;
}


// Set 设置数据
func (data *Int2) Set(value *Int2) {
    data._value1 = value._value1;
    data._value2 = value._value2;
}


// Read 读取数据
func (data *Int2) Read(fileName string, reader scorpioproto.IScorpioReader) {
    data._value1 = reader.ReadInt32();
    data._value2 = reader.ReadInt32();
}

// Int2Read 读取数据
func Int2Read(fileName string, reader scorpioproto.IScorpioReader) *Int2 {
    ret := new(Int2)
    ret.Read(fileName, reader)
    return ret
}
