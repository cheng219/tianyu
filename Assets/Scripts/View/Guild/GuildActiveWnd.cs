//===============================
//作者：邓成
//日期：2016/5/5
//用途：仙盟活跃界面类
//===============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class GuildActiveWnd : GUIBase {
    public UIButton btnClose;
    public UIGrid itemParent;
    public UIScrollView scrollView;
    public DailyMustDoItemUI dailyMustDoItemUI;
    public LivelyRewardItemUI[] livelyItems;
    public UIProgressBar livelyProgress;
    public UILabel livelyCount;
    public UILabel myLivelyCount;
    public LivelyRewardBoxUI boxUI;

    public UIGrid rankParent;
    public UIScrollView rankScrollView;
    public GuildLivelyRankItemUI rankItem;

    protected List<DailyMustDoItemUI> mustDoItemList = new List<DailyMustDoItemUI>();
    protected List<GuildLivelyRankItemUI> rankItemList = new List<GuildLivelyRankItemUI>();
    void Awake()
    {
        layer = GUIZLayer.NORMALWINDOW;
        mutualExclusion = true;
        if (btnClose != null) UIEventListener.Get(btnClose.gameObject).onClick = CloseWnd;
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        Refresh();
        GameCenter.guildMng.C2S_ReqLivelyData();
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
            GameCenter.guildMng.OnGotGuildLivelyDataEvent += Refresh;
            GameCenter.guildMng.OnGotGuildLivelyRewardEvent += ShowLivelyData;
        }
        else
        {
            GameCenter.guildMng.OnGotGuildLivelyDataEvent -= Refresh;
            GameCenter.guildMng.OnGotGuildLivelyRewardEvent -= ShowLivelyData;
        }
    }

    void Refresh()
    {
        pt_guild_liveness_info_d50e guildLivelyData = GameCenter.guildMng.guildLivelyData;
        List<GuildLivelyInfo> itemList = new List<GuildLivelyInfo>();
        FDictionary guildLivelyDic = ConfigMng.Instance.GetGuildLivelyRefTable();
        foreach (var item in guildLivelyDic.Values)
        {
            GuildLivelyInfo info = null;
            GuildLivelyRef refData = item as GuildLivelyRef;
            if (guildLivelyData != null)
            {
                for (int i = 0, length = guildLivelyData.task_list.Count; i < length; i++)
                {
                    if (refData.id == guildLivelyData.task_list[i].task_id)
                    {
                        info = new GuildLivelyInfo(guildLivelyData.task_list[i]);
                        break;
                    }
                }
            }
            if (info == null) info = new GuildLivelyInfo(refData.id);
            itemList.Add(info);
        }
        ShowMustDoItems(itemList);
        if (guildLivelyData != null)
        {
            if (myLivelyCount != null) myLivelyCount.text = guildLivelyData.liveness_self.ToString();
            if (livelyCount != null) livelyCount.text = guildLivelyData.liveness_guild.ToString();
            ShowRankItems(guildLivelyData.member_info_list);
        }
        ShowLivelyData();
        
    }

    void ShowMustDoItems(List<GuildLivelyInfo> itemList)
    {
        for (int i = 0; i < mustDoItemList.Count; i++)
        {
            mustDoItemList[i].gameObject.SetActive(false);
        }
        for (int i = 0, max = itemList.Count; i < max; i++)
        {
            if (mustDoItemList.Count < i + 1)
            {
                mustDoItemList.Add(dailyMustDoItemUI.CreateNew(itemParent.transform));
            }
            mustDoItemList[i].gameObject.SetActive(true);
            mustDoItemList[i].SetData(itemList[i]);
        }
        if (itemParent != null) itemParent.repositionNow = true;
        if (scrollView != null) scrollView.SetDragAmount(0, 0, false);
        
    }
    void ShowLivelyData()
    {
        if (livelyItems != null)
        {
            FDictionary livelyReward = ConfigMng.Instance.GetGuildLivelyRewardRefTable();
            int index = 0;
            foreach (GuildLivelyRewardRef reward in livelyReward.Values)
            {
                if (livelyItems.Length > index && livelyItems[index] != null)
                {
                    livelyItems[index].SetData(reward, ShowLivelyBoxReward);
                }
                index++;
            }
        }
        int maxCount = ConfigMng.Instance.GetGuildLivelyMaxCount();
        if (livelyProgress != null) livelyProgress.value = Mathf.Min(1f, (float)GameCenter.guildMng.CurLivelyCount / (float)maxCount);
        if (livelyCount != null) livelyCount.text = GameCenter.guildMng.CurLivelyCount.ToString() + "/" + maxCount;
    }

    void ShowLivelyBoxReward(GuildLivelyRewardRef _livelyRewardRef)
    {
        if (boxUI != null) boxUI.SetData(_livelyRewardRef);
    }

    void ShowRankItems(List<guild_liveness_member_info> itemList)
    {
        for (int i = 0; i < rankItemList.Count; i++)
        {
            rankItemList[i].gameObject.SetActive(false);
        }
        for (int i = 0, max = itemList.Count; i < max; i++)
        {
            if (rankItemList.Count < i + 1)
            {
                rankItemList.Add(rankItem.CreateNew(rankParent.transform));
            }
            rankItemList[i].gameObject.SetActive(true);
            rankItemList[i].SetData(itemList[i]);
        }
        if (rankParent != null) rankParent.repositionNow = true;
        if (rankScrollView != null) rankScrollView.SetDragAmount(0, 0, false);
    }

    void CloseWnd(GameObject go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.GUILDMAIN);
    }
}
