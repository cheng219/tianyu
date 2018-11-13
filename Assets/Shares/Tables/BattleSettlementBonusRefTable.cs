//======================================================
//作者:朱素云
//日期:2017/5/10
//用途:火焰山结算奖励
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleSettlementBonusRefTable : AssetTable
{
    public List<BattleSettlementBonusRef> infoList = new List<BattleSettlementBonusRef>();
}

[System.Serializable]
public class BattleSettlementBonusRef
{
    /// <summary>
    /// 1 胜利 2 失败 3平局
    /// </summary>
    public int id; 
    /// <summary>
    /// 奖励列表
    /// </summary>
    public List<ItemValue> rewardList = new List<ItemValue>(); 
}
