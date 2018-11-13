//======================================================
//作者:朱素云
//日期:2017/5/2
//用途:每日首充界面
//======================================================
using UnityEngine;
using System.Collections;

public class DailyFirstRechargeWnd : GUIBase
{
    public UILabel getRebackLab;
    public UILabel getRebackdesLab;
    public UITimer remainTime;
    public UIButton rechargeBtn;
    public UIButton closeBtn;

    void Awake()
    {
        mutualExclusion = true;
        layer = GUIZLayer.NORMALWINDOW;
    }
     
	// Use this for initialization
	void Start () {

        if (rechargeBtn != null) UIEventListener.Get(rechargeBtn.gameObject).onClick = delegate { GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE); };
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = delegate { 
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
            GameCenter.uIMng.ReleaseGUI(GUIType.DAILYFIRSTRECHARGE);
        };
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

    void Refresh()
    {
        if (getRebackLab != null) getRebackLab.text = GameCenter.openServerRewardMng.rebackPercent.ToString();
        if (getRebackdesLab != null) getRebackdesLab.text = GameCenter.openServerRewardMng.rebackPercent.ToString();
        if (remainTime != null)
        {
            remainTime.StartIntervalTimer(GameCenter.openServerRewardMng.reminTime);
            remainTime.onTimeOut = (x) =>
            {
                GameCenter.openServerRewardMng.reminTime = 0;
            };
        }
    }
}
