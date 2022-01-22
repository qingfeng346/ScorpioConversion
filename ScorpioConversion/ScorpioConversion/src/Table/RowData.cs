using System.Collections.Generic;
namespace Scorpio.Conversion {
    public class RowValue {
        public string value;
        public static implicit operator RowValue(string value) {
            return new RowValue() { value = value };
        }
        public static implicit operator string(RowValue value) {
            return value.value;
        }
    }
    public class RowData {
        public int RowNumber = 0;
        public string Key = "";
        public List<RowValue> Values = new List<RowValue>();
    }
}