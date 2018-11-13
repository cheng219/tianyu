//====================================================
//作者：吴江
//日期：2015/5/7
//用途：登录数据管理类
//======================================================



using UnityEngine;
using System.Collections;
using st.net.NetBase;
using System;
using System.Collections.Generic;

using System.Text;

/// <summary>
/// 登录数据管理类 by吴江
/// </summary>
public class LoginMng
{
    #region 定义
    public enum ConnectServerType
    {
        Queue,
        Game,
    }


    public enum LoginPassType
    {
        IP,
        NAME,
        WORD,
        PORT,
        SERVERNAME,
    }
    #endregion

    #region 数据
    /// <summary>
    /// 登录ip by吴江
    /// </summary>
    protected string quaue_ip = string.Empty;
    /// <summary>
    /// 登录ip（访问器） by吴江
    /// </summary>
    public string Quaue_IP
    {
        set
        {
            if (quaue_ip != value)
            {
                quaue_ip = value;
                PlayerPrefs.SetString("LastIP", quaue_ip);
                if (onLoginPassChange != null)
                {
                    onLoginPassChange(LoginPassType.IP);
                }
            }
        }
        get { return quaue_ip; }
    }
    /// <summary>
    /// 登录端口 by吴江
    /// </summary>
    protected int quaue_port = 8000;
    /// <summary>
    /// 登录端口（访问器） by吴江
    /// </summary>
    public int Quaue_port
    {
        set
        {
            if (quaue_port != value)
            {
                quaue_port = value;
				PlayerPrefs.SetInt("LastPort", quaue_port);
                if (onLoginPassChange != null)
                {
                    onLoginPassChange(LoginPassType.PORT);
                }
            }
        }
        get { return quaue_port; }
    }
    /// <summary>
    /// 登录ip by吴江
    /// </summary>
    protected string login_ip = string.Empty;
    /// <summary>
    /// 登录ip（访问器） by吴江
    /// </summary>
    public string Login_IP
    {
        set
        {
            if (login_ip != value)
            {
                login_ip = value;
                PlayerPrefs.SetString("LastLoginIP", login_ip);
                if (onLoginPassChange != null)
                {
                    onLoginPassChange(LoginPassType.IP);
                }
            }
        }
        get { return login_ip; }
    }
    /// <summary>
    /// 登录端口 by吴江
    /// </summary>
    protected int login_port = 8000;
    /// <summary>
    /// 登录端口（访问器） by吴江
    /// </summary>
    public int Login_port
    {
        set
        {
            if (login_port != value)
            {
                login_port = value;
				PlayerPrefs.SetInt("LastLoginPort", login_port);
                if (onLoginPassChange != null)
                {
                    onLoginPassChange(LoginPassType.PORT);
                }
            }
        }
        get { return login_port; }
    }
    /// <summary>
    /// 登录帐号 by吴江
    /// </summary>
    protected string login_name = string.Empty;
    /// <summary>
    /// 登录帐号（访问器） by吴江
    /// </summary>
    public string Login_Name
    {
        set
        {
            if (login_name != value)
            {
				login_name = value;
                PlayerPrefs.SetString("LastName", login_name);
                if (onLoginPassChange != null)
                {
                    onLoginPassChange(LoginPassType.NAME);
                }
            }
        }
        get { return login_name; }
    }
    /// <summary>
    /// 登录密码 by吴江
    /// </summary>
    protected string login_word = string.Empty;
    /// <summary>
    /// 登录密码（访问器） by吴江
    /// </summary>
    public string Login_Word
    {
        set
        {
            if (login_word != value)
            {
                login_word = value;
                PlayerPrefs.SetString("LastPass", login_word);
                if (onLoginPassChange != null)
                {
                    onLoginPassChange(LoginPassType.WORD);
                }
            }
        }
        get { return login_word; }
    }
	protected string login_ID = "0";
	/// <summary>
	/// 登陆服务器ID
	/// </summary>
	public string Login_ID
	{
		get{return login_ID;}
		set
		{
			login_ID = value;
			PlayerPrefs.SetString("LoginID", login_ID);
		}
	}

    protected string platformID = string.Empty;
    public string PlatformID
    {
        get { return platformID; }
    }

	protected string loginServerID = string.Empty;
	/// <summary>
	/// 服务器登陆ID
	/// </summary>
	public string LoginServerID
	{
		set
		{
			loginServerID = value;
			if (onLoginPassChange != null)
			{
				onLoginPassChange(LoginPassType.SERVERNAME);
			}
		}
		get{return loginServerID;}
	}

	protected string loginServerName = string.Empty;
	/// <summary>
	/// 服务器名称
	/// </summary>
	public string LoginServerName
	{
		set
		{
			loginServerName = value;
			if (onLoginPassChange != null)
			{
				onLoginPassChange(LoginPassType.SERVERNAME);
			}
		}
		get { return loginServerName; }
	}
	/// <summary>
	/// 中文转数字字符串 
	/// </summary>
	string Chinese2Number(string text)
	{ 
		string number = string.Empty;
		for(int i = 0,len = text.Length;i < len;i++)
		{
			int temp = char.ConvertToUtf32(text,i);
			number = new System.Text.StringBuilder().Append(number).Append(number==string.Empty?"":"-").Append(temp).ToString();
		}
		return number;
	}
	/// <summary>
	/// 数字字符串解析成中文  
	/// </summary>
	string Number2Chinese(string number)
	{
		string chinese = string.Empty;
		string[] tempStr = number.Split("-"[0]);
		for (int i = 0,max=tempStr.Length; i < max; i++) {
			int tempInt = int.Parse(tempStr[i]);
			chinese = chinese + char.ConvertFromUtf32(tempInt);
		}
		return chinese;
	}
    /// <summary>
    /// 登录信息变化的事件 by吴江
    /// </summary>
    public static System.Action<LoginPassType> onLoginPassChange;
    /// <summary>
    /// 角色列表发生变化的事件 by吴江
    /// </summary>
    public static System.Action OnRoleListChange;
    /// <summary>
    /// 选择的角色 by吴江
    /// </summary>
    protected PlayerBaseInfo selRole = null;
    /// <summary>
    /// 选择的角色 by吴江
    /// </summary>
    public PlayerBaseInfo SelRole
    {
        get { return selRole; }
        set
        {
            if (selRole != value)
            {
                selRole = value;
                if (OnSelRoleChange != null)
                {
                    OnSelRoleChange();
                }
            }
        }
    }
    /// <summary>
    /// 当前选择的角色发生变化的事件 by吴江
    /// </summary>
    public static System.Action OnSelRoleChange;
    public void SetSelRole(uint _pid)
    {
        if (LoginPlayerDic.ContainsKey(_pid))
        {
            SelRole =  LoginPlayerDic[_pid] as PlayerBaseInfo;
        }
    }
    public PlayerBaseInfo GetSelRole(uint _pid)
    {
        if (LoginPlayerDic.ContainsKey(_pid))
        {
            return LoginPlayerDic[_pid] as PlayerBaseInfo;
        }
        return null;
    }
	/// <summary>
	/// 人物创建成功
	/// </summary>
	public System.Action OnCreatePlayer;
    /// <summary>
    /// 选择服务器时出现网络连接失败时将点击选区设置成当前服务器名 by 易睿
    /// </summary>
    public System.Action onCloseChoiceEvent;
    public void CloseServerChoiceWnd()
    {
        if (onCloseChoiceEvent != null)
            onCloseChoiceEvent();
    }
    /// <summary>
    /// 当前连接的服务器类型 by吴江
    /// </summary>
    protected ConnectServerType curConnectServerType = ConnectServerType.Queue;
    public ConnectServerType CurConnectServerType
    {
        get
        {
            return curConnectServerType;
        }
    }
    /// <summary>
    /// 角色列表
    /// </summary>
    protected FDictionary loginPlayerDic = new FDictionary();
    /// <summary>
    /// 角色列表数据 by吴江
    /// </summary>
    public FDictionary LoginPlayerDic
    {
        get
        {
            return loginPlayerDic;
        }
    }

	public int GetPlayerLastLoginTime(int _serverInstanceID)
	{
		if(loginPlayerDic.ContainsKey(_serverInstanceID))
		{
			PlayerBaseInfo info = loginPlayerDic[_serverInstanceID] as PlayerBaseInfo;
			if(info != null)
				return info.GetLastLoginTime;
		}
		Debug.LogError("玩家ID:"+_serverInstanceID+",GetLastLoginTime获取失败!");
		return 0;
	}

    /// <summary>
    /// 当前选中的角色id by吴江
    /// </summary>
    protected uint curSelectPlayerID;
    /// <summary>
    /// 当前选中的角色id by吴江
    /// </summary>
    public uint CurSelectPlayerID
    {
        get
        {
            return curSelectPlayerID;
        }
        protected set
        {
            curSelectPlayerID = value;
        }
    }
    /// <summary>
    /// 当前的排队key by吴江
    /// </summary>
    protected string curQueueKey;
    /// <summary>
    /// 当前的排队key by吴江
    /// </summary>
    public string CurQueueKey
    {
        get
        {
            return curQueueKey;
        }
        protected set
        {
            curQueueKey = value;
        }
    }

    public FDictionary createPlayerInfoDic = new FDictionary();
    /// <summary>
    /// 是否主动断开
    /// </summary>
    protected bool isActiveDisconnection = false;
    public bool IsActiveDisconnection
    {
        get
        {
            return isActiveDisconnection;
        }
        set
        {
            isActiveDisconnection = value;
        }
    }

    protected uint loginInSerlizeID = 0;
    /// <summary>
    /// 进入游戏的等待号
    /// </summary>
    public uint LoginInSerlizeID
    {
        get
        {
            return loginInSerlizeID;
        }
        set
        {
            loginInSerlizeID = value;
        }
    }

	public System.Action<string> OnServerListUpdateEvent;
	/// <summary>
	/// 获取到服务器列表后,显示出来  by邓成
	/// </summary>
	/// <param name="_serverResult">Server result.</param>
	public void ShowServerList(string _serverResult)
	{
		if(OnServerListUpdateEvent != null)
			OnServerListUpdateEvent(_serverResult);
	} 
    /// <summary>
    /// 是否创建过角色 by 朱素云
    /// </summary>
    public bool isCreatePlayer = false;
    /// <summary>
    /// 是否登陆成功
    /// </summary>
    public bool isLogin = false;
    #endregion

    #region 构造
    /// <summary>
    /// 返回该管理类的唯一实例 by吴江
    /// </summary>
    /// <returns></returns>
    public static LoginMng CreateNew()
    {
        if (GameCenter.loginMng == null)
        {
            LoginMng loginMng = new LoginMng();
            loginMng.Init();
            return loginMng;
        }
        else
        {
            GameCenter.loginMng.UnRegist();
            GameCenter.loginMng.Init();
            return GameCenter.loginMng;
        }
    }
	/// <summary>
	/// ping值改变
	/// </summary>
	public System.Action OnPingChange;
    /// <summary>
    /// 注册
    /// </summary>
    void Init()
    {
    //    InitPlatform();
		if(login_name == string.Empty && PlayerPrefs.HasKey("LastName"))     
        	login_name = PlayerPrefs.GetString("LastName");
		if(login_word == string.Empty && PlayerPrefs.HasKey("LastPass"))    
       		login_word = PlayerPrefs.GetString("LastPass");
		if(PlayerPrefs.HasKey("LastIP"))    
		{
			quaue_ip = PlayerPrefs.GetString("LastIP");
		}			
		if(PlayerPrefs.HasKey("LastLoginPort"))
		{
			login_port = PlayerPrefs.GetInt("LastLoginPort");
		}
		if(PlayerPrefs.HasKey("LastPort"))
		{
			quaue_port = PlayerPrefs.GetInt("LastPort");
		}
		if (PlayerPrefs.HasKey("ServerName"))
		{
			string number = PlayerPrefs.GetString("ServerName");
			loginServerName = Number2Chinese(number);//PlayerPrefs存储中文有问题,这是将存储的数字解析成中文
		}
        MsgHander.Regist(0xA102, S2C_OnGetPlayerInfoList);
        MsgHander.Regist(0xA104, S2C_OnGetQueueInfo);
        MsgHander.Regist(0xA105, S2C_OnGetQueueEnd);
        MsgHander.Regist(0xA107, S2C_OnCreateCharResult);
        MsgHander.Regist(0xB102, S2C_OnGetMainPlayerInfo);
        MsgHander.Regist(0xD001, S2C_OnGetErrorInfo);
        MsgHander.Regist(0xA10A, S2C_Ping);
		MsgHander.Regist(0xA002, S2C_OnGetSDKBindInfo);
		MsgHander.Regist(0xA004,S2C_OnLoginFailed);
		GameCenter.instance.PingListNum.Clear();
    }
    /// <summary>
    /// 注销
    /// </summary>
    void UnRegist()
    {
        MsgHander.UnRegist(0xA102, S2C_OnGetPlayerInfoList);
        MsgHander.UnRegist(0xA104, S2C_OnGetQueueInfo);
        MsgHander.UnRegist(0xA105, S2C_OnGetQueueEnd);
        MsgHander.UnRegist(0xA107, S2C_OnCreateCharResult);
        MsgHander.UnRegist(0xB102, S2C_OnGetMainPlayerInfo);
        MsgHander.UnRegist(0xD001, S2C_OnGetErrorInfo);
        MsgHander.UnRegist(0xA10A, S2C_Ping);
        MsgHander.UnRegist(0xA002, S2C_OnGetSDKBindInfo);
		MsgHander.UnRegist(0xA004,S2C_OnLoginFailed);
		isLoadFrom = false;
        createPlayerInfoDic.Clear();
        loginPlayerDic.Clear();
    }
    #endregion

    #region 通信
    #region S2C
    public void S2C_OnGetPlayerInfoList(Pt _info)
    {
        pt_usr_list_a102 pt_usr_list_A102Info = _info as pt_usr_list_a102;
        loginPlayerDic.Clear();
        if (pt_usr_list_A102Info != null)
        {
            bool noTargetPlayer = true;
            uint lastPlayerID = 0;
			for (int i = 0,max=pt_usr_list_A102Info.usr_list.Count; i < max; i++) {
				create_usr_info item = pt_usr_list_A102Info.usr_list[i];
				PlayerBaseInfo info = new PlayerBaseInfo(item);
				loginPlayerDic[info.ServerInstanceID] = info;
                Debug.Log("item.raw_server_id:" + item.raw_server_id + ",loginServerID:" + loginServerID);
                if (GameCenter.instance.isPlatform)
                {
                    if (item.raw_server_id.ToString() == loginServerID)
                    {
                        CurSelectPlayerID = (uint)info.ServerInstanceID;
                        noTargetPlayer = false;
                    }
                }
                lastPlayerID = (uint)info.ServerInstanceID;
			}
            if (noTargetPlayer) CurSelectPlayerID = lastPlayerID;//容错,没找到目标角色时,取最后一个
        }
        if (loginPlayerDic.Count > 0)//如果角色数量大于0，则跳转至选择角色状态，否则跳转至创角状态 by吴江 
        {
            if (GameCenter.instance.IsReConnecteding)
            {
                if (CurSelectPlayerID > 0)
                {
                    C2S_AskQueue(CurSelectPlayerID);
                }
                else
                {
                    CharacterSelectStage stage = GameCenter.curGameStage as CharacterSelectStage;
                    if (stage != null)
                    {
                        stage.GoToWait();
                    }
                    else
                    {
                        CharacterCreateStage charaStage = GameCenter.curGameStage as CharacterCreateStage;
                        if (charaStage != null)
                        {
                            charaStage.GoToWait();
                        }
                    }
                }
                return;
            }
            else
            {
                if (CurSelectPlayerID > 0)
                {
                    C2S_AskQueue(CurSelectPlayerID);
                }
            }
        }
        else
        {
            if (GameCenter.instance.IsReConnecteding)
            {
                CharacterCreateStage stage = GameCenter.curGameStage as CharacterCreateStage;
                if (stage != null)
                {
                    stage.GoToWait();
                }
            }
            else
            {
                GameCenter.instance.GoCreatChar();
            }
        }
    }

    /// <summary>
    /// 如果服务端返回了这个信息,说明需要排队. 但是如果客户端已经跳过了排队阶段,那么无视该协议 by吴江
    /// </summary>
    /// <param name="_info"></param>
    public void S2C_OnGetQueueInfo(Pt _info)
    {
        pt_queue_info_a104 pt = _info as pt_queue_info_a104;
        if (pt != null)
        {
            Debug.Log("当前排队总数:" + pt.max_num);
            Debug.Log("当前自己的排队位置:" + pt.cur_num);
        }
    }

	public Action<byte> OnLoginFailedEvent;
	/// <summary>
	/// 断线重连挤号返回协议
	/// </summary>
	protected void S2C_OnLoginFailed(Pt _info)
	{
		pt_login_failed_a004 pt = _info as pt_login_failed_a004;
		if(pt != null)
		{
			if(OnLoginFailedEvent != null)
				OnLoginFailedEvent(pt.reason);
		}
	}

    public void S2C_OnGetErrorInfo(Pt _info)
    {
        pt_error_info_d001 pt = _info as pt_error_info_d001;
		for (int i = 0,max=pt.msg.Count; i < max; i++) {
			st.net.NetBase.normal_info item = pt.msg[i];
//			Debug.LogError("服务端返回错误报告:" + item.data);
		}
    }

    public void S2C_OnGetSDKBindInfo(Pt _info)
    {
		pt_login_data_a002 msg = _info as pt_login_data_a002;
        if (msg != null)
        {
//			Debug.Log("S2C_OnGetSDKBindInfo:"+msg.data);
            LynSdkManager.Instance.LynUserInfoBind(msg.data);
            if (msg.data.Contains("data"))
            {
                LitJson.JsonData jsonData = LitJson.JsonMapper.ToObject(msg.data);
                string data = (string)jsonData["data"]["data"];
                string[] arr = data.Split('|');
                if (arr.Length > 0)
                {
                    platformID = arr[arr.Length - 1];
                    Debug.Log(platformID);
                }
            }
            else
            {
                Debug.LogError("数据结构错误:" + msg.data);
            }
        }
    }

    public void S2C_OnGetQueueEnd(Pt _info)
    {
        pt_net_info_a105 pt = _info as pt_net_info_a105;
        if (pt != null)
        {
            Login_IP = pt.ip;
            Login_port = (int)pt.port;
            CurQueueKey = pt.key;
            isActiveDisconnection = true;
            NetMsgMng.ConectClose();
            LoginInSerlizeID = NetMsgMng.CreateNewSerializeID();
        }
    }




    public void S2C_OnGetMainPlayerInfo(Pt _info)
    {
        isLogin = true;
        //Debug.logger.Log("S2C_OnGetMainPlayerInfo");
        pt_usr_info_b102 pt = _info as pt_usr_info_b102;
        GameCenter.mainPlayerMng = MainPlayerMng.CreateNew(new MainPlayerInfo(pt));
		GameCenter.mainPlayerMng.C2S_LoginInGame();
        GameCenter.mainPlayerMng.ApplySubData();
        if (MainPlayerMng.OnCreateNew != null)
        {
            MainPlayerMng.OnCreateNew();
        }
        if (GameCenter.instance.isPlatform)
        {
            MainPlayerInfo mainPlayerInfo = GameCenter.mainPlayerMng.MainPlayerInfo;
            //这里必须加角色创建时间(中间件要求) By邓成
            string playerName = mainPlayerInfo.Name + "|" + GetPlayerLastLoginTime(mainPlayerInfo.ServerInstanceID);
            LynSdkManager.Instance.UsrEnterGame(LoginServerID, LoginServerName, (ulong)mainPlayerInfo.ServerInstanceID, playerName, mainPlayerInfo.CurLevel,
				mainPlayerInfo.Prof.ToString(), mainPlayerInfo.GuildID.ToString(), mainPlayerInfo.VipLevel.ToString(), mainPlayerInfo.FightValue.ToString(), PlatformID);
            YvVoiceSdk.YvVoiceLogin(mainPlayerInfo.Name, mainPlayerInfo.ServerInstanceID);//云娃语音登录
            //if (GameCenter.instance.isDataEyePattern)
            //{
            //    DCAccount.setLevel(GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel);
            //    DCAccount.login(Login_ID.ToString(), LoginServerName);
            //}
        }
    }

    public bool isOnCreateChar = false;
    public void S2C_OnCreateCharResult(Pt _info)
    { 
        pt_create_usr_list_a107 pt = _info as pt_create_usr_list_a107;
        uint creatCharID = 0;
		for (int i = 0,max=pt.usr_list.Count; i < max; i++) {
			create_usr_info item = pt.usr_list[i];
			PlayerBaseInfo info = new PlayerBaseInfo(item);
			loginPlayerDic[info.ServerInstanceID] = info;
			creatCharID = (uint)info.ServerInstanceID;
		}
        isCreatePlayer = true;
        //GameCenter.instance.GoSelectChar();
        GameCenter.loginMng.C2S_AskQueue(creatCharID);
    }
	
    /// <summary>
    /// ping值
    /// </summary>
    public void S2C_Ping(Pt _info)
    {
        pt_ping_a10a pt = _info as pt_ping_a10a;
		if(pt != null)
		{
        	GameCenter.instance.PingTime = (DateTime.Now.Ticks - GameCenter.instance.PingStartTime) / 10000;
			if(OnPingChange != null){
				OnPingChange();
			}
		}
    }
    #endregion
    #region C2S
    /// <summary>
    /// 尝试连接指定IP的排队服务器 by吴江
    /// </summary>
    /// <param name="_ip"></param>
    public void C2S_ConectQueueServer(string _ip,int _port)
    {
        GameCenter.msgLoackingMng.CleanSerializeList();
        LoginInSerlizeID = 0;
        isActiveDisconnection = false;
        curConnectServerType = ConnectServerType.Queue;
        Quaue_IP = _ip;
        Quaue_port = _port;
        NetMsgMng.ConectServer(Quaue_IP, Quaue_port);
        if (!GameCenter.instance.IsReConnecteding)
        {
            GameCenter.instance.GoWaitConnect();
        }
    }
    /// <summary>
    /// 尝试连接指定IP的游戏服务器 by吴江
    /// </summary>
    /// <param name="_ip"></param>
    public void C2S_ConectGameServer()
    {
        curConnectServerType = ConnectServerType.Game;
        NetMsgMng.ConectServer(Login_IP, Login_port);
        if (!GameCenter.instance.IsReConnecteding)
        {
            GameCenter.instance.GoWaitConnect();
        }
    }
    /// <summary>
    /// 向服务端发送登录的请求   by吴江
    /// </summary>
    public void C2S_Login()
    {
        pt_login_a001 msg = new pt_login_a001();
		msg.seq = NetMsgMng.CreateNewSerializeID();
        msg.account = Login_Name;
        msg.password = Login_Word;
		msg.platform = (byte)(GameCenter.instance.isPlatform?1:0);//这里的平台标识,只是与后台的约定。与SDK的平台类型无关 By邓成
        NetMsgMng.SendMsg(msg);
	}
    /// <summary>
    /// 请求建立角色
    /// </summary>
    /// <param name="_prof"></param>
    public void C2S_ReqCreateChar(int _prof,string _name)
	{
		if(_name.Contains(" "))
		{
			GameCenter.messageMng.AddClientMsg(399);
			return;
		}
		if(!CheckBadWord(_name)){return ;}
		
        pt_req_create_usr_a006 msg = new pt_req_create_usr_a006();
        msg.seq = NetMsgMng.CreateNewSerializeID();
        msg.prof = (uint)_prof;
        msg.name = _name;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 询问队列信息
    /// </summary>
    /// <param name="_loginPID"></param>
    public void C2S_AskQueue(uint _loginPID)
    {
        CurSelectPlayerID = _loginPID;
        pt_req_net_a003 msg = new pt_req_net_a003();
        msg.uid = _loginPID;
        msg.seq = NetMsgMng.CreateNewSerializeID();
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 选中角色，并且链接游戏服务器成功后，请求进入游戏 by吴江
    /// </summary>
    public void C2S_EnterGame()
    {
        isActiveDisconnection = false;
        pt_usr_enter_b001 msg = new pt_usr_enter_b001();
        msg.uid = CurSelectPlayerID;
        msg.key = CurQueueKey;
        msg.seq = LoginInSerlizeID; ;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// ping值
    /// </summary>
    public void C2S_Ping()
    {
		LoginStage stage = GameCenter.curGameStage as LoginStage;
		if(GameCenter.mainPlayerMng == null || stage != null)return ;
        pt_ping_a10a msg = new pt_ping_a10a();
        GameCenter.instance.PingStartTime = DateTime.Now.Ticks;
        NetMsgMng.SendMsg(msg);
    }

    #endregion
    #endregion
	bool isLoadFrom = false;
	/// <summary>
	/// 敏感字符检查 by 何明军
	/// </summary>
	public bool CheckBadWord(string _text){
		if(!isLoadFrom){
			BadWordChecker.loadFromResources("Texture/Mingganzi");
			isLoadFrom = true;
		}
		string str = BadWordChecker.checkBadWord(_text);
		if(str != null){
			MessageST mst = new MessageST();
			mst.messID = 299;
			mst.words = new string[1]{str};
			GameCenter.messageMng.AddClientMsg(mst);
			return false;
		}
		return true;
	}
	/// <summary>
	/// 检查字是否在字集中 by 何明军
	/// </summary>
	public string FontHasCharacter(UIFont font,string contents){
		bool check = false;
		char[] chars = contents.ToCharArray();
		for(int i=0;i<chars.Length;i++){
			if(font.bmFont.GetGlyph(chars[i]) == null){
				contents = contents.Replace(chars[i],'■');
				check = true;
			}
		}
		if(check)return contents;
		return string.Empty;
	}

    /// <summary>
    /// 检查字是否在字集中 by 邓成
    /// </summary>
    public bool FontHasAllCharacter(UIFont font, string contents)
    {
        bool _hasAllCharaster = true;
        char[] chars = contents.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            if (font.bmFont.GetGlyph(chars[i]) == null)
            {
                _hasAllCharaster = false;
                break;
            }
        }
        if (contents.Contains("■"))
            return false;//有的地方在输入的时候将不包含的字替换成了"■",那么包含这个字的我们也认为检查不通过
        return _hasAllCharaster;
    }

	protected string sdkUserName = string.Empty;
	/// <summary>
	/// SDK登陆名字
	/// </summary>
	public string SDKUserName
	{
		get{return sdkUserName;}
		set
		{
			sdkUserName = value;
		}
	}
    /// <summary>
    /// 检测生僻字(后续的需求不明确先这样写一个方法)
    /// </summary>
    public bool CheckName(string _name)
    {
        if (_name.Contains("[") || _name.Contains("]") || _name.Contains("\\") || _name.Contains("/") || _name.Contains("-")|| _name.Contains(".")||_name.Contains("。"))
        {
            return true;
        }
        else
            return false;
    }
    public string SDKUserID = string.Empty;

	public int CurServerPage = 0;

    #region 辅助逻辑
    public void OnConectChange(bool _conectState)
    {
        if (_conectState)
        {
				//C2S_Login();
        }
    }

    public void InitCreatePlayerInfo()
    {
		createPlayerInfoDic.Add(1, new PlayerBaseInfo(1, 1,true));
		createPlayerInfoDic.Add(2, new PlayerBaseInfo(2, 2,true));
		createPlayerInfoDic.Add(3, new PlayerBaseInfo(3, 3,true));
    }
    #endregion
}
