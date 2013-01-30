#include "BmpLib.h"

bool BMPFILE::Save_BMP(int Type,char * filename,unsigned char Data[],long offset,Entry_768 & entry)
{

   bool Saved = true;


   unsigned char *bmp = NULL;
   unsigned char *buffer = NULL;
   long newsize;

   int namepoint;
   namepoint=strlen(filename);
   namepoint-=4;

   *(filename+namepoint+1)='b';
   *(filename+namepoint+2)='m';
   *(filename+namepoint+3)='p';
   *(filename+namepoint+4)=0x00;
   //Entry_768 entry;
        

   switch(Type)
   {
	   case 3: 
	   case 2:
	   case 4:
	   case 5:	
               bmp=Extract_768(offset,Data,&entry);
              if(bit_16)
		      {
                 buffer=ConvertRGBToBMPBuffer_16 (bmp,entry.X,entry.Y, &newsize);
                 if(!SaveBMP_16(buffer,entry.X,entry.Y,newsize,filename))
		            {Saved = false;}
		      }
		      else
		      {
		         buffer=ConvertRGBToBMPBuffer(bmp,entry.X,entry.Y,newsize);
                 if(!SaveBMP(buffer,entry.X,entry.Y,newsize,filename))
		            {Saved = false;}
		      }
	   break;


	case 0: 
			bmp=Extract_Tile(offset,Data,&entry);
            if(bit_16)
		    {
               buffer=ConvertRGBToBMPBuffer_16 (bmp,entry.X,entry.Y,&newsize);
               if(!SaveBMP_16(buffer,entry.X,entry.Y,newsize,filename))
		       {Saved = false;}
	       	}
		    else
		    {
		       buffer=ConvertRGBToBMPBuffer (bmp,entry.X,entry.Y,newsize);
               if(!SaveBMP(buffer,entry.X,entry.Y,newsize,filename))
		         {Saved = false;}
		    }
	break;

    case 1:
	case 6:	
	default: 
		break;
}
	
if(bmp!=NULL)
delete [] bmp;

if(buffer!=NULL)
delete [] buffer;

return(Saved);
}

/*************************************************************************


*************************************************************************/
unsigned char * BMPFILE::Extract_Tile(long offset, unsigned char *Data, Entry_768 *entry )
{

unsigned char *bmp;
entry->X=45;
entry->Y=46;

if(!bit_16)
{
    
    bmp=new unsigned char [46*45*3];
    
	for(int k=0; k<46*45; k++)
	{
		*(bmp+k*3)=200;
		*(bmp+k*3+1)=200;
		*(bmp+k*3+2)=200;
	}

long count=0;



for (int i=0;i<24;i++) {
  for (int j=23-i;j<22+i;j++) {


    *(bmp+(((i*45-45)+j)*3))=(Palette[((int)*(Data+offset+count))*3]);
    *(bmp+(((i*45-45)+j)*3)+1)=(Palette[((int)*(Data+offset+count))*3+1]);
    *(bmp+(((i*45-45)+j)*3)+2)=(Palette[((int)*(Data+offset+count))*3+2]);
   count++;
  }
}

for (int i=23;i<46;i++) {
  for (int j=i-23;j<68-i;j++) {
    *(bmp+((i*45-45+j)*3))=(Palette[((int)*(Data+offset+count))*3]);
    *(bmp+((i*45-45+j)*3)+1)=(Palette[((int)*(Data+offset+count))*3+1]);
    *(bmp+((i*45-45+j)*3)+2)=(Palette[((int)*(Data+offset+count))*3+2]);
   count++;
  }
}
	}
else
{

   bmp=new unsigned char [46*45*2];
   short trans=20;
   trans=trans<<5;
   trans+=20;
   trans=trans<<5;
   trans+=20;
   for(int k=0; k<46*45; k++)
   {
   memcpy((void*)&*(bmp+k*2),(const void*)&trans,sizeof(short));
	   }


long count=0;



for (int i=0;i<24;i++) {
	for (int j=23-i;j<22+i;j++) {

    *(bmp+(((i*45-45)+j)*2))=*(Data+offset+(count*2));
    *(bmp+(((i*45-45)+j)*2)+1)=*(Data+offset+(count*2)+1);
	count++;
  }
}

for (int i=23;i<46;i++) {
  for (int j=i-23;j<68-i;j++) {

    *(bmp+(((i*45-45)+j)*2))=*(Data+offset+(count*2));
    *(bmp+(((i*45-45)+j)*2)+1)=*(Data+offset+(count*2)+1);
	count++;
  }
}

	}


return(bmp);

	}

/*************************************************************************





**************************************************************************/

unsigned char * BMPFILE::Extract_768(long offset, unsigned char Data[], Entry_768 *entry )
{


unsigned char *bmp=NULL;
long count2=0;
long index=0;
int length=0;


unsigned int width = *((int*)(Data+offset));
unsigned int height = *((int*)(Data+offset+4));
entry->X = width;
entry->Y = height;
int i = 0,tem = 0;

bmp = new unsigned char [width*height*4];

if(bit_16)
{
	
short temtrans=0;

if(ref.rgbBlue==255 && ref.rgbGreen==0 && ref.rgbRed==0)
{

temtrans+=31;

}
else if(ref.rgbBlue==0 && ref.rgbGreen==255 && ref.rgbRed==0)
{
	
temtrans=temtrans<<5;
temtrans+=31;
temtrans=temtrans<<5;

	
}
else if(ref.rgbBlue==0 && ref.rgbGreen==0 && ref.rgbRed==255)
{
	temtrans+=31;
	temtrans=temtrans<<10;
	
}
else if(ref.rgbBlue==255 && ref.rgbGreen==0 && ref.rgbRed==255)
{
	
temtrans+=31;
temtrans=temtrans<<10;
temtrans+=31;

	
}
else if(ref.rgbBlue==255 && ref.rgbGreen==255 && ref.rgbRed==255)
{temtrans=~temtrans;}
else
{}
	
for(unsigned int i=0; i<width*height; i++)
{
memcpy((void*)&bmp[i*2],(const void *)&temtrans,sizeof(temtrans));
}		
}
else
{
for(unsigned int i=0; i<width*height*3; i++)
{
bmp[i]=ref.rgbRed;
bmp[++i]=ref.rgbGreen;
bmp[++i]=ref.rgbBlue;
}
}

//ofstream oifile;

int temp=0;
short tems=0;
RGBQUAD color;


long count=0;

for(count=17; ((int)*(Data+offset+count)) != 0 && count<width*height*2; count++)
{
  temp=0;
  temp=(*(Data+offset+count));
	switch(temp)
	{

    case 0x04:
    case 0x14:
    case 0x24:
    case 0x34:
    case 0x44:
	case 0x54:
	case 0x64:
        count++;
		length = ((int)*(Data+offset+count));
		while(length > 0)
		{
			count++;
			if(!bit_16)
			{
			bmp[index*3]=(Palette[((int)*(Data+offset+count-1))*3] ^ temp/10);

			bmp[index*3+1]=(Palette[((int)*(Data+offset+count-1))*3+1] ^ temp/10);

			bmp[index*3+2]=(Palette[((int)*(Data+offset+count-1))*3+2] ^ temp/10);
			}
			else
			{
            color.rgbRed=((int)*(Data+offset+count-1))*3;
            color.rgbGreen=((int)*(Data+offset+count-1))*3+1;
            color.rgbBlue=((int)*(Data+offset+count-1))*3+2;

            tems=Color_24_16(color);
            memcpy((void *)&bmp[index*2],(const void*)&tems,sizeof(short));
			}
			index++;
			length--;
		}
		break;

	case 3:
		break;
	case 2:

		if(bit_16)
		{

		count++;
		length = (*(Data+offset+count));
				
		
		while(length > 0)
		{
			count++;
			
            memcpy((void *)&bmp[index*2],(const void*)&*((short*)(Data+offset+count)),sizeof(short));
		    index++;
			length--;
			
			count++;
		}
			}
		else
		{
		count++;
		length = (*(Data+offset+count));
				
		
		while(length > 0)
		{
			count++;
			bmp[index*3]=(Palette[((int)*(Data+offset+count))*3]);

			bmp[index*3+1]=(Palette[((int)*(Data+offset+count))*3+1]);

			bmp[index*3+2]=(Palette[((int)*(Data+offset+count))*3+2]);
			index++;
			length--;
			}
			}
		break;
			
	case 1: 
          count++;
		index += (*(Data+offset+count));
		break;
		
	case 5:
      length = (*(Data+offset+count+1));
      while(length > 0)
      {
         count +=2;
         *((short*)(Data+offset+count));
         index++;

         length--;
      }
      count++;
      break;
	

	
	default:
  
		if(bit_16)
		{

		count++;
		length = (*(Data+offset+count));
				
		
		while(length > 0)
		{
			count++;
            memcpy((void *)&bmp[index*2],(const void*)&*((short*)(Data+offset+count)),sizeof(short));
		    index++;
			length--;
			
			count++;
		}
			}
		else
		{
		count++;
		length = (*(Data+offset+count));
		
		while(length > 0)
		{
			count++;
			bmp[index*3]=(Palette[((int)*(Data+offset+count))*3]);

			bmp[index*3+1]=(Palette[((int)*(Data+offset+count))*3+1]);

			bmp[index*3+2]=(Palette[((int)*(Data+offset+count))*3+2]);
			index++;
			length--;
			}
		}
		break;
	}
}

return(bmp);
}
/*************************************************************************


*************************************************************************/
unsigned char * BMPFILE::Extract_Tile_Clean(long offset, unsigned char *Data, Entry_768 *entry )
{

unsigned char *bmp;
entry->X=45;
entry->Y=45;

if(!bit_16)
{
    
    bmp=new unsigned char [45*45*3];
    
	for(int k=0; k<45*45; k++)
	{
	*(bmp+k*3)=248;
    *(bmp+k*3+1)=7;
    *(bmp+k*3+2)=248;
	}

long count=0;



for (int i=0;i<24;i++) {
  for (int j=23-i;j<22+i;j++) {


    *(bmp+(((i*45-45)+j)*3))=(Palette[((int)*(Data+offset+count))*3]);
    *(bmp+(((i*45-45)+j)*3)+1)=(Palette[((int)*(Data+offset+count))*3+1]);
    *(bmp+(((i*45-45)+j)*3)+2)=(Palette[((int)*(Data+offset+count))*3+2]);
   count++;
  }
}

for (int i=23;i<45;i++) {
  for (int j=i-23;j<68-i;j++) {
    *(bmp+((i*45-45+j)*3))=(Palette[((int)*(Data+offset+count))*3]);
    *(bmp+((i*45-45+j)*3)+1)=(Palette[((int)*(Data+offset+count))*3+1]);
    *(bmp+((i*45-45+j)*3)+2)=(Palette[((int)*(Data+offset+count))*3+2]);
   count++;
  }
}
	}
else
{

   bmp=new unsigned char [45*45*2];

	short temtrans=0;

	temtrans+=31;
	temtrans=temtrans<<10;
	temtrans+=31;	
	for(unsigned int i=0; i<45*45; i++)
	{
		memcpy((void*)&bmp[i*2],(const void *)&temtrans,sizeof(temtrans));
	}


long count=0;



for (int i=0;i<24;i++) {
	for (int j=23-i;j<22+i;j++) {

    *(bmp+(((i*45-45)+j)*2))=*(Data+offset+(count*2));
    *(bmp+(((i*45-45)+j)*2)+1)=*(Data+offset+(count*2)+1);
	count++;
  }
}

for (int i=23;i<45/*46*/;i++) {
  for (int j=i-23;j<68-i;j++) {

    *(bmp+(((i*45-45)+j)*2))=*(Data+offset+(count*2));
    *(bmp+(((i*45-45)+j)*2)+1)=*(Data+offset+(count*2)+1);
	count++;
  }
}

	}

	if(bit_16)
	{
		long len;	
		unsigned char * bmp2 = Convert_16_24(bmp,(45*45)*2,len);
		delete [] bmp;
		return(bmp2);
	}
	else
	{
		return(bmp);
	}
}

/*************************************************************************





**************************************************************************/

unsigned char * BMPFILE::Extract_768_Clean(long offset, unsigned char Data[], Entry_768 *entry )
{


unsigned char *bmp=NULL;
long count2=0;
long index=0;
int length=0;


unsigned int width = *((int*)(Data+offset));
unsigned int height = *((int*)(Data+offset+4));
entry->X = width;
entry->Y = height;
int i = 0,tem = 0;

bmp = new unsigned char [width*height*3];

if(bit_16)
{
	
short temtrans=0;

//if(ref.rgbBlue==255 && ref.rgbGreen==0 && ref.rgbRed==255)
//{	
temtrans+=31;
temtrans=temtrans<<10;
temtrans+=31;	
//}
	
for(unsigned int i=0; i<width*height; i++)
{
memcpy((void*)&bmp[i*2],(const void *)&temtrans,sizeof(temtrans));
}		
}
else
{
for(unsigned int i=0; i<width*height*3; i++)
{
bmp[i]=255;
bmp[++i]=0;
bmp[++i]=255;
}
}

//ofstream oifile;

int temp=0;
short tems=0;
RGBQUAD color;


long count=0;

for(count=17; ((int)*(Data+offset+count)) != 0 && (count-17)<width*height*2; count++)
{
  temp=0;
  temp=(*(Data+offset+count));
	switch(temp)
	{

    case 0x04:
    case 0x14:
    case 0x24:
    case 0x34:
    case 0x44:
	case 0x54:
	case 0x64:
		{
			BYTE rRed,rBlue,rGreen;

			switch(temp)
			{
				case 0x04:rRed = 240;rBlue=0;rGreen=0;break;
				case 0x14:rRed = 0;rBlue=240;rGreen=0;break;
				case 0x24:rRed = 0;rBlue=0;rGreen=240;break;
				case 0x34:rRed = 120;rBlue=0;rGreen=0;break;
				case 0x44:rRed = 0;rBlue=120;rGreen=0;break;
				case 0x54:rRed = 0;rBlue=0;rGreen=120;break;
				case 0x64:rRed = 220;rBlue=220;rGreen=100;break;
				default:break;
			}
        count++;
		length = ((int)*(Data+offset+count));
		while(length > 0)
		{
			count++;
			if(!bit_16)
			{
				bmp[index*3]=(Palette[((int)*(Data+offset+count-1))*3] ^ temp/10);
				bmp[index*3+1]=(Palette[((int)*(Data+offset+count-1))*3+1] ^ temp/10);
				bmp[index*3+2]=(Palette[((int)*(Data+offset+count-1))*3+2] ^ temp/10);
			}
			else
			{
				color.rgbRed=(((int)*(Data+offset+count-1))*3) ^ rRed;
				color.rgbGreen=(((int)*(Data+offset+count-1))*3+1) ^ rGreen;
				color.rgbBlue=(((int)*(Data+offset+count-1))*3+2) ^ rBlue;
				tems=Color_24_16(color);
				memcpy((void *)&bmp[index*2],(const void*)&tems,sizeof(short));
			}
			index++;
			length--;
		}
		}
		break;

	case 3:
		break;
	case 2:

		if(bit_16)
		{
		count++;
		length = (*(Data+offset+count));
				
		
		while(length > 0)
		{
			count++;
			
            memcpy((void *)&bmp[index*2],(const void*)&*((short*)(Data+offset+count)),sizeof(short));
		    index++;
			length--;
			
			count++;
		}
/*		count++;
		length = (*(Data+offset+count));
				
		short val = 0;
		while(length > 0)
		{
			count++;

            //memcpy((void *)&bmp[index*2],(const void*)&*((short*)(Data+offset+count)),sizeof(short));
		    memcpy((void *)&val,(const void*)&*((short*)(Data+offset+count)),sizeof(short));  
			RGBQUAD tems = Color_16_24(val);
            memcpy((void *)&bmp[index*3],(const void*)&tems.rgbRed,sizeof(BYTE));
            memcpy((void *)&bmp[index*3+1],(const void*)&tems.rgbGreen,sizeof(BYTE));
            memcpy((void *)&bmp[index*3+2],(const void*)&tems.rgbBlue,sizeof(BYTE));
			index++;
			length--;
			
			count++;
		}*/
			}
		else
		{
		count++;
		length = (*(Data+offset+count));
				
		
		while(length > 0)
		{
			count++;
			bmp[index*3]=(Palette[((int)*(Data+offset+count))*3]);

			bmp[index*3+1]=(Palette[((int)*(Data+offset+count))*3+1]);

			bmp[index*3+2]=(Palette[((int)*(Data+offset+count))*3+2]);
			index++;
			length--;
			}
			}
		break;
			
	case 1: 
          count++;
		index += (*(Data+offset+count));
		break;
		
	case 5:
      length = (*(Data+offset+count+1));
      while(length > 0)
      {
         count +=2;
         *((short*)(Data+offset+count));
         index++;

         length--;
      }
      count++;
      break;
	

	
	default:
  
		if(bit_16)
		{
		count++;
		length = (*(Data+offset+count));
				
		
		while(length > 0)
		{
			count++;
			
            memcpy((void *)&bmp[index*2],(const void*)&*((short*)(Data+offset+count)),sizeof(short));
		    index++;
			length--;
			
			count++;
		}
/*		count++;
		length = (*(Data+offset+count));
				
		
		short val = 0;
		while(length > 0)
		{
			count++;

            //memcpy((void *)&bmp[index*2],(const void*)&*((short*)(Data+offset+count)),sizeof(short));
		    memcpy((void *)&val,(const void*)&*((short*)(Data+offset+count)),sizeof(short));  
			RGBQUAD tems = Color_16_24(val);
            memcpy((void *)&bmp[index*3],(const void*)&tems.rgbRed,sizeof(BYTE));
            memcpy((void *)&bmp[index*3+1],(const void*)&tems.rgbGreen,sizeof(BYTE));
            memcpy((void *)&bmp[index*3+2],(const void*)&tems.rgbBlue,sizeof(BYTE));
			index++;
			length--;
			
			count++;
		}*/
			}
		else
		{
		count++;
		length = (*(Data+offset+count));
		
		while(length > 0)
		{
			count++;
			bmp[index*3]=(Palette[((int)*(Data+offset+count))*3]);

			bmp[index*3+1]=(Palette[((int)*(Data+offset+count))*3+1]);

			bmp[index*3+2]=(Palette[((int)*(Data+offset+count))*3+2]);
			index++;
			length--;
			}
		}
		break;
	}
}
	if(bit_16)
	{
		long len;	
		unsigned char * bmp2 = Convert_16_24(bmp,(width*height)*2,len);
		delete [] bmp;
		return(bmp2);
	}
	else
	{
		return(bmp);
	}
}
/*************************************************************************





**************************************************************************/



/*************************************************************************
	
	file created on:	2002/08/30   19:33
	filename: 			Bmp.cpp
	author:				Andreas Hartl<andy@runicsoft.com>

		visit http://www.runicsoft.com for updates and more information

	purpose:	functions to load raw bmp data,
	                      to save raw bmp data,
						  to convert RGB data to raw bmp data,
						  to convert raw bmp data to RGB data
						  and to use the WinAPI to select
							a bitmap into a device context

The code in this file is subject to the RunicSoft source code licence 
	( http://www.runicsoft.com/sourcelicence.txt )

**************************************************************************/

// #include <windows.h>
// #include <stdio.h>       // for memset


/*******************************************************************
BYTE* ConvertRGBToBMPBuffer ( BYTE* Buffer, int width, 
		int height, long* newsize )


This function takes as input an array of RGB values, it's width
and height.
The buffer gets then transformed to an array that can be used
to write to a windows bitmap file. The size of the array
is returned in newsize, the array itself is the
return value of the function.
Both input and output buffers must be deleted by the
calling function.

The input buffer is expected to consist of width * height
RGB triplets. Thus the total size of the buffer is taken as
width * height * 3.

The function then transforms this buffer so that it can be written 
to a windows bitmap file:
First the RGB triplets are converted to BGR.
Then the buffer is swapped around since .bmps store
images uside-down.
Finally the buffer gets DWORD ( 32bit ) aligned, 
meaning that each scanline ( 3 * width bytes ) gets
padded with 0x00 bytes up to the next DWORD boundary


*******************************************************************/
//#include "BMPLIB.h"

unsigned char* ConvertRGBToBMPBuffer ( unsigned char* Buffer, int width, int height, long& newsize )
{

	// first make sure the parameters are valid
	if ( ( NULL == Buffer ) || ( width == 0 ) || ( height == 0 ) )
		return NULL;

	// now we have to find with how many bytes
	// we have to pad for the next DWORD boundary	

	int padding = 0;
	int scanlinebytes = width * 3;
	while ( ( scanlinebytes + padding ) % 4 != 0 )     // DWORD = 4 bytes
		padding++;
	// get the padded scanline width
	int psw = scanlinebytes + padding;
	
	// we can already store the size of the new padded buffer
	newsize = height * psw;

	// and create new buffer
	BYTE* newbuf = new BYTE[newsize];
	
	// fill the buffer with zero bytes then we dont have to add
	// extra padding zero bytes later on
	memset ( newbuf, 0, newsize );

	// now we loop trough all bytes of the original buffer, 
	// swap the R and B bytes and the scanlines
	long bufpos = 0;   
	long newpos = 0;
	for ( int y = 0; y < height; y++ )
		for ( int x = 0; x < 3 * width; x+=3 )
		{
			bufpos = y * 3 * width + x;     // position in original buffer
			newpos = ( height - y - 1 ) * psw + x;           // position in padded buffer

			newbuf[newpos] = Buffer[bufpos+2];       // swap r and b
			newbuf[newpos + 1] = Buffer[bufpos + 1]; // g stays
			newbuf[newpos + 2] = Buffer[bufpos];     // swap b and r
		}

	return newbuf;
}

/***************************************************************
BYTE* ConvertBMPToRGBBuffer ( BYTE* Buffer, 
		int width, int height )

This function takes as input the data array
from a bitmap and its width and height.
It then converts the bmp data into an RGB array.
The calling function must delete both the input
and output arrays.
The size of the returned array will be 
width * height * 3
On error the returb value is NULL, else the
RGB array.


The Buffer is expected to be the exact data read out
from a .bmp file.  
The function will swap it around, since images
are stored upside-down in bmps.
The BGR triplets from the image data will
be converted to RGB.
And finally the function removes padding bytes.
The returned arraay consits then of
width * height RGB triplets.

*****************************************************************/

BYTE* ConvertBMPToRGBBuffer ( BYTE* Buffer, int width, int height )
{
	// first make sure the parameters are valid
	if ( ( NULL == Buffer ) || ( width == 0 ) || ( height == 0 ) )
		return NULL;

	// find the number of padding bytes
		
	int padding = 0;
	int scanlinebytes = width * 3;
	while ( ( scanlinebytes + padding ) % 4 != 0 )     // DWORD = 4 bytes
		padding++;
	// get the padded scanline width
	int psw = scanlinebytes + padding;

	// create new buffer
	BYTE* newbuf = new BYTE[width*height*3];
	
	// now we loop trough all bytes of the original buffer, 
	// swap the R and B bytes and the scanlines
	long bufpos = 0;   
	long newpos = 0;
	for ( int y = 0; y < height; y++ )
		for ( int x = 0; x < 3 * width; x+=3 )
		{
			newpos = y * 3 * width + x;     
			bufpos = ( height - y - 1 ) * psw + x;

			newbuf[newpos] = Buffer[bufpos + 2];       
			newbuf[newpos + 1] = Buffer[bufpos+1]; 
			newbuf[newpos + 2] = Buffer[bufpos];     
		}

	return newbuf;
}


/***************************************************************
BYTE* ConvertBMPToRGBBuffer_16 ( BYTE* Buffer, 
		int width, int height )

This function takes as input the data array
from a bitmap and its width and height.
It then converts the bmp data into an RGB array.
The calling function must delete both the input
and output arrays.
The size of the returned array will be 
width * height * 2
On error the returb value is NULL, else the
RGB array.


The Buffer is expected to be the exact data read out
from a .bmp file.  
The function will swap it around, since images
are stored upside-down in bmps.
The BGR triplets from the image data will
be converted to RGB.
And finally the function removes padding bytes.
The returned arraay consits then of
width * height RGB triplets.

*****************************************************************/

BYTE* ConvertBMPToRGBBuffer_16 ( BYTE* Buffer, int width, int height )
{
	// first make sure the parameters are valid
	if ( ( NULL == Buffer ) || ( width == 0 ) || ( height == 0 ) )
		return NULL;

	// find the number of padding bytes
		
	int padding = 0;
	int scanlinebytes = width * 2;
	while ( ( scanlinebytes + padding ) % 4 != 0 )     // DWORD = 4 bytes
		padding++;
	// get the padded scanline width
	int psw = scanlinebytes + padding;

	// create new buffer
	BYTE* newbuf = new BYTE[width*height*2];
	
	// now we loop trough all bytes of the original buffer, 
	// swap the R and B bytes and the scanlines
	long bufpos = 0;   
	long newpos = 0;
	for ( int y = 0; y < height; y++ )
		for ( int x = 0; x < 2 * width; x+=2 )
		{
			newpos = y * 2 * width + x;     
			bufpos = ( height - y - 1 ) * psw + x;

			newbuf[newpos] = Buffer[bufpos];       
			newbuf[newpos + 1] = Buffer[bufpos+1]; 
			//newbuf[newpos + 2] = Buffer[bufpos];     


		}

	return newbuf;
}



/***********************************************
bool LoadBMPIntoDC ( HDC hDC, char* bmpfile )

Takes in a device context and the name of a
bitmap to load. If an error occures the function
returns false, else the contents of the bmp
are blitted to the HDC 

************************************************/

bool LoadBMPIntoDC ( HDC hDC, char* bmpfile )
{
	// check if params are valid 
      if ( ( NULL == hDC  ) || ( NULL == bmpfile ) )
		return false;      

	// load bitmap into a bitmap handle
	HBITMAP hBmp = (HBITMAP)LoadImage ( NULL, bmpfile, IMAGE_BITMAP, 0, 0,
		LR_LOADFROMFILE );
	
	if ( NULL == hBmp )
		return false;        // failed to load image
 
	// bitmaps can only be selected into memory dcs:
	HDC dcmem = CreateCompatibleDC ( NULL );

	// now select bitmap into the memory dc
	if ( NULL == SelectObject ( dcmem, hBmp ) )
	{	// failed to load bitmap into device context
		DeleteDC ( dcmem ); 
		return false; 
	}

	
	// now get the bmp size
	BITMAP bm;
	GetObject ( hBmp, sizeof(bm), &bm );
	// and blit it to the visible dc
	if ( BitBlt ( hDC, 0, 0, bm.bmWidth, bm.bmHeight, dcmem,
		0, 0, SRCCOPY ) == 0 )
	{	// failed the blit
		DeleteDC ( dcmem ); 
		return false; 
	}
		   	
	DeleteDC ( dcmem );  // clear up the memory dc
	
	return true;
}



/***************************************************************
bool SaveBMP ( BYTE* Buffer, int width, int height, 
		long paddedsize, char* bmpfile )

Function takes a buffer of size <paddedsize> 
and saves it as a <width> * <height> sized bitmap 
under the supplied filename.
On error the return value is false.

***************************************************************/

bool SaveBMP ( BYTE* Buffer, int width, int height, long paddedsize, char* bmpfile )
{
	// declare bmp structures 
	BITMAPFILEHEADER bmfh;
	BITMAPINFOHEADER info;
	
	// andinitialize them to zero
	memset ( &bmfh, 0, sizeof (BITMAPFILEHEADER ) );
	memset ( &info, 0, sizeof (BITMAPINFOHEADER ) );
	
	// fill the fileheader with data
	bmfh.bfType = 0x4d42;       // 0x4d42 = 'BM'
	bmfh.bfReserved1 = 0;
	bmfh.bfReserved2 = 0;
	bmfh.bfSize = sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER) + paddedsize;
	bmfh.bfOffBits = 0x36;		// number of bytes to start of bitmap bits
	
	// fill the infoheader

	info.biSize = sizeof(BITMAPINFOHEADER);
	info.biWidth = width;
	info.biHeight = height;
	info.biPlanes = 1;			// we only have one bitplane
	info.biBitCount = 24;		// RGB mode is 24 bits
	info.biCompression = BI_RGB;	
	info.biSizeImage = 0;		// can be 0 for 24 bit images
	info.biXPelsPerMeter = 0x0ec4;     // paint and PSP use this values
	info.biYPelsPerMeter = 0x0ec4;     
	info.biClrUsed = 0;			// we are in RGB mode and have no palette
	info.biClrImportant = 0;    // all colors are important

	// now we open the file to write to
	HANDLE file = CreateFile ( bmpfile , GENERIC_WRITE, FILE_SHARE_READ,
		 NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL );
	if ( file == NULL )
	{
		CloseHandle ( file );
		return false;
	}
	
	// write file header
	unsigned long bwritten;
	if ( WriteFile ( file, &bmfh, sizeof ( BITMAPFILEHEADER ), &bwritten, NULL ) == false )
	{	
		CloseHandle ( file );
		return false;
	}
	// write infoheader
	if ( WriteFile ( file, &info, sizeof ( BITMAPINFOHEADER ), &bwritten, NULL ) == false )
	{	
		CloseHandle ( file );
		return false;
	}
	// write image data
	if ( WriteFile ( file, Buffer, paddedsize, &bwritten, NULL ) == false )
	{	
		CloseHandle ( file );
		return false;
	}
	
	// and close file
	CloseHandle ( file );

	return true;
}

/*******************************************************************
BYTE* LoadBMP ( int* width, int* height, long* size 
		char* bmpfile )

The function loads a 24 bit bitmap from bmpfile, 
stores it's width and height in the supplied variables
and the whole size of the data (padded) in <size>
and returns a buffer of the image data 

On error the return value is NULL. 

  NOTE: make sure you [] delete the returned array at end of 
		program!!!
*******************************************************************/

BYTE* LoadBMP ( int* width, int* height, long* size, char* bmpfile )
{
	// declare bitmap structures
	BITMAPFILEHEADER bmpheader;
	BITMAPINFOHEADER bmpinfo;
	// value to be used in ReadFile funcs
	DWORD bytesread;
	// open file to read from
	HANDLE file = CreateFile ( bmpfile , GENERIC_READ, FILE_SHARE_READ,
		 NULL, OPEN_EXISTING, FILE_FLAG_SEQUENTIAL_SCAN, NULL );
	if ( NULL == file )
		return NULL; // coudn't open file
	
	
	// read file header
	if ( ReadFile ( file, &bmpheader, sizeof ( BITMAPFILEHEADER ), &bytesread, NULL ) == false )
	{
		CloseHandle ( file );
		return NULL;
	}

	//read bitmap info

	if ( ReadFile ( file, &bmpinfo, sizeof ( BITMAPINFOHEADER ), &bytesread, NULL ) == false )
	{
		CloseHandle ( file );
		return NULL;
	}
	
	// check if file is actually a bmp
	if ( bmpheader.bfType != 'MB' )
	{
		CloseHandle ( file );
		return NULL;
	}

	// get image measurements
	*width   = bmpinfo.biWidth;
	*height  = abs ( bmpinfo.biHeight );

	// check if bmp is uncompressed
	if ( bmpinfo.biCompression != BI_RGB )
	{
		CloseHandle ( file );
		return NULL;
	}

	// check if we have 24 bit bmp
	if ( bmpinfo.biBitCount != 24 )
	{
		CloseHandle ( file );
		return NULL;
	}
	

	// create buffer to hold the data
	*size = bmpheader.bfSize - bmpheader.bfOffBits;
	BYTE* Buffer = new BYTE[ *size ];
	// move file pointer to start of bitmap data
	SetFilePointer ( file, bmpheader.bfOffBits, NULL, FILE_BEGIN );
	// read bmp data
	if ( ReadFile ( file, Buffer, *size, &bytesread, NULL ) == false )
	{
		delete [] Buffer;
		CloseHandle ( file );
		return NULL;
	}

	// everything successful here: close file and return buffer
	
	CloseHandle ( file );

	return Buffer;
}

/*******************************************************************
BYTE* LoadBMP_16 ( int* width, int* height, long* size 
		char* bmpfile )

The function loads a 24 bit bitmap from bmpfile, 
stores it's width and height in the supplied variables
and the whole size of the data (padded) in <size>
and returns a buffer of the image data 

On error the return value is NULL. 

  NOTE: make sure you [] delete the returned array at end of 
		program!!!
*******************************************************************/

BYTE* LoadBMP_16 ( int* width, int* height, long* size, char* bmpfile )
{
	// declare bitmap structures
	BITMAPFILEHEADER bmpheader;
	BITMAPINFOHEADER bmpinfo;
	// value to be used in ReadFile funcs
	DWORD bytesread;
	// open file to read from
	HANDLE file = CreateFile ( bmpfile , GENERIC_READ, FILE_SHARE_READ,
		 NULL, OPEN_EXISTING, FILE_FLAG_SEQUENTIAL_SCAN, NULL );
	if ( NULL == file )
		return NULL; // coudn't open file
	
	
	// read file header
	if ( ReadFile ( file, &bmpheader, sizeof ( BITMAPFILEHEADER ), &bytesread, NULL ) == false )
	{   size=NULL;
		CloseHandle ( file );
		return NULL;
	}

	//read bitmap info

	if ( ReadFile ( file, &bmpinfo, sizeof ( BITMAPINFOHEADER ), &bytesread, NULL ) == false )
	{   size=NULL;
		CloseHandle ( file );
		return NULL;
	}
	
	// check if file is actually a bmp
	if ( bmpheader.bfType != 'MB' )
	{   size=NULL;
		CloseHandle ( file );
		return NULL;
	}

	// get image measurements
	*width   = bmpinfo.biWidth;
	*height  = abs ( bmpinfo.biHeight );

	// check if bmp is uncompressed
	if ( bmpinfo.biCompression != BI_RGB )
	{   size=NULL;
		CloseHandle ( file );
		return NULL;
	}

	// check if we have 24 bit bmp
	if ( bmpinfo.biBitCount != 16 )
	{   size=NULL;
	
		CloseHandle ( file );
		return NULL;
	}
	

	// create buffer to hold the data
	*size = bmpheader.bfSize - bmpheader.bfOffBits;
	BYTE* Buffer = new BYTE[ *size ];
	// move file pointer to start of bitmap data
	SetFilePointer ( file, bmpheader.bfOffBits, NULL, FILE_BEGIN );
	// read bmp data
	if ( ReadFile ( file, Buffer, *size, &bytesread, NULL ) == false )
	{
		delete [] Buffer;
		CloseHandle ( file );
		return NULL;
	}

	// everything successful here: close file and return buffer
	
	CloseHandle ( file );

	return Buffer;
}


/*******************************************************************/
//#include "BMPLIB.h"
/***************************************************************/

bool SaveBMP_16 ( BYTE* Buffer, int width, int height, long paddedsize, char* bmpfile )
{
	// declare bmp structures 
	BITMAPFILEHEADER bmfh;
	BITMAPINFOHEADER info;
	
	// andinitialize them to zero
	memset ( &bmfh, 0, sizeof (BITMAPFILEHEADER ) );
	memset ( &info, 0, sizeof (BITMAPINFOHEADER ) );
	
	// fill the fileheader with data
	bmfh.bfType = 0x4d42;       // 0x4d42 = 'BM'
	bmfh.bfReserved1 = 0;
	bmfh.bfReserved2 = 0;
	bmfh.bfSize = sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER) + paddedsize;
	bmfh.bfOffBits = 0x36;		// number of bytes to start of bitmap bits
	
	// fill the infoheader

	info.biSize = sizeof(BITMAPINFOHEADER);
	info.biWidth = width;
	info.biHeight = height;
	info.biPlanes = 1;			// we only have one bitplane
	info.biBitCount = 16;//24;		// RGB mode is 24 bits
	info.biCompression = BI_RGB;	
	info.biSizeImage = 0;		// can be 0 for 24 bit images
	info.biXPelsPerMeter = 0x0ec4;     // paint and PSP use this values
	info.biYPelsPerMeter = 0x0ec4;     
	info.biClrUsed = 0;			// we are in RGB mode and have no palette
	info.biClrImportant = 0;    // all colors are important

	// now we open the file to write to
	HANDLE file = CreateFile ( bmpfile , GENERIC_WRITE, FILE_SHARE_READ,
		 NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL );
	if ( file == NULL )
	{
		CloseHandle ( file );
		return false;
	}
	
	// write file header
	unsigned long bwritten;
	if ( WriteFile ( file, &bmfh, sizeof ( BITMAPFILEHEADER ), &bwritten, NULL ) == false )
	{	
		CloseHandle ( file );
		return false;
	}
	// write infoheader
	if ( WriteFile ( file, &info, sizeof ( BITMAPINFOHEADER ), &bwritten, NULL ) == false )
	{	
		CloseHandle ( file );
		return false;
	}
	// write image data
	if ( WriteFile ( file, Buffer, paddedsize, &bwritten, NULL ) == false )
	{	
		CloseHandle ( file );
		return false;
	}
	
	// and close file
	CloseHandle ( file );

	return true;
}

/*******************************************************************/

unsigned char* ConvertRGBToBMPBuffer_16 ( unsigned char* Buffer, int width, int height, long* newsize )
{

	// first make sure the parameters are valid
	if ( ( NULL == Buffer ) || ( width == 0 ) || ( height == 0 ) )
		return NULL;

	// now we have to find with how many bytes
	// we have to pad for the next DWORD boundary	

	int padding = 0;
	int scanlinebytes = width * 2;
	while ( ( scanlinebytes + padding ) % 4 != 0 )     // DWORD = 4 bytes
		padding++;
	// get the padded scanline width
	int psw = scanlinebytes + padding;
	
	// we can already store the size of the new padded buffer
	*newsize = height * psw;

	// and create new buffer
	BYTE* newbuf = new BYTE[*newsize];
	
	// fill the buffer with zero bytes then we dont have to add
	// extra padding zero bytes later on
	memset ( newbuf, 0, *newsize );

	// now we loop trough all bytes of the original buffer, 
	// swap the R and B bytes and the scanlines
	long bufpos = 0;   
	long newpos = 0;
	for ( int y = 0; y < height; y++ )
		for ( int x = 0; x < 2 * width; x+=2 )
		{
			bufpos = y * 2 * width + x;     // position in original buffer
			newpos = ( height - y - 1 ) * psw + x;           // position in padded buffer

			newbuf[newpos] = Buffer[bufpos];      // swap r and b
			newbuf[newpos + 1] = Buffer[bufpos + 1]; // g stays
			//newbuf[newpos + 2] = Buffer[bufpos];     // swap b and r
		}

	return newbuf;
}
/*********************************************************************
 *********************************************************************

COLOR SHIFTING FUNCTIONS

 *********************************************************************
/*********************************************************************/
unsigned char *Convert_24_16(unsigned char Buffer[],long len,long &newlen)
{
  RGBQUAD color;
  unsigned char *newbuff;
  newbuff = new unsigned char [(len/3)*2];

	for(int i=0; i<len/3; i++)
	{
       color.rgbRed=Buffer[i*3];
       color.rgbGreen=Buffer[i*3+1];
       color.rgbBlue=Buffer[i*3+2];
	   short tem1=Color_24_16(color);
	   memcpy((void*)&*(newbuff+i*2),(const void*)&tem1,sizeof(short));
	}

newlen=(len/3)*2;
return(newbuff);	
}

/*******************





********************/

unsigned char *Convert_16_24(unsigned char Buffer[],long len,long &newlen)
{
	
  unsigned char *newbuff;
  long len2 = len/2;
  newlen=(len2)*3;
  newbuff = new unsigned char [newlen];
  unsigned char * buffloc = newbuff;
  unsigned short * colloc = (unsigned short*)Buffer;
  for(int i=0; i<len2; i++)
  {
	  //Advances the pointers at the same time as it calcs the new color value
	  //See Color_16_24() function for explanation
	*(buffloc+2) = (unsigned char)((( ((unsigned short)(*colloc<<11))>>8 ) ));
	*(buffloc+1) = (unsigned char)((( ((unsigned short)(*colloc<<6))>>8 ) ));
	*(buffloc) = (unsigned char)((( ((unsigned short)(*colloc<<1))>>8 ) ));
	buffloc +=3;
	colloc++;
  }

return(newbuff);	
}

/*******************





********************/

short Color_24_16(RGBQUAD val)
{

short color=0;

if(val.rgbRed==0)
{color=color<<5;}
else if(val.rgbRed>244)
{color+=31;color=color<<5;}
else
{color+=(val.rgbRed/8);color=color<<5;}

if(val.rgbGreen==0)
{color=color<<5;}
else if(val.rgbGreen>244)
{color+=31;color=color<<5;}
else
{color+=(val.rgbGreen/8);color=color<<5;}

if(val.rgbBlue==0)
{}
else if(val.rgbBlue>244)
{color+=31;}
else
{color+=(val.rgbBlue/8);}	


return(color);
}


/*******************





********************/

void Color_16_24(unsigned short val, unsigned char &r, unsigned char &g, unsigned char &b)
{

b = (unsigned char)((( ((unsigned short)(val<<11))>>8 ) ));
g = (unsigned char)((( ((unsigned short)(val<<6))>>8 ) ));
r = (unsigned char)((( ((unsigned short)(val<<1))>>8 ) ));
//Extracts colors from short, then multiplies by 255/256 and devides by 31/32

}

