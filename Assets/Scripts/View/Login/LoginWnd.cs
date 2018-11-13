//==============================================
//作者：吴江
//日期：2015/5/21
//用途：登陆窗口
//=================================================




using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 登陆窗口 by吴江
/// </summary>
public class LoginWnd : SubWnd
{
    #region UI控件对象
    /// <summary>
    /// 登陆按钮 by吴江
    /// </summary>
    public GameObject loginBtn;
	/// <summary>
	/// 选服测试按钮  by邓成
	/// </summary>
	public GameObject selectServerBtn;

	public GameObject noticeBtn;
    /// <summary>
    /// 帐号输入框 by吴江
    /// </summary>
    public UIInput passNameTextField;
    /// <summary>
    /// 密码输入框 by吴江
    /// </summary>
    public UIInput passWordTextField;
    /// <summary>
    /// IP输入框 by吴江
    /// </summary>
    public UIInput serverIPTextField;
    /// <summary>
    /// 端口输入框 by吴江
    /// </summary>
    public UIInput serverPortTextField;
    #endregion

    #region 数据
    /// <summary>
    /// 是否开始连接
    /// </summary>
    protected bool isStartConnect = false;
    /// <summary>
    /// 开始连接的时间
    /// </summary>
    protected float startConnectTime = 0;
    #endregion


    void Awake()
    {
        if (loginBtn != null)
        {
            UIEventListener.Get(loginBtn.gameObject).onClick = OnClickLoginBtn;
        }
		if (selectServerBtn != null)
		{
			UIEventListener.Get(selectServerBtn.gameObject).onClick = OnClickServerBtn;
		}
		if(noticeBtn != null)
		{
			UIEventListener.Get(noticeBtn).onClick = OnClickNoticeBtn;
		}
    }


    protected override void OnOpen()
    {
        base.OnOpen();
        GameCenter.instance.ShowNotice();
        Refresh();
    }
    #region 刷新
    protected void Refresh()
    {
        passNameTextField.value = GameCenter.loginMng.Login_Name;
        passWordTextField.value = GameCenter.loginMng.Login_Word;

		if (!GameCenter.instance.isPlatform)
		{
			if(PlayerPrefs.HasKey("LastPort") && PlayerPrefs.HasKey("LastIP"))//已经登陆过才显示保存IP,否则使用预制默认
			{
				serverIPTextField.value = GameCenter.loginMng.Quaue_IP;
				serverPortTextField.value = GameCenter.loginMng.Quaue_port.ToString();
			}
		}
    }
    #endregion


    #region 控件事件
	protected void OnClickServerBtn(GameObject _btn)
	{
		GameCenter.uIMng.SwitchToSubUI(SubGUIType.SELECTSERVER);
	}

	protected void OnClickNoticeBtn(GameObject _btn)
	{
		GameCenter.instance.ShowNotice();
	}

    /// <summary>
    /// 点击登陆按钮的操作
    /// </summary>
    /// <param name="_btn"></param>
    protected void OnClickLoginBtn(GameObject _btn)
    {
        GameCenter.loginMng.Quaue_IP = serverIPTextField.value;
        GameCenter.loginMng.Quaue_port = Convert.ToInt32(serverPortTextField.value);
        GameCenter.loginMng.Login_Name = passNameTextField.value;
        GameCenter.loginMng.Login_Word = passWordTextField.value;
        if (GameCenter.instance.IsConnected)
        {
            GameCenter.loginMng.C2S_Login();
        }
        else
        {
            isStartConnect = true;
            startConnectTime = Time.time;
            GameCenter.loginMng.C2S_ConectQueueServer(GameCenter.loginMng.Quaue_IP, GameCenter.loginMng.Quaue_port);
        }
    }
    #endregion
}
