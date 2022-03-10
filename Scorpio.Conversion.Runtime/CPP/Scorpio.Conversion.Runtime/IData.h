#ifndef __SCORPIO_CONVERSION_RUNTIME_IDATA__
#define __SCORPIO_CONVERSION_RUNTIME_IDATA__
namespace Scorpio {
    namespace Conversion {
        namespace Runtime {
            class IData
            {
            public:
                virtual void * GetData(string key) = 0;
            };
        }
    }
}
#endif