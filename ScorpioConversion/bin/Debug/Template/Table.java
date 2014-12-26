public class __TableName extends ITable {
	final String FILE_MD5_CODE = "__MD5";
    private String m_fileName = "";
    private int m_count = 0;
    private HashMap<__Key, __DataName> m_dataArray = new HashMap<__Key, __DataName>();
    public __TableName (String fileName) {
        m_fileName = fileName;
    }
    public void Initialize() throws Exception {
        m_dataArray.clear();
        ScorpioReader reader = new ScorpioReader(TableUtil.GetBuffer(m_fileName));
        int iRow = TableUtil.ReadHead(reader, m_fileName, FILE_MD5_CODE);
        for (int i = 0; i < iRow; ++i) {
            __DataName pData = __DataName.Read(reader);
            if (Contains(pData.ID()))
                throw new Exception("文件" + m_fileName + "有重复项 ID : " + pData.ID());
            m_dataArray.put(pData.ID(),pData);
        }
        m_count = m_dataArray.size();
        reader.Close();
    }
    public __DataName GetElement(__Key ID) {
        if (Contains(ID)) return m_dataArray.get(ID);
        TableUtil.Warning("MT_Data_Active key is not exist " + ID);
		return null;
	}
    public final HashMap<__Key, __DataName> Datas() {
		return m_dataArray;
	}
    @Override
    public boolean Contains(__Key ID) {
        return m_dataArray.containsKey(ID);
    }
    @Override
	public IData GetValue(__Key ID) {
		return GetElement(ID);
	}
    @Override
	public int Count() {
		return m_count;
	}

}