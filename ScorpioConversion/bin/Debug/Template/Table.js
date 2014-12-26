__TableName = 
{
    m_fileName = "",
    m_count = 0,
    m_dataArray = {},
    Construct : function(fileName)
    {
        var ret = clone(this)
        ret.m_fileName = fileName
        return ret
    }
    function Initialize() {
        this.m_dataArray = TableUtil.ReadDatas(this.m_fileName, "__MD5")
        this.m_count = table.count(this.m_dataArray)
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