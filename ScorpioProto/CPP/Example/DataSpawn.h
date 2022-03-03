#ifndef ____DataSpawn_H__
#define ____DataSpawn_H__
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

#include "Int2.h"

namespace Datas {
class DataSpawn : public IData {
    
    private:
        __int32 ID;
        __int32 TestInt;
        string TestString;
        string TestLanguage;
        bool TestBool;
        Int2* TestInt2;
        __int32 TestEnumName;
    public:
        /* <summary> 测试ID 此值必须唯一 而且必须为int型  默认值() </summary> */
        __int32 GetID() { return ID; }
        /* <summary> int类型  默认值() </summary> */
        __int32 GetTestInt() { return TestInt; }
        /* <summary> string类型  默认值() </summary> */
        string GetTestString() { return TestString; }
        /* <summary> 测试多国语言  默认值() </summary> */
        string GetTestLanguage() { return TestLanguage; }
        /* <summary> bool类型  默认值() </summary> */
        bool GetTestBool() { return TestBool; }
        /* <summary> 自定义类型 根据ExcelConfig下 table.sco文件定义的Int2解析 类型为table_后面的名字 格式为 , 隔开  默认值() </summary> */
        Int2* GetTestInt2() { return TestInt2; }
        /* <summary> 自定义枚举  默认值() </summary> */
        __int32 GetTestEnumName() { return TestEnumName; }
    
        DataSpawn(string fileName, IReader * reader) {
            this->ID = reader->ReadInt32();
            this->TestInt = reader->ReadInt32();
            this->TestString = reader->ReadString();
            delete this->TestString.c_str();            this->TestLanguage = reader->ReadString();
            this->TestBool = reader->ReadBool();
            this->TestInt2 = new Int2(fileName, reader);
            this->TestEnumName = reader->ReadInt32();
        }
    
        void * GetData(string key) {
            if (key == "ID") return &ID;
            if (key == "TestInt") return &TestInt;
            if (key == "TestString") return &TestString;
            if (key == "TestLanguage") return &TestLanguage;
            if (key == "TestBool") return &TestBool;
            if (key == "TestInt2") return &TestInt2;
            if (key == "TestEnumName") return &TestEnumName;
            return nullptr;
        }
    
        void Set(DataSpawn * value) {
            this->ID = value->ID;
            this->TestInt = value->TestInt;
            this->TestString = value->TestString;
            this->TestLanguage = value->TestLanguage;
            this->TestBool = value->TestBool;
            this->TestInt2 = value->TestInt2;
            this->TestEnumName = value->TestEnumName;
        }
    
        string ToString() {
            return "";
        }
};
}
#endif
