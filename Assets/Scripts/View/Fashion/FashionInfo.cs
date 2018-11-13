//====================================================
//作者: 黄洪兴
//日期：2016/3/18
//用途：时装数据层对象
//======================================================




using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FashionServerData
{
	//时装ID
	public int id;
	//是否拥有
	public int isOwn;
	//是否穿戴
	public int isPut;
	//剩余时间
    public RemainTime remainTime;
	public List<int> rune_list;
	public int rune_use;
}

/// <summary>
/// 技能单位数据层对象 by 贺丰
/// </summary>
public class FashionInfo 
{
	#region 服务端数据 
	FashionServerData fashionData;
	#endregion

	#region 静态配置数据 
	FashionRef fashionRef = null;
	public FashionRef FashionRef
	{
		get
		{
			if (fashionRef != null) return fashionRef;
			fashionRef = ConfigMng.Instance.GetFashionRef(fashionData.id);
			return fashionRef;
		}
	}
	#region 构造 
	public FashionInfo(int _id)
	{
		fashionData = new FashionServerData();
		fashionData .id = _id;
		fashionData.isOwn  = 0;
		fashionData.isPut = 0;
		fashionData.remainTime = null;
	}




	public FashionInfo(int _id, int _own,int _put,RemainTime _time)
	{
		fashionData = new FashionServerData();
		fashionData .id = _id;
		fashionData.isOwn  = _own;
		fashionData.isPut = _put;
		fashionData.remainTime = _time;
		// skillData.rune_use = _curRune;
	}
	#endregion

	#region 访问器

    public void SetOwn(bool b)
    {
        fashionData.isOwn = b ? 1 : 0;
    }


    public RemainTime RemainTime
    {
        get
        {
            return fashionData.remainTime;
        }
    }


	/// <summary>
	/// 服务端发来的时装ID
	/// </summary>
	public int FashionID
	{
		get { return fashionData.id; }
	}
	/// <summary>
	/// 是否拥有
	/// </summary>
	public bool IsOwn
	{
		get { return fashionData.isOwn==1; }
	}
	/// <summary>
	/// 是否穿戴
	/// </summary>
	public bool IsPut
	{
		get
		{
			return fashionData.isPut==1;
		}
	}


	/// <summary>
	/// 时装类型
	/// </summary>
	public int FashionType
	{
		get
		{
			return FashionRef.type;
		}
	}
	/// <summary>
	/// 时装名字
	/// </summary>
	public string FashionName
	{
		get
		{
			return FashionRef.name;
		}
	}





	/// <summary>
	/// 时装描述
	/// </summary>
	public string FashionDes
	{
		get
		{
			return FashionRef.des;
		}
	}
	/// <summary>
	/// 时装对应的物品ID
	/// </summary>
	public int Item
	{
		get
		{
			return FashionRef.item;
		}
	}
	/// <summary>
	/// 模型资源
	/// </summary>
	public string AssetName
	{
		get
		{
			return FashionRef.assetName;
		}
	}
	/// <summary>
	/// 附加属性
	/// </summary>
    public List<ItemValue> Attribute
	{
		get
		{
			return FashionRef.attribute;
		}
	}
	/// <summary>
	/// 升级所需物品
	/// </summary>
    public List<ItemValue> UpdataItem
	{
		get
		{
			return FashionRef.updataItem;
		}
	}
	/// <summary>
	/// 战力
	/// </summary>
	public int Gs
	{
		get
		{
			return FashionRef.gs;
		}
	}
	/// <summary>
	/// 时装获得途径 
	/// </summary>
	public int Type
	{
		get
		{
			return FashionRef.getType;
		}
	}
	/// <summary>
	/// 限时时间
	/// </summary>
	public int Time
	{
		get
		{
			return FashionRef.time;
		}
	}

    /// <summary>
    /// 对应商品ID
    /// </summary>
    public int ShopID
    {
        get
        {
            return FashionRef.shopId;
        }
    }

    /// <summary>
    /// 对应职业
    /// </summary>
    public int Prof
    {
        get
        {
            return FashionRef.prof;
        }
    }


    /// <summary>
    /// 对应的永久时装ID
    /// </summary>
    public int TempID
    {
        get
        {
            return FashionRef.tempId;
        }
    }

    /// <summary>
    /// 对应的临时时装ID
    /// </summary>
    public int ForeverTempID
    {
        get
        {
            return FashionRef.forever_temp_id;
        }
    }

    /// <summary>
    /// 升级为永久需要消耗的物品数量
    /// </summary>
    public int UseItemNum
    {
        get
        {
            return FashionRef.useItemNum;
        }
    }

    EquipmentInfo itemInfo=null;

    /// <summary>
    /// 时装对应的物品信息
    /// </summary>
    public EquipmentInfo ItemInfo
    {
        get
        {
            if (itemInfo != null)
                return itemInfo;
            itemInfo = new EquipmentInfo(Item,EquipmentBelongTo.PREVIEW);
            return itemInfo;
        }
    }

    /// <summary>
    /// 商城跳转类型
    /// </summary>
    public int ShopType
    {
        get
        {
            return FashionRef.shopType;
        }
    }



	#endregion
	#endregion
}
