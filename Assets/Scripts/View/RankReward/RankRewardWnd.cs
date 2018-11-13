//==================================
//作者：朱素云
//日期：2016/4/29
//用途：等级奖励界面
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RankRewardWnd : GUIBase
{
    #region 数据 
    public UIButton closeBtn;
    private UIGrid grid;
    public RankRewardUi rankRewardUi;
    private List<RankRewardUi> rewardDic = new List<RankRewardUi>();
    private MainPlayerInfo MainData
    {
        get
        {
            return GameCenter.mainPlayerMng.MainPlayerInfo;
        }
    }
    /// <summary>
    /// 已经领取的等级奖励
    /// </summary>
    private Dictionary<int, int> levRewardDic
    {
        get
        {
            return GameCenter.rankRewardMng.levRewardDic;
        }
    }
    private UIScrollView scrollView;
    #endregion 

    #region 构造
    void Awake()
    { 
        GameCenter.rankRewardMng.C2S_ReqGetLevRewardInfo(RewardType.LEVREWARD);
        mutualExclusion = true;
        layer = GUIZLayer.NORMALWINDOW;
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = OnCloseWnd;
        rankRewardUi.gameObject.SetActive(false);
        grid = rankRewardUi.transform.parent.GetComponent<UIGrid>();
        if (grid != null) scrollView = grid.transform.parent.GetComponent<UIScrollView>(); 
    }
    protected override void OnOpen()
    { 
        base.OnOpen();
        ShowLevReward();
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
            GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += RereshWnd;
            GameCenter.rankRewardMng.OnLevRewardUpdate += ShowLevReward; 
        }
        else
        {
            GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= RereshWnd;
            GameCenter.rankRewardMng.OnLevRewardUpdate -= ShowLevReward; 
        }
    }
    #endregion 

    #region 控件事件
    void OnCloseWnd(GameObject go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.NONE); 
    } 
    #endregion

    #region 刷新 
    void RereshWnd(ActorBaseTag tag, ulong value, bool _fromAbility)
    {
        if (tag == ActorBaseTag.Level)
        { 
            ShowLevReward();
        } 
    }
    void ShowLevReward()
    {
        for (int i = 0, max = rewardDic.Count; i < max; i++)
        {
            rewardDic[i].gameObject.SetActive(false);
        }
        List<LevelRewardLevelRef> stepDic = GameCenter.rankRewardMng.rewardList;  
        for (int i = 0,max = stepDic.Count; i < max; i++) 
        {
            int id = stepDic[i].level;
            if (rewardDic.Count < i + 1)
            {
                rewardDic.Add(rankRewardUi.CreateNew(grid.transform, i));
            } 
            rewardDic[i].gameObject.SetActive(true);
            rewardDic[i].Show(stepDic[i]); 
        } 
        grid.repositionNow = true; 
    } 
    #endregion
}
