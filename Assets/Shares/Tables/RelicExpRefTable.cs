//==============================
//作者：龙英杰
//日期：2015/09/24
//用途：上古遗物表经验静态配置
//==============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RelicExpRefTable : AssetTable
{
    public List<RelicExpRef> infoList = new List<RelicExpRef>();
}


[System.Serializable]
public class RelicExpRef
{
    
    /// <summary>
    /// 等级
    /// </summary>
    public int level;
    /// <summary>
    /// 所需碎片量
    /// </summary>
    public int itemNum;

}
