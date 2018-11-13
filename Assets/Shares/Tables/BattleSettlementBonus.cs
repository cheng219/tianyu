//======================================================
//作者:朱素云
//日期:2017/5/10
//用途:火焰山结算奖励
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleSettlementBonus : AssetTable
{
    public List<BattleFieldRef> infoList = new List<BattleFieldRef>();
}

[System.Serializable]
public class BattleSettlementBo
{
    /// <summary>
    /// 副本组id
    /// </summary>
    public int id;
    /// <summary>
    /// 等级奖励图标
    /// </summary>
    public string icon;
    /// <summary>
    /// 奖励条件（积分）
    /// </summary>
    public List<int> rewardConditionList;
    /// <summary>
    /// 奖励列表
    /// </summary>
    public List<ItemValue> rewardList = new List<ItemValue>();
    /// <summary>
    /// 评分说明
    /// </summary>
    public string scoreDes;
}
