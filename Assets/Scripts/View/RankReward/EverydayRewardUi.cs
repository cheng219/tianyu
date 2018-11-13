//==================================
//作者：朱素云
//日期：2016/5/10
//用途：每日签到ui
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EverydayRewardUi : MonoBehaviour {

    public ItemUI item;
    public UILabel vipTimeLab;//27 text
    public UILabel dayLab;
    public GameObject alreadTakeObj;
    public GameObject untakeObj;
    public GameObject effect;
    public GameObject yes;
    public GameObject bigReward;//不可领取的特殊大奖励背景图
    public GameObject cantakeBigReward;//可以领的特殊奖励背景图
    public Transform todayTake;

    protected EverydayRewardRef data;
    public EverydayRewardRef EverydayRewardRef
    {
        get
        {
            return data;
        }
        set
        {
            if (value != null) data = value;
            Show();
        }
    } 
    protected Dictionary<int, int> rewardDic
    {
        get
        {
            return GameCenter.rankRewardMng.rewardDic;
        }
    }

    public EverydayRewardUi CreateNew(Transform _parent, int _index)
    {
        GameObject obj = Instantiate(this.gameObject) as GameObject;
        obj.transform.parent = _parent;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = new Vector3((_index%60) * 122, -(_index / 6) * 161.6f, 0);
        return obj.GetComponent<EverydayRewardUi>();
    }

    void Show()
    {
        if (EverydayRewardRef != null)
        { 
            if (item != null)
            {
                if (EverydayRewardRef.item.Count > 0)
                {
                    item.FillInfo(new EquipmentInfo(EverydayRewardRef.item[0].eid, EverydayRewardRef.item[0].count, EquipmentBelongTo.PREVIEW)); 
                }
            }
            if (vipTimeLab != null)
            {
                if (EverydayRewardRef.times > 0)
                    vipTimeLab.text = ConfigMng.Instance.GetUItext(27, new string[2] { EverydayRewardRef.vip.ToString(), EverydayRewardRef.times.ToString() });
                else
                    vipTimeLab.gameObject.SetActive(false);
            }
            if (dayLab != null) dayLab.text = EverydayRewardRef.des;
            if (bigReward != null) bigReward.SetActive(false);
            if (cantakeBigReward != null) cantakeBigReward.SetActive(false);
            if (alreadTakeObj != null) alreadTakeObj.SetActive(false);
            if (untakeObj != null) untakeObj.SetActive(false);
            if (todayTake != null) todayTake.gameObject.SetActive(false);
            if (yes != null) yes.SetActive(rewardDic.ContainsKey(EverydayRewardRef.id));
            if (effect != null) effect.SetActive(false);

            int curId = GameCenter.rankRewardMng.rewardDay;

            item.ShowTooltip = true;
            if (rewardDic.ContainsKey(EverydayRewardRef.id))//判断是否领取
            {
                if (bigReward != null) bigReward.SetActive(EverydayRewardRef.special == 1);

                todayTake.gameObject.SetActive(true); 
                if (EverydayRewardRef.id == curId)
                {
                    todayTake.gameObject.SetActive(false);
                    if (alreadTakeObj != null) alreadTakeObj.SetActive(EverydayRewardRef.special != 1);
                    if (cantakeBigReward != null) cantakeBigReward.SetActive(EverydayRewardRef.special == 1);
                }
            }
            else
            {
                if (EverydayRewardRef.special == 1)
                {
                    if (cantakeBigReward != null) cantakeBigReward.SetActive(EverydayRewardRef.id == curId);
                    if (bigReward != null) bigReward.SetActive(EverydayRewardRef.id != curId);
                }
                if (untakeObj != null) untakeObj.SetActive(EverydayRewardRef.special != 1);

                if (EverydayRewardRef.id == curId)
                {
                    effect.SetActive(true);
                    item.ShowTooltip = false; 
                }
            }  
        }
    }
}
