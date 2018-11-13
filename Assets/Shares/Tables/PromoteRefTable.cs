//=========================
//作者：黄洪兴
//日期：2016/04/6
//用途：升阶静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PromoteRefTable : AssetTable
{
	public List<PromoteRef> infoList = new List<PromoteRef>();
}


[System.Serializable]
public class PromoteRef
{
	/// <summary>
	/// ID
	/// </summary>
	public	int id;
	/// <summary>
	/// 当前装备ID
	/// </summary>
	public	int start_item;
	/// <summary>
	/// 装备对应的职业
	/// </summary>
	public	int prof;
	/// <summary>
	/// 升阶消耗的物品
	/// </summary>
	public List<ItemValue> materialItem;
	/// <summary>
	/// 升阶后获得的物品ID
	/// </summary>
	public int end_item;

    public long coin;




}