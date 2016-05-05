package scorpiogame.proto;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import Scorpio.Commons.*;
import Scorpio.Table.*;
@SuppressWarnings("unused")
public class TableTest extends ITable {
	final String FILE_MD5_CODE = "a3c72d072e44d2c473850e1cd61b0e24";
    private int m_count = 0;
    private HashMap<Integer, DataTest> m_dataArray = new HashMap<Integer, DataTest>();
    public TableTest Initialize(String fileName) {
        m_dataArray.clear();
        ScorpioReader reader = new ScorpioReader(TableUtil.GetBuffer(fileName));
        int iRow = TableUtil.ReadHead(reader, fileName, FILE_MD5_CODE);
        for (int i = 0; i < iRow; ++i) {
            DataTest pData = DataTest.Read(reader);
            if (m_dataArray.containsKey(pData.ID()))
                throw new RuntimeException("文件" + fileName + "有重复项 ID : " + pData.ID());
            m_dataArray.put(pData.ID(), pData);
        }
        m_count = m_dataArray.size();
        reader.Close();
        return this;
    }
    public DataTest GetElement(Integer ID) {
        if (Contains(ID)) return m_dataArray.get(ID);
        TableUtil.Warning("DataTest key is not exist " + ID);
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
    public final HashMap<Integer, DataTest> Datas() {
        return m_dataArray;
    }
}
