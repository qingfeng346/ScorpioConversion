#ifndef __IMESSAGE__
#define __IMESSAGE__
#include "../Commons/ScorpioWriter.h"
using namespace Scorpio::Commons;
namespace Scorpio {
	namespace Message {
		class IMessage {
		protected:
			int __Sign = 0;
			void AddSign(int index);
		public:
			bool HasSign(int index);
			char * Serialize();
			virtual void Write(ScorpioWriter * writer) = 0;
		};
	}
}
#endif
