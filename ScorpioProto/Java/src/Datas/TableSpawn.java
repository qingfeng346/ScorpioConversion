package Datas;
//本文件为自动生成，请不要手动修改
import java.util.*;
import ScorpioProto.Commons.*;
import ScorpioProto.Table.*;

public class TableSpawn implements ITable<Integer, DataSpawn> {
    final String FILE_MD5_CODE = "484cdae7d179982f1c7868078204d81d";
    private int m_count = 0;
    private HashMap<Integer, DataSpawn> m_dataArray = new HashMap<Integer, DataSpawn>();
    public TableSpawn Initialize(String fileName, IScorpioReader reader) throws Exception {
        int iRow = reader.ReadHead(fileName, FILE_MD5_CODE);
        for (int i = 0; i < iRow; ++i) {
            DataSpawn pData = DataSpawn.Read(fileName, reader);
            if (m_dataArray.containsKey(pData.ID()))
                m_dataArray.get(pData.ID()).Set(pData);
            else
                m_dataArray.put(pData.ID(), pData);
        }
        m_count = m_dataArray.size();
        return this;
    }
    public DataSpawn GetValue(Integer ID) throws Exception {
        if (m_dataArray.containsKey(ID)) 
            return m_dataArray.get(ID);
        throw new Exception("TableSpawn not found data : " + ID);
    }
    public boolean Contains(Integer ID) {
        return m_dataArray.containsKey(ID);
    }
    public final HashMap<Integer, DataSpawn> Datas() {
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