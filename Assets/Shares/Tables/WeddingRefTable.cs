//=========================
//作者：黄洪兴
//日期：2016/04/14
//用途：仙侣静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeddingRefTable : AssetTable
{
	public List<WeddingRef> infoList = new List<WeddingRef>();
}


[System.Serializable]
public class WeddingRef
{
	/// <summary>
	/// ID
	/// </summary>
	public	int id;
	/// <summary>
	/// 婚礼名称
	/// </summary>
	public	string name;
	/// <summary>
	/// 婚礼名称的图片名
	/// </summary>
	public	string pic;
	/// <summary>
	/// 信物id
	/// </summary>
	public int token_id;
	/// <summary>
	/// 消耗的货币类型id
	/// </summary>
	public int consume;
	/// <summary>
	/// 消耗的货币数量
	/// </summary>
	public int consume_num;
	/// <summary>
	/// 信物的属性
	/// </summary>
	public List<AttributePair> attrs;




}