package scov
import (
    "scorpioproto"
)


// TableSpawn 类名
type TableSpawn struct {
    count int
    dataArray map[int32]*DataSpawn
}

// Initialize 初始化
func (table *TableSpawn) Initialize(fileName string, reader scorpioproto.IScorpioReader) {
    if table.dataArray == nil {
		table.dataArray = map[int32]*DataSpawn{}
	}
    iRow := scorpioproto.TableUtilReadHead(reader, fileName, "484cdae7d179982f1c7868078204d81d");
    for i := 0; i < iRow; i++ {
        pData := DataSpawnRead(fileName, reader)
        if table.Contains(pData.ID()) {
            table.dataArray[pData.ID()].Set(pData)
        } else {
            table.dataArray[pData.ID()] = pData;
        }
    }
    table.count = len(table.dataArray)
}

// Contains 是否包含数据
func (table *TableSpawn) Contains(ID int32) bool {
    if _, ok := table.dataArray[ID]; ok {
        return true
    }
    return false
}

// GetValue 获取数据
func (table *TableSpawn) GetValue(ID int32) *DataSpawn {
    if table.Contains(ID) {
        return table.dataArray[ID];
    }
    //TableUtilWarning("DataSpawn key is not exist " + ID);
    return nil;
}

// Datas 所有数据
func (table *TableSpawn) Datas() map[int32]*DataSpawn {
    return table.dataArray
}

// Count 数量
func (table *TableSpawn) Count() int {
    return table.count
}
