//==================================
//作者：邓成
//日期：2016/10/31
//用途：仙盟成员扩充界面类
//=================================

using UnityEngine;
using System.Collections;

public class GuildMemberAddWnd : SubWnd {

	public GameObject btnClose;
	public GameObject btnAdd;
	public UILabel curMemberCount;
	public UILabel nextMemberCount;
	public GameObject maxCountObj;
	public GameObject normalObj;
	public UILabel labDiamond;

	public UILabel curExpandTimes;
	public UILabel nextExpandTimes;
	public ItemUI itemUI;
	public UIToggle toggleQuickBuy;
	void Awake()
	{
		if(btnClose != null)UIEventListener.Get(btnClose).onClick = CloseWnd;
		if(btnAdd != null)UIEventListener.Get(btnAdd).onClick = ExpandCount;
		if(toggleQuickBuy != null)EventDelegate.Add(toggleQuickBuy.onChange,OnChange);
	}

	protected override void OnOpen ()
	{
		base.OnOpen ();
		if(toggleQuickBuy != null)toggleQuickBuy.value = false;
		isQuickBuy = false;
		RefreshDiamond();
		RefreshGuildInfo();
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
			GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += RefreshDiamond;
			GameCenter.guildMng.OnGetPublicEvent += RefreshGuildInfo;
		}else
		{
			GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= RefreshDiamond;
			GameCenter.guildMng.OnGetPublicEvent -= RefreshGuildInfo;
		}
	}

	void RefreshDiamond(ActorBaseTag _tag,ulong _val,bool _from)
	{
		if(_tag == ActorBaseTag.Diamond)
		{
			RefreshDiamond();
		}
	}

	void RefreshGuildInfo()
	{
		GuildInfo myGuildInfo = GameCenter.guildMng.MyGuildInfo;
		if(myGuildInfo != null)
		{
			bool isMaxCount = myGuildInfo.MemberExpandTimes >= GuildMng.MEMBER_EXPAND_MAX_TIMES;
			if(maxCountObj != null)maxCountObj.SetActive(isMaxCount);
			if(normalObj != null)normalObj.SetActive(!isMaxCount);
			if(curExpandTimes != null)curExpandTimes.text = myGuildInfo.MemberExpandTimes.ToString();
			if(nextExpandTimes != null)nextExpandTimes.text = (isMaxCount?myGuildInfo.MemberExpandTimes:(myGuildInfo.MemberExpandTimes+1)).ToString();
			if(curMemberCount != null)curMemberCount.text = myGuildInfo.MemberMaxCount.ToString();
			if(nextMemberCount != null)nextMemberCount.text = (isMaxCount?myGuildInfo.MemberMaxCount:(myGuildInfo.MemberMaxCount+GuildMng.MEMBER_EXPAND_PER_COUNT)).ToString();
			if(itemUI != null)
			{
				//int itemNum = GameCenter.inventoryMng.GetNumberByType(2600044);
				EquipmentInfo item = new EquipmentInfo(2600044,1,EquipmentBelongTo.PREVIEW);
				itemUI.FillInfo(item);
			}
		}
	}

	void RefreshDiamond()
	{
		int itemNum = GameCenter.inventoryMng.GetNumberByType(2600044);
		int cost = 0;
		if(itemNum == 0)
		{
			EquipmentInfo item = new EquipmentInfo(2600044,itemNum,EquipmentBelongTo.PREVIEW);
			cost = (int)item.DiamondPrice;
		}
        if (labDiamond != null) labDiamond.text = cost.ToString() + "/" + GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount.ToString();
	}

	protected bool isQuickBuy = false;
	void OnChange()
	{
		if(toggleQuickBuy != null)isQuickBuy = toggleQuickBuy.value;
	}

	void ExpandCount(GameObject go)
	{
		GameCenter.guildMng.C2S_ExpandMemberCount(isQuickBuy);
	}

	void CloseWnd(GameObject go)
	{
		CloseUI();
	}
}
