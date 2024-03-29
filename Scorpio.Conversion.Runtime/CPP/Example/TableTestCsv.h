#ifndef ____TableTestCsv_H__
#define ____TableTestCsv_H__
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
#include "DataTestCsv.h"
namespace Datas {
    class TableTestCsv : public ITable {
        const char * FILE_MD5_CODE = "f07d3fff17de6b37025a951b272f6c4c";
    private:
        int m_count = 0;
        map<__int32, DataTestCsv*>* m_dataArray = new map<__int32, DataTestCsv*>();
    public:
        TableTestCsv * Initialize(string fileName, IReader * reader) {
            int row = reader->ReadInt32();
            if (reader->ReadString() != FILE_MD5_CODE) {
                throw new exception("File schemas do not match [TableTestCsv]");
            }
            ConversionUtil::ReadHead(reader);
            for (int i = 0; i < row; ++i) {
                DataTestCsv * pData = new DataTestCsv(fileName, reader);
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
        DataTestCsv * GetValue(__int32 ID) {
            if (m_dataArray->find(ID) != m_dataArray->end())
                return (*m_dataArray)[ID];
            throw new exception("TableTestCsv not found data : {ID}");
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
        map<__int32, DataTestCsv*>* Datas() {
            return m_dataArray;
        }
        int Count() {
            return m_count;
        }
    };
}
#endif