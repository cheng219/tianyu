//using UnityEngine;
//using System;
//using System.Collections;


using UnityEngine;


public sealed class LynSdkManager : AbstractSdkFactory
{

	/*				*/
	private static LynSdkManager _instance;

	/*				*/
	private ISDK Isdk;


	//-------------------------
	private LynSdkManager() { Isdk = ConcreateSdk(); }


	public static LynSdkManager Instance
	{
		get
		{
			if (_instance == null)
				_instance = new LynSdkManager();
			return _instance;
		}
	}

	//	LynSdkManager.instance.ChUserInfoBind(tokenResult);
	public void LynUserInfoBind(string tokenResult)
	{
		if (Isdk == null) return;

		Isdk.doUserInfoBind(tokenResult);
	}


	public void InitSdk()
	{
		if (Isdk == null) return;

		Isdk.InitSDK();
	}


	/* user's do action!!!	*/
	public void UsrLogin(string uniObjectName, string functionName)
	{
		if (Isdk == null) return;

		Isdk.doLogin(uniObjectName, functionName);
	}

	public void UsrPayment(string orderId, string serverNo, int productId, string productName, int money, int pMoney)// int paymode, int currency) 
	{
		if (Isdk == null) return;

		Isdk.doPayment(orderId, serverNo, productId, productName, money, pMoney);//,  paymode,  currency);
	}

	/*						*/
	public void UsrLogout() { if (Isdk == null) return; Isdk.doLogout(); }


	public void UsrLevelUp(int level)
	{
		if (Isdk == null) return;

		Isdk.doLevelUp(level);
	}


	/**
	 *  v5.9 增加采集参数    
			Profession      //职业
			GroupID         //公会
			Vip             //vip等级
			CE              //战力

			platformId      // 
	 */
	public void UsrEnterGame(string serverNo, string serverName, ulong roleId, string roleName, int level, string Profession, string GroupID, string VIP, string CE, string platformId)
	{
		Isdk.doEnterGame(serverNo, serverName, roleId, roleName, level, Profession, GroupID, VIP, CE, platformId);
	}


	public void ShowFloatView(int show)
	{
		Isdk.showFloatView(show);
	}

	
	// 
	private static string sdkStubVersion = "0.0.0";
	
	// 
	public static string SDKStubVersion
	{
		get { return sdkStubVersion; }
	}

	public override LynSdkBase ConcreateSdk()
	{
		string platform = "Unkown";

		#if UNITY_ANDROID
			if (Application.platform == RuntimePlatform.Android)
				platform = "Android";
		#elif UNITY_IPHONE && IOS_JB
			if (Application.platform == RuntimePlatform.IPhonePlayer)
				platform = "Ios";
		#elif UNITY_IPHONE && IOS_AS
			if (Application.platform == RuntimePlatform.IPhonePlayer)
				platform = "Ios";
		#endif

		DGConstructorInfo DG = SDKFormater.getSDKConstructor(platform);
		if (DG.ci == null) return null;

		sdkStubVersion = DG.version;

		return DG.ci.Invoke(new object[] { DG.version }) as LynSdkBase;
	}


	public ISDK SDKInterface
	{
		get { return this.Isdk; }
	}


	public string GetSourceId()
	{
		return Isdk.GetSourceId();
	}


	public void UCShare(int plateform, int shareWay, string res, string describe)
	{
		Isdk.UCShare(plateform, shareWay, res, describe);
	}

	public void DCBrightness(int state)
	{
		Isdk.DCBrightness(state);
	}

	public string GetAppVersion()
	{
		return Isdk.GetAppVersion();
	}


	public int GetAppVersionCode()
	{
		return Isdk.GetAppVersionCode();
	}


	/*						*/
	public void UsrLogout1()
	{
		if (Isdk == null) return;

		Isdk.doLogout1();
	}

	/*                      */
	public void UsrShare(int plateForm, string tile, string posturl, string imageurl, string description)
	{
		if (Isdk == null) return;

		Isdk.UC_share(plateForm, tile, posturl, imageurl, description);
	}

	/*                      */
	public void Usrinvite(int plateForm, string tile, string description)
	{
		if (Isdk == null) return;

		Isdk.UC_invite(plateForm, tile, description);
	}

	/*                      */
	public void UsrshowCenter()
	{
		if (Isdk == null) return;

		Isdk.UC_showCenter();
	}

	/*                      */
	public string GetDownloadDir()
	{
		if (Isdk == null) return "";

		return Isdk.GetDownloadDir();
	}



	public void GetHttpsResponse(string url, string operate, System.Collections.Generic.Dictionary<string, string> parms, string uniObjectName, string functionName)
	{
		if (Isdk == null) return;

		Isdk.GetHttpsResponse(url, operate, parms, uniObjectName, functionName);
	}

	/**
	 * 
	 */
	public bool QueryAssetsIsDownloaded()
	{
		if (Isdk == null) return false;

		return Isdk.QueryAssetsIsDownloaded();
	}

	public string GetHotPatchUrl()
	{
		if (Isdk == null) return "";

		return Isdk.GetHotPatchUrl(GetSourceId(), GetAppVersion());
	}

	public void SetNotificationInfos(int type, string time, string title, string content)
	{
		if (Isdk == null) return;

		Isdk.SetNotificationInfos(type, time, title, content);
	}
	
	public void ReportConnectionLose(string scene, string cause)
	{
		if (Isdk == null) return;

		Isdk.ReportConnectionLose(scene, cause);
	}


	/*  
	 *  v5.9.2 增加 
			
	 
	 */
	public void MsgToClient(string msg)
	{
		GameObject.Find("GameCenter").SendMessageUpwards("OnMd5UrlInited", msg);
	}



	/*
	 *  v6.0.0 增加
			
			通过SDK层 调起Activity 窗口   通过 WebView打开 URL链接 

	 */

	/// <summary>
	///  通过SDK层 调起Activity 窗口, 通过WebView打开 URL链接 
	///  参数type为类型：1、美女主播，   2、客服
	/// </summary>
	/// <param name="type">类型</param>
	public void OpenUrlBYSDK(int type)
	{
		if (Isdk == null) return;

		Isdk.OpenUrlBYSDK (type);
	}
}
