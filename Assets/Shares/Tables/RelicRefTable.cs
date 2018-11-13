//==============================
//作者：龙英杰
//日期：2015/09/21
//用途：上古遗物表静态配置
//==============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RelicRefTable : AssetTable
{
    public List<RelicRef> infoList = new List<RelicRef>();
}


[System.Serializable]
public class RelicRef
{
    /// <summary>
    /// 遗物名称
    /// </summary>
    public string name;
    /// <summary>
    /// 遗物介绍文本
    /// </summary>
    public string intro;
    /// <summary>
    /// 遗物图片
    /// </summary>
    public string relicPic;
    /// <summary>
    /// 遗物ID
    /// </summary>
    public int id;
    /// <summary>
    /// 所需碎片数量
    /// </summary>
    public int needed_item_num;
    /// <summary>
    /// 碎片物品ID
    /// </summary>
    public int item_id;
    /// <summary>
    /// 被动技能
    /// </summary>
    public int skill;
    /// <summary>
    /// 战力
    /// </summary>
    public int gs;
    /// <summary>
    /// 遗物的最大等级
    /// </summary>
    public int maxLev;
    /// <summary>
    /// 属性类型数组
    /// </summary>
    public List<RelicProp> propList = new List<RelicProp>();

}

[System.Serializable]
public class RelicProp
{
    public ActorPropertyTag tag;
    public int baseValue;
    public int growsValue;
}
