#include "Thinglib.h"
#include "CryptFuncs.h"


//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
int ThingBin::Parse_String(char str[],int strLen)
{

	int aniType=0;

//CString str2=str;

if(strstr(str,"Update")!=NULL || strstr(str,"UPDATE")!=NULL)
//if(str2.Find("Update")!=(-1) || str2.Find("UPDATE")!=(-1))
{return(0);}

        if(
			!strcmp(str,"StaticDraw") ||
			!strcmp(str,"ArmorDraw")  ||
			!strcmp(str,"WeaponDraw") ||
		    !strcmp(str,"SlaveDraw")  ||		
			!strcmp(str,"BaseDraw")  
			)
		{aniType=1;}
		 else if(!strcmp(str,"PlayerDraw"))
		 {aniType=10;}
        else if(
			!strcmp(str,"AnimateDraw")         ||
			!strcmp(str,"SphericalShieldDraw") ||
			!strcmp(str,"WeaponAnimateDraw")   ||
		    !strcmp(str,"FlagDraw")            ||		
			!strcmp(str,"SummonEffectDraw")    ||
			!strcmp(str,"ReleasedSoulDraw")    ||
			!strcmp(str,"GlyphDraw")
			)
		{aniType=2;}
        else if(
			!strcmp(str,"AnimateStateDraw") ||
			!strcmp(str,"MonsterDraw")      ||
          //  !strcmp(str,"PlayerDraw")       ||
		    !strcmp(str,"MaidenDraw")			
			)
		{aniType=3;}
        else if(
			!strcmp(str,"BoulderDraw")       ||
			!strcmp(str,"StaticRandomDraw")  ||
			!strcmp(str,"DoorDraw")          ||
		    !strcmp(str,"ArrowDraw")         ||		
			!strcmp(str,"HarpoonDraw")       ||
			!strcmp(str,"WeakArrowDraw")
			)
		{aniType=4;}
		else if(!strcmp(str,"VectorAnimateDraw"))
		{aniType=5;}
		else if(!strcmp(str,"ConditionalAnimateDraw") || !strcmp(str,"MonsterGeneratorDraw"))
		{aniType=6;}
		else if(!strcmp(str,"MENUICON"))
		{aniType=7;}
		else if(!strcmp(str,"PRETTYIMAGE"))
		{aniType=8;}
        else if(strstr(str,"Draw")!=NULL)
		{aniType=9;}
		else
		{aniType=0;}


return(aniType);
}
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
bool ThingBin::Load_Thingdb(char FileName[])
{

ifstream file;
file.open(FileName,ios::in | ios::binary);
long count=0;
file.seekg(0,ios::end);
long len = file.tellg();
file.seekg(0,ios::beg);
unsigned char *buff;
//long count=0;


buff = new unsigned char [len+10];
memset(buff,0x00,len+10);

file.read((char*)buff,len);
file.close();
Ucrypt(buff,len,true,NULL,Crypt_Thing);
memset(Type,0x00,5);
memcpy((void*)&*(Type),(const void*)&*(buff+count),sizeof(int));
//count+=4;


Load_Tiles(buff,count, Thing.Tile);
Load_Walls(buff,count, Thing.Wall);

while(strcmp(Type,"GAMI") && count<len)
	  {
         //  Walls.Walls[Walls.numWalls]=Load_Wall(count,buff);
           //Walls.numWalls++;
	        memset(Type,0x00,5);
            memcpy((void*)&*(Type),(const void*)&*(buff+count),sizeof(int));
            count++;
		  }
count+=3;
Load_Images(buff,count,Thing.Image);

while(strcmp(Type,"GNHT"))
	  {
         //  Walls.Walls[Walls.numWalls]=Load_Wall(count,buff);
           //Walls.numWalls++;
 	        memset(Type,0x00,5);          
	        memcpy((void*)&*(Type),(const void*)&*(buff+count),sizeof(int));
            count++;
		  }
//count+=3;
Load_Objects(buff,count,len, Thing.Object);

//out.Close();

delete [] buff;
Is_Loaded=true;
return(true);
}
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
bool ThingBin::Load_Images(unsigned char *buff,long &count, GAMISTRUCT & Images)
{

GAMI Image;

int numImages = 0;
memcpy((void*)&numImages,(const void*)&*(buff+count),sizeof(int));
count+=4;

int pos=0;
//numImages=20; //NEED TO MAKE THIS A REAL VALUE BY ADDING ANIMATION SUPPORT!!!

      while(pos<numImages) 
	  {
           Load_Image(count,buff,Image);
		   Images.Images.Add(Image);
		   memset((void*)&Image, 0x00,sizeof(GAMI));

		   pos++;
		  }
            memcpy((void*)&*(Type),(const void*)&*(buff+count),sizeof(int));
            count+=4;


return(true);
}
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
bool ThingBin::Load_Walls(unsigned char *buff,long &count, LLAWSTRUCT & Walls)
{

//LLAWSTRUCT Walls;
//Walls.Load();
LLAW Wall;
      while(!strcmp(Type,"LLAW"))
	  {
		   
	       memset((void*)&Wall, 0x00,sizeof(LLAW));
           Load_Wall(count,buff,Wall);
	       Thing.Wall.Walls.Add(Wall);
		   memcpy((void*)&*(Type),(const void*)&*(buff+count),sizeof(int));
           count+=4;
	  }

memset((void*)&Wall, 0x00,sizeof(LLAW));

return(true);
}
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
bool ThingBin::Load_Tiles(unsigned char *buff,long &count,ROLFSTRUCT & Data)
{



   ROLF Tile;
   memset(Type,0x00,5);
   memcpy((void*)&*(Type),(const void*)&*(buff+count),sizeof(int));
   count+=4;
      while(!strcmp(Type,"ROLF") || !strcmp(Type,"EGDE"))
	  {
           
		   
           Load_Tile(count,buff,Tile);
		   Thing.Tile.Tiles.Add(Tile);
	       memset((void*)&Tile, 0x00,sizeof(ROLF));
           
             memcpy((void*)&*(Type),(const void*)&*(buff+count),sizeof(int));
             count+=4;
	  }


return(true);
}
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
bool ThingBin::Load_Objects(unsigned char *buff,long &count,long filelen,GNHTSTRUCT & Data)
{

      GNHT Object;
      while(!strcmp(Type,"GNHT"))  
       {
	       memset((void*)&Object, 0x00,sizeof(GNHT));
           Load_Object(count,buff,filelen,Object);
		   Thing.Object.Objects.Add(Object);
           memset((void*)&Object, 0x00,sizeof(GNHT));

		  while(strcmp(Type,"GNHT") && count<filelen)
		  {
             memcpy((void*)&*(Type),(const void*)&*(buff+count),sizeof(int));
             count++;
		  }
	   }


return(true);
}
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
bool ThingBin::Load_Object(long &count,unsigned char *buff,long filelen, GNHT & Object)
{

  int aniType=0;
  int strLen=0;
  char str[255];
  int Length32=0;
  long Length64=0;
  long finishPos=0;
  //CString str2;
  int k=0,j=0;
  int subcount=0;
  TATS Stat;
  memset((void*)&Stat,0x00,sizeof(TATS));
  Property Prop;
  memset((void*)&Prop,0x00,sizeof(Property));

  TATS ExtentStat;
  memset((void*)&ExtentStat,0x00,sizeof(TATS));
  Property ExtentProp;
  memset((void*)&ExtentProp,0x00,sizeof(Property));

  BYTE numStats=0;

memcpy((void*)&Object.nameLen,(const void*)&*(buff+count),sizeof(BYTE));
count++;

memset(Object.Name,0x00,50);
memcpy((void*)&Object.Name,(const void*)&*(buff+count),Object.nameLen);
count+=Object.nameLen;

//LOAD EVERYTHING ELSE;;;;


memset(Type,0x00,4);
while(strcmp(Type,"GNHT") && count<filelen)
	  {
       
        memcpy((void*)&strLen,(const void*)&*(buff+count),sizeof(char));
        count++;

		memset(str,0x00,255);
        memcpy((void*)&str,(const void*)&*(buff+count),strLen);
        count+=strLen;


        aniType=Parse_String(str,strLen);
		Property propString;

        if(aniType==0)
		{
          
		  memset((void*)&propString,0x00,sizeof(Property));
		  memcpy((void*)&propString.Value,(const void*)&str,255);

		  //str2=str;
		  
			/*  if(str2.Find("SUBCLASS",0)!=(-1) && Object.typeFound==false)
			  {	 }
			  else
			  {
		  if(str2.Find("CLASS",0)!=(-1) && Object.typeFound==false)
		  {
		  


		          		  if(str2.Find("MONSTERGENERATOR",0)!=(-1))
						  {Object.obType=MONSTERGEN;}

						  else if(str2.Find("MONSTER",0)!=(-1))
						  {Object.obType=MONSTER;}
						 
						  else if(str2.Find("ARMOR",0)!=(-1))
						  {Object.obType=ARMOR;}

						  else if(str2.Find("WEAPON",0)!=(-1))
						  {Object.obType=WEAPON;}
						  else if(str2.Find("WAND",0)!=(-1))
						  {Object.obType=WEAPON;}

						  else if(str2.Find("PLAYER",0)!=(-1))
						  {Object.obType=PLAYER;}
						  else if(str2.Find("MONSTERGENERATOR",0)!=(-1))
						  {Object.obType=MONSTERGEN;}
						  else if(str2.Find("FOOD",0)!=(-1))
						  {Object.obType=FOOD;}
						  else if(str2.Find("DOOR",0)!=(-1))
						  {Object.obType=DOOR;}
						  else if(str2.Find("ELEVATOR",0)!=(-1))
						  {Object.obType=ELEVATOR;}
						  else if(str2.Find("FLAG",0)!=(-1))
						  {Object.obType=FLAG;}
						  else if(str2.Find("HOLE",0)!=(-1))
						  {Object.obType=HOLE;}
						  else if(str2.Find("INFO_BOOK",0)!=(-1))
						  {Object.obType=INFOBOOK;}
						  else if(str2.Find("TREASURE",0)!=(-1))
						  {Object.obType=TREASURE;}
						  else if(str2.Find("TRIGGER",0)!=(-1))
						  {Object.obType=TRIGGER;}
						  else if(str2.Find("KEY",0)!=(-1))
						  {Object.obType=KEY;}
						  else if(str2.Find("MISSILE",0)!=(-1))
						  {Object.obType=MISSLE;}




						  else
						  {Object.obType=0;}



		  
		  Object.typeFound=true;
			}
				  }*/

		  Object.Properties.Add(propString);
          memset((void*)&propString,0x00,sizeof(Property));
		  //Object.numStrings++;
		}


//ALL FINISH POS OPERATIONS CAN HAVE *ANI = new [section size/4];
//WILL OVER BUFFER, but should be within decent results.


        switch(aniType)
		{
            case 0:break;


            case 1: //return(Object);
				count+=((8 - count % 8) % 8);
				memcpy((void*)&Length64,(const void*)&*(buff+count),sizeof(Length64));
                count+=(8);
				//count+=Length64;
                finishPos=count+Length64;
			
                
                //if(!strcmp(str,"ArmorDraw"))
			//	{Object.obType=ARMOR;}
               // else if(!strcmp(str,"WeaponDraw"))
			//	{Object.obType=WEAPON;}

				//Object.numStats=1;


				if(!strcmp(str,"SlaveDraw"))
				{
                memcpy((void*)&Length32,(const void*)&*(buff+count),sizeof(BYTE));
                count+=sizeof(BYTE);
				while((Length32--)!=0)
				{
				    Object.ImageCode=0;
					//memcpy((void*)&Object.aniList[Object.aniNum],(const void*)&*(buff+count),sizeof(int));
 				    memcpy((void*)&Object.ImageCode,(const void*)&*(buff+count),sizeof(int));
				//	Object.Stat[0].Loop.Images[Object.Stat[0].Loop.numImages]=Object.ImageCode;
				//	Object.Stat[0].Loop.numImages++;                  
					///////////////////////////////////////////////////////////
					Stat.Images.Add(Object.ImageCode);
					//Object.Stat[0].Loop.List.Add(Object.ImageCode);
					////////////////////////////////////////////////////////////
					
					count+=sizeof(int);
					//Object.aniNum++;
					}
				}
				else
				{  
					Object.ImageCode=0;
				//	memcpy((void*)&Object.aniList[Object.aniNum],(const void*)&*(buff+count),sizeof(int));
					memcpy((void*)&Object.ImageCode,(const void*)&*(buff+count),sizeof(int));
				//	Object.Stat[0].Loop.Images[Object.Stat[0].Loop.numImages]=Object.ImageCode;
				//	Object.Stat[0].Loop.numImages++;                 
					////////////////////////////////////////////////////////////
					Stat.Images.Add(Object.ImageCode);
					//Object.Stat[0].Loop.List.Add(Object.ImageCode);
					////////////////////////////////////////////////////////////
					
					count+=sizeof(int);
				//	Object.aniNum++;
				}
                count=finishPos;
				Object.Stats.Add(Stat);
				memcpy((void*)&Object.aniName,(const void*)&str,50);
				break;

            case 2:// return(Object);
                
                count+=((8 - count % 8) % 8);//SkipToNextBoundary
				memcpy((void*)&Length64,(const void*)&*(buff+count),sizeof(Length64));
                count+=8;
				finishPos = Length64 + count;
                count+=2;
      
				memcpy((void*)&Length32,(const void*)&*(buff+count),sizeof(BYTE));
                count+=sizeof(BYTE);				

				memcpy((void*)&Object.aniName,(const void*)&*(buff+count),Length32);
                count+=Length32;


				strcpy((char*)Stat.loopName,Object.aniName);
				//Object.numStats=1;
				

						while (count < finishPos)
						{
							
						if(count+4>finishPos)
						{count=finishPos;} //DO NOT READ THIS ANIMATION!
						else
						{
			            	//memcpy((void*)&Object.aniList[Object.aniNum],(const void*)&*(buff+count),sizeof(int));
 			            	//Object.aniNum++;
							Object.ImageCode=0;
							memcpy((void*)&Object.ImageCode,(const void*)&*(buff+count),sizeof(int));
					//		Object.Stat[0].Loop.Images[Object.Stat[0].Loop.numImages]=Object.ImageCode;
					//		Object.Stat[0].Loop.numImages++;
					////////////////////////////////////////////////////////////
							Stat.Images.Add(Object.ImageCode);
							//Object.Stat[0].Loop.List.Add(Object.ImageCode);
					////////////////////////////////////////////////////////////							


							count+=sizeof(int);
							
							
							//Object.Stat[0].Loop.numImages++;
							
							
							}
						}
			    count=finishPos;
	
				memcpy((void*)&Object.aniName,(const void*)&str,50);
				Object.Stats.Add(Stat);
				break;

			case 10: //PLAYERDRAW

           
			    Length64=0;
				count+=((8 - count % 8) % 8);//SkipToNextBoundary
				memcpy((void*)&Length64,(const void*)&*(buff+count),sizeof(Length64));
                count+=8;
				finishPos=count+Length64;
			//	count=finishPos;

                //Object.PlayerStat = new TATS [2000];
				//Object.SubCats = new CString [500];

                while(count<finishPos)
				{
					memset((void*)&Stat,0x00,sizeof(TATS));
					memcpy((void*)&Type,(const void*)&*(buff+count),sizeof(int));
					count+=4;
					if(!strcmp(Type," DNE"))
					{count=finishPos;}
					else
					{
                        if(!strcmp(Type,"UQES"))
						{
						memset(Object.aniName,0x00,50);
						Length32=0;
                        memcpy((void*)&Length32,(const void*)&*(buff+count),sizeof(BYTE));
                        count++;
						memcpy((void*)&Object.aniName,(const void*)&*(buff+count),Length32);
						count+=Length32;
						strcpy((char*)Stat.loopName,Object.aniName);
						///////////////Object.PlayerStats.Get(Object.numPlayerStats)->loopName=Object.aniName;
							}
						else
						{

						memset(Object.aniName,0x00,50);
						//count++;
						Length32=0;
                        memcpy((void*)&Length32,(const void*)&*(buff+count),sizeof(BYTE));
                        count++;
						memcpy((void*)&Object.aniName,(const void*)&*(buff+count),Length32);
						count+=Length32;
						strcpy((char*)Stat.nullString,Object.aniName);
						strcpy((char*)Prop.Value,Object.aniName);
						Prop.ValueLen = strlen(Object.aniName);
						Object.Subcats.Add(Prop);
						memset((void*)&Prop,0x00,sizeof(Property));
                        //////Object.PlayerStat[Object.numPlayerStats].nullString=Object.aniName;
                        //////Object.SubCats[Object.numSubCats]=Object.aniName;
						count+=2;
						 

						memset(Object.aniName,0x00,50);
						Length32=0;
                        memcpy((void*)&Length32,(const void*)&*(buff+count),sizeof(BYTE));
                        count++;
						memcpy((void*)&Object.aniName,(const void*)&*(buff+count),Length32);
						count+=Length32;
						strcpy((char*)Stat.loopName,Object.aniName);
                        //Object.PlayerStat[Object.numPlayerStats].loopName=Object.aniName;						
                        
						
						memcpy((void*)&Type,(const void*)&*(buff+count),sizeof(int));
						if(!strcmp(Type,"UQES"))
						{
						  count+=4;
						
						  memset(Object.aniName,0x00,50);
						  Length32=0;
                          memcpy((void*)&Length32,(const void*)&*(buff+count),sizeof(BYTE));
                          count++;
						  memcpy((void*)&Object.aniName,(const void*)&*(buff+count),Length32);
						  count+=Length32;
						  strcpy((char*)Stat.loopName,Object.aniName);
						  //Object.PlayerStat[Object.numPlayerStats].loopName=Object.aniName;					
						
						}
						//Object.numSubCats++;
							}


 
                        memcpy((void*)&Type,(const void*)&*(buff+count),sizeof(int));
						subcount=0;
						while(strcmp(Type," DNE") && strcmp(Type,"TATS") && strcmp(Type,"UQES"))
						{
						  //Object.Stat[Object.numStats].Loop.Images[Object.Stat[Object.numStats].Loop.numImages]=0;
                          Object.ImageCode=0;
						  memcpy((void*)&Object.ImageCode,(const void*)&*(buff+count),sizeof(int));							
						  count+=4;
						  //Object.Stat[Object.numStats].Loop.numImages++;
						  Stat.Images.Add(Object.ImageCode);
						  Stat.SubCat = Object.Subcats.numNodes;
						  //Object.PlayerStat[Object.numPlayerStats].Loop.List.Add(Object.ImageCode);
						  //Object.PlayerStat[Object.numPlayerStats].SubCat=Object.numSubCats;
						  memcpy((void*)&Type,(const void*)&*(buff+count),sizeof(int));
					
							
							}
						
						//Object.numPlayerStats++;
						Object.PlayerStats.Add(Stat);
                        memset((void*)&Stat,0x00,sizeof(TATS));
							}
					}
	

				memcpy((void*)&Object.aniName,(const void*)&str,50);
				count=finishPos;

				memcpy((void*)&Type,(const void*)&*(buff+count),sizeof(int));
				while(strcmp(Type," DNE"))
				{
					memcpy((void*)&Type,(const void*)&*(buff+count),sizeof(int));
					count++;
					}
				count+=3;
				break;



            case 3:// return(Object);
				

		//
				// THIS IS THE JACK DRAWING!!!.....BE CAREFULL!
				//
                

			    Length64=0;
				count+=((8 - count % 8) % 8);//SkipToNextBoundary
				memcpy((void*)&Length64,(const void*)&*(buff+count),sizeof(Length64));
                count+=8;
				finishPos=count+Length64;

				if(/*!strcmp(str,"PlayerDraw") && */strcmp(str,"MonsterDraw") && strcmp(str,"MaidenDraw") && strcmp(str,"AnimateDraw"))
				{
					count=finishPos;
				break;						
					}

                while(count<finishPos)
				{
					memcpy((void*)&Type,(const void*)&*(buff+count),sizeof(int));
					count+=4;
					if(!strcmp(Type," DNE"))
					{count=finishPos;}
					else
					{
						memset(Object.aniName,0x00,20);
						count++;
						Length32=0;
                        memcpy((void*)&Length32,(const void*)&*(buff+count),sizeof(BYTE));
                        count++;
						memcpy((void*)&Object.aniName,(const void*)&*(buff+count),Length32);
						count+=Length32;
						strcpy((char*)Stat.nullString,Object.aniName);
                        //Object.Stat[Object.numStats].nullString=Object.aniName;
                         count+=4;
						 

						memset(Object.aniName,0x00,20);
						Length32=0;
                        memcpy((void*)&Length32,(const void*)&*(buff+count),sizeof(BYTE));
                        count++;
						memcpy((void*)&Object.aniName,(const void*)&*(buff+count),Length32);
						count+=Length32;
						strcpy((char*)Stat.loopName,Object.aniName);
						//Object.Stat[Object.numStats].loopName=Object.aniName;						


                    
 
                        memcpy((void*)&Type,(const void*)&*(buff+count),sizeof(int));
						subcount=0;
						while(strcmp(Type," DNE") && strcmp(Type,"TATS"))
						{
						  //Object.Stat[Object.numStats].Loop.Images[Object.Stat[Object.numStats].Loop.numImages]=0;
                          Object.ImageCode=0;
						  memcpy((void*)&Object.ImageCode,(const void*)&*(buff+count),sizeof(int));							
						  count+=4;
						  //Object.Stat[Object.numStats].Loop.numImages++;
						  Stat.Images.Add(Object.ImageCode);
						  //Object.Stat[Object.numStats].Loop.List.Add(Object.ImageCode);
						  memcpy((void*)&Type,(const void*)&*(buff+count),sizeof(int));
					
							
							}
						Object.Stats.Add(Stat);
                        memset((void*)&Stat,0x00,sizeof(TATS));
						//Object.numStats++;
							}
					}
	

				memcpy((void*)&Object.aniName,(const void*)&str,50);
				count=finishPos;

				memcpy((void*)&Type,(const void*)&*(buff+count),sizeof(int));
				while(strcmp(Type," DNE"))
				{
					memcpy((void*)&Type,(const void*)&*(buff+count),sizeof(int));
					count++;
					}
				count+=3;
				break;


            case 4: //return(Object);
				count+=((8 - count % 8) % 8);//SkipToNextBoundary
				
				memcpy((void*)&Length64,(const void*)&*(buff+count),sizeof(Length64));
                count+=8;
				
				BYTE numFrames;
				memcpy((void*)&numFrames,(const void*)&*(buff+count),sizeof(BYTE));
                count+=sizeof(BYTE);
				
              //  Object.numStats=1;

				while (numFrames-- > 0)
				{//rdr.ReadInt32();
				
				   memcpy((void*)&Object.ImageCode,(const void*)&*(buff+count),sizeof(int));
				   
				  // Object.Stat[0].Loop.Images[Object.Stat[0].Loop.numImages]=Object.ImageCode;
				  // Object.Stat[0].Loop.numImages++;					
					////////////////////////////////////////////////////////////
				   Stat.Images.Add(Object.ImageCode); 
				   //Object.Stat[0].Loop.List.Add(Object.ImageCode);
					////////////////////////////////////////////////////////////
				   //memcpy((void*)&Object.aniList[Object.aniNum],(const void*)&*(buff+count),sizeof(int));
                count+=sizeof(int);
				//Object.aniNum++;
					}//count+=4;
				Object.Stats.Add(Stat);
                memset((void*)&Stat,0x00,sizeof(TATS));
				memcpy((void*)&Object.aniName,(const void*)&str,50);
				break;

            case 5: //return(Object);
				count+=((8 - count % 8) % 8);//SkipToNextBoundary
				memcpy((void*)&Length64,(const void*)&*(buff+count),sizeof(Length64));
                count+=8;
				finishPos = Length64 + count;
                count+=2;
      
				memcpy((void*)&Length32,(const void*)&*(buff+count),sizeof(BYTE));
                count+=sizeof(BYTE);				

				memcpy((void*)&Object.aniName,(const void*)&*(buff+count),Length32);
                count+=Length32;
	
				strcpy((char*)Stat.loopName,Object.aniName);
                //Object.Stat[0].loopName=Object.aniName;
                //Object.numStats=1;

						//FIXME: this may be a Loop of Loops and should probably be constructed as such
						//HACK: right now we just read until we reach the given length, tacking on the frames to the existing ones
						while (count < finishPos)
						{
							
						if(count+4>finishPos)
						{count=finishPos;} //DO NOT READ THIS ANIMATION!
						else
						{
							Object.ImageCode=0;
							memcpy((void*)&Object.ImageCode,(const void*)&*(buff+count),sizeof(int));
						//    Object.Stat[0].Loop.Images[Object.Stat[0].Loop.numImages]=Object.ImageCode;
							
						//	Object.Stat[0].Loop.numImages++;
					  ////////////////////////////////////////////////////////////
							Stat.Images.Add(Object.ImageCode);
							//Object.Stat[0].Loop.List.Add(Object.ImageCode);
					  ////////////////////////////////////////////////////////////
							//	memcpy((void*)&Object.aniList[Object.aniNum],(const void*)&*(buff+count),sizeof(int));
                            count+=sizeof(int);
			             //	Object.aniNum++;							
							
							}
						}
			    count=finishPos;
				Object.Stats.Add(Stat);
                memset((void*)&Stat,0x00,sizeof(TATS));
				memcpy((void*)&Object.aniName,(const void*)&str,50);
				break;

            case 6: //return(Object);
				count+=((8 - count % 8) % 8);//SkipToNextBoundary
				Length64=0;
				memcpy((void*)&Length64,(const void*)&*(buff+count),sizeof(Length64));
                count+=8;
				finishPos = Length64 + count;

				//BYTE numAni;
			    numStats=0;
				memcpy((void*)&numStats,(const void*)&*(buff+count),sizeof(BYTE));
                count+=sizeof(BYTE);


				for(j=0; j<numStats; j++)
				{
					memset((void*)&Stat,0x00,sizeof(TATS));
					Length64=0;
					Length32=0;
				    memcpy((void*)&Length64,(const void*)&*(buff+count),sizeof(BYTE));//Num Images
                    count+=sizeof(BYTE);
				    count++;
				    
					memcpy((void*)&Length32,(const void*)&*(buff+count),sizeof(BYTE));
                    count+=sizeof(BYTE);

					memset(Object.aniName,0x00,70);
                    memcpy((void*)&Object.aniName,(const void*)&*(buff+count),Length32);
					strcpy((char*)Stat.loopName,Object.aniName);
					//Object.Stat[j].loopName=Object.aniName;
					count+=Length32;

//                    Object.Stat[j].Loop.numImages=0;
					for(k=0; k<Length64; k++)
					{
                      Object.ImageCode=0;
					  memcpy((void*)&Object.ImageCode,(const void*)&*(buff+count),sizeof(int));
                      count+=sizeof(int);
					 // Object.Stat[j].Loop.numImages++;					  
					////////////////////////////////////////////////////////////
                    //Object.Stat[j].Loop.Images[k]=Object.ImageCode;
					  Stat.Images.Add(Object.ImageCode);
					  //Object.Stat[j].Loop.List.Add(Object.ImageCode);
					////////////////////////////////////////////////////////////
			
					}
					Object.Stats.Add(Stat);
                    memset((void*)&Stat,0x00,sizeof(TATS));
				}

						
						count=finishPos;
					
				 memcpy((void*)&Object.aniName,(const void*)&str,50);
				 break;

            case 7: //return(Object);
				memcpy((void*)&Object.MenuIcon,(const void*)&*(buff+count),sizeof(int));
                count+=sizeof(int);				
				break;

            case 8: //return(Object);
				memcpy((void*)&Object.PrettyImage,(const void*)&*(buff+count),sizeof(int));
                count+=sizeof(int);				
				
				break;

            case 9: //return(Object);

				count+=((8 - count % 8) % 8);//SkipToNextBoundary
				count+=8;
				break;
            
			default :break;
		}

	   if(*(buff+count)==0x00)
				{
				count++;
				}
	   memcpy((void*)&*(Type),(const void*)&*(buff+count),sizeof(int));
       }

count+=4;

//////////////////////////////end remove
 memset((void*)&Stat,0x00,sizeof(TATS));
 memset((void*)&Prop,0x00,sizeof(Property));
return(true);
}




//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////



bool ThingBin::Load_Audio(long &count,unsigned char *buff)
{

while(strcmp(Type,"GNHT"))
{

memcpy((void*)&*(Type),(const void*)&*(buff+count)/*[i]*/,sizeof(int));
count++;
}
count--;//count-=4;

return(0);
}


//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////


bool ThingBin::Load_Wall(long &count,unsigned char *buff, LLAW & Wall)
{
	long numObjects = 0;
	int Len= 0;
    Property Object;
	memset((void*)&Object,0x00,sizeof(Property));

//LLAW Wall;

Wall.Unknown = (int)*(buff+count);
//memcpy((void*)&Wall->Unknown,(const void*)&*(buff+count),sizeof(int));
count+=4;

Wall.nameLen = (BYTE)*(buff+count);
//memcpy((void*)&Wall->nameLen,(const void*)&*(buff+count),sizeof(BYTE));
count++;

memset(Wall.Name,0x00,50);
memcpy((void*)&Wall.Name,(const void*)&*(buff+count),Wall.nameLen);
count+=Wall.nameLen;


//out.Write(&Wall.Name,Wall.nameLen);
//out.Write("\n",1);


count+=14;
count+=((8 - count % 8) % 8);

//while(tem==0)
//{
numObjects = (long)*(buff+count);
//memcpy((void*)&numObjects,(const void*)&*(buff+count),sizeof(long));
count+=sizeof(long);
//}
count+=((8 - count % 8) % 8);


for(int i=0; i<numObjects; i++)
{

Object.ValueLen = NULL;
Object.ValueLen = (BYTE)*(buff+count);
//memcpy((void*)&Object.ValueLen,(const void*)&*(buff+count),sizeof(BYTE));
count++;

memset(Object.Value,0x00,50);
memcpy((void*)&Object.Value,(const void*)&*(buff+count),Object.ValueLen);
count+=Object.ValueLen;
Wall.Objects.Add(Object);
memset((void*)&Object,0x00,sizeof(Property));
}

Wall.Sound_Open_Len=0;
memset(Wall.SoundOpen,0x00,50);
Wall.Sound_Open_Len = (BYTE)*(buff+count);
//memcpy((void*)&Wall->Sound_Open_Len,(const void*)&*(buff+count),sizeof(BYTE));
count++;
memcpy((void*)&Wall.SoundOpen,(const void*)&*(buff+count),Wall.Sound_Open_Len);
count+=Wall.Sound_Open_Len;

memset(Wall.SoundClose,0x00,50);
Wall.Sound_Close_Len=0;
Wall.Sound_Close_Len = (BYTE)*(buff+count);
//memcpy((void*)&Wall->Sound_Close_Len,(const void*)&*(buff+count),sizeof(BYTE));
count++;
memcpy((void*)&Wall.SoundClose,(const void*)&*(buff+count),Wall.Sound_Close_Len);
count+=Wall.Sound_Close_Len;

memset(Wall.SoundDestroy,0x00,50);
Wall.Sound_Destroy_Len = (BYTE)*(buff+count);
//memcpy((void*)&Wall->Sound_Destroy_Len,(const void*)&*(buff+count),sizeof(BYTE));
count++;
memcpy((void*)&Wall.SoundDestroy,(const void*)&*(buff+count),Wall.Sound_Destroy_Len);
count+=Wall.Sound_Destroy_Len;


//MOVE TO IMAGE BOUNDRY!!!!
count+=((8 - count % 8) % 8);
while((int)*(buff+count)!=0x00)
{
count++;
count+=((8 - count % 8) % 8);
}
count++;
count+=((8 - count % 8) % 8);


short tem;//=0;

while(strcmp(Type," DNE"))
{
//tem=0;
	Wall.ImageCode=0;
Wall.ImageCode = (int)*(buff+count);
	//memcpy((void*)&Wall->ImageCode,(const void*)&*(buff+count),sizeof(int));
count+=4;

Wall.Images.Add(Wall.ImageCode);

memcpy((void*)&*(Type),(const void*)&*(buff+count),sizeof(int));
if(strcmp(Type," DNE"))
{
memcpy((void*)&tem,(const void*)&*(buff+count),sizeof(short));
count+=2;


if(tem>0)
{count+=14;}
else
{count+=6;}
}


memcpy((void*)&*(Type),(const void*)&*(buff+count),sizeof(int));
//count++;
}
count+=4;
return(true);
}



//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////


//LOAD THE EDGES AND TILES!!!////////////////


bool ThingBin::Load_Tile(long &count,unsigned char *buff, ROLF & Tile)
{

unsigned int ImageCode = 0;


memcpy((void*)&Tile.Unknown,(const void*)&*(buff+count),sizeof(int));
count+=4;

memcpy((void*)&Tile.nameLen,(const void*)&*(buff+count),sizeof(BYTE));
count++;

memset(Tile.Name,0x00,50);
memcpy((void*)&Tile.Name,(const void*)&*(buff+count),Tile.nameLen);
count+=Tile.nameLen;



memcpy((void*)&Tile.Unknown2,(const void*)&*(buff+count),sizeof(short));
count+=2;

memcpy((void*)&*(Tile.Unknowns),(const void*)&*(buff+count),12);
count+=12;


if(!strcmp(Type,"ROLF"))
{
	memcpy((void*)&Tile.one,(const void*)&*(buff+count),2);
    count+=2;
}


while(true)
{
memcpy((void*)&*(Type),(const void*)&*(buff+count)/*[i]*/,sizeof(int));
if(!strcmp(Type," DNE"))
{count+=4;break;}
else
{
     ImageCode=0;
     memcpy((void*)&ImageCode,(const void*)&*(buff+count),sizeof(int));
	 Tile.Images.Add(ImageCode);
count+=4;
}

	}

return(true);
}
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
bool ThingBin::Load_Image(long &count,unsigned char *buff, GAMI & Image)
{

  int numLoop=0,temp=0;
  unsigned int ImageCode = 0;
  BYTE numImages = 0;




memcpy((void*)&Image.nameLen,(const void*)&*(buff+count),sizeof(BYTE));
count++;

memset(Image.Name,0x00,50);
memcpy((void*)&Image.Name,(const void*)&*(buff+count),Image.nameLen);
count+=Image.nameLen;

memcpy((void*)&numImages,(const void*)&*(buff+count),sizeof(BYTE));
count++;

if(numImages == 1)
{	
memcpy((void*)&ImageCode,(const void*)&*(buff+count),sizeof(int));
count+=4;
Image.Images.Add((unsigned int &)ImageCode);
strcpy((char*)Image.aniName,"Static Image");
	}
else
{
memcpy((void*)&numLoop,(const void*)&*(buff+count),sizeof(BYTE));
count+=2;

memcpy((void*)&temp,(const void*)&*(buff+count),sizeof(BYTE));
count++;

memset(Image.aniName,0x00,50);
memcpy((void*)&Image.aniName,(const void*)&*(buff+count),temp);
count+=temp;

for(int i=0; i<numLoop; i++)
{
memcpy((void*)&ImageCode,(const void*)&*(buff+count),sizeof(int));
count+=4;	
Image.Images.Add(ImageCode);
//Image.numImages++;	
}


}

return(true);
}