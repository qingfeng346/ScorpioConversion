
package Datas;
import Scorpio.Conversion.IReader;
public abstract class TableManagerBase {
    public abstract IReader GetReader(String name) throws Exception;
    private TableTest _tableTest = null;
    public TableTest getTest() throws Exception {
        if (this._tableTest == null) {
            IReader reader = GetReader("Test");
            this._tableTest = new TableTest().Initialize("Test", reader);
            reader.Close();
        }
        return this._tableTest;
    }
    private TableTestCsv _tableTestCsv = null;
    public TableTestCsv getTestCsv() throws Exception {
        if (this._tableTestCsv == null) {
            IReader reader = GetReader("TestCsv");
            this._tableTestCsv = new TableTestCsv().Initialize("TestCsv", reader);
            reader.Close();
        }
        return this._tableTestCsv;
    }
    private TableSpawn _tableTest1 = null;
    public TableSpawn getSpawnTest1() throws Exception {
        if (this._tableTest1 == null) {
            IReader reader = GetReader("Test1");
            this._tableTest1 = new TableSpawn().Initialize("Test1", reader);
            reader.Close();
        }
        return this._tableTest1;
    }
    private TableSpawn _tableTest2 = null;
    public TableSpawn getSpawnTest2() throws Exception {
        if (this._tableTest2 == null) {
            IReader reader = GetReader("Test2");
            this._tableTest2 = new TableSpawn().Initialize("Test2", reader);
            reader.Close();
        }
        return this._tableTest2;
    }
    public TableSpawn getSpawn(String name) throws Exception {
        if ("Test1".equals(name)) return getSpawnTest1();
        if ("Test2".equals(name)) return getSpawnTest2();
        return null;
    }
}