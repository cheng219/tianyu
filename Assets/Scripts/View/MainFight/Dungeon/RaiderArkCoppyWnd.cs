//======================================================
//作者:鲁家旗
//日期:2016/11/25
//用途:夺宝奇兵副本显示类
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class RaiderArkCoppyWnd : SubWnd {
    public UILabel whiteOrb;
    public UILabel greenOrb;
    public UILabel blueOrb;
    public UILabel violetOrb;
    public UIButton npcBtn;
    public UITimer timer;
    public RaiderArkEndUI raiderArkEndUI;

    void Start()
    {
        if (npcBtn != null) UIEventListener.Get(npcBtn.gameObject).onClick = delegate {
            GameCenter.taskMng.PublicTraceToNpc(500072);
        };
    }
    protected override void OnOpen()
    {
        base.OnOpen();
    }
    protected override void OnClose()
    {
        base.OnClose();
    }
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            GameCenter.dungeonMng.OnDungeonTimeUpdate += ShowTime;
            GameCenter.activityMng.OnStopCollect += GoNpc;
            GameCenter.activityMng.OnUpdateJewelry += RefrshRaiderArk;
            GameCenter.activityMng.OnGetReward += RefrshReward;
        }
        else
        {
            GameCenter.dungeonMng.OnDungeonTimeUpdate -= ShowTime;
            GameCenter.activityMng.OnStopCollect -= GoNpc;
            GameCenter.activityMng.OnUpdateJewelry -= RefrshRaiderArk;
            GameCenter.activityMng.OnGetReward -= RefrshReward;
        }
    }
    void ShowTime()
    {
        int time = GameCenter.dungeonMng.DungeonTime;
        if (timer != null) timer.StartIntervalTimer(time);
    }
    void GoNpc()
    {
        GameCenter.taskMng.PublicTraceToNpc(500072);
    }
    void RefrshRaiderArk()
    {
        List<jewelry_list> list = GameCenter.activityMng.jewelryList;
        for (int i = 0, max = list.Count; i < max; i++)
        {
            switch (list[i].jewelry_id)
            {
                case 1:
                    if (whiteOrb != null) whiteOrb.text = list[i].num + "/40";
                    break;
                case 2:
                    if (greenOrb != null) greenOrb.text = list[i].num.ToString();
                    break;
                case 3:
                    if (blueOrb != null) blueOrb.text = list[i].num.ToString();
                    break;
                case 4:
                    if (violetOrb != null) violetOrb.text = list[i].num.ToString();
                    break;
            }
        }
    }
    void RefrshReward(List<reward_list> _list)
    {
        if (raiderArkEndUI != null)
        {
            raiderArkEndUI.gameObject.SetActive(true);
            raiderArkEndUI.Refresh(_list);
        }
    }
}
