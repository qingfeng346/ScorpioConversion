#ifndef __SCORPIO_CONVERSION_RUNTIME_DEFAULTREADER__
#define __SCORPIO_CONVERSION_RUNTIME_DEFAULTREADER__
#include <iostream>
#include <fstream>
#include "IReader.h"
using namespace std;
namespace Scorpio {
    namespace Conversion {
        namespace Runtime {
            class DefaultReader : public IReader
            {
            private:
                char* buffer;
                char* buf;
                bool closeStream;
            public:
                DefaultReader(char* buffer, bool closeStream = false);
                bool ReadBool();
                __int8 ReadInt8();
                unsigned __int8 ReadUInt8();
                __int16 ReadInt16();
                unsigned __int16 ReadUInt16();
                __int32 ReadInt32();
                unsigned __int32 ReadUInt32();
                __int64 ReadInt64();
                unsigned __int64 ReadUInt64();
                float ReadFloat();
                double ReadDouble();
                __int64 ReadDateTime();
                char* ReadString();
                char* ReadBytes();
                void Close();
            };
        }
    }
}

#endif // !__SCORPIO_CONVERSION_RUNTIME_DEFAULTREADER__
