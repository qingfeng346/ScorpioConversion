//本文件为自动生成，请不要手动修改
package ScorpioProtoTest;
import Scorpio.Commons.*;
import Scorpio.Message.*;
public class Msg_C2G_Test2 extends IMessage {
    private Msg_C2G_Test _Value1;
    public Msg_C2G_Test getValue1() { return _Value1; }
    public Msg_C2G_Test2 setValue1(Msg_C2G_Test value) { _Value1 = value; AddSign(1); return this; } 
    @Override
    public void Write(ScorpioWriter writer) {
        writer.WriteInt32(__Sign);
        if (HasSign(1)) { _Value1.Write(writer); }
    }
    @Override
    public void Read(ScorpioReader reader) {
        __Sign = reader.ReadInt32();
        if (HasSign(1)) { _Value1 = Msg_C2G_Test.Readimpl(reader); }
    }
    @Override
    public IMessage New() {
        return new Msg_C2G_Test2();
    }
    @Override
    public int GetID() {
        return 1;
    }
    
    public static Msg_C2G_Test2 Readimpl(ScorpioReader reader) {
        Msg_C2G_Test2 ret = new Msg_C2G_Test2();
        ret.Read(reader);
        return ret;
    }
    public static Msg_C2G_Test2 Deserialize(byte[] data) {
        return Readimpl(new ScorpioReader(data));
    }
    @Override
    public String toString() {
        return "{ " + 
                "Value1 : " + _Value1 + 
                " }";
    }
}