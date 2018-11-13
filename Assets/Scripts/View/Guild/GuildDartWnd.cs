//===============================
//作者：邓成
//日期：2016/5/13
//用途：仙盟运镖界面类
//===============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuildDartWnd : SubWnd {
	public UIButton btnTrace;
	public UIButton btnDes;
	public UIButton btnClose;
	public UIGrid gridPanel;
	public GuildProtectRankUI guildProtectRankUI;

	protected List<GuildProtectRankUI> rankItemList = new List<GuildProtectRankUI>();

	void Awake()
	{
		if(btnClose != null)UIEventListener.Get(btnClose.gameObject).onClick = CloseWnd;
		if(btnTrace != null)UIEventListener.Get(btnTrace.gameObject).onClick = TraceToDart;
	}
	protected override void OnOpen ()
	{
		base.OnOpen ();
		GameCenter.activityMng.C2S_ReqGuildDartRank();
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
			GameCenter.activityMng.OnGotGuildDartRankListEvent += ShowRank;
		}else
		{
			GameCenter.activityMng.OnGotGuildDartRankListEvent -= ShowRank;
		}
	}
	void ShowRank()
	{
		for (int i = 0; i < rankItemList.Count; i++)
		{
			rankItemList[i].gameObject.SetActive(false);
		}
		List<st.net.NetBase.rank_info_base> rankList = GameCenter.activityMng.GuildDartRankList;
		for (int i = 0,max=rankList.Count; i < max; i++) 
		{
			if (rankItemList.Count < i + 1)
			{
				rankItemList.Add(guildProtectRankUI.CreateNew(gridPanel.transform));
			}
			rankItemList[i].gameObject.SetActive(true);
			rankItemList[i].SetData(rankList[i],i+1);
		}
		if(gridPanel != null)gridPanel.repositionNow = true;
	}

	void TraceToDart(GameObject go)
	{
		GameCenter.activityMng.C2S_ReqDartPos(DartType.GuildDart);
	}
	void CloseWnd(GameObject go)
	{
		CloseUI();
	}
}
