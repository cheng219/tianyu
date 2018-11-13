//======================================================
//作者:鲁家旗
//日期:2017/1/18
//用途:铸魂奖励表
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CastsoulRewardRefTable : AssetTable
{
    public List<CastsoulRewardRef> infoList = new List<CastsoulRewardRef>();
}

[System.Serializable]
public class CastsoulRewardRef
{
    /// <summary>
    /// ID
    /// </summary>
    public int id;
    /// <summary>
    /// 需要铸魂的次数
    /// </summary>
    public int num;
    /// <summary>
    /// 奖励物品
    /// </summary>
    public ItemValue reward;
}
