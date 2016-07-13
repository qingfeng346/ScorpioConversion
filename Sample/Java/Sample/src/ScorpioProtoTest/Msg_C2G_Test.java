//本文件为自动生成，请不要手动修改
package ScorpioProtoTest;
import Scorpio.Commons.*;
import Scorpio.Message.*;
public class Msg_C2G_Test extends IMessage {
    private Integer _Value1;
    public Integer getValue1() { return _Value1; }
    public Msg_C2G_Test setValue1(Integer value) { _Value1 = value; AddSign(1); return this; } 
    private String _Value2;
    public String getValue2() { return _Value2; }
    public Msg_C2G_Test setValue2(String value) { _Value2 = value; AddSign(2); return this; } 
    private java.util.List<Integer> _Value3 = new java.util.ArrayList<Integer>();
    public java.util.List<Integer> getValue3() { return _Value3; }
    public Msg_C2G_Test setValue3(java.util.List<Integer> value) { _Value3 = value; AddSign(3); return this; }
    public Msg_C2G_Test addValue3(Integer value) { _Value3.add(value); AddSign(3); return this; }
    public Msg_C2G_Test insertValue3(int index, Integer value) { _Value3.add(index, value); AddSign(3); return this; } 
    @Override
    public void Write(ScorpioWriter writer) {
        writer.WriteInt32(__Sign);
        if (HasSign(1)) { writer.WriteInt32(_Value1); }
        if (HasSign(2)) { writer.WriteString(_Value2); }
        if (HasSign(3)) {
            writer.WriteInt32(_Value3.size());
            for (int i = 0;i < _Value3.size(); ++i) { writer.WriteInt32(_Value3.get(i)); }
        }
    }
    @Override
    public void Read(ScorpioReader reader) {
        __Sign = reader.ReadInt32();
        if (HasSign(1)) { _Value1 = reader.ReadInt32(); }
        if (HasSign(2)) { _Value2 = reader.ReadString(); }
        if (HasSign(3)) {
            int number = reader.ReadInt32();
            _Value3 = new java.util.ArrayList<Integer>();
            for (int i = 0;i < number; ++i) { _Value3.add(reader.ReadInt32()); }
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