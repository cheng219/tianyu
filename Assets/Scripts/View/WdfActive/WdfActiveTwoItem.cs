//======================================================
//作者:黄洪兴
//日期:2016/07/13
//用途:精彩活动类型2组件
//======================================================
using UnityEngine;
using System.Collections;

public class WdfActiveTwoItem : SubWnd {

    public UITimer remainTime;
    public UILabel activeDes;
    public UILabel activeCount;
    public ItemUI item;
    public UILabel itemDes;
    public UISlider uislider;
    public UILabel nums;
    public UILabel itemRemainNums;
    public GameObject getBtn;
    public GameObject rechargeBtn;



    protected WdfActiveTypeData info;

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
        if (this == null)
            return;
        if (_info == null)
            return;
        info = _info;
        this.gameObject.SetActive(true);
        if (remainTime != null)
        {
            remainTime.StartIntervalTimer((int)_info.rest_time);
            remainTime.onTimeOut = (x) =>
            {
                ReFreshInfo();
            };
        }
        if (activeDes != null)
            activeDes.text = _info.desc;
        if (activeCount != null)
            activeCount.text =ConfigMng.Instance.GetUItext(357) +_info.counter_value.ToString()+ConfigMng.Instance.GetUItext(309);
        if (_info.details.Count > 0)
        {
            if (item != null)
            {
                if (_info.details[0].reward_info.Count > 0)
                {
                    EquipmentInfo eq = _info.details[0].reward_info[0];
                    if (eq != null)
                        item.FillInfo(eq);
                }
            }
            if (itemDes!=null)
            {
                itemDes.text = _info.details[0].desc;

            }
            if (uislider != null)
                uislider.value = (float)_info.details[0].value1 / (float)_info.details[0].value2;
            if (nums != null)
                nums.text = _info.details[0].value1.ToString() + "/" + _info.details[0].value2.ToString();
            if (itemRemainNums != null)
            {
                itemRemainNums.text = (_info.details[0].total_reward_times - _info.details[0].reward_times).ToString();
            }
            if (getBtn != null)
            {
                UIEventListener.Get(getBtn).onClick = GetReward;
                getBtn.SetActive((_info.details[0].total_reward_times - _info.details[0].reward_times) > 0);
            }
            if (rechargeBtn != null)
            {
                UIEventListener.Get(rechargeBtn).onClick = GoRecharge;
                rechargeBtn.SetActive((_info.details[0].total_reward_times - _info.details[0].reward_times) <= 0);

            }
            if ((_info.details[0].total_reward_times - _info.details[0].reward_times) <= 0&& GameCenter.wdfActiveMng.RedDic.ContainsKey(GameCenter.wdfActiveMng.CurWdfActiveType))
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
    }

    void GetReward(GameObject _go)
    {
        if (info.details.Count>0)
        GameCenter.wdfActiveMng.C2S_AskActivitysRewards((int)info.id, (int)info.details[0].index);
    }

    void GoRecharge(GameObject _go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);

    }


    void ReFreshInfo()
    {
        GameCenter.wdfActiveMng.CurWdfActiveType = 0;
        GameCenter.wdfActiveMng.needReset = true;
        GameCenter.wdfActiveMng.C2S_AskAllActivitysInfo();
    }





}
