//=======================
//作者：龙英杰
//日期：2015/12/03
//用途：活跃度静态配置
//=======================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivitiesRefTable : AssetTable
{
    public List<ActivitiesRef> infoList = new List<ActivitiesRef>();
}

[System.Serializable]
public class ActivitiesRef
{
    /// <summary>
    /// ID
    /// </summary>
    public int id;
    /// <summary>
    /// 活跃度点数
    /// </summary>
    public int actRewards;
    /// <summary>
    /// 活跃度次数
    /// </summary>
    public int actTimes;
    /// <summary>
    /// 活跃度描述
    /// </summary>
    public string actText;

}
