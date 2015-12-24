public class __TableName : ITable {
	const string FILE_MD5_CODE = "__MD5";
    private int m_count = 0;
    private Dictionary<__KeyType, __DataName> m_dataArray = new Dictionary<__KeyType, __DataName>();
    public __TableName Initialize(string fileName) {
        m_dataArray.Clear();
        ScorpioReader reader = new ScorpioReader(TableUtil.GetBuffer(fileName));
        int iRow = TableUtil.ReadHead(reader, fileName, FILE_MD5_CODE);
        for (int i = 0; i < iRow; ++i) {
            __DataName pData = __DataName.Read(reader);
            if (Contains(pData.ID()))
                throw new System.Exception("文件[" + fileName + "]有重复项 ID : " + pData.ID());
            m_dataArray.Add(pData.ID(), pData);
        }
        m_count = m_dataArray.Count;
        reader.Close();
        return this;
    }
    public __DataName GetElement(__KeyType ID) {
		if (Contains(ID)) return m_dataArray[ID];
        TableUtil.Warning("__DataName key is not exist " + ID);
		return null;
	}
    public override IData GetValue(__KeyType ID) {
		return GetElement(ID);
	}
    public override bool Contains(__KeyType ID) {
        return m_dataArray.ContainsKey(ID);
    }
	public override int Count() {
		return m_count;
	}
    public Dictionary<__KeyType, __DataName> Datas() {
		return m_dataArray;
	}
}