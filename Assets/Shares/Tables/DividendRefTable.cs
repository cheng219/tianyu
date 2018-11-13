//=========================
//作者：李邵南
//日期：2017/04/14
//用途：登陆红利静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DividendRefTable : AssetTable
{
    public List<DividendRef> infoList = new List<DividendRef>();
}

[System.Serializable]
public class DividendRef
{
    /// <summary>
    /// ID
    /// </summary>
    public int ID;
    /// <summary>
    /// 需要的天数
    /// </summary>
    public int need_days;
    /// <summary>
    /// 奖励物品信息
    /// </summary>
    public List<ItemValue> items = new List<ItemValue>();
    /// <summary>
    /// 描述文本信息
    /// </summary>
    public string txt;
}
