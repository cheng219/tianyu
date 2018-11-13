//==================================
//作者：黄洪兴
//日期：2016/5/3
//用途：在线奖励界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OnlineRewardWnd : GUIBase {

    protected int curScale = -1; 
	public GameObject rewardBtn; 
    public UITimer timer;
    public GameObject timerObj;  
    public UILabel lottery;//抽奖
    public UILabel lotteying;//抽奖中 
    public OnlineRewardUi[] onlineRewards; 
	public GameObject CloseBtn;
    OnlineRewardItemInfo curInfo;  
    protected bool isFirstOpen = false; 
    public TweenRotation tweenRotation;
    public UISpriteEx getRewardEx;

	void Awake()
	{
		if (CloseBtn != null)
            UIEventListener.Get(CloseBtn).onClick = delegate { GameCenter.uIMng.SwitchToUI(GUIType.NONE); };
		mutualExclusion = true;
	} 

	protected override void OnOpen()
	{
		base.OnOpen();
        isFirstOpen = true; 
        GameCenter.onlineRewardMng.C2S_AskOnlineRewardInfo(2);
        Refresh();
	}
	protected override void OnClose()
	{
		base.OnClose();
        isFirstOpen = false;
        if (lotteying.gameObject.activeSelf)
        {
            GameCenter.onlineRewardMng.C2S_AskGetOnlineReward(1);
        }
	}
	protected override void HandEvent(bool _bind)
	{
		base.HandEvent(_bind);
		if (_bind)
		{
            //GameCenter.onlineRewardMng.OnGetOnlineRewardInfo += Refresh;
            GameCenter.onlineRewardMng.OnBginTotateUpdate += StartCircle;
            if (rewardBtn != null) UIEventListener.Get(rewardBtn).onClick += GetReward;
            if (timer != null) timer.onTimeOut = (x) => { RefreshState(); };

		}
		else
		{
            //GameCenter.onlineRewardMng.OnGetOnlineRewardInfo -= Refresh;
            GameCenter.onlineRewardMng.OnBginTotateUpdate -= StartCircle;
            if (rewardBtn != null) UIEventListener.Get(rewardBtn).onClick -= GetReward; 
		}
	}
	protected int remainTime=0; 
	void remainTimeUpdate()
	{ 
        if (curInfo.RemainTime == -1)
        {
            if (rewardBtn != null)
            {
                rewardBtn.SetActive(true); 
            }
            if (timerObj != null) timerObj.SetActive(false);
            if (getRewardEx != null) getRewardEx.IsGray = UISpriteEx.ColorGray.normal;
            return;
        } 
        remainTime = curInfo.RemainTime - ((int)Time.time - curInfo.ReceiveTime);
        if (remainTime > 0 && !GameCenter.onlineRewardMng.IsGetedAll)//等待
        {
            if (timer != null) timer.StartIntervalTimer(remainTime);  
            if (timerObj != null) timerObj.SetActive(true);
            if (rewardBtn != null) rewardBtn.SetActive(false);
            if (getRewardEx != null) getRewardEx.IsGray = UISpriteEx.ColorGray.Gray;
        }
        else//可以抽奖
        {
            if (timer != null) timer.StartIntervalTimer(0); 
            if (timerObj != null) timerObj.SetActive(false);
            if (rewardBtn != null) rewardBtn.SetActive(true);
            if (getRewardEx != null) getRewardEx.IsGray = UISpriteEx.ColorGray.normal;
            RefreshState();
        }
       
	}

    /// <summary>
    /// 可以抽奖
    /// </summary>
    void RefreshState()
    { 
        if (timer != null) timer.StartIntervalTimer(0);
        if (timerObj != null) timerObj.SetActive(false);
        if (rewardBtn != null) rewardBtn.SetActive(true);
        if (lotteying != null) lotteying.gameObject.SetActive(false);
        if (lottery != null) lottery.gameObject.SetActive(true);
        if (getRewardEx != null) getRewardEx.IsGray = UISpriteEx.ColorGray.normal;
    }

	void Refresh()
	{ 
        if (lotteying != null) lotteying.gameObject.SetActive(false);
        if (lottery != null) lottery.gameObject.SetActive(true); 
        List<OnlineRewardItemInfo> OnlineRewardItem = GameCenter.onlineRewardMng.OnlineRewardItem;
        for (int i = 0, max = onlineRewards.Length; i < max; i++)
        {
            if (OnlineRewardItem.Count > i)
            {
                onlineRewards[i].item.FillInfo(OnlineRewardItem[i].RewardItem);
                onlineRewards[i].iconEx.spriteName = OnlineRewardItem[i].RewardItem.IconName;
            }
            if (OnlineRewardItem[i].ID == GameCenter.onlineRewardMng.curLottryNum && !isFirstOpen)
            {
                onlineRewards[i].chooseBack.SetActive(true);
            }
            else
            {
                onlineRewards[i].chooseBack.SetActive(false);
            }
            if (GameCenter.onlineRewardMng.alreadyTakeReward.Contains(OnlineRewardItem[i].ID))
            { 
                if (GameCenter.onlineRewardMng.curLottryNum != -1 && GameCenter.onlineRewardMng.curLottryNum == OnlineRewardItem[i].ID && !isFirstOpen)
                {
                    curScale = i; 
                }
                else
                { 
                    onlineRewards[i].areadyTake.gameObject.SetActive(true);
                    onlineRewards[i].areadyTake.enabled = false;
                }
                onlineRewards[i].iconEx.IsGray = UISpriteEx.ColorGray.Gray;
            }
            else
            {
                onlineRewards[i].areadyTake.gameObject.SetActive(false);
                onlineRewards[i].iconEx.IsGray = UISpriteEx.ColorGray.normal;
            }
        }
        if (GameCenter.onlineRewardMng.CurRewardItem >= 0 && OnlineRewardItem.Count > GameCenter.onlineRewardMng.CurRewardItem)
        {
            curInfo = OnlineRewardItem[GameCenter.onlineRewardMng.CurRewardItem];
        }
        if (curInfo == null)
            return;
        EquipmentInfo item = curInfo.RewardItem;  
            //if (rewardItem != null && item != null) rewardItem.FillInfo(item);
        if (curInfo.ReceiveTime!=0)
		remainTimeUpdate (); 
        isFirstOpen = false;
        ResetAlreadyTakeBack(); 
	}


    void ResetAlreadyTakeBack()
    {
        if (onlineRewards.Length > curScale && curScale >= 0)
        {
            onlineRewards[curScale].areadyTake.gameObject.SetActive(true);
            onlineRewards[curScale].areadyTake.ResetToBeginning();
            onlineRewards[curScale].areadyTake.enabled = true; 
        } 
    } 
     
    /// <summary>
    /// 请求领取奖励
    /// </summary>
    /// <param name="obj"></param>
    void GetReward(GameObject obj)
    {
        if (rewardBtn.gameObject.activeSelf)
        {
            if (GameCenter.onlineRewardMng.IsGetedAll)
            {
                GameCenter.messageMng.AddClientMsg(507);
                
            }
            else 
            {
                for (int i = 0, max = onlineRewards.Length; i < max; i++)
                {
                    onlineRewards[i].chooseBack.SetActive(false); 
                } 
                GameCenter.onlineRewardMng.C2S_AskGetOnlineReward(1);
            }
        }
    }


    public void StartCircle(int _selectIndex)
    {
        if (lotteying != null) lotteying.gameObject.SetActive(true);
        if (lottery != null) lottery.gameObject.SetActive(false); 
        Quaternion oldDirection = tweenRotation.transform.localRotation;
        tweenRotation.ResetToBeginning();
        int circle = Random.Range(2, 5);
        //  float justRoundAround = Random.Range(0, 7) + 3600.0f;
        float justRoundAround = circle * 360f + 45 * _selectIndex - 22.5f;
        tweenRotation.from = oldDirection.eulerAngles;
        tweenRotation.to = new Vector3(0f, 0f, -justRoundAround);
        tweenRotation.duration = 3;
        tweenRotation.PlayForward();
        tweenRotation.gameObject.SetActive(true);
    }

    public void FninishRotate()
    {
        Refresh();
    }
}
