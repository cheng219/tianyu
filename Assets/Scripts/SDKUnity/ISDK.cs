
//using System.Collections;

public interface ISDK  {

	void InitSDK(params object[] values);

	// user Login 's method;
	void doLogin(params object[] values);
	
	// user Pay 's menthod;
	void doPayment(params object[] values);

	// user Logout 's menthod;
	void doLogout(params object[] values);



    // user DoEnterGame 's menthod;
    void doEnterGame(params object[] values);

	// user DoEnterGame 's menthod;
	void doLevelUp(params object[] values);

	//
	void doUserInfoBind(params object[] values);


	///////////////////////////////////////////////
	void showFloatView(params object[] value);

	// 
	string GetSourceId();


	//share
	void UCShare(params object[] value);

	//DC_Brightness
	void DCBrightness(params object[] value);

	// version 
	string GetAppVersion();

	// version 
	int GetAppVersionCode();



    // 5.0 - 新增注销功能   ;
    void doLogout1(params object[] values);
    
    // 5.0 - 新增邀请功能   ；
    void UC_invite(params object[] values);

    // 5.0 - 新增分享功能    ；
    void UC_share(params object[] values);

    // 5.0 - 新增打开用户中心    ；
    void UC_showCenter();

    // 5.0 - 获取资源下载目录     ；
    string GetDownloadDir();

    // 5.0 - 解决unity https访问的问题     ；
    void GetHttpsResponse(string url,  string operate, System.Collections.Generic.Dictionary<string, string> parms, string uniObjectName, string functionName);

    // 5.0 - 解决unity  查询download 状态     ；
    bool QueryAssetsIsDownloaded();

    // 5.6 - 获取热更 url  
    string GetHotPatchUrl(string sourceId, string appVersion);

    // 5.6 - 设置 Notification 
    void SetNotificationInfos(int type, string time, string title, string content);



    // 5.8.1 - 设置 Notification 
    void ReportConnectionLose(string scene, string cause);


    // 6.0.0 - 调用sdk Activity 打开WebView 窗口
    void OpenUrlBYSDK(int type);
}
