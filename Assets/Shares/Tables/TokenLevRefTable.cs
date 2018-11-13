//=========================
//作者：黄洪兴
//日期：2016/04/14
//用途：仙侣信物静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TokenLevRefTable : AssetTable
{
	public List<TokenLevRef> infoList = new List<TokenLevRef>();
}


[System.Serializable]
public class TokenLevRef
{
	/// <summary>
	/// ID
	/// </summary>
	public	int id;
	/// <summary>
	/// 升到该等级需要的经验
	/// </summary>
	public	int exp;
	/// <summary>
	/// 消耗的物品
	/// </summary>
	public List<ItemValue> consume=new List<ItemValue>();
	/// <summary>
	/// 信物的属性
	/// </summary>
	public List<AttributePair> attrs=new List<AttributePair>();




}