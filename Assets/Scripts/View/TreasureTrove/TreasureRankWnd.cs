//==================================
//作者：邓成
//日期：2017/4/6
//用途：宝藏活动(排行)至尊豪礼界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreasureRankWnd : SubWnd {
    public TreasureRankRewardUI[] rewardItems;
    public TreasureRankItemUI myRankItemUI;
    public TreasureRankItemUI otherRankItemUI;
    public UIScrollView scrollView;
    public UIGrid grid;
    public UITimer time;
    protected List<TreasureRankItemUI> rankItems = new List<TreasureRankItemUI>();
    protected override void OnOpen()
    {
        base.OnOpen();
        ClearItem();
        GameCenter.treasureTroveMng.C2S_ReqTreasureRankReward();
        GameCenter.treasureTroveMng.C2S_ReqTreasureRank(15, 1);
    }
    protected override void OnClose()
    {
        ClearItem();
        base.OnClose();
    }
    protected override void HandEvent(bool _bind)
    {
        if (_bind)
        {
            GameCenter.treasureTroveMng.updateTreasureRank += ShowReward;
            GameCenter.treasureTroveMng.updateTreasureRankInfo += ShowRank;
        }
        else
        {
            GameCenter.treasureTroveMng.updateTreasureRank -= ShowReward;
            GameCenter.treasureTroveMng.updateTreasureRankInfo -= ShowRank;
        }
    }

    void RefreshRankWnd()
    {
        ShowReward();
        ShowRank();
    }
    void ShowReward()
    {
        List<List<ItemValue>> listInfo = GameCenter.treasureTroveMng.GetRankReward();
        List<string> listDes = GameCenter.treasureTroveMng.GetRankRewardDes();
        if (rewardItems != null)
        {
            if (listInfo != null && listDes != null)
            {
                for (int i = 0, len = rewardItems.Length; i < len; i++)
                {
                    rewardItems[i].refreshAll(listDes[i], listInfo[i]);
                }
            }
        }
    }

    void ShowRank()
    {
        rankItems.Clear();
        otherRankItemUI.gameObject.SetActive(true);
        List<st.net.NetBase.rank_info_base> rankList = GameCenter.treasureTroveMng.GetrankList();
        //Debug.Log("GameCenter.treasureTroveMng.OpenTimes:" + GameCenter.treasureTroveMng.OpenTimes);
        if (myRankItemUI != null) myRankItemUI.SetData(GameCenter.treasureTroveMng.Rank,GameCenter.treasureTroveMng.OpenTimes);//SetData传入后台数据做参数
        if(rewardItems!=null&& rankList!=null&& grid!=null)
        {
            for(int i=0,count = rankList.Count;i<count;i++)
            {
                //Debug.Log("名字：" + rankList[i].name);
                if (rankList[i].name.Equals(GameCenter.mainPlayerMng.MainPlayerInfo.Name))
                {
                    if (myRankItemUI != null)
                        rankItems.Add(TreasureRankItemUI.Create(grid.transform, otherRankItemUI.gameObject,Color.green));
                }
                else
                {
                    if (otherRankItemUI != null)
                    {
                        rankItems.Add(TreasureRankItemUI.CreateNew(grid.transform, otherRankItemUI.gameObject));
                    }
                }
                //rankItems[i].SetData(rankList[i]);
            }
            for(int k=0,count = rankItems.Count;k < count;k++)
            {
                rankItems[k].SetData(k+1,rankList[k]);
            }
            grid.Reposition();
        }
        if(time!=null)
        {
            //Debug.Log("GameCenter.treasureTroveMng.RankTime" + GameCenter.treasureTroveMng.RankTime);
            time.StartIntervalTimer(GameCenter.treasureTroveMng.RankTime);
        }
        //if (myRankItemUI != null)
        //{
        //    TreasureRankItemUI item = myRankItemUI.CreateNew(grid.transform);
        //}
        otherRankItemUI.gameObject.SetActive(false);
    }
    void ClearItem()
    {
        if(grid!=null)
        {
            List<Transform> list = grid.GetChildList();
            for (int i = 0, count = list.Count; i < count; i++)
            {
                //if(!list[i].gameObject.activeSelf)
                GameObject.DestroyImmediate(list[i].gameObject);
            }
        }
    }
    void OnDestroy()
    {
        ClearItem();
    }
}
