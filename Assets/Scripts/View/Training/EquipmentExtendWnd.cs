//==============================================
//作者：邓成
//日期：2016/3/29
//用途：装备继承界面类
//==============================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentExtendWnd : SubWnd {
	public ItemUI curItemUI;
	public UIButton btnClearMainItem;
	/// <summary>
	/// 副装备
	/// </summary>
	public ItemUI viceItemUI;
	public UIButton btnClearViceItem;

	public UIToggle toggleStrengthAttr;
	public UIToggle toggleWashAttr;
	public UIToggle toggleLuckAttr;
	public UILabel labConsume;
    public UILabelOpenEquipDesUI consumeDesUI;

	public UIButton btnExtend;
	protected bool haveTwoEquip = false;

	public UIToggle toggleQuickBuy;
	public UILabel quickCost;

	void Start()
	{
		if(btnExtend != null)UIEventListener.Get(btnExtend.gameObject).onClick = ExtendAttr;
		if(btnClearMainItem != null)UIEventListener.Get(btnClearMainItem.gameObject).onClick = ClearMainItem;
		if(btnClearViceItem != null)UIEventListener.Get(btnClearViceItem.gameObject).onClick = ClearViceItem; 
	}
    void OnEnable()
    {
        if (toggleQuickBuy != null) EventDelegate.Add(toggleQuickBuy.onChange, OnChooseQuickBuy);
    }
    void OnDisable()
    {
        if (toggleQuickBuy != null) EventDelegate.Remove(toggleQuickBuy.onChange, OnChooseQuickBuy);
    }
	protected override void OnOpen ()
	{
		base.OnOpen ();
		InitWnd();
	}
	protected override void OnClose ()
	{
		base.OnClose ();
		GameCenter.equipmentTrainingMng.CurSlot = EquipSlot.None;
		GameCenter.equipmentTrainingMng.CurViceEquipmentInfo = null;
	}
	protected override void HandEvent (bool _bind)
	{
		base.HandEvent (_bind);
		if(_bind)
		{
			GameCenter.equipmentTrainingMng.OnSelectEquipmentUpdate += ShowMainItemInfo;
			GameCenter.equipmentTrainingMng.OnViceEquipmentUpdate += ShowViceItemInfo;
			GameCenter.equipmentTrainingMng.OnQuickBuyCostUpdateEvent += OnChooseQuickBuy;
            GameCenter.equipmentTrainingMng.OnExtendEquipmentResultEvent += ExtendResult;
		}else
		{
			GameCenter.equipmentTrainingMng.OnSelectEquipmentUpdate -= ShowMainItemInfo;
			GameCenter.equipmentTrainingMng.OnViceEquipmentUpdate -= ShowViceItemInfo;
			GameCenter.equipmentTrainingMng.OnQuickBuyCostUpdateEvent -= OnChooseQuickBuy;
            GameCenter.equipmentTrainingMng.OnExtendEquipmentResultEvent -= ExtendResult;
		}
	}
	void InitWnd()
	{
		GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo = null;
		if(curItemUI != null)curItemUI.FillInfo(null);
		if(viceItemUI != null)viceItemUI.FillInfo(null);
		if(btnClearMainItem != null)btnClearMainItem.gameObject.SetActive(false);
		if(btnClearViceItem != null)btnClearViceItem.gameObject.SetActive(false);
		if(toggleWashAttr != null)toggleWashAttr.value = false;
		if(toggleStrengthAttr != null)toggleStrengthAttr.value = false;
		if(toggleLuckAttr != null)toggleLuckAttr.value = false;
		if(labConsume != null)labConsume.text = string.Empty;
		if(toggleQuickBuy != null)toggleQuickBuy.value = false;
	}
	void ExtendResult()
	{
	//	ShowMainItemInfo();
	//	ShowViceItemInfo();
        ClearMainItem(null);
	}
	void ShowMainItemInfo()
	{
		EquipmentInfo mainEquip = GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo != null?new EquipmentInfo(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo,EquipmentBelongTo.PREVIEW):null;
		if(curItemUI != null)curItemUI.FillInfo(mainEquip);
		if(btnClearMainItem != null)btnClearMainItem.gameObject.SetActive(mainEquip != null);
		if(mainEquip != null)
		{
			if(mainEquip.UpgradeLv >= 1)toggleStrengthAttr.value = true;
			if(mainEquip.EquOne != 0)toggleWashAttr.value = true;
			if(mainEquip.LuckyLv >= 1)toggleLuckAttr.value = true;
			GameCenter.equipmentTrainingMng.ShowEquipmentBySlot(mainEquip.Slot);
		}else
		{
			GameCenter.equipmentTrainingMng.ShowEquipmentBySlot(EquipSlot.None);
		}
	}
	void ShowViceItemInfo()
	{
		EquipmentInfo viceEquip = GameCenter.equipmentTrainingMng.CurViceEquipmentInfo != null?new EquipmentInfo(GameCenter.equipmentTrainingMng.CurViceEquipmentInfo,EquipmentBelongTo.PREVIEW):null;
		if(viceItemUI != null)viceItemUI.FillInfo(viceEquip);
		if(btnClearViceItem != null)btnClearViceItem.gameObject.SetActive(viceEquip != null);
		ShowMaterials();
	}
	void ExtendAttr(GameObject go)
	{
		GameCenter.equipmentTrainingMng.ChooseQuickBuy = toggleQuickBuy.value;
        EquipmentInfo mainEquip = GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo;
        //用来继承主装备属性的副装备
        EquipmentInfo subsidiaryEquip = GameCenter.equipmentTrainingMng.CurViceEquipmentInfo;
        if (subsidiaryEquip == null)
		{
			GameCenter.messageMng.AddClientMsg(513);
			return;
		}
        //增加继承时基础战力的逻辑判断(只能低战力继承高战力) by 唐源
        if (mainEquip != null && subsidiaryEquip != null&&mainEquip.EquipmentGs > subsidiaryEquip.EquipmentGs)
        {
            GameCenter.messageMng.AddClientMsg(533);
            return;
        }
        bool needSureTip = false;//是否需要再次确认的提示
		List<int> extendList = new List<int>();
        bool havaAttrCanExtend = false;
        if (toggleStrengthAttr.value)
        {
            extendList.Add(1);
            if (mainEquip != null && mainEquip.UpgradeLv > 0) havaAttrCanExtend = true;
            if (mainEquip.UpgradeLv <= GameCenter.equipmentTrainingMng.CurViceEquipmentInfo.UpgradeLv)
            {
                needSureTip = true;
            }
        }
        if (toggleWashAttr.value)
        {
            extendList.Add(2);
            if (mainEquip != null && mainEquip.EquOne != 0) havaAttrCanExtend = true;
        }
        if (toggleLuckAttr.value)
        {
            extendList.Add(3);
            if (mainEquip != null && mainEquip.LuckyLv > 0) havaAttrCanExtend = true;
            if (mainEquip.LuckyLv <= GameCenter.equipmentTrainingMng.CurViceEquipmentInfo.LuckyLv)
            {
                needSureTip = true;
            }
        }
        if (!havaAttrCanExtend)
        {
            GameCenter.messageMng.AddClientMsg(263);
            return;
        }
        if (extendList.Count == 0 || !mainEquip.CanExtend)
		{
			GameCenter.messageMng.AddClientMsg(263);
			return;
		}
        if (needSureTip)
        {
            MessageST mst = new MessageST();
            mst.messID = 516;
            mst.delYes = (x) =>
                {
                    SureExtend(extendList);
                };
            GameCenter.messageMng.AddClientMsg(mst);
        }
        else
        {
            SureExtend(extendList);
        }
	}

    void SureExtend(List<int> extendList)
    {
        if (enough)
        {
            GameCenter.equipmentTrainingMng.C2S_ExtendEquipAttr(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID, GameCenter.equipmentTrainingMng.CurViceEquipmentInfo.InstanceID, extendList, false);
        }
        else if (GameCenter.equipmentTrainingMng.ChooseQuickBuy)
        {
            if ((ulong)GameCenter.equipmentTrainingMng.QuickBuyCost > GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount)
            {
                //GameCenter.messageMng.AddClientMsg(137);
                MessageST mst1 = new MessageST();
                mst1.messID = 137;
                mst1.delYes = (y) =>
                {
                    GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
                };
                GameCenter.messageMng.AddClientMsg(mst1);
                return;
            }
            //快捷购买
            GameCenter.equipmentTrainingMng.C2S_ExtendEquipAttr(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID, GameCenter.equipmentTrainingMng.CurViceEquipmentInfo.InstanceID, extendList, true);
        }
        else
        {
            GameCenter.messageMng.AddClientMsg(141);
        }
    }

	bool enough = true;
	List<EquipmentInfo> lackItemList = new List<EquipmentInfo>();
	void ShowMaterials()
	{
		enough = true;
		lackItemList.Clear();
		EquipmentInfo mainEquip = GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo;
		EquipmentInfo viceEquip = GameCenter.equipmentTrainingMng.CurViceEquipmentInfo;
		if(mainEquip == null || viceEquip == null)
		{
			if(labConsume != null)labConsume.text = string.Empty;
			return;
		}
		List<ItemValue> itemList = new List<ItemValue>();
		if(mainEquip.UseReqLevel <= 30 && viceEquip.UseReqLevel <= 30)
		{
			//无消耗
		}else
		{
			if(mainEquip.UseReqLevel >= viceEquip.UseReqLevel)
			{
				InheritLuckyRef luckyRef = ConfigMng.Instance.GetInheritLuckyRefByID(0);
				for (int j = 0,count=luckyRef.consumeItem.Count; j < count; j++) 
				{
					AddItem(itemList,luckyRef.consumeItem[j]);
				}
			}else if(mainEquip.UseReqLevel < viceEquip.UseReqLevel)
			{
				ItemValue itemValue = ConfigMng.Instance.GetInheritConsumeByLv(mainEquip.UseReqLevel,viceEquip.UseReqLevel);
				if(itemValue != null)AddItem(itemList,itemValue);
			}
		}
		EquipmentInfo lackItem = null;
		System.Text.StringBuilder builder = new System.Text.StringBuilder();
		for (int i = 0,max=itemList.Count; i < max; i++) 
		{
			string str = GameHelper.GetStringWithBagNumber(itemList[i],out enough,out lackItem);
			builder.Append(str);
			if(i < max-1)builder.Append("\n");
			if(lackItem != null)
				lackItemList.Add(lackItem);
		}
		if(labConsume != null)labConsume.text = builder.ToString();
        if (consumeDesUI != null) consumeDesUI.OnChange();
		GameCenter.equipmentTrainingMng.quickBuyList = lackItemList;
	}
	bool AddItem(List<ItemValue> itemList,ItemValue item)
	{
		if(item.eid == 0)return false;
		for (int i = 0,max=itemList.Count; i < max; i++) 
		{
			if(itemList[i].eid == item.eid)
			{
				itemList[i].count += item.count;
				return true;
			}
		}
		itemList.Add(item);
		return false;
	}

	void OnChooseQuickBuy()
	{
		GameCenter.equipmentTrainingMng.ChooseQuickBuy = toggleQuickBuy.value;
        bool redColor = (ulong)GameCenter.equipmentTrainingMng.QuickBuyCost > GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount;
		System.Text.StringBuilder builder = new System.Text.StringBuilder();
		if(quickCost != null)
            quickCost.text = builder.Append(GameCenter.equipmentTrainingMng.QuickBuyCost).Append((redColor ? "/[ff0000]" : "/[00ff00]")).Append(GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount).Append("[-]").ToString();
	}

	void ClearMainItem(GameObject go)
	{
		GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo = null;
		GameCenter.equipmentTrainingMng.CurViceEquipmentInfo = null;
        if (toggleStrengthAttr != null) toggleStrengthAttr.value = false;
        if (toggleWashAttr != null) toggleWashAttr.value = false;
        if (toggleLuckAttr != null) toggleLuckAttr.value = false;
	}
	void ClearViceItem(GameObject go)
	{
		GameCenter.equipmentTrainingMng.CurViceEquipmentInfo = null;
	}
}
