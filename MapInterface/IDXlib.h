#ifndef IDX_LIB
#define IDX_LIB

#include <fstream>
#include "ClassDefs.h"
#include "MordBase.h"


using namespace std;


#pragma once

class IDX_DB
{
private:

	fstream file;
    char idxpath[512];

public:

	struct Entry
	{
       char Entry_Type;
	   unsigned char NameLen;
       char Entry_Name[50];
	   unsigned long startPos;
       unsigned int EntryLen;
       unsigned int EntryLenCom;
       unsigned int Width;
       unsigned int Height;
       unsigned long IDXLoc;
       unsigned int OffsetX;
       unsigned int OffsetY;

       Entry()
       {
          memset((void*)this,0x00,sizeof(*this));
       }

	};

	struct SectionHeader
	{
       unsigned long startPosCom;
	   unsigned long startPos;
	   unsigned int Unknown;

	   unsigned int SectionLenCom;
	   unsigned int SectionLen;

	   unsigned int Offset;
	   unsigned long fileOffset;
	   unsigned long EntryOffset;

	   int NumElements;
	   LIST<Entry> Elements;

	   void CalcSize(); // Runs through all entries and calculates the total size
	                    // for com and non com
	   SectionHeader()
	   {
         memset((void*)this,0x00,sizeof(*this));
	   }
	   ~SectionHeader()
	   {
		   Elements.Clear();
	   }
	};

   LIST<SectionHeader> Sections;



public:

   bool bit_16;

   void Set_Bit_16()
   {bit_16=true;}
   void Set_Bit_8()
   {bit_16=false;}

   void Set_Path(char *idxpath2 = NULL)
   {
       strcpy(idxpath,idxpath2);
   }

   SectionHeader* GetSection(unsigned long entryNum); // Gets the section for an entry
   Entry* GetEntry(unsigned long entryNum); // Get the entry structure for the numbered entry
   
   // Gets the section header AS WELL as the entry class
   bool GetAll(unsigned long entryNum, Entry ** entry, SectionHeader ** section);


   bool AlterEntry(unsigned long entrynum,
	               unsigned long newsize,
	               unsigned long newsizecom);

   bool AddEntry(unsigned long entrynum,
	               unsigned long size,
	               unsigned long sizecom);
   
   unsigned long Create_New_Section(); // Returns the section number
   unsigned long GetLastEntry();       // Returns the last entry number

   bool LoadIdx();
   bool SaveIdx();

   IDX_DB()
   {
   }
   ~IDX_DB()
   {
     Sections.Clear();
   }

};


#endif