///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/5/28
//用途：对象池管理器
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// 对象池管理器 by 吴江
/// </summary>
public class Spawner : MonoBehaviour
{

    #region 构造 by吴江
    public GameObject poolObject = null;


    protected bool inited = false;
    public bool Inited
    {
        get { return inited; }
    }

    /// <summary>
    /// 初始化 by吴江
    /// </summary>
    /// <param name="_callback"></param>
    public void Init(System.Action _callback)
    {
        StartCoroutine(StartInit(_callback));
    }
    /// <summary>
    /// 刷新，清理非初始化必要内容
    /// </summary>
    public void Refresh()
    {
        CleanEffectPool();
    }

    /// <summary>
    /// 初始化  全局静态对象池在这个接口中初始化。（如被击特效等根据场景来的对象池就不能在这里初始化） by吴江
    /// </summary>
    /// <param name="_callback"></param>
    /// <returns></returns>
    protected IEnumerator StartInit(System.Action _callback)
    {


        poolObject = new GameObject("Pool Cache");
        poolObject.transform.position = Vector3.zero;
        GameObject.DontDestroyOnLoad(poolObject);

        //////点击地面特效
        int pendings = 0;
        pendings++;
        AssetMng.GetEeffctAssetObject("cursor_b02", (x) =>
            {
                pendings--;
                if (x != null)
                {
                    GameObject indicatorPoolCache = new GameObject("IndicatorPoolCache");
                    indicatorPoolCache.transform.parent = poolObject.transform;
                    indicatorPoolCache.transform.localPosition = Vector3.zero;
                    indicatorPool.Init(x, 3, indicatorPoolCache.transform);
                }
                else
                {
                    Debug.LogError("地面点击特效加载失败！");
                }
            });
        while (pendings > 0)
        {
            yield return new WaitForFixedUpdate();
        }


        //脚下光圈
        pendings++;
        AssetMng.GetEeffctAssetObject("m_f_005", (x) =>
        {
            pendings--;
            if (x != null)
            {
                GameObject ringEffecterPoolCache = new GameObject("RingEffecterPoolCache");
                ringEffecterPoolCache.transform.parent = poolObject.transform;
                ringEffecterPoolCache.transform.localPosition = Vector3.zero;
                ringEffecterPool.Init(x, 1, ringEffecterPoolCache.transform);
            }
            else
            {
                Debug.LogError("光圈特效特效加载失败！");
            }
        });
        while (pendings > 0)
        {
            yield return new WaitForFixedUpdate();
        }


        //阴影投射器
        //GameObject shadowPoolCache = new GameObject("shadowPoolCache");
        //shadowPoolCache.transform.parent = poolObject.transform;
        //shadowPoolCache.transform.localPosition = Vector3.zero;

        //GameObject prefab = Instantiate(exResources.GetResource(ResourceType.OTHER, "ShadowProjector")) as GameObject;
        //prefab.SetActive(false);
        //shadowEffectorPool.Init(prefab, 5, shadowPoolCache.transform);

        //技能预警投射器
        GameObject abilityShadowPoolCache = new GameObject("abilityShadowPoolCache");
        abilityShadowPoolCache.transform.parent = poolObject.transform;
        abilityShadowPoolCache.transform.localPosition = Vector3.zero;

        GameObject abilityShadowPrefab = Instantiate(exResources.GetResource(ResourceType.OTHER, "AbilityProjector")) as GameObject;
        abilityShadowPrefab.SetActive(false);
        AbilityShadowEffectorPool.Init(abilityShadowPrefab, 2, abilityShadowPoolCache.transform);


        //漂浮文字
        GameObject stateTexterPoolCache = new GameObject("StateTexterPoolCache");
        stateTexterPoolCache.transform.parent = GameCenter.cameraMng.uiCamera.transform.root;
        stateTexterPoolCache.transform.localPosition = Vector3.zero;

        GameObject stateTexterPrefab = Instantiate(exResources.GetResource(ResourceType.TEXT, "StateTexter")) as GameObject;
       // UILabel lb = NGUITools.AddWidget<UILabel>(stateTexterPoolCache);
        NumberLookAt lookCtrl = stateTexterPrefab.GetComponent<NumberLookAt>();
        if (lookCtrl == null) lookCtrl = stateTexterPrefab.AddComponent<NumberLookAt>();
        lookCtrl.target = GameCenter.cameraMng.mainCamera;
        stateTexterPool.Init(stateTexterPrefab, 0, stateTexterPoolCache.transform);//在不是0的时候  最开始的几下看不到数字 by邓成


        ////弹道
        GameObject ballisticCurvePoolCache = new GameObject("ballisticCurvePoolCache");
        ballisticCurvePoolCache.transform.parent = poolObject.transform;
        ballisticCurvePoolCache.transform.localPosition = Vector3.zero;

        GameObject ballisticCurveprefab = new GameObject("BallisticCurve");
        ballisticCurveprefab.AddComponent<AbilityBallisticCurve>();
        abilityBallisticCurvePool.Init(ballisticCurveprefab, 3, ballisticCurvePoolCache.transform);

        ////技能延迟对象
        GameObject abilityDelayEffectCtrlPoolCache = new GameObject("abilityDelayEffectCtrlPoolCache");
        abilityDelayEffectCtrlPoolCache.transform.parent = poolObject.transform;
        abilityDelayEffectCtrlPoolCache.transform.localPosition = Vector3.zero;

        GameObject abilityDelayEffectCtrlprefab = new GameObject("abilityDelayEffectCtrl");
        ballisticCurveprefab.AddComponent<AbilityDelayEffectCtrl>();
        abilityDelayEffectCtrlPool.Init(abilityDelayEffectCtrlprefab, 5, abilityDelayEffectCtrlPoolCache.transform);

        inited = true;
        if (_callback != null)
        {
            _callback();
        }
    }
    #endregion 

    #region 阴影投射器 by吴江
    /// <summary>
    /// 阴影投射器对象池 by吴江
    /// </summary>
    protected exComponentPool<ShadowEffecter> shadowEffectorPool = new exComponentPool<ShadowEffecter>();

    /// <summary>
    /// 从对象池中取出 阴影投射器的对象 by吴江
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_rot"></param>
    /// <returns></returns>
    public ShadowEffecter SpawnShadowEffecter(Vector3 _pos, Quaternion _rot, Transform _parent)
    {
        ShadowEffecter shadowEffecter = shadowEffectorPool.Request(_pos, _rot, _parent);
        if (_parent != null)
        {
            shadowEffecter.gameObject.layer = _parent.gameObject.layer;
        }
        shadowEffecter.OnSpawned();
        return shadowEffecter;
    }

    /// <summary>
    /// 将 阴影投射器的对象归还到对象池 by吴江
    /// </summary>
    /// <param name="_indicator"></param>
    public void DespawnShadowEffecter(ShadowEffecter _shadowEffecter)
    {
        _shadowEffecter.OnDespawned();
        shadowEffectorPool.Return(_shadowEffecter);
    }
    #endregion

    #region 技能预警投射器 by吴江
    /// <summary>
    /// 阴影投射器对象池 by吴江
    /// </summary>
    protected exComponentPool<AbilityShadowEffecter> AbilityShadowEffectorPool = new exComponentPool<AbilityShadowEffecter>();

    /// <summary>
    /// 从对象池中取出技能预警投射器的对象 by吴江
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_rot"></param>
    /// <returns></returns>
    public AbilityShadowEffecter SpawnAbilityShadowEffecter(Vector3 _pos, float _rotY, float _wid, float _length, AlertAreaType _type, Color _color, float _duration)
    {
        AbilityShadowEffecter abilityShadowEffecter = AbilityShadowEffectorPool.Request(_pos);
        abilityShadowEffecter.OnSpawned(_type, _color, _rotY,_wid,_length, _duration);
        return abilityShadowEffecter;
    }

    /// <summary>
    /// 从对象池中取出技能预警投射器的对象 by吴江
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_rot"></param>
    /// <returns></returns>
    public AbilityShadowEffecter SpawnAbilityShadowEffecter(Transform _parent, float _rotY, float _wid, float _length, AlertAreaType _type, Color _color, float _duration)
    {
        AbilityShadowEffecter abilityShadowEffecter = AbilityShadowEffectorPool.Request(Vector3.zero);
        abilityShadowEffecter.transform.parent = _parent;
        abilityShadowEffecter.transform.localPosition = new Vector3(0, 10.0f, 0);
        abilityShadowEffecter.OnSpawned(_type, _color, _rotY, _wid, _length, _duration);
        return abilityShadowEffecter;
    }

    /// <summary>
    /// 将 技能预警投射器的对象归还到对象池 by吴江
    /// </summary>
    /// <param name="_indicator"></param>
    public void DespawnAbilityShadowEffecter(AbilityShadowEffecter _abilityShadowEffecter)
    {
        _abilityShadowEffecter.OnDespawned();
        AbilityShadowEffectorPool.Return(_abilityShadowEffecter);
    }
    #endregion

    #region 对象脚下光圈 by吴江
    /// <summary>
    /// 光圈对象池 by吴江
    /// </summary>
    protected exComponentPool<RingEffecter> ringEffecterPool = new exComponentPool<RingEffecter>();

    /// <summary>
    /// 从对象池中取出脚下光圈的对象 by吴江
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_rot"></param>
    /// <returns></returns>
    public RingEffecter SpawnRingEffecter(Vector3 _pos, Quaternion _rot,Transform _parent,Color _color)
    {
        RingEffecter ringEffecter = ringEffecterPool.Request(_pos, _rot, _parent);
		if(ringEffecter != null)
		{
	        if (_parent != null)
	        {
	            ringEffecter.gameObject.layer = _parent.gameObject.layer;
	        }
	        ringEffecter.OnSpawned(_color);
		}
        return ringEffecter;
    }

    /// <summary>
    /// 将脚下光圈的对象归还到对象池 by吴江
    /// </summary>
    /// <param name="_indicator"></param>
    public void DespawnRingEffecter(RingEffecter _ringEffecter)
    {
        _ringEffecter.OnDespawned();
        ringEffecterPool.Return(_ringEffecter);
    }
    #endregion

    #region 地面点击特效 by吴江
    /// <summary>
    /// 点击地面对象池 by吴江
    /// </summary>
    protected exComponentPool<Indicator> indicatorPool = new exComponentPool<Indicator>();
    /// <summary>
    /// 从对象池中取出点击地面特效的对象 by吴江
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_rot"></param>
    /// <returns></returns>
    public Indicator SpawnIndicator(Vector3 _pos, Quaternion _rot)
    {
        Indicator indicator = indicatorPool.Request(_pos, _rot);
        if (indicator != null) indicator.OnSpawned();
        return indicator;
    }

    /// <summary>
    /// 将点击地面特效的对象归还到对象池 by吴江
    /// </summary>
    /// <param name="_indicator"></param>
    public void DespawnIndicator(Indicator _indicator)
    {
        _indicator.OnDespawned();
        indicatorPool.Return(_indicator);
    }
    #endregion 

    #region 漂浮文字 by吴江
    /// <summary>
    /// 漂浮文字对象池 by吴江
    /// </summary>
    protected exComponentPool<StateTexter> stateTexterPool = new exComponentPool<StateTexter>();
    /// <summary>
    /// 从对象池中取出漂浮文字对象 by吴江
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_rot"></param>
    /// <returns></returns>
    public StateTexter SpawnStateTexter(AbilityResultInfo _info)
    {
        //if (!SystemSettingMng.ShowStateTexts) return null;
        StateTexter stateTexter = stateTexterPool.Request();
        stateTexter.OnSpawned(_info);
        return stateTexter;
    }

    /// <summary>
    /// 从对象池中取出漂浮文字对象,生命恢复 by何明军
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_rot"></param>
    /// <returns></returns>
    public StateTexter SpawnStateTexter(int val)
    {
        //if (!SystemSettingMng.ShowStateTexts) return null;
        StateTexter stateTexter = stateTexterPool.Request();
        stateTexter.OnSpawned(val);
        return stateTexter;
    }

    /// <summary>
    /// 将漂浮文字的对象归还到对象池 by吴江
    /// </summary>
    /// <param name="_indicator"></param>
    public void DespawnStateTexter(StateTexter _stateTexter)
    {
        _stateTexter.OnDespawned();
        stateTexterPool.Return(_stateTexter);
    }
    #endregion

    #region 特效 by吴江
    protected void CleanEffectPool()
    {
        defEffectPoolDictionary.Clear();
        for (int i = 0; i < poolObject.transform.childCount; i++)
        {
            GameObject obj = poolObject.transform.GetChild(i).gameObject;
            if (obj.name.Contains("ExPoolCache"))
            {
                Destroy(obj);
            }
        }
    }

    /// <summary>
    /// 特效对象池列表 by吴江
    /// </summary>
    protected Dictionary<string, exComponentPool<PoolEffecter>> defEffectPoolDictionary = new Dictionary<string, exComponentPool<PoolEffecter>>();

    /// <summary>
    /// 等待回调队列的key结构 by吴江
    /// </summary>
    protected struct CallKey
    {
        public string name;
        public float duration;
        public System.Action<PoolEffecter> callBack;
        public CallKey(string _name, float _duration, System.Action<PoolEffecter> _callBack)
        {
            name = _name;
            duration = _duration;
            callBack = _callBack;
        }
    }

    public List<string> effectPoolCacheList = new List<string>();
    /// <summary>
    /// 等待回调队列 by吴江
    /// </summary>
    protected Dictionary<string, List<CallKey>> waitCallBack = new Dictionary<string, List<CallKey>>();

    /// <summary>
    /// 从对象池中取出特效对象  因为没有进行预加载，因此只能采用异步回调的方式 by吴江
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_rot"></param>
    /// <returns></returns>
    public void SpawnEffecter(string _effectName, float _duration,System.Action<PoolEffecter> _callback,bool _matchingSpeed)
    {
        PoolEffecter defEffecter = null;
        exComponentPool<PoolEffecter> pool = null;
        if (defEffectPoolDictionary.TryGetValue(_effectName, out pool))
        {
            //如果缓存中已经有这个对象池，那么直接回调结束 by吴江
            defEffecter = pool.Request();
            if (_callback != null) _callback(defEffecter);
            defEffecter.OnSpawned(_duration, _matchingSpeed);
        }
        else
        {
            //如果缓存中尚无这个对象池；那么看看请求队列中是否已经有，如果尚无请求，则新建请求。 如有请求，则把回调压入之前的请求中。 by吴江
            CallKey callKey = new CallKey(_effectName, _duration, _callback);
            if (!waitCallBack.ContainsKey(_effectName))
            {
                List<CallKey> list = new List<CallKey>();
                list.Add(callKey);
                waitCallBack[_effectName] = list;

                    BuildDefEffecterPool(_effectName, (x) =>
                    {
                        if (x != null)
                        {
                            pool = x;
                            defEffectPoolDictionary.Add(_effectName, pool);
                            if (waitCallBack.ContainsKey(_effectName))
                            {
                                int count = waitCallBack[_effectName].Count;
                                for (int i = 0; i < count; i++)
                                {
                                    defEffecter = pool.Request();
                                    if (waitCallBack[_effectName][i].callBack != null)
                                    {
                                        waitCallBack[_effectName][i].callBack(defEffecter);
                                    }
                                    defEffecter.OnSpawned(_duration, _matchingSpeed);
                                }
                                waitCallBack.Remove(_effectName);
                            }
                        }
                       
                    });
            }
            else
            {
                waitCallBack[_effectName].Add(callKey);
            }
        }
    }

    /// <summary>
    /// 将特效对象归还到对象池 by吴江
    /// </summary>
    /// <param name="_indicator"></param>
    public bool DespawnEffecter(PoolEffecter _Effecter)
    {
        if (_Effecter == null)
        {
            Debug.LogError("_Effecter为空!无法归还!");
            return false;
        }
        _Effecter.OnDespawned();
        exComponentPool<PoolEffecter> pool = null;
        if (defEffectPoolDictionary.TryGetValue(_Effecter.name, out pool))
        {
            if (pool != null)
            {
                try
                {
                    pool.Return(_Effecter);
                    return true;
                }
                catch
                {
                    GameObject.DestroyImmediate(_Effecter);
                }
            }
            else
            {
                GameObject.DestroyImmediate(_Effecter);
            }
        }
        else
        {
            GameObject.DestroyImmediate(_Effecter);
        }
        return false;
    }

    /// <summary>
    /// 创建一个特定特效的对象池 by吴江
    /// </summary>
    /// <param name="_effectName"></param>
    /// <param name="_callback"></param>
    public void BuildDefEffecterPool(string _effectName,System.Action<exComponentPool<PoolEffecter>> _callback)
    {
        if (!effectPoolCacheList.Contains(_effectName))
        {
            effectPoolCacheList.Add(_effectName);
        }
        AssetMng.GetEffectInstance(_effectName, (x) =>
        {
            exComponentPool<PoolEffecter> pool = null;
            if (x != null)
            {
                x.name = _effectName;
                pool = new exComponentPool<PoolEffecter>();
                GameObject.DontDestroyOnLoad(x);
                GameObject defEffecterPoolCache = new GameObject(_effectName + " ExPoolCache");
                defEffecterPoolCache.transform.parent = poolObject.transform;
                defEffecterPoolCache.transform.localPosition = Vector3.zero;
                pool.Init(x, 3, defEffecterPoolCache.transform);
            }
            else
            {
                Debug.LogError(_effectName + "特效加载失败！");
            }
            if (_callback != null) _callback(pool);
        });
    }

    /// <summary>
    /// 删除一个特效的对象池（一般用于切换场景时） by吴江
    /// </summary>
    /// <param name="_effectName"></param>
    public void DeleteDefEffecterPool(string _effectName)
    {
        if (effectPoolCacheList.Contains(_effectName))
        {
            effectPoolCacheList.Remove(_effectName);
        }
        exComponentPool<PoolEffecter> pool = null;
        if (defEffectPoolDictionary.TryGetValue(_effectName, out pool))
        {
            pool.CollectAllBack();
            GameObject.DestroyImmediate(pool.poolTrasfrom.gameObject,true);
            defEffectPoolDictionary.Remove(_effectName);
        }
        else
        {
            Debug.LogError("找不到试图删除的对象池:" + _effectName);
        }
    }
    #endregion

    #region 弹道
    /// <summary>
    /// 弹道对象池 by吴江
    /// </summary>
    protected exComponentPool<AbilityBallisticCurve> abilityBallisticCurvePool = new exComponentPool<AbilityBallisticCurve>();
    /// <summary>
    /// 从对象池中取出弹道的对象 by吴江
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_rot"></param>
    /// <returns></returns>
    public AbilityBallisticCurve SpawnAbilityBallisticCurve(AbilityBallisticCurveInfo _info)
    {
        AbilityBallisticCurve abilityBallisticCurve = abilityBallisticCurvePool.Request(_info.StartPos, Quaternion.Euler(_info.Direction));
        abilityBallisticCurve.OnSpawned(_info);
        return abilityBallisticCurve;
    }

    /// <summary>
    /// 将弹道对象归还到对象池 by吴江
    /// </summary>
    /// <param name="_indicator"></param>
    public void DespawnAbilityBallisticCurve(AbilityBallisticCurve abilityBallisticCurve)
    {
        abilityBallisticCurve.OnDespawned();
        abilityBallisticCurvePool.Return(abilityBallisticCurve);
    }
    #endregion

    #region 技能延迟特效
    /// <summary>
    /// 技能延迟特效对象池 by吴江
    /// </summary>
    protected exComponentPool<AbilityDelayEffectCtrl> abilityDelayEffectCtrlPool = new exComponentPool<AbilityDelayEffectCtrl>();
    /// <summary>
    /// 从对象池中取出技能延迟特效的对象 by吴江
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_rot"></param>
    /// <returns></returns>
    public AbilityDelayEffectCtrl SpawnAbilityDelayEffectCtrl(AbilityDelayEffectRefData _delayData, Transform _userTransform)
    {
        if (_delayData == null || _userTransform == null)
        {
            if (_delayData == null) Debug.LogError("技能延迟特效信息为空!");
            return null;
        }
       
        float eulerAngleY = _userTransform.localEulerAngles.y;
        float x = Mathf.Sin(eulerAngleY * Mathf.Deg2Rad) * _delayData.diffPos.z + Mathf.Cos(eulerAngleY * Mathf.Deg2Rad) * _delayData.diffPos.x;
        float z = Mathf.Cos(eulerAngleY * Mathf.Deg2Rad) * _delayData.diffPos.z + Mathf.Sin(eulerAngleY * Mathf.Deg2Rad) * _delayData.diffPos.x;
       // Debug.logger.Log("eulerAngleY = " + eulerAngleY + " , Mathf.Sin(eulerAngleY * Mathf.Deg2Rad) = " + Mathf.Sin(eulerAngleY * Mathf.Deg2Rad) + " , Mathf.Cos(eulerAngleY * Mathf.Deg2Rad) = " + Mathf.Cos(eulerAngleY * Mathf.Deg2Rad));
        Vector3 diffPos = new Vector3(x, _delayData.diffPos.y, z);
        Vector3 pos = diffPos + _userTransform.position;
       // Debug.logger.Log("玩家坐标" + _userTransform.position + " , 差异坐标" + diffPos + " , 最终坐标" + pos);
        AbilityDelayEffectCtrl abilityDelayEffectCtrl = abilityDelayEffectCtrlPool.Request(pos, _userTransform.rotation);
        abilityDelayEffectCtrl.OnSpawned(_delayData.effectName, _delayData.startTIme, _delayData.duration, pos);
        return abilityDelayEffectCtrl;
    }

    /// <summary>
    /// 将技能延迟特效对象归还到对象池 by吴江
    /// </summary>
    /// <param name="_indicator"></param>
    public void DespawnAbilityDelayEffectCtrl(AbilityDelayEffectCtrl _abilityDelayEffectCtrl)
    {
        _abilityDelayEffectCtrl.OnDespawned();
        abilityDelayEffectCtrlPool.Return(_abilityDelayEffectCtrl);
    }
    #endregion


    /// <summary>
    /// 固定帧检测，释放冗余对象，还原对象池 by吴江
    /// </summary>
    void Update()
    {
        if (Time.frameCount % 100 == 0)//这个检测的速率可以调整，不宜太过频繁  by吴江
        {
            ringEffecterPool.Tick();
            indicatorPool.Tick();
            stateTexterPool.Tick();
            foreach (var item in defEffectPoolDictionary.Values)
            {
                item.Tick();
            }
        }
    }

}
