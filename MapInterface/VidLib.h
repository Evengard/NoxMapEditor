#ifndef VID_LIB
#define VID_LIB


#include "IDXlib.h"
#include "BMPlib.h"
#include "RegEdit.h"
#include "DDutil.h"
#include "GuiDisplay.h"
#include <fstream>
using namespace std;

typedef void (CALLBACK* NXZDECRYPT)(unsigned char *,unsigned int,unsigned char *,unsigned int);

class NxImage
{
public:
  unsigned int offsetX;
  unsigned int offsetY;
  unsigned int width;
  unsigned int height;
  unsigned char ID[50];
  unsigned char * data;
  bool ValidImage;

  /////////////////////////////////////
  // Loads the image to the structure//
  /////////////////////////////////////
  bool Load(unsigned int X, unsigned int Y, 
	        unsigned int offX, unsigned int offY,
			unsigned char *dat, char* name)
  {
	  if (dat)
	  {
		  data = dat;
		  width = X;
		  height = Y;
		  offsetX = offX;
		  offsetY = offY;
		  strcpy((char*)ID,name);
		  ValidImage = true;
	  }
	  return(ValidImage);
  }
  
  void Unload()
  {
     if( data )
		 delete [] data;
	 data = NULL;
     //memset(this,0x00,sizeof(*this));
  }

  bool DrawToSurface(int X, int Y, CSurface * Surface,PIXEL_FORMAT * pixF, bool bit_16)
  {

    if( !Surface || Surface->m_pdds == NULL )
        return false;


	unsigned char * buff2 = NULL;
	long len = 0;
    DDSURFACEDESC2 ddsd;

    ZeroMemory( &ddsd,sizeof(ddsd) );
    ddsd.dwSize = sizeof(ddsd);

    // Lock the surface to directly write to the surface memory 
	if( FAILED( Surface->m_pdds->Lock( NULL, &ddsd, DDLOCK_WAIT | DDLOCK_NOSYSLOCK , NULL ) ) )
        return false;

if( bit_16 )
{
buff2 = Convert_16_24(data,(height*width)*2,len);
}


//if(pixF.BPP == 32 )//|| pixF.BPP == 24) // Need to change the 24 bit
//{
    int size = X*4;
	BYTE* pDDSColor = (BYTE*) ( (BYTE*) ddsd.lpSurface + ( Y ) * ddsd.lPitch +(size));
    //BYTE rVal=RGB_GETBLUE(color),gVal=RGB_GETGREEN(color),bVal=RGB_GETRED(color);
	long index= 0;
	unsigned char * buff;

	if( bit_16 )
		buff = buff2;
	else
		buff = data;

	BYTE col = 0;
    for( DWORD iY = 0; iY < height + 1; iY++ )
    {
        for( DWORD iX = 0; iX < width; iX++ )
        {
			   *pDDSColor = ((buff[index*3]));
*pDDSColor++;
			   *pDDSColor = ((buff[index*3+1]));
*pDDSColor++;
			  *pDDSColor = ((buff[index*3+2]));
*pDDSColor++;
			   //*pDDSColor = col;
			   pDDSColor++;
			   index++;
        }
        pDDSColor = (BYTE*) ( (BYTE*) ddsd.lpSurface + ( Y+iY ) * ddsd.lPitch + (size));
    }

	if( bit_16)
		delete [] buff2;
//}
//else if (pixF.BPP == 16)
//{
/*	RGBQUAD col;
	col.rgbRed = RGB_GETBLUE(color);
	col.rgbBlue = RGB_GETRED(color);
	col.rgbGreen = RGB_GETGREEN(color);
    short *tem;

    for( short iY = 0; iY < ddsd.dwHeight; iY++ )
    {
        for( short iX = 0; iX < ddsd.dwWidth; iX++ )
        {
               tem = (short*) (( (BYTE*) ddsd.lpSurface + ( iY ) * ddsd.lPitch ) + (iX*sizeof(short)) );
	/*	       	col.rgbRed = data;
	            col.rgbBlue = RGB_GETRED(color);
	            col.rgbGreen = RGB_GETGREEN(color);
			   *(tem) |= pixF.Color_24_16(col); //RGBA_MAKE(red,green,blue,0); RGBQUAD
        }

    }*/
//}

    // Unlock the surface
   Surface->m_pdds->Unlock(NULL); 

    return true;	  	  
  }

  void Save();          // Need to code
 
  NxImage()
  {
    memset(this,0x00,sizeof(*this));
  }
  ~NxImage()
  {
     if( data )
	    delete [] data;
	 data = NULL;
  }
};


class VideoBag
{
public:

    HINSTANCE hDLL;            // Handle to DLL
    NXZDECRYPT nxzDecrypt;    // Function pointer
    BMPFILE bmpfile;	
    IDX_DB idx;
	char bagpath[512];
	char thingpath[512];

    // These are used so there is no file stream overhead
    fstream file; // Our video.bag file
    bool OpenVideo() // Open it
	{	
		return(true);
	}

    bool CloseVideo() // Close it
	{
		return(true);
	}

    NxImage * VideoBag::Extract(fstream * file,
	                                IDX_DB::SectionHeader * Header,
	                                IDX_DB::Entry * Entry, char * outFile = NULL);


	bool Init(char * NXZpath = NULL, char * IDXpath = NULL, char * Videopath = NULL, char * Palpath = NULL)
	{
	    char buff[512];
		char path[512];

		if( !NXZpath || !IDXpath || !Videopath || !Palpath)
		{
          REGISTRY Reg;
          Reg.OpenKey(HKEY_LOCAL_MACHINE,"SOFTWARE\\Westwood\\Nox");
	      sprintf(buff,"%s",Reg.GetKeyVal("InstallPath"));
	      buff[strlen(buff)-8] = 0x00;
	      Reg.CloseKey();
		}
		if ( NXZpath )
		   LoadNXZ( NXZpath );
		else
		{
           sprintf(path,"nxzdll.dll"/*,buff*/);
		   if(!LoadNXZ( path ))
		   {
           sprintf(path,"%s//nxzdll.dll",buff);
		   LoadNXZ( path );
		   }
		}

		if( Palpath )
		   bmpfile.Load_Palette( Palpath );
		else
		{
           sprintf(path,"%s\\default.pal",buff);
		   bmpfile.Load_Palette(path);
		}

		if( IDXpath )
		   idx.Set_Path( IDXpath );
		else
		{
           sprintf(path,"%s\\video.idx",buff);
		   idx.Set_Path( path );
		}
		idx.LoadIdx();
	    
		if( Videopath )
		   strcpy(bagpath,Videopath);
		else
		{
           sprintf(bagpath,"%s\\video.bag",buff);
           sprintf(thingpath,"%s\\thing.bin",buff);
		}

		if( idx.bit_16 )
			bmpfile.Set_Bit_16();
		else
			bmpfile.Set_Bit_8();

		return(true);
	}

	bool LoadNXZ(char * path)
	{
           hDLL = LoadLibrary(path);
		   if (hDLL != NULL)
		   {
              nxzDecrypt = (NXZDECRYPT)GetProcAddress(hDLL,"nxzDecrypt");
              if (!nxzDecrypt)
			  {
				  // handle the error
                  FreeLibrary(hDLL); 
				  return(false);
			  }
			  return(true);
			}
		   
		   return(false);
	}

   char Get_RLE_Count(unsigned char value)
   {

      if((value >> 7) && ((value%128) >> 6))
      {
         value = value%128;
         value = value%64;
         return(value);
      }
      return(0);
   }

   /***DESTRUCTOR***/
   ~VideoBag()
   {FreeLibrary(hDLL);}

};


#endif