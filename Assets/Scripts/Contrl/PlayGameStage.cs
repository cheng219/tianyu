///////////////////////////////////////////////////////////////////////////////
// 作者：吴江
// 日期：2015/5/13
// 用途：主城平台和地下城平台的基类
///////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 主城平台和地下城平台的基类 by吴江
/// </summary>
public class PlayGameStage : GameStage
{


    protected float startRunTime = 0;
    protected float startCheckTime = 10.0f;
    protected float lastCheckTime = 0;
    protected bool hasCheckQuality = false;
	/// <summary>
	///  渲染距离
	/// </summary>
	protected int cullDistance = 15;

    /// <summary>
    /// 当前主玩家表现层对象
    /// </summary>
    [System.NonSerialized]
    protected MainPlayer curMainPlayer;
    /// <summary>
    /// 当前主玩家表现层对象
    /// </summary>
    public MainPlayer CurMainPlayer
    {
        get { return curMainPlayer; }
    }

    /// <summary>
    /// 场景预加载特效列表,更换场景时清除 by吴江
    /// </summary>
    protected Dictionary<string, bool> monsterCacheEffectResState = new Dictionary<string, bool>();
    /// <summary>
    /// 其他玩家的技能特效列表(主城不需要用到)
    /// </summary>
    protected Dictionary<string, bool> otherPlayerEffectResState = new Dictionary<string, bool>();

    /// <summary>
     /// 玩家普通攻击预加载特效列表。更换玩家时清除 by吴江
    /// </summary>
    protected Dictionary<string, bool> mainPlayerCacheEffectResState = new Dictionary<string, bool>();

    /// <summary>
    /// 玩家激活技能攻击预加载特效列表。更换玩家时清除 by吴江
    /// </summary>
    protected Dictionary<string, bool> mainPlayerEnableSkillResState = new Dictionary<string, bool>();

    /// <summary>
    /// 玩家骨骼预加载特效列表。更换玩家时清除 by吴江
    /// </summary>
    protected Dictionary<string, bool> mainPlayerBoneEffectResState = new Dictionary<string, bool>();
    /// <summary>
    /// 玩家坐骑骨骼预加载特效列表。更换坐骑时清除 by吴江
    /// </summary>
    protected Dictionary<string, bool> mainMountBoneEffectResState = new Dictionary<string, bool>();

    /// <summary>
    /// 获取需要预加载的所有特效 by吴江
    /// </summary>
    /// <returns></returns>
    public List<string> GetEffectPreloadList()
    {
        List<string> all = new List<string>(monsterCacheEffectResState.Keys);
        all.AddRange(mainPlayerCacheEffectResState.Keys);
        all.AddRange(mainPlayerEnableSkillResState.Keys);
        all.AddRange(mainPlayerBoneEffectResState.Keys);
        all.AddRange(mainMountBoneEffectResState.Keys);
        //阴影和点击地面的特效属性常驻特效，需要加到列表中。 否则会被卸载.
        all.Add("cursor_b02");
        all.Add("m_f_005");
        if (GameCenter.mainPlayerMng.MainPlayerInfo.CurStarEffect.Length > 0)
        {
            all.Add(GameCenter.mainPlayerMng.MainPlayerInfo.CurStarEffect);
        }
        return all;
    }

    public class skill_data
    {
        public int skill;
        public int lev;
    }


    protected bool hasRegist = false;

    protected override void Regist()
    {
        base.Regist();
        if (hasRegist) return;
        hasRegist = true;
        sceneMng.SceneStateChange += OnSceneStateChange;
        SceneMng.OnCleanAll += CleanAll;
        SceneMng.OnNPCInfoListUpdate += BuildNPCs;
        SceneMng.OnMobInfoListUpdate += BuildMonsters;
        SceneMng.OnEntourageInfoListUpdate += BuildEntourages;
        SceneMng.OnDropInfoListUpdate += BuildDropItems;
        GameCenter.mercenaryMng.OnPetUpdate += BuildMainEntourage;
        SceneMng.OnObjTalking += OnObjectTalking;
        SceneMng.OnTrapInfoDicUpdate += BuildTraps;
        SceneMng.OnOPCInfoListUpdate += BuildOPCs;
        SceneMng.OnSceneItemInfoListUpdate += BuildItems;
        SceneMng.OnModelInfoListUpdate += BuildModels;
        SceneMng.OnDelInterObj += DeleteInterObj;
        GameCenter.taskMng.OnTaskListUpdate += RefreshSmartNPCsShowHide; 
		GameCenter.skillMng.OnChangeSkill += OnPreloadEnableSkill;
        GameCenter.systemSettingMng.OnUpdateOtherPlayerTitle += UpdateOtherPlayerTitle;
        GameCenter.systemSettingMng.OnUpdateMonsterName += UpdateMonsterName;
        GameCenter.systemSettingMng.OnUpdateItemName += UpdateItemName;
        GameCenter.systemSettingMng.OnUpdateMaxPlayer += UpdateOtherPlayerList;
        GameCenter.systemSettingMng.OnUpdateOtherPlayerPet += UpdateOtherPlayerPetShow;
        GameCenter.systemSettingMng.OnUpdateOtherPlayerSkill += UpdateOtherPlayerSkillEffect;
        GameCenter.systemSettingMng.OnUpdateOtherPlayerMagicWeapon += RefreshOtherPlayerWingAndMagic;
        GameCenter.systemSettingMng.OnUpdateOtherPlayerWing += RefreshOtherPlayerWingAndMagic;

        GameCenter.teamMng.onTeammateUpdateEvent += UpdateOtherPlayerList; 
        GameCenter.guildMng.OnUpdateMemberEvent += UpdateOtherPlayerList;
        GameCenter.OnConnectStateChange += ConnectStateChange;
        GameCenter.msgLoackingMng.OnUpdateCmdDictionary += OnUpdateCmdDictionary;
    }

    public override void UnRegist()
    {
        base.UnRegist();
        MainPlayerInfo mpInfo = GameCenter.mainPlayerMng.MainPlayerInfo;
        if (mpInfo != null)
        {
            mpInfo.OnCurShowEquipUpdate -= OnRefreshBoneEffect;
            if (mpInfo.CurMountInfo != null)
            {
                mpInfo.CurMountInfo.OnEquipUpdate -= OnRefereshMountBoneEffect;
                mpInfo.CurMountInfo.OnEquipUpdate -= OnRefereshMountBoneEffect;
                mpInfo.CurMountInfo.OnRideStateUpdate -= OnRefereshMountBoneEffect;
            }
        }
        sceneMng.SceneStateChange -= OnSceneStateChange;
        SceneMng.OnCleanAll -= CleanAll;
        SceneMng.OnNPCInfoListUpdate -= BuildNPCs;
        SceneMng.OnMobInfoListUpdate -= BuildMonsters;
        SceneMng.OnDropInfoListUpdate -= BuildDropItems;
        SceneMng.OnEntourageInfoListUpdate -= BuildEntourages;
        GameCenter.mercenaryMng.OnPetUpdate -= BuildMainEntourage;
        SceneMng.OnTrapInfoDicUpdate -= BuildTraps;
        SceneMng.OnOPCInfoListUpdate -= BuildOPCs;
        SceneMng.OnObjTalking -= OnObjectTalking;
        SceneMng.OnSceneItemInfoListUpdate -= BuildItems;
        SceneMng.OnModelInfoListUpdate -= BuildModels;
        SceneMng.OnDelInterObj -= DeleteInterObj;
        GameCenter.taskMng.OnTaskListUpdate -= RefreshSmartNPCsShowHide; 
		GameCenter.skillMng.OnChangeSkill -= OnPreloadEnableSkill; 
        GameCenter.systemSettingMng.OnUpdateOtherPlayerTitle -= UpdateOtherPlayerTitle;
        GameCenter.systemSettingMng.OnUpdateMonsterName -= UpdateMonsterName;
        GameCenter.systemSettingMng.OnUpdateItemName -= UpdateItemName;
        GameCenter.systemSettingMng.OnUpdateMaxPlayer -= UpdateOtherPlayerList;
        GameCenter.teamMng.onTeammateUpdateEvent -= UpdateOtherPlayerList; 
        GameCenter.guildMng.OnUpdateMemberEvent -= UpdateOtherPlayerList;
        GameCenter.systemSettingMng.OnUpdateOtherPlayerPet -= UpdateOtherPlayerPetShow;
        GameCenter.systemSettingMng.OnUpdateOtherPlayerSkill -= UpdateOtherPlayerSkillEffect;
        GameCenter.systemSettingMng.OnUpdateOtherPlayerMagicWeapon -= RefreshOtherPlayerWingAndMagic;
        GameCenter.systemSettingMng.OnUpdateOtherPlayerWing -= RefreshOtherPlayerWingAndMagic;
        GameCenter.OnConnectStateChange -= ConnectStateChange;
        GameCenter.msgLoackingMng.OnUpdateCmdDictionary -= OnUpdateCmdDictionary;
        if (GameCenter.curMainPlayer != null && !GameCenter.curMainPlayer.inputListener.CheckLock())
        {
            GameCenter.curMainPlayer.inputListener.CleanLock();
        }
        UnLoadOPCEquipments();
		GameCenter.uIMng.ReleaseGUI(GUIType.FUNCTION);
    }
	

    #region 监听网络是否断开
    protected void ConnectStateChange(bool isConnected)
    {
        LoginStage stage = GameCenter.curGameStage as LoginStage;
        if (stage != null && stage.CurLoginState == LoginStage.EventType.PASS_WORD)
        {
            return;
        }
        if (!GameCenter.instance.IsReConnecteding && !NetCenter.Connected && GameCenter.messageMng != null && GameCenter.loginMng.CurConnectServerType == LoginMng.ConnectServerType.Game)
        {
            stateMachine.Send((int)EventType.RECONNECT);
        }
    }

    protected void OnGameStateToLogin(object[] obj)
    {
        GameCenter.instance.GoPassWord();
    }

    public void ExitReconnect()
    {
        GameCenter.curMainPlayer.ReBindInfo();
        stateMachine.Send((int)EventType.RUN);
    }

    protected void CheckWifi()
    {
        if (Time.frameCount % 10 == 0)
        {
            if (myLastIP != "0.0.0.0" && myLastIP != Network.player.ipAddress)
            {
                stateMachine.Send((int)EventType.RECONNECT);
                myLastIP = Network.player.ipAddress;
            }
        }
    }
    #endregion


    #region 过场动画部分
    public enum EventType
    {
        AWAKE = fsm.Event.USER_FIELD + 1,
        NEWBIE_GUIDE,
        LOAD,
        RUN,
        STOP,
        BLACK_PROCESS,
        SCENE_ANIMATION,
        RECONNECT,
    }
    /// <summary>
    /// 过场动画状态 by邓成
    /// </summary>
    //protected fsm.SceneAnimStage sceneAnimState;

    protected override void InitStateMachine()
    {
        //sceneAnimState = new fsm.SceneAnimStage("sceneAnimState", stateMachine);
    }

//    protected void OnSceneAnimUpdate()
//    {
//        if (GameCenter.sceneAnimMng.RestAnimCount > 0)
//        {
//            stateMachine.Send((int)EventType.SCENE_ANIMATION);
//			if (GameCenter.mainPlayerMng.MainPlayerInfo.CurMountInfo != null && GameCenter.mainPlayerMng.MainPlayerInfo.CurMountInfo.IsRiding)
//			{
//				GameCenter.newMountMng.C2S_ReqRideMount(ChangeMount.DOWNRIDE, GameCenter.mainPlayerMng.MainPlayerInfo.CurMountInfo.ConfigID);
//			}
//        }
//    }
    #endregion


    #region 加载
    /// <summary>
    /// 加载场景的任务列表 by吴江
    /// </summary>
    protected List<Action<string, int, Action>> loadSceneTaskList = new List<Action<string, int, Action>>();
    /// <summary>
    /// 压入加载任务 by吴江
    /// </summary>
    /// <param name="_delegate"></param>
    public void AddTask(Action<string, int, Action> _delegate)
    {
        loadSceneTaskList.Add(_delegate);
    }
    /// <summary>
    /// 加载任务完成 by吴江
    /// </summary>
    protected void TaskComplete()
    {
        if (loadSceneTaskList.Count > 0) loadSceneTaskList.RemoveAt(0);
    }
    /// <summary>
    /// 开始执行加载任务 by吴江
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <param name="_sceneIndex"></param>
    protected void ActTask(string _sceneName, int _sceneIndex)
    {
        if (loadSceneTaskList.Count <= 0) return;
        Action<string, int, Action> tast = loadSceneTaskList[0];
        if (tast != null) tast(_sceneName, _sceneIndex, () =>
        {
            TaskComplete();
            ActTask(_sceneName, _sceneIndex);
        });
    }
    /// <summary>
    /// 开始场景以及场景内物体加载的主流程 by吴江
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <param name="_sceneIndex"></param>
    public void StartLoadScene(string _sceneName, int _sceneIndex)
    {
        AddTask(SceneLoadUtil.instance.StartLoadConfig);
        AddTask(GameCenter.cameraMng.Init);
        PlayGameStage stage = GameCenter.curGameStage as PlayGameStage;
        if (stage != null)
        {
            SceneLoadUtil.instance.UnLoadScene();
            AddTask(SceneLoadUtil.instance.StartLoadMapConfig);
            AddTask(stage.BuildPool);
            AddTask(stage.BuildOPCs);
            AddTask(stage.BuildItems);
            AddTask(stage.BuildModels);
            AddTask(stage.BuildNPCs);
            AddTask(stage.BuildMonsters);
            AddTask(stage.BuildTraps);
            AddTask(SceneLoadUtil.instance.StartLoadResources);
            AddTask(stage.BuildMainPlayer);//主角要在特效之前,因为技能预加载
            AddTask(EffectLoadUtil.instance.StartLoadConfig);
            AddTask(EffectLoadUtil.instance.StartLoadResources);
            AddTask(stage.BuildMainEntourage);
            AddTask(UnloadUseness);
            AddTask(GameCenter.cameraMng.InitMap);
            AddTask(stage.InitFlyPoint);
        }
        else
        {
            Debug.LogError("当前游戏控制台为空！");
        }
        if (stage != null)
        {
            AddTask(stage.Wait);//用这个方式取代之前的invoke 0.5秒延时关闭load界面 by界面
        }

        ActTask(_sceneName, _sceneIndex);
		SceneRef sceneRef = GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef;
		if(sceneRef != null)
			cullDistance = sceneRef.autofight_distance;
    }
    /// <summary>
    /// 卸载不需要的 by吴江
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <param name="_sceneIndex"></param>
    /// <param name="_callBack"></param>
    protected void UnloadUseness(string _sceneName, int _sceneIndex, Action _callBack)
    {
        SceneRoot sceneRoot = SceneRoot.GetInstance();
        if (sceneRoot != null)
        {
            sceneRoot.InitDirectionalLightLayer();
            sceneRoot.DirectionalLightActive(SystemSettingMng.RealTimeShadow);
        }

        List<SceneConfigData> list = new List<SceneConfigData>();
        list.Add(EffectLoadUtil.instance.LastConfigList);
        list.Add(SceneLoadUtil.instance.LastConfigList);
        LoadUtil.UnLoadUseness(list);
        Resources.UnloadUnusedAssets();
        sceneRoot = null;
        GC.Collect();
        if (_callBack != null) _callBack();
    }
    /// <summary>
    /// 开始加载资源
    /// </summary>
    /// <param name="scene"></param>
    public void DoStartLoad(int scene)
    {
        monsterCacheEffectResState.Clear();
        if (scene == 0)
        {
            List<string> noneed = new List<string>();
            AssetMng.instance.UpdateCache(noneed);
            GameCenter.instance.GoPassWord();
            return;
        }

        SceneRef sceneRef = ConfigMng.Instance.GetSceneRef(scene);
        if (sceneRef != null)
        {
            StartLoadScene(sceneRef.res_prefab, scene);
        }
        else
        {
            Debug.LogError("can not find scene scene = " + scene);
        }
    }


    protected virtual void OnSceneStateChange(bool _succeed)
    {
        if (_succeed)
        {

            GameCenter.uIMng.ReleaseGUI(GUIType.LOADING);
			
			if(GameCenter.duplicateMng.openCopyForceTip){
				GameCenter.duplicateMng.openCopyForceTip = false;
				GameCenter.uIMng.SwitchToUI(GUIType.FORCETIP);
			}
            //if (GameCenter.sceneAnimMng.RestAnimCount == 0)
            //{
            GameCenter.cameraMng.BlackCoverAll(false);
			GameCenter.cameraMng.curLogicMapTex2D = null;
            //}
            Resources.UnloadUnusedAssets();
			GameCenter.uIMng.GenGUI(GUIType.FUNCTION, true);
			
			GameCenter.uIMng.OpenFuncShowMenu(false);

            if (GameCenter.mainPlayerMng.MainPlayerInfo.LastSceneID == 160001)
            {
                GameCenter.uIMng.SwitchToUI(GUIType.ARENE);
            }
            if (GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType == SceneUiType.BATTLEFIGHT)
            {
                GameCenter.uIMng.GenGUI(GUIType.BATTLECOMENTDES, true);
            }
        }
    }


    protected SceneRoot curSceneRoot = null;
    public void SetSceneEffects(SceneRoot _sr)
    {
        curSceneRoot = _sr;
    }

    public void LoadSceneEffects()
    {
        if (curSceneRoot != null)
        {
            for (int i = 0; i < curSceneRoot.effecthelpers.Length; i++)
            {
                SceneEffectHelper help = curSceneRoot.effecthelpers[i];
				if(help == null)continue;
                AssetMng.GetEffectInstance(help.EffectName, (x) =>
                    {
						if (x != null && help != null)
                        {
                            x.transform.parent = help.transform;
                            x.transform.localPosition = Vector3.zero;
                            x.transform.localScale = Vector3.one;
                        }
                    });
            }
            curSceneRoot = null;
        }
    }


    #endregion



    /// <summary>
    /// 固定帧执行  by吴江
    /// </summary>
    protected override void Update()
    {
        base.Update();
        if (sceneMng != null && sceneMng.EnterSucceed && Time.frameCount % 10 == 0) // 这个方法不需要太过频繁执行 by吴江
        {
            DisplayMobs();
            DisplayTraps();
            if (Time.frameCount % 30 == 0)
            {
                DisplayNPCs();
                DisplayFlyPoints();
                DisplayOPCs();
                DisplayEntourages();
                DisplayDropItems();
                DisplaySceneItems();
                DisplayModels();
                PreLoadMonstersAbility();
                PreLoadOtherPlayerAbility();
            }
        }
    }


    #region 创建虚拟体和创建实体

    protected void CleanAll()
    {
        for (int i = 0; i < mobList.Count; i++)
        {
            mobList[i].UnRegist();
            mobList[i].DestroySelf();
            mobList[i] = null;
        }
        mobList.Clear();

        for (int i = 0; i < opcList.Count; i++)
        {
            opcList[i].UnRegist();
            opcList[i].DestroySelf();
            opcList[i] = null;
        }
        opcList.Clear();

        for (int i = 0; i < sceneItemList.Count; i++)
        {
            sceneItemList[i].UnRegist();
            sceneItemList[i].DestroySelf();
            sceneItemList[i] = null;
        }
        sceneItemList.Clear();

        for (int i = 0; i < entourageList.Count; i++)
        {
            entourageList[i].UnRegist();
            entourageList[i].DestroySelf();
            entourageList[i] = null;
        }
        entourageList.Clear();


        for (int i = 0; i < dropItemList.Count; i++)
        {
            dropItemList[i].UnRegist();
            dropItemList[i].DestroySelf();
            dropItemList[i] = null;
        }
        dropItemList.Clear();

        for (int i = 0; i < trapList.Count; i++)
        {
            trapList[i].UnRegist();
            trapList[i].DestroySelf();
            trapList[i] = null;
        }
        trapList.Clear();

        for (int i = 0; i < modelList.Count; i++)
        {
            modelList[i].UnRegist();
            modelList[i].DestroySelf();
            modelList[i] = null;
        }
        modelList.Clear();
    }

    #region 主角和主随从
    protected void BuildMainEntourage()
    {
        BuildMainEntourage(string.Empty, 0, null);
    }
    /// <summary>
    /// 创建主玩家随从对象 by吴江
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <param name="_sceneIndex"></param>
    /// <param name="_callBack"></param>
    protected void BuildMainEntourage(string _sceneName, int _sceneIndex, Action _callBack)
    {
        if (GameCenter.curMainEntourage != null &&
            (GameCenter.mercenaryMng.curMercernaryInfo == null || GameCenter.curMainEntourage.id != GameCenter.mercenaryMng.curMercernaryInfo.ServerInstanceID || GameCenter.curMainEntourage.isDead))
        {
            GameObject.DestroyImmediate(GameCenter.curMainEntourage.gameObject);
            GameCenter.curMainEntourage = null;
        }
        if (GameCenter.curMainEntourage == null)
        {
            if (GameCenter.mercenaryMng.curMercernaryInfo != null)
            {
                MainEntourage mpc = MainEntourage.CreateDummy(GameCenter.mercenaryMng.curMercernaryInfo);
                mpc.StartAsyncCreate((x) =>
                {
                    GameObject.DontDestroyOnLoad(x);
                    GameCenter.curMainEntourage = x;
                    GameCenter.abilityMng.SetEntourage(x, GameCenter.mercenaryMng.curMercernaryInfo);


                    if (_callBack != null)
                    {
                        _callBack();
                    }
                });
            }
            else
            {
                if (_callBack != null)
                {
                    _callBack();
                }
            }
        }
        else
        {
            GameCenter.curMainEntourage.InitPos();
            if (GameCenter.curMainEntourage.fxCtrl != null) GameCenter.curMainEntourage.fxCtrl.CleanBuffEffects();
            GameCenter.curMainEntourage.SetInfoInCombat(false);
            GameCenter.curMainEntourage.gameStage = this;
            OnPositionChanged(GameCenter.curMainEntourage);
            if (_callBack != null)
            {
                _callBack();
            }
        }
    }
    /// <summary>
    /// 创建主玩家对象 by吴江
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <param name="_sceneIndex"></param>
    /// <param name="_callBack"></param>
    protected void BuildMainPlayer(string _sceneName, int _sceneIndex, Action _callBack)
    {
        if (GameCenter.curMainPlayer != null && GameCenter.curMainPlayer.id != GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
        {
            GameObject.DestroyImmediate(GameCenter.curMainPlayer.gameObject);
            GameCenter.curMainPlayer = null;
            curMainPlayer = GameCenter.curMainPlayer;
            mainPlayerCacheEffectResState.Clear();
            mainPlayerEnableSkillResState.Clear();
            mainPlayerBoneEffectResState.Clear();
        }
        if (GameCenter.curMainPlayer == null)
        {
            GameCenter.curMainPlayer = MainPlayer.CreateDummy(GameCenter.mainPlayerMng.MainPlayerInfo);
            curMainPlayer = GameCenter.curMainPlayer;

            curMainPlayer.StartAsyncCreate((x) =>
            {
                if (!curMainPlayer.actorInfo.IsHide)
                {
                    GameCenter.cameraMng.FocusOn(x, 0);
                }
                else
                {
                    curMainPlayer.Show(false);
                }

                GameObject.DontDestroyOnLoad(x);

                OnRefreshDefaultSkill();
                OnRefreshEnbleSkill();
                OnRefreshBoneEffect();
                OnRefereshMountBoneEffect();


                //GameCenter.uIMng.GenGUI(GUIType.CONTINU_HIT, false);
                //GameCenter.uIMng.GenGUI(GUIType.DEADWND, false);
                if (_callBack != null)
                {
                    _callBack();
                }
            });
        }
        else
        {
            curMainPlayer = GameCenter.curMainPlayer;
            GameCenter.curMainPlayer.ReBindInfo();
            GameCenter.curMainPlayer.InitPos();
            if (GameCenter.curMainPlayer.fxCtrl != null) GameCenter.curMainPlayer.fxCtrl.CleanBuffEffects();
            GameCenter.curMainPlayer.gameStage = this;
            OnPositionChanged(GameCenter.curMainPlayer);
            if (_callBack != null)
            {
                _callBack();
            }
        }
        MainPlayerInfo mpInfo = GameCenter.mainPlayerMng.MainPlayerInfo;
        if (mpInfo != null)
        {
            mpInfo.OnCurShowEquipUpdate += OnRefreshBoneEffect;
            if (mpInfo.CurMountInfo != null)
            {
                mpInfo.CurMountInfo.OnEquipUpdate += OnRefereshMountBoneEffect;
                mpInfo.CurMountInfo.OnRideStateUpdate += OnRefereshMountBoneEffect;
            }
        }
    }
    /// <summary>
    /// 主玩家当前激活的技能更新
    /// </summary>
    public void OnRefreshEnbleSkill()
    {
        List<string> strs = GameCenter.skillMng.GetAbilityEffectNames();
        foreach (var item in strs)
        {
            if (!mainPlayerEnableSkillResState.ContainsKey(item))//加上没有包含的
            {
                mainPlayerEnableSkillResState[item] = false;
            }
        }
        List<string> keys = new List<string>(mainPlayerEnableSkillResState.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            if (!strs.Contains(keys[i]))//删除多于的
            {
                mainPlayerEnableSkillResState.Remove(keys[i]);
                continue;
            }

            }
    }

    public void OnPreloadEnableSkill()
    {
        if (!GameCenter.sceneMng.EnterSucceed) return;
        OnRefreshEnbleSkill();
        List<string> keys = new List<string>(mainPlayerEnableSkillResState.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            string item = keys[i];
            if (!mainPlayerEnableSkillResState[item])
            {
                Action preLoad = Utils.Functor<string>(item, (z) =>
                {
                    AssetMng.GetEeffctAssetObject(z, (x) =>
                    {
                        if (x != null && mainPlayerEnableSkillResState.ContainsKey(z))
                        {
                            mainPlayerEnableSkillResState[z] = true;
                        }
                    });
                });
                preLoad();
            }
        }
    }

    public void OnRefreshDefaultSkill()
    {
        List<string> list = GameCenter.abilityMng.GetDefaultAbilityEffectNames();
        if (list != null)
        {
            foreach (var item in list)
            {
                if (item != "0" && !mainPlayerCacheEffectResState.ContainsKey(item))
                    mainPlayerCacheEffectResState[item] = false;
            }
            List<string> already = new List<string>(mainPlayerCacheEffectResState.Keys);
            foreach (var item in already)
            {
                if (!list.Contains(item))
                {
                    mainPlayerCacheEffectResState.Remove(item);
                }
            }
        }
    }

    public void OnRefereshMountBoneEffect(bool _ride)
    {
        if (_ride)
        {
            OnRefereshMountBoneEffect();
        }
    }

    public void OnRefereshMountBoneEffect()
    {
        mainMountBoneEffectResState.Clear();
        MountInfo info = GameCenter.mainPlayerMng.MainPlayerInfo.CurMountInfo;
        if (info == null) return;
        List<BoneEffectRef> list = info.BoneEffectList;
        for (int i = 0; i < list.Count; i++)
        {
			BoneEffectRef bone = list[i];
			if (!string.IsNullOrEmpty(bone.effectName))
            {
				mainMountBoneEffectResState[bone.effectName] = false;
            }
        }
    }

    public void OnRefreshBoneEffect()
    {
        mainPlayerBoneEffectResState.Clear();
        MainPlayerInfo info = GameCenter.mainPlayerMng.MainPlayerInfo;
        if (info != null)
        {
            foreach (var item in info.CurShowDictionary.Values)
            {
                if (item != null && item.BoneEffectList.Count > 0)
                {
                    foreach (var effect in item.BoneEffectList)
                    {
                        mainPlayerBoneEffectResState[effect.effectName] = false;
                    }
                }
            }
        }
    }

    public void OnRefreshBoneEffect(EquipSlot _slot)
    {
        OnRefreshBoneEffect();
    }
    #endregion

    #region 怪物
    /// <summary>
    /// 更新怪物名字
    /// </summary>
    public void UpdateMonsterName()
    {
        for (int i = 0; i < mobList.Count; i++)
        {
            mobList[i].UpdateName();
        }

    }

    protected List<Monster> mobList = new List<Monster>();
    /// <summary>
    /// 创建怪物 by吴江
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <param name="_sceneIndex"></param>
    /// <param name="_callBack"></param>
    protected void BuildMonsters(string _sceneName, int _sceneIndex, Action _callBack)
    {
        BuildMonsters();
        if (_callBack != null) _callBack();
    }
    /// <summary>
    /// 创建怪物 by吴江
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <param name="_sceneIndex"></param>
    /// <param name="_callBack"></param>
    protected void BuildMonsters()
    {
        if (sceneMng == null || sceneMng.MobInfoDictionary == null) return;
        foreach (MonsterInfo item in sceneMng.MobInfoDictionary.Values)
        {
            Monster mob = GetObject(ObjectType.MOB, item.ServerInstanceID) as Monster;
            if (mob == null && item.IsAlive)
            {
                mobList.Add(Monster.CreateDummy(item));
                //if (item.SpecialAbility != null)
                //{
                //    foreach (var skill in item.SpecialAbility)
                //    {
                //        if (skill == 0) break;
                //        DSSkillLevelRef dSSkillLevelRef = ConfigMng.Instance.GetDSSkillLevelRef(skill, 1);
                //        if (dSSkillLevelRef == null) continue;
                //        if (dSSkillLevelRef.effectAtkRes != "0" && !monsterCacheEffectResState.ContainsKey(dSSkillLevelRef.effectAtkRes))
                //        {
                //            monsterCacheEffectResState[dSSkillLevelRef.effectAtkRes] = false;
                //        }
                //        if (dSSkillLevelRef.effectDefRes != "0" && !monsterCacheEffectResState.ContainsKey(dSSkillLevelRef.effectDefRes))
                //        {
                //            monsterCacheEffectResState[dSSkillLevelRef.effectDefRes] = false;
                //        }
                //    }
                //}
                //if (item.NormalAbility > 0)
                //{
                //    DSSkillLevelRef dSSkillLevelRef = ConfigMng.Instance.GetDSSkillLevelRef(item.NormalAbility, 1);
                //    if (dSSkillLevelRef == null) continue;
                //    if (dSSkillLevelRef.effectAtkRes != "0" && !monsterCacheEffectResState.ContainsKey(dSSkillLevelRef.effectAtkRes))
                //    {
                //        monsterCacheEffectResState[dSSkillLevelRef.effectAtkRes] = false;
                //    }
                //    if (dSSkillLevelRef.effectDefRes != "0" && !monsterCacheEffectResState.ContainsKey(dSSkillLevelRef.effectDefRes))
                //    {
                //        monsterCacheEffectResState[dSSkillLevelRef.effectDefRes] = false;
                //    }
                //}
            }
            else
            {
            }
        }
    }


    /// <summary>
    /// 展示怪物 by吴江
    /// </summary>
    public void DisplayMobs()
    {
        if (curMainPlayer == null) return;
        for (int i = 0; i < mobList.Count; i++)
        {
            Monster mob = mobList[i];
			if (!SystemSettingMng.CullingShow || IsSectorInRange(mob.curSector, curMainPlayer.curSector, cullDistance))//如果在可见范围内 by吴江
            {
                if (mob.isDummy && !mob.isDownloading)
                {
                    mob.isDownloading = true;
                    mob.StartAsyncCreate(!mob.IsActor);
                    break;
                }
                else if (!mob.isDummy && !mob.IsShowing)
                {
                    mob.Show(true);
                }
            }
            else
            {
                if (!mob.isDummy && mob.IsShowing)
                {
                    mob.Show(false);
                }
            }
        }
    }
    /// <summary>
    /// 预加载怪物技能特效 by吴江
    /// </summary>
    protected void PreLoadMonstersAbility()
    {
        //foreach (var item in monsterCacheEffectResState)
        //{
        //    if (!item.Value)
        //    {
        //        Action preLoad = Utils.Functor<string>(item.Key, (z) => {
        //            AssetMng.GetEeffctAssetObject(z, (x) =>
        //            {
        //                if (x != null) monsterCacheEffectResState[z] = true;
        //            });
        //        });
        //        preLoad();
        //        break;
        //    }
        //}
    }
    #endregion

    #region 其他玩家
    /// <summary>
    /// 更新其它玩家称号  by黄洪兴
    /// </summary>
    public void UpdateOtherPlayerTitle()
    {
        for (int i = 0; i < opcList.Count; i++)
        {
            opcList[i].UpdateItemName();
        }
    }

    /// <summary>
    /// 更新其它玩家技能特效显示 by黄洪兴
    /// </summary>
    public void UpdateOtherPlayerSkillEffect()
    {
        for (int i = 0; i < opcList.Count; i++)
        {
            if (opcList[i].fxCtrl!=null)
            opcList[i].fxCtrl.SetHide(!GameCenter.systemSettingMng.OtherPlayerSkill);
        }
    }


    public void RefreshOtherPlayerWingAndMagic()
    {
        for (int i = 0; i < opcList.Count; i++)
        {
            opcList[i].actorInfo.RefreshWingAndMagic();
        }

    }

    protected List<OtherPlayer> opcList = new List<OtherPlayer>();
    /// <summary>
    /// 创建其他玩家 by吴江
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <param name="_sceneIndex"></param>
    /// <param name="_callBack"></param>
    protected void BuildOPCs(string _sceneName, int _sceneIndex, Action _callBack)
    {
        BuildOPCs();
        if (_callBack != null) _callBack();
    }
    /// <summary>
    /// 创建其他玩家 by吴江
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <param name="_sceneIndex"></param>
    /// <param name="_callBack"></param>
    protected void BuildOPCs()
    {
        if (sceneMng == null || sceneMng.OPCInfoDictionary == null) return;
        foreach (OtherPlayerInfo item in sceneMng.OPCInfoDictionary.Values)
        {
            OtherPlayer opc = GetObject(ObjectType.Player, item.ServerInstanceID) as OtherPlayer;
            if (opc == null)
            {
                opcList.Add(OtherPlayer.CreateDummy(item));

                //if (sceneType != SceneType.CITY)
                //{
                //    //主城不需要预加载其他玩家的技能 by吴江
                //    if (item.SkillList != null)
                //    {
                //        foreach (var skill in item.SkillList)
                //        {
                //            if (skill == 0) break;
                //            DSSkillLevelRef dSSkillLevelRef = ConfigMng.Instance.GetDSSkillLevelRef(skill, 1);
                //            if (dSSkillLevelRef == null) continue;
                //            if (dSSkillLevelRef.effectAtkRes != "0" && !otherPlayerEffectResState.ContainsKey(dSSkillLevelRef.effectAtkRes))
                //            {
                //                otherPlayerEffectResState[dSSkillLevelRef.effectAtkRes] = false;
                //            }
                //            if (dSSkillLevelRef.effectDefRes != "0" && !otherPlayerEffectResState.ContainsKey(dSSkillLevelRef.effectDefRes))
                //            {
                //                otherPlayerEffectResState[dSSkillLevelRef.effectDefRes] = false;
                //            }
                //        }
                //    }
                //}
            }
            else
            {

            }
        }
    }
    /// <summary>
    /// 需要显示的OPC列表 
    /// </summary>
    protected List<int> realOpcInfoList = new List<int>();
    public void UpdateOtherPlayerList()
    {
        int _maxOpc = GameCenter.systemSettingMng.MaxPlayer;
        realOpcInfoList.Clear();
        if (_maxOpc == 0)
        {
            return;
        }
        List<int> opcInfoList = new List<int>();
        int count = 0;
        for (int i = 0; i < opcList.Count; i++)
        {
             if (!opcList[i].IsHide)
            {
                opcInfoList.Add(opcList[i].actorInfo.ServerInstanceID);
            }
        }

        for (int i = 0; i < opcInfoList.Count; i++)
        {
            if (count >= _maxOpc)
                break;
            if (!realOpcInfoList.Contains(opcInfoList[i]))
            {
                realOpcInfoList.Add(opcInfoList[i]);
                count += 1;
            }
        }
    }
    /// <summary>
    /// 展示opc by吴江
    /// </summary>
    public void DisplayOPCs()
    {
        if (curMainPlayer == null) return;
        UpdateOtherPlayerList();
        for (int i = 0; i < opcList.Count; i++)
        {
            OtherPlayer opc = opcList[i];
            if (!SystemSettingMng.CullingShow || IsSectorInRange(opc.curSector, curMainPlayer.curSector, opc.cullDistance))//如果在可见范围内 by吴江
            {
                if (realOpcInfoList.Contains(opc.actorInfo.ServerInstanceID))//需要显示的玩家列表中有此OPC,做对应处理
                {
                    if (opc.isDummy && !opc.isDownloading)
                    {
                        opc.isDownloading = true;
                        opc.StartAsyncCreate();
                        break;
                    }
                    else if (!opc.isDummy && !opc.IsShowing)
                    {
						if(opc.actorInfo.IsHide == false)//加了隐藏buff的玩家不需要显示
							opc.Show(true);
                    }
                }
                else                                                                //需要显示的玩家列表中无此OPC,直接隐藏
                {
                    if (opc.IsShowing)
                    {
                        opc.Show(false);
                    }
                }
            }
            else
            {
                if (!opc.isDummy && opc.IsShowing)
                {
                    opc.Show(false);
                }
            }
        }
    }
    /// <summary>
    /// 预加载玩家技能特效 by吴江
    /// </summary>
    protected void PreLoadOtherPlayerAbility()
    {
        //if (sceneType == SceneType.CITY) return;//主城不需要预加载其他玩家的技能 by吴江
        //foreach (var item in otherPlayerEffectResState)
        //{
        //    if (!item.Value)
        //    {
        //        Action preLoad = Utils.Functor<string>(item.Key, (z) =>
        //        {
        //            AssetMng.GetEeffctAssetObject(z, (x) =>
        //            {
        //                if (x != null)
        //                {
        //                    otherPlayerEffectResState[z] = true;
        //                }
        //            });
        //        });
        //        preLoad();
        //        break;
        //    }
        //}
    }
    #endregion

    #region NPC
    protected List<NPC> npcList = new List<NPC>();
    /// <summary>
    /// 创建NPC by吴江
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <param name="_sceneIndex"></param>
    /// <param name="_callBack"></param>
    protected void BuildNPCs(string _sceneName, int _sceneIndex, Action _callBack)
    {
        BuildNPCs();
        if (_callBack != null) _callBack();
    }
    /// <summary>
    /// 创建NPC by吴江
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <param name="_sceneIndex"></param>
    /// <param name="_callBack"></param>
    protected void BuildNPCs()
    {
        if (sceneMng == null || sceneMng.NPCInfoDictionary == null) return;
        foreach (NPCInfo item in sceneMng.NPCInfoDictionary.Values)
        {
            NPC npc = GetObject(ObjectType.NPC, item.ServerInstanceID) as NPC;
            if (npc == null)
            {
                npcList.Add(NPC.CreateDummy(item));
            }
            else
            {
            }
        }
    }
    /// <summary>
    /// 展示npc by吴江
    /// </summary>
    public void DisplayNPCs()
    {
        if (curMainPlayer == null) return;
        for (int i = 0; i < npcList.Count; i++)
        {
            NPC npc = npcList[i];
            if (!SystemSettingMng.CullingShow || IsSectorInRange(npc.curSector, curMainPlayer.curSector, npc.cullDistance))//如果在可见范围内 by吴江
            {
                if (npc.isDummy && !npc.isDownloading)
                {
                    npc.isDownloading = true;
                    npc.StartAsyncCreate((x) =>
                    {
                    });
                    break;
                }
                else if (!npc.isDummy && !npc.IsShowing)
                {
                    npc.Show(true);
                }
            }
            else
            {
                if (!npc.isDummy && npc.IsShowing)
                {
                    npc.Show(false);
                }
            }
        }
    }
    /// <summary>
    /// 刷新智能NPC的显隐 by吴江
    /// </summary>
    protected void RefreshSmartNPCsShowHide(TaskDataType _datatype, TaskType _type)
    {
        if (_type == TaskType.Branch || _type == TaskType.Main)
        {
            GameCenter.sceneMng.UpdateSceneNPCInstance();
        }
    }
    #endregion

    #region 场景物品
    protected List<SceneItem> sceneItemList = new List<SceneItem>();
    /// <summary>
    /// 创建场景物品 by吴江
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <param name="_sceneIndex"></param>
    /// <param name="_callBack"></param>
    protected void BuildItems(string _sceneName, int _sceneIndex, Action _callBack)
    {
        BuildItems();
        if (_callBack != null) _callBack();
    }
    /// <summary>
    /// 创建场景物品 by吴江
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <param name="_sceneIndex"></param>
    /// <param name="_callBack"></param>
    protected void BuildItems()
    {
        if (sceneMng == null || sceneMng.SceneItemInfoDictionary == null) return;
        foreach (SceneItemInfo item in sceneMng.SceneItemInfoDictionary.Values)
        {
            SceneItem trigger = GetObject(ObjectType.SceneItem, item.ServerInstanceID) as SceneItem;
            if (trigger == null && item.IsAlive)
            {
                sceneItemList.Add(SceneItem.CreateDummy(item));
            }
            else
            {

            }
        }
    }


    /// <summary>
    /// 展示场景触物品 by吴江
    /// </summary>
    public void DisplaySceneItems()
    {
        if (curMainPlayer == null) return;
        for (int i = 0; i < sceneItemList.Count; i++)
        {
            SceneItem sceneItem = sceneItemList[i];
            if (!SystemSettingMng.CullingShow || IsSectorInRange(sceneItem.curSector, curMainPlayer.curSector, sceneItem.cullDistance))//如果在可见范围内 by吴江
            {
                if (sceneItem.isDummy && !sceneItem.isDownloading)
                {
                    sceneItem.isDownloading = true;
                    sceneItem.StartAsyncCreate();
                    break;
                }
                else if (!sceneItem.isDummy)
                {
                    sceneItem.gameObject.SetActive(true);
                }
            }
            else
            {
                if (!sceneItem.isDummy)
                {
                    sceneItem.gameObject.SetActive(false);
                }
            }

        }
    }
    #endregion

    #region 其他随从
    protected List<OtherEntourage> entourageList = new List<OtherEntourage>();
    /// <summary>
    /// 创建随从 by吴江
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <param name="_sceneIndex"></param>
    /// <param name="_callBack"></param>
    protected void BuildEntourages(string _sceneName, int _sceneIndex, Action _callBack)
    {
        BuildEntourages();
        if (_callBack != null) _callBack();
    }
    /// <summary>
    /// 创建随从 by吴江
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <param name="_sceneIndex"></param>
    /// <param name="_callBack"></param>
    protected void BuildEntourages()
    {
        if (sceneMng == null || sceneMng.EntourageInfoDictionary == null) return;
        foreach (MercenaryInfo item in sceneMng.EntourageInfoDictionary.Values)
        {
            OtherEntourage entourage = GetObject(ObjectType.Entourage, item.ServerInstanceID) as OtherEntourage;
            if (entourage == null)
            {
                entourageList.Add(OtherEntourage.CreateDummy(item));
            }
            else
            {

            }
        }
    }

    /// <summary>
    /// 展示opc by吴江
    /// </summary>
    public void DisplayEntourages()
    {
        if (curMainPlayer == null) return;
        for (int i = 0; i < entourageList.Count; i++)
        {
            OtherEntourage ope = entourageList[i];
            if (!SystemSettingMng.CullingShow || IsSectorInRange(ope.curSector, curMainPlayer.curSector, ope.cullDistance))//如果在可见范围内 by吴江
            {
                if (ope.OwnerPlayer != null && !ope.OwnerPlayer.isDummy && ope.OwnerPlayer.IsShowing)//主人显示,随从才显示
                {
                    if (ope.isDummy && !ope.isDownloading)
                    {
                        ope.isDownloading = true;
                        ope.StartAsyncCreate();
                        break;
                    }
                    else if (!ope.isDummy && !ope.IsShowing)
                    {
                        ope.Show(true);
                    }
                }
                else                                                                //主人隐藏则隐藏
                {
                    if (ope.IsShowing)
                    {
                        ope.Show(false);
                    }
                }


                if (!GameCenter.systemSettingMng.OtherPlayerPet)
                {
                    ope.Show(false);
                }

            }
            else
            {
                if (!ope.isDummy && ope.IsShowing)
                {
                    ope.Show(false);
                }
            }
        }
    }


    void UpdateOtherPlayerPetShow()
    {
        for (int i = 0; i < entourageList.Count; i++)
        {
            entourageList[i].Show(GameCenter.systemSettingMng.OtherPlayerPet);            
        }

    }


    #endregion

    #region 掉落物品
    /// <summary>
    /// 更新场景掉落物品名字
    /// </summary>
    public void UpdateItemName()
    {
        for (int i = 0; i < dropItemList.Count; i++)
        {
            dropItemList[i].UpdateName();
        }
    }
    protected List<DropItem> dropItemList = new List<DropItem>();
    /// <summary>
    /// 创建掉落物品 by吴江
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <param name="_sceneIndex"></param>
    /// <param name="_callBack"></param>
    protected void BuildDropItems()
    {
        if (sceneMng == null || sceneMng.DropItemDictionary == null) return;
        foreach (DropItemInfo item in sceneMng.DropItemDictionary.Values)
        {
            DropItem mob = GetObject(ObjectType.DropItem, item.ServerInstanceID) as DropItem;
            if (mob == null)
            {
                dropItemList.Add(DropItem.CreateDummy(item));
            }
            else
            {
            }
        }
    }
    /// <summary>
    /// 展示场景掉落物品 by吴江
    /// </summary>
    public void DisplayDropItems()
    {
        if (curMainPlayer == null) return;
        for (int i = 0; i < dropItemList.Count; i++)
        {
            DropItem dropItem = dropItemList[i];
            if (!SystemSettingMng.CullingShow || IsSectorInRange(dropItem.curSector, curMainPlayer.curSector, dropItem.cullDistance))//如果在可见范围内 by吴江
            {
                if (dropItem.isDummy && !dropItem.isDownloading)
                {
                    dropItem.isDownloading = true;
                    dropItem.StartAsyncCreate();
                }
                else if (!dropItem.isDummy)
                {
                    dropItem.gameObject.SetActive(true);
                }
            }
            else
            {
                if (!dropItem.isDummy)
                {
                    dropItem.gameObject.SetActive(false);
                }
            }
        }
    }
    #endregion

    #region 陷阱
    protected List<Trap> trapList = new List<Trap>();
    /// <summary>
    /// 创建陷阱 by吴江
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <param name="_sceneIndex"></param>
    /// <param name="_callBack"></param>
    protected void BuildTraps(string _sceneName, int _sceneIndex, Action _callBack)
    {
        BuildTraps();
        if (_callBack != null) _callBack();
    }
    /// <summary>
    /// 创建陷阱 by吴江
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <param name="_sceneIndex"></param>
    /// <param name="_callBack"></param>
    protected void BuildTraps()
    {
        if (sceneMng == null || sceneMng.TrapInfoDictionary == null) return;
        foreach (TrapInfo item in sceneMng.TrapInfoDictionary.Values)
        {
            if (item.IsDead) continue;
            Trap trap = GetObject(ObjectType.Trap, item.InstanceID) as Trap;
            if (trap == null)
            {
                trapList.Add(Trap.CreateDummy(item));
            }
            else
            {
            }
        }
    }

    /// <summary>
    /// 展示陷阱 by吴江
    /// </summary>
    public void DisplayTraps()
    {
        if (curMainPlayer == null) return;
        for (int i = 0; i < trapList.Count; i++)
        {
            Trap trap = trapList[i];
            if (!SystemSettingMng.CullingShow || IsSectorInRange(trap.curSector, curMainPlayer.curSector, trap.cullDistance))//如果在可见范围内 by吴江
            {
                if (trap.isDummy && !trap.isDownloading)
                {
                    trap.isDownloading = true;
                    trap.StartAsyncCreate();
                    break;
                }
                else if (!trap.isDummy)
                {
                    trap.gameObject.SetActive(true);
                }
            }
            else
            {
                if (!trap.isDummy)
                {
                    trap.gameObject.SetActive(false);
                }
            }
        }
    }
    #endregion

    #region 雕像
    protected List<Model> modelList = new List<Model>();
    /// <summary>
    /// 创建场景雕像 by吴江
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <param name="_sceneIndex"></param>
    /// <param name="_callBack"></param>
    protected void BuildModels(string _sceneName, int _sceneIndex, Action _callBack)
    {
        BuildModels();
        if (_callBack != null) _callBack();
    }
    /// <summary>
    /// 创建场景雕像 by吴江
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <param name="_sceneIndex"></param>
    /// <param name="_callBack"></param>
    protected void BuildModels()
    {
        if (sceneMng == null || sceneMng.ModelInfoDictionary == null) return;
        foreach (OtherPlayerInfo item in sceneMng.ModelInfoDictionary.Values)
        {
            Model model = GetObject(ObjectType.Model, item.ServerInstanceID) as Model;
            if (model == null && item.IsAlive)
            {
                modelList.Add(Model.CreateDummy(item));
            }
            else
            {

            }
        }
    }
    /// <summary>
    /// 展示场景触物品 by吴江
    /// </summary>
    public void DisplayModels()
    {
        if (curMainPlayer == null) return;
        for (int i = 0; i < modelList.Count; i++)
        {
            Model model = modelList[i];
            if (!SystemSettingMng.CullingShow || IsSectorInRange(model.curSector, curMainPlayer.curSector, model.cullDistance))//如果在可见范围内 by吴江
            {
                if (model.isDummy && !model.isDownloading)
                {
                    model.isDownloading = true;
                    model.StartAsyncCreate();
                    break;
                }
                else if (!model.isDummy && !model.IsShowing)
                {
                    model.Show(true);
                }
            }
            else
            {
                if (!model.isDummy && model.IsShowing)
                {
                    model.Show(false);
                }
            }

        }
    }
    #endregion

    #region 传送点
    protected List<FlyPoint> flyPointList = new List<FlyPoint>();
    /// <summary>
    /// 初始化传送点 by吴江
    /// </summary>
    protected void InitFlyPoint(string _sceneName, int _sceneIndex, Action _callBack)
    {
        SceneRef sceneRef = ConfigMng.Instance.GetSceneRef(_sceneIndex);
        if (sceneRef == null)
        {
            Debug.LogError("找不到场景" + _sceneIndex + "的客户端配置，无法初始化传送点，请检查！");
            if (_callBack != null)
            {
                _callBack();
            }
            return;
        }
        //仅主城有传送点
        List<FlyPointRef> flyPoints = ConfigMng.Instance.GetFlyPointRefByScene(_sceneIndex);
        for (int i = 0; i < flyPoints.Count; i++)
        {
            flyPointList.Add(FlyPoint.CreateDummy(flyPoints[i]));
        }
        if (_callBack != null)
        {
            _callBack();
        }
    }
    /// <summary>
    /// 展示传送点 by吴江
    /// </summary>
    public void DisplayFlyPoints()
    {
        if (curMainPlayer == null) return;
        for (int i = 0; i < flyPointList.Count; i++)
        {
            FlyPoint flyPoint = flyPointList[i];
            if (!SystemSettingMng.CullingShow || IsSectorInRange(flyPoint.curSector, curMainPlayer.curSector, flyPoint.cullDistance))//如果在可见范围内
            {
                if (flyPoint.isDummy && !flyPoint.isDownloading)
                {
                    flyPoint.isDownloading = true;
                    flyPoint.StartAsyncCreate((x) =>
                    {
                    });
                    break;
                }
                else if (!flyPoint.isDummy && !flyPoint.gameObject.activeSelf)
                {
                    flyPoint.gameObject.SetActive(true);
                }
            }
            else
            {
                if (flyPoint != null && !flyPoint.isDummy && flyPoint.gameObject.activeSelf)
                {
                    flyPoint.gameObject.SetActive(false);
                }
            }
        }
    }
    #endregion

    #region 通用
    protected void DeleteInterObj(ObjectType _type, int _instanceID)
    {
        switch (_type)
        {
            case ObjectType.Player:
                OtherPlayer opc = this.GetOtherPlayer(_instanceID);
                if (realOpcInfoList.Contains(_instanceID))//玩家退出,刷新realOpcInfoList 
                {
                    realOpcInfoList.Remove(_instanceID);
                }
                if (opcList.Contains(opc))
                {
                    opcList.Remove(opc);
                }
                if (opc != null)
                {
                    opc.UnRegist();
                    opc.DelayToDestroy(0.5f);
                }
                break;
            case ObjectType.MOB:
                Monster mob = this.GetMOB(_instanceID);
                if (mobList.Contains(mob))
                {
                    mobList.Remove(mob);
                }
                if (mob != null)
                {
                    mob.UnRegist();
                    mob.DelayToDestroy(0.5f);
                }
                break;
            case ObjectType.NPC:
                NPC npc = this.GetNPC(_instanceID);
                if (npcList.Contains(npc))
                {
                    npcList.Remove(npc);
                }
                if (npc != null)
                {
                    npc.UnRegist();
                    npc.DelayToDestroy(0.5f);
                }
                break;
            case ObjectType.SceneItem:
                SceneItem item = this.GetSceneItem(_instanceID);
                if (sceneItemList.Contains(item))
                {
                    sceneItemList.Remove(item);
                }
                if (item != null)
                {
                    item.UnRegist();
                    item.Dead();
                }
                break;
            case ObjectType.DropItem:
                DropItem dropItem = this.GetDropItem(_instanceID);
//                if (dropItem.OwnerID.Contains((uint)GameCenter.curMainPlayer.id) && !dropItem.IsTimeOut) //特效播放放到c012里面处理 by邓成
//                {
//                    GameCenter.curMainPlayer.DoPickUpEffect(dropItem.transform.position);
//                }
                if (dropItemList.Contains(dropItem))
                {
                    dropItemList.Remove(dropItem);
                }
                if (dropItem != null)
                {
                    dropItem.UnRegist();
                    Destroy(dropItem);
                }
                break;
            case ObjectType.Trap:
                Trap trap = this.GetTrap(_instanceID);
                if (trapList.Contains(trap))
                {
                    trapList.Remove(trap);
                }
                if (trap != null)
                {
                    trap.UnRegist();
                    Destroy(trap);
                }
                break;
            case ObjectType.Entourage:
                EntourageBase Entourage = this.GetEntourage(_instanceID);
                OtherEntourage oe = Entourage as OtherEntourage;
                if (oe != null && entourageList.Contains(oe))
                {
                    entourageList.Remove(oe);
                }
                if (Entourage != null)
                {
                    Entourage.UnRegist();
                    Destroy(Entourage);
                }
                break;
            case ObjectType.Model:
                Model model = this.GetObject(ObjectType.Model, _instanceID) as Model;
                if (modelList.Contains(model))
                {
                    modelList.Remove(model);
                }
                if (model != null)
                {
                    model.UnRegist();
                    Destroy(model);
                }
                break;
            default:
                return;
        }
    }
    /// <summary>
    /// 对象说话，走后台通知的只有怪物，其他的走前台AI   by吴江
    /// </summary>
    /// <param name="_objID"></param>
    /// <param name="_talkID"></param>
    protected void OnObjectTalking(int _objID, int _talkID)
    {
        Actor actor = GameCenter.curGameStage.GetMOB(_objID);
        if (actor == null) actor = GameCenter.curGameStage.GetNPC(_objID);
        if (actor == null) actor = GameCenter.curGameStage.GetSceneItem(_objID);
        if (actor != null)
        {
//            BubbleRef refData = ConfigMng.Instance.GetBubbleRef(_talkID);
//            if (refData != null)
//            {
//                actor.BubbleTalk(refData.content, refData.time);
//            }
        }
    }
    #endregion

    #endregion

    /// <summary>
    /// 初始化对象池 by吴江
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <param name="_sceneIndex"></param>
    /// <param name="_callBack"></param>
    protected void BuildPool(string _sceneName, int _sceneIndex, Action _callBack)
    {
        Spawner spawner = GameCenter.spawner;
        if (spawner != null && !spawner.Inited)
        {
            spawner.Init(_callBack);
            return;
        }
        else
        {
            spawner.Refresh();
            if (_callBack != null)
            {
                _callBack();
            }
        }
    }

    /// <summary>
    /// 自定义延时函数 by吴江
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <param name="_sceneIndex"></param>
    /// <param name="_callBack"></param>
    protected void Wait(string _sceneName, int _sceneIndex, Action _callBack)
    {
        if (this != null)
        StartCoroutine(Wait(0.2f, _callBack));
    }
    /// <summary>
    /// 自定义延时函数 by吴江
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <param name="_sceneIndex"></param>
    /// <param name="_callBack"></param>
    protected IEnumerator Wait(float _time, Action _callBack)
    {
        yield return new WaitForSeconds(_time);
        if (this != null && _callBack != null) _callBack();
    }


    protected List<int> fpsList = new List<int>();
    protected void CheckQuality()
    {
        if (!SystemSettingMng.HasAutoSetQuality)
        {
            if (!hasCheckQuality)
            {
                if (Time.time - startRunTime < startCheckTime)
                {
                    if (Time.time - lastCheckTime >= 1.0f)
                    {
                        fpsList.Add(GameCenter.instance.FPS);
                        lastCheckTime = Time.time;
                    }
                }
                else
                {
                    int outTimeCount = 0;
                    for (int i = 0; i < fpsList.Count; i++)
                    {
                        if (fpsList[i] <= 20)
                        {
                            outTimeCount++;
                        }
                    }
                    if (outTimeCount / (float)fpsList.Count > 0.4f)
                    {
                        GameCenter.messageMng.AddClientMsg(258,
                                delegate
                                {
                                    GameCenter.systemSettingMng.CurRendererQuality = SystemSettingMng.RendererQuality.LOW;
                                    SystemSettingMng.RealTimeShadow = false;
                                    SystemSettingMng.HasAutoSetQuality = true;
                                },
                                delegate
                                {
                                    SystemSettingMng.HasAutoSetQuality = true;
                                });

                    }
                    SystemSettingMng.HasAutoSetQuality = true;
                }
            }
        }
    }


    protected void OnUpdateCmdDictionary()
    {
        if (GameCenter.msgLoackingMng.HasSerializeWaiting)
        {
            GameCenter.uIMng.GenGUI(GUIType.PANELLOADING, true);
        }
        else
        {
            GameCenter.uIMng.ReleaseGUI(GUIType.PANELLOADING);
        }
    }


}
