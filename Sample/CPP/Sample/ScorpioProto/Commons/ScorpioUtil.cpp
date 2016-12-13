#include "ScorpioUtil.h"
namespace Scorpio {
	namespace Commons {
		bool ScorpioUtil::HasSign(int sign, int index) {
			return (sign & (1 << index)) != 0;
		}
		int ScorpioUtil::AddSign(int sign, int index) {
			if ((sign & (1 << index)) == 0)
				sign |= (1 << index);
			return sign;
		}
	}
}