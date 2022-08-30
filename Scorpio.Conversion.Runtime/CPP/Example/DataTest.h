#ifndef ____DataTest_H__
#define ____DataTest_H__
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

#include "Int3.h"

namespace Datas {
class DataTest : public IData {
    
    private:
        __int32 TestID;
        __int32 testEnum;
        vector<Int3*>* TestDate;
        __int64 TestDateTime;
        __int32 TestInt;
        char* TestBytes;
        string TestLanguage;
    public:
        __int32 GetID() { return TestID; }
        /* <summary> 注释  默认值() </summary> */
        __int32 GetTestID() { return TestID; }
        /* <summary>   默认值(value1) </summary> */
        __int32 GettestEnum() { return testEnum; }
        /* <summary>   默认值() </summary> */
        vector<Int3*>* GetTestDate() { return TestDate; }
        /* <summary>   默认值(2010/10/20 10:20) </summary> */
        __int64 GetTestDateTime() { return TestDateTime; }
        /* <summary>   默认值(999) </summary> */
        __int32 GetTestInt() { return TestInt; }
        /* <summary> 内容为1234567890的base64数据  默认值(base64://MTIzNDU2Nzg5MA==) </summary> */
        char* GetTestBytes() { return TestBytes; }
        /* <summary>   默认值() </summary> */
        string GetTestLanguage() { return TestLanguage; }
    
        DataTest(string fileName, IReader * reader) {
            this->TestID = reader->ReadInt32();
            this->testEnum = reader->ReadInt32();
            {
                vector<Int3*>* list = new vector<Int3*>();
                int number = reader->ReadInt32();
                for (int i = 0; i < number; ++i) { 
                    list->push_back(new Int3(fileName, reader)); 
                }
                this->TestDate = list;
            }
            this->TestDateTime = reader->ReadDateTime();
            this->TestInt = reader->ReadInt32();
            this->TestBytes = reader->ReadBytes();
            this->TestLanguage = reader->ReadL10n(fileName + ".TestLanguage." + this.ID);
        }
    
        void * GetData(string key) {
            if (key == "TestID") return &TestID;
            if (key == "testEnum") return &testEnum;
            if (key == "TestDate") return &TestDate;
            if (key == "TestDateTime") return &TestDateTime;
            if (key == "TestInt") return &TestInt;
            if (key == "TestBytes") return &TestBytes;
            if (key == "TestLanguage") return &TestLanguage;
            return nullptr;
        }
    
        void Set(DataTest * value) {
            this->TestID = value->TestID;
            this->testEnum = value->testEnum;
            this->TestDate = value->TestDate;
            this->TestDateTime = value->TestDateTime;
            this->TestInt = value->TestInt;
            this->TestBytes = value->TestBytes;
            this->TestLanguage = value->TestLanguage;
        }
    
        string ToString() {
            return "";
        }
};
}
#endif
