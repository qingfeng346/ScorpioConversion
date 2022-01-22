
namespace Datas {
    public partial class TableManager {
        private TableTest _tableTest = null;
        public TableTest getTest() {
            if (this._tableTest == null) {
                using var reader = GetReader("Test");
                this._tableTest = new TableTest().Initialize("Test", reader);
            }
            return this._tableTest;
        }
        private TableSpawn _tableTest1 = null;
        public TableSpawn getSpawnTest1() {
            if (this._tableTest1 == null) {
                using var reader = GetReader("Test1");
                this._tableTest1 = new TableSpawn().Initialize("Test1", reader);
            }
            return this._tableTest1;
        }
        private TableSpawn _tableTest2 = null;
        public TableSpawn getSpawnTest2() {
            if (this._tableTest2 == null) {
                using var reader = GetReader("Test2");
                this._tableTest2 = new TableSpawn().Initialize("Test2", reader);
            }
            return this._tableTest2;
        }
        public TableSpawn getSpawn(string name) {
            if (name == "Test1") return getSpawnTest1();
            if (name == "Test2") return getSpawnTest2();
            return null;
        }
    }
}