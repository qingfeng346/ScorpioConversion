package Datas;
//本文件为自动生成，请不要手动修改
import java.util.*;
import ScorpioProto.Commons.*;
import ScorpioProto.Table.*;

public class TableTest implements ITable<Integer, DataTest> {
    final String FILE_MD5_CODE = "f07d3fff17de6b37025a951b272f6c4c";
    private int m_count = 0;
    private HashMap<Integer, DataTest> m_dataArray = new HashMap<Integer, DataTest>();
    public TableTest Initialize(String fileName, IScorpioReader reader) throws Exception {
        int iRow = reader.ReadHead(fileName, FILE_MD5_CODE);
        for (int i = 0; i < iRow; ++i) {
            DataTest pData = DataTest.Read(fileName, reader);
            if (m_dataArray.containsKey(pData.ID()))
                m_dataArray.get(pData.ID()).Set(pData);
            else
                m_dataArray.put(pData.ID(), pData);
        }
        m_count = m_dataArray.size();
        return this;
    }
    public DataTest GetValue(Integer ID) throws Exception {
        if (m_dataArray.containsKey(ID)) 
            return m_dataArray.get(ID);
        throw new Exception("TableTest not found data : " + ID);
    }
    public boolean Contains(Integer ID) {
        return m_dataArray.containsKey(ID);
    }
    public final HashMap<Integer, DataTest> Datas() {
        return m_dataArray;
    }
    public IData GetValueObject(Object ID) throws Exception {
        return GetValue((Integer)ID);
    }
    public boolean ContainsObject(Object ID) {
        return Contains((Integer)ID);
    }
    public int Count() {
        return m_count;
    }
}