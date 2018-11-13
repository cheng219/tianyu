//======================================================
//作者:鲁家旗
//日期:2016/12/29
//用途:新手引导进入副本前弹出的窗口
//======================================================
using UnityEngine;
using System.Collections;

public class NewFunctionCoppyTip : GUIBase {
    public TweenScale tweenScale;
    protected bool hasGoToCopy = false;
    void Awake()
    {
        mutualExclusion = true;
        Layer = GUIZLayer.TIP;
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        hasGoToCopy = false;
        if (tweenScale != null)
        {
            EventDelegate.Remove(tweenScale.onFinished, GoToCopy);
            EventDelegate.Add(tweenScale.onFinished, GoToCopy);
        }
    }
    protected override void OnClose()
    {
        base.OnClose();
        GoToCopy();//界面被强制关闭了也要进入副本 否则会中断进入副本,影响流程
    }
    void GoToCopy()
    {
        if (hasGoToCopy) return;//不能重复进来,会报错  by邓成
        hasGoToCopy = true;
        if (GameCenter.noviceGuideMng.newFunctionCopyId != 0)
        {
            GameCenter.mainPlayerMng.C2STaskFly(GameCenter.noviceGuideMng.newFunctionCopyId);
            GameCenter.noviceGuideMng.newFunctionCopyId = 0;
        }
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
    }
}
