///////////////////////////////////////////////////////////////////////////////////////////
//作者：吴江
//最后修改时间：2015/3/13
//脚本描述：用来负责加载场景和管理场景资源的类。
///////////////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EffectLoadUtil : LoadUtil
{
    #region 数据
    public static EffectLoadUtil instance;

    public Transform finalObjCacheTrans = null;


    /// <summary>
    /// 场景游戏中途加载的特效物体实例。在离开场景时全部销毁
    /// </summary>
    protected Dictionary<string, GameObject> runTimeObjDic = new Dictionary<string, GameObject>();

    protected Dictionary<string, Action<GameObject>> waitingAction = new Dictionary<string, Action<GameObject>>();
    #endregion


    void Awake()
    {
        instance = this;
        mainPath = AssetMng.GetPathWithoutExtension("effect/", AssetPathType.PersistentDataPath);
    }

    /// <summary>
    /// 开始加载配置数据
    /// </summary>
    /// <param name="_data"></param>
    /// <param name="_callBack"></param>
    public void StartLoadConfig(string _sceneName, int _sceneIndex, Action _callBack)
    {
        ResourcesTotalCount = 0;
        ResourcesCurCount = 0;
        List<string> nullList = new List<string>();
        foreach (var item in finalObjDic)
        {
            if (item.Value == null) nullList.Add(item.Key);
        }
        foreach (var item in nullList)
        {
            finalObjDic.Remove(item);
        }
        PlayGameStage stage = GameCenter.curGameStage as PlayGameStage;
        if (stage != null)
        {
            List<string> wholeNeeds = stage.GetEffectPreloadList();
            List<string> needDelete = new List<string>();

            List<string> spawnerlist = GameCenter.spawner.effectPoolCacheList;
            for (int i = 0; i < spawnerlist.Count; i++)
            {
                if (!wholeNeeds.Contains(spawnerlist[i]))
                {
                    needDelete.Add(spawnerlist[i]);
                }
            }
            for (int i = 0; i < needDelete.Count; i++)
            {
                GameCenter.spawner.DeleteDefEffecterPool(needDelete[i]);
            }
            StartCoroutine(LoadConfig(mainPath, wholeNeeds, _callBack));
        }
        else
        {
            if (_callBack != null)
            {
                _callBack();
            }
        }
    }

    public void StartLoadResources(string _sceneName, int _sceneIndex, Action _callBack)
    {
        PlayGameStage stage = GameCenter.curGameStage as PlayGameStage;
        if (stage != null)
        {
            StartCoroutine(LoadResources(mainPath, stage.GetEffectPreloadList(), _callBack));
        }
        else
        {
            if (_callBack != null)
            {
                _callBack();
            }
        }
    }


    public void GetSingleEffectGameObj(string _effectName, Action<GameObject> _callBack, bool _active = true)
    {
        GetSingleEffect(_effectName, (x) =>
        {
            if (_callBack != null)
            {
                // string str = string.Empty;
                // Debug.Log(logmaterial(ref str, x));
                if (x == null)
                {
                    _callBack(null);
                }
                else
                {
                    GameObject instance = GameObject.Instantiate(x) as GameObject;
                    if (_active)
                    {
                        _callBack(SetActive(instance, true));
                    }
                    else
                    {
                        _callBack(instance);
                    }
                }
            }

        });
    }


    protected string logmaterial(ref string _str, GameObject _obj)
    {
        if (_obj == null) return string.Empty;
        if (_obj.GetComponent<Renderer>() != null)
        {
            _str += _obj.name + ":" + _obj.GetComponent<Renderer>().material.name + "   ,  ";
        }
        if (_obj.transform.childCount > 0)
        {
            for (int i = 0; i < _obj.transform.childCount; i++)
            {
                logmaterial(ref _str, _obj.transform.GetChild(i).gameObject);
            }
        }
        return _str;
    }


    public void GetSingleEffect(string _effectName, Action<GameObject> _callBack)
    {
        if (_effectName == string.Empty || _effectName == null)
        {
            return;
        }
        if (finalObjDic.ContainsKey(_effectName))
        {
            if (_callBack != null) _callBack(finalObjDic[_effectName]);
        }
        else
        {
            if (waitingAction.ContainsKey(_effectName))
            {
                if (_callBack != null)
                {
                    waitingAction[_effectName] += _callBack;
                }
            }
            else
            {
                if (_callBack != null)
                {
                    waitingAction.Add(_effectName, _callBack);
                }
                StartCoroutine(LoadSingleEffect(mainPath, _effectName, () =>
                {
                    if (waitingAction.ContainsKey(_effectName) && waitingAction[_effectName] != null && _callBack != null)
                    {
                        waitingAction[_effectName](finalObjDic.ContainsKey(_effectName) ? finalObjDic[_effectName] : null);
                        waitingAction.Remove(_effectName);
                    }
                }));
            }
        }
    }


    protected IEnumerator LoadSingleEffect(string _mainPath, string _name, Action _callBack)
    {
        List<string> str = new List<string>();
        str.Add(_name);
        //根据列表，下载配置文件，存入缓存列表
        DownloadConfig(_mainPath, str, 0);

        //等待下载结束
        while (configPendings > 0) yield return new WaitForFixedUpdate();

        SceneConfigData curSingleConfigData = null;
        //确保本次所需要的所有特效配置文件都已经在缓存中
        if (configRefDic.ContainsKey(_name))
        {
            curSingleConfigData = configRefDic[_name] as SceneConfigData;
        }
        if (curSingleConfigData != null)
        {
            for (int i = 0; i < str.Count; i++)
            {
                DownloadFinalObject(_mainPath, str, i);
            }
            while (finalObjectPendings > 0)
            {
                yield return new WaitForFixedUpdate();
            }

            List<string> needText = GetNeedLoadAsset(curSingleConfigData, AssetType.texture);
            for (int i = 0; i < needText.Count; i++)
            {
                DownloadTexture(_mainPath, needText, i);
            }
            while (texturePendings > 0)
            {
                yield return new WaitForFixedUpdate();
            }


            List<string> needMat = GetNeedLoadAsset(curSingleConfigData, AssetType.material);
            for (int i = 0; i < needMat.Count; i++)
            {
                DownloadMaterial(_mainPath, needMat, i);
            }
            while (materialPendings > 0)
            {
                yield return new WaitForFixedUpdate();
            }

            BindMaterialDepend(curSingleConfigData.materialAssetList);

            while (finalObjInitPendings > 0)
            {
                yield return new WaitForFixedUpdate();
            }
            TryBindSceneRelation(curSingleConfigData);

        }
        else
        {
            Debug.LogError("can't find config data that names " + _name);
        }

        if (_callBack != null) _callBack();
    }




    protected void GetEffectConfig(int scene)
    {
        // List<string> effectNames = GetGetAllEffectNames(scene);

    }

    //		
    //	public List<string> GetAllArrow(int prof)//sxj
    //	{
    //		 List<int> ids = STUtil.DBFind("Arrow", "player = " + prof );
    //		string Path;
    //		foreach (int id in ids) 
    //		{
    //			string effcet_res = STUtil.DBGetStr("Arrow", "effect", id);
    //			if(effcet_res!="0" && effcet_res != "")
    //			{
    //				//Path = STUtil.GetPath("FXEffect/" + effcet_res, 1);
    //				ArrowList.Add(effcet_res);
    //			}
    //		}
    //		
    //	}

    #region 实例加载

    #endregion

    protected override void TryBindSceneRelation(List<SceneConfigData> _data)
    {
        foreach (var item in _data)
        {
            TryBindSceneRelation(item);
        }
    }

    protected override void InitFinalObject(string _name)
    {
        base.InitFinalObject(_name);
        if (prefabAssetBundleRefDic.ContainsKey(_name))
        {
            if (prefabAssetBundleRefDic[_name] == null)
            {
                if (!finalObjDic.ContainsKey(_name) || finalObjDic[_name] == null)
                {
                    Debug.LogError("特效预制资源:" + _name + "为空，请检查资源！");
                }
            }
            else
            {
                if (prefabAssetBundleRefDic[_name].assetBundle == null)
                {
                    //Debug.LogError("特效预制资源" + _name + "资源文件存在,但是加载后其中的assetBundle引用为空,无法获取资源,请检查!");
                }
                else
                {
                    UnityEngine.Object[] list = prefabAssetBundleRefDic[_name].assetBundle.LoadAllAssets();
                    foreach (var item in list)
                    {
                        GameObject obj = item as GameObject;
                        if (obj != null) SetActive(obj, false);
                    }
                }
            }
        }
    }

    protected void TryBindSceneRelation(SceneConfigData _data)
    {
        if (!finalObjDic.ContainsKey(_data.Name) || finalObjDic[_data.Name] == null)
        {
            if (prefabAssetBundleRefDic.ContainsKey(_data.Name) && prefabAssetBundleRefDic[_data.Name] != null && prefabAssetBundleRefDic[_data.Name].assetBundle != null)
            {
                UnityEngine.Object obj = null;

                UnityEngine.Object[] list = prefabAssetBundleRefDic[_data.Name].assetBundle.LoadAllAssets();
                if (list.Length > 0)
                {
                    obj = list[0];
                }
                finalObjDic[_data.Name] = GameObject.Instantiate(obj, new Vector3(10000, 10000, 10000), Quaternion.identity) as GameObject;
                FXRoot fr = finalObjDic[_data.Name].GetComponent<FXRoot>();
                if (fr == null)
                {
                    fr = finalObjDic[_data.Name].AddComponent<FXRoot>();
                    fr.InitLength();
                }
                if (finalObjDic[_data.Name] != null)
                {
                    finalObjDic[_data.Name].name = _data.Name;
                    finalObjDic[_data.Name].SetActive(false);
                }
                else
                {
                    finalObjDic.Remove(_data.Name);
                    Debug.LogError("asset ref " + _data.Name + " is null ,please check it !");
                }
                prefabAssetBundleRefDic[_data.Name].assetBundle.Unload(false);
            }
            else
            {
                Debug.LogError("找不到名为 " + _data.Name + " 的预制资源，请检查！如果前面没有其他加载报错，有可能是配置表和资源名称的大小写不匹配的问题！");
            }
        }
        if (_data == null || _data.sceneAsset == null || _data.sceneAsset.gameObjectRelationship == null)
        {
            Debug.LogError("配置文件有问题，请检查!");
            return;
        }
        if (!finalObjDic.ContainsKey(_data.Name) || finalObjDic[_data.Name] == null)
        {
            Debug.LogError("找不到名为 " + _data.Name + "的预制，请检查");
            return;
        }
        ProcessGameObjectRelation(finalObjDic[_data.Name], _data.sceneAsset.gameObjectRelationship);
        finalObjDic[_data.Name].transform.parent = finalObjCacheTrans;

    }

}