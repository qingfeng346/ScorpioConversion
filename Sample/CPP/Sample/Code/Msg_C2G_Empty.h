#ifndef __Msg_C2G_Empty_H__
#define __Msg_C2G_Empty_H__
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
class Msg_C2G_Empty : public IMessage {
    public: ~Msg_C2G_Empty() {
    }
    public: void Write(ScorpioWriter * writer) {
        writer->WriteInt32(__Sign);
    }
    public: static Msg_C2G_Empty * Read(ScorpioReader * reader) {
        Msg_C2G_Empty * ret = new Msg_C2G_Empty();
        ret->__Sign = reader->ReadInt32();
        return ret;
    }
    public: static Msg_C2G_Empty * Deserialize(char * data) {
        ScorpioReader * reader = new ScorpioReader(data);
        Msg_C2G_Empty * ret = Read(reader);
        reader->Close();
        delete reader;
        reader = nullptr;
        return ret;
    }
};
}
#endif