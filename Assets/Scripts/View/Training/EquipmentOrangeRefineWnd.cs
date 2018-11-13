//==============================================
//作者：邓成
//日期：2016/3/29
//用途：装备橙炼界面类
//==============================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentOrangeRefineWnd : SubWnd {
	public ItemUI curItemUI;
	public ItemUI targetItemUI;
	public UILabel curAttr;
	public UILabel targetAttr;
	public UILabel labStone;
	public UILabel labCoin;
	public UIButton btnRefine;

	public UIToggle toggleQuickBuy;
	public UILabel quickCost;
	void Start()
	{
		if(btnRefine != null)UIEventListener.Get(btnRefine.gameObject).onClick = OrangeRefine; 
	}
	protected override void OnOpen ()
	{
		base.OnOpen ();
        if (toggleQuickBuy != null) EventDelegate.Add(toggleQuickBuy.onChange, OnChooseQuickBuy);
	}
	protected override void OnClose ()
	{
		base.OnClose ();
        if (toggleQuickBuy != null) EventDelegate.Remove(toggleQuickBuy.onChange, OnChooseQuickBuy);
	}
	protected override void HandEvent (bool _bind)
	{
		base.HandEvent (_bind);
		if(_bind)
		{
			GameCenter.equipmentTrainingMng.OnSelectEquipmentUpdate += ShowItemInfo;
			GameCenter.equipmentTrainingMng.OnQuickBuyCostUpdateEvent += OnChooseQuickBuy;
			GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += OnBaseUpdate;
		}else
		{
			GameCenter.equipmentTrainingMng.OnSelectEquipmentUpdate -= ShowItemInfo;
			GameCenter.equipmentTrainingMng.OnQuickBuyCostUpdateEvent -= OnChooseQuickBuy;
			GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= OnBaseUpdate;
		}
	}
	bool enough = true;
	bool enoughMaterial = true;
	bool enoughCoin = true;
	protected EquipmentInfo orangeCostItem = null;
	List<EquipmentInfo> lackEquipList = new List<EquipmentInfo>();
	void ShowItemInfo()
	{
		lackEquipList.Clear();
		enough = true;
		enoughMaterial = true;
		enoughCoin = true;
		EquipmentInfo equip = GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo == null?null:new EquipmentInfo(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo,EquipmentBelongTo.PREVIEW);
		if(curItemUI != null)curItemUI.FillInfo(equip);
		if(equip != null)
		{
			if(curAttr != null)curAttr.text = GameHelper.GetAttributeNameAndValue(equip.AttributePairList);
			OrangeRefineRef orangeRefineRef = ConfigMng.Instance.GetOrangeRefineRefByEid(equip.EID);
			if(orangeRefineRef != null)
			{
				EquipmentInfo info = new EquipmentInfo(equip,orangeRefineRef.end_item);
				if(targetItemUI != null)targetItemUI.FillInfo(info);
				if(targetAttr != null)targetAttr.text = GameHelper.GetAttributeNameAndValue(info.AttributePairList);
				EquipmentInfo lackEquip = null;
				System.Text.StringBuilder builder = new System.Text.StringBuilder();
				for (int i = 0,max=orangeRefineRef.materialItem.Count; i < max; i++) 
				{
					string str = GameHelper.GetStringWithBagNumber(orangeRefineRef.materialItem[i],out enough,out orangeCostItem);
					builder.Append(str);
					if(enough == false)
					{
						enoughMaterial = enough;
					}
					builder.Append("\n");
				}
				ItemValue coinItem = new ItemValue(5,(int)orangeRefineRef.coin);
				string coinStr = GameHelper.GetStringWithBagNumber(coinItem,out enoughCoin,out lackEquip);
				builder.Append(coinStr);
				if(lackEquip != null)
					lackEquipList.Add(lackEquip);
				if(labStone != null)labStone.text = builder.ToString();
			}else
			{
			//	GameCenter.messageMng.AddClientMsg("选中装备无法橙炼!");
			}
			GameCenter.equipmentTrainingMng.quickBuyList = lackEquipList;
		}else
		{
			if(targetItemUI != null)targetItemUI.FillInfo(null);
			if(curAttr != null)curAttr.text = string.Empty;
			if(targetAttr != null)targetAttr.text = string.Empty;
			if(labStone != null)labStone.text = string.Empty;
		}
	}
	void OrangeRefine(GameObject go)
	{
		GameCenter.equipmentTrainingMng.ChooseQuickBuy = toggleQuickBuy.value;
		if(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo == null)
		{
			GameCenter.messageMng.AddClientMsg(ConfigMng.Instance.GetUItext(353));
			return;
		}
		if(!enoughMaterial)
		{
			MessageST mst = new MessageST();
			mst.messID = 216;
			mst.words = new string[1]{orangeCostItem == null?ConfigMng.Instance.GetUItext(354):orangeCostItem.ItemName};
			GameCenter.messageMng.AddClientMsg(mst);
			return;
		}
		if (!enoughCoin && !GameCenter.equipmentTrainingMng.ChooseQuickBuy)
		{
			GameCenter.messageMng.AddClientMsg(155);
			return;
		}
		if(GameCenter.equipmentTrainingMng.ChooseQuickBuy && !enoughCoin)
		{
			//快捷购买
			GameCenter.equipmentTrainingMng.C2S_UpgradeEquip(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID,2,true);
			return;
		}
		//正常购买
		GameCenter.equipmentTrainingMng.C2S_UpgradeEquip(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID,2,false);
	}

	void OnChooseQuickBuy()
	{
		GameCenter.equipmentTrainingMng.ChooseQuickBuy = toggleQuickBuy.value;
        bool redColor = (ulong)GameCenter.equipmentTrainingMng.QuickBuyCost > GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount;
		System.Text.StringBuilder builder = new System.Text.StringBuilder();
		if(quickCost != null)
            quickCost.text = builder.Append(GameCenter.equipmentTrainingMng.QuickBuyCost).Append((redColor ? "/[ff0000]" : "/[00ff00]")).Append(GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount).Append("[-]").ToString();
	}

	void OnBaseUpdate(ActorBaseTag tag,ulong val,bool state)
	{
		if(tag == ActorBaseTag.Diamond)
		{
			OnChooseQuickBuy();
		}
	}
}
