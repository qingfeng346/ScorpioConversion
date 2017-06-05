#ifndef ____TableManager_H__
#define ____TableManager_H__
#include "Commons/ScorpioReader.h"
#include "Commons/ScorpioWriter.h"
#include "Table/IData.h"
#include "Table/ITable.h"
#include "Table/TableUtil.h"
#include "Message/IMessage.h"
#include <unordered_map>
#include <vector>
using namespace Scorpio::Commons;
using namespace Scorpio::Table;
using namespace Scorpio::Message;

#include "TableTest.h"
#include "TableLanguage.h"
#include "TableSpawn.h"
namespace ScorpioProtoTest{

class TableManager {
    public: void Reset() {
        delete mTest; mTest = nullptr;
        LanguageArray.clear();
        SpawnArray.clear();
    }
    private: TableTest * mTest = nullptr;
    public: TableTest * GetTest() { if (mTest == nullptr) { mTest = new TableTest(); mTest->Initialize("Test"); } return mTest; }
    private: std::unordered_map<char*, TableLanguage*> LanguageArray;
    private: TableLanguage * GetSpawns_Language(char * key) {
        if (LanguageArray.find(key) != LanguageArray.end())
            return LanguageArray[key];
        TableLanguage * data = new TableLanguage();
        data->Initialize(key);
        return LanguageArray[key] = data;
    }
    private: std::unordered_map<char*, TableSpawn*> SpawnArray;
    private: TableSpawn * GetSpawns_Spawn(char * key) {
        if (SpawnArray.find(key) != SpawnArray.end())
            return SpawnArray[key];
        TableSpawn * data = new TableSpawn();
        data->Initialize(key);
        return SpawnArray[key] = data;
    }
};
}
#endif