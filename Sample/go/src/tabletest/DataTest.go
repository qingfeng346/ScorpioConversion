package tabletest
import (
    "scorpioproto"
    "container/list"
)

// DataTest 类名
type DataTest struct {
    
    _TestID int32
    _testEnum TestEnum
    _TestDate *list.List
}


// ID   默认值()
func (data *DataTest) ID() int32 { return data._TestID; }

// GetTestID   默认值()
func (data *DataTest) GetTestID() int32 { return data._TestID; }

// GettestEnum   默认值()
func (data *DataTest) GettestEnum() TestEnum { return data._testEnum; }

// GetTestDate   默认值()
func (data *DataTest) GetTestDate() *list.List { return data._TestDate; }


// GetData 获取数据
func (data *DataTest) GetData(key string) interface{} {
    if "TestID" == key {
        return data._TestID;
    }
    if "testEnum" == key {
        return data._testEnum;
    }
    if "TestDate" == key {
        return data._TestDate;
    }
    return nil;
}


// Set 设置数据
func (data *DataTest) Set(value *DataTest) {
    data._TestID = value._TestID;
    data._testEnum = value._testEnum;
    data._TestDate = value._TestDate;
}


// Read 读取数据
func (data *DataTest) Read(fileName string, reader scorpioproto.IScorpioReader) {
    data._TestID = reader.ReadInt32();
    data._testEnum = TestEnum(reader.ReadInt32());
    {
        data._TestDate = list.New();
        number := int(reader.ReadInt32());
        for i := 0; i < number; i++ { 
            data._TestDate.PushBack(Int3Read(fileName, reader)); 
        }
    }
}

// DataTestRead 读取数据
func DataTestRead(fileName string, reader scorpioproto.IScorpioReader) *DataTest {
    ret := new(DataTest)
    ret.Read(fileName, reader)
    return ret
}
