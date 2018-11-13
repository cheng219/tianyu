using UnityEngine;
//using System.Collections;
using System.Collections.Generic;
//using System.Runtime.InteropServices;
//using System;


[SDK("Ios", "6.0.0")]
public sealed class LynSdk_ios : LynSdkBase
{

#if UNITY_IPHONE 
	[DllImport("__Internal")]
	private static extern void lynJBInit();
	[DllImport("__Internal")]
	private static extern void lynJBLogin();
	[DllImport("__Internal")]
	private static extern void lynJBLoginParam(string GameObjectName,string FunctionName);
	[DllImport("__Internal")]
	private static extern void lynJBBindUserInfo(string GameServerback);
	[DllImport("__Internal")]
	private static extern void lynJBLevelUpWithLevel(string level);
	[DllImport("__Internal")]
	private static extern void lynJBPayActionWithOutPayNo(string outPayNo,string serverNo,string productID,string productName,string payMoney,string gMoney,string paytype,string payunity);
	[DllImport("__Internal")]
	private static extern void lynJBLogout();
	[DllImport("__Internal")]
	private static extern void lynJBSetUnityReceiver(string objname);
	[DllImport("__Internal")]
	private static extern void lynJBEnterGameWithServerNo(string serverNo,string serverName,string roleId,string roleName,string level,string Profession,string GroupID,string VIP,string CE,string platformId);
	[DllImport("__Internal")]
	private static extern float lynJBGetFreeMemory();
	[DllImport("__Internal")]
	private static extern string lynJBGetsourcId();
    [DllImport("__Internal")]
    private static extern string lynJBGetappversion();
  	[DllImport("__Internal")]
	private static extern string lynJBGetAppVersionCode();
	[DllImport("__Internal")]
	private static extern void lynJBLogout1();
	[DllImport("__Internal")]
	private static extern void lynJBShareIos(string pfID,string title,string posturl,string imageurl,string description);
	[DllImport("__Internal")]
	private static extern void lynJBYaoQing(string pfID,string title,string description);
    [DllImport("__Internal")]
	private static extern string lynShowCenter();
	[DllImport("__Internal")]
	private static extern string lynhttpresponse(string url,string operate,string dicstr,string uniObjectName,string functionName);
	[DllImport ("__Internal")]   
	private static extern string lynsdkGetImage(string nameTag);//获取图片路径   nameTag        修改的界面标记         0 登录    1 加载
    [DllImport("__Internal")]
	private static extern string lynGethoturl(string sourceId,string appVersion);
	[DllImport("__Internal")]
	private static extern void lynnoticesdk(string type, string time, string title, string content);
	[DllImport("__Internal")]
	private static extern void lynlosereport(string scene, string cause);
    [DllImport("__Internal")]
	private static extern void lynwebaction(string type);
#endif

    public LynSdk_ios()
        : base()
    {

    }

    /*  5.0 增加  管理sdk 版本信息 */
    public LynSdk_ios(string version)
        : this()
    {
        this.sdkVersion = version;
    }

    public override void InitSDK(params object[] values)
    {

#if UNITY_IPHONE
		//#if IOS_JB
		lynJBInit();
#endif
        //		#if IOS_AS
        //		//Debug.log("AS");
        //		#endif
        //		#endif
    }

    public override void doLogin(params object[] values)
    {

        if (values.Length != 2) return;

        string uniObjectName = values[0].ToString();
        string functionName = values[1].ToString();

        Debug.Log("LynSdkManager + doLogin param1=" + uniObjectName + "  param2=" + functionName);
#if UNITY_IPHONE
		//#if IOS_JB
		lynJBLoginParam(uniObjectName,functionName);
		//#endif
#endif
    }

    public override void doPayment(params object[] values)
    {
        if (values.Length == 8 || values.Length == 6)
        {
            string outPayNo = values[0].ToString();
            string serverNo = values[1].ToString();
            string productID = values[2].ToString();
            string productName = values[3].ToString();
            string payMoney = values[4].ToString();
            string gMoney = values[5].ToString();
            string paytype = "123";
            string payunity = "123";
            if (values.Length == 8)
            {
                paytype = values[6].ToString();
                payunity = values[7].ToString();
            }
#if UNITY_IPHONE
       // #if IOS_JB
		    lynJBPayActionWithOutPayNo(outPayNo,serverNo,productID,productName,payMoney,gMoney,paytype,payunity);
       // #endif
#endif
        }
    }

    public override void doLogout(params object[] values)
    {
#if UNITY_IPHONE
		//#if IOS_JB
		lynJBLogout();
		//#endif	
#endif
    }
	/*  v5.9 增加采集参数    
	Profession      //职业
	GroupID         //公会
	Vip             //vip等级
	CE              //战力

	platformId      // 
	*/
    public override void doEnterGame(params object[] values)
    {
		 
		string serverNo = values [0].ToString ();
	    string serverName = values [1].ToString ();
		string roleId = values [2].ToString ();
		string roleName = values [3].ToString ();
		string level = values [4].ToString ();

		string Profession = "0";
		string GroupID = "0";
		string Vip = "0";
		string CE = "0";
		string platformId = "0";
		if  (values.Length > 5)
		{
			Profession = values[5].ToString();
			GroupID = values[6].ToString();
			Vip = values[7].ToString();
			CE = values[8].ToString();
			platformId = values[9].ToString();
		}

#if UNITY_IPHONE
		//#if IOS_JB
	lynJBEnterGameWithServerNo(serverNo,serverName,roleId,roleName,level,Profession,GroupID,Vip,CE,platformId);
		//#endif
#endif
    }


    public override void doLevelUp(params object[] values)
    {
        if (values.Length != 1) return;


        string level = values[0].ToString();
#if UNITY_IPHONE
		//#if IOS_JB
		lynJBLevelUpWithLevel(level);
		//#endif	
#endif
    }
    public override void doUserInfoBind(params object[] values)
    {
        if (values.Length != 1) return;

        string tokenResult = values[0].ToString();
#if UNITY_IPHONE
		//#if IOS_JB
		lynJBBindUserInfo(tokenResult);
		//#endif	
#endif
    }
    public override string GetSourceId()
    {
        string sourceid = "0";
#if UNITY_IPHONE
		//#if IOS_JB
		sourceid = lynJBGetsourcId();
		//#endif
#endif
        return sourceid;
    }


    // add 
    public override void UCShare(params object[] values) { }

    public override void DCBrightness(params object[] values) { }

    public override string GetAppVersion()
    {
        string version = "0";
#if UNITY_IPHONE
        //#if IOS_JB
		version = lynJBGetappversion();
       // #endif
#endif
        return version;


    }

    public override int GetAppVersionCode()
    {
        string versionCode = "0";
#if UNITY_IPHONE
       // #if IOS_JB
		versionCode = lynJBGetAppVersionCode();
       // #endif
#endif
        int myversionCode = int.Parse(versionCode);
        return myversionCode;

        // return 0;	
    }




    public override void doLogout1(params object[] values)
    {
#if UNITY_IPHONE
       // #if IOS_JB
		    lynJBLogout1();
       // #endif
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
        string pfID = values[0].ToString();
        string title = values[1].ToString();
        string posturl = values[2].ToString();
        string imageurl = values[3].ToString();
        string description = values[4].ToString();
#if UNITY_IPHONE
		//#if IOS_JB
		lynJBShareIos(pfID,title,posturl,imageurl,description);
		//#endif
#endif
        // 该方法 由熊浩补全 。
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
        string pfID = values[0].ToString();
        string title = values[1].ToString();
        string description = values[2].ToString();
#if UNITY_IPHONE
		//#if IOS_JB
		lynJBYaoQing(pfID,title,description);
		//#endif
#endif

        // 该方法 由熊浩补全 。
    }

    /* 
     * 5.0  增加用户中心 
     *      操作方法  打开sdk 用户中心操作 
     */
    public override void UC_showCenter()
    {
#if UNITY_IPHONE
		//#if IOS_JB
		    lynShowCenter();
       // #endif
#endif
        // 该方法 由熊浩补全 。
    }


    /* 
    * 5.0  https 访问接口
    *  
    */
    public override void GetHttpsResponse(string url, string operate, Dictionary<string, string> list, string uniObjectName, string functionName)
    {
        string dicstr = "";
        foreach (string key in list.Keys)
        {
            dicstr = dicstr + key + "=" + list[key] + ",";
        }


        // 
#if UNITY_IPHONE
		lynhttpresponse(url,operate,dicstr,uniObjectName,functionName);
#endif

        // 该方法 由熊浩补全 。

        return;
    }



    /* 
    * 获取热更地址
    *  
    */
    public override string GetHotPatchUrl(string sourceId, string appVersion)
    {

        string gethoturl = "";
        //
#if UNITY_IPHONE
		gethoturl = lynGethoturl(sourceId,appVersion);
#endif
        //
        return gethoturl;
    }




    /*
     * 推送通知
     * 
     */
    public override void SetNotificationInfos(int type, string time, string title, string content)
    {
        #if UNITY_IPHONE
		        string typestr = type.ToString();
		        lynnoticesdk(typestr,time,title,content);

        #endif
    }

    /*
     * 异常上报
     * 
     */
    public override void ReportConnectionLose(string scene, string cause)
    {
	    #if UNITY_IPHONE

	    lynlosereport(scene,cause);

	    #endif
    }




    /// <summary>
    ///  通过SDK层 调起Activity 窗口, 通过WebView打开 URL链接 
    ///  参数type为类型：1、美女主播，   2、客服
    /// </summary>
    /// <param name="type">类型</param>
    public override void OpenUrlBYSDK(int type)
    {
#if UNITY_IPHONE
		string typestr = type.ToString();
		lynwebaction(typestr);
#endif
    }


}
