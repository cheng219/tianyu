//=========================
//作者：龙英杰
//日期：2015/11/19
//用途：宝石静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DiamondRefTable : AssetTable
{
    public List<DiamondRef> infoList = new List<DiamondRef>();
}


[System.Serializable]
public class DiamondRef
{
    /// <summary>
    /// 宝石ID
    /// </summary>
    public int id;
    /// <summary>
    /// 宝石名
    /// </summary>
    public string diamonName;
    /// <summary>
    /// 宝石图标
    /// </summary>
    public string picture;
    /// <summary>
    /// 对应装备位置
    /// </summary>
    public int diamondPosition;
    /// <summary>
    /// 宝石位置
    /// </summary>
    public int order;
    /// <summary>
    /// 打开限制   1位置上为等级  2位置上为开启物品ID
    /// </summary>
    public int openItem;
    /// <summary>
    /// 等级上限
    /// </summary>
    public int upgradeLimit;
    /// <summary>
    /// 属性ID
    /// </summary>
    public int property;
    /// <summary>
    /// 属性初始值
    /// </summary>
    public int PropertyValue;
    /// <summary>
    /// 属性成长值
    /// </summary>
    public int growth;
    /// <summary>
    /// 战力值
    /// </summary>
    public int gs;
    /// <summary>
    /// 宝石开启等级
    /// </summary>
    public int openLev;
    /// <summary>
    /// 升级系数  带入升级所需经验公式中
    /// </summary>
    public float upgrade;
    /// <summary>
    /// 升级碎片ID
    /// </summary>
    public List<int> upgradeQuantityList = new List<int>();

}
