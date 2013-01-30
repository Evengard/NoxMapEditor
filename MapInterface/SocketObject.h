//###################################################################################
//#																					#
//#		            	Chapter 7 - Socket Object Class Definition					#		
//#																					#
//#						Class to Handle Client/Server Communication					#
//#																					#
//#						Todd Barron, 08/06/2000										#
//#																					#
//###################################################################################
  //BOT myBot;
  //myBot.Send_Console_Msg("sysop set sysop 0000",2);
  //myBot.Send_Console_Msg("sysop racoiaws",2);
  //myBot.Send_Console_Msg("sysop load riverrun",2);

#ifndef SOCKET_OBJECT

#define SOCKET_OBJECT

// Windows Sockets Include, Also need ws2_32.lib included in project
#include <winsock.h>
#include <stdio.h>
#include <iostream>

using namespace std;

// Get packet codes
#define WSAGETSELECTERROR(lParam)       HIWORD(lParam)
#define WSAGETSELECTEVENT(lParam)       LOWORD(lParam)

// Define the Windows Message for my socket class
#define WM_SOCKET INT_MAX

//*******************************************************************************
//
// Class: PACKET
//
// Author: Joshua Eugene Statzer
//
// Desc:
//       This class interfaces with the SOCKET_OBJECT class to send and recieve
//       information over socket connections. It supports data reading/writing
//       and has overloaded stream operators in the SOCKET_OBJECT class for it
//       to be read and written to/from.
//
//*******************************************************************************
const int MAX_DATA = 512;

class PACKET
{
public:
	    // static const int MAX_DATA = 512;

public:

         unsigned char PACKET_DATA[MAX_DATA];
		 int PACKET_LEN;
		 int WLoc;
		 int RLoc;

		 bool iWLoc(int val)
		 {
            if( (WLoc + val) > MAX_DATA )
			{
				WLoc = 0;
			    return(false);
			}

			WLoc += val;
			return(true);
		 }

		 bool iRLoc(int val)
		 {
            if( (RLoc + val) > MAX_DATA )
			{
				RLoc = 0;
			    return(false);
			}

			RLoc += val;
			return(true);
		 }

public:

         unsigned char* GetData()
         {return((unsigned char*)&PACKET_DATA);}

		 int GetDataLen()
		 {return(PACKET_LEN);}

		 void SetDataLen(int val)
		 {PACKET_LEN = val;}

		 int TellR()
		 {return(RLoc);}

		 int TellW()
		 {return(RLoc);}

		 void SeekR(int Loc = NULL)
		 {RLoc = Loc;}

		 void SeekW(int Loc = NULL)
		 {WLoc = Loc;}

		 bool EOP()
		 {
			 return(RLoc >= PACKET_LEN);
		 }

		friend ostream& operator<< (ostream &stream, PACKET &Packet);
        friend char * operator>> (PACKET &Packet, char *buffer);
        
		PACKET operator + (PACKET &aPacket);

		int operator +=(int val)
		{
		  iRLoc(val);
		  return(RLoc);
		}
		int operator ++()
		{
		  iRLoc(1);
		  return(RLoc);
		} 
		int operator++(int)
        {
          return(++*this); 
        }

		int operator -=(int val)
		{
		  RLoc -= val;
		  return(RLoc);
		}
		int operator --()
		{
		  RLoc --;
		  return(RLoc);
		}
		int operator--(int)
        {
          return(--*this); 
        }

	    PACKET()
	    {
           Clear();
	    }
       
	    void Clear()
	    {
           memset((void*)&*this,0x00,sizeof(*this));
	    }

        void operator ^ ( int XOR_KEY );


        template <typename var>
        bool GET(var & Variable, int Loc =(-1));

	    template <typename var>
	    bool PUT(var Variable, int Loc=(-1) );
	
	    template <typename var>
	    bool READ(var *Variable, int Len, int Loc=(-1) );
	  
	    template <typename var>
	    bool WRITE(var *Variable, int Len, int Loc=(-1) );
		

};

// Class Object
class SocketObject  
{
	private:

	public:
		SOCKET						skSocket;
		int							iStatus;
		bool HAS_CONNECTION;
		
		// Constructor
		SocketObject();
		SocketObject(char* szServerAddress, int iPort,bool Sock_Stream);
		void Init();

		// Tests whether a returned value indicates that a socket is closed
		int Test(int val);

		// Destructor
		~SocketObject();

		// Accept a client's request to connect
		bool Accept(SocketObject& skAcceptSocket);
		// Listen for clients to connect to
		int Listen(void);
		// Open a server listening port
		int Bind(int iPort);
		// Close connection
		void Disconnect();

		// Connect to a server
		// If Sock_Stream then it is NOT datagram
		bool Connect(char* szServerAddress, int iPort,bool Sock_Stream = false);
        int UnblockSocket();
        /*
        FLAGS
        FD_READ          Want to receive notification of readiness for reading 
        FD_WRITE         Want to receive notification of readiness for writing 
        FD_OOB           Want to receive notification of the arrival of out-of-band data 
        FD_ACCEPT        Want to receive notification of incoming connections 
        FD_CONNECT       Want to receive notification of completed connection or multi-point join operation 
        FD_CLOSE         Want to receive notification of socket closure 
        */

        int BindToWindow(HWND win, unsigned int MessageID,long Events = FD_READ);


		int Recv(char *szBuffer, int iBufLen, int iFlags = NULL);
		int Recv(PACKET &Packet, int iFlags = NULL);
		int Send(char *szBuffer, int iBufLen, int iFlags = NULL);
		int Send(PACKET &Packet, int iFlags = NULL);

		friend int operator<< (SocketObject &sock, PACKET &Packet);
        friend int operator>> (SocketObject &sock, PACKET &Packet);

};



template <typename var>
bool PACKET::GET(var & Variable, int Loc )
	    {
			int temLoc = Loc;
		    if( Loc == (-1) )
               temLoc = RLoc;

           Variable = *(((var*)&PACKET_DATA[ temLoc]) );
		   if( (Loc == (-1)) )
		      return(iRLoc(sizeof(var)));
		   else
		      return(true);

	    }
	    template <typename var>
	    bool PACKET::PUT(var Variable, int Loc )
	    {
			int temLoc = Loc;
		    if( Loc == (-1) )
			    temLoc = WLoc;

		    int END_SIZE,SIZE;
		    if( (END_SIZE = (temLoc + sizeof(var))) > MAX_DATA )
			    return(false);

		    SIZE = sizeof(var);

            *(((var*)&PACKET_DATA[ temLoc]) ) = Variable;

		    if( END_SIZE > PACKET_LEN )
			{
			  iWLoc(END_SIZE - PACKET_LEN);
			  PACKET_LEN += END_SIZE - PACKET_LEN;
			}

		  //  if( (temLoc + SIZE) > WLoc /*&& Loc == (-1)*/ )
		    //  return(iWLoc((temLoc + SIZE) - (WLoc)));
			//else
			  return(true);
	    }
	    template <typename var>
	    bool PACKET::READ(var *Variable, int Len, int Loc )
	    {
			int temLoc = Loc;
		    if( Loc == (-1) )
			    temLoc = RLoc;

		    memset((void*)Variable,0x00,Len);
		    memcpy((void*)Variable, (const void*)&PACKET_DATA[temLoc], sizeof(var)*Len);

		   if( Loc == (-1) )
		      return(iRLoc(sizeof(var)*Len)); 
		   else
		      return(true);
	    }
	    template <typename var>
		bool PACKET::WRITE(var *Variable, int Len, int Loc )
	    {
			int temLoc = Loc;
		    if( Loc == (-1) )
			    temLoc = WLoc;

		    int END_SIZE,SIZE;
		    if( (END_SIZE = (temLoc + (Len * sizeof(var)) ) ) > MAX_DATA )
			    return(false);

		    SIZE = END_SIZE - temLoc;

		    memset((void*)&PACKET_DATA[temLoc],0x00,Len);
		    memcpy((void*)&PACKET_DATA[temLoc], (const void*)Variable, SIZE);
		   
		    if( END_SIZE > PACKET_LEN )
			{
			   iWLoc( END_SIZE - PACKET_LEN );
		       PACKET_LEN += END_SIZE - PACKET_LEN;
			}

			//if( (temLoc + SIZE) > WLoc /*&& Loc == (-1)*/ )
		    //  return(iWLoc((temLoc + SIZE)-(WLoc)));
			//else
			  return(true);
			
	    }


#endif









