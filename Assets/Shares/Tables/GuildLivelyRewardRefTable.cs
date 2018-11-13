//======================================================
//作者:邓成
//日期:2017/5/5
//用途:仙盟活跃度奖励
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GuildLivelyRewardRefTable : AssetTable {

    public List<GuildLivelyRewardRef> infoList = new List<GuildLivelyRewardRef>();
}
[System.Serializable]
public class GuildLivelyRewardRef
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
    /// 宝箱名
    /// </summary>
    public string name;
}