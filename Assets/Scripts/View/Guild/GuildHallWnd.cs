//==================================
//作者：邓成
//日期：2016/4/12
//用途：仙盟大厅界面类
//=================================

using UnityEngine;
using System.Collections;

public class GuildHallWnd : SubWnd {
	public string guildExpDes = ConfigMng.Instance.GetUItext(282);
	public string guildMaxLevelDes = ConfigMng.Instance.GetUItext(283);
	public int guildMaxLevel = 11;
	public UILabel labGuildExpDes;
	public UILabel labGuildExpNum;
	public UIProgressBar pbGuildExp;
	public GameObject canGetFx;
	public UIButton[] guildBtns;

	public GuildDonateWnd donateWnd;
	public GuildDartWnd dartWnd;
	public enum GuildBtnType
	{
		GUILDSALARY,//已改为仙盟活跃
		GUILDSTORE,
		/// <summary>
		/// 仙盟捐献
		/// </summary>
		GUILDDONATE,
		GUILDSKILL,
		GUILDSTORAGE,
		GUILDPROTECT,
		/// <summary>
		/// 仙盟运镖
		/// </summary>
		GUILDTRANSPORT,
		/// <summary>
		/// 仙盟城战
		/// </summary>
		GUILDFIGHT,
	}
	void Awake()
	{

		if(guildBtns != null)
		{
			for (int i = 0; i < guildBtns.Length; i++)
			{
				UIEventListener.Get(guildBtns[i].gameObject).onClick = OpenWndByBtn;
				UIEventListener.Get(guildBtns[i].gameObject).parameter = i; 
			}
		} 
	}
	protected override void OnOpen()
	{
		base.OnOpen();
		ShowHallInfo(); 
	}
	protected override void OnClose()
	{
		base.OnClose();
	}
	protected override void HandEvent(bool _bind)
	{
		base.HandEvent(_bind);
		if (_bind)
		{
			GameCenter.guildMng.OnGetPublicEvent += ShowHallInfo;
			GameCenter.guildMng.OnUpdateSalaryStateEvent += ShowHallInfo;
		}
		else
		{ 
			GameCenter.guildMng.OnGetPublicEvent -= ShowHallInfo;
			GameCenter.guildMng.OnUpdateSalaryStateEvent -= ShowHallInfo;
		}
	}
	void ShowHallInfo()
	{
		if(GameCenter.guildMng.MyGuildInfo != null)
		{
			GuildRef guildRef = GameCenter.guildMng.MyGuildInfo.CurGuildRef;
			if(guildRef == null)return;
			int guildLevel = GameCenter.guildMng.MyGuildInfo.GuildLv;
			int totalExp = guildRef.exp;
			int curExp = GameCenter.guildMng.MyGuildInfo.GuildExp;
			int expDiff = totalExp - curExp;
			if(labGuildExpDes != null)labGuildExpDes.text = (guildLevel < guildMaxLevel)?string.Format(guildExpDes,guildLevel,expDiff,guildLevel+1):string.Format(guildMaxLevelDes,guildLevel);
			if(labGuildExpNum != null)labGuildExpNum.text = curExp+"/"+totalExp;
			if(pbGuildExp != null)pbGuildExp.value = (float)curExp/(float)totalExp;
            if (canGetFx != null) canGetFx.SetActive(GameCenter.guildMng.HaveRewardCanGet);
		}
	}
	void OpenWndByBtn(GameObject go)
	{
		GuildBtnType btnType =  (GuildBtnType)(int)UIEventListener.Get(go.gameObject).parameter;
		switch(btnType)
		{
		case GuildBtnType.GUILDSTORE:
            GameCenter.guildMng.NeedOpenGuildWnd = true;
			GameCenter.uIMng.SwitchToUI(GUIType.GUILDSHOP);
			break;
		case GuildBtnType.GUILDSKILL:
			GameCenter.uIMng.SwitchToUI(GUIType.GUILDSILL);
			break;
		case GuildBtnType.GUILDDONATE:
            if (GameCenter.guildMng.restDonateTimes <= 0)
            {
                GameCenter.messageMng.AddClientMsg(558);
                return;
            }
			if(donateWnd != null)donateWnd.OpenUI();
			break;
		case GuildBtnType.GUILDFIGHT:
            GameCenter.guildMng.NeedOpenGuildWnd = true;
			GameCenter.uIMng.SwitchToUI(GUIType.GUILDSIEGE);
			break;
		case GuildBtnType.GUILDPROTECT:
			GameCenter.uIMng.SwitchToUI(GUIType.GUILDPROTECT);
			break;
		case GuildBtnType.GUILDSALARY:
            //if(GameCenter.guildMng.MyGuildInfo != null && GameCenter.guildMng.MyGuildInfo.haveGotSalary)
            //{
            //    GameCenter.messageMng.AddClientMsg(304);
            //    return;
            //}
            //GameCenter.guildMng.C2S_ReqGetGuildSalary();
            GameCenter.uIMng.SwitchToUI(GUIType.GUILDACTIVE);
			break;
		case GuildBtnType.GUILDSTORAGE:
			GameCenter.uIMng.SwitchToUI(GUIType.GUILDSTORAGE);
			GameCenter.uIMng.GenGUI(GUIType.BACKPACKWND,true);
			break;
		case GuildBtnType.GUILDTRANSPORT:
			if(dartWnd != null)dartWnd.OpenUI();
			break;
		}
	}
}
