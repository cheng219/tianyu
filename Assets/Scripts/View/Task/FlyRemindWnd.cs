//======================================================
//作者:朱素云
//日期:2017/1/17
//用途:传送花费提示界面
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlyRemindWnd : GUIBase {

    public UIButton recharge;
    public UIButton sure;
    public UIButton close;
    public UIToggle tog;
    //protected TaskPathFind pathFind;

    void Awake()
    {
        mutualExclusion = false;
        layer = GUIZLayer.TIP;
        if (recharge != null) UIEventListener.Get(recharge.gameObject).onClick = delegate
        {
            GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
            GameCenter.uIMng.ReleaseGUI(GUIType.FLYREMIND);
        };
        if (sure != null) UIEventListener.Get(sure.gameObject).onClick = delegate
        {
            if (GameCenter.mainPlayerMng.MainPlayerInfo.RealYuanCount < 10)
            { 
                GameCenter.practiceMng.ReminderWnd(12, ConfigMng.Instance.GetUItext(347));
            }
            else
            {
                if (GameCenter.taskMng.flyType == 1)
                {
                    FlyToInTask();
                }
                if (GameCenter.taskMng.flyType == 2)
                {
                    FlyToInMap();
                }
            }
  
            //{
            //    if (pathFind == null) pathFind = GameCenter.uIMng.GetGui<TaskPathFind>();
            //    if (pathFind != null)
            //    {
            //        pathFind.FlyYes();
            //    }
            //}
            GameCenter.uIMng.ReleaseGUI(GUIType.FLYREMIND);
        };
        if (close != null) UIEventListener.Get(close.gameObject).onClick = delegate { GameCenter.uIMng.ReleaseGUI(GUIType.FLYREMIND); };
        if (tog != null)
        { 
            EventDelegate.Remove(tog.onChange, TogOnChange);
            EventDelegate.Add(tog.onChange, TogOnChange);
        }
    } 

    protected override void OnOpen()
    {
        base.OnOpen(); 
        tog.value = false;
    }
    protected override void OnClose()
    {
        base.OnClose(); 
    }

    protected void TogOnChange()
    {
        GameCenter.systemSettingMng.ShowFlyTips = tog.value;
    }

    void FlyToInMap() 
    {
        int sceneId = GameCenter.taskMng.seceneId;
        Vector3 target = GameCenter.taskMng.flyVec;
        GameCenter.curMainPlayer.StopMovingTo();
        GameCenter.curMainPlayer.CancelCommands();
        GameCenter.curMainPlayer.GoNormal();
        if (sceneId == 0)
        {
            SceneRef scene = GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef;
            if (scene != null)
            {
                if (scene.allow_fly_by_pos_src == 1)
                {
                    GameCenter.mainPlayerMng.C2S_Fly_Pint(GameCenter.mainPlayerMng.MainPlayerInfo.SceneID, (int)target.x, (int)target.y);
                }
                else
                {
                    GameCenter.messageMng.AddClientMsg(86);
                }
            }
            //this.gameObject.SetActive(false);
        }
        else
        {
            GameCenter.mainPlayerMng.C2S_Fly_Pint(sceneId, (int)target.x, (int)target.z);
            //this.gameObject.SetActive(false);
        }
    }




    void FlyToInTask()
    { 
        int targetID = GameCenter.taskMng.CurTargetID;
        int sceneId = GameCenter.taskMng.CurTargetSceneID;
        Vector3 point = GameCenter.taskMng.CurTargetPoint;
        List<FlyExRef> flyExRefList = ConfigMng.Instance.GetAllFlyExRef();

        GameCenter.mainPlayerMng.isStartingFlyEffect = false;
        GameCenter.curMainPlayer.ClearFlyEffect();

        for (int i = 0; i < flyExRefList.Count; i++)
        {
            if (flyExRefList[i].flyScence == sceneId)
            {
                sceneId = flyExRefList[i].goScence;
                point = new Vector3(flyExRefList[i].goScenceXZ.x, 0, flyExRefList[i].goScenceXZ.y);
            }
        }
        if (point != Vector3.zero)
        {
            TaskInfo taskInfo = GameCenter.taskMng.CurfocusTask;
            if (sceneId == 0)
            {
                GameCenter.curMainPlayer.CancelCommands();
                GameCenter.curMainPlayer.StopForFly();
                Command_FlyTo flyTo = new Command_FlyTo();
                flyTo.targetScene = GameCenter.mainPlayerMng.MainPlayerInfo.SceneID;
                flyTo.targetPos = point;
                flyTo.targetID = targetID;
                GameCenter.curMainPlayer.commandMng.PushCommand(flyTo);

                if (taskInfo != null && taskInfo.TargetPos.x == point.x && taskInfo.TargetPos.y == point.y)
                {
                    GameCenter.taskMng.CurTaskNeedFly = true;
                }
                else
                {
                    GameCenter.taskMng.CurTaskNeedFly = false;
                }

                GameCenter.taskMng.CurTargetSceneID = 0;
                GameCenter.taskMng.CurTargetPoint = Vector3.zero;
                GameCenter.taskMng.CurTargetID = 0;
                //GameCenter.curMainPlayer.GoNormal();
                //Debug.Log("结束后ID为" + GameCenter.taskMng.CurTargetID);
            }
            else
            {
                GameCenter.curMainPlayer.CancelCommands();
                GameCenter.curMainPlayer.StopForFly();

                Command_FlyTo flyTo = new Command_FlyTo();
                flyTo.targetScene = sceneId;
                flyTo.targetPos = point;
                flyTo.targetID = targetID;
                GameCenter.curMainPlayer.commandMng.PushCommand(flyTo);

                if (taskInfo != null && taskInfo.TargetPos.x == point.x && taskInfo.TargetPos.y == point.y && taskInfo.ContentValue == sceneId)
                {
                    GameCenter.taskMng.CurTaskNeedFly = true;
                }
                else
                {
                    GameCenter.taskMng.CurTaskNeedFly = false;
                }

                GameCenter.taskMng.CurTargetSceneID = 0;
                GameCenter.taskMng.CurTargetPoint = Vector3.zero;
                GameCenter.taskMng.CurTargetID = 0;
                //GameCenter.curMainPlayer.GoNormal();
                //Debug.Log("结束后ID为" + GameCenter.taskMng.CurTargetID);
            }
        }
    }
}
