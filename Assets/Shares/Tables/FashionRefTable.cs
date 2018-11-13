//=========================
//作者：黄洪兴
//日期：2016/3/19
//用途：时装静态配置表
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FashionRefTable : AssetTable
{
	public List<FashionRef> infoList = new List<FashionRef>();
}

[System.Serializable]
public class FashionRef
{


	public int id;
	public int type;
	public string name;
	public string des;
	public string assetName;
	public int item;
	public List<ItemValue> attribute=new List<ItemValue>();
	public int time;
	public List<ItemValue> updataItem=new List<ItemValue>();
	public int gs;
	public int getType;
    public int shopId;
    public int prof;
    public int tempId;
    public int useItemNum;
    public int shopType;
    public int forever_temp_id;
	//	public 	List<AttributePair> attrs = new List<AttributePair> ();


}
