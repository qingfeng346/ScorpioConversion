using System;
using System.Collections.Generic;
using System.Text;

public class TemplateJava {
    public const string Head = @"import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import Scorpio.Commons.*;
import Scorpio.Table.*;
@SuppressWarnings(""unused"")
";
    public const string Table = @"public class __TableName extends ITable {
	final String FILE_MD5_CODE = ""__MD5"";
    private int m_count = 0;
    private HashMap<__KeyType, __DataName> m_dataArray = new HashMap<__KeyType, __DataName>();
    public __TableName Initialize(TableManager tableManager, String fileName) {
        m_dataArray.clear();
        ScorpioReader reader = new ScorpioReader(TableUtil.GetBuffer(fileName));
        int iRow = TableUtil.ReadHead(reader, fileName, FILE_MD5_CODE);
        for (int i = 0; i < iRow; ++i) {
            __DataName pData = __DataName.Read(tableManager, fileName, reader);
            if (m_dataArray.containsKey(pData.ID()))
                throw new RuntimeException(""文件"" + fileName + ""有重复项 ID : "" + pData.ID());
            m_dataArray.put(pData.ID(), pData);
        }
        m_count = m_dataArray.size();
        reader.Close();
        return this;
    }
    public __DataName GetElement(__KeyType ID) {
        if (Contains(ID)) return m_dataArray.get(ID);
        TableUtil.Warning(""__DataName key is not exist "" + ID);
        return null;
    }
    @Override
    public IData GetValue(__KeyType ID) {
        return GetElement(ID);
    }
    @Override
    public boolean Contains(__KeyType ID) {
        return m_dataArray.containsKey(ID);
    }
    @Override
    public int Count() {
        return m_count;
    }
    public final HashMap<__KeyType, __DataName> Datas() {
        return m_dataArray;
    }
}
";
}
