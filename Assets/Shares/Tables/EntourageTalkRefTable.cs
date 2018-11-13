//==============================
//作者：龙英杰
//日期：2015/12/03
//用途：随从说话表静态配置
//==============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntourageTalkRefTable : AssetTable
{
    public List<EntourageTalkRef> infoList = new List<EntourageTalkRef>();
}

[System.Serializable]
public class EntourageTalkRef
{
    /// <summary>
    /// 天赋ID
    /// </summary>
    public int id;

    public List<EntourageTalkStepRef> stepList = new List<EntourageTalkStepRef>();


    protected Dictionary<EntourageTalkType, EntourageTalkStepRef> stepDic = new Dictionary<EntourageTalkType, EntourageTalkStepRef>();

    public void InitData()
    {
        foreach (var item in stepList)
        {
            stepDic[item.talkType] = item;
        }
    }

    public EntourageTalkStepRef GetEntourageTalkStepRef(EntourageTalkType _step)
    {
        if (stepDic.ContainsKey(_step))
        {
            return stepDic[_step];
        }
        return null;
    }
}

[System.Serializable]
public class EntourageTalkStepRef
{
    /// <summary>
    /// ID
    /// </summary>
    public int entourageId;
    /// <summary>
    /// 最大间隔时间
    /// </summary>
    public int intervalTime;
    /// <summary>
    /// 泡泡组
    /// </summary>
    public List<int> bubbleIdGroup = new List<int>();
    /// <summary>
    /// 随从说话情况
    /// </summary>
    public EntourageTalkType talkType;
}
/// <summary>
/// 随从说话情况
/// </summary>
public enum EntourageTalkType
{
    //NONE = 0,
    /// <summary>
    /// 召唤时
    /// </summary>
    CALL = 1,
    /// <summary>
    /// 长时间无目标
    /// </summary>
    NOTARGET = 2,
    /// <summary>
    /// 攻击敌人时
    /// </summary>
    ATTACK = 3,
}