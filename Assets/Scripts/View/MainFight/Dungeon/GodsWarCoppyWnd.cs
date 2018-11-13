//===============================
//作者：邓成
//日期：2016/4/26
//用途：封神战副本显示类
//===============================

using UnityEngine;
using System.Collections;

public class GodsWarCoppyWnd : SubWnd {
	public UILabel curLayerNum;
	public UILabel upLevCondition;
	public UILabel downLevCondition;

	public GameObject tipGo;
	public UITimer timerDown;

	protected override void OnOpen ()
	{
		base.OnOpen ();
		if(tipGo != null)tipGo.SetActive(false);
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
			GameCenter.dungeonMng.OnDGLayerUpdate += ShowLayer;
			GameCenter.dungeonMng.OnDGScoreUpdate += ShowScore;
			GameCenter.dungeonMng.OnDGDeadTimesUpdate += ShowDeadTimes;
			GameCenter.activityMng.OnGotUpDownStateEvent += ShowUpTip;
		}else
		{
			GameCenter.dungeonMng.OnDGLayerUpdate -= ShowLayer;
			GameCenter.dungeonMng.OnDGScoreUpdate -= ShowScore;
			GameCenter.dungeonMng.OnDGDeadTimesUpdate -= ShowDeadTimes;
			GameCenter.activityMng.OnGotUpDownStateEvent -= ShowUpTip;
		}
	}
	void ShowLayer()
	{
		if(curLayerNum != null)curLayerNum.text = GameCenter.dungeonMng.DungeonLayer.ToString()+"/"+GameCenter.dungeonMng.DungeonMaxLayer;
	}
	void ShowScore()
	{
		if(upLevCondition != null)upLevCondition.text = GameCenter.dungeonMng.CurDGScore.ToString()+"/"+GameCenter.dungeonMng.MaxDGScore;
	}
	void ShowDeadTimes()
	{
		int max = GameCenter.dungeonMng.DungeonMaxDeadTimes;
		if(downLevCondition != null)downLevCondition.text = (max>999)?ConfigMng.Instance.GetUItext(297):(GameCenter.dungeonMng.DungeonDeadTimes.ToString()+"/"+max.ToString());
	}
	void ShowUpTip()
	{
		if(tipGo != null)tipGo.SetActive(true);
		if(timerDown != null)
		{
			timerDown.StartIntervalTimer(GameCenter.activityMng.countDownTime);
			timerDown.onTimeOut = (x)=>
			{
				if(tipGo != null)tipGo.SetActive(false);
			};
		}
	}
}
