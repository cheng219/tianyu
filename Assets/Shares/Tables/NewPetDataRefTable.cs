//=========================
//作者：黄洪兴
//日期：2016/03/05
//用途：宠物成长资质灵修配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewPetDataRefTable : AssetTable
{
	public List<NewPetDataRef> infoList = new List<NewPetDataRef>();
}


[System.Serializable]
public class NewPetDataRef
{
	/// <summary>
	/// 级别
	/// </summary>
	public	int level;
	/// <summary>
	/// 增加的属性
	/// </summary>
	public List<ItemValue> petLevel=new List<ItemValue>();
	/// <summary>
	///战力
	/// </summary>
	public int lvGs;
	/// <summary>
	/// 成长属性
	/// </summary>
	public List<ItemValue> chengZhang=new List<ItemValue>();
	/// <summary>
	///成长经验
	/// </summary>
	public int cZExp;
	/// <summary>
	/// 成长物品
	/// </summary>
	public List<ItemValue> cZIem=new List<ItemValue>();
	/// <summary>
	/// 提升成长经验倍率的概率
	/// </summary>
	public List<int> cZChance=new List<int>();
   /// <summary>
   /// 成长称号
   /// </summary>
	public int cZTitle;
	/// <summary>
	/// 成长战力
	/// </summary>
	public int cZGs;
	/// <summary>
	/// 守护时宠物百分比加成
	/// </summary>
	public int zhiZi;
	/// <summary>
	/// 资质直接增加的属性
	/// </summary>
	public List<ItemValue> zZlevel=new List<ItemValue>();

	/// <summary>
	/// 资质经验
	/// </summary>
	public int zZExp;
	/// <summary>
	///资质物品
	/// </summary>
	public List<ItemValue>  zZItem=new List<ItemValue>();
	/// <summary>
	/// 资质战力
	/// </summary>
	public int zZGs;

	/// <summary>
	/// 灵修的倍率
	/// </summary>
	public List<int> lXChance=new List<int>();
	/// <summary>
	/// 灵修经验
	/// </summary>
	public List<int> lXExp=new List<int>();
	/// <summary>
	/// 灵修物品
	/// </summary>
	public List<ItemValue> lXItem=new List<ItemValue>();
	/// <summary>
	/// 灵修战力
	/// </summary>
	public int lXGs;

}