#ifndef __SCORPIO_UTIL
#define __SCORPIO_UTIL
namespace Scorpio {
	namespace Commons {
		class ScorpioUtil
		{
		public:
			static bool HasSign(int sign, int index);
			static int AddSign(int sign, int index);
		};
	}
}
#endif
