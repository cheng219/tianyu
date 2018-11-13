//===============================
//作者：邓成
//日期：2016/4/26
//用途：死亡荒漠副本显示类
//===============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class DesertCoppyWnd : SubWnd {
	public UITimer timer;
	public UILabel labAlive;
	public UILabel labWave;
	public UIProgressBar waveProgress;

	public UILabel labBossName;
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
			GameCenter.dungeonMng.OnDGReliveTimesUpdate += ShowRelive;
			GameCenter.dungeonMng.OnDGKillMonsterUpdate += ShowMonster;
			GameCenter.dungeonMng.OnDGWaveUpdate += ShowWaveProgress;
		}else
		{
			GameCenter.dungeonMng.OnDungeonTimeUpdate -= ShowTime;
			GameCenter.dungeonMng.OnDGReliveTimesUpdate -= ShowRelive;
			GameCenter.dungeonMng.OnDGKillMonsterUpdate -= ShowMonster;
			GameCenter.dungeonMng.OnDGWaveUpdate -= ShowWaveProgress;
		}
	}
	void ShowTime()
	{
		int time = GameCenter.dungeonMng.DungeonTime;
		if(timer != null)timer.StartIntervalTimer(time);
	}
	void ShowRelive()
	{
		int reliveTimes = GameCenter.dungeonMng.DungeonReliveTimes;
		if(labAlive != null)labAlive.text = reliveTimes + "/3";
	}
	void ShowWaveProgress()
	{
		DungeonMng dungeonMng = GameCenter.dungeonMng;
		if(labWave != null)labWave.text = dungeonMng.DungeonWave+"/"+dungeonMng.DungeonMaxWave;
		if(waveProgress != null)waveProgress.value = (float)dungeonMng.DungeonWave/(float)dungeonMng.DungeonMaxWave;
	}
	void ShowMonster()
	{
		List<boss_count> killMob = GameCenter.dungeonMng.DungeonKillMonster;
		System.Text.StringBuilder builder = new System.Text.StringBuilder();
		for (int i = 0,max=killMob.Count; i < max; i++) 
		{
			boss_count boss = killMob[i];
			MonsterRef mon = ConfigMng.Instance.GetMonsterRef(boss.monster_type);
			builder.Append(ConfigMng.Instance.GetUItext(296)).Append(mon == null?string.Empty:mon.name).Append("(").Append(boss.cur_num).Append("/").Append(boss.amount).Append(")");
			if(i < max)builder.Append("\n");
		}
		if(labBossName != null)labBossName.text = builder.ToString();
	}
}
