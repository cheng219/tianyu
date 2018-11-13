//=======================
//作者：龙英杰
//日期：2016/2/24
//用途：英雄挑战静态配置
//=======================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HerosChallengeRefTable : AssetTable
{
    public List<HerosChallengeRef> infoList = new List<HerosChallengeRef>();
}


[System.Serializable]
public class HerosChallengeRef
{
    public int id;
    /// <summary>
    /// 怪物ID
    /// </summary>
    public int monsterId;
    /// <summary>
    /// 等级需求
    /// </summary>
    public int levelRequire;
    /// <summary>
    /// 宝箱ID
    /// </summary>
    public int boxId;
    /// <summary>
    /// 副本ID
    /// </summary>
    public int dungeonID;
    /// <summary>
    /// 说明
    /// </summary>
    public string des;
    /// <summary>
    /// 奖励显示
    /// </summary>
    public List<int> rewardShowList = new List<int>();
}

