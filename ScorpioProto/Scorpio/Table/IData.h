#ifndef __IDATA__
#define __IDATA__
namespace Scorpio {
	namespace Table {
		class IData
		{
		public:
			virtual bool IsInvalid() = 0;
			virtual void* GetData(char * key) = 0;
		};
	}
}
#endif
