//===============================
//作者：邓成
//日期：2016/4/26
//用途：灵兽岛副本显示类
//===============================

using UnityEngine;
using System.Collections;

public class PetCoppyWnd : SubWnd {
	public UITimer timer;
	public UITimer timerDead;
	public UILabel petName;

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
			GameCenter.dungeonMng.OnDGPetTimeUpdate += ShowPetTime;
		}else
		{
			GameCenter.dungeonMng.OnDungeonTimeUpdate -= ShowTime;
			GameCenter.dungeonMng.OnDGPetTimeUpdate -= ShowPetTime;
		}
	}
	void ShowTime()
	{
		int time = GameCenter.dungeonMng.DungeonTime;
		if(timer != null)timer.StartIntervalTimer(time);
	}
	void ShowPetTime()
	{
		int time = GameCenter.dungeonMng.DungeonPetTime;
		if(timerDead != null)timerDead.StartIntervalTimer(time);
		if(petName != null)petName.text = GameCenter.dungeonMng.DungeonPetName;
	}
}