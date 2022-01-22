
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
    }
}