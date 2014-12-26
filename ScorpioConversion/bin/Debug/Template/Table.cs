public class __TableName : ITable {
	const string FILE_MD5_CODE = "__MD5";
    private string m_fileName = "";
    private int m_count = 0;
    private Dictionary<__Key, __DataName> m_dataArray = new Dictionary<__Key, __DataName>();
    public __TableName (string fileName) {
        m_fileName = fileName;
    }
    public void Initialize() {
        m_dataArray.Clear();
        ScorpioReader reader = new ScorpioReader(TableUtil.GetBuffer(m_fileName));
        int iRow = TableUtil.ReadHead(reader, m_fileName, FILE_MD5_CODE);
        for (int i = 0; i < iRow; ++i) {
            __DataName pData = __DataName.Read(reader);
            if (Contains(pData.ID()))
                throw new System.Exception("文件[" + m_fileName + "]有重复项 ID : " + pData.ID());
            m_dataArray.Add(pData.ID(), pData);
        }
        m_count = m_dataArray.Count;
        reader.Close();
    }
    public __DataName GetElement(__Key ID) {
		if (Contains(ID)) return m_dataArray[ID];
        TableUtil.Warning("__DataName key is not exist " + ID);
		return null;
	}
    public Dictionary<__Key, __DataName> Datas() {
		return m_dataArray;
	}
    public override bool Contains(__Key ID) {
        return m_dataArray.ContainsKey(ID);
    }
	public override IData GetValue(__Key ID) {
		return GetElement(ID);
	}
	public override int Count() {
		return m_count;
	}
}