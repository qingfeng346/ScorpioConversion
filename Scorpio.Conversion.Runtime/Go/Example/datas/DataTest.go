package datas
//本文件为自动生成，请不要手动修改
import "github.com/qingfeng346/ScorpioConversion/Scorpio.Conversion.Runtime/Go/ScorpioConversionRuntime"
import "fmt"

import "container/list"
import "time"
type DataTest struct {
   
    TestID int32
    testEnum int32
    TestDate *list.List
    TestDateTime time.Time
    TestInt int32
    TestBytes []byte
}

// ID 注释  默认值()
func (data *DataTest) GetID() int32 { 
    return data.TestID;
}
// GetTestID 注释  默认值()
func (data *DataTest) GetTestID() int32 { 
    return data.TestID;
}
// GettestEnum   默认值()
func (data *DataTest) GettestEnum() int32 { 
    return data.testEnum;
}
// GetTestDate   默认值()
func (data *DataTest) GetTestDate() *list.List { 
    return data.TestDate;
}
// GetTestDateTime   默认值()
func (data *DataTest) GetTestDateTime() time.Time { 
    return data.TestDateTime;
}
// GetTestInt   默认值(999)
func (data *DataTest) GetTestInt() int32 { 
    return data.TestInt;
}
// GetTestBytes 内容为1234567890的base64数据  默认值(base64://MTIzNDU2Nzg5MA==)
func (data *DataTest) GetTestBytes() []byte { 
    return data.TestBytes;
}

func NewDataTest(fileName string, reader ScorpioConversionRuntime.IReader) *DataTest {
    data := &DataTest{}
    data.TestID = reader.ReadInt32();
    data.testEnum = reader.ReadInt32();
    {
        data.TestDate = list.New();
        number := int(reader.ReadInt32());
        for i := 0; i < number; i++ { 
            data.TestDate.PushBack(NewInt3(fileName, reader));
        }
    }
    data.TestDateTime = reader.ReadDateTime();
    data.TestInt = reader.ReadInt32();
    data.TestBytes = reader.ReadBytes();
    return data
}

func (data *DataTest) GetData(key string) interface{} {
    if key == "TestID" {
        return data.TestID;
    }
    if key == "testEnum" {
        return data.testEnum;
    }
    if key == "TestDate" {
        return data.TestDate;
    }
    if key == "TestDateTime" {
        return data.TestDateTime;
    }
    if key == "TestInt" {
        return data.TestInt;
    }
    if key == "TestBytes" {
        return data.TestBytes;
    }
    return nil;
}

func (data *DataTest) Set(value *DataTest) {
    data.TestID = value.TestID;
    data.testEnum = value.testEnum;
    data.TestDate = value.TestDate;
    data.TestDateTime = value.TestDateTime;
    data.TestInt = value.TestInt;
    data.TestBytes = value.TestBytes;
}

func (data *DataTest) String() string {
    return fmt.Sprintf("TestID : %v , testEnum : %v , TestDate : %v , TestDateTime : %v , TestInt : %v , TestBytes : %v", data.TestID, data.testEnum, data.TestDate, data.TestDateTime, data.TestInt, data.TestBytes);
}
