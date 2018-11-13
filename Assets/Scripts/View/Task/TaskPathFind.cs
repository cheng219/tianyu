//======================================================
//作者:黄洪兴
//日期:2016/7/18
//用途:寻路UI
//======================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TaskPathFind : GUIBase
{ 
    /// <summary>
    /// 飞行按钮
    /// </summary>
    public GameObject flyBtn;
    private List<FlyExRef> flyExRefList = new List<FlyExRef>();
    int sceneId=0;
    Vector3 point=Vector3.zero;
	protected int targetID = 0;

    public GameObject under10LevRemind;//10级以下的提示
    public GameObject under20LevRemind; //10—20级提示

    public GameObject effctBink;

    protected bool isOpen = false; 

    protected override void OnOpen()
    {
        base.OnOpen(); 
        isOpen = true;
        RefreWnd(true);
        GameCenter.taskMng.flyType = 1;
        GameCenter.curMainPlayer.isFirstOpen = true;
        if (flyBtn != null) UIEventListener.Get(flyBtn).onClick = FlyToPint;
        Refresh();
        flyExRefList = ConfigMng.Instance.GetAllFlyExRef();
        GameCenter.curMainPlayer.OnCloseOrStartFly += RefreWnd;
    }

    protected override void OnClose()
    {
        base.OnClose();
        isOpen = false;
        GameCenter.curMainPlayer.OnCloseOrStartFly -= RefreWnd;
    }
    void SetActive(bool _bool)
    { 
        if (flyBtn != null)
        {
            if (!_bool)
            {
                CancelInvoke("HideBtn");
                Invoke("HideBtn", 0.1f);
            }
            else
            {
                CancelInvoke("HideBtn");
                this.gameObject.SetActive(true);
            }
        } 
    }
    void RefreWnd(bool _open)
    {
        if (_open) GameCenter.taskMng.flyType = 1;
        isOpen = _open;
        SetActive(_open);
        Refresh();
        flyExRefList = ConfigMng.Instance.GetAllFlyExRef();
    }
    void HideBtn()
    { 
        this.gameObject.SetActive(false);
    } 
    void Refresh()
    { 
		targetID = GameCenter.taskMng.CurTargetID;
        sceneId = GameCenter.taskMng.CurTargetSceneID;
        point = GameCenter.taskMng.CurTargetPoint;
        SceneRef scene = GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef;
        if(scene!=null)
        {
            if (GameCenter.vipMng.VipData != null && GameCenter.vipMng.VipData.vLev > 0 || (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel >= 10 && GameCenter.mainPlayerMng.MainPlayerInfo.RealYuanCount >= 10))
            {
                effctBink.SetActive(true);
            }
            else
            {
                effctBink.SetActive(false);
            }
            if (GameCenter.vipMng.VipData != null && GameCenter.vipMng.VipData.vLev > 0)
            {
                under10LevRemind.SetActive(false);
                under20LevRemind.SetActive(false);
            }
            else
            {
                if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel < 10)
                {
                    under10LevRemind.SetActive(true);
                    under20LevRemind.SetActive(false);
                }
                else if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel <= 20)
                {
                     
                    under10LevRemind.SetActive(false);
                    under20LevRemind.SetActive(true);
                }
                else
                {
                    under10LevRemind.SetActive(false);
                    under20LevRemind.SetActive(false);
                }
            }  
            if (scene.id == sceneId)
            {
                if (flyBtn != null && isOpen)
                    flyBtn.gameObject.SetActive(scene.allow_fly_by_pos_src == 1);
            }
            else
            {
                if (flyBtn != null && isOpen)
                    flyBtn.gameObject.SetActive(scene.allow_fly_by_pos_dest == 1);
            }
        }

    }


    void FlyTo()
    {
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

    public void FlyYes()
    {
        SetActive(false); 
		//GameCenter.mainPlayerMng.isStartingFlyEffect = true;
        GameCenter.curMainPlayer.PlayBeginFlyEffect();
        CancelInvoke("FlyTo");
        Invoke("FlyTo", 0.55f);
    }

    void FlyToPint(GameObject _obj)
    {
        if (GameCenter.taskMng.CurTargetSceneID == 0 || GameCenter.taskMng.CurTargetSceneID == GameCenter.mainPlayerMng.MainPlayerInfo.SceneID)
        {
            Vector3 targetPos = ActorMoveFSM.LineCast(GameCenter.taskMng.CurTargetPoint,true);
            Vector3 curPos = GameCenter.curMainPlayer.transform.position;
            Vector3[] path = GameStageUtility.StartPath(targetPos,curPos);
            float distance = 0;
            if (path != null)
            {
                distance = path.Length == 2 ? Vector3.Distance(targetPos, curPos) : path.CountPathDistance();
                //Debug.Log("distance:" + distance);
                if (distance < 10)
                {
                    GameCenter.messageMng.AddClientMsg(509);
                    return;//距离太近的时候不传送
                }
            }
        }
        GameCenter.curMainPlayer.StopForFly();

        Refresh();

        if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel >= 10 && GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel < 21)
        {
            FlyYes();
        }
        //else if(GameCenter.taskMng.CurfocusTask.TaskType == TaskType.Ring&&GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel >= 10)
        //{
        //    FlyYes();
        //    return;
        //}
        else if (GameCenter.vipMng.VipData != null && GameCenter.vipMng.VipData.vLev <= 0 && GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel < 10)
        {
            GameCenter.messageMng.AddClientMsg(435);
        }
        else if (GameCenter.vipMng.VipData != null && GameCenter.vipMng.VipData.vLev > 0)
        {
            FlyYes();
        }
        else
        {
            if (GameCenter.mainPlayerMng.MainPlayerInfo.RealYuanCount < 10)
            {
                GameCenter.curMainPlayer.StopMovingTo();
                GameCenter.curMainPlayer.CancelCommands();
                GameCenter.curMainPlayer.GoNormal();
                GameCenter.uIMng.GenGUI(GUIType.FLYREMIND, true);
                //GameCenter.practiceMng.ReminderWnd(12, "真元");
            }
            else
            {
                if (GameCenter.systemSettingMng.ShowFlyTips)
                {
                    FlyYes();
                }
                else
                {
                    GameCenter.curMainPlayer.StopMovingTo();
                    GameCenter.curMainPlayer.CancelCommands();
                    GameCenter.curMainPlayer.GoNormal();                   
                    GameCenter.uIMng.GenGUI(GUIType.FLYREMIND, true);
                }

            }
        }

    } 
}
