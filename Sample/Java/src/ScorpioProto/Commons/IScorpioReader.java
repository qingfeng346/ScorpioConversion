package ScorpioProto.Commons;

import java.util.Calendar;

public interface IScorpioReader {
	boolean ReadBool();
	byte ReadInt8();
	short ReadInt16();
	int ReadInt32();
	long ReadInt64();
	float ReadFloat();
	double ReadDouble();
	String ReadString();
	Calendar ReadDateTime();
	byte[] ReadBytes();
}