// This is the main DLL file.
#include "MapInterface.h"
#include "ProcessFuncs.h"
namespace MapInterface // Map interface namespace
{
	bool MAP_GUI::RemoveTile( int X, int Y) // Remove tile
	{
		Map->Floor.tileMap[Y][X].TileImg = NULL;
        Map->Floor.tileMap[Y][X].pTile = NULL;
		memset(&Map->Floor.tileMap[Y][X],0x00,sizeof(TILE_INFO));
		Map->Floor.tileMap[Y][X].Tile = (-1);

		return(true); // Why always true?
	}
	void MAP_GUI::SetBG(BYTE r,BYTE g,BYTE b)
	{
      Map->BGColor = RGB(b,g,r);
	}
	void MAP_GUI::ClearMap()
	{
		Map->Floor.Clear();
		Map->Objects.Clear();
		Map->X = 0;
		Map->Y = 0;
	}
	void MAP_GUI::ClearTiles()
	{
		Map->Floor.Clear();
	}
	void MAP_GUI::ClearObjects()
	{
		Map->Objects.Clear();
	}

    void MAP_GUI::SetLoc(long X, long Y)//, int &lastX, int &lastY)
	{		
		Map->X = X - Map->Display.WINDOW_WIDTH/2;
	    Map->Y = Y - Map->Display.WINDOW_HEIGHT/2;

	   if(((Map->X+1)/23)>253-(Map->Display.WINDOW_WIDTH/23))
	   {Map->X=23*(253-(Map->Display.WINDOW_WIDTH/23));}
  	   else if(((Map->X+1)/23)<2)
	   {Map->X=46;}  
   
	   if(((Map->Y+1)/23)>(253)-(Map->Display.WINDOW_HEIGHT/23))
	   {Map->Y=23*((253)-(Map->Display.WINDOW_HEIGHT/23));}
  	   else if(((Map->Y+1)/23)<1)
	   {Map->Y=46;}
	}
	bool MAP_GUI::SetXY(long X, long Y)//, int &lastX, int &lastY)
	{	
       Map->X += X;
	   Map->Y += Y;
       
	   if(((Map->X+1)/23)>253-(Map->Display.WINDOW_WIDTH/23))
	   {Map->X=23*(253-(Map->Display.WINDOW_WIDTH/23));}
  	   else if(((Map->X+1)/23)<2)
	   {Map->X=46;}  
   
	   if(((Map->Y+1)/23)>(253)-(Map->Display.WINDOW_HEIGHT/23))
	   {Map->Y=23*((253)-(Map->Display.WINDOW_HEIGHT/23));}
  	   else if(((Map->Y+1)/23)<1)
	   {Map->Y=46;}

		return(true);
	}
	char * MAP_GUI::getName(int num)
	{
		return(Map->Thing.Thing.Object.Objects.Get(num)->Name);
	}
	/*bool MAP_GUI::SaveWallToBMP( void* WallNum, void* fileName)
	{
		if( !WallNum || !fileName)
			return(false);

		char buff[255];
		char buff2[255];
		strcpy(buff,(const char*)WallNum);
		strcpy(buff2,(const char*)fileName);

		return(Map->SaveWallBMP(buff,buff2));
	}*/
	bool MAP_GUI::SaveObjectToBMP( void* ObjNum, void* fileName)
	{
		if( !ObjNum || !fileName)
			return(false);

		char buff[255];
		char buff2[255];
		strcpy(buff,(const char*)ObjNum);
		strcpy(buff2,(const char*)fileName);

		return(Map->SaveObjectBMP(buff,buff2));
	}
	bool MAP_GUI::AddObject(void *ObjNum, int X, int Y, int callbackID, int DoorFacing)
    {
		if( !ObjNum )
			return(false);

		char buff[255];
		strcpy(buff,(const char*)ObjNum);
		return(Map->AddObject(buff,X,Y,callbackID,DoorFacing));
    }	
	bool MAP_GUI::DeleteObject(int callbackID)
    {
		return(Map->DeleteObject(callbackID));
    }	
	bool MAP_GUI::AddTile(void *tileNum, int X, int Y,int variation)
    {
		if( !tileNum )
			return(false);

		char buff[255];
		strcpy(buff,(const char*)tileNum);
		return(Map->AddTile(buff,X,Y,variation));
    }

	void MAP_GUI::ReInit(int x, int y)
    {
	  Map->Display.WINDOW_WIDTH = x;
	  Map->Display.WINDOW_HEIGHT = y;
      Map->Display.InitDirectDraw(win,true);

    }

	void MAP_GUI::Render(bool Magnify)
    {
	 Map->Draw(true,true,true,Map->BGColor);

	 if( Magnify )
	    Map->Draw_Magnify(MOUSEX,MOUSEY);

	 Map->Display.Render();
	}

    bool MAP_GUI::Update_Window()
	{
      Map->Display.Update_Bounds();
    return(1);
	}

	int MAP_GUI::InitScreen(unsigned int hWnd)
    {
		win = (HWND)hWnd;
		Map->Display.Create(0,0,win);
		Map->X = 100;
		Map->Y = 100;
		return(true);
	}
}