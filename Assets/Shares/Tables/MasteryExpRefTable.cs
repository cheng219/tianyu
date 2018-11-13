//==============================
//作者：龙英杰
//日期：2015/11/3
//用途：专精经验表静态配置
//==============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasteryExpRefTable : AssetTable
{
    public List<MasteryExpRef> infoList = new List<MasteryExpRef>();
}


[System.Serializable]
public class MasteryExpRef
{
    /// <summary>
    /// 天赋ID
    /// </summary>
    public int id;

    public List<MasteryStepRef> stepList = new List<MasteryStepRef>();


    protected Dictionary<int, MasteryStepRef> stepDic = new Dictionary<int, MasteryStepRef>();

    public void InitData()
    {
        foreach (var item in stepList)
        {
            stepDic[item.lvl] = item;
        }
    }

    public MasteryStepRef GetMasteryStepRef(int _step)
    {
        if (stepDic.ContainsKey(_step))
        {
            return stepDic[_step];
        }
        return null;
    }


}

[System.Serializable]
public class MasteryStepRef 
{
    /// <summary>
    /// 天赋ID
    /// </summary>
    public int id;
    /// <summary>
    /// 天赋等级
    /// </summary>
    public int lvl;
    /// <summary>
    /// 升级所需资源类型
    /// </summary>
    public int resType;
    /// <summary>
    /// 升级所需资源数量
    /// </summary>
    public int resNum;
    /// <summary>
    /// 升级所需玩家等级
    /// </summary>
    public int playerLevel;
}