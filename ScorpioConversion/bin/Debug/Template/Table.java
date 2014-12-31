public class __TableName extends ITable {
	final String FILE_MD5_CODE = "__MD5";
    private int m_count = 0;
    private HashMap<__KeyType, __DataName> m_dataArray = new HashMap<__KeyType, __DataName>();
    public __TableName Initialize(String fileName) throws Exception {
        m_dataArray.clear();
        ScorpioReader reader = new ScorpioReader(TableUtil.GetBuffer(fileName));
        int iRow = TableUtil.ReadHead(reader, fileName, FILE_MD5_CODE);
        for (int i = 0; i < iRow; ++i) {
            __DataName pData = __DataName.Read(reader);
            if (Contains(pData.ID()))
                throw new Exception("文件" + fileName + "有重复项 ID : " + pData.ID());
            m_dataArray.put(pData.ID(),pData);
        }
        m_count = m_dataArray.size();
        reader.Close();
        return this;
    }
    public __DataName GetElement(__KeyType ID) {
        if (Contains(ID)) return m_dataArray.get(ID);
        TableUtil.Warning("MT_Data_Active key is not exist " + ID);
		return null;
	}
    public final HashMap<__KeyType, __DataName> Datas() {
		return m_dataArray;
	}
    @Override
    public boolean Contains(__KeyType ID) {
        return m_dataArray.containsKey(ID);
    }
    @Override
	public IData GetValue(__KeyType ID) {
		return GetElement(ID);
	}
    @Override
	public int Count() {
		return m_count;
	}

}