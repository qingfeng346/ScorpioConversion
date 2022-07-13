using System.Collections.Generic;
namespace Scorpio.Conversion.Engine {
    public class RowValue {
        public string name;
        public string value;
        public RowValue(string name, string value) {
            this.name = name;
            this.value = value;
        }
    }
    public class RowData {
        public int RowNumber = 0;
        public string Key = "";
        public List<RowValue> Values = new List<RowValue>();
    }
    public class MergeField {
        public string Name;
        public string DefaultValue = "";
        public bool IsInvalid;
    }
}