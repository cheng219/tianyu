//====================================================
//作者: 黄洪兴
//日期：2016/4/20
//用途：市场拍卖物品数据层对象
//======================================================




using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MarketItemServerData
{
    /// <summary>
    ///服务端唯一ID
    /// </summary>
	public int eid;
    /// <summary>
    /// 单价
    /// </summary>
    public int price;
    /// <summary>
    /// 货币种类
    /// </summary>
    public int priceType;
    /// <summary>
    /// 剩余时间
    /// </summary>
    public int remainTime;

}

/// <summary>
/// 市场拍卖物品数据层对象 
/// </summary>
public class MarketItemInfo 
{
	#region 服务端数据 
	MarketItemServerData marketItemData;
	#endregion

	#region 静态配置数据 
	MarketRef marketTypeRef = null;
	public MarketRef MarketTypeRef
	{
		get
		{
			return marketTypeRef;
		}
	}
	#region 构造 
    public MarketItemInfo(st.net.NetBase.shelve_item_info _info)
	{
        marketItemData = new MarketItemServerData();
        if ((int)_info.item_info.Count > 0)
        {
            marketItemData.eid = (int)_info.id;
            equipmentInfo = new EquipmentInfo((int)_info.item_info[0].type, (int)_info.item_info[0].id, (int)_info.item_info[0].num, EquipmentBelongTo.PREVIEW);
        }
        else
        {
            marketItemData.eid =(int) _info.id;
            equipmentInfo = new EquipmentInfo((int)_info.type, (int)_info.id, (int)_info.num, EquipmentBelongTo.PREVIEW);

        }

        marketItemData.price = (int)_info.price;
        marketItemData.remainTime =(int) _info.rest_time;
        marketItemData.priceType = _info.currency;



	}
    public MarketItemInfo()
    {
    }
	#endregion

	#region 访问器
	/// <summary>
	/// 服务端唯一ID 用于购买拍卖物品
	/// </summary>
	public int EID
	{	
		get{
            return marketItemData.eid;
		}
	}



    EquipmentInfo equipmentInfo;
	/// <summary>
	/// 拍卖物品对应的物品信息
	/// </summary>
	/// <value>The equipment info.</value>
	public EquipmentInfo EquipmentInfo
	{
		get{
            return equipmentInfo;
		}
	}
	/// <summary>
	/// 物品数量
	/// </summary>
	/// <value>The nums.</value>
	public int Nums
	{
		get{
            return EquipmentInfo.StackCurCount;
		}
	}

	/// <summary>
	/// 单价
	/// </summary>
	/// <value>The price.</value>
	public int Price
	{
		get{

			return marketItemData.price;
		}
	}

	/// <summary>
	/// 货币类型
	/// </summary>
	/// <value>The price.</value>
	public int PriceType
	{
		get{

			return marketItemData.priceType;
		}
	}

    /// <summary>
    /// 剩余时间
    /// </summary>
    public int RemainTime
    {

        get
        {
            return marketItemData.remainTime;
        }
    }


	#endregion
	#endregion
}
