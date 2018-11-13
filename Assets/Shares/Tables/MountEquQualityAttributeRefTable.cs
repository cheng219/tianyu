//======================================================
//作者:鲁家旗
//日期:2016/12/20
//用途:坐骑装备品质属性
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MountEquQualityAttributeRefTable : AssetTable
{
    public List<MountEquQualityAttributeRef> infoList = new List<MountEquQualityAttributeRef>();
}
[System.Serializable]
public class MountEquQualityAttributeRef
{
    /// <summary>
    /// ID
    /// </summary>
    public int id;
    /// <summary>
    /// 品质
    /// </summary>
    public int quality;
    /// <summary>
    /// 装备所属部位
    /// </summary>
    public int position;
    /// <summary>
    /// 属性
    /// </summary>
    public List<AttributePair> attrs = new List<AttributePair>();
    /// <summary>
    /// 战力
    /// </summary>
    public int gs;
}
 
