///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/10/30
//用途：坐骑的动画控制器
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MountAnimFSM : ActorAnimFSM
{
    protected override void InitStateMachine()
    {
        stateMachine.mode = fsm.State.Mode.Parallel;

        fsm.State basicState = new fsm.State("basicState", stateMachine);

        fsm.State idle = new fsm.State("idle1", basicState);
        idle.onEnter += EnterIdleState;
        idle.onAction += UpdateIdleState;

        fsm.State move = new fsm.State("move", basicState);
        move.onEnter += EnterMoveState;
        move.onExit += ExitMoveState;


        idle.Add<fsm.EventTransition>(move, (int)EventType.Move);
		


        move.Add<fsm.EventTransition>(idle, (int)EventType.StopMoving);

    }





    protected override void EnterIdleState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (_from.name == "castNoMove")
        {
            anim.CrossFade(curIdleName, 0.1f);
        }
        else
        {
            anim.CrossFade(curIdleName, 0.3f);
        }
       // timer = Time.time + UnityEngine.Random.Range(5f, 48f);
    }


    protected override void UpdateIdleState(fsm.State _curState)
    {
    }






    protected override void EnterMoveState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        anim.Rewind(curMoveName);
        float fadeDuraton = 0.3f;
        anim.CrossFade(curMoveName, fadeDuraton);
    }

    protected override void ExitMoveState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
    }

}
