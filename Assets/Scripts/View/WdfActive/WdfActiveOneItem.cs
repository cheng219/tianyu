//======================================================
//作者:黄洪兴
//日期:2016/7/13
//用途:精彩活动类型1组件
//======================================================
using UnityEngine;
using System.Collections;

public class WdfActiveOneItem : SubWnd {

    public UITimer remainTime;
    public UILabel activeDes;
    public UILabel activeCount;
    public GameObject rewardGrid;
    public GameObject rewardItemInstance;
    public UIScrollView view;

    public UIButton redRechargeBtn;//红利充值按钮
    public GameObject recharged;//红利已经激活

    bool red = false;

	// Use this for initialization
    void Start()
    {

        if (redRechargeBtn != null) UIEventListener.Get(redRechargeBtn.gameObject).onClick = delegate
            {
            #if UNITY_IOS 
             GameCenter.rechargeMng.C2S_RequestRecharge(15);;//android is 4     ios is 15 
            #else
             GameCenter.rechargeMng.C2S_RequestRecharge(4);
            #endif
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
        if (this == null||_info==null)
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

        if (redRechargeBtn != null) redRechargeBtn.gameObject.SetActive(_info.counter_value != 1);
        if (recharged != null) recharged.SetActive(_info.counter_value == 1);
         
        if (activeDes != null)
            activeDes.text = _info.desc;
        if (activeCount != null)
        {
            //string[] word = { _info.counter_value.ToString() };


            switch (_info.type)
            {
                case 1:
                    activeCount.text = ConfigMng.Instance.GetUItext(357)+_info.counter_value.ToString()+ConfigMng.Instance.GetUItext(341);
                    break;
                case 2:
                case 3:
                case 4:
                    activeCount.text =ConfigMng.Instance.GetUItext(357) + _info.counter_value.ToString() + ConfigMng.Instance.GetUItext(309);
                    break;   
                default:
                    activeCount.text =string.Empty;
                    break;
            }
        }
        Vector3 V3 = Vector3.zero;
        DestroyItem();
        for (int i = 0; i < _info.details.Count; i++)
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
                ui.Refresh(_info.details[i], _info);
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

        if (view != null && GameCenter.wdfActiveMng.needReset)
        {
            view.ResetPosition();
            GameCenter.wdfActiveMng.needReset = false;

        }

    }


    void ReFreshInfo()
    {
        GameCenter.wdfActiveMng.CurWdfActiveType = 0;
        GameCenter.wdfActiveMng.needReset = true;
        GameCenter.wdfActiveMng.C2S_AskAllActivitysInfo();
    }

    void DestroyItem()
    {
        if (rewardGrid != null)
        {
            rewardGrid.transform.DestroyChildren();
        }
    }


}
