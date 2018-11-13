///////////////////////////////////////////////////////////////////////////////
// 作者：吴江
// 日期：2015/5/13
// 用途：主城平台的状态机
///////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// 主城平台的状态机 by吴江
/// </summary>
public class CityStage : PlayGameStage
{
	/// <summary>
    /// 主城界面类型变化的事件 by吴江
    /// </summary>
    public static System.Action onCityStateChange;



    #region 状态机

    protected override void InitStateMachine()
    {
        base.InitStateMachine();

        fsm.State awake = new fsm.State("awake", stateMachine);
        awake.onEnter += EnterAwakeState;
        awake.onAction += UpdateAwakeState;

        fsm.State load = new fsm.State("load", stateMachine);
        load.onEnter += EnterLoadtate;
        load.onAction += UpdateLoadState;
        load.onExit += ExitLoadState;

        reconnectState = new fsm.State("reconnectState", stateMachine);
        reconnectState.onEnter += EnterReconnectState;
        reconnectState.onAction += UpdateReconnectState;
        reconnectState.onExit += ExitReconnectState;

        fsm.State run = new fsm.State("run", stateMachine);
        run.onEnter += EnterRunState;
        run.onAction += UpdateRunState;
        run.onExit += ExitRunState;

        reconnectState.Add<fsm.EventTransition>(run, (int)EventType.RUN);

        run.Add<fsm.EventTransition>(reconnectState, (int)EventType.RECONNECT);

        awake.Add<fsm.EventTransition>(load, (int)EventType.LOAD);
        load.Add<fsm.EventTransition>(run, (int)EventType.RUN);
		load.Add<fsm.EventTransition>(awake, (int)EventType.AWAKE);
        load.Add<fsm.EventTransition>(reconnectState, (int)EventType.RECONNECT);//加载的时候可能掉线重连

        stateMachine.initState = awake;
    }

    /// <summary>
    /// 进入启动状态 by吴江
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    /// <param name="_event"></param>
    protected void EnterAwakeState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        myLastIP = Network.player.ipAddress;
        GameCenter.OnConnectStateChange += ConnectStateChange;
        SceneManager.LoadScene("CityStage");

        SceneID = (int)GameCenter.mainPlayerMng.MainPlayerInfo.SceneID;
        GameCenter.cameraMng.MainCameraActive(false);
    }
    /// <summary>
    /// 更新启动状态 by吴江
    /// </summary>
    /// <param name="_curState"></param>
    protected void UpdateAwakeState(fsm.State _curState)
    {
        if (SceneManager.GetActiveScene().name == "CityStage")
        {
            stateMachine.Send((int)EventType.LOAD);
        }
    }

    /// <summary>
    /// 进入加载状态 by吴江
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    /// <param name="_event"></param>
    protected void EnterLoadtate(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        GameCenter.cameraMng.MainCameraActive(true);
        GameCenter.cameraMng.BlackCoverAll(true);
		//登陆界面隐藏
        GameCenter.uIMng.ReleaseGUI(GUIType.LOGIN);
        //GameCenter.uIMng.ReleaseGUI(GUIType.CHOOSE_CHAR);
        //加载场景
        if (_from.name == "awake")
        {
            DoStartLoad(sceneID);
        }
    }
    /// <summary>
    /// 更新加载状态 by吴江
    /// </summary>
    /// <param name="_curState"></param>
    protected void UpdateLoadState(fsm.State _curState)
    {
        if (loadSceneTaskList.Count == 0)
        {
            if (!GameCenter.instance.IsReConnecteding && !NetCenter.Connected && GameCenter.messageMng != null && GameCenter.loginMng.CurConnectServerType == LoginMng.ConnectServerType.Game)
            {
                stateMachine.Send((int)EventType.RECONNECT);
            }
            else
            {
                stateMachine.Send((int)EventType.RUN);
            }
        }
    }
    /// <summary>
    /// 推出加载状态 by吴江
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    /// <param name="_event"></param>
    protected virtual void ExitLoadState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        Debug.Log("ExitLoadState");
        if (_to.name == "run")
        {
            sceneMng.C2S_EnterSceneSucceed();
            GameCenter.instance.GoRunCity();
            GameCenter.OnConnectStateChange -= ConnectStateChange;
            Regist();
        }
    }

    /// <summary>
    /// 加载结束，进入游戏运行状态 by吴江
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    /// <param name="_event"></param>
    protected void EnterRunState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (_from.name != "load" && _from.name != "newbieGuide")
        {
            GameCenter.cameraMng.BlackCoverAll(false);
        }
        SystemSettingMng.CullingShow = true;
		LoadSceneEffects();
        //NPC
        BuildNPCs();
        BuildMonsters(); ;
        BuildOPCs();
        BuildItems();
    }


    protected void UpdateRunState(fsm.State _curState)
    {
        CheckWifi();
    }

    protected void ExitRunState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        GameCenter.uIMng.ReleaseGUI(GUIType.LITTLEMAP);
        GameCenter.uIMng.ReleaseGUI(GUIType.TASK);
        GameCenter.uIMng.ReleaseGUI(GUIType.MAINFIGHT);
		GameCenter.uIMng.ReleaseGUI(GUIType.GUILDACTIVITYCOPPY);
    }
    #endregion




    void OnDisable()
    {
        UnRegist();
        GameCenter.uIMng.ReleaseGUI(GUIType.LITTLEMAP);
        GameCenter.uIMng.ReleaseGUI(GUIType.TASK);
        GameCenter.uIMng.ReleaseGUI(GUIType.MAINFIGHT);
		GameCenter.uIMng.ReleaseGUI(GUIType.COPYMULTIPLEWND);
		GameCenter.uIMng.ReleaseGUI(GUIType.GUILDACTIVITYCOPPY);
        if (GameCenter.curMainPlayer != null)
        {
            GameCenter.curMainPlayer.CurTarget = null;
        }
		GameCenter.uIMng.ReleaseGUI(GUIType.FUNCTION);
        GameCenter.uIMng.ReleaseGUI(GUIType.GUILDFIGHT);
    }

    protected override void OnSceneStateChange(bool _succeed)
    {
        base.OnSceneStateChange(_succeed);
        if (_succeed)
        {
            GameCenter.uIMng.SwitchToUI(GUIType.LITTLEMAP);
            GameCenter.uIMng.SwitchToUI(GUIType.TASK);
            GameCenter.uIMng.SwitchToUI(GUIType.MAINFIGHT);
//            if (!GameCenter.mainPlayerMng.MainPlayerInfo.IsJoinedCamp && GameCenter.mainPlayerMng.IsOpenFun(FunctionType.Camp))
//            {
//                GameCenter.uIMng.SwitchToUI(GUIType.CAMPJOIN);
//            }
        }
	}

}
