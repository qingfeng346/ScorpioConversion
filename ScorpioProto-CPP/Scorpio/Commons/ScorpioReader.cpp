#include "ScorpioReader.h"
#include <stdio.h>
#include <string.h>
namespace Scorpio {
	namespace Commons {
		ScorpioReader::ScorpioReader(char * buffer) {
			this->buf = buffer;
			this->buffer = buffer;
		}
		void ScorpioReader::Close() {
			delete buf;
			buf = nullptr;
			buffer = nullptr;
		}
		bool ScorpioReader::ReadBool() {
			return ReadInt8() == (__int8)1;
		}
		__int8 ScorpioReader::ReadInt8() {
			__int8 ret = *((__int8*)buffer);
			buffer += sizeof(__int8);
			return ret;
		}
		__int16 ScorpioReader::ReadInt16() {
			__int16 ret = *((__int16*)buffer);
			buffer += sizeof(__int16);
			return ret;
		}
		__int32 ScorpioReader::ReadInt32() {
			__int32 ret = *((__int32*)buffer);
			buffer += sizeof(__int32);
			return ret;
		}
		__int64 ScorpioReader::ReadInt64() {
			__int64 ret = *((__int64*)buffer);
			buffer += sizeof(__int64);
			return ret;
		}
		float ScorpioReader::ReadFloat() {
			float ret = *((float*)buffer);
			buffer += sizeof(float);
			return ret;
		}
		double ScorpioReader::ReadDouble() {
			double ret = *((double*)buffer);
			buffer += sizeof(double);
			return ret;
		}
		char * ScorpioReader::ReadString() {
			char * start = buffer;
			while (ReadInt8() != (__int8)0) { }
			__int64 length = buffer - start;
			char * ret = new char[length];
			memcpy(ret, start, length);
			return ret;
		}
		char * ScorpioReader::ReadBytes() {
			int length = ReadInt32();
			char * ret = new char[length];
			memcpy(ret, buffer, length);
			buffer += length;
			return ret;
		}
	}
}