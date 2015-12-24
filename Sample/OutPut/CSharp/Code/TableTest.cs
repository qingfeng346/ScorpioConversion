using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Scorpio.Commons;
using Scorpio.Table;
namespace scorpiogame.proto {
public class TableTest : ITable {
	const string FILE_MD5_CODE = "34a59b0b8327d56e524598fdec293a9b";
    private int m_count = 0;
    private Dictionary<int, DataTest> m_dataArray = new Dictionary<int, DataTest>();
    public TableTest Initialize(string fileName) {
        m_dataArray.Clear();
        ScorpioReader reader = new ScorpioReader(TableUtil.GetBuffer(fileName));
        int iRow = TableUtil.ReadHead(reader, fileName, FILE_MD5_CODE);
        for (int i = 0; i < iRow; ++i) {
            DataTest pData = DataTest.Read(reader);
            if (Contains(pData.ID()))
                throw new System.Exception("文件[" + fileName + "]有重复项 ID : " + pData.ID());
            m_dataArray.Add(pData.ID(), pData);
        }
        m_count = m_dataArray.Count;
        reader.Close();
        return this;
    }
    public DataTest GetElement(int ID) {
		if (Contains(ID)) return m_dataArray[ID];
        TableUtil.Warning("DataTest key is not exist " + ID);
		return null;
	}
    public override IData GetValue(int ID) {
		return GetElement(ID);
	}
    public override bool Contains(int ID) {
        return m_dataArray.ContainsKey(ID);
    }
	public override int Count() {
		return m_count;
	}
    public Dictionary<int, DataTest> Datas() {
		return m_dataArray;
	}
}
}