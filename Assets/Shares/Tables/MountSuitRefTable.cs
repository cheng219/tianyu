//======================================================
//作者:鲁家旗
//日期:2016/12/20
//用途:坐骑装备套装属性
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MountSuitRefTable : AssetTable
{
    public List<MountSuitRef> infoList = new List<MountSuitRef>();
}

[System.Serializable]
public class MountSuitRef
{
    /// <summary>
    /// 品质
    /// </summary>
    public int quality;
    /// <summary>
    /// 套装属性
    /// </summary>
    public List<AttributePair> attrs = new List<AttributePair>();
    /// <summary>
    /// 套装战力
    /// </summary>
    public int gs;
    /// <summary>
    /// 激活时显示的标题文本
    /// </summary>
    public string curDes;
    /// <summary>
    /// 未激活时显示的标题文本
    /// </summary>
    public string nextDes;
}
