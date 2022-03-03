#ifndef ____Int3_H__
#define ____Int3_H__
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
class Int3 : public IData {
    
    private:
        vector<Int2*>* Value1;
        __int32 Value2;
    public:
        /* <summary>   默认值() </summary> */
        vector<Int2*>* GetValue1() { return Value1; }
        /* <summary>   默认值() </summary> */
        __int32 GetValue2() { return Value2; }
    
        Int3(string fileName, IReader * reader) {
            {
                vector<Int2*>* list = new vector<Int2*>();
                int number = reader->ReadInt32();
                for (int i = 0; i < number; ++i) { 
                    list->push_back(new Int2(fileName, reader)); 
                }
                this->Value1 = list;
            }
            this->Value2 = reader->ReadInt32();
        }
    
        void * GetData(string key) {
            if (key == "Value1") return &Value1;
            if (key == "Value2") return &Value2;
            return nullptr;
        }
    
        void Set(Int3 * value) {
            this->Value1 = value->Value1;
            this->Value2 = value->Value2;
        }
    
        string ToString() {
            return "";
        }
};
}
#endif
