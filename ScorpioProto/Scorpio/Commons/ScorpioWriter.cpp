#include "ScorpioWriter.h"
#include <stdio.h>
#include <string.h>
namespace Scorpio {
	namespace Commons {
		ScorpioWriter::ScorpioWriter()
		{
			buffer = nullptr;
			length = 0;
			capacity = 0;
		}
		ScorpioWriter::~ScorpioWriter() {
			if (buffer) {
				delete buffer;
				buffer = nullptr;
			}
		}
		void ScorpioWriter::EnsureCapacity(size_t value) {
			if (value < capacity)
				return;
			size_t num = value;
			if (num < 256)
				num = 256;
			if (num < capacity * 2)
				num = capacity * 2;
			capacity = num;
			char * array = new char[capacity];
			if (buffer != nullptr) {
				memcpy(array, buffer, length);
				delete buffer;
			}
			buffer = array;
		}
		void ScorpioWriter::WriteBool(bool value) {
			WriteInt8(value ? (__int8)1 : (__int8)0);
		}
		void ScorpioWriter::WriteInt8(__int8 value) {
			EnsureCapacity(length + sizeof(__int8));
			(*(buffer + length)) = value;
			length += sizeof(__int8);
		}
		void ScorpioWriter::WriteInt16(__int16 value) {
			EnsureCapacity(length + sizeof(__int16));
			*((__int16*)(buffer + length)) = value;
			length += sizeof(__int16);
		}
		void ScorpioWriter::WriteInt32(__int32 value) {
			EnsureCapacity(length + sizeof(__int32));
			*((__int32*)(buffer + length)) = value;
			length += sizeof(__int32);
		}
		void ScorpioWriter::WriteInt64(__int64 value) {
			EnsureCapacity(length + sizeof(__int64));
			*((__int64*)(buffer + length)) = value;
			length += sizeof(__int64);
		}
		void ScorpioWriter::WriteFloat(float value) {
			EnsureCapacity(length + sizeof(float));
			*((float*)(buffer + length)) = value;
			length += sizeof(float);
		}
		void ScorpioWriter::WriteDouble(double value) {
			EnsureCapacity(length + sizeof(double));
			*((double*)(buffer + length)) = value;
			length += sizeof(double);
		}
		void ScorpioWriter::WriteString(const char * value) {
			size_t l = strlen(value);
			EnsureCapacity(length + l + 1);
			memcpy(buffer + length, value, l);
			length += l;
			buffer[length] = (__int8)0;
			length += sizeof(__int8);
		}
		void ScorpioWriter::WriteBytes(char * value) {

		}
		char * ScorpioWriter::ToArray() {
			char * ret = new char[length];
			memcpy(ret, buffer, length);
			return ret;
		}
	}
}
