//===============================
//作者：邓成
//日期：2016/5/16
//用途：仙域守护副本显示类
//===============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class GuildProtectCoppyWnd : SubWnd {
	public UILabel labWave;
	public UITimer timer;
	public UILabel labMobNum;
	public UILabel labDifficulty;
	public GameObject resultGo;
	public TweenScale winPic;
	public TweenScale losePic;

	public GameObject protectPop;

	public GuildProtectRankUI myRankUI;
	public UIGrid rankGrid;

	protected Dictionary<int,GuildProtectRankUI> allItems = new Dictionary<int, GuildProtectRankUI>();

	protected override void OnOpen ()
	{
		base.OnOpen ();
		if(protectPop != null)protectPop.SetActive(true);
		InitWnd();
	}
	protected override void OnClose ()
	{
		base.OnClose ();
		if(protectPop != null)protectPop.SetActive(false);
	}
	protected override void HandEvent (bool _bind)
	{
		base.HandEvent (_bind);
		if(_bind)
		{
			GameCenter.dungeonMng.OnDungeonTimeUpdate += ShowTime;
			GameCenter.dungeonMng.OnDGWaveUpdate += ShowWave;
			GameCenter.dungeonMng.OnDGMonsterUpdate += ShowMonsterNum;
			GameCenter.activityMng.OnGotProtectActivityRankEvent += ShowRankUI;
			GameCenter.dungeonMng.OnGuildProtectResultUpdate += ShowResult;
		}else
		{
			GameCenter.dungeonMng.OnDungeonTimeUpdate -= ShowTime;
			GameCenter.dungeonMng.OnDGWaveUpdate -= ShowWave;
			GameCenter.dungeonMng.OnDGMonsterUpdate -= ShowMonsterNum;
			GameCenter.activityMng.OnGotProtectActivityRankEvent -= ShowRankUI;
			GameCenter.dungeonMng.OnGuildProtectResultUpdate -= ShowResult;
		}
	}
	void InitWnd()
	{
		GameCenter.activityMng.C2S_ReqProtectActivityRank();
		ShowTime();
		ShowWave();
		ShowMonsterNum();
		SceneRef sceneRef = GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef;
		if(labDifficulty != null)labDifficulty.text = sceneRef==null?string.Empty:sceneRef.name;
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
	void ShowMonsterNum()
	{
		if(labMobNum != null)labMobNum.text = GameCenter.dungeonMng.DungeonMonsterNum.ToString();;
	}
	void ShowRankUI()
	{
		bool haveSelf = false;
		List<guild_guard_rank> protectActivityRankList = GameCenter.activityMng.ProtectActivityRankList;
		for (int i = 0,max=protectActivityRankList.Count; i < max; i++) {
			GuildProtectRankUI item;
			if(!allItems.TryGetValue(i,out item))
			{
				if(rankGrid != null)item = myRankUI.CreateNew(rankGrid.transform);
				allItems[i] = item;
			}
			item = allItems[i];
			if(item != null)item.SetData(protectActivityRankList[i],i+1);
			if(protectActivityRankList[i].uid == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
			{
				if(myRankUI != null)
				{
					myRankUI.SetData(protectActivityRankList[i],i+1);
					myRankUI.gameObject.SetActive(true);
				}
				haveSelf = true;
			}
		}
		if(rankGrid != null)rankGrid.repositionNow = true;
		if(!haveSelf)myRankUI.gameObject.SetActive(false);
	}
	void HideAllItems()
	{
		foreach(GuildProtectRankUI item in allItems.Values)
		{
			if(item != null)
				item.gameObject.SetActive(false);
		}
	}

	void ShowResult()
	{
		if(resultGo != null)
			resultGo.SetActive(true);
		if(winPic != null && losePic != null)
		{
			if(GameCenter.dungeonMng.GuildProtectResult == 1)
			{
				losePic.gameObject.SetActive(false);
				winPic.gameObject.SetActive(true);
				winPic.ResetToBeginning();
				winPic.AddOnFinished(()=>
					{
						if(resultGo != null)resultGo.SetActive(false);
					});
				winPic.enabled = true;
			}else
			{
				winPic.gameObject.SetActive(false);
				losePic.gameObject.SetActive(true);
				losePic.ResetToBeginning();
				losePic.AddOnFinished(()=>
					{
						if(resultGo != null)resultGo.SetActive(false);
					});
				losePic.enabled = true;
			}
		}
	}
}
