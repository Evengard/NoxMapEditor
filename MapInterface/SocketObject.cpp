

#include "SocketObject.h"
#include <fstream>

// PACKET FUNCTIONS

void PACKET::operator ^ ( int XOR_KEY )
{
	unsigned char* loc = PACKET_DATA;
    for(int i=0; i<PACKET_LEN; i++,loc++)
       *loc ^= XOR_KEY;
}


PACKET PACKET::operator+ (PACKET &aPacket)
{
	PACKET tem;
    tem.WRITE(PACKET_DATA,PACKET_LEN);
    tem.WRITE(aPacket.PACKET_DATA,aPacket.PACKET_LEN,PACKET_LEN);
	return(tem);
}


ostream & operator<< (ostream &stream, PACKET &Packet)
{
   stream.write((const char *)Packet.GetData(),Packet.GetDataLen());
   return(stream);
}

char * operator>> (PACKET &Packet, char *buffer)
{
  strcpy( buffer, (const char*)(Packet.PACKET_DATA + Packet.RLoc) );
  Packet.iRLoc( strlen(buffer) );
  return( buffer );
}

// SOCKET OBJECT FUNCTIONS

int SocketObject::Test(int val)
{
   if( val == SOCKET_ERROR )
	   HAS_CONNECTION = false;
   return(val); 
}
// Default Constructor
SocketObject::SocketObject()
{
  Init();
}

// Connect Constructor
SocketObject::SocketObject(char* szServerAddress, int iPort, bool Sock_Stream)
{
  Init();
  Connect(szServerAddress, iPort, Sock_Stream);
}

// Destructor
SocketObject::~SocketObject()
{
	Disconnect();
}


void SocketObject::Init()
{
	WSADATA wsaData;
	WORD	wVersionRequested;

	wVersionRequested = MAKEWORD( 2, 0 );

	skSocket = INVALID_SOCKET;
	iStatus = WSAStartup(wVersionRequested,&wsaData);
	HAS_CONNECTION = false;
}




// Connect
bool SocketObject::Connect(char* szServerAddress, int iPort, bool Sock_Stream)
{
	struct		sockaddr_in serv_addr;
	LPHOSTENT	lphost;

	memset(&serv_addr,0,sizeof(sockaddr_in));
	serv_addr.sin_family = AF_INET;
	serv_addr.sin_addr.s_addr = inet_addr(szServerAddress);

	if (serv_addr.sin_addr.s_addr == INADDR_NONE)
	{
		lphost = gethostbyname(szServerAddress);
		if (lphost != NULL)
			serv_addr.sin_addr.s_addr = ((LPIN_ADDR)lphost->h_addr)->s_addr;
		else
		{
			WSASetLastError(WSAEINVAL);
			return FALSE;
		}
	}

	serv_addr.sin_port = htons(iPort);

	// Open the socket
if(Sock_Stream==true)
{skSocket = socket(AF_INET, SOCK_STREAM, 0);}
else
{skSocket = socket(AF_INET, SOCK_DGRAM, 0);}


	if(skSocket == INVALID_SOCKET)
	{
		return false;
	}

	int err = connect(skSocket, (struct sockaddr*)&serv_addr,sizeof(sockaddr));
	if(err == SOCKET_ERROR)
	{
		Disconnect();
		return false;
	}

	HAS_CONNECTION = true;
	return true;
}

void SocketObject::Disconnect()
{
	if(skSocket != INVALID_SOCKET)
	{
		closesocket(skSocket);
		skSocket = INVALID_SOCKET;
	}
	HAS_CONNECTION = false;
}

int SocketObject::Bind(int iPort)
{
	sockaddr_in saServerAddress;

	skSocket = socket(AF_INET, SOCK_STREAM, 0);
	
	if(skSocket == INVALID_SOCKET)
	{
		return false;
	}

	memset(&saServerAddress, 0, sizeof(sockaddr_in));

	saServerAddress.sin_family = AF_INET;
	saServerAddress.sin_addr.s_addr = htonl(INADDR_ANY);
	saServerAddress.sin_port = htons(iPort);

	if( bind(skSocket, (sockaddr*) &saServerAddress, sizeof(sockaddr)) == SOCKET_ERROR)
	{
		Disconnect();
		return false;
	}
	else
		return true;
}

int SocketObject::Listen( void )
{
	return listen( skSocket, 32 );
}

bool SocketObject::Accept( SocketObject &skAcceptSocket )
{
	sockaddr_in saClientAddress;
	int			iClientSize = sizeof(sockaddr_in);
	SOCKADDR	IPAddress;

	skAcceptSocket.skSocket = accept( skSocket, (struct sockaddr*)&saClientAddress, &iClientSize );
	
	if( skAcceptSocket.skSocket == INVALID_SOCKET ) 
	{
		return false;
	}
	else 
	{
		memcpy(&IPAddress,&saClientAddress,sizeof(saClientAddress));
			return true;
	}
}

int SocketObject::Recv( char *szBuffer, int iBufLen, int iFlags)
{
	recv(skSocket, szBuffer, iBufLen, iFlags);
	return true;
}

int SocketObject::Send(char *szBuffer, int iBufLen, int iFlags)
{
	return Test(send(skSocket,szBuffer,iBufLen,iFlags));
}


int SocketObject::Recv( PACKET &Packet, int iFlags)
{
	Packet.Clear();
	int len = recv(skSocket, (char*)Packet.GetData(), MAX_DATA, iFlags);
	Packet.SetDataLen(len);
	return true;//(len);
}

int SocketObject::Send( PACKET &Packet, int iFlags)
{
	return Test(send(skSocket, (char*)Packet.GetData(),(Packet.GetDataLen()),iFlags));
}


int SocketObject::UnblockSocket()
{ 
   unsigned long val = 1;
   return ioctlsocket(skSocket,FIONBIO,&val);
}

       /*
        FLAGS
        FD_READ          Want to receive notification of readiness for reading 
        FD_WRITE         Want to receive notification of readiness for writing 
        FD_OOB           Want to receive notification of the arrival of out-of-band data 
        FD_ACCEPT        Want to receive notification of incoming connections 
        FD_CONNECT       Want to receive notification of completed connection or multi-point join operation 
        FD_CLOSE         Want to receive notification of socket closure 
        */
int SocketObject::BindToWindow(HWND win, unsigned int MessageID,long Events)
{
  return(WSAAsyncSelect(skSocket,win,MessageID,Events));
}

int operator<< (SocketObject &sock, PACKET &Packet)
{
   return(sock.Send( Packet, NULL )); 
}
int operator>> (SocketObject &sock, PACKET &Packet)
{
   return(sock.Recv( Packet, NULL )); 
}
