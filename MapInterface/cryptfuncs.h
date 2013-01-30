
#include <windows.h>
#include <iostream>
#include <fstream>
using namespace std;

#pragma once


#define Crypt_Error 0
#define Crypt_Plr   1
#define Crypt_Map   2
#define Crypt_SoundSet 3
#define Crypt_Thing    4
#define Crypt_Mod      5
#define Crypt_Game     6
#define Crypt_Monster   7


BYTE Ucrypt(BYTE *data,int BuffLen,int mode, unsigned int *table,int tabl);
int CryptFunc(char Filename_In[100],char Filename_Out[100],bool Decrypt,int Type);
