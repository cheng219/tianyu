///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/8/26
//用途：动画状态机基类
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class SceneItemAnimFSM : ActorAnimFSM
{

    #region 数据
    public enum EventType {
        Normal = fsm.Event.USER_FIELD + 1,
        Appear,
        Disappear,
    }

    protected string deadAnimName = string.Empty;
    protected float deadAnimTime = 0.0f;
    #endregion

    #region UNITY
    protected new void Awake()
    {
        base.Awake();
        anim = gameObject.GetComponentInChildren<Animation>();
    }
    #endregion

    #region 外部接口
    /// <summary>
    /// 死亡
    /// </summary>
    public void Dead()
    {
        if (stateMachine != null)
        {
            stateMachine.Start();
            stateMachine.Send((int)EventType.Disappear);
        }
    }
    /// <summary>
    /// 设置死亡死亡动作
    /// </summary>
    /// <param name="_animName"></param>
    /// <param name="_time"></param>
    public void SetDeadAnim(string _animName, float _time)
    {
        deadAnimName = _animName;
        deadAnimTime = _time;
    }
    #endregion



    #region 状态机
    protected override void InitStateMachine () {
        base.InitStateMachine();
        stateMachine.mode = fsm.State.Mode.Parallel;

        fsm.State basicState = new fsm.State("basicState", stateMachine);

        fsm.State normal = new fsm.State("normal", basicState);
        normal.onEnter += EnterNormalState;

        fsm.State appear = new fsm.State("appear", basicState);
        appear.onEnter += EnterAppearState;


        fsm.State disappear = new fsm.State("disappear", basicState);
        disappear.onEnter += EnterDisappearState;
		
        normal.Add<fsm.EventTransition>(appear, (int)EventType.Appear);
        normal.Add<fsm.EventTransition>(disappear, (int)EventType.Disappear);		
    }



    void EnterNormalState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
		if(anim == null)
			anim = gameObject.GetComponentInChildren<Animation>();		
    //    anim.CrossFade(curIdleName, 0.5f);
    }


    void EnterAppearState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (anim == null)
        {
            anim = gameObject.GetComponentInChildren<Animation>();
        }
        anim.CrossFade("airwall_loop", 0.5f);
    }




    void EnterDisappearState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (anim == null)
        {
            anim = gameObject.GetComponentInChildren<Animation>();
        }
        if (anim == null || deadAnimName == string.Empty) return;
        if (anim[deadAnimName] != null)
        {
            if (deadAnimTime <= 0)
            {
                anim[deadAnimName].speed = 1000;
            }
            else
            {
                anim[deadAnimName].speed = anim[deadAnimName].length / deadAnimTime;
            }
            anim.CrossFade(deadAnimName, 0.1f);
        }
    }

    #endregion

}
