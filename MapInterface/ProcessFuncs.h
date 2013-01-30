#include <windows.h>
#include <iostream>
#include <fstream>
using namespace std;

class PROCESS
{

public:
	
HANDLE window;
HWND hWindow;
DWORD pid;
unsigned long bytes;
char Buffer[100];
	

int WriteByte(DWORD Address,BYTE Value);
char *ReadByte(DWORD Address,int Num);
int LoadProcess(char ThreadName[30]);
int WriteByteArray(DWORD Address,char Value[100],int Num,bool Null_It);

PROCESS()
{
  memset((void*)&*this,0x00,sizeof(*this));
}
private:
};

		
