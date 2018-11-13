//=========================
//作者：黄洪兴
//日期：2016/04/19
//用途：修行消耗静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StyliteMoneyRefTable : AssetTable
{
	public List<StyliteMoneyRef> infoList = new List<StyliteMoneyRef>();
}


[System.Serializable]
public class StyliteMoneyRef
{
	/// <summary>
	///次数
	/// </summary>
	public	int time;
	/// <summary>
	/// 金钱
	/// </summary>
	public int money;
	/// <summary>
	/// 元宝
	/// </summary>
	public int specialMoney;



}