//=============================
//作者：龙英杰
//日期：2015/10/26
//用途：技能符文表
//=============================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillRuneRefTable : AssetTable
{
    public List<SkillRuneRef> infoList = new List<SkillRuneRef>();
}


[System.Serializable]
public class SkillRuneRef
{
    /// <summary>
    /// ID
    /// </summary>
    public int runeId;
    /// <summary>
    /// 解锁需求玩家等级
    /// </summary>
    public int unlockPlayerLvl;
    /// <summary>
    /// 解锁需求技能等级
    /// </summary>
    public int unlockSkillLvl;
    /// <summary>
    /// 解锁需求资源数
    /// </summary>
    public int unlockPrice;
    /// <summary>
    /// 解锁所需道具
    /// </summary>
    public int unlockItem;
    /// <summary>
    /// 符文名
    /// </summary>
    public string name;
    /// <summary>
    /// 符文说明
    /// </summary>
    public string des;
    /// <summary>
    /// 符文图标
    /// </summary>
    public string runeIcon;
    /// <summary>
    /// 符文对应的技能
    /// </summary>
    public int performanceID;
    /// <summary>
    /// 从属技能ID
    /// </summary>
    public int skillMainId;

}
