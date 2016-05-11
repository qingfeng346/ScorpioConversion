#ifndef __Msg_C2G_Test2_H__
#define __Msg_C2G_Test2_H__
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

namespace ScorpioProtoTest{
//本文件为自动生成，请不要手动修改
class Msg_C2G_Test2 : public IMessage {
    private: Msg_C2G_Test * _Value1;
    public: Msg_C2G_Test * getValue1() { return _Value1; }
    public: Msg_C2G_Test2 * setValue1(Msg_C2G_Test * value) { _Value1 = value; AddSign(1); return this; } 
    public: ~Msg_C2G_Test2() {
        delete _Value1; _Value1 = nullptr; 
    }
    public: void Write(ScorpioWriter * writer) {
        writer->WriteInt32(__Sign);
        if (HasSign(1)) { _Value1->Write(writer); }
    }
    public: static Msg_C2G_Test2 * Read(ScorpioReader * reader) {
        Msg_C2G_Test2 * ret = new Msg_C2G_Test2();
        ret->__Sign = reader->ReadInt32();
        if (ret->HasSign(1)) { ret->_Value1 = Msg_C2G_Test::Read(reader); }
        return ret;
    }
    public: static Msg_C2G_Test2 * Deserialize(char * data) {
        ScorpioReader * reader = new ScorpioReader(data);
        Msg_C2G_Test2 * ret = Read(reader);
        reader->Close();
        delete reader;
        reader = nullptr;
        return ret;
    }
};
}
#endif