//======================================================
//作者:朱素云
//日期:2016/7/13
//用途:周卡ui
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeekCardUi : MonoBehaviour 
{
    /// <summary>
    /// 领取物品和数目
    /// </summary>
    public UILabel rewardCountLab;
    /// <summary>
    /// 充值比例
    /// </summary>
    public UILabel rechargeLab;
    /// <summary>
    /// 领取奖励
    /// </summary>
    public UIButton rechargeBtn;
    /// <summary>
    /// 充值按钮
    /// </summary>
    public UIButton gotoRechargeBtn;
    /// <summary>
    /// 已领取
    /// </summary>
    public UIButton alreadyTake;
    private Dictionary<int, int> weekRewardDic
    {
        get
        {
            return GameCenter.weekCardMng.weekRewardDic;
        }
    }
    private WeekRewardStatus rewardState;


    public void Show(int _type, List<ItemValue> _info, WeekCardRef _week)
    {
        int diamonCount = 0;//元宝
        int lijinCount = 0;//礼金 
        for (int i = 0; i < _info.Count; i++)
        {
            if (_info[i].eid == 1)
            {
                diamonCount = _info[i].count;
            }
            else
            {
                lijinCount = _info[i].count;
            }
        } 
        if (rewardCountLab != null) rewardCountLab.text = ConfigMng.Instance.GetUItext(75, new string[2] { diamonCount.ToString(), lijinCount.ToString() }); 
        if (GameCenter.weekCardMng.weekRecharge >= _week.price)
        {
            if (rechargeLab != null) rechargeLab.text = "[6ef574]" + GameCenter.weekCardMng.weekRecharge + "/" + _week.price;
        }
        else
        {
            if (rechargeLab != null) rechargeLab.text = "[ff0000]" + GameCenter.weekCardMng.weekRecharge + "/" + _week.price;
        } 
        if (weekRewardDic.ContainsKey(_type))
            rewardState = (WeekRewardStatus)weekRewardDic[_type];//获取状态

        if (rewardState == WeekRewardStatus.ALREADTAKE)//已经领取
        {
            if (alreadyTake != null) alreadyTake.gameObject.SetActive(true);
            if (rechargeBtn != null) rechargeBtn.gameObject.SetActive(false);
            if (gotoRechargeBtn != null) gotoRechargeBtn.gameObject.SetActive(false);
        }
        if (rewardState == WeekRewardStatus.CANTAKE)//可以领取
        {
            if (alreadyTake != null) alreadyTake.gameObject.SetActive(false);
            if (gotoRechargeBtn != null) gotoRechargeBtn.gameObject.SetActive(false);
            if (rechargeBtn != null)
            {
                rechargeBtn.gameObject.SetActive(true);
                UIEventListener.Get(rechargeBtn.gameObject).onClick = delegate { GameCenter.weekCardMng.C2S_ReqTakeWeekReward(_week.id); };
            }
        }
        if (rewardState == WeekRewardStatus.UNTAKE)//还不可领取，请充值
        {
            if (alreadyTake != null) alreadyTake.gameObject.SetActive(false);
            if (rechargeBtn != null) rechargeBtn.gameObject.SetActive(false);
            if (gotoRechargeBtn != null)
            {
                gotoRechargeBtn.gameObject.SetActive(true);
                UIEventListener.Get(gotoRechargeBtn.gameObject).onClick = delegate { GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE); };
            }
        }
    }  
}
