//=========================
//作者：黄洪兴
//日期：2016/05/10
//用途：首冲奖励静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FirstChargeRefTable : AssetTable
{
    public List<FirstChargeRef> infoList = new List<FirstChargeRef>();
}


[System.Serializable]
public class FirstChargeRef
{
    /// <summary>
    /// 职业ID
    /// </summary>
    public int prof;
    /// <summary>
    /// 奖励ID
    /// </summary>
    public List<ItemValue> reward = new List<ItemValue>();



    /// <summary>
    /// 左边模型名称
    /// </summary>
    public string lModel;
    /// <summary>
    /// 右边模型名称
    /// </summary>
    public string rModel;



}


