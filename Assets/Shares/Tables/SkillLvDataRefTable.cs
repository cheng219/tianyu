 //=====================================
//作者：易睿
//日期：2015/7/15
//用途：技能等级静态配置
//==========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillLvDataRefTable : AssetTable
{
    public List<SkillLvDataRef> infoList = new List<SkillLvDataRef>();
}


[System.Serializable]
public class SkillLvDataRef
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
    /// 技能参数1，一般做倍数
    /// </summary>
    public float powerOne;
    /// <summary>
    /// 技能参数2，一般做附加值
    /// </summary>
    public float powerTwo;
    /// <summary>
    /// 附加压制值
    /// </summary>
    public float longSuffering;
    /// <summary>
    /// 额外附加仇恨
    /// </summary>
    public int threaten;
    /// <summary>
    /// mp
    /// </summary>
    public int mp;
    /// <summary>
    /// 技能CD时间
    /// </summary>
    public float cd;
    ///// <summary>
    ///// 对目标BUFFiD组
    ///// </summary>
    //public List<int> targetBuffId = new List<int>();
    ///// <summary>
    ///// 对目标BUFF参数组
    ///// </summary>
    //public List<float> targetBuffPara = new List<float>();
    ///// <summary>
    ///// 对目标BUFF持续时间组
    ///// </summary>
    //public List<float> targetBuffTime = new List<float>();
    ///// <summary>
    ///// 对自己BUFFiD组
    ///// </summary>
    //public List<int> selfBuffId = new List<int>();
    ///// <summary>
    ///// 对自己BUFF参数组
    ///// </summary>
    //public List<float> selfBuffPara = new List<float>();
    ///// <summary>
    ///// 对自己BUFF持续时间组
    ///// </summary>
    //public List<float> selfBuffTime = new List<float>();

    /// <summary>
    /// 对目标buff组
    /// </summary>
    public List<SkillLvBuff> targetBuff = new List<SkillLvBuff>();
    /// <summary>
    /// 对自己buff组
    /// </summary>
    public List<SkillLvBuff> selfBuff = new List<SkillLvBuff>();

}

 [System.Serializable]
public class SkillLvBuff
{
    public int buffId;
    public float buffPara;
    public float buffTime;
}

