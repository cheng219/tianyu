//====================================================
//作者: 黄洪兴
//日期：2016/4/5
//用途：商城商品的数据层对象
//======================================================




using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MallItemServerData
{
	public int id;

}

/// <summary>
///商城商品数据层对象 
/// </summary>
public class MallItemInfo 
{
	#region 服务端数据 
	MallItemServerData mallItemData;
	#endregion

	#region 静态配置数据 
	MallRef mallItemRef = null;
	public MallRef MallItemRef
	{
		get
		{
			if (mallItemRef != null) return mallItemRef;
			mallItemRef = ConfigMng.Instance.GetMallRef(mallItemData.id);
			return mallItemRef;
		}
	}
	#region 构造 
	public MallItemInfo(int _id)
	{
		mallItemData = new MallItemServerData ();
		mallItemData.id = _id;
	}
	public MallItemInfo(int _id, int _lv,int _curRune)
	{

	}
	public MallItemInfo(st.net.NetBase.normal_skill_list _info)
	{

	}
	#endregion

	#region 访问器
	/// <summary>
	/// 商品ID
	/// </summary>
	public int ID
	{	
		get { return mallItemData.id; }
	}
	/// <summary>
	/// 商品对应的物品
	/// </summary>
	public EquipmentInfo Item
	{
		get { 
			EquipmentInfo eq = new EquipmentInfo ( MallItemRef.item,EquipmentBelongTo.PREVIEW);
			return eq; 
		}
	}
	/// <summary>
	/// 购买方式
	/// </summary>
	public EquipmentInfo BuyWay
	{
		get
		{
			EquipmentInfo eq = new EquipmentInfo ( MallItemRef.buyWay,EquipmentBelongTo.PREVIEW);
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
			return MallItemRef.price;
		}
	}
	/// <summary>
	/// 商品种类
	/// </summary>
	public int Type
	{
		get {
			return MallItemRef.type;
		}

	}
	/// <summary>
	/// 每日限购数量
	/// </summary>
	public int Amount
	{
		get {
			return MallItemRef.amount;
		}

	}
	/// <summary>
	/// VIP等级限制
	/// </summary>
	public int Vip_level
	{
		get {
			return MallItemRef.vip_level;
		}

	}
	/// <summary>
	/// 原价
	/// </summary>
	public string OriginalPrice
	{
		get {
			return MallItemRef.originalPrice;
		}

	}
    /// <summary>
    /// 现价
    /// </summary>
    public string NowPrice
    {
        get
        {
            return MallItemRef.nowPrice;
        }

    }
	/// <summary>
	/// 特殊显示
	/// </summary>
	public List<string> Tab
	{
		get {
			return MallItemRef.tab;
		}

	}
    public int Prof
    {
        get
        {
            return MallItemRef.occupation;
        }
    }
	public int buyedNum=0;




	#endregion
	#endregion
}
