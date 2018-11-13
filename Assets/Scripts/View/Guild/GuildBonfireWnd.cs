//==================================
//作者：黄洪兴
//日期：2016/5/23
//用途：仙盟篝火界面类
//=================================

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GuildBonfireWnd : GUIBase
{
    public GameObject closeBtn;
    public GuildBonfireItemContainer guildBonfireItemContainer;

    void Awake()
    {
        mutualExclusion = true;
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        if (closeBtn != null) UIEventListener.Get(closeBtn).onClick += CloseThis;
        GameCenter.activityMng.OnGoTOtherBonfireInfo += Refresh;
        GameCenter.activityMng.C2S_AskOtherBonfireInfo();
    }
    protected override void OnClose()
    {
        base.OnClose();
        if (closeBtn != null) UIEventListener.Get(closeBtn).onClick -= CloseThis;
        GameCenter.activityMng.OnGoTOtherBonfireInfo -= Refresh;
    }
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
           
        }
        else
        {
           

        }
    }
    //void Update()
    //{

    //}

    void Refresh()
    {
        guildBonfireItemContainer.RefreshItems(GameCenter.activityMng.OtherBonfireList);

    }
    void CloseThis(GameObject obj)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
    }

    void OnPreviousSessionBtn(GameObject obj)
    {
    }
    void OnNextSessionBtn(GameObject obj)
    {
    }
    void OnStarBtn(GameObject obj)
    {


    }

}
