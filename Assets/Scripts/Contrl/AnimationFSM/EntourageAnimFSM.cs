///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2016/2/29
//用途：生物动画状态机
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// 生物动画状态机
/// </summary>
public class EntourageAnimFSM : SmartActorAnimFSM
{

    protected string startFightName = string.Empty;
    protected float startFightDuration = 0;

    #region 外部调用接口
    #region 设置
    /// <summary>
    /// 设置稍息动作名称
    /// </summary>
    /// <param name="_randIdleName"></param>
    public void SetStartFightAnimationName(string _name, AnimationClip _animClip, bool _replace = false)
    {
        AnimationState state = GetAnimationState(_name, _animClip, _replace);
        if (state)
        {
            state.layer = 0;
            state.wrapMode = WrapMode.Once;
        }
        startFightName = _name;
    }
    #endregion
    #region 执行
    public void StartFight()
    {
        stateMachine.Send((int)EventType.StartFight);
    }
    #endregion
    #endregion


    #region 状态机
    protected fsm.State startCombat;

    protected override void InitStateMachine()
    {
        base.InitStateMachine();

        startCombat = new fsm.State("startCombat", basicState);
        startCombat.onEnter += EnterStartFightState;
        startCombat.onExit += ExitStartFightState;
        startCombat.onAction += UpdateStartFightState;


        idle.Add<fsm.EventTransition>(startCombat, (int)EventType.StartFight);
        durative.Add<fsm.EventTransition>(startCombat, (int)EventType.StartFight); ;
        stopDurative.Add<fsm.EventTransition>(startCombat, (int)EventType.StartFight);
        switchCombat.Add<fsm.EventTransition>(startCombat, (int)EventType.StartFight);
        randIdle.Add<fsm.EventTransition>(startCombat, (int)EventType.StartFight);
        move.Add<fsm.EventTransition>(startCombat, (int)EventType.StartFight);
        dead.Add<fsm.EventTransition>(startCombat, (int)EventType.StartFight);

        startCombat.Add<fsm.EventTransition>(idle, (int)EventType.Idle);

    }


    void EnterStartFightState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (anim[startFightName] != null)
        {
            startFightDuration = anim[startFightName].length;
            anim.Rewind(startFightName);
            anim.CrossFade(startFightName, 0.1f);
        }
        else
        {
            stateMachine.Send((int)EventType.Idle);
        }
    }

    void ExitStartFightState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {

    }

    void UpdateStartFightState(fsm.State _curState)
    {
        startFightDuration -= Time.deltaTime;
        if (startFightDuration <= 0)
        {
            stateMachine.Send((int)EventType.Idle);
        }
    }

    #endregion

}
