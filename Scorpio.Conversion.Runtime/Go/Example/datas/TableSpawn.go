package datas
//本文件为自动生成，请不要手动修改
import "github.com/qingfeng346/ScorpioConversion/Scorpio.Conversion.Runtime/Go/ScorpioConversionRuntime"
import "errors"
type TableSpawn struct {
    count int
    dataArray map[int32]*DataSpawn
}
func (table *TableSpawn) Initialize(fileName string, reader ScorpioConversionRuntime.IReader) error {
    if table.dataArray == nil {
		table.dataArray = map[int32]*DataSpawn{}
	}
    row := int(reader.ReadInt32());
    layoutMD5 := reader.ReadString();
    if (layoutMD5 != "484cdae7d179982f1c7868078204d81d") {
        return errors.New("File schemas do not match [TableSpawn] : " + fileName)
    }
    ScorpioConversionRuntime.ReadHead(reader);
    for i := 0; i < row; i++ {
        var pData = NewDataSpawn(fileName, reader);
        if table.Contains(pData.GetID()) {
            table.dataArray[pData.GetID()].Set(pData)
        } else {
            table.dataArray[pData.GetID()] = pData;
        }
    }
    table.count = len(table.dataArray)
    return nil
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
    return nil;
}
func (table *TableSpawn) Datas() map[int32]*DataSpawn {
    return table.dataArray
}
func (table *TableSpawn) GetValueObject(ID interface{}) ScorpioConversionRuntime.IData {
    return table.GetValue(ID.(int32))
}
func (table *TableSpawn) ContainsObject(ID interface{}) bool {
   return table.Contains(ID.(int32))
}
func (table *TableSpawn) Count() int {
    return table.count;
}