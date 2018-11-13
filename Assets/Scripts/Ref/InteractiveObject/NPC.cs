//===========================================
//作者：吴江
//日期: 2015/5/22
//用途：NPC表现层对象
//===========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// NPC表现层对象 by吴江
/// </summary>
public class NPC : SmartActor
{
    /// <summary>
    /// 基本数据
    /// </summary>
    protected new NPCInfo actorInfo
    {
        get { return base.actorInfo as NPCInfo; }
        set
        {
            base.actorInfo = value;
        }
    }


    /// <summary>
    /// 动画控制器
    /// </summary>
    [System.NonSerialized]
    protected new NPCAnimFSM animFSM = null;

    protected new NPCRendererCtrl rendererCtrl;

    public string NpcName
    {
        get
        {
            return actorInfo.Name;
        }
    }



    public float FocusX
    {
        get
        {
            return actorInfo.FocusX;
        }
    }

    public float FocusY
    {
        get
        {
            return actorInfo.FocusY;
        }
    }

    public float FocusDistance
    {
        get
        {
            return actorInfo.FocusDistance;
        }
    }

    private Animation cachedAnimation_;
    private bool cacheAnimationInited = false;
    public Animation cachedAnimation
    {
        get
        {
            if (cachedAnimation_ == null && !cacheAnimationInited)
            {
                cachedAnimation_ = animationRoot.gameObject.GetComponent<Animation>();
                cacheAnimationInited = true;
            }
            return cachedAnimation_;
        }
    }


    /// <summary>
    /// NPC类型  by吴江
    /// </summary>
    public NPCType npcType = NPCType.NORMOL;

    /// <summary>
    /// 创建NPC的净数据对象 by吴江
    /// </summary>
    /// <param name="_info"></param>
    /// <returns></returns>
    public static NPC CreateDummy(NPCInfo _info)
    {
        GameObject newGO = null;
        if (GameCenter.instance.dummyMobPrefab != null)
        {
            newGO = Instantiate(GameCenter.instance.dummyNpcPrefab) as GameObject;
            newGO.name = "Dummy NPC [" + _info.ServerInstanceID + "]";
        }
        else
        {
            newGO = new GameObject("Dummy NPC[" + _info.ServerInstanceID + "]");
        }
        newGO.tag = "NPC";
        newGO.SetMaskLayer(LayerMask.NameToLayer("NPC"));
        NPC npc = newGO.AddComponent<NPC>();
        npc.actorInfo = _info;
        npc.moveFSM = newGO.AddComponent<ActorMoveFSM>();
        npc.curMoveSpeed = npc.actorInfo.StaticMoveSpeed * MOVE_SPEED_BASE;
        npc.CurRealSpeed = npc.curMoveSpeed;
        npc.isDummy_ = true;
        npc.RegistMoveEvent(true);
        GameCenter.curGameStage.PlaceGameObjectFromServer(npc, _info.ServerPos.x, _info.ServerPos.z, _info.RotationY);
        GameCenter.curGameStage.AddObject(npc);
        npc.ReGetTaskList();
        return npc;
    }


    public virtual void StartAsyncCreate(Action<NPC> _callback)
    {
        StartCoroutine(CreateAsync(_callback));
    }

    protected IEnumerator CreateAsync(Action<NPC> _callback)
    {
        if (isDummy_ == false)
        {
            GameSys.LogError("You can only start create NPC in dummy: " + actorInfo.ServerInstanceID);
            yield break;
        }

        isDownloading_ = true;
        NPC npc = null;
        //NPCRendererCtrl myRendererCtrl = null;
        bool faild = false;
        pendingDownload = Create(actorInfo, delegate(NPC _npc, EResult _result)
        {
            if (_result != EResult.Success)
            {
                faild = true;
                return;
            }
            npc = _npc;
            pendingDownload = null;

            //myRendererCtrl = npc.GetComponent<NPCRendererCtrl>();   //TO TO:初始化出来应该先隐藏，由任务等其他信息决定是否要显示 by吴江
            //myRendererCtrl.Show(false, true);
        });
        while (npc == null || npc.inited == false)
        {
            if (faild) yield break;
            yield return null;
        }

        pendingDownload = null;
        isDownloading_ = false;
        if (_callback != null)
        {
            _callback(npc);
        }
    }

    protected void OnUpdateAI()
    {
        //停止AI
        stateMachine.Send((int)EventType.Check);
        stateMachine.Stop();
        
        //还原坐标
       // GameCenter.curGameStage.PlaceGameObjectFromServer(this, (int)actorInfo.ServerPos.x, (int)actorInfo.ServerPos.y, actorInfo.RotationY);

        //删除表现层对象
        int count = this.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            GameObject obj = this.transform.GetChild(i).gameObject;
            if (obj == headTextCtrl.TextParent) continue;
            Destroy(this.transform.GetChild(i).gameObject);
        }
        if(headTextCtrl != null)
        {
            Destroy(headTextCtrl);
            headTextCtrl = null;
        }
        isDummy_ = true;

        //重新创建
        StartAsyncCreate(null);
    } 


    protected AssetMng.DownloadID Create(NPCInfo _info, System.Action<NPC, EResult> _callback)
    {
        return exResources.GetNPC(_info.Type, delegate(GameObject _asset, EResult _result)
        {
            if (GameCenter.IsDummyDestroyed(ObjectType.NPC, _info.ServerInstanceID))
            {
                _callback(null, EResult.Failed);
                return;
            }
            if (_result != EResult.Success)
            {
                _callback(null, _result);
                return;
            }

            this.gameObject.name = "NPC[" + _info.ServerInstanceID + "]";

            GameObject newGO = Instantiate(_asset) as GameObject;
            newGO.name = _asset.name;
            animationRoot = newGO.transform;
            newGO.transform.parent = this.gameObject.transform;
            newGO.transform.localEulerAngles = Vector3.zero;
            newGO.transform.localPosition = Vector3.zero;
            newGO.transform.localScale = Vector3.one * _info.ModulScale;
            newGO.AddComponent<NPCRendererCtrl>();
            newGO.AddComponent<FXCtrl>();
            newGO.SetActive(false);
            newGO.SetActive(true);
            if (_info.IsSmart)
            {
                newGO.AddComponent<NPCAnimFSM>();
            }


            animationRoot = newGO.transform;
            isDummy_ = false;

            Init();
            _callback(this, _result);
        });
    }



    protected new void Awake()
    {
        typeID = ObjectType.NPC;
        base.Awake();
    }


    protected override void Init()
    {
        height = actorInfo.Hight;
        nameHeight = height;

        if (headTextCtrl == null) headTextCtrl = this.gameObject.AddComponent<HeadTextCtrl>();
        headTextCtrl.SetName(actorInfo.Name);
        headTextCtrl.SetTitle(actorInfo.Title);

        base.Init();

        rendererCtrl = this.gameObject.GetComponentInChildrenFast<NPCRendererCtrl>();
        rendererCtrl.Init(actorInfo, fxCtrl);
        inited_ = true;


        curMoveSpeed = actorInfo.StaticMoveSpeed * MOVE_SPEED_BASE;
        CurRealSpeed = curMoveSpeed;

        if (isDummy) return;

        InitAnimation();

        this.gameObject.tag = "NPC";
        ActiveBoxCollider(true, actorInfo.ColliderRadius);
        rendererCtrl.ResetOriginalLayer(LayerMask.NameToLayer("NPC"));
        rendererCtrl.SetLayer(this.gameObject.layer);
        fxCtrl.DoShadowEffect(true);
      //  OnTaskUpDate(TaskDataType.Started, TaskType.UnKnow);
        inited_ = true;

        if (stateMachine != null)
        {
            stateMachine.Start();
        }
    }

    protected override void InitAnimation()
    {
        var ani = cachedAnimation;
        if (ani && ani["idle1"])
        {
            ani["idle1"].wrapMode = WrapMode.Loop;
            ani.Play("idle1");
        }
        animFSM = base.animFSM as NPCAnimFSM;
        if (animFSM != null)
        {
            animFSM.SetAnimationName("idle1", "move2", "rise", "rise", "wait", "move2");
            animFSM.StartStateMachine();
            animFSM.SetMoveSpeed(actorInfo.ModelMoveScale <= 0 ? 1.0f : CurRealSpeed / actorInfo.ModelMoveScale);
        }
    }

    /// <summary>
    /// 勿删，Start（）实际在dummy状态被调用，要在启动时做的事情请去自定义的Init()接口 by吴江
    /// </summary>
    protected  void Start()
    {
    }


    protected override void Regist()
    {
        base.Regist();
        OnTaskUpDate(TaskDataType.Started, TaskType.UnKnow);
        GameCenter.taskMng.OnTaskListUpdate += OnTaskUpDate;
        GameCenter.taskMng.AddNewTask += OnTaskUpDate;
        GameCenter.taskMng.CommitForNewTask += OnTaskUpDate;
        GameCenter.taskMng.updateSingleTask += OnTaskProgress;
        actorInfo.OnAiUpdate += OnUpdateAI;
    }

    public override void UnRegist()
    {
        base.UnRegist();
        GameCenter.taskMng.OnTaskListUpdate -= OnTaskUpDate;
        GameCenter.taskMng.AddNewTask -= OnTaskUpDate;
        GameCenter.taskMng.CommitForNewTask -= OnTaskUpDate;
        GameCenter.taskMng.updateSingleTask -= OnTaskProgress;
        actorInfo.OnAiUpdate -= OnUpdateAI;
    }



    #region 任务相关 by吴江
    protected NPCTaskState curNPCTaskState = NPCTaskState.None;
    public NPCTaskState CurNPCTaskState
    {
        get
        {
            return curNPCTaskState;
        }
    }
    /// <summary>
    /// NPC身上的任务列表
    /// </summary>
    protected List<TaskInfo> taskList = null;
    /// <summary>
    /// 重新获取任务列表,返回任务状态
    /// </summary>
    /// <returns></returns>
    protected NPCTaskState ReGetTaskList()
    {
        NPCTaskState state = NPCTaskState.None;
        taskList = GameCenter.taskMng.GetNPCTaskList(actorInfo.Type);//修改
        if (taskList == null || taskList.Count == 0)
        {
            curNPCTaskState = state;
            return state;
        }
        foreach (var item in taskList)
        {
            switch (item.TaskState)
            {
                case TaskStateType.Process:
                    if (state == NPCTaskState.None) state = NPCTaskState.HasTake;
                    break;
                case TaskStateType.UnTake:
                    state = NPCTaskState.CanTake;
                    break;
                case TaskStateType.Finished:
                    state = NPCTaskState.CanCommit;
                    curNPCTaskState = state;
                    return state;
                default:
                    break;
            }
        }
        curNPCTaskState = state;
        return state;
    }

    /// <summary>
    /// 刷新任务状态
    /// </summary>
    public void OnTaskUpDate(TaskDataType _dataType, TaskType _taskType)
    {
        if (headTextCtrl != null)
        {
            headTextCtrl.SetTaskTag(ReGetTaskList());
        }
    }
    /// <summary>
    /// 刷新任务状态
    /// </summary>
    protected void OnTaskUpDate(TaskInfo _newTask)
    {
        if (_newTask == null) return;
        if (_newTask.TakeFromNPCID != actorInfo.Type && _newTask.CommitNPCID != actorInfo.Type) return;
        if (headTextCtrl != null) headTextCtrl.SetTaskTag(ReGetTaskList());
    }

    protected void OnTaskProgress(TaskInfo _task)
    {
        if (_task == null) return;
        if (_task.TakeFromNPCID != actorInfo.Type && _task.CommitNPCID != actorInfo.Type) return;
        if (_task.TaskState == TaskStateType.Process) return;
        if (headTextCtrl != null) headTextCtrl.SetTaskTag(ReGetTaskList());
    }
    #endregion


    #region AI相关 by吴江 
    public enum EventType
    {
        Idle = fsm.Event.USER_FIELD + 1,
        Follow,
        Cower,
        Backon,
        Arrive,
        Lead,
        Fight,
        Check,
        Avoid,
        Finished,
    }

    fsm.State follow;
    fsm.State led;
    fsm.State idle;
    fsm.State cower;
    fsm.State backon;
    fsm.State arrive;
    fsm.State check;
    fsm.State avoid;
    fsm.State finished;

    private int idleRange = 3;
    private int followRange = 10;
    private int leadRange = 6;
    private int beckonRange = 10;
   // private int cowerRange = 16;
   // private int fightRange = 20;
    private int arriveRange = 10;
    private int startAvoidRange = 5;
    private int avoidToRange = 7;
    private float beckonRandomTime = 10f;
    private float timer = 0;

    public System.Action<int> finishedCallback;

    protected NPCActionRef curNPCActionRef;

    protected Actor curTarget;

    protected override void InitStateMachine()
    {
        check = new fsm.State("check", stateMachine);
        check.onEnter += EnterCheckState;
        check.onExit += ExitCheckState;
        check.onAction += UpdateCheckState;

        idle = new fsm.State("idle1", stateMachine);
        idle.onEnter += EnterIdleState;
        idle.onExit += ExitIdleState;
        idle.onAction += UpdateIdleState;

        follow = new fsm.State("follow", stateMachine);
        follow.onEnter += EnterFollowState;
        follow.onExit += ExitFollowState;
        follow.onAction += UpdateFollowState;

        cower = new fsm.State("cower", stateMachine);
        cower.onEnter += EnterCowerState;
        cower.onExit += ExitCowerState;
        cower.onAction += UpdateCowerState;

        backon = new fsm.State("backon", stateMachine);
        backon.onEnter += EnterBackonState;
        backon.onExit += ExitBackonState;
        backon.onAction += UpdateBackonState;

        arrive = new fsm.State("arrive", stateMachine);
        arrive.onEnter += EnterArriveState;
        arrive.onExit += ExitArriveState;
        arrive.onAction += UpdateArriveState;

        avoid = new fsm.State("avoid", stateMachine);
        avoid.onEnter += EnterAvoidState;
        avoid.onExit += ExitAvoidState;
        avoid.onAction += UpdateAvoidState;

        led = new fsm.State("led", stateMachine);
        led.onEnter += EnterLedState;
        led.onExit += ExitLedState;
        led.onAction += UpdateLedState;

        finished = new fsm.State("finished", stateMachine);
        finished.onEnter += EnterFinishedState;


        check.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        check.Add<fsm.EventTransition>(finished, (int)EventType.Finished);


        idle.Add<fsm.EventTransition>(follow, (int)EventType.Follow);
        idle.Add<fsm.EventTransition>(cower, (int)EventType.Cower);
        idle.Add<fsm.EventTransition>(backon, (int)EventType.Backon);
        idle.Add<fsm.EventTransition>(arrive, (int)EventType.Arrive);
        idle.Add<fsm.EventTransition>(led, (int)EventType.Lead);
        idle.Add<fsm.EventTransition>(check, (int)EventType.Check);
        idle.Add<fsm.EventTransition>(avoid, (int)EventType.Avoid);

        follow.Add<fsm.EventTransition>(cower, (int)EventType.Cower);
        follow.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        follow.Add<fsm.EventTransition>(arrive, (int)EventType.Arrive);
        follow.Add<fsm.EventTransition>(check, (int)EventType.Check);
        follow.Add<fsm.EventTransition>(avoid, (int)EventType.Avoid);

        cower.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        cower.Add<fsm.EventTransition>(check, (int)EventType.Check);
        cower.Add<fsm.EventTransition>(avoid, (int)EventType.Avoid);

        backon.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        backon.Add<fsm.EventTransition>(check, (int)EventType.Check);
        backon.Add<fsm.EventTransition>(avoid, (int)EventType.Avoid);

        arrive.Add<fsm.EventTransition>(cower, (int)EventType.Cower);
        arrive.Add<fsm.EventTransition>(finished, (int)EventType.Finished);
        arrive.Add<fsm.EventTransition>(check, (int)EventType.Check);
        arrive.Add<fsm.EventTransition>(avoid, (int)EventType.Avoid);

        led.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        led.Add<fsm.EventTransition>(cower, (int)EventType.Cower);
        led.Add<fsm.EventTransition>(backon, (int)EventType.Backon);
        led.Add<fsm.EventTransition>(arrive, (int)EventType.Arrive);
        led.Add<fsm.EventTransition>(check, (int)EventType.Check);
        led.Add<fsm.EventTransition>(avoid, (int)EventType.Avoid);

        avoid.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        avoid.Add<fsm.EventTransition>(cower, (int)EventType.Cower);
        avoid.Add<fsm.EventTransition>(backon, (int)EventType.Backon);
        avoid.Add<fsm.EventTransition>(arrive, (int)EventType.Arrive);
        avoid.Add<fsm.EventTransition>(check, (int)EventType.Check);
        avoid.Add<fsm.EventTransition>(led, (int)EventType.Lead);

        stateMachine.onStop += OnFinish;
        stateMachine.initState = check;

    }


    public void StartAction(NPCActionRef _refData, System.Action<int> _callBack)
    {
        curNPCActionRef = _refData;
        if (_refData == null) return;

        Debug.logger.Log("_refData.targetType = " + _refData.targetType);
        switch (_refData.targetType)
        {
            case TargetType.NONE:
                curTarget = null;
                break;
            case TargetType.MAINPLAYER:
                curTarget = GameCenter.curMainPlayer;
                break;
            case TargetType.MONSTER:
                break;
            case TargetType.NPC:
                break;
            case TargetType.SCENEITEM:
                break;
            default:
                break;
        }

        finishedCallback = _callBack;
        stateMachine.logDebugInfo = false;

        if (headTextCtrl != null)
        {
            headTextCtrl.SetTaskTag(NPCTaskState.None);
        }
        curActionStep++;
        stateMachine.Send((int)EventType.Idle);
    }


    new void Update()
    {
        base.Update();
        stateMachine.Update();
    }

    void FaceToPlayer()
    {
        Vector3 dir = GameCenter.curMainPlayer.transform.position - transform.position;
        dir.y = 0;
        FaceTo(dir.normalized);
    }

    #region AI检索状态 by吴江
    /// <summary>
    /// 当前进行到的步骤
    /// </summary>
    protected int curActionStep = 0;
    /// <summary>
    /// 延迟行动的时间
    /// </summary>
    protected float delayActTime = 0;
    protected float startCountTime = 0;

    protected NPCActionRef curActionRef = null;
    void EnterCheckState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        startCountTime = Time.time;
        delayActTime = 0;
        StopMovingTo();
        if (animFSM != null)
        {
            animFSM.Idle();
        }
        if (!actorInfo.IsSmart)
        {
            stateMachine.Stop();
            return;
        }
        List<int> actionList = actorInfo.ActionList;
        if (actionList == null || actionList.Count == 0) return;
        switch (actorInfo.ActionModelType)
        {
            case ActionType.NONE:
                stateMachine.Send((int)EventType.Finished);
                break;
            case ActionType.ONCE:
                if (actionList.Count <= curActionStep)
                {
                    stateMachine.Send((int)EventType.Finished);
                }
                else
                {
                    curActionRef = ConfigMng.Instance.GetNPCActionRef(actionList[curActionStep]);
                    if (curActionRef != null)
                    {
                        StartAction(curActionRef, finishedCallback);
                    }
                }
                break;
            case ActionType.RANDOM:
                curActionStep = UnityEngine.Random.Range(0, actionList.Count - 1);
                curActionRef = ConfigMng.Instance.GetNPCActionRef(actionList[curActionStep]);
                if (curActionRef != null)
                {
                    StartAction(curActionRef, finishedCallback);
                }
                break;
            case ActionType.LOOP:
                if (actionList.Count <= curActionStep)
                {
                    curActionStep = 0;
                }
                curActionRef = ConfigMng.Instance.GetNPCActionRef(actionList[curActionStep]);
                if (curActionRef != null)
                {
                    StartAction(curActionRef, finishedCallback);
                }
                break;
            case ActionType.RANDOMINTERVAL:
                delayActTime = actorInfo.ActionDelayTime;
                curActionStep = UnityEngine.Random.Range(0, actionList.Count - 1);
                curActionRef = ConfigMng.Instance.GetNPCActionRef(actionList[curActionStep]);
                break;
            default:
                break;
        }
    }
    void ExitCheckState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
    }
    void UpdateCheckState(fsm.State _curState)
    {
        if (Time.time - startCountTime >= delayActTime)
        {
            if (curActionRef != null)
            {
                StartAction(curActionRef, finishedCallback);
            }
        }
    }
    #endregion

    #region 静止状态 by吴江
    void EnterIdleState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        GameSys.Log("NPC停下来思考");
        StopMovingTo();
        FaceToPlayer();
        if (animFSM != null)
        {
            animFSM.Idle();
        }
    }
    void ExitIdleState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        GameSys.Log("NPC结束思考");
    }
    void UpdateIdleState(fsm.State _curState)
    {
        if (curNPCActionRef == null) return;
        float lengthSqr = 0;
        if (curTarget == null)
        {
            lengthSqr = (transform.position - curNPCActionRef.targetPos).sqrMagnitude;
        }
        else
        {
            lengthSqr = (transform.position - curTarget.transform.position).sqrMagnitude;
        }

        if ((transform.position - curNPCActionRef.targetPos).sqrMagnitude < arriveRange * arriveRange)
        {
            stateMachine.Send((int)EventType.Arrive);
        }
        else if (lengthSqr <= idleRange * idleRange)
        {
            switch (curNPCActionRef.sort)
            {
                case TypeAction.MOVE:
                    stateMachine.Send((int)EventType.Arrive);
                    break;
                case TypeAction.LEAD:
                    stateMachine.Send((int)EventType.Lead);
                    break;
                case TypeAction.FOLLOW:
                    break;
                case TypeAction.AVOID:
                    stateMachine.Send((int)EventType.Avoid);
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (curNPCActionRef.sort)
            {
                case TypeAction.MOVE:
                    stateMachine.Send((int)EventType.Arrive);
                    break;
                case TypeAction.LEAD:
                    if (lengthSqr <= leadRange * leadRange)
                    {
                        stateMachine.Send((int)EventType.Lead);
                    }
                    else if (lengthSqr <= beckonRange * beckonRange)
                    {
                        stateMachine.Send((int)EventType.Backon);
                    }
                    break;
                case TypeAction.FOLLOW:
                    if (lengthSqr >= followRange * followRange) 
                    {
                        stateMachine.Send((int)EventType.Follow);
                    }
                    break;
                case TypeAction.AVOID:
                    stateMachine.Send((int)EventType.Avoid);
                    break;
                default:
                    break;
            }
        }
    }
    #endregion

    #region 跟随状态 by吴江
    int followSlot = 0;
    Vector3 lastPos = Vector3.zero;
    void EnterFollowState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        GameSys.Log("NPC开始跟随");
        if (moveFSM != null)
        {
            moveFSM.UnLockMoving();
        }
        int count = curTarget.positionSlots.Count;
        bool foundSlot = false;
        for (int i = 0; i < count; i++)
        {
            if (curTarget.positionSlots[i].occupyObj == null)
            {
                foundSlot = true;
                followSlot = i;
            }
        }
        if (!foundSlot)
        {
            followSlot = curTarget.AddPositionSlot();
        }
        curTarget.positionSlots[followSlot].occupyObj = this.gameObject;
        MoveTo(curTarget.positionSlots[followSlot].obj.transform.position);
    }
    void ExitFollowState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        GameSys.Log("NPC停止跟随");
        StopMovingTo();
    }
    void UpdateFollowState(fsm.State _curState)
    {
        float lengthSqr = (curNPCActionRef.targetPos - curTarget.positionSlots[followSlot].obj.transform.position).sqrMagnitude;
        if (lengthSqr < arriveRange * arriveRange)
        {
            stateMachine.Send((int)EventType.Arrive);
        }
        lengthSqr = (transform.position - curTarget.positionSlots[followSlot].obj.transform.position).sqrMagnitude;
        if (lengthSqr <= idleRange * idleRange)// || lengthSqr > followRange * followRange)
        {
            stateMachine.Send((int)EventType.Idle);
        }
        else
        {
            if (!IsMoving && lastPos != curTarget.positionSlots[followSlot].obj.transform.position)
            {
                lastPos = curTarget.positionSlots[followSlot].obj.transform.position;
                MoveTo(lastPos);
            }
        }
    }
    #endregion

    #region 害怕状态 by吴江
    void EnterCowerState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        GameSys.Log("NPC感到害怕");
        if (animFSM != null)
        {
            animFSM.Cower();
        }
        timer = 1;
    }
    void ExitCowerState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        GameSys.Log("NPC不再感到害怕");
        //结束动画
    }
    void UpdateCowerState(fsm.State _curState)
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            stateMachine.Send((int)EventType.Idle);
        }
    }
     #endregion

    #region 召唤状态 by吴江
    void EnterBackonState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        GameSys.Log("NPC进入招手状态");
        timer = 0;
        if(animFSM != null)
            animFSM.BeckOn();
    }
    void ExitBackonState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        GameSys.Log("NPC离开招手状态");
    }
    void UpdateBackonState(fsm.State _curState)
    {
        float lengthSqr = (transform.position - curTarget.transform.position).sqrMagnitude;
        if (lengthSqr > beckonRange * beckonRange || lengthSqr <= idleRange * idleRange)
        {
            stateMachine.Send((int)EventType.Idle);
        }
        FaceToPlayer();

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            if (beckonRandomTime < 2f)
            {
                beckonRandomTime = 2f;
            }
            timer = UnityEngine.Random.Range(2f, beckonRandomTime);
            if (animFSM != null)
                animFSM.BeckOn();
        }
    }
    #endregion

    #region 移动/到达状态 by吴江
    float checkTime = 0;
    void EnterArriveState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        MoveTo(curNPCActionRef.targetPos);
        checkTime = Time.time;
    }
    void ExitArriveState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        StopMovingTo();
        //任务完成
    }
    void UpdateArriveState(fsm.State _curState)
    {
        if (moveFSM.path == null || moveFSM.path.Length == 0 || (moveFSM.path != null && moveFSM.path.Length > 0 &&(transform.position - moveFSM.path[moveFSM.path.Length - 1]).sqrMagnitude < 1))
        {
            stateMachine.Send((int)EventType.Check);
        }
        else
        {
            //保障行为不会被意外中断,或者因为短距离寻径导致未完成，每0.5秒重新执行一次
            if (Time.time - checkTime > 0.5f)
            {
                checkTime = Time.time;
                MoveTo(curNPCActionRef.targetPos);
            }
        }
    }
    #endregion

    #region 带领状态 by吴江
    void EnterLedState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        GameSys.Log("开始带领玩家走向任务终点");
        MoveTo(curNPCActionRef.targetPos);
        checkTime = Time.time;
    }
    void ExitLedState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        GameSys.Log("停止带领");
        StopMovingTo();
    }
    void UpdateLedState(fsm.State _curState)
    {
        float lengthSqr = (curNPCActionRef.targetPos - transform.position).sqrMagnitude;
        if (lengthSqr < arriveRange * arriveRange && GameCenter.curGameStage.SceneID == curNPCActionRef.targetScene)
        {
            stateMachine.Send((int)EventType.Arrive);
        }
        lengthSqr = (transform.position - curTarget.transform.position).sqrMagnitude;
        if (lengthSqr > leadRange * leadRange)
        {
            stateMachine.Send((int)EventType.Idle);
        }
        else
        {
            //保障行为不会被意外中断,或者因为短距离寻径导致未完成，每0.5秒重新执行
            if (Time.time - checkTime > 0.5f)
            {
                checkTime = Time.time;
                MoveTo(curNPCActionRef.targetPos);
            }
        }
    }
    #endregion

    #region 躲避状态 by吴江
    void EnterAvoidState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
    }
    void ExitAvoidState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
    }
    void UpdateAvoidState(fsm.State _curState)
    {
        if (curTarget == null) stateMachine.Send((int)EventType.Check);
        float lengthSqr = (curTarget.transform.position - transform.position).sqrMagnitude;
        if (lengthSqr > avoidToRange * avoidToRange)
        {
            stateMachine.Send((int)EventType.Idle);
        }
        else
        {
            if (lengthSqr < startAvoidRange * startAvoidRange)
            {
                if (moveFSM != null && !moveFSM.isMoving)
                {
                    MoveTo(GetAvoidPos(curTarget.transform.position));
                }
            }
        }
    }

    /// <summary>
    /// 获取一个逃跑点
    /// </summary>
    /// <param name="_avoidTarPos"></param>
    protected Vector3 GetAvoidPos(Vector3 _avoidTarPos)
    {
        int dirY = UnityEngine.Random.Range(0, 360);
        Vector3 dir = new Vector3(Mathf.Sin(dirY * Mathf.Deg2Rad), 0.0f, Mathf.Cos(dirY * Mathf.Deg2Rad));
        Vector3 tarPost = _avoidTarPos + dir * 2 * startAvoidRange;
        Vector3[] path = GameStageUtility.StartPath(transform.position, tarPost);
        if (path != null)
        {
            return tarPost;
        }
        else
        {
            return GetAvoidPos(_avoidTarPos);
        }
    }
    #endregion

    #region 完成结束状态 by吴江
    void EnterFinishedState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        GameSys.Log("任务完成");
        stateMachine.Stop();

        StopMovingTo();
        if (animFSM != null)
        {
            animFSM.Idle();
        }
        if (finishedCallback != null)
        {
            finishedCallback(actorInfo.ServerInstanceID);
        }
    }
    #endregion

    /// <summary>
    /// 因为放弃任务而中断
    /// </summary>
    public void GiveUp()
    {
        stateMachine.Send((int)EventType.Idle);
        stateMachine.Stop();
    }

    void OnFinish()
    {
        curActionStep = 0;
    }
    #endregion


    public override void BubbleTalk(string _text, float _time)
    {
        nameHeight = actorInfo.NameHeight;
        base.BubbleTalk(_text, _time);
    }


    /// <summary>
    /// 移动到指定终点  by吴江
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_maxDistance"></param>
    /// <param name="_noStop"></param>
    public override void MoveTo(Vector3 _pos, float _maxDistance = 10.0f, bool _noStop = false)
    {
        if (moveFSM != null)
        {
            Vector3[] path = GameStageUtility.StartPath(transform.position, _pos);

            if (path == null) return;
            moveFSM.path = path;

            if (moveFSM.path == null)
            {
                GameSys.Log("NPC's path not found! Target: " + _pos);
                moveFSM.destPos = moveFSM.transform.position;
                moveFSM.StopMovingTo();
                return;
            }
            MoveTo(moveFSM.path);
        }
    }

    /// <summary>
    /// 被点击后做的反应 by吴江
    /// </summary>
    public void BeClick()
    {
        if (cachedAnimation && cachedAnimation["wait"])
        {
            cachedAnimation["wait"].wrapMode = WrapMode.Once;
            cachedAnimation.Play("wait");
            cachedAnimation.CrossFadeQueued("stand");
        }
//        if (headTextCtrl != null && actorInfo.ClickBubble != null)
//        {
//            headTextCtrl.SetBubble(actorInfo.ClickBubble.content,actorInfo.ClickBubble.time);
//        }
    }
    /// <summary>
    /// 取消冒泡说话 by吴江
    /// </summary>
    public void CancelBubble()
    {
        if (headTextCtrl != null)
        {
            headTextCtrl.ForceDisableBubble();
        }
    }

    public override void DoRingEffect(Color _color, bool _active, float _radius)
    {
        base.DoRingEffect(_color, _active, _radius);
        if (_active)
        {
            FaceToPlayer();
        }
        else
        {
            FaceTo(actorInfo.RotationY);
        }
    }

    public override void InteractionSound()
    {
        base.InteractionSound();
        GameCenter.soundMng.PlaySound(actorInfo.ActionIngSoundRes, 1.0f, false, true);
    }

    new void OnDestroy()
    {
        base.OnDestroy();
        FDictionary dic = GameCenter.sceneMng.NPCInfoDictionary;
        bool got = false;
        foreach (NPCInfo item in dic.Values)
        {
            if (item.Type == actorInfo.Type)
            {
                got = true;
                break;
            }
        }
        if (!got)//如果场景中已经没有同类怪了，那么卸载资源
        {
            AssetMng.instance.UnloadUrl(AssetMng.GetPathWithExtension(actorInfo.AssetName, actorInfo.AssetType));
        }
    }

}
