//=========================
//作者：黄洪兴
//日期：2016/04/1
//用途：铸魂次数静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CastSoulTimeRefTable : AssetTable
{
	public List<CastSoulTimeRef> infoList = new List<CastSoulTimeRef>();
}


[System.Serializable]
public class CastSoulTimeRef
{
	/// <summary>
	/// 次数
	/// </summary>
	public	int time;
	/// <summary>
	/// 普通铸魂消耗的物品
	/// </summary>
	public List<ItemValue> giftMoney=new List<ItemValue> ();
	/// <summary>
	/// 高级铸魂消耗的物品
	/// </summary>
	public List<ItemValue> specialMoney=new List<ItemValue> ();
	/// <summary>
	/// 暴击比率 万分比
	/// </summary>
	public int chance;

}