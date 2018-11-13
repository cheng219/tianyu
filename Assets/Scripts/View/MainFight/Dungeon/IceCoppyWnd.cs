//===============================
//作者：邓成
//日期：2016/4/26
//用途：寒冰炼狱副本显示类
//===============================

using UnityEngine;
using System.Collections;

public class IceCoppyWnd : SubWnd {
	public UILabel curLayerNum;
	public UITimer timer;
	public UILabel monsterNum;

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
			GameCenter.dungeonMng.OnDGMonsterUpdate += ShowMonsterNum;
			GameCenter.dungeonMng.OnDGLayerUpdate += ShowLayer;
		}else
		{
			GameCenter.dungeonMng.OnDungeonTimeUpdate -= ShowTime;
			GameCenter.dungeonMng.OnDGMonsterUpdate -= ShowMonsterNum;
			GameCenter.dungeonMng.OnDGLayerUpdate -= ShowLayer;
		}
	}
	void ShowTime()
	{
		int time = GameCenter.dungeonMng.DungeonTime;
		if(timer != null)timer.StartIntervalTimer(time);
	}
	void ShowMonsterNum()
	{
		if(monsterNum != null)monsterNum.text = GameCenter.dungeonMng.DungeonMonsterNum.ToString();
	}
	void ShowLayer()
	{
		if(curLayerNum != null)curLayerNum.text = GameCenter.dungeonMng.DungeonLayer.ToString();
	}
}
