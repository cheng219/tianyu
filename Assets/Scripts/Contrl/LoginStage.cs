///////////////////////////////////////////////////////////////////////////////
// 作者：吴江
// 日期：2015/5/20
// 用途：登录平台的状态机
///////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Text;
using System.Security.Cryptography;
using System;

public class LoginStage : GameStage {



    /// <summary>
    /// 当前登录界面类型
    /// </summary>
    protected EventType curLoginState = EventType.AWAKE;
    /// <summary>
    /// 当前登录界面类型(访问器)
    /// </summary>
    public EventType CurLoginState
    {
        set
        {
            if (curLoginState != value)
            {
                curLoginState = value;
                if (onLoginStateChange != null)
                {
                    onLoginStateChange();
                }
            }
        }
        get { return curLoginState; }
    }
    /// <summary>
    /// 当前登录界面类型变化的事件
    /// </summary>
    public static System.Action onLoginStateChange;




    public enum EventType
    {
        AWAKE = fsm.Event.USER_FIELD + 1,
        PASS_WORD,
        FIND_PASS,
        MAIN_LAND,
        REGIST,
        STOP,
		WAITCREATE,
    }


    protected override void InitStateMachine()
    {
        base.InitStateMachine();

        fsm.State awake = new fsm.State("awake", stateMachine);
        awake.onEnter += EnterAwakeState;
        awake.onAction += UpdateAwakeState;


        fsm.State password = new fsm.State("password", stateMachine);
        password.onEnter += EnterPassWordState;
		password.onAction += UpdatePassWordState;
		password.onExit += ExitPassWordState;


        fsm.State findpass = new fsm.State("findpass", stateMachine);
        findpass.onEnter += EnterFindPassState;

        fsm.State mainland = new fsm.State("mainland", stateMachine);


        fsm.State regist = new fsm.State("regist", stateMachine);
        regist.onEnter += EnterRegisState;

        fsm.State stop = new fsm.State("stop", stateMachine);
        stop.onEnter += EnterStopState;
		
		//等待选择
		fsm.State waitcreate = new fsm.State("waitcreate", stateMachine);
		waitcreate.onEnter += EnterWaitCreateState;
		waitcreate.onAction += UpdateWaitCreateState;

        awake.Add<fsm.EventTransition>(password, (int)EventType.PASS_WORD);

        password.Add<fsm.EventTransition>(findpass, (int)EventType.FIND_PASS);
        password.Add<fsm.EventTransition>(mainland, (int)EventType.MAIN_LAND);
        password.Add<fsm.EventTransition>(regist, (int)EventType.REGIST);
        password.Add<fsm.EventTransition>(stop, (int)EventType.STOP);

        findpass.Add<fsm.EventTransition>(password, (int)EventType.PASS_WORD);

        regist.Add<fsm.EventTransition>(password, (int)EventType.PASS_WORD);

		password.Add<fsm.EventTransition>(waitcreate, (int)EventType.WAITCREATE);
        stateMachine.initState = awake;
    }




    protected void EnterAwakeState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        GameCenter.instance.IsReConnecteding = false;
		GameCenter.uIMng.ReleaseGUI(GUIType.MERRYGOROUND);
        GameCenter.uIMng.ReleaseGUI(GUIType.LOADING);
        GameCenter.uIMng.ReleaseGUI(GUIType.MAINFIGHT);
        GameCenter.uIMng.ReleaseGUI(GUIType.LITTLEMAP);
        GameCenter.uIMng.ReleaseGUI(GUIType.TASK);
		GameCenter.uIMng.ReleaseGUI(GUIType.GUILDACTIVITYCOPPY);
        GameCenter.uIMng.ReleaseGUI(GUIType.TASK_FINDING);
        if (GameCenter.curMainPlayer != null)
        {
            Destroy(GameCenter.curMainPlayer.gameObject);
        }
        if (GameCenter.curMainEntourage != null)
        {
            Destroy(GameCenter.curMainEntourage.gameObject);
        }
        //GameCenter.uIMng.GenGUI(GUIType.LOGIN,true);
        GameCenter.cameraMng.BlackCoverAll(false);
        SceneManager.LoadScene("LoginStage");
        YvVoiceSdk.YvVoiceLogOut();
        CurLoginState = EventType.AWAKE;
        NetMsgMng.ConectClose();
    }


    protected void UpdateAwakeState(fsm.State _curState)
    {
        if (SceneManager.GetActiveScene().name == "LoginStage")
        {
			InitSector(100, 100, 1, 1);
            GameCenter.uIMng.SwitchToUI(GUIType.LOGIN);
            stateMachine.Send((int)EventType.PASS_WORD);
        }
    }


    protected void EnterPassWordState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        GameCenter.cameraMng.BlackCoverAll(false);
        GameCenter.curGameStage = this;
		Regist();
        CurLoginState = EventType.PASS_WORD;
        if (GameCenter.instance.IsConnected)
        {
            NetMsgMng.ConectClose();
        }
        if (GameCenter.instance.isPlatform && !GameCenter.instance.isSwitchAccount)//此方法从GameCenter放到此处,解决SDK登陆框导致UI放大问题  By邓成
		{
            LynSdkManager.Instance.UsrLogin(GameCenter.instance.gameObject.name, "OnLoginResult");
			GameCenter.instance.platformLoginSeralizeID = (int)NetMsgMng.CreateNewSerializeID();
			GameCenter.msgLoackingMng.UpdateSerializeList(GameCenter.instance.platformLoginSeralizeID, true);
		}
    }
	protected void ExitPassWordState(fsm.State _from, fsm.State _to, fsm.Event _event){

	}


	protected void UpdatePassWordState(fsm.State _curState){
	}







    protected void EnterFindPassState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        CurLoginState = EventType.FIND_PASS;
    }


    protected void EnterStopState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        CurLoginState = EventType.STOP;
    }


    protected void EnterRegisState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        CurLoginState = EventType.REGIST;
    }
	
	#region 等待选择 by 何明军
	protected void EnterWaitCreateState (fsm.State _from, fsm.State _to, fsm.Event _event)
	{
	}

	protected void UpdateWaitCreateState(fsm.State _curState)
	{

	}
	#endregion

	protected new virtual void Regist()
	{
		GameCenter.OnConnectStateChange += OnConnectStateChange;
        GameCenter.msgLoackingMng.OnUpdateCmdDictionary += OnUpdateCmdDictionary;
	}

	protected new virtual void UnRegist()
	{
		GameCenter.OnConnectStateChange -= OnConnectStateChange;
        GameCenter.msgLoackingMng.OnUpdateCmdDictionary -= OnUpdateCmdDictionary;
	}



	void OnConnectStateChange(bool _connect)
	{
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

    public void GoPassWord()
    {
        stateMachine.Send((int)EventType.PASS_WORD);
    }

    public void GoFindPass()
    {
        stateMachine.Send((int)EventType.FIND_PASS);
    }

    public void GoRegist()
    {
        stateMachine.Send((int)EventType.REGIST);
    }

	void OnDisable()
	{
		UnRegist();
	}
	//
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
    #region 获取服务器信息
    protected int getServerInfoSerlizeID = 0;//手动应答号ID
    public int GetServerInfoSerlizeID
    {
        get
        {
            return getServerInfoSerlizeID;
        }
        protected set
        {
            getServerInfoSerlizeID = value;
        }
    }
    protected bool firstAsk = true;//是否第一次压人应答号


    public void GetServerInfo(int _page)
    {
        StartCoroutine(GetServer(_page));
    }


    protected float startGetServerTime = 0;
    protected bool openGetServerSecondWnd = false;


    IEnumerator GetServer(int _page)
    {
        string sourceId = "201";
        string version = "1.4.1";
        if (GameCenter.instance.isPlatform)
        {
            sourceId = LynSdkManager.Instance.GetSourceId();
			version = LynSdkManager.Instance.GetAppVersion();
            //NGUIDebug.Log("version11111111:" + version);
            //NGUIDebug.Log("sourceId:" + sourceId);
        }
        if (sourceId == "")
        {
            sourceId = "0";
        }
        long dateTime = DateTime.Now.Ticks / 10000;
        string signString = "GetServerInfo" + sourceId + dateTime + "LynSDK";
        int lastID = 0;
        if (PlayerPrefs.HasKey("LastID"))
        {
            //lastID = GameCenter.loginMng.Login_ID;
        }
        string wwwText = string.Empty;
        if (GameCenter.instance.isPlatform)
        {
            if (GameCenter.instance.isInsideTest)
            {
                wwwText = string.Format("http://192.168.1.249:8080/game_gm/rpc/chooseServer?act=GetServerInfo&sourceId={0}&time={1}&pg={2}&sign={3}&svrid={4}&versions={5}", sourceId, dateTime, _page, StrToMD5(signString), lastID, version);
            }
            else
            {
                wwwText = string.Format("http://gm.lynlzqy.com:8080/game_gm/rpc/chooseServer?act=GetServerInfo&sourceId={0}&time={1}&pg={2}&sign={3}&svrid={4}&versions={5}", sourceId, dateTime, _page, StrToMD5(signString), lastID, version);
            }

        }
        else//电脑不传版本号
        {
            if (GameCenter.instance.isInsideTest)
            {
                wwwText = string.Format("http://192.168.1.249:8080/game_gm/rpc/chooseServer?act=GetServerInfo&sourceId={0}&time={1}&pg={2}&sign={3}&svrid={4}", sourceId, dateTime, _page, StrToMD5(signString), lastID);
            }
            else
            {
                wwwText = string.Format("http://gm.lynlzqy.com:8080/game_gm/rpc/chooseServer?act=GetServerInfo&sourceId={0}&time={1}&pg={2}&sign={3}&svrid={4}", sourceId, dateTime, _page, StrToMD5(signString), lastID);
            }
        }
        //NGUIDebug.Log("wwwText" + wwwText);
        WWW www = new WWW(wwwText);
        startGetServerTime = Time.time;
        GetServerInfoSerlizeID = (int)NetMsgMng.CreateNewSerializeID();
        GameCenter.msgLoackingMng.UpdateSerializeList(GetServerInfoSerlizeID, true);
        yield return www;
        if (www.isDone)
        {
            //NGUIDebug.Log("-isDone--www" + www.text);
            firstAsk = true;
            if (!www.text.Contains("result"))
            {
                //NGUIDebug.Log("-result-11-");
            }
            else
            {
                //NGUIDebug.Log("-result-222-");
                GameCenter.msgLoackingMng.UpdateSerializeList(GetServerInfoSerlizeID, false);
                GetServerInfoSerlizeID = 0;
				GameCenter.loginMng.ShowServerList(www.text);
            }
        }
    }


    public void TipReLoadServerInfo()
    {
        GameCenter.messageMng.AddClientMsg(307, delegate
        {
            //NGUIDebug.Log("SetDelay" + GameCenter.loginMng.CurServerPage);
            GetServerInfo(GameCenter.loginMng.CurServerPage);
        },
        delegate
        {
            Application.Quit();
        });
    }
    protected string StrToMD5(string _str)
    {
        byte[] bytes = Encoding.Default.GetBytes(_str.Trim());
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] sign = md5.ComputeHash(bytes);
        string temp = "";
        for (int i = 0; i < sign.Length; i++)
        {
            temp += sign[i].ToString("x2");
        }
        return temp.ToLower();
    }
    #endregion
}
