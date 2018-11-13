//=========================
//作者：黄洪兴
//日期：2016/04/5
//用途：藏宝阁静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreasureHouseRefTable : AssetTable
{
	public List<TreasureHouseRef> infoList = new List<TreasureHouseRef>();
}


[System.Serializable]
public class TreasureHouseRef
{
    /// <summary>
    /// 宝箱ID
    /// </summary>
	public	int chest;
	/// <summary>
	/// 抽一次需要的元宝
	/// </summary>
	public	int extract1;
	/// <summary>
	/// 抽十次需要的元宝
	/// </summary>
	public	int extract10;
	/// <summary>
	/// 抽五次需要的元宝
	/// </summary>
	public	int extract50;
	/// <summary>
	/// 小奖励显示物品ID
	/// </summary>
	public	List<int> smallAward;
	/// <summary>
	/// 大奖励显示物品ID
	/// </summary>
	public	List<int> bigAward;
	/// <summary>
	/// 钥匙ID
	/// </summary>
	public	int keyID;

    public List<int> bigAward1 = new List<int>();


    public List<int> bigAward2 = new List<int>();



}