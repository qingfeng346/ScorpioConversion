package scov
import (
    "scorpioproto"
)


// TableTest 类名
type TableTest struct {
    count int
    dataArray map[int32]*DataTest
}

// Initialize 初始化
func (table *TableTest) Initialize(fileName string, reader scorpioproto.IScorpioReader) {
    if table.dataArray == nil {
		table.dataArray = map[int32]*DataTest{}
	}
    iRow := scorpioproto.TableUtilReadHead(reader, fileName, "898169d7e1d0c2be013482d2c80052cc");
    for i := 0; i < iRow; i++ {
        pData := DataTestRead(fileName, reader)
        if table.Contains(pData.ID()) {
            table.dataArray[pData.ID()].Set(pData)
        } else {
            table.dataArray[pData.ID()] = pData;
        }
    }
    table.count = len(table.dataArray)
}

// Contains 是否包含数据
func (table *TableTest) Contains(ID int32) bool {
    if _, ok := table.dataArray[ID]; ok {
        return true
    }
    return false
}

// GetValue 获取数据
func (table *TableTest) GetValue(ID int32) *DataTest {
    if table.Contains(ID) {
        return table.dataArray[ID];
    }
    //TableUtilWarning("DataTest key is not exist " + ID);
    return nil;
}

// Datas 所有数据
func (table *TableTest) Datas() map[int32]*DataTest {
    return table.dataArray
}

// Count 数量
func (table *TableTest) Count() int {
    return table.count
}
