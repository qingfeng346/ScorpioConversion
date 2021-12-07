//本文件为自动生成，请不要手动修改
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ScorpioProto.Commons;
using ScorpioProto.Table;

namespace Datas {
    public partial class TableTest : ITable {
        const string FILE_MD5_CODE = "f07d3fff17de6b37025a951b272f6c4c";
        private int m_count = 0;
        private Dictionary<int, DataTest> m_dataArray = new Dictionary<int, DataTest>();
        public TableTest Initialize(string fileName, IScorpioReader reader) {
            var iRow = TableUtil.ReadHead(reader, fileName, FILE_MD5_CODE);
            for (var i = 0; i < iRow; ++i) {
                var pData = DataTest.Read(fileName, reader);
                if (m_dataArray.TryGetValue(pData.ID(), out var value))
                    value.Set(pData);
                else
                    m_dataArray[pData.ID()] = pData;
            }
            m_count = m_dataArray.Count;
            return this;
        }
        public DataTest GetValue(int ID) {
            if (m_dataArray.TryGetValue(ID, out var value))
                return value;
            TableUtil.Warning("TableTest key is not exist " + ID);
            return null;
        }
        public bool Contains(int ID) {
            return m_dataArray.ContainsKey(ID);
        }
        public Dictionary<int, DataTest> Datas() {
            return m_dataArray;
        }
        public IData GetValueObject(object ID) {
            return GetValue((int)ID);
        }
        public bool ContainsObject(object ID) {
            return Contains((int)ID);
        }
        public IDictionary GetDatas() {
            return Datas();
        }
        public int Count() {
            return m_count;
        }
    }
}