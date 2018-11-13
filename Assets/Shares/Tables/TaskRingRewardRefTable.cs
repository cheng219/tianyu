//=========================
//作者：朱素云
//日期：2017/5/6
//用途：环任务奖励静态配置表
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskRingRewardRefTable : AssetTable
{
    public List<TaskRingRewardRef> infoList = new List<TaskRingRewardRef>();
}

[System.Serializable]
public class TaskRingRewardRef
{

    /// <summary>
    /// 副本ID
    /// </summary>
    public int id;
    /// <summary>
    /// 难度
    /// </summary>
    public int difficuilty;    
    /// 奖励
    /// </summary>
    public List<ItemValue> reward = new List<ItemValue>();
    /// <summary>
    /// 环任务名字
    /// </summary>
    public string typeName;
    /// <summary>
    /// 描述
    /// </summary>
    public string des;
    /// <summary>
    /// 图片
    /// </summary>
    public string icon;
}
