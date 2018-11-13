//=========================
//作者：黄洪兴
//日期：2016/3/8
//用途：合成静态配置表
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlendRefTable : AssetTable
{
	public List<BlendRef> infoList = new List<BlendRef>();
}

[System.Serializable]
public class BlendRef
{
	public int id;
	public int sort;
	public string need_items_type;
	public string need_items_num;
	public string end_items_type;
	public string end_items_num;

	public List<ItemValue> needItems = new List<ItemValue> ();
	public List<ItemValue> itemsEnd=new List<ItemValue> ();

}
