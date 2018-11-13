//===============================
//作者：邓成
//日期：2016/4/26
//用途：封印BOSS副本显示类
//===============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class SealBossCoppyWnd : SubWnd {
	public UITimer timer;
	public UILabel labAlive;
	public UILabel labMonsterNum;
	protected override void OnOpen ()
	{
		base.OnOpen ();
		ShowRelive();
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
			GameCenter.dungeonMng.OnDGReliveTimesUpdate += ShowRelive;
			GameCenter.dungeonMng.OnDGMonsterUpdate += ShowMonsterNum;
		}else
		{
			GameCenter.dungeonMng.OnDungeonTimeUpdate -= ShowTime;
			GameCenter.dungeonMng.OnDGReliveTimesUpdate -= ShowRelive;
			GameCenter.dungeonMng.OnDGMonsterUpdate -= ShowMonsterNum;
		}
	}
	void ShowTime()
	{
		int time = GameCenter.dungeonMng.DungeonTime;
		if(timer != null)timer.StartIntervalTimer(time);
	}
	void ShowRelive()
	{
		SceneRef sceneRef = GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef;
		if(sceneRef != null)
		{
			if(labAlive != null)labAlive.text = GameCenter.dungeonMng.DungeonReliveTimes + "/"+sceneRef.reviveNum;
		}
	}
	void ShowMonsterNum()
	{
		if(labMonsterNum != null)labMonsterNum.text = GameCenter.dungeonMng.DungeonMonsterNum.ToString();
	}
}
