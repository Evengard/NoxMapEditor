
#include "IDXlib.h"


IDX_DB::SectionHeader* IDX_DB::GetSection(unsigned long entryNum) // Gets the section for an entry
{
	long val = 0;

    SectionHeader *tem = NULL;
	for( tem = Sections.Get();
		 tem && ((val+tem->NumElements) < entryNum);
		 tem = Sections.Get(), val+=tem->NumElements);
	Sections.ClearGet();

    if( val <= entryNum && tem)
		return(tem);
	else
		return(NULL);
}
IDX_DB::Entry* IDX_DB::GetEntry(unsigned long entryNum) // Get the entry structure for the numbered entry
{	
	long val = 0;

	SectionHeader *tem = NULL;
	for( tem = Sections.Get();
		 tem && ((val+tem->NumElements) < entryNum);
		 tem = Sections.Get(), val+=tem->NumElements);
	Sections.ClearGet();

    if( val <= entryNum && tem)
		return(tem->Elements.Get(entryNum - val));
	else
		return(NULL);

}
   
// Gets the section header AS WELL as the entry class
bool IDX_DB::GetAll(unsigned long entryNum, Entry ** entry, SectionHeader ** section)
{
	long val = 0;
    
	SectionHeader *tem = NULL;
	for( tem = Sections.Get();
		 tem && ((val+tem->NumElements) < entryNum);
		 val+=tem->NumElements, tem = Sections.Get());
	Sections.ClearGet();

    if( val <= entryNum && tem)
	{
		*entry = tem->Elements.Get(entryNum - val - 1);
		*section = tem;
	   return(true);
	}
	else
	{
		entry = NULL;
		section = NULL;
	   return(false);
	}
}

////////////////////////////////////////////////////////////////////
//
//
////////////////////////////////////////////////////////////////////
bool IDX_DB::SaveIdx()
{
	int i = 0;
	file.open(idxpath,ios::out | ios::binary);
	
	//EB BC ED FA D4 39 32 00 (NUM SECTIONS *int*)  00 80 00 00
	//D5 7A 00 00 61 1F 02 00
	char sub[] = {0xEB,0xBC,0xED,0xFA,0xD4,0x39,0x32,0x00};
	char type16[] = {0x00,0x80,0x00,0x00,0xD5,0x7A,0x00,0x00};
	char type8[] = {0x00,0x80,0x00,0x00,0x00,0x80,0x00,0x00};
	char sub2[] = {0x61,0x1F,0x02,0x00};
	file.write(sub,sizeof(sub));
	file.write((const char*)&Sections.numNodes,sizeof(int));
	
	if( bit_16 )
	   file.write(type16,sizeof(type16));
	else
	   file.write(type8,sizeof(type8));

	file.write(sub2,sizeof(sub2));


	SectionHeader * Header = Sections.Get();
	Entry * Element= NULL;

    while( Header )
	{
	  file.write((const char*)&Header->Unknown,sizeof(Header->Unknown));
      file.write((const char*)&Header->SectionLen,sizeof(Header->SectionLen));
      file.write((const char*)&Header->SectionLenCom,sizeof(Header->SectionLenCom));
      file.write((const char*)&Header->NumElements,sizeof(Header->NumElements));

	  Element = Header->Elements.Get();
      while( Element )
      {
       
	    file.put(Element->NameLen);
		file.write((const char*)&Element->Entry_Name,Element->NameLen-1);
		file.put(0x00);
		file.write((const char*)&Element->Entry_Type,sizeof(Element->Entry_Type));
		file.write((const char*)&Element->EntryLen,sizeof(Element->EntryLen));		
		file.write((const char*)&Element->EntryLenCom,sizeof(Element->EntryLenCom));
		Element = Header->Elements.Get();
      }

      Header->Elements.ClearGet();
      Header = Sections.Get();
	}

	Sections.ClearGet();

    file.close();
  return(true);
}

////////////////////////////////////////////////////////////////////
//
//
////////////////////////////////////////////////////////////////////
bool IDX_DB::LoadIdx()
{

	char filebuff[512];
	sprintf(filebuff,"%s",idxpath);
	file.open(filebuff,ios::in | ios::binary);

long count=0;
int i;
int totalLen = 0;
char a;
SectionHeader Header;
Entry Element;

long Entry_Offset=0;
long pos=0;

long SectionOffset = 0;
long SectionOffsetCom = 0;

unsigned char *buff;

file.seekg(0,ios::end);
long len=file.tellg();
file.seekg(0,ios::beg);

buff = new unsigned char [len+1];
file.read((char*)buff,len);
file.close();

pos += 8;
memcpy((void*)&totalLen,(const void*)&*(buff+pos),sizeof(totalLen));
pos += 8;


if( *(buff+pos) == 0xD5 )
    bit_16 = true;
else
    bit_16 = false;


pos += 8;



while( Sections.numNodes < totalLen )
{

	
      Entry_Offset=0;

      memcpy((void*)&Header.Unknown,(const void*)&*(buff+pos),sizeof(Header.Unknown));
	  pos+=sizeof(Header.Unknown);

      memcpy((void*)&Header.SectionLen,(const void*)&*(buff+pos),sizeof(Header.SectionLen));
	  pos+=sizeof(Header.SectionLen);
 
      memcpy((void*)&Header.SectionLenCom,(const void*)&*(buff+pos),sizeof(Header.SectionLenCom));
	  pos+=sizeof(Header.SectionLenCom);

      memcpy((void*)&Header.NumElements,(const void*)&*(buff+pos),sizeof(Header.NumElements));
	  pos+=sizeof(Header.NumElements);
	  


   if(Header.NumElements<0)
      Header.NumElements = 1;



   for(i=0; i<Header.NumElements; i++)
   {
       
      memcpy((void*)&a,(const void*)&*(buff+pos),sizeof(a));
	  pos+=sizeof(a);

	  Element.NameLen = a;
      
            memcpy((void*)&Element.Entry_Name,(const void*)&*(buff+pos),(a-1));
	        pos+=(a-1);
			pos++;

            memcpy((void*)&Element.Entry_Type,(const void*)&*(buff+pos),sizeof(Element.Entry_Type));
	        pos+=sizeof(Element.Entry_Type);			
			
            memcpy((void*)&Element.EntryLen,(const void*)&*(buff+pos),sizeof(Element.EntryLen));
	        pos+=sizeof(Element.EntryLen);            
			
            memcpy((void*)&Element.EntryLenCom,(const void*)&*(buff+pos),sizeof(Element.EntryLenCom));
	        pos+=sizeof(Element.EntryLenCom);             	

	  Element.startPos = Entry_Offset;
	  Entry_Offset += Element.EntryLen;
	  Header.Elements.Add(Element);
	  memset((void*)&Element,0x00,sizeof(Entry));
   }
      Header.startPos = SectionOffset;
	  Header.startPosCom = SectionOffsetCom;
      SectionOffset += Header.SectionLen;
      SectionOffsetCom += Header.SectionLenCom;
      Sections.Add(Header);
	  memset((void*)&Header,0x00,sizeof(SectionHeader));
	}


  delete [] buff;
  return(true);
}



