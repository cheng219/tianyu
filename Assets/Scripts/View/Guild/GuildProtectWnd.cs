//===============================
//作者：邓成
//日期：2016/5/13
//用途：仙域守护界面类
//===============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class GuildProtectWnd : GUIBase {

	public UIToggle[] toggleLevel;
	public UIButton btnOpen;
	public UIButton btnEnter;
	public UIButton btnClose;
	public UIButton btnRank;
	public UILabel labOpenState;

	public UITexture textureShow;

	public UIGrid rankPanel;
	protected Dictionary<int,RankItemUI> allItemList = new Dictionary<int, RankItemUI>();

	void Awake()
	{
		layer = GUIZLayer.NORMALWINDOW;
		mutualExclusion = true;
		if(btnClose != null)UIEventListener.Get(btnClose.gameObject).onClick = CloseWnd;
		if(btnEnter != null)UIEventListener.Get(btnEnter.gameObject).onClick = EnterActivity;
		if(btnOpen != null)UIEventListener.Get(btnOpen.gameObject).onClick = OpenActivity;
		if(btnRank != null)UIEventListener.Get(btnRank.gameObject).onClick = ReqRankInfo;
	}
	protected override void OnOpen ()
	{
		base.OnOpen ();
		if(textureShow != null)
		{
			ConfigMng.Instance.GetBigUIIcon("Pic_xm_xysh_tu",ShowTexture);
		}
		GameCenter.activityMng.C2S_ReqProtectActivityInfo();
	}
	protected override void OnClose ()
	{
		base.OnClose ();
		ConfigMng.Instance.RemoveBigUIIcon("Pic_xm_xysh_tu");
	}
	protected override void HandEvent (bool _bind)
	{
		base.HandEvent (_bind);
		if(_bind)
		{
			GameCenter.activityMng.OnGotProtectActivityRankEvent += ShowRank;
			GameCenter.activityMng.OnGotProtectActivityEvent += ShowActivityInfo;
		}else
		{
			GameCenter.activityMng.OnGotProtectActivityRankEvent -= ShowRank;
			GameCenter.activityMng.OnGotProtectActivityEvent -= ShowActivityInfo;
		}
	}
	void ShowTexture(Texture2D texture)
	{
		if(textureShow != null)textureShow.mainTexture = texture;
	}
	void ShowActivityInfo()
	{
        int openVal = GameCenter.activityMng.ProtectActivityOpenValue;
        bool isOpen = GameCenter.activityMng.isProtectActivityOpen;
		if(btnEnter != null && btnOpen != null)
		{
			btnEnter.isEnabled = isOpen;
			btnOpen.isEnabled = !isOpen;
		}
		if(labOpenState != null)labOpenState.enabled = isOpen;
		if(toggleLevel != null)
		{
			for (int i = 0,max=toggleLevel.Length; i < max; i++) {
                if (isOpen)
                {
                    toggleLevel[i].value = openVal == i + 1;
                    toggleLevel[i].enabled = false;//活动开启之后,不能选择难度
                }
                else
                {
                    toggleLevel[i].enabled = true;
                    if(i == 0)
                        toggleLevel[i].value = true;
                }
			}
		}
	}
	void ShowRank()
	{
		List<guild_guard_rank> rankList = GameCenter.activityMng.ProtectActivityRankList;
		for (int i = 0,max=rankList.Count; i < max; i++) {
			RankItemUI item = null;
			if(!allItemList.TryGetValue(i,out item))
			{
				item = RankItemUI.CreateNew(rankPanel.transform);
				allItemList[i] = item;
			}
			item = allItemList[i];
			if(item != null)item.SetData(rankList[i],i);
		}
		if(rankPanel != null)rankPanel.repositionNow = true;
	}

	void CloseWnd(GameObject go)
	{
		GameCenter.uIMng.SwitchToUI(GUIType.GUILDMAIN);
	}
	void EnterActivity(GameObject go)
	{
		GameCenter.activityMng.C2S_EnterProtectActivity();
	}
	void OpenActivity(GameObject go)
	{
		if(GameCenter.guildMng.isMyPresident || GameCenter.guildMng.isMyViceChairman)
			GameCenter.activityMng.C2S_OpenProtectActivity(GetDifficulty());
		else
			GameCenter.messageMng.AddClientMsg(294);
	}
	/// <summary>
	/// 请求排行榜信息
	/// </summary>
	/// <param name="go">Go.</param>
	void ReqRankInfo(GameObject go)
	{
		GameCenter.activityMng.C2S_ReqProtectActivityRank();
	}
	int GetDifficulty()
	{
		if(toggleLevel != null)
		{
			for (int i = 0,max=toggleLevel.Length; i < max; i++) {
				if(toggleLevel[i] != null && toggleLevel[i].value)
					return i+1;
			}
		}
		return 0;
	}
}
