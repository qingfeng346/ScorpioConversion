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

public class TableTest implements ITable<Integer, DataTest> {
	final String FILE_MD5_CODE = "898169d7e1d0c2be013482d2c80052cc";
    private int m_count = 0;
    private HashMap<Integer, DataTest> m_dataArray = new HashMap<Integer, DataTest>();
    public TableTest Initialize(String fileName, IScorpioReader reader) {
        int iRow = TableUtil.ReadHead(reader, fileName, FILE_MD5_CODE);
        for (int i = 0; i < iRow; ++i) {
            DataTest pData = DataTest.Read(fileName, reader);
            if (m_dataArray.containsKey(pData.ID())) {
                m_dataArray.get(pData.ID()).Set(pData);
            } else {
                m_dataArray.put(pData.ID(), pData);
            }
        }
        m_count = m_dataArray.size();
        return this;
    }
    
    public DataTest GetValue(Integer ID) {
        if (m_dataArray.containsKey(ID)) return m_dataArray.get(ID);
        TableUtil.Warning("DataTest key is not exist " + ID);
        return null;
    }

    public boolean Contains(Integer ID) {
        return m_dataArray.containsKey(ID);
    }

    public final HashMap<Integer, DataTest> Datas() {
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
