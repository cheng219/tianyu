//======================================================
//作者:朱素云
//日期:2017/4/5
//用途:抽奖奖励ui
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class WdfLotteryReward : MonoBehaviour
{
    public UIButton closeBtn;
    public UIButton sureBtn;
    public UIButton doAgainBtn;
    public List<WdfLotteryRewardItem> items = new List<WdfLotteryRewardItem>();
    public UILabel price;
    protected List<lucky_wheel_reward_info> rewardList = new List<lucky_wheel_reward_info>();
    protected float timer = 0;
    protected int curId = 0;
    protected bool isShow = false; 

	// Use this for initialization
	void Awake () {
        HideAll();
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = delegate { HideAll(); this.gameObject.SetActive(false); };
        if (sureBtn != null) UIEventListener.Get(sureBtn.gameObject).onClick = delegate { HideAll(); this.gameObject.SetActive(false); };
        if (doAgainBtn != null) UIEventListener.Get(doAgainBtn.gameObject).onClick = delegate {
            HideAll();
            if (items.Count > 1)
            {
                //请求再转十次
                GameCenter.openServerRewardMng.C2S_AskLottery(2); 
            }
            else
            { 
                //请求再转一次
                GameCenter.openServerRewardMng.C2S_AskLottery(1);
            }
            this.gameObject.SetActive(false);
        };
	}

    void HideAll()
    {
        for (int i = 0, max = items.Count; i < max; i++)
        {
            items[i].tween.enabled = false; 
            items[i].gameObject.SetActive(false); 
        }
    }
	
	// Update is called once per frame
    void Update()
    {
        if (isShow)
        {
            if (Time.time - timer > 0.5)
            {
                curId++;
                isShow = false;
                fillInfo(curId);
            }
        }
    }

    void fillInfo(int _id)
    {
        if (items.Count > _id && items.Count > _id)
        { 
            items[_id].tween.ResetToBeginning();
            items[_id].tween.enabled = true; 
            items[_id].SetData(rewardList[_id]);
            items[_id].gameObject.SetActive(true); 

            if (rewardList.Count > curId + 1)
            {
                timer = Time.time;
                isShow = true;
            }
            else
            {
                isShow = false;
                timer = 0;
            }
        }
    }

    public void SetData(List<lucky_wheel_reward_info> _rewardList)
    {
        HideAll();
        rewardList = _rewardList;
        curId = 0;
        if (_rewardList.Count > 1)
        {
            fillInfo(curId);
        }
        else
        {
            for (int i = 0, max = _rewardList.Count; i < max; i++)
            {
                if (items.Count > i)
                {
                    items[i].SetData(_rewardList[i]);
                    items[i].gameObject.SetActive(true);
                    items[i].tween.ResetToBeginning();
                    items[i].tween.enabled = true; 
                }
            }
        }
        if (GameCenter.openServerRewardMng.lotteryData != null)
        {
            if (items.Count > 1)
            {
                if (price != null) price.text = GameCenter.openServerRewardMng.lotteryData.price2.ToString();
            }
            else
            {
                if (price != null) price.text = GameCenter.openServerRewardMng.lotteryData.price1.ToString();
            }
        }
    }
}
