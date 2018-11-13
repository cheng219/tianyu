//==========================
//作者：龙英杰
//日期：2015/12/12
//用途：科技经验表静态配置
//==========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TechnologyExpRefTable : AssetTable
{
    public List<TechnologyExpRef> infoList = new List<TechnologyExpRef>();
}


[System.Serializable]
public class TechnologyExpRef
{
    public int technologyId;

    public List<TechnologyStepRef> stepList = new List<TechnologyStepRef>();


    protected Dictionary<int, TechnologyStepRef> stepDic = new Dictionary<int, TechnologyStepRef>();

    public void InitData()
    {
        foreach (var item in stepList)
        {
            stepDic[item.level] = item;
        }
    }

    public TechnologyStepRef GetMasteryStepRef(int _step)
    {
        if (stepDic.ContainsKey(_step))
        {
            return stepDic[_step];
        }
        return null;
    }

}

[System.Serializable]
public class TechnologyStepRef
{
    /// <summary>
    /// id
    /// </summary>
    public int id;
    /// <summary>
    /// 科技id
    /// </summary>
    public int technologyId;
    /// <summary>
    /// 等级
    /// </summary>
    public int level;
    /// <summary>
    /// 属性id
    /// </summary>
    public int propid;
    /// <summary>
    /// 属性值
    /// </summary>
    public int propVal;
    /// <summary>
    /// 需求经验
    /// </summary>
    public int needexp;

}

