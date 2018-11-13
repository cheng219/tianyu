//====================================================
//作者：邓成
//日期：2016/3/3
//用途：背包物品在其他界面的显示类(分解、合成)
//======================================================

using UnityEngine;
using System.Collections;

public class DecomposeItemUI : MonoBehaviour {
	public ItemUI itemUI;
	public UIToggle toggleChoose;
	public GameObject redTip;

	protected EquipmentInfo CurEquipmentInfo = null;
	protected SubGUIType CurSubGUIType = SubGUIType.NONE;
	/// <summary>
	/// 返回是否显示红点提示
	/// </summary>
	public void SetData(EquipmentInfo equipmentInfo,UIEventListener.VoidDelegate clickEvent,SubGUIType subGUIType = SubGUIType.NONE)
	{		
		if(CurEquipmentInfo != null)//重复SetData,去掉前一个物品的委托
		{
			CurEquipmentInfo.OnPropertyUpdate -= OnPropertyUpdate;
		}
		CurSubGUIType = subGUIType;
		CurEquipmentInfo = equipmentInfo;
		if(itemUI != null)
			itemUI.FillInfo(new EquipmentInfo(equipmentInfo,EquipmentBelongTo.PREVIEW));
		UIEventListener.Get(gameObject).onClick = clickEvent;
		UIEventListener.Get(gameObject).parameter = equipmentInfo;
		if(CurEquipmentInfo != null)
		{
			CurEquipmentInfo.OnPropertyUpdate += OnPropertyUpdate;
		}
		if(toggleChoose != null)toggleChoose.value = false;
		ShowRedTip();
	}
	public void ShowRedTip()
	{
		bool showRedTip = false;
		switch(CurSubGUIType)
		{
		case SubGUIType.STRENGTHING:
			showRedTip = CurEquipmentInfo.RealCanStrength;
			break;
		case SubGUIType.EQUIPMENTUPGRADE:
			showRedTip = CurEquipmentInfo.RealCanUpgrade;
			break;
		case SubGUIType.EQUIPMENTWASH:
			showRedTip = CurEquipmentInfo.RealCanWash;
			break;
		case SubGUIType.ORANGEREFINE:
			showRedTip = CurEquipmentInfo.RealCanOrangeRefine;
			break;
        case SubGUIType.MOUNTEQUIP:
            showRedTip = CurEquipmentInfo.RealCanStrength || CurEquipmentInfo.RealCanUpgrade;
            break;
		default:
			break;
		}
		if(redTip != null)redTip.SetActive(showRedTip);
	}
	public void SetChecked()
	{
        if (CurSubGUIType == SubGUIType.MOUNTEQUIP)
            GameCenter.newMountMng.CurSelectEquipmentInfo = CurEquipmentInfo;
        else
            GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo = CurEquipmentInfo;
		if(toggleChoose != null)toggleChoose.value = true;
	}

	void OnDisable()
	{
		if(CurEquipmentInfo != null)
		{
			CurEquipmentInfo.OnPropertyUpdate -= OnPropertyUpdate;
		}
		CurEquipmentInfo = null;
		UIEventListener.Get(gameObject).onClick = null;
		UIEventListener.Get(gameObject).parameter = null;
	}
	void OnDestroy()
	{
		if(CurEquipmentInfo != null)
		{
			CurEquipmentInfo.OnPropertyUpdate -= OnPropertyUpdate;
		}
		CurEquipmentInfo = null;
		UIEventListener.Get(gameObject).onClick = null;
		UIEventListener.Get(gameObject).parameter = null;
	}
	void OnPropertyUpdate()
	{
		if(itemUI != null)
			itemUI.FillInfo(new EquipmentInfo(CurEquipmentInfo,EquipmentBelongTo.PREVIEW));
		UIEventListener.Get(gameObject).parameter = CurEquipmentInfo;
		ShowRedTip();
	}

	public DecomposeItemUI CreateNew(Transform _parent)
	{
		GameObject obj = Instantiate(this.gameObject) as GameObject;
		obj.transform.parent = _parent;
		obj.transform.localScale = Vector3.one;
		obj.transform.localPosition = Vector3.zero;
		return obj.GetComponent<DecomposeItemUI>();
	}
}
