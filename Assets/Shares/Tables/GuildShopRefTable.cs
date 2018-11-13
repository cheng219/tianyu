//=========================
//作者：黄洪兴
//日期：2016/04/14
//用途：仙盟商店静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuildShopRefTable : AssetTable
{
	public List<GuildShopRef> infoList = new List<GuildShopRef>();
}


[System.Serializable]
public class GuildShopRef
{
	/// <summary>
	/// 商品ID
	/// </summary>
	public	int id;
	/// <summary>
	/// 商品对应物品
	/// </summary>
	public int item;
	/// <summary>
	/// 该组物品的数量
	/// </summary>
	public int num;
	/// <summary>
	/// 价格
	/// </summary>
	public int price;
	/// <summary>
	/// 限购数量  不限填0
	/// </summary>
	public int amount;


}