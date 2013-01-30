#include "ProcessFuncs.h"
/*
HWND FindWindow(
		LPCTSTR lpClassName,  // pointer to class name
		LPCTSTR lpWindowName  // pointer to window name
	);

DWORD GetWindowThreadProcessId(
		HWND hWnd,             // handle to window
		LPDWORD lpdwProcessId  // address of variable for process identifier
	);


HANDLE OpenProcess(
		DWORD dwDesiredAccess,  // access flag
		BOOL bInheritHandle,    // handle inheritance flag
		DWORD dwProcessId       // process identifier
	);
	
	BOOL WriteProcessMemory(

              HANDLE  hProcess,			// handle of process whose memory is written to 
              LPVOID  lpBaseAddress,		// address to start writing to
              LPVOID  lpBuffer,			// address of buffer to write data to
              DWORD  cbWrite,			// number of bytes to write     
              LPDWORD  lpNumberOfBytesWritten 	// actual number of bytes written
        );
	*/
int PROCESS::LoadProcess(char ThreadName[30])
{

	hWindow = FindWindow(NULL, ThreadName);
			if (hWindow) {
				GetWindowThreadProcessId(hWindow, &pid);
				window=OpenProcess(PROCESS_ALL_ACCESS, false, pid);
			}
if(window && hWindow)
{
return(true);
}
else
{
return(false);
}
}
int PROCESS::WriteByte(DWORD Address,BYTE Value)
{		
			if(WriteProcessMemory(window, (void*)Address,
					 (void *)&Value, 1, &bytes) != 0)
			{return(bytes);}
			else
			{return(false);}
}


char *PROCESS::ReadByte(DWORD Address,int Num)
{	
			if(ReadProcessMemory(window, (void*)Address,
					 (void *)&Buffer, Num, &bytes) != 0)
			{return(Buffer);}
else
{return(false);}

}

int PROCESS::WriteByteArray(DWORD Address,char Value[100],int Num,bool Null_It)
{
int we=(int)Address;

if(Num==0)
{
for(int i=0; i<strlen(Value); i++)
{
            if(WriteProcessMemory(window, (void*)we,
					 (void *)&Value[i], 1, &bytes) != 0)
			{}
			else
			{return(false);}
we++;
Address=(DWORD)we;
}

if(Null_It==true)
{
	char nul=0x00;

if(WriteProcessMemory(window, (void*)we,
					 (void *)&nul, 1, &bytes) != 0)
			{}
			else
			{return(false);}

}


return(true);
}


else
{
for(int i=0; i<Num; i++)
{
	
	if(WriteProcessMemory(window, (void*)we,
					 (void *)&Value[i], Num, &bytes) != 0)
			{}
			else
			{return(false);}

we++;
Address=(DWORD)we;
}

if(Null_It==true)
{
	char nul=0x00;

if(WriteProcessMemory(window, (void*)we,
					 (void *)&nul, 1, &bytes) != 0)
			{}
			else
			{return(false);}

}

}
return(true);
}