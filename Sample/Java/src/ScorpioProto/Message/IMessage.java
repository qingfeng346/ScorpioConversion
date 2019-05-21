package ScorpioProto.Message;

import ScorpioProto.Commons.ScorpioReader;
import ScorpioProto.Commons.ScorpioUtil;
import ScorpioProto.Commons.ScorpioWriter;

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
	public final void Parser(byte[] buffer) {
		Read(new ScorpioReader(buffer));
	}
	public abstract void Write(ScorpioWriter writer);
	public abstract void Read(ScorpioReader reader);
	public abstract IMessage New();
	public abstract int GetID();
}