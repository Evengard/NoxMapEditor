
#include "GuiDisplay.h"
#include <math.h>
#include <stdio.h>

#define Detor(angle) ((((float)angle)*(3.14159))/180)

/**********************************************************************

Function: Rotate an xy point

**********************************************************************/
bool GUI_DISPLAY::RotatePoint(int &X,int &Y,float Degree)
{
   float X2=X;
   Degree = Detor(Degree);

   X = X*cos(Degree) - Y*sin(Degree);
   Y = X2*sin(Degree) + Y*cos(Degree);

return(1);
}
/**********************************************************************

Function: Draw directly to the surface

**********************************************************************/
bool GUI_DISPLAY::BltBitmapData(BYTE* data, int Width, int Height, LPDIRECTDRAWSURFACE7 pDDS)
{
  
	if( !data || !Height || !Width)
		return(NULL);

    DDSURFACEDESC2 ddsd;
    ZeroMemory( &ddsd,sizeof(ddsd) );
    ddsd.dwSize = sizeof(ddsd);

    // Lock the surface to directly write to the surface memory 
    if( FAILED( pDDS->Lock( NULL, &ddsd, DDLOCK_WAIT, NULL ) ) )
        return false;

	BYTE rVal,gVal,bVal;

if( pixF.BPP == 32 )//|| pixF.BPP == 24) // Need to change the 24 bit
{
	int X = 0;
	int Y = 0;
	DWORD* pDDSColor = (DWORD*) ( (BYTE*) ddsd.lpSurface);
	//DWORD ymax = Height*ddsd.lPitch;

	for( DWORD iY = 0; iY < Height/*ymax*/; iY+=1/*ddsd.lPitch*/ )
    {
        for( DWORD iX = 0; iX < Width; iX++ )
        {
			rVal=*(data++);
			gVal=*(data++);
			bVal=*(data++);

			*pDDSColor = (rVal<<pixF.Red) + (gVal<<pixF.Green) + (bVal<<pixF.Blue);
			pDDSColor++;
        }
		pDDSColor = (DWORD*) ( (BYTE*) ddsd.lpSurface + ( iY+1 )*ddsd.lPitch);
    }  
	}
    // Unlock the surface
    pDDS->Unlock(NULL); 

    return true;
}
/**********************************************************************

Function: Draw directly to the surface

**********************************************************************/
bool GUI_DISPLAY::Color_Shift_Surface(CSurface *surface)
{

    
    DDSURFACEDESC2 ddsd;


    //DDPIXELFORMAT pixStuff;

    LPDIRECTDRAWSURFACE7 pDDS = surface->GetDDrawSurface();
    //pDDS->GetPixelFormat(&pixStuff);

    ZeroMemory( &ddsd,sizeof(ddsd) );
    ddsd.dwSize = sizeof(ddsd);

    // Lock the surface to directly write to the surface memory 
    if( FAILED( pDDS->Lock( NULL, &ddsd, DDLOCK_WAIT, NULL ) ) )
        return false;


    for( DWORD iY = 0; iY < ddsd.dwHeight; iY++ )
    {
        for( DWORD iX = 0; iX < ddsd.dwWidth; iX++ )
        {
               DWORD * tem = (DWORD*) (( (BYTE*) ddsd.lpSurface + ( iY /*+ 1*/ ) * ddsd.lPitch ) + (iX*sizeof(DWORD)) );
			   *(tem) = ~*(tem); //RGBA_MAKE(red,green,blue,0); RGBQUAD
        }

    }
    
    // Unlock the surface
    pDDS->Unlock(NULL); 

    return true;
}
/**********************************************************************

Function: Color Overlay A Directx Surface

**********************************************************************/
bool GUI_DISPLAY::Color_Surface(int X,int Y,int Width,int Height,DWORD color,LPDIRECTDRAWSURFACE7 pDDS)
{

    DDSURFACEDESC2 ddsd;

    ZeroMemory( &ddsd,sizeof(ddsd) );
    ddsd.dwSize = sizeof(ddsd);

    // Lock the surface to directly write to the surface memory 
    if( FAILED( pDDS->Lock( NULL, &ddsd, DDLOCK_WAIT | DDLOCK_NOSYSLOCK , NULL ) ) )
        return false;


if(pixF.BPP == 32 )//|| pixF.BPP == 24) // Need to change the 24 bit
{
    int size = X*sizeof(DWORD);
	DWORD* pDDSColor = (DWORD*) ( (BYTE*) ddsd.lpSurface + ( Y + 1 ) * ddsd.lPitch +(size));
    BYTE rVal=RGB_GETBLUE(color),gVal=RGB_GETGREEN(color),bVal=RGB_GETRED(color);
    DWORD col = (rVal<<pixF.Red) | (gVal<<pixF.Green) | (bVal<<pixF.Blue);

    for( DWORD iY = 0; iY < Height; iY++ )
    {
        for( DWORD iX = 0; iX < Width; iX++ )
        {
			   *pDDSColor |= col;   
			   pDDSColor++;
        }
        pDDSColor = (DWORD*) ( (BYTE*) ddsd.lpSurface + ( Y+iY + 1 ) * ddsd.lPitch + (size));
    }
}
else if (pixF.BPP == 16)
{
	RGBQUAD col;
	col.rgbRed = RGB_GETBLUE(color);
	col.rgbBlue = RGB_GETRED(color);
	col.rgbGreen = RGB_GETGREEN(color);
    short *tem;

    for( short iY = 0; iY < ddsd.dwHeight; iY++ )
    {
        for( short iX = 0; iX < ddsd.dwWidth; iX++ )
        {
               tem = (short*) (( (BYTE*) ddsd.lpSurface + ( iY /*+ 1*/ ) * ddsd.lPitch ) + (iX*sizeof(short)) );
		       *(tem) |= pixF.Color_24_16(col); //RGBA_MAKE(red,green,blue,0); RGBQUAD
        }

    }
}

    // Unlock the surface
   pDDS->Unlock(NULL); 

    return true;
}
/**********************************************************************

Function: Color Overlay A Directx Surface

**********************************************************************/
bool GUI_DISPLAY::Color_Surface(int X,int Y,int Width,int Height,DWORD color,CSurface *surface)
{

    DDSURFACEDESC2 ddsd;

    //DDPIXELFORMAT pixStuff;

    LPDIRECTDRAWSURFACE7 pDDS = surface->GetDDrawSurface();
    //pDDS->GetPixelFormat(&pixStuff);

    ZeroMemory( &ddsd,sizeof(ddsd) );
    ddsd.dwSize = sizeof(ddsd);

    // Lock the surface to directly write to the surface memory 
    if( FAILED( pDDS->Lock( NULL, &ddsd, DDLOCK_WAIT, NULL ) ) )
        return false;


	//screenBPP = ddsd.ddpfPixelFormat.dwRGBBitCount;
   // RGBQUAD col;


if(pixF.BPP == 32)
{
	BYTE r,g,b;
	DWORD* pDDSColor = 0;//(DWORD*) ( (BYTE*) ddsd.lpSurface + ( Y + 1 ) * ddsd.lPitch );

    for( DWORD iY = 0; iY < Height; iY++ )
    {
        for( DWORD iX = 0; iX < Width; iX++ )
        {
              pDDSColor = (DWORD*) (( (BYTE*) ddsd.lpSurface + ( Y+iY /*+ 1*/ ) * ddsd.lPitch ) + ((X+iX)*sizeof(DWORD)) );
			   
				  r = ((*pDDSColor>>pixF.Red) | RGB_GETBLUE(color));

				  g = ((*pDDSColor>>pixF.Green) | RGB_GETGREEN(color));

				  b = ((*pDDSColor>>pixF.Blue) | RGB_GETRED(color));

			   *pDDSColor = pixF.Make_Pixel(r,g,b,NULL);
			//   pDDSColor++;
        }
          //pDDSColor = (DWORD*) ( (BYTE*) ddsd.lpSurface + ( iY + 1 ) * ddsd.lPitch );
    }
}
else if (pixF.BPP == 16)
{
	RGBQUAD col;
	col.rgbRed = RGB_GETBLUE(color);
	col.rgbBlue = RGB_GETRED(color);
	col.rgbGreen = RGB_GETGREEN(color);

    for( short iY = 0; iY < ddsd.dwHeight; iY++ )
    {
        for( short iX = 0; iX < ddsd.dwWidth; iX++ )
        {
               short * tem = (short*) (( (BYTE*) ddsd.lpSurface + ( iY /*+ 1*/ ) * ddsd.lPitch ) + (iX*sizeof(short)) );
		       *(tem) = pixF.Color_24_16(col); //RGBA_MAKE(red,green,blue,0); RGBQUAD
        }

    }
}
else if (pixF.BPP == 24)
{
    for( DWORD iY = 0; iY < ddsd.dwHeight; iY++ )
    {
        for( DWORD iX = 0; iX < ddsd.dwWidth; iX++ )
        {
               DWORD * tem = (DWORD*) (( (BYTE*) ddsd.lpSurface + ( iY /*+ 1*/ ) * ddsd.lPitch ) + (iX*24) );
		       *(tem) = pixF.Make_Pixel(100,NULL,NULL,NULL); //RGBA_MAKE(red,green,blue,0); RGBQUAD
        }

    }
}

    // Unlock the surface
   pDDS->Unlock(NULL); 

    return true;
}

/**********************************************************************

Function: Draw Text

**********************************************************************/
bool GUI_DISPLAY::Draw_Text(char string[],DWORD bgColor,DWORD fColor,int X,int Y,bool Transparent)
{

    HDC     hDC = NULL;
    HRESULT hr;

	LPDIRECTDRAWSURFACE7 m_pdds = Display->GetBackBuffer();

    if( m_pdds == NULL || string == NULL )
        return false;

    // Make sure this surface is restored.
    if( FAILED( hr = m_pdds->Restore() ) )
        return (0);

    if( FAILED( hr = m_pdds->GetDC( &hDC ) ) )
        return (0);

    // Set the background and foreground color
    SetBkColor( hDC, bgColor );
    SetTextColor( hDC, fColor );
	
	if(Transparent)
	  SetBkMode(hDC,TRANSPARENT); 

    // Use GDI to draw the text on the surface
    TextOut( hDC, X, Y, string, strlen(string) );

    if( FAILED( hr = m_pdds->ReleaseDC( hDC ) ) )
        return (0);

return(1);
}

/**********************************************************************

Function: Create Window View

**********************************************************************/
bool GUI_DISPLAY::Create(int winX,int winY,HWND win2)
{

	if(win2 != NULL)
	{
	  win=win2;
	}

  RECT rec;
  GetClientRect( win,&rec );

  WINDOW_WIDTH=rec.right;
  WINDOW_HEIGHT=rec.bottom;
  WINDOW_X=winX;
  WINDOW_Y=winY;
  
  if(win2==NULL)
   InitDirectDraw(win,true);
  else
   InitDirectDraw(win);


    DDSURFACEDESC2 ddsd;
    ZeroMemory( &ddsd,sizeof(ddsd) );
    ddsd.dwSize = sizeof(ddsd);

	// These functions get the display format;

    // Lock the surface to directly write to the surface memory 
    if( FAILED( Display->GetFrontBuffer()->Lock( NULL, &ddsd, DDLOCK_WAIT, NULL ) ) )
        return false;

	pixF.BPP = (BYTE)ddsd.ddpfPixelFormat.dwRGBBitCount;
    pixF.SetValues(ddsd.ddpfPixelFormat.dwRBitMask,ddsd.ddpfPixelFormat.dwGBitMask,ddsd.ddpfPixelFormat.dwBBitMask,ddsd.ddpfPixelFormat.dwRGBAlphaBitMask);
   
	Display->GetFrontBuffer()->Unlock(NULL); 




return(true);
}

/**********************************************************************

Function: Draw view to backsurface

**********************************************************************/
bool GUI_DISPLAY::Draw_GUI(bool DrawWalls,bool DrawTiles,bool DrawObjects)
{
  Display->Blt(0,0,NULL,NULL,BackGround->GetDDrawSurface() ,NULL,NULL);
return(1);
}

/**********************************************************************

Function: Render View

**********************************************************************/
bool GUI_DISPLAY::Render()
{
    if( FAILED(Display->Present()))
		return(0);
return(1);
}

/**********************************************************************

Function: Initialize Direct Draw

**********************************************************************/
HRESULT GUI_DISPLAY::InitDirectDraw( HWND hWnd,bool isReset)
{
    LPDIRECTDRAWPALETTE pDDPal = NULL; 
    HRESULT	hr;

   if(isReset)
   {
      delete Display;
   }

    Display = new CDisplay();
    if( FAILED( hr = Display->CreateWindowedDisplay( hWnd, WINDOW_WIDTH, WINDOW_HEIGHT ) ) )
    {
        MessageBox( hWnd, TEXT("Failed initializing DirectDraw."),
                    TEXT("DirectDraw Sample"), MB_ICONERROR | MB_OK );
        return hr;
    }
	
    Display->Clear(RGB(0,0,0));

	/*if(!isReset)
	{
	CSurface *me;

	sprintf(buff,"%ssplash.bmp",BinPath);
	if( !FAILED(Display->CreateSurfaceFromBitmap(&me,buff,157,105)))
	{
	
    Display->Blt(WINDOW_WIDTH/2-(157/2)-15,WINDOW_HEIGHT/2-(105/2),157,105,me->GetDDrawSurface(),NULL);
	me->Destroy();
	}
	}*/

    DDCAPS ddcaps;
    ZeroMemory( &ddcaps, sizeof(ddcaps) );
    ddcaps.dwSize = sizeof(ddcaps);
    Display->GetDirectDraw()->GetCaps( &ddcaps, NULL );
    if( (ddcaps.dwCaps2 & DDCAPS2_CANRENDERWINDOWED) != 0 )
    {
      Can_Draw_GDI = true;
    }
	
    if( FAILED( hr = Display->Present() ) )
        return hr;

	if( !isReset )
	{
	//BACKGROUND SURFACE
	if ( FAILED( hr = Display->CreateSurface(&BackGround,WINDOW_WIDTH,WINDOW_HEIGHT )))
		 return hr; 
	}

    LPDIRECTDRAWCLIPPER pcClipper;

    if( FAILED( hr = Display->m_pDD->CreateClipper( 0, &pcClipper, NULL ) ) )
        return E_FAIL;

    RECT rec;
    CLIPLIST ClipList;


    rec.left=WINDOW_X;
    rec.top =WINDOW_Y;
    rec.right = WINDOW_WIDTH;
    rec.bottom = WINDOW_HEIGHT;
    ClipList.hdr.dwSize=sizeof(RGNDATAHEADER);
    ClipList.hdr.iType=RDH_RECTANGLES;
    ClipList.hdr.nCount=1;
    ClipList.hdr.nRgnSize=0;
    memcpy(&ClipList.hdr.rcBound, &rec, sizeof(RECT));
    memcpy(&ClipList.rgndata, &rec, sizeof(RECT));


    if( FAILED( hr = pcClipper->SetClipList((LPRGNDATA)&ClipList,0)))
	{
       pcClipper->Release();
       return hr;
    }
   /* if( FAILED( hr = tileMap.tileSurface->m_pdds->SetClipper( pcClipper ) ) )
    {
        pcClipper->Release();
        return E_FAIL;
    }*/

    pcClipper->Release();

    if(isReset)
	{return(1);}

return S_OK;
}

/**********************************************************************

Function: Initialize Direct Draw

**********************************************************************/
bool GUI_DISPLAY::Update_Bounds()
{
	Display->UpdateBounds();
return(true);
}


