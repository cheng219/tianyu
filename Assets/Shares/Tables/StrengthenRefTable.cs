//=========================
//作者：黄洪兴
//日期：2016/03/29
//用途：强化静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StrengthenRefTable : AssetTable
{
	public List<StrengthenRef> infoList = new List<StrengthenRef>();
}


[System.Serializable]
public class StrengthenRef
{
	public	int id;
	/// <summary>
	/// 装备的当前强化等级
	/// </summary>
	public int lev;
	/// <summary>
	/// 强化一次消耗的物品
	/// </summary>
	public List<ItemValue> items;

    public int coin;
	/// <summary>
	/// 完美强化一次消耗的物品
	/// </summary>
	public List<ItemValue> perfectItems;
	/// <summary>
	/// 分解时获得的物品
	/// </summary>
	public List<ItemValue> decoItems;

}