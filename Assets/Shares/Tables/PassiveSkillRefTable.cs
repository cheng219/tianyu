//====================
//作者：黄洪兴
//日期：2016/03/24
//用途：被动技能配置
//====================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PassiveSkillRefTable : AssetTable
{
    public List<PassiveSkillRef> infoList = new List<PassiveSkillRef>();
}


[System.Serializable]
public class PassiveSkillRef
{
    public int id;
    public int probability;


    public string name;
    /// <summary>
    /// 说明
    /// </summary>
    public string des;

    public PassiveSkillType type;
}


[System.Serializable]
public enum PassiveSkillType
{
    /// <summary>
    /// 玩家技能
    /// </summary>
    PLAYER=1,
    /// <summary>
    /// 法宝技能
    /// </summary>
    MAGIC = 2,
    /// <summary>
    /// 翅膀技能
    /// </summary>
    WING = 3,



}