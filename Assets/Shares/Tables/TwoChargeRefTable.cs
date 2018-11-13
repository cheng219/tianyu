//=========================
//作者：李邵南
//日期：2017/04/06
//用途：二冲奖励静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TwoChargeRefTable : AssetTable
{
    public List<TwoChargeRef> infoList = new List<TwoChargeRef>();
}


[System.Serializable]
public class TwoChargeRef
{
    /// <summary>
    /// 职业ID
    /// </summary>
    public int id;
    /// <summary>
    /// 奖励ID
    /// </summary>
    public List<ItemValue> reward = new List<ItemValue>();
}


