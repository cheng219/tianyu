///////////////////////////////////////////////////////////////////////////////
// 作者：吴江
// 日期：2015/5/13
// 用途：选角平台的状态机
///////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CharacterSelectStage : GameStage
{
    public enum EventType
    {
        AWAKE = fsm.Event.USER_FIELD + 1,
        START,
        WAIT,
        STOP,
        RECONNECT,
    }

    protected GameObject choosePlayerSceneObj;
    protected LoginSceneCtrl loginSceneCtrl;





    protected override void InitStateMachine()
    {
        base.InitStateMachine();

        fsm.State awake = new fsm.State("awake", stateMachine);
        awake.onEnter += EnterAwakeState;
        awake.onAction += UpdateAwakeState;

        fsm.State start = new fsm.State("start", stateMachine);
        start.onEnter += EnterBegainState;

        fsm.State wait = new fsm.State("wait", stateMachine);
        wait.onEnter += EnterWaitState;
        wait.onAction += UpdateWaitState;

        reconnectState = new fsm.State("reconnectState", stateMachine);
        reconnectState.onEnter += EnterReconnectState;
        reconnectState.onAction += UpdateReconnectState;
        reconnectState.onExit += ExitReconnectState;

        fsm.State stop = new fsm.State("stop", stateMachine);
        stop.onEnter += EnterStopState;


        awake.Add<fsm.EventTransition>(stop, (int)EventType.STOP);
        awake.Add<fsm.EventTransition>(start, (int)EventType.START);

        start.Add<fsm.EventTransition>(wait, (int)EventType.WAIT);
        start.Add<fsm.EventTransition>(reconnectState, (int)EventType.RECONNECT);

        wait.Add<fsm.EventTransition>(start, (int)EventType.START);
        wait.Add<fsm.EventTransition>(reconnectState, (int)EventType.RECONNECT);

        reconnectState.Add<fsm.EventTransition>(start, (int)EventType.START);
        reconnectState.Add<fsm.EventTransition>(wait, (int)EventType.WAIT);

        stateMachine.initState = awake;
    }

    protected void EnterAwakeState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        GameCenter.cameraMng.BlackCoverAll(true);
        GameCenter.uIMng.ReleaseGUI(GUIType.PREVIEW_MAIN);
        GameCenter.uIMng.ReleaseGUI(GUIType.CHAT);
        GameCenter.uIMng.ReleaseGUI(GUIType.MAINFIGHT);
        GameCenter.uIMng.ReleaseGUI(GUIType.TASK);
        GameCenter.uIMng.ReleaseGUI(GUIType.LITTLEMAP);
        if (GameCenter.curMainPlayer != null)
        {
            Destroy(GameCenter.curMainPlayer.gameObject);
        }
        if (GameCenter.curMainEntourage != null)
        {
            Destroy(GameCenter.curMainEntourage.gameObject);
        }
        SceneManager.LoadScene("CharacterSelectStage");

    }


    protected void UpdateAwakeState(fsm.State _curState)
    {
        if (SceneManager.GetActiveScene().name == "CharacterSelectStage")
        {
            choosePlayerSceneObj = GameObject.Find("LoginScene");
            if (choosePlayerSceneObj != null)
            {
                loginSceneCtrl = choosePlayerSceneObj.GetComponent<LoginSceneCtrl>();
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


    protected void EnterBegainState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        //创建坐标网格
        InitSector(20, 20, 1, 1);


        //GameCenter.uIMng.SwitchToUI(GUIType.CHOOSE_CHAR);

        GameCenter.cameraMng.BlackCoverAll(false);
    }




    #region 等待 by吴江
    protected void EnterWaitState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
    }


    protected void UpdateWaitState(fsm.State _curState)
    {
    }
    #endregion


    protected void EnterStopState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (choosePlayerSceneObj != null)
        {
            Destroy(choosePlayerSceneObj);
            choosePlayerSceneObj = null;
        }
    }



    public void Stop()
    {
        stateMachine.Send((int)EventType.STOP);
    }
    public void GoToWait()
    {
        stateMachine.Send((int)EventType.WAIT);
    }


    void OnEnable()
    {
        Regist();
    }


    void OnDisable()
    {
        UnRegist();
        //GameCenter.uIMng.ReleaseGUI(GUIType.CHOOSE_CHAR);
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
        if (choosePlayerSceneObj != null)
        {
            Destroy(choosePlayerSceneObj);
            choosePlayerSceneObj = null;
        }
    }



    protected new void Regist()
    {
        GameCenter.OnConnectStateChange += OnConnectStateChange;
        GameCenter.msgLoackingMng.OnUpdateCmdDictionary += OnUpdateCmdDictionary;
    }


    protected new void UnRegist()
    {
        GameCenter.OnConnectStateChange -= OnConnectStateChange;
        GameCenter.msgLoackingMng.OnUpdateCmdDictionary -= OnUpdateCmdDictionary;
    }



    void OnConnectStateChange(bool _connect)
    {
        if (!_connect)
        {
            if (GameCenter.loginMng.CurConnectServerType == LoginMng.ConnectServerType.Queue && GameCenter.loginMng.IsActiveDisconnection)
            {
                stateMachine.Send((int)EventType.WAIT);
                GameCenter.loginMng.C2S_ConectGameServer();
            }
            else
            {
                stateMachine.Send((int)EventType.RECONNECT);
            }
        }
        else if (GameCenter.loginMng.CurConnectServerType == LoginMng.ConnectServerType.Game)
        {
            //链接成功，申请进入游戏
            GameCenter.loginMng.C2S_EnterGame();
        }
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

    /// <summary>
    /// 问答号
    /// </summary>
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
