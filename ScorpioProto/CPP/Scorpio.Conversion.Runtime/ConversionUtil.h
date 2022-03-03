#ifndef __SCORPIO_CONVERSION_RUNTIME_CONVERSIONUTIL__
#define __SCORPIO_CONVERSION_RUNTIME_CONVERSIONUTIL__
#include "IReader.h"
namespace Scorpio {
	namespace Conversion {
		namespace Runtime {
			class ConversionUtil {
			public:
				static void ReadHead(IReader* reader);
			};
		}
	}
}
#endif	// !__SCORPIO_CONVERSION_RUNTIME_CONVERSIONUTIL__
