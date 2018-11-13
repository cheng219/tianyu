//==================================
//作者：李邵南
//日期：2017/4/6
//用途：二冲活动界面类
//=================================

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TwoChargeWnd : SubWnd
{
    /// <summary>
    /// 点击充值按钮
    /// </summary>
    public UIButton rechargeBtn;
    /// <summary>
    /// 物品名字
    /// </summary>
    public GameObject[] nameLabel;
    /// <summary>
    /// 点击领取按钮
    /// </summary>
    public UIButton receiveBtn;
    /// <summary>
    /// 奖励物品
    /// </summary>
    public ItemUI[] items;
    protected TwoChargeRef twoCharge = null;
    private TwoChargeWnd twoChargeWnd;
    void Awake() 
    {
        GameCenter.twoChargeMng.C2S_ReqGetTwoChargeInfo();
        if (rechargeBtn != null) UIEventListener.Get(rechargeBtn.gameObject).onClick = Recharge;
        if (receiveBtn != null) UIEventListener.Get(receiveBtn.gameObject).onClick = Receive;
        UpdateShow();
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        UpdateShow();
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
            GameCenter.twoChargeMng.OnOpenTwoChargeUpdate += UpdateShow;
        }
        else
        {
            GameCenter.twoChargeMng.OnOpenTwoChargeUpdate -= UpdateShow;
        }
    }

    //点击充值按钮
    void Recharge(GameObject _go) 
    {
        //跳转到充值界面
        GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
    }
    //点击领取按钮
    void Receive(GameObject _go) 
    {
         GameCenter.twoChargeMng.C2S_ReqTakeReward();
         GameCenter.twoChargeMng.OnOpenTwoChargeUpdate -= UpdateShow;
    }
    //更新显示
    void UpdateShow() 
    {
        twoCharge = ConfigMng.Instance.GetTwoChargeRef(1);
        if (twoCharge != null)
        {
            if (GameCenter.twoChargeMng.stage == 1)
            {
                rechargeBtn.gameObject.SetActive(false);
                receiveBtn.gameObject.SetActive(true);
            }
            else
            {
                rechargeBtn.gameObject.SetActive(true);
                receiveBtn.gameObject.SetActive(false);
            }
            for (int i = 0, max = items.Length; i < max; i++)
            {
                if (twoCharge.reward.Count > i)
                {
                    items[i].FillInfo(new EquipmentInfo(twoCharge.reward[i].eid, twoCharge.reward[i].count, EquipmentBelongTo.PREVIEW));
                    nameLabel[i].gameObject.SetActive(false);
                }
                else
                    items[i].gameObject.SetActive(false);
            }
        }
    }
}
