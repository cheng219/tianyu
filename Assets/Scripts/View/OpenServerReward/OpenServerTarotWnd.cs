//======================================================
//作者:朱素云
//日期:2017/4/7
//用途:开服贺礼塔罗牌活动界面
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OpenServerTarotWnd : SubWnd
{
    public UITimer restTime;//剩余时间
    public UILabel activeCount;//活动次数
    public UIButton rechargeBtn;
    public UIButton turnCardBtn;//翻牌
    public UILabel needVip;//需要vip等级
    public UILabel consumeCoin;//需要消耗的数目
    public UILabel getCoin;//可以获得的数目 
    public RoyalRewardUI rewardUi;
    public GameObject effect;
    public UISpriteEx spEx; 
    protected float timer;
    protected bool isOpenCD = false;
    protected WdfTaroatData data = null;
    public UISprite redHint;

    public GameObject activityNotEndGo;
    public GameObject activityEndGo;

    // Use this for initialization
    void Start()
    { 
        if (rechargeBtn != null) UIEventListener.Get(rechargeBtn.gameObject).onClick = delegate {
            if (!isOpenCD)
            {
                GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
            }
        };

        if (turnCardBtn != null) UIEventListener.Get(turnCardBtn.gameObject).onClick = OnClickGetReward;
         
        if (effect != null) effect.SetActive(false);
        if (rewardUi != null) rewardUi.gameObject.SetActive(false);
        if (spEx != null) spEx.IsGray = UISpriteEx.ColorGray.normal;
    }
     
    protected override void OnOpen()
    {
        base.OnOpen();
        isOpenCD = false;
        Refresh();
    }
    protected override void OnClose()
    {
        base.OnClose();
        isOpenCD = false;
    }

    void Update()
    {
        if (isOpenCD)
        {
            if (spEx != null) spEx.IsGray = UISpriteEx.ColorGray.Gray;
            if (Time.time - timer > 3)
            {
                if (spEx != null) spEx.IsGray = UISpriteEx.ColorGray.normal;
                isOpenCD = false;
            }
        }
    }

    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            GameCenter.openServerRewardMng.OnTarotDataUpdate += Refresh;
            GameCenter.openServerRewardMng.OnTaroatRewardUpdate += GetResult; 
        }
        else
        {
            GameCenter.openServerRewardMng.OnTarotDataUpdate -= Refresh;
            GameCenter.openServerRewardMng.OnTaroatRewardUpdate -= GetResult;
        }
    }

    void OnClickGetReward(GameObject go)
    {
        if (data != null)
        {
            if ((int)GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount < data.consume)
            {
                GameCenter.openServerRewardMng.AddDiamondRemind();
                return;
            }
        }
        if (!isOpenCD)
        {
            isOpenCD = true;
            timer = Time.time;
            GameCenter.openServerRewardMng.C2S_AskTaroatInfo(2);
        } 
    }

    void Refresh()
    { 
        data = GameCenter.openServerRewardMng.wdfTaroatData;
        if (data != null)
        {

            bool isRed = GameCenter.openServerRewardMng.isTaroatRed();
            if (needVip != null)
            {
                if (data.needVipLev > 0 && GameCenter.vipMng.VipData != null && GameCenter.vipMng.VipData.vLev < data.needVipLev)
                {
                    if (needVip != null) needVip.gameObject.SetActive(true);
                    if (turnCardBtn != null) turnCardBtn.gameObject.SetActive(false);
                    if (rechargeBtn != null) rechargeBtn.gameObject.SetActive(true);
                }
                else
                {
                    if (needVip != null) needVip.gameObject.SetActive(false);
                    if (turnCardBtn != null) turnCardBtn.gameObject.SetActive(true);
                    if (rechargeBtn != null) rechargeBtn.gameObject.SetActive(false);
                }
            }
            if (restTime != null)
            {
                restTime.StartIntervalTimer(data.restTime);
                restTime.onTimeOut = (x) =>
                    {
                        GameCenter.openServerRewardMng.C2S_AskTaroatInfo(1);
                    };
            }
            if (activeCount != null) activeCount.text = data.activeCount.ToString();
            if (needVip != null) needVip.text = ConfigMng.Instance.GetUItext(338) + data.needVipLev.ToString();
            if (consumeCoin != null) consumeCoin.text = data.consume.ToString();
            if (getCoin != null) getCoin.text = data.getNumMax + "—" + data.getNumMin;
            if (redHint != null) redHint.gameObject.SetActive(isRed);
        }
        else
        {
            if (restTime != null) restTime.StopTimer();
            if (activeCount != null) activeCount.text = "0";
        }
        if (activityEndGo != null) activityEndGo.SetActive(data == null);
        if (activityNotEndGo != null) activityNotEndGo.SetActive(data != null);
    }

    void GetResult()
    {
        if (effect != null) effect.SetActive(true);
        CancelInvoke("ShowReward");
        Invoke("ShowReward", 1.7f); 
    }
    void ShowReward()
    {
        if (effect != null) effect.SetActive(false);
        List<EquipmentInfo> list = GameCenter.openServerRewardMng.taroatRewards;
        if (rewardUi != null)
        {
            rewardUi.gameObject.SetActive(true);
            rewardUi.CreateRewardItem(list);
        } 
    }
}
