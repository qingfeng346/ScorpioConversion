package Datas;
//本文件为自动生成，请不要手动修改
import java.util.*;
import Scorpio.Conversion.Runtime.*;

public class TableTestCsv implements ITable {
    final String FILE_MD5_CODE = "f07d3fff17de6b37025a951b272f6c4c";
    private int m_count = 0;
    private HashMap<Integer, DataTestCsv> m_dataArray = new HashMap<Integer, DataTestCsv>();
    public TableTestCsv Initialize(String fileName, IReader reader) throws Exception {
        int row = reader.ReadInt32();
        if (!FILE_MD5_CODE.equals(reader.ReadString())) {
            throw new Exception("File schemas do not match [TableTestCsv] : " + fileName);
        }
        ConversionUtil.ReadHead(reader);
        for (int i = 0; i < row; ++i) {
            DataTestCsv pData = new DataTestCsv(fileName, reader);
            if (m_dataArray.containsKey(pData.ID()))
                m_dataArray.get(pData.ID()).Set(pData);
            else
                m_dataArray.put(pData.ID(), pData);
        }
        m_count = m_dataArray.size();
        return this;
    }
    public DataTestCsv GetValue(Integer ID) throws Exception {
        if (m_dataArray.containsKey(ID)) 
            return m_dataArray.get(ID);
        throw new Exception("TableTestCsv not found data : " + ID);
    }
    public boolean Contains(Integer ID) {
        return m_dataArray.containsKey(ID);
    }
    public final HashMap<Integer, DataTestCsv> Datas() {
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