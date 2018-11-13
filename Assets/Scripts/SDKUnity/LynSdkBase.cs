
//using System;
//using System.Collections;
using System.Collections.Generic;

public class LynSdkBase : AbstractLynSdk, ISDK {


	/*					*/
	public LynSdkBase() {

	}

	/**
	 * ISDK 's Implement method
	 */
	public virtual void InitSDK(params object[] values)    	{	;	}

	public virtual void doLogin(params object[] values)	 	{	;	}

	public virtual void doPayment(params object[] values)  	{	;	}

	public virtual void doLogout(params object[] values)   	{	;	}

	// user DoEnterGame 's menthod;
	public virtual void doEnterGame(params object[] values)	{	;	}
	
	// user DoEnterGame 's menthod;
	public virtual void doLevelUp(params object[] values)	{	;	}

	// user 
	public virtual void doUserInfoBind(params object[] values)	{	;	}

	/// <summary>
	/// //////////////////////////////////
	/// </summary>
	/// <param name="values">Values.</param>
	public virtual void showFloatView(params object[] values)	{	;	}




	public virtual string GetSourceId()	{ return "0"; }

	public virtual void UCShare(params object[] value) { ; }

	public virtual void DCBrightness(params object[] value) { ; }

	public virtual string GetAppVersion() { return "0"; }

	public virtual int GetAppVersionCode(){ return 0; }



	/// <summary>
	/// 注销方法    
	/// </summary>
	/// <param name="values"></param>
	public virtual void doLogout1(params object[] values) {; }


	/// <summary>
	/// 邀请朋友
	///     pfID            平台id         1-微信    2-QQ    3-facebook
	///     title           邀请标题
	///     description     邀请内容
	/// </summary>
	/// <param name="values"></param>
	public virtual void UC_invite(params object[] values) {; }


	/// <summary>
	/// 应用分享
	///     pfID            平台id         1-微信    2-QQ    3-facebook
	///     title           分享标题
	///     posturl         目标链接
	///     imageurl        图片地址
	///     description     分享内容
	/// </summary>
	public virtual void UC_share(params object[] values) {; }


	public virtual void UC_showCenter() {;  }

	/// <summary>
	/// SDK 版本信息
	///     增加sdk 插件版本信息   便于多版本管理
	///     
	///     继承类通过  SDKVersion 属性进行 设置、获取   默认值为 1.0   
	/// </summary>
	protected string sdkVersion = "1.0";

	public string SDKVersion
	{
		get {  return this.sdkVersion;  }
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	public virtual string GetDownloadDir() { return ""; }

	/// <summary>
	/// https 访问接口
	/// 
	/// </summary>
	/// <param name="url"></param>
	/// <param name="parms"></param>
	/// <returns></returns>
	public virtual void GetHttpsResponse(string url, string operate, Dictionary<string, string> parms, string uniObjectName, string functionName) { ; }


	public virtual bool QueryAssetsIsDownloaded() { return false; }


	public virtual string GetHotPatchUrl(string sourceId, string appVersion) { return ""; }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="type"></param>
	/// <param name="time"></param>
	/// <param name="title"></param>
	/// <param name="content"></param>
	public virtual void SetNotificationInfos(int type, string time, string title, string content) {; }


	/// <summary>
	/// 
	/// </summary>
	/// <param name="scene"></param>
	/// <param name="cause"></param>
	public virtual void ReportConnectionLose(string scene, string cause) {; }



	public virtual void OpenUrlBYSDK(int type) {; }
}

