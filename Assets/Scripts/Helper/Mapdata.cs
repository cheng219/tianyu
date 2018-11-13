using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;

public class Mapdata
{
	
   
    public static int map_x;
    public static int map_y;
	
	
	
	
	//储存在Dictionary的方式
	public static Dictionary<String, bool> S_mapdata = new Dictionary<string, bool>();
	
    public static void getmap()
    {
      
        switch(Application.loadedLevelName)
        {
            case "fuben00":
              GetMapData1("MapData/fuben00");
              //Debug.Log("地图处理fuben00");
                break;
		    case "fuben01":
              GetMapData1("MapData/fuben01");
              //Debug.Log("地图处理fuben01");
                break;
			 case "fuben02":
              GetMapData1("MapData/fuben02");
              //Debug.Log("地图处理fuben02");
                break;
            case "zhucheng00":
              GetMapData1("MapData/zhucheng00");
              //Debug.Log("地图处理zhucheng00");
               break;
        }


    }
	
    public static void GetMapData1(String mappath)
    {
        S_mapdata.Clear();
        TextAsset mapdata = Resources.Load(mappath, typeof(TextAsset)) as TextAsset;

        byte[] bytes = mapdata.bytes;

        string _mapdata = Encoding.UTF8.GetString(bytes);//UTF-16

        _mapdata = _mapdata.ToLower();
        //string[] settings = _mapdata.Split('\n');
        string[] settings = _mapdata.Split(char.Parse("\n"));

        string Smap_x = settings[0];
        string Smap_y = settings[1];
        Smap_x = Smap_x.Replace("max_x()->","");
        Smap_x = Smap_x.Replace(".", "");

        Smap_y = Smap_y.Replace("max_y()->", "");
        Smap_y = Smap_y.Replace(".", "");
        map_x = int.Parse(Smap_x);
        map_y = int.Parse(Smap_y);



        for (int i = 2; i < settings.Length; )
        {
            String dataline = settings[i++];

            dataline = dataline.Trim();
            dataline = dataline.Replace("get(", "");
            dataline = dataline.Replace(")", "");
            dataline = dataline.Replace("->0;", "");
            S_mapdata.Add(dataline.ToString(),false);
        }

        for (int i = 0; i < map_x; i++)
        {
            for (int j = 0; j < map_y; j++)
            {
                string s = i + "," + j;
                if (!S_mapdata.ContainsKey(s))
                    S_mapdata.Add(s, true);
            }
        }



    }
	
	//储存在二维数组的方式
	public static bool[,] thismap;
	
  	public static void ReadMapTxt()
	{		  			
	     ReadMapString("MapData/"+Application.loadedLevelName);
	}
	
    public static void ReadMapString(String mappath)
    {
        S_mapdata.Clear();
        TextAsset mapdata = Resources.Load(mappath, typeof(TextAsset)) as TextAsset;

        byte[] bytes = mapdata.bytes;

        string _mapdata = Encoding.UTF8.GetString(bytes);//UTF-16

        _mapdata = _mapdata.ToLower();
        //string[] settings = _mapdata.Split('\n');
        string[] settings = _mapdata.Split(char.Parse("\n"));

        string Smap_x = settings[0];
        string Smap_y = settings[1];
        Smap_x = Smap_x.Replace("max_x()->","");
        Smap_x = Smap_x.Replace(".", "");

        Smap_y = Smap_y.Replace("max_y()->", "");
        Smap_y = Smap_y.Replace(".", "");
        map_x = int.Parse(Smap_x);
        map_y = int.Parse(Smap_y);

		thismap=new bool[map_x,map_y];
		
		
		for (int i = 0; i < map_x; i++)
        {
            for (int j = 0; j < map_y; j++)
            {
               thismap[i,j]=true;
            }
        }
		
        for (int i = 2; i < settings.Length; )
        {
            String dataline = settings[i++];

            dataline = dataline.Trim();
            dataline = dataline.Replace("get(", "");
            dataline = dataline.Replace(")", "");
            dataline = dataline.Replace("->0;", "");
			string[] xy = dataline.Split(',');
			thismap[int.Parse(xy[0]),int.Parse(xy[1])]=false;          
        }

    }
	
}
