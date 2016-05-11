#ifndef __Msg_C2G_Test3_H__
#define __Msg_C2G_Test3_H__
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
#include "Msg_C2G_Test.h"
#include "Msg_C2G_Test2.h"
#include "TestEnum.h"

namespace ScorpioProtoTest{
//本文件为自动生成，请不要手动修改
class Msg_C2G_Test3 : public IMessage {
    private: std::vector<Msg_C2G_Test *> _Value1;
    public: std::vector<Msg_C2G_Test *> getValue1() { return _Value1; }
    public: Msg_C2G_Test3 * setValue1(std::vector<Msg_C2G_Test *> value) { _Value1 = value; AddSign(1); return this; } 
    private: Msg_C2G_Test2 * _Value2;
    public: Msg_C2G_Test2 * getValue2() { return _Value2; }
    public: Msg_C2G_Test3 * setValue2(Msg_C2G_Test2 * value) { _Value2 = value; AddSign(2); return this; } 
    private: TestEnum _Value3;
    public: TestEnum getValue3() { return _Value3; }
    public: Msg_C2G_Test3 * setValue3(TestEnum value) { _Value3 = value; AddSign(3); return this; } 
    private: std::vector<TestEnum> _Value4;
    public: std::vector<TestEnum> getValue4() { return _Value4; }
    public: Msg_C2G_Test3 * setValue4(std::vector<TestEnum> value) { _Value4 = value; AddSign(4); return this; } 
    public: ~Msg_C2G_Test3() {
        for (size_t i = 0;i < _Value1.size(); ++i) {
            delete _Value1[i];
        }
        _Value1.clear();
        delete _Value2; _Value2 = nullptr; 
    }
    public: void Write(ScorpioWriter * writer) {
        writer->WriteInt32(__Sign);
        if (HasSign(1)) {
            writer->WriteInt32(static_cast<__int32>(_Value1.size()));
            for (size_t i = 0;i < _Value1.size(); ++i) { _Value1[i]->Write(writer); }
        }
        if (HasSign(2)) { _Value2->Write(writer); }
        if (HasSign(3)) { writer->WriteInt32((int)_Value3); }
        if (HasSign(4)) {
            writer->WriteInt32(static_cast<__int32>(_Value4.size()));
            for (size_t i = 0;i < _Value4.size(); ++i) { writer->WriteInt32((int)_Value4[i]); }
        }
    }
    public: static Msg_C2G_Test3 * Read(ScorpioReader * reader) {
        Msg_C2G_Test3 * ret = new Msg_C2G_Test3();
        ret->__Sign = reader->ReadInt32();
        if (ret->HasSign(1)) {
            int number = reader->ReadInt32();
            ret->_Value1 = std::vector<Msg_C2G_Test *>();
            for (int i = 0;i < number; ++i) { ret->_Value1.push_back(Msg_C2G_Test::Read(reader)); }
        }
        if (ret->HasSign(2)) { ret->_Value2 = Msg_C2G_Test2::Read(reader); }
        if (ret->HasSign(3)) { ret->_Value3 = (TestEnum)reader->ReadInt32(); }
        if (ret->HasSign(4)) {
            int number = reader->ReadInt32();
            ret->_Value4 = std::vector<TestEnum>();
            for (int i = 0;i < number; ++i) { ret->_Value4.push_back((TestEnum)reader->ReadInt32()); }
        }
        return ret;
    }
    public: static Msg_C2G_Test3 * Deserialize(char * data) {
        ScorpioReader * reader = new ScorpioReader(data);
        Msg_C2G_Test3 * ret = Read(reader);
        reader->Close();
        delete reader;
        reader = nullptr;
        return ret;
    }
};
}
#endif