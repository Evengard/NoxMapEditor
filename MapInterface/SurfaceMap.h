
#ifndef SURF_MAP
#define SURF_MAP
#pragma once

#include "DDutil.h"
#include "DXutil.h"
#include "ClassDefs.h"
#include "VidLib.h"

struct IMAGE_SIZE
{

public:
	int X;
	int Y;
	int Width;
	int Height;

	IMAGE_SIZE()
	{
      X=0;
	  Y=0;
	  Width=0;
	  Height=0;
	}
};

struct SURFACE
{
public:

  int numChildren;
  CSurface *surface;
  CSurface *name;
  int Image_ID;
  int Width;
  int Height;
  int OffsetX;
  int OffsetY;
  unsigned char ImgType;
  //NxImage img;
  
  //LIST<IMAGE_SIZE> Sizes;
  //int numSizes;

  bool Remove()
  {
    if(numChildren>1)
	{numChildren--;return(1);}
	else
	{
		//Sizes.Clear();
		if(name!=NULL){delete name;name=NULL;}
		if(surface!=NULL){delete surface;surface=NULL;return(1);}
	}
  
  return(0);
  }

  SURFACE()
  {surface=NULL;name=NULL;numChildren=0;Image_ID=0;ImgType=0;}
  
  ~SURFACE()
  {
	if(surface!=NULL){delete surface; surface=NULL;}
	if(name!=NULL){delete name;name=NULL;}
  }
};

#endif