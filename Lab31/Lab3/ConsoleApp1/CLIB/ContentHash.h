#pragma once
#ifndef CONTENT_HASH
#define CONTENT_HASH
#include "memory.h"
extern "C" {
__declspec(dllexport) unsigned char* ComputeHash(unsigned char* buffer1,unsigned char*buffer2, int bufLen,int buf2Len, int* outLen);
__declspec(dllexport) void DeleteResource(unsigned char* buffer);
}

#endif