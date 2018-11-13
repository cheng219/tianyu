//==================================
//作者：邓成
//日期：2016/4/13
//用途：公会成员界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuildMemberWnd : SubWnd {
	public GameObject parent;
	public Vector4 positionInfo;

	public GameObject btnParent;
	public UIButton btnUpPosition;
	public UIButton btnDownPosition;
	//public UILabel labBtnUpPosition;

	public UIButton btnPreview;
	public UIButton btnFourceOut;
	public UIButton btnAddFriend;
	public UIButton btnGiveLeader;

	protected GuildMemberData curSelectMember;
	protected Dictionary<int,GuildMemberItem> memberList = new Dictionary<int, GuildMemberItem>();
	void Awake()
	{
		if(btnUpPosition != null)UIEventListener.Get(btnUpPosition.gameObject).onClick = UpPosition;
		if(btnDownPosition != null)UIEventListener.Get(btnDownPosition.gameObject).onClick = DownPosition;
		if(btnPreview != null)UIEventListener.Get(btnPreview.gameObject).onClick = Preview;
		if(btnFourceOut != null)UIEventListener.Get(btnFourceOut.gameObject).onClick = FourceOut;
		if(btnAddFriend != null)UIEventListener.Get(btnAddFriend.gameObject).onClick = AddFriend;
		if(btnGiveLeader != null)UIEventListener.Get(btnGiveLeader.gameObject).onClick = GiveLeader;
	}
	protected override void OnOpen()
	{
		base.OnOpen();
		GameCenter.guildMng.C2S_GuildMemberList();
	}
	protected override void OnClose()
	{
		base.OnClose();
		ClearData();
	}
	void ClearData()
	{
		curSelectMember = null;
		if(btnParent != null)btnParent.SetActive(false);
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
			GameCenter.guildMng.OnUpdateMemberEvent += ShowMemberList;
		}
		else
		{ 
			GameCenter.guildMng.OnUpdateMemberEvent -= ShowMemberList;
		}
	}

	void ShowMemberList()
	{
		HideMemberList();
		List<GuildMemberData> guildMembers = GameCenter.guildMng.GuildMemberList;
		int index = 0;
		//Debug.Log("guildMembers len:"+guildMembers.Count);
		for (int i = 0,max=guildMembers.Count; i < max; i++) 
		{
			GuildMemberData data = guildMembers[i];
			GuildMemberItem item = null;
			if(!memberList.TryGetValue(index,out item))
			{
				item = GuildMemberItem.CreateNew(parent.transform,new Vector3(positionInfo.x,positionInfo.y + positionInfo.w*index,-1));
				memberList[index] = item;
			}
			if(item != null)
			{
				item.SetDate(data,SelectMember);
				item.gameObject.SetActive(true);
			}
			index++;
		}
	}
	void HideMemberList()
	{
		foreach(GuildMemberItem go in memberList.Values)
		{
			if(go != null)go.gameObject.SetActive(false);
		}
	}
	void SelectMember(GuildMemberData data ,bool select)
	{
		if(select)
		{
			if(data.memberID == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)return;//不能操作自己
			if(btnParent != null)btnParent.SetActive(true);
			if(GameCenter.guildMng.MyGuildInfo != null)
			{
				GuildMemPosition myPos = GameCenter.guildMng.MyGuildInfo.MyPosition;
				if(btnUpPosition != null)btnUpPosition.isEnabled = (myPos == GuildMemPosition.CHAIRMAN && data.memberPosition != GuildMemPosition.VICECHAIRMAN);
				if(btnDownPosition != null)btnDownPosition.isEnabled = (myPos == GuildMemPosition.CHAIRMAN && data.memberPosition != GuildMemPosition.MEMBER);
				if(btnFourceOut != null)btnFourceOut.isEnabled = (myPos != GuildMemPosition.MEMBER);
				if(btnGiveLeader != null)btnGiveLeader.isEnabled = (myPos == GuildMemPosition.CHAIRMAN);
			}
			curSelectMember = data;
		}
	}

	protected void DownPosition(GameObject go)
	{
		if(curSelectMember != null)
		{
			if(curSelectMember.memberPosition == GuildMemPosition.ELDER)
			{
				GameCenter.guildMng.C2S_PositionChange(curSelectMember.memberID,(int)GuildMemPosition.MEMBER);
			}
			if(curSelectMember.memberPosition == GuildMemPosition.VICECHAIRMAN)
			{
				GameCenter.guildMng.C2S_PositionChange(curSelectMember.memberID,(int)GuildMemPosition.ELDER);
			}
		}
	}

	protected void UpPosition(GameObject go)
	{
		if(curSelectMember != null)
		{
			if(curSelectMember.memberPosition == GuildMemPosition.ELDER)
			{
				if(GameCenter.guildMng.haveViceChairman)
				{
					GameCenter.messageMng.AddClientMsg(431);
					return;
				}
				GameCenter.guildMng.C2S_PositionChange(curSelectMember.memberID,(int)GuildMemPosition.VICECHAIRMAN);
			}
			if(curSelectMember.memberPosition == GuildMemPosition.MEMBER)
				GameCenter.guildMng.C2S_PositionChange(curSelectMember.memberID,(int)GuildMemPosition.ELDER);
		}
	}
	protected void Preview(GameObject go)
	{
		if(curSelectMember != null)
			GameCenter.previewManager.C2S_AskOPCPreview(curSelectMember.memberID);
	}
	protected void FourceOut(GameObject go)
	{
		if(curSelectMember != null)
			GameCenter.guildMng.C2S_FourceOutGuild(curSelectMember.memberID);
	}
	protected void AddFriend(GameObject go)
	{
		if(curSelectMember != null)
			GameCenter.friendsMng.C2S_AddFriend(curSelectMember.memberID);
	}
	protected void GiveLeader(GameObject go)
	{
		if(curSelectMember != null)
			GameCenter.guildMng.C2S_PositionChange(curSelectMember.memberID,(int)GuildMemPosition.CHAIRMAN);
	}
}
