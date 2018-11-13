//====================================================
//作者:吴江
//日期:2015/2/5
//用途:NPC的动画控制组件
//=======================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// NPC的动画控制组件
/// </summary>
public class NPCAnimFSM : SmartActorAnimFSM
{


    protected string curBeckonName = "Beckon";
    protected string curCowerName = "Cower";
    protected string curRandomIdleName = "RandomIdle";
    private bool hasCower = false;
    private bool hasBeckon = false;
    private bool hasWalk = false;
    protected bool hasRun = false;
    protected bool hasIdle = false;

    private EventType curStatus_ = EventType.Idle;
    public EventType curStatus
    {
        get { return curStatus_; }
    }



    protected override void InitStateMachine()
    {
        base.InitStateMachine();

        fsm.State cower = new fsm.State("cower", stateMachine);
        cower.onEnter += EnterCowerState;
        cower.onAction += UpdateCowerState;

        fsm.State backOn = new fsm.State("beckon", stateMachine);
        backOn.onEnter += EnterBeckonState;
        backOn.onAction += UpdateBeckonState;



 
        idle.Add<fsm.EventTransition>(cower, (int)EventType.Cower);
        idle.Add<fsm.EventTransition>(backOn, (int)EventType.Beckon);

        cower.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        cower.Add<fsm.EventTransition>(backOn, (int)EventType.Beckon);
        cower.Add<fsm.EventTransition>(move, (int)EventType.Move);

        backOn.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        backOn.Add<fsm.EventTransition>(cower, (int)EventType.Cower);
        backOn.Add<fsm.EventTransition>(move, (int)EventType.Move);

        move.Add<fsm.EventTransition>(backOn, (int)EventType.Beckon);
        move.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        move.Add<fsm.EventTransition>(cower, (int)EventType.Cower);

        randIdle.Add<fsm.EventTransition>(cower, (int)EventType.Cower);
        randIdle.Add<fsm.EventTransition>(backOn, (int)EventType.Beckon);


    }



    void Update()
    {
        if (stateMachine != null)
            stateMachine.Update();
    }


    public void SetAnimationName(string _idleName, string _runName, string _beckonName, string _cowerName, string _randomIdleName, string _walkName)
    {
        hasCower = null != anim.GetClip(_cowerName);
        hasIdle = null != anim.GetClip(_idleName);
        hasBeckon = null != anim.GetClip(_beckonName);
        hasRun = null != anim.GetClip(_runName);
        hasRamdomIdle = null != anim.GetClip(_randomIdleName);
        hasWalk = null != anim.GetClip(_walkName);

        curIdleName = _idleName;
        if (hasRun)
        {
            curMoveName = _runName;
        }
        else if (hasWalk)
        {
            curMoveName = _walkName;
        }
        curBeckonName = _beckonName;
        curCowerName = _cowerName;
        curRandomIdleName = _randomIdleName;
    }




    //表现害怕的动作
    public void Cower()
    {
        if (!isMoving)
            stateMachine.Send((int)NPCAnimFSM.EventType.Cower);
    }

    //表现招手的动作
    public void BeckOn()
    {
        if (!isMoving)
            stateMachine.Send((int)NPCAnimFSM.EventType.Beckon);
    }







    void EnterBeckonState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        timer = Time.time;
        curStatus_ = EventType.Beckon; ;
        if (hasBeckon)
        {
            duration = anim.GetClip(curBeckonName).length;
            anim.CrossFade(curBeckonName);
        }
    }

    void UpdateBeckonState(fsm.State _curState)
    {
        if ((Time.time - timer) > duration)
        {
            stateMachine.Send((int)NPCAnimFSM.EventType.Idle);
        }
    }



    void EnterCowerState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        timer = Time.time;
        curStatus_ = EventType.Cower;
        //Debug.Log("进入恐惧动画");
        if (!hasCower) return;
        duration = anim.GetClip(curCowerName).length;
        anim.CrossFade(curCowerName);
    }

    void UpdateCowerState(fsm.State _curState)
    {
        if (Time.time - timer > duration)
        {
            stateMachine.Send((int)NPCAnimFSM.EventType.Idle);
        }
    }


}
