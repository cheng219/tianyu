//==============================
//作者：邓成
//日期：2016/7/12
//用途：选服界面类
//==============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Security.Cryptography;

public class SelectServerWnd : SubWnd {
	/// <summary>
	/// 返回重新登陆账号
	/// </summary>
	public GameObject btnReturn;
	/// <summary>
	/// 公告
	/// </summary>
	public GameObject btnNotice;
	/// <summary>
	/// 切换服务器
	/// </summary>
	public GameObject btnChangeServer;
	/// <summary>
	/// 开始游戏
	/// </summary>
	public GameObject btnEnterGame;
    public ServerChoiceItem lastLoginServer;
    protected ServerChoiceData lastServerData;

	public GameObject serversPanel;
	public UIGrid serverGrid;
	public ServerChoiceItem serverItem;
	public UIGrid toggleGrid;
	public ServerPageItem myServerBtn;
	/// <summary>
	/// 服务器选择界面开始游戏
	/// </summary>
	public GameObject btnEnterServer;

	protected List<ServerChoiceItem> serverChoiceItems = new List<ServerChoiceItem>();
	protected List<ServerPageItem> serverPageItems = new List<ServerPageItem>();
	/// <summary>
	/// key为页数
	/// </summary>
	protected Dictionary<int, List<ServerChoiceData>> serverChoiceDic= new Dictionary<int, List<ServerChoiceData>>(); 
	protected List<ServerPageData> serverPageList = new List<ServerPageData>();

	void Awake()
	{
		if(btnChangeServer != null)UIEventListener.Get(btnChangeServer).onClick = OnClickServer;
		if(btnReturn != null)UIEventListener.Get(btnReturn).onClick = OnClickReturn;
		if(btnNotice != null)UIEventListener.Get(btnNotice).onClick = OnClickNotice;
		if(btnEnterGame != null)UIEventListener.Get(btnEnterGame).onClick = OnClickLoginBtn;
		if(btnEnterServer != null)UIEventListener.Get(btnEnterServer).onClick = OnClickLoginBtn;
	}
	protected override void OnOpen ()
	{
		base.OnOpen ();
        GameCenter.instance.ShowNotice();
		GetMyServerInfo();
	}
	protected override void OnClose ()
	{
		base.OnClose ();
	}
	protected override void HandEvent (bool _bind)
	{
		base.HandEvent (_bind);
		if(_bind)
		{
			GameCenter.loginMng.OnServerListUpdateEvent += SetServerData;
			LoginMng.onLoginPassChange += RefreshServerName;
		}else
		{
			GameCenter.loginMng.OnServerListUpdateEvent -= SetServerData;
			LoginMng.onLoginPassChange -= RefreshServerName;
		}
	}
	#region 控件监听
	void OnClickReturn(GameObject go)
	{
		if(GameCenter.instance.isPlatform)
			GameCenter.uIMng.SwitchToSubUI(SubGUIType.PLATFORMLOGIN);
		else
			GameCenter.uIMng.SwitchToSubUI(SubGUIType.NORMALLOGIN);
	}
	void OnClickNotice(GameObject go)
	{
		GameCenter.instance.ShowNotice();
	}
	/// <summary>
	/// 点击登陆按钮的操作
	/// </summary>
	/// <param name="_btn"></param>
	protected void OnClickLoginBtn(GameObject _btn)
	{
		//dateTime = 
		StartCoroutine("CheckWhiteList");
	}
	/// <summary>
	/// 打开选服列表
	/// </summary>
	void OnClickServer(GameObject go)
	{
		if(serversPanel != null)serversPanel.SetActive(true);
		ShowPages();
		ShowServers();//这里显示的是历史服务器
	}
	/// <summary>
	/// 关闭选服列表
	/// </summary>
	void OnClickCloseServer(GameObject go)
	{
		
	}
	#endregion

	#region 显示
	void RefreshServerName(LoginMng.LoginPassType passType)
	{
		if(passType == LoginMng.LoginPassType.SERVERNAME)
		{
		//	if(curLastServerName != null)curLastServerName.text = GameCenter.loginMng.LoginServerName;
		//	if(curLastServerState != null)curLastServerState.text = GameCenter.loginMng.LoginServerID;
		}
	}

	void SetServerData(string wwwText)
	{
		if(!serverChoiceDic.ContainsKey(GameCenter.loginMng.CurServerPage))
			serverChoiceDic[GameCenter.loginMng.CurServerPage] = new List<ServerChoiceData>();
		else
			serverChoiceDic[GameCenter.loginMng.CurServerPage].Clear();
		serverPageList.Clear();
		if (wwwText != string.Empty && wwwText != "")
		{
			LitJson.JsonData jsonData = LitJson.JsonMapper.ToObject(wwwText);
			if (jsonData != null && wwwText.Contains("state"))
			{
				if ((int)jsonData["state"] == 1)
				{
					if (wwwText.Contains("data") && wwwText.Contains("page"))
					{
                        int totalSize = (int)jsonData["data"]["page"]["total"];
                        int totalPage = (totalSize % 10 == 0) ? totalSize / 10 : (totalSize / 10 + 1);
						for (int i = 0; i <= totalPage; i++) {
							ServerPageData data = new ServerPageData(i);
							serverPageList.Add(data);
						}
					}
                    for (int i = 0; i < jsonData["data"]["serverList"].Count; i++)
					{
						ServerChoiceData refData = new ServerChoiceData();
                        refData.serverID = (jsonData["data"]["serverList"][i]["svrid"]).ToString();
                        refData.serverName = (string)jsonData["data"]["serverList"][i]["svrname"];
                        refData.serverStatus = (int)jsonData["data"]["serverList"][i]["status"];
						//refData.serverIP = (string)jsonData["serverinfos"][i]["ip"];
						//refData.serverPort = (string)jsonData["serverinfos"][i]["port"];
						serverChoiceDic[GameCenter.loginMng.CurServerPage].Add(refData);
					}
				}
				else//服务器维护或者认证失败
				{
					Debug.Log("服务器验证失败或正在维护");
				}
			}
		}
		ShowServers();
	}
	void ShowServers()
	{
		int page = GameCenter.loginMng.CurServerPage;
		List<ServerChoiceData> serverChoiceList = serverChoiceDic.ContainsKey(page)?serverChoiceDic[page]:new List<ServerChoiceData>();
		for (int i = 0,max=serverChoiceItems.Count; i < max; i++) {
			if(serverChoiceItems[i] != null)
			{
				serverChoiceItems[i].SetUnChecked();
				serverChoiceItems[i].gameObject.SetActive(false);
			}
		}
		ServerChoiceItem lastServer = null;
		for (int i = 0,max=serverChoiceList.Count; i < max; i++) {
			ServerChoiceItem serverChoiceItem = null;
			if(serverChoiceItems.Count <= i)
			{
				if(serverItem != null && serversPanel != null)
				{
					serverChoiceItem = serverItem.CreateNew(serverGrid.transform);
					serverChoiceItems.Add(serverChoiceItem);
				}
			}
			serverChoiceItem = serverChoiceItems.Count > i?serverChoiceItems[i]:null;
			if(serverChoiceItem != null)
			{
				serverChoiceItem.gameObject.SetActive(true);
				serverChoiceItem.SetData(serverChoiceList[i],OnChooseServer);
				if(i == 0)//默认选中第一个
					lastServer = serverChoiceItems[i];
			}
		}
		if(serverGrid != null)serverGrid.repositionNow = true;
		if(lastServer != null)lastServer.SetChecked();
	}
	void OnChooseServer(ServerChoiceData data)
	{
		GameCenter.loginMng.LoginServerName = data.serverName;
		GameCenter.loginMng.LoginServerID = data.serverID;
	}

	void ShowPages()
	{
		for (int i = 0,max=serverPageItems.Count; i < max; i++) {
			if(serverPageItems[i] != null)
				serverPageItems[i].gameObject.SetActive(false);
		}
		ServerPageItem myPageItem = null;
		for (int i = 0,max=serverPageList.Count; i < max; i++) {
			ServerPageItem serverPageItem = null;
			if(serverPageItems.Count <= i)
			{
				if(myServerBtn != null && serversPanel != null)
				{
					serverPageItem = myServerBtn.CreateNew(toggleGrid.transform);
					serverPageItems.Add(serverPageItem);
				}
			}
			serverPageItem = serverPageItems.Count > i?serverPageItems[i]:null;
			if(serverPageItem != null)
			{
				serverPageItem.gameObject.SetActive(true);
				serverPageItem.SetData(serverPageList[i],OnChoosePage);
				if(serverPageList[i].curPage == GameCenter.loginMng.CurServerPage)
					myPageItem = serverPageItems[i];
			}
		}
		if(toggleGrid != null)toggleGrid.repositionNow = true;
		if(myPageItem != null)myPageItem.SetChected();
	}
	void OnChoosePage(ServerPageData data)
	{
		GameCenter.loginMng.CurServerPage = data.curPage;
		if(serverChoiceDic.ContainsKey(data.curPage))
		{
			ShowServers();
		}else
		{
			GetServerInfo(data.curPage);
		}
	}
	#endregion

	#region 白名单
	protected long dateTime = 0;
	protected int loginSeralizedID = 0;

	IEnumerator CheckWhiteList()
	{
		string sourceId = "0";
		dateTime = DateTime.Now.Ticks / 10000;
		if (GameCenter.instance.isPlatform)
			sourceId = LynSdkManager.Instance.GetSourceId();
		int serverID = int.Parse(GameCenter.loginMng.LoginServerID);
        string userID = GameCenter.loginMng.SDKUserID;
        string sign = string.Format(SystemSettingMng.WHITE_LIST_HTTP_ADDRESS_PARAMETER,sourceId,serverID, dateTime);
        string signResult = GameHelper.SignString(sign);
        //Debug.Log("WHITE_LIST signResult:" + signResult);
		string wwwText = string.Empty;
		if (GameCenter.instance.isInsideTest)
		{
            wwwText = string.Format(SystemSettingMng.WHITE_LIST_HTTP_ADDRESS, sourceId, serverID, dateTime, signResult);
		}
		else
		{
			wwwText = string.Format(SystemSettingMng.WHITE_LIST_HTTP_ADDRESS, sourceId, serverID, dateTime, signResult);
		}
		WWW www = new WWW(wwwText);
		loginSeralizedID = (int)NetMsgMng.CreateNewSerializeID();
		GameCenter.msgLoackingMng.UpdateSerializeList(loginSeralizedID, true);
		yield return www;
		GameCenter.msgLoackingMng.UpdateSerializeList(loginSeralizedID, false);
		Debug.Log("www.text:" + www.text);
		LitJson.JsonData jsonData = LitJson.JsonMapper.ToObject(www.text);
		if(www.text.Contains("state"))
		{
			if ((int)jsonData["state"] == 1)
			{
				GameCenter.loginMng.Login_ID = (jsonData["data"]["svrID"]).ToString();
				GameCenter.loginMng.Quaue_IP = (string)jsonData["data"]["ip"];
				GameCenter.loginMng.Quaue_port = int.Parse((string)jsonData["data"]["port"]);
				if (GameCenter.instance.IsConnected)
				{
					GameCenter.loginMng.C2S_Login();
				}
				else
				{
					GameCenter.loginMng.C2S_ConectQueueServer(GameCenter.loginMng.Quaue_IP, GameCenter.loginMng.Quaue_port);
				}
			}
			else
			{
                GameCenter.messageMng.AddClientMsg(358);//验证不成功就提示维护中
                /*
				switch ((LoginBackType)((int)jsonData["state"]))
				{ 
				case LoginBackType.MAINTAIN:
					GameCenter.messageMng.AddClientMsg(358);
					break;
				case LoginBackType.SIGNERROR:
					GameCenter.messageMng.AddClientMsg(359);
					break;
				case LoginBackType.MISSING:
					GameCenter.messageMng.AddClientMsg(359);
					break;
				case LoginBackType.NOSERVERID:
					GameCenter.messageMng.AddClientMsg(359);
					break;
				}
                */
			}
		}
	}
	/// <summary>
	/// 登陆请求时返回的类型
	/// </summary>
	public enum LoginBackType
	{ 
		/// <summary>
		/// 成功
		/// </summary>
		SUCCESS = 0,
		/// <summary>
		/// 服务器维护或者关闭
		/// </summary>
		MAINTAIN,
		/// <summary>
		/// 签名认证失败 （错误ID 1001）
		/// </summary>
		SIGNERROR,
		/// <summary>
		/// 缺少参数传递 （错误ID 1002）
		/// </summary>
		MISSING,
		/// <summary>
		/// 找不到服务器ID （错误ID 1003）
		/// </summary>
		NOSERVERID,
	}
	#endregion

	#region 获取服务器信息
	/// <summary>
	/// 获取某页的服务器列表
	/// </summary>
	public void GetServerInfo(int _page)
	{
		StartCoroutine(GetServer(_page));
	}
	IEnumerator GetServer(int _page)
	{
		string sourceId = "201";
		string version = "1.4.1";
		if (GameCenter.instance.isPlatform)
		{
			sourceId = LynSdkManager.Instance.GetSourceId();
			version = LynSdkManager.Instance.GetAppVersion();
		}
		if (sourceId == "")
		{
			sourceId = "0";
		}
		long dateTime = DateTime.Now.Ticks / 10000;
        string sign = string.Format(SystemSettingMng.PAGE_SERVER_HTTP_ADDRESS_PARAMETER, _page,sourceId, dateTime, version);
        string signResult = GameHelper.SignString(sign);
        //Debug.Log("PAGE_SERVER signResult:" + signResult);
		string wwwText = string.Empty;
		if (GameCenter.instance.isInsideTest)
		{
            wwwText = string.Format(SystemSettingMng.PAGE_SERVER_HTTP_ADDRESS, _page, sourceId, dateTime, version, signResult);
		}
		else
		{
            wwwText = string.Format(SystemSettingMng.PAGE_SERVER_HTTP_ADDRESS, _page, sourceId, dateTime, version, signResult);
		}
		//NGUIDebug.Log("wwwText" + wwwText);
		WWW www = new WWW(wwwText);
		int serlizeID = (int)NetMsgMng.CreateNewSerializeID();
		GameCenter.msgLoackingMng.UpdateSerializeList(serlizeID, true);
		yield return www;
		if (www.isDone)
		{
			Debug.Log("www.text:"+www.text);
			//NGUIDebug.Log("-isDone--www" + www.text);
			if (!www.text.Contains("state"))
			{
				//NGUIDebug.Log("-result-11-");
			}
			else
			{
				//NGUIDebug.Log("-result-222-");
				GameCenter.msgLoackingMng.UpdateSerializeList(serlizeID, false);
				GameCenter.loginMng.ShowServerList(www.text);
			}
		}
	}
	/// <summary>
	/// 重新加载
	/// </summary>
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
	/// <summary>
	/// MD5加密
	/// </summary>
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

	#region 获取最近登录服务器信息
	/// <summary>
	/// 获取玩家登陆过的服务器列表
	/// </summary>
	public void GetMyServerInfo()
	{
		GameCenter.loginMng.CurServerPage = 0;
		StartCoroutine(GetMyServer());
	}
	/// <summary>
	/// 获取玩家登陆过的服务器列表
	/// </summary>
	IEnumerator GetMyServer()
	{
		string sourceId = "201";
		string version = "3.6.5";
		if (GameCenter.instance.isPlatform)
		{
			sourceId = LynSdkManager.Instance.GetSourceId();
			version = LynSdkManager.Instance.GetAppVersion();
		}
		if (sourceId == "")
		{
			sourceId = "0";
		}
		long dateTime = DateTime.Now.Ticks / 10000;
		string lastID = GameCenter.loginMng.SDKUserID;
        string sign = string.Format(SystemSettingMng.MY_SERVER_HTTP_ADDRESS_PARAMETER, sourceId, dateTime, lastID, version);
        string signResult = GameHelper.SignString(sign);
        //Debug.Log("MY_SERVER signResult:" + signResult);
        string wwwText = string.Empty;
		if (GameCenter.instance.isInsideTest)
		{
            wwwText = string.Format(SystemSettingMng.MY_SERVER_HTTP_ADDRESS, sourceId, dateTime, lastID, version, signResult);
		}else
		{
            wwwText = string.Format(SystemSettingMng.MY_SERVER_HTTP_ADDRESS, sourceId, dateTime, lastID, version, signResult);
		}
		WWW www = new WWW(wwwText);
		int serlizeID = (int)NetMsgMng.CreateNewSerializeID();
		GameCenter.msgLoackingMng.UpdateSerializeList(serlizeID, true);
		yield return www;
		if (www.isDone)
		{
			Debug.Log("www.text:"+www.text);
			if (!www.text.Contains("state"))
			{
			}
			else
			{
				GameCenter.msgLoackingMng.UpdateSerializeList(serlizeID, false);
				SetMyServerInfo(www.text);
			}
		}
	}
	/// <summary>
	/// 保存自己的历史服
	/// </summary>
	void SetMyServerInfo(string wwwText)
	{
		if(!serverChoiceDic.ContainsKey(0))
		{
			serverChoiceDic[0] = new List<ServerChoiceData>();
		}else
		{
			serverChoiceDic[0].Clear();
		}
		serverPageList.Clear();
		if(!string.IsNullOrEmpty(wwwText))
		{
			LitJson.JsonData jsonData = LitJson.JsonMapper.ToObject(wwwText);
			if (jsonData != null && wwwText.Contains("state"))
			{
				if ((int)jsonData["state"] == 1)
				{
					//总页数
                    if (wwwText.Contains("data") && wwwText.Contains("page") && wwwText.Contains("total"))
					{
                        int totalSize = (int)jsonData["data"]["page"]["total"];
                        int totalPage = (totalSize % 10 == 0) ? totalSize / 10 : (totalSize / 10 + 1);
						for (int i = 0; i <= totalPage; i++) {
							ServerPageData data = new ServerPageData(i);
							serverPageList.Add(data);
						}
					}
					//推荐服or上次登陆服
					if(wwwText.Contains("data") && wwwText.Contains("lastlogin"))
					{
                        GameCenter.loginMng.LoginServerID = jsonData["data"]["lastlogin"]["svrNO"].ToString();
                        GameCenter.loginMng.LoginServerName = jsonData["data"]["lastlogin"]["svrName"].ToString();
                        lastServerData = new ServerChoiceData();
                        lastServerData.serverID = jsonData["data"]["lastlogin"]["svrNO"].ToString();
                        lastServerData.serverName = jsonData["data"]["lastlogin"]["svrName"].ToString();
                        lastServerData.serverStatus = (int)jsonData["data"]["lastlogin"]["state"];
                        if (lastLoginServer != null) lastLoginServer.SetData(lastServerData,null);
					}
					//历史服
                    if (wwwText.Contains("data") && wwwText.Contains("history") && jsonData["data"]["history"] != null)
                    {
                        for (int i = 0; i < jsonData["data"]["history"].Count; i++)
                        {
                            //string[] historyItem = ((string)jsonData["history"][i]).Split('|');
                            ServerChoiceData refData = new ServerChoiceData();
                            refData.serverID = jsonData["data"]["history"][i]["svrNO"].ToString();
                            refData.serverName = jsonData["data"]["history"][i]["svrName"].ToString();
                            refData.serverStatus = (int)jsonData["data"]["history"][i]["state"];
                            serverChoiceDic[0].Add(refData);
                        }
                    }
				}else
				{
					Debug.Log("服务器验证失败或正在维护");
				}
			}
		}
	}
	#endregion
}
public class ServerChoiceData
{
	public string serverID = string.Empty;
	public string serverName = string.Empty;
	public int serverStatus = 0;
	public string serverIP = string.Empty;
	public string serverPort = string.Empty;
}
public class ServerPageData
{
	public ServerPageData(int _curPage)
	{
		curPage = _curPage;
	}
	public int curPage = 0;
	public string CurPageName
	{
		get
		{
			switch(curPage)
			{
			case 0:
				return ConfigMng.Instance.GetUItext(294);
			default:
				return ((curPage-1)*10+1).ToString()+"-"+(curPage*10)+ConfigMng.Instance.GetUItext(295);
			}
		}
	}
}