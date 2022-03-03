#ifndef ____Int2_H__
#define ____Int2_H__
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

namespace Datas {
class Int2 : public IData {
    
    private:
        __int32 Value1;
        __int32 Value2;
    public:
        /* <summary>   默认值() </summary> */
        __int32 GetValue1() { return Value1; }
        /* <summary>   默认值() </summary> */
        __int32 GetValue2() { return Value2; }
    
        Int2(string fileName, IReader * reader) {
            this->Value1 = reader->ReadInt32();
            this->Value2 = reader->ReadInt32();
        }
    
        void * GetData(string key) {
            if (key == "Value1") return &Value1;
            if (key == "Value2") return &Value2;
            return nullptr;
        }
    
        void Set(Int2 * value) {
            this->Value1 = value->Value1;
            this->Value2 = value->Value2;
        }
    
        string ToString() {
            return "";
        }
};
}
#endif
