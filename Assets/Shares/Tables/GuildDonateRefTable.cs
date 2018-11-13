//=========================
//作者：朱素云
//日期：2017/5/4
//用途：仙盟捐献配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuildDonateRefTable : AssetTable
{
    public List<GuildDonateRef> infoList = new List<GuildDonateRef>();
}


[System.Serializable]
public class GuildDonateRef
{
    /// <summary>
    /// id
    /// </summary>
    public int id; 
    /// <summary>
    /// 花费
    /// </summary>
    public List<ItemValue> cost = new List<ItemValue>(); 
    /// <summary>
    /// 奖励
    /// </summary>
    public List<ItemValue> reward = new List<ItemValue>();
    /// <summary>
    ///捐献名称
    /// </summary>
    public string donationName;
    /// <summary>
    /// 描述
    /// </summary>
    public string des;

}