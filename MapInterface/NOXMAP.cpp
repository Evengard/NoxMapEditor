#include "NOXMAP.h"
/*bool NOX_MAP::AddWall(int X, int Y)
{
	if( X < 0 || X > 255 ||
		Y < 0 || Y > 255)
		return(false);

	//NOX_MAP::

return(true);
}*/
void NOX_MAP::Draw_Magnify(int x,int y)
{

	if(x < 0 || x > Display.WINDOW_WIDTH || y < 0 || y > Display.WINDOW_HEIGHT)
		return;


	int mod=MAG_MAX;
	if(x<MAG_MAX)
       mod=x;
	if(y<MAG_MAX)
	   mod=y;


	if(x>Display.WINDOW_WIDTH-MAG_MAX)
       mod=Display.WINDOW_WIDTH-x;
	if(y>Display.WINDOW_HEIGHT-MAG_MAX)
	   mod=Display.WINDOW_HEIGHT-y;


	RECT prc;
	prc.top=y-mod;
	prc.bottom=y+mod;
	prc.left=x-mod;
	prc.right=x+mod;

	Display.Display->Blt(0,0,MAG_SIZE,MAG_SIZE,Display.Display->GetBackBuffer(),&prc); 
    Display.Display->Blt_ColorRec(MAG_SIZE,0,2,MAG_SIZE+2,Display.Display->GetBackBuffer(),RGB(155,155,155));
    Display.Display->Blt_ColorRec(0,MAG_SIZE,MAG_SIZE+2,2,Display.Display->GetBackBuffer(),RGB(155,155,155));
}
bool NOX_MAP::AddTile(char objName[], int X, int Y, int variation)
{
	if( X < 0 || X > 255 ||
		Y < 0 || Y > 255)
		return(false);
	int ObjNum=0;
	ROLF *tile = NULL;
	for(tile = Thing.Thing.Tile.Tiles.Get(); tile && strcmp(tile->Name,objName); tile = Thing.Thing.Tile.Tiles.Get(), ObjNum++);
	Thing.Thing.Tile.Tiles.ClearGet();

	if( tile == NULL )
		return false;
	IDX_DB::Entry * Entry = NULL;
	IDX_DB::SectionHeader * Section = NULL;
	CSurface * surf;
    SURFACE tem;
	SURFACE * test;
	
		unsigned int val = *tile->Images.Get(variation); 
		tile->Images.ClearGet();
		val++;

test = Floor.TileImgs.Find(val);
if( !test )
{
	surf = NULL;
    fstream file;
	file.open(Video.bagpath,ios::in | ios::binary);
		Video.idx.GetAll(val,&Entry,&Section);
		
		if( Entry )
		{
		
			NxImage *img = Video.Extract(&file,Section,Entry);
			Display.Display->CreateSurface(&surf,img->width,img->height);
			if(surf)
				surf->SetColorKey(RGB(248,7,248));
			Display.BltBitmapData(img->data,img->width,img->height,surf->GetDDrawSurface());
			tem.surface = surf;
			tem.Height = img->height;
			tem.Width = img->width;
			tem.Image_ID = val;
			tem.ImgType = Entry->Entry_Type;
			tem.OffsetX = Entry->OffsetX;
			tem.OffsetY = Entry->OffsetY;
			img->Unload();
		}


		//LOAD THE NAME AS WELL NOW!!!!!
		Floor.TileImgs.Add(tem,val);    
		file.close();
		Floor.tileMap[X][Y].pTile = &Floor.TileImgs.last->msg;
}
else
{
  Floor.tileMap[X][Y].pTile = test;
}
		Floor.tileMap[X][Y].Tile = ObjNum;
		Floor.tileMap[X][Y].tVari = variation;
		Floor.tileMap[X][Y].TileImg = val;
		Floor.TileImgs.ClearGet();
	
		tem.surface = NULL;
		tem.name = NULL;
		surf = NULL;


return(true);
}
void NOX_MAP::Draw(bool DrawFloor, bool DrawWalls, bool DrawObjects, DWORD FloorColor)
{
Display.Display->Clear(FloorColor);
int TileSize = 23; //23
int Subtractor = 255; // 255
int FullTileSize = 46; // 45

int/*int*/ xoff = X%TileSize;	
int /*int*/ yoff = Y%TileSize;
float /*int*/ x = X/TileSize;
float /*int*/ y = Y/TileSize;

if( x < 0 )
   x = 0;
else if (x >(Subtractor - Display.WINDOW_WIDTH/TileSize))
   x = Subtractor - Display.WINDOW_WIDTH/TileSize;		

if( y < 0 )
   y = 0;
else if (y > (Subtractor - Display.WINDOW_HEIGHT/TileSize))
   y = Subtractor - Display.WINDOW_HEIGHT/TileSize;

	for(int i=(0),i2=(-1); i<(Display.WINDOW_HEIGHT/TileSize)+3/*Overlap so pretty edges*/;  i++,i2++)
	{
       for(int j=(0),j2=(-1); j<(Display.WINDOW_WIDTH/TileSize)+3/*Overlap*/; j++,j2++)
	   {
          if(Floor.tileMap[i+Y/TileSize][j+X/TileSize].Tile != (-1))
		  {       
			 SURFACE * tem = Floor.tileMap[i+Y/TileSize][j+X/TileSize].pTile; //=  Floor.TileImgs.Find(Floor.tileMap[i+Y/TileSize][j+X/TileSize].TileImg);
			 if( tem && tem->surface )
		        Display.Display->Blt(((j2)*TileSize)-xoff,((i2)*TileSize)-yoff,(FullTileSize),(FullTileSize),tem->surface->GetDDrawSurface(),NULL,DDBLT_KEYSRC);
	      }
	   }
	}

	OBJECT *obj = NULL;
	xoff = Display.WINDOW_WIDTH;
	yoff = Display.WINDOW_HEIGHT;

	for( int i=0; i<Objects.Objects.numNodes; i++)
	{
		obj = Objects.Objects.Get(i);
		if( !obj )
		{
			Objects.Objects.Remove(i);
			i--;
			continue;
		}
		if( (obj->X + obj->Width) > X && (obj->X) < X+xoff &&
            (obj->Y + obj->Height) > Y && (obj->Y) < Y+yoff)
		{
			// Use the objects extents to display it
			//Thing.Thing.Object.Objects.Get(obj->objID);
			SURFACE * tem = Objects.ObjImages.Find(obj->imgID);
			int xs = obj->Xlen;
			int ys = obj->Ylen;

			if( xs > 0 && obj->ExtentBox)
				xs /= 2;
			if( ys > 0 && obj->ExtentBox)
				ys /= 2;


			 if( tem && tem->surface )
			 {
				 if( obj->ExtentBox )
					 Display.Display->Blt((((obj->X-X)-obj->Width/2)-(xs)),((((obj->Y-Y)-obj->Height)-(ys))),tem->Width,tem->Height,tem->surface->GetDDrawSurface(),NULL,DDBLT_KEYSRC);
				 else if(obj->ExtentCircle)
					 Display.Display->Blt((((obj->X-X)-obj->Width/2)-xs),((((obj->Y-Y))-obj->Height)-xs),tem->Width,tem->Height,tem->surface->GetDDrawSurface(),NULL,DDBLT_KEYSRC);
				 else
					Display.Display->Blt(((obj->X-X)-/*tem->OffsetX*/obj->Width),((obj->Y-Y)-/*tem->OffsetY*/obj->Height),tem->Width,tem->Height,tem->surface->GetDDrawSurface(),NULL,DDBLT_KEYSRC);
			 }		
		}
	}
	Objects.Objects.ClearGet();

//Display.Draw_Text("Nox Graphical Map Editor Interface",
//				  RGB(0,0,0),RGB(120,120,120),0,0);
//Display.Draw_Text("Alpha 1.5",
//				  RGB(0,0,0),RGB(120,120,120),0,15);
//Display.Draw_Text("April 17 2007",
//				  RGB(0,0,0),RGB(120,120,120),0,30);

if( scaleOn )
{
	int x =ScaleSurface->GetDDrawDesc()->dwWidth;
		int y= ScaleSurface->GetDDrawDesc()->dwHeight;
	Display.Display->Blt(MOUSE_X-x/2,MOUSE_Y-y/2,x,y,ScaleSurface->GetDDrawSurface(),NULL,DDBLT_KEYSRC);
}

}
/*bool NOX_MAP::SaveWallBMP(char wallName[], char fileName[]) 
{
	int WallNum =0;
	LLAW * wall;
	for(wall = Thing.Thing.Wall.Walls.Get(); wall && strcmp(((char*)&wall->Name),wallName); wall = Thing.Thing.Wall.Walls.Get(), WallNum++);
	Thing.Thing.Wall.Walls.ClearGet();

	//OBJECT object;
    //SURFACE tem;
    //SURFACE * test = NULL;

	if( !wall )
      return(false);

	IDX_DB::Entry * Entry = NULL;
	IDX_DB::SectionHeader * Section = NULL;
    unsigned int val = 0;

	//wall->ImageCode
	if(wall->Images.numNodes > 0)	
		val = *wall->Images.Get(0);
          
		  if( !val || val < 0 )	
			  return(false);
	
		  Video.idx.GetAll(val,&Entry,&Section);
		  if( !Entry || !Section)
			  return false;

		  fstream file;	
		  file.open(Video.bagpath,ios::in | ios::binary);

		  Video.Extract(&file,Section,Entry,fileName//*"TESTMAP.bmp");
		  file.close();
return(true);
}*/
bool NOX_MAP::SaveObjectBMP(char objName[], char fileName[]) 
{
	int ObjNum =0;
	GNHT * obj;
	for(obj = Thing.Thing.Object.Objects.Get(); obj && strcmp(obj->Name,objName); obj = Thing.Thing.Object.Objects.Get(), ObjNum++);
	Thing.Thing.Object.Objects.ClearGet();

	OBJECT object;
    //SURFACE tem;
    //SURFACE * test = NULL;

	if( !obj )
      return(false);

	IDX_DB::Entry * Entry = NULL;
	IDX_DB::SectionHeader * Section = NULL;
    long val = 0;


		 // else if(obj->Stats.numNodes > 0 && obj->Stats.numNodes<2)
           //   val = *obj->Stats.Get(0)->Images.Get(0);

		  if(obj->Stats.numNodes)
              val = *obj->Stats.Get(0)->Images.Get(0);
		  else if(obj->PrettyImage!=NULL)
			  val = obj->PrettyImage;
		  else if(obj->MenuIcon!=NULL)
			  val = obj->MenuIcon;	
		  else
			  return(false);
	      if( val )
			  val++;
		  else
			  return(false);
          
		  if( !val || val < 0 )
			  return(false);

          //test = Objects.ObjImages.Find(val);
         // if( val && !(test = Objects.ObjImages.Find(val)) )
		 // {

		  Video.idx.GetAll((unsigned long)val,&Entry,&Section);
		  if( !Entry || !Section)
			  return false;
		  fstream file;	
		  file.open(Video.bagpath,ios::in | ios::binary);

		  Video.Extract(&file,Section,Entry,fileName/*"TESTMAP.bmp"*/);
		  file.close();
		  // Display.Display->CreateSurfaceFromBitmap(surface,"TESTMAP.bmp");
			 // Display.Display->CreateSurface(surface,img->width,img->height);
			  //Display.BltBitmapData(img->data, img->width, img->height, (*surface)->GetDDrawSurface());
			  //img->DrawToSurface(0,0,*surface,&Display.pixF,true);
			  //delete img;
			  //if( *surface )
			  //    (*surface)->SetColorKey(RGB(248,7,248));
			  //remove("TESTMAP.bmp");
		  //return(true);
		 // }
return(false);
}
bool NOX_MAP::LoadObject(char objName[], CSurface ** surface)
{
	int ObjNum =0;
	GNHT * obj;
	for(obj = Thing.Thing.Object.Objects.Get(); obj && strcmp(obj->Name,objName); obj = Thing.Thing.Object.Objects.Get(), ObjNum++);
	Thing.Thing.Object.Objects.ClearGet();

	OBJECT object;
    SURFACE tem;
    SURFACE * test = NULL;

	if( !obj )
      return(false);

	IDX_DB::Entry * Entry = NULL;
	IDX_DB::SectionHeader * Section = NULL;
    long val = 0;


		 // else if(obj->Stats.numNodes > 0 && obj->Stats.numNodes<2)
           //   val = *obj->Stats.Get(0)->Images.Get(0);

		  if(obj->Stats.numNodes)
              val = *obj->Stats.Get(0)->Images.Get(0);
		  else if(obj->PrettyImage!=NULL)
			  val = obj->PrettyImage;
		  else if(obj->MenuIcon!=NULL)
			  val = obj->MenuIcon;	
		  else
			  return(false);
	      if( val )
			  val++;
		  else
			  return(false);
          
		  if( !val || val < 0 )
			  return(false);

          //test = Objects.ObjImages.Find(val);
          if( val && !(test = Objects.ObjImages.Find(val)) )
		  {

		  Video.idx.GetAll((unsigned long)val,&Entry,&Section);
		  fstream file;	
		  file.open(Video.bagpath,ios::in | ios::binary);

				  NxImage * img = Video.Extract(&file,Section,Entry,/*"c://TESTMAP.bmp"*/NULL/*"TESTMAP.bmp"*/);
			 // Display.Display->CreateSurfaceFromBitmap(surface,"TESTMAP.bmp");
			  Display.Display->CreateSurface(surface,img->width,img->height);
			  Display.BltBitmapData(img->data, img->width, img->height, (*surface)->GetDDrawSurface());
			  //img->DrawToSurface(0,0,*surface,&Display.pixF,true);
			  delete img;
			  if( *surface )
			      (*surface)->SetColorKey(RGB(248,7,248));
			  //remove("TESTMAP.bmp");
		  }
return(true);
}
bool NOX_MAP::AddObject(char objName[], int X, int Y, int callback, int DoorFacing)
{
	/*RGBQUAD test;
	test.rgbBlue = 255;
	test.rgbRed = 255;
	Video.bmpfile.Set_Color(test);
*/
	int ObjNum =0;
	GNHT * obj;
	for(obj = Thing.Thing.Object.Objects.Get(); obj && strcmp(obj->Name,objName); obj = Thing.Thing.Object.Objects.Get(), ObjNum++);
	Thing.Thing.Object.Objects.ClearGet();
	OBJECT object;


    CSurface * surf = NULL;
    SURFACE tem;
    SURFACE * test = NULL;

	if( !obj )
      return(false);

	IDX_DB::Entry * Entry = NULL;
	IDX_DB::SectionHeader * Section = NULL;
    long val = 0;


		 // else if(obj->Stats.numNodes > 0 && obj->Stats.numNodes<2)
           //   val = *obj->Stats.Get(0)->Images.Get(0);
	if( DoorFacing == -1)
	{
		  if(obj->Stats.numNodes)
              val = *obj->Stats.Get(0)->Images.Get(0);
		  else if(obj->PrettyImage!=NULL)
			  val = obj->PrettyImage;
		  else if(obj->MenuIcon!=NULL)
			  val = obj->MenuIcon;	
		  else
			  return(false);
	      if( val )
			  val++;
		  else
			  return(false);
          
	}
	else
	{
		switch(DoorFacing)
		{
			case 0x00: // South
				val = *obj->Stats.Get(0)->Images.Get(0);
				break;
			case 0x08: // West
				val = *obj->Stats.Get(0)->Images.Get(8);
				break;
			case 0x10: // North
				val = *obj->Stats.Get(0)->Images.Get(16);
				break;
			case 0x18: // East
				val = *obj->Stats.Get(0)->Images.Get(24);
				break;

			default:break;
		}
		  if( val )
			  val++;
		  else
			  return(false);
	}


		  if( !val || val < 0 )
			  return(false);

          //test = Objects.ObjImages.Find(val);
          if( val && !(test = Objects.ObjImages.Find(val)) )
		  {

		  Video.idx.GetAll((unsigned long)val,&Entry,&Section);


			  //Video.Extract(&file,Section,Entry,"c://TESTMAP.bmp");
			 // Display.Display->CreateSurfaceFromBitmap(&surf,"C://TESTMAP.bmp");
			 // if( surf )
			 //     surf->SetColorKey(RGB(0,0,0));
			  //remove("c://TESTMAP.bmp");
			  

///////////////////////////////////////////////////////////////////////////////////////////////
if( Entry )
{		  
	fstream file;
    file.open(Video.bagpath,ios::in | ios::binary);

	NxImage *img = Video.Extract(&file,Section,Entry);

	if( img && img->height >0 && img->width >0)
	{
		Display.Display->CreateSurface(&surf,img->width,img->height);
	if( surf )
		surf->SetColorKey(RGB(248,7,248));
		Display.BltBitmapData(img->data, img->width, img->height, surf->GetDDrawSurface());
	}	
		tem.surface = surf;
		tem.Height = img->height;
		tem.Width = img->width;
		tem.Image_ID = val;
		tem.ImgType = Entry->Entry_Type;
		tem.OffsetX = Entry->OffsetX;
		tem.OffsetY = Entry->OffsetY;
		//tem.img.Load(img->width,img->height,img->offsetX,img->offsetY,img->data,(char*)&img->ID);
		img->Unload();
		//LOAD THE NAME AS WELL NOW!!!!!
		Objects.ObjImages.Add(tem,val);
		test = &Objects.ObjImages.last->msg;

	file.close();
}	  
///////////////////////////////////////////////////////////////////////////////////////////////
		  }
		 object.imgID = val;
		 object.objID = ObjNum;
		 object.X = X;
		 object.Y = Y;
		 object.callbackID = callback;
		 object.Xlen = 0;
		 object.Ylen = 0;
		 object.Z = 0;
		 object.zSizeX = 0;
		 object.zSizeY = 0;
		 object.DoorFacing = DoorFacing;
		 //object.ExtentBox = obj->Properties;
		 object.ExtentBox = false;
		 object.ExtentCircle = false;

		 Property *prop;
		 prop = obj->Properties.Get();
		 while( prop != NULL)
		 {
			char * st = strstr((char*)prop->Value,"EXTENT");
			if( st != NULL)
			{
				st+=9;
				if(strstr(st,"CIRCLE") != NULL)
				{
					st += 7;
					int val = atoi(st);
					object.ExtentCircle = true;
					object.Xlen = val;
				
					//is circle
					// read 1 #
				}
				else if(strstr(st,"BOX") != NULL)
				{
					st += 4;
					int val = atoi(st);
					while( *st != ' ')
						st++;
					st++;
					int val2 = atoi(st);
					object.ExtentBox = true;
					object.Xlen = val;
					object.Ylen = val2;
					//is box
					// read 2 #'s
				}
			}
			st = strstr((char*)prop->Value,"Z");
			if( st != NULL)
			{
				st += 4;
				object.Z = atoi(st);
			}
			st = strstr((char*)prop->Value,"ZSIZE");
			if( st != NULL)
			{
				st += 8;
				int val = atoi(st);
				while( *st != ' ')
					st++;
				st++;
				int val2 = atoi(st);
				object.zSizeX = val;
				object.zSizeY = val2;
			}	
			st = strstr((char*)prop->Value,"SIZE");
			if( st != NULL)
			{
				
			}	
			prop = obj->Properties.Get();
		 }
		 obj->Properties.ClearGet();
		
		if(test)
		 {
			object.Height = test->Height;
			object.Width = test->Width;
		 }
		 Objects.Objects.Add(object,object.callbackID);
	
	tem.surface = NULL;
	tem.name = NULL;
    surf = NULL;
	/*test.rgbBlue = 0;
	test.rgbRed = 0;
	Video.bmpfile.Set_Color(test);
*/
return(true);
}

bool NOX_MAP::DeleteObject(int callback)
{
	int i=0;
	OBJECT * obj = Objects.Objects.Get();

	for(; obj !=NULL && obj->callbackID != callback; obj = Objects.Objects.Get())
	{i++;}

	Objects.Objects.ClearGet();

	if( obj != NULL )
	{
		Objects.Objects.Remove(i);
		return true;
	}

return(false);
}
bool NOX_MAP::Scale(bool on)
{
 if( ScaleSurface )
 {
	ScaleSurface->Destroy();
	ScaleSurface = NULL;
 }
 scaleOn = on;
 if( on )
	LoadObject("NPC",&ScaleSurface);
return(true);
}