//-----------------------------------------------------------------------------
// File: ddutil.cpp
//
// Desc: Routines for loading bitmap and palettes from resources
//
// Copyright (C) 1998-2001 Microsoft Corporation. All Rights Reserved.
//-----------------------------------------------------------------------------

#ifndef DDUTIL_H
#define DDUTIL_H

#include <ddraw.h>
#include <d3d.h>
//#include "BmpLib.h"





//-----------------------------------------------------------------------------
// Classes defined in this header file 
//-----------------------------------------------------------------------------
class CDisplay;
class CSurface;




//-----------------------------------------------------------------------------
// Flags for the CDisplay and CSurface methods
//-----------------------------------------------------------------------------
#define DSURFACELOCK_READ
#define DSURFACELOCK_WRITE

	typedef struct {RGNDATAHEADER hdr; RECT rgndata[4]; } CLIPLIST, *LPCLIPLIST;


//-----------------------------------------------------------------------------
// Name: class CDisplay
// Desc: Class to handle all DDraw aspects of a display, including creation of
//       front and back buffers, creating offscreen surfaces and palettes,
//       and blitting surface and displaying bitmaps.
//-----------------------------------------------------------------------------
class CDisplay
{
public:
    LPDIRECTDRAW7        m_pDD;
	LPDIRECTDRAWSURFACE7 BackGroundBuffer;
protected:

    LPDIRECTDRAWSURFACE7 m_pddsFrontBuffer;
    LPDIRECTDRAWSURFACE7 m_pddsBackBuffer;
    LPDIRECTDRAWSURFACE7 m_pddsBackBufferLeft; // For stereo modes

    HWND                 m_hWnd;
    RECT                 m_rcWindow;
    BOOL                 m_bWindowed;
    BOOL                 m_bStereo;

public:
    CDisplay();
    ~CDisplay();

    // Access functions
    enum Status_Flags
	{
		LOST_FOCUS = 0x01,
		NEED_REFRESH = LOST_FOCUS << 1
	};
	DWORD STATUS_FLAGS;

    bool Reset()
	{
	  m_pDD->Release();
	  m_pddsFrontBuffer->Release();
	  m_pddsBackBuffer->Release();
	  BackGroundBuffer->Release();
    
	  return(1);
	}

    HWND                 GetHWnd()           { return m_hWnd; }
    LPDIRECTDRAW7        GetDirectDraw()     { return m_pDD; }
    LPDIRECTDRAWSURFACE7 GetFrontBuffer()    { return m_pddsFrontBuffer; }
    LPDIRECTDRAWSURFACE7 GetBackBuffer()     { return m_pddsBackBuffer; }
    LPDIRECTDRAWSURFACE7 GetBackBufferLeft() { return m_pddsBackBufferLeft; }

    // Status functions
    BOOL    IsWindowed()                     { return m_bWindowed; }
    BOOL    IsStereo()                       { return m_bStereo; }

    // Creation/destruction methods
    HRESULT CreateFullScreenDisplay( HWND hWnd, DWORD dwWidth, DWORD dwHeight,
		                             DWORD dwBPP );
    HRESULT CreateWindowedDisplay( HWND hWnd, DWORD dwWidth, DWORD dwHeight,bool Clipped=false);
    HRESULT InitClipper();
    HRESULT UpdateBounds(int &WINDOW_WIDTH,int &WINDOW_HEIGHT);
    HRESULT UpdateBounds();
    virtual HRESULT DestroyObjects();

    // Methods to create child objects
    HRESULT CreateSurface( CSurface** ppSurface, DWORD dwWidth,
		                   DWORD dwHeight );
    HRESULT CreateSurfaceFromBitmap( CSurface** ppSurface, TCHAR* strBMP);
    HRESULT CreateSurfaceFromBitmap( CSurface** ppSurface, TCHAR* strBMP,
		                             DWORD dwDesiredWidth,
									 DWORD dwDesiredHeight );
    HRESULT CreateSurfaceFromText( CSurface** ppSurface, HFONT hFont,
		                           TCHAR* strText, 
								   COLORREF crBackground,
								   COLORREF crForeground );
    HRESULT CreatePaletteFromBitmap( LPDIRECTDRAWPALETTE* ppPalette, const TCHAR* strBMP );

    // Display methods
    HRESULT Blt_ColorRec( DWORD x, DWORD y,DWORD Width,DWORD Height, LPDIRECTDRAWSURFACE7 pdds,DWORD dwColor=NULL);

    HRESULT Clear( DWORD dwColor = 0L );
    HRESULT ColorKeyBlt( DWORD x, DWORD y, LPDIRECTDRAWSURFACE7 pdds,
                         RECT* prc = NULL );
    HRESULT Blt( DWORD x, DWORD y, DWORD Width, DWORD Height, LPDIRECTDRAWSURFACE7 pdds,
		         RECT* prc=NULL, DWORD dwFlags=0 );
    HRESULT Blt_Fast( DWORD x, DWORD y, LPDIRECTDRAWSURFACE7 pdds,
		         RECT* prc=NULL, DWORD dwFlags=0 );
    HRESULT Blt_Trans( DWORD x, DWORD y, LPDIRECTDRAWSURFACE7 pdds,
		         RECT* prc=NULL, DWORD dwFlags=0, int Alpha=0 );

    HRESULT Scale_Blt( DWORD x, DWORD y, LPDIRECTDRAWSURFACE7 pdds,
		         RECT* prc=NULL, DWORD dwFlags=0 );


    HRESULT Blt( DWORD x, DWORD y, DWORD Width, DWORD Height, CSurface* pSurface, RECT* prc = NULL );
    HRESULT BltFast( DWORD x, DWORD y,CSurface* pSurface, RECT* prc = NULL );
    HRESULT Blt_Trans( DWORD x, DWORD y, CSurface* pSurface, RECT* prc = NULL, int Alpha=0 );

    HRESULT ShowBitmap( HBITMAP hbm, LPDIRECTDRAWPALETTE pPalette=NULL );
    HRESULT SetPalette( LPDIRECTDRAWPALETTE pPalette );
    HRESULT Present();

    HRESULT DrawText( HFONT hFont, TCHAR* strText, DWORD dwOriginX, DWORD dwOriginY,
		              COLORREF crBackground, COLORREF crForeground );


};




//-----------------------------------------------------------------------------
// Name: class CSurface
// Desc: Class to handle aspects of a DirectDrawSurface.
//-----------------------------------------------------------------------------
class CSurface
{
public:
    LPDIRECTDRAWSURFACE7 m_pdds;

private:
    DDSURFACEDESC2       m_ddsd;
    BOOL                 m_bColorKeyed;

public:

     HRESULT CSurface::BltFast( DWORD x, DWORD y, CSurface* pSurface, RECT* prc );
     HRESULT CSurface::Blt_Fast( DWORD x, DWORD y, LPDIRECTDRAWSURFACE7 pdds, RECT* prc,
                       DWORD dwFlags );

     HRESULT Blt( DWORD x, DWORD y,DWORD Width,DWORD Height, LPDIRECTDRAWSURFACE7 pdds,
		         RECT* prc=NULL, DWORD dwFlags=0 );

     HRESULT Blt( DWORD x, DWORD y,DWORD Width,DWORD Height, CSurface* pSurface, RECT* prc = NULL );

     HRESULT DrawBitmapEx( TCHAR *strBMP, 
                              DWORD dwBMPOriginX, DWORD dwBMPOriginY, 
                              DWORD dwBMPWidth, DWORD dwBMPHeight,DWORD DesiredWidth, DWORD DesiredHeight , int xOffset);
	
	LPDIRECTDRAWSURFACE7 GetDDrawSurface() { return m_pdds; }
	LPDDSURFACEDESC2 GetDDrawDesc() { return &m_ddsd; }
    BOOL                 IsColorKeyed()    { return m_bColorKeyed; }

    HRESULT DrawBitmap( HBITMAP hBMP, DWORD dwBMPOriginX = 0, DWORD dwBMPOriginY = 0, 
		                DWORD dwBMPWidth = 0, DWORD dwBMPHeight = 0 );
    HRESULT DrawBitmap( TCHAR* strBMP, DWORD dwDesiredWidth, DWORD dwDesiredHeight );
    HRESULT DrawText( HFONT hFont, TCHAR* strText, DWORD dwOriginX, DWORD dwOriginY,
		              COLORREF crBackground, COLORREF crForeground );

    HRESULT Clear( DWORD dwColor );

    HRESULT SetColorKey( DWORD dwColorKey );
    DWORD   ConvertGDIColor( COLORREF dwGDIColor );
    static HRESULT GetBitMaskInfo( DWORD dwBitMask, DWORD* pdwShift, DWORD* pdwBits );

    HRESULT Create( LPDIRECTDRAW7 pDD, DDSURFACEDESC2* pddsd );
    HRESULT Create( LPDIRECTDRAWSURFACE7 pdds );
    HRESULT Destroy();

    CSurface();
    ~CSurface();
};




#endif // DDUTIL_H

