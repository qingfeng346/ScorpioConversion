package datas
//本文件为自动生成，请不要手动修改
import "github.com/qingfeng346/ScorpioConversion/Scorpio.Conversion.Runtime/Go/ScorpioConversionRuntime"
import "fmt"

import "container/list"
type Int3 struct {
   
    Value1 *list.List
    Value2 int32
}

// GetValue1   默认值()
func (data *Int3) GetValue1() *list.List { 
    return data.Value1;
}
// GetValue2   默认值()
func (data *Int3) GetValue2() int32 { 
    return data.Value2;
}

func NewInt3(fileName string, reader ScorpioConversionRuntime.IReader) *Int3 {
    data := &Int3{}
    {
        data.Value1 = list.New();
        number := int(reader.ReadInt32());
        for i := 0; i < number; i++ { 
            data.Value1.PushBack(NewInt2(fileName, reader));
        }
    }
    data.Value2 = reader.ReadInt32();
    return data
}

func (data *Int3) GetData(key string) interface{} {
    if key == "Value1" {
        return data.Value1;
    }
    if key == "Value2" {
        return data.Value2;
    }
    return nil;
}

func (data *Int3) Set(value *Int3) {
    data.Value1 = value.Value1;
    data.Value2 = value.Value2;
}

func (data *Int3) String() string {
    return fmt.Sprintf("Value1 : %v , Value2 : %v", data.Value1, data.Value2);
}
