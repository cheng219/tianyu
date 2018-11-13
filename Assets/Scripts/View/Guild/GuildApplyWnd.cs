//==================================
//作者：邓成
//日期：2016/4/13
//用途：仙盟审核界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuildApplyWnd : SubWnd {
	public GameObject parent;
	public Vector4 positionInfo;

	public UIButton oneKeyAccept;
	public UIButton ignorePage;
	public UIButton autoAccept;
	public UILabel labOpenState;

	public UIInput inputFightValue;
	public UIToggle toggleOpen;

	protected Dictionary<int,GuildMemberItem> memberList = new Dictionary<int, GuildMemberItem>();
	void Awake()
	{
		if(autoAccept != null)UIEventListener.Get(autoAccept.gameObject).onClick = AutoAccept;
		if(ignorePage != null)UIEventListener.Get(ignorePage.gameObject).onClick = IgnorePage;
		if(oneKeyAccept != null)UIEventListener.Get(oneKeyAccept.gameObject).onClick = OneKeyAccept;
	}

	protected override void OnOpen()
	{
		base.OnOpen();
		GameCenter.guildMng.C2S_GuildJoinList();
	}
	protected override void OnClose()
	{
		base.OnClose();
		ClearData();
	}
	void ClearData()
	{
		foreach(GuildMemberItem go in memberList.Values)
		{
			Destroy(go.gameObject);
		}
		memberList.Clear();
	}
	protected override void HandEvent(bool _bind)
	{
		base.HandEvent(_bind);
		if (_bind)
		{
			GameCenter.guildMng.OnGetNewMemberEvent += ShowMemberList;
			GameCenter.guildMng.OnUpdateAutoStateEvent += ShowAutoReplyState;
		}
		else
		{ 
			GameCenter.guildMng.OnGetNewMemberEvent -= ShowMemberList;
			GameCenter.guildMng.OnUpdateAutoStateEvent -= ShowAutoReplyState;
		}
	}
	void ShowMemberList()
	{
		HideMemberList();
		List<GuildMemberData> guildMembers = GameCenter.guildMng.newMemberList;
		int index = 0;
		foreach(GuildMemberData data in guildMembers)
		{
			GuildMemberItem item = null;
			if(!memberList.TryGetValue(index,out item))
			{
				item = GuildMemberItem.CreateNewJoin(parent.transform,new Vector3(positionInfo.x,positionInfo.y + positionInfo.w*index,-1));
				memberList[index] = item;
			}
			if(item != null)
			{
				item.SetDate(data);
				item.gameObject.SetActive(true);
			}
			index++;
		}
		ShowAutoReplyState();
	}
	void ShowAutoReplyState()
	{
		if(toggleOpen != null)toggleOpen.value = GameCenter.guildMng.AutoAgreeToggle;
		if(labOpenState != null)labOpenState.text = (GameCenter.guildMng.AutoAgreeToggle?ConfigMng.Instance.GetUItext(278):ConfigMng.Instance.GetUItext(279));
		if(inputFightValue != null)inputFightValue.value = GameCenter.guildMng.AutoAgreeValue.ToString();
	}

	void HideMemberList()
	{
		foreach(GuildMemberItem go in memberList.Values)
		{
			if(go != null)go.gameObject.SetActive(false);
		}
	}
	void AutoAccept(GameObject go)
	{
		if(GameCenter.guildMng.MyGuildInfo == null || GameCenter.guildMng.MyGuildInfo.MyPosition == GuildMemPosition.MEMBER)
		{
			GameCenter.messageMng.AddClientMsg(55);
			return;
		}
		if(toggleOpen != null && toggleOpen.value &&  inputFightValue != null)
		{
			int fightVal = 0;
			if(int.TryParse(inputFightValue.value,out fightVal))
				GameCenter.guildMng.C2S_AutoReplyJoin(true,fightVal);
		}else
			GameCenter.guildMng.C2S_AutoReplyJoin(false,0);
	}
	void IgnorePage(GameObject go)
	{
		if(GameCenter.guildMng.MyGuildInfo == null || GameCenter.guildMng.MyGuildInfo.MyPosition == GuildMemPosition.MEMBER)
		{
			GameCenter.messageMng.AddClientMsg(55);
			return;
		}
		GameCenter.guildMng.C2S_RefruseJoin();
	}
	void OneKeyAccept(GameObject go)
	{
		if(GameCenter.guildMng.MyGuildInfo == null || GameCenter.guildMng.MyGuildInfo.MyPosition == GuildMemPosition.MEMBER)
		{
			GameCenter.messageMng.AddClientMsg(55);
			return;
		}
		GameCenter.guildMng.C2S_ReplyJoin();
	}
}
