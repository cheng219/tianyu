///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/5/13
//用途：生物动画状态机
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// 生物动画状态机
/// </summary>
public class SmartActorAnimFSM : ActorAnimFSM
{
    #region  数据
    protected string curDurativeName = string.Empty;
    protected string curCollectAnimName = string.Empty;
    protected float abilityTimer = 0.0f;
    // protected float abilityDuration = 0.0f;
    public string lastAbilityName = string.Empty;
    protected string curAbilityAnimName = "";
    protected string curAbilityAnimNameClone = "";
    protected int curAbilityID = -1;
    protected EventType curEventType = EventType.Idle;
    public EventType CurEventType
    {
        get
        {
            return curEventType;
        }
        protected set
        {
            curEventType = value;
        }
    }
    /// <summary>
    /// 当前正在施放的技能，如果为-1则代表当前没有释放
    /// </summary>
    public int CurAbilityID
    {
        get
        {
            return curAbilityID;
        }
    }
    protected bool needBlendToStop = false;
    protected float blendToStopDuration = 1.0f;
    protected bool isCasting = false;
    protected bool firstAnim = false;
    /// <summary>
    /// 是否正在移动中
    /// </summary>
    protected bool isMoving = false;
    /// <summary>
    /// 是否正在移动中
    /// </summary>
    public bool IsMoving
    {
        get
        {
            return isMoving;
        }
    }
    /// <summary>
    /// 是否僵直状态中
    /// </summary>
    public bool IsRigiditing
    {
        get
        {
            float duration = Time.time - castTime;
            return duration <= rigidityTime;
        }
    }
    /// <summary>
    /// 持续动作时间
    /// </summary>
    protected float durativeTime = -1;
    /// <summary>
    /// 持续动作开始时间
    /// </summary>
    protected float durativeStartTime = 0;
    /// <summary>
    /// 动作保护时间内 by吴江
    /// </summary>
    public bool IsProtecting
    {
        get
        {
            if (durativeTime > 0 && Time.time - durativeStartTime < durativeTime)
            {
                return true;
            }
            float duration = Time.time - normalCastTime;
            return duration <= normalAttakProtectTime;
        }
    }
    public void DebugProtecting()
    {
        if (IsProtecting)
        {
            Debug.logger.Log("durativeTime = " + durativeTime + " , durativeStartTime = " + durativeStartTime + " , Time.time - durativeStartTime = " + (Time.time - durativeStartTime));
            Debug.logger.Log("duration = " + duration + " , normalCastTime = " + normalCastTime + " , normalAttakProtectTime = " + normalAttakProtectTime);
        }
    }
    /// <summary>
    /// 技能(普通攻击)开始施放时间
    /// </summary>
    protected float normalCastTime = 0;
    /// <summary>
    /// 技能开始施放时间
    /// </summary>
    protected float castTime = 0;
    /// <summary>
    /// 僵直时间
    /// </summary>
    protected float rigidityTime = 0;
    /// <summary>
    /// 僵直开始时间
    /// </summary>
    protected float rigidityStartTime = 0;
    /// <summary>
    /// 普通攻击的保护时间长度,比硬直要长,比动画时间要略短  by吴江
    /// </summary>
    protected float normalAttakProtectTime = 0;
    public bool IsCasting
    {
        get
        {
            return isCasting;
        }
    }

    public FXCtrl fxCtrlRef = null;
    public HeadTextCtrl headTextCtrlRef = null;
    public ActorMoveFSM moveFSMRef = null;
    #endregion 

    #region 外部调用接口
    #region 设置
    /// <summary>
    /// 设置稍息动作名称
    /// </summary>
    /// <param name="_randIdleName"></param>
    public void SetRandIdleAnimationName(string _name, AnimationClip _animClip, bool _replace = false)
    {
        AnimationState state = GetAnimationState(_name, _animClip, _replace);
        if (state)
        {
            state.layer = 0;
            state.wrapMode = WrapMode.Loop;
        }
        curRandIdleName = _name;
        hasRamdomIdle = null != anim.GetClip(curRandIdleName);
    }
    /// <summary>
    /// 设置持续动作名称
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_animClip"></param>
    /// <param name="_replace"></param>
    public void SetDurativeAnimationName(string _name, AnimationClip _animClip, bool _replace = false)
    {
        AnimationState state = GetAnimationState(_name, _animClip, _replace);
        if (state)
        {
            state.layer = 0;
            state.wrapMode = WrapMode.Loop;
        }
        curDurativeName = _name;
    }

    /// <summary>
    /// 设置采集动作名称
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_animClip"></param>
    /// <param name="_replace"></param>
    public void SetCollectAnimationName(string _name, AnimationClip _animClip, bool _replace = false)
    {
        AnimationState state = GetAnimationState(_name, _animClip, _replace);
        if (state)
        {
            state.layer = 0;
            state.wrapMode = WrapMode.Loop;
        }
        curCollectAnimName = _name;
    }
    #endregion
    #region 执行
    /// <summary>
    /// 停止一切动作
    /// </summary>
    public override void Stop()
    {
        curAbilityAnimName = "";
        curAbilityAnimNameClone = "";
        needBlendToStop = false;
        isCasting = false;
        firstAnim = false;
        isMoving = false;

        base.Stop();
    }
    /// <summary>
    /// 受伤结束
    /// </summary>
    public void StopInjury()
    {
        stateMachine.Send((int)EventType.InjuryFinish);
    }
    /// <summary>
    /// 死亡
    /// </summary>
    public void Dead(bool _already = false)
    {
        alreadyDead = _already;
        stateMachine.Send((int)EventType.Dead);
    }
    /// <summary>
    /// 被击
    /// </summary>
    public void BeHit(float _time,float _weight =0.0f, string _animName = "hit")
    {
        if (_animName == string.Empty) return;
        if (anim != null)
        {
            if (anim[_animName] != null)
            {
                anim[_animName].wrapMode = WrapMode.Once;
                anim[_animName].blendMode = AnimationBlendMode.Blend;
                anim[_animName].layer = 3;
                anim.Rewind(_animName);
                anim.Blend(_animName, _weight, _time);
            }
            else
            {
                Debug.LogError("找不到动画:" + _animName);
            }
        }
    }

    public void StopAnim()
    {
        if (anim != null)
        {
            anim.Stop();
        }
    }
    /// <summary>
    /// 被僵直
    /// </summary>
    /// <param name="_time"></param>
    public void BeRigidity(float _time)
    {
        if (rigidityTime - (Time.time - castTime) < _time)
        {
            rigidityTime = _time;
            castTime = Time.time;
        }
    }

    /// <summary>
    /// 被击倒
    /// </summary>
    /// <param name="_donwTime"></param>
    /// <param name="_upTime"></param>
    public void BeKickDown(float _moveTime, float _rigidityTime)
    {
        kickrigidityTime = _rigidityTime;
        stateMachine.Send((int)EventType.Injury);
    }
    public void BeKickFly(float _moveTime, float _rigidityTime)
    {
        kickrigidityTime = _rigidityTime;
        if (CurEventType == EventType.InjuryFlyPrepare || CurEventType == EventType.InjuryFlyProcess)
        {
            stateMachine.Send((int)EventType.InjuryFlyProcess);
        }
        else
        {
            stateMachine.Send((int)EventType.InjuryFlyPrepare);
        }
    }
    /// <summary>
    /// 获取动画时间长度
    /// </summary>
    /// <param name="_animName"></param>
    /// <returns></returns>
    public float GetAnimLength(string _animName)
    {
        AnimationState animState = GetAnimationState(_animName, null);
        if (animState)
        {
            return animState.clip.length;
        }
        return 0;
    }
    /// <summary>
    /// 进入战斗状态切换
    /// </summary>
    /// <param name="_inCombat"></param>
    public void SetInCombat(bool _inCombat)
    {
        if (isInCombat != _inCombat)
        {
            isInCombat = _inCombat;
            curIdleName = isInCombat ? combatIdleName : normalIdleName;
            curMoveName = isInCombat ? combatMoveName : normalMoveName;
            stateMachine.Send((int)ActorAnimFSM.EventType.SwitchToCombat);
        }
    }
    /// <summary>
    /// 重启状态机
    /// </summary>
    public void ReStart()
    {
        if (stateMachine != null)
        {
            stateMachine.Restart();
            isCasting = false;
            
            stateMachine.Send((int)EventType.Idle);
        }
    }
    /// <summary>
    /// 跳转到指定状态 by吴江
    /// </summary>
    /// <param name="_type"></param>
    public void GoState(EventType _type)
    {
        stateMachine.Send((int)_type);
    }
    /// <summary>
    /// 稍息 by吴江
    /// </summary>
    public void RandIdle()
    {
        if (isMoving) return;
        stateMachine.Send((int)ActorAnimFSM.EventType.RndIdle);
    }
    /// <summary>
    /// 立正 by吴江
    /// </summary>
    public void IdleImmediate()
    {
        if (anim && anim[curIdleName])
        {
            anim[curIdleName].wrapMode = WrapMode.Loop;
            anim.Play(curIdleName);
        }
    }

    public void StaticIdleImmediate()
    {
        if (anim && anim[curIdleName])
        {
            anim[curIdleName].wrapMode = WrapMode.Once;
            anim[curIdleName].speed = 1000.0f;
            anim.Play(curIdleName);
        }
    }
    /// <summary>
    /// 上下坐骑 by吴江
    /// </summary>
    /// <param name="_ride"></param>
    public void OnMount(bool _ride)
    {
        if (_ride)
        {
            stateMachine.Send(isMoving ? (int)ActorAnimFSM.EventType.MountMove : (int)ActorAnimFSM.EventType.MountIdle);
        }
        else
        {
            stateMachine.Send(isMoving ? (int)ActorAnimFSM.EventType.Move : (int)ActorAnimFSM.EventType.Idle);
        }
    }
    /// <summary>
    /// 坐骑移动的事件 by吴江
    /// </summary>
    public void MountMove()
    {
        stateMachine.Send((int)ActorAnimFSM.EventType.MountMove);
    }
    /// <summary>
    /// 坐骑停止移动的事件 by吴江
    /// </summary>
    public void MountStopMoving()
    {
        stateMachine.Send((int)ActorAnimFSM.EventType.MountIdle);
    }
    /// <summary>
    /// 持续动作 by吴江
    /// </summary>
    /// <param name="_animName"></param>
    public void Durative(string _animName,float _time = -1,WrapMode _mode = WrapMode.Loop)
    {
        if (_time <= 0 || _animName == string.Empty) return;
        durativeTime = _time;
        SetDurativeAnimationName(_animName, null);
        if (anim[_animName] != null)
        {
            anim[_animName].wrapMode = _mode;
            if (_mode == WrapMode.Once)
            {
                anim[_animName].speed = anim[_animName].length / _time;
            }
            stateMachine.Send((int)ActorAnimFSM.EventType.Durative);
        }
    }
    /// <summary>
    /// 结束持续动作 by吴江
    /// </summary>
    public void StopDurative()
    {
        durativeTime = 0;
        stateMachine.Send((int)ActorAnimFSM.EventType.StopDurative);
    }

    /// <summary>
    /// 持续动作时间
    /// </summary>
    protected float collectTime = -1;
    /// <summary>
    /// 采集动作开始时间
    /// </summary>
    protected float collectStartTime = 0;
    public void Collect(string _animName, float _time = -1, WrapMode _mode = WrapMode.Loop)
    {
        if (_time <= 0 || _animName == string.Empty) return;
        collectTime = _time;
        SetCollectAnimationName(_animName, null);
        if (anim[_animName] != null)
        {
            anim[_animName].wrapMode = _mode;
            if (_mode == WrapMode.Once)
            {
                anim[_animName].speed = anim[_animName].length / _time;
            }
            stateMachine.Send((int)ActorAnimFSM.EventType.Collect);
        }
    }
    /// <summary>
    /// 结束采集动作 by邓成
    /// </summary>
    public void StopCollect(bool _isMoving)
    {
        collectTime = 0;
        //stateMachine.Send((int)ActorAnimFSM.EventType.StopDurative);
        if (CurEventType == EventType.AbilityPrepare || CurEventType == EventType.AbilityProcess)
        {
            //退出采集动作时,若处在放技能状态,不设置动作状态
        }
        else
        {
            if (_isMoving)
            {
                Move();
            }
            else
            {
                Idle();
            }
        }
    }
    #endregion
    #endregion

    #region 状态机
    protected fsm.State switchCombat;
    protected fsm.State mountMove;
    protected fsm.State mountIdle;
    protected fsm.State injury;
    protected fsm.State injuryFinish;
    protected fsm.State dead;
    protected fsm.State collect;
    protected fsm.State durative;
    protected fsm.State stopDurative;
    protected fsm.State abilityPrepare;
    protected fsm.State abilityProcess;
    protected fsm.State abilityEnding;
    protected fsm.State injuryFlyPrepare;
    protected fsm.State injuryFlyProcess;
    protected fsm.State injuryFlyEnding;


    protected override void InitStateMachine()
    {
        base.InitStateMachine();

        switchCombat = new fsm.State("switchCombat", basicState);
        switchCombat.onEnter += EnterSwitchCombatState;
        switchCombat.onAction += UpdateSwitchCombatState;

		fsm.State movie = new fsm.State("movie", basicState);
		movie.onEnter += EnterMovieState;
		movie.onAction += UpdateMovieState;

        mountMove = new fsm.State("mountMove", basicState);
        mountMove.onEnter += EnterMountMoveState;
        mountMove.onExit += ExitMountMoveState;

        mountIdle = new fsm.State("mountIdle", basicState);
        mountIdle.onEnter += EnterMountIdleState;
        mountIdle.onExit += ExitMountIdleState;


        injury = new fsm.State("injury", basicState);
        injury.onEnter += EnterInjuryState;
        injury.onExit += ExitInjuryState;
        injury.onAction += UpdateInjuryState;

        injuryFinish = new fsm.State("injuryFinish", basicState);
        injuryFinish.onEnter += EnterInjuryFinishState;
        injuryFinish.onExit += ExitInjuryFinishState;
        injuryFinish.onAction += UpdateInjuryFinishState;

        dead = new fsm.State("dead", basicState);
        dead.onEnter += EnterDeadState;
        dead.onAction += UpdateDeadState;
        dead.onExit += ExitDeadState;

        collect = new fsm.State("collect", basicState);
        collect.onEnter += EnterCollectState;
        collect.onAction += UpdateCollectState;
        collect.onExit += ExitCollectState;

        durative = new fsm.State("durative", basicState);
        durative.onEnter += EnterDurativeState;
        durative.onAction += UpdateDurativeState;
        durative.onExit += ExitDurativeState;

        stopDurative = new fsm.State("stopDurative", basicState);
        stopDurative.onEnter += EnterStopDurativeState;
        stopDurative.onExit += ExitDurativeState;



        abilityPrepare = new fsm.State("abilityPrepare", basicState);
        abilityPrepare.onEnter += EnterAbilityPrepareState;
        abilityPrepare.onAction += UpdateAbilityPrepareState;
        abilityPrepare.onExit += ExitAbilityPrepareState;

        abilityProcess = new fsm.State("abilityProcess", basicState);
        abilityProcess.onEnter += EnterAbilityProcessState;
        abilityProcess.onAction += UpdateAbilityProcessState;
        abilityProcess.onExit += ExitAbilityProcessState;


        abilityEnding = new fsm.State("abilityEnding", basicState);
        abilityEnding.onEnter += EnterAbilityEndingState;
        abilityEnding.onAction += UpdateAbilityEndingState;
        abilityEnding.onExit += ExitAbilityEndingState;

        injuryFlyPrepare = new fsm.State("injuryFlyPrepare", basicState);
        injuryFlyPrepare.onEnter += EnterInjuryFlyPrepareState;
        injuryFlyPrepare.onAction += UpdateInjuryFlyPrepareState;
        injuryFlyPrepare.onExit += ExitInjuryFlyPrepareState;

        injuryFlyProcess = new fsm.State("injuryFlyProcess", basicState);
        injuryFlyProcess.onEnter += EnterInjuryFlyProcessState;
        injuryFlyProcess.onAction += UpdateInjuryFlyProcessState;
        injuryFlyProcess.onExit += ExitInjuryFlyProcessState;

        injuryFlyEnding = new fsm.State("injuryFlyEnding", basicState);
        injuryFlyEnding.onEnter += EnterInjuryFlyEndingState;
        injuryFlyEnding.onAction += UpdateInjuryFlyEndingState;
        injuryFlyEnding.onExit += ExitInjuryFlyEndingState;

        



        idle.Add<fsm.EventTransition>(dead, (int)EventType.Dead);
        idle.Add<fsm.EventTransition>(move, (int)EventType.Move);
        idle.Add<fsm.EventTransition>(injury, (int)EventType.Injury);
        idle.Add<fsm.EventTransition>(randIdle, (int)EventType.RndIdle);
        idle.Add<fsm.EventTransition>(switchCombat, (int)EventType.SwitchToCombat);
        idle.Add<fsm.EventTransition>(mountMove, (int)EventType.MountMove);
        idle.Add<fsm.EventTransition>(mountIdle, (int)EventType.MountIdle);
        idle.Add<fsm.EventTransition>(durative, (int)EventType.Durative);
        idle.Add<fsm.EventTransition>(collect, (int)EventType.Collect);
        idle.Add<fsm.EventTransition>(abilityPrepare, (int)EventType.AbilityPrepare);
        idle.Add<fsm.EventTransition>(injuryFlyPrepare, (int)EventType.InjuryFlyPrepare);
		idle.Add<fsm.EventTransition>(movie, (int)EventType.Movie);

        durative.Add<fsm.EventTransition>(stopDurative, (int)EventType.StopDurative);
        durative.Add<fsm.EventTransition>(dead, (int)EventType.Dead);

        collect.Add<fsm.EventTransition>(durative, (int)EventType.Durative);
        collect.Add<fsm.EventTransition>(dead, (int)EventType.Dead);
        collect.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        collect.Add<fsm.EventTransition>(move, (int)EventType.Move);
        collect.Add<fsm.EventTransition>(abilityPrepare, (int)EventType.AbilityPrepare);
        collect.Add<fsm.EventTransition>(abilityProcess, (int)EventType.AbilityProcess);
        collect.Add<fsm.EventTransition>(mountIdle, (int)EventType.MountIdle);
        collect.Add<fsm.EventTransition>(mountMove, (int)EventType.MountMove);

        stopDurative.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        stopDurative.Add<fsm.EventTransition>(dead, (int)EventType.Dead);
        stopDurative.Add<fsm.EventTransition>(move, (int)EventType.Move);
        stopDurative.Add<fsm.EventTransition>(injury, (int)EventType.Injury);
        stopDurative.Add<fsm.EventTransition>(abilityPrepare, (int)EventType.AbilityPrepare);
        stopDurative.Add<fsm.EventTransition>(injuryFlyPrepare, (int)EventType.InjuryFlyPrepare);

        injuryFlyPrepare.Add<fsm.EventTransition>(injuryFlyProcess, (int)EventType.InjuryFlyProcess);
        injuryFlyPrepare.Add<fsm.EventTransition>(dead, (int)EventType.Dead);
        injuryFlyPrepare.Add<fsm.EventTransition>(move, (int)EventType.Move);
        injuryFlyPrepare.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        injuryFlyPrepare.Add<fsm.EventTransition>(injury, (int)EventType.Injury);

        injuryFlyProcess.Add<fsm.EventTransition>(injuryFlyProcess, (int)EventType.InjuryFlyProcess);
        injuryFlyProcess.Add<fsm.EventTransition>(injuryFlyEnding, (int)EventType.InjuryFlyEnd);
        injuryFlyProcess.Add<fsm.EventTransition>(dead, (int)EventType.Dead);
        injuryFlyProcess.Add<fsm.EventTransition>(move, (int)EventType.Move);
        injuryFlyProcess.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        injuryFlyProcess.Add<fsm.EventTransition>(injury, (int)EventType.Injury);

        injuryFlyEnding.Add<fsm.EventTransition>(injuryFlyProcess, (int)EventType.InjuryFlyProcess);
        injuryFlyEnding.Add<fsm.EventTransition>(injuryFlyPrepare, (int)EventType.InjuryFlyPrepare);
        injuryFlyEnding.Add<fsm.EventTransition>(dead, (int)EventType.Dead);
        injuryFlyEnding.Add<fsm.EventTransition>(move, (int)EventType.Move);
        injuryFlyEnding.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        injuryFlyEnding.Add<fsm.EventTransition>(injury, (int)EventType.Injury);


        switchCombat.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        switchCombat.Add<fsm.EventTransition>(injury, (int)EventType.Injury);
        switchCombat.Add<fsm.EventTransition>(abilityPrepare, (int)EventType.AbilityPrepare);
        switchCombat.Add<fsm.EventTransition>(injuryFlyPrepare, (int)EventType.InjuryFlyPrepare);
        switchCombat.Add<fsm.EventTransition>(dead, (int)EventType.Dead);

        randIdle.Add<fsm.EventTransition>(dead, (int)EventType.Dead);
        randIdle.Add<fsm.EventTransition>(move, (int)EventType.Move);
        randIdle.Add<fsm.EventTransition>(injury, (int)EventType.Injury);
        randIdle.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        randIdle.Add<fsm.EventTransition>(mountMove, (int)EventType.MountMove);
        randIdle.Add<fsm.EventTransition>(mountIdle, (int)EventType.MountIdle);
        randIdle.Add<fsm.EventTransition>(durative, (int)EventType.Durative);
        randIdle.Add<fsm.EventTransition>(abilityPrepare, (int)EventType.AbilityPrepare);
        randIdle.Add<fsm.EventTransition>(injuryFlyPrepare, (int)EventType.InjuryFlyPrepare);

        move.Add<fsm.EventTransition>(dead, (int)EventType.Dead);
        move.Add<fsm.EventTransition>(idle, (int)EventType.StopMoving);
        move.Add<fsm.EventTransition>(mountMove, (int)EventType.MountMove);
        move.Add<fsm.EventTransition>(mountIdle, (int)EventType.MountIdle);
        move.Add<fsm.EventTransition>(durative, (int)EventType.Durative);
        move.Add<fsm.EventTransition>(injury, (int)EventType.Injury);
        move.Add<fsm.EventTransition>(abilityPrepare, (int)EventType.AbilityPrepare);
        move.Add<fsm.EventTransition>(injuryFlyPrepare, (int)EventType.InjuryFlyPrepare);
        move.Add<fsm.EventTransition>(collect, (int)EventType.Collect);

        mountMove.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        mountMove.Add<fsm.EventTransition>(move, (int)EventType.Move);
        mountMove.Add<fsm.EventTransition>(mountIdle, (int)EventType.MountIdle);
        mountMove.Add<fsm.EventTransition>(abilityPrepare, (int)EventType.AbilityPrepare);

        mountIdle.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        mountIdle.Add<fsm.EventTransition>(move, (int)EventType.Move);
        mountIdle.Add<fsm.EventTransition>(mountMove, (int)EventType.MountMove);
        mountIdle.Add<fsm.EventTransition>(abilityPrepare, (int)EventType.AbilityPrepare);


        injury.Add<fsm.EventTransition>(dead, (int)EventType.Dead);
        injury.Add<fsm.EventTransition>(move, (int)EventType.Move);
        injury.Add<fsm.EventTransition>(injuryFinish, (int)EventType.InjuryFinish);
        injury.Add<fsm.EventTransition>(injuryFlyPrepare, (int)EventType.InjuryFlyPrepare);

        injuryFinish.Add<fsm.EventTransition>(dead, (int)EventType.Dead);
        injuryFinish.Add<fsm.EventTransition>(move, (int)EventType.Move);
        injuryFinish.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        injuryFinish.Add<fsm.EventTransition>(injury, (int)EventType.Injury);
        injuryFinish.Add<fsm.EventTransition>(injuryFlyPrepare, (int)EventType.InjuryFlyPrepare);

        dead.Add<fsm.EventTransition>(idle, (int)EventType.Idle);


        abilityPrepare.Add<fsm.EventTransition>(abilityProcess, (int)EventType.AbilityProcess);
        abilityPrepare.Add<fsm.EventTransition>(abilityEnding, (int)EventType.AbilityEnding);
        abilityPrepare.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        abilityPrepare.Add<fsm.EventTransition>(move, (int)EventType.Move);
        abilityPrepare.Add<fsm.EventTransition>(durative, (int)EventType.Durative);
        abilityPrepare.Add<fsm.EventTransition>(injury, (int)EventType.Injury);
        abilityPrepare.Add<fsm.EventTransition>(injuryFlyPrepare, (int)EventType.InjuryFlyPrepare);
        abilityPrepare.Add<fsm.EventTransition>(dead, (int)EventType.Dead);
		abilityPrepare.Add<fsm.EventTransition>(mountIdle, (int)EventType.MountIdle);
		abilityPrepare.Add<fsm.EventTransition>(mountMove, (int)EventType.MountMove);

        abilityProcess.Add<fsm.EventTransition>(abilityEnding, (int)EventType.AbilityEnding);
        abilityProcess.Add<fsm.EventTransition>(durative, (int)EventType.Durative);
        abilityProcess.Add<fsm.EventTransition>(abilityPrepare, (int)EventType.AbilityPrepare);
        abilityProcess.Add<fsm.EventTransition>(injury, (int)EventType.Injury);
        abilityProcess.Add<fsm.EventTransition>(injuryFlyPrepare, (int)EventType.InjuryFlyPrepare);
        abilityProcess.Add<fsm.EventTransition>(move, (int)EventType.Move);
        abilityProcess.Add<fsm.EventTransition>(dead, (int)EventType.Dead);
		abilityProcess.Add<fsm.EventTransition>(mountIdle, (int)EventType.MountIdle);
		abilityProcess.Add<fsm.EventTransition>(mountMove, (int)EventType.MountMove);

        abilityEnding.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        abilityEnding.Add<fsm.EventTransition>(move, (int)EventType.Move);
        abilityEnding.Add<fsm.EventTransition>(durative, (int)EventType.Durative);
        abilityEnding.Add<fsm.EventTransition>(abilityPrepare, (int)EventType.AbilityPrepare);
        abilityEnding.Add<fsm.EventTransition>(injury, (int)EventType.Injury);
        abilityEnding.Add<fsm.EventTransition>(injuryFlyPrepare, (int)EventType.InjuryFlyPrepare);
        abilityEnding.Add<fsm.EventTransition>(dead, (int)EventType.Dead);
		abilityEnding.Add<fsm.EventTransition>(mountIdle, (int)EventType.MountIdle);
		abilityEnding.Add<fsm.EventTransition>(mountMove, (int)EventType.MountMove);
    }

    #region 立正和稍息 by吴江
    /// <summary>
    /// 稍息的动作名称
    /// </summary>
    protected string curRandIdleName = "waiting";
    /// <summary>
    /// 是否有站立展示动作
    /// </summary>
    protected bool hasPreviewIdle = false;
    /// <summary>
    /// 是否有站立稍息动作
    /// </summary>
    protected bool hasRamdomIdle = false;
    /// <summary>
    /// 站立的持续时间
    /// </summary>
    protected float duration = 0.0f;
    /// <summary>
    /// 已站立的时间
    /// </summary>
    protected float timer = 0.0f;
    /// <summary>
    /// 稍息随机时间上限
    /// </summary>
    public float randomLimit = 48.0f;
    protected override void EnterIdleState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        CurEventType = EventType.Idle;
        if (_from.name != "stopCast")
        {
            if (anim[curIdleName] != null)
            {
                anim[curIdleName].speed = 1.0f;
                anim.CrossFade(curIdleName, 0.5f);
            }
            else
            {
                Debug.LogError("对象:" + this.transform.root.name + " , 没有动画 : " + curIdleName + " , 站立失败!");
            }
        }
        else
        {
            AnimationState state = anim.GetPlayingAnimation();
            if (state != null && state.name == "hit")
            {
                anim[curIdleName].speed = 1.0f;
                anim.CrossFade(curIdleName, 0.5f);
            }
            else
            {
                anim[curIdleName].speed = 1.0f;
                anim.CrossFade(curIdleName, 0.5f);
                anim.PlayQueued(curIdleName, QueueMode.CompleteOthers);
            }
        }
        timer = Time.time + UnityEngine.Random.Range(5f, randomLimit);
    }

    protected override void UpdateIdleState(fsm.State _curState)
    {
        if (hasRamdomIdle && anim != null && anim.enabled && !isMoving)
        {
            if (Time.time - timer > 0)
            {
                timer = UnityEngine.Random.Range(5f, randomLimit);
                stateMachine.Send((int)EventType.RndIdle);
            }
        }
    }


    protected override void ExitIdleState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        anim.Stop(curIdleName);
    }

    protected override void EnterRandIdleState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        CurEventType = EventType.RndIdle;
        if (isMoving)
        {
            stateMachine.Send((int)EventType.Move);
            return;
        }
        timer = Time.time;
        if (hasRamdomIdle)
        {
            anim[curRandIdleName].speed = 1.0f;
            duration = anim.GetClip(curRandIdleName).length;
            if (_from.name == "castNoMove")
            {
                anim.CrossFade(curRandIdleName, 0.3f);
            }
            else
            {
                anim.CrossFade(curRandIdleName, 0.1f);
            }
            anim.CrossFadeQueued(curIdleName, 0.5f, QueueMode.CompleteOthers);
        }
        else
        {
            
            stateMachine.Send((int)EventType.Idle);
        }
    }

    protected override void UpdateRondomState(fsm.State _curState)
    {
        if (isMoving)
        {
            stateMachine.Send((int)EventType.Move);
            return;
        }
        if (Time.time - timer > duration && !isMoving)
        {
            
            stateMachine.Send((int)EventType.Idle);
        }
    }
    #endregion
    #region 步行 by吴江
    protected override void EnterMoveState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        CurEventType = EventType.Move;
        isMoving = true;
        float fadeDuraton = 0.3f;
        if (anim[curMoveName] != null)
        {
            anim[curMoveName].wrapMode = WrapMode.Loop;
            if (_from.name == "jump")
            {
                BlendToStop("jump", curMoveName, 1.0f);
                //anim.CrossFadeQueued(curMoveName);
            }
            else
            {
                anim.CrossFade(curMoveName, fadeDuraton);

            }
        }
        else
        {
            Debug.LogError(this.gameObject.name + " 没有 " + curMoveName + "动画!");
        }
    }

    protected override void ExitMoveState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        isMoving = false;
    }
    #endregion
    #region 进入战斗 by吴江
    protected virtual void EnterSwitchCombatState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        CurEventType = EventType.SwitchToCombat;
        stateMachine.Send((int)EventType.Idle);

    }

    protected virtual void UpdateSwitchCombatState(fsm.State _curState)
    {
    }
    #endregion

	#region 展示动作
	void EnterMovieState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
		anim.Play("idle2_1");
		anim.wrapMode = WrapMode.Loop;
	}


	void UpdateMovieState(fsm.State _curState)
	{

	}
	#endregion

    #region 骑行 by吴江
    protected virtual void EnterMountMoveState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        CurEventType = EventType.MountMove;
        isMoving = true;
        string mountMoveName = "ride";
        //float fadeDuraton = 0.1f;
        if (anim[mountMoveName] != null)
        {
            anim.Rewind(mountMoveName);
            anim.Play(mountMoveName);
            //anim.CrossFade(mountMoveName, fadeDuraton);
        }
        else
        {
            stateMachine.Send((int)EventType.Move);
        }
    }

    protected virtual void ExitMountMoveState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        isMoving = false;
    }
    #endregion
    #region 骑止 by吴江
    protected virtual void EnterMountIdleState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        CurEventType = EventType.MountIdle;
        string mountIdleName = "ride";
       // float fadeDuraton = 0.1f;
        if (anim[mountIdleName] != null)
        {
            anim.Rewind(mountIdleName);
            anim.Play(mountIdleName);
            //anim.CrossFade(mountIdleName, fadeDuraton);
        }
        else
        {
            
            stateMachine.Send((int)ActorAnimFSM.EventType.Idle);
        }
    }

    protected virtual void ExitMountIdleState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {

    }
    #endregion
    #region 受伤倒地 by吴江
    protected float injuryTimer = 0.0f;
    protected float injuryDuration = 0.0f;
    protected float kickrigidityTime = 0.0f;
    protected bool hasInitInjured = false;
    protected float injuryTotalLength = 0.0f;
    protected float injuryDownTime = 0.0f;
    protected float injuryUpTime = 0.0f;
    protected AnimationState downState = null;
    protected AnimationState upState = null;

    void EnterInjuryState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        CurEventType = EventType.Injury;
        StopCoroutine("BlendToStop_CO");
        injuryTimer = -Time.deltaTime;
        injuryDuration = 0.0f;

        if (!hasInitInjured)
        {
            downState = anim[injuredDonwName];
            upState = anim[injuredUpName];
            if (downState && upState)
            {
                downState.wrapMode = WrapMode.ClampForever;
                upState.wrapMode = WrapMode.Once;
                injuryDownTime = downState.length;
                injuryUpTime = upState.length;
                injuryTotalLength = injuryDownTime + injuryUpTime;
            }
            hasInitInjured = true;
        }

        if (kickrigidityTime == 0) kickrigidityTime = injuryTotalLength;
        if (kickrigidityTime <= injuryTotalLength)
        {
            float rate = injuryTotalLength / kickrigidityTime;
            downState.speed = rate;
            upState.speed = rate;
            injuryDuration = injuryDownTime * kickrigidityTime / injuryTotalLength;
        }
        else
        {
            downState.speed = 1.0f; ;
            upState.speed = 1.0f;
            injuryDuration = kickrigidityTime - injuryUpTime;
        }


        anim.Rewind(injuredDonwName);
        anim.CrossFade(injuredDonwName,0.1f);
    }

    void ExitInjuryState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
    }

    void UpdateInjuryState(fsm.State _curState)
    {
        injuryTimer += Time.deltaTime;
        if (injuryTimer >= injuryDuration)
        {
            stateMachine.Send((int)EventType.InjuryFinish);
            return;
        }
    }


    void EnterInjuryFinishState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        injuryTimer = -Time.deltaTime;
        injuryDuration = 0.0f;


        if (kickrigidityTime <= injuryTotalLength)
        {
            injuryDuration = injuryUpTime * kickrigidityTime / injuryTotalLength;
        }
        else
        {
            injuryDuration = injuryUpTime;
        }

        anim.Rewind(injuredUpName);
        anim.CrossFade(injuredUpName,0.1f);
    }

    void ExitInjuryFinishState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        //BlendToStop(injuredUpName, 0.1f);
    }

    void UpdateInjuryFinishState(fsm.State _curState)
    {
        injuryTimer += Time.deltaTime;
        if (injuryTimer >= injuryDuration)
        {
            StopCast();
            return;
        }
    }
    #endregion
    #region 受伤击飞 by吴江
    protected float injuryFlyTimer = 0.0f;
    protected float injuryFlyUpDuration = 0.0f;
    protected float injuryFlyProcessDuration = 0.0f;
    protected float injuryFlyDonwDuration = 0.0f;
    protected bool hasInitInjuredFly = false;
    protected float injuryFlyTotalLength = 0.0f;
    protected float injuryFlyDownTime = 0.0f;
    protected float injuryFlyProcessTime = 0.0f;
    protected float injuryFlyUpTime = 0.0f;
    protected AnimationState flyDownState = null;
    protected AnimationState flyProcessState = null;
    protected AnimationState flyUpState = null;

    /// <summary>
    /// 进入前摇状态 by吴江
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    /// <param name="_event"></param>
    protected void EnterInjuryFlyPrepareState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (flyUpState == null || flyProcessState == null || flyDownState == null)
        {
            stateMachine.Send((int)EventType.Idle);
            return;
        }
        CurEventType = EventType.InjuryFlyPrepare;
        StopCoroutine("BlendToStop_CO");
        injuryTimer = -Time.deltaTime;

        if (!hasInitInjuredFly)
        {
            flyUpState = anim[injuredFlyUpName];
            flyProcessState = anim[injuredFlyProcessName];
            flyDownState = anim[injuredFlyDonwName];
            if (flyUpState && flyProcessState && flyDownState)
            {
                flyProcessState.wrapMode = WrapMode.ClampForever;
                flyUpState.wrapMode = WrapMode.Once;
                flyDownState.wrapMode = WrapMode.Once;

                injuryFlyUpTime = flyUpState.length;
                injuryFlyProcessTime = flyProcessState.length;
                injuryFlyDownTime = flyDownState.length;

                injuryFlyTotalLength = injuryFlyUpTime + injuryFlyProcessTime + injuryFlyDownTime;
            }
            hasInitInjuredFly = true;
        }


        if (kickrigidityTime == 0) kickrigidityTime = injuryFlyTotalLength;
        if (kickrigidityTime <= injuryFlyTotalLength)
        {
            float rate = injuryFlyTotalLength / kickrigidityTime;
            flyUpState.speed = rate;
            flyProcessState.speed = rate;
            flyDownState.speed = rate;
            injuryFlyUpDuration = injuryFlyUpTime * kickrigidityTime / injuryFlyTotalLength;
        }
        else
        {
            flyUpState.speed = 1.0f; ;
            flyProcessState.speed = 1.0f;
            flyDownState.speed = 1.0f;
            injuryFlyUpDuration = injuryFlyUpTime;
        }


        anim.Rewind(injuredFlyUpName);
        anim.CrossFade(injuredFlyUpName, 0.1f);
    }
    /// <summary>
    /// 前摇状态更新 by吴江
    /// </summary>
    /// <param name="_curState"></param>
    protected void UpdateInjuryFlyPrepareState(fsm.State _curState)
    {
        injuryTimer += Time.deltaTime;
        if (injuryTimer >= injuryFlyUpDuration)
        {
            stateMachine.Send((int)EventType.InjuryFlyProcess);
            return;
        }
    }
    /// <summary>
    /// 离开前摇状态 by吴江
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    /// <param name="_event"></param>
    protected void ExitInjuryFlyPrepareState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
    
    }


    /// <summary>
    /// 进入释放状态 by吴江
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    /// <param name="_event"></param>
    protected void EnterInjuryFlyProcessState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (flyUpState == null || flyProcessState == null || flyDownState == null)
        {
            stateMachine.Send((int)EventType.Idle);
            return;
        }
        CurEventType = EventType.InjuryFlyProcess;
        injuryTimer = -Time.deltaTime;

        if (kickrigidityTime == 0) kickrigidityTime = injuryFlyTotalLength;
        if (kickrigidityTime <= injuryFlyTotalLength)
        {
            float rate = injuryFlyTotalLength / kickrigidityTime;
            flyUpState.speed = rate;
            flyProcessState.speed = rate;
            flyDownState.speed = rate;
            injuryFlyProcessDuration = injuryFlyProcessTime * kickrigidityTime / injuryFlyTotalLength;
            injuryFlyDonwDuration = injuryFlyDownTime * kickrigidityTime / injuryFlyTotalLength;
        }
        else
        {
            flyUpState.speed = 1.0f; ;
            flyProcessState.speed = 1.0f;
            flyDownState.speed = 1.0f;
            injuryFlyProcessDuration = injuryFlyProcessTime;
            injuryFlyDonwDuration = injuryFlyDownTime;
        }

        anim.Rewind(injuredFlyProcessName);
        anim.CrossFade(injuredFlyProcessName, 0.1f);
    
    }

    /// <summary>
    /// 释放状态更新 by吴江
    /// </summary>
    /// <param name="_curState"></param>
    protected void UpdateInjuryFlyProcessState(fsm.State _curState)
    {
        injuryTimer += Time.deltaTime;
        if (injuryTimer >= injuryFlyProcessDuration)
        {
            stateMachine.Send((int)EventType.InjuryFlyEnd);
            return;
        }
    }
    /// <summary>
    /// 离开释放状态 by吴江
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    /// <param name="_event"></param>
    protected void ExitInjuryFlyProcessState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
    
    }

    /// <summary>
    /// 进入后摇状态 by吴江
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    /// <param name="_event"></param>
    protected void EnterInjuryFlyEndingState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (flyUpState == null || flyProcessState == null || flyDownState == null)
        {
            stateMachine.Send((int)EventType.Idle);
            return;
        }
        CurEventType = EventType.InjuryFlyEnd;
        injuryTimer = -Time.deltaTime;

        anim.Rewind(injuredFlyDonwName);
        anim.CrossFade(injuredFlyDonwName, 0.1f);
    }
    /// <summary>
    /// 后摇状态更新 by吴江
    /// </summary>
    /// <param name="_curState"></param>
    protected void UpdateInjuryFlyEndingState(fsm.State _curState)
    {
        injuryTimer += Time.deltaTime;
        if (injuryTimer >= injuryFlyDonwDuration)
        {
            StopCast();
            return;
        }
    }
    /// <summary>
    /// 离开后摇状态 by吴江
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    /// <param name="_event"></param>
    protected void ExitInjuryFlyEndingState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
       
    }




    #endregion
    #region 死亡 by吴江
    float deadDuration = 0;
    float deadTimer = 0;
    protected bool alreadyDead = false;
    protected bool hasDeadEnd = false;
    public System.Action OnDeadEnd;
    void EnterDeadState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        CurEventType = EventType.Dead;
        isCasting = false;
        anim.Stop();
        if (alreadyDead)
        {
            deadTimer = 0;
            if (anim["die"])
            {
                anim["die"].wrapMode = WrapMode.Once;
                anim["die"].speed = 1000f;
                anim.Play("die");
            }
            else
            {
                Debug.LogError(this.gameObject.name + "没有死亡动画 die");
            }
            deadDuration = 0;
            hasDeadEnd = true;
            if (OnDeadEnd != null)
            {
                OnDeadEnd();
            }
        }
        else
        {
            if (anim["die"])
            {
                anim["die"].speed = 1.0f;
                deadDuration = anim["die"].length;
                anim["die"].wrapMode = WrapMode.Once;
            }
            else
            {
                Debug.LogError(this.gameObject.name + "没有死亡动画 die");
            }
            anim.CrossFade("die");
            deadTimer = 0;
            hasDeadEnd = false;
        }
    }

    void UpdateDeadState(fsm.State _curState)
    {
        deadTimer += Time.deltaTime;
        if (deadTimer >= deadDuration && !hasDeadEnd)
        {
            hasDeadEnd = true;
            if (OnDeadEnd != null)
            {
                OnDeadEnd();
            }
        }
    }

    void ExitDeadState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
    }
    #endregion
    #region 持续动作 by吴江
    protected virtual void EnterDurativeState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        CurEventType = EventType.Durative;
        isCasting = true;
        durativeStartTime = Time.time;
        anim.Stop();
        anim.CrossFade(curDurativeName);
    }

    protected virtual void UpdateDurativeState(fsm.State _curState)
    {
        if (durativeTime > 0)
        {
            if (Time.time - durativeStartTime >= durativeTime)
            {
                stateMachine.Send((int)EventType.StopDurative);
            }
        }
    }

    protected virtual void ExitDurativeState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        durativeTime = -1;
    }

    protected virtual void EnterStopDurativeState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        //Debug.logger.Log("EnterStopDurativeState");
        isCasting = false;
        if (moveFSMRef != null)
        {
            if (moveFSMRef.isMoving)
            {
                stateMachine.Send((int)EventType.Move);
            }
            else
            {
                
                stateMachine.Send((int)EventType.Idle);
            }
        }
        else
        {
            
            stateMachine.Send((int)EventType.Idle);
        }
    }
    protected virtual void ExitStopDurativeState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {

    }
    #endregion

    #region 采集动作 by邓成
    protected virtual void EnterCollectState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        CurEventType = EventType.Collect;
        isCasting = true;
        collectStartTime = Time.time;
        anim.Stop();
        anim.CrossFade(curCollectAnimName);
    }

    protected virtual void UpdateCollectState(fsm.State _curState)
    {
        if (collectTime > 0)
        {
            if (Time.time - collectStartTime >= collectTime)
            {
            //    stateMachine.Send((int)EventType.StopDurative);
            }
        }
    }

    protected virtual void ExitCollectState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        durativeTime = -1;
    }
    #endregion

    #region 技能动作 by吴江
    protected string curPlayingAnimationName = string.Empty;
    /// <summary>
    /// 停步,开始动作
    /// </summary>
    /// <param name="_animInfo"></param>
    public virtual void Cast(AbilityInstance _animInfo)
    {
        lastAbilityName = _animInfo.AbilityName;
        castTime = Time.time;
        if (_animInfo.thisSkillMode == SkillMode.NORMALSKILL)
        {
            normalCastTime = Time.time;
        }
        rigidityTime = _animInfo.RigidityTime;
        rigidityStartTime = _animInfo.RigidityStartTime;

        normalAttakProtectTime = _animInfo.ProtectDuration;

        blendToStopDuration = 0.1f;

        curCastAbility = _animInfo;
        stateMachine.Send((int)EventType.AbilityPrepare);

    }
    /// <summary>
    /// 停止技能  by吴江 
    /// </summary>
    public virtual void StopCast()
    {
        lastCastAbility = curCastAbility;
        if (CurEventType != EventType.AbilityEnding && CurEventType != EventType.Jump)
        {
            if (moveFSMRef != null)
            {
                if (moveFSMRef.isMoving)
                {
                    stateMachine.Send((int)EventType.Move);
                }
                else
                {
                    stateMachine.Send((int)EventType.Idle);
                }
            }
            else
            {
                stateMachine.Send((int)EventType.Idle);
            }
        }
        if (fxCtrlRef != null)
        {
            fxCtrlRef.ClearAttckWholeTimeEffect();
        }

        needBlendToStop = false;
        curAbilityAnimName = "";
        curAbilityAnimNameClone = "";
        curAbilityID = -1;

        isCasting = false;
        if (fxCtrlRef != null)
        {
            fxCtrlRef.CancelAttackEffect();
        }

    }

    protected AbilityInstance lastCastAbility = null;
    protected AbilityInstance curCastAbility = null;

    protected float abilityDutation = 0.0f;


    /// <summary>
    /// 进入前摇状态 by吴江
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    /// <param name="_event"></param>
    protected void EnterAbilityPrepareState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        CurEventType = EventType.AbilityPrepare;
        isCasting = true;
        if (curBlendingToStopName != "")
        {
            StopCoroutine("BlendToStop_CO");
            anim.Stop(curBlendingToStopName);
            curBlendingToStopName = "";
        }


        curPlayingAnimationName = string.Empty;
        if (curCastAbility == null)
        {
            StopCast();
            return;
        }
        if (fxCtrlRef != null)
        {
            fxCtrlRef.DoAttckWholeTimeEffect(curCastAbility.WholetimeEffect,this.transform);
        }
        if (headTextCtrlRef != null && curCastAbility.PopID != 0)//在技能前摇时开始冒泡
        {
            PoPoRef popRef = ConfigMng.Instance.GetPoPoRef(curCastAbility.PopID);
            if (popRef != null)
            {
                int pro = popRef.probability / 100;
                System.Random random = new System.Random();
                bool canBubble = random.Next(1, 101) <= pro;
                if (canBubble) headTextCtrlRef.SetBubble(popRef.content, popRef.time);
            }
        }
        if (!curCastAbility.NeedPrepare || curCastAbility.PrepareAnimationName == string.Empty || curCastAbility.PrepareDuration <= 0)
        {
            abilityDutation = 0;
            return;
        }
        abilityDutation = curCastAbility.PrepareDuration;
        string curAnim = curCastAbility.PrepareAnimationName;
        float speedRate = 1.0f;
        if (anim[curAnim] != null)
        {
            anim[curAnim].wrapMode = WrapMode.Once;
            if (abilityDutation != 0)
            {
                speedRate = anim[curAnim].length / abilityDutation;
            }
            anim[curAnim].speed = speedRate;
            //anim.CrossFade(curAnim, 0.2f);
            anim.Play(curAnim);
            curPlayingAnimationName = curAnim;
        }
        if (fxCtrlRef != null && curCastAbility.PrepareEffectName != string.Empty)
        {
            fxCtrlRef.DoAttackEffect(curCastAbility.PrepareEffectName, abilityDutation, speedRate, Vector3.one,this.transform);
        }
    }
    /// <summary>
    /// 前摇状态更新 by吴江
    /// </summary>
    /// <param name="_curState"></param>
    protected void UpdateAbilityPrepareState(fsm.State _curState)
    {
        abilityDutation -= Time.deltaTime;
        if (abilityDutation <= 0 && curCastAbility.HasServerConfirm)
        {
            if (curCastAbility.ProcessAnimationNameList.Count == 0)
            {
                if (curCastAbility.EndingAnimationName == string.Empty || curCastAbility.EndingDuration == 0)
                {
                    StopCast();
                }
                else
                {
                    stateMachine.Send((int)EventType.AbilityEnding);
                }
            }
            else
            {
                //Debug.logger.Log("后台确认，跳转技能过程，时间截点 " + Time.time);
                stateMachine.Send((int)EventType.AbilityProcess);
            }
        }
    }
    /// <summary>
    /// 离开前摇状态 by吴江
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    /// <param name="_event"></param>
    protected void ExitAbilityPrepareState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (_to.name != "abilityProcess")
        {
            if (curCastAbility.ArrowID > 0)
            {
                GameCenter.sceneMng.C2C_AddBallisticCurve(curCastAbility);
            }
        }
        abilityDutation = 0.0f;
    }


    protected int processIndex;

    protected Vector3 curTarPos = Vector3.zero;
    protected Quaternion curQuaternion = Quaternion.identity;

    /// <summary>
    /// 进入释放状态 by吴江
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    /// <param name="_event"></param>
    protected void EnterAbilityProcessState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        curTarPos = Vector3.zero;
        CurEventType = EventType.AbilityProcess;
        processIndex = 0;
        if (curCastAbility == null)
        {
            stateMachine.Send((int)EventType.Idle);
            return;
        }
        List<AbilityDelayEffectRefData> AbilityDelayEffectRefDataList = curCastAbility.AbilityDelayEffectRefDataList;
        if (AbilityDelayEffectRefDataList.Count > 0)
        {
            for (int i = 0; i < AbilityDelayEffectRefDataList.Count; i++)
            {
                GameCenter.spawner.SpawnAbilityDelayEffectCtrl(AbilityDelayEffectRefDataList[i], this.transform.parent);
            }
        }
        if (curCastAbility.ProcessAnimationNameList.Count == 0)
        {
            stateMachine.Send((int)EventType.AbilityEnding);
            return;
        }

        switch (curCastAbility.CurClientShowType)
        {
            case ClientShowType.Invinciblechop:
                Transform tarTransform = curCastAbility.PopTarTransform();
                if (tarTransform != null)
                {
                    curTarPos = Utils.GetRandomPos(tarTransform);
                    curTarPos = curTarPos.SetY(curTarPos.y - 0.5f);
                    curQuaternion = Quaternion.LookRotation(tarTransform.position);
                }
                else
                {
                    this.transform.localPosition = Vector3.zero;
                    this.transform.rotation = Quaternion.identity;
                }
                if (headTextCtrlRef != null)
                {
                    headTextCtrlRef.textParent.SetActive(false);
                }
                break;
        }

        string curSound = curCastAbility.ProcessSoundPairList.Count > processIndex ? curCastAbility.ProcessSoundPairList[processIndex].res : string.Empty;
        GameCenter.soundMng.PlaySound(curSound, SoundMng.GetSceneSoundValue(transform, GameCenter.curMainPlayer.transform), false, true);

        string curAnim = curCastAbility.ProcessAnimationNameList[processIndex];
        abilityDutation = curCastAbility.ProcessDurationList[processIndex];
        float speedRate = 1.0f;
        if (anim[curAnim] != null)
        {
            anim[curAnim].wrapMode = WrapMode.Once;
            if (abilityDutation != 0)
            {
                speedRate = anim[curAnim].length / abilityDutation;
            }
            anim[curAnim].speed = speedRate;
            //if (curAnim == curPlayingAnimationName)
            //{
            //    anim.Play(curAnim);
            //}
            //else
            //{
            //    anim.CrossFade(curAnim);
            //}
            anim.Play(curAnim);
            if (fxCtrlRef != null && curCastAbility.ProcessEffectList.Count > processIndex && curCastAbility.ProcessEffectList[processIndex] != string.Empty)
            {
                fxCtrlRef.DoAttackEffect(curCastAbility.ProcessEffectList[processIndex], abilityDutation, speedRate, Vector3.one, this.transform);
            }
            curPlayingAnimationName = curAnim;
            ++processIndex;
        }
        else
        {
            Debug.LogError("技能动作" + curAnim + "为空!播放失败!");
        }
        startProgressTime = Time.time;
    }

    protected float startProgressTime = 0;
    /// <summary>
    /// 释放状态更新 by吴江
    /// </summary>
    /// <param name="_curState"></param>
    protected void UpdateAbilityProcessState(fsm.State _curState)
    {
        if (abilityDutation <= Time.time - startProgressTime)
        {
            startProgressTime = Time.time;
            if (processIndex < curCastAbility.ProcessAnimationNameList.Count)
            {
                switch (curCastAbility.CurClientShowType)
                {
                    case ClientShowType.Invinciblechop:
                        Transform tarTransform = curCastAbility.PopTarTransform();
                        if (tarTransform != null)
                        {
                            curTarPos = Utils.GetRandomPos(tarTransform);
                            curQuaternion = Quaternion.LookRotation(tarTransform.position);
                        }
                        else
                        {
                            this.transform.localPosition = Vector3.zero;
                            this.transform.rotation = Quaternion.identity;
                        }
                        break;
                }

                string curSound = curCastAbility.ProcessSoundPairList.Count > processIndex ? curCastAbility.ProcessSoundPairList[processIndex].res : string.Empty;
                GameCenter.soundMng.PlaySound(curSound, SoundMng.GetSceneSoundValue(transform, GameCenter.curMainPlayer.transform), false, true);

                string curAnim = curCastAbility.ProcessAnimationNameList[processIndex];
                abilityDutation = curCastAbility.ProcessDurationList[processIndex];
                float speedRate = 1.0f;
                if (anim[curAnim] != null)
                {
                    anim[curAnim].wrapMode = WrapMode.Once;
                    if (abilityDutation != 0)
                    {
                        speedRate = anim[curAnim].length / abilityDutation;
                    }
                    anim[curAnim].speed = speedRate;
                    //if (curAnim == curPlayingAnimationName)
                    //{
                    //    anim.Play(curAnim);
                    //}
                    //else
                    //{
                    //    anim.CrossFade(curAnim, 0.2f);
                    //}
                    anim.Play(curAnim);
                    curPlayingAnimationName = curAnim;
                }
                if (fxCtrlRef != null && curCastAbility.ProcessEffectList.Count > processIndex && curCastAbility.ProcessEffectList[processIndex] != string.Empty)
                {
                    fxCtrlRef.DoAttackEffect(curCastAbility.ProcessEffectList[processIndex], abilityDutation, speedRate, Vector3.one, this.transform);
                }
            }
            else
            {
                stateMachine.Send((int)EventType.AbilityEnding);
            }
            ++processIndex;
        }
        if (curTarPos != Vector3.zero)
        {
            this.transform.position = curTarPos;
            this.transform.rotation = curQuaternion;
        }
    }
    /// <summary>
    /// 离开释放状态 by吴江
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    /// <param name="_event"></param>
    protected void ExitAbilityProcessState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        curTarPos = Vector3.zero;
        if (curCastAbility.ArrowID > 0)
        {
            GameCenter.sceneMng.C2C_AddBallisticCurve(curCastAbility);
        }
        this.transform.localPosition = Vector3.zero;
        this.transform.localEulerAngles = Vector3.zero;
        if (headTextCtrlRef != null)
        {
            headTextCtrlRef.textParent.SetActive(true);
        }
        abilityDutation = 0.0f;
        processIndex = 0;
    }

    /// <summary>
    /// 进入后摇状态 by吴江
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    /// <param name="_event"></param>
    protected void EnterAbilityEndingState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (fxCtrlRef != null)
        {
            fxCtrlRef.ClearAttckWholeTimeEffect();
        }
        CurEventType = EventType.AbilityEnding;
        if (curCastAbility == null)
        {
            stateMachine.Send((int)EventType.Idle);
            return;
        }
        if (curCastAbility.EndingDuration == 0)
        {
            StopCast();
            return;
        }
        abilityDutation = curCastAbility.EndingDuration;
        float speedRate = 1.0f;
        if (anim[curCastAbility.EndingAnimationName] != null)
        {
            anim[curCastAbility.EndingAnimationName].wrapMode = WrapMode.Once;
            if (abilityDutation != 0)
            {
                speedRate = anim[curCastAbility.EndingAnimationName].length / abilityDutation;
            }
            anim[curCastAbility.EndingAnimationName].speed = speedRate;
            anim.Play(curCastAbility.EndingAnimationName);
            //anim.CrossFade(curCastAbility.EndingAnimationName);
            curPlayingAnimationName = curCastAbility.EndingAnimationName;
        }
        if (fxCtrlRef != null && curCastAbility.EndingEffectName != string.Empty)
        {
            fxCtrlRef.DoAttackEffect(curCastAbility.EndingEffectName, abilityDutation, speedRate, Vector3.one, this.transform);
        }
        enterEndingTime = Time.time;
    }
    protected float enterEndingTime;
    /// <summary>
    /// 后摇状态更新 by吴江
    /// </summary>
    /// <param name="_curState"></param>
    protected void UpdateAbilityEndingState(fsm.State _curState)
    {
        if (abilityDutation <= Time.time - enterEndingTime)
        {
            StopCast();
            if (moveFSMRef != null)
            {
                if (moveFSMRef.isMoving)
                {
                    stateMachine.Send((int)EventType.Move);
                }
                else
                {

                    stateMachine.Send((int)EventType.Idle);
                }
            }
            else
            {

                stateMachine.Send((int)EventType.Idle);
            }
        }
    }
    /// <summary>
    /// 离开后摇状态 by吴江
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    /// <param name="_event"></param>
    protected void ExitAbilityEndingState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        abilityDutation = 0;
    }




    #endregion
    #endregion




}
