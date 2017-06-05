//本文件为自动生成，请不要手动修改
package ScorpioProtoTest;
import Scorpio.Commons.*;
import Scorpio.Message.*;
public class Msg_C2G_Empty extends IMessage {
    @Override
    public void Write(ScorpioWriter writer) {
        writer.WriteInt32(__Sign);
    }
    @Override
    public void Read(ScorpioReader reader) {
        __Sign = reader.ReadInt32();
    }
    @Override
    public IMessage New() {
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
    @Override
    public String toString() {
        return "{ " + "" + 
                " }";
    }
}