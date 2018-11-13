//=========================
//作者：黄洪兴
//日期：2016/04/5
//用途：装备洗练属性值静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentWashValueRefTable : AssetTable
{
	public List<EquipmentWashValueRef> infoList = new List<EquipmentWashValueRef>();
}


[System.Serializable]
public class EquipmentWashValueRef
{
	/// <summary>
	/// id
	/// </summary>
	public	int id;
	/// <summary>
	/// 属性类型
	/// </summary>
	public int att_type; 
	/// <summary>
	/// 属性品质
	/// </summary>
	public int att_quality; 
	/// <summary>
	/// 出现几率
	/// </summary>
	public int probability; 
	/// <summary>
	/// 属性具体值
	/// </summary>
	public int value; 
	/// <summary>
	/// 属性增加的战斗力
	/// </summary>
	public int gs; 
	/// <summary>
	/// 继承时相应等级需要消耗的物品ID
	/// </summary>
	public List<int> inh_cons;
	/// <summary>
	/// 继承时相应等级需要消耗的物品数量
	/// </summary>
	public List<int> inh_cons_num;
	/// <summary>
	/// 继承需要消耗的物品集合
	/// </summary>
	/// <value>The items.</value>
	public List<ItemValue> Items=new List<ItemValue>();




}