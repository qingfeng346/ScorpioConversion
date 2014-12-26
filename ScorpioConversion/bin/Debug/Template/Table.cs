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
        int iRow = reader.ReadInt32();                      //行数
        if (reader.ReadString() != FILE_MD5_CODE)           //验证文件MD5(检测结构是否改变)
            throw new System.Exception("文件[" + m_fileName + "]版本验证失败");
        int i,number;
        {
            number = reader.ReadInt32();        //字段数量
            for (i = 0; i < number; ++i) {
                if (reader.ReadInt8() == 0) {   //基础类型
                    reader.ReadInt8();          //基础类型索引
                    reader.ReadBool();          //是否是数组
                } else {                        //自定义类
                    reader.ReadString();        //自定义类名称
                    reader.ReadBool();          //是否是数组
                }
            }
        }
        int customNumber = reader.ReadInt32();  //自定义类数量
        for (i = 0; i < customNumber; ++i) {
            reader.ReadString();                //读取自定义类名字
            number = reader.ReadInt32();        //字段数量
            for (i = 0; i < number; ++i) {
                if (reader.ReadInt8() == 0) {   //基础类型
                    reader.ReadInt8();          //基础类型索引
                    reader.ReadBool();          //是否是数组
                } else {                        //自定义类
                    reader.ReadString();        //自定义类名称
                    reader.ReadBool();          //是否是数组
                }
            }
        }
        for (i = 0; i < iRow; ++i) {
            __DataName pData = __DataName.Read(reader);
            if (Contains(pData.ID()))
                throw new System.Exception("文件[" + m_fileName + "]有重复项 ID : " + pData.ID());
            m_dataArray.Add(pData.ID(), pData);
        }
        m_count = m_dataArray.Count;
        reader.Close();
    }
    public override bool Contains(__Key ID) {
        return m_dataArray.ContainsKey(ID);
    }
	public override __DataName GetValue(__Key ID) {
		if (Contains(ID)) return m_dataArray[ID];
        TableUtil.Warning("__DataName key is not exist " + ID);
		return null;
	}
	public override int Count() {
		return m_count;
	}
	public Dictionary<__Key, __DataName> Datas() {
		return m_dataArray;
	}
}