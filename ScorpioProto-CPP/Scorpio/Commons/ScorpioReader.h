#ifndef __SCORPIO_READER
#define __SCORPIO_READER
namespace Scorpio {
	namespace Commons {

	}
}
namespace Scorpio {
	namespace Commons {
		class ScorpioReader
		{
		private:
			unsigned char * buf;
			unsigned char * buffer;
		public:
			ScorpioReader(unsigned char * buffer);
			void Close();
			bool ReadBool();
			__int8 ReadInt8();
			__int16 ReadInt16();
			__int32 ReadInt32();
			__int64 ReadInt64();
			float ReadFloat();
			double ReadDouble();
			char * ReadString();
			unsigned char * ReadBytes();
		};
	}
}

#endif
