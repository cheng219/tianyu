//=====================
//作者：黄洪兴
//日期：2016/1/25
//用途：等级奖励
//=====================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelRewardRefTable : AssetTable
{
    public List<LevelRewardRef> infoList = new List<LevelRewardRef>();
}

[System.Serializable]
public class LevelRewardRef
{
    public int level;
    public int prof;

    public List<LevelRewardLevelRef> stepList = new List<LevelRewardLevelRef>();
    protected Dictionary<int, LevelRewardLevelRef> stepDic = new Dictionary<int, LevelRewardLevelRef>();

    public void InitData()
    {
        stepDic.Clear();
        foreach (var item in stepList)
        {
            stepDic[item.level] = item;
        }
    }

    public LevelRewardLevelRef GetLevelRewardRef(int level)
    {
        if (stepDic.ContainsKey(level))
        {
            return stepDic[level];
        }
        return null;
    }

}




[System.Serializable]
public class LevelRewardLevelRef
{
    /// <summary>
    /// 职业
    /// </summary>
    public int reqProf; 
    /// <summary>
    /// 等级奖励等级
    /// </summary>
    public int level;
    /// <summary>
    /// 奖励
    /// </summary>
	public List<ItemValue> item=new List<ItemValue>();
}
