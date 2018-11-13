//=========================
//作者：黄洪兴
//日期：2016/03/29
//用途：强化属性静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttributeRefTable : AssetTable
{
	public List<AttributeRef> infoList = new List<AttributeRef>();
}


[System.Serializable]
public class AttributeRef
{
	public	int id;
	/// <summary>
	/// 转生
	/// </summary>
	public int reborn;
	/// <summary>
	/// 显示等级
	/// </summary>
	public int display_level;
	/// <summary>
	/// 最大经验
	/// </summary>
	public long max_exp;
	/// <summary>
	///升级方式
	/// </summary>
	public int upgrade_type;
	/// <summary>
	/// 移速
	/// </summary>
	public int moveSpd;


}