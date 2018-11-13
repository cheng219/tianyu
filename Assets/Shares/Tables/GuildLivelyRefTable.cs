//=========================
//作者：邓成
//日期：2017/5/5
//用途：仙盟活跃度静态配置表
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuildLivelyRefTable : AssetTable
{
    public List<GuildLivelyRef> infoList = new List<GuildLivelyRef>();
}

[System.Serializable]
public class GuildLivelyRef
{
    public int id;

    public string name;

    public string Des;
    public bool needGoBtn;//是否有前往按钮
    public List<TaskConditionRef> task_condition = new List<TaskConditionRef>();
    public string guitype;
    public int uinum;
    /// <summary>
    /// 奖励活跃度数量
    /// </summary>
    public int rewardlive;
    public List<int> where;
}
