//==============================================
//作者：邓成
//日期：2016/3/23
//用途：装备强化界面类
//==============================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class EquipmentStrengthenWnd : SubWnd {
	public ItemUI curEquipmentUI;
	//public ItemUI nextEquipmentUI;
    public UILabel curUpgradeLv;
    public UILabel labAddUpgradeLv;
	public UILabel curAttrNum;
	public UILabel strengthLvDes;
	public UILabel labPerfectNum;
	public UIProgressBar perfectNum;
    public GameObject isMaxLvGo;
    public GameObject notMaxLvGo;
	/// <summary>
	/// 单次强化
	/// </summary>
	public UIButton btnStrengthen;
	/// <summary>
	/// 一键强化
	/// </summary>
	public UIButton btnOneKeyStrengthen;

	public UIButton btnPerfectStrengthen;

    public GemInlaySlotStateUI[] slotState;

	public GameObject consumeGo;
    public GameObject perfectConsumeGo;
	public UILabel labStoneNum;
	public GameObject perfectGo;
	public UIToggle normalStrengthToggle;
	public UILabel labSpecialThing;

	public UILabel labCoin;
    public UILabel labPerfectCoin;

	public UIToggle toggleQuickBuy;
	public UILabel quickCost;
    public UIFxAutoActive effect;//升级特效
	protected bool firstOpenWnd = false;
    protected float ClickCD=3.0f;
	protected override void OnOpen ()
	{
		base.OnOpen ();
		if(toggleQuickBuy != null)toggleQuickBuy.value = false;
		firstOpenWnd = true;
		RefreshWnd();
        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.STRENGTHENING,false);
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
			if(btnStrengthen != null)UIEventListener.Get(btnStrengthen.gameObject).onClick = StrengthenOne;
			if(btnOneKeyStrengthen != null)UIEventListener.Get(btnOneKeyStrengthen.gameObject).onClick = StrengthenOneKey;
			if(btnPerfectStrengthen != null)UIEventListener.Get(btnPerfectStrengthen.gameObject).onClick = PerfectStrengthen; 
            if (toggleQuickBuy != null) EventDelegate.Add(toggleQuickBuy.onChange, OnChooseQuickBuy);
			GameCenter.equipmentTrainingMng.OnSelectEquipmentUpdate += RefreshWnd;
            GameCenter.equipmentTrainingMng.OnSelectEquipmentUpdate += showEffect;
			GameCenter.equipmentTrainingMng.OnQuickBuyCostUpdateEvent += OnChooseQuickBuy;
			GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += OnBaseUpdate;
		}else
		{
            if (toggleQuickBuy != null) EventDelegate.Remove(toggleQuickBuy.onChange, OnChooseQuickBuy);
			GameCenter.equipmentTrainingMng.OnSelectEquipmentUpdate -= RefreshWnd;
            GameCenter.equipmentTrainingMng.OnSelectEquipmentUpdate -= showEffect;
			GameCenter.equipmentTrainingMng.OnQuickBuyCostUpdateEvent -= OnChooseQuickBuy;
			GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= OnBaseUpdate;
		}
	}
	bool enough = true;
	bool enoughPerfect = true;
	List<EquipmentInfo> lackEquipList = new List<EquipmentInfo>();
    void showEffect()
    {
        if (effect != null)
        {
			if(!firstOpenWnd)//第一次打开强化界面,不显示特效
			{
				effect.ShowFx();
			}
			firstOpenWnd = false;
        }
    }
	void RefreshWnd()
	{
		lackEquipList.Clear();
		enough = true;
		enoughPerfect = true;
        ResetWnd();
		if(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo == null)
		{
			return;
		}
		EquipmentInfo curEquip = new EquipmentInfo(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo,EquipmentBelongTo.PREVIEW);
		if(curEquip != null)
		{
            bool isMaxLv = curEquip.MaxPowerLvValue <= curEquip.UpgradeLv;
            RefreshGemSlots(curEquip.UpgradeLv);
            if (curUpgradeLv != null) curUpgradeLv.text = "+"+curEquip.UpgradeLv.ToString();
            if (labAddUpgradeLv != null) labAddUpgradeLv.enabled = !isMaxLv;
            if (strengthLvDes != null) strengthLvDes.text = ConfigMng.Instance.GetStrengthenLvDesfByLv(curEquip.UpgradeLv).Replace("\\n","\n"); ;
			if(labPerfectNum != null)labPerfectNum.text = curEquip.StrengthExp.ToString()+"%";
			if(perfectNum != null)perfectNum.value = (float)curEquip.StrengthExp/100f;
			if(curEquipmentUI != null)curEquipmentUI.FillInfo(curEquip);
            if (curAttrNum != null) curAttrNum.text = GameHelper.GetAttributeNameAndValue(curEquip.StrengthValue);
            if (isMaxLvGo != null) isMaxLvGo.SetActive(isMaxLv);
            if (notMaxLvGo != null) notMaxLvGo.SetActive(!isMaxLv);
            if (!isMaxLv)
            {
                if (btnStrengthen != null)
                {
                    CancelInvoke("SetStrengthenActive");
                    Invoke("SetStrengthenActive",0.1f);
                    //btnStrengthen.isEnabled = true;
                }
                if (btnOneKeyStrengthen != null)
                {
                    CancelInvoke("SetOneKeyStrengthenActive");
                    Invoke("SetOneKeyStrengthenActive",0.1f);
                    //btnOneKeyStrengthen.isEnabled = true;
                }
                if (btnPerfectStrengthen != null) btnPerfectStrengthen.isEnabled = true;
            }
            else
            {
                if (perfectGo != null) perfectGo.SetActive(true);//强化满的时候也显示金锤子
            }
            EquipmentInfo nextEquip = (!isMaxLv ? new EquipmentInfo(curEquip) : null);
            if (nextEquip != null)
			{
                if (curAttrNum != null) curAttrNum.text = GameHelper.GetAttributeNameAndValue(curEquip.StrengthValue, nextEquip.StrengthValue);
				if(consumeGo != null)consumeGo.SetActive(true);
                if (perfectConsumeGo != null) perfectConsumeGo.SetActive(true);
            }
			StrengthenRef strengthenRef = ConfigMng.Instance.GetStrengthenRefByLv(curEquip.UpgradeLv+1);
            if (strengthenRef != null)
            {
                EquipmentInfo lackEquip = null;
                StringBuilder builder = new StringBuilder();
                for (int i = 0, max = strengthenRef.items.Count; i < max; i++)
                {
                    ItemValue item = strengthenRef.items[i];
                    builder.Append(GameHelper.GetStringWithBagNumber(item, out enough, out lackEquip));
                    if (i < max - 1)
                        builder.Append("\n");
                    if (lackEquip != null)
                        lackEquipList.Add(lackEquip);
                }
                if (labStoneNum != null) labStoneNum.text = builder.ToString();
                if (labCoin != null) labCoin.text = GameHelper.GetStringWithBagNumber(5, (ulong)strengthenRef.coin);
                if (labPerfectCoin != null) labPerfectCoin.text = GameHelper.GetStringWithBagNumber(5, (ulong)strengthenRef.coin);
                if (strengthenRef.perfectItems.Count > 0)
                {
                    ItemValue item = strengthenRef.perfectItems[0];
                    if (item.eid == 0)
                    {
                        if (normalStrengthToggle != null) normalStrengthToggle.value = true;
                        if (perfectGo != null) perfectGo.SetActive(false);
                    }
                    else
                    {
                        if (perfectGo != null) perfectGo.SetActive(true);
                        if (labSpecialThing != null) labSpecialThing.text = GameHelper.GetStringWithBagNumber(item, out enoughPerfect);
                    }
                }
                else
                {
                    if (perfectGo != null) perfectGo.SetActive(false);
                }
            }
			GameCenter.equipmentTrainingMng.quickBuyList = lackEquipList;
		}
	}

    void ResetWnd()
    {
        if (curEquipmentUI != null) curEquipmentUI.FillInfo(null);
        if (curUpgradeLv != null) curUpgradeLv.text = string.Empty ;
        if (labAddUpgradeLv != null) labAddUpgradeLv.enabled = false;
        if (labPerfectNum != null) labPerfectNum.text = "0%";
        if (perfectNum != null) perfectNum.value = 0f;
        if (curAttrNum != null) curAttrNum.text = string.Empty;
        if (consumeGo != null) consumeGo.SetActive(false);
        if (perfectConsumeGo != null) perfectConsumeGo.SetActive(false);
        if (strengthLvDes != null) strengthLvDes.text = ConfigMng.Instance.GetStrengthenLvDesfByLv(1).Replace("\\n","\n");
        if (btnStrengthen != null) btnStrengthen.isEnabled = false;
        if (btnOneKeyStrengthen != null) btnOneKeyStrengthen.isEnabled = false;
        if (btnPerfectStrengthen != null) btnPerfectStrengthen.isEnabled = false;
        RefreshGemSlots(0);
    }

    void RefreshGemSlots(int _upgradeLv)
    {
        List<InsetRef> openSlots = ConfigMng.Instance.GetInlaySlotsByLv(_upgradeLv);
        List<InsetRef> allSlots = ConfigMng.Instance.GetInlaySlotsByLv(int.MaxValue);
        if (slotState != null)
        {
            for (int i = 0,length=slotState.Length; i < length; i++)
            {
                if (slotState[i] != null)
                {
                    if (openSlots.Count > i)
                    {
                        slotState[i].SetData(string.Empty,true);
                    }
                    else
                    {

                        slotState[i].SetData(allSlots.Count > i?allSlots[i].des:string.Empty, false);
                    }
                }
            }
        }
    }
    void SetHide()
    {
        if (btnOneKeyStrengthen != null)
            btnOneKeyStrengthen.isEnabled = false;
    }
	void StrengthenOne(GameObject go)
	{
		if(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo == null)
		{
			GameCenter.messageMng.AddClientMsg(ConfigMng.Instance.GetUItext(355));
			return;
		}
        //没有勾选上材料不足，用元宝购买
		if (enough)
		{
            //正常购买
			GameCenter.equipmentTrainingMng.C2S_Strengthen(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID, 0,false);
        }
        else if (GameCenter.equipmentTrainingMng.ChooseQuickBuy)
        {
            if ((ulong)GameCenter.equipmentTrainingMng.QuickBuyCost > GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount)
			{
				//GameCenter.messageMng.AddClientMsg(137);
				MessageST mst1 = new MessageST();
				mst1.messID = 137;
				mst1.delYes = (y)=>
				{
					GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
				};
				GameCenter.messageMng.AddClientMsg(mst1);
				return;
			}
            //快捷购买
			GameCenter.equipmentTrainingMng.C2S_Strengthen(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID, 0,true);
        }
        else
        {
			GameCenter.messageMng.AddClientMsg(141);
        }
	}
	void StrengthenOneKey(GameObject go)
	{
        //StopCoroutine("ContinueClickDelay");
		if(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo == null)
		{
            GameCenter.messageMng.AddClientMsg(ConfigMng.Instance.GetUItext(355));
			return;
		}
		//没有勾选上材料不足，用元宝购买
		if (enough)
		{
            //正常购买
            GameCenter.equipmentTrainingMng.C2S_Strengthen(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID, 1,false);
        }
		else if (GameCenter.equipmentTrainingMng.ChooseQuickBuy)
		{
            if ((ulong)GameCenter.equipmentTrainingMng.QuickBuyCost > GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount)
			{
				//GameCenter.messageMng.AddClientMsg(137);
				MessageST mst1 = new MessageST();
				mst1.messID = 137;
				mst1.delYes = (y)=>
				{
					GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
				};
				GameCenter.messageMng.AddClientMsg(mst1);
				return;
			}
            //快捷购买
            //Debug.Log("快捷购买");
            GameCenter.equipmentTrainingMng.C2S_Strengthen(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID, 1, true);
        }
		else
		{
			GameCenter.messageMng.AddClientMsg(141);
		}
	}
	void PerfectStrengthen(GameObject go)
	{
		if(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo == null)
		{
            GameCenter.messageMng.AddClientMsg(ConfigMng.Instance.GetUItext(355));
			return;
		}
		if(enoughPerfect == false)
		{ 
			GameCenter.messageMng.AddClientMsg(141);
			return;
		}
        GameCenter.equipmentTrainingMng.C2S_Strengthen(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID,2,false);
    }

	void OnChooseQuickBuy()
	{
		GameCenter.equipmentTrainingMng.ChooseQuickBuy = toggleQuickBuy.value;
        bool redColor = (ulong)GameCenter.equipmentTrainingMng.QuickBuyCost > GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount;
		System.Text.StringBuilder builder = new StringBuilder();
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
    //防止连续点击
    void SetStrengthenActive()
    {
        btnStrengthen.isEnabled = true;
    }
    void SetOneKeyStrengthenActive()
    {
        btnOneKeyStrengthen.isEnabled = true;
    }
}
