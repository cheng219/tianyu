//=========================
//作者：黄洪兴
//日期：2016/04/6
//用途：幸运继承静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InheritLuckyRefTable : AssetTable
{
	public List<InheritLuckyRef> infoList = new List<InheritLuckyRef>();
}


[System.Serializable]
public class InheritLuckyRef
{
	/// <summary>
	/// ID
	/// </summary>
	public	int id;
	/// <summary>
	/// 幸运值
	/// </summary>
	public	int lev;
	/// <summary>
	/// 消耗的物品
	/// </summary>
	public List<ItemValue> consumeItem;



}