#ifndef __ITABLE__
#define __ITABLE__
#include "IData.h"
namespace Scorpio {
	namespace Table {
		class ITable {
		public:
			virtual bool Contains(int ID) = 0;
			virtual int Count() = 0;
			virtual IData * GetValue(int key) = 0;
		};
	}
}
#endif

