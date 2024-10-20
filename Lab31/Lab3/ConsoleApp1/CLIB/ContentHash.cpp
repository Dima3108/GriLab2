#include"ContentHash.h"
#include"sha384.h"
#include"SHA256.h"
#include"sha512.h"
#include"iostream"
#include"Constant.h"
using namespace std;
bool IsZeroBuf(byte* buffer, int bLen) {
	for(int i=0;i<bLen;i++)
		if (static_cast<int>(buffer[i]) != 0) {
			return false;
		}
	return true;
}
unsigned char* ComputeHash(unsigned char* buffer1, unsigned char* buffer2, int bufLen, int buf2Len, int* outLen) {
	int index = static_cast<int>(buffer1[0]) % static_cast<int>(buffer2[0]);
	string s = "";
	int pos1 = 0, pos2 = 0;
	//cout << index << endl;
	if (IsZeroBuf(buffer1, bufLen) || IsZeroBuf(buffer2, buf2Len))
	{
		cout << "zero buffer!\n" << endl;
	}
	if (index % 2 == 0) {
		
		while ((int)((s.size())/2) != buf2Len + bufLen) {
			if (pos1 < bufLen) {
				//s +=ENCODE(static_cast<int>( buffer1[pos1++]));
				int v = static_cast<int>(buffer1[pos1++]);
				s += baseTable[(int)(v / 64)];
				s += baseTable[(int)(v % 64)];
			}
			if (pos2 < buf2Len) {
				//s +=ENCODE(static_cast<int>( buffer2[pos2++]));
				int v = static_cast<int>(buffer2[pos2++]);
				s += baseTable[(int)(v / 64)];
				s += baseTable[(int)(v % 64)];
			}
		}
	}
	else {
		//int pos1 = 0, pos2 = 0;
		while ((int)(s.size()/2) != buf2Len + bufLen) {
			
			if (pos2 < buf2Len) {
				//s += buffer2[pos2++];
				int v = static_cast<int>(buffer2[pos2++]);
				s += baseTable[(int)(v / 64)];
				s += baseTable[(int)(v % 64)];
			}
			if (pos1 < bufLen) {
				//s += buffer1[pos1++];
				int v = static_cast<int>(buffer1[pos1++]);
				s += baseTable[(int)(v / 64)];
				s += baseTable[(int)(v % 64)];
			}
		}
	}
	//printf("end s write!\n");
	//std::cout<<endl<<"s:" << s << std::endl;
	SHA384 sha;
	SHA256 sha2;
	SHA512 sha3;
	string res;
	switch (static_cast<int>(buffer1[1])%3)
	{
	case 0:
		res = sha.hash(s);
		break;
	case 1:
		res = sha2.hash(s);
		break;
	case 2:
		res = sha3.hash(s);
		break;
	}
	//printf("end hash\n");
	*outLen = res.size()*2;
	string r__ = "";
	unsigned char* r_ = (unsigned char*)malloc(sizeof(unsigned char) * (*outLen));
	for (int i = 0; i < res.size(); i++) {
		//r_[i] = ENCODE(static_cast<int>(res[i]));
		//string s= ENCODE(static_cast<int>(res[i]));
		int v = static_cast<int>(res[i]);
		r__ += baseTable[(int)(v / 64)];
		r_[2*i] =static_cast<byte>( baseTable[(int)(v / 64)]);
		r__ += baseTable[(int)(v % 64)];
		r_[(2*i) + 1] =static_cast<byte>( baseTable[(int)(v % 64)]);
	}
	//std::cout<<std::endl << r__ << std::endl;
	return r_;
}
void DeleteResource(unsigned char* buffer) {
	free((void*)buffer);
}