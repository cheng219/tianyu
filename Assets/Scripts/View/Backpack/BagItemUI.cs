//====================================================
//作者：邓成
//日期：2016/3/3
//用途：背包物品在其他界面的显示类(分解、合成)
//======================================================

using UnityEngine;
using System.Collections;

public class BagItemUI : MonoBehaviour {
	public ItemUI itemUI;

	public void SetData(EquipmentInfo equipmentInfo,UIEventListener.VoidDelegate clickEvent)
	{
		if(itemUI != null)
			itemUI.FillInfo(equipmentInfo);
		UIEventListener.Get(gameObject).onClick = clickEvent;
		UIEventListener.Get(gameObject).parameter = equipmentInfo;
	}
}
