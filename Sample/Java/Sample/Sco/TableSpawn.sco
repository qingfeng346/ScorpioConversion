TableSpawn = {
    m_count = 0,
    m_dataArray = {},
    function Initialize(fileName) {
        this.m_dataArray = TableUtil.ReadDatas(_SCRIPT, fileName, "DataSpawn", "ID", "34a59b0b8327d56e524598fdec293a9b")
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