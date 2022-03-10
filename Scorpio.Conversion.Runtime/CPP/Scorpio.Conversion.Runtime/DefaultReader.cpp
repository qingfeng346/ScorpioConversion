#include "DefaultReader.h"
#include <stdio.h>
#include <string.h>
namespace Scorpio {
    namespace Conversion {
        namespace Runtime {
            DefaultReader::DefaultReader(char* buffer, bool closeStream) {
                this->buf = buffer;
                this->buffer = buffer;
                this->closeStream = closeStream;
            }
            bool DefaultReader::ReadBool() {
                return ReadInt8() == (__int8)1;
            }
            __int8 DefaultReader::ReadInt8() {
                __int8 ret = *((__int8*)buffer);
                buffer += sizeof(__int8);
                return ret;
            }
            unsigned __int8 DefaultReader::ReadUInt8() {
                unsigned __int8 ret = *((unsigned __int8*)buffer);
                buffer += sizeof(unsigned __int8);
                return ret;
            }
            __int16 DefaultReader::ReadInt16() {
                __int16 ret = *((__int16*)buffer);
                buffer += sizeof(__int16);
                return ret;
            }
            unsigned __int16 DefaultReader::ReadUInt16() {
                unsigned __int16 ret = *((unsigned __int16*)buffer);
                buffer += sizeof(unsigned __int16);
                return ret;
            }
            __int32 DefaultReader::ReadInt32() {
                __int32 ret = *((__int32*)buffer);
                buffer += sizeof(__int32);
                return ret;
            }
            unsigned __int32 DefaultReader::ReadUInt32() {
                unsigned __int32 ret = *((unsigned __int32*)buffer);
                buffer += sizeof(unsigned __int32);
                return ret;
            }
            __int64 DefaultReader::ReadInt64() {
                __int64 ret = *((__int64*)buffer);
                buffer += sizeof(__int64);
                return ret;
            }
            unsigned __int64 DefaultReader::ReadUInt64() {
                unsigned __int64 ret = *((unsigned __int64*)buffer);
                buffer += sizeof(unsigned __int64);
                return ret;
            }
            float DefaultReader::ReadFloat() {
                float ret = *((float*)buffer);
                buffer += sizeof(float);
                return ret;
            }
            double DefaultReader::ReadDouble() {
                double ret = *((double*)buffer);
                buffer += sizeof(double);
                return ret;
            }
            __int64 DefaultReader::ReadDateTime() {
                return ReadInt64();
            }
            char* DefaultReader::ReadString() {
                unsigned __int16 length = ReadUInt16();
                char* ret = new char[length];
                memcpy(ret, buffer, length);
                buffer += length;
                return ret;
            }
            char* DefaultReader::ReadBytes() {
                int length = ReadInt32();
                char* ret = new char[length];
                memcpy(ret, buffer, length);
                buffer += length;
                return ret;
            }
            void DefaultReader::Close() {
                if (this->closeStream) {
                    delete[] buf;
                }
                this->buf = nullptr;
                this->buffer = nullptr;
            }
        }
    }
}
