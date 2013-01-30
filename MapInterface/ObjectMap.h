#ifndef OBJ_MAP
#define OBJ_MAP
#pragma once

#include "ClassDefs.h"
#include "GuiDisplay.h"
#include "SurfaceMap.h"

struct OBJECT
{
  int callbackID;
  int objID;
  int imgID;
  int X;
  int Y;
  int Height;
  int Width;
  bool ExtentCircle;
  bool ExtentBox;
  int Xlen;
  int Ylen;
  int Z;
  int zSizeX;
  int zSizeY;
  int DoorFacing;
};

class OBJECT_MAP
{
public:

  FASTLIST<OBJECT> Objects;
  FASTLIST<SURFACE> ObjImages;

  void Clear()
  {
	  Objects.Clear();
	  ObjImages.Clear();
  }
  SURFACE* FindImage(int ImgID)
  {
	  return(ObjImages.Find(ImgID));
  }
  void AddImage(int ObjID,int ImgID)
  {
  
  int numChildren;
  CSurface *surface;
  CSurface *name;
  int Image_ID;
  int Width;
  int Height;
  int OffsetX;
  int OffsetY;
  unsigned char ImgType;

  }
 OBJECT_MAP()
 {

 }

};


#endif