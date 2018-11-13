///////////////////////////////////////////////////////////////////////////////////////////
//作者：吴江
//最后修改时间：2015/3/13
//脚本描述：用来负责加载场景和管理场景资源的类。
///////////////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;


public class SceneLoadUtil : LoadUtil
{


    #region 数据
    public static SceneLoadUtil instance;
    #endregion


    void Awake()
    {
        instance = this;
        mainPath = AssetMng.GetPathWithoutExtension("scene/", AssetPathType.PersistentDataPath);
        if (transform.parent == null)
        {
            DontDestroyOnLoad(this);
        }
    }
    /// <summary>
    /// 开始加载配置数据
    /// </summary>
    /// <param name="_data"></param>
    /// <param name="_callBack"></param>
    public void StartLoadConfig(string _sceneName, int _sceneIndex, Action _callBack)
    {
    //    Debug.Log("开始加载地图配置！");
        ResourcesTotalCount = 0;
        ResourcesCurCount = 0;

        //获取本次所需要的所有名称
        List<string> wholeNeedNames = new List<string>();
        wholeNeedNames.Add(_sceneName);

        StartCoroutine(LoadConfig(mainPath, wholeNeedNames, _callBack));
    }

    public void StartLoadResources(string _sceneName, int _sceneIndex, Action _callBack)
    {
        string mainPath = AssetMng.GetPathWithoutExtension("scene/", AssetPathType.PersistentDataPath);

        //获取本次所需要的所有特效名称
        List<string> wholeNeedNames = new List<string>();

        SceneRef refData = ConfigMng.Instance.GetSceneRef(_sceneIndex);
        if (refData != null)
        {
            wholeNeedNames.Add(refData.replaceNavmesh);
        }

        wholeNeedNames.Add(_sceneName);


        StartCoroutine(LoadResources(mainPath, wholeNeedNames, _callBack, true));
    }


    protected override void InitFinalObject(string _name)
    {
        if (_name.Contains("nav")) return;
        base.InitFinalObject(_name);
        StartCoroutine(LoadingScene(_name));
    }

    protected List<string> lastSceneNameList = new List<string>();

    protected IEnumerator LoadingScene(string _name)
    {

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_name, LoadSceneMode.Additive);
        while (asyncOperation == null || !asyncOperation.isDone)
        {
            yield return new WaitForFixedUpdate();
        }
        lastSceneNameList.Add(_name);
        Scene scene = SceneManager.GetSceneByName(_name);
        SceneManager.SetActiveScene(scene);
        TryBindSceneRelation(curConfigList);
        canAsyncBind = true;
        //if (asyncOperation != null && asyncOperation.isDone)
        //{
        //    finalObjInitPendings--;
        //    //场景不能unload 因为存在重新进场景的情况 by吴江
        //    //if (prefabAssetBundleRefDic.ContainsKey(_name))
        //    //{
        //    //    prefabAssetBundleRefDic[_name].assetBundle.Unload(false);
        //    //}
        //}
    }


    protected override void TryBindSceneRelation(List<SceneConfigData> _data)
    {
        List<SceneConfigData> tempList = new List<SceneConfigData>(_data); //这里有可能对原来的列表进行操作，直接操作的话有可能数组越界，因此这里new一个列表进行遍历 by吴江
        foreach (var item in tempList)
        {
            TryBindSceneRelation(item);
        }
    }

    protected void TryBindSceneRelation(SceneConfigData _data)
    {
        if (_data == null || _data.sceneAsset == null || _data.sceneAsset.gameObjectRelationship == null)
        {
            Debug.LogError("The SceneConfigData has some problem , please check it !");
            return;
        }
        GameObject myScene = GameObject.Find(_data.sceneAsset.sceneName);
        if (myScene != null)
        {
            if (finalObjDic.ContainsKey(_data.sceneAsset.sceneName))
            {
                DestroyImmediate(finalObjDic[_data.sceneAsset.sceneName], true);
                finalObjDic.Remove(_data.sceneAsset.sceneName);
            }
            finalObjDic[_data.sceneAsset.sceneName] = myScene;
            SceneRoot sr = myScene.GetComponent<SceneRoot>();
            if (sr != null)
            {
                PlayGameStage s = GameCenter.curGameStage as PlayGameStage;
                if (s != null)
                {
                    s.SetSceneEffects(sr);
                }
            }
            ProcessGameObjectRelation(myScene, _data.sceneAsset.gameObjectRelationship);
        }
        else
        {
            Debug.LogError(_data.sceneAsset.sceneName  + " 场景对象为空!请美术检查资源名称以及配置文件！");
        }

    }


    #region navMesh
    public void UnLoadScene()
    {
        for (int i = 0; i < lastSceneNameList.Count; i++)
        {
            SceneManager.UnloadScene(lastSceneNameList[i]);
        }
        lastSceneNameList.Clear();
    }

        /// <summary>
    /// 加载掩码 by吴江
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <param name="_sceneIndex"></param>
    /// <param name="_callBack"></param>
    public void StartLoadMapConfig(string _sceneName, int _sceneIndex, Action _callBack)
    {
        SceneRef sceneRef = ConfigMng.Instance.GetSceneRef(_sceneIndex);
        if (sceneRef != null)
        {
            string mainPath = AssetMng.GetPathWithoutExtension("scene/", AssetPathType.PersistentDataPath);
            StartCoroutine(LoadMapConfig(mainPath , sceneRef.replaceNavmesh, () =>
                {
                    if (_callBack != null)
                    {
                        _callBack();
                    }
                }));
        }
        else
        {
            Debug.LogError("场景 " + _sceneName + "的配置文件找不到！");
        }
    }


    protected IEnumerator LoadMapConfig(string _mainPath,string _name, System.Action _callBack)
    {
        if (prefabAssetBundleRefDic.ContainsKey(_name))
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_name, LoadSceneMode.Additive);
            //AsyncOperation asyncOperation = Application.LoadLevel(_name);
            while (asyncOperation == null || !asyncOperation.isDone)
            {
                yield return new WaitForFixedUpdate();
            }
            lastSceneNameList.Add(_name);
            GameObject sceneObj = GameObject.Find(_name);
            if (sceneObj != null)
            {
                if (finalObjDic.ContainsKey(_name))
                {
                    DestroyImmediate(finalObjDic[_name], true);
                    finalObjDic.Remove(_name);
                }
                finalObjDic[_name] = sceneObj;
                MeshRenderer[] mrs = sceneObj.GetComponentsInChildren<MeshRenderer>(true);
                for (int i = 0; i < mrs.Length; i++)
                {
                    if (mrs[i].material != null)
                    {
                        mrs[i].material.shader = Shader.Find(mrs[i].material.shader.name);
                    }
                }
            }
            GameCenter.curGameStage.InitSector(400, 400, 1, 1);
            if (_callBack != null)
            {
                _callBack();
            }
        }
        else
        {
            string url = _mainPath + _name + ".object";
            WWW www = new WWW(url);
            while (www == null || !www.isDone)
            {
                yield return new WaitForFixedUpdate();
            }
            if (www != null && www.isDone)
            {
                if (www.assetBundle != null)
                {
                    prefabAssetBundleRefDic[_name] = www;
                    AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_name, LoadSceneMode.Additive);
                    lastSceneNameList.Add(_name);
                    while (asyncOperation == null || !asyncOperation.isDone)
                    {
                        yield return new WaitForFixedUpdate();
                    }
                    GameObject sceneObj = GameObject.Find(_name);
                    if (sceneObj != null)
                    {
                        if (finalObjDic.ContainsKey(_name))
                        {
                            DestroyImmediate(finalObjDic[_name], true);
                            finalObjDic.Remove(_name);
                        }
                        finalObjDic[_name] = sceneObj;
                        MeshRenderer[] mrs = sceneObj.GetComponentsInChildren<MeshRenderer>(true);
                        for (int i = 0; i < mrs.Length; i++)
                        {
                            if (mrs[i].material != null)
                            {
                                mrs[i].material.shader = Shader.Find(mrs[i].material.shader.name);
                            }
                        }
					}else
					{
						Debug.LogError("GameObject.Find找不到:"+_name+"的GameObject");
					}
                    GameCenter.curGameStage.InitSector(400, 400, 1, 1);
                }
                else
                {
                    Debug.LogError(_name + " 资源为空!");
                }
                if (_callBack != null)
                {
                    _callBack();
                }
            }
        }
    }
    #endregion 
}
