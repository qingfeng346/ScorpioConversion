//本文件为自动生成，请不要手动修改
using System.Collections.Generic;
using Scorpio.Commons;
using Scorpio.Message;
namespace ScorpioProtoTest {
public class Msg_C2G_Test3 : IMessage {
    private List<Msg_C2G_Test> _Value1;
    public List<Msg_C2G_Test> getValue1() { return _Value1; }
    public Msg_C2G_Test3 setValue1(List<Msg_C2G_Test> value) { _Value1 = value; AddSign(1); return this; } 
    private Msg_C2G_Test2 _Value2;
    public Msg_C2G_Test2 getValue2() { return _Value2; }
    public Msg_C2G_Test3 setValue2(Msg_C2G_Test2 value) { _Value2 = value; AddSign(2); return this; } 
    private TestEnum _Value3;
    public TestEnum getValue3() { return _Value3; }
    public Msg_C2G_Test3 setValue3(TestEnum value) { _Value3 = value; AddSign(3); return this; } 
    private List<TestEnum> _Value4;
    public List<TestEnum> getValue4() { return _Value4; }
    public Msg_C2G_Test3 setValue4(List<TestEnum> value) { _Value4 = value; AddSign(4); return this; } 
    public override void Write(ScorpioWriter writer) {
        writer.WriteInt32(__Sign);
        if (HasSign(1)) {
            writer.WriteInt32(_Value1.Count);
            for (int i = 0;i < _Value1.Count; ++i) { _Value1[i].Write(writer); }
        }
        if (HasSign(2)) { _Value2.Write(writer); }
        if (HasSign(3)) { writer.WriteInt32((int)_Value3); }
        if (HasSign(4)) {
            writer.WriteInt32(_Value4.Count);
            for (int i = 0;i < _Value4.Count; ++i) { writer.WriteInt32((int)_Value4[i]); }
        }
    }
    public override void Read(ScorpioReader reader) {
        __Sign = reader.ReadInt32();
        if (HasSign(1)) {
            int number = reader.ReadInt32();
            _Value1 = new List<Msg_C2G_Test>();
            for (int i = 0;i < number; ++i) { _Value1.Add(Msg_C2G_Test.Readimpl(reader)); }
        }
        if (HasSign(2)) { _Value2 = Msg_C2G_Test2.Readimpl(reader); }
        if (HasSign(3)) { _Value3 = (TestEnum)reader.ReadInt32(); }
        if (HasSign(4)) {
            int number = reader.ReadInt32();
            _Value4 = new List<TestEnum>();
            for (int i = 0;i < number; ++i) { _Value4.Add((TestEnum)reader.ReadInt32()); }
        }
    }
    public override IMessage New() {
        return new Msg_C2G_Test3();
    }
    public override int GetID() {
        return 2;
    }
    
    public static Msg_C2G_Test3 Readimpl(ScorpioReader reader) {
        Msg_C2G_Test3 ret = new Msg_C2G_Test3();
        ret.Read(reader);
        return ret;
    }
    public static Msg_C2G_Test3 Deserialize(byte[] data) {
        return Readimpl(new ScorpioReader(data));
    }
    public override string ToString() {
        return "{ " + 
                "Value1 : " + ScorpioUtil.ToString(_Value1) + "," + 
                "Value2 : " + _Value2 + "," + 
                "Value3 : " + _Value3 + "," + 
                "Value4 : " + ScorpioUtil.ToString(_Value4) + 
                " }";
    }
}
}