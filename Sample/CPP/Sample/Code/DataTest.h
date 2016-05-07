#ifndef __DataTest_H__
#define __DataTest_H__
#include "Commons/ScorpioReader.h"
#include "Commons/ScorpioWriter.h"
#include "Table/IData.h"
#include "Table/ITable.h"
#include "Table/TableUtil.h"
#include <unordered_map>
#include <vector>
using namespace Scorpio::Commons;
using namespace Scorpio::Table;
#include "Int2.h"
#include "TestEnum.h"
#include "Int3.h"

namespace ScorpioProtoTest{
class DataTest : public IData {
    private: bool m_IsInvalid;
    private: __int32 _ID;
    /// <summary> 测试ID 此值必须唯一 而且必须为int型() </summary>
    public: __int32 getID() { return _ID; }
    public: __int32 ID() { return _ID; }
    private: __int32 _TestInt;
    /// <summary> int类型(20) </summary>
    public: __int32 getTestInt() { return _TestInt; }
    private: char * _TestString;
    /// <summary> string类型(aaa) </summary>
    public: char * getTestString() { return _TestString; }
    private: bool _TestBool;
    /// <summary> bool类型() </summary>
    public: bool getTestBool() { return _TestBool; }
    private: Int2 * _TestInt2;
    /// <summary> 自定义类型 根据ExcelConfig下 table.sco文件定义的Int2解析 类型为table_后面的名字 格式为 , 隔开() </summary>
    public: Int2 * getTestInt2() { return _TestInt2; }
    private: TestEnum _TestEnumName;
    /// <summary> 自定义枚举() </summary>
    public: TestEnum getTestEnumName() { return _TestEnumName; }
    private: std::vector<__int32> _TestArray;
    /// <summary> array类型 以逗号隔开() </summary>
    public: std::vector<__int32> getTestArray() { return _TestArray; }
    private: std::vector<Int2 *> _TestArray2;
    /// <summary> array类型 自定义类型 每一个中括号为一个单位() </summary>
    public: std::vector<Int2 *> getTestArray2() { return _TestArray2; }
    private: Int3 * _TestInt3;
    /// <summary> 嵌套类型() </summary>
    public: Int3 * getTestInt3() { return _TestInt3; }
    public: void* GetData(char * key ) {
        if (strcmp(key, "ID") == 0) return &_ID;
        if (strcmp(key, "TestInt") == 0) return &_TestInt;
        if (strcmp(key, "TestString") == 0) return &_TestString;
        if (strcmp(key, "TestBool") == 0) return &_TestBool;
        if (strcmp(key, "TestInt2") == 0) return &_TestInt2;
        if (strcmp(key, "TestEnumName") == 0) return &_TestEnumName;
        if (strcmp(key, "TestArray") == 0) return &_TestArray;
        if (strcmp(key, "TestArray2") == 0) return &_TestArray2;
        if (strcmp(key, "TestInt3") == 0) return &_TestInt3;
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
        if (!TableUtil::IsInvalid(_TestArray)) return false;
        if (!TableUtil::IsInvalid(_TestArray2)) return false;
        if (!TableUtil::IsInvalid(_TestInt3)) return false;
        return true;
    }
    public: static DataTest * Read(ScorpioReader * reader) {
        DataTest * ret = new DataTest();
        ret->_ID = reader->ReadInt32();
        ret->_TestInt = reader->ReadInt32();
        ret->_TestString = reader->ReadString();
        ret->_TestBool = reader->ReadBool();
        ret->_TestInt2 = Int2::Read(reader);
        ret->_TestEnumName = (TestEnum)reader->ReadInt32();
        {
            int number = reader->ReadInt32();
            for (int i = 0;i < number; ++i) { ret->_TestArray.push_back(reader->ReadInt32()); }
        }
        {
            int number = reader->ReadInt32();
            for (int i = 0;i < number; ++i) { ret->_TestArray2.push_back(Int2::Read(reader)); }
        }
        ret->_TestInt3 = Int3::Read(reader);
        ret->m_IsInvalid = ret->IsInvalid_impl();
        return ret;
    }
};
}
#endif
