//======================================
//作者:吴江
//日期:2016/3/2
//用途:断线重新连接的窗口
//======================================

using UnityEngine;
using System.Collections;

public class ReturnExitWnd : GUIBase
{
    public GameObject goWaitBtn;
    public GameObject goBackBtn;
    public UITimer showTime;


    float openTime = 0;
    float limitTime = 30;

    void Awake()
    {
        openTime = Time.time;
        mutualExclusion = false;
        Layer = GUIZLayer.TIP + 200;
        showTime.StartTimer();
        if (GameCenter.instance.IsConnected)
        {
            NetMsgMng.ConectClose();
        }
        UIEventListener.Get(goBackBtn).onClick -= OnBtnPassWord;
        UIEventListener.Get(goWaitBtn).onClick -= OnBtnWait;
        UIEventListener.Get(goBackBtn).onClick += OnBtnPassWord;
        UIEventListener.Get(goWaitBtn).onClick += OnBtnWait;
        showTime.StartIntervalTimer((int)limitTime);
    }


    void Update()
    {
        if (Time.time - openTime >= limitTime)
        {
            OnBtnWait(null);
        }
    }


    void OnBtnPassWord(GameObject games)
    {
        UpdateAssetStage us = GameCenter.curGameStage as UpdateAssetStage;
        if (us != null)
        {
            us.NeedTick = false;
            us.ForceReset();
        }
        GameCenter.uIMng.ReleaseGUI(GUIType.RETURN_EXIT);
        GameCenter.instance.GameExit();
    }

    void OnBtnWait(GameObject games)
    {
        UpdateAssetStage us = GameCenter.curGameStage as UpdateAssetStage;
        if (us != null)
        {
            us.NeedTick = false;
        }
        GameCenter.uIMng.ReleaseGUI(GUIType.RETURN_EXIT);
    }



}
