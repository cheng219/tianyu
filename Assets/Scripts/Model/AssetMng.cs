///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/5/9
//用途：资源下载管理类
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

#if !UNITY_WEBPLAYER
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endif


public enum EResult
{
    Unknown = -1,
    Success,
    Failed,
    TimeOut,
    Error404,
    Cancelled,
    NotFound,
}

/// <summary>
/// 资源下载管理类 by吴江
/// </summary>
public class AssetMng : MonoBehaviour
{
	/// <summary>
	/// 当前版本的增量包ID(读取客户端配置之后不为0) 
	/// </summary>
	public static int curAddAsset = 3;//先设置成3,否则不经过updateassets场景进游戏都会需要下载增量包 
	/// <summary>
	/// 需要下载的增量包ID(切换场景,发现该场景需求的增量包没有,则不为0) 
	/// </summary>
	public static int needAddAsset = 0;
	
    /// <summary>
    /// 加载的任务对象 by吴江
    /// </summary>
    public class DownloadID
    {
        public string shortURL = "";
        public System.Action<UnityEngine.Object, EResult> onComplete = null;
        public System.Action<float> onProgressUpdate = null;
        public bool isStatic = false;

        public DownloadID(string _shortURL,
                            System.Action<UnityEngine.Object, EResult> _completeCallback,
                            System.Action<float> _progressCallback,
                            bool _isStatic = false )
        {
            shortURL = _shortURL;
            onComplete = _completeCallback;
            onProgressUpdate = _progressCallback;
            isStatic = _isStatic;
        }


    }

    /// <summary>
    /// 加载的任务信息 by吴江
    /// </summary>
    public class DownloadInfo
    {
        public string shortURL = "";
        public float timeout = -1.0f;
        public bool isStatic = false;
        public System.Action<UnityEngine.Object, EResult> onComplete = null;
        public System.Action<float> onProgressUpdate = null;
        public int retry = 3;

        public DownloadInfo(string _shortURL,
                              float _timeout,
                              System.Action<UnityEngine.Object, EResult> _completeCallback,
                              System.Action<float> _progressCallback,
                                bool _isStatic = false)
        {
            shortURL = _shortURL;
            timeout = _timeout;
            onComplete = _completeCallback;
            onProgressUpdate = _progressCallback;
            isStatic = _isStatic;
        }
    }

    /// <summary>
    /// 唯一实例 by吴江
    /// </summary>
    public static AssetMng instance = null;

    /// <summary>
    /// 是否已经初始化完毕 by吴江
    /// </summary>
    public static bool initialized = false;



    /// <summary>
    /// 最大同时加载数量 by吴江
    /// </summary>
    public int maxDownloads = 10;

    /// <summary>
    /// 基本路径 by吴江
    /// </summary>
    protected static string baseURL = "";

    /// <summary>
    /// 资源缓存对象
    /// </summary>
    public class WWWCache
    {
        public bool isStatic = false;
        public string url;
        public UnityEngine.Object obj = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_www">www资源引用</param>
        /// <param name="_isStatic">拆箱后的资源引用是否免于卸载</param>
        /// <param name="_needUnload">拆箱前的资源引用是否免于卸载</param>
        public WWWCache(WWW _www, bool _isStatic, bool _needUnload = true)
        {
            if (_www != null)
            {
                url = _www.url;
                if (_www.assetBundle != null && _needUnload)
                {
                    UnityEngine.Object[] objList = _www.assetBundle.LoadAllAssets();
                    if (objList.Length > 0)
                    {
                        obj = objList[0];
                    }
                    _www.assetBundle.Unload(false);
                }
                _www.Dispose();
                _www = null;
            }
            isStatic = _isStatic;
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_www">www资源引用</param>
        /// <param name="_isStatic">拆箱后的资源引用是否免于卸载</param>
        /// <param name="_needUnload">拆箱前的资源引用是否免于卸载</param>
        public WWWCache(string _url, UnityEngine.Object _obj, bool _isStatic, bool _needUnload = true)
        {
            url = _url;
            obj = _obj;
            isStatic = _isStatic;
        }
    }


    /// <summary>
    /// 当前的资源缓存队列 by吴江
    /// </summary>
    protected Dictionary<string, WWWCache> cache = new Dictionary<string, WWWCache>();

    /// <summary>
    /// 当前正在加载的加载信息队列 by吴江
    /// </summary>
    protected Dictionary<string, DownloadInfo> processingDownloads = new Dictionary<string, DownloadInfo>();

    /// <summary>
    /// 当前等待加载的任务队列 by吴江
    /// </summary>
    protected Queue<DownloadInfo> pendingDownloads = new Queue<DownloadInfo>();

    /// <summary>
    /// 当前同时进行的加载数量 by吴江
    /// </summary>
    protected int downloadThreads = 0;



    void Awake()
    {
        instance = this;
        if (transform.parent == null)
        {
            DontDestroyOnLoad(this);
        }
    }



    //按path类型获取完整路径 by吴江
    static public string GetPathWithoutExtension(string path, AssetPathType pathtype)
    {
		path = PlatformPathMng.SubPathByPlatform(path);
		return PlatformPathMng.GetWWWPath(path,pathtype,"");

    }


    //按path类型获取完整路径 by吴江 
    static public string GetPathWithExtension(string path, AssetPathType pathtype)
    {
			path = PlatformPathMng.SubPathByPlatform(path);
			return PlatformPathMng.GetWWWPath(path,pathtype,".assetbundle");
    }



	/// <summary>
	/// 获取XML文件的路径 by 
	/// </summary>
    static public string GetXmlLoadFilePath(string path, AssetPathType pathtype)
	{
		path = PlatformPathMng.SubPathByPlatform(path);
		return PlatformPathMng.GetFilePath(path,pathtype,"");
//#if UNITY_ANDROID
//		if (Application.platform == RuntimePlatform.Android)
//		{
//			if(pathtype==0)
//				return Application.streamingAssetsPath + "/assetbundles/" + path;
//			else
//			{
//				return Application.persistentDataPath + "/assetbundles_android/" + path;
//			}
//		}
//		else            
//		{
//			if (pathtype == 0)
//				return "file:///" +Application.streamingAssetsPath + "/assetbundles/" + path;
//			else
//			{
//				return Application.persistentDataPath + "/assetbundles_android/" + path;
//			}
//		}
//#elif UNITY_IPHONE		
//        if (Application.platform == RuntimePlatform.IPhonePlayer)
//        {
//            if(pathtype==0)
//       		    return Application.streamingAssetsPath + "/assetbundles/" + path;
//		   else
//		    {
//			    return "file:///" + Application.persistentDataPath + "/assetbundles_ios/" + path;
//		    }
//        }
//        else            
//        {
//            if (pathtype == 0)
//                return "file:///" +Application.streamingAssetsPath + "/assetbundles/" + path;
//           else
//            {
//                return Application.persistentDataPath + "/assetbundles_ios/" + path;
//            }
//        }
//		
//#else
//		if (pathtype == 0)
//			return "file://" + Application.streamingAssetsPath + "/assetbundles/" + path;
//		else
//		{
//			return "file:///" + Application.persistentDataPath + "/assetbundles_win32/" + path;
//		}
//#endif
	}
	
    //按path类型获取完整路径（没有斜杆） 
    static public string GetPathWithoutDiagonal(string path, AssetPathType pathtype)
    {
		path = PlatformPathMng.SubPathByPlatform(path);
		return PlatformPathMng.GetFilePath(path,pathtype,"");
//        if (Application.platform == RuntimePlatform.Android)
//        {
//            if (pathtype == 0)
//                return Application.streamingAssetsPath + "/assetbundles" + path;
//            else if (pathtype == 1)
//            {
//                return "file:///" + Application.persistentDataPath + "/assetbundles_android" + path;
//            }
//            else
//                return baseURL + "/assetbundles_http" + path;
//        }
//        else
//        {
//            if (pathtype == 0)
//                return "file://" + Application.streamingAssetsPath + "/assetbundles" + path;
//            else if (pathtype == 1)
//            {
//                return Application.persistentDataPath + "/assetbundles_android" + path;
//            }
//            else
//                return baseURL + "/assetbundles_http" + path;
//        }
    }	
    /// <summary>
    /// 初始化 by吴江
    /// </summary>
    /// <returns></returns>
    public bool Init()
    {
        if (initialized == false)
        {
            // init internal value from game system settings
            baseURL = "";//GetFilePath("", 1);
            //Debug.LogInternal("资源地址: " + baseURL);

            SceneLoadUtil sceneLoadUtil = this.GetComponent<SceneLoadUtil>();
            if (sceneLoadUtil == null) sceneLoadUtil = this.gameObject.AddComponent<SceneLoadUtil>();

            GameObject effectAssetFinalObj = new GameObject("EffectAssetFinalObjCache");
            effectAssetFinalObj.transform.parent = this.transform;
            effectAssetFinalObj.transform.localPosition = Vector3.zero;
            EffectLoadUtil effectLoadUtil = this.GetComponent<EffectLoadUtil>();
            if (effectLoadUtil == null) effectLoadUtil = this.gameObject.AddComponent<EffectLoadUtil>();
            effectLoadUtil.finalObjCacheTrans = effectAssetFinalObj.transform;

            initialized = true;
        }
        return true;
    }



    void Update()
    {
        if (pendingDownloads.Count > 0)
        {
            int freeThreads = maxDownloads - downloadThreads;
            for (int i = 0; i < freeThreads; ++i)
            {
                if (pendingDownloads.Count > 0)
                {
                    DownloadInfo info = pendingDownloads.Dequeue();
                    //我们有可能取消下载
                    StartCoroutine(Download(info));
                }
            }
        }
    }


    void OnDestroy()
    {
        StopAllCoroutines();
        CancelInvoke();
    }



    public void RedirectAssetServer(string _baseURL)
    {
        baseURL = _baseURL;
       // Debug.LogInternal("Redirect Asset Server To: " + baseURL);
    }



    //IEnumerator Download(DownloadInfo _info)
    //{
    //    string url = baseURL + _info.shortURL;
    //    url = url.Remove(0, 8);
    //    UnityEngine.Object obj = null;
    //    AssetBundleCreateRequest ac = AssetBundle.LoadFromFileAsync(url);

    //    ++downloadThreads;
    //    float timer = 0.0f;
    //    while (ac.isDone == false)
    //    {
    //        // 检查是否需要取消本次加载
    //        if (_info.onComplete == null)
    //        {
    //            float timer2 = 0.0f;
    //            while (timer2 <= 5.0f && ac.progress <= 0.1f)
    //            {
    //                yield return null;
    //                if (_info.onComplete != null)
    //                {
    //                    break;
    //                }
    //                timer2 += Time.deltaTime;
    //            }

    //            //限制5秒加载时间，超时则直接回调
    //            if (_info.onComplete == null)
    //            {
    //                yield break;
    //            }
    //        }

    //        //更新进度
    //        if (_info.onProgressUpdate != null)
    //        {
    //            float progress = ac.progress;
    //            _info.onProgressUpdate(progress);
    //        }
    //        yield return null;

    //        // 检查是否超时
    //        timer += Time.deltaTime;
    //        if (_info.timeout > 0.0f)
    //        {
    //            if (timer >= _info.timeout)
    //            {
    //                while (ac.progress <= 0.1f)
    //                {
    //                    yield return new WaitForSeconds(5.0f);
    //                }
    //                yield break;
    //            }
    //        }
    //    }
    //    yield return ac;
    //    //
    //    if (_info.onProgressUpdate != null)
    //    {
    //        _info.onProgressUpdate(ac.progress);
    //    }


    //    AssetBundle ab = ac.assetBundle;
    //    if (ab != null)
    //    {

    //        UnityEngine.Object[] objList = ab.LoadAllAssets();
    //        if (objList.Length > 0)
    //        {
    //            obj = objList[0];
    //        }
    //        ab.Unload(false);
    //        ab = null;
    //    }

    //    cache[_info.shortURL] = new WWWCache(url, obj, _info.isStatic);
    //    processingDownloads.Remove(_info.shortURL);
    //    --downloadThreads;
    //    if (_info.onComplete != null)
    //    {
    //        _info.onComplete(obj, EResult.Success);
    //    }
    //}










    IEnumerator Download(DownloadInfo _info)
    {
        string url = baseURL + _info.shortURL;
        // NGUIDebug.Log("www--"+url);
        WWW www = null;
        www = new WWW(url);

        ++downloadThreads;
        float timer = 0.0f;
        while (www.isDone == false)
        {
            //检查报错
            if (www.error != null)
            {
                DownloadFailed(_info, www);
                www.Dispose();
                www = null;
                yield break;
            }

            // 检查是否需要取消本次加载
            if (_info.onComplete == null)
            {
                www.threadPriority = ThreadPriority.Low;
                float timer2 = 0.0f;
                while (timer2 <= 5.0f && www.progress <= 0.1f)
                {
                    yield return null;
                    if (_info.onComplete != null)
                    {
                        break;
                    }
                    timer2 += Time.deltaTime;
                }

                //限制5秒加载时间，超时则直接回调
                if (_info.onComplete == null)
                {
                    DownloadCancelled(_info, www);
                    www.Dispose();
                    www = null;
                    yield break;
                }
            }

            //更新进度
            if (_info.onProgressUpdate != null)
            {
                float progress = www.progress;
                _info.onProgressUpdate(progress);
            }
            yield return null;

            // 检查是否超时
            timer += Time.deltaTime;
            if (_info.timeout > 0.0f)
            {
                if (timer >= _info.timeout)
                {
                    www.threadPriority = ThreadPriority.Low;
                    while (www.progress <= 0.1f)
                    {
                        yield return new WaitForSeconds(5.0f);
                    }

                    DownloadTimeout(_info, www);
                    www.Dispose();
                    www = null;
                    yield break;
                }
            }
        }
        yield return www;

        //检查是否有错误
        if (www.error != null)
        {
            DownloadFailed(_info, www);
            www = null;
            yield break;
        }

        //
        if (_info.onProgressUpdate != null)
        {
            _info.onProgressUpdate(www.progress);
        }

        UnityEngine.Object obj = null;
        if (www.assetBundle != null)
        {
            UnityEngine.Object[] objList = www.assetBundle.LoadAllAssets();
            if (objList.Length > 0)
            {
                obj = objList[0];
            }
            www.assetBundle.Unload(false);
        }

        cache[_info.shortURL] = new WWWCache(url, obj, _info.isStatic);
        processingDownloads.Remove(_info.shortURL);
        --downloadThreads;
        if (_info.onComplete != null)
        {
            _info.onComplete(obj, EResult.Success);
        }
        www.Dispose();
        www = null;
    }


    /// <summary>
    /// 加载任务超时处理 by吴江
    /// </summary>
    /// <param name="_info"></param>
    /// <param name="_www"></param>
    protected void DownloadTimeout(DownloadInfo _info, WWW _www)
    {
        Debug.LogError("加载 " + _info.shortURL + ", 超时.");
        if (_info.onComplete != null)
            _info.onComplete(null, EResult.TimeOut);

        processingDownloads.Remove(_info.shortURL);
        --downloadThreads;
    }


    /// <summary>
    /// 加载任务失败处理 by吴江
    /// </summary>
    /// <param name="_info"></param>
    /// <param name="_www"></param>
    protected void DownloadFailed(DownloadInfo _info, WWW _www)
    {
        if (_info.retry > 0)
        {
            _info.retry -= 1;
            pendingDownloads.Enqueue(_info);
            --downloadThreads;
            return;
        }

        Debug.LogError(_www.url + "加载失败 , " + _www.error);
        if (_info.onComplete != null)
            _info.onComplete(null, EResult.Failed);

        processingDownloads.Remove(_info.shortURL);
        --downloadThreads;
    }


    /// <summary>
    /// 加载任务取消处理 by吴江
    /// </summary>
    /// <param name="_info"></param>
    /// <param name="_www"></param>
    protected void DownloadCancelled(DownloadInfo _info, WWW _www)
    {
       // Debug.LogInternal("取消加载： " + _info.shortURL);
        processingDownloads.Remove(_info.shortURL);
        --downloadThreads;
    }


    /// <summary>
    /// 取消加载任务 by吴江
    /// </summary>
    /// <param name="_shortURL"></param>
    public void CancelDownload(string _shortURL)
    {
        if (processingDownloads.ContainsKey(_shortURL))
        {
            DownloadInfo info = processingDownloads[_shortURL];
            info.onComplete = null;
            info.onProgressUpdate = null;
        }
    }

    /// <summary>
    /// 取消加载 by吴江
    /// </summary>
    /// <param name="_id"></param>
    public void CancelDownload(DownloadID _id)
    {
        if (_id == null)
            return;

        if (processingDownloads.ContainsKey(_id.shortURL))
        {
            DownloadInfo info = processingDownloads[_id.shortURL];
            if (info.onComplete != null)
                info.onComplete -= _id.onComplete;
            if (_id.onProgressUpdate != null)
                info.onProgressUpdate -= _id.onProgressUpdate;
        }
    }


    /// <summary>
    /// 加载指定类型 by吴江
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_shortURL"></param>
    /// <param name="_onComplete"></param>
    /// <param name="_async"></param>
    /// <param name="_onDownloadProgressUpdate"></param>
    /// <param name="_onLoadAsyncProgressUpdate"></param>
    /// <returns></returns>
    public DownloadID LoadAsset<T>(string _shortURL,
                                 System.Action<T, EResult> _onComplete,
                                 bool _async = false,
                                 System.Action<float> _onDownloadProgressUpdate = null,
                                 System.Action<float> _onLoadAsyncProgressUpdate = null) where T : UnityEngine.Object
    {
#if !UNITYPRO
        _async = false;
#endif

        return LoadAsset<T>(_shortURL,
                              "", // that means you are using the mainAsset
                              _onComplete,
                              _async,
                              _onDownloadProgressUpdate,
                              _onLoadAsyncProgressUpdate);
    }


    /// <summary>
    /// 加载指定类型 by吴江
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_shortURL"></param>
    /// <param name="_name"></param>
    /// <param name="_onComplete"></param>
    /// <param name="_async"></param>
    /// <param name="_onDownloadProgressUpdate"></param>
    /// <param name="_onLoadAsyncProgressUpdate"></param>
    /// <param name="_logErrorIfFailed"></param>
    /// <returns></returns>
    public DownloadID LoadAsset<T>(string _shortURL,
                                 string _name,
                                 System.Action<T, EResult> _onComplete,
                                 bool _async = false,
                                 System.Action<float> _onDownloadProgressUpdate = null,
                                 System.Action<float> _onLoadAsyncProgressUpdate = null,
                                 bool _logErrorIfFailed = true) where T : UnityEngine.Object
    {
#if !UNITYPRO
        _async = false;
#endif

        DownloadID id = Download(_shortURL,
                                   delegate(UnityEngine.Object _obj, EResult _result)
                                   {
                                       if (_result == EResult.Success)
                                       {
                                           if (_obj == null)
                                           {
                                               if (_logErrorIfFailed)
                                                   Debug.LogError("Failed to load assetBundle from " + _shortURL);
                                               _onComplete(null, EResult.Failed);
                                           }
                                           else
                                           {
                                               if (_name == "")
                                               {
                                                   T asset = _obj as T;
                                                   if (asset)
                                                   {
                                                       _onComplete(asset, EResult.Success);
                                                   }
                                                   else
                                                   {
                                                       if (_logErrorIfFailed)
                                                           Debug.LogError("Failed to load " + _name + " from " + _shortURL);
                                                       _onComplete(asset, EResult.NotFound);
                                                   }
                                               }
                                               else
                                               {
                                                   if (_async)
                                                   {
                                                       Debug.LogError("未启用异步加载！");
                                                       //StartCoroutine(LoadAssetAsync<T>(_objs.assetBundle,
                                                       //                                    _shortURL,
                                                       //                                    _name,
                                                       //                                    _onComplete,
                                                       //                                    _onLoadAsyncProgressUpdate));
                                                   }
                                                   else
                                                   {
                                                       T asset = _obj as T;
                                                       if (asset != null)
                                                       {
                                                           _onComplete(asset, EResult.Success);
                                                       }
                                                       else
                                                       {
                                                           if (_logErrorIfFailed)
                                                               Debug.LogError("Failed to load " + _name + " from " + _shortURL);
                                                           _onComplete(asset, EResult.NotFound);
                                                       }
                                                   }
                                               }
                                           }
                                       }
                                       else
                                       {
                                           _onComplete(null, _result);
                                       }
                                   },
                                   _onDownloadProgressUpdate);

        return id;
    }




    /// <summary>
    /// 异步加载 by吴江
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_assetBundle"></param>
    /// <param name="_shortURL"></param>
    /// <param name="_name"></param>
    /// <param name="_onComplete"></param>
    /// <param name="_onLoadAsyncProgressUpdate"></param>
    /// <returns></returns>
    protected IEnumerator LoadAssetAsync<T>(AssetBundle _assetBundle,
                                          string _shortURL,
                                          string _name,
                                          System.Action<T, EResult> _onComplete,
                                          System.Action<float> _onLoadAsyncProgressUpdate = null) where T :UnityEngine.Object
    {
        AssetBundleRequest request = _assetBundle.LoadAllAssetsAsync(typeof(T));
        if (_onLoadAsyncProgressUpdate == null)
            yield return request;
        else
        {
            while (request.isDone == false)
            {
                _onLoadAsyncProgressUpdate(request.progress);
                yield return null;
            }
            _onLoadAsyncProgressUpdate(request.progress);
        }

        T asset = request.asset as T;
        if (asset != null)
        {
            _onComplete(asset, EResult.Success);
        }
        else
        {
            Debug.LogError("Failed to load " + _name + " from " + _shortURL);
            _onComplete(asset, EResult.NotFound);
        }
    }

    /// <summary>
    /// 开始加载任务 by吴江
    /// </summary>
    /// <param name="_shortURL"></param>
    /// <param name="_onComplete"></param>
    /// <param name="_onProgressUpdate"></param>
    /// <param name="isStatic"></param>
    /// <returns></returns>
    public DownloadID Download(string _shortURL,
                                 System.Action<UnityEngine.Object, EResult> _onComplete,
                                 System.Action<float> _onProgressUpdate = null,
                                    bool isStatic = false)
    {
        return DownloadInSeconds(_shortURL, -1.0f, _onComplete, _onProgressUpdate, isStatic);
    }

    /// <summary>
    /// 开始限时加载任务 by吴江
    /// </summary>
    /// <param name="_shortURL"></param>
    /// <param name="_timeout"></param>
    /// <param name="_onComplete"></param>
    /// <param name="_onProgressUpdate"></param>
    /// <param name="_isStatic"></param>
    /// <returns></returns>
    public DownloadID DownloadInSeconds(string _shortURL,
                                      float _timeout,
                                      System.Action<UnityEngine.Object, EResult> _onComplete,
                                      System.Action<float> _onProgressUpdate = null,
                                        bool _isStatic = false)
    {
        if (_onComplete == null)
        {
            Debug.LogError("The _onComplete can not be null");
            return null;
        }

        //
        DownloadInfo info = null;

        //如果我们要的资源已经加载过了，则直接取（TO DO：这里要对www进行unLoad，目前没找到合适的办法）by吴江
        if (processingDownloads.TryGetValue(_shortURL, out info))
        {
            if (info.timeout > 0.0f)
            {
                if (_timeout <= 0.0f || info.timeout < _timeout)
                {
                    info.timeout = _timeout;
                }
            }

            info.onComplete += _onComplete;
            if (_onProgressUpdate != null)
                info.onProgressUpdate += _onProgressUpdate;
            return new DownloadID(_shortURL, _onComplete, _onProgressUpdate, info.isStatic);
        }
        else
        {
            WWWCache wwwCache;
            if (cache.TryGetValue(_shortURL, out wwwCache))
            {
                UnityEngine.Object obj = wwwCache.obj;
                if (obj == null)
                {
                    Debug.LogError("wwwcatch缓存中存在,但实际对象已经被销毁 , " + _shortURL);
                }
                _onComplete(obj, EResult.Success);
                return null;
            }
        }

        //
        info = new DownloadInfo(_shortURL, _timeout, _onComplete, _onProgressUpdate, _isStatic);
        processingDownloads[_shortURL] = info;

        //检查加载数量是否已经越界 如果是，则将任务加入到等待队列中
        if (downloadThreads >= maxDownloads)
        {
            // Debug.LogInternal ( "pending for downloads " + _shortURL );
            pendingDownloads.Enqueue(info);
            return new DownloadID(_shortURL, _onComplete, _onProgressUpdate, _isStatic);
        }

        //以上条件都不满足，那么才下载
        StartCoroutine(Download(info));
        return new DownloadID(_shortURL, _onComplete, _onProgressUpdate, _isStatic);
    }


    /// <summary>
    /// 更新缓存，把不需要的卸载，需要的留下 by吴江
    /// </summary>
    /// <param name="needs"></param>
    /// <returns></returns>
    public List<string> UpdateCache(List<string> needs)
    {
        //unload不需要的
        List<string> _set = new List<string>();
        foreach (string url in cache.Keys)
        {
            if (!needs.Contains(url) && !cache[url].isStatic)
                _set.Add(url);
        }
        foreach (string urlDel in _set)
        {
            UnloadUrl(urlDel);
        }

        //从需求列表中剔除已经存在的
        _set = new List<string>();
        for (int i = 0; i < needs.Count; i++)
        {
             if (cache.ContainsKey(needs[i]))
            {
                _set.Add(needs[i]);
            }
        }
        for (int i = 0; i < _set.Count; i++)
        {
            if (needs.Contains(_set[i]))
            {
                needs.Remove(_set[i]);
            }
        }
        return needs;
    }


    /// <summary>
    /// 卸载资源 by吴江
    /// </summary>
    /// <param name="url"></param>
    public void UnloadUrl(string url)
    {
        WWWCache wwwCache;
        lock (cache)
        {
            if (cache.TryGetValue(url, out wwwCache))
            {
                DestroyImmediate(wwwCache.obj, true);
                wwwCache.obj = null;
                cache.Remove(url);
            }
        }
    }


    /// <summary>
    /// 获取一个特效实例（非源） by吴江
    /// </summary>
    /// <param name="_effctName"></param>
    /// <param name="_callback"></param>
    /// <param name="_active"></param>
    public static void GetEffectInstance(string _effctName, Action<GameObject> _callback, bool _active = true)
    {
        EffectLoadUtil.instance.GetSingleEffectGameObj(_effctName, _callback, _active);
    }

    /// <summary>
    /// 获取特效源资源（除非非常了解这个接口，否则建议建议使用GetEffectInstance） by吴江
    /// </summary>
    /// <param name="_effctName"></param>
    /// <param name="_callback"></param>
    public static void GetEeffctAssetObject(string _effctName, Action<GameObject> _callback)
    {
        EffectLoadUtil.instance.GetSingleEffect(_effctName, _callback);
    }
    
    public void StartLoadScriptsConfig(string assetName,Action<string> _finish)
    {
        StartCoroutine(LoadScriptsConfig(assetName,_finish));
    }
    IEnumerator LoadScriptsConfig(string assetName,Action<string> _finish)
    {
        string scriptsConfig = string.Empty;
        string path = AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + assetName;
        Debug.Log("path:" + path);
        WWW www = new WWW(path);
        while (www == null || !www.isDone)
        {
            yield return new WaitForFixedUpdate();
        }
        if (www != null && www.isDone)
        {
            scriptsConfig = www.text;
            Debug.Log("text:" + www.text);
        }
        else
        {
            Debug.LogError("资源加载失败:" + path);
        }
        if (_finish != null) _finish(scriptsConfig);
        www.Dispose();
        www = null;
        yield break;
    }
}
