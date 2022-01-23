
#import "Int2.sco"
#import "Int3.sco"
#import "TestEnum.sco"
#import "TableTest.sco"
#import "DataTest.sco"
#import "TableSpawn.sco"
#import "DataSpawn.sco"
TableManager = {
    getTest(ID) {
        if (this._tableTest == null) {
            var reader = GetReader("Test");
            this._tableTest = new TableTest().Initialize("Test", reader);
            reader.Close()
        }
        return ID == null ? this._tableTest : this._tableTest(ID);
    }
    getSpawnTest1(ID) {
        if (this._tableTest1 == null) {
            var reader = GetReader("Test1");
            this._tableTest1 = new TableSpawn().Initialize("Test1", reader);
            reader.Close()
        }
        return ID == null ? this._tableSpawn : this._tableSpawn(ID);
    }
    getSpawnTest2(ID) {
        if (this._tableTest2 == null) {
            var reader = GetReader("Test2");
            this._tableTest2 = new TableSpawn().Initialize("Test2", reader);
            reader.Close()
        }
        return ID == null ? this._tableSpawn : this._tableSpawn(ID);
    }
    getSpawn(name) {
        if (name == "Test1") { return this.getSpawnTest1(); }
        if (name == "Test2") { return this.getSpawnTest2(); }
        return null;
    }
}