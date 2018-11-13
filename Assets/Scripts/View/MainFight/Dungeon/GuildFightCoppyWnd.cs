//===============================
//作者：黄洪兴
//日期：2016/5/14
//用途：仙域争霸副本显示类
//===============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class GuildFightCoppyWnd : SubWnd
{
    public UITimer timer;
    //public UILabel timer;
    public List<GuildFightRankItemUI> rankList = new List<GuildFightRankItemUI>();

	public GameObject fightPop;

	protected override void OnOpen ()
	{
		base.OnOpen ();
		if(fightPop != null)fightPop.SetActive(true);
		init();
	}
	protected override void OnClose ()
	{
		base.OnClose ();
		if(fightPop != null)fightPop.SetActive(false);
	}

    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            GameCenter.guildFightMng.OnGetGuildFightRankUpdate+= RefreshRank;
            GameCenter.guildFightMng.OnGetGuildFightTimeUpdate += RefreshTime;
        }
        else
        {
            GameCenter.guildFightMng.OnGetGuildFightRankUpdate -= RefreshRank;
            GameCenter.guildFightMng.OnGetGuildFightTimeUpdate -= RefreshTime;
        }
    }



    public void init()
    {
        RefreshRank();
        RefreshTime();

    }

    void RefreshTime()
    {
        if (timer != null)
        {
            if (GameCenter.guildFightMng.remainTime != null)
            {
                //Debug.Log("倒计时  " + GameCenter.guildFightMng.remainTime.remainTime);
                timer.StartIntervalTimer(GameCenter.guildFightMng.remainTime.remainTime);
            }
        }
    }


    void RefreshRank()
    {
        if (GameCenter.guildFightMng.ScoreList != null)
        {
            for (int i = 0; i < GameCenter.guildFightMng.ScoreList.Count; i++)
            {
                if (i + 1 > rankList.Count)
                    continue;
                rankList[i].FillInfo(GameCenter.guildFightMng.ScoreList[i]);

            }

            if (GameCenter.guildFightMng.ScoreList.Count < rankList.Count)
            {
                for (int i = GameCenter.guildFightMng.ScoreList.Count; i < rankList.Count; i++)
                {
                    rankList[i].FillInfo(null);
                }
            }
        }
        else
        {
            for (int i = 0; i < rankList.Count; i++)
            {
                rankList[i].FillInfo(null);
            }
        }
    }




    void ShowTime()
    {

    }
    void ShowRelive()
    {
       
    }

}
