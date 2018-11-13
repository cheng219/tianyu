//===============================
//作者：邓成
//日期：2016/4/26
//用途：镇魔塔副本显示类
//===============================

using UnityEngine;
using System.Collections;

public class TowerCoppyWnd : SubWnd {
	public UILabel curLayerNum;
	public UITimer timer;
	public UILabel labAlive;
    //public UILabel labGotReward;
    //public UILabel labDontGotReward;

	public GameObject tipGo;
	public UITimer tipGoTimer;
	public UILabel tipGoCloseT;
	//public UILabel tipGoNum;
	public UITimer timerCountDown;

    public UILabel totalStar;//累计获得星星
    public UILabel totalDropReward;//累计掉落奖励
    public UISprite[] star;

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
			GameCenter.dungeonMng.OnDGWaveUpdate += ShowWave;
			GameCenter.dungeonMng.OnDGReliveTimesUpdate += ShowRelive;
			//GameCenter.dungeonMng.OnTowerRewardStateEvent += ShowRewardState;
			GameCenter.duplicateMng.OnMagicTowerUIOpen += ShowMagicTower;
            GameCenter.dungeonMng.OnTowerTotalStarEvent += ShowTotalStar;
            GameCenter.dungeonMng.OnTowerTotalDropEvent += ShowTotalDrop; 
		}else
		{
			GameCenter.dungeonMng.OnDungeonTimeUpdate -= ShowTime;
			GameCenter.dungeonMng.OnDGWaveUpdate -= ShowWave;
			GameCenter.dungeonMng.OnDGReliveTimesUpdate -= ShowRelive;
			//GameCenter.dungeonMng.OnTowerRewardStateEvent -= ShowRewardState;
			GameCenter.duplicateMng.OnMagicTowerUIOpen -= ShowMagicTower;
            GameCenter.dungeonMng.OnTowerTotalStarEvent -= ShowTotalStar;
            GameCenter.dungeonMng.OnTowerTotalDropEvent -= ShowTotalDrop;
		}
	}
	void ShowMagicTower(int _star,int time){
		if(tipGo != null)tipGo.SetActive(true);
		if(tipGoCloseT != null)tipGoCloseT.text = time.ToString();
		//if(tipGoNum != null)tipGoNum.text = num.ToString();
		if(tipGoTimer != null)
		{
			tipGoTimer.StopTimer();
			tipGoTimer.StartIntervalTimer(3);
			tipGoTimer.onTimeOut = delegate {
				tipGo.SetActive(false);
			};
		}
        for (int i = 0, max = star.Length; i < max; i++)
        {
            if (_star > i)
            {
                star[i].gameObject.SetActive(true);
            }
            else
            {
                star[i].gameObject.SetActive(false);
            }
        }
	}
	void ShowTime()
	{
		int time = GameCenter.dungeonMng.DungeonTime;
		if(timer != null)timer.StartIntervalTimer(time);
	}
	void ShowWave()
	{
		if(curLayerNum != null)curLayerNum.text = GameCenter.dungeonMng.DungeonWave.ToString()+"/"+GameCenter.dungeonMng.DungeonMaxWave;
	}
	void ShowRelive()
	{
		if(labAlive != null)labAlive.text = GameCenter.dungeonMng.DungeonReliveTimes + "/3";
	}
    //void ShowRewardState()
    //{
    //    if(labDontGotReward != null)labDontGotReward.enabled = !GameCenter.dungeonMng.HaveGotTowerReward;
    //    if(labGotReward != null)labGotReward.enabled = GameCenter.dungeonMng.HaveGotTowerReward;
    //}
    void ShowTotalStar()
    {
        if (totalStar != null) totalStar.text = GameCenter.dungeonMng.totalSatr.ToString();  
    }
    void ShowTotalDrop()
    {
        if (totalDropReward != null) totalDropReward.text = GameCenter.dungeonMng.totalDropInteger.ToString();
    }
}
