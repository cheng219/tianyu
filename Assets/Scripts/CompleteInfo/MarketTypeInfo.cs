//====================================================
//作者: 黄洪兴
//日期：2016/4/19
//用途：市场大分页数据层对象
//======================================================




using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MarketTypeServerData
{
	public int id;

}

/// <summary>
/// 市场大分页数据层对象 
/// </summary>
public class MarketTypeInfo 
{
	#region 服务端数据 
	MarketTypeServerData marketTypeData;
	#endregion

	#region 静态配置数据 
	MarketRef marketTypeRef = null;
	public MarketRef MarketTypeRef
	{
		get
		{
			if (marketTypeRef != null) return marketTypeRef;
			marketTypeRef = ConfigMng.Instance.GetMarketTypeRef(marketTypeData.id);
			return marketTypeRef;
		}
	}
	#region 构造 
	public MarketTypeInfo(int _id)
	{
		marketTypeData = new MarketTypeServerData ();
		marketTypeData.id = _id;

	}
		
	#endregion

	#region 访问器
	/// <summary>
	/// ID
	/// </summary>
	public int ID
	{	
		get { return marketTypeData.id; }
	}
	/// <summary>
	/// 大分页使用字段中的哪个内容
	/// </summary>
	public List<string> Tabmessage
	{
		get
		{
			return MarketTypeRef.Tabmessage;
		}
	}
	/// <summary>
	/// 大分页的名字
	/// </summary>
	public string Tabname
	{
		get
		{
			return MarketTypeRef.Tabname;
		}
	}
	/// <summary>
	/// 此大分页中的小分页使用物品表哪个字段
	/// </summary>
	public string Page
	{
		get {
			return MarketTypeRef.Page;
		}

	}

	/// <summary>
	/// 此大分页中的小分页使用物品表哪个字段
	/// </summary>
	public List<string> Pagemessage
	{
		get {
			return MarketTypeRef.Paessage;
		}

	}

	/// <summary>
	/// 此大分页中的小分页的名字
	/// </summary>
	public List<string> Pagename
	{
		get {
			return MarketTypeRef.Pagename;
		}

	}
	/// <summary>
	/// 是否显示小分页
	/// </summary>
	public bool ShowPage=false;



	#endregion
	#endregion
}
