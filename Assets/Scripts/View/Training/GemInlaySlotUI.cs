//==============================================
//作者：邓成
//日期：2016/3/29
//用途：装备镶嵌槽位的界面类
//==============================================

using UnityEngine;
using System.Collections;

public class GemInlaySlotUI : MonoBehaviour {
	public ItemUI gemItem;
	public UILabel unlockDes;
	public UISprite canInlayIcon;
	public UILabel exLabelDes;

	/// <summary>
	/// Sets the data.
	/// </summary>
	public void SetData(st.net.NetBase.pos_des gemState)
	{
		EquipmentInfo info = null;
		if(gemState.type != 0)
		{
			info = new EquipmentInfo(gemState,EquipmentBelongTo.EQUIP);//镶嵌到装备上的宝石从属于装备
			if(exLabelDes != null)exLabelDes.enabled = false;
		}else
		{
			if(canInlayIcon != null)canInlayIcon.enabled = true;
			if(exLabelDes != null)exLabelDes.enabled = true;
		}
		if(gemItem != null)
		{
			gemItem.gameObject.SetActive(true);
			gemItem.FillInfo(info);
		}
		if(unlockDes != null)unlockDes.enabled = false;
	}
	public void SetEmpty()
	{
		if(gemItem != null)
		{
			gemItem.FillInfo(null);
			gemItem.gameObject.SetActive(false);
		}
		if(unlockDes != null)unlockDes.enabled = true;
		if(canInlayIcon != null)canInlayIcon.enabled = false;
		if(exLabelDes != null)exLabelDes.enabled = true;
	}
}
