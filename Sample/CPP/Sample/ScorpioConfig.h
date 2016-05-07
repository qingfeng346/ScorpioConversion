#ifndef ScorpioConfig_h__
#define ScorpioConfig_h__

#include <malloc.h>

#define null 0
#define Safe_Release(p)			if(p){p->Release();}
#define Safe_Delete(p)			if(p){delete p;}
#define Safe_Delete_Array(p)	if(p){delete[] p;}
#define Safe_Free(p)			if(p){free(p);}
#define Malloc(t,s)				(t*)malloc(sizeof(t)*s)

#if _WIN64
	typedef unsigned __int64 size_t;
#else
	typedef unsigned __int32 size_t;
#endif

#endif // ScorpioConfig_h__