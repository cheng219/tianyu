//==================================
//作者：黄洪兴
//日期：2016/7/14
//用途：开服贺礼界面类(开服贺礼活动和抽奖活动)
//=================================

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class OpenServerRewardWnd : GUIBase
{
    public UIToggle togLottery;
    public UIToggle togOpenServer;
    public UIToggle togTarot;

    //public WdfLotteryWnd lotteryWnd; 
    //public OpenServerActiveWnd openServerWnd;
    //public OpenServerTarotWnd openServerTarotWnd;

    public GameObject closeBtn; 
    public UIGrid activeTypeGird;  

    void Awake()
    {
        mutualExclusion = true;
        allSubWndNeedInstantiate = true;
        if (closeBtn != null) UIEventListener.Get(closeBtn).onClick = CloseThis;
        GameCenter.openServerRewardMng.C2S_AskAllOpenServerRewardInfo();
        GameCenter.openServerRewardMng.C2S_AskLotteryInfo();
        GameCenter.openServerRewardMng.C2S_AskTaroatInfo(1);
        if (togLottery != null)
        {
            UIEventListener.Get(togLottery.gameObject).onClick = OnClickTog;
            UIEventListener.Get(togLottery.gameObject).parameter = OpenServerType.lottery;
        }
        if (togOpenServer != null)
        {
            UIEventListener.Get(togOpenServer.gameObject).onClick = OnClickTog;
            UIEventListener.Get(togOpenServer.gameObject).parameter = OpenServerType.openServerce;
        }
        if (togTarot != null)
        {
            UIEventListener.Get(togTarot.gameObject).onClick = OnClickTog;
            UIEventListener.Get(togTarot.gameObject).parameter = OpenServerType.tarot;
        } 
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        GameCenter.openServerRewardMng.IsRotateOver = false;
        GameCenter.openServerRewardMng.isRotate = false;
        RefreshActiveTog(); 
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
            GameCenter.openServerRewardMng.OnCurOpenTypeUpdate += OnCurOpenTypeUpdate;
            GameCenter.openServerRewardMng.OnGetAllOpenServerInfo += RefreshActiveTog;
            GameCenter.openServerRewardMng.OnLotteryDataUpdate += RefreshActiveTog;
            GameCenter.openServerRewardMng.OnTarotDataUpdate += RefreshActiveTog;
        }
        else
        { 
            GameCenter.openServerRewardMng.OnCurOpenTypeUpdate -= OnCurOpenTypeUpdate;
            GameCenter.openServerRewardMng.OnGetAllOpenServerInfo -= RefreshActiveTog;
            GameCenter.openServerRewardMng.OnLotteryDataUpdate -= RefreshActiveTog;
            GameCenter.openServerRewardMng.OnTarotDataUpdate -= RefreshActiveTog;
        }
    }

    void OnClickTog(GameObject go)
    {
        if (GameCenter.openServerRewardMng.isRotate) return;
        OpenServerType type = (OpenServerType)UIEventListener.Get(go).parameter;
        if (GameCenter.openServerRewardMng.curOpenServerType != type)
        {
            GameCenter.openServerRewardMng.curOpenServerType = type;
        }
    }


    void OnCurOpenTypeUpdate()
    {
        int index = (int)GameCenter.openServerRewardMng.curOpenServerType;
        if (index != 0 && index <= subWndArray.Length)
        {
            //Debug.Log("type:" + subWndArray[index - 1].type);
            SwitchToSubWnd(subWndArray[index-1].type);
        }
    }
  

    void RefreshActiveTog()
    {
        if (GameCenter.openServerRewardMng.isRotate) return;
        if (GameCenter.openServerRewardMng.ServerData != null)//贺礼活动开了
        {
            if (togOpenServer != null) togOpenServer.gameObject.SetActive(true);
            if (GameCenter.openServerRewardMng.curOpenServerType == OpenServerType.none)
            {
                GameCenter.openServerRewardMng.curOpenServerType = OpenServerType.openServerce;
                if (togOpenServer != null) togOpenServer.value = true;
            }
        }
        else
        {
            if (togOpenServer != null) togOpenServer.gameObject.SetActive(false);
        }
        if (GameCenter.openServerRewardMng.lotteryData != null)//抽奖活动开了
        {
            if (togLottery != null) togLottery.gameObject.SetActive(true);
            if (GameCenter.openServerRewardMng.curOpenServerType == OpenServerType.none)
            {
                GameCenter.openServerRewardMng.curOpenServerType = OpenServerType.lottery;
                if (togLottery != null) togLottery.value = true;
            }
        }
        else
        {
            if (togLottery != null) togLottery.gameObject.SetActive(false);
        }
        if (GameCenter.openServerRewardMng.wdfTaroatData != null)//塔罗牌活动开了
        {
            if (togTarot != null) togTarot.gameObject.SetActive(true);
            if (GameCenter.openServerRewardMng.curOpenServerType == OpenServerType.none)
            {
                GameCenter.openServerRewardMng.curOpenServerType = OpenServerType.tarot;
                if (togTarot != null) togTarot.value = true;
            }
        }
        else
        {
            if (togTarot != null) togTarot.gameObject.SetActive(false);
        }
        if (togOpenServer != null && togOpenServer.gameObject.activeSelf && togOpenServer.value)
        {
            GameCenter.openServerRewardMng.curOpenServerType = OpenServerType.openServerce;
        }
        if (togLottery != null && togLottery.gameObject.activeSelf && togLottery.value)
        {
            GameCenter.openServerRewardMng.curOpenServerType = OpenServerType.lottery;
        }
        if (togTarot != null && togTarot.gameObject.activeSelf && togTarot.value)
        {
            GameCenter.openServerRewardMng.curOpenServerType = OpenServerType.tarot;
        }
        if (activeTypeGird != null)
        {
            activeTypeGird.repositionNow = true;
        }
    }

    void CloseThis(GameObject _obj)
    {
        if (GameCenter.openServerRewardMng.isRotate) return;
        GameCenter.openServerRewardMng.curOpenServerType = OpenServerType.none;
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
    } 

}

public enum OpenServerType
{ 
    none,
    /// <summary>
    /// 贺礼
    /// </summary>
    openServerce,
    /// <summary>
    /// 抽奖
    /// </summary>
    lottery,
    /// <summary>
    /// 塔罗牌
    /// </summary>
    tarot,
}
