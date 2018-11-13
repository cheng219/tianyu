//=========================
//作者：龙英杰
//日期：2015/11/19
//用途：商店静态配置
//=========================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StoreRefTable : AssetTable
{
    public List<StoreRef> infoList = new List<StoreRef>();
}


[System.Serializable]
public class StoreRef
{
    /// <summary>
    /// ID
    /// </summary>
    public int id;
    /// <summary>
    /// 商店名字
    /// </summary>
    public string name;
    /// <summary>
    /// 用于界面显示的说明文字
    /// </summary>
    public string des;
    /// <summary>
    /// 手动刷新此商店时需要消耗的货币类型,为0则不能刷新，不显示刷新按钮
    /// </summary>
    public int refreshCostType;
    /// <summary>
    /// 手动刷新此商店时需要消耗的对应货币数量,为0则不能刷新，不显示刷新按钮
    /// </summary>
    public int refrshCost;
    /// <summary>
    /// 限制等级
    /// </summary>
    public int authorityLevel;
    /// <summary>
    /// 刷新折扣率时，能出现的折扣下限（目标值~1），0为不能刷新折扣率，原价出售
    /// </summary>
    public int discountLimit;
    /// <summary>
    /// 主资源类型
    /// </summary>
    public int mianRes;
    /// <summary>
    /// 广告位展示物品id
    /// </summary>
    public List<int> showId = new List<int>();
    /// <summary>
    /// 商品格子组
    /// </summary>
    public List<int> cellsList = new List<int>();
    /// <summary>
    /// 为0则不是限时商店，不为0则是限时商店，值为到期的日期时间
    /// </summary>
    public List<int> offTimeList = new List<int>();
    /// <summary>
    ///  商店的权限类型
    /// </summary>
    public AuthorityType authorityType;

}
/// <summary>
/// 商店的权限类型，枚举，NO为非权限商店
/// </summary>
public enum AuthorityType
{ 
    NONE = 0,
    /// <summary>
    /// 非权限商店
    /// </summary>
    NO = 1,
    /// <summary>
    /// VIP
    /// </summary>
    VIP = 2,
    /// <summary>
    /// 军衔
    /// </summary>
    MILITARY = 3,
}