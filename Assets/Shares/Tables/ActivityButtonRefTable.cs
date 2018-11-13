//=========================
//作者：黄洪兴
//日期：2016/04/25
//用途：活动大厅静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivityButtonRefTable : AssetTable
{
	public List<ActivityButtonRef> infoList = new List<ActivityButtonRef>();
}


[System.Serializable]
public class ActivityButtonRef
{
	public	int id;
	public int type;
	public string name;
	public int mapId;
	public List<int> mapXY=new List<int>();

	public string pageId;
	public List<string> time=new List<string>();



}


