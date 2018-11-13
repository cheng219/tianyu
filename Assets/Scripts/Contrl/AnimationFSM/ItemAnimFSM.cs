///////////////////////////////////////////////////////////////////////////////
//作者：
//日期：2015/11/26
//用途：场景物品的动画控制器
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemAnimFSM : SmartActorAnimFSM {

	protected override void InitStateMachine ()
	{
		stateMachine.mode = fsm.State.Mode.Parallel;
		
		fsm.State basicState = new fsm.State("basicState", stateMachine);

		fsm.State idle = new fsm.State("idle1", basicState);
		idle.onEnter += EnterIdleState;
		idle.onAction += UpdateIdleState;

		fsm.State actioning = new fsm.State("actioning",basicState);
		actioning.onEnter += EnterActioningState;
		actioning.onExit += ExitActioningState;
		
		fsm.State actioned = new fsm.State("actioned",basicState);
		actioned.onEnter += EnterActionedState;
		actioned.onExit += ExitActionedState;

		actioning.Add<fsm.EventTransition>(idle,(int)EventType.Idle);

		actioned.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
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
	}


    protected override void UpdateIdleState(fsm.State _curState)
	{

	}
	void EnterActioningState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
		Debug.Log("EnterActioningState:"+actioningAnim);
		anim.Stop();
		anim.CrossFade(actioningAnim);
	}
	void ExitActioningState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
		
	}
	void EnterActionedState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
		Debug.Log("EnterActionedState:"+actionedAnim);
		anim.Stop();
		anim.CrossFade(actionedAnim);
	}
	void ExitActionedState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
		
	}
}
