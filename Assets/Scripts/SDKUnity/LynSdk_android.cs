using UnityEngine;
using System;
//using System.Collections;
using System.Collections.Generic;

[SDK("Android",  "6.0.0")]
public sealed class LynSdk_android : LynSdkBase {

	#if UNITY_ANDROID
		private string JAVA_CLASS = "com.dg.CHSdkManager";
		private AndroidJavaClass lynSdk;
	#endif

	public LynSdk_android():base() {
		#if UNITY_ANDROID
			lynSdk = new AndroidJavaClass(JAVA_CLASS);
		#endif
	}

    /*  5.0 增加  管理sdk 版本信息 */
    public LynSdk_android(string version) : this()
    {
//        Debug.Log("LynSdk_android  constructor 11111111!!!!!!!!!" );
        this.sdkVersion = version;
    }


    public override void InitSDK(params object[] values) {
	
//		Debug.Log("InitSDK ---- values[0]=" + values[0].ToString());
//		Debug.Log("InitSDK ---- values[1]=" + values[1].ToString());

		#if UNITY_ANDROID
			lynSdk.CallStatic("InitChSdk");
		#endif
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="values"> 
	/// 	values[0]   string uniObjectName
	/// 	values[1]   string functionName
	/// </param>
	public override void doLogin(params object[] values) 	{	

		if (values.Length != 2)  return;

		string uniObjectName   	= values[0].ToString();
		string functionName  	= values[1].ToString();

	//	Debug.Log("LynSdkManager + doLogin param1=" + uniObjectName + "  param2=" + functionName);
		#if UNITY_ANDROID
		    lynSdk.CallStatic("ChUserLogin", uniObjectName, functionName );
		#endif
	}


	public override void doPayment(params object[] values)  
	{
		if (values.Length != 6)  return;

		string orderId   	= values[0].ToString();
		string serverNo  	= values[1].ToString();
		int productId   	= Convert.ToInt32(values[2]);
		string productName 	= values[3].ToString();
		int money          	= Convert.ToInt32(values[4]);
		int pMoney         	= Convert.ToInt32(values[5]);

        /* 5.0  增加参数   越南增加参数  */
        //        int paymode         = Convert.ToInt32(values[6]);
        //        int currency        = Convert.ToInt32(values[7]);

#if UNITY_ANDROID
        lynSdk.CallStatic("ChPayAction", orderId, serverNo, productId, productName, money, pMoney);//, paymode, currency);
		#endif
	}
	
	public override void doLogout(params object[] values)   
	{	
		#if UNITY_ANDROID
			lynSdk.CallStatic("ChUserLogout");
		#endif
	}
	
	public override void doEnterGame(params object[] values)  
	{	
		if (values.Length != 10)  return;

		string serverNo = values[0].ToString();
		string serverName = values[1].ToString();
		//int roleId = Convert.ToInt32(values[2]);
        string roleId = values[2].ToString();
		string roleName = values[3].ToString();
		int level = Convert.ToInt32(values[4]);

   // v5.9  增加参数     string Profession, string GroupID, string VIP, string CE
        string Profession = values[5].ToString();
        string GroupID = values[6].ToString();
        string VIP = values[7].ToString();
        string CE = values[8].ToString();
        string platformId = values[9].ToString();


        #if UNITY_ANDROID
            lynSdk.CallStatic("ChEnterGame",serverNo,serverName,roleId,roleName,level,Profession,GroupID,VIP,CE,platformId);
        #endif

    }

    public override void doLevelUp(params object[] values)   
	{	
		if (values.Length != 1)  return;

		int level = Convert.ToInt32(values[0]);
		#if UNITY_ANDROID
			lynSdk.CallStatic("ChUserUpLevel", level);
		#endif
	}

	public override void doUserInfoBind(params object[] values)
	{
		if (values.Length != 1)  return;
		
		string tokenResult = values[0].ToString();
		#if UNITY_ANDROID
			lynSdk.CallStatic("ChUserInfoBind", tokenResult);
		#endif
	}

	public override void showFloatView(params object[] values)
	{
		if (values.Length != 1)  return;
		
		int show = Convert.ToInt32(values[0]);
		#if UNITY_ANDROID
			lynSdk.CallStatic("ChShowFloatView", show);
		#endif
	
	}

	public override string GetSourceId()
	{
		string sourceid="0";
		#if UNITY_ANDROID
		/* ? why		*/
		if (Application.platform == RuntimePlatform.Android)
		{
			sourceid = lynSdk.CallStatic<string>("GetSourceId");
		}
		//	sourceid = lynSdk.CallStatic<string>("GetSourceId");
		#endif

		return sourceid;
	}
	

	public override void UCShare(params object[] values) 
	{
		if (values.Length != 4)  return;
	
		int plateform = Convert.ToInt32(values[0]);
		int type = Convert.ToInt32(values[1]);

		string ob = values[2].ToString();
		string describe = values[3].ToString();

		#if UNITY_ANDROID
		    lynSdk.CallStatic("UCShare", plateform, type, ob, describe);
		#endif
	}
	

	public override void DCBrightness(params object[] values)
	{
		if (values.Length != 1)  return;
		
		int state = Convert.ToInt32(values[0]);

		#if UNITY_ANDROID
		lynSdk.CallStatic("DCBrightness", state);
		#endif
	}


	public override string GetAppVersion()
	{
		string ver="0";
		#if UNITY_ANDROID
		/* ? why		*/
		if (Application.platform == RuntimePlatform.Android)
		{
			ver = lynSdk.CallStatic<string>("GetAppVersion");
		}
		#endif
		
		return ver;
	}

	public override int GetAppVersionCode()
	{ 		
		int vcode=0;

        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
		{
            vcode = lynSdk.CallStatic<int>("GetAppVersionCode");
        }
        #endif
     
        return vcode;
	}

    /*
     * 5.0  增加注销功能
     *      
     *      操作类型
     *          调用时 务必先切换场景到平台登陆前， 然后调用该接口   client 要能响应 OnLoginResult事件 。。。 
     * 
     */
    public override void doLogout1(params object[] values)
    {
        #if UNITY_ANDROID
            lynSdk.CallStatic("ChUserLogout1");
        #endif
    }


    /* 
     * 5.0  增加应用分享功能
     * 
     *      pfID            平台id         1-微信    2-QQ    3-facebook
     *      title           分享标题
     *      posturl         目标链接
     *      imageurl        图片地址
     *      description     分享内容
     * 
     */
    public override void UC_share(params object[] values)
    {
        if (values.Length != 5) return;

        int plateform   = Convert.ToInt32(values[0]);
        string title    = values[1].ToString();
        string posturl  = values[2].ToString();
        string imageurl = values[3].ToString();
        string description = values[4].ToString();

        #if UNITY_ANDROID
            lynSdk.CallStatic("UC_share", plateform, title, posturl, imageurl, description);
        #endif
    }

    /* 
     * 5.0  增加邀请好友功能
     * 
     *      pfID            平台id         1-微信    2-QQ    3-facebook
     *      title           邀请标题
     *      description     邀请内容
     */
    public override void UC_invite(params object[] values)
    {
        if (values.Length != 3) return;

        int plateform       = Convert.ToInt32(values[0]);
        string title        = values[1].ToString();
        string description  = values[2].ToString();

        #if UNITY_ANDROID
            lynSdk.CallStatic("UC_invite", plateform, title, description );
        #endif
    }

    /* 
     * 5.0  增加用户中心 
     *      操作方法  打开sdk 用户中心操作 
     */
    public override void UC_showCenter()
    {
        #if UNITY_ANDROID
            lynSdk.CallStatic("UC_showCenter");
        #endif
    }

    public override string GetDownloadDir()
    {
        string dir = "";

        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            dir = lynSdk.CallStatic<string>("getDownloadDir");
        }
        #endif

        return dir;
    }



    /* <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <param name="parms"></param>
    /// <returns></returns>
    */
    public override void GetHttpsResponse(string url, string operate, Dictionary<string, string> parms, string uniObjectName, string functionName)
    {
        AndroidJavaObject map = dicToMap(parms);

        if (map == null) return;

    #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            lynSdk.CallStatic("getHttpsResponse", url , operate,  map,  uniObjectName,  functionName);
        }
    #endif
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override bool QueryAssetsIsDownloaded()
    {
        bool ret = false;

     #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            ret = lynSdk.CallStatic<bool>("QueryAssetsIsDownloaded");
        }
     #endif
        return ret;
    }


    public override string GetHotPatchUrl(string sourceId, string appVersion)
    {
        string url = "";

        #if UNITY_ANDROID
                if (Application.platform == RuntimePlatform.Android)
                {
                    url = lynSdk.CallStatic<string>("getHotPatchUrl", sourceId, appVersion);
                }
        #endif
        return url; 
    }



    private AndroidJavaObject dicToMap(Dictionary<string, string> dictionary)
    {
        if (dictionary == null)
            return null;

        AndroidJavaObject map = new AndroidJavaObject("java.util.HashMap");
        foreach (KeyValuePair<string, string> pair in dictionary)
        {
            map.Call<string>("put", pair.Key, pair.Value);
        }
        return map;
    }


    public override void SetNotificationInfos(int type, string time, string title, string content)
    {

#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            lynSdk.CallStatic("SetNotificationInfos", type, time, title, content);
        }
#endif
    }


    public override void ReportConnectionLose(string scene, string cause)
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            lynSdk.CallStatic("ReportConnectionLose", scene, cause);
        }
#endif  
    }



    public override void OpenUrlBYSDK(int type)
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            lynSdk.CallStatic("UC_OpenUrlBYSDK", type);
        }
#endif
    }

}
