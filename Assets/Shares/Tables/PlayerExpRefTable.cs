//==============================
//作者：龙英杰
//日期：2015/11/02
//用途：角色升级所需经验静态配置表
//==============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerExpRefTable : AssetTable
{
    public List<PlayerExpRef> infoList = new List<PlayerExpRef>();
}


[System.Serializable]
public class PlayerExpRef
{

    /// <summary>
    /// 等级
    /// </summary>
    public int level;
    /// <summary>
    /// 所需碎片量
    /// </summary>
    public ulong exp;
    /// <summary>
    /// 每日奖励宝箱的掉落ID
    /// </summary>
    public int dailyReward;
    /// <summary>
    /// 每日经验
    /// </summary>
    public int dailyExp;
    /// <summary>
    /// 每日金币
    /// </summary>
    public int dailyCoin;
    /// <summary>
    /// 悬赏重置的花费数
    /// </summary>
    public int resetDailyCost;

}
