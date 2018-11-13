//=========================
//作者：龙英杰
//日期：2015/09/22
//用途：随从经验表静态配置
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntourageExpRefTable : AssetTable
{
    public List<EntourageExpRef> infoList = new List<EntourageExpRef>();
}


[System.Serializable]
public class EntourageExpRef
{
    /// <summary>
    /// 等级
    /// </summary>
    public int level;
    /// <summary>
    /// 经验
    /// </summary>
    public int exp;

}
