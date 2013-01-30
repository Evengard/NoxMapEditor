// MapInterface.h
#include "IDXLib.h"
#include "VidLib.h"
#include "GuiDisplay.h"
#include "NOXMAP.h"

#pragma once

using namespace System;

namespace MapInterface {
/*	public __delegate void ObjMovedHandler( int x, int y, int ObjNum );
	class EventHandlerCallback;
*/
	public ref class MAP_GUI
	{
	private:
		NOX_MAP* Map;
    
	HWND win;
	//CDisplay*  Display;
	public:
		MAP_GUI()
		{
			Map = new NOX_MAP();
		}

		~MAP_GUI()
		{
		}

//		EventHandlerCallback EventHandler;
        void ClearMap();

		int Scrollx;
		int Scrolly;
		int MOUSEX;
		int MOUSEY;
		int MAG_MAX;
		int MAG_SIZE;

		//typedef void (__stdcall *ObjMove_CALLBACK)(int ObjNum,int X,int Y);
		
		int * GetSurface()
		{
			HDC me;
			Map->Display.Display->GetBackBuffer()->GetDC(&me);
			return((int*)&me);
		}
		void ReleaseSurface(int * hdc)
		{
			Map->Display.Display->GetBackBuffer()->ReleaseDC((HDC)*hdc);
		}
		/*void SetObjCallback(ObjMove_CALLBACK func )
		{
			Map->ObjMoveFunc = (ObjMove_CALLBACK)func;
			//Map->AddObjectMoveCallback( func );
		}*/
		bool Scale(bool on)
		{
           return(Map->Scale(on));
		}
		int GetCenterX()
		{
			return(Map->X + Map->Display.WINDOW_WIDTH/2);
		}
		int GetCenterY()
		{
			return(Map->Y + Map->Display.WINDOW_HEIGHT/2);
		}

		void Magnify(int MAX,int SIZE) // This needs a total re-write	
		{
           Map->MAG_MAX = 550-MAX; // Where does the 550 come from?
		   Map->MAG_SIZE = SIZE;
		   MAG_MAX = MAX;
		   MAG_SIZE = SIZE;
		}
		   void SetMouse(int X, int Y)
		   {
             Map->SetMouse((long)X,(long)Y);	
		   }
		   void SetBG(BYTE r,BYTE g,BYTE b); // Set Background color
		   void SetLoc(long X, long Y);
		   bool SetXY(long X, long Y);//,int &lastX, int &lastY);
		   char * getName(int num);
		   bool RemoveTile( int X, int Y);
           bool AddTile( void* ObjNum, int X, int Y,int variation);
		   bool AddObject( void* TileNum, int X, int Y, int callbackID, int DoorFacing);
		   bool SaveObjectToBMP( void* ObjNum, void* fileName);
		   //bool SaveWallToBMP( void* WallNum, void* fileName);
		   bool DeleteObject(int callbackID);
		   int InitScreen(unsigned int hWnd);
		   void ReInit(int x, int y);
		   void Render(bool Magnify);
		   bool Update_Window();

		   void ClearTiles(); // Clear all tile entries	
		   void ClearObjects(); // Clear all object entries
		   //void ClearWalls();
/*
short Load_Tile_Loc(int X,int Y,int TileNum=NULL,char TileName[]=NULL,int Variation=NULL);
    bool Load_Tile(int TileNum=NULL,char TileName[]=NULL);
    bool Load_Wall(int WallNum=NULL,char WallName[]=NULL);
	int Find_Object(char Name[]);

	bool RotatePoint(int &X,int &Y,float Degree);
    void Draw_Magnify(int x, int y);

    bool Draw_Text(char TEXT[],DWORD bgColor,DWORD fColor,int X,int Y,bool Transparent=true);
    bool Test_LClick_Tiles(int x,int y,int Type=NULL,int Vari=NULL,bool WALL = false,int Wall=NULL,int WallType=NULL);
    short Test_LClick_Sprites(int x,int y);
	bool Deselect_All();
	bool Update_Selected(int x,int y);
    bool Clear_Tile_Select();

    int Test_For_Tile(int &imgID);
	int Test_For_Image(int &imgID);
    bool Create(int winX,int winY,HWND win,bool Clipped=false,char *NoxPath=NULL,char *Binpath=NULL);
    bool Draw_GUI(bool DrawWalls=true,bool DrawTiles=true,bool DrawObjects=true);
	bool Add_Image_By_Item_Name(char name[]);
	bool Add_Image_By_Code(int imgCode);
	bool Add_Image(int Width,int Height,DWORD colorKey,char filename[]=NULL,int Imagenum=NULL,char Name[]=NULL);
    bool Add_Object(int x,int y,int Wx,int Wy,int imgNum,bool Is_Selected=false,int Ob_Type=NULL,int objNum=NULL);
	bool Add_Object(int x=0,int y=0,int objectNum=NULL,char Name[]=NULL,int objNum=NULL);
    bool Remove_Object(DDSPRITE *Object);
*/
	};

/*	public __gc class EventHandlerCallback
	{


		EventHandlerCallback()
		{}
		~EventHandlerCallback()
		{}
	};*/
}
