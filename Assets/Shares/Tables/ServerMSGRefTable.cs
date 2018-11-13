//=============================
//作者： 龙英杰
//日期：2015/10/23
//用途：客服端提示静态配置数据（错误提示）
//=============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerMSGRefTable : AssetTable {

	public List<ServerMSGRef> infoList = new List<ServerMSGRef>();
}

[System.Serializable]
public class ServerMSGRef
{
    public int id;
    public MsgRefData refData;
}

[System.Serializable]
public class MsgRefData 
{
	public string title;
	public string messStr;
	public int sort;
	public Vector3 flowStartV3 = Vector3.zero;//起始的坐标
	public Vector3 flowEndV3 = Vector3.zero;//终点的坐标
	public Vector3 flowLocaV3 = Vector3.zero;//终点的坐标
	public int size = 25;//文字大小
	public bool showBg = true;//背景是否显示
	
	public float stopTime;//提示停留时间
	public float moveTime;//浮动时间
	
	public float holdTime;//同类型提示间隔时间
	
	public float acceleration;//渐变加速度
	
	public List<int> showType = new List<int>();//显示类型
	public string font;//字体

	public ItemValue item;
    public GUIType ButtonSort;
    public string tips;
}