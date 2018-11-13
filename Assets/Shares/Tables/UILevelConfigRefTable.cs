//=================================================
//作者：龙英杰
//日期：2015/9/14
//用途：UI层级管理
//=================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UILevelConfigRefTable : AssetTable
{

    public List<UILevelConfigRef> infoList = new List<UILevelConfigRef>();
}

[System.Serializable]
public class UILevelConfigRef
{
    /// <summary>
    /// 一级界面枚举
    /// </summary>
    public GUIType levelOneUI;
    /// <summary>
    /// 二级界面枚举
    /// </summary>
    public List<SubGUIType> levelTwoUIGroup = new List<SubGUIType>();
}
