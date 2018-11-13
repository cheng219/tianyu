//=====================
//作者：龙英杰
//日期：2016/1/25
//用途：开服七日奖励
//=====================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DailyRewardRefTable : AssetTable
{
    public List<DailyRewardRef> infoList = new List<DailyRewardRef>();
}


[System.Serializable]
public class DailyRewardRef
{
    /// <summary>
    /// 开服七日奖励天数
    /// </summary>
    public int day;
    /// <summary>
    /// 开服七日奖励掉落表Id
    /// </summary>
    public int dropContentId;
    /// <summary>
    /// 说明
    /// </summary>
    public string des;
}
