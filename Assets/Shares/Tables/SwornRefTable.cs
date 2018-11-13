//=========================
//作者：黄洪兴
//日期：2016/04/15
//用途：结义静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwornRefTable : AssetTable
{
	public List<SwornRef> infoList = new List<SwornRef>();
}


[System.Serializable]
public class SwornRef
{
	/// <summary>
	///id
	/// </summary>
	public	int id;
	/// <summary>
	/// 升到该等级需要的情义值
	/// </summary>
	public int friend_ship;
	/// <summary>
	/// 情义等级显示的图片
	/// </summary>
	public string pic;

	/// <summary>
	/// 组队经验加成
	/// </summary>
	public int ranks_exp;

	/// <summary>
	/// 技能增加的属性
	/// </summary>
	public List<AttributePair> attrs=new List<AttributePair>();
	/// <summary>
	/// 第一个宝箱奖励
	/// </summary>
	public List<ItemValue> reward1;
	/// <summary>
	/// 第二个宝箱奖励
	/// </summary>
	public List<ItemValue> reward2;
	/// <summary>
	/// 第三个宝箱奖励
	/// </summary>
	public List<ItemValue> reward3;





}