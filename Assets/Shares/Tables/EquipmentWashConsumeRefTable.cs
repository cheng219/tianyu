//=========================
//作者：黄洪兴
//日期：2016/04/5
//用途：装备洗练消耗静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentWashConsumeRefTable : AssetTable
{
	public List<EquipmentWashConsumeRef> infoList = new List<EquipmentWashConsumeRef>();
}


[System.Serializable]
public class EquipmentWashConsumeRef
{
	public int id;
	/// <summary>
	/// 装备的品质
	/// </summary>
	public	int quality;
	/// <summary>
	/// 可洗练出的属性数量
	/// </summary>
	public int attr_num; 
	/// <summary>
	/// 消耗物品的ID
	/// </summary>
	public List<int> consume_ID=new List<int> (); 
	/// <summary>
	/// 消耗物品的数量
	/// </summary>
	public List<int> consume_num=new List<int> (); 
	/// <summary>
	/// 消耗物品的ID
	/// </summary>
	public List<int> locking_ID=new List<int> (); 
	/// <summary>
	/// 消耗物品的ID
	/// </summary>
	public List<int> locking_num=new List<int> (); 
	/// <summary>
	/// 消耗的物品
	/// </summary>
	public List<ItemValue> consumeItem;
	/// <summary>
	/// 锁定消耗的物品
	/// </summary>
	public List<ItemValue> lockingConsumeItem;
	public int exp;



}