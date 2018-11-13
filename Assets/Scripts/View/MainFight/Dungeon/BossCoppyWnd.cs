//============================
//作者：邓成
//日期：2017/5/4
//用途：BOSS副本显示界面类
//============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossCoppyWnd : SubWnd {
    public UITimer timer;
    public UILabel labAddAttrNum;
    public GameObject btnAddAttr;
    public BossCoppyBuyAttrUI buyAttrUI;
    public GameObject maxAttrGo;
    public UILabel remainBossNum;
    public UIScrollView scrollView;
    public UIGrid grid;
    public BossListSingle bossSingle;
    public List<BossListSingle> bossList = new List<BossListSingle>();

    void Awake()
    {
        if (btnAddAttr != null) UIEventListener.Get(btnAddAttr).onClick = AddAttr;
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        ShowTime();
    }
    protected override void OnClose()
    {
        base.OnClose();
        bossList.Clear();
    }

    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            GameCenter.bossChallengeMng.OnBossCoppyDataUpdateEvent += Refresh;
            GameCenter.dungeonMng.OnDungeonTimeUpdate += ShowTime;
        }
        else
        {
            GameCenter.bossChallengeMng.OnBossCoppyDataUpdateEvent -= Refresh;
            GameCenter.dungeonMng.OnDungeonTimeUpdate -= ShowTime;
        }
    }
    void ShowTime()
    {
        int time = GameCenter.dungeonMng.DungeonTime;
        if (timer != null) timer.StartIntervalTimer(time);
    }

    void Refresh()
    {
        for (int i = 0; i < bossList.Count; i++)
        {
            bossList[i].gameObject.SetActive(false);
        }
        List<st.net.NetBase.boss_copy_list> bossCopyList = GameCenter.bossChallengeMng.bossList;
        int index = 0;
        for (int i = 0, max = bossCopyList.Count; i < max; i++)
        {
            st.net.NetBase.boss_copy_list item = bossCopyList[i];
            if (bossList.Count < index + 1)
            {
                bossList.Add(bossSingle.CreateNew(grid.transform, index));
            }
            bossList[index].gameObject.SetActive(true);
            bossList[index].SetBossCopyItemInfo(item);
            UIEventListener.Get(bossList[index].gameObject).onClick -= OnClickMoveToMobBtn;
            UIEventListener.Get(bossList[index].gameObject).onClick += OnClickMoveToMobBtn;
            UIEventListener.Get(bossList[index].gameObject).parameter = item;
            index++;
        }
        if (labAddAttrNum != null) labAddAttrNum.text = (GameCenter.bossChallengeMng.CurBossCoppyAttrNum*10).ToString();// +"%";
        if (remainBossNum != null) remainBossNum.text = GameCenter.bossChallengeMng.RemainBossCount.ToString();
        if (btnAddAttr != null) btnAddAttr.SetActive(GameCenter.bossChallengeMng.CurBossCoppyAttrNum != 10);
        if (maxAttrGo != null) maxAttrGo.SetActive(GameCenter.bossChallengeMng.CurBossCoppyAttrNum == 10);
    }

    void AddAttr(GameObject go)
    {
    //    GameCenter.bossChallengeMng.C2S_ReqAddBossCoppyData(2,1);
        if (buyAttrUI != null) buyAttrUI.SetToBuyShow(GameCenter.bossChallengeMng.CurBossCoppyAttrNum);
    }

    void OnClickMoveToMobBtn(GameObject go)
    { 
        GameCenter.curMainPlayer.commandMng.CancelCommands();
        st.net.NetBase.boss_copy_list info = UIEventListener.Get(go).parameter as st.net.NetBase.boss_copy_list;
        if (info != null)
        {
            BossRef mob = ConfigMng.Instance.GetBossRefByID((int)info.boss_id);
            if (mob != null)
            {
                GameCenter.curMainPlayer.GoTraceTarget(mob.sceneID, mob.sceneX, mob.sceneY);
            }     
        }
    }
}
