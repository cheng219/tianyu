//==================================
//作者：吴江
//日期:2015/3/7
//用途：二次确认框
//=====================================

using UnityEngine;
using System.Collections;

public class SecondConfirmWnd : GUIBase {


    public GameObject btnYes;
    public GameObject btnNo;
    public UILabel showText;

    protected static string content = string.Empty;
    protected static System.Action actionYes;
    protected static System.Action actionNo;

    void Awake()
    {
        mutualExclusion = false;
        Layer = GUIZLayer.TOPWINDOW;
        UIEventListener.Get(btnYes).onClick = OnClickYes;
        UIEventListener.Get(btnNo).onClick = OnClickNo;
    }

    public static void OpenConfirm(string _content,System.Action _yes, System.Action _no)
    {
        actionYes = _yes;
        actionNo = _no;
        content = _content;
        GameCenter.uIMng.GenGUI(GUIType.SECONDCONFIRM, true);
    }


    protected override void OnOpen()
    {
        base.OnOpen();
        showText.text = content;
    }

    protected void OnClickYes(GameObject _go)
    {
        if (actionYes != null)
        {
            actionYes();
        }
        GameCenter.uIMng.ReleaseGUI(GUIType.SECONDCONFIRM);
    }

    protected void OnClickNo(GameObject _go)
    {
        if (actionNo != null)
        {
            actionNo();
        }
        GameCenter.uIMng.ReleaseGUI(GUIType.SECONDCONFIRM);
    }
}
