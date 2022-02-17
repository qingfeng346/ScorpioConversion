
const TableTest = require("./TableTest")
const TableTestCsv = require("./TableTestCsv")
const TableSpawn = require("./TableSpawn")
class TableManager {
    getTest() {
        if (this._tableTest == null) {
            var reader = this.GetReader("Test");
            this._tableTest = new TableTest().Initialize("Test", reader);
            reader.Close()
        }
        return this._tableTest;
    }
    getTestCsv() {
        if (this._tableTestCsv == null) {
            var reader = this.GetReader("TestCsv");
            this._tableTestCsv = new TableTestCsv().Initialize("TestCsv", reader);
            reader.Close()
        }
        return this._tableTestCsv;
    }
    getSpawnTest1() {
        if (this._tableTest1 == null) {
            var reader = this.GetReader("Test1");
            this._tableTest1 = new TableSpawn().Initialize("Test1", reader);
            reader.Close()
        }
        return this._tableTest1;
    }
    getSpawnTest2() {
        if (this._tableTest2 == null) {
            var reader = this.GetReader("Test2");
            this._tableTest2 = new TableSpawn().Initialize("Test2", reader);
            reader.Close()
        }
        return this._tableTest2;
    }
    getSpawn(name) {
        if (name == "Test1") { return this.getSpawnTest1(); }
        if (name == "Test2") { return this.getSpawnTest2(); }
        return null;
    }
}
module.exports = new TableManager();