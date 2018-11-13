//======================================================
//作者:朱素云
//日期:2017/4/14
//用途:开服活动登录红利
//======================================================
using UnityEngine;
using System.Collections;

public class LoginBonusWnd : SubWnd
{

    public UIButton redRechargeBtn;//红利充值按钮
    public GameObject recharged;//红利已经激活
    public GameObject rewardGrid;
    public GameObject rewardItemInstance;
    public UIScrollView view; 

	// Use this for initialization
    void Start()
    {
        GameCenter.weekCardMng.C2S_ReqGetLoginBonusInfo();
        if (redRechargeBtn != null) UIEventListener.Get(redRechargeBtn.gameObject).onClick = delegate
        {
#if UNITY_IOS 
             GameCenter.rechargeMng.C2S_RequestRecharge(15);;//android is 4     ios is 15 
#else
            GameCenter.rechargeMng.C2S_RequestRecharge(4);
#endif
        };

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    protected override void OnOpen()
    {
        base.OnOpen();
        Refresh();
        GameCenter.weekCardMng.OnLoginBonusUpdate += Refresh;
    }
    protected override void OnClose()
    {
        base.OnClose();
        GameCenter.weekCardMng.OnLoginBonusUpdate -= Refresh;
    }
     
    void Refresh()
    {
        WdfActiveTypeData data = GameCenter.weekCardMng.loginBonusData;
        if (data == null || this== null)
            return; 
        //Debug.Log("刷新活动详情" + GameCenter.wdfActiveMng.CurWdfActiveType);
        //this.gameObject.SetActive(true);

        if (redRechargeBtn != null) redRechargeBtn.gameObject.SetActive(!GameCenter.weekCardMng.isCanTakeLoginBunus);
        if (recharged != null) recharged.SetActive(GameCenter.weekCardMng.isCanTakeLoginBunus);
          
        Vector3 V3 = Vector3.zero;
        DestroyItem();
        for (int i = 0, max = data.details.Count; i < max; i++)
        {
            if (rewardGrid == null || rewardItemInstance == null)
                return;
            GameObject obj = Instantiate(rewardItemInstance) as GameObject;
            if (obj == null)
                return;
            Transform parentTransf = rewardGrid.transform;
            obj.transform.parent = parentTransf;
            obj.transform.localPosition = V3;
            obj.transform.localScale = Vector3.one;
            obj.SetActive(true);
            V3 = new Vector3(V3.x, V3.y - 130, V3.z);
            WdfActiveRewardItem ui = obj.GetComponent<WdfActiveRewardItem>();
            if (ui != null)
            {
                ui.Refresh(data.details[i], data);
            } 
        }
        if (view != null)
        {
            view.ResetPosition(); 
        }
    }

    void DestroyItem()
    {
        if (rewardGrid != null)
        {
            rewardGrid.transform.DestroyChildren();
        }
    }
}
