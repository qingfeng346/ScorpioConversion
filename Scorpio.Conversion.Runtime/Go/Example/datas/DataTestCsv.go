package datas
//本文件为自动生成，请不要手动修改
import "github.com/qingfeng346/ScorpioConversion/Scorpio.Conversion.Runtime/Go/ScorpioConversionRuntime"
import "fmt"

import "container/list"
import "time"
type DataTestCsv struct {
   
    TestID int32
    testEnum int32
    TestDate *list.List
    TestDateTime time.Time
    TestInt int32
}

// ID ×¢ÊÍ  默认值()
func (data *DataTestCsv) GetID() int32 { 
    return data.TestID;
}
// GetTestID ×¢ÊÍ  默认值()
func (data *DataTestCsv) GetTestID() int32 { 
    return data.TestID;
}
// GettestEnum   默认值()
func (data *DataTestCsv) GettestEnum() int32 { 
    return data.testEnum;
}
// GetTestDate   默认值()
func (data *DataTestCsv) GetTestDate() *list.List { 
    return data.TestDate;
}
// GetTestDateTime   默认值()
func (data *DataTestCsv) GetTestDateTime() time.Time { 
    return data.TestDateTime;
}
// GetTestInt   默认值(999)
func (data *DataTestCsv) GetTestInt() int32 { 
    return data.TestInt;
}

func NewDataTestCsv(fileName string, reader ScorpioConversionRuntime.IReader) *DataTestCsv {
    data := &DataTestCsv{}
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
    return data
}

func (data *DataTestCsv) GetData(key string) interface{} {
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
    return nil;
}

func (data *DataTestCsv) Set(value *DataTestCsv) {
    data.TestID = value.TestID;
    data.testEnum = value.testEnum;
    data.TestDate = value.TestDate;
    data.TestDateTime = value.TestDateTime;
    data.TestInt = value.TestInt;
}

func (data *DataTestCsv) String() string {
    return fmt.Sprintf("TestID : %v , testEnum : %v , TestDate : %v , TestDateTime : %v , TestInt : %v", data.TestID, data.testEnum, data.TestDate, data.TestDateTime, data.TestInt);
}
