//=========================
//作者：黄洪兴
//日期：2016/04/5
//用途：藏宝阁奖励静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RewardGroupRefTable : AssetTable
{
	public List<RewardGroupRef> infoList = new List<RewardGroupRef>();
}


[System.Serializable]
public class RewardGroupRef
{
	/// <summary>
	/// ID
	/// </summary>
	public	int id;
	/// <summary>
	/// 组名
	/// </summary>
	public	string name;
	/// <summary>
	/// 所属成员
	/// </summary>
	public	List<int> memberId;
	/// <summary>
	/// 职业ID
	/// </summary>
	public int occupation;




}