/************************************************************************
// Name: Mordbase
// Goal: Fully functional and error free automated database solution
//
// Uses: LIST and other properties, is templated for multipurpose use
//
************************************************************************/
#ifndef MORDBASE_H
#define MORDBASE_H

#include "ClassDefs.h"
#include <fstream>

using namespace std;

//STD ERROR CODES

#define MOB_OK                       0 // All is well
#define MOB_ERROR                    1 // Generic error message
#define MOB_ERROR_FILE_NOT_FOUND     2 // File not found
#define MOB_ERROR_FILE_NO_CREATE     3 // Cannot create file
#define MOB_BAD_LOCATION             4 // No entry at that location



template <typename var>
class MORDBASE
{
private:
   
   long ID_MAX;
   LIST<var> Entries;

public:


   short AddEntry(var Entry)
   {
	   if( Entries.numNodes < 0 )
		   return(MOB_ERROR);

         Entries.Add(Entry,ID_MAX);
		 ID_MAX++;

	   return(MOB_OK);
   }

   var * GetEntry(long loc)
   {
	   if( loc > Entries.numNodes || loc < 0 )
		   return(MB_BAD_LOCATION);
	   
	   return(Entries.Get(loc));
   }

   long GetEntryCount()
   {
     return(Entries.numNodes);
   }

   short RemoveEntry( long loc = (-1) )
   {
	   if( loc > Entries.numNodes || loc < 0 )
		   return(MOB_BAD_LOCATION);

	   Entries.Remove(loc);
	   return(MOB_OK);
   }

/*********************************************************************************/
//
//
//
//
/*********************************************************************************/
unsigned char LoadDBFromFile(char *fileName) // Returns error code
{
  ifstream ifile;
  ifile.open(fileName,ios::in | ios::binary);
  
  Entries.Clear();
  long len = NULL;
  long i = NULL;

  ID_MAX = 0;
  Entries.numNodes = 0;

  ifile.read((char*)&ID_MAX,sizeof(long));
  ifile.read((char*)&len,sizeof(long));
  var Entry;

  for(; i < len; i++)
  {
    memset((void*)&Entry,0x00,sizeof(var));
	ifile.read((char*)&Entry,sizeof(Entry));
	Entries.Add(Entry);
  }

  ifile.close();
  return(MB_OK);
}

/*********************************************************************************/
//
//
//
//
/*********************************************************************************/
short SaveDBToFile(char *fileName)   // Returns Error code
{
   ofstream ofile;
   ofile.open(fileName,ios::out,ios::binary);
   
   ofile.write((const char*)&ID_MAX,sizeof(long));
   ofile.write((const char*)&Entries.numNodes,sizeof(long));

   int i;
   for(i = 0; i < Entries.numNodes; i++)
   {
	   ofile.write((const char*)&*(Entries.Get()),sizeof(var));
   }

   Entries.ClearGet();

   ofile.close();
 return(MB_OK);
}


MORDBASE()
{
  memset((void*)this,0x00,sizeof(*this));

}

};

#endif