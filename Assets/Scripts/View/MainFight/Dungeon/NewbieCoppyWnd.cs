//============================
//作者：邓成
//日期：2017/5/11
//用途：新手副本显示界面类
//============================

using UnityEngine;
using System.Collections;

public class NewbieCoppyWnd : SubWnd
{
    public UITimer timer;
    public UILabel labAlive;
    public UILabel labMonsterNum;

    public TweenScale tweenScale;
    public GameObject progressGo;
    protected override void OnOpen()
    {
        base.OnOpen();
        ShowRelive();
        if (progressGo != null)
        {
            progressGo.SetActive(false);
        }
        
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
            GameCenter.noviceGuideMng.OnOutNewbieCoppyEvent += StartProgress;
            GameCenter.dungeonMng.OnDungeonTimeUpdate += ShowTime;
            GameCenter.dungeonMng.OnDGReliveTimesUpdate += ShowRelive;
            GameCenter.dungeonMng.OnDGMonsterUpdate += ShowMonsterNum;
        }
        else
        {
            GameCenter.dungeonMng.OnDungeonTimeUpdate -= ShowTime;
            GameCenter.dungeonMng.OnDGReliveTimesUpdate -= ShowRelive;
            GameCenter.dungeonMng.OnDGMonsterUpdate -= ShowMonsterNum;
            GameCenter.noviceGuideMng.OnOutNewbieCoppyEvent -= StartProgress;
        }
    }

    void StartProgress()
    {
        if (progressGo != null)
        {
            progressGo.SetActive(true);
        }
        if (tweenScale != null)
        {
            tweenScale.ResetToBeginning();
            EventDelegate.Remove(tweenScale.onFinished, OutCopy);
            EventDelegate.Add(tweenScale.onFinished, OutCopy);
            tweenScale.enabled = true;
        }
    }

    void OutCopy()
    {
        if (progressGo != null)
        {
            progressGo.SetActive(false);
        }
        GameCenter.duplicateMng.C2S_OutCopy();
    }
    

    void ShowTime()
    {
        int time = GameCenter.dungeonMng.DungeonTime;
        if (timer != null) timer.StartIntervalTimer(time);
    }
    void ShowRelive()
    {
        SceneRef sceneRef = GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef;
        if (sceneRef != null)
        {
            if (labAlive != null) labAlive.text = GameCenter.dungeonMng.DungeonReliveTimes + "/" + sceneRef.reviveNum;
        }
    }
    void ShowMonsterNum()
    {
        if (labMonsterNum != null) labMonsterNum.text = GameCenter.dungeonMng.DungeonMonsterNum.ToString();
    }
}
