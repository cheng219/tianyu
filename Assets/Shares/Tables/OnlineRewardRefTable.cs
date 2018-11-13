//=========================
//作者：黄洪兴
//日期：2016/05/4
//用途：在线奖励静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OnlineRewardRefTable : AssetTable
{
	public List<OnlineRewardRef> infoList = new List<OnlineRewardRef>();
}


[System.Serializable]
public class OnlineRewardRef
{
	public	int id;
	public int time;
	public List<ItemValue> item=new List<ItemValue>();


}


