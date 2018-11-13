/// <summary>
/// 创建人物角色动画状态机
/// 
/// 
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatePlayerAnimFSM : PlayerAnimFSM {

    protected float time = 0;
    protected new float duration = 0;
	protected string animName = string.Empty;
	protected string effectName = string.Empty;
	private FXCtrl fxCtrl = null;

    protected List<int> shakeTimeList = new List<int>();
    protected System.Action OnShakeEvent;
    protected System.Action OnFinish;

    public void DoPreviewAnim(AbilityInstance curCastAbility, FXCtrl _fxCtrl, System.Action _shake, System.Action _finish)
	{
		animName = curCastAbility.ProcessAnimationNameList.Count > 0?curCastAbility.ProcessAnimationNameList[0]:string.Empty;
		effectName = curCastAbility.ProcessEffectList.Count > 0?curCastAbility.ProcessEffectList[0]:string.Empty;
        shakeTimeList = new List<int>(curCastAbility.ProcessShakeTimeList);//需要新构造一个,否则会被Remove到长度0
		fxCtrl = _fxCtrl;
        OnShakeEvent = _shake;
        OnFinish = _finish;
		//Debug.Log("animName:"+animName+",effectName:"+effectName);
		stateMachine.Send((int)EventType.SelectedEnd);
	}

    protected override void InitStateMachine()
    {
        base.InitStateMachine();

        fsm.State select = new fsm.State("select", basicState);
        select.onEnter += EnterSelectedState;
        select.onAction += UpdateSelectedState;
        select.onExit += ExitSelectedState;

        select.Add<fsm.EventTransition>(idle, (int)EventType.Idle);
        idle.Add<fsm.EventTransition>(select, (int)EventType.SelectedEnd);
    }

    #region 选中后摇
    void EnterSelectedState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
		abilityDutation = 1.0f;
		float speedRate = 1.0f;
		if (anim[animName] != null)
		{
			anim[animName].wrapMode = WrapMode.Once;
			anim[animName].speed = speedRate;
			abilityDutation = anim[animName].length;
			anim.Play(animName);
		}
		if (fxCtrl != null)
		{
			fxCtrl.DoAttackEffect(effectName, abilityDutation, speedRate, Vector3.one,this.transform);
		}

        time = Time.time;
    }

    void UpdateSelectedState(fsm.State _curState)
    {
		if(Time.time - time >= abilityDutation)
		{
			stateMachine.Send((int)EventType.Idle);
		}
        int dicTime = (int)((Time.time - time) * 1000);
        if (CheckShake(dicTime))
        {
            if (OnShakeEvent != null) OnShakeEvent();
        }
    }

    void ExitSelectedState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (OnFinish != null) OnFinish();
    }
    /// <summary>
    /// 到达时间点则震屏,允许误差0.05s  by邓成
    /// </summary>
    bool CheckShake(int _dicTime)
    {
        for (int i = 0,length=shakeTimeList.Count; i < length; i++)
        {
            if (Mathf.Abs(shakeTimeList[i] - _dicTime) < 50)
            {
                shakeTimeList.RemoveAt(i);
                return true;
            }
        }
        return false;
    }
    #endregion

}
