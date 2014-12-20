using System;
using System.Collections.Generic;
public class __TableName : MT_TableBase {
	const string FILE_MD5_CODE = "__MD5";
    private string fileName = "";
    private Dictionary<__Key, __DataName> m_dataArray = new Dictionary<__Key, __DataName>();
    public __TableName (string fileName) {
        this.fileName = fileName;
    }
    public void Initialize() {
        m_dataArray.Clear();
        ScorpioReader reader = new ScorpioReader(TableUtil.GetBuffer(fileName));
        int iRow = reader.ReadInt32();                      //行数
        int iColums = reader.ReadInt32();                   //列数
        int iCodeNum = reader.ReadInt32();                  //自定义类数量
        if (reader.ReadString() != FILE_MD5_CODE)           //验证文件MD5(检测结构是否改变)
            throw new System.Exception("文件[" + fileName + "]版本验证失败");
        int i,j,index;
        for (i = 0; i < iColums; ++i) {
            index = reader.ReadInt32();                     //读取列类型
            reader.ReadInt32();                             //读取列是否是数组
            if (index == TableUtil.CLASS_VALUE) {           //如果列是自定义类
                reader.ReadString();                        //读取类名称
                for (j = 0; j < reader.ReadInt32(); ++j) {  //自定义类字段个数(全部转成基础类型以后)
                    reader.ReadInt32();                     //取出字段类型
                }
            }
        }
        for (i = 0; i < iRow; ++i) {
            __DataName pData = __DataName.Read(reader, fileName);
            if (Contains(pData.ID()))
                throw new System.Exception("文件[" + fileName + "]有重复项 ID : " + pData.ID());
            m_dataArray.Add(pData.ID(), pData);
        }
        reader.Close();
    }
    public override bool Contains(__Key ID) {
        return m_dataArray.ContainsKey(ID);
    }
	public __DataName GetElement(__Key ID) {
		if (Contains(ID))
			return m_dataArray[ID];
        TableUtil.Warning("__DataName key is not exist " + ID);
		return null;
	}
	public override __DataName GetValue(__Key ID) {
        return GetElement(ID);
	}
	public override int Count() {
		return m_dataArray.Count;
	}
	public Dictionary<__Key, __DataName> Datas() {
		return m_dataArray;
	}
}