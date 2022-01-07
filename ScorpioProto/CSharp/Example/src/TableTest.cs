//本文件为自动生成，请不要手动修改
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Scorpio.Conversion;

namespace Datas {
    public partial class TableTest : ITable {
        const string FILE_MD5_CODE = "f07d3fff17de6b37025a951b272f6c4c";
        private int m_count = 0;
        private Dictionary<int, DataTest> m_dataArray = new Dictionary<int, DataTest>();
        public TableTest Initialize(string fileName, IReader reader) {
            var row = reader.ReadInt32();
            var layoutMD5 = reader.ReadString();
            if (layoutMD5 != FILE_MD5_CODE) {
                throw new Exception("File schemas do not match [TableTest] : " + fileName);
            }
            ConversionUtil.ReadHead(reader);
            for (var i = 0; i < row; ++i) {
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
            throw new Exception($"TableTest not found data : {ID}");
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