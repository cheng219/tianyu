//==============================
//作者：龙英杰
//日期：2015/11/17
//用途：副本组静态配置
//==============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonGroupRefTable : AssetTable
{
    public List<DungeonGroupRef> infoList = new List<DungeonGroupRef>();
}


[System.Serializable]
public class DungeonGroupRef
{
    /// <summary>
    /// 副本组ID
    /// </summary>
    public int id;
    /// <summary>
    /// 副本名
    /// </summary>
    public string name;
    /// <summary>
    /// 图标
    /// </summary>
    public string icon;
    /// <summary>
    /// 普通
    /// </summary>
    public int normalLev;
    /// <summary>
    /// 困难
    /// </summary>
    public int hardLev;
    /// <summary>
    /// 英雄
    /// </summary>
    public int heroLev;
    /// <summary>
    /// 副本最大免费次数
    /// </summary>
    public int freeTimes;
    /// <summary>
    /// 副本开启需要的玩家等级
    /// </summary>
    public int openPlayerLevel;

}
