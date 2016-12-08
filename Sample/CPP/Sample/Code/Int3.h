#ifndef __Int3_H__
#define __Int3_H__
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

#include "Int2.h"

namespace ScorpioProtoTest{
class Int3 : public IData {
    private: bool m_IsInvalid;
    private: std::vector<Int2 *> _Value1;
    /* <summary>   默认值() </summary> */
    public: std::vector<Int2 *> getValue1() { return _Value1; }
    private: __int32 _Value2;
    /* <summary>   默认值() </summary> */
    public: __int32 getValue2() { return _Value2; }
    public: void* GetData(char * key ) {
        if (strcmp(key, "Value1") == 0) return &_Value1;
        if (strcmp(key, "Value2") == 0) return &_Value2;
        return nullptr;
    }
    public: bool IsInvalid() { return m_IsInvalid; }
    private: bool IsInvalid_impl() {
        if (!TableUtil::IsInvalid(_Value1)) return false;
        if (!TableUtil::IsInvalid(_Value2)) return false;
        return true;
    }
    public: static Int3 * Read(ScorpioReader * reader) {
        Int3 * ret = new Int3();
        {
            int number = reader->ReadInt32();
            for (int i = 0;i < number; ++i) { ret->_Value1.push_back(Int2::Read(reader)); }
        }
        ret->_Value2 = reader->ReadInt32();
        ret->m_IsInvalid = ret->IsInvalid_impl();
        return ret;
    }
};
}
#endif
