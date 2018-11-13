//======================================================
//作者:朱素云
//日期:2016/7/13
//用途:周卡充值界面
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RechargeWeekWnd : SubWnd
{ 
    /// 关闭界面
    /// </summary>
    public UIButton closeX; 
    /// <summary>
    /// 特惠/至尊
    /// </summary>
    public List<WeekCardUi> weekTypeUi = new List<WeekCardUi>();

    private Dictionary<int, int> weekRewardDic
    {
        get
        {
            return GameCenter.weekCardMng.weekRewardDic;
        }
    }

    #region 构造 

    void Awake()
    {
        GameCenter.weekCardMng.C2S_ReqGetWeekInfo(); 
        if (closeX != null) UIEventListener.Get(closeX.gameObject).onClick = OnCloseWnd; 
    }
    protected override void OnOpen()
    { 
        base.OnOpen();
        Refresh();
        GameCenter.weekCardMng.OnWeekRewardUpdate += Refresh;
    }
    protected override void OnClose()
    {
        base.OnClose();
        GameCenter.weekCardMng.OnWeekRewardUpdate -= Refresh;
    }
    void OnCloseWnd(GameObject go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
    } 
    #endregion

    void Refresh()
    {
        int i = 0;
        foreach (int type in weekRewardDic.Keys)
        {
            WeekCardRef week = ConfigMng.Instance.GetWeekCardRef(type);
            if (i < weekTypeUi.Count)
            {
                if (week != null)
                { 
                    weekTypeUi[i].Show(type, week.every_day_reward, week);
                }
            }
            i++;
        } 
    }
}
