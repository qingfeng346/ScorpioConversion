//本文件为自动生成，请不要手动修改
package scorpiogame.proto;
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
    public static Msg_C2G_Test2 Read(ScorpioReader reader) {
        Msg_C2G_Test2 ret = new Msg_C2G_Test2();
        ret.__Sign = reader.ReadInt32();
        if (ret.HasSign(1)) { ret._Value1 = Msg_C2G_Test.Read(reader); }
        return ret;
    }
    public static Msg_C2G_Test2 Deserialize(byte[] data) {
        return Read(new ScorpioReader(data));
    }
}