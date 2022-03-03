#ifndef __SCORPIO_CONVERSION_RUNTIME_ITABLE__
#define __SCORPIO_CONVERSION_RUNTIME_ITABLE__
#include "IData.h"
namespace Scorpio {
	namespace Conversion {
		namespace Runtime {
			class ITable
			{
			public:
				virtual bool ContainsObject(void* ID) = 0;
				virtual IData* GetValueObject(void* ID) = 0;
				virtual int Count() = 0;
			};
		}
	}
}
#endif