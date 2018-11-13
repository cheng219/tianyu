//=======================
//作者：龙英杰
//日期：2015/12/03
//用途：活跃奖励静态配置
//=======================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActRewardsRefTable : AssetTable
{
    public List<ActRewardsRef> infoList = new List<ActRewardsRef>();
}

[System.Serializable]
public class ActRewardsRef
{
    /// <summary>
    /// ID
    /// </summary>
    public int id;
    /// <summary>
    /// 活跃度
    /// </summary>
    public int requireAct;
    /// <summary>
    /// 活跃度奖励物品ID
    /// </summary>
    public int actRewardItemID;
    /// <summary>
    /// 活跃度奖励物品数量
    /// </summary>
    public int actRewardItemNum;

}
