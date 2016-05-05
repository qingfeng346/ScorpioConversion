using System;
using System.Collections.Generic;
using System.Text;

public class TemplateScorpio {
    public const string Head = @"";
    public const string Table = @"__TableName = {
    m_count = 0,
    m_dataArray = {},
    function Initialize(fileName) {
        this.m_dataArray = TableUtil.ReadDatas(_SCRIPT, fileName, ""__DataName"", ""__KeyName"", ""__MD5"")
        this.m_count = table.count(this.m_dataArray)
        return this
    }
    function GetElement(ID) {
        return this.m_dataArray[ID]
    }
    function GetValue(ID) {
        return this.m_dataArray[ID]
    }
    function Contains(ID) {
        return table.containskey(this.m_dataArray, ID)
    }
    function Count() {
        return this.m_count
    }
    function Datas() {
        return this.m_dataArray
    }
}
";
}
