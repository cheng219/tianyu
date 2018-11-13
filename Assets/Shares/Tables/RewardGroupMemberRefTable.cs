//=========================
//作者：黄洪兴
//日期：2016/04/5
//用途：藏宝阁奖励成员静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RewardGroupMemberRefTable : AssetTable
{
	public List<RewardGroupMemberRef> infoList = new List<RewardGroupMemberRef>();
}


[System.Serializable]
public class RewardGroupMemberRef
{
	/// <summary>
	/// ID
	/// </summary>
	public	int id;
	/// <summary>
	/// 显示名，填0时不显示名字
	/// </summary>
	public	string name;
	/// <summary>
	/// 包含的物品ID
	/// </summary>
	public	List<int> item;




}