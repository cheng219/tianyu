//======================================================
//作者:鲁家旗
//日期:2017/1/19
//用途:皇室宝箱静态配置
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoyalBoxRefTable : AssetTable {
    public List<RoyalBoxRef> infoList = new List<RoyalBoxRef>();
}

[System.Serializable]
public class RoyalBoxRef
{
    /// <summary>
    /// 宝箱物品ID
    /// </summary>
    public int boxItemID;
    /// <summary>
    /// 开启消耗时间
    /// </summary>
    public int time;
    /// <summary>
    /// 未开启宝箱图片
    /// </summary>
    public string notOpenIcon;
    /// <summary>
    /// 已开启宝箱图片
    /// </summary>
    public string haveOpenIcon;
    /// <summary>
    /// 宝箱特效
    /// </summary>
    public string effect;
}