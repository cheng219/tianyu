//=========================
//作者：黄洪兴
//日期：2016/04/1
//用途：铸魂静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CastSoulRefTable : AssetTable
{
	public List<CastSoulRef> infoList = new List<CastSoulRef>();
}


[System.Serializable]
public class CastSoulRef
{
	public	int id;
	/// <summary>
	/// 魂类型
	/// </summary>
	public	int type;
	/// <summary>
	/// 普通铸魂获得的物品
	/// </summary>
	public List<ItemValue> normalItem=new List<ItemValue> ();
	/// <summary>
	/// 高级铸魂获得的物品
	/// </summary>
	public List<ItemValue> highItem=new List<ItemValue> ();



}