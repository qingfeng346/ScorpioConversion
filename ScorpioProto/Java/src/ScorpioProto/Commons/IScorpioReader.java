package ScorpioProto.Commons;

import java.util.Calendar;

public interface IScorpioReader {
	boolean ReadBool();
	byte ReadInt8();
	byte ReadUInt8();
	short ReadInt16();
	short ReadUInt16();
	int ReadInt32();
	int ReadUInt32();
	long ReadInt64();
	long ReadUInt64();
	float ReadFloat();
	double ReadDouble();
	String ReadString();
	Calendar ReadDateTime();
	byte[] ReadBytes();
}