//本文件为自动生成，请不要手动修改
package ScorpioProtoTest;
import Scorpio.Commons.*;
import Scorpio.Message.*;
public class Msg_C2G_Test3 extends IMessage {
    private java.util.List<Msg_C2G_Test> _Value1 = new java.util.ArrayList<Msg_C2G_Test>();
    public java.util.List<Msg_C2G_Test> getValue1() { return _Value1; }
    public Msg_C2G_Test3 setValue1(java.util.List<Msg_C2G_Test> value) { _Value1 = value; AddSign(1); return this; }
    public Msg_C2G_Test3 addValue1(Msg_C2G_Test value) { _Value1.add(value); AddSign(1); return this; }
    public Msg_C2G_Test3 insertValue1(int index, Msg_C2G_Test value) { _Value1.add(index, value); AddSign(1); return this; } 
    private Msg_C2G_Test2 _Value2;
    public Msg_C2G_Test2 getValue2() { return _Value2; }
    public Msg_C2G_Test3 setValue2(Msg_C2G_Test2 value) { _Value2 = value; AddSign(2); return this; } 
    private TestEnum _Value3;
    public TestEnum getValue3() { return _Value3; }
    public Msg_C2G_Test3 setValue3(TestEnum value) { _Value3 = value; AddSign(3); return this; } 
    private java.util.List<TestEnum> _Value4 = new java.util.ArrayList<TestEnum>();
    public java.util.List<TestEnum> getValue4() { return _Value4; }
    public Msg_C2G_Test3 setValue4(java.util.List<TestEnum> value) { _Value4 = value; AddSign(4); return this; }
    public Msg_C2G_Test3 addValue4(TestEnum value) { _Value4.add(value); AddSign(4); return this; }
    public Msg_C2G_Test3 insertValue4(int index, TestEnum value) { _Value4.add(index, value); AddSign(4); return this; } 
    @Override
    public void Write(ScorpioWriter writer) {
        writer.WriteInt32(__Sign);
        if (HasSign(1)) {
            writer.WriteInt32(_Value1.size());
            for (int i = 0;i < _Value1.size(); ++i) { _Value1.get(i).Write(writer); }
        }
        if (HasSign(2)) { _Value2.Write(writer); }
        if (HasSign(3)) { writer.WriteInt32(_Value3.getValue()); }
        if (HasSign(4)) {
            writer.WriteInt32(_Value4.size());
            for (int i = 0;i < _Value4.size(); ++i) { writer.WriteInt32(_Value4.get(i).getValue()); }
        }
    }
    @Override
    public void Read(ScorpioReader reader) {
        __Sign = reader.ReadInt32();
        if (HasSign(1)) {
            int number = reader.ReadInt32();
            _Value1 = new java.util.ArrayList<Msg_C2G_Test>();
            for (int i = 0;i < number; ++i) { _Value1.add(Msg_C2G_Test.Readimpl(reader)); }
        }
        if (HasSign(2)) { _Value2 = Msg_C2G_Test2.Readimpl(reader); }
        if (HasSign(3)) { _Value3 = TestEnum.valueOf(reader.ReadInt32()); }
        if (HasSign(4)) {
            int number = reader.ReadInt32();
            _Value4 = new java.util.ArrayList<TestEnum>();
            for (int i = 0;i < number; ++i) { _Value4.add(TestEnum.valueOf(reader.ReadInt32())); }
        }
    }
    @Override
    public IMessage New() {
        return new Msg_C2G_Test3();
    }
    public static Msg_C2G_Test3 Readimpl(ScorpioReader reader) {
        Msg_C2G_Test3 ret = new Msg_C2G_Test3();
        ret.Read(reader);
        return ret;
    }
    public static Msg_C2G_Test3 Deserialize(byte[] data) {
        return Readimpl(new ScorpioReader(data));
    }
}