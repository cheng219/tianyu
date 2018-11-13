//===============================
//作者：邓成
//日期：2016/8/11
//用途：仙盟攻城副本显示类
//===============================

using UnityEngine;
using System.Collections;

public class GuildSiegeCoppyWnd : SubWnd {
	public GameObject fightGo;
	public UILabel labWestHp;
	public UILabel labEastHp;
	public UILabel labNorthHp;
	public GameObject westGo;
	public GameObject eastGo;
	public GameObject northGo;

	public GameObject openGo;
	public UITimer waitTimer;

	public GameObject closeGo;

	public Vector3 westMobPosition;
	public Vector3 eastMobPosition;
	public Vector3 northMobPosition;

	void Awake()
	{
		if(westGo != null)UIEventListener.Get(westGo).onClick = TraceToWest;
		if(eastGo != null)UIEventListener.Get(eastGo).onClick = TraceToEast;
		if(northGo != null)UIEventListener.Get(northGo).onClick = TraceToNorth;
	}
	void TraceToWest(GameObject go)
	{
		if(westMobPosition != Vector3.zero)
			GameCenter.curMainPlayer.GoTraceTarget(westMobPosition.x,westMobPosition.z);
	}
	void TraceToEast(GameObject go)
	{
		if(eastMobPosition != Vector3.zero)
			GameCenter.curMainPlayer.GoTraceTarget(eastMobPosition.x,eastMobPosition.z);
	}
	void TraceToNorth(GameObject go)
	{
		if(northMobPosition != Vector3.zero)
			GameCenter.curMainPlayer.GoTraceTarget(northMobPosition.x,northMobPosition.z);
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
			GameCenter.dungeonMng.OnGuildSiegeStoneUpdate += RefreshHp;
			GameCenter.dungeonMng.OnGuildSiegeStateUpdate += Refresh;
		}else
		{
			GameCenter.dungeonMng.OnGuildSiegeStoneUpdate -= RefreshHp;
			GameCenter.dungeonMng.OnGuildSiegeStateUpdate -= Refresh;
		}
	}
	void Refresh()
	{
		if(fightGo != null)fightGo.SetActive(false);
		if(openGo != null)openGo.SetActive(false);
		if(closeGo != null)closeGo.SetActive(false);
		switch(GameCenter.dungeonMng.guildSiegeState)
		{
		case GuildSiegeState.Fighting:
			if(fightGo != null)fightGo.SetActive(true);
			break;
		case GuildSiegeState.Open:
			if(openGo != null)openGo.SetActive(true);
			if(waitTimer != null)
			{
				waitTimer.StartIntervalTimer(GameCenter.dungeonMng.GuildSiegeWaitTime - (int)Time.realtimeSinceStartup);
				waitTimer.onTimeOut = (x)=>
				{
					if(openGo != null)openGo.SetActive(false);
					if(closeGo != null)closeGo.SetActive(true);
				};
			}
			break;
		case GuildSiegeState.Close:
			if(closeGo != null)closeGo.SetActive(true);
			break;
		}
	}
	void RefreshHp(int _type,int _curHp,int _maxHp)
	{
		string hp = _curHp + "/" + _maxHp;
		switch(_type)
		{
		case 1:
			if(labNorthHp != null)labNorthHp.text = hp;
			break;
		case 2:
			if(labWestHp != null)labWestHp.text = hp;
			break;
		case 3:
			if(labEastHp != null)labEastHp.text = hp;
			break;
		}
	}
}
