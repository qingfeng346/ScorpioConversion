#ifndef __SCORPIO_READER
#define __SCORPIO_READER
namespace Scorpio {
	namespace Commons {
		class ScorpioReader
		{
		private:
			char * buf;
			char * buffer;
		public:
			ScorpioReader(char * buffer);
			void Close();
			bool ReadBool();
			__int8 ReadInt8();
			__int16 ReadInt16();
			__int32 ReadInt32();
			__int64 ReadInt64();
			float ReadFloat();
			double ReadDouble();
			char * ReadString();
			char * ReadBytes();
		};
	}
}

#endif
