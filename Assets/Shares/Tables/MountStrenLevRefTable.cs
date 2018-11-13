//======================================================
//作者:鲁家旗
//日期:2016/12/20
//用途:坐骑装备等级
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MountStrenLevRefTable : AssetTable
{
    public List<MountStrenLevRef> infoList = new List<MountStrenLevRef>();
}
[System.Serializable]
public class MountStrenLevRef
{
    /// <summary>
    /// ID
    /// </summary>
    public int id;
    /// <summary>
    /// 装备当前等级
    /// </summary>
    public int lev;
    /// <summary>
    /// 装备所属部位
    /// </summary>
    public int position;
    /// <summary>
    /// 装备强化到该等级后的属性
    /// </summary>
    public List<AttributePair> attr = new List<AttributePair>();
    /// <summary>
    /// 战力
    /// </summary>
    public int gs;
}
