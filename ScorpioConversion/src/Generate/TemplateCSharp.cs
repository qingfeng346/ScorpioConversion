using System;
using System.Collections.Generic;
using System.Text;
public class TemplateCSharp {
    public const string Head = @"using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Scorpio.Commons;
using Scorpio.Table;
";
    public const string Table = @"public class __TableName : TableBase<__KeyType, __DataName> {
	const string FILE_MD5_CODE = ""__MD5"";
    private int m_count = 0;
    private Dictionary<__KeyType, __DataName> m_dataArray = new Dictionary<__KeyType, __DataName>();
    public __TableName Initialize(Dictionary<string, string> l10n, string fileName) {
        using (var reader = new ScorpioReader(TableUtil.GetBuffer(fileName))) {
            int iRow = TableUtil.ReadHead(reader, fileName, FILE_MD5_CODE);
            for (var i = 0; i < iRow; ++i) {
                var pData = __DataName.Read(l10n, fileName, reader);
                if (m_dataArray.ContainsKey(pData.ID())) {
                    m_dataArray[pData.ID()].Set(pData);
                } else {
                    m_dataArray.Add(pData.ID(), pData);
                }
            }
            m_count = m_dataArray.Count;
            return this;
        }
    }
    public IData GetValue(__KeyType ID) {
        if (m_dataArray.ContainsKey(ID)) return m_dataArray[ID];
        TableUtil.Warning(""__DataName key is not exist "" + ID);
        return null;
    }
    public bool Contains(__KeyType ID) {
        return m_dataArray.ContainsKey(ID);
    }
    public Dictionary<__KeyType, __DataName> Datas() {
        return m_dataArray;
    }
    
    public IData GetValueObject(object ID) {
        return GetValue(ID as __KeyType);
    }
    public bool ContainsObject(object ID) {
        return Contains(ID as __KeyType);
    }
    public IDictionary GetDatas() {
        return Datas();
    }
    public int Count() {
        return m_count;
    }
}
";
}