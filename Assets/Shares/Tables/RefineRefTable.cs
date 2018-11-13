//=========================
//作者：黄洪兴
//日期：2016/3/4
//用途：淬炼静态配置表
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefineRefTable : AssetTable
{
	public List<RefineRef> infoList = new List<RefineRef>();
}

[System.Serializable]
public class RefineRef
{
	public int id;
	public int relationID;
	public int stage;
	public int star;
	public string labelName;
	public List<int> attributeId=new List<int>();
	public List<int> attributeNum=new List<int>();
	public int model;
	public List<int> consume=new List<int>();
	public List<int> consumeNum=new List<int>();
	public  int fighting;
	public List<int> randomExp=new List<int>();


}
