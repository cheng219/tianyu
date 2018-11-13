//=========================
//作者：黄洪兴
//日期：2016/04/14
//用途：公会技能静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuildSkillRefTable : AssetTable
{
	public List<GuildSkillRef> infoList = new List<GuildSkillRef>();
}


[System.Serializable]
public class GuildSkillRef
{
	/// <summary>
	///技能id
	/// </summary>
	public	int id;
	/// <summary>
	/// 技能名字
	/// </summary>
	public string name;
	/// <summary>
	/// 技能的等级
	/// </summary>
	public int lev;
	/// <summary>
	/// 技能增加的属性
	/// </summary>
	public List<AttributePair> attrs=new List<AttributePair>();
	/// <summary>
	/// 公会前提等级
	/// </summary>
	public int need1;
	/// <summary>
	/// 贡献度前提
	/// </summary>
	public int need2;
	/// <summary>
	/// 升级的消耗
	/// </summary>
	public int cost;
	/// <summary>
	/// 文本描述
	/// </summary>
	public string des;
	/// <summary>
	/// 技能的图标
	/// </summary>
	public string icon;
	/// <summary>
	/// 每级增加的战力
	/// </summary>
	public int gs;






}