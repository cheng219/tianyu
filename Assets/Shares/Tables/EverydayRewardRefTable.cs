//=====================
//作者：黄洪兴
//日期：2016/1/25
//用途：每日奖励
//=====================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EverydayRewardRefTable : AssetTable
{
	public List<EverydayRewardRef> infoList = new List<EverydayRewardRef>();
}


[System.Serializable]
public class EverydayRewardRef
{
	/// <summary>
	/// id
	/// </summary>
	public int id;
	/// <summary>
	/// 奖励
	/// </summary>
	public List<ItemValue> item=new List<ItemValue>();

	public int vip;
	/// <summary>
	/// 翻倍，没有翻倍则为0
	/// </summary>
	public int times;
    /// <summary>
    /// 第几天
    /// </summary>
    public string des;
    /// <summary>
    /// 等于1为特殊奖励
    /// </summary>
    public int special;
}
