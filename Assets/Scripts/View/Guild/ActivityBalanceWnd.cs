//======================================================
//作者:鲁家旗
//日期:2016.8.2
//用途:活动结算界面
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class ActivityBalanceWnd : GUIBase{
    public UIButton closeBtn;
    public UITimer cdTimer;
    public ItemUI[] reward;
    public UILabel desLabel;
    public GameObject rewardGo;
    public GameObject notRewardGo;
    
    void Awake()
    {
        layer = GUIZLayer.TOPWINDOW;
        mutualExclusion = true;
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = delegate
        {
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
            GameCenter.duplicateMng.C2S_OutCopy();
        };
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        if (GameCenter.guildFightMng.isGuildFight)
            RefreshActivityGuildFight();
        else if (GameCenter.activityMng.isGuildStormCity)
            RefreshActivityGuildStormCity();
        else
            RefreshActivityBalance();
        if (cdTimer != null)
        {
            cdTimer.StartIntervalTimer(10);
            cdTimer.onTimeOut = delegate
            {
                GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                GameCenter.duplicateMng.C2S_OutCopy();
            };
        }
    }
    protected override void OnClose()
    {
        base.OnClose();
    }
    void RefreshActivityBalance()
    {
        if (GameCenter.activityMng.activeState == 1)
        {
            List<reward_list> rewardList = GameCenter.activityMng.rewardList;
            if (rewardGo != null) rewardGo.SetActive(true);
            if(notRewardGo != null) notRewardGo.SetActive(false);
            for (int i = 0; i < reward.Length; i++)
            {
                if (i < rewardList.Count)
                {
                    reward[i].gameObject.SetActive(true);
                    reward[i].FillInfo(new EquipmentInfo((int)rewardList[i].type, (int)rewardList[i].num, EquipmentBelongTo.PREVIEW));
                }
                else
                    reward[i].gameObject.SetActive(false);
            }
        }
        else
        {
            if (rewardGo != null) rewardGo.SetActive(false);
            if (notRewardGo != null) notRewardGo.SetActive(true);
            if (desLabel != null) desLabel.text = ConfigMng.Instance.GetUItext(89);
        }
    }
    /// <summary>
    /// 攻城战
    /// </summary>
    void RefreshActivityGuildStormCity()
    {
        if (rewardGo != null) rewardGo.SetActive(false);
        if (notRewardGo != null) notRewardGo.SetActive(true);
        if (desLabel != null) desLabel.text = ConfigMng.Instance.GetUItext(90, new string[1] { GameCenter.activityMng.vctorName });
    }
    /// <summary>
    /// 仙盟战
    /// </summary>
    void RefreshActivityGuildFight()
    {
        if (rewardGo != null) rewardGo.SetActive(false);
        if (notRewardGo != null) notRewardGo.SetActive(true);
        if (desLabel != null) desLabel.text = ConfigMng.Instance.GetUItext(119, new string[1] { GameCenter.guildFightMng.vectorGuildName });
    }
}
