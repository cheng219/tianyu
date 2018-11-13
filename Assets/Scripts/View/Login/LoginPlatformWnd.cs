//==============================
//作者：邓成
//日期：2016/7/12
//用途：平台登录界面类
//==============================

using UnityEngine;
using System.Collections;

public class LoginPlatformWnd : SubWnd {
	public GameObject btnLoginSDK;

	void Awake()
	{
		if(btnLoginSDK != null)UIEventListener.Get(btnLoginSDK).onClick = LoginSDK;
	}
	protected override void OnOpen ()
	{
		base.OnOpen ();
	}
	protected override void OnClose ()
	{
		base.OnClose ();
	}
	void LoginSDK(GameObject go)
	{
		if(GameCenter.instance.isPlatform)
			LynSdkManager.Instance.UsrLogin(GameCenter.instance.gameObject.name,"OnLoginResult");
	}
}
