#ifndef __TABLE_UTIL
#define __TABLE_UTIL
#include <vector>
#include "../Commons/ScorpioReader.h"
#include "IData.h"
using namespace Scorpio::Commons;
namespace Scorpio {
	namespace Table {
		class ITableUtil
		{
		public:
			virtual char * GetBuffer(const char * fileName) = 0;
			virtual void Warning(const char * message) = 0;
		};
		class TableUtil
		{
		private:
			static ITableUtil *IUtil;
		public:
			static void SetTableUtil(ITableUtil * util) { IUtil = util; }
			static char * GetBuffer(const char * fileName);
			static void Warning(const char * message);
			static int ReadHead(ScorpioReader * reader, const char * fileName, const char * md5);
		private:
			static const __int8 INVALID_INT8 = (__int8)127;
			static const __int16 INVALID_INT16 = (__int16)127;
			static const __int32 INVALID_INT32 = (__int32)127;
			static const __int64 INVALID_INT64 = (__int64)127;
			static float INVALID_FLOAT;
			static double INVALID_DOUBLE;
		public:
			static bool IsInvalid(__int8 val);
			static bool IsInvalid(__int16 val);
			static bool IsInvalid(__int32 val);
			static bool IsInvalid(__int64 val);
			static bool IsInvalid(float val);
			static bool IsInvalid(double val);
			static bool IsInvalid(const char * val);
			template<class T>
			static bool IsInvalid(std::vector<T> val) {
				return val.empty();
			}
			static bool IsInvalid(IData * val);

			static bool IsInvalidInt8(__int8 val);
			static bool IsInvalidInt16(__int16 val);
			static bool IsInvalidInt32(__int32 val);
			static bool IsInvalidInt64(__int64 val);
			static bool IsInvalidFloat(float val);
			static bool IsInvalidDouble(double val);
			static bool IsInvalidString(const char * val);
			template<typename T> static bool IsInvalidList(std::vector<T> val);
			static bool IsInvalidData(IData *val);
		};
	}
}
#endif