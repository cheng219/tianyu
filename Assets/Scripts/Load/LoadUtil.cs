///////////////////////////////////////////////////////////////////////////////////////////
//作者：吴江
//最后修改时间：2015/3/13
//脚本描述：用来负责加载场景和管理场景资源的类。
///////////////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class LoadUtil : MonoBehaviour
{
    public class skill_data
    {
        public int skill;
        public int lev;
    }

    public enum ShaderPropertyType
    {
        Color = 0,
        Vector = 1,
        Float = 2,
        Range = 3,
        TexEnv = 4,
    }



    #region 数据

    protected string mainPath = string.Empty;
    public string MainPath
    {
        get
        {
            return mainPath;
        }

    }


    /// <summary>
    /// 上次场景加载的所有特效物体实例。游戏中要用到的特效都应该从这个数据中取实例去拷贝。因为asset中的资源是没有完成资源关联的。（而且在实例完成以后，asset的资源马上被卸载）
    /// </summary>
    protected Dictionary<string, GameObject> finalObjDic = new Dictionary<string, GameObject>();

    /// <summary>
    /// 本次所加载的资源总数
    /// </summary>
    protected int resourcesTotalCount = 0;
    // <summary>
    /// 本次所加载的资源总数
    /// </summary>
    public int ResourcesTotalCount
    {
        get { return resourcesTotalCount; }
        set
        {
            if (resourcesTotalCount != value)
            {
                resourcesTotalCount = value;
                if (LoadingUpdate != null) LoadingUpdate();
            }
        }
    }

    /// <summary>
    /// 本次加载的当前已经完成的资源总数
    /// </summary>
    protected int resourcesCurCount = 0;
    /// <summary>
    /// 本次加载的当前已经完成的资源总数
    /// </summary>
    public int ResourcesCurCount
    {
        get { return resourcesCurCount; }
        set
        {
            if (resourcesCurCount != value)
            {
                resourcesCurCount = value;
                if (LoadingUpdate != null)
                {
                    LoadingUpdate();
                }
            }
        }
    }


    protected int progressDelay = 10000;

    /// <summary>
    /// 加载进度变化的事件
    /// </summary>
    public Action LoadingUpdate;

    /// <summary>
    /// 加载事项全部完成的事件
    /// </summary>
    public Action complete;

    /// <summary>
    /// 上次加载所保留的所有特效配置数据，用来在下一次加载时比对卸载
    /// </summary>
    protected List<SceneConfigData> lastConfigList = new List<SceneConfigData>();

    public SceneConfigData LastConfigList
    {
        get
        {
            return MergeNeed(lastConfigList);
        }
    }


    protected List<SceneConfigData> curConfigList = new List<SceneConfigData>();


    protected SceneConfigData needLoadData;

    protected SceneConfigData needDeleteData;

    #region 资源主对象
    protected List<string> needLoadList = new List<string>();

    protected List<string> needDeleteList = new List<string>();


    /// <summary>
    /// 配置文件资源缓存
    /// </summary>
    protected Dictionary<string, UnityEngine.Object> configRefDic = new Dictionary<string, UnityEngine.Object>();

    /// <summary>
    /// 场景/预制的资源缓存，在游戏中实例化后立即卸载，之后的拷贝由内存中的实例拷贝。
    /// </summary>
    protected Dictionary<string, WWW> prefabAssetBundleRefDic = new Dictionary<string, WWW>();
    #endregion


    #region log
    /// <summary>
    /// 是否输出加载的时间记录
    /// </summary>
    public bool log = false;

    #endregion
    #endregion


    public virtual void ResetProgress()
    {
        resourcesCurCount = 0;
        resourcesTotalCount = 0;
    }


    protected IEnumerator LoadConfig(string _mainPath, List<string> _wholeNeedNames, Action _callBack)
    {
        //NGUIDebug.Log("开始LoadConfig");
        curConfigList.Clear();

        //通过跟上一个场景比较，得到本次需要加载的特效名称列表，和需要删除的特效名称列表
        CompareMain(_wholeNeedNames, lastConfigList, ref needLoadList);

        //个数+配置文件个数
        ResourcesTotalCount += needLoadList.Count > 0 ? (needLoadList.Count * 2 + progressDelay) : 0;

        for (int i = 0; i < needLoadList.Count; i++)
        {
            string ConfigPath = _mainPath + "config/" + needLoadList[i] + ".assetbundle";
            if (!isFileExist(ConfigPath))
            {
                needLoadList.RemoveAt(i);
                i--;
            }
            else
            {
                // NGUIDebug.Log("yyyyyyyyyyyyyyyyyyy ConfigPath=" + ConfigPath);
            }
        }
        //根据列表，下载配置文件，存入缓存列表
        for (int i = 0; i < needLoadList.Count; i++)
        {
             DownloadConfig(_mainPath, needLoadList, i);
        }
       // DownloadConfig(_mainPath, needLoadList, 0);


        //等待下载结束
        while (configPendings > 0) yield return new WaitForFixedUpdate();

        //确保本次所需要的所有特效配置文件都已经在缓存中
        for (int i = 0; i < _wholeNeedNames.Count; i++)
        {
             if (configRefDic.ContainsKey(_wholeNeedNames[i]))
            {
                SceneConfigData curSceneConfigData = configRefDic[_wholeNeedNames[i]] as SceneConfigData;
                curConfigList.Add(curSceneConfigData);
            }
            else
            {
                Debug.LogError("can't find config data that names " + _wholeNeedNames[i]);
                // NGUIDebug.Log("can't find config data that names " + item);
            }
        }

        if (_callBack != null) _callBack();
    }

    /// <summary>
    /// 这个值只用于场景加载,场景加载成功后,再绑定场景资源.
    /// </summary>
    protected bool canAsyncBind = false;

    public IEnumerator LoadResources(string _mainPath, List<string> _wholeNeedNames, Action _callBack, bool _asyncBind = false)
    {
        if (_asyncBind)
        {
            canAsyncBind = false;
        }
        if (needLoadList.Count == 0)//如果是相同场景
        {
            foreach (var item in _wholeNeedNames)
            {
                InitFinalObject(item);
            }
        }
        else
        {
            if (curConfigList == null || curConfigList.Count == 0)
            {
                Debug.LogError("本次资源加载任务没有找到配置文件，直接跳过！");
            }
            else
            {
                if (needLoadData != null)
                {
                    DestroyImmediate(needLoadData, true);
                    needLoadData = null;
                }
                CompareChilds(curConfigList, ref needLoadData);
                ResourcesTotalCount += (needLoadData.materialAssetList.Count + needLoadData.materialDataList.Count + needLoadData.textureDataList.Count
                    + needLoadData.shaderDataList.Count - progressDelay);
                ResourcesCurCount = needLoadList.Count;//配置文件已经下载完成
            }

        }
        if (needLoadData != null)
        {
            for (int i = 0; i < needLoadData.textureDataList.Count; i++)
            {
                DownloadTexture(_mainPath, needLoadData.textureDataList, i);
            }
            while (texturePendings > 0)
            {
                yield return new WaitForFixedUpdate();
            }


            for (int i = 0; i < needLoadData.materialDataList.Count; i++)
            {
                DownloadMaterial(_mainPath, needLoadData.materialDataList, i);
            }
            while (materialPendings > 0)
            {
                yield return new WaitForFixedUpdate();
            }

            BindMaterialDepend(MergeMaterialAssetList(curConfigList));


            while (finalObjInitPendings > 0)
            {
                yield return new WaitForFixedUpdate();
            }
        }
        if (needLoadList.Count > 0)
        {
            for (int i = 0; i < needLoadList.Count; i++)
            {
                DownloadFinalObject(_mainPath, needLoadList, i);
            }
           // DownloadFinalObject(_mainPath, needLoadList, 0);
            while (finalObjectPendings > 0)
            {
                yield return new WaitForFixedUpdate();
            }
        }
        if (!_asyncBind)
        {
            TryBindSceneRelation(curConfigList);

            //清理主对象缓存
            DeleteMainAssetBundle(_wholeNeedNames);



            lastConfigList.Clear();
            foreach (var item in curConfigList)
            {
                lastConfigList.Add(item);
            }

            DestroyImmediate(needLoadData, true);
            needLoadData = null;
            curConfigList.Clear();


            if (complete != null) complete();

            if (_callBack != null) _callBack();
        }
        else
        {
            while (!canAsyncBind)
            {
                yield return new WaitForFixedUpdate();
            }
            //清理主对象缓存
            DeleteMainAssetBundle(_wholeNeedNames);

            lastConfigList.Clear();
            foreach (var item in curConfigList)
            {
                lastConfigList.Add(item);
            }

            DestroyImmediate(needLoadData, true);
            needLoadData = null;
            curConfigList.Clear();

            if (complete != null) complete();

            if (_callBack != null) _callBack();
        }


    }




    #region 新旧场景配置文件比较

    protected List<string> GetNeedLoadAsset(SceneConfigData _newData, AssetType _type)
    {
        List<string> need = new List<string>();
        switch (_type)
        {
            case AssetType.texture:
                for (int i = 0; i < _newData.textureDataList.Count; i++)
                {
                    string item = _newData.textureDataList[i];
                    if (!LoadCache.textureRefDic.ContainsKey(item) && !need.Contains(item)) need.Add(item);
                }
                break;
            case AssetType.material:
                for (int i = 0; i < _newData.materialDataList.Count; i++)
                {
                    string item = _newData.materialDataList[i];
                    if (!LoadCache.materialRefDic.ContainsKey(item) && !need.Contains(item))
                    {
                        need.Add(item);
                    }
                }
                break;
            default:
                break;
        }
        return need;
    }


    public static SceneConfigData MergeNeed(List<SceneConfigData> _data)
    {
        SceneConfigData newdata = SceneConfigData.CreateInstance<SceneConfigData>();
        newdata.Name = string.Empty;
        for (int i = 0; i < _data.Count; i++)
        {
            SceneConfigData item = _data[i];
            for (int j = 0; j < item.textureDataList.Count; j++)
            {
                if (!newdata.textureDataList.Contains(item.textureDataList[j]))
                {
                    newdata.textureDataList.Add(item.textureDataList[j]);
                }
            }
            for (int j = 0; j < item.shaderDataList.Count; j++)
            {
                if (!newdata.shaderDataList.Contains(item.shaderDataList[j]))
                {
                    newdata.shaderDataList.Add(item.shaderDataList[j]);
                }
            }
            for (int j = 0; j < item.materialDataList.Count; j++)
            {
                if (!newdata.materialDataList.Contains(item.materialDataList[j]))
                {
                    newdata.materialDataList.Add(item.materialDataList[j]);
                }
            }
        }
        return newdata;

    }



    protected void CompareMain(List<string> _newData, List<SceneConfigData> _oldData, ref List<string> _needload)
    {
        _needload.Clear();

        List<string> oldList = new List<string>();
        if (_oldData != null)
        {
            for (int i = 0; i < _oldData.Count; i++)
            {
                SceneConfigData item = _oldData[i];
                if (!oldList.Contains(item.Name)) oldList.Add(item.Name);
            }
        }
        for (int i = 0; i < _newData.Count; i++)
        {
            string item = _newData[i];
            if (!oldList.Contains(item) && !_needload.Contains(item)) _needload.Add(item);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="_newData">新场景所需要的整体资源</param>
    /// <param name="_oldData">原场景已经有的整体资源</param>
    /// <returns>新场景还需要加载的资源表</returns>
    protected void CompareChilds(List<SceneConfigData> _newData, ref SceneConfigData _needload)
    {
        SceneConfigData mergedNewData = MergeNeed(_newData);
        _needload = GetNeedLoad(mergedNewData);

        DestroyImmediate(mergedNewData, true);
        mergedNewData = null;
    }



    /// <summary>
    /// 获取需要新加载的资源配置表 by吴江
    /// </summary>
    /// <param name="_newData">新场景所需要的整体资源</param>
    /// <param name="_oldData">原场景已经有的整体资源</param>
    /// <returns>新场景还需要加载的资源表</returns>
    private SceneConfigData GetNeedLoad(SceneConfigData _newData)
    {
        SceneConfigData needLoadData = SceneConfigData.CreateInstance<SceneConfigData>();


        needLoadData.name = _newData.name;
        needLoadData.instanceID = _newData.instanceID;

        //获取旧场景中不存在的新资源
        foreach (var newData in _newData.textureDataList)
        {
            if (!LoadCache.HasLoaded(newData, AssetType.texture) && !needLoadData.textureDataList.Contains(newData))
            {
                needLoadData.textureDataList.Add(newData);
            }
        }
        foreach (var newData in _newData.shaderDataList)
        {
            if (!LoadCache.HasLoaded(newData, AssetType.shader) && !needLoadData.shaderDataList.Contains(newData))
            {
                needLoadData.shaderDataList.Add(newData);
            }
        }
        foreach (var newData in _newData.materialDataList)
        {
            if (!LoadCache.HasLoaded(newData, AssetType.material) && !needLoadData.materialDataList.Contains(newData))
            {
                needLoadData.materialDataList.Add(newData);
            }
        }
        return needLoadData;
    }


    /// <summary>
    /// 获取需要删除的资源配置表 by吴江
    /// </summary>
    /// <param name="_newData">新场景所需要的整体资源</param>
    /// <param name="_oldData">原场景已经有的整体资源</param>
    /// <returns>新场景还需要加载的资源表</returns>
    public static void UnLoadUseness(List<SceneConfigData> _newData)
    {
        List<string> unloadTexList = new List<string>();
        List<string> unloadMatList = new List<string>();

        SceneConfigData allNeed = MergeNeed(_newData);
        foreach (var item in LoadCache.textureRefDic.Keys)
        {
            if (!allNeed.textureDataList.Contains(item)) unloadTexList.Add(item);
        }

        foreach (var item in LoadCache.materialRefDic.Keys)
        {
            if (!allNeed.materialDataList.Contains(item)) unloadMatList.Add(item);
        }


        for (int i = 0; i < unloadTexList.Count; i++)
        {
            LoadCache.UnLoad(unloadTexList[i], AssetType.texture);
        }
        for (int i = 0; i < unloadMatList.Count; i++)
        {
            LoadCache.UnLoad(unloadMatList[i], AssetType.material);
        }
    }



    protected virtual void DeleteMainAssetBundle(List<string> _loadList)
    {
        List<string> needdelete = new List<string>();
        foreach (var item in configRefDic.Keys)
        {
            if (!_loadList.Contains(item)) needdelete.Add(item);
        }
        List<SceneConfigData> datalist = new List<SceneConfigData>();
        for (int i = 0; i < needdelete.Count; i++)
        {
            for (int j = 0; j < lastConfigList.Count; j++)
            {
                if (lastConfigList[j].Name == needdelete[i])
                {
                    datalist.Add(lastConfigList[j]);
                }
            }
        }


        needdelete.Clear();
        foreach (var item in prefabAssetBundleRefDic.Keys)
        {
            if (!_loadList.Contains(item)) needdelete.Add(item);
        }
        for (int i = 0; i < needdelete.Count; i++)
        {
            
            string item = needdelete[i];
            if (prefabAssetBundleRefDic[item] != null && prefabAssetBundleRefDic[item].assetBundle != null)
            {
                prefabAssetBundleRefDic[item].assetBundle.Unload(true);
            }
            prefabAssetBundleRefDic.Remove(item);
            if (finalObjDic.ContainsKey(item))
            {
                DestroyImmediate(finalObjDic[item],true);
                finalObjDic.Remove(item);
            }
        }

    }
    #endregion

    #region 配置文件加载


    public static bool isFileExist(string path)
    {

        //        if (path.StartsWith("file://"))
        //        {
        //            path = path.Substring(7);
        //
        //        }
        //  Debug.Log("isFileExist " + path);
        return PlatformPathMng.isFileExist(path);
    }

    protected int configPendings = 0;
    protected List<string> waitConfig = new List<string>();
    protected IEnumerator downloadConfig(string _mainPath, List<string> _list, int _index, Action<string, List<string>, int> _moveNext)
    {
        if (_list.Count <= _index) yield break;
        string name = _list[_index];
        //如果缓存列表中已经有，则不再重复下载
        if (configRefDic.ContainsKey(name) || waitConfig.Contains(name))
        {
           // if (_moveNext != null) _moveNext(_mainPath, _list, ++_index);
            yield break;
        }
        else
        {
            configPendings++;
            string ConfigPath = _mainPath + "config/" + name + ".assetbundle";
            if (!isFileExist(ConfigPath))
            {

                configPendings--;
                Debug.LogWarning("File not Exists ConfigPath=" + ConfigPath);
                if (_moveNext != null) _moveNext(_mainPath, _list, ++_index);
                yield break;
            }
            waitConfig.Add(name);
            using (WWW www = new WWW(ConfigPath))
            {
                yield return www;
                if (www != null && www.isDone)
                {
                    waitConfig.Remove(name);
                    configPendings--;
                    if (www.assetBundle != null)
                    {
                        SceneConfigData data = null;
                        try
                        {
                            data = www.assetBundle.mainAsset as SceneConfigData;
                        }
                        catch
                        {
                            Debug.LogError(ConfigPath + "数据装箱转换出错！");
                        }
                        if (data != null)
                        {
                            data.sceneAsset.gameObjectRelationship.InitData();
                            if (configRefDic.ContainsKey(name))
                            {
                                DestroyImmediate(configRefDic[name],true);
                                configRefDic[name] = data;
                            }
                            else
                            {
                                configRefDic.Add(name, data);
                            }
                        }
                        else
                        {
                            Debug.Log("www.assetBundle.mainAsset is null");
                        }
                        www.assetBundle.Unload(false);
                    }
                    else
                    {
                        Debug.Log("load false");
                    }
                    if (_moveNext != null)
                    {
                        _moveNext(_mainPath, _list, ++_index);
                    }
                    else
                    {
                    }
                }
            }


        }


    }

    /// <summary>
    /// 驱动配置加载进程的递归函数
    /// </summary>
    /// <param name="_mainPath">资源主路径</param>
    /// <param name="_list">资源列表</param>
    /// <param name="_index">当前进度</param>
    protected void DownloadConfig(string _mainPath, List<string> _list, int _index)
    {
        if (_list.Count > _index)
        {
            ResourcesCurCount++;
            StartCoroutine(downloadConfig(_mainPath, _list, _index, DownloadConfig));
        }
    }
    #endregion

    #region 材质加载

    /// <summary>
    /// 材质加载的进程数量
    /// </summary>
    protected int materialPendings = 0;

    /// <summary>
    /// 驱动材质加载进程的递归函数
    /// </summary>
    /// <param name="_mainPath">资源主路径</param>
    /// <param name="_list">资源列表</param>
    /// <param name="_index">当前进度</param>
    protected void DownloadMaterial(string _mainPath, List<string> _list, int _index)
    {
        if (_list.Count > _index)
        {
            string name = _list[_index];
            //如果缓存列表中已经有，则不再重复下载
            if (LoadCache.HasLoaded(name, AssetType.material) || LoadCache.waitMaterial.Contains(name))
            {
                return;
            }
            else
            {
                materialPendings++;
                LoadCache.waitMaterial.Add(name);
                string Path = _mainPath + name + ".material";
                AssetMng.instance.LoadAsset<Material>(Path, (x, y) =>
                {
                    materialPendings--;
                    LoadCache.waitMaterial.Remove(name);
                    LoadCache.materialRefDic[name] = x;
                });
            }
        }
    }
    #endregion

    #region 贴图加载
    /// <summary>
    /// 贴图加载的进程数量
    /// </summary>
    protected int texturePendings = 0;

    /// <summary>
    /// 驱动贴图加载进程的递归函数
    /// </summary>
    /// <param name="_mainPath">资源主路径</param>
    /// <param name="_list">资源列表</param>
    /// <param name="_index">当前进度</param>
    protected void DownloadTexture(string _mainPath, List<string> _list, int _index)
    {
        if (_list.Count > _index)
        {
            string name = _list[_index];
            //如果缓存列表中已经有，则不再重复下载
            if (LoadCache.HasLoaded(name, AssetType.texture) || LoadCache.waitTexture.Contains(name))
            {
                return;
            }
            else
            {
                texturePendings++;
                LoadCache.waitTexture.Add(name);
                string Path = _mainPath + name + ".texture";
                AssetMng.instance.LoadAsset<Texture>(Path, (x, y) =>
                {
                    texturePendings--;
                    LoadCache.waitTexture.Remove(name);
                    LoadCache.textureRefDic[name] = x;
                });
            }
        }
    }
    #endregion

    #region 最终物体加载
    /// <summary>
    /// 场景加载的进程数量
    /// </summary>
    protected int finalObjectPendings = 0;
    protected List<string> waitFinalObject = new List<string>();
    /// <summary>
    /// 场景加载进程
    /// </summary>
    /// <param name="_mainPath">资源主路径</param>
    /// <param name="_name">场景名称</param>
    /// <param name="_data">场景信息</param>
    /// <returns></returns>
    protected IEnumerator downloadFinalObject(string _mainPath, List<string> _list, int _index, Action<string, List<string>, int> _moveNext)
    {
        if (_list.Count <= _index) yield break;
        string name = _list[_index];
        //如果缓存列表中已经有，则不再重复下载
        if (prefabAssetBundleRefDic.ContainsKey(name) || waitFinalObject.Contains(name))
        {
            if (!waitFinalObject.Contains(name))
            {
                InitFinalObject(name);
            }
            //if (_moveNext != null) _moveNext(_mainPath, _list, ++_index);
            yield break;
        }
        else
        {
            finalObjectPendings++;
            string scenePath = _mainPath + name + ".object";
            //NGUIDebug.Log("www-Scene-"+scenePath);
            if (!isFileExist(scenePath))
            {
                finalObjectPendings--;
                Debug.LogWarning("File not Exists scenePath=" + scenePath);
                if (_moveNext != null) _moveNext(_mainPath, _list,++_index);
                yield break;
            }
            waitFinalObject.Add(name);
            WWW www = new WWW(scenePath);
            while (www == null || !www.isDone)
            {
                yield return new WaitForFixedUpdate();
            }
            waitFinalObject.Remove(name);
            finalObjectPendings--;
            ResourcesCurCount++;
            if (www != null && www.isDone)
            {
                if (www.assetBundle != null)
                {
                    prefabAssetBundleRefDic[name] = www;
                    InitFinalObject(name);
                }
                else
                {
                    Debug.LogError("预制 " + name + " 资源为空!");
                }
                //if (_moveNext != null) _moveNext(_mainPath, _list, ++_index);
            }
            else
            {
                Debug.LogError("加载预制 " + name + " 失败！");
            }
        }
    }

    protected void DownloadFinalObject(string _mainPath, List<string> _list, int _index)
    {
        if (_list.Count > _index)
        {
            StartCoroutine(downloadFinalObject(_mainPath, _list, _index, DownloadFinalObject));
        }
    }


    public void DebugGameObjectRelation(GameObject _prefab, GameObjectRelationship _relationship)
    {
        if (_prefab == null || _relationship == null) return;//如果物体为空或关联性配置为空，返回
        if (_relationship.matNames.Count > 0)
        {
            int count = _relationship.matNames.Count;
            for (int i = 0; i < count; i++)//遍历关联性配置数据
            {
                Debug.logger.Log("mat = " + _relationship.matNames[i]);
            }
        }
        //对子物体进行递归
        if (_prefab.transform.childCount > 0 && _relationship.childRelationList.Count > 0)
        {
            int fixedCount = Mathf.Min(_prefab.transform.childCount, _relationship.childRelationList.Count);
            for (int i = 0; i < fixedCount; i++)
            {
                DebugGameObjectRelation(_prefab.transform.GetChild(i).gameObject, _relationship.childRelationList[i]);
            }
        }
    }


    /// <summary>
    /// 根据关联性配置组装物体
    /// </summary>
    /// <param name="_prefab"></param>
    /// <param name="_relationship"></param>
    public void ProcessGameObjectRelation(GameObject _prefab, GameObjectRelationship _relationship)
    {
        if (_prefab == null || _relationship == null) return;//如果物体为空或关联性配置为空，返回
        if (_relationship.matNames.Count > 0 && _prefab.GetComponent<Renderer>() != null)//如果关联性配置中数据为0或者该物体没有渲染组件，返回
        {
            int count = _relationship.matNames.Count;
            Material[] matArray = _prefab.GetComponent<Renderer>().sharedMaterials;
            for (int i = 0; i < count; i++)//遍历关联性配置数据
            {
                if (i >= matArray.Length)
                {
                    Debug.LogError(_prefab.name + "的配置文件记录的材质数量与实际材质数量不符合，请美术检查资源！");
                    break;
                }
                if (LoadCache.HasLoaded(_relationship.matNames[i], AssetType.material))//如果材质资源缓存中有关联性配置中需求的材质
                {
                    matArray[i] = LoadCache.GetMaterial(_relationship.matNames[i]);//materialAssetBundleRefDic[_relationship.matNames[i]].mainAsset as Material;
                    if (matArray[i] != null && matArray[i].shader != null)
                    {
                        Shader temp = Shader.Find(matArray[i].shader.name);
                        if (temp != null)
                            matArray[i].shader = temp;
                    }
                }
                else
                {
                    Debug.LogError(_prefab.name + " , can't find material that names " + _relationship.matNames[i] + " , please check it ! " + LoadCache.materialRefDic.Count);
                }
            }
            _prefab.GetComponent<Renderer>().sharedMaterials = matArray;
            //_prefab.SetActive(true);
        }
        //对子物体进行递归
        if (_prefab.transform.childCount > 0 && _relationship.childRelationList.Count > 0)
        {
            int fixedCount = Mathf.Min(_prefab.transform.childCount, _relationship.childRelationList.Count);
            for (int i = 0; i < fixedCount; i++)
            {
                ProcessGameObjectRelation(_prefab.transform.GetChild(i).gameObject, _relationship.childRelationList[i]);
            }
        }
    }

    protected void BindMaterialDepend(List<MaterialAsset> _materialAssetList)
    {
        foreach (var materialAsset in _materialAssetList)
        {
            string matName = materialAsset.matName.ToLower();
            if (!LoadCache.HasLoaded(matName, AssetType.material))
            {
                Debug.Log(matName + "找不到");
                continue;
            }
            MaterialRelationship rl = materialAsset.materialRelationship;

            Material mat = LoadCache.GetMaterial(matName);//materialAssetBundleRefDic[materialAsset.matName].mainAsset as Material;
            if (rl != null && mat.name.ToLower().Equals(rl.matName.ToLower()))
            {
                mat.shader = Shader.Find(rl.sdName);//GetShader(rl.sdName);//shaderAssetBundleRefDic[rl.sdName].mainAsset as Shader;
                foreach (var item in rl.propertyPairList)
                {
                    ShaderPropertyType type = (ShaderPropertyType)item.type;
                    switch (type)
                    {
                        case ShaderPropertyType.Color:
                            mat.SetColor(item.propertyName, item.color);
                            break;
                        case ShaderPropertyType.Vector:
                            mat.SetVector(item.propertyName, item.v4);
                            break;
                        case ShaderPropertyType.Float:
                        case ShaderPropertyType.Range:
                            mat.SetFloat(item.propertyName, item.value);
                            break;
                        case ShaderPropertyType.TexEnv:
                            if (LoadCache.HasLoaded(item.texName, AssetType.texture))
                            {
                                mat.SetTexture(item.propertyName, LoadCache.GetTexture(item.texName));//textureAssetBundleRefDic[item.texName].mainAsset as Texture);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
    #endregion


    /// <summary>
    /// 实例化的进程数量
    /// </summary>
    protected int finalObjInitPendings = 0;
    protected virtual void InitFinalObject(string _name)
    {
    }

    protected List<MaterialAsset> MergeMaterialAssetList(List<SceneConfigData> _configList)
    {
        Dictionary<string, MaterialAsset> dictionary = new Dictionary<string, MaterialAsset>();
        foreach (var item in _configList)
        {
            foreach (var matasset in item.materialAssetList)
            {
                string matName = matasset.matName.ToLower();
                if (!dictionary.ContainsKey(matName))
                {
                    dictionary[matName] = matasset;
                }
            }
        }
        List<MaterialAsset> list = new List<MaterialAsset>();
        foreach (var item in dictionary.Values)
        {
            list.Add(item);
        }
        return list;
    }

    protected virtual void TryBindSceneRelation(List<SceneConfigData> _data)
    {
    }


    public static GameObject SetActive(GameObject _obj, bool _active)
    {
        if (_obj == null) return null;
        _obj.SetActive(_active);
        //if (_obj.transform.childCount > 0)
        //{
        //    for (int i = 0; i < _obj.transform.childCount; i++)
        //    {
        //        SetActive(_obj.transform.GetChild(i).gameObject, _active);
        //    }
        //}
        return _obj;

    }

}
