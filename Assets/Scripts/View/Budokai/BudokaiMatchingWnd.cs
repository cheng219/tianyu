//==================================
//作者：黄洪兴
//日期：2016/5/11
//用途：武道会匹配成功面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BudokaiMatchingWnd : GUIBase
{
    public UILabel targetName;
    public UILabel time;
    public GameObject neverHint;
    public GameObject substitute;
    public GameObject selfGo;

    void Awake()
    {
        mutualExclusion = true;
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        if(time!=null)
        time.text= GameCenter.activityMng.RemainTime.ToString();
        InvokeRepeating("RefreshTime", 0.2f, 0.5f);
        if (neverHint != null) UIEventListener.Get(neverHint).onClick += NeverHint;
        if (substitute != null) UIEventListener.Get(substitute).onClick += Substitute;
        if (selfGo != null) UIEventListener.Get(selfGo).onClick += SelfGo;
        Refresh();

    }
    protected override void OnClose()
    {
        base.OnClose();
        if (neverHint != null) UIEventListener.Get(neverHint).onClick -= NeverHint;
        if (substitute != null) UIEventListener.Get(substitute).onClick -= Substitute;
        if (selfGo != null) UIEventListener.Get(selfGo).onClick -= SelfGo;
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



    void Refresh()
    {
        if (targetName!=null)
        targetName.text = GameCenter.activityMng.OpponentName;
    }




    void RefreshTime()
    {
        if (GameCenter.activityMng.RemainTime - (int)(Time.time - GameCenter.activityMng.GetTime) <= 0)
        {
          GameCenter.uIMng.SwitchToUI(GUIType.NONE);
          return;
        }
        if(time!=null)
       time.text = (GameCenter.activityMng.RemainTime - (int)(Time.time - GameCenter.activityMng.GetTime)).ToString();
    }

    void NeverHint(GameObject obj)
    {
        GameCenter.activityMng.C2S_EnterBudokai(2);
        GameCenter.activityMng.NeverHint = true;
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);

    }

    void Substitute(GameObject obj)
    {
        GameCenter.activityMng.C2S_EnterBudokai(0);
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);

    }
    void SelfGo(GameObject obj)
    {
        GameCenter.activityMng.C2S_EnterBudokai(1);
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
       
    }



}
