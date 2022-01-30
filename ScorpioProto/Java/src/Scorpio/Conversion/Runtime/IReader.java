package Scorpio.Conversion.Runtime;

import java.util.Date;

public interface IReader {
	boolean ReadBool() throws Exception;
	byte ReadInt8() throws Exception;
	int ReadUInt8() throws Exception;
	short ReadInt16() throws Exception;
	int ReadUInt16() throws Exception;
	int ReadInt32() throws Exception;
	int ReadUInt32() throws Exception;
	long ReadInt64() throws Exception;
	long ReadUInt64() throws Exception;
	float ReadFloat() throws Exception;
	double ReadDouble() throws Exception;
	String ReadString() throws Exception;
	Date ReadDateTime() throws Exception;
	byte[] ReadBytes() throws Exception;
	void Close() throws Exception;
}