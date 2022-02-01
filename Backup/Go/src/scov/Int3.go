package scov
import (
    "scorpioproto"
    "container/list"
)

// Int3 类名
type Int3 struct {
    
    _Value1 *list.List
    _Value2 int32
}


// ID   默认值()
func (data *Int3) ID() *Int2 { return data._Value1; }

// GetValue1   默认值()
func (data *Int3) GetValue1() *list.List { return data._Value1; }

// GetValue2   默认值()
func (data *Int3) GetValue2() int32 { return data._Value2; }


// GetData 获取数据
func (data *Int3) GetData(key string) interface{} {
    if "Value1" == key {
        return data._Value1;
    }
    if "Value2" == key {
        return data._Value2;
    }
    return nil;
}


// Set 设置数据
func (data *Int3) Set(value *Int3) {
    data._Value1 = value._Value1;
    data._Value2 = value._Value2;
}


// Read 读取数据
func (data *Int3) Read(fileName string, reader scorpioproto.IScorpioReader) {
    {
        data._Value1 = list.New();
        number := int(reader.ReadInt32());
        for i := 0; i < number; i++ { 
            data._Value1.PushBack(Int2Read(fileName, reader)); 
        }
    }
    data._Value2 = reader.ReadInt32();
}

// Int3Read 读取数据
func Int3Read(fileName string, reader scorpioproto.IScorpioReader) *Int3 {
    ret := new(Int3)
    ret.Read(fileName, reader)
    return ret
}
