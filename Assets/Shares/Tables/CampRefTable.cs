//=================================
//作者：龙英杰
//日期：2016/1/20
//用途：阵营
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CampRefTable : AssetTable
{
    public List<CampRef> infoList = new List<CampRef>();
}

[System.Serializable]
public class CampRef
{
    /// <summary>
    /// 阵营ID
    /// </summary>
    public int campId;
    /// <summary>
    /// 阵营BUFF ID
    /// </summary>
    public int campBuffId;
    /// <summary>
    /// 阵营名字
    /// </summary>
    public string campName;
    /// <summary>
    /// 阵营说明
    /// </summary>
    public string campDes;
    /// <summary>
    /// 阵营大图标
    /// </summary>
    public string campBigIcon;
    /// <summary>
    /// 阵营小图标
    /// </summary>
    public string campSmallIcon;

}
