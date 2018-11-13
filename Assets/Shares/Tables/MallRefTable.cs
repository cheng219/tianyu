//=========================
//作者：黄洪兴
//日期：2016/03/28
//用途：商城静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MallRefTable : AssetTable
{
	public List<MallRef> infoList = new List<MallRef>();
}


[System.Serializable]
public class MallRef
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
	/// 每日限购数量
	/// </summary>
	public int amount;
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
	/// <summary>
	/// VIP等级限制
	/// </summary>
	public int vip_level;
	/// <summary>
	/// 特殊显示
	/// </summary>
	public List<string> tab=new List<string>();
	/// <summary>
	/// 原价
	/// </summary>
	public string originalPrice;
    public int occupation;

    /// <summary>
    /// 现价
    /// </summary>
    public string nowPrice;


}