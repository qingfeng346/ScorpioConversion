#include "IMessage.h"
#include "../Commons/ScorpioUtil.h"
using namespace Scorpio::Commons;
namespace Scorpio {
	namespace Message {
		void IMessage::AddSign(int index) {
			__Sign = ScorpioUtil::AddSign(__Sign, index);
		}
		bool IMessage::HasSign(int index) {
			return ScorpioUtil::HasSign(__Sign, index);
		}
		char * IMessage::Serialize() {
			ScorpioWriter * writer = new ScorpioWriter();
			Write(writer);
			char * ret = writer->ToArray();
			delete writer;
			writer = nullptr;
			return ret;
		}
	}
}