__TableName = 
{
    m_count = 0,
    m_dataArray = {},
    function Initialize(fileName) {
        this.m_dataArray = TableUtil.ReadDatas(_SCRIPT, fileName, "__DataName", "__KeyName", "__MD5")
        this.m_count = table.count(this.m_dataArray)
        return this
    }
    function Contains(ID) {
        return this.m_dataArray[ID] != null
    }
    function GetElement(ID) {
        return this.m_dataArray[ID]
    }
    function GetValue(ID) {
        return this.m_dataArray[ID]
    }
    function Count() {
        return this.m_count
    }
}