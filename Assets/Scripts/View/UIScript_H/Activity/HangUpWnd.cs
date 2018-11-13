//============================
//作者：邓成
//日期：2017/5/2
//用途：挂机副本界面类
//============================

using UnityEngine;
using System.Collections;

public class HangUpWnd : GUIBase {
    public UILabel remainMonsterNum;
    public UIButton btnGoFirst;
    public UIButton btnGoSecond;
    public UILabel remainAddTimes;
    public UIButton btnClose;
    public UILabel labVipLimit;//'VIP2进入'
    public UITexture backTexture;
    void Awake()
    {
        layer = GUIZLayer.NORMALWINDOW;
        mutualExclusion = true;
        if (btnGoSecond != null) UIEventListener.Get(btnGoSecond.gameObject).onClick = GoSecond;
        if (btnGoFirst != null) UIEventListener.Get(btnGoFirst.gameObject).onClick = GoFirst;
        if (btnClose != null) UIEventListener.Get(btnClose.gameObject).onClick = CloseWnd;
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        //Refresh();
        GameCenter.activityMng.C2S_ReqHangUpCoppyData(1);
        ConfigMng.Instance.GetBigUIIcon("Pic_jjc_bg", SetBackTexture);
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
            GameCenter.activityMng.OnHanguUpCoppyDataUpdateEvent += Refresh;
        }
        else
        {
            GameCenter.activityMng.OnHanguUpCoppyDataUpdateEvent -= Refresh;
        }
    }

    protected void SetBackTexture(Texture2D texture2D)
    {
        if (backTexture != null)
            backTexture.mainTexture = texture2D;
    }

    void Refresh()
    {
        if (remainMonsterNum != null) remainMonsterNum.text = GameCenter.activityMng.HangUpCoppyRemainMonsterNum.ToString();
        if (remainAddTimes != null) remainAddTimes.text = GameCenter.activityMng.HangUpRemainBuyTimes.ToString();
        if (labVipLimit != null) labVipLimit.enabled = GameCenter.mainPlayerMng.MainPlayerInfo.VipLev < 2;
    }

    void GoSecond(GameObject go)
    {
        if (GameCenter.mainPlayerMng.MainPlayerInfo.VipLev < 2)
        {
            GameCenter.messageMng.AddClientMsg(154);
            return;
        }
        GameCenter.activityMng.C2S_FlyHangUpCoppy(160012);
    }
    void GoFirst(GameObject go)
    {
        GameCenter.activityMng.C2S_FlyHangUpCoppy(160011);
    }
    void CloseWnd(GameObject go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
    }

}
