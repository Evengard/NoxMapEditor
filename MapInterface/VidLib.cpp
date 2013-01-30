#include "VidLib.h"

/********************************************************/
/********************************************************/




/********************************************************/
/********************************************************/
NxImage* VideoBag::Extract(fstream * file,
								 IDX_DB::SectionHeader * Header,
								 IDX_DB::Entry * Entry, char * outFile)
{
   
   Entry_768 entry;
   unsigned char *map,*map2;
   map= new unsigned char [Header->SectionLen];
   map2= new  unsigned char [Header->SectionLen];

   memset(map,0x0,Header->SectionLenCom);
   memset(map2,0x0,Header->SectionLen);

   file->seekg(Header->startPosCom,ios::beg);
   file->read((char *)map,Header->SectionLenCom);

   nxzDecrypt(map,Header->SectionLenCom,map2,Header->SectionLen);

   memcpy((void*)&Entry->OffsetX,(const void*)&*(map2+Entry->startPos+8),sizeof(int));
   memcpy((void*)&Entry->OffsetY,(const void*)&*(map2+Entry->startPos+12),sizeof(int));

   if(outFile)
   {
      char filename2[255];
	  sprintf(filename2,"%s",outFile);
      if(bmpfile.Save_BMP(Entry->Entry_Type,filename2,map2,Entry->startPos,entry))
	  {
	   int val = true;	  
	  }
      //TEST FOR FAILURE HERE!!
   }

   if(map!=NULL)
      {delete [] map;}

   if( !outFile )
   { 
	   NxImage * image = new NxImage;
	  
	   switch(Entry->Entry_Type)
   {
	   case 3: 
	   case 2:
	   case 4:
	   case 5:	
               map = bmpfile.Extract_768_Clean( Entry->startPos, map2, &entry);
			   break;
		case 0: 
			   map=bmpfile.Extract_Tile_Clean( Entry->startPos, map2, &entry);
			   break;
		case 1:
		case 6:	
		default: 
			break;
}
 
	   // Is now RGB format
	   Entry->Width = entry.X;
	   Entry->Height = entry.Y;
	   image->Load(entry.X,entry.Y,Entry->OffsetX,Entry->OffsetY,map,Entry->Entry_Name);
	   
    
	   if(map2!=NULL)
          {delete [] map2;}
	   return(image);
   }

	   Entry->Width = entry.X;
	   Entry->Height = entry.Y;
   
	 if(map2!=NULL)
      {delete [] map2;}

   return(NULL);
}
