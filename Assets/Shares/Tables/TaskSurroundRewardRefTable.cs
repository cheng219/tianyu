//=========================
//作者：黄洪兴
//日期：2016/6/18
//用途：环式任务奖励静态配置表
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskSurroundRewardRefTable : AssetTable
{
    public List<TaskSurroundRewardRef> infoList = new List<TaskSurroundRewardRef>();
}

[System.Serializable]
public class TaskSurroundRewardRef
{


    public int Lev;
    public int gold;
    public int exp;
}
