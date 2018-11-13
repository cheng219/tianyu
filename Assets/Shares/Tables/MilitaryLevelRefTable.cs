//========================
//作者：龙英杰
//日期：2016/2/18
//用途：军衔等级静态配置
//========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MilitaryLevelRefTable : AssetTable
{
    public List<MilitaryLevelRef> infoList = new List<MilitaryLevelRef>();
}


[System.Serializable]
public class MilitaryLevelRef
{
    /// <summary>
    /// 索引ID
    /// </summary>
    public int id;
    /// <summary>
    /// 军衔名称
    /// </summary>
    public string rankname;
    /// <summary>
    /// 军衔等级的映射
    /// </summary>
    public string pic;
    /// <summary>
    /// 阵营
    /// </summary>
    public int camp;
    /// <summary>
    /// 军衔对应的等级
    /// </summary>
    public int level;
    /// <summary>
    /// 升级所需经验
    /// </summary>
    public int rankExp;
    /// <summary>
    /// 掉落ID
    /// </summary>
    public int rewardId;
    /// <summary>
    /// 被击杀给予荣誉值
    /// </summary>
    public int bekilledRep;
}
