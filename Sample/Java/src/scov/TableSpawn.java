package scov;

import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import ScorpioProto.Commons.*;
import ScorpioProto.Table.*;
@SuppressWarnings("unused")

public class TableSpawn implements ITable<Integer, DataSpawn> {
	final String FILE_MD5_CODE = "484cdae7d179982f1c7868078204d81d";
    private int m_count = 0;
    private HashMap<Integer, DataSpawn> m_dataArray = new HashMap<Integer, DataSpawn>();
    public TableSpawn Initialize(String fileName, IScorpioReader reader) {
        int iRow = TableUtil.ReadHead(reader, fileName, FILE_MD5_CODE);
        for (int i = 0; i < iRow; ++i) {
            DataSpawn pData = DataSpawn.Read(fileName, reader);
            if (m_dataArray.containsKey(pData.ID())) {
                m_dataArray.get(pData.ID()).Set(pData);
            } else {
                m_dataArray.put(pData.ID(), pData);
            }
        }
        m_count = m_dataArray.size();
        return this;
    }
    
    public DataSpawn GetValue(Integer ID) {
        if (m_dataArray.containsKey(ID)) return m_dataArray.get(ID);
        TableUtil.Warning("DataSpawn key is not exist " + ID);
        return null;
    }

    public boolean Contains(Integer ID) {
        return m_dataArray.containsKey(ID);
    }

    public final HashMap<Integer, DataSpawn> Datas() {
        return m_dataArray;
    }
    
    public IData GetValueObject(Object ID) {
        return GetValue((Integer)ID);
    }

    public boolean ContainsObject(Object ID) {
        return Contains((Integer)ID);
    }

    public int Count() {
        return m_count;
    }
}
