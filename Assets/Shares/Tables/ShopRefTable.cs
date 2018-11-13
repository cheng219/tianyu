//=========================
//作者：黄洪兴
//日期：2016/03/28
//用途：商店静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopRefTable : AssetTable
{
	public List<ShopRef> infoList = new List<ShopRef>();
}


[System.Serializable]
public class ShopRef
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
	/// 购买方式
	/// </summary>
	public int buyWay;
	/// <summary>
	/// 价格
	/// </summary>
	public int price;
	/// <summary>
	/// 商品种类
	/// </summary>
	public int type;
    public int occupation;
    public string buyIcon;


}