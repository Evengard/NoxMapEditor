#ifndef CLASS_DEFS_H
#define CLASS_DEFS_H

#include <fstream>
#include <windows.h>
using namespace std;

/////////////////////////////////////////////////////////////////////
//
//  This structure is used to hold bitmap data after RGB conversion
//
/////////////////////////////////////////////////////////////////////
struct RGBImage // RGB format Bitmap holding structure
{
  short X;
  short Y;
  char * Buffer;
  long bufLen;

  void FillImage( short Width, short Height, char* Data, long len)
  {
    X = Width;
	Y = Height;
	Buffer = Data;
	bufLen = len;
  }

};




// CHAT LIST
class CHAT_LIST
{
private:

	struct NODE
	{
	   DWORD Color;
       char msg[255];
	   NODE * next;
	   NODE()
	   {
         memset((void*)&*this,NULL,sizeof(*this));
	   }
	};

	NODE *first;
	NODE *last;

public:
    
	static const int MAX_MSGS = 11;
	long numNodes;

	void Add(char msg[], DWORD Color)
	{
	  NODE ** node = &first;

      if( first )
	  {
		  node = &last->next;
	      numNodes++;
	  }
	  *node = new NODE;
	  last = *node;

	  if( !first )
		  first = *node;

	  strcpy(last->msg,msg);
	  last->Color = Color;

	}

	char * GET_MSG( long number, DWORD & Color )
	{
	   NODE * tem = first;
       for(long i = 0; i < number && tem; i++, tem = tem->next);
	   
       if( tem )
	   {
          Color = tem->Color;
	      return( tem->msg );
	   }
	   else
	   {
          Color = NULL;
		  return( NULL );
	   }
	}
	void Clear()
	{
		NODE *tem = first;
		for(; first; delete first, first = tem)
		    tem = first->next;
		
		numNodes = 0;
		first = NULL;
		last = NULL;
	}

	void Export()
	{
		ofstream ofile;
		ofile.open("Chat_Log.txt",ios::out | ios::app);

		if( numNodes )
		   ofile << "\n" << "CHAT_LOG\n";

		NODE *tem = first;
		for(; first; delete first, first = tem)
		{
			ofile << first->msg << "\n";
		    tem = first->next;
		}
        ofile.close();
		numNodes = 0;
		first = NULL;
		last = NULL;
	}

	CHAT_LIST()
	{
      memset((void*)&*this,NULL,sizeof(*this));
	  first = NULL;
	  last = NULL;
	}
	~CHAT_LIST()
	{
		NODE *tem = first;
		for(; first; delete first, first = tem)
		    tem = first->next;
      //Export();
	}		
};




//********************************************************************//
//********************************************************************//
//********************************************************************//
	struct PATH_NODE
	{
       int x;
	   int y;

	   PATH_NODE()
	   {
         x = NULL;
		 y = NULL;
	   }
	};
//********************************************************************
//
//  Generic list class
//
//********************************************************************
template <typename var>
class LIST
{

protected:

	struct NODE
	{ 
		var msg;
		long ID;
		NODE * next;
		NODE * prev;
		NODE()
		{
          next = NULL;
		  prev = NULL;
		  ID = NULL;
		}
	};
public:
	NODE *first;
	NODE *last;
    NODE *cur;

public:
    
	long numNodes;

	void Remove( long number )
	{
       NODE * tem = first;
	   NODE * temback = NULL;

	   long i;
       for(i = 0; i < number && tem; i++, temback = tem,tem = tem->next);

	   if( tem && i == number )
	   {
          if( temback )
			  temback->next = tem->next;
          if( tem == first )
			  first = tem->next;
		  if ( tem == cur )
			  cur = first;
		  if ( tem == last )
			  last = tem->prev;
		  delete tem;
		  numNodes--;
	   }

	}
	void Add(var & val, long ID = NULL)
	{
	  bool Is_First = true;
	  NODE ** node = &first;

      if( first )
	  {
		  node = &last->next;
		  Is_First = false;
	  }

	  *node = new NODE;
	  last = *node;

	  last->msg = val;
	  last->ID = ID;
      numNodes++;

	  if( Is_First )
		  cur = first;
	}

	var * Get( long number = (-1) )
	{
	   if( number == (-1) )
	   {
		   if( !cur )
		   {
               cur = first;
			   return( NULL );
		   }
		   var *tem = &cur->msg;
           cur = cur->next;
		   return( tem );
	   }

	   NODE * tem = first;
       for(long i = 0; i < number && tem; i++, tem = tem->next);
	      return( &tem->msg );

	}
	void ClearGet()
	{
      this->cur = first;
	}
    void Clear()
	{
		NODE *tem = first;
		for(; first; delete first, first = tem)
		    tem = first->next;
		last = NULL;
		numNodes = NULL;
	}
public:

    var * Find( long ID )
	{  
	   NODE * tem = first;
       for(; tem && ID != tem->ID; tem = tem->next);

	   if( tem )
	      return(&tem->msg);
	   else
	      return(NULL);
	}

	LIST()
	{
      memset((void*)&*this,NULL,sizeof(*this));
	  first = NULL;
	  last = NULL;
	  cur = first;
	}
	~LIST()
	{
       Clear();
	}		
};

//********************************************************************
//
//  Fast list class, uses another list to make searching faster.
//
//********************************************************************
template <typename var>
class FASTLIST : public LIST<var>
{

private:

	LIST<NODE *> SpeedList; // Stores 1000 places to increase speed.

public:
    


	void Remove( long number )
	{
	  LIST::Remove( number );	  
	  if( (numNodes / 1000) < SpeedList.numNodes)
		   SpeedList.Remove(SpeedList.numNodes);

	}
	void Add(var & val, long ID = NULL)
	{
	  LIST::Add( val, ID );
	  if( !((numNodes%1000) - 1) )
		  SpeedList.Add(last);
	}

	var * Get( long number = (-1) )
	{

	   if( number == (-1) || number < 1000)
		   return(LIST::Get());
 
	   int skipCount = 0;
	   int leftCount = number;
	   NODE * tem = first;

	   if( number > 1000 )
	   {
          skipCount = number / 1000;
          tem = *SpeedList.Get(skipCount);
		  leftCount = number % 1000;
	   }

       for(long i = 0; i < leftCount && tem; i++, tem = tem->next);
	      return( &tem->msg );
	}
	void ClearGet()
	{
      this->cur = first;
	}
    void Clear()
	{
		NODE *tem = first;
		for(; first; delete first, first = tem)
		    tem = first->next;
		last = NULL;
		numNodes = NULL;

		SpeedList.Clear();
	}
public:

    var * Find( long ID )
	{  
	   NODE * tem = first;
       for(; tem && ID != tem->ID; tem = tem->next);

	   if( tem )
	      return(&tem->msg);
	   else
	      return(NULL);
	}

	FASTLIST()
	{
      memset((void*)&*this,NULL,sizeof(*this));
	  first = NULL;
	  last = NULL;
	  cur = first;
	}
	~FASTLIST()
	{
       Clear();
	}		
};
//********************************************************************
//
//  Action Queue for packet sends
//
//********************************************************************
#define EMPTY_QUEUE (-1)

struct ARG_STRUCT
{
  int MSG_ID;
  long arg1;
  long arg2;
};

class QUEUE : private LIST<ARG_STRUCT>
{
public:
   void Add(int MSG_ID, long arg1 = NULL, long arg2 = NULL )
   {
     ARG_STRUCT tem;
	 tem.MSG_ID = MSG_ID;
	 tem.arg1 = arg1;
	 tem.arg2 = arg2;
	 LIST::Add(tem);
   }
   long GetCount()
   {
	  return( numNodes );
   }
   ARG_STRUCT Get()
   {
      ARG_STRUCT tem;
	  if( !LIST::numNodes )
	  {
          tem.MSG_ID = EMPTY_QUEUE;
	      return(tem);
	  }

	  tem = *(LIST::Get( 0 ));
	  LIST::Remove( 0 );
	  return( tem );
   }
   void Clear()
   {
	  LIST::Clear();
   }
   QUEUE()
   {
	  LIST();
      memset((void*)&*this,NULL,sizeof(*this));
   }
   ~QUEUE()
   {
	   LIST::Clear();
   }

};
#endif