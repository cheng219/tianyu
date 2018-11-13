//================================================================================
//作者：吴江
//日期：2015/12/16
//用途：声音资源加载组件
//================================================================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Object = UnityEngine.Object;
using System.IO;
using System;

public class SoundLoader : MonoBehaviour {


    /// <summary>
    /// 声音剪辑缓存
    /// </summary>
    public Dictionary<string, AudioClip> mySoundClipCache = new Dictionary<string, AudioClip>();
    /// <summary>
    /// 剔除开关
    /// </summary>
    public bool cullingActive = false;
    /// <summary>
    /// 剔除上限
    /// </summary>
    public int cullAbove = 40;

    void Update()
    {
        int count = this.m_kExecQueue.Count;
        int num = 0;//每帧从0开始计数
        int num2 = 1;//控制每帧添加的任务数 默认1个（适度修改）
        for (int i = 0; i < this.m_kLoadingQueue.Count; i++)
        {
            if (num >= num2)
            {
                break;
            }
            LoadingTask item = this.m_kLoadingQueue[i];
            this.m_kExecQueue.Add(item);
            num++;
        }
        //上面的for 中 加入了几个任务就从需要加载的队列中移除
        this.m_kLoadingQueue.RemoveRange(0, num);

        //循环新增任务  进行加载
        for (int j = count; j < this.m_kExecQueue.Count; j++)
        {
            StartCoroutine(DoLoad(this.m_kExecQueue[j]));
        }
    }


    #region 缓存操作
    /// <summary>
    /// 添加到缓存
    /// </summary>
    public void AddCache(string _soundName)
    {
        if (_soundName == "0" || _soundName == string.Empty) return;
        if (this.mySoundClipCache.ContainsKey(_soundName))
        {
            if (this.mySoundClipCache[_soundName] == null)
            {
                mySoundClipCache.Remove(_soundName);
            }
            else
            {
                return;
            }
        }
        AudioLoader audioLoader = new AudioLoader();
        string audioBundlePath = SoundLoader.GetAudioAssetPath(_soundName);
        audioLoader.kName = _soundName;
        RequestAsyncLoad(audioBundlePath, _soundName, audioLoader);
    }


    /// <summary>
    /// 添加到缓存
    /// </summary>
    public void AddCache(string kName, AudioClip kClip)
    {
        if (this.mySoundClipCache.ContainsKey(kName))
        {
            return;
        }
        this.mySoundClipCache.Add(kName, kClip);
    }

    /// <summary>
    /// 清除缓存
    /// </summary>
    public void ClearCache()
    {
        foreach (var item in this.mySoundClipCache)
        {
            string audioBundlePath = SoundLoader.GetAudioAssetPath(item.Key);
            UnloadAsset(audioBundlePath);
            if (item.Value != null)
                GameObject.Destroy(item.Value);
            cullingActive = false;
        }
        this.mySoundClipCache.Clear();
    }

    public void CheackAbove()
    {
        if (!this.cullingActive && this.mySoundClipCache.Count > this.cullAbove)
        {
            this.cullingActive = true;
            StartCoroutine("CullSoundCache");
        }
    }

    private int cullMaxPerPass = 3;
    private float cullDelay = 0.1f;
    /// <summary>
    /// 剔除缓存（全清除）  完善中
    /// </summary>
    IEnumerator CullSoundCache()
    {
        yield return new WaitForSeconds(cullDelay);
        while (mySoundClipCache.Count > cullAbove)
        {
            for (int i = 0; i < this.cullMaxPerPass; i++)
            {
                if (mySoundClipCache.Count > 0)
                {
                    List<string> temp = new List<string>(mySoundClipCache.Keys);
                    string audioBundlePath = GetAudioAssetPath(temp[0]);
                    UnloadAsset(audioBundlePath);
                    if (mySoundClipCache[temp[0]] != null)
                        Destroy(mySoundClipCache[temp[0]]);
                    mySoundClipCache.Remove(temp[0]);
                    temp.RemoveAt(0);
                }
            }
            yield return new WaitForSeconds(cullDelay);
        }
        cullingActive = false;
    }
    #endregion


    /// <summary>
    /// 异步加载声音自动播放
    /// </summary>
    public AudioAutoPlayLoader LoadSoundAsyncAutoPlay(string kClipName, float volumn_mostTimeNoUse, bool bIgnoreDuplicate, bool bIgnoreVolumeSetting, int playCount = 1)
    {
        AudioAutoPlayLoader callback = new AudioAutoPlayLoader();
        callback.kName = kClipName;
        callback.fVol = volumn_mostTimeNoUse;
        callback.bIgnoreDuplicate = bIgnoreDuplicate;
        callback.bIgnoreVolumeSetting = bIgnoreVolumeSetting;
        string audioBundlePath = GetAudioAssetPath(kClipName);
        RequestAsyncLoad(audioBundlePath, kClipName, callback);
        return callback;
    }

    #region 加载部分
    #region 数据 变量
    /// <summary>
    /// 声音路径分支
    /// </summary>
    public static readonly string kAudioPath = "Sound/";
    /// <summary>
    /// 正在加载的序列
    /// </summary>
    List<LoadingTask> m_kLoadingQueue = new List<LoadingTask>();

    /// <summary>
    ///执行队列
    /// </summary>
    List<LoadingTask> m_kExecQueue = new List<LoadingTask>();

    /// <summary>
    ///储存基本的Assetbundle 如：换装模型  基础图集 UI 等 （为以后预留）
    /// </summary>
    List<AssetBundle> m_kLoadedBundle = new List<AssetBundle>();

    /// <summary>
    /// 已加载的资源
    /// </summary>
    Dictionary<string, Object> m_kLoadedAsset = new Dictionary<string, Object>();

    /// <summary>
    /// 当前操作数
    /// </summary>
    int m_nCurrentBundleOp;


    #endregion

    /// <summary>
    /// 转换 the chs to ASCII.
    /// </summary>
    public static string ConvertChsToAscii(string kName)
    {
        if (string.IsNullOrEmpty(kName))
        {
            return string.Empty;
        }
        char[] separator = new char[]
		{
			'/'
		};
        string[] array = kName.Split(separator);
        StringBuilder stringBuilder = new StringBuilder();
        string[] array2 = array;
        for (int i = 0; i < array2.Length; i++)
        {
            string text = array2[i];
            char[] chars = text.ToCharArray();
            string value = UnicodeToHex(chars);
            stringBuilder.Append(value);
            stringBuilder.Append("/");
        }
        stringBuilder.Remove(stringBuilder.Length - 1, 1);
        return stringBuilder.ToString();
    }

    /// <summary>
    /// 获取声音路径
    /// </summary>
    public static string GetAudioAssetPath(string kName)
    {
        return GetAssetPath(kName, kAudioPath, true);
    }
    /// <summary>
    /// 获得资源路径（加载优先级别：外部文件原后缀>外部文件AB后缀>内部文件原后缀>内部文件AB后缀）
    /// </summary>

    public static string GetAssetPath(string kName, string kPath, bool bConvert)
    {
        if (kName == "0") return kName;
        //内部路径原后缀 textInsideOriginalExtension textInsideNormalExtension
        string textInsideOriginalExtension = GetAssetPathByPlatform(kPath, false, false) + (!bConvert ? kName : ConvertChsToAscii(kName));
        //内部路径标准后缀
        string textInsideNormalExtension = Path.ChangeExtension(textInsideOriginalExtension, "assetbundle");

        //外部路径原后缀
        string textExternalOriginalExtension = GetAssetPathByPlatform(kPath, false, true) + (!bConvert ? kName : ConvertChsToAscii(kName));
        //外部路径标准后缀
        string textExternalNormalExtension = Path.ChangeExtension(textExternalOriginalExtension, "assetbundle");

        string path = string.Empty;

        //加载优先级别：外部文件原后缀>外部文件AB后缀>内部文件原后缀>内部文件AB后缀

        if (File.Exists(textExternalOriginalExtension))
        {
            path = "file://" + textExternalOriginalExtension;
        }
        else if (File.Exists(textExternalNormalExtension))
        {
            path = "file://" + textExternalNormalExtension;
        }
        else if (File.Exists(textInsideOriginalExtension))
        {
            path = "file:///" + textInsideOriginalExtension;
        }
        else if (File.Exists(textInsideNormalExtension))
        {
            path = "file:///" + textInsideNormalExtension;
        }
        else
        {
            Debug.LogError("声音文件不存在,请检查：" + kName.ToString() + ",path:" + path);
            path = "";
        }
        return path;
    }

    /// <summary>
    /// 跨平台路径适配
    /// </summary>
    public static string GetAssetPathByPlatform(string kAssetPath, bool bCompress, bool bPersistPath)
    {
        string result = string.Empty;
        kAssetPath = PlatformPathMng.SubPathByPlatform(kAssetPath);
        if (bPersistPath)
            result = PlatformPathMng.GetFilePath(kAssetPath, AssetPathType.StreamingAssetsPath, "");
        else
            result = PlatformPathMng.GetFilePath(kAssetPath, AssetPathType.PersistentDataPath, "");
        return result;
    }

    /// <summary>
    ///处理文件夹（预留）
    /// </summary>
    public static void ProcessDirectory(string targetDirectory, string kName, ref string kFullPath)
    {
        string text = targetDirectory + "/" + kName;
        if (File.Exists(text))
        {
            kFullPath = text;
            return;
        }
        string[] directories = Directory.GetDirectories(targetDirectory);
        string[] array = directories;
        for (int i = 0; i < array.Length; i++)
        {
            string text2 = array[i];
            text = text2 + "/" + kName + ".png";
            if (File.Exists(text))
            {
                kFullPath = text;
                break;
            }
            text = text2 + "/" + kName + ".tga";
            if (File.Exists(text))
            {
                kFullPath = text;
                break;
            }
            ProcessDirectory(text2, kName, ref kFullPath);
        }
    }

    /// <summary>
    /// 转换到16进制
    /// </summary>
    public static string UnicodeToHex(char[] chars)
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < chars.Length; i++)
        {
            char c = chars[i];
            if (c < '\u0080')
            {
                stringBuilder.Append(c);
            }
            else
            {
                stringBuilder.Append(string.Format("{0:X4}", (int)c));
            }
        }
        return stringBuilder.ToString();
    }

    /// <summary>
    /// 把Assetbundle添加到已加载Assetbundle列表
    /// </summary>
    public void AddLoadedBaseBundle(AssetBundle kBundle)
    {
        if (kBundle == null)
        {
            return;
        }
        this.m_kLoadedBundle.Add(kBundle);
    }
    /// <summary>
    /// 清除已加载的Assetbundle
    /// </summary>
    public void ClearLoadedAsset()
    {
        AssetBundle item = null;
        foreach (AssetBundle current in this.m_kLoadedBundle)
        {
            if (!(current == null))
            {
                if (current.name.Contains("UIBaseBundle"))// UIBaseBundle 预留 
                {
                    item = current;
                }
                else
                {
                    current.Unload(true);
                }
            }
        }
        this.m_kLoadedBundle.Clear();
        this.m_kLoadedBundle.Add(item);
        this.m_kLoadedAsset.Clear();
    }

    public void DecConcurrentOpCount()
    {
        this.m_nCurrentBundleOp--;
    }

    public void IncConcurrentOpCount()
    {
        this.m_nCurrentBundleOp++;
    }

    /// <summary>
    /// 获取已加载资源
    /// </summary>
    public Object GetLoadedAsset(string kAssetName)
    {
        if (this.m_kLoadedAsset.ContainsKey(kAssetName))
        {
            return this.m_kLoadedAsset[kAssetName];
        }
        return null;
    }

    /// <summary>
    /// 获取已加载资源(assetbundle)
    /// </summary>
    public AssetBundle IsBaseBundleLoaded(string kAssetPath)
    {
        for (int i = this.m_kLoadedBundle.Count - 1; i >= 0; i--)
        {
            if (this.m_kLoadedBundle[i] == null)
            {
                this.m_kLoadedBundle.RemoveAt(i);
            }
            else
            {
                if (this.m_kLoadedBundle[i].name == kAssetPath)
                {
                    return this.m_kLoadedBundle[i];
                }
            }
        }
        return null;
    }

    /// <summary>
    ///某资源是否正在加载中
    /// </summary>
    public int IsLoading(string _assetName)
    {
        for (int i = 0; i < this.m_kLoadingQueue.Count; i++)
        {
            if (this.m_kLoadingQueue[i].kAssetPath == _assetName)
            {
                return i;
            }
        }
        return -1;
    }


    /// <summary>
    /// 为以后做优先级别预留
    /// </summary>
    private bool important(string kName)
    {
        return kName.Contains("Skill") || kName.Contains("Ability");
    }

    /// <summary>
    /// 请求异步加载
    /// </summary>
    public void RequestAsyncLoad(string assetPath, string kName, LoaderCallback kEventReceiver)
    {
        if (assetPath == null || assetPath == string.Empty || assetPath.Contains("xx"))
        {
            return;
        }
        int num = IsLoading(assetPath);
        if (num != -1)
        {
            this.m_kLoadingQueue[num].kEventReceivers.Add(kEventReceiver);
            return;
        }
        if (this.m_kLoadedAsset.ContainsKey(assetPath))
        {
            kEventReceiver.OnLoaded(this.m_kLoadedAsset[assetPath], true);
            return;
        }
        for (int i = 0; i < this.m_kExecQueue.Count; i++)
        {
            if (this.m_kExecQueue[i] != null && this.m_kExecQueue[i].kAssetPath == assetPath)
            {
                this.m_kExecQueue[i].kEventReceivers.Add(kEventReceiver);
                return;
            }
        }
        if (this.important(kName))
        {
            this.m_kLoadingQueue.Insert(0, new LoadingTask(assetPath, kName, kEventReceiver));
        }
        else
        {
            this.m_kLoadingQueue.Add(new LoadingTask(assetPath, kName, kEventReceiver));
        }
    }


    /// <summary>
    /// 卸载资源 通过对象
    /// </summary>
    public void UnloadAsset(GameObject kPrefab)
    {
        foreach (KeyValuePair<string, Object> current in this.m_kLoadedAsset)
        {
            if (kPrefab == current.Value)
            {
                Resources.UnloadAsset(kPrefab);
                this.m_kLoadedAsset.Remove(current.Key);
                break;
            }
        }
    }
    /// <summary>
    /// 卸载资源 通过名字
    /// </summary>
    public void UnloadAsset(string kAssetName)
    {
        if (this.m_kLoadedAsset.ContainsKey(kAssetName))
        {
            Destroy(this.m_kLoadedAsset[kAssetName]);
            this.m_kLoadedAsset.Remove(kAssetName);
        }
    }


    /// <summary>
    /// 加载协同
    /// </summary>
    IEnumerator DoLoad(LoadingTask kCurrentTask)
    {
        WWW www = new WWW(kCurrentTask.kAssetPath);
        yield return www;
        if (www.isDone)
        {
            //检查报错
            if (www.error != null)
            {
                GameSys.Log(www.error);
                www = null;
                yield break;
            }
            AssetBundle bundle = www.assetBundle;
            if (bundle != null)
            {
                kCurrentTask.OnLoaded(bundle.mainAsset, true);
                this.m_kLoadedAsset.Add(kCurrentTask.kAssetPath, bundle.mainAsset);
            }
            else
            {
                AudioClip audioClip = www.audioClip;
                if (audioClip != null)
                {
                    kCurrentTask.OnLoaded(audioClip, true);
                    this.m_kLoadedAsset.Add(kCurrentTask.kAssetPath, audioClip);
                }
            }
        }
        else
        {
            GameSys.Log("声音加载错误");
        }
        //加载完毕从执行序列中移除
        if (this.m_kExecQueue.Contains(kCurrentTask))
            this.m_kExecQueue.Remove(kCurrentTask);
    }

    /// <summary>
    /// Raises the destroy event.有点多余
    /// </summary>
    void OnDestroy()
    {
        StopAllCoroutines();
        CancelInvoke();
    }


    /// <summary>
    /// 加载任务对象类
    /// </summary>
    private class LoadingTask
    {
        public string kAssetPath;
        public string kMainName;
        public List<LoaderCallback> kEventReceivers = new List<LoaderCallback>();
        public LoadingTask(string kPath, string kMain, LoaderCallback kobj)
        {
            this.kMainName = kMain;
            this.kAssetPath = kPath;
            this.kEventReceivers.Add(kobj);
        }
        public void OnLoaded(Object kObj, bool bResult)
        {
            for (int i = 0; i < this.kEventReceivers.Count; i++)
            {
                this.kEventReceivers[i].OnLoaded(kObj, bResult);
            }
        }
    }
    #endregion
}



public class LoaderCallback
{
    /// <summary>
    /// 加载结果（对与错）
    /// </summary>
    public bool bResult;

    /// <summary>
    /// 是否加载完成
    /// </summary>
    public bool bLoaded;

    /// <summary>
    /// 加载完成 执行的方法
    /// </summary>
    public virtual void OnLoaded(Object kObj, bool bError)
    {
    }




}
