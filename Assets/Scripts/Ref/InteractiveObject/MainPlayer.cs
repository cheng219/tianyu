//=========================================
//作者：吴江
//日期：2015/5/15
//用途：主玩家表现层对象
//===========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 主玩家表现层对象 by吴江
/// </summary>
public class MainPlayer : PlayerBase
{
    #region 数据
    /// <summary>
    /// 技能管理器 by吴江
    /// </summary>
    [System.NonSerialized]
    public AbilityMng abilityMng;
    /// <summary>
    /// 命令管理器 by吴江
    /// </summary>
    [System.NonSerialized]
    public CommandMng commandMng;
    /// <summary>
    /// 玩家的操作收听组件
    /// </summary>
    [System.NonSerialized]
    public PlayerInputListener inputListener;
    /// <summary>
    /// 当前锁定目标 by吴江
    /// </summary>
    protected InteractiveObject curTarget = null;
    /// <summary>
    /// 是否在自动战斗中
    /// </summary>
    public bool IsInAutoFight
    {
        get
        {
            return aiFithtFSM == null ? false : aiFithtFSM.IsInAutoFIght;
        }
    }
    public bool IsInAutoFightBeforeDead = false;//死亡前的状态

    public bool NotFighting
    {
        get
        {
            return aiFithtFSM == null ? false : aiFithtFSM.NotFighting;
        }
    }

    /// <summary>
    /// 是否已经激活操作
    /// </summary>
    public bool hasCtrlAwake = false;

    /// <summary>
    /// 当前锁定目标 by吴江
    /// </summary>
    public InteractiveObject CurTarget
    {
        get
        {
            return curTarget;
        }
        set
        {
            if (curTarget != value)
            {
                if (curTarget != null && curTarget.fxCtrl != null)
                {
                    curTarget.DoRingEffect(Color.yellow, false, 1);
                }
                SmartActor sm = value as SmartActor;
                if (sm != null && sm.IsActor)//演员不能选中
                {
                    return;
                }
                if (sm != null && sm.typeID == ObjectType.Player)
                {
                    OtherPlayer opc = sm as OtherPlayer;
					if (opc == null)
                    {
                        return;
                    }
                }
                curTarget = value;
                if (curTarget != null && curTarget.fxCtrl != null)
                {
                    float radius = 1.0f;
                    if (curTarget.typeID == ObjectType.MOB) //如果是怪物，要根据碰撞对光圈缩放
                    {
                        MonsterInfo info = GameCenter.sceneMng.GetMobInfo(curTarget.id);
                        if (info != null)
                        {
                            radius = info.RingSize;
                        }
                    }
                    curTarget.DoRingEffect(GetRelationColor(curTarget), true, radius);
                }
                if (OnTargetChange != null)
                {
                    OnTargetChange();
                }
                if (curTarget != null && actorInfo.CurSceneUiType != SceneUiType.ARENA && actorInfo.CurSceneUiType != SceneUiType.BUDOKAI)//竞技场不显示 by邓成
					GameCenter.uIMng.GenGUI(GUIType.MONSTER_HEAD,true);
//                if (abilityMng != null)
//                {
//                    abilityMng.ResetDefaultAbility();切换怪物也不一定重置普攻
//                }
			}else
			{
//				if (curTarget != null && curTarget.fxCtrl != null)
//				{
//					curTarget.DoRingEffect(Color.yellow, false, 1);
//				}
				//注销掉,会导致光圈看不到  by邓成
			}
        }
    }


    public bool isCastingNoMove
    {
        get
        {
            if (animFSM == null || moveFSM == null) return true;
            return animFSM.IsCasting || moveFSM.isMoveLocked;
        }
    }


    /// <summary>
    /// 当前进入的传送点对象 by吴江
    /// </summary>
    public FlyPoint curTrigerFlyPoint = null;

    /// <summary>
    /// 自动战斗时,手动选中的怪物,得改变aiFithtFSM里的thisTarget 
    /// </summary>
    public void SetAiFightTarget(SmartActor _actor)
    {
        if (AttakType != AttackType.NONE)//自动战斗时,手动选中的怪物,得改变aiFithtFSM里的thisTarget
        {
            if (aiFithtFSM != null)
                aiFithtFSM.SetThisTarget(_actor);
        }
    }
	/// <summary>
	/// 打一下怪物
	/// </summary>
	public void HitTargetOnce(SmartActor _actor)
	{
		if (AttakType != AttackType.NONE)//自动战斗时,手动选中的怪物,得改变aiFithtFSM里的thisTarget
		{
			if (aiFithtFSM != null)
				aiFithtFSM.SetThisTarget(_actor);
		}else
		{
			if(!isRigidity)
			{
				AbilityInstance instance = this.abilityMng.GetNextDefaultAbility(_actor);
				if(instance.RestCD <= 0)
					TryUseAbility(instance,true);
			}else
			{
				
			}
		}
	}

    public TracePoint CurTargetPoint = null;

    public FlyPoint CurClickFlyPoint = null;

    /// <summary>
    /// 寻路到掩码点事件
    /// </summary>
    public Action CannotMoveTo;

    /// <summary>
    /// 当前目标发生变化的事件
    /// </summary>
    public System.Action OnTargetChange;

    protected AttackType attakType = AttackType.NONE;
    /// <summary>
    /// 当前是否自动攻击 by吴江
    /// </summary>
    public AttackType AttakType
    {
        get
        {
            return attakType;
        }
        set
        {
            if (attakType != value)
            {
                attakType = value;
                if (aiFithtFSM != null)
                {
                    if (attakType == AttackType.NONE)
                    {
                        aiFithtFSM.StopStateMachine();
//                        if (abilityMng != null)
//                        {
//                            abilityMng.ResetDefaultAbility();
//                        }
                    }
                    else
                    {
                        aiFithtFSM.StartStateMachine();
                    }
                }
            }
        }
    }


    public enum AttackType
    {
        NONE,
        NORMALFIGHT,//自动战斗
        AUTOFIGHT,//托管
		/// <summary>
		/// 自动完成任务
		/// </summary>
		COMPLETE_TASK,
		/// <summary>
		/// 自动运镖
		/// </summary>
		AUTODART,
    }

    /// <summary>
    /// 完整数据层对象引用  by吴江
    /// </summary>
    public new MainPlayerInfo actorInfo
    {
        get { return base.actorInfo as MainPlayerInfo; }
        set
        {
            base.actorInfo = value;
        }
    }
    /// <summary>
    /// 自动战斗AI状态机
    /// </summary>
    protected PlayerAutoFightFSM aiFithtFSM;
	/// <summary>
	/// 自动运镖状态机
	/// </summary>
	protected PlayerAutoDartFSM dartFSM;





    public System.Action<bool> OnMoveStart;
    #endregion

    #region UNITY
    protected new void Awake()
    {
        base.Awake();
        commandMng = new CommandMng(this);
		
    }
    void Start()
    {
        stateMachine.Start();
        InvokeRepeating("OnAutoFight", 0.0f, 5.5f);
    }
    /// <summary>
    /// 主玩家除了基类的每帧逻辑外，还需要每帧执行一次命令管理器 by吴江
    /// </summary>
    protected new void Update()
    {
        base.Update();
        commandMng.Tick();
        stateMachine.Update();
        UpdateCheck();
        if (Time.frameCount % 10 == 0)
        {
            OnDeath();
        }

    }
    #endregion

    #region 构造
    /// <summary>
    /// 创建净数据对象
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static MainPlayer CreateDummy(MainPlayerInfo _info)
    {
        GameObject newGO = null;
        if (GameCenter.instance.dummyOpcPrefab != null)
        {
            newGO = Instantiate(GameCenter.instance.dummyOpcPrefab) as GameObject;
            newGO.name = "Dummy MPC [" + _info.ServerInstanceID + "]";
        }
        else
        {
            newGO = new GameObject("Dummy MPC[" + _info.ServerInstanceID + "]");
        }
        newGO.tag = "Player";
        newGO.SetMaskLayer(LayerMask.NameToLayer("Player"));
        PlayerInputListener input = newGO.AddComponent<PlayerInputListener>();
        CharacterController cctrl = newGO.AddComponent<CharacterController>();
        cctrl.slopeLimit = 60.0f;
        cctrl.stepOffset = 1f;
        MainPlayer newMPC = newGO.AddComponent<MainPlayer>();
        newMPC.isDummy_ = true;
        newMPC.moveFSM = newGO.AddComponent<MainPlayerMoveFSM>();
        newMPC.id = _info.ServerInstanceID;
        newMPC.actorInfo = _info;
        newMPC.inputListener = input;
        newMPC.curMoveSpeed = newMPC.actorInfo.StaticSpeed * MOVE_SPEED_BASE;
        newMPC.CurRealSpeed = newMPC.curMoveSpeed;
        newMPC.transform.localRotation = new Quaternion(0, _info.RotationY, 0, 0);
        GameCenter.curGameStage.PlaceGameObjectFromServer(newMPC, _info.ServerPos.x, _info.ServerPos.z, (int)newMPC.transform.localEulerAngles.y);
        GameCenter.curGameStage.AddObject(newMPC);
        return newMPC;
    }


    public void StartAsyncCreate(System.Action<MainPlayer> _callback = null)
    {
        inited_ = false;
        StartCoroutine(CreateAsync(_callback));
    }

    IEnumerator CreateAsync(System.Action<MainPlayer> _callback = null)
    {
        if (isDummy_ == false)
        {
            GameSys.LogError("You can only start create other player in dummy: " + actorInfo.ServerInstanceID);
            yield break;
        }
        //
        isDownloading_ = true;				//判断是否正在下载，防止重复创建

        MainPlayer mpc = null;
        pendingDownload = Create(actorInfo, delegate(MainPlayer _mpc, EResult _result)
        {
            if (_result != EResult.Success)
            {
                return;
            }

            mpc = _mpc;
            if (!actorInfo.IsHide) mpc.Show(true);
            pendingDownload = null;
            isDownloading_ = false;
            if (!actorInfo.IsAlive)
            {
                mpc.Dead();
            }
            if (_callback != null)
            {
                _callback(this);
            }
        });
    }

    /// <summary>
    /// 主玩家不需要dummy状态，因此直接创建  by吴江
    /// </summary>
    /// <param name="_data"></param>
    /// <param name="_callback"></param>
    protected AssetMng.DownloadID Create(MainPlayerInfo _data, System.Action<MainPlayer, EResult> _callback)
    {
        return exResources.GetRace((int)_data.Prof,
                                 delegate(GameObject _asset, EResult _result)
                                 {

                                     if (_result != EResult.Success)
                                     {
                                         _callback(null, _result);
                                         return;
                                     }

                                     this.gameObject.name = "Main Player[" + actorInfo.ServerInstanceID + "]";
                                     GameObject newGo = Instantiate(_asset) as GameObject;
                                     animationRoot = newGo.transform;
                                     newGo.name = _asset.name;
                                     newGo.transform.parent = transform;
                                     newGo.transform.localEulerAngles = Vector3.zero;
                                     newGo.transform.localPosition = Vector3.zero;
                                     newGo.AddComponent<PlayerAnimFSM>();
                                     newGo.AddComponent<PlayerRendererCtrl>();
                                     newGo.SetActive(false);
                                     newGo.SetActive(true);
                                     FXCtrl fx = this.gameObject.AddComponent<FXCtrl>();
                                     fx.SetUnLimit(true);

                                     Init();
                                     isDummy_ = false;
                                     _callback(this, _result);
                                 });
    }
    /// <summary>
    /// 初始化
    /// </summary>
    protected override void Init()
    {
        base.Init();
        hasCtrlAwake = false; 
        gameObject.SetMaskLayer(LayerMask.NameToLayer("Player"));
        rendererCtrl.ResetOriginalLayer(LayerMask.NameToLayer("Player"));
        rendererCtrl.ResetLayer();
        headTextCtrl.SetName("[b]" + actorInfo.Name);
        if (actorInfo.GuildName != string.Empty)
        {
            headTextCtrl.SetGuildName(actorInfo.GuildName);
        }
        headTextCtrl.SetTitleSprite(actorInfo.TitleIcon);
        if (aiFithtFSM == null)
        {
            aiFithtFSM = this.gameObject.GetComponent<PlayerAutoFightFSM>();
            if (aiFithtFSM == null)
            {
                aiFithtFSM = this.gameObject.AddComponent<PlayerAutoFightFSM>();
            }
            aiFithtFSM.commandMng = commandMng;
            aiFithtFSM.StartStateMachine();
            aiFithtFSM.StopStateMachine();
        }
		if(dartFSM == null)
		{
			dartFSM = this.gameObject.GetComponent<PlayerAutoDartFSM>();
			if (dartFSM == null)
			{
				dartFSM = this.gameObject.AddComponent<PlayerAutoDartFSM>();
			}
			dartFSM.commandMng = commandMng;
			dartFSM.StartStateMachine();
			dartFSM.StopStateMachine();
		}
        abilityMng = GameCenter.abilityMng;
        abilityMng.SetPlayer(this);
        stateMachine.Start();
        IsFriend = true;
        if (!actorInfo.IsAlive)
        {
            Dead();
        }
        fxCtrl.DoShadowEffect(true);
       
        if (animFSM != null)
        {
            InitAnimation();
        }
        if (moveFSM != null)
        {
            moveFSM.FaceTo(180);
        }
        inited_ = true;
    }

    /// <summary>
    /// 注册事件监听
    /// </summary>
    protected override void Regist()
    {
        base.Regist();
        GameCenter.sceneMng.SceneStateChange += OnSceneStateChange;
        actorInfo.OnBaseDiffUpdate += BaseChange;
        RockerItem.OnDragStateChange += OnDragMoveStateChange;
        GameCenter.abilityMng.OnMainPlayerBeHit+= OnHangUp;
        SceneMng.OnAddDropItem += OnDropItem;
		actorInfo.OnSafetyAreaUpdate += OnSafetyAreaStateUpdate;
		actorInfo.OnMountRideStateUpdate += OnMountRideStateUpdate;
        GameCenter.inventoryMng.OnEquipUpdate += PlayStrenEffect; 
    }
    /// <summary>
    /// 注销事件监听
    /// </summary>
    public override void UnRegist()
    {
        base.UnRegist();
        GameCenter.sceneMng.SceneStateChange -= OnSceneStateChange;
        actorInfo.OnBaseDiffUpdate -= BaseChange;
        RockerItem.OnDragStateChange -= OnDragMoveStateChange;
        GameCenter.abilityMng.OnMainPlayerBeHit -= OnHangUp;
		actorInfo.OnSafetyAreaUpdate -= OnSafetyAreaStateUpdate;
        SceneMng.OnAddDropItem -= OnDropItem;
		actorInfo.OnMountRideStateUpdate -= OnMountRideStateUpdate;
        GameCenter.inventoryMng.OnEquipUpdate -= PlayStrenEffect; 
    }
    /// <summary>
    /// 播放传送开始的特效
    /// </summary>
    public void PlayBeginFlyEffect()
    {
        if (fxCtrl != null)
        { 
			fxCtrl.ClearFlyEffect();
			fxCtrl.DoFlyEffect("t_j_f_1_01");
        } 
    }
    /// <summary>
   /// 播放传送完成的特效
    /// </summary>
   public void PlayFlyOverEffect()
   {
		GameCenter.mainPlayerMng.isStartingFlyEffect = true;
       if (fxCtrl != null)
       { 
			fxCtrl.ClearFlyEffect();
			fxCtrl.DoFlyEffect("t_j_f_1_02");
       }
       if (GameCenter.taskMng.CurTaskNeedFly && GameCenter.taskMng.CurfocusTask  != null&& GameCenter.taskMng.IsTaskNeedAutoFight(GameCenter.taskMng.CurfocusTask))
       { 
           GameCenter.curMainPlayer.GoNormal();
           GameCenter.taskMng.TraceToAction(GameCenter.taskMng.CurfocusTask);
       }
   }
   public void ClearFlyEffect()
   {
       if (fxCtrl != null)
       {
			fxCtrl.ClearFlyEffect();
       }
   } 
    /// <summary>
    /// 达到条件播放强化特效
    /// </summary>
    void PlayStrenEffect()
    {
        if (fxCtrl != null)
        {
            //fxCtrl.ClearStrengthEffect();
            GameCenter.inventoryMng.PlayEquStrengEffectName(GameCenter.inventoryMng.GetPlayerEquList(), fxCtrl);
        }
    }
	/// <summary>
	/// 玩家在寻路中被击下马,重置上马条件  by邓成
	/// </summary>
	void OnMountRideStateUpdate(bool _ride, bool _isChange)
	{
		if(_ride == false && CurFSMState == EventType.TASK_PATH_FIND)
		{
			taskPathStateTime = 0f;
			sendRideMessage = false;
		}
	}

	void OnSafetyAreaStateUpdate(bool isInSafetyArea)
	{
		if(isInSafetyArea)
		{
			GameCenter.messageMng.AddClientMsg(309);
		}else
		{
			GameCenter.messageMng.AddClientMsg(310);
		}
	}
    /// <summary>
    /// 掉落物黑名单（拾取过一次但是没捡起来的） by邓成
    /// </summary>
    public List<DropItemInfo> blackDropItemList = new List<DropItemInfo>();
    public void OnDropItem()
    {
        if (NotFighting)
            return;
        if (GameCenter.inventoryMng.IsBagFull)
            return;
       aiFithtFSM.GoSelfCtrl();
        GameCenter.curMainPlayer.CancelCommands();
        Command_AutoPick autoPick = new Command_AutoPick();
        autoPick.cannotList = blackDropItemList;
        autoPick .player= this;
        GameCenter.curMainPlayer.commandMng.PushCommand(autoPick);
    }


    bool flyed;
    /// <summary>
    /// 自动回城或者随机  by黄洪兴
    /// </summary>
    /// <param name="_SmartActor"></param>
    void OnHangUp(SmartActor _SmartActor)
    {
        flyed = false;
            if (GameCenter.systemSettingMng.IsFlyOpen)
            {
                if ((float)GameCenter.systemSettingMng.FlyLifeNum / 100 >=(float)actorInfo.CurHP/ (float)actorInfo.MaxHP)
                {
                    if (GameCenter.inventoryMng.GetNumberByType(2600015) > 0 && actorInfo.CurLevel > new EquipmentInfo(2600015,EquipmentBelongTo.PREVIEW).UseReqLevel)
                    {
                        GameCenter.inventoryMng.C2S_UseItem(GameCenter.inventoryMng.GetEquipByType(2600015));
                        flyed = true;
                    }

                }               
            }
            if (GameCenter.systemSettingMng.IsRadomFlyOpen && !flyed)
            {
                if ((float)GameCenter.systemSettingMng.RandomFlyLifeNum / 100 >= (float)actorInfo.CurHP / (float)actorInfo.MaxHP)
                {
                    if (GameCenter.inventoryMng.GetNumberByType(2600016) > 0 && actorInfo.CurLevel > new EquipmentInfo(2600016, EquipmentBelongTo.PREVIEW).UseReqLevel)
                    {
                        GameCenter.inventoryMng.C2S_UseItem(GameCenter.inventoryMng.GetEquipByType(2600016));
                    }

                }        

            }
			
         }
    #endregion
   
    /// <summary>
    /// 根据服务端发送的坐标传送至当前场景的目标点
    /// </summary>
    public void ChangePos(pt_scene_tele_b107 pt)
    {
		Vector3 pos = new Vector3(pt.x,0f, pt.z);
		Vector3 playerPos = new Vector3(transform.position.x,0f, transform.position.z);
		//Debug.Log("pos:"+pos);
		if(GameStageUtility.CheckPosition(playerPos,pos))
		{
			GameCenter.curGameStage.PlaceGameObjectFromServer(this, pt.x, pt.z, (int)this.transform.localEulerAngles.y);
		}else
		{
			Vector3 tarPos = Utils.GetRandomPos(pos);
			//Debug.Log("重置玩家位置!"+tarPos);
			GameCenter.curGameStage.PlaceGameObjectFromServer(this, tarPos.x, tarPos.z, (int)this.transform.localEulerAngles.y);
		}
		StopForFly(); //用StopForFly 解决Command_FlyTo跨场景无法选中NPC的bug  by邓成
       // Debug.Log("传送成功 目标点(" + pt.x + "," + pt.y + "," + pt.z+")");

    }
    /// <summary>
    /// 初始化坐标 by吴江
    /// </summary>
    public void InitPos()
    {
        GameCenter.curGameStage.PlaceGameObjectFromServer(this, actorInfo.ServerPos.x, actorInfo.ServerPos.z, (int)this.transform.localEulerAngles.y);
		StopForFly();//用StopForFly 解决Command_FlyTo跨场景无法选中NPC的bug  by邓成
    }
    /// <summary>
    /// 重新引用信息
    /// </summary>
    public void ReBindInfo()
    {
        UnRegist();
        actorInfo = GameCenter.mainPlayerMng.MainPlayerInfo;
        Regist();
        EndCollect();
        ReBuildMount();
		rendererCtrl.ReBindInfo(actorInfo);
        if (soulObject != null)
        {
            soulObject.SetActive(false);
        }
    }
    /// <summary>
    /// 根据对象属性取得应该看到的名字/圆圈颜色
    /// </summary>
    /// <param name="_obj"></param>
    /// <returns></returns>
    public Color GetRelationColor(InteractiveObject _obj)
    {
        switch (_obj.typeID)
        {
            case ObjectType.Player:
                OtherPlayer opc = _obj as OtherPlayer;
                if (opc == null) return Color.green;
                return ConfigMng.Instance.GetRelationColor(Camp, opc.Camp, GameCenter.curGameStage.SceneType);
            case ObjectType.MOB:
                Monster mob = _obj as Monster;
                if (mob == null) return Color.red;
                return ConfigMng.Instance.GetRelationColor(Camp, mob.Camp, GameCenter.curGameStage.SceneType);
            case ObjectType.NPC:
                return Color.yellow;
            case ObjectType.Entourage:
                EntourageBase Entourage = _obj as EntourageBase;
                if (Entourage.Owner != null && Entourage.Owner == GameCenter.curMainPlayer)
                {
                    return Color.green;
                }
                else
                {
                    return ConfigMng.Instance.GetRelationColor(GameCenter.curMainPlayer.Camp, Entourage.Camp, GameCenter.curGameStage.SceneType);
                }
            case ObjectType.FlyPoint:
                return Color.yellow;
            default:
                return Color.yellow;
        }
    }
    /// <summary>
    /// 服务端最后确认的坐标 by吴江
    /// </summary>
    public Vector2 ServerPosition
    {
        get
        {
            MainPlayerInfo info = actorInfo as MainPlayerInfo;
            if (info != null)
            {
                return info.ServerPos;
            }
            return Vector2.zero;
        }
    }
    /// <summary>
    /// 死亡
    /// </summary>
    public override void Dead(bool _already = false)
    {
        if (isDead) return;
        IsInAutoFightBeforeDead = IsInAutoFight;
        GoNormal();
        ForceCancelAbility();
        base.Dead(_already);
        CurTarget = null;
        AttakType = AttackType.NONE;//结束自动战斗状态,请带上结束的事件,否则UI上表现还是自动战斗中  
        //if (onExitAutoFightEvent != null)
        //    onExitAutoFightEvent();
        //if (onExitFightEvent != null)
        //    onExitFightEvent();
        moveFSM.LockMoving();
    }


    public override void BeHit(AbilityResultInfo _info)
    {
        base.BeHit(_info);
        if (_info != null)
        {
            if (CurTarget != null && (CurTarget.typeID != ObjectType.Player || !PlayerAutoFightFSM.IsEnemy(CurTarget)))
            {
                if (_info.UserActor != null && _info.UserActor.typeID == ObjectType.Player && PlayerAutoFightFSM.IsEnemy(_info.UserActor))
                {
                    CurTarget = _info.UserActor;
                    aiFithtFSM.SetThisTarget(_info.UserActor);
                }
            }
        }

    }

    public void CancelCommands()
    {
        curTryUseAbility = null;
        commandMng.CancelCommands();
    }

    /// <summary>
    /// 禁止外部使用
    /// </summary>
    /// <param name="_instance"></param>
    public override void UseAbility(AbilityInstance _instance)
    {
        if (isDead) return;
        curTryUseAbility = null;
        if (_instance == null) return;
        if (abilityMng.CheckIsDefaultAbility(_instance))
        {
			abilityMng.UsedDefaultAbility(_instance);
        }
        base.UseAbility(_instance);
        abilityMng.C2S_UseAbility(_instance);
    }
    /// <summary>
    /// 使用技能（供外部使用） by吴江
    /// </summary>
    ///  <param name="_trace">是否追踪</param>
    /// <returns></returns>
    public bool TryUseAbility(AbilityInstance _instance, bool _trace, bool noTarget = false)
    {
		if (isRigidity || isDead)
		{
			curTryUseAbility = null;
			return false;
		}
		if(_instance != null && _instance.TargetActor != null)//有攻击目标的技能,不能在安全区释放  by邓成
		{
			OtherPlayer opc = _instance.TargetActor as OtherPlayer;
			if(opc != null && (opc.IsInSafetyArea || actorInfo.IsInSafetyArea))
				return false;
		}
        if (actorInfo.CurMountInfo != null && actorInfo.CurMountInfo.IsRiding)
        {
            GameCenter.newMountMng.C2S_ReqRideMount(ChangeMount.DOWNRIDE, actorInfo.CurMountInfo.ConfigID, MountReqRideType.AUTO);
            return false;
        }
        if (_instance.thisSkillMode != SkillMode.NORMALSKILL && IsSilent)
        {
            curTryUseAbility = null;
            return false;
        }
        if (!inputListener.CheckLock())
        {
            curTryUseAbility = null;
            return false;
        }
        if (_instance.NeedMP > actorInfo.CurMP)
        {
            curTryUseAbility = null;
            GameCenter.messageMng.AddClientMsg(43);
            return false;
        }
        if (IsMoving)
        {
            StopMovingTo();
            StopMovingTowards();
        }
        CurTarget = _instance.TargetActor;
		if (_instance.NeedTarget && (CurTarget == null || !PlayerAutoFightFSM.IsEnemy(CurTarget)))
        {
            switch (_instance.ThisSkillTargetType)
            {
                case SkillTargetType.ENEMY:
                    TryGetColosestAttackTarget(RelationType.AUTOMATEDATTACKS, 10.0f);//如果没目标，尝试找10码范围内的目标 by吴江
                    _instance.SetActor(this, CurTarget as SmartActor);
                    break;
                case SkillTargetType.SELF:
                    CurTarget = this;
                    _instance.SetActor(this, this);
                    break;
                case SkillTargetType.TEAMMATE:
                    TryGetColosestAttackTarget(RelationType.NO_ATTAK, 10.0f);//如果没目标，尝试找10码范围内的目标 by吴江
                    _instance.SetActor(this, CurTarget as SmartActor);
                    break;
                default:
                    break;
            }
        }
        if (_instance.NeedTarget)
        {
            if (CurTarget != null)
            {
                switch (_instance.ThisSkillTargetType)
                {
                    case SkillTargetType.ENEMY:
                        if (!PlayerAutoFightFSM.IsEnemy(CurTarget))
                        {
                            curTryUseAbility = null;
                            GameCenter.messageMng.AddClientMsg(64);
                            return false;
                        }
                        break;
                    case SkillTargetType.SELF:
                        if (CurTarget != this)
                        {
                            curTryUseAbility = null;
                            GameCenter.messageMng.AddClientMsg(64);
                            return false;
                        }
                        break;
                    case SkillTargetType.TEAMMATE:
                        if (PlayerAutoFightFSM.IsEnemy(CurTarget))
                        {
                            curTryUseAbility = null;
                            GameCenter.messageMng.AddClientMsg(64);
                            return false;
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                curTryUseAbility = null;
                GameCenter.messageMng.AddClientMsg(45);
                return false;
            }
        }
        if (abilityMng != null && abilityMng.HasLockState)
        {
            curTryUseAbility = null;
            return false;
        }
        if (moveFSM != null && moveFSM.isMoving)
        {
            StopMovingTo();
        }
        CancelAbility();

        CancelCommands();
        curTryUseAbility = _instance;
        if (_trace)
        {
            if (CurTarget == null)
            {
                //if (_instance.NeedTarget) return false;
                Command_CastAbilityOn cmdCastAbilityOn = new Command_CastAbilityOn();
                cmdCastAbilityOn.abilityInstance = _instance;
                cmdCastAbilityOn.target = noTarget ? null : CurTarget; ;
                commandMng.PushCommand(cmdCastAbilityOn);

            }
            else
            {
                Vector3[] path = GameStageUtility.StartPath(transform.position, CurTarget.transform.position);
                if (path == null || path.Length == 0)
                {
                    //if (GameCenter.mainPlayerMng.CurGuideMYPoint != null && GameCenter.mainPlayerMng.CurGuideMYPoint.x != 0)//有路点找路点
                    //{
                    //    Command_MoveTo cmdMoveTo = new Command_MoveTo();
                    //    cmdMoveTo.DestMapConfigPos = MYPOint.world2localV3(GameCenter.mainPlayerMng.CurGuideMYPoint);
                    //    cmdMoveTo.maxDistance = 0f;
                    //    commandMng.PushCommand(cmdMoveTo);
                    //}
                    //else 
                    if (CurTarget != null && (CurTarget.transform.position - this.transform.position).sqrMagnitude <= 2.0f * 2.0f)//有怪物(玩家和怪物重叠)直接打怪
                    {
                        Command_CastAbilityOn cmdCastAbilityOn = new Command_CastAbilityOn();
                        cmdCastAbilityOn.abilityInstance = _instance;
                        cmdCastAbilityOn.target = noTarget ? null : CurTarget;
                        commandMng.PushCommand(cmdCastAbilityOn);
                    }
                }
                else
                {
					//满足条件优先使用冲刺技能
					AbilityInstance rushAbility = abilityMng.GetRoundAbility((SmartActor)CurTarget);
					if (abilityMng.CheckIsDefaultAbility(_instance) && rushAbility != null && (CurTarget.transform.position - this.transform.position).sqrMagnitude >= _instance.AttackDistance*_instance.AttackDistance)
					{
						Command_TraceTarget cmdTraceTarget = new Command_TraceTarget();
						cmdTraceTarget.abilityInstance = rushAbility;
						cmdTraceTarget.target = CurTarget;
						cmdTraceTarget.updatePathDeltaTime = 1.0f;
						commandMng.PushCommand(cmdTraceTarget);

						Command_CastAbilityOn rushAbilityCom = new Command_CastAbilityOn();
						rushAbilityCom.abilityInstance = rushAbility;
						rushAbilityCom.target = noTarget ? null : CurTarget; ;
						commandMng.PushCommand(rushAbilityCom);
					}else
					{
						Command_TraceTarget cmdTraceTarget = new Command_TraceTarget();
						cmdTraceTarget.abilityInstance = _instance;
						cmdTraceTarget.target = CurTarget;
						cmdTraceTarget.updatePathDeltaTime = 1.0f;
						commandMng.PushCommand(cmdTraceTarget);

						Command_CastAbilityOn cmdCastAbilityOn = new Command_CastAbilityOn();
						cmdCastAbilityOn.abilityInstance = _instance;
						cmdCastAbilityOn.target = noTarget ? null : CurTarget; ;
						commandMng.PushCommand(cmdCastAbilityOn);
					}
                }
            }
        }
        else
        {
            if (CurTarget != null && !noTarget && curTryUseAbility.DirTurnType != TurnType.NOTTURN)
            {
                FaceToNoLerp(CurTarget.gameObject.transform.position - transform.position);
            }
            //这里一定不能直接使用技能,而必须是压入命令.  压入命令的话,执行命令是下一帧(因为必须先等本帧的状态机切换结束)  by 吴江
            Command_CastAbilityOn cmdCastAbilityOn = new Command_CastAbilityOn();
            cmdCastAbilityOn.abilityInstance = _instance;
            cmdCastAbilityOn.target = noTarget ? null : CurTarget; ;
            commandMng.PushCommand(cmdCastAbilityOn);
        }
        return true;
    }
    /// <summary>
    /// 使用技能（供外部使用）
    /// </summary>
    /// <param name="_abilityID">技能ID</param>
    /// <param name="_abilityLev">技能等级</param>
    ///  <param name="_trace">是否追踪</param>
    /// <returns></returns>
    public bool TryUseAbility(int _abilityID, int _abilityLev, bool _trace)
    {
        AbilityInstance abilityInstance = new AbilityInstance(_abilityID, _abilityLev, this, CurTarget as SmartActor);
        return TryUseAbility(abilityInstance, _trace);
    }

    /// <summary>
    /// 使用治疗术 by 贺丰
    /// </summary>
    /// <param name="_instance"></param>
    public void TryUseAddHp(AbilityInstance _instance)
    {
        if (isDead) return;
        curTryUseAbility = null;
        if (_instance == null) return;
        if (abilityMng.CheckIsDefaultAbility(_instance))
        {
			abilityMng.UsedDefaultAbility(_instance);
        }
        base.UseAbility(_instance);
        abilityMng.C2S_UseAddHpAbility(_instance);
    }
		
	protected float abilityMoveEndTime = 0f;
	public override void AbilityMoveEnd (AbilityMoveData _abilityMoveData)
	{
		if(Mathf.Abs(abilityMoveEndTime - Time.time) <= 0.5f)//这个事件抛出了两次,做下处理
			return;
		abilityMoveEndTime = Time.time;
		base.AbilityMoveEnd (_abilityMoveData);
		if(_abilityMoveData.user == this.gameObject && abilityMng.IsRoundAbility(_abilityMoveData.DataInfo))
		{
			AbilityInstance instance = this.abilityMng.GetNextDefaultAbility((SmartActor)CurTarget);
			Command_CastAbilityOn cmdCastAbilityOn = new Command_CastAbilityOn();
			cmdCastAbilityOn.abilityInstance = instance;
			cmdCastAbilityOn.target = CurTarget; ;
			commandMng.PushCommand(cmdCastAbilityOn);
		}
	}

    public void BuffTest(int _typeID, bool _add)
    {
        if (!GameCenter.instance.isDevelopmentPattern) return;
        //测试buff
        pt_scene_chg_buff_c008 data = new pt_scene_chg_buff_c008();
        data.buff_type = (uint)_typeID;
        data.obj_sort = 1;
        data.oid = (uint)actorInfo.ServerInstanceID;
        data.buff_power = 5000;
        data.buff_len = 3000;
        BuffInfo info = new BuffInfo(data);

        if (_add)
        {
            actorInfo.Update(info, info.BuffTypeID, info.ActorID);
        }
        else
        {
            actorInfo.Update(null, info.BuffTypeID, info.ActorID);
        }
    }

    public override void ReLive()
    {
        base.ReLive();
        if (IsInAutoFightBeforeDead && GameCenter.curGameStage.SceneType == SceneType.DUNGEONS && actorInfo.CurSceneUiType != SceneUiType.RAIDERARK)
        {
            GoAutoFight();
        }
        else
        {
            GoNormal();
        }
    }

    public override void PositionChange()
    {
        base.PositionChange();
        SceneMng sceneMng = GameCenter.sceneMng;
        if (Time.frameCount % 3 == 0 && sceneMng != null && sceneMng.HasAreaBuff)
        {
            List<AreaBuffRef> areaBuffList = sceneMng.AreaBuffList;
            for (int i = 0, length = areaBuffList.Count; i < length; i++)
            {
                AreaBuffRef areaBuff = areaBuffList[i];
                Vector2 center = new Vector2(areaBuff.coordinate.x, areaBuff.coordinate.z);
                Vector2 mypos = new Vector2(this.transform.position.x, this.transform.position.z);
                if ((center - mypos).sqrMagnitude <= areaBuff.radius && (areaBuff.camp == 0 || actorInfo.Camp == areaBuff.camp))
                {  
                    sceneMng.EnterBuffArea(areaBuff.id, true, areaBuff.inTip);
                    break;
                }
                else
                {
                    if (areaBuff.camp == 0 || actorInfo.Camp == areaBuff.camp)
                        sceneMng.EnterBuffArea(areaBuff.id, false, areaBuff.outTip);
                    else
                        sceneMng.EnterBuffArea(areaBuff.id, false, 0);
                }
            }
        }
    }


    /// <summary>
    /// 技能测试，正式版禁用
    /// </summary>
    public void AbilityTest()
    {
        if (!GameCenter.instance.isDevelopmentPattern) return;
        //测试技能
        //if (isRigidity) return;
        //commandMng.CancelCommands();
        //AbilityInstance abilityInstance = new AbilityInstance(3, 1, this, CurTarget as SmartActor);
        //UseAbility(abilityInstance);

        //if (Input.GetKey(KeyCode.B))
        //{ 
        //    GameCenter.uIMng.SwitchToUI(GUIType.SPRITEANIMAL);
        //}


        //测试被击中施放技能
        //AbilityInstance abilityInstance = new AbilityInstance(4, 20,1302, this, this);
        ////UseAbility(abilityInstance);
        //st.net.NetBase.skill_effect ef = new st.net.NetBase.skill_effect();
        //ef.target_id = (uint)actorInfo.ServerInstanceID;
        //ef.target_sort = (uint)ObjectType.Player;
        //ef.atk_sort = (uint)AttackResultType.ATT_SORT_NARMAL;
        //ef.def_sort = (uint)DefResultType.DEF_SORT_KICK2;
        //ef.demage = (uint)1000;
        //ef.cur_hp = (uint)actorInfo.CurHP;
        //ef.effect_x = this.transform.position.x + 5;
        //ef.effect_y = this.transform.position.y;
        //ef.effect_z = this.transform.position.z;

        //actorInfo.BeInfluencedByOther(abilityInstance,ef);

        //打开试用翅膀窗口
//        if (Input.GetKeyDown(KeyCode.B))
//        {
//            GameCenter.uIMng.SwitchToUI(GUIType.LOVEPACKAGE);
//        }
        //测试buff
        //pt_scene_chg_buff_c008 data = new pt_scene_chg_buff_c008();
        //data.buff_type = 1;
        //data.obj_sort  = 1;
        //data.oid = (uint)actorInfo.ServerInstanceID;
        //data.buff_power = 5000;
        //data.buff_len = 3000;
        //BuffInfo info = new BuffInfo(data);
        //foreach (var item in GameCenter.sceneMng.MOBInfoList.Values)
        //{
        //    item.Update(info, info.BuffTypeID, info.ActorID);
        //}
        // actorInfo.Update(info, info.BuffTypeID, info.ActorID);


        ////测试被击
        //BeInfluencedByOther(new AbilityResultInfo.AbilityInfluenceInfo());

        //测试野兽斗士转职
        //ServerData.SetProfData data = new ServerData.SetProfData();
        //data.newProf = actorInfo.Prof == 2 ? 1 : 2;
        //actorInfo.Update(data);

        //测试技能预警
        //Vector3 xx = this.gameObject.transform.position;
        //xx = new Vector3(xx.x, xx.y + 5.0f, xx.z);
        //GameCenter.spawner.SpawnAbilityShadowEffecter(xx, 0, 4, 6, AbilityProjectorType.Circle30, new Color(0.8f, 0.1f, 0.1f, 0.6f), 4.0f);

        //if (headTextCtrl != null) headTextCtrl.SetCountDown(10);

        // GameCenter.uIMng.SwitchToUI(GUIType.COSMETIC);
        //坐骑测试
        //ServerData.MountData data = new ServerData.MountData();
        //data.id = 1;
        //data.isRiding = actorInfo.CurMountInfo == null || !actorInfo.CurMountInfo.IsRiding;
        //actorInfo.Update(data);

        //测试坐骑上转职
        //if (actorInfo.IsOnMount)
        //{
        //    StartCoroutine(WaitForQuitMount());
        //}
        //else
        //{
        //    ServerData.SetProfData data = new ServerData.SetProfData();
        //    data.newProf = actorInfo.Prof == 2 ? 1 : 2;
        //    actorInfo.Update(data);
        //}


        //测试角色界面子窗口跳转
        // ClothespressWnd.OpenAndGoSubWnd(ClothespressWnd.TogType.MOUNT);


        //int id = 1; int countDownTime = 30000;
        //GameCenter.uIMng.SwitchToUI(GUIType.COMBATCOUNTDOWN);
        //if (GameCenter.sceneMng.onCountDownEvent  != null)
        //    GameCenter.sceneMng.onCountDownEvent(id, countDownTime);

    }
    /// <summary>
    /// 升级时播放特效
    /// </summary>
    protected void BaseChange(ActorBaseTag _tag, int _value, bool _fromAbility)
    {
        switch (_tag)
        {
            case ActorBaseTag.CurHP:
				if(_value > 0){
				GameCenter.spawner.SpawnStateTexter(_value);
				}
                break;
            case ActorBaseTag.CurMP:
                break;
            case ActorBaseTag.UnBindCoin:
                break;
            case ActorBaseTag.Diamond:
                break;
            case ActorBaseTag.Level:
                if (fxCtrl != null)
                {
                    fxCtrl.ClearLevelUPEffect();
                    fxCtrl.DoLevelUPEffect("e_c_cast_001");
                    string[] level = new string[1];
                    level[0] = actorInfo.Level.ToString();
                    GameCenter.messageMng.AddClientMsg(39, level);
                    GameCenter.soundMng.PlaySound("levelup.mp3", 0.5f, false, true);
                }
                break;
            case ActorBaseTag.Exp:
                break;
            case ActorBaseTag.TOTAL:
                break;
            default:
                break;
        }
    }

    public void OnDragMoveStateChange(bool _draging)
    {
        if (GameCenter.curMainPlayer.isRigidity) return;
        if (!_draging)
        {
            MoveEnd();
        }
        else
        {
            MoveStart();
        }

    }
    /// <summary>
    /// 开始移动
    /// </summary>
    public override void MoveStart()
    {
        base.MoveStart();
        animFSM.SetMoveSpeed(CurRealSpeed / (actorInfo.AnimationMoveSpeedBase * actorInfo.ModelScale));
        if (OnMoveStart != null)
        {
            OnMoveStart(true);
        }
    }
    /// <summary>
    /// 停止移动
    /// </summary>
    public override void MoveEnd()
    {
        if (PlayerInputListener.isDragingRockerItem) return;
        base.MoveEnd();
        if (OnMoveStart != null)
        {
            OnMoveStart(false);
        }
    }
    /// <summary>
    /// 停止当前行为,察看弹出的消息
    /// </summary>
    public void StopForCheckMsg()
    {
        ForceCancelAbility();
        StopMovingTo();
        CancelCommands();
		GoHoldOn();
    }
	/// <summary>
	/// 传送之后停止移动 但不清除命令 by邓成
	/// </summary>
	public void StopForFly()
	{
		ForceCancelAbility();
		StopMovingTo();
		GoNormal();
	}

	public void StopForNextMove()
	{
		CancelCommands();
		StopMovingTo();
		GoNormal();
	}

    protected override void OnBuffUpdate(int _buffID, bool _add)
    {
        base.OnBuffUpdate(_buffID, _add);
        if (IsActor) return;
        BuffInfo info = actorInfo.GetBuffInfo(_buffID);
        if (fxCtrl != null && info != null)
        {
            if (_add)
            {
                switch (info.ContrlType)
                {
                    case BuffControlSortType.STUN:
                    case BuffControlSortType.BANISH:
                    case BuffControlSortType.SLEEP:
                    case BuffControlSortType.FEAR:
                        inputListener.AddLockType(PlayerInputListener.LockType.CTRL_BUFF);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (info.ContrlType)
                {
                    case BuffControlSortType.STUN:
                    case BuffControlSortType.BANISH:
                    case BuffControlSortType.SLEEP:
                    case BuffControlSortType.FEAR:
                        inputListener.RemoveLockType(PlayerInputListener.LockType.CTRL_BUFF);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    #region 状态机
    /// <summary>
    /// 游戏状态枚举 by吴江
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// 普通状态
        /// </summary>
        NORMAL = fsm.Event.USER_FIELD + 1,
        /// <summary>
        /// 任务寻路中
        /// </summary>
        TASK_PATH_FIND,
        /// <summary>
        /// 自动战斗中
        /// </summary>
        AI_FIGHT_CTRL,
		/// <summary>
		/// 自动运镖中
		/// </summary>
		AI_DART_CTRL,
        /// <summary>
        /// 挂起状态
        /// </summary>
        HOLD_ON,
        /// <summary>
        /// 普通寻路中
        /// </summary>
        PATH_FIND,
        /// <summary>
        /// 采集物品中
        /// </summary>
        COLLECT_ITEM,
    }

    /// <summary>
    /// 寻到指定地点后玩家行为
    /// </summary>
    public enum PlayerActionPathEnd
    {
        /// <summary>
        /// 无行为
        /// </summary>
        NONE,
        /// <summary>
        /// 进行任务操作
        /// </summary>
        TASKACTION,
        /// <summary>
        /// 寻宝
        /// </summary>
        SEARCHCHESTACTION,
    }

    protected fsm.State taksPathFind;
    protected fsm.State aiFight;
	protected fsm.State aiDart;
    protected fsm.State holdOn;
    protected fsm.State normalPathFind;

    protected EventType curFSMState = EventType.NORMAL;
    /// <summary>
    /// 当前的状态
    /// </summary>
    public EventType CurFSMState
    {
        get
        {
            return curFSMState;
        }
    }
    protected EventType lastFSMState = EventType.NORMAL;

    /// <summary>
    /// 初始化状态机 by吴江
    /// </summary>
    protected override void InitStateMachine()
    {
        normal = new fsm.State("normal", stateMachine);
        normal.onEnter += EnterNormalState;
        normal.onExit += ExitNormalState;
        normal.onAction += UpdateNormalState;

        taksPathFind = new fsm.State("taksPathFind", stateMachine);
        taksPathFind.onEnter += EnterTaskPathState;
        taksPathFind.onExit += ExitTaskPathState;
        taksPathFind.onAction += UpdateTaskPathState;

        normalPathFind = new fsm.State("normalPathFind", stateMachine);
        normalPathFind.onEnter += EnterPathState;
        normalPathFind.onExit += ExitPathState;
        normalPathFind.onAction += UpdatePathState;


        aiFight = new fsm.State("aiFight", stateMachine);
        aiFight.onEnter += EnterAiFightState;
        aiFight.onExit += ExitAiFightState;
        aiFight.onAction += UpdateAiFightState;

		aiDart = new fsm.State("aiDart",stateMachine);
		aiDart.onEnter += EnterAiDartState;
		aiDart.onExit += ExitAiDartState;
		aiDart.onAction += UpdateAiDartState;

        holdOn = new fsm.State("holdOn", stateMachine);
        holdOn.onEnter += EnterHoldOnState;
        holdOn.onExit += ExitHoldOnState;
        holdOn.onAction += UpdateHoldOnState;



        normal.Add<fsm.EventTransition>(taksPathFind, (int)EventType.TASK_PATH_FIND);
        normal.Add<fsm.EventTransition>(normalPathFind, (int)EventType.PATH_FIND);
        normal.Add<fsm.EventTransition>(aiFight, (int)EventType.AI_FIGHT_CTRL);
        normal.Add<fsm.EventTransition>(holdOn, (int)EventType.HOLD_ON);
		normal.Add<fsm.EventTransition>(aiDart, (int)EventType.AI_DART_CTRL);

        taksPathFind.Add<fsm.EventTransition>(normal, (int)EventType.NORMAL);
        taksPathFind.Add<fsm.EventTransition>(aiFight, (int)EventType.AI_FIGHT_CTRL);
        taksPathFind.Add<fsm.EventTransition>(holdOn, (int)EventType.HOLD_ON);
        taksPathFind.Add<fsm.EventTransition>(normalPathFind, (int)EventType.PATH_FIND);
		taksPathFind.Add<fsm.EventTransition>(aiDart, (int)EventType.AI_DART_CTRL);

        normalPathFind.Add<fsm.EventTransition>(normal, (int)EventType.NORMAL);
        normalPathFind.Add<fsm.EventTransition>(aiFight, (int)EventType.AI_FIGHT_CTRL);
        normalPathFind.Add<fsm.EventTransition>(holdOn, (int)EventType.HOLD_ON);
        normalPathFind.Add<fsm.EventTransition>(taksPathFind, (int)EventType.TASK_PATH_FIND);
		normalPathFind.Add<fsm.EventTransition>(aiDart, (int)EventType.AI_DART_CTRL);

        aiFight.Add<fsm.EventTransition>(normal, (int)EventType.NORMAL);
        aiFight.Add<fsm.EventTransition>(taksPathFind, (int)EventType.TASK_PATH_FIND);
        aiFight.Add<fsm.EventTransition>(holdOn, (int)EventType.HOLD_ON);
        aiFight.Add<fsm.EventTransition>(normalPathFind, (int)EventType.PATH_FIND);
		aiFight.Add<fsm.EventTransition>(aiDart, (int)EventType.AI_DART_CTRL);

		aiDart.Add<fsm.EventTransition>(normal, (int)EventType.NORMAL);
		aiDart.Add<fsm.EventTransition>(taksPathFind, (int)EventType.TASK_PATH_FIND);
		aiDart.Add<fsm.EventTransition>(holdOn, (int)EventType.HOLD_ON);
		aiDart.Add<fsm.EventTransition>(normalPathFind, (int)EventType.PATH_FIND);
		aiDart.Add<fsm.EventTransition>(aiFight, (int)EventType.AI_FIGHT_CTRL);

        holdOn.Add<fsm.EventTransition>(aiFight, (int)EventType.AI_FIGHT_CTRL);
        holdOn.Add<fsm.EventTransition>(normal, (int)EventType.NORMAL);
        holdOn.Add<fsm.EventTransition>(taksPathFind, (int)EventType.TASK_PATH_FIND);
        holdOn.Add<fsm.EventTransition>(normalPathFind, (int)EventType.PATH_FIND);
		holdOn.Add<fsm.EventTransition>(aiDart, (int)EventType.AI_DART_CTRL);

        stateMachine.initState = normal;
    }


    #region 常规状态
    protected virtual void EnterNormalState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        curFSMState = EventType.NORMAL;
    }

    protected virtual void ExitNormalState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        lastFSMState = EventType.NORMAL;
    }

    protected virtual void UpdateNormalState(fsm.State _curState)
    {
    }
    #endregion
    #region 任务寻路
	/// <summary>
	/// 处在寻路状态中的时间
	/// </summary>
	protected float taskPathStateTime = 0f;
	/// <summary>
	/// 是否已经发送上坐骑消息
	/// </summary>
	protected bool sendRideMessage = false;
    protected TaskSinglePath curTaskPath = null;
    protected bool curTaskPathDirty = true;
    /// <summary>
    /// 打开或者关闭传送界面
    /// </summary>
    public Action<bool> OnCloseOrStartFly;
    /// <summary>
    /// 是否是第一次打开传送界面
    /// </summary>
    public bool isFirstOpen = false;
    protected virtual void EnterTaskPathState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        //Debug.Log("EnterTaskPathState");
		taskPathStateTime = 0;
		sendRideMessage = false;
        curFSMState = EventType.TASK_PATH_FIND;
        curTaskPath = null;
        if (!isFirstOpen)
            GameCenter.uIMng.GenGUI(GUIType.TASK_FINDING, true);
        else if (OnCloseOrStartFly != null)
            OnCloseOrStartFly(true);
        TaskPathFind wnd = GameCenter.uIMng.GetGui<TaskPathFind>();
        if (wnd != null)
        {
            wnd.transform.localPosition = new Vector3(wnd.transform.localPosition.x, wnd.transform.localPosition.y, wnd.transform.localPosition.z);
        }
		GameCenter.uIMng.OpenFuncShowMenu(false);
    }
    
    protected virtual void ExitTaskPathState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        //TO DO：关闭寻路UI
        lastFSMState = EventType.TASK_PATH_FIND;
        //GameCenter.uIMng.ReleaseGUI(GUIType.TASK_FINDING);
        if (OnCloseOrStartFly != null)
            OnCloseOrStartFly(false);
        CurTargetPoint = null;
    }

    protected virtual void UpdateTaskPathState(fsm.State _curState)
    {
		if (curTaskPath == null)
		{
			curTaskPath = GameCenter.taskMng.PopTaskPath();
			// Debug.Log(curTaskPath);
			curTaskPathDirty = true;
			if (curTaskPath == null)
			{
                commandMng.CancelCommands();//取消之前的命令,解决小地图寻路到传送门，传送之后继续寻路的bug  by邓成
				stateMachine.Send((int)EventType.NORMAL);
				GoCompleteTask();
			}
		}
		else if (curTaskPathDirty)
		{
			if (TryActionCurTaskTrace())
			{
				curTaskPathDirty = false;
			}
		}
		else if (!commandMng.HasCommand())
		{
			curTaskPath = null;
		}
		taskPathStateTime += Time.deltaTime;
        if (taskPathStateTime > 3.0f && !sendRideMessage && !GameCenter.sceneMng.isChangingScene)//切换场景中不能上马
		{
            if (GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType != SceneUiType.RAIDERARK && actorInfo.CurSceneType != SceneType.ARENA)
            {
                if (actorInfo.CurMountInfo != null && !actorInfo.CurMountInfo.IsRiding)
                {
                    GameCenter.newMountMng.C2S_ReqRideMount(ChangeMount.RIDEMOUNT, actorInfo.CurMountInfo.ConfigID, MountReqRideType.AUTO);
                    sendRideMessage = true;
                }
            }
		}
    }

	/// <summary>
	/// 结束任务寻径后自动行为(执行未完成的任务)
	/// </summary>
	public void GoCompleteTask() 
	{
		TaskInfo curTask = GameCenter.taskMng.CurfocusTask;
		if(curTask != null && curTask.TaskState == TaskStateType.Process && curTask.ContentType == TaskContentType.DO_SOMETHING)	
		{
			if(curTask.TaskGuideType == GuideType.MOVEANDSTAY && curTask.IsStartProgressCondition) 
			{
                if (GameCenter.taskMng.OnTaskGuideUpdateEvent != null)
                    GameCenter.taskMng.OnTaskGuideUpdateEvent(curTask);
				return;
			}
			AttakType = AttackType.COMPLETE_TASK;
			stateMachine.Send((int)EventType.AI_FIGHT_CTRL);
		}
	}

    /// <summary>
    /// 结束任务寻径后根据任务要求进行不同的行为
    /// </summary>
    void EndFindPathEnterTaskAI()
    {
        TaskInfo curinfo = GameCenter.taskMng.CurfocusTask;
        switch (curinfo.ConditionRefList[0].sort)
        {
            case TaskConditionType.CollectSceneItem:
			case TaskConditionType.CollectAnyItem:
                {

                    SceneItem temp = GameCenter.curGameStage.GetClosestSceneItem(this);
                    Debug.Log(temp);
                    CommandTriggerTarget trigCmd = new CommandTriggerTarget();
                    trigCmd.target = temp;
                    this.commandMng.PushCommand(trigCmd);
                } break;
            default:
                GoAutoFight();
                break;
        }

    }


    #endregion
    #region 普通寻路
    protected virtual void EnterPathState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        curFSMState = EventType.PATH_FIND;
        //curTaskPath = null;
        if (actorInfo.CurMountInfo != null && !actorInfo.CurMountInfo.IsRiding && !GameCenter.sceneMng.isChangingScene)//切换场景中不能上马
        {
            if (actorInfo.CurSceneType != SceneType.ARENA && GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType != SceneUiType.RAIDERARK)
            {
                GameCenter.newMountMng.C2S_ReqRideMount(ChangeMount.RIDEMOUNT, actorInfo.CurMountInfo.ConfigID, MountReqRideType.AUTO);
            }
        }
    }

    protected virtual void ExitPathState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        //TO DO：关闭寻路UI
        //lastFSMState = EventType.PATH_FIND;
        //GameCenter.uIMng.CloseGui<TaskStateWnd>();
    }

    protected virtual void UpdatePathState(fsm.State _curState)
    {
        if (curTaskPath == null)
        {
            curTaskPath = GameCenter.taskMng.PopTaskPath();
            curTaskPathDirty = true;
            if (curTaskPath == null)
            {
                stateMachine.Send((int)EventType.NORMAL);
            }
        }
        else if (curTaskPathDirty)
        {
            if (TryActionCurTaskTrace())
            {
                curTaskPathDirty = false;
            }
        }
        //还是不自己做这一步了，交由任务寻路目标来决定
        //if (GameCenter.taskMng.RestPathCount <= 0)//如果是寻路的最后一步了
        //{
        //    if (curTaskPath.target != null && curTaskPath.target == CurTarget)
        //    {
        //        if ((transform.position - CurTarget.transform.position).sqrMagnitude <= 3.0f * 3.0f) //已经到达最终目标点了  by吴江
        //        {
        //            stateMachine.Send((int)EventType.NORMAL);
        //        }
        //    }
        //}
    }
    #endregion
    #region 自动战斗状态
    protected virtual void EnterAiFightState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (animFSM != null)
        {
            animFSM.SetInCombat(true);
        }
        aiFithtFSM.enabled = true;
        aiFithtFSM.StartStateMachine();
        curFSMState = EventType.AI_FIGHT_CTRL;
    }

    protected virtual void ExitAiFightState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        aiFithtFSM.StopStateMachine();
        aiFithtFSM.enabled = false;
        lastFSMState = EventType.AI_FIGHT_CTRL;
    }

    protected virtual void UpdateAiFightState(fsm.State _curState)
    {
    }
    #endregion

	#region 自动运镖状态
	protected virtual void EnterAiDartState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
		if (animFSM != null)
		{
			animFSM.SetInCombat(true);
		}
		dartFSM.enabled = true;
		dartFSM.StartStateMachine();
		curFSMState = EventType.AI_DART_CTRL;
	}

	protected virtual void ExitAiDartState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
		dartFSM.StopStateMachine();
		dartFSM.enabled = false;
		lastFSMState = EventType.AI_DART_CTRL;
	}

	protected virtual void UpdateAiDartState(fsm.State _curState)
	{
	}
	#endregion

    #region 挂起状态
    protected virtual void EnterHoldOnState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        curFSMState = EventType.HOLD_ON;
        StopMovingTo();
        StopMovingTowards();
        CancelAbility();
        CurTarget = null;
        if (moveFSM != null)
        {
            moveFSM.LockMoving();
        }
        //      autoAttak = AttackType.NONE;
		//CancelCommands();   状态机里不做CancelCommands,不好控制  by邓成
        //  abilityMng.StopContinHit();	
    }

    protected virtual void ExitHoldOnState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (moveFSM != null)
        {
            moveFSM.UnLockMoving();
        }
        lastFSMState = EventType.HOLD_ON;
    }

    protected virtual void UpdateHoldOnState(fsm.State _curState)
    {
    }
    #endregion
    #endregion



    /// <summary>
    /// 开始当前任务寻路节点 by吴江
    /// </summary>
    protected bool TryActionCurTaskTrace()
    {
        if (curTaskPath == null || !curTaskPath.TryGenTarget()) return false;
        CancelCommands();

        if (curTaskPath.target != null)
        {
            Command_MoveTo cmdMoveTo = new Command_MoveTo();
            Vector3 targetPos = new Vector3();
            //解决寻路和NPC位置重叠的BUG BY ljq
            if (curTaskPath.targetType == ObjectType.NPC)
            {
                targetPos = GameCenter.taskMng.CurTargetPoint;
            }
            else
                targetPos = curTaskPath.target.gameObject.transform.position;
            cmdMoveTo.destPos = ActorMoveFSM.LineCast(targetPos,true);
            cmdMoveTo.maxDistance = 0f;
            commandMng.PushCommand(cmdMoveTo);

//			Command_TraceTarget traceTarget = new Command_TraceTarget();
//			traceTarget.target = curTaskPath.target;
//			commandMng.PushCommand(traceTarget);

			CommandTriggerTarget cmdTriggerTaskTarget = new CommandTriggerTarget();
            cmdTriggerTaskTarget.minDistance = 3.5f;
            cmdTriggerTaskTarget.target = curTaskPath.target;
            commandMng.PushCommand(cmdTriggerTaskTarget);

            //CurTarget = curTaskPath.target;  //先不设目标,到了目标位置再设  否则看不到选中光圈
            return true;
        }
        else if (curTaskPath.targetPos != Vector3.zero)
        {
            Command_MoveTo cmdMoveTo = new Command_MoveTo();
            cmdMoveTo.destPos = ActorMoveFSM.LineCast(curTaskPath.targetPos, true);
            cmdMoveTo.maxDistance = 0f;
            commandMng.PushCommand(cmdMoveTo);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 切换场景中，需要挂起主玩家
    /// </summary>
    /// <param name="_succeed"></param>
    protected void OnSceneStateChange(bool _succeed)
    {
        //if (fsm.SceneAnimStage.IsWatching) return;//过场动画时由过场动画控制
		if (_succeed) //如果是切换场景成功后
		{ 
            StopMovingTowards();
			actorInfo.CleanBuff();
			AttakType = AttackType.NONE;
            if (GameCenter.curGameStage.SceneType == SceneType.ARENA)//进入竞技场下坐骑
            {
                if (actorInfo.CurMountInfo != null && actorInfo.CurMountInfo.IsRiding)//下坐骑
                {
                    GameCenter.newMountMng.C2S_ReqRideMount(ChangeMount.DOWNRIDE, actorInfo.CurMountInfo.ConfigID, MountReqRideType.AUTO);
                }
            }
			if (GameCenter.curGameStage.SceneType == SceneType.CITY && lastFSMState != EventType.TASK_PATH_FIND) //只要进了主城就把玩家状态设置成Normal,排除任务寻路
			{
				lastFSMState = EventType.NORMAL;
			}
            else if (GameCenter.curGameStage.SceneType == SceneType.DUNGEONS && actorInfo.CurSceneUiType != SceneUiType.RAIDERARK)
			{ 
				lastFSMState = EventType.AI_FIGHT_CTRL;
				GoAutoFight();
			}
			else
			{
                if (lastFSMState == EventType.AI_FIGHT_CTRL) lastFSMState = EventType.NORMAL;
			}

			bool isInMount = actorInfo.CurMountInfo != null && actorInfo.CurMountInfo.IsRiding;
			SetInCombat(!(GameCenter.curGameStage is CityStage) && !isInMount);
			if(isInMount)
			{
				actorInfo.CurMountInfo.ReShowLevEffect();
			}
			if(fxCtrl != null && actorInfo.CurShowDictionary.ContainsKey(EquipSlot.wing))
			{
				fxCtrl.CleanBonesEffect();
				fxCtrl.ShowBoneEffect(true);
			}
			if(fxCtrl != null)
			{
				fxCtrl.ClearStrengthEffect();
            	PlayStrenEffect();
			}
            if (fxCtrl != null)
            {
                fxCtrl.ClearFlyEffect();
            } 
		}
		if (_succeed)
		{
			stateMachine.Send((int)lastFSMState);
            blackDropItemList.Clear();
		}
		else
		{
			GoHoldOn();
		}	
    }

    /// <summary>
    /// 开始追踪指定任务 by吴江
    /// </summary>
    public bool GoTraceTask()
    {
        // Debug.Log("GoTraceTask1");
        AttakType = AttackType.NONE;
        if (GameCenter.taskMng.RestPathCount > 0)
        {
			stateMachine.Send((int)EventType.TASK_PATH_FIND);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 开始追踪指定任务 by吴江
    /// </summary>
    public bool GoTraceTask(TaskInfo _info)
    {
        //   Debug.Log("GoTraceTask2");

        if (_info == null) return false;
        if (GameCenter.curGameStage.SceneType == SceneType.DUNGEONS && _info.TaskState == TaskStateType.Finished) return false;//当前为副本时，不进行任务提交
        AttakType = AttackType.NONE;

        GameCenter.taskMng.CurfocusTask = _info;
		TaskInfo curTask = GameCenter.taskMng.CurfocusTask;
		// Debug.Log(curTask.TaskName + ":" + _info.TaskName + ":" + CurFSMState);
		if (CurFSMState == EventType.TASK_PATH_FIND && curTask != null && curTask.ID == _info.ID && curTask.Step == _info.Step)
		{

			// return true;//由于需要切换任务寻路，这里貌似就没用了
		}
        GameCenter.taskMng.BuildCurFocusTaskPath();
        //    Debug.Log(GameCenter.taskMng.RestPathCount);
        if (GameCenter.taskMng.RestPathCount > 0)
        {
            //playerActionPathEnd = PlayerActionPathEnd.TASKACTION;
            stateMachine.Send((int)EventType.TASK_PATH_FIND);
            return true;
        }
        return false;
    }

	public bool GoTraceTarget(int sceneID,int sceneX,int sceneY)
	{
		AttakType = AttackType.NONE;
		GameCenter.taskMng.TraceToScene(sceneID,new Vector3(sceneX,0,sceneY));
		if (GameCenter.taskMng.RestPathCount > 0)
		{
			stateMachine.Send((int)EventType.TASK_PATH_FIND);
			return true;
		}
		return false;
	}
	/// <summary>
	/// 点击地面的移动,其他途径勿调用  by邓成
	/// </summary>
	public bool GoTraceTarget(float sceneX,float sceneY)
	{
		CancelCommands();
		GameCenter.taskMng.ClearTaskPath();
		AttakType = AttackType.NONE;
		GameCenter.taskMng.TraceToScene(actorInfo.SceneID,new Vector3(sceneX,0f,sceneY));
		if (GameCenter.taskMng.RestPathCount > 0)
		{
			stateMachine.Send((int)EventType.TASK_PATH_FIND);
			return true;
		}
		return false;
	}

    /// <summary>
    /// 探宝寻路
    /// </summary>
    public void GoTraceSearchTreasure()
    {

        //  Debug.Log(GameCenter.taskMng.RestPathCount);
        if (GameCenter.taskMng.RestPathCount > 0)
        {
            stateMachine.Send((int)EventType.NORMAL);//置为Normal，不然不能从任务寻路切换到寻宝寻路
            //playerActionPathEnd = PlayerActionPathEnd.SEARCHCHESTACTION;
            stateMachine.Send((int)EventType.TASK_PATH_FIND);
        }

    }



    /// <summary>
    /// 进入挂起状态
    /// </summary>
    public void GoHoldOn()
    {
        hasCtrlAwake = false;
        AttakType = AttackType.NONE;
        stateMachine.Send((int)EventType.HOLD_ON);
    }

	public void GoBackOn()
	{
		stateMachine.Send((int)lastFSMState);
	}

    /// <summary>
    /// 返回正常状态,结束诸如自动战斗，任务寻路等状态。但在挂起状态中无用 by吴江
    /// </summary>
    public void GoNormal()
    {
        if (curFSMState != EventType.HOLD_ON && curFSMState != EventType.NORMAL)
        {
            AttakType = AttackType.NONE;
            stateMachine.Send((int)EventType.NORMAL);
        }
    }

    /// <summary>
   /// 退出寻路
    /// </summary>
    public void BreakPathFound()
    {
        if (curFSMState == EventType.PATH_FIND || curFSMState == EventType.TASK_PATH_FIND)
        {
            GoNormal();
        }
    }

    public void BreakAutoFight()
    {
        if (curFSMState == EventType.AI_FIGHT_CTRL && aiFithtFSM != null)
        {
            aiFithtFSM.GoSelfCtrl();
        }
		if (curFSMState == EventType.AI_DART_CTRL && dartFSM != null)
		{
			dartFSM.GoSelfCtrl();
		}
    }



    /// <summary>
    /// 小范围寻怪,自动开启  
    /// </summary>
    public void GoNormalFight()
    {
        AttakType = AttackType.NORMALFIGHT;
        stateMachine.Send((int)EventType.AI_FIGHT_CTRL);
    }
    /// <summary>
    /// 关闭自动选怪(手动关闭)
    /// </summary>
    public void ExitAIFight()
    {
		AttakType = AttackType.NONE;
        CurTarget = null;
		StopMovingTo();
		CancelCommands();
        stateMachine.Send((int)EventType.NORMAL);
    }
    /// <summary>
    /// 开启托管(手动)
    /// </summary>
    public void GoAutoFight()
    {
		if(!GoAutoDart())//优先运镖
		{
		    AttakType = AttackType.AUTOFIGHT;
		    stateMachine.Send((int)EventType.AI_FIGHT_CTRL);
		}
    }
	/// <summary>
	/// 开启自动运镖
	/// </summary>
	public bool GoAutoDart()
	{
		MonsterInfo mobInfo = GameCenter.sceneMng.GetMyDartInfo();
		if(mobInfo != null)
		{
			AttakType = AttackType.AUTODART;
			stateMachine.Send((int)EventType.AI_DART_CTRL);
			return true;
		}
		return false;
	}
    /// <summary>
    /// 关闭托管(手动关闭)
    /// </summary>
    public void ExitAutoFight()
    {
        if (AttakType != AttackType.NORMALFIGHT)
        {
            stateMachine.Send((int)lastFSMState);
        }
    }

    protected void UpdateCheck()
    {
        if (moveFSM != null && moveFSM.isMoveLocked)
        {
            if (Time.frameCount / 60 == 0)
            {
                GameCenter.loginMng.C2S_Ping();
            }
        }
    }
	/// <summary>
	/// 设置主玩家的目标,在一定范围内自动选择  by邓成
	/// </summary>
	void SetCurTarget()
	{
		float distance = float.MaxValue;
		SmartActor smartTarget = CurTarget as SmartActor;
		if(smartTarget == null)
		{
			CurTarget = GameCenter.curGameStage.GetClosestSmartActor(this,RelationType.AUTOMATEDATTACKS,ref distance);
		}else
		{
			//玩家与怪的距离大于15,重新找怪
			if(smartTarget.isDead || (CurTarget.transform.position - transform.position).sqrMagnitude > 15.0f * 15.0f)
			{
				SmartActor tempActor = GameCenter.curGameStage.GetClosestSmartActor(this,RelationType.AUTOMATEDATTACKS,ref distance);
				if(tempActor != null && tempActor.id != CurTarget.id)
				{
					CurTarget = tempActor;
				}
			}
		}
	}

    protected override void EndCollect()
    {
        base.EndCollect();
        List<SceneItem> sceneItemList = GameCenter.curGameStage.GetSceneItems();
        for (int i = 0,length=sceneItemList.Count; i < length; i++)
        {
            SceneItem item = sceneItemList[i];
            if (item != null) item.ClearCollectEffect();
        }
    }

    /// <summary>
    /// 切换目标
    /// </summary>
    public override void ChangeTarget()
    {
        base.ChangeTarget();
        CancelCommands();
        if (aiFithtFSM != null)//自动战斗中切换目标
        {
            aiFithtFSM.ChangeTarget();
        }
        else//非自动战斗中切换目标
        {
            SmartActor thisTarget = null;
            float distance = 0;
			//全体模式下可以切换目标玩家
			if(actorInfo.CurPkMode == PkMode.PKMODE_ALL)
			{
				thisTarget = GameCenter.curGameStage.GetClosestPlayer(this, RelationType.AUTOMATEDATTACKS, ref distance);
				//没有玩家目标,切换怪物
				if (thisTarget == null || !thisTarget.isDead)
				{
					thisTarget = GameCenter.curGameStage.GetAnotherMob(thisTarget == null ? -1 : thisTarget.id);
				}
			}else
			{
				thisTarget = GameCenter.curGameStage.GetAnotherMob(thisTarget == null ? -1 : thisTarget.id);
			}
            if (thisTarget == null)
            {
                thisTarget = GameCenter.curGameStage.GetClosestMob(this, ref distance);
            }
            if (thisTarget != null)
                CurTarget = thisTarget;
        }
    }

    /// <summary>
    /// 跳跃 by吴江
    /// </summary>
    public override void Jump()
    {
        if (!inputListener.CheckLock()) return;
        base.Jump();
        GameCenter.mainPlayerMng.C2S_Jump();
    }

    public override void MoveTo(Vector3[] _path, float _rotY = 0, bool _faceWhenStop = false)
    {
        hasCtrlAwake = true;
        base.MoveTo(_path, _rotY, _faceWhenStop);
    }

    public override void MoveTowards(Vector3 _dir)
    {
        hasCtrlAwake = true;
        base.MoveTowards(_dir);
    }


    public void UnLockRef()
    {
        if (aiFithtFSM != null)
        {
            aiFithtFSM.UnRegist();
        }
    }

    /// <summary>
    /// 尝试获取一定范围内的敌对目标作为攻击目标(优先面前) by吴江
    /// </summary>
    public void TryGetColosestAttackTarget(RelationType _type, float _distanceLimit)
    {
        if (CurTarget == null || !PlayerAutoFightFSM.IsEnemy(CurTarget))
        {
            float distance = float.MaxValue;
            SmartActor temp = GameCenter.curGameStage.GetClosestSmartActorInFront(this, _type, ref distance);
            if (temp == null || distance > _distanceLimit)
            {
                temp = GameCenter.curGameStage.GetClosestSmartActor(this, _type, ref distance);
            }
            if (distance > _distanceLimit)
            {
                return;
            }
            CurTarget = temp;
        }
    }

    /// <summary>
    /// 自动吃药
    /// </summary>
    void OnAutoFight()
    {
        if (actorInfo.IsAlive && GameCenter.systemSettingMng.IsSafeOpen && actorInfo.IsCanUseDrug)
        {
            if ((float)GameCenter.systemSettingMng.SafeLifeNum / 100 >= (float)actorInfo.CurHP / (float)actorInfo.MaxHP)
            {
                //int specialItem = 0;
                List<int> list = ConfigMng.Instance.GetHpAutoItemByLev(actorInfo.CurLevel);
                bool b = false;
                if (GameCenter.systemSettingMng.SafeModel)//从大到小使用
                {
                    for (int i = list.Count - 1; i > -1; i--)
                    {
                        if (b)
                            continue;
                        if (GameCenter.inventoryMng.GetNumberByType(list[i]) > 0)
                        {
                            //Debug.Log("自动战斗自动使用血瓶");
                            GameCenter.inventoryMng.C2S_UseItem(GameCenter.inventoryMng.GetEquipByType(list[i]));
                            i = -100;
                            b = true;
                            GameCenter.shopMng.ShowedHPDrugBtn = false;
                            //showDrugLackWndHp = true;
                        }
                        else
                        {
                            if (!GameCenter.systemSettingMng.IsAutoBuy) GameCenter.shopMng.ShowedHPDrugBtn = true;
                        }
                    }
                    if (!b)
                    {
                        if (GameCenter.systemSettingMng.IsAutoBuy)
                        {
                            GameCenter.shopMng.ShowedHPDrugBtn = false;
                            int price = ConfigMng.Instance.GetAutoItemPriceByLev(actorInfo.CurLevel, 1);
                            if (actorInfo.TotalCoinCount >= (ulong)price)
                                AskAutoBuy(1);
                        }
                        else
                        {
                            //if (!GameCenter.shopMng.ShowedHPDrugBtn)
                            {
                                GameCenter.shopMng.CurLackHPItemInfo = ConfigMng.Instance.GetAutoItemTypeByLev(actorInfo.CurLevel, 1);
                                if (GameCenter.inventoryMng.GetNumberByType(GameCenter.shopMng.CurLackHPItemInfo.EID) > 0)
                                {
                                    GameCenter.shopMng.ShowedHPDrugBtn = false;
                                }
                                else
                                {
                                    GameCenter.shopMng.ShowedHPDrugBtn = true;
                                }
                                //if (GameCenter.shopMng.ShowDrugBtn != null)
                                //{
                                //    GameCenter.shopMng.ShowDrugBtn();
                                //}
                                //GameCenter.shopMng.ShowedHPDrugBtn = true;
                                //GameCenter.uIMng.SwitchToUI(GUIType.DRUGLACKWND);
                                //showDrugLackWndHp = false;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (b)
                            continue;
                        if (GameCenter.inventoryMng.GetNumberByType(list[i]) > 0)
                        {
                            //Debug.Log("自动战斗自动使用血瓶");
                            GameCenter.inventoryMng.C2S_UseItem(GameCenter.inventoryMng.GetEquipByType(list[i]));
                            i = list.Count;
                            b = true;
                            GameCenter.shopMng.ShowedHPDrugBtn = false;
                            //showDrugLackWndHp = true;
                        } 
                        else
                        {
                            if (!GameCenter.systemSettingMng.IsAutoBuy) GameCenter.shopMng.ShowedHPDrugBtn = true;
                        }
                    }
                    //if (!b)
                    //{
                    //    if (specialItem != 0)
                    //    {
                    //        if (GameCenter.inventoryMng.GetNumberByType(specialItem) > 0)
                    //        {
                    //            //Debug.Log("自动战斗自动使用血瓶");
                    //            GameCenter.inventoryMng.C2S_UseItem(GameCenter.inventoryMng.GetEquipByType(specialItem));
                    //            b = true;
                    //            //showDrugLackWndHp = true;
                    //            GameCenter.shopMng.ShowedHPDrugBtn = false;
                    //        }
                    //        else
                    //        {
                    //            if (!GameCenter.systemSettingMng.IsAutoBuy) GameCenter.shopMng.ShowedHPDrugBtn = true;
                    //        }
                    //    }
                    //}
                    if (!b)
                    {
                        if (GameCenter.systemSettingMng.IsAutoBuy)
                        {
                            GameCenter.shopMng.ShowedHPDrugBtn = false;
                            int price = ConfigMng.Instance.GetAutoItemPriceByLev(actorInfo.CurLevel, 1);
                            if (actorInfo.TotalCoinCount >= (ulong)price)
                                AskAutoBuy(1);
                        }
                        else
                        {
                            //if (!GameCenter.shopMng.ShowedHPDrugBtn)
                            {
                                GameCenter.shopMng.CurLackHPItemInfo = ConfigMng.Instance.GetAutoItemTypeByLev(actorInfo.CurLevel, 1);
                                if (GameCenter.inventoryMng.GetNumberByType(GameCenter.shopMng.CurLackHPItemInfo.EID) > 0)
                                {
                                    GameCenter.shopMng.ShowedHPDrugBtn = false;
                                }
                                else
                                {
                                    GameCenter.shopMng.ShowedHPDrugBtn = true;
                                }
                                //if (GameCenter.shopMng.ShowDrugBtn != null)
                                //{
                                //    GameCenter.shopMng.ShowDrugBtn();
                                //}
                                //GameCenter.shopMng.ShowedHPDrugBtn = true;
                                //GameCenter.uIMng.SwitchToUI(GUIType.DRUGLACKWND);
                                //showDrugLackWndHp = false;
                            }
                        }
                    }
                }
            }
            else 
            {
                GameCenter.shopMng.ShowedHPDrugBtn = false;
            }
 //               Debug.Log((float)GameCenter.systemSettingMng.SafeMagicNum / 100 + ":" + (float)actorInfo.CurMP / (float)actorInfo.MaxMP);
                if ((float)GameCenter.systemSettingMng.SafeMagicNum / 100 >= (float)actorInfo.CurMP / (float)actorInfo.MaxMP)
                {

                    List<int> list = ConfigMng.Instance.GetMpAutoItemByLev(actorInfo.CurLevel);
                    bool b = false;
                    if (GameCenter.systemSettingMng.SafeModel)
                    {
                        for (int i = list.Count - 1; i > -1; i--)
                        {
                            if (b)
                                continue;
                            if (GameCenter.inventoryMng.GetNumberByType(list[i]) > 0)
                            {
                                // Debug.Log("自动战斗自动使用蓝瓶");
                                GameCenter.inventoryMng.C2S_UseItem(GameCenter.inventoryMng.GetEquipByType(list[i]));
                                b = true;
                                i = -100;
                                GameCenter.shopMng.ShowedMPDrugBtn = false;
                                //showDrugLackWndMp = true;
                            }
                            else
                            {
                                if (!GameCenter.systemSettingMng.IsAutoBuy) GameCenter.shopMng.ShowedMPDrugBtn = true;
                            }
                        }

                        if (!b)
                        {
                            if (GameCenter.systemSettingMng.IsAutoBuy)
                            {
                                GameCenter.shopMng.ShowedMPDrugBtn = false;
                                int price = ConfigMng.Instance.GetAutoItemPriceByLev(actorInfo.CurLevel, 2);
                                if (actorInfo.TotalCoinCount >= (ulong)price)
                                    AskAutoBuy(2);
                            }
                            else
                            {
                                //if (!GameCenter.shopMng.ShowedMPDrugBtn)
                                {
                                    GameCenter.shopMng.CurLackMPItemInfo = ConfigMng.Instance.GetAutoItemTypeByLev(actorInfo.CurLevel, 2);
                                    if (GameCenter.inventoryMng.GetNumberByType(GameCenter.shopMng.CurLackMPItemInfo.EID) > 0)
                                    {
                                        GameCenter.shopMng.ShowedMPDrugBtn = false;
                                    }
                                    else
                                    {
                                        GameCenter.shopMng.ShowedMPDrugBtn = true;
                                    }
                                    //if (GameCenter.shopMng.ShowDrugBtn != null)
                                    //{
                                    //    GameCenter.shopMng.ShowDrugBtn();
                                    //}
                                    //GameCenter.shopMng.ShowedMPDrugBtn = true;
                                    //GameCenter.uIMng.SwitchToUI(GUIType.DRUGLACKWND);
                                    //showDrugLackWndMp = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (b)
                                continue;
                            if (GameCenter.inventoryMng.GetNumberByType(list[i]) > 0)
                            {
                                //Debug.Log("自动战斗自动使用蓝瓶");
                                GameCenter.inventoryMng.C2S_UseItem(GameCenter.inventoryMng.GetEquipByType(list[i]));
                                b = true;
                                i = list.Count;
                                GameCenter.shopMng.ShowedMPDrugBtn = false;
                                //showDrugLackWndMp = true;

                            }
                            else
                            {
                                if (!GameCenter.systemSettingMng.IsAutoBuy) GameCenter.shopMng.ShowedMPDrugBtn = true;
                            }
                        }
                        if (!b)
                        {
                            if (GameCenter.systemSettingMng.IsAutoBuy)
                            {
                                GameCenter.shopMng.ShowedMPDrugBtn = false;
                                int price = ConfigMng.Instance.GetAutoItemPriceByLev(actorInfo.CurLevel, 2);
                                if (actorInfo.TotalCoinCount >= (ulong)price)
                                    AskAutoBuy(2);
                            }
                            else
                            {
                                //if (!GameCenter.shopMng.ShowedMPDrugBtn)
                                {
                                    GameCenter.shopMng.CurLackMPItemInfo = ConfigMng.Instance.GetAutoItemTypeByLev(actorInfo.CurLevel, 2);
                                    if (GameCenter.inventoryMng.GetNumberByType(GameCenter.shopMng.CurLackMPItemInfo.EID) > 0)
                                    {
                                        GameCenter.shopMng.ShowedMPDrugBtn = false;
                                    }
                                    else
                                    {
                                        GameCenter.shopMng.ShowedMPDrugBtn = true;
                                    }
                                    //if (GameCenter.shopMng.ShowDrugBtn != null)
                                    //{
                                    //    GameCenter.shopMng.ShowDrugBtn();
                                    //}
                                    //GameCenter.shopMng.ShowedMPDrugBtn = true;
                                    //GameCenter.uIMng.SwitchToUI(GUIType.DRUGLACKWND);
                                    //showDrugLackWndMp = false;
                                }
                            }
                        }
                    }
                }
                else 
                {
                    GameCenter.shopMng.ShowedMPDrugBtn = false;
                }
        }
    }




    void OnDeath()
    {
        if (isDead&&GameCenter.uIMng.CurOpenType==GUIType.NONE)
        {
            if ((int)(GameCenter.resurrectionMng.ReviveTime - (Time.time - GameCenter.resurrectionMng.GotTime)) > 0)
            {
                GameCenter.uIMng.SwitchToUI(GUIType.RESURRECTION);
            }
        }

    }

    /// <summary>
    /// 请求自动购买药品并使用 1血2蓝
    /// </summary>
    void AskAutoBuy(int _type)
    {
        //Debug.Log("发送请求买药"+_type);
        pt_figtht_quik_buy_d687 msg = new pt_figtht_quik_buy_d687();
        msg.type = _type;
        NetMsgMng.SendMsg(msg);
    }
}
