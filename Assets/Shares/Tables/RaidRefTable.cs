//==========================
//作者：龙英杰
//日期：2015/12/12
//用途：公会副本静态配置
//==========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RaidRefTable : AssetTable
{
    public List<RaidRef> infoList = new List<RaidRef>();
}

[System.Serializable]
public class RaidRef
{
    /// <summary>
    /// id
    /// </summary>
    public int id;
    /// <summary>
    /// 开启等级
    /// </summary>
    public int openLevel;
    /// <summary>
    /// 重置CD
    /// </summary>
    public int cd;
    /// <summary>
    /// 需求公会资源数
    /// </summary>
    public int needGuildResNum;
    /// <summary>
    /// 奖励
    /// </summary>
    public int reward;
    /// <summary>
    /// 场景ID
    /// </summary>
    public int sceneID;
    /// <summary>
    /// 掉落ID
    /// </summary>
    public int dropcontentId;
    /// <summary>
    /// 公会副本名
    /// </summary>
    public string raidName;
    /// <summary>
    /// 介绍
    /// </summary>
    public string text;
    /// <summary>
    /// boss名
    /// </summary>
    public string bossName;
    /// <summary>
    /// boss图片
    /// </summary>
    public string bossPic;

}
