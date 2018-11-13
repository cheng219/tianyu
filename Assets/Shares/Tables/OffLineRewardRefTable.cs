//==============================
//作者：唐源
//日期：2017/3/10
//用途：离线奖励静态配置
//==============================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OffLineRewardRefTable : AssetTable
{
    public List<OffLineRewardRef> OffLineRewardList = new List<OffLineRewardRef>();
}
[System.Serializable]
public class OffLineRewardRef
{
    /// <summary>
    /// 玩家等级(根据等级来获取离线奖励)
    /// </summary>
    public int playerLevel;
    /// <summary>
    /// 奖励列表
    /// </summary>
    public List<ItemValue> rewardList = new List<ItemValue>();
}

