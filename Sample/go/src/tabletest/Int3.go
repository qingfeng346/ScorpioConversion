package tabletest
import (
    "scorpioproto"
)

// Int3 类名
type Int3 struct {
    
    _value1 *Int2
    _value2 int32
}


// ID   默认值()
func (data *Int3) ID() *Int2 { return data._value1; }

// Getvalue1   默认值()
func (data *Int3) Getvalue1() *Int2 { return data._value1; }

// Getvalue2   默认值()
func (data *Int3) Getvalue2() int32 { return data._value2; }


// GetData 获取数据
func (data *Int3) GetData(key string) interface{} {
    if "value1" == key {
        return data._value1;
    }
    if "value2" == key {
        return data._value2;
    }
    return nil;
}


// Set 设置数据
func (data *Int3) Set(value *Int3) {
    data._value1 = value._value1;
    data._value2 = value._value2;
}


// Read 读取数据
func (data *Int3) Read(fileName string, reader scorpioproto.IScorpioReader) {
    data._value1 = Int2Read(fileName, reader);
    data._value2 = reader.ReadInt32();
}

// Int3Read 读取数据
func Int3Read(fileName string, reader scorpioproto.IScorpioReader) *Int3 {
    ret := new(Int3)
    ret.Read(fileName, reader)
    return ret
}
