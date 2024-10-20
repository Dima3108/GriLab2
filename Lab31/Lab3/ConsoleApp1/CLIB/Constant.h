#pragma once
#ifndef CONSTANT_H
#define CONSTANT_H
char baseTable[] = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm0123456789!";
#define ENCODE(x)(baseTable[x/64]+baseTable[x%64])
#define DECODE(x,y)(64*x+y)
typedef unsigned char byte;
#endif // !CONSTANT_H
