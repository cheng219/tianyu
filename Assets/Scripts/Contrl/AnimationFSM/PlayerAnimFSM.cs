///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/5/29
//用途:人物动画控制基类
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 人物动画控制基类
/// </summary>
public class PlayerAnimFSM : SmartActorAnimFSM
{
    #region 数据
    #endregion

    #region 构造和初始化
    protected override void InitStateMachine()
    {
        base.InitStateMachine();
        fsm.State fightWin = new fsm.State("fightWin", basicState);
        fightWin.onEnter += EnterFightWinState;
        fightWin.onAction += UpdateFightWinState;

        fsm.State fightLose = new fsm.State("fightLose", basicState);
        fightLose.onEnter += EnterFightLoseState;
        fightLose.onAction += UpdateFightLoseState;

        fsm.State jump = new fsm.State("jump", basicState);
        jump.onEnter += EnterJumpState;
        jump.onAction += UpdateJumpState;
        jump.onExit += ExitJumpState;


        durative = new fsm.State("magic", basicState);
        durative.onEnter += EnterMagicState;
        durative.onExit += ExitMagicState;

        idle.Add<fsm.EventTransition>(fightWin, (int)EventType.FightWin);
        idle.Add<fsm.EventTransition>(fightLose, (int)EventType.FightLose);

        idle.Add<fsm.EventTransition>(jump, (int)EventType.Jump);

        move.Add<fsm.EventTransition>(jump, (int)EventType.Jump);
        switchCombat.Add<fsm.EventTransition>(jump, (int)EventType.Jump);
        abilityEnding.Add<fsm.EventTransition>(jump, (int)EventType.Jump);

        jump.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        jump.Add<fsm.EventTransition>(move, (int)EventType.Move);
        jump.Add<fsm.EventTransition>(switchCombat, (int)EventType.SwitchToCombat);
        jump.Add<fsm.EventTransition>(abilityPrepare, (int)EventType.AbilityPrepare);
        jump.Add<fsm.EventTransition>(durative, (int)EventType.Durative);





        fightLose.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        fightWin.Add<fsm.EventTransition>(idle, (int)EventType.Idle);

        durative.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        durative.Add<fsm.EventTransition>(dead, (int)EventType.Dead);
        durative.Add<fsm.EventTransition>(move, (int)EventType.Move);
        durative.Add<fsm.EventTransition>(injury, (int)EventType.Injury);
        durative.Add<fsm.EventTransition>(randIdle, (int)EventType.RndIdle);
        durative.Add<fsm.EventTransition>(switchCombat, (int)EventType.SwitchToCombat);
        durative.Add<fsm.EventTransition>(mountMove, (int)EventType.MountMove);
        durative.Add<fsm.EventTransition>(mountIdle, (int)EventType.MountIdle);
        durative.Add<fsm.EventTransition>(abilityPrepare, (int)EventType.AbilityPrepare);
    }

    #endregion

    #region 外部控制调用接口
    /// <summary>
    /// 跳跃 by吴江
    /// </summary>
    public void Jump()
    {
        stateMachine.Send((int)EventType.Jump);
    }


    #endregion

    #region 状态机方法
    #region 战斗胜利 
    float winDuration = 0;
    float winStartTime;
    void EnterFightWinState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        CurEventType = EventType.FightWin;
        AnimationState clip = anim["fete"];
        if (clip)
        {
            winStartTime = Time.time;
            winDuration = clip.length;
            anim.CrossFade("fete");
        }
        else
        {
            stateMachine.Send((int)EventType.Idle);
        }
    }

    void ExitFightWinState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
    }

    void UpdateFightWinState(fsm.State _curState)
    {
        if (Time.time - winStartTime >= winDuration)
        {
            stateMachine.Send((int)EventType.Idle);
        }
    }
    #endregion
    #region 战斗失败
    float loseDuration = 0;
    float loseStartTime;
    protected void EnterFightLoseState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        CurEventType = EventType.FightLose;
        AnimationState clip = anim["lose"];
        if (clip)
        {
            loseStartTime = Time.time;
            loseDuration = clip.length;
            anim.CrossFade("lose");
        }
        else
        {
            stateMachine.Send((int)EventType.Idle);
        }
    }

    protected void ExitFightLoseState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
    }

    protected void UpdateFightLoseState(fsm.State _curState)
    {
        if (Time.time - loseStartTime >= loseDuration)
        {
            stateMachine.Send((int)EventType.Idle);
        }
    }
    #endregion
    #region 跳跃
    float jumpDuration = 0;
    float jumpStartTime;
    protected void EnterJumpState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        CurEventType = EventType.Jump;
        isCasting = true;
        AnimationState clip = anim["jump"];
        if (clip)
        {
            clip.wrapMode = WrapMode.Once;
            jumpStartTime = Time.time;
            jumpDuration = clip.length;
            anim.CrossFade("jump");
        }
        else
        {
            stateMachine.Send((int)EventType.Idle);
        }
    }

    protected void ExitJumpState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
    }

    protected void UpdateJumpState(fsm.State _curState)
    {
        if (Time.time - jumpStartTime >= jumpDuration)
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
    }
    #endregion
    #region 采集物品
    protected void EnterMagicState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
        if (anim["magic01"] != null)
        {
            anim.CrossFade("magic01");
            anim["magic01"].wrapMode = WrapMode.Loop;
        }
	}
    protected void ExitMagicState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{

    }
    #endregion
    #endregion
}
