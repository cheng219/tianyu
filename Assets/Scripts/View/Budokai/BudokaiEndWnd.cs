//==================================
//作者：黄洪兴
//日期：2016/5/10
//用途：武道会主界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;

public class BudokaiEndWnd : GUIBase
{

    public GameObject XBtn;
    public GameObject WinObj;
    public GameObject LoseObj;



    void Awake()
    {
        mutualExclusion = true;
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
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            if (XBtn != null)
                UIEventListener.Get(XBtn).onClick += BtnClose;
        }
        else
        {
            if (XBtn != null)
                UIEventListener.Get(XBtn).onClick -= BtnClose;
        }
    }



    void Refresh()
    {
        if(WinObj!=null)
            WinObj.SetActive(GameCenter.activityMng.IsWin);
        if (LoseObj != null)
            LoseObj.SetActive(!GameCenter.activityMng.IsWin);
		Invoke("InvokeClose",10f);
    }





    void BtnClose(GameObject _go)
    {
		CancelInvoke();
        GameCenter.activityMng.C2S_AskOutBudokai();
    }
	void InvokeClose()
	{
		GameCenter.activityMng.C2S_AskOutBudokai();
	}


}
