//====================================================
//作者: 黄洪兴
//日期：2016/3/28
//用途：仙盟商店商品的数据层对象
//======================================================




using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GuildShopItemServerData
{
	public int id;

}

/// <summary>
/// 商店商品数据层对象 
/// </summary>
public class GuildShopItemInfo 
{
	#region 服务端数据 
	GuildShopItemServerData guildShopItemData;
	#endregion

	#region 静态配置数据 
	GuildShopRef guildShopItemRef = null;
	public GuildShopRef GuildShopItemRef
	{
		get
		{
			if (guildShopItemRef != null) return guildShopItemRef;
			guildShopItemRef = ConfigMng.Instance.GetGuildShopRef(guildShopItemData.id);
			return guildShopItemRef;
		}
	}
	#region 构造 
	public GuildShopItemInfo(int _id)
	{
		guildShopItemData = new GuildShopItemServerData ();
		guildShopItemData.id = _id;

	}
	public GuildShopItemInfo(int _sid, int eq,int _nums)
	{


	}
	public GuildShopItemInfo(st.net.NetBase.normal_skill_list _info)
	{

	}
	#endregion

	#region 访问器
	/// <summary>
	/// 商品ID
	/// </summary>
	public int ID
	{	
		get { return guildShopItemData.id; }
	}
	/// <summary>
	/// 商品对应的物品
	/// </summary>
	public EquipmentInfo Item
	{
		get { 
				EquipmentInfo eq = new EquipmentInfo (GuildShopItemRef.item,GuildShopItemRef.amount, EquipmentBelongTo.GUILDSHOP);
				return eq;
		}
	}
	/// <summary>
	/// 价格
	/// </summary>
	public int Price
	{
		get
		{
			return GuildShopItemRef.price;
		}
	}
	/// <summary>
	/// 限购数量
	/// </summary>
	/// <value>The amount.</value>
	public int Amount
	{
		get{
			return GuildShopItemRef.num;
		}
	}
	public int BuyedNum=0;	



	#endregion
	#endregion
}
