package datas
//本文件为自动生成，请不要手动修改
import "github.com/qingfeng346/ScorpioConversion/Scorpio.Conversion.Runtime/Go/ScorpioConversionRuntime"
import "fmt"

type Int2 struct {
   
    Value1 int32
    Value2 int32
}

// GetValue1   默认值()
func (data *Int2) GetValue1() int32 { 
    return data.Value1;
}
// GetValue2   默认值()
func (data *Int2) GetValue2() int32 { 
    return data.Value2;
}

func NewInt2(fileName string, reader ScorpioConversionRuntime.IReader) *Int2 {
    data := &Int2{}
    data.Value1 = reader.ReadInt32();
    data.Value2 = reader.ReadInt32();
    return data
}

func (data *Int2) GetData(key string) interface{} {
    if key == "Value1" {
        return data.Value1;
    }
    if key == "Value2" {
        return data.Value2;
    }
    return nil;
}

func (data *Int2) Set(value *Int2) {
    data.Value1 = value.Value1;
    data.Value2 = value.Value2;
}

func (data *Int2) String() string {
    return fmt.Sprintf("Value1 [%v] , Value2 [%v]", data.Value1, data.Value2);
}
