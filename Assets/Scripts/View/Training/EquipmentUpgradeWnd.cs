//==============================================
//作者：邓成
//日期：2016/3/29
//用途：装备升阶界面类
//==============================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentUpgradeWnd : SubWnd {
	public ItemUI curItemUI;
	public ItemUI targetItemUI;
	public UILabel curAttr;
	public UILabel targetAttr;
	public UILabel labStone;
	public UILabel labCoin;
	public UIButton btnUpgrade;

	public UIToggle toggleQuickBuy;
	public UILabel quickCost;
	void Start()
	{
		if(btnUpgrade != null)UIEventListener.Get(btnUpgrade.gameObject).onClick = UpgradeEquip; 
	}
	protected override void OnOpen ()
	{
		base.OnOpen ();
	}
	protected override void OnClose ()
	{
		base.OnClose ();
	}
	protected override void HandEvent (bool _bind)
	{
		base.HandEvent (_bind);
		if(_bind)
		{
            if (toggleQuickBuy != null) EventDelegate.Add(toggleQuickBuy.onChange, OnChooseQuickBuy);
			GameCenter.equipmentTrainingMng.OnSelectEquipmentUpdate += ShowItemInfo;
			GameCenter.equipmentTrainingMng.OnQuickBuyCostUpdateEvent += OnChooseQuickBuy;
			GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += OnBaseUpdate;
		}else
		{
            if (toggleQuickBuy != null) EventDelegate.Remove(toggleQuickBuy.onChange, OnChooseQuickBuy);
			GameCenter.equipmentTrainingMng.OnSelectEquipmentUpdate -= ShowItemInfo;
			GameCenter.equipmentTrainingMng.OnQuickBuyCostUpdateEvent -= OnChooseQuickBuy;
			GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= OnBaseUpdate;
		}
	}
	bool bigEquipLevel = false;
    bool targetEquipNobind = false;//目标装备变为不绑定
	bool enoughMaterial = true;
	bool enoughCoin = true;
	List<EquipmentInfo> lackEquipList = new List<EquipmentInfo>();
	void ShowItemInfo()
	{
		enoughCoin = true;
		enoughMaterial = true;
		bigEquipLevel = false;

		lackEquipList.Clear();
		EquipmentInfo equip = GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo == null?null:new EquipmentInfo(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo,EquipmentBelongTo.PREVIEW);
		if(curItemUI != null)curItemUI.FillInfo(equip);
		if(equip != null)
		{
			if(curAttr != null)curAttr.text = GameHelper.GetAttributeNameAndValue(equip.AttributePairList);
			PromoteRef promoteRef = ConfigMng.Instance.GetPromoteRefByEid(equip.EID);
			if(promoteRef != null)
			{
				EquipmentInfo info = new EquipmentInfo(equip,promoteRef.end_item);
                if (info != null) targetEquipNobind = (equip.IsBind && info.BindType == EquipmentBindType.UnBind);
				bigEquipLevel = (info.UseReqLevel > GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel);
                if (targetItemUI != null)
                {
                    targetItemUI.FillInfo(info);
                    targetItemUI.SetBindByClient(info.BindType == EquipmentBindType.UnBind);
                }
				if(targetAttr != null)targetAttr.text = GameHelper.GetAttributeNameAndValue(info.AttributePairList);
				System.Text.StringBuilder builder = new System.Text.StringBuilder();
				bool enough = false;
				EquipmentInfo lackEquip = null;
				for (int i = 0,max=promoteRef.materialItem.Count; i < max; i++) 
				{
					string str = GameHelper.GetStringWithBagNumber(promoteRef.materialItem[i],out enough);
					if(enough == false)
					{
						enoughMaterial = enough;
					}
					builder.Append(str);
					if(i < max-1)builder.Append("\n");
				}
				ItemValue coinItem = new ItemValue(5,(int)promoteRef.coin);
				string coinStr = GameHelper.GetStringWithBagNumber(coinItem,out enoughCoin,out lackEquip);
				if(labCoin != null)labCoin.text = coinStr;
				if(lackEquip != null)
					lackEquipList.Add(lackEquip);
				if(labStone != null)labStone.text = builder.ToString();
			}else
			{
			//	GameCenter.messageMng.AddClientMsg("升阶表中查不到此装备!");
				if(targetItemUI != null)targetItemUI.FillInfo(null);
				if(targetAttr != null)targetAttr.text = string.Empty;
				if(labStone != null)labStone.text = string.Empty;
				if(labCoin != null)labCoin.text = string.Empty;
			}
			GameCenter.equipmentTrainingMng.quickBuyList = lackEquipList;
		}else
		{
			if(targetItemUI != null)targetItemUI.FillInfo(null);
			if(curAttr != null)curAttr.text = string.Empty;
			if(targetAttr != null)targetAttr.text = string.Empty;
			if(labStone != null)labStone.text = string.Empty;
			if(labCoin != null)labCoin.text = string.Empty;
		}
	}
	void UpgradeEquip(GameObject go)
	{
        if (targetEquipNobind)
        {
            MessageST mst = new MessageST();
            mst.messID = 531;
            mst.delYes = (x) =>
                {
                    SureUpgradeEquip();
                };
            GameCenter.messageMng.AddClientMsg(mst);
        }
        else
        {
            SureUpgradeEquip();
        }
	}

    void SureUpgradeEquip()
    {
        GameCenter.equipmentTrainingMng.ChooseQuickBuy = toggleQuickBuy.value;
        if (bigEquipLevel)
        {
            GameCenter.messageMng.AddClientMsg(248);
            return;
        }
        if (!enoughMaterial)
        {
            GameCenter.messageMng.AddClientMsg(209);
            return;
        }
        if (!enoughCoin && !GameCenter.equipmentTrainingMng.ChooseQuickBuy)
        {
            GameCenter.messageMng.AddClientMsg(155);
            return;
        }
        if (GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo == null) return;
        if (GameCenter.equipmentTrainingMng.ChooseQuickBuy && !enoughCoin)
        {
            //快捷购买
            GameCenter.equipmentTrainingMng.C2S_UpgradeEquip(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID, 1, true);
            return;
        }
        GameCenter.equipmentTrainingMng.C2S_UpgradeEquip(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID, 1, false);
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
