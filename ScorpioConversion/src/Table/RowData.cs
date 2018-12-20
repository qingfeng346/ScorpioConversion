using System;
using System.Collections.Generic;
using System.Text;

public class RowData {
    public int RowNumber = 0;
    public List<string> Values = new List<string>();
    public string this[int index] { get { return Values[index]; } }
}
