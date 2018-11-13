//========================
//作者：龙英杰
//日期：2016/2/3
//用途：试炼场静态配置
//========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrialsRefTable : AssetTable
{
    public List<TrialsRef> infoList = new List<TrialsRef>();
}


[System.Serializable]
public class TrialsRef
{
    /// <summary>
    /// 试炼场组ID
    /// </summary>
    public int trialId;
    /// <summary>
    /// 进入次数
    /// </summary>
    public int intoNum;
    /// <summary>
    /// 试炼场名字
    /// </summary>
    public string trialName;
    /// <summary>
    /// 说明
    /// </summary>
    public string trialDes;
    /// <summary>
    /// 物品ID（卷轴）
    /// </summary>
    public int reelID;
    /// <summary>
    /// 试炼场开启等级
    /// </summary>
    public int openLevel;
    ///// <summary>
    ///// 下一个装备品质
    ///// </summary>
    //public int nextEquipment;

}
