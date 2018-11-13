//======================================================
//作者:朱素云
//日期:2017/3/29
//用途:连充豪礼活动类型
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WdfActiveFourItem : SubWnd {

    public UITimer remainTime;
    public UILabel activeDes;
    public UIButton rechargeBtn;
    public UILabel rechargedDay1;//完成288充值第多少天
    public UILabel rechargedDay2;//完成588充值第多少天
    public UILabel rechargedDay3;//完成986充值第多少天
    public UILabel[] needDay;//充值档次
    public UILabel[] needDiamond;//需要的元宝

    bool red = false;

    public List<WdfActiveRewardItem> WdfActiveRewardItems;//规格已经固定
	// Use this for initialization
	void Start () {

        if (rechargeBtn != null) UIEventListener.Get(rechargeBtn.gameObject).onClick = delegate
            {
                GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
            };
	
	}

    protected override void OnOpen()
    {
        base.OnOpen();
        if (GameCenter.wdfActiveMng.CurWdfActiveItemInfo == null)
            return;
        WdfActiveTypeData curWdfActiveItemInfo = GameCenter.wdfActiveMng.CurWdfActiveItemInfo;
        Refresh(curWdfActiveItemInfo);
    }

    public void Refresh(WdfActiveTypeData _info)
    {
        if (this == null || _info == null)
            return;
        red = false;
        //Debug.Log("刷新活动详情" + GameCenter.wdfActiveMng.CurWdfActiveType);
        this.gameObject.SetActive(true);
        if (remainTime != null)
        {
            remainTime.StartIntervalTimer((int)_info.rest_time);
            remainTime.onTimeOut = (x) =>
            {
                ReFreshInfo();
            };
        }
        if (rechargedDay1 != null) rechargedDay1.text = (_info.counter_value >> 16 & 0xff).ToString();
        if (rechargedDay2 != null) rechargedDay2.text = (_info.counter_value>>8 & 0xff).ToString();
        if (rechargedDay3 != null) rechargedDay3.text = (_info.counter_value & 0xff).ToString();
        if (activeDes != null)activeDes.text = _info.desc;
         
        for (int i = 0, max = WdfActiveRewardItems.Count; i < max; i++)
        {
            if (_info.details.Count > i)
            {
                WdfActiveRewardItems[i].Refresh(_info.details[i], _info);
            }
            if (needDay.Length > i)
            {
                needDay[i].text = ConfigMng.Instance.GetUItext(358) + _info.details[i].value1 + ConfigMng.Instance.GetUItext(341);
            }
            if (i % 3 == 0)
            { 
                if (needDiamond.Length > i / 3)
                {
                    needDiamond[i / 3].text = _info.details[i].value2.ToString();
                }
            }
            if (!red && _info.details[i].total_reward_times > _info.details[i].reward_times && _info.details[i].total_reward_times > 0)
            {
                //Debug.Log("有奖励" + _info.details[i].total_reward_times + ":" + _info.details[i].reward_times);
                red = true;

            } 
        }
        if (!red && GameCenter.wdfActiveMng.RedDic.ContainsKey(GameCenter.wdfActiveMng.CurWdfActiveType))
        {
            if (GameCenter.wdfActiveMng.RedDic[GameCenter.wdfActiveMng.CurWdfActiveType])
            {
                //Debug.Log("设置为没有奖励");
                GameCenter.wdfActiveMng.RedDic[GameCenter.wdfActiveMng.CurWdfActiveType] = false;
                if (GameCenter.wdfActiveMng.RefreshRed != null)
                    GameCenter.wdfActiveMng.RefreshRed();
            }
        } 
    }

    void ReFreshInfo()
    {
        GameCenter.wdfActiveMng.CurWdfActiveType = 0;
        GameCenter.wdfActiveMng.needReset = true;
        GameCenter.wdfActiveMng.C2S_AskAllActivitysInfo();
    }
}
