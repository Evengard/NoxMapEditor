#ifndef BMPLIB_H
#define BMPLIB_H


#include <windows.h>
#include <fstream>
#include "ClassDefs.h"
using namespace std;

#pragma once




unsigned char *Convert_24_16(unsigned char Buffer[],long len,long &newlen);
unsigned char *Convert_16_24(unsigned char Buffer[],long len,long &newlen);
short Color_24_16(RGBQUAD val);
void Color_16_24(unsigned short val, unsigned char &r, unsigned char &g, unsigned char &b);


unsigned char *ConvertRGBToBMPBuffer ( unsigned char* Buffer, int width, int height, long& newsize );
unsigned char *ConvertRGBToBMPBuffer_16 ( unsigned char* Buffer, int width, int height, long* newsize );
unsigned char *ConvertBMPToRGBBuffer ( unsigned char* Buffer, int width, int height );
unsigned char *ConvertBMPToRGBBuffer_16 ( unsigned char* Buffer, int width, int height );
bool SaveBMP ( unsigned char* Buffer, int width, int height, long paddedsize, char* bmpfile );
bool SaveBMP_16 ( unsigned char* Buffer, int width, int height, long paddedsize, char* bmpfile );
unsigned char *LoadBMP ( int* width, int* height, long* size, char* bmpfile);
unsigned char *LoadBMP_16 ( int* width, int* height, long* size, char* bmpfile);


struct Entry_768
{

int X;
int Y;
int Unknown1;
int Unknown2;
unsigned char Unknown3;

};



class BMPFILE
{

	private:
	public:

        bool bit_16;
        unsigned char Palette[768];
        RGBQUAD ref;
        bool Save_BMP(int Type,char * filename,unsigned char Data[],long offset,Entry_768 & entry);
        unsigned char * Extract_768(long offset,unsigned char Data[],Entry_768 *entry);
        unsigned char * Extract_Tile(long offset,unsigned char *Data,Entry_768 *entry);
        unsigned char * Extract_768_Clean(long offset,unsigned char Data[],Entry_768 *entry);
        unsigned char * Extract_Tile_Clean(long offset,unsigned char *Data,Entry_768 *entry);
   
		void Set_Color(RGBQUAD ref2)
		{
           ref.rgbBlue=ref2.rgbBlue;
           ref.rgbGreen=ref2.rgbGreen;
           ref.rgbRed=ref2.rgbRed;
		}

        void Set_Bit_16()
        {bit_16=true;}
        void Set_Bit_8()
        {bit_16=false;}


		void Load_Palette(char * fileLoc)
		{

          ifstream ifile;
          ifile.open(fileLoc,ios::in | ios::binary);
          ifile.seekg(7,ios::beg);
          int count=0;

          while(count<768)
          {
            ifile.get((char &)Palette[count]);
            ifile.get((char &)Palette[count+1]);
            ifile.get((char &)Palette[count+2]);
            count+=3;
          }
          ifile.close();
		};

   BMPFILE()
   {
   bit_16=false;
   };

};


#endif