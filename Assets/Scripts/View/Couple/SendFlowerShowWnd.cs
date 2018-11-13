//======================================================
//作者:朱素云
//日期:2016/12/7
//用途:送花全屏炫耀
//======================================================
using UnityEngine;
using System.Collections;

public class SendFlowerShowWnd : GUIBase {

    public UILabel sendName;
    public UILabel receiveName;
    protected float timer = 0;

    void Awake()
    {
        mutualExclusion = false;
        layer = GUIZLayer.TIP;
    }
    void Update()
    {
        if (Time.time-timer > 9)
        {
            GameCenter.uIMng.ReleaseGUI(GUIType.SHOWFLOWER);
        } 
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        timer = Time.time;
        Show(); 
    }
    protected override void OnClose()
    {
        base.OnClose();
        timer = 0;
    }

    void Show()
    { 
        sendName.text = GameCenter.coupleMng.sendFowerName;
        receiveName.text = GameCenter.coupleMng.receiveFlowerName;
    }
}
