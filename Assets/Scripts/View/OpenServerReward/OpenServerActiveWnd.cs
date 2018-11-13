//======================================================
//作者:朱素云
//日期:2017/4/6
//用途:开服贺礼活动类型
//======================================================
using UnityEngine;
using System.Collections;

public class OpenServerActiveWnd : SubWnd
{
    public UITimer remainTime;
    public UILabel diamoNums;
    public GameObject rechargeBtn;
    public GameObject rewardsGird;
    public GameObject rewardInstance;

    protected OpenServerRewardInfoData serverData;

	// Use this for initialization
	void Start () {

        if (rechargeBtn != null) UIEventListener.Get(rechargeBtn).onClick = GoRecharge;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected override void OnOpen()
    {
        base.OnOpen();
        Refresh();
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
            GameCenter.openServerRewardMng.OnGetAllOpenServerInfo += Refresh;
            GameCenter.openServerRewardMng.OnGetOpenGiftResult += Refresh;
        }
        else
        { 
            GameCenter.openServerRewardMng.OnGetAllOpenServerInfo -= Refresh;
            GameCenter.openServerRewardMng.OnGetOpenGiftResult -= Refresh;

        }
    }

    void DestroyItem()
    {
        if (rewardsGird != null)
        {
            rewardsGird.transform.DestroyChildren();
        }
    } 

    void Refresh()
    {
        if (diamoNums != null)
            diamoNums.text = GameCenter.mainPlayerMng.MainPlayerInfo.DiamondCountText;
        DestroyItem();
        if (GameCenter.openServerRewardMng.ServerData != null)
        {
            Vector3 V3 = Vector3.zero;
            serverData = GameCenter.openServerRewardMng.ServerData;
            if (remainTime != null)
            {
                remainTime.StartIntervalTimer(serverData.remainTime);
                remainTime.onTimeOut = (x) => {
                    GameCenter.openServerRewardMng.C2S_AskAllOpenServerRewardInfo();
                };
            } 
            for (int i = 0 , max = serverData.rewardItems.Count; i < max; i++)
            {
                if (rewardsGird == null || rewardInstance == null)
                    return;
                GameObject obj = Instantiate(rewardInstance) as GameObject;
                if (obj == null)
                    return;
                Transform parentTransf = rewardsGird.transform;
                obj.transform.parent = parentTransf;
                obj.transform.localPosition = V3;
                obj.transform.localScale = Vector3.one;
                obj.SetActive(true);
                V3 = new Vector3(V3.x + 300, V3.y, V3.z);
                OpenServerRewardItemUI ui = obj.GetComponent<OpenServerRewardItemUI>();
                if (ui != null)
                {
                    ui.Refresh(serverData.rewardItems[i]);
                }
            } 
        } 
    } 
     
    void GoRecharge(GameObject _obj)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
    }
}
