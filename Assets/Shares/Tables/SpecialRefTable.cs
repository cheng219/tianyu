//=========================
//作者：黄洪兴
//日期：2016/04/13
//用途：特殊字段静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpecialRefTable : AssetTable
{
	public List<SpecialRef> infoList = new List<SpecialRef>();
}


[System.Serializable]
public class SpecialRef
{
	/// <summary>
	/// ID
	/// </summary>
	public	int id;
	/// <summary>
	/// 角色背包初始格子数
	/// </summary>
	public int bag_init_space;
	/// <summary>
	/// 解锁背包对应消耗的元宝数
	/// </summary>
	public int bag_time_diamo;
	/// <summary>
	/// 传送一次花费的金额
	/// </summary>
	public ItemValue fly_coin_num=new ItemValue();
	/// <summary>
	/// 获得宠物技能所消耗的物品ID
	/// </summary>
	public int petSkillItem;
	/// <summary>
	/// 送花界面三个花的id
	/// </summary>
	public List<int> flowers=new List<int>();
	/// <summary>
	/// 传送到仇人身边消耗的元宝
	/// </summary>
	public int Enemy_transport;










}