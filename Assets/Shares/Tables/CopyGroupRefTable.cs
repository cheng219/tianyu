//=========================
//作者：黄洪兴
//日期：2016/4/7
//用途：副本组静态配置表
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CopyGroupRefTable : AssetTable
{
	public List<CopyGroupRef> infoList = new List<CopyGroupRef>();
}

[System.Serializable]
public class CopyGroupRef
{

	/// <summary>
	/// 副本组ID
	/// </summary>
	public int id;
	/// <summary>
	/// 开启所需等级索引
	/// </summary>
	public int lv;
	/// <summary>
	/// 副本组类型
	/// </summary>
	public int sort;
	/// <summary>
	/// 进入次数
	/// </summary>
	public int num;
	/// <summary>
	/// 奖励
	/// </summary>
	public List<int> reward=new List<int> ();
	/// <summary>
	/// 背景图
	/// </summary>
	public string icon;
	/// <summary>
	/// 所属副本
	/// </summary>
	public List<int> copy=new List<int>();
	/// <summary>
	/// 副本组名字
	/// </summary>
	public string name;


}
