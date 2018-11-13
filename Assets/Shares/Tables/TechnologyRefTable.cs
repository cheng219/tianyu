//==========================
//作者：龙英杰
//日期：2015/12/12
//用途：科技表静态配置
//==========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TechnologyRefTable : AssetTable
{
    public List<TechnologyRef> infoList = new List<TechnologyRef>();
}


[System.Serializable]
public class TechnologyRef
{
    /// <summary>
    /// 科技等级
    /// </summary>
    public int id;
    /// <summary>
    /// 解锁等级
    /// </summary>
    public int openLevel;
    /// <summary>
    /// 名字
    /// </summary>
    public string technologyName;
    /// <summary>
    /// 图标
    /// </summary>
    public string pic;
    /// <summary>
    /// 介绍
    /// </summary>
    public string text;

}

