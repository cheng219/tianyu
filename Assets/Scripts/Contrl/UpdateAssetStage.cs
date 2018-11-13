//======================================
//作者:吴江
//日期:2015/1/23
//用途：资源增量检查更新，apk检查更新
//========================================


using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Text;

public class UpdateAssetStage : GameStage {

    public enum EventType
    {
        AWAKE = fsm.Event.USER_FIELD + 1,
        GET_UI_AND_CHECK_ASSET,
        WRONG,
		DECOMPRESSION,
        DECOMPRESSION1,
		DECOMPRESSION2,
		DECOMPRESSION3,
        DECOMPRESSION4,
        DECOMPRESSION5,
        DECOMPRESSION6,
        DECOMPRESSION7,
        DECOMPRESSION8,
        WAITCOMPAREMD5,
        COMPAREMD5,
        LOADASSET,
        STOP,
    }

	protected EventType curFSMState = EventType.AWAKE;

    public enum DecompressionWay
    {
        ZIP,
        ASSET_BUNDLE,
    }



    #region 数据
    protected float waitTime = 40.0f;

    protected float startWaitTime = float.MaxValue;

    public bool NeedTick
    {
        get
        {
            return Time.time - startWaitTime >= waitTime;
        }
        set
        {
            startWaitTime = Time.time;
        }
    }

	public int NeedStorageSize = 200;

	public WWW w = null;

    /// <summary>
    /// 是否已解压力资源，0表示无；1，表示有
    /// </summary>
    public bool HasDecompressionAsset
    {
        get
        {

       		 if (PlayerPrefs.HasKey("HasDecompressionAsset")){			
		            return PlayerPrefs.GetInt("HasDecompressionAsset") == 1;
			}
			else
				return false;

        }
        set
        {
            PlayerPrefs.SetInt("HasDecompressionAsset", value ? 1 : 0);
        }
    }


    protected long totalLoadSize = 0;
    public long TotalLoadSize
    {
        protected set
        {
            if (totalLoadSize != value)
            {
                totalLoadSize = value;
                if (OnLoadCountUpdate != null)
                {
                    OnLoadCountUpdate((CurLoadedSize / 1048576.0f).ToString("0:0.00") + "/" + (TotalLoadSize / 1048576.0f).ToString("0:0.00"));
                }
            }
        }
        get
        {
            return totalLoadSize;
        }
    }

    protected long curLoadedSize = 0;
    public long CurLoadedSize
    {
        protected set
        {
            if (curLoadedSize != value)
            {
                curLoadedSize = value;
                if (OnLoadCountUpdate != null)
                {
                    OnLoadCountUpdate((CurLoadedSize / 1048576.0f).ToString("0.00") + "M/" + (TotalLoadSize / 1048576.0f).ToString("0.00") + "M" + " 文件个数 " + curHaveLoad + " /" + needLoadList.Count + " 文件名 ：" + curLoadName);
                }
            }
        }
        get
        {
            return curLoadedSize;
        }
    }

    public static  System.Action<string> OnLoadCountUpdate;

    public static System.Action<EventType> OnEventTypeUpdateEvent;

    protected float curLoadRate = 0.0f;
    public float CurLoadRate
    {
        protected set
        {
            if (curLoadRate != value)
            {
                curLoadRate = value;
            }
        }
        get
        {
            return curLoadRate;
        }
    }



    protected float curLoadFileRate = 0.0f;
    public float CurLoadFileRate
    {
        protected set
        {
            if (curLoadFileRate != value)
            {
                curLoadFileRate = value;
            }
        }
        get
        {
            return curLoadFileRate;
        }
    }

    protected string curFileName = string.Empty;
    public string CurFileName
    {
        protected set
        {
            if (curFileName != value)
            {
                curFileName = value;
            }
        }
        get
        {
            return curFileName;
        }
    }



	/// <summary>
	/// 剩余空间提示,需求大小,剩余大小
	/// </summary>
	public Action<int,int> OnShowStorageSizeTip;

    protected DecompressionWay decompressionWay = DecompressionWay.ZIP;
    #endregion

    /// <summary>
    /// 初始化状态机
    /// </summary>
    protected override void InitStateMachine ()
	{
		base.InitStateMachine ();

		fsm.State awake = new fsm.State("awake", stateMachine);
        awake.onEnter += EnterAwakeState;
        awake.onAction += UpdateAwakeState;

        fsm.State getUIAndCheckAsset = new fsm.State("getUIAndCheckAsset", stateMachine);
        getUIAndCheckAsset.onEnter += EnterGetUIAndCheckAssetState;
        getUIAndCheckAsset.onAction += UpdateGetUIAndCheckAssetState;

        fsm.State wrong = new fsm.State("wrong", stateMachine);
        wrong.onEnter += EnterWrong;

        fsm.State decompression1 = new fsm.State("decompression1", stateMachine);
        decompression1.onEnter += EnterDecompression1State;
        decompression1.onAction += UpdateDecompressionState;

		fsm.State decompression2 = new fsm.State("decompression2", stateMachine);
		decompression2.onEnter += EnterDecompression2State;
		decompression2.onAction += UpdateDecompressionState;

		fsm.State decompression3 = new fsm.State("decompression3", stateMachine);
		decompression3.onEnter += EnterDecompression3State;
		decompression3.onAction += UpdateDecompressionState;

        fsm.State decompression4 = new fsm.State("decompression4", stateMachine);
        decompression4.onEnter += EnterDecompression4State;
        decompression4.onAction += UpdateDecompressionState;

        fsm.State decompression5 = new fsm.State("decompression5", stateMachine);
        decompression5.onEnter += EnterDecompression5State;
        decompression5.onAction += UpdateDecompressionState;


        fsm.State decompression6 = new fsm.State("decompression6", stateMachine);
        decompression6.onEnter += EnterDecompression6State;
        decompression6.onAction += UpdateDecompressionState;


        fsm.State decompression7 = new fsm.State("decompression7", stateMachine);
        decompression7.onEnter += EnterDecompression7State;
        decompression7.onAction += UpdateDecompressionState;


        fsm.State decompression8 = new fsm.State("decompression8", stateMachine);
        decompression8.onEnter += EnterDecompression8State;
        decompression8.onAction += UpdateDecompressionState;

        fsm.State waitCompareMD5 = new fsm.State("waitCompareMD5",stateMachine);
        waitCompareMD5.onEnter += EnterWaitCompareMD5State;
        waitCompareMD5.onAction += UpdateWaitCompareMD5State;

        fsm.State csompareMD5 = new fsm.State("csompareMD5", stateMachine);
        csompareMD5.onEnter += EnterCompareMD5State;
        csompareMD5.onAction += UpdateCompareMD5State;

        fsm.State loadAsset = new fsm.State("loadAsset", stateMachine);
        loadAsset.onEnter += EnterLoadAssetState;
        loadAsset.onAction += UpdateLoadAssetState;

        fsm.State stop = new fsm.State("stop", stateMachine);
        stop.onEnter += EnterStopState;


        awake.Add<fsm.EventTransition>(getUIAndCheckAsset, (int)EventType.GET_UI_AND_CHECK_ASSET);

        getUIAndCheckAsset.Add<fsm.EventTransition>(decompression1, (int)EventType.DECOMPRESSION1);
        getUIAndCheckAsset.Add<fsm.EventTransition>(waitCompareMD5, (int)EventType.WAITCOMPAREMD5);
        getUIAndCheckAsset.Add<fsm.EventTransition>(stop, (int)EventType.STOP);

        wrong.Add<fsm.EventTransition>(decompression1, (int)EventType.DECOMPRESSION1);
        wrong.Add<fsm.EventTransition>(decompression2, (int)EventType.DECOMPRESSION2);
        wrong.Add<fsm.EventTransition>(decompression3, (int)EventType.DECOMPRESSION3);
        wrong.Add<fsm.EventTransition>(decompression4, (int)EventType.DECOMPRESSION4);
        wrong.Add<fsm.EventTransition>(decompression5, (int)EventType.DECOMPRESSION5);
        wrong.Add<fsm.EventTransition>(decompression6, (int)EventType.DECOMPRESSION6);
        wrong.Add<fsm.EventTransition>(decompression7, (int)EventType.DECOMPRESSION7);
        wrong.Add<fsm.EventTransition>(decompression8, (int)EventType.DECOMPRESSION8);

		decompression1.Add<fsm.EventTransition>(decompression2, (int)EventType.DECOMPRESSION2);
        decompression1.Add<fsm.EventTransition>(wrong, (int)EventType.WRONG);
		decompression1.Add<fsm.EventTransition>(stop, (int)EventType.STOP);//tiaoshi

		decompression2.Add<fsm.EventTransition>(decompression3, (int)EventType.DECOMPRESSION3);
        decompression2.Add<fsm.EventTransition>(wrong, (int)EventType.WRONG);
		decompression2.Add<fsm.EventTransition>(stop, (int)EventType.STOP);//tiaoshi

        decompression3.Add<fsm.EventTransition>(decompression4, (int)EventType.DECOMPRESSION4);
        decompression3.Add<fsm.EventTransition>(wrong, (int)EventType.WRONG);
		decompression3.Add<fsm.EventTransition>(stop, (int)EventType.STOP);//tiaoshi

        decompression4.Add<fsm.EventTransition>(decompression5, (int)EventType.DECOMPRESSION5);
        decompression4.Add<fsm.EventTransition>(wrong, (int)EventType.WRONG);
        decompression4.Add<fsm.EventTransition>(stop, (int)EventType.STOP);//tiaoshi

        decompression5.Add<fsm.EventTransition>(decompression6, (int)EventType.DECOMPRESSION6);
        decompression5.Add<fsm.EventTransition>(wrong, (int)EventType.WRONG);
        decompression5.Add<fsm.EventTransition>(stop, (int)EventType.STOP);//tiaoshi

        decompression6.Add<fsm.EventTransition>(decompression7, (int)EventType.DECOMPRESSION7);
        decompression6.Add<fsm.EventTransition>(wrong, (int)EventType.WRONG);
        decompression6.Add<fsm.EventTransition>(stop, (int)EventType.STOP);//tiaoshi

        decompression7.Add<fsm.EventTransition>(decompression8, (int)EventType.DECOMPRESSION8);
        decompression7.Add<fsm.EventTransition>(wrong, (int)EventType.WRONG);
        decompression7.Add<fsm.EventTransition>(stop, (int)EventType.STOP);//tiaoshi

        decompression8.Add<fsm.EventTransition>(wrong, (int)EventType.WRONG);
        decompression8.Add<fsm.EventTransition>(stop, (int)EventType.STOP);//tiaoshi


        decompression8.Add<fsm.EventTransition>(waitCompareMD5, (int)EventType.WAITCOMPAREMD5);

        waitCompareMD5.Add<fsm.EventTransition>(csompareMD5, (int)EventType.COMPAREMD5);
        waitCompareMD5.Add<fsm.EventTransition>(stop, (int)EventType.STOP);

        csompareMD5.Add<fsm.EventTransition>(loadAsset, (int)EventType.LOADASSET);
        csompareMD5.Add<fsm.EventTransition>(stop, (int)EventType.STOP);
        loadAsset.Add<fsm.EventTransition>(stop, (int)EventType.STOP);

		stateMachine.initState = awake;
	}


    #region 启动
    protected void EnterAwakeState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (GameCenter.instance.isDevelopmentPattern)
        {
            HasDecompressionAsset = true;
        }
        GameCenter.uIMng.SwitchToUI(GUIType.UPDATEASSET);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;//永不待机
        SceneManager.LoadScene("UpdateAssets");
		//将初始化从UPDATEAPK移到AWAKE,DECOMPRESSION也需要用到  
		//ApkUpdateMng.instance.initSdk();
        if (OnEventTypeUpdateEvent != null)
            OnEventTypeUpdateEvent(EventType.AWAKE);
    }

    protected void UpdateAwakeState(fsm.State _curState)
    {
        if (SceneManager.GetActiveScene().name == "UpdateAssets")
        {
            stateMachine.Send((int)EventType.GET_UI_AND_CHECK_ASSET);
        }
    }
    #endregion

    #region 获取UI控件,检查资源是否已经解压
    protected bool checkFinished;
    protected void EnterGetUIAndCheckAssetState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (!GameCenter.instance.isDevelopmentPattern && GameCenter.instance.isPlatform)
        {
            HasDecompressionAsset = GameCenter.systemSettingMng.LastVersionCode == LynSdkManager.Instance.GetAppVersionCode();
        }

        //检查资源是否已经解压
        if (decompressionWay == DecompressionWay.ZIP ? HasDecompressionAsset : File.Exists(PlatformPathMng.GetFilePath(PlatformPathMng.GetPlatformString, AssetPathType.PersistentDataPath, "/versionAssetMD5.txt")))
        {
            if (GameCenter.instance.isDevelopmentPattern || GameCenter.instance.isNotNeedToMd5)
            {
                stateMachine.Send((int)EventType.STOP);
            }
            else
            {
                //如果已经解压,则取检查md5版本
                stateMachine.Send((int)EventType.WAITCOMPAREMD5);
            }
           
        }
        else
        {
            Debug.logger.Log("DECOMPRESSION1");
//#if WITHOUT_ASSET
//            InitServerZipMd5();
//#endif
            //如果尚未解压,那么先取出资源,再检查apk版本
            stateMachine.Send((int)EventType.DECOMPRESSION1);
            
        }
        
    }

    protected void UpdateGetUIAndCheckAssetState(fsm.State _curState)
    {
        //if (pendings == 0)
        //{
        //    stateMachine.Send((int)EventType.DECOMPRESSION1);
        //}
    }

    protected void EnterWrong(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        stateMachine.Send((int)curFSMState);
    }


    #region MD5
    #region 资源相关
    protected int pendings = 0;
    protected Dictionary<string, VersionAsset> serverVersionZipList = new Dictionary<string, VersionAsset>();
    protected Dictionary<string, VersionAsset> clientVersionAssetList = new Dictionary<string, VersionAsset>();
    protected Dictionary<string, VersionAsset> serverVersionAssetList = new Dictionary<string, VersionAsset>();
    public WWW serverMD5Config;
    public WWW clientMD5Config;
    public long loadTotalSize;


    IEnumerator LoadTxt(string _path, System.Action<string, bool> _callBack)
    {
        WWW www = new WWW(_path);
        yield return www;
        if (_callBack != null)
        {
            _callBack(www.text, www.error != null);
            www.Dispose();
            www = null;
        }
    }


    IEnumerator LoadWWW(string _path, System.Action<WWW, bool> _callBack)
    {
        float startTime = Time.time;
        WWW www = new WWW(_path);
        while (!www.isDone)
        {
            if (www.error != null)
            {
                //Debug.LogError(www.error);
                Debug.logger.Log("www.error != null  " + www.error);
                if (_callBack != null)
                {
                    _callBack(null, true);
                }
                www.Dispose();
                www = null;
                yield break;
            }
            if (Time.time - startTime > 10.0f)
            {
                Debug.logger.Log("拉取MD5超时 " + _path);
                if (_callBack != null)
                {
                    _callBack(null, true);
                }
                www.Dispose();
                www = null;
                yield break;
            }
            yield return null;
        }
        yield return www;
        if (_callBack != null)
        {
            _callBack(www, www.error != null);
            if (www.error != null)
            {
                Debug.Log("www.error:" + www.error);
            }
        }
    }


    //float loadSpeed = 0;
    //float lastLoadSize = 0;


    IEnumerator LoadBytes(string _path, System.Action<byte[], bool> _callBack)
    {
        float startTime = Time.time;
        float startCountTime = Time.time;
        //lastLoadSize = 0;
        float lastProgress = 0;
        WWW www = new WWW(_path);
        while (!www.isDone)
        {
            //loadSpeed = lastLoadSize / (Time.time - startTime);
            //if (Time.frameCount % 30 == 0)
            //{
            //    NGUIDebug.Log(loadSpeed.ToString("0.00") + "m/s");
            //}
            if (www.error != null)
            {
                //Debug.LogError(www.error);
                Debug.logger.Log("www.error != null  " + www.error);
                if (_callBack != null)
                {
                    _callBack(null, true);
                }
                www.Dispose();
                www = null;
                yield break;
            }
            if (www.progress > lastProgress)
            {
                startCountTime = Time.time;
                lastProgress = www.progress;
            }
            if (Time.time - startCountTime > 5.0f)
            {
                Debug.logger.Log("进程没变化超过5秒下载超时 " + _path);
                if (_callBack != null)
                {
                    _callBack(null, true);
                }
                www.Dispose();
                www = null;
                yield break;
            }
            yield return null;
        }
        yield return www;
        if (_callBack != null)
        {
            //lastLoadSize = www.bytes.Length / 1048576.0f;
            //loadSpeed = lastLoadSize / (Time.time - startTime);
            //NGUIDebug.Log(loadSpeed.ToString("0.00") + "m/s");
            _callBack(www.bytes, www.error != null);
            if (www.error != null)
            {
                Debug.LogError(www.error);
            }
            www.Dispose();
            www = null;
        }
    }


    public void InitServerZipMd5()
    {

        pendings += 1;
        string path = PlatformPathMng.GetWWWPath(PlatformPathMng.GetPlatformString, AssetPathType.PersistentDataPath, "/versionAssetMD5.txt");
        if (GameCenter.instance.isPlatform)
        {
            path = "http://cdn.wow.nutsfun.com/patches/ZIP/versionAssetMD5.txt";
        }
        StartCoroutine(LoadTxt(path, (x, y) =>
        {
            --pendings;
            if (!y)
            {
                serverVersionZipList = ReadVersionAssetMd5(x);
            }
            else
            {
                Debug.LogError("加载Server versionZipMD5.txt出错");
                serverVersionZipList.Clear();
            }
        }));
    }


    public void InitClientVersionAssetMd5()
    {
        //    NGUIDebug.Log("本地MD5地址：" + PlatformPathMng.GetWWWPath(PlatformPathMng.GetPlatformString, AssetPathType.PersistentDataPath, "/versionAssetMD5.txt"));
        pendings += 1;
        string path = PlatformPathMng.GetWWWPath(PlatformPathMng.GetPlatformString, AssetPathType.PersistentDataPath, "/versionAssetMD5.txt");
        StartCoroutine(LoadWWW(path, (x, y) =>
            {
                --pendings;
                if (!y)
                {
                    clientMD5Config = x;
                    clientVersionAssetList = ReadVersionAssetMd5(x.text);
                }
                else
                {
                    Debug.LogError("加载Client versionAssetMD5.txt出错");
                    clientVersionAssetList.Clear(); 
                }
            }));
    }

    protected int faildCount = 0;
    public void InitServerVersionAssetMd5(string _serverURL)
    {
        //Debug.Log("远程MD5地址：" + _serverURL);
        pendings += 1;
        StartCoroutine(LoadWWW(_serverURL, (x, y) =>
        {
            --pendings;
            if (!y)
            {
                serverMD5Config = x;
                serverVersionAssetList = ReadVersionAssetMd5(x.text);
					Debug.logger.Log("serverVersionAssetList.Count = " + serverVersionAssetList.Count);
            }
            else
            {
                //if (faildCount >= 5)
                //{
                    SecondConfirmWnd.OpenConfirm(GameHelper.netFaildText, () =>
                    {
                        stateMachine.Send((int)EventType.STOP);
                        return;

                    }, () =>
                    {
                        Application.Quit();
                        NetMsgMng.ConectClose();//关掉net
                        return;
                    });
                //}
                //faildCount++;
                Debug.LogError("加载Server versionAssetMD5.txt出错");
                //InitServerVersionAssetMd5(_serverURL);
            }
        }));
    }


    public Dictionary<string, VersionAsset> ReadVersionAssetMd5(string txtValue)
    {
        string[] tempTable = txtValue.Split("\n"[0]);
        Dictionary<string, VersionAsset> versionAssetDic = new Dictionary<string, VersionAsset>();
        foreach (string tempStr in tempTable)
        {
            string[] tempRow = tempStr.Split(',');
            if (tempRow.Length == 3)
            {
                VersionAsset versionAsset = new VersionAsset();
                string[] str = tempRow[0].Split('/');
                versionAsset.originStr = tempStr;
                versionAsset.subDir = str.Length > 1 ? tempRow[0].Remove(tempRow[0].LastIndexOf('/') + 1) : string.Empty;
                versionAsset.assetName = tempRow[0];
                versionAsset.md5Value = tempRow[1];
                versionAsset.assetSize = int.Parse(tempRow[2]);
                versionAssetDic[versionAsset.assetName] = versionAsset;
            }
        }
        return versionAssetDic;
    }



    public List<VersionAsset> ReadVersionAssetMd5ForCheck(string txtValue)
    {
        loadTotalSize = 0;
        string[] tempTable = txtValue.Split("\n"[0]);
        List<VersionAsset> versionAssetList = new List<VersionAsset>();
        foreach (string tempStr in tempTable)
        {
            if (tempStr.Contains("meta")) continue;
            string[] tempRow = tempStr.Split(',');
            if (tempRow.Length == 3)
            {
                VersionAsset versionAsset = new VersionAsset();
                string[] str = tempRow[0].Split('/');
                versionAsset.originStr = tempStr;
                versionAsset.subDir = str.Length > 1 ? tempRow[0].Remove(tempRow[0].LastIndexOf('/') + 1) : string.Empty;
                versionAsset.assetName = tempRow[0];
                versionAsset.md5Value = tempRow[1];
                versionAsset.assetSize = int.Parse(tempRow[2]);
                loadTotalSize += versionAsset.assetSize;
                versionAssetList.Add(versionAsset);
            }
        }
        return versionAssetList;
    }
    public Dictionary<string, VersionAsset> GetNeedLoadAsset()
    {
        loadTotalSize = 0;
        if (clientVersionAssetList == null || clientVersionAssetList.Count == 0)
        {
            //    NGUIDebug.Log("本地MD5为空，全部需要下载！");
            foreach (var item in serverVersionAssetList.Values)
            {
                if (item.actionType != VersionAsset.ActionType.DELETE)
                {
                    item.actionType = VersionAsset.ActionType.ADD;
                    loadTotalSize += item.assetSize;
                }
            }
            return serverVersionAssetList;
        }
        if (serverVersionAssetList == null || serverVersionAssetList.Count == 0)
        {
            return serverVersionAssetList;
        }
        Dictionary<string, VersionAsset> needLoadList = new Dictionary<string, VersionAsset>();
        foreach (string assetName in serverVersionAssetList.Keys)
        {
            VersionAsset assetMd5 = serverVersionAssetList[assetName];
            if (clientVersionAssetList.ContainsKey(assetName))
            {
                if (clientVersionAssetList[assetName].md5Value != assetMd5.md5Value)
                {
                    assetMd5.actionType = VersionAsset.ActionType.UPDATE;
                    needLoadList[assetName] = assetMd5;//服务器有,客户端有,且不同,则需更新资源
                    loadTotalSize += assetMd5.assetSize;
                }
                clientVersionAssetList.Remove(assetName);//检测完直接Remove,最后剩下的就是客户端需要删除的资源
            }
            else
            {
                assetMd5.actionType = VersionAsset.ActionType.ADD;
                needLoadList[assetName] = assetMd5;//服务器有,客户端没有,则新添加资源
                loadTotalSize += assetMd5.assetSize;
            }
        }
        foreach (string assetName in clientVersionAssetList.Keys)
        {
            needLoadList[assetName] = clientVersionAssetList[assetName];
            needLoadList[assetName].actionType = VersionAsset.ActionType.DELETE;//客户端需要删除的资源
        }
        return needLoadList;
    }
    #endregion
    #endregion
    #endregion

    #region 资源解压
    System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

    protected void EnterDecompression1State(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        startWaitTime = Time.time;
		curFSMState = EventType.DECOMPRESSION1;
#if UNITY_ANDROID
        //if (Application.platform == RuntimePlatform.Android)
        //{
        //    int size = (int)ApkUpdateMng.instance.GetRemainingSize();
        //    if (size <= NeedStorageSize)
        //    {
        //        if (OnShowStorageSizeTip != null)
        //            OnShowStorageSizeTip(NeedStorageSize, size);
        //        return;
        //    }
        //}
#endif
        fromMD5 = null;
        startLoadSubAsset = false;
		StartCoroutine(StartLoadTotalAsset(curFSMState));
        if (OnEventTypeUpdateEvent != null)
            OnEventTypeUpdateEvent(EventType.DECOMPRESSION);
    }
	protected void EnterDecompression2State(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
		if(pack != null)
			pack.Progress = 0f;
		curFSMState = EventType.DECOMPRESSION2;
		#if UNITY_ANDROID
        //if(Application.platform == RuntimePlatform.Android)
        //{
        //    int size = (int)ApkUpdateMng.instance.GetRemainingSize();
        //    if(size <= NeedStorageSize)
        //    {
        //        if(OnShowStorageSizeTip != null)
        //            OnShowStorageSizeTip(NeedStorageSize,size);
        //        return;
        //    }
        //}
		#endif
		//    NGUIDebug.Log("EnterDecompressionState");
		fromMD5 = null;
		startLoadSubAsset = false;
		StartCoroutine(StartLoadTotalAsset(curFSMState));
	}
	protected void EnterDecompression3State(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
		if(pack != null)
			pack.Progress = 0f;
		curFSMState = EventType.DECOMPRESSION3;
		#if UNITY_ANDROID
        //if(Application.platform == RuntimePlatform.Android)
        //{
        //    int size = (int)ApkUpdateMng.instance.GetRemainingSize();
        //    if(size <= NeedStorageSize)
        //    {
        //        if(OnShowStorageSizeTip != null)
        //            OnShowStorageSizeTip(NeedStorageSize,size);
        //        return;
        //    }
        //}
		#endif
		//    NGUIDebug.Log("EnterDecompressionState");
		fromMD5 = null;
		startLoadSubAsset = false;
		StartCoroutine(StartLoadTotalAsset(curFSMState));
	}
    protected void EnterDecompression4State(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (pack != null)
            pack.Progress = 0f;
        curFSMState = EventType.DECOMPRESSION4;
#if UNITY_ANDROID
        //if(Application.platform == RuntimePlatform.Android)
        //{
        //    int size = (int)ApkUpdateMng.instance.GetRemainingSize();
        //    if(size <= NeedStorageSize)
        //    {
        //        if(OnShowStorageSizeTip != null)
        //            OnShowStorageSizeTip(NeedStorageSize,size);
        //        return;
        //    }
        //}
#endif
        //    NGUIDebug.Log("EnterDecompressionState");
        fromMD5 = null;
        startLoadSubAsset = false;
        StartCoroutine(StartLoadTotalAsset(curFSMState));
    }
    protected void EnterDecompression5State(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (pack != null)
            pack.Progress = 0f;
        curFSMState = EventType.DECOMPRESSION5;
#if UNITY_ANDROID
        //if(Application.platform == RuntimePlatform.Android)
        //{
        //    int size = (int)ApkUpdateMng.instance.GetRemainingSize();
        //    if(size <= NeedStorageSize)
        //    {
        //        if(OnShowStorageSizeTip != null)
        //            OnShowStorageSizeTip(NeedStorageSize,size);
        //        return;
        //    }
        //}
#endif
        //    NGUIDebug.Log("EnterDecompressionState");
        fromMD5 = null;
        startLoadSubAsset = false;
        StartCoroutine(StartLoadTotalAsset(curFSMState));
    }
    protected void EnterDecompression6State(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (pack != null)
            pack.Progress = 0f;
        curFSMState = EventType.DECOMPRESSION6;
#if UNITY_ANDROID
        //if(Application.platform == RuntimePlatform.Android)
        //{
        //    int size = (int)ApkUpdateMng.instance.GetRemainingSize();
        //    if(size <= NeedStorageSize)
        //    {
        //        if(OnShowStorageSizeTip != null)
        //            OnShowStorageSizeTip(NeedStorageSize,size);
        //        return;
        //    }
        //}
#endif
        //    NGUIDebug.Log("EnterDecompressionState");
        fromMD5 = null;
        startLoadSubAsset = false;
        StartCoroutine(StartLoadTotalAsset(curFSMState));
    }
    protected void EnterDecompression7State(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (pack != null)
            pack.Progress = 0f;
        curFSMState = EventType.DECOMPRESSION7;
#if UNITY_ANDROID
        //if(Application.platform == RuntimePlatform.Android)
        //{
        //    int size = (int)ApkUpdateMng.instance.GetRemainingSize();
        //    if(size <= NeedStorageSize)
        //    {
        //        if(OnShowStorageSizeTip != null)
        //            OnShowStorageSizeTip(NeedStorageSize,size);
        //        return;
        //    }
        //}
#endif
        //    NGUIDebug.Log("EnterDecompressionState");
        fromMD5 = null;
        startLoadSubAsset = false;
        StartCoroutine(StartLoadTotalAsset(curFSMState));
    }
    protected void EnterDecompression8State(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (pack != null)
            pack.Progress = 0f;
        curFSMState = EventType.DECOMPRESSION8;
#if UNITY_ANDROID
        //if(Application.platform == RuntimePlatform.Android)
        //{
        //    int size = (int)ApkUpdateMng.instance.GetRemainingSize();
        //    if(size <= NeedStorageSize)
        //    {
        //        if(OnShowStorageSizeTip != null)
        //            OnShowStorageSizeTip(NeedStorageSize,size);
        //        return;
        //    }
        //}
#endif
        //    NGUIDebug.Log("EnterDecompressionState");
        fromMD5 = null;
        startLoadSubAsset = false;
        StartCoroutine(StartLoadTotalAsset(curFSMState));
    }


    string fromPath = string.Empty;
    string toPath = string.Empty;
    WWW fromMD5 = null;
    bool startLoadSubAsset = false;
    string zipName = string.Empty;
    UnpackZip pack = null;

    IEnumerator StartLoadTotalAsset(EventType _type)
    {
        CurLoadRate = 0;
        CurLoadFileRate = 1;
        string zipFromPath = PlatformPathMng.GetWWWPath(PlatformPathMng.GetPlatformString, AssetPathType.StreamingAssetsPath, "");
        string zipToPath = PlatformPathMng.GetFilePath(PlatformPathMng.GetPlatformString, AssetPathType.PersistentDataPath, "");
        fromPath = zipFromPath + "/";
        toPath = zipToPath + "/";


        if (decompressionWay == DecompressionWay.ZIP)
        {
            string zipLastName = string.Empty;
            switch (_type)
            {
                case EventType.DECOMPRESSION1:
                    zipLastName = "1.zip";
                    break;
                case EventType.DECOMPRESSION2:
                    zipLastName = "2.zip";
                    break;
                case EventType.DECOMPRESSION3:
                    zipLastName = "3.zip";
                    break;
                case EventType.DECOMPRESSION4:
                    zipLastName = "4.zip";
                    break;
                case EventType.DECOMPRESSION5:
                    zipLastName = "5.zip";
                    break;
                case EventType.DECOMPRESSION6:
                    zipLastName = "6.zip";
                    break;
                case EventType.DECOMPRESSION7:
                    zipLastName = "7.zip";
                    break;
                case EventType.DECOMPRESSION8:
                    zipLastName = "8.zip";
                    break;
                default:
                    break;
            }
            w = new WWW(zipFromPath + zipLastName);
            yield return w;
            if (w.error != null)
            {
                Debug.logger.Log("w.error = " + w.error);
                //NGUIDebug.Log(w.error + ",uri:" + zipFromPath + zipLastName + ",跳转检查md5");
                if (GameCenter.instance.isDevelopmentPattern || GameCenter.instance.isNotNeedToMd5)
                {
                    stateMachine.Send((int)EventType.STOP);
                }
                else
                {
                    //如果是精简包,没有压缩资源,那么跳转检查md5
                    stateMachine.Send((int)EventType.WAITCOMPAREMD5);
                }
            }
            else
            {
                if (w.bytes != null)
                {
                    //byte[] retVal = md5.ComputeHash(w.bytes);
                    //string curHash = ToHex(retVal, false);//System.Text.Encoding.Unicode.GetString(retVal);
                    //string curKey = "assetbundles_android" + zipLastName;
                    //if (serverVersionZipList.ContainsKey(curKey))
                    //{
                    //    if (!serverVersionZipList[curKey].md5Value.Equals(curHash))
                    //    {
                    //        Debug.LogError("下载失败，远程zip的md5为：" + serverVersionZipList[curKey].md5Value + " , 本次下载的zip的md5为" + curHash + " , 重新开始下载！" + _type);
                    //        stateMachine.Send((int)EventType.WRONG);
                    //        w.Dispose();
                    //        w = null;
                    //        yield break;
                    //    }
                    //    else
                    //    {
                    zipName = zipToPath + Time.time.ToString() + ".zip";
                    File.WriteAllBytes(zipName, w.bytes);
                    pack = UnpackZip.GetInstance(gameObject);
                    pack.StartUnpack1(zipName, toPath);
                    //    }
                    //}
                }
                else
                {
                    // NGUIDebug.Log("w.bytes == null");
                }
            }
        }
    }


    public string ToHex(byte[] bytes, bool upperCase)
    {
        StringBuilder result = new StringBuilder(bytes.Length * 2);

        for (int i = 0; i < bytes.Length; i++)
            result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));

        return result.ToString();
    }

    protected void UpdateDecompressionState(fsm.State _curState)
    {
        if (NeedTick)
        {
            GameCenter.uIMng.GenGUI(GUIType.RETURN_EXIT, true);
            NeedTick = false;
        }
        if (decompressionWay == DecompressionWay.ZIP)
        {
            if (pack != null)
            {
                string[] list = pack.CurFileName.Split('/');
                CurFileName = list.Length > 0 ? list[list.Length - 1] : string.Empty;
                //CurLoadedSize = pack.HasUnPackCount;
                //TotalLoadSize = pack.TotalUnPackCount;
                switch (curFSMState)
                {
                    case EventType.DECOMPRESSION1:
                        CurLoadRate = pack.Progress * (1f / 8f);
                        break;
                    case EventType.DECOMPRESSION2:
                        CurLoadRate = 1f / 8f + pack.Progress * (1f / 8f);
                        break;
                    case EventType.DECOMPRESSION3:
                        CurLoadRate = 2f / 8f + pack.Progress * (1f / 8f);
                        break;
                    case EventType.DECOMPRESSION4:
                        CurLoadRate = 3f / 8f + pack.Progress * (1f / 8f);
                        break;
                    case EventType.DECOMPRESSION5:
                        CurLoadRate = 4f / 8f + pack.Progress * (1f / 8f);
                        break;
                    case EventType.DECOMPRESSION6:
                        CurLoadRate = 5f / 8f + pack.Progress * (1f / 8f);
                        break;
                    case EventType.DECOMPRESSION7:
                        CurLoadRate = 6f / 8f + pack.Progress * (1f / 8f);
                        break;
                    case EventType.DECOMPRESSION8:
                        CurLoadRate = 7f / 8f + pack.Progress * (1f / 8f);
                        break;
                }

//                if (CurLoadFileRate >= 1)
//                {
//                    StartCoroutine(FakeCurLoadFileProgress());
//                }
                if (!HasDecompressionAsset)
                {
					if (pack.Progress >= 1)
                    {
                        File.Delete(zipName);
                        w.Dispose();
						w = null;//释放内存
                        switch (curFSMState)
                        {
                            case EventType.DECOMPRESSION1:
                                stateMachine.Send((int)EventType.DECOMPRESSION2);
                                break;
                            case EventType.DECOMPRESSION2:
                                stateMachine.Send((int)EventType.DECOMPRESSION3);
                                break;
                            case EventType.DECOMPRESSION3:
                                stateMachine.Send((int)EventType.DECOMPRESSION4);
                                break;
                            case EventType.DECOMPRESSION4:
                                stateMachine.Send((int)EventType.DECOMPRESSION5);
                                break;
                            case EventType.DECOMPRESSION5:
                                stateMachine.Send((int)EventType.DECOMPRESSION6);
                                break;
                            case EventType.DECOMPRESSION6:
                                stateMachine.Send((int)EventType.DECOMPRESSION7);
                                break;
                            case EventType.DECOMPRESSION7:
                                stateMachine.Send((int)EventType.DECOMPRESSION8);
                                break;
                            case EventType.DECOMPRESSION8:
                                if (!GameCenter.instance.isDevelopmentPattern && GameCenter.instance.isPlatform)
                                {
                                    GameCenter.systemSettingMng.LastVersionCode = LynSdkManager.Instance.GetAppVersionCode();
                                }
                                HasDecompressionAsset = true;
                                break;
                        }

                    }
                }
                else
                {
                    pack = null;
                    if (GameCenter.instance.isDevelopmentPattern || GameCenter.instance.isNotNeedToMd5)
                    {
                        stateMachine.Send((int)EventType.STOP);
                    }
                    else
                    {
                        stateMachine.Send((int)EventType.WAITCOMPAREMD5);
                        if (OnEventTypeUpdateEvent != null)
                            OnEventTypeUpdateEvent(EventType.COMPAREMD5);
                    }
                }
            }
        }
    }
    #endregion

    #region 获取服务器地址
    protected void EnterInitServerAdressState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
    }


    protected void UpdateInitServerAdressState(fsm.State _curState)
    {
    }
    #endregion
	
	#region MD5文件比对处理,检查资源是否需要更新
    //当前已经下载了多少个文件
    protected int curHaveLoad = 0;
    //当前正在下载的文件名
    protected string curLoadName = string.Empty;
	Dictionary<string, VersionAsset> needLoadList = null;
    protected string assetsServerURL = string.Empty;
	protected void EnterCompareMD5State(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
        Debug.Log("进入md5比对");
        toPath = PlatformPathMng.GetFilePath(PlatformPathMng.GetPlatformString, AssetPathType.PersistentDataPath, "") + "/";
        InitClientVersionAssetMd5();
        InitTemp();
       // assetsServerURL = "file:///ftp://120.92.227.108/assetbundles_android";
        //assetsServerURL = "file:///D:/assetbundles_android";
        if (GameCenter.instance.isDevelopmentPattern && !GameCenter.instance.isPlatform)
        {
            assetsServerURL = "https://patches.lynlzqy.com/patches/10/cn/6.12.4/assetbundles_android";
        }
        else
        {
            assetsServerURL = LynSdkManager.Instance.GetHotPatchUrl();
            Debug.Log("远程MD5地址：" + assetsServerURL);
            if (assetsServerURL == string.Empty || assetsServerURL == "Fail")
            {
                stateMachine.Send((int)EventType.STOP);
            }
        }
        InitServerVersionAssetMd5(assetsServerURL + "/versionAssetMD5.txt");
    }

    protected void UpdateCompareMD5State(fsm.State _curState)
    {
        if (pendings == 0 && serverMD5Config != null)
        {
            fromMD5 = serverMD5Config;
            needLoadList = GetNeedLoadAsset();
            CheckTempLoadSize();
            //Debug.logger.Log("获取服务端md5文件成功,需要更新的数量为 : " + needLoadList.Count);
            if (needLoadList == null || needLoadList.Count == 0)
            {
                stateMachine.Send((int)EventType.STOP);
            }
            else
            {
                //NGUIDebug.Log("needLoadList.Count = " + needLoadList.Count);
                stateMachine.Send((int)EventType.LOADASSET);
            }
        }
    }
    #endregion

    #region 开始更新
    protected string tempPath = string.Empty;
    protected Dictionary<string, VersionAsset> tempDic = new Dictionary<string, VersionAsset>();
    protected void InitTemp()
    {
        tempPath = PlatformPathMng.GetWWWPath(PlatformPathMng.GetPlatformString, AssetPathType.PersistentDataPath, "/tempAssetMD5.txt");
        if (!File.Exists(toPath + "tempAssetMD5.txt"))
        {
            tempDic.Clear();
            return;
        }
        pendings += 1;
        StartCoroutine(LoadWWW(tempPath, (x, y) =>
        {
            --pendings;
            if (!y)
            {
                tempDic = ReadVersionAssetMd5(x.text);
            }
            else
            {
                Debug.LogError("加载tempAssetMD5.txt出错,无法执行断点下载");
                tempDic.Clear();
            }
        }));
    }


    protected void CheckTempLoadSize()
    {
        curHaveLoad = 0;
        foreach (var item in tempDic.Values)
        {
            loadTotalSize -= item.assetSize;
            curHaveLoad++;
        }
        loadTotalSize = (int)Mathf.Max(0, loadTotalSize);
    }


	protected List<string> tempStringList = new List<string>();

    /// <summary>
    /// 保存断点记录文件
    /// </summary>
    /// <param name="_path"></param>
    /// <param name="_appendValue"></param>
    protected void SaveTemp(string _appendValue)
	{
		_appendValue += "\n";
		tempStringList.Add (_appendValue);
	}


	protected bool isCreatingTemp = false;
	protected void WriteTemp()
	{
		lock (tempStringList) {
			if (!File.Exists (toPath + "tempAssetMD5.txt"))
			{
				if (!isCreatingTemp) {
					FileStream st = File.Create (toPath + "tempAssetMD5.txt");
					st.Close ();
					isCreatingTemp = true;
					return;
				} else {
					return;
				}
			}
			for (int i = 0; i < tempStringList.Count; i++) {
				File.AppendAllText (toPath + "tempAssetMD5.txt", tempStringList[i], Encoding.UTF8);
			}
		}
		tempStringList.Clear();
	}

    /// <summary>
    ///清理断点记录文件
    /// </summary>
    protected bool CleanTemp()
    {
        if (File.Exists(toPath + "tempAssetMD5.txt"))
        {
            File.Delete(toPath + "tempAssetMD5.txt");
        }
        if (File.Exists(toPath + "tempAssetMD5.txt"))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// 检查是否已经断点下载
    /// </summary>
    /// <returns></returns>
    protected bool CheckTempHas(string _assetName,string _md5)
    {
        if (tempDic.ContainsKey(_assetName) && tempDic[_assetName].md5Value == _md5)
        {
            return true;
        }
        return false;
    }


    protected void EnterLoadAssetState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (OnEventTypeUpdateEvent != null)
            OnEventTypeUpdateEvent(EventType.LOADASSET);
        //Debug.logger.Log("开始下载更新文件");
        needAssetList.Clear();
        errorCountDic.Clear();
        //NGUIDebug.Log("EnterLoadAssetState");
        curLoadName = string.Empty;
        curLoadedSize = 0;
        CurLoadFileRate = 1;
        CurFileName = string.Empty;
        totalLoadSize = loadTotalSize;
        fromPath = assetsServerURL + "/";
        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) //如果是在wifi环境下,直接开始下载
        {
            checkFinished = false;
            needAssetList = new List<VersionAsset>(needLoadList.Values);
            startLoadSubAsset = true;

        }
        else //如果非wifi环境下,则需要询问
        {
            SecondConfirmWnd.OpenConfirm(GameHelper.secondComfirmText.Replace("#0#", (TotalLoadSize / 1048576.0f).ToString("0.00")), () =>
                {
                    toPath = PlatformPathMng.GetFilePath(PlatformPathMng.GetPlatformString, AssetPathType.PersistentDataPath, "") + "/";
                    checkFinished = false;
                    needAssetList = new List<VersionAsset>(needLoadList.Values);
                    startLoadSubAsset = true;
                    //Debug.logger.Log("确认进入下载!");

                }, () =>
                    {
                        Application.Quit();
                        NetMsgMng.ConectClose();//关掉net
                    });
        }


    }


    //int testindex = 0;
    protected Dictionary<string, int> errorCountDic = new Dictionary<string, int>();

    void LoadAsset(VersionAsset _info)
    {
        //if (testindex > 5)
        //{
        //    Debug.LogError("hold");
        //}
        //testindex++;
        string path = toPath + _info.assetName;
        //Debug.logger.Log("开始下载更新文件:" + _info.assetName + " ,大小:" + _info.assetSize + " , 更新类型: " + _info.actionType);
        if (_info.actionType == VersionAsset.ActionType.ADD || _info.actionType == VersionAsset.ActionType.UPDATE)
        {
            string[] namelist = _info.assetName.Split('/');
            curLoadName = namelist.Length > 0 ? namelist[namelist.Length - 1] : string.Empty;
            pendings++;
            string url = fromPath + _info.assetName;
            //AssetMng.DownloadID id = 
            StartCoroutine(LoadBytes(url, (x, y) =>
        {
            Debug.logger.Log(_info.assetName + "下载结果: " + y);
            string dir = toPath + _info.subDir;
            if (!File.Exists(path))
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
            pendings--;

            if (y)
            {
                //Debug.logger.Log(_info.assetName + "下载失败，重新压入下载队列!弹出网络异常框！");
                needAssetList.Insert(0, _info);
                //if (errorCountDic.ContainsKey(url))
                //{
                //    errorCountDic[url]++;
                //}
                //else
                //{
                //    errorCountDic[url] = 1;
                //}
                //if (errorCountDic[url] < 5)
                //{
                //    needAssetList.Insert(0, _info);
                //}
                //else
                //{
                startLoadSubAsset = false;
                //下载中途网络异常
                SecondConfirmWnd.OpenConfirm(GameHelper.loadNetFaildText, () =>
                {
                    startLoadSubAsset = true;
                    //errorCountDic[url] = 0;
                    return;

                }, () =>
                {
                    Application.Quit();
                    NetMsgMng.ConectClose();//关掉net
                    return;
                });
                return;
                //}
                //return;
            }

            byte[] retVal = md5.ComputeHash(x);
            string curHash = ToHex(retVal, false);
            if (_info.md5Value.Equals(curHash))
            {
                //Debug.logger.Log(_info.assetName + "比对md5成功，断点记录下载成功! ");
                SaveTemp(_info.originStr);
            }
            else
            {
                Debug.LogError("下载失败，远程asset的md5为：" + _info.md5Value + " , 本次下载的asset的md5为" + curHash + " , 重新开始下载！");
                if (errorCountDic.ContainsKey(url))
                {
                    errorCountDic[url]++;
                }
                else
                {
                    errorCountDic[url] = 1;
                }
                if (errorCountDic[url] < 5)
                {
                    needAssetList.Insert(0, _info);
                }
                return;
            }

            File.WriteAllBytes(path, x);
            AssetMng.instance.UnloadUrl(fromPath + _info.assetName);
            x = null;
            curHaveLoad++;
            CurLoadedSize += _info.assetSize;
            CurLoadRate = CurLoadedSize / (float)TotalLoadSize;
            string[] list = _info.assetName.Split('/');
            CurFileName = list.Length > 0 ? list[list.Length - 1] : string.Empty;
        }));
        }
        else if (_info.actionType == VersionAsset.ActionType.DELETE)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

    List<VersionAsset> needAssetList = new List<VersionAsset>();
    protected void UpdateLoadAssetState(fsm.State _curState)
    {
		if (Time.frameCount % 10 == 0) {
			WriteTemp ();
			//Debug.logger.Log ("剩余下载任务：" + needAssetList.Count);
		}
        if (startLoadSubAsset && pendings < 5)
        {
			for (int i = 0; i < 10; i++) {
				if (needAssetList.Count > 0) {
					VersionAsset item = needAssetList [0];
                    if (item != null)
                    {
                        if (!CheckTempHas(item.assetName, item.md5Value))
                        {
                            LoadAsset(item);
                        }
                        else
                        {
                            // Debug.logger.Log("检测到" + item.assetName + "已经存在于断点下载记录中,跳过!进行下一个下载");
                        }
                    }
					needAssetList.RemoveAt (0);
                    //Debug.logger.Log("剩余需要下载的队列长度: " + needAssetList.Count);
				}
			}
        }
        if (startLoadSubAsset && pendings == 0 && needAssetList.Count == 0)
        {
            //Debug.logger.Log("下载更新文件结束!");
            File.WriteAllBytes(toPath + "versionAssetMD5.txt", fromMD5.bytes);
            CleanTemp();
            //资源更新完成,进入游戏
            stateMachine.Send((int)EventType.STOP);
            startLoadSubAsset = false;
        }
    }


    #endregion

    #region 退出 
    protected void EnterStopState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        GameCenter.uIMng.ReleaseGUI(GUIType.RETURN_EXIT);
        NeedTick = false;
        GameCenter.uIMng.ReleaseGUI(GUIType.UPDATEASSET);
		fromMD5 = null;
        if (clientMD5Config != null)
        {
            clientMD5Config.Dispose();
            clientMD5Config = null;
        }
        if (serverMD5Config != null)
        {
            serverMD5Config.Dispose();
            clientMD5Config = null;
        }
		Caching.CleanCache();
        GameCenter.instance.GoInitConfig();
    }


    public void ForceReset()
    {
        HasDecompressionAsset = false;
        GameCenter.systemSettingMng.LastVersionCode = 0;
    }
    #endregion

    #region 等待MD5初始化
    protected float startWaitCompareMD5Time = 0f;
    protected int tryTimes = 0;
    protected void EnterWaitCompareMD5State(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        Debug.Log("EnterWaitCompareMD5State");
        startWaitCompareMD5Time = Time.time;
        tryTimes = 0;
        if (GameCenter.instance.CanInitMd5Url)
            stateMachine.Send((int)EventType.COMPAREMD5);
    }

    protected void UpdateWaitCompareMD5State(fsm.State _curState)
    {
        if (Time.time - startWaitCompareMD5Time > 5)//5秒后直接比对MD5
        {
            stateMachine.Send((int)EventType.COMPAREMD5);
        }
        if (Mathf.FloorToInt(Time.time - startWaitCompareMD5Time) > tryTimes)//一秒尝试一次
        {
            tryTimes++;
            if (GameCenter.instance.CanInitMd5Url)
                stateMachine.Send((int)EventType.COMPAREMD5);
        }
    }

    #endregion
}
