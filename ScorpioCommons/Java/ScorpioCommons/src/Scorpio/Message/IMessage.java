package Scorpio.Message;

import Scorpio.Commons.ScorpioUtil;
import Scorpio.Commons.ScorpioWriter;

public abstract class IMessage {
    protected int __Sign = 0;
    protected final void AddSign(int index) {
        __Sign = ScorpioUtil.AddSign(__Sign, index);
    }
    public final boolean HasSign(int index) {
        return ScorpioUtil.HasSign(__Sign, index);
    }
    public final byte[] Serialize() {
    	ScorpioWriter writer = new ScorpioWriter();
        Write(writer);
        return writer.ToArray();
    }
    public abstract void Write(ScorpioWriter writer);
}