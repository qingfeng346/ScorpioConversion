public class __TableName extends ITable {
	final String FILE_MD5_CODE = "__MD5";
    private String fileName = "";
    private HashMap<__Key, __DataName> m_dataArray = new HashMap<__Key, __DataName>();
    public __TableName (String fileName) {
        this.fileName = fileName;
    }
    public void Initialize() throws Exception {
        m_dataArray.clear();
        ScorpioReader reader = new ScorpioReader(TableUtil.GetBuffer(fileName));
        int iRow = reader.ReadInt32();                      //行数   
        if (!reader.ReadString().equals(FILE_MD5_CODE))     //验证文件MD5(检测结构是否改变)
            throw new Exception("文件" + fileName + "版本验证失败");
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
                throw new Exception("文件" + fileName + "有重复项 ID : " + pData.ID());
            m_dataArray.put(pData.ID(),pData);
        }
        reader.Close();
    }
    @Override
    public boolean Contains(Integer ID) {
        return m_dataArray.containsKey(ID);
    }
	public __DataName GetElement(Integer ID) {
		if (Contains(ID))
			return m_dataArray.get(ID);
        TableUtil.Warning("MT_Data_Active key is not exist " + ID);
		return null;
	}
    @Override
	public __DataName GetValue(Integer ID) {
		return GetElement(ID);
	}
    @Override
	public int Count() {
		return m_dataArray.size();
	}
	public final HashMap<__Key, __DataName> Datas() {
		return m_dataArray;
	}
}