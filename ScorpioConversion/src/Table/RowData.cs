using System;
using System.Collections.Generic;
using System.Text;
using NPOI.SS.UserModel;
public class RowValue {
    public string value;
    public ICell cell;
}
public class RowData {
    public int RowNumber = 0;
    public string Key = "";
    public List<RowValue> Values = new List<RowValue>();
    public RowValue this[int index] { get { return Values[index]; } }
}
