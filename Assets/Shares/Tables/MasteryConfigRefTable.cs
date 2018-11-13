//==============================
//作者：龙英杰
//日期：2015/11/3
//用途：专精表静态配置
//==============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasteryConfigRefTable : AssetTable
{
    public List<MasteryConfigRef> infoList = new List<MasteryConfigRef>();
}


[System.Serializable]
public class MasteryConfigRef
{
    /// <summary>
    /// 天赋名称
    /// </summary>
    public string name;
    /// <summary>
    /// 专精图标
    /// </summary>
    public string icon;
    /// <summary>
    /// 天赋ID
    /// </summary>
    public int id;
    /// <summary>
    /// 开启玩家等级
    /// </summary>
    public int openPlyLvl;
    /// <summary>
    /// 属性类型数组
    /// </summary>
    public List<MasteryProp> propList = new List<MasteryProp>();

}

[System.Serializable]
public class MasteryProp
{
    /// <summary>
    /// 属性
    /// </summary>
    public ActorPropertyTag tag;
    /// <summary>
    /// 成长值
    /// </summary>
    public int growsValue;
    /// <summary>
    /// 属性解锁等级
    /// </summary>
    public int typeUnlockLv;
}
