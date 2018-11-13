//==================================
//作者：李邵南
//日期：2017/3/8
//用途：奇缘系统
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OnMiacleclick : SubWnd
{
    public GameObject enterBtn;
    public UILabel changeLabel;
    public UITimer totleTimer;
    public GameObject effect;
    public TweenPosition tweenPositon;
    public TweenScale tweenScale;
    void Awake()
    {
        if (enterBtn != null) UIEventListener.Get(enterBtn.gameObject).onClick = OnOpenThisUI;
    }

    void Start() 
    {
      
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        //MiracleEnterWnd();//
        if (totleTimer != null && GameCenter.miracleMng.miracleStatus == AccessState.ACCEPTED && GameCenter.miracleMng.miracleStatus != AccessState.NONE)
       {
         totleTimer.StartIntervalTimer(GameCenter.miracleMng.restTime - (int)Time.realtimeSinceStartup);
       }
    }

    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            GameCenter.miracleMng.OnMiracleEnterUpdateEvent += MiracleEnterWnd; 
        }
        else
        {
            GameCenter.miracleMng.OnMiracleEnterUpdateEvent -= MiracleEnterWnd; 
        }
    }

    void MiracleEnterWnd() 
    {
        ShowAnimation();
        //如果任务成功
       if (GameCenter.miracleMng.miracleStatus == AccessState.SUCCEED)
        {
            if (totleTimer != null)
            {
                //暂停计时
                totleTimer.StopTimer();
                if (changeLabel != null)
                    changeLabel.text = ConfigMng.Instance.GetUItext(141);
            }
            if (effect != null) 
            {
               effect.SetActive(true);
            }
        }
        if (totleTimer != null && GameCenter.miracleMng.miracleStatus == AccessState.ACCEPTED && GameCenter.miracleMng.miracleStatus != AccessState.NONE)
        {
            totleTimer.StartIntervalTimer(GameCenter.miracleMng.restTime - (int)Time.realtimeSinceStartup);
        }
    }

    void Update() 
    {
       //如果任务失败
       if (GameCenter.miracleMng.restTime - (int)Time.realtimeSinceStartup<=0 &&
           GameCenter.mainPlayerMng.MainPlayerInfo.CurFightVal < GameCenter.miracleMng.scoreTarget)
        {
           if(changeLabel!=null)
               changeLabel.text = ConfigMng.Instance.GetUItext(141);
           if (effect != null)
           {
               effect.SetActive(true);
           }
        }
    }

    void OnOpenThisUI(GameObject _go) 
    {
        //请求任务协议
        //GameCenter.miracleMng.C2S_ReqRoyalMiracleList();
        GameCenter.uIMng.SwitchToUI(GUIType.MIRACLE);
        //GameCenter.miracleMng.OnMiracleEnterUpdateEvent += MiracleEnterWnd;
    }

    /// <summary>
    /// 动画展示提醒玩家
    /// </summary>
    void ShowAnimation()
    { 
        if (GameCenter.miracleMng.miracleStatus == AccessState.ACCESS)//如果有可接的奇缘显示动画提示
        {
            if (tweenPositon != null)
            {
                tweenPositon.ResetToBeginning();
                tweenPositon.enabled = true;
            }
            if (tweenScale != null)
            {
                tweenScale.ResetToBeginning();
                tweenScale.enabled = true;
            }
        }
    }
}
