//===============================
//作者：邓成
//日期：2016/4/26
//用途：无量圣地副本显示类
//===============================

using UnityEngine;
using System.Collections;

public class HolyLandCoppyWnd : SubWnd {
	public UITimer timer;
	public UILabel monsterNum;
	public UILabel maxHitNum;//最大斩击

	public UILabel curHitNum;
	public UILabel addAttrNum;

	protected override void OnOpen ()
	{
		base.OnOpen ();
		InitWnd();
		ShowMonsterNum();
		ShowTime();
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
			GameCenter.dungeonMng.OnDGKillMonsterUpdate += ShowMonsterNum;
		}else
		{
			GameCenter.dungeonMng.OnDungeonTimeUpdate -= ShowTime;
			GameCenter.dungeonMng.OnDGKillMonsterUpdate -= ShowMonsterNum;
		}
	}
	void ShowTime()
	{
		int time = GameCenter.dungeonMng.DungeonTime;
		if(timer != null)timer.StartIntervalTimer(time);
	}
	void ShowMonsterNum()
	{
		if(GameCenter.dungeonMng.DungeonKillMonster != null)
		{
			st.net.NetBase.boss_count boss = GameCenter.dungeonMng.DungeonKillMonster.Count > 0?GameCenter.dungeonMng.DungeonKillMonster[0]:null;
			if(boss != null)
			{
				if(monsterNum != null)monsterNum.text = boss.cur_num.ToString()+"/"+boss.amount.ToString();
			}
			if(maxHitNum != null)maxHitNum.text = GameCenter.dungeonMng.MaxEvenKill.ToString();
			if(curHitNum != null)curHitNum.text = GameCenter.dungeonMng.CurEvenKill.ToString();
			//int addAttr = 5*((GameCenter.dungeonMng.CurEvenKill/8 > 20)?20:GameCenter.dungeonMng.CurEvenKill/8);//属性最多加100%
			//if(addAttrNum != null)addAttrNum.text = addAttr.ToString();
		}
	}
	void InitWnd()
	{
		if(maxHitNum != null)maxHitNum.text = "0";
		if(curHitNum != null)curHitNum.text = "0";
		//if(addAttrNum != null)addAttrNum.text = "0";
	}
}
