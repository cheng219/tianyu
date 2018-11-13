//==========================
//作者：龙英杰
//日期：2015/12/12
//用途：公会捐献表静态配置
//==========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DonationRefTable : AssetTable
{
    public List<DonationRef> infoList = new List<DonationRef>();
}

[System.Serializable]
public class DonationRef
{
    /// <summary>
    /// id
    /// </summary>
    public int id;
    /// <summary>
    /// 捐献类型
    /// </summary>
    public int donationItem;
    /// <summary>
    /// 捐献值
    /// </summary>
    public int donationVal;
    /// <summary>
    /// 公会资源数量
    /// </summary>
    public int guildResNum;
    /// <summary>
    /// 捐献奖励的数量
    /// </summary>
    public int number;
    /// <summary>
    /// 名字
    /// </summary>
    public string donationName;

}

