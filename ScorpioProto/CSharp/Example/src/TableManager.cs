
namespace Datas {
    public partial class TableManager {
        private TableTest _tableTest = null;
        public TableTest Test {
            get {
                if (this._tableTest == null) {
                    using (var reader = GetReader("Test")) {
                        this._tableTest = new TableTest().Initialize("Test", reader);
                    }
                }
                return this._tableTest;
            }
        }
        private TableTestCsv _tableTestCsv = null;
        public TableTestCsv TestCsv {
            get {
                if (this._tableTestCsv == null) {
                    using (var reader = GetReader("TestCsv")) {
                        this._tableTestCsv = new TableTestCsv().Initialize("TestCsv", reader);
                    }
                }
                return this._tableTestCsv;
            }
        }
        private TableSpawn _tableTest1 = null;
        public TableSpawn SpawnTest1 {
            get {
                if (this._tableTest1 == null) {
                    using (var reader = GetReader("Test1")) {
                        this._tableTest1 = new TableSpawn().Initialize("Test1", reader);
                    }
                }
                return this._tableTest1;
            }
        }
        private TableSpawn _tableTest2 = null;
        public TableSpawn SpawnTest2 {
            get {
                if (this._tableTest2 == null) {
                    using (var reader = GetReader("Test2")) {
                        this._tableTest2 = new TableSpawn().Initialize("Test2", reader);
                    }
                }
                return this._tableTest2;
            }
        }
        public TableSpawn GetSpawn(string name) {
            if (name == "Test1") return SpawnTest1;
            if (name == "Test2") return SpawnTest2;
            return null;
        }
    }
}