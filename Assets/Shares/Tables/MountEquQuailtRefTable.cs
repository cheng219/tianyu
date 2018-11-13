//======================================================
//作者:鲁家旗
//日期:2016/12/20
//用途:坐骑装备品质
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MountEquQuailtRefTable : AssetTable
{
    public List<MountEquQuailtRef> infoList = new List<MountEquQuailtRef>();
}
[System.Serializable]
public class MountEquQuailtRef
{
    /// <summary>
    /// 装备id
    /// </summary>
    public int id;
    /// <summary>
    /// 装备位置
    /// </summary>
    public int position;
    /// <summary>
    /// 装备品质
    /// </summary>
    public int quality;
    /// <summary>
    /// 升到下一级消耗
    /// </summary>
    public List<ItemValue> consume;
    /// <summary>
    /// 分解时获得的物品
    /// </summary>
    public ItemValue deco_cons;
    
}
