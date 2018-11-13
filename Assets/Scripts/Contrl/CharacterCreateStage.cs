///////////////////////////////////////////////////////////////////////////////
// 作者：吴江
// 日期：2015/5/13
// 用途：创角平台的状态机
///////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class CharacterCreateStage : GameStage
{
    public new enum EventType
    {
        AWAKE = fsm.Event.USER_FIELD + 1,
        START,
        WAITLOAD,
		CAMERAFLY,
		WAITCREATE,
        SELECTPROCESS,
        SELECTSURE,
        MOVETOSCENE,
        RECONNECT,
    }


    protected Camera myCamera;

    protected LoginSceneCtrl loginSceneCtrl;
    protected GameObject playerSceneObj;

    protected Light pointLight = null;

    protected Animator leftAnimator = null;
    protected Animator rightAnimator = null;

//    protected GameObject createPlayerSceneObj;
//    protected CreateCharacter ctrl;
//	public  uint CreateCharactorProf = 0;
//	FlashUI flash;

    protected override void InitStateMachine()
    {
        base.InitStateMachine();

        fsm.State awake = new fsm.State("awake", stateMachine);
        awake.onEnter += EnterAwakeState;
        awake.onAction += UpdateAwakeState;

        fsm.State start = new fsm.State("start", stateMachine);
        start.onEnter += EnterBegainState;
		

		
		//等待选择
		fsm.State waitcreate = new fsm.State("waitcreate", stateMachine);
        waitcreate.onEnter += EnterWaitCreateState;
        waitcreate.onAction += UpdateWaitCreateState;

        reconnectState = new fsm.State("reconnectState", stateMachine);
        reconnectState.onEnter += EnterReconnectState;
        reconnectState.onAction += UpdateReconnectState;
        reconnectState.onExit += ExitReconnectState;

        //转换条件
        awake.Add<fsm.EventTransition>(start, (int)EventType.START);
        awake.Add<fsm.EventTransition>(reconnectState, (int)EventType.RECONNECT);

        start.Add<fsm.EventTransition>(waitcreate, (int)EventType.WAITCREATE);
        start.Add<fsm.EventTransition>(reconnectState, (int)EventType.RECONNECT);

        waitcreate.Add<fsm.EventTransition>(reconnectState, (int)EventType.RECONNECT);

        reconnectState.Add<fsm.EventTransition>(start, (int)EventType.START);
        reconnectState.Add<fsm.EventTransition>(waitcreate, (int)EventType.WAITCREATE);

        stateMachine.initState = awake;
    }


    #region 启动

    protected void EnterAwakeState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (GameCenter.curMainPlayer != null)
        {
            Destroy(GameCenter.curMainPlayer.gameObject);
        }
        if (GameCenter.curMainEntourage != null)
        {
            Destroy(GameCenter.curMainEntourage.gameObject);
        }
        SceneManager.LoadScene("CharacterCreateStage");
    }


    protected void UpdateAwakeState(fsm.State _curState)
    {
        if (SceneManager.GetActiveScene().name == "CharacterCreateStage")
        {
            playerSceneObj = GameObject.Find("LoginScene");
            if (playerSceneObj != null)
            {
                loginSceneCtrl = playerSceneObj.GetComponent<LoginSceneCtrl>();
                if (loginSceneCtrl == null)
                {
                    Debug.LogError("LoginSceneCtrl 组件丢失");
                }
                else
                {
                    stateMachine.Send((int)EventType.START);
                }
            }
        }

    }
    #endregion

    #region 初始化
    protected void EnterBegainState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        Regist();
		//创建坐标网格
		InitSector(25, 25, 1, 1);


		GameCenter.uIMng.SwitchToUI(GUIType.CREATE_CHAR);

        stateMachine.Send((int)EventType.WAITCREATE);

        GameCenter.cameraMng.BlackCoverAll(false);
    }
    #endregion



    #region 等待选择 by吴江
    protected void EnterWaitCreateState (fsm.State _from, fsm.State _to, fsm.Event _event)
	{
	}

    protected void UpdateWaitCreateState(fsm.State _curState)
    {
        
    }
    #endregion


    /// <summary>
    /// 跳转停止选择状态
    /// </summary>
    public void GoToWait()
    {
        stateMachine.Send((int)EventType.WAITCREATE);
    }

	/// <summary>
	/// 跳转停止选择状态
	/// </summary>
    public void Stop()
    {
		GameCenter.OnConnectStateChange -= OnConnectStateChange;
		GameCenter.uIMng.ReleaseGUI(GUIType.CREATE_CHAR);
    }
	
	
	void OnDisable()
	{
        UnRegist();
		GameCenter.uIMng.ReleaseGUI(GUIType.CREATE_CHAR);
//		createPlayerList.Clear();
//		createPlayerList = null;
//		GameCenter.uIMng.SwitchToUI(GUIType.NONE);
//		if (createPlayerSceneObj != null)
//        {
//            Destroy(createPlayerSceneObj);
//            createPlayerSceneObj = null;
//        }
	}

    protected new virtual void Regist()
    {
        GameCenter.OnConnectStateChange += OnConnectStateChange;
        GameCenter.loginMng.OnCreatePlayer += OnCreatePlayer;
        GameCenter.msgLoackingMng.OnUpdateCmdDictionary += OnUpdateCmdDictionary;
    }

    protected new virtual void UnRegist()
    {
        GameCenter.OnConnectStateChange -= OnConnectStateChange;
        GameCenter.loginMng.OnCreatePlayer -= OnCreatePlayer;
        GameCenter.msgLoackingMng.OnUpdateCmdDictionary -= OnUpdateCmdDictionary;
    }


    void OnConnectStateChange(bool _connect)
    {
        if (!_connect && !GameCenter.loginMng.IsActiveDisconnection)
        {
            if (GameCenter.uIMng.CurOpenType != GUIType.RECONNECT)
            {
                stateMachine.Send((int)EventType.RECONNECT);
                LynSdkManager.Instance.ReportConnectionLose("0", "创建角色时,非主动断开连接,socket抛出断开事件!.");
            }
            return;
        }
        if (!_connect && GameCenter.loginMng.CurConnectServerType == LoginMng.ConnectServerType.Queue)
        {
            stateMachine.Send((int)EventType.WAITCREATE);
            GameCenter.loginMng.C2S_ConectGameServer();
        }
        else if (_connect && GameCenter.loginMng.CurConnectServerType == LoginMng.ConnectServerType.Game)
        {
            //链接成功，申请进入游戏
            GameCenter.loginMng.C2S_EnterGame();
        }
    }



    void OnCreatePlayer()
    {
        stateMachine.Send((int)EventType.SELECTSURE);
    }


    protected PlayerBaseInfo curSelectRole = null;

    public PlayerBaseInfo CurSelectRole
    {
        get
        {
            return curSelectRole;
        }
        set
        {
            curSelectRole = value;
            if (loginSceneCtrl != null && curSelectRole != null)
            {
                loginSceneCtrl.SetCurPlayer(curSelectRole);
            }
        }

    }

    protected void OnUpdateCmdDictionary()
    {
        if (GameCenter.msgLoackingMng.HasSerializeWaiting)
        {
            GameCenter.uIMng.GenGUI(GUIType.PANELLOADING, true);
        }
        else
        {
            GameCenter.uIMng.ReleaseGUI(GUIType.PANELLOADING);
        }
    }
}
