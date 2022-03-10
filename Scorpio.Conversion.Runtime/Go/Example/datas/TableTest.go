package datas
//本文件为自动生成，请不要手动修改
import "github.com/qingfeng346/ScorpioConversion/Scorpio.Conversion.Runtime/Go/ScorpioConversionRuntime"
import "errors"
type TableTest struct {
    count int
    dataArray map[int32]*DataTest
}
func (table *TableTest) Initialize(fileName string, reader ScorpioConversionRuntime.IReader) error {
    if table.dataArray == nil {
		table.dataArray = map[int32]*DataTest{}
	}
    row := int(reader.ReadInt32());
    layoutMD5 := reader.ReadString();
    if (layoutMD5 != "5c86f5006b60d711c1ca95a5ea69b8db") {
        return errors.New("File schemas do not match [TableTest] : " + fileName)
    }
    ScorpioConversionRuntime.ReadHead(reader);
    for i := 0; i < row; i++ {
        var pData = NewDataTest(fileName, reader);
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
    return nil;
}
func (table *TableTest) Datas() map[int32]*DataTest {
    return table.dataArray
}
func (table *TableTest) GetValueObject(ID interface{}) ScorpioConversionRuntime.IData {
    return table.GetValue(ID.(int32))
}
func (table *TableTest) ContainsObject(ID interface{}) bool {
   return table.Contains(ID.(int32))
}
func (table *TableTest) Count() int {
    return table.count;
}