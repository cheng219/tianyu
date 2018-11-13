//======================================
//作者:吴江
//日期:2016/3/2
//用途:断线重新连接的窗口
//======================================

using UnityEngine;
using System.Collections;

public class ReturnLoginWnd : GUIBase
{
    public GameObject goBackBtn;
    public UITimer showTime;


    float openTime = 0;
    float limitTime = 10;

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
        UIEventListener.Get(goBackBtn).onClick += OnBtnPassWord;
        showTime.StartIntervalTimer((int)limitTime);
    }


    void Update()
    {
        if (Time.time - openTime >= limitTime)
        {
            OnBtnPassWord(null);
        }
    }


    void OnBtnPassWord(GameObject games)
    {
        
        GameCenter.instance.IsReConnecteding = false;
        GameCenter.instance.GoPassWord();
        GameCenter.uIMng.ReleaseGUI(GUIType.RETURN_LOGIN);
        
    }
}
