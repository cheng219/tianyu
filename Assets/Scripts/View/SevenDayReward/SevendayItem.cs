//======================================================
//作者:鲁家旗
//日期:2016.8.24
//用途:七天奖励单个Item
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SevendayItem : MonoBehaviour {
    public UILabel dayLabel;
    public ItemUI[] rewardList;
    public ItemUI[] vipList;
    public UIButton getNormalBtn;
    public UIButton getVipBtn;
    public UILabel vipLabel;
    public UILabel GetAllReward;//一键全部领取按钮
    public UISprite[] spList = new UISprite[5];

    protected Dictionary<int, SevendDayData> SevendayDic
    {
        get
        {
            return GameCenter.sevenDayMng.sevendDic;
        }
    }
    public void SetItem(SevenDaysRef _data)
    {
        if (_data == null) return;
        for (int i = 0; i < spList.Length; i++)
        {
            spList[i].enabled = false;
        }
        //新加贵重物品显示框
        List<int> needShow = _data.specialList;
        for (int j = 0; j < needShow.Count; j++)
        {
            if (needShow[j] <= spList.Length && needShow[j] != 0)
                spList[needShow[j] - 1].enabled = true;
        }
        if (dayLabel != null) dayLabel.text = ConfigMng.Instance.GetUItext(181) + _data.day + ConfigMng.Instance.GetUItext(341);
        for (int i = 0; i < rewardList.Length; i++)
        {
            if (rewardList[i] != null)
                rewardList[i].FillInfo(new EquipmentInfo(_data.reward[i].eid, _data.reward[i].count, EquipmentBelongTo.PREVIEW));
        }
        for (int i = 0; i < vipList.Length; i++)
        {
            if (vipList[i] != null)
                vipList[i].FillInfo(new EquipmentInfo(_data.vipReward[i].eid, _data.vipReward[i].count, EquipmentBelongTo.PREVIEW));
        }
        UISpriteEx normalSp = getNormalBtn.GetComponentInChildren<UISpriteEx>();
        UISpriteEx vipSp = getVipBtn.GetComponentInChildren<UISpriteEx>();
        if (_data.day <= GameCenter.sevenDayMng.day)//可以领取奖励的天数
        {
            //新加一键全部领取
            if (GameCenter.sevenDayMng.BoolVip(_data.day))
            {
                if (GetAllReward != null) GetAllReward.text = ConfigMng.Instance.GetUItext(342);
            }
            if (normalSp != null) normalSp.IsGray = UISpriteEx.ColorGray.normal;
            if (SevendayDic.ContainsKey(_data.day))
            {
                //普通奖励已经领取
                if (SevendayDic[_data.day].Normal == (int)RewardGetState.GETREWARD)
                {
                    getNormalBtn.gameObject.SetActive(false);
                }
                //普通奖励已经领取，vip奖励没有领取
                if (SevendayDic[_data.day].Normal == (int)RewardGetState.GETREWARD && SevendayDic[_data.day].Vip == (int)RewardGetState.NOTGETREWARD)
                {
                    getVipBtn.gameObject.SetActive(true);
                    //vip等级达到
                    if (GameCenter.sevenDayMng.BoolVip(_data.day))
                    {
                        if (vipSp != null) vipSp.IsGray = UISpriteEx.ColorGray.normal;
                    }
                    else
                    {
                        if (vipSp != null) vipSp.IsGray = UISpriteEx.ColorGray.Gray;
                    }
                }
                //普通奖励已经领取，vip奖励已经领取
                else if (SevendayDic[_data.day].Normal == (int)RewardGetState.GETREWARD && SevendayDic[_data.day].Vip == (int)RewardGetState.GETREWARD)
                {
                    getVipBtn.gameObject.SetActive(true);
                    getVipBtn.GetComponentInChildren<UILabel>().text = ConfigMng.Instance.GetUItext(343);
                    if (vipSp != null) vipSp.IsGray = UISpriteEx.ColorGray.Gray;
                }
            }
        }
        else
            if (normalSp != null) normalSp.IsGray = UISpriteEx.ColorGray.Gray;

        if (getNormalBtn != null)
        {
            UIEventListener.Get(getNormalBtn.gameObject).onClick = delegate
            {
                if (_data.day <= GameCenter.sevenDayMng.day)
                {
                    //新加一键全部领取
                    if (GameCenter.sevenDayMng.BoolVip(_data.day))
                    {
                        GameCenter.sevenDayMng.ReqGetReward(_data.day, false);
                        GameCenter.sevenDayMng.ReqGetReward(_data.day, true);
                    }
                    else
                        GameCenter.sevenDayMng.ReqGetReward(_data.day, false);
                }
            };
        }
        if (getVipBtn != null)
        {
            UIEventListener.Get(getVipBtn.gameObject).onClick = delegate
            {
                if (GameCenter.sevenDayMng.BoolVip(_data.day))
                {
                    if (SevendayDic.ContainsKey(_data.day))
                    {
                        if (SevendayDic[_data.day].Vip == (int)RewardGetState.NOTGETREWARD)
                        {
                            GameCenter.sevenDayMng.ReqGetReward(_data.day, true);
                        }
                    }
                }
            };
        }
        //vip信息
        //if (SevendayDic.ContainsKey(_data.day) && SevendayDic[_data.day].Normal == (int)RewardGetState.GETREWARD)
        //{
        //    if (vipLabel != null) vipLabel.gameObject.SetActive(true);
        //}
        //else
        //{
        //    if (vipLabel != null) vipLabel.gameObject.SetActive(false);
        //}
        if(vipLabel != null)
            vipLabel.text = ConfigMng.Instance.GetUItext(50, new string[1]{_data.vipLimit.ToString()});
        if (GameCenter.vipMng.VipData.vLev >= _data.vipLimit)
        {
            if (vipLabel != null) vipLabel.color = Color.green;
        }
        else if (vipLabel != null)
            vipLabel.color = Color.red;
    }
}
