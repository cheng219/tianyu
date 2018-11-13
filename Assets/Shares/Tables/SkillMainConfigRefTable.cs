//=================================
//作者：龙英杰
//日期：2015/7/15
//用途：技能主表数据结构
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillMainConfigRefTable : AssetTable
{

    public List<SkillMainConfigRef> infoList = new List<SkillMainConfigRef>();
}

[System.Serializable]
public class SkillMainConfigRef
{
    /// <summary>
    /// 技能ID
    /// </summary>
    public int skillId;
    /// <summary>
    /// 技能名称
    /// </summary>
    public string skillName;
    /// <summary>
    /// 技能描述
    /// </summary>
    public string skillDes;
    /// <summary>
    /// 图标
    /// </summary>
    public string skillIcon;
    /// <summary>
    /// 技能类型
    /// </summary>
    public SkillMode skillMode;
    /// <summary>
    /// 对应职业
    /// </summary>
    public int skillRole;
    /// <summary>
    /// 等级上限
    /// </summary>
    public int skillLevelLimit;
    /// <summary>
    /// 解锁等级
    /// </summary>
    public int unlockLvl;
    /// <summary>
    /// 主界面的按钮位置
    /// </summary>
    public int skillField;
    /// <summary>
    /// 基础符文
    /// </summary>
    public int baseRune;
    /// <summary>
    /// 高级符文
    /// </summary>
    public List<int> proRuneList = new List<int>();

    /// <summary>
    /// 技能类型
    /// </summary>
    public int skilltype;

    /// <summary>
    /// 学习技能时的吐槽
    /// </summary>
    public string skillRes;



    public float probability;
}

public enum SkillMode
{
    /// <summary>
    /// 客户端技能
    /// </summary>
    CLIENTSKILL,
    /// <summary>
    /// 脚本技能
    /// </summary>
    SCRIPTSKILL,
    /// <summary>
    /// 被动技能
    /// </summary>
    PASSIVESKILL,
    /// <summary>
    /// 普攻技能
    /// </summary>
    NORMALSKILL,

    UNUSED_SKILL,
}

