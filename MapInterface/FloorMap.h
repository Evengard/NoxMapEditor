#ifndef FLOR_MAP
#define FLOR_MAP
#pragma once

#include "GuiDisplay.h"
#include "ClassDefs.h"
#include "SurfaceMap.h"


struct TILE_INFO
{
   int Tile;
   SURFACE * pTile;
   int TileImg;
   int tVari;
   
	enum WallSides : int
	{
		NORTH = 1,
		SOUTH = 8,
		EAST = 16,
		WEST = 32,
		SE = 64,
		SW = 128,
		NE = 256,
		NW = 512
	}WallInfo;
   int Wall;
   int wVari;

   TILE_INFO()
   {
      memset((void*)this,0x00,sizeof(*this));
	  Tile = (-1);
   }
};




class FLOOR_MAP
{
public:
  TILE_INFO tileMap[257][257];
  FASTLIST<SURFACE> TileImgs;
  void Clear()
  {
    memset(tileMap,0x00,sizeof(tileMap));
	TileImgs.Clear();
  }

 FLOOR_MAP()
 {
   memset((void*)this,0x00,sizeof(*this));
 }

};
#endif