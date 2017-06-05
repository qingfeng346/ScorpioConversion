TableLanguage = {
    m_tableManager = null,
    m_count = 0,
    m_dataArray = {},
    function Initialize(tableManager, fileName) {
        this.m_tableManager = tableManager
        this.m_dataArray = ScorpioSerializer.ReadDatas(_SCRIPT, tableManager, fileName, "DataLanguage", "Index", "6ff23656c42bd70405cfb2df35b38a27")
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