//=============================
//作者：龙英杰
//日期：2015/10/26
//用途：技能升级数据配置
//=============================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillMainLvRefTable : AssetTable
{
    public List<SkillMainLvRef> infoList = new List<SkillMainLvRef>();
}


[System.Serializable]
public class SkillMainLvRef
{
    /// <summary>
    /// 技能ID
    /// </summary>
    public int skillId;
    /// <summary>
    /// 技能等级
    /// </summary>
    public int skillLv;
    /// <summary>
    /// 学习需求等级
    /// </summary>
    public int learnLv;
    /// <summary>
    /// 货币类型
    /// </summary>
    public int coinType;
    /// <summary>
    /// 货币值
    /// </summary>
    public int learnCoin;
    /// <summary>
    /// 第二货币类型
    /// </summary>
    public int spType;
    /// <summary>
    /// 第二货币值
    /// </summary>
    public int learnSp;
    /// <summary>
    /// 附加战力
    /// </summary>
    public int gs;
    
}
