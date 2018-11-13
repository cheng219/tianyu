//======================================================
//作者:唐源
//日期:2017/3/10
//用途:离线经验
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class OffLineRewardWnd : SubWnd {
    #region UI控件
    /// <summary>
    /// 离线修炼的开始时间
    /// </summary>
    public UILabel startTime;
    /// <summary>
    /// 离线奖励的累计时间
    /// </summary>
    public UILabel amountTime;
    /// <summary>
    /// 奖励物品预制的UIGrid
    /// </summary>
    public UIGrid rewardGrid;
    /// <summary>
    /// 奖励界面预览(用来显示奖励界面)
    /// </summary>
    public GameObject rewardPrefab;
    /// <summary>
    ///用于初始化复刻奖励物品的预制
    /// </summary>
    public GameObject sample;
    /// <summary>
    ///点击打开奖励界面预览的按钮
    /// </summary>
    public UIButton showBtn;
    /// <summary>
    ///点击领取奖励
    /// </summary>
    public UIButton cashing;
    /// <summary>
    ///点击关闭奖励预览
    /// </summary>
    public UIButton closeBtn;
    List<ItemUI> itemList = new List<ItemUI>();

    #endregion
    #region Unity 函数

    #endregion
    #region 事件句柄
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            if (showBtn != null)
                UIEventListener.Get(showBtn.gameObject).onClick += Show;
            if (cashing != null)
                UIEventListener.Get(cashing.gameObject).onClick += OnClickCash;
            if(closeBtn!=null)
                UIEventListener.Get(closeBtn.gameObject).onClick += Hide;
        }
        else
        {
            if (showBtn != null)
                UIEventListener.Get(showBtn.gameObject).onClick -= Show;
            if (cashing != null)
                UIEventListener.Get(cashing.gameObject).onClick -= OnClickCash;
            if (closeBtn != null)
                UIEventListener.Get(closeBtn.gameObject).onClick -= Hide;
        }
    }
    #endregion
    #region OnOpen
    protected override void OnOpen()
    {
        base.OnOpen();
        //打开窗口的时候刷新一次界面
        Refresh();
    }

    protected override void OnClose()
    {
        base.OnClose();
    }
    #endregion
    #region 界面的刷新与显示
    /// <summary>
    /// 刷新界面
    /// </summary>
    void Refresh()
    {
        List<ItemValue> reward = new List<ItemValue>();
        if (GameCenter.offLineRewardMng.RewardData.rewardList != null)
            reward = GameCenter.offLineRewardMng.RewardData.rewardList;
        else
        {
            Debug.LogError("离线奖励为空找郑梧孜");
            return;
        }
        for (int i = 0; i < reward.Count; i++)
        {
            if(sample!=null&& rewardGrid!=null)
            {
                GameObject go = Instantiate(sample);
                go.transform.parent = rewardGrid.transform;
                go.transform.localScale = transform.localScale;
                itemList.Add(go.GetComponent<ItemUI>());
                itemList[i].FillInfo(new EquipmentInfo(reward[i].eid, reward[i].count, EquipmentBelongTo.PREVIEW));
            }
            rewardGrid.Reposition();
        }
        if(startTime!=null)
        startTime.text = TimeStamp.TimeToChineseFormat(GameCenter.offLineRewardMng.OffLineTime);
        if (amountTime != null)
            amountTime.text = TimeStamp.TimeToHMString(GameCenter.offLineRewardMng.AmountTime);
    }
    /// <summary>
    /// 奖励界面的显示
    /// </summary>
    void Show(GameObject _obj)
    {
        if(rewardPrefab!=null)
        {
            rewardPrefab.SetActive(true);
        }
    }
    /// <summary>
    /// 奖励界面的隐藏
    /// </summary>
    void Hide(GameObject _obj)
    {
        if (rewardPrefab != null)
        {
            rewardPrefab.SetActive(false);
        }
    }
    #endregion
    #region 辅助逻辑(领奖与关窗)
    /// <summary>
    /// 领取奖励
    /// </summary>
    void OnClickCash(GameObject _obj)
    {
        GameCenter.offLineRewardMng.C2S_RequestReward();
        Close();
    }
    /// <summary>
    /// 关闭窗口
    /// </summary>
    void Close()
    {
        //GameCenter.uIMng.ReleaseGUI(GUIType.OFFLINEREWARD);
        CloseUI();
    }
    #endregion
}
