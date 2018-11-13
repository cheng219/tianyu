//=========================
//作者：黄洪兴
//日期：2016/4/7
//用途：副本静态配置表
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CopyRefTable : AssetTable
{
	public List<CopyRef> infoList = new List<CopyRef>();
}

[System.Serializable]
public class CopyRef
{

	/// <summary>
	/// 副本ID
	/// </summary>
	public int id;
	/// <summary>
	/// 战力
	/// </summary>
	public int fighting;
	/// <summary>
	/// 副本难度文字
	/// </summary>
	public string difficulty;
	/// <summary>
	/// 需求等级
	/// </summary>
	public int lvId;
    /// <summary>
    /// 副本组
    /// </summary>
    public int copyGroup;
    /// <summary>
    /// 副本图片
    /// </summary>
    public string Icon;
    /// <summary>
    /// 一个非化身队友额外奖励
    /// </summary>
    public List<ItemValue> reward1 = new List<ItemValue>();
    /// <summary>
    /// 两个非化身队友额外奖励
    /// </summary>
    public List<ItemValue> reward2 = new List<ItemValue>();

}
