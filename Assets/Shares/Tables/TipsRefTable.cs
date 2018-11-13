//======================
//作者：龙英杰
//日期：2016/2/14
//用途：小提示静态配置
//======================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TipsRefTable : AssetTable
{
    public List<TipsRef> infoList = new List<TipsRef>();
}


[System.Serializable]
public class TipsRef
{
    /// <summary>
    /// id
    /// </summary>
    public int id;
    /// <summary>
    /// 提示
    /// </summary>
    public string tips;

}
