#ifndef NX_MAP
#define NX_MAP
#pragma once

#include "ClassDefs.h"
#include "ObjectMap.h"
#include "FloorMap.h"
#include "GuiDisplay.h"
#include "SurfaceMap.h"
#include "VidLib.h"
#include "ThingLib.h"

class NOX_MAP
{
public:

	  long X;
	  long Y;
	  long MOUSE_X;
	  long MOUSE_Y;
	  DWORD BGColor;

	  //Callbacks
	  //typedef void (__stdcall *ObjMove_CALLBACK)(int ObjNum,int X,int Y);
	  //ObjMove_CALLBACK ObjMoveFunc; 
	 // public delegate void OBJMoved_CALLBACK(int ObjNum,int X,int Y);
	  //The Display
      VideoBag Video;
	  ThingBin Thing;
      GUI_DISPLAY Display;
      
	  //The Map Class'
	  OBJECT_MAP Objects;
	  FLOOR_MAP Floor;

	  //Surfaces
	  CSurface* FloorSurface;
	  CSurface* ObjectSurface;
	  CSurface* ScaleSurface;

	  //Drawing
	  int MAG_MAX;
	  int MAG_SIZE;
	  bool scaleOn;
      void Draw_Magnify(int x,int y);
	  void Draw(bool DrawFloor = true, bool DrawWalls = true, bool DrawObjects = true, DWORD FloorColor = NULL);
	  //Construction
	  bool LoadObject(char objName[], CSurface ** surface);
	  bool AddObject(char objName[], int X, int Y, int callback, int DoorFacing = -1);
	  bool SaveObjectBMP(char objName[], char fileName[]);
	 // bool SaveWallBMP(char wallName[], char fileName[]);
	  bool AddTile(char objName[], int X, int Y, int variation = NULL);

	  /*void AddObjectMoveCallback(ObjMove_CALLBACK func)
	  {
		ObjMoveFunc = func;
		int x = 0;
		int y=50;
		int obj = 100;
		ObjMoveFunc(x,y,obj);
	  }*/

	  bool DeleteObject(int callback);
	  bool Scale(bool on);
	  void SetMouse(long X, long Y)
	  {
		MOUSE_X = X;
		MOUSE_Y = Y;
	  }


      NOX_MAP()
      {
		ScaleSurface = NULL; // Null out the construct
	    scaleOn = false;
		Video.Init();
		Video.idx.LoadIdx();
		Thing.Load_Thingdb(Video.thingpath);
		X=NULL;
		Y=NULL;
		BGColor = NULL;
	    MAG_MAX = 50;
	    MAG_SIZE = 100;
      }

};


#endif