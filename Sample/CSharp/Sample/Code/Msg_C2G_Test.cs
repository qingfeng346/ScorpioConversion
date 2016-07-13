//本文件为自动生成，请不要手动修改
using System.Collections.Generic;
using Scorpio.Commons;
using Scorpio.Message;
namespace ScorpioProtoTest {
public class Msg_C2G_Test : IMessage {
    private int _Value1;
    public int getValue1() { return _Value1; }
    public Msg_C2G_Test setValue1(int value) { _Value1 = value; AddSign(1); return this; } 
    private string _Value2;
    public string getValue2() { return _Value2; }
    public Msg_C2G_Test setValue2(string value) { _Value2 = value; AddSign(2); return this; } 
    private List<int> _Value3;
    public List<int> getValue3() { return _Value3; }
    public Msg_C2G_Test setValue3(List<int> value) { _Value3 = value; AddSign(3); return this; } 
    public override void Write(ScorpioWriter writer) {
        writer.WriteInt32(__Sign);
        if (HasSign(1)) { writer.WriteInt32(_Value1); }
        if (HasSign(2)) { writer.WriteString(_Value2); }
        if (HasSign(3)) {
            writer.WriteInt32(_Value3.Count);
            for (int i = 0;i < _Value3.Count; ++i) { writer.WriteInt32(_Value3[i]); }
        }
    }
    public override void Read(ScorpioReader reader) {
        __Sign = reader.ReadInt32();
        if (HasSign(1)) { _Value1 = reader.ReadInt32(); }
        if (HasSign(2)) { _Value2 = reader.ReadString(); }
        if (HasSign(3)) {
            int number = reader.ReadInt32();
            _Value3 = new List<int>();
            for (int i = 0;i < number; ++i) { _Value3.Add(reader.ReadInt32()); }
        }
    }
    public static Msg_C2G_Test Readimpl(ScorpioReader reader) {
        Msg_C2G_Test ret = new Msg_C2G_Test();
        ret.Read(reader);
        return ret;
    }
    public static Msg_C2G_Test Deserialize(byte[] data) {
        return Readimpl(new ScorpioReader(data));
    }
}
}