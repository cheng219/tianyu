//======================================================
//作者:鲁家旗
//日期:2017/1/5
//用途:升级提示UI
//======================================================
using UnityEngine;
using System.Collections;

public class UpTipWnd : GUIBase {
    public UIButton toFirstBtn;
    public UIButton closeBtn;

    void Awake()
    {
        mutualExclusion = false;
        Layer = GUIZLayer.NORMALWINDOW;
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = delegate {
            GameCenter.uIMng.ReleaseGUI(GUIType.UPTIPUI);
        };
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        if (toFirstBtn != null) UIEventListener.Get(toFirstBtn.gameObject).onClick = delegate
        {
            GameCenter.uIMng.ReleaseGUI(GUIType.UPTIPUI);
            GameCenter.uIMng.SwitchToUI(GUIType.FIRSTCHARGEBONUS);
        };
    }
    protected override void OnClose()
    {
        base.OnClose();
    }
}
