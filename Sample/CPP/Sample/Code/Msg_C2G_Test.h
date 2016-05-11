#ifndef __Msg_C2G_Test_H__
#define __Msg_C2G_Test_H__
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
//本文件为自动生成，请不要手动修改
class Msg_C2G_Test : public IMessage {
    private: __int32 _Value1;
    public: __int32 getValue1() { return _Value1; }
    public: Msg_C2G_Test * setValue1(__int32 value) { _Value1 = value; AddSign(1); return this; } 
    private: char * _Value2;
    public: char * getValue2() { return _Value2; }
    public: Msg_C2G_Test * setValue2(char * value) { _Value2 = value; AddSign(2); return this; } 
    private: std::vector<__int32> _Value3;
    public: std::vector<__int32> getValue3() { return _Value3; }
    public: Msg_C2G_Test * setValue3(std::vector<__int32> value) { _Value3 = value; AddSign(3); return this; } 
    public: ~Msg_C2G_Test() {
    }
    public: void Write(ScorpioWriter * writer) {
        writer->WriteInt32(__Sign);
        if (HasSign(1)) { writer->WriteInt32(_Value1); }
        if (HasSign(2)) { writer->WriteString(_Value2); }
        if (HasSign(3)) {
            writer->WriteInt32(static_cast<__int32>(_Value3.size()));
            for (size_t i = 0;i < _Value3.size(); ++i) { writer->WriteInt32(_Value3[i]); }
        }
    }
    public: static Msg_C2G_Test * Read(ScorpioReader * reader) {
        Msg_C2G_Test * ret = new Msg_C2G_Test();
        ret->__Sign = reader->ReadInt32();
        if (ret->HasSign(1)) { ret->_Value1 = reader->ReadInt32(); }
        if (ret->HasSign(2)) { ret->_Value2 = reader->ReadString(); }
        if (ret->HasSign(3)) {
            int number = reader->ReadInt32();
            ret->_Value3 = std::vector<__int32>();
            for (int i = 0;i < number; ++i) { ret->_Value3.push_back(reader->ReadInt32()); }
        }
        return ret;
    }
    public: static Msg_C2G_Test * Deserialize(char * data) {
        ScorpioReader * reader = new ScorpioReader(data);
        Msg_C2G_Test * ret = Read(reader);
        reader->Close();
        delete reader;
        reader = nullptr;
        return ret;
    }
};
}
#endif