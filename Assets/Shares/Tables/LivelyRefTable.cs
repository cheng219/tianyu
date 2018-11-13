//=========================
//作者：黄洪兴
//日期：2016/7/8
//用途：活跃度静态配置表
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LivelyRefTable : AssetTable
{
    public List<LivelyRef> infoList = new List<LivelyRef>();
}

[System.Serializable]
public class LivelyRef
{


    public int id;

    public int type;

    public int need_lev;

    public int lvId;

    public string name;

    public string Icon;

    public string Des;
    public int sort;



    public List<TaskConditionRef> task_condition = new List<TaskConditionRef>();


    public ItemValue reward;

    public string guitype;
    public int uinum;
    /// <summary>
    /// 奖励活跃度数量
    /// </summary>
    public int rewardlive;
    public List<int> where;
}
