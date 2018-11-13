//======================================================
//作者:鲁家旗
//日期:2016/12/20
//用途:坐骑装备培养消耗
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MountStrenConsumeRefTable : AssetTable {

    public List<MountStrenConsumeRef> infoList = new List<MountStrenConsumeRef>();
}
[System.Serializable]
public class MountStrenConsumeRef
{
    /// <summary>
    /// 等级
    /// </summary>
    public int lev;
    /// <summary>
    /// 普通强化一次消耗物品
    /// </summary>
    public List<ItemValue> item;
    /// <summary>
    /// 强化一次增加的基础强化值
    /// </summary>
    public List<int> add_exp1;
    /// <summary>
    /// 强化一次增加的爆发强化值
    /// </summary>
    public List<int> add_exp2;
    /// <summary>
    /// 分解时获得的物品
    /// </summary>
    public ItemValue deco_cons;
}