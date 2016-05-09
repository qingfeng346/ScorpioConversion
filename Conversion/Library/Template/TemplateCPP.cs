using System;
using System.Collections.Generic;
using System.Text;

public class TemplateCPP {
    public const string Head = @"#include ""Commons/ScorpioReader.h""
#include ""Commons/ScorpioWriter.h""
#include ""Table/IData.h""
#include ""Table/ITable.h""
#include ""Table/TableUtil.h""
#include ""Message/IMessage.h""
#include <unordered_map>
#include <vector>
using namespace Scorpio::Commons;
using namespace Scorpio::Table;
using namespace Scorpio::Message;
";
    public const string Table = @"class __TableName : public ITable {
	const char * FILE_MD5_CODE = ""__MD5"";
    private:
        size_t m_count;
        std::unordered_map<__KeyType, __DataName*> m_dataArray;
    public: 
        __TableName * Initialize(char * fileName) {
            m_dataArray.clear();
            ScorpioReader * reader = new ScorpioReader(TableUtil::GetBuffer(fileName));
            int iRow = TableUtil::ReadHead(reader, fileName, FILE_MD5_CODE);
            for (int i = 0; i < iRow; ++i) {
                __DataName* pData = __DataName::Read(reader);
                if (Contains(pData->ID()))
                    throw new std::exception(""文件有重复项 ID "");
                m_dataArray[pData->ID()] = pData;
            }
            reader->Close();
            delete reader;
            reader = nullptr;
            m_count = m_dataArray.size();
            return this;
        }
        __DataName* GetElement(__KeyType ID) {
            if (Contains(ID)) return m_dataArray[ID];
            TableUtil::Warning(""__DataName key is not exist "");
            return nullptr;
        }
        IData* GetValue(__KeyType ID) {
            return GetElement(ID);
        }
        size_t Count() {
            return m_count;
        }
        bool Contains(int ID) {
            return (m_dataArray.find(ID) != m_dataArray.end());
        }
};
";
}
