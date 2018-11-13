//==================================
//作者：吴江
//日期:2015/6/19
//用途：控制对象移动的状态机
//=================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 控制对象移动的状态机 by吴江
/// </summary>
public class ActorMoveFSM : FSMBase
{
    #region 数据
    /// <summary>
    /// 是否为虚拟体状态 by吴江
    /// </summary>
    protected bool isDummy_ = true;
    /// <summary>
    /// 是否为虚拟体状态 by吴江
    /// </summary>
    public bool IsDummy
    {
        get
        {
            return isDummy_;
        }
        set
        {
            if (isDummy_ != value)
            {
                isDummy_ = value;
                if (!isDummy_)
                {
                    this.transform.position = LineCast(this.transform.position, true);
                    if (path != null && path.Length > 0)
                    {
                        for (int i = 0; i < path.Length; i++)
                        {
                            path[i] = LineCast(path[i], true);
                        }
                    }
                }
            }
        }
    }

    public System.Action OnPositionChange;
    public System.Action OnMoveStart;
    public System.Action OnMoveEnd;
	/// <summary>
	/// 技能移动结束事件
	/// </summary>
	public System.Action<AbilityMoveData> OnAbilityMoveEnd;
    #endregion

    #region UNITY
    void Start()
    {
        stateMachine.Start();
    }

    protected void Update()
    {
        stateMachine.Update();
    }

    /// <summary>
    /// 等udpate中的事情做完，再做这里的
    /// </summary>
    protected void LateUpdate()
    {
        if (positionDirty)
        {
            if (OnPositionChange != null)
                OnPositionChange();
            positionDirty = false;
        }
        if (lastMoveState != isMoving && curEventType != EventType.GM_MOVE)
        {
            //			Debug.Log("isMoving = " + isMoving);
            if (isMoving)
            {
                if (OnMoveStart != null)
                {
					//Debug.Log("OnMoveStart");
                    OnMoveStart();
                }
            }
            else
            {
                if (OnMoveEnd != null)
                {
                    OnMoveEnd();
                }
                velocity = Vector3.zero;
            }
            lastMoveState = isMoving;
        }
    }
    #endregion

    #region 状态机
    /// <summary>
    /// 游戏状态枚举 by吴江
    /// </summary>
    public enum EventType
    {
        NORMALMOVE = fsm.Event.USER_FIELD + 1,
        PATHMOVE,
        KEYBOARDMOVE,
        ABILITYMOVE,
        GM_MOVE,
    }

    protected fsm.State normalMove;
    protected fsm.State pathMove;
    protected fsm.State keyboardMove;
    protected fsm.State abilityMove;
    protected fsm.State gameMasterMove;

    protected EventType curEventType = EventType.PATHMOVE;

    /// <summary>
    /// 初始化状态机 by吴江
    /// </summary>
    protected override void InitStateMachine()
    {
        normalMove = new fsm.State("normalMove", stateMachine);
        normalMove.onEnter += EnterNormalState;
        normalMove.onExit += ExitNormalState;
        normalMove.onAction += UpdateNormalState;

        pathMove = new fsm.State("pathMove", stateMachine);
        pathMove.onEnter += EnterPathMoveState;
        pathMove.onExit += ExitPathMoveState;
        pathMove.onAction += UpdatePathMoveState;


        keyboardMove = new fsm.State("keyboardMove", stateMachine);
        keyboardMove.onEnter += EnterKeyBoardMoveState;
        keyboardMove.onExit += ExitKeyBoardMoveState;
        keyboardMove.onAction += UpdateKeyBoardMoveState;


        abilityMove = new fsm.State("abilityMove", stateMachine);
        abilityMove.onEnter += EnterAbilityMoveState;
        abilityMove.onExit += ExitAbilityMoveState;
        abilityMove.onAction += UpdateAbilityMoveState;

        gameMasterMove = new fsm.State("gameMasterMove", stateMachine);
        gameMasterMove.onEnter += EnterGameMasterMoveState;
        gameMasterMove.onExit += ExitGameMasterMoveState;
        gameMasterMove.onAction += UpdateGameMasterMoveState;

        normalMove.Add<fsm.EventTransition>(pathMove, (int)EventType.PATHMOVE);
        normalMove.Add<fsm.EventTransition>(keyboardMove, (int)EventType.KEYBOARDMOVE);
        normalMove.Add<fsm.EventTransition>(abilityMove, (int)EventType.ABILITYMOVE);
        normalMove.Add<fsm.EventTransition>(gameMasterMove, (int)EventType.GM_MOVE);

        pathMove.Add<fsm.EventTransition>(normalMove, (int)EventType.NORMALMOVE);
        pathMove.Add<fsm.EventTransition>(keyboardMove, (int)EventType.KEYBOARDMOVE);
        pathMove.Add<fsm.EventTransition>(abilityMove, (int)EventType.ABILITYMOVE);
        pathMove.Add<fsm.EventTransition>(gameMasterMove, (int)EventType.GM_MOVE);

        keyboardMove.Add<fsm.EventTransition>(normalMove, (int)EventType.NORMALMOVE);
        keyboardMove.Add<fsm.EventTransition>(pathMove, (int)EventType.PATHMOVE);
        keyboardMove.Add<fsm.EventTransition>(abilityMove, (int)EventType.ABILITYMOVE);
        keyboardMove.Add<fsm.EventTransition>(gameMasterMove, (int)EventType.GM_MOVE);

        abilityMove.Add<fsm.EventTransition>(normalMove, (int)EventType.NORMALMOVE);
        abilityMove.Add<fsm.EventTransition>(pathMove, (int)EventType.PATHMOVE);
        abilityMove.Add<fsm.EventTransition>(gameMasterMove, (int)EventType.GM_MOVE);


        gameMasterMove.Add<fsm.EventTransition>(normalMove, (int)EventType.NORMALMOVE);
        gameMasterMove.Add<fsm.EventTransition>(keyboardMove, (int)EventType.KEYBOARDMOVE);
        gameMasterMove.Add<fsm.EventTransition>(abilityMove, (int)EventType.ABILITYMOVE);
        gameMasterMove.Add<fsm.EventTransition>(pathMove, (int)EventType.PATHMOVE);


        stateMachine.initState = pathMove;
    }


    #region 指定目标点移动
    protected virtual void EnterNormalState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        curEventType = EventType.NORMALMOVE;
    }

    protected virtual void ExitNormalState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {

    }

    protected virtual void UpdateNormalState(fsm.State _curState)
    {
        UpdatePointMove();
        UpdateRotation();
    }
    #endregion

    #region 指定路径移动
    protected virtual void EnterPathMoveState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
		//Debug.Log("EnterPathMoveState");
        curEventType = EventType.PATHMOVE;
    }

    protected virtual void ExitPathMoveState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {

    }

    protected virtual void UpdatePathMoveState(fsm.State _curState)
    {
        UpdatePathMove();
        UpdateRotation();
    }
    #endregion

    #region 键盘控制移动
    protected virtual void EnterKeyBoardMoveState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        curEventType = EventType.KEYBOARDMOVE;
        movingTowards = true;
    }

    protected virtual void ExitKeyBoardMoveState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (onStopMovingTowards != null)
            onStopMovingTowards();
        Stop();
        movingTowards = false;
    }

    protected virtual void UpdateKeyBoardMoveState(fsm.State _curState)
    {
        UpdateKeyboardMove();
        UpdateRotation();
    }
    #endregion

    #region 技能移动
    protected virtual void EnterAbilityMoveState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        curEventType = EventType.ABILITYMOVE;
    }

    protected virtual void ExitAbilityMoveState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {

    }

    protected virtual void UpdateAbilityMoveState(fsm.State _curState)
    {
        UpdateAbilityMove();
    }
    #endregion

    #region GM瞬间移动
    protected virtual void EnterGameMasterMoveState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        curEventType = EventType.GM_MOVE;
    }

    protected virtual void ExitGameMasterMoveState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
    }

    protected virtual void UpdateGameMasterMoveState(fsm.State _curState)
    {
        UpdateLerpMove();
        UpdateRotation();
    }
    #endregion
    #endregion

    #region 移动相关 by吴江
    /// <summary>
    /// 当前帧的移动矢量 by吴江
    /// </summary>
    protected Vector3 velocity = Vector3.zero;
    /// <summary>
    /// 当前帧的移动矢量 by吴江
    /// </summary>
    public Vector3 GetVelocity() { return velocity; }

    /// <summary>
    /// 最大移动速度 by吴江
    /// </summary>
    public virtual float curRealSpeed
    {
        get;
        set;
    }

    /// <summary>
    /// 单帧最大移动举例 by吴江
    /// </summary>
    public float maxMoveDistance = 1.0f;

    /// <summary>
    /// 是否已经被禁止移动
    /// </summary>
    public bool isMoveLocked { get { return lockMoving; } }

    public void ResetLastReportPosition() { lastReportPos = transform.position; }

    public void ResetLastReportPosition(Vector3 pos) { lastReportPos = pos; }


    /// <summary>
    /// 是否正在移动（包括移动位置和移动朝向）
    /// </summary>
    public bool isMoving { get { return movingTo || movingTowards; } }

    public bool isMovingTo
    {
        get
        {
            return movingTo;
        }
    }
    /// <summary>
    /// 是否正在移动朝向
    /// </summary>
    public bool isMovingTowards { get { return movingTowards; } }


    /// <summary>
    /// 是否禁止移动
    /// </summary>
    protected bool lockMoving = false;


    /// <summary>
    /// 是否正移动朝向
    /// </summary>
    protected bool movingTowards = false;

    /// <summary>
    /// 移动是否即将停止
    /// </summary>
    [HideInInspector]
    public bool movingWillStop = false;


    public float GetMoveSpeed() { return curRealSpeed; }


    /// <summary>
    /// 位置状态是否尚在本地修正中
    /// </summary>
    protected bool positionDirty = false;

    /// <summary>
    /// 上一帧的移动状态，用来修正动画
    /// </summary>
	protected bool lastMoveState = false;

	void OnDisable()
	{
		lastMoveState = false;
		if (OnMoveEnd != null)
		{
			OnMoveEnd();
		}
		velocity = Vector3.zero;
	}

    /// <summary>
    /// 上一次向服务端报告的位置
    /// </summary>
    protected Vector3 lastReportPos = Vector3.zero;

    /// <summary>
    /// 禁止移动 by吴江
    /// </summary>
    public virtual void LockMoving()
    {
        movingTo = false;
        movingTowards = false;
        Stop();
        lockMoving = true;
    }

    /// <summary>
    /// 取消禁止移动 by吴江
    /// </summary>
    public virtual void UnLockMoving()
    {
        lockMoving = false;
    }

    /// <summary>
    /// 执行移动 by吴江
    /// </summary>
    /// <param name="_nextStep"></param>
    protected virtual void ApplyMove(Vector3 _nextStep)
    {
        transform.position += _nextStep;
        transform.position = LineCast(transform.position, !IsDummy);
    }

    protected static RaycastHit hitInfo;


    public static Vector3 PathCast(Vector3 _pos, Vector3 _des)
    {
        if (Physics.Linecast(_pos, _des, out hitInfo, LayerMng.blockMask))
        {
            Vector3 diff = Vector3.zero;
            if ((_pos - _des).sqrMagnitude < 0.1f)
            {
                return _pos;
            }
            else
            {
                diff = (_pos - _des).normalized * 0.1f;
            }
            return hitInfo.point + diff;
        }
        return _des;
    }

    private static Vector3 LineCast(Vector3 _pos, float _startHeight, float _endHeight)
    {
        Vector3 start = new Vector3(_pos.x, _startHeight, _pos.z);
        Vector3 end = new Vector3(_pos.x, _endHeight, _pos.z);

        if (Physics.Linecast(start, end, out hitInfo, LayerMng.lineCastMask))
        {
            return hitInfo.point;
        }
        return _pos;
    }

    public static bool CheckBlockCast(Vector3 _pos, float _startHeight, float _endHeight)
    {
        Vector3 start = new Vector3(_pos.x, _startHeight, _pos.z);
        Vector3 end = new Vector3(_pos.x, _endHeight, _pos.z);
        if (Physics.Linecast(start, end, out hitInfo, LayerMng.sceneItemMask))
        {
            if (hitInfo.transform != null)
            {
                SceneItem sceneitem = hitInfo.transform.gameObject.GetComponent<SceneItem>();
                if (sceneitem != null && sceneitem.MySceneFunctionType == SceneFunctionType.BLOCK)
                {
                    return true;
                }
            }
            return false;
        }
        return false;
    }

    public static Vector3 LineCast(Vector3 _pos, bool _isNotDummy)
    {
        if (_isNotDummy)
        {
            return LineCast(_pos, _pos.y + 1000.0f, _pos.y - 1000.0f);
        }
        else
        {
            return _pos;
        }
    }

    public static Vector3 LineCast(Vector2 _pos, bool _isNotDummy)
    {
        if (_isNotDummy)
        {
            return LineCast(new Vector3(_pos.x, 0, _pos.y), 1000.0f, -1000.0f);
        }
        else
        {
            return _pos;
        }
    }

    public static Vector3 CameraLineCast(Vector3 _pos)
    {
        Vector3 start = new Vector3(_pos.x, _pos.y + 1000.0f, _pos.z);
        Vector3 end = new Vector3(_pos.x, _pos.y - 1000.0f, _pos.z);

        if (Physics.Linecast(start, end, out hitInfo, LayerMng.lineCastMask))
        {
            if (_pos.y <= hitInfo.point.y)
            {
                return new Vector3(hitInfo.point.x, hitInfo.point.y + 2.0f, hitInfo.point.z);
            }
            else
            {
                return _pos;
            }
        }
        return _pos;
    }
    /// <summary>
    /// 停止移动和停止转向  by吴江
    /// </summary>
    public void Stop()
    {
        if (movingTo == false && movingTowards == false)
        {
            if (needApplyStopDir)
            {
                needApplyStopDir = false;
                destDir = destDirOnStop;
            }
            if (onStopMoving != null)
                onStopMoving();
        }
    }



    #region 键盘移动
    protected float tickTime = 0.5f;
    protected float diffTickTime = 0.2f;
    /// <summary>
    /// 直接朝某个方向行走（一般用于键盘的asdw操作，手游暂时用不到） by吴江
    /// </summary>
    /// <param name="_dir"></param>
    public virtual void MoveTowards(Vector3 _dir)
    {
        if (curEventType != EventType.KEYBOARDMOVE)
        {
            stateMachine.Send((int)EventType.KEYBOARDMOVE);
        }
        movingTo = false;
        path = null;
        nextPathPoint = -1;
        if (lockMoving)
            return;

        if (_dir != Vector3.zero)
        {
            destPos = transform.position + _dir * curRealSpeed * tickTime;
            destDir = _dir;
            movingWillStop = true;
            //StopMovingTo();
        }
    }

    /// <summary>
    /// 停止方向移动（一般用于键盘的asdw操作，手游暂时用不到） by吴江
    /// </summary>
    public virtual void StopMovingTowards()
    {
        if (curEventType == EventType.KEYBOARDMOVE)
        {
            stateMachine.Send((int)EventType.PATHMOVE);
        }
    }

    /// <summary>
    /// 停止转向的事件 by吴江
    /// </summary>
    public System.Action onStopMovingTowards = null;

    protected void UpdateKeyboardMove()
    {
        Vector3 wantedVelocity = destDir * curRealSpeed;

        velocity = wantedVelocity;

        Vector3 nextStep = velocity * Time.deltaTime;

        Vector3 newPos = transform.position + nextStep;
        //Vector3 newPos = LineCast(transform.position.SetY(-500) + nextStep, !IsDummy);
        //if (newPos == transform.position) return;
        //if (newPos.y < -499)
        //{
        //    return;
        //}
        newPos = PathCast(transform.position, newPos);
        NavMeshHit navMeshHit = new NavMeshHit();
        bool pathFound = NavMesh.SamplePosition(newPos, out navMeshHit, 10.0f, NavMesh.AllAreas);
        if (pathFound)
        {
            if (!IsClosingEdge(navMeshHit.position))//判断是否靠近边界
            {
                transform.position = navMeshHit.position;//newPos为行走区域外面时,pathFound也为true,navMeshHit.position的位置为行走区域最边沿
            }
            positionDirty = true;
        }
        else
        {
            NavMeshPath resultPath = new NavMeshPath();
            pathFound = NavMesh.CalculatePath(transform.position, newPos, NavMesh.AllAreas, resultPath);
            if (pathFound)
            {
                if ((newPos - transform.position).sqrMagnitude > 0.8 * nextStep.sqrMagnitude)
                {
                    transform.position = newPos;
                }
                positionDirty = true;
            }
        }

    }
    /// <summary>
    /// 是否靠近边界 by邓成
    /// </summary>
    /// <param name="desPos"></param>
    /// <returns></returns>
    bool IsClosingEdge(Vector3 desPos)
    {
        NavMeshHit navMeshEdge = new NavMeshHit();
        bool edgeFound = NavMesh.FindClosestEdge(desPos, out navMeshEdge, NavMesh.AllAreas);
        if (edgeFound)
        {
            if ((desPos - navMeshEdge.position).sqrMagnitude < 0.05f)
                return true;
        }
        return false;
    }


    /// <summary>
    /// 是否已经计算过tan值
    /// </summary>
    protected static bool hasInitTan = false;
    /// <summary>
    /// 66.5度角的tan值
    /// </summary>
    protected static float tanBigValue = 0;
    /// <summary>
    /// 22.5度角的tan值
    /// </summary>
    protected static float tanSmallValue = 0;
    /// <summary>
    /// 将摇杆的相对位置转换为8方向之一
    /// </summary>
    /// <param name="_uiDir"></param>
    /// <returns></returns>
    public static Vector3 TranslateTo8Dir(Vector2 _uiDir)
    {
        if (!hasInitTan)
        {
            hasInitTan = true;
            tanBigValue = Mathf.Tan(Mathf.Deg2Rad * 67.5f);
            tanSmallValue = Mathf.Tan(Mathf.Deg2Rad * 22.5f);
        }
        float curTan = Mathf.Abs(_uiDir.y / _uiDir.x);

        float tempx = 0;
        if (_uiDir.x != 0)
        {
            tempx = _uiDir.x > 0 ? 1 : -1;
        }
        float tempy = 0;
        if (_uiDir.y != 0)
        {
            tempy = _uiDir.y > 0 ? 1 : -1;
        }

        if (curTan >= tanBigValue)
        {
            return new Vector3(0, 0, tempy);
        }
        else if (curTan <= tanSmallValue)
        {
            return new Vector3(tempx, 0, 0);
        }
        else
        {
            return new Vector3(tempx, 0, tempy);
        }
    }


    #endregion

    #region GM限时瞬间移动
    [HideInInspector]
    public bool lerpWhenStop = false;

    protected float lerpToDestDuration = 0.4f;

    protected float lerpTimer = 0.0f;

    protected Vector3 lerpStart = Vector3.zero;

    protected Vector3 lerpDest = Vector3.zero;

    /// <summary>
    /// 插值移动结束的事件 by吴江
    /// </summary>
    public System.Action onLerpFinished = null;

    /// <summary>
    /// 瞬间移动至某点 by吴江
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_noStop"></param>
    public void MoveToLerpOnly(Vector3 _pos, bool _noStop = false)
    {
        if (lockMoving)
            return;

        destPos = _pos;

        Vector3 delta = destPos - transform.position;
        delta.y = 0.0f;
        destDir = delta;
        destDir.Normalize();

        movingTo = true;
        movingWillStop = (_noStop == false);

        FaceTo(destDirOnStop);
        LerpToDest(destPos, 0.2f);

        if (_noStop == false)
        {
            lerpWhenStop = true;
        }
    }

    /// <summary>
    /// 指定时间直接移动至某点 by吴江
    /// </summary>
    /// <param name="_destPos"></param>
    /// <param name="_duration"></param>
    public void LerpToDest(Vector3 _destPos, float _duration, bool _noStop = false)
    {
        stateMachine.Send((int)EventType.GM_MOVE);
        lerpStart = transform.position;
        lerpDest = _destPos;
        lerpTimer = 0.0f;
        destPos = _destPos;
        path = null;
        nextPathPoint = -1;
        lerpToDestDuration = _duration;
        FaceToNoLerp(_destPos - this.transform.position);
        if (!_noStop)
        {
            lerpWhenStop = true;
        }
    }

    void UpdateLerpMove()
    {
        lerpTimer += Time.deltaTime;
        if (lerpTimer <= lerpToDestDuration)
        {
            float ratio = lerpTimer / lerpToDestDuration;
            Vector3 pos = Vector3.Lerp(lerpStart, lerpDest, ratio);
            transform.position = pos;
        }
        else
        {
            transform.position = lerpDest;
            if (lerpWhenStop)
            {
                lerpWhenStop = false;
                StopMovingTo();
            }
            if (onLerpFinished != null)
            {
                onLerpFinished();
            }
            stateMachine.Send((int)EventType.PATHMOVE);
        }
        ApplyMove(Vector3.zero);
        positionDirty = true;
    }
    #endregion

    #region 指定目标点移动
    /// <summary>
    /// 停止移动的事件(包括停止移动位置和停止转向) by吴江
    /// </summary>
    public System.Action onStopMoving = null;

    /// <summary>
    /// 停止移动位置的事件 by吴江
    /// </summary>
    public System.Action onStopMovingTo = null;

    /// <summary>
    /// 是否正在移动坐标
    /// </summary>
    [HideInInspector]
    public bool movingTo = false;

    /// <summary>
    /// 坐标移动的目标坐标
    /// </summary>
    [HideInInspector]
    public Vector3 destPos = Vector3.zero;
    /// <summary>
    /// 移动到指定终点  by吴江
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_maxDistance"></param>
    /// <param name="_noStop"></param>
    public virtual void MoveTo(Vector3 _pos, float _maxDistance = 10.0f, bool _noStop = false)
    {
        if (lockMoving)
            return;

        destPos = _pos;


        Vector3 delta = destPos - transform.position;
        delta.y = 0.0f;
        destDir = delta;
        destDir.Normalize();

        movingTo = true;
        movingWillStop = (_noStop == false);

        if (delta.sqrMagnitude <= (0.1f * curRealSpeed) * (0.1f * curRealSpeed))
        {
            FaceTo(destDirOnStop);
            LerpToDest(destPos, lerpToDestDuration);
        }



        if (_noStop == false)
        {
            lerpWhenStop = true;
        }
        stateMachine.Send((int)EventType.NORMALMOVE);
    }

    /// <summary>
    /// 停止移动 by吴江
    /// </summary>
    public virtual void StopMovingTo()
    {
        if (movingTo)
        {
            movingTo = false;
            path = null;
            nextPathPoint = -1;

            if (onStopMovingTo != null)
                onStopMovingTo();
            Stop();
        }
    }

    void UpdatePointMove()
    {
        if (!movingTo) return;
        bool reachDest = false;
        Vector3 wantedVelocity = destDir * curRealSpeed;
        Vector3 distance = Vector3.zero;

        if (movingWillStop)
        {
            distance = destPos - transform.position;
            distance.y = 0.0f;
            float sqrMagnitude = distance.sqrMagnitude;
            if (sqrMagnitude <= maxMoveDistance * maxMoveDistance)
            {
                if (sqrMagnitude <= 0.5f * 0.5f)
                {
                    reachDest = true;
                }
            }
        }
        velocity = wantedVelocity;

        Vector3 nextStep = velocity * Time.deltaTime;
        Vector3 nextPos = transform.position + nextStep;

        if (reachDest == false)
        {
            Vector3 dir2 = destPos - nextPos;
            dir2.y = 0.0f;
            dir2.Normalize();
            if (Vector3.Angle(destDir, dir2) >= 90.0f)
            {
                reachDest = true;
            }
        }

        if (reachDest)
        {
            if (movingWillStop)
            {
                if (lerpWhenStop)
                {
                    LerpToDest(destPos, distance.magnitude / Mathf.Max(curRealSpeed, 1.0f));
                }
                else
                {
                    StopMovingTo();
                }

                ApplyMove(Vector3.zero);
                positionDirty = true;
            }
        }
        else
        {
            ApplyMove(nextStep);
            positionDirty = true;
        }
    }

    #endregion

    #region 路径移动


    /// <summary>
    /// 根据指定路径移动 by吴江
    /// </summary>
    /// <param name="_path"></param>
    public virtual void MoveTo(Vector3[] _path, float _rotY = 0, bool _needApplyStopDir = false)
    {
        path = _path;

        if (path == null)
        {
            destPos = transform.position;
            StopMovingTo();
            return;
        }
        nextPathPoint = 0;

        if (path.Length > nextPathPoint)
        {
            destPos = path[nextPathPoint];
        }
        else
        {
            destPos = transform.position;
        }


        destDir = destPos - transform.position;
        destDir.y = 0.0f;
        destDir.Normalize();



        movingTo = true;
        movingWillStop = true;

        needApplyStopDir = _needApplyStopDir;
        destDirOnStop = LineCast(new Vector3(Mathf.Sin(_rotY * Mathf.Deg2Rad), 0.0f, Mathf.Cos(_rotY * Mathf.Deg2Rad)), true);
        stateMachine.Send((int)EventType.PATHMOVE);
    }

    //移动（寻径）
    /// <summary>
    /// 路径
    /// </summary>
    [HideInInspector]
    public Vector3[] path = null;

    /// <summary>
    /// 当前执行路径的序号
    /// </summary>
    [HideInInspector]
    public int nextPathPoint = -1;

    public Vector3[] GetPath() { return path; }

    /// <summary>
    /// 获取移动路径的下一步 by吴江
    /// </summary>
    /// <returns></returns>
    public Vector3 GetNextPathPoint()
    {
        if (path.Length > 1 && (transform.position - path[0]).sqrMagnitude < 0.5f)
        {
            //避免返回错误的index by吴江
            return path[1];
        }
        return path[nextPathPoint];
    }

    /// <summary>
    /// 获取移动路径的下下步 by吴江
    /// </summary>
    /// <returns></returns>
    public Vector3 GetNextNextPathPoint()
    {
        if (path.Length > 2 && (transform.position - path[0]).sqrMagnitude < 0.5f)
        {
            //避免返回错误的index
            return path[2];
        }
        if (nextPathPoint >= path.Length - 1)
        {
            return Vector3.zero;
        }
        return path[nextPathPoint + 1];
    }


    void UpdatePathMove()
    {
        if (lockMoving) return;
        if (!movingTo || path == null) return;
        //根据当前是否最后一步，来决定是否要停下来
        if (nextPathPoint >= path.Length - 1)
        {
            movingWillStop = true;
        }
        else
        {
            movingWillStop = false;
        }
        //取得距离矢量
        Vector3 distance = destPos - transform.position;

        //根据举例矢量得到朝向和该朝向下当前的移动距离
        Vector3 wantedDir = distance.normalized;
        Vector3 wantedVelocity = wantedDir * curRealSpeed;
        //将目标朝向设置为当前的移动矢量朝向（在修正朝向的update中将会引用该值）
        destDir = wantedDir;
        destDir.y = 0.0f;
        //如果距离过近，视为已经到达
        distance = distance.SetY(0);
        bool reachDest = distance.sqrMagnitude <= (0.05f * 0.05f);

        velocity = wantedVelocity;
        Vector3 nextStep = velocity * Time.deltaTime;
        if (distance.sqrMagnitude < nextStep.sqrMagnitude)
        {
            nextStep = distance;
        }
        Vector3 nextPos = transform.position + nextStep;
        if (movingWillStop)
        {
            Vector3 dir2 = destPos - nextPos;
            dir2.y = 0.0f;
            dir2.Normalize();

            if (Vector3.Angle(destDir, dir2) >= 90.0f)
            {
                reachDest = true;
            }
        }
        if (reachDest)
        {
            if (movingWillStop)
            {
                transform.position = destPos;//如果直接ApplyMove(Vector3.zero)，容易造成MoveTo一直到达不到终点，使玩家(挂机)行为卡住。
                StopMovingTo();
                positionDirty = true;
            }
            else
            {
                ApplyMove(nextStep);
                positionDirty = true;

                nextPathPoint += 1;
                destPos = path[nextPathPoint];
                destDir = destPos - transform.position;
                destDir.y = 0.0f;
                destDir.Normalize();
            }
        }
        else
        {
            ApplyMove(nextStep);
            positionDirty = true;
        }
    }
    #endregion

    #region 技能移动

    protected List<AbilityMoveData> waitForActMoveAbilityList = new List<AbilityMoveData>();
    protected AbilityMoveData curAbilityMoveData = null;

    ///// <summary>
    ///// 服务端通知使用技能 by吴江
    ///// </summary>
    ///// <param name="_info"></param>
    //public void UseAbility(AbilityInfo _info)
    //{
    //    if (_info == null) return;
    //    StopMovingTo();
    //    if (_info.MoveType != AbilityType.Normal)
    //    {
    //        waitForActMoveAbilityList.Add(new AbilityMoveData(_info, this.gameObject));
    //        stateMachine.Send((int)EventType.ABILITYMOVE);
    //    }
    //}

    /// <summary>
    /// 客户端通知使用技能 by吴江
    /// </summary>
    /// <param name="_info"></param>
    public void UseAbility(AbilityInstance _instance)
    {
        if (!_instance.HasServerConfirm || (_instance.endPosX == 0 && _instance.endPosZ == 0)) return;
        if (_instance.PerformanceRef.selfShiftType != SelfShiftType.NO)
        {
            waitForActMoveAbilityList.Add(new AbilityMoveData(_instance, this.gameObject));
            stateMachine.Send((int)EventType.ABILITYMOVE);
        }
    }

    /// <summary>
    /// 客户端通知使用技能 by吴江
    /// </summary>
    /// <param name="_info"></param>
    public void UseAbility(AbilityResultInfo _instance)
    {
        if (_instance.NeedMove)
        {
            waitForActMoveAbilityList.Add(new AbilityMoveData(_instance, this.gameObject));
            stateMachine.Send((int)EventType.ABILITYMOVE);
        }
    }

    ///// <summary>
    ///// 服务端通知被技能影响 by吴江
    ///// </summary>
    ///// <param name="_info"></param>
    //public void UseAbility(AbilityResultInfo.AbilityInfluenceInfo _info)
    //{
    //    if (_info.abilityType != AbilityType.Normal)
    //    {
    //        StopMovingTo();

    //        waitForActMoveAbilityList.Add(new AbilityMoveData(_info, this.gameObject));
    //        stateMachine.Send((int)EventType.ABILITYMOVE);

    //        if (_info.abilityType == AbilityType.STOP)
    //        {
    //            MYPOint clientCurPos = MYPOint.local2worldV3(transform.position);
    //            float distance = Mathf.Max(Mathf.Abs(clientCurPos.x - _info.destPos.x / 1000.0f), Mathf.Abs(clientCurPos.y - _info.destPos.y / 1000.0f));
    //            if (distance > 1.0f)
    //            {
    //                transform.position = _info.destPos;
    //                destPos = transform.position;
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// 取消技能施放 by吴江
    /// </summary>
    /// <param name="_needNoticeServer"></param>
    public void CancelAbility(bool _isPathMove = true)
    {
        waitForActMoveAbilityList.Clear();
        curAbilityMoveData = null;
        if (_isPathMove)
        {
            stateMachine.Send((int)EventType.PATHMOVE);
        }
        else
        {
            stateMachine.Send((int)EventType.KEYBOARDMOVE);
        }
    }

    void UpdateAbilityMove()
    {
        if (waitForActMoveAbilityList.Count > 0)
        {
            if (curAbilityMoveData == null)
            {
                curAbilityMoveData = waitForActMoveAbilityList[0];
                if (!curAbilityMoveData.isForce && lockMoving) return;
                destPos = curAbilityMoveData.AbilityDestPos;
                Vector3 rotation = curAbilityMoveData.faceToPos - this.transform.position;
                FaceToNoLerp(new Vector3(rotation.x, 0, rotation.z));

            }
            if (!curAbilityMoveData.isForce && lockMoving || !curAbilityMoveData.CanStartMove) return;
            curAbilityMoveData.InitBeforeUse();
            float curRushTime = Time.time - curAbilityMoveData.abilityStartTime;
            //Debug.LogWarning("当前帧技能移动开始: 移动前坐标 = " + transform.position + " , 目标点: = " + curAbilityMoveData.AbilityDestPos);
            if (curAbilityMoveData != null && (transform.position - curAbilityMoveData.AbilityDestPos).sqrMagnitude > 0.01f * 0.01f)//一定要以距离判断是否到位，而不能以时间判断。否则在帧率低的机器上极容易出现坐标错位导致很多bug  by吴江
            {
                if (curRushTime >= 0)
                {
                    float rate = 1.0f;
                    if (curAbilityMoveData.rushHoldTime > 0)
                    {
                        rate = Mathf.Min(1.0f, curRushTime / curAbilityMoveData.rushHoldTime);
                    }
                    transform.position = LineCast(Vector3.Lerp(curAbilityMoveData.startPos, curAbilityMoveData.AbilityDestPos, rate), IsDummy);
                    Vector3 rotation = curAbilityMoveData.faceToPos - this.transform.position;
                    FaceTo(new Vector3(rotation.x, 0, rotation.z));
                    positionDirty = true;
                }
            }
            else if (curRushTime >= curAbilityMoveData.rushHoldTime)
            {
				if(OnAbilityMoveEnd != null)
					OnAbilityMoveEnd(curAbilityMoveData);
                waitForActMoveAbilityList.RemoveAt(0);
                curAbilityMoveData = null;
            }
        }
        if (waitForActMoveAbilityList.Count <= 0)
        {
            StopMovingTo();
            stateMachine.Send((int)EventType.PATHMOVE);
        }
        //Debug.LogWarning("当前帧技能移动结束: 移动后坐标 = " + transform.position);
    }
    #endregion
    #endregion

    #region 朝向相关 by吴江
    protected bool fixRotation = false;
    /// <summary>
    /// 移动结束时的目标朝向
    /// </summary>
    protected Vector3 destDirOnStop = Vector3.zero;

    /// <summary>
    /// 是否需要在移动结束时修正朝向
    /// </summary>
    protected bool needApplyStopDir = false;

    /// <summary>
    /// 朝向移动的目标朝向
    /// </summary>
    [HideInInspector]
    public Vector3 destDir = new Vector3(0.0f, 0.0f, 1.0f);

    /// <summary>
    /// 每帧执行维护朝向相关逻辑 by吴江
    /// </summary>
    protected void UpdateRotation()
    {
        if (!fixRotation && destDir != Vector3.zero)
        {
            Quaternion quat = Quaternion.identity;
            quat.SetLookRotation(destDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, quat, 10.0f * Time.deltaTime);
        }
    }



    /// <summary>
    /// 移动停止时自动朝向某个方向  by吴江
    /// </summary>
    /// <param name="_dir"></param>
    public void FaceToOnStop(Vector3 _dir)
    {
        destDirOnStop = _dir;
        needApplyStopDir = true;
    }

    /// <summary>
    /// 朝向某个方向，直接赋Y值的重载 by吴江
    /// </summary>
    /// <param name="_eulerAngleY"></param>
    public void FaceTo(float _eulerAngleY)
    {
        Vector3 dir = new Vector3(Mathf.Sin(_eulerAngleY * Mathf.Deg2Rad), 0.0f, Mathf.Cos(_eulerAngleY * Mathf.Deg2Rad));
        FaceTo(dir);
    }

    /// <summary>
    /// 看向某个点 by吴江
    /// </summary>
    /// <param name="_dir"></param>
    public void FaceTo(Vector3 _dir)
    {
        destDir = _dir;
    }

    /// <summary>
    /// 直接看向某个方向，直接赋y值并且不经过时间插值的重载 by吴江
    /// </summary>
    /// <param name="_eulerAngleY"></param>
    public void FaceToNoLerp(float _eulerAngleY)
    {
        Vector3 dir = new Vector3(Mathf.Sin(_eulerAngleY * Mathf.Deg2Rad), 0.0f, Mathf.Cos(_eulerAngleY * Mathf.Deg2Rad));
        FaceToNoLerp(dir);
    }

    /// <summary>
    /// 直接看向某个点，不经过时间插值的重载 by吴江
    /// </summary>
    /// <param name="_dir"></param>
    public void FaceToNoLerp(Vector3 _dir)
    {
        destDir = _dir;
        if (destDir == Vector3.zero)
        {
            destDir = new Vector3(1.0f, 0.0f, 1.0f);
            destDir.Normalize();
        }

        Quaternion quat = Quaternion.identity;
        quat.SetLookRotation(destDir);
        transform.rotation = quat;
    }
    #endregion







}



public class AbilityMoveData
{
    public bool isForce = false;
    public GameObject user;
    public GameObject target;
    public SelfShiftType moveType = SelfShiftType.NO;
    public float abilityStartTime = 0;
    public float rushHoldTime = 0;
    public Vector3 direction;
    public float rushValue = 0;
    /// <summary>
    /// 当前的朝向
    /// </summary>
    public float DirY
    {
        get
        {
            Vector3 dir = AbilityDestPos - user.transform.position;
            Quaternion quat = Quaternion.identity;
            quat.SetLookRotation(dir);
            return (int)quat.eulerAngles.y;
        }
    }
    public Vector3 fromActorPos = Vector3.zero;
    protected Vector3 abilityDestPos = Vector3.zero;
    public Vector3 AbilityDestPos
    {
        set
        {
            //Debug.LogWarning("设置目标点: abilityDestPos = " + value);
            abilityDestPos = value;
        }
        get
        {
            return abilityDestPos;
        }
    }

    public Vector3 faceToPos;

    public Vector3 startPos = Vector3.zero;
    protected float prepareDuration = 0;
    protected float startCaculatTime = 0;
    protected bool hasInit = false;
    protected AbilityInstance dataInfo = null;

	public AbilityInstance DataInfo
	{
		get
		{
			return dataInfo;
		}
	}

    /// <summary>
    /// 是否已经经过服务端确认  by吴江
    /// </summary>
    public bool CanStartMove
    {
        get
        {
            if (dataInfo != null)
            {
                if (Time.time - startCaculatTime >= prepareDuration)
                {
                    return dataInfo.HasServerConfirm;
                }
            }
            return true;
        }
    }

    /// <summary>
    /// 技能正式开始时的计算 by吴江
    /// </summary>
    public void InitBeforeUse()
    {
        if (!hasInit)
        {
            abilityStartTime = Time.time;
            //InitDestination();
            //InitDirection();
            startPos = user.transform.localPosition;
            hasInit = true;
        }
    }

    /// <summary>
    /// 初始化目标点(客户端自己判断的，不妥，改为统一后台通知，以保持一致性) by吴江
    /// </summary>
    protected void InitDestination()
    {
        //switch (moveType)
        //{
        //    case SelfShiftType.BLINK:
        //        AbilityDestPos = user.transform.position + direction * rushValue;
        //        break;
        //    case SelfShiftType.PATH:
        //        if (target == null)
        //        {
        //            AbilityDestPos = user.transform.position + direction * rushValue;
        //        }
        //        else
        //        {
        //            Vector3 dir = (target.transform.position - user.transform.position).normalized;
        //            AbilityDestPos = target.transform.position + dir * dataInfo.AimOffset;
        //        }
        //        break;
        //    case SelfShiftType.DIRECTION:
        //        if (target == null)
        //        {
        //            AbilityDestPos = user.transform.position + direction * rushValue;
        //        }
        //        else
        //        {
        //            Vector3 dir = (target.transform.position - user.transform.position).normalized;
        //            AbilityDestPos = target.transform.position + dir * dataInfo.AimOffset;
        //            if (((target.transform.position + dir * dataInfo.AimOffset) - user.transform.position).sqrMagnitude > (dir * rushValue).sqrMagnitude)
        //            {
        //                AbilityDestPos = user.transform.position + dir * rushValue;
        //            }
        //        }
        //        break;
        //    case SelfShiftType.NO:
        //        AbilityDestPos = user.transform.localPosition;
        //        startPos = AbilityDestPos;
        //        break;
        //    default:
        //        break;
        //}
        //startPos = user.transform.localPosition;
    }
    /// <summary>
    /// 初始化目标朝向 (客户端自己判断的，不妥，改为统一后台通知，以保持一致性) by吴江
    /// </summary>
    protected void InitDirection()
    {
        switch (moveType)
        {
            case SelfShiftType.BLINK:
                faceToPos = user.transform.position + direction * (rushValue + 1);
                break;
            case SelfShiftType.PATH:
                if (target == null)
                {
                    faceToPos = user.transform.position + direction * (rushValue + 1);
                }
                else
                {
                    faceToPos = target.transform.position;
                }
                break;
            case SelfShiftType.DIRECTION:
                if (target == null)
                {
                    faceToPos = user.transform.position + direction * (rushValue + 1);
                }
                else
                {
                    faceToPos = target.transform.position;
                }
                break;
            case SelfShiftType.NO:
                faceToPos = user.transform.position + direction * (rushValue + 1);
                break;
            default:
                break;
        }
    }


    public AbilityMoveData(AbilityInstance _data, GameObject _user)
    {
        dataInfo = _data;
        if (dataInfo.NeedPrepare)
        {
            startCaculatTime = Time.time;
            prepareDuration = dataInfo.PrepareDuration;
        }
        moveType = _data.PerformanceRef.selfShiftType;
        user = _user;
        if (_data.TargetActor != null)
        {
            target = _data.TargetActor.gameObject;
        }
        AbilityDestPos = ActorMoveFSM.LineCast(new Vector3(_data.endPosX, _data.endPosY, _data.endPosZ), true);
        direction = Quaternion.Euler(0.0f, _data.DirY, 0.0f) * new Vector3(0, 0, 1);
        rushValue = _data.RushValue;
        rushHoldTime = _data.RushHoldTime;
        InitDirection();
    }


    public AbilityMoveData(AbilityResultInfo _data, GameObject _user)
    {
        moveType = _data.NeedMove ? SelfShiftType.DIRECTION : SelfShiftType.NO;
        isForce = true;
        user = _user;
        AbilityDestPos = ActorMoveFSM.LineCast(_data.curDestPos, true);
        direction = _data.Dir;
        rushHoldTime = _data.curMoveTime;
        if (_data.target != null)
        {
            target = _data.target.gameObject;
        }
        faceToPos = _data.UserActor.transform.position;
    }

    //public AbilityMoveData(AbilityInfo _data, GameObject _user)
    //{
    //    isForce = true;
    //    abilityType = _data.MoveType;
    //    user = _user;
    //    dirY = _data.RotationY;
    //    rushValue = _data.RushValue;
    //    rushStartTime = _data.RushStartTime;
    //    rushHoldTime = _data.RushHoldTime;
    //    needCaculatDest = true;
    //}


    //public AbilityMoveData(AbilityResultInfo.AbilityInfluenceInfo _data, GameObject _user)
    //{
    //    isForce = true;
    //    abilityType = _data.abilityType;
    //    user = _user;
    //    needCaculatDest = false;
    //    dirY = -1;
    //    if (abilityType == AbilityType.STOP)
    //    {
    //        fromActorPos = _data.FromActor.transform.position;
    //    }
    //    rushStartTime = 0;
    //    rushHoldTime = _data.rushTotalTime / 1000f;
    //    AbilityDestPos = _data.destPos;
    //}

}