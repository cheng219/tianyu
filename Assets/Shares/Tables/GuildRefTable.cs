//=========================
//作者：黄洪兴
//日期：2016/04/14
//用途：公会静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuildRefTable : AssetTable
{
	public List<GuildRef> infoList = new List<GuildRef>();
}


[System.Serializable]
public class GuildRef
{
	/// <summary>
	/// 公会等级
	/// </summary>
	public	int lev;
	/// <summary>
	/// 公会升到下一及需要的经验
	/// </summary>
	public int exp;
	/// <summary>
	/// 公会成员上限
	/// </summary>
	public int num;
	/// <summary>
	/// 当前等级的俸禄
	/// </summary>
	public int welfare;



}