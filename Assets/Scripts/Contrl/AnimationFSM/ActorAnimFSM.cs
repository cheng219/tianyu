///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/5/13
//用途：动画状态机基类
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class ActorAnimFSM : FSMBase
{

    #region 数据
    public enum EventType {
        Idle = fsm.Event.USER_FIELD + 1,
        RndIdle,
        Move,
        StopMoving,
        Jump,
        SwitchToCombat,
        SwitchToExplore,
        Injury,
        InjuryFinish,
        InjuryFlyPrepare,
        InjuryFlyProcess,
        InjuryFlyEnd,
        BlockBreak,
        BlockBreakFinish,
        Dead,
        TurnLeft,
        TurnRight,
        MountMove,
        MountIdle,
        /// <summary>
        /// buff持续动作
        /// </summary>
		Durative,//
        /// <summary>
        /// 结束buff持续动作
        /// </summary>
        StopDurative,
        FightWin,
        FightLose,
        Create,
        Cower,
        Beckon,
        Beckoning,
		OriginIdle,
		SelectedShake,
		SelectedProcess,
        SelectedEnd,
        CancelSelectedShake,
        CancelSelectedProcess,
        CancelSelectedEnd,
		NotSelected,
        Surprised,
		ShowIdle,//
		DoNotSelected,//
        ShowPose,
        SelectEndRun,
		Movie,//展示动作 用于公会龙
        /// <summary>
        /// 技能前摇 by吴江
        /// </summary>
        AbilityPrepare,
        /// <summary>
        /// 技能过程 by吴江
        /// </summary>
        AbilityProcess,
        /// <summary>
        /// 技能后摇 by吴江
        /// </summary>
        AbilityEnding,
        /// <summary>
        /// 技能结束 by吴江
        /// </summary>
        //StopCast,
        /// <summary>
        /// 出战动作
        /// </summary>
        StartFight,
        /// <summary>
        /// 采集动作
        /// </summary>
        Collect,
    }


    /// <summary>
    /// 暂停动画的对象
    /// </summary>
    public class PausedAnimState {
        public AnimationState animState = null;
        protected float resumeSpeed = 1.0f;

        public PausedAnimState ( AnimationState _animState ) {
            animState = _animState;
            resumeSpeed = _animState.speed;
        }

        public void Pause () { 
            if ( animState )
                animState.speed = 0.0f; 
        }
        public void Resume () { 
            if ( animState )
                animState.speed = resumeSpeed; 
        }
    }


    [System.NonSerialized] public Transform upperBody;


    protected bool paused = false;
    protected List<PausedAnimState> pausedAnimStates = new List<PausedAnimState>();

    protected string curIdleName = "idle2";
    protected string normalIdleName = "idle2";
    protected string combatIdleName = "idle2";
    protected string curMoveName = "move2";
    protected string normalMoveName = "move1";
    protected string combatMoveName = "move2";
    protected string injuredDonwName = "kickdown";
    protected string injuredUpName = "getup";
    protected string injuredFlyDonwName = "kickfly_03";
    protected string injuredFlyProcessName = "kickfly_02";
    protected string injuredFlyUpName = "kickfly_01";
    protected string deadName = "die";

    protected bool isInCombat = false;

	protected string actioningAnim = "use";
	protected string actionedAnim = "used";
    
    protected string curBlendingToStopName = "";
    protected Animation anim = null;
    private List<AnimationClip> unloadAnimationClips = new List<AnimationClip>();
    #endregion

    #region UNITY
    protected new void Awake () {
        base.Awake();
        anim = gameObject.GetComponentInChildrenFast<Animation>();
    }


    void LateUpdate()
    {
        if (stateMachine != null)
            stateMachine.Update();
    }
    #endregion

    #region 设置
    /// <summary>
    /// 设置死亡 动作 by吴江
    /// </summary>
    /// <param name="_combat"></param>
    public void SetupDeadAnimationName(string _deadName)
    {
        deadName = _deadName;
    }
    /// <summary>
    /// 设置站立动作和移动动作 by吴江
    /// </summary>
    /// <param name="_combat"></param>
    public void SetupIdleAndMoveAnimationName(string _idleName, string _moveName)
    {
        normalIdleName = _idleName;
        curIdleName = isInCombat ? combatIdleName : normalIdleName;
        SetupIdleAnimation(curIdleName, null);

        normalMoveName = _moveName;
        curMoveName = isInCombat ? combatMoveName : normalMoveName;
        SetupMoveAnimation(curMoveName, null);
    }

    /// <summary>
    /// 设置战斗站立动作 by吴江
    /// </summary>
    /// <param name="_combat"></param>
    public void SetupCombatAnimationName(string _combat, AnimationClip _animClip, bool _replace = false)
    {
        combatIdleName = _combat;
        AnimationState state = GetAnimationState(combatIdleName, _animClip, _replace);
        if (state)
        {
            state.layer = 0;
            state.wrapMode = WrapMode.Loop;
        }
        curIdleName = isInCombat ? combatIdleName : normalIdleName;
    }

    /// <summary>
    /// 设置战斗站立动作 by吴江
    /// </summary>
    /// <param name="_combat"></param>
    public void SetupCombatMoveAnimationName(string _combat, AnimationClip _animClip, bool _replace = false)
    {
        combatMoveName = _combat;
        AnimationState state = GetAnimationState(combatIdleName, _animClip, _replace);
        if (state)
        {
            state.layer = 0;
            state.wrapMode = WrapMode.Loop;
        }
        curMoveName = isInCombat ? combatMoveName : normalMoveName;
    }

    /// <summary>
    /// 设置立正动作 by吴江
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_animClip"></param>
    /// <param name="_replace"></param>
    public void SetupIdleAnimation(string _name, AnimationClip _animClip, bool _replace = false)
    {
        AnimationState state = GetAnimationState(_name, _animClip, _replace);
        if (state)
        {
            state.layer = 0;
            state.wrapMode = WrapMode.Loop;
        }
        curIdleName = _name;
    }
    /// <summary>
    /// 设置奔跑动作 by吴江
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_animClip"></param>
    /// <param name="_replace"></param>
    public void SetupMoveAnimation(string _name, AnimationClip _animClip, bool _replace = false)
    {
        AnimationState state = GetAnimationState(_name, _animClip, _replace);
        if (state)
        {
            state.layer = 0;
            state.wrapMode = WrapMode.Loop;
        }
        curMoveName = _name;
    }
    /// <summary>
    /// 设置受伤倒地动作 by吴江
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_animClip"></param>
    /// <param name="_replace"></param>
    public void SetupInjuryAnimation(string _name, AnimationClip _animClip, bool _replace)
    {
        AnimationState state = GetAnimationState(_name, _animClip, _replace);
        if (state)
        {
            injuredDonwName = _name;
            //state.layer = 3;
            state.wrapMode = WrapMode.Once;
            state.blendMode = AnimationBlendMode.Blend;
        }
    }
    /// <summary>
    /// 设置起身动作 by吴江
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_animClip"></param>
    /// <param name="_replace"></param>
    public void SetupInjuryUpAnimation(string _name, AnimationClip _animClip, bool _replace)
    {
        AnimationState state = GetAnimationState(_name, _animClip, _replace);
        if (state)
        {
            injuredUpName = _name;
            //state.layer = 3;
            state.wrapMode = WrapMode.Once;
            state.blendMode = AnimationBlendMode.Blend;
        }
    }
    /// <summary>
    /// 设置技能动作 by吴江
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_animClip"></param>
    /// <param name="_replace"></param>
    public void SetupAbilityAnimation(string _name, AnimationClip _animClip, bool _replace = false)
    {
        AnimationState state = GetAnimationState(_name, _animClip, _replace);
        if (state)
        {
            state.layer = 2;
            state.wrapMode = WrapMode.ClampForever;
            // state.AddMixingTransform (upperBody);
        }
    }
    /// <summary>
    /// 设置死亡动作 by吴江
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_animClip"></param>
    /// <param name="_replace"></param>
    public void SetupDeathAnimation(string _name, AnimationClip _animClip, bool _replace = false)
    {
        AnimationState state = GetAnimationState(_name, _animClip, _replace);
        if (state)
        {
            state.layer = 5;
            state.wrapMode = WrapMode.ClampForever;
        }
    }
    /// <summary>
    /// 设置上半身受伤动作 by吴江
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_animClip"></param>
    public void SetupInjuryLiteAnimation(string _name, AnimationClip _animClip)
    {
        AnimationState state = GetAnimationState(_name, _animClip);
        if (state)
        {
            state.layer = 3;
            state.wrapMode = WrapMode.ClampForever;
            state.AddMixingTransform(upperBody);
        }
    }
    #endregion

    #region FSM
    protected fsm.State basicState;
    protected fsm.State idle;
    protected fsm.State randIdle;
    protected fsm.State move;


    protected override void InitStateMachine()
    {
        stateMachine.mode = fsm.State.Mode.Parallel;

        basicState = new fsm.State("basicState", stateMachine);

        idle = new fsm.State("idle1", basicState);
        idle.onEnter += EnterIdleState;
        idle.onAction += UpdateIdleState;
        idle.onExit += ExitIdleState;

        randIdle = new fsm.State("randIdle", basicState);
        randIdle.onEnter += EnterRandIdleState;
        randIdle.onAction += UpdateRondomState;

        move = new fsm.State("move", basicState);
        move.onEnter += EnterMoveState;
        move.onExit += ExitMoveState;
    }



    public void StartStateMachine () {
        if ( stateMachine != null )
            stateMachine.Start();
    }



    //==========================立正=====================
    protected virtual void EnterIdleState(fsm.State _from, fsm.State _to, fsm.Event _event) { }

    protected virtual void UpdateIdleState(fsm.State _curState) { }

    protected virtual void ExitIdleState(fsm.State _from, fsm.State _to, fsm.Event _event) { }
    //===========================稍息==========================
    protected virtual void EnterRandIdleState(fsm.State _from, fsm.State _to, fsm.Event _event){}

    protected virtual void UpdateRondomState(fsm.State _curState) { }

    //==========================跑步===========================
    protected virtual void EnterMoveState(fsm.State _from, fsm.State _to, fsm.Event _event) {  }

    protected virtual void ExitMoveState(fsm.State _from, fsm.State _to, fsm.Event _event) { }
    #endregion


    public virtual void Stop () {
        StopAllCoroutines();
        CancelInvoke();

        // foreach ( AnimationState animState in anim ) {
        //     animState.enabled = false;
        //     animState.weight = 0.0f;
        // }
        anim.Stop ();

        if ( stateMachine != null )
            stateMachine.Stop();
    }
    

    /// <summary>
    /// 混入动画对象
    /// </summary>
    protected struct BlendToStopParams {
        public string name;
        public string cloneName;
        public float fadeLength;

        public BlendToStopParams ( string _name, string _cloneName, float _fadeLength ) {
            name = _name;
            cloneName = _cloneName;
            fadeLength = _fadeLength;
        } 
    }

    protected IEnumerator BlendToStop_CO ( BlendToStopParams _params ) {
        curBlendingToStopName = _params.name;
        if ( anim[_params.cloneName] != null )
            anim.Blend( _params.cloneName, 0.5f, _params.fadeLength );

        yield return new WaitForSeconds(_params.fadeLength);

        anim.Stop(_params.name);
        curBlendingToStopName = "";
    } 

    protected IEnumerator BlendToStopLayer_CO ( int _layer, float _fadeLength = 0.3f ) {
        AnimationState[] stateList = new AnimationState[anim.GetClipCount()];
        int cnt = 0;
        foreach ( AnimationState state in anim) {
            if ( state.layer == _layer ) {
                if ( state.enabled )
                    anim.Blend( state.name, 0.0f, _fadeLength );
                stateList[cnt] = state;
                ++cnt;
            }
        }
        yield return new WaitForSeconds(_fadeLength);
        for ( int i = 0; i < cnt; ++i ) {
            if ( stateList[i] != null )
                anim.Stop(stateList[i].name);
        }
    } 


    /// <summary>
    /// 混入指定名称和指定时间长度的动画
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_fadeLength"></param>
    public void BlendToStop ( string _name, float _fadeLength = 0.3f ) {
        StartCoroutine( "BlendToStop_CO", new BlendToStopParams(_name, _name, _fadeLength) );
    }

    /// <summary>
    /// 混入指定名称和指定时间长度的动画
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_fadeLength"></param>
    public void BlendToStop ( string _name, string _cloneName, float _fadeLength = 0.3f ) {
        StartCoroutine( "BlendToStop_CO", new BlendToStopParams(_name, _cloneName, _fadeLength) );
    }


    /// <summary>
    /// 暂停动画
    /// </summary>
    public void Pause () {
        if ( paused == false ) {
            paused = true;

            stateMachine.Pause();
            foreach ( AnimationState state in anim) {
                if ( state.enabled ) {
                    PausedAnimState pausedAnimState = new PausedAnimState(state);
                    pausedAnimState.Pause();
                    pausedAnimStates.Add( pausedAnimState );
                }
            }
        }
    }


    /// <summary>
    /// 取消动画暂停
    /// </summary>
    public void Resume () {
        if ( paused ) {
            paused = false;

            stateMachine.Resume();
            foreach ( PausedAnimState pausedAnimState in pausedAnimStates ) {
                pausedAnimState.Resume();
            }
            pausedAnimStates.Clear();
        }
    }


    /// <summary>
    /// 停止指定层的动画
    /// </summary>
    /// <param name="_layer">指定层</param>
    public void Stop ( int _layer ) {
        foreach ( AnimationState state in anim) {
            if ( state.layer == _layer ) {
                anim.Stop(state.name);
            }
        }
    }

    /// <summary>
    /// 进入站立动画
    /// </summary>
    public void Idle () {
        
        stateMachine.Send( (int)EventType.Idle ); 
    }

	public void Movie()
	{
		stateMachine.Send( (int)EventType.Movie); 
	}

    /// <summary>
    /// 开始移动动画
    /// </summary>
    public virtual void Move () {
        stateMachine.Send( (int)EventType.Move ); 
    }
    /// <summary>
    /// 停止移动动画
    /// </summary>
    public void StopMoving () {
        stateMachine.Send( (int)EventType.StopMoving ); 
    }
    
    /// <summary>
    /// 根据移动速度设置移动动画速率
    /// </summary>
    /// <param name="_speed"></param>
    public void SetMoveSpeed ( float _speed ) {
        AnimationState animState = CurrentMoveAnimationState();
        if ( animState )
            animState.speed = _speed; 
    }


    /// <summary>
    /// 修改动画
    /// </summary>
    /// <param name="_name">动画名称</param>
    /// <param name="_clip">动画曲线</param>
    /// <param name="_replace">是否替换</param>
    /// <returns></returns>
    protected AnimationState GetAnimationState ( string _name, AnimationClip _clip, bool _replace = false ) {
        if (anim == null) return null;
        if ( _replace ) {
            AnimationState state = anim[_name];
            if ( !_clip ) {
                return state;
            }
            if ( state ) {
                if ( state.clip == _clip )
                    return state;

                AnimationClip oldClip = state.clip;
                anim.RemoveClip ( _name );

                int idx = unloadAnimationClips.IndexOf(oldClip);
                if ( idx != -1 ) {
                    unloadAnimationClips.RemoveAt(idx);
                    Object.DestroyImmediate(oldClip);
                }
            }
        }
        else {
            AnimationState state = anim[_name];
            if ( state )
                return state;
            if ( !_clip )
                return null;
        }
        anim.AddClip ( _clip, _name );
        AnimationState newState = anim[_name];
        if ( !object.ReferenceEquals(_clip, newState.clip) ) {
            unloadAnimationClips.Add(newState.clip);
        }
        return newState;
    }

	protected void OnDestroy() {
		foreach ( AnimationClip animClip in unloadAnimationClips ) {
			Destroy(animClip);
		}
		unloadAnimationClips.Clear();
	}


    /// <summary>
    /// 获取当前的移动动画
    /// </summary>
    /// <returns></returns>
    protected AnimationState CurrentMoveAnimationState () { return anim[curMoveName]; }

    public void ReEnable()
    {
        anim.CrossFade(curIdleName, 0.2f);
    }
}
