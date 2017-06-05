package ScorpioProtoTest;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import Scorpio.Commons.*;
import Scorpio.Table.*;
@SuppressWarnings("unused")
public class TableSpawn extends ITable {
	final String FILE_MD5_CODE = "0ba5325ff1b00e87895cb7961961320a";
    private int m_count = 0;
    private HashMap<Integer, DataSpawn> m_dataArray = new HashMap<Integer, DataSpawn>();
    public TableSpawn Initialize(TableManager tableManager, String fileName) {
        m_dataArray.clear();
        ScorpioReader reader = new ScorpioReader(TableUtil.GetBuffer(fileName));
        int iRow = TableUtil.ReadHead(reader, fileName, FILE_MD5_CODE);
        for (int i = 0; i < iRow; ++i) {
            DataSpawn pData = DataSpawn.Read(tableManager, fileName, reader);
            if (m_dataArray.containsKey(pData.ID()))
                throw new RuntimeException("文件" + fileName + "有重复项 ID : " + pData.ID());
            m_dataArray.put(pData.ID(), pData);
        }
        m_count = m_dataArray.size();
        reader.Close();
        return this;
    }
    public DataSpawn GetElement(Integer ID) {
        if (Contains(ID)) return m_dataArray.get(ID);
        TableUtil.Warning("DataSpawn key is not exist " + ID);
        return null;
    }
    @Override
    public IData GetValue(Integer ID) {
        return GetElement(ID);
    }
    @Override
    public boolean Contains(Integer ID) {
        return m_dataArray.containsKey(ID);
    }
    @Override
    public int Count() {
        return m_count;
    }
    public final HashMap<Integer, DataSpawn> Datas() {
        return m_dataArray;
    }
}
