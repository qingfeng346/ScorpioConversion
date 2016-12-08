#ifndef __DataSpawn_H__
#define __DataSpawn_H__
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
#include "TestEnum.h"

namespace ScorpioProtoTest{
class DataSpawn : public IData {
    private: bool m_IsInvalid;
    private: __int32 _ID;
    /* <summary> 测试ID 此值必须唯一 而且必须为int型  默认值() </summary> */
    public: __int32 getID() { return _ID; }
    public: __int32 ID() { return _ID; }
    private: __int32 _TestInt;
    /* <summary> int类型  默认值() </summary> */
    public: __int32 getTestInt() { return _TestInt; }
    private: char * _TestString;
    /* <summary> string类型  默认值() </summary> */
    public: char * getTestString() { return _TestString; }
    private: bool _TestBool;
    /* <summary> bool类型  默认值() </summary> */
    public: bool getTestBool() { return _TestBool; }
    private: Int2 * _TestInt2;
    /* <summary> 自定义类型 根据ExcelConfig下 table.sco文件定义的Int2解析 类型为table_后面的名字 格式为 , 隔开  默认值() </summary> */
    public: Int2 * getTestInt2() { return _TestInt2; }
    private: TestEnum _TestEnumName;
    /* <summary> 自定义枚举  默认值() </summary> */
    public: TestEnum getTestEnumName() { return _TestEnumName; }
    public: void* GetData(char * key ) {
        if (strcmp(key, "ID") == 0) return &_ID;
        if (strcmp(key, "TestInt") == 0) return &_TestInt;
        if (strcmp(key, "TestString") == 0) return &_TestString;
        if (strcmp(key, "TestBool") == 0) return &_TestBool;
        if (strcmp(key, "TestInt2") == 0) return &_TestInt2;
        if (strcmp(key, "TestEnumName") == 0) return &_TestEnumName;
        return nullptr;
    }
    public: bool IsInvalid() { return m_IsInvalid; }
    private: bool IsInvalid_impl() {
        if (!TableUtil::IsInvalid(_ID)) return false;
        if (!TableUtil::IsInvalid(_TestInt)) return false;
        if (!TableUtil::IsInvalid(_TestString)) return false;
        if (!TableUtil::IsInvalid(_TestBool)) return false;
        if (!TableUtil::IsInvalid(_TestInt2)) return false;
        if (!TableUtil::IsInvalid(_TestEnumName)) return false;
        return true;
    }
    public: static DataSpawn * Read(ScorpioReader * reader) {
        DataSpawn * ret = new DataSpawn();
        ret->_ID = reader->ReadInt32();
        ret->_TestInt = reader->ReadInt32();
        ret->_TestString = reader->ReadString();
        ret->_TestBool = reader->ReadBool();
        ret->_TestInt2 = Int2::Read(reader);
        ret->_TestEnumName = (TestEnum)reader->ReadInt32();
        ret->m_IsInvalid = ret->IsInvalid_impl();
        return ret;
    }
};
}
#endif
