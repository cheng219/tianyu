//=========================
//作者：黄洪兴
//日期：2016/04/6
//用途：橙炼静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrangeRefineRefTable : AssetTable
{
	public List<OrangeRefineRef> infoList = new List<OrangeRefineRef>();
}


[System.Serializable]
public class OrangeRefineRef
{
	/// <summary>
	/// ID
	/// </summary>
	public	int id;
	/// <summary>
	/// 当前装备ID
	/// </summary>
	public	int start_item;
	/// <summary>
	/// 装备对应的职业
	/// </summary>
	public	int prof;
	/// <summary>
	/// 升阶消耗的物品
	/// </summary>
	public List<ItemValue> materialItem;
	/// <summary>
	/// 升阶后获得的物品ID
	/// </summary>
	public int end_item;
    public long coin;




}