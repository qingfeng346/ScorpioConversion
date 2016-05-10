#ifndef __Int2_H__
#define __Int2_H__
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


namespace ScorpioProtoTest{
class Int2 : public IData {
    private: bool m_IsInvalid;
    private: __int32 _Value1;
    /// <summary> () </summary>
    public: __int32 getValue1() { return _Value1; }
    private: __int32 _Value2;
    /// <summary> () </summary>
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
    public: static Int2 * Read(ScorpioReader * reader) {
        Int2 * ret = new Int2();
        ret->_Value1 = reader->ReadInt32();
        ret->_Value2 = reader->ReadInt32();
        ret->m_IsInvalid = ret->IsInvalid_impl();
        return ret;
    }
};
}
#endif
