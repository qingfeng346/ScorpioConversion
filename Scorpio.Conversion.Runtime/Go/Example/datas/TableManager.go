package datas
type TableManager struct {
    _tableTest *TableTest
    _tableTestCsv *TableTestCsv
    _tableTest1 *TableSpawn
    _tableTest2 *TableSpawn
}
func (tableManager *TableManager) GetTest() *TableTest {
    if (tableManager._tableTest == nil) {
        reader := GetReader("Test")
        tableManager._tableTest = new(TableTest)
        tableManager._tableTest.Initialize("Test", reader)
        reader.Close()
    }
    return tableManager._tableTest
}
func (tableManager *TableManager) GetTestCsv() *TableTestCsv {
    if (tableManager._tableTestCsv == nil) {
        reader := GetReader("TestCsv")
        tableManager._tableTestCsv = new(TableTestCsv)
        tableManager._tableTestCsv.Initialize("TestCsv", reader)
        reader.Close()
    }
    return tableManager._tableTestCsv
}
func (tableManager *TableManager) GetSpawnTest1() *TableSpawn {
    if (tableManager._tableTest1 == nil) {
        reader := GetReader("Test1")
        tableManager._tableTest1 = new(TableSpawn)
        tableManager._tableTest1.Initialize("Test1", reader)
        reader.Close()
    }
    return tableManager._tableTest1
}
func (tableManager *TableManager) GetSpawnTest2() *TableSpawn {
    if (tableManager._tableTest2 == nil) {
        reader := GetReader("Test2")
        tableManager._tableTest2 = new(TableSpawn)
        tableManager._tableTest2.Initialize("Test2", reader)
        reader.Close()
    }
    return tableManager._tableTest2
}
func (tableManager *TableManager) GetSpawn(name string) *TableSpawn {
    if (name == "Test1") {
        return tableManager.GetSpawnTest1();
    }
    if (name == "Test2") {
        return tableManager.GetSpawnTest2();
    }
    return nil;
}