#pragma once
//#include "stdafx.h"
#include "ddutil.h"
#include "dxutil.h"





class PIXEL_FORMAT
{

public:
  BYTE BPP;
  BYTE Red;
  BYTE Green;
  BYTE Blue;
  BYTE Alpha;

  DWORD Pixel;
  
  inline BYTE GetRed(DWORD color)
  {
   return((BYTE)(color >> Red));
  }
  inline BYTE GetBlue(DWORD color)
  {
   return((BYTE)(color >> Blue));
  }
  inline BYTE GetGreen(DWORD color)
  {
   return((BYTE)(color >> Green));
  }

  inline void Get_Pixel(DWORD color,RGBQUAD *col)
  {
    col->rgbRed = (BYTE)(color >> Red);
    col->rgbGreen = (BYTE)(color >> Green);
    col->rgbBlue = (BYTE)(color >> Blue);
  }

  inline DWORD Make_Pixel(BYTE rVal,BYTE gVal,BYTE bVal,BYTE aVal)
  {
   Pixel = 0;
   Pixel |= (rVal << Red);
   Pixel |= (gVal << Green);
   Pixel |= (bVal << Blue);
   //Pixel |= (aVal << Alpha);
   return(Pixel);
  }

  // No need to set values in 16 bit mode, it should use a singular value
  void SetValues(DWORD rVal,DWORD gVal,DWORD bVal,DWORD aVal)
  {
	int i;
    
	if(BPP == 16)
	{
        for(i=0; ((bVal >> (i)) > 0); (Blue = (i)),i++); // Get Blue
        for(i=0; ((gVal >> (i)) > 0); (Green = (i)),i++); // Get Green
        for(i=0; ((rVal >> (i)) > 0); (Red = (i)),i++); // Get Red
        for(i=0; (aVal >> (8*i)); (Alpha = (i*8)),i++); // Get Alpha
		return;
	}

    for(i=0; (bVal >> (i*8)); (Blue = (i*8)),i++); // Get Blue
    for(i=0; (gVal >> (i*8)); (Green = (i*8)),i++); // Get Green
    for(i=0; (rVal >> (i*8)); (Red = (i*8)),i++); // Get Red
    //for(i=0; (aVal >> (8*i)); (Alpha = (i*8)),i++); // Get Alpha
  }

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


  RGBQUAD Color_16_24(short val)
  {
    RGBQUAD color;
	int j,i;
    int len=0;
    for(int i=0; i<5; i++)
    {
      int mul=1;
      for(j=0; j<i; j++)
      {mul=mul*2;}
      len+=val%2*mul;

      val=val>>1;
    }
      color.rgbBlue=len;
      val=val>>5;
      len=0;
      for(i=0; i<5; i++)
      {
       int mul=1;
       for(j=0; j<i; j++)
       {mul=mul*2;}
       len+=val%2*mul;
       val=val>>1;
      }
      color.rgbGreen=len;
      val=val>>5;
      len=0;
      for(i=0; i<5; i++)
      {
      int mul=1;
      for(j=0; j<i; j++)
      {mul=mul*2;}
      len+=val%2*mul;
      val=val>>1;
      }
      color.rgbRed=len;
    return(color);
  }
};

class GUI_DISPLAY
{

public:

    PIXEL_FORMAT pixF;
    HFONT GuiFont; // The standard font for the game

	bool Reload_Images;
    bool Is_Splash;
	bool Is_Loading;
	bool Is_Minimized;

	bool Can_Draw_GDI;

	int WINDOW_WIDTH;
	int WINDOW_HEIGHT;
	int WINDOW_X;
	int WINDOW_Y;

	int MOUSE_X;
	int MOUSE_Y;

    bool ShowCur;

	HWND win;

	CDisplay*  Display;
	CSurface*  BackGround;
    
	DWORD Get_Display_Status()
	{
		return( Display->STATUS_FLAGS );
	}
	void Set_Display_Status(DWORD newVal)
	{
        Display->STATUS_FLAGS = newVal;
	}
	bool Update_Bounds();
    bool BltBitmapData(BYTE* data, int Width, int Height, LPDIRECTDRAWSURFACE7 pDDS);
    bool Color_Surface(int X,int Y,int Width,int Height,DWORD color,LPDIRECTDRAWSURFACE7 surface);
    bool Color_Surface(int X,int Y,int Width,int Height,DWORD color,CSurface *surface);
    bool Color_Shift_Surface(CSurface *surface);
	bool RotatePoint(int &X,int &Y,float Degree);
    bool Draw_Text(char TEXT[],DWORD bgColor,DWORD fColor,int X,int Y,bool Transparent=true);
    bool Create(int winX,int winY,HWND win);
    bool Draw_GUI(bool DrawWalls=true,bool DrawTiles=true,bool DrawObjects=true);
	bool Render();

    HRESULT InitDirectDraw( HWND hWnd,bool isReset=false);

GUI_DISPLAY(int WIN_WIDTH,int WIN_HEIGHT)
{
  WINDOW_WIDTH = WIN_WIDTH;
  WINDOW_HEIGHT = WIN_HEIGHT;
  Display = NULL;
}

GUI_DISPLAY()
{
  BackGround = NULL;
  WINDOW_WIDTH = 0;
  WINDOW_HEIGHT = 0;
  Display = NULL;
  ShowCur = false;
  Can_Draw_GDI = false;
  Is_Minimized = false;
  Reload_Images = false;
}

~GUI_DISPLAY()
{
	if(BackGround)
		delete BackGround;
	
}

};

