#ifndef __SCORPIO_CONVERSION_RUNTIME_IREADER__
#define __SCORPIO_CONVERSION_RUNTIME_IREADER__
namespace Scorpio {
    namespace Conversion {
        namespace Runtime {
            class IReader {
            public:
                virtual bool ReadBool() = 0;
                virtual __int8 ReadInt8() = 0;
                virtual unsigned __int8 ReadUInt8() = 0;
                virtual __int16 ReadInt16() = 0;
                virtual unsigned __int16 ReadUInt16() = 0;
                virtual __int32 ReadInt32() = 0;
                virtual unsigned __int32 ReadUInt32() = 0;
                virtual __int64 ReadInt64() = 0;
                virtual unsigned __int64 ReadUInt64() = 0;
                virtual float ReadFloat() = 0;
                virtual double ReadDouble() = 0;
                virtual __int64 ReadDateTime() = 0;
                virtual char* ReadString() = 0;
                virtual char* ReadBytes() = 0;
                virtual void Close() = 0;
            };
        }
    }
}
#endif	// !__SCORPIO_CONVERSION_RUNTIME_IREADER__
