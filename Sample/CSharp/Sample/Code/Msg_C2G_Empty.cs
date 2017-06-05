//本文件为自动生成，请不要手动修改
using System.Collections.Generic;
using Scorpio.Commons;
using Scorpio.Message;
namespace ScorpioProtoTest {
public class Msg_C2G_Empty : IMessage {
    public override void Write(ScorpioWriter writer) {
        writer.WriteInt32(__Sign);
    }
    public override void Read(ScorpioReader reader) {
        __Sign = reader.ReadInt32();
    }
    public override IMessage New() {
        return new Msg_C2G_Empty();
    }
    public static Msg_C2G_Empty Readimpl(ScorpioReader reader) {
        Msg_C2G_Empty ret = new Msg_C2G_Empty();
        ret.Read(reader);
        return ret;
    }
    public static Msg_C2G_Empty Deserialize(byte[] data) {
        return Readimpl(new ScorpioReader(data));
    }
    public override string ToString() {
        return "{ " + "" + 
                " }";
    }
}
}