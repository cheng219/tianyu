//======================================================
//作者:鲁家旗
//日期:2017/1/19
//用途:活跃度奖励
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LivelyRewardRefTable : AssetTable {

    public List<LivelyRewardRef> infoList = new List<LivelyRewardRef>();
}
[System.Serializable]
public class LivelyRewardRef
{
    /// <summary>
    /// id
    /// </summary>
    public int id;
    /// <summary>
    /// 需要多少活跃度
    /// </summary>
    public int need;
    /// <summary>
    /// 奖励物品
    /// </summary>
    public List<ItemValue> reward = new List<ItemValue>();
    /// <summary>
    /// vip几可以双倍领取
    /// </summary>
    public int vip;
    /// <summary>
    /// 宝箱名
    /// </summary>
    public string name;
}