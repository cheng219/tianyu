//===============================
//作者：邓成
//日期：2016/4/26
//用途：无尽副本显示类
//===============================

using UnityEngine;
using System.Collections;

public class EndlessCoppyWnd : SubWnd {
	public UILabel curLayerNum;
	public UITimer timer;
	public UILabel labMonsterNum;
	public UILabel labCapterName;

	protected override void OnOpen ()
	{
		base.OnOpen ();
		if(labCapterName != null)
		{
			CheckPointRef pointRef = ConfigMng.Instance.GetCheckPointRef(GameCenter.mainPlayerMng.MainPlayerInfo.SceneID);
			if(pointRef != null)
				labCapterName.text = pointRef.name;
		}
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
			GameCenter.duplicateMng.OnOpenCopySettlement += StopTimer;
		}else
		{
			GameCenter.dungeonMng.OnDungeonTimeUpdate -= ShowTime;
			GameCenter.dungeonMng.OnDGMonsterUpdate -= ShowMonsterNum;
			GameCenter.duplicateMng.OnOpenCopySettlement -= StopTimer;
		}
	}
	void ShowTime()
	{
		int time = GameCenter.dungeonMng.DungeonTime;
		if(timer != null)timer.StartIntervalTimer(time);
	}
	void ShowMonsterNum()
	{
		if(labMonsterNum != null)labMonsterNum.text = GameCenter.dungeonMng.DungeonMonsterNum.ToString();
	}
	void StopTimer()
	{
		if(timer != null)timer.StopTimer();
	}
}
