//=========================
//作者：李邵南
//日期：2017/04/17
//用途：七日挑战奖励静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SevenDaysTaskRewardRefTable : AssetTable
{
    public List<SevenDaysTaskRewardRef> infoList = new List<SevenDaysTaskRewardRef>();
}

[System.Serializable]
public class SevenDaysTaskRewardRef
{
    /// <summary>
    /// 自增长ID
    /// </summary>
    public int id;
    /// <summary>
    /// 天数
    /// </summary>
    public int day;
    /// <summary>
    /// 奖励物品信息
    /// </summary>
    public List<int> reward = new List<int>();
    /// <summary>
    /// 奖励物品数量
    /// </summary>
    public List<int> rewardnum = new List<int>();
    /// <summary>
    /// 标题
    /// </summary>
    public string des1;
    /// <summary>
    /// 描述文本
    /// </summary>
    public string des2;
    /// <summary>
    /// 图片名称
    /// </summary>
    public string Pic;
    /// <summary>
    /// 显示奖励
    /// </summary>
    public int showreward;
}