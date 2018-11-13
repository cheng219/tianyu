//===========================
//作者：龙英杰
//日期：2016/2/3
//用途：试炼场关卡难度静态配置
//===========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrialsDiffRefTable : AssetTable
{
    public List<TrialsDiffRef> infoList = new List<TrialsDiffRef>();
}


[System.Serializable]
public class TrialsDiffRef
{
    /// <summary>
    /// 试炼场组ID
    /// </summary>
    public int trialId;

    public List<TrialsDiffStepRef> stepList = new List<TrialsDiffStepRef>();


    protected Dictionary<int, TrialsDiffStepRef> stepDic = new Dictionary<int, TrialsDiffStepRef>();

    public void InitData()
    {
        foreach (var item in stepList)
        {
            stepDic[item.diffID] = item;
        }
    }

    public TrialsDiffStepRef GetTrialsDiffStepRef(int _step)
    {
        if (stepDic.ContainsKey(_step))
        {
            return stepDic[_step];
        }
        return null;
    }


}

[System.Serializable]
public class TrialsDiffStepRef
{
    /// <summary>
    /// 试炼场组ID
    /// </summary>
    public int trialId;
    /// <summary>
    /// 关卡难度ID
    /// </summary>
    public int diffID;
    /// <summary>
    /// 关卡难度名字
    /// </summary>
    public string diffName;
    /// <summary>
    /// 关卡难度开启等级
    /// </summary>
    public int diffOpenLev;
    /// <summary>
    /// 对应的副本内容ID
    /// </summary>
    public int dungeonId;
    /// <summary>
    /// 产出物品
    /// </summary>
    public List<int> produceIcon = new List<int>();
}