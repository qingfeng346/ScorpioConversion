using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Scorpio.Commons;
using Scorpio.Table;
namespace ScorpioProtoTest {
public class TableSpawn : ITable {
	const string FILE_MD5_CODE = "0ba5325ff1b00e87895cb7961961320a";
    private int m_count = 0;
    private Dictionary<int, DataSpawn> m_dataArray = new Dictionary<int, DataSpawn>();
    public TableSpawn Initialize(TableManager tableManager, string fileName) {
        m_dataArray.Clear();
        ScorpioReader reader = new ScorpioReader(TableUtil.GetBuffer(fileName));
        int iRow = TableUtil.ReadHead(reader, fileName, FILE_MD5_CODE);
        for (int i = 0; i < iRow; ++i) {
            DataSpawn pData = DataSpawn.Read(tableManager, fileName, reader);
            if (Contains(pData.ID()))
                throw new System.Exception("文件[" + fileName + "]有重复项 ID : " + pData.ID());
            m_dataArray.Add(pData.ID(), pData);
        }
        m_count = m_dataArray.Count;
        reader.Close();
        return this;
    }
    public DataSpawn GetElement(int ID) {
        if (Contains(ID)) return m_dataArray[ID];
        TableUtil.Warning("DataSpawn key is not exist " + ID);
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
    public Dictionary<int, DataSpawn> Datas() {
        return m_dataArray;
    }
}
}
