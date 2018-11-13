//======================================================
//作者:
//日期:
//用途:
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleFieldRefTable : AssetTable
{
    public List<BattleFieldRef> infoList = new List<BattleFieldRef>();
}

[System.Serializable]
public class BattleFieldRef
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
