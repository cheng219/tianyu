///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/5/26
//用途：怪物的动画控制器
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MobAnimFSM : SmartActorAnimFSM
{

    protected string createAnimName = "create";

    /// <summary>
    /// 设置出生动作名称
    /// </summary>
    /// <param name="_randIdleName"></param>
    public void SetCreateAnimationName(string _name, AnimationClip _animClip, bool _replace = false)
    {
        AnimationState state = GetAnimationState(_name, _animClip, _replace);
        if (state)
        {
            state.layer = 0;
            state.wrapMode = WrapMode.Once;
        }
        createAnimName = _name;
    }

    protected override void InitStateMachine()
    {
        base.InitStateMachine();
        stateMachine.mode = fsm.State.Mode.Parallel;

        fsm.State basicState = new fsm.State("basicState", stateMachine);

        fsm.State create = new fsm.State("create", basicState);
        create.onEnter += EnterCreateState;
        create.onExit += ExitCreateState;
        create.onAction += UpdateCreateState;

        idle.Add<fsm.EventTransition>(create, (int)EventType.Create);

        create.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        create.Add<fsm.EventTransition>(dead, (int)EventType.Dead);
        create.Add<fsm.EventTransition>(move, (int)EventType.Move);
        create.Add<fsm.EventTransition>(injury, (int)EventType.Injury);
        create.Add<fsm.EventTransition>(randIdle, (int)EventType.RndIdle);

    }
	


    public void Creating()
    {
        if (isMoving) return;
        stateMachine.Send((int)EventType.Create);
    }
	
    #region 出生 by吴江
    protected float createTimer = 0.0f;
    protected float createDuration = 0.0f;
    void EnterCreateState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (anim[createAnimName] != null)
        {
            createTimer = 0;
            createDuration = anim[createAnimName].length;
            anim[createAnimName].wrapMode = WrapMode.Once;
            anim.CrossFade(createAnimName);
        }
    }

    void UpdateCreateState(fsm.State _curState)
    {
        createTimer += Time.deltaTime;
        if (createTimer >= createDuration)
        {
            stateMachine.Send((int)EventType.Idle);
            return;
        }
    }

    void ExitCreateState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
    }
    #endregion
}
