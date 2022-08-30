#ifndef ____TableTest_H__
#define ____TableTest_H__
//本文件为自动生成，请不要手动修改
#include <IReader.h>
#include <IData.h>
#include <ITable.h>
#include <ConversionUtil.h>
#include <map>
#include <vector>
#include <string>
using namespace std;
using namespace Scorpio::Conversion::Runtime;
#include "DataTest.h"
namespace Datas {
    class TableTest : public ITable {
        const char * FILE_MD5_CODE = "09fce78ed0fbcdd2f1806a9c3567245d";
    private:
        int m_count = 0;
        map<__int32, DataTest*>* m_dataArray = new map<__int32, DataTest*>();
    public:
        TableTest * Initialize(string fileName, IReader * reader) {
            int row = reader->ReadInt32();
            if (reader->ReadString() != FILE_MD5_CODE) {
                throw new exception("File schemas do not match [TableTest]");
            }
            ConversionUtil::ReadHead(reader);
            for (int i = 0; i < row; ++i) {
                DataTest * pData = new DataTest(fileName, reader);
                if (Contains(pData->GetID())) {
                    (*m_dataArray)[pData->GetID()]->Set(pData);
                    delete pData;
                    pData = nullptr;
                } else {
                    (*m_dataArray)[pData->GetID()] = pData;
                }
            }
            m_count = m_dataArray->size();
            return this;
        }
        DataTest * GetValue(__int32 ID) {
            if (m_dataArray->find(ID) != m_dataArray->end())
                return (*m_dataArray)[ID];
            throw new exception("TableTest not found data : {ID}");
        }
        bool Contains(__int32 ID) {
            return m_dataArray->find(ID) != m_dataArray->end();
        }
        IData * GetValueObject(void * ID) {
            return GetValue(*(__int32*)ID);
        }
        bool ContainsObject(void * ID) {
            return Contains(*(__int32*)ID);
        }
        map<__int32, DataTest*>* Datas() {
            return m_dataArray;
        }
        int Count() {
            return m_count;
        }
    };
}
#endif