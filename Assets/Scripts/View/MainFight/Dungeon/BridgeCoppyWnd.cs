//===============================
//作者：邓成
//日期：2016/4/26
//用途：断魂桥副本显示类
//===============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class BridgeCoppyWnd : SubWnd {
	public UILabel labWave;
	public UITimer timer;
	public UILabel labHp;
	public UIButton[] btnHelp;//守护
	public GameObject[] btnFx;
	public GameObject normalWave;
	public GameObject specialWave;

	protected override void OnOpen ()
	{
		base.OnOpen ();
		ShowTime();
		ShowWave();
		SceneRef scene = GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef;
		if(scene != null)
		{
			if(normalWave != null)normalWave.SetActive(scene.uiType == (int)SceneUiType.BRIDGE && scene.id != 110311);
			if(specialWave != null)specialWave.SetActive(scene.uiType == (int)SceneUiType.BRIDGE && scene.id == 110311);
		}
	}
	protected override void OnClose ()
	{
		base.OnClose ();
		if(mobInfo != null)mobInfo.OnBaseUpdate -= RefreshHp;
	}
	protected override void HandEvent (bool _bind)
	{
		base.HandEvent (_bind);
		if(_bind)
		{
			GameCenter.dungeonMng.OnDungeonTimeUpdate += ShowTime;
			GameCenter.dungeonMng.OnDGWaveUpdate += ShowWave;
			GameCenter.dungeonMng.OnBridgeBossListUpdateEvent += ShowBridgeBoss;
			GameCenter.dungeonMng.OnBridgeNPCUpdateEvent += SetBridgeNPC;
		}else
		{
			GameCenter.dungeonMng.OnDungeonTimeUpdate -= ShowTime;
			GameCenter.dungeonMng.OnDGWaveUpdate -= ShowWave;
			GameCenter.dungeonMng.OnBridgeBossListUpdateEvent -= ShowBridgeBoss;
			GameCenter.dungeonMng.OnBridgeNPCUpdateEvent -= SetBridgeNPC;
		}
	}
	void ShowTime()
	{
		int time = GameCenter.dungeonMng.DungeonTime;
		if(timer != null)timer.StartIntervalTimer(time);
	}
	void ShowWave()
	{
		if(labWave != null)labWave.text = GameCenter.dungeonMng.DungeonWave.ToString()+"/"+GameCenter.dungeonMng.DungeonMaxWave;
	}
	void ShowBridgeBoss()
	{
		List<call_boss_list> bossList = GameCenter.dungeonMng.BridgeBossList;
		for (int i = 0,max=bossList.Count; i < max; i++) 
		{
			bool canCall = (bossList[i].call_state == 1);
			if(btnHelp != null && btnHelp.Length > i && btnHelp[i] != null)
			{
				if(btnHelp[i] != null)
				{
					btnHelp[i].isEnabled = canCall;//1表示可以召唤
					UIEventListener.Get(btnHelp[i].gameObject).parameter = bossList[i].boss_id;
					UIEventListener.Get(btnHelp[i].gameObject).onClick = ClickCallBoss;
				}
			}
			if(btnFx != null && btnFx.Length > i && btnFx[i] != null)
			{
				if(btnFx[i] != null)btnFx[i].SetActive(canCall);
			}
		}
	}

	MonsterInfo mobInfo = null;
	void SetBridgeNPC()
	{
		if(GameCenter.dungeonMng.BridgeNpcID != 0)
		{
			if(mobInfo != null)
			{
				mobInfo.OnBaseUpdate -= RefreshHp;
			}
			mobInfo = GameCenter.sceneMng.GetMobInfo(GameCenter.dungeonMng.BridgeNpcID);
			if(mobInfo != null)
			{
				RefreshHp();
				mobInfo.OnBaseUpdate += RefreshHp;
			}
		}
	}
	void RefreshHp(ActorBaseTag tag,ulong val,bool state)
	{
		if(mobInfo != null && labHp != null)
		{
			labHp.text = mobInfo.CurHPText+"/"+mobInfo.MaxHPText;	
		}
	}
	void RefreshHp()
	{
		if(mobInfo != null && labHp != null)
		{
			labHp.text = mobInfo.CurHPText+"/"+mobInfo.MaxHPText;	
		}
	}
	void ClickCallBoss(GameObject go)
	{
		int bossId = (int)UIEventListener.Get(go.gameObject).parameter;
		GameCenter.dungeonMng.C2S_ReqCallBridgeBoss(bossId);
	}
}