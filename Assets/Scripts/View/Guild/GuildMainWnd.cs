//==================================
//作者：邓成
//日期：2016/4/12
//用途：仙盟主界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuildMainWnd : GUIBase {

	public UILabel labGuildName;
	public UILabel labGuildLeader;
	public UILabel labGuildMemCount;
	public UILabel labGuildRanking;
	public UIInput labGuildNotice;
	public UIButton btnOtherGuild;
	public UIButton btnOutGuild;
	public UIButton btnChangeNotice;
	public UIButton btnGuildRecruit;//仙盟招募

	public UIButton btnExpandMember;

	public UIButton btnClose;
	public SubWnd[] baseWnd;
    public enum GuildWndType
    { 
        GUILDHALL =0,
        GUILDMEMBER,
        GUILDLOG,
        GUILDFIGHT,
		GUILDAPPLY,
		GUILDEXPAND,
    }
    /// <summary>
    /// 分类页签
    /// </summary>
    public UIToggle[] toggles;

    void Awake()
    {
        mutualExclusion = true;
        layer = GUIZLayer.NORMALWINDOW;
		if(toggles != null)
		{
			for (int i = 0; i < toggles.Length; i++)
			{
                EventDelegate.Remove(toggles[i].onChange, OpenWndByType);
				EventDelegate.Add(toggles[i].onChange, OpenWndByType);
			}
		}
		if(btnOutGuild != null)UIEventListener.Get(btnOutGuild.gameObject).onClick = OutGuild;
		if(btnChangeNotice != null)UIEventListener.Get(btnChangeNotice.gameObject).onClick = ChangeNotice;
		if(btnClose != null)UIEventListener.Get(btnClose.gameObject).onClick = CloseWnd;
		if(btnGuildRecruit != null)UIEventListener.Get(btnGuildRecruit.gameObject).onClick = GuildRecruit;
		if(btnOtherGuild != null)UIEventListener.Get(btnOtherGuild.gameObject).onClick = OtherGuild;
		if(btnExpandMember != null)UIEventListener.Get(btnExpandMember.gameObject).onClick = OpenExpandWnd;
    }
    protected override void OnOpen()
    {
        base.OnOpen();
		GameCenter.guildMng.C2S_GuildInfo();
		if(baseWnd[(int)GuildWndType.GUILDHALL] != null)baseWnd[(int)GuildWndType.GUILDHALL].OpenUI();
    }
    protected override void OnClose()
    {
        base.OnClose();
		CloseAllSubWnd();
    }
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
			GameCenter.guildMng.OnGetPublicEvent += ShowGuildInfo;
			GameCenter.guildMng.OnUpdateNoticeEvent += ShowGuildInfo;
        }
        else
        { 
			GameCenter.guildMng.OnGetPublicEvent -= ShowGuildInfo;
			GameCenter.guildMng.OnUpdateNoticeEvent -= ShowGuildInfo;
        }
    }
	void ShowGuildInfo()
	{
		GuildInfo guildInfo = GameCenter.guildMng.MyGuildInfo;
		if(guildInfo != null)
		{
			GuildRef guildRef = GameCenter.guildMng.MyGuildInfo.CurGuildRef;
			if(labGuildName != null)labGuildName.text = guildInfo.GuildName;
			if(labGuildLeader != null)labGuildLeader.text = guildInfo.PresidentName;
			if(labGuildMemCount != null && guildRef != null)labGuildMemCount.text = guildInfo.MemberNum.ToString()+"/"+(guildInfo.MemberMaxCount);
			if(labGuildRanking != null)labGuildRanking.text = guildInfo.GuildRank.ToString();
			if(labGuildNotice != null)labGuildNotice.value = guildInfo.Notice;
		}
	}
	void OtherGuild(GameObject go)
	{
		GameCenter.uIMng.SwitchToUI(GUIType.GUILDLIST);
	}

	void OpenExpandWnd(GameObject go)
	{
		if(baseWnd[(int)GuildWndType.GUILDEXPAND] != null)baseWnd[(int)GuildWndType.GUILDEXPAND].OpenUI();
	}
	void OutGuild(GameObject go)
	{
		if(GameCenter.guildMng.isMyPresident)
		{
			if(GameCenter.guildMng.MemberCount > 1)
			{
				GameCenter.messageMng.AddClientMsg(296);
			}else
			{
				MessageST mst = new MessageST();
				mst.messID = 92;
				mst.delYes = (x)=>
				{
					MessageST mst2 = new MessageST();
					mst2.messID = 446;
					mst2.delYes = (y)=>
					{
						GameCenter.guildMng.C2S_OutGuild();
						GameCenter.uIMng.SwitchToUI(GUIType.NONE);
					};
					GameCenter.messageMng.AddClientMsg(mst2);
				};
				GameCenter.messageMng.AddClientMsg(mst);
			}
		}else
		{
			MessageST mst = new MessageST();
			mst.messID = 92;
			mst.delYes = (x)=>
			{
				GameCenter.guildMng.C2S_OutGuild();
				GameCenter.uIMng.SwitchToUI(GUIType.NONE);
			};
			GameCenter.messageMng.AddClientMsg(mst);
		}
	}
	void ChangeNotice(GameObject go)
	{
		if(GameCenter.guildMng.MyGuildInfo == null || GameCenter.guildMng.MyGuildInfo.MyPosition == GuildMemPosition.MEMBER)
		{
			GameCenter.messageMng.AddClientMsg(55);
			return;
		}
		if(labGuildNotice != null)
			GameCenter.guildMng.C2S_ChangeGuildNotice(labGuildNotice.value);
	}
	void GuildRecruit(GameObject go)
	{
		if(GameCenter.guildMng.MyGuildInfo == null || GameCenter.guildMng.MyGuildInfo.MyPosition == GuildMemPosition.MEMBER)
		{
			GameCenter.messageMng.AddClientMsg(55);
			return;
		}
		GameCenter.chatMng.SendNotice(ConfigMng.Instance.GetUItext(42,new string[1]{GameCenter.mainPlayerMng.MainPlayerInfo.GuildName}));
	}
	void CloseWnd(GameObject go)
	{
		GameCenter.uIMng.SwitchToUI(GUIType.NONE);
	}

    void OpenWndByType()
    {
		CloseAllSubWnd();
		GuildWndType type = GuildWndType.GUILDHALL;
        for (int i = 0; i < toggles.Length; i++)
        {
			if (toggles[i] != null && toggles[i].value)
            {
                type = (GuildWndType)i;
                break;
            }
        }
		if(baseWnd[(int)type] != null)baseWnd[(int)type].OpenUI();
    }
    void CloseAllSubWnd()
    {
		if(baseWnd != null)
		{
			for (int i = 0,max=baseWnd.Length; i < max; i++) 
				if(baseWnd[i] != null && i != (int)GuildWndType.GUILDEXPAND)
					baseWnd[i].CloseUI();
		}
    }
}
