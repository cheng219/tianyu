//===============================
//作者：邓成
//日期：2016/4/26
//用途：仙盟篝火显示类
//===============================

using UnityEngine;
using System.Collections;

public class GuildFireCoppyWnd : SubWnd {
	public UILabel labDes;
	public UITimer timer;
	public UILabel labExp;
	public UILabel labStone;
	public UILabel labExpPercent;
	public UILabel labGuildName;
	public UIButton btnOtherGuild;
	public GameObject tipGo;

	protected bool showTipGo = false;

	void Awake()
	{
		if(btnOtherGuild != null)UIEventListener.Get(btnOtherGuild.gameObject).onClick = OpenOtherGuild;
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
			GameCenter.dungeonMng.OnDungeonTimeUpdate += ShowTime;
			GameCenter.dungeonMng.OnGuildFireUpdateEvent += ShowFireInfo;
			GameCenter.dungeonMng.OnGuildFireNameUpdateEvent += ShowFireTip;
		}else
		{
			GameCenter.dungeonMng.OnDungeonTimeUpdate -= ShowTime;
			GameCenter.dungeonMng.OnGuildFireUpdateEvent -= ShowFireInfo;
			GameCenter.dungeonMng.OnGuildFireNameUpdateEvent -= ShowFireTip;
		}
	}
	void OpenOtherGuild(GameObject go)
	{
		GameCenter.uIMng.SwitchToUI(GUIType.GUILDBONFIRE);
	}
	void ShowTime()
	{
		int time = GameCenter.dungeonMng.DungeonTime;
		if(timer != null)timer.StartIntervalTimer(time);
	}
	void ShowFireInfo()
	{
		DungeonMng dungeonMng = GameCenter.dungeonMng;
		if(labExp != null)labExp.text = dungeonMng.GuildFireExp.ToString();
		if(labExpPercent != null)labExpPercent.text = dungeonMng.GuildFireExpPercent.ToString()+"%";
		if(labStone != null)labStone.text = dungeonMng.GuildFireStoneCurHp+"/"+dungeonMng.GuildFireStoneMaxHp;
		if(labGuildName != null)labGuildName.text = dungeonMng.GuildFireCurGuildName;
	}
	void ShowFireTip()
	{
		DungeonMng dungeonMng = GameCenter.dungeonMng;
		if(!showTipGo && dungeonMng.GuildFireCurGuildName.Equals(GameCenter.mainPlayerMng.MainPlayerInfo.GuildName))
		{
			if(tipGo != null)tipGo.SetActive(true);//仙盟篝火的提示只在自己公会显示一次
			showTipGo = true;
		}else
		{
			if(tipGo != null)tipGo.SetActive(false);
		}
	}
}
