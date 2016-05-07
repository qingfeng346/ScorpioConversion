#ifndef __SCORPIO_WRITER
#define __SCORPIO_WRITER
namespace Scorpio {
	namespace Commons {
		class ScorpioWriter
		{
		private:
			size_t length;
			size_t capacity;
			unsigned char * buffer;
			void EnsureCapacity(size_t value);
		public:
			ScorpioWriter();
			~ScorpioWriter();
			void WriteBool(bool value);
			void WriteInt8(__int8 value);
			void WriteInt16(__int16 value);
			void WriteInt32(__int32 value);
			void WriteInt64(__int64 value);
			void WriteFloat(float value);
			void WriteDouble(double value);
			void WriteString(const char * value);
			void WriteBytes(unsigned char * value);
			unsigned char * ToArray();
		};
	}
}
#endif

