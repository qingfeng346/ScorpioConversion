#ifndef __TableSpawn_H__
#define __TableSpawn_H__
#include "Commons/ScorpioReader.h"
#include "Commons/ScorpioWriter.h"
#include "Table/IData.h"
#include "Table/ITable.h"
#include "Table/TableUtil.h"
#include <unordered_map>
#include <vector>
using namespace Scorpio::Commons;
using namespace Scorpio::Table;
#include "DataSpawn.h"
namespace ScorpioProtoTest{
class TableSpawn : public ITable {
	const char * FILE_MD5_CODE = "34a59b0b8327d56e524598fdec293a9b";
    private:
        size_t m_count;
        std::unordered_map<__int32, DataSpawn*> m_dataArray;
    public: 
        TableSpawn * Initialize(char * fileName) {
            m_dataArray.clear();
            ScorpioReader * reader = new ScorpioReader(TableUtil::GetBuffer(fileName));
            int iRow = TableUtil::ReadHead(reader, fileName, FILE_MD5_CODE);
            for (int i = 0; i < iRow; ++i) {
                DataSpawn* pData = DataSpawn::Read(reader);
                if (Contains(pData->ID()))
                    throw new std::exception("文件有重复项 ID ");
                m_dataArray[pData->ID()] = pData;
            }
            reader->Close();
            delete reader;
            reader = nullptr;
            m_count = m_dataArray.size();
            return this;
        }
        DataSpawn* GetElement(__int32 ID) {
            if (Contains(ID)) return m_dataArray[ID];
            TableUtil::Warning("DataSpawn key is not exist ");
            return nullptr;
        }
        IData* GetValue(__int32 ID) {
            return GetElement(ID);
        }
        size_t Count() {
            return m_count;
        }
        bool Contains(int ID) {
            return (m_dataArray.find(ID) != m_dataArray.end());
        }
};

}
#endif
