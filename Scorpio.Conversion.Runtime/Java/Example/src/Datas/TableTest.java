package Datas;
//本文件为自动生成，请不要手动修改
import java.util.*;
import Scorpio.Conversion.Runtime.*;

public class TableTest implements ITable {
    final String FILE_MD5_CODE = "09fce78ed0fbcdd2f1806a9c3567245d";
    private int m_count = 0;
    private HashMap<Integer, DataTest> m_dataArray = new HashMap<Integer, DataTest>();
    public TableTest Initialize(String fileName, IReader reader) throws Exception {
        int row = reader.ReadInt32();
        if (!FILE_MD5_CODE.equals(reader.ReadString())) {
            throw new Exception("File schemas do not match [TableTest] : " + fileName);
        }
        ConversionUtil.ReadHead(reader);
        for (int i = 0; i < row; ++i) {
            DataTest pData = new DataTest(fileName, reader);
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