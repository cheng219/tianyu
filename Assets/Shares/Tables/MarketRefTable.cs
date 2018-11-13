//=========================
//作者：黄洪兴
//日期：2016/04/18
//用途：市场大分页态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MarketRefTable : AssetTable
{
	public List<MarketRef> infoList = new List<MarketRef>();
}


[System.Serializable]
public class MarketRef
{
	/// <summary>
	///id
	/// </summary>
	public	int id;
	/// <summary>
	/// 使用该字段的什么内容
	/// </summary>
	public List<string> Tabmessage=new List<string>();
	/// <summary>
	/// 大分页的名字
	/// </summary>
	public string Tabname;
	/// <summary>
	/// 此大分页中的小分页使用的字段
	/// </summary>
	public string Page;
	/// <summary>
	/// 小分页使用的字段中的内容
	/// </summary>
    public List<string> Paessage = new List<string>();
	/// <summary>
	/// 小分页对应的中文名字
	/// </summary>
	public List<string> Pagename=new List<string> ();







}