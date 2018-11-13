//=========================================
//作者：吴江
//日期：2015/5/15
//用途：主随从表现层对象
//===========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 主玩家表现层对象 by吴江
/// </summary>
public class MainEntourage : EntourageBase
{
    #region 数据
    [System.NonSerialized]
    public AbilityMng abilityMng;
    /// <summary>
    /// 命令管理器 by吴江
    /// </summary>
    [System.NonSerialized]
    public CommandMng commandMng;


    public bool isCastingNoMove
    {
        get
        {
            if (animFSM == null || moveFSM == null) return true;
            return animFSM.IsCasting || moveFSM.isMoveLocked;
        }
    }


    /// <summary>
    /// 当前目标发生变化的事件
    /// </summary>
    public System.Action OnTargetChange;




    public enum AttackType
    {
        NONE,
        NORMALFIGHT,//自动战斗
    }

    /// <summary>
    /// 自动战斗AI状态机
    /// </summary>
    protected EntourageAutoFightFSM aiFithtFSM;

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
    }
    /// <summary>
    /// 主玩家除了基类的每帧逻辑外，还需要每帧执行一次命令管理器 by吴江
    /// </summary>
    protected new void Update()
    {
        base.Update();
        commandMng.Tick();
        stateMachine.Update();
    }

    void OnDisable()
    {
    }


    new void OnDestroy()
    {
        base.OnDestroy();
        if(actorInfo!=null)
        actorInfo.OnAwakeUpdate -= OnStartAI;
    }
    #endregion

    #region 构造
    /// <summary>
    /// 创建净数据对象
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static MainEntourage CreateDummy(MercenaryInfo _info)
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
        newGO.tag = "Entourage";
        newGO.SetMaskLayer(LayerMask.NameToLayer("Entourage"));
        MainEntourage newMPC = newGO.AddComponent<MainEntourage>();
        newMPC.isDummy_ = true;
        newMPC.moveFSM = newGO.AddComponent<MainEntourageMoveFSM>();
        MainEntourageMoveFSM movefsm = newMPC.moveFSM as MainEntourageMoveFSM;
        movefsm.SetInfo(_info);
        newMPC.id = _info.ServerInstanceID;
        newMPC.actorInfo = _info;
        newMPC.curMoveSpeed = newMPC.actorInfo.StaticSpeed * MOVE_SPEED_BASE;
        newMPC.CurRealSpeed = newMPC.curMoveSpeed;
        newMPC.InitPos();
        return newMPC;
    }


    public void StartAsyncCreate(System.Action<MainEntourage> _callback = null)
    {
        inited_ = false;
        StartCoroutine(CreateAsync(_callback));
    }

    IEnumerator CreateAsync(System.Action<MainEntourage> _callback = null)
    {
        if (isDummy_ == false)
        {
            GameSys.LogError("You can only start create other player in dummy: " + actorInfo.ServerInstanceID);
            yield break;
        }
        //
        isDownloading_ = true;				//判断是否正在下载，防止重复创建

        MainEntourage mpc = null;
        pendingDownload = Create(actorInfo, delegate(MainEntourage _mpc, EResult _result)
        {
            if (_result != EResult.Success)
            {
                return;
            }

            mpc = _mpc;
            mpc.Show(true);
            pendingDownload = null;
            isDownloading_ = false;
            if (!actorInfo.IsAlive)
            {
                mpc.Dead(true);
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
    protected AssetMng.DownloadID Create(MercenaryInfo _data, System.Action<MainEntourage, EResult> _callback)
    {
        return exResources.GetEntourage(_data.AssetName,
                                 delegate(GameObject _asset, EResult _result)
                                 {
                                     if (_result != EResult.Success)
                                     {
                                         _callback(null, _result);
                                         return;
                                     }
                                     this.gameObject.name = "MainEntourage[" + actorInfo.ServerInstanceID + "]";
                                     GameObject newGo = Instantiate(_asset) as GameObject;
                                     animationRoot = newGo.transform;
                                     newGo.name = _asset.name;
                                     newGo.transform.parent = transform;
                                     newGo.transform.localEulerAngles = Vector3.zero;
                                     newGo.transform.localPosition = Vector3.zero;
                                     newGo.AddComponent<SmartActorAnimFSM>();
                                     newGo.AddComponent<SmartActorRendererCtrl>();
                                     this.gameObject.AddComponent<FXCtrl>();
                                     newGo.transform.localScale *= actorInfo.ModelScale;
                                     newGo.SetActive(false);
                                     newGo.SetActive(true);

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
        gameObject.SetMaskLayer(LayerMask.NameToLayer("Entourage"));
        rendererCtrl.ResetOriginalLayer(LayerMask.NameToLayer("Entourage"));
        rendererCtrl.ResetLayer();
        headTextCtrl.SetPetName(actorInfo.NoColorName);
        headTextCtrl.SetPetNameColor(actorInfo.PetNameColor);
        headTextCtrl.SetPetOwnerName(actorInfo.NoColorOwnerName);
        if (actorInfo.PetTitleName != string.Empty)
        {
            headTextCtrl.SetTitleSprite(actorInfo.PetTitleName);
        }
        MainPlayerFocus = true;
        if (aiFithtFSM == null)
        {
            aiFithtFSM = this.gameObject.GetComponent<EntourageAutoFightFSM>();
            if (aiFithtFSM == null)
            {
                aiFithtFSM = this.gameObject.AddComponent<EntourageAutoFightFSM>();
            }
            aiFithtFSM.commandMng = commandMng;
            aiFithtFSM.Init(actorInfo);
        }

        abilityMng = GameCenter.abilityMng;
        stateMachine.Start();
        stateMachine.Send((int)EventType.AI_FIGHT_CTRL);
        inited_ = true;

        if (!actorInfo.IsAlive)
        {
            Dead(true);
        }
        fxCtrl.DoShadowEffect(true);
        inited_ = true;
        if (animFSM != null)
        {
            InitAnimation();
        }
        if (moveFSM != null)
        {
            moveFSM.FaceTo(180);
        }
        if (GameCenter.sceneMng.EnterSucceed && !actorInfo.HasAwake)
        {
            GameCenter.mercenaryMng.C2S_EntrouageAwake();
        }
    }

    protected override void InitAnimation()
    {
        animFSM.SetupIdleAndMoveAnimationName("idle2", "move2");
        animFSM.SetupCombatAnimationName("idle2", null);
        animFSM.SetRandIdleAnimationName("waiting", null);
        animFSM.SetupInjuryAnimation("kickdown", null, false);
        animFSM.SetupInjuryUpAnimation("getup", null, false);
        animFSM.SetupDeathAnimation("striked", null);
        animFSM.fxCtrlRef = fxCtrl;
        animFSM.headTextCtrlRef = headTextCtrl;
        animFSM.moveFSMRef = moveFSM;
        CityStage city = GameCenter.curGameStage as CityStage;
        animFSM.SetInCombat(city == null);
        animFSM.StartStateMachine();
    }

    /// <summary>
    /// 注册事件监听
    /// </summary>
    protected override void Regist()
    {
        base.Regist();
        GameCenter.sceneMng.SceneStateChange += OnSceneStateChange;
        actorInfo.OnAwakeUpdate += OnStartAI;
        actorInfo.OnPetNameUpdate += OnNameChange;
        actorInfo.OnPetAptitudeUpdate += OnPetNameColorChange;
        actorInfo.OnPetTtleUpdate += OnTitleNameChange;
        actorInfo.OnOwnerNameUpdateEvent += OnOwnerNameUpdate;
    }
    /// <summary>
    /// 注销事件监听
    /// </summary>
    public override void UnRegist()
    {
        base.UnRegist();
        GameCenter.sceneMng.SceneStateChange -= OnSceneStateChange;
        actorInfo.OnAwakeUpdate -= OnStartAI;
        actorInfo.OnPetNameUpdate -= OnNameChange;
        actorInfo.OnPetTtleUpdate -= OnTitleNameChange;
        actorInfo.OnPetAptitudeUpdate -= OnPetNameColorChange;
        actorInfo.OnOwnerNameUpdateEvent -= OnOwnerNameUpdate;
    }

    public void UnLockRef()
    {
        //if (aiFithtFSM != null)
        //{
        //    aiFithtFSM.UnRegist();
        //}
    }
     #endregion


    /// <summary>
    /// 初始化坐标 by吴江
    /// </summary>
    public void InitPos()
    {
        Vector3 initpos = Utils.GetRandomPos(GameCenter.curMainPlayer.transform);
        GameCenter.curGameStage.PlaceGameObjectFromServer(this, initpos.x, initpos.z, (int)this.transform.localEulerAngles.y);
        StopForCheckMsg();
    }


    protected override void OnOwnerNameUpdate()
    {
        base.OnOwnerNameUpdate();
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public override void Dead(bool _already)
    {
        ForceCancelAbility();
        base.Dead(_already);
        moveFSM.LockMoving();
    }
    /// <summary>
    /// 禁止外部使用
    /// </summary>
    /// <param name="_instance"></param>
    public override void UseAbility(AbilityInstance _instance)
    {
        if (isDead) return;
        if (_instance.thisSkillMode != SkillMode.NORMALSKILL && isSilent)
        {
            return;
        }
        curTryUseAbility = null;
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
        if (isRigidity || isDead || _instance == null) return false;
        if (_instance.thisSkillMode != SkillMode.NORMALSKILL && isSilent)
        {
            return false;
        }
        if (abilityMng != null && abilityMng.HasLockState)
        {
            return false;
        }
        if (moveFSM != null && moveFSM.isMoving)
        {
            StopMovingTo();
        }
        CancelAbility();

        commandMng.CancelCommands();
        curTryUseAbility = _instance;
        if (_trace)
        {
            if (_instance.TargetActor == null)
            {
                if (_instance.NeedTarget) return false;
                Command_CastAbilityOn cmdCastAbilityOn = new Command_CastAbilityOn();
                cmdCastAbilityOn.abilityInstance = _instance;
                cmdCastAbilityOn.target = noTarget ? null : _instance.TargetActor; ;
                commandMng.PushCommand(cmdCastAbilityOn);

            }
            else
            {
                Vector3[] path = GameStageUtility.StartPath(transform.position, _instance.TargetActor.transform.position);
                if (path == null || path.Length == 0)
                {
                    if (_instance.TargetActor != null && (_instance.TargetActor.transform.position - this.transform.position).sqrMagnitude <= 2.0f * 2.0f)//有怪物(玩家和怪物重叠)直接打怪
                    {
                        Command_CastAbilityOn cmdCastAbilityOn = new Command_CastAbilityOn();
                        cmdCastAbilityOn.abilityInstance = _instance;
                        cmdCastAbilityOn.target = noTarget ? null : _instance.TargetActor;
                        commandMng.PushCommand(cmdCastAbilityOn);
                    }
                }
                else
                {
                    //
                    Command_TraceTarget cmdTraceTarget = new Command_TraceTarget();
                    cmdTraceTarget.abilityInstance = _instance;
                    cmdTraceTarget.target = _instance.TargetActor;
                    cmdTraceTarget.updatePathDeltaTime = 1.0f;
                    commandMng.PushCommand(cmdTraceTarget);

                    //
                    Command_CastAbilityOn cmdCastAbilityOn = new Command_CastAbilityOn();
                    cmdCastAbilityOn.abilityInstance = _instance;
                    cmdCastAbilityOn.target = noTarget ? null : _instance.TargetActor; ;
                    commandMng.PushCommand(cmdCastAbilityOn);
                }
            }
        }
        else
        {
            if (_instance.TargetActor != null && !noTarget)
            {
                FaceToNoLerp(_instance.TargetActor.gameObject.transform.position - transform.position);
            }
            //这里一定不能直接使用技能,而必须是压入命令.  压入命令的话,执行命令是下一帧(因为必须先等本帧的状态机切换结束)  by 吴江
            Command_CastAbilityOn cmdCastAbilityOn = new Command_CastAbilityOn();
            cmdCastAbilityOn.abilityInstance = _instance;
            cmdCastAbilityOn.target = noTarget ? null : _instance.TargetActor; ;
            commandMng.PushCommand(cmdCastAbilityOn);
        }
        return true;
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
        commandMng.CancelCommands();
    }

    #region 状态机
    /// <summary>
    /// 游戏状态枚举 by吴江
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// 普通状态,跟随主角
        /// </summary>
        NORMAL = fsm.Event.USER_FIELD + 1,
        /// <summary>
        /// 自动战斗中
        /// </summary>
        AI_FIGHT_CTRL,
        /// <summary>
        /// 挂起状态
        /// </summary>
        HOLD_ON,
    }

    protected fsm.State taksPathFind;
    protected fsm.State aiFight;
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


        aiFight = new fsm.State("aiFight", stateMachine);
        aiFight.onEnter += EnterAiFightState;
        aiFight.onExit += ExitAiFightState;
        aiFight.onAction += UpdateAiFightState;


        holdOn = new fsm.State("holdOn", stateMachine);
        holdOn.onEnter += EnterHoldOnState;
        holdOn.onExit += ExitHoldOnState;
        holdOn.onAction += UpdateHoldOnState;



        normal.Add<fsm.EventTransition>(aiFight, (int)EventType.AI_FIGHT_CTRL);
        normal.Add<fsm.EventTransition>(holdOn, (int)EventType.HOLD_ON);


        aiFight.Add<fsm.EventTransition>(normal, (int)EventType.NORMAL);
        aiFight.Add<fsm.EventTransition>(holdOn, (int)EventType.HOLD_ON);
 

        holdOn.Add<fsm.EventTransition>(aiFight, (int)EventType.AI_FIGHT_CTRL);
        holdOn.Add<fsm.EventTransition>(normal, (int)EventType.NORMAL);



        stateMachine.initState = holdOn;
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
    #region 自动战斗状态
    protected virtual void EnterAiFightState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (aiFithtFSM != null)
        {
            if (animFSM != null)
            {
                animFSM.SetInCombat(true);
            }
            aiFithtFSM.enabled = true;
            aiFithtFSM.StartStateMachine();
            curFSMState = EventType.AI_FIGHT_CTRL;
        }
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
    #region 挂起状态
    protected virtual void EnterHoldOnState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        curFSMState = EventType.HOLD_ON;
        StopMovingTo();
        CancelAbility();
        if (moveFSM != null)
        {
            moveFSM.LockMoving();
        }
        commandMng.CancelCommands();
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
    /// 切换场景中，需要挂起主玩家
    /// </summary>
    /// <param name="_succeed"></param>
    protected void OnSceneStateChange(bool _succeed)
    {
        if (!isDummy && _succeed)
        {
            GameCenter.mercenaryMng.C2S_EntrouageAwake();
        }
        if (_succeed)
        {
            actorInfo.CleanBuff();
//            if (headTextCtrl != null)
//            {
//                headTextCtrl.HideBlood();
//            }
            SetInfoInCombat(!(GameCenter.curGameStage is CityStage));
        }
        else
        {
            actorInfo.HasAwake = false;
        }
    }


    /// <summary>
    /// 返回进入挂起状态前的状态
    /// </summary>
    public void GoBackOn()
    {
        stateMachine.Send((int)lastFSMState);
    }

    /// <summary>
    /// 进入挂起状态
    /// </summary>
    public void GoHoldOn()
    {
        stateMachine.Send((int)EventType.HOLD_ON);
    }


    /// <summary>
    /// 返回正常状态,结束诸如自动战斗，任务寻路等状态。但在挂起状态中无用 by吴江
    /// </summary>
    public void GoNormal()
    {
        if (curFSMState != EventType.HOLD_ON && curFSMState != EventType.NORMAL)
        {
            stateMachine.Send((int)EventType.NORMAL);
        }
    }
    /// <summary>
    /// 进入战斗AI状态 by吴江
    /// </summary>
    public void GoFight()
    {
        if (curFSMState != EventType.AI_FIGHT_CTRL)
        {
            stateMachine.Send((int)EventType.AI_FIGHT_CTRL);
        }
    }



    /// <summary>
    /// 切换目标
    /// </summary>
    public override void ChangeTarget()
    {
        base.ChangeTarget();
        commandMng.CancelCommands();
        if (aiFithtFSM != null)//自动战斗中切换目标
        {
            aiFithtFSM.ChangeTarget();
        }
    }
    /// <summary>
    /// 技能变化
    /// </summary>
    protected void OnSkillChangeEvent()
    {
        abilityMng.SetEntourage(this, actorInfo);
    }

    protected void OnStartAI(bool _awake)
    {
        if (_awake)
        {
            if (this.gameObject == null) return;
            id = actorInfo.ServerInstanceID;
            this.gameObject.name = "MainEntourage[" + actorInfo.ServerInstanceID + "]";
            abilityMng.SetEntourage(this, actorInfo);
            Command_MoveTo movetoCMD = new Command_MoveTo();
            movetoCMD.destPos =Utils.GetRandomPos(GameCenter.curMainPlayer.transform);
            commandMng.PushCommand(movetoCMD);
            GoFight();
            if (GameCenter.curGameStage.GetEntourage(actorInfo.ServerInstanceID) == null)
            {
                GameCenter.curGameStage.AddObject(this);
            }
        }
    } 
}
