#ifndef __DataLanguage_H__
#define __DataLanguage_H__
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
class DataLanguage : public IData {
    private: bool m_IsInvalid;
    private: __int32 _Index;
    /* <summary> 索引  默认值() </summary> */
    public: __int32 getIndex() { return _Index; }
    public: __int32 ID() { return _Index; }
    private: char * _Key;
    /* <summary> 关键字  默认值() </summary> */
    public: char * getKey() { return _Key; }
    private: char * _Text;
    /* <summary> 文字  默认值() </summary> */
    public: char * getText() { return _Text; }
    public: void* GetData(char * key ) {
        if (strcmp(key, "Index") == 0) return &_Index;
        if (strcmp(key, "Key") == 0) return &_Key;
        if (strcmp(key, "Text") == 0) return &_Text;
        return nullptr;
    }
    public: bool IsInvalid() { return m_IsInvalid; }
    private: bool IsInvalid_impl() {
        if (!TableUtil::IsInvalid(_Index)) return false;
        if (!TableUtil::IsInvalid(_Key)) return false;
        if (!TableUtil::IsInvalid(_Text)) return false;
        return true;
    }
    public: static DataLanguage * Read(ScorpioReader * reader) {
        DataLanguage * ret = new DataLanguage();
        ret->_Index = reader->ReadInt32();
        ret->_Key = reader->ReadString();
        ret->_Text = reader->ReadString();
        ret->m_IsInvalid = ret->IsInvalid_impl();
        return ret;
    }
};
}
#endif
