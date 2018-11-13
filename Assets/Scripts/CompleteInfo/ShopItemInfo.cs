//====================================================
//作者: 黄洪兴
//日期：2016/3/28
//用途：商店商品的数据层对象
//======================================================




using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ShopItemServerData
{
	public int id;

}

/// <summary>
/// 商店商品数据层对象 
/// </summary>
public class ShopItemInfo 
{
	#region 服务端数据 
	ShopItemServerData shopItemData;
	#endregion

	#region 静态配置数据 
	ShopRef shopItemRef = null;
	public ShopRef ShopItemRef
	{
		get
		{
			if (shopItemRef != null) return shopItemRef;
			shopItemRef = ConfigMng.Instance.GetShopRef(shopItemData.id);
			return shopItemRef;
		}
	}
	#region 构造 
	public ShopItemInfo(int _id)
	{
		shopItemData = new ShopItemServerData ();
		shopItemData.id = _id;

	}
	public ShopItemInfo(int _sid, int eq,int _nums)
	{
		soleID = _sid;
		redeemID = eq;
		nums = _nums;


	}
	public ShopItemInfo(st.net.NetBase.normal_skill_list _info)
	{

	}
	#endregion

	#region 访问器
	/// <summary>
	/// 商品ID
	/// </summary>
	public int ID
	{	
		get { return shopItemData.id; }
	}
	/// <summary>
	/// 商品对应的物品
	/// </summary>
	public EquipmentInfo Item
	{
		get { 
			if (soleID == 0) {
				EquipmentInfo eq = new EquipmentInfo (ShopItemRef.item, EquipmentBelongTo.SHOPWND);
				return eq;
			} else {
				EquipmentInfo eq = new EquipmentInfo (redeemID,nums, EquipmentBelongTo.REDEEM);
				return eq;
			}
		}
	}
	/// <summary>
	/// 购买方式
	/// </summary>
	public EquipmentInfo BuyWay
	{
		get
		{
			EquipmentInfo eq = new EquipmentInfo ( ShopItemRef.buyWay,EquipmentBelongTo.PREVIEW);
			return eq;
		}
	}

    /// <summary>
    /// 价格图标
    /// </summary>
    public string BuyIcon
    {
        get
        {
            return ShopItemRef.buyIcon;
        }
    }
	/// <summary>
	/// 价格
	/// </summary>
	public int Price
	{
		get
		{
			return ShopItemRef.price;
		}
	}
	/// <summary>
	/// 商品种类
	/// </summary>
	public int Type
	{
		get {
			return ShopItemRef.type;
		}

	}
    public int Prof
    {
        get
        {
            return ShopItemRef.occupation;
        }
    }
	/// <summary>
	/// 唯一ID
	/// </summary>
	public int soleID=0;
    /// <summary>
    /// 物品ID 仅用于购回物品
    /// </summary>
	public int redeemID=0;
	/// <summary>
	/// 物品数量 仅用于物品回购
	/// </summary>
	public int nums=0;



	#endregion
	#endregion
}
