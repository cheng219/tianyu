//=====================
//作者：黄洪兴
//日期：2016/1/25
//用途：七天登录奖励
//=====================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SevenDaysRefTable : AssetTable
{
    public List<SevenDaysProfRef> infoList = new List<SevenDaysProfRef>();
}

[System.Serializable]
public class SevenDaysProfRef
{
    public int prof;
    public int day;
    public List<SevenDaysRef> stepList = new List<SevenDaysRef>();
    protected Dictionary<int, SevenDaysRef> stepDic = new Dictionary<int, SevenDaysRef>();

    public void InitData()
    {
        stepDic.Clear();
        foreach (var item in stepList)
        {
            stepDic[item.day] = item;
        }
    }

    public SevenDaysRef GetLevelRewardRef(int day)
    {
        if (stepDic.ContainsKey(day))
        {
            return stepDic[day];
        }
        return null;
    }

}




[System.Serializable]
public class SevenDaysRef
{
    public int day;
    /// <summary>
    /// 职业
    /// </summary>
    public int Prof;
    /// <summary>
    /// 奖励
    /// </summary>
    public List<ItemValue> reward = new List<ItemValue>();

    /// <summary>
    /// VIP奖励
    /// </summary>
    public List<ItemValue> vipReward=new List<ItemValue>();
    /// <summary>
    /// VIP奖励等级限制
    /// </summary>
    public int vipLimit;
    /// <summary>
    /// 贵重物品需要显示框
    /// </summary>
    public List<int> specialList = new List<int>();
    /// <summary>
    /// 展示物品
    /// </summary>
    public List<int> itemIdList = new List<int>();
}
