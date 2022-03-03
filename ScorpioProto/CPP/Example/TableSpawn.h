#ifndef ____TableSpawn_H__
#define ____TableSpawn_H__
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
#include "DataSpawn.h"
namespace Datas {
    class TableSpawn : public ITable {
        const char * FILE_MD5_CODE = "484cdae7d179982f1c7868078204d81d";
    private:
        int m_count = 0;
        map<__int32, DataSpawn*>* m_dataArray = new map<__int32, DataSpawn*>();
    public:
        TableSpawn * Initialize(string fileName, IReader * reader) {
            int row = reader->ReadInt32();
            if (reader->ReadString() != FILE_MD5_CODE) {
                throw new exception("File schemas do not match [TableSpawn]");
            }
            ConversionUtil::ReadHead(reader);
            for (int i = 0; i < row; ++i) {
                DataSpawn * pData = new DataSpawn(fileName, reader);
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
        DataSpawn * GetValue(__int32 ID) {
            if (m_dataArray->find(ID) != m_dataArray->end())
                return (*m_dataArray)[ID];
            throw new exception("TableSpawn not found data : {ID}");
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
        map<__int32, DataSpawn*>* Datas() {
            return m_dataArray;
        }
        int Count() {
            return m_count;
        }
    };
}
#endif