//==============================
//作者：龙英杰
//日期：2016/1/30
//用途：章节奖励静态配置
//==============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StoryRewardRefTable : AssetTable
{
    public List<StoryRewardRef> infoList = new List<StoryRewardRef>();
}


[System.Serializable]
public class StoryRewardRef
{
    /// <summary>
    /// ID
    /// </summary>
    public int id;
    /// <summary>
    /// 3号职业需要的奖励物品ID
    /// </summary>
    public int rewardItemThree;
    /// <summary>
    /// 6号职业需要的奖励物品ID
    /// </summary>
    public int rewardItemSix;
    /// <summary>
    /// 9号职业需要的奖励物品ID
    /// </summary>
    public int rewardItemNine;
    /// <summary>
    /// 需要的任务数量
    /// </summary>
    public int needTaskNum;
    /// <summary>
    /// 是否是最后一个章节
    /// </summary>
    public int isEnd;
}
