//==============================================
//作者：邓成
//日期：2016/7/11
//用途：玩家自动运镖的AI状态机
//==============================================

using UnityEngine;
using System.Collections;

public class PlayerAutoDartFSM : FSMBase {

	public static System.Action<bool> OnAutoDartStateUpdate;
	protected MainPlayer thisPlayer = null;
	public CommandMng commandMng;

	public Monster CurDart = null;
	public MonsterInfo CurMonsterInfo = null;
	/// </summary>
	/// 跟随距离
	/// </summary>
	public float followRange = 4.0f;

	/// <summary>
	/// 状态枚举 by吴江
	/// </summary>
	public enum EventType
	{
		AWAKE = fsm.Event.USER_FIELD + 1,
		FINDDART,//寻找镖车
		AUTODART,//托管
		SELF_CTRL,
	}

	protected override void InitStateMachine ()
	{
		base.InitStateMachine ();
		fsm.State awake = new fsm.State("awake", stateMachine);
		awake.onEnter += EnterAwakeState;
		awake.onExit += ExitAwakeState;
		awake.onAction += UpdateAwakeState;

		fsm.State findDart = new fsm.State("findDart", stateMachine);
		findDart.onEnter += EnterFindDartState;
		findDart.onExit += ExitFindDartState;
		findDart.onAction += UpdateFindDartState;

		fsm.State autoDart = new fsm.State("autoDart", stateMachine);
		autoDart.onEnter += EnterAutoDartState;
		autoDart.onExit += ExitAutoDartState;
		autoDart.onAction += UpdateAutoDartState;

		fsm.State selfCtrl = new fsm.State("selfCtrl", stateMachine);
		selfCtrl.onEnter += EnterSelfCtrlState;
		selfCtrl.onExit += ExitSelfCtrlState;
		selfCtrl.onAction += UpdateSelfCtrlState;

		stateMachine.onStop = OnStop;
		stateMachine.onStart = OnStart;

		awake.Add<fsm.EventTransition>(findDart,(int)EventType.FINDDART);
		awake.Add<fsm.EventTransition>(selfCtrl, (int)EventType.SELF_CTRL);

		findDart.Add<fsm.EventTransition>(autoDart,(int)EventType.AUTODART);
		findDart.Add<fsm.EventTransition>(selfCtrl, (int)EventType.SELF_CTRL);

		autoDart.Add<fsm.EventTransition>(selfCtrl, (int)EventType.SELF_CTRL);
		autoDart.Add<fsm.EventTransition>(findDart, (int)EventType.FINDDART);

		selfCtrl.Add<fsm.EventTransition>(autoDart,(int)EventType.AUTODART);
		selfCtrl.Add<fsm.EventTransition>(findDart,(int)EventType.FINDDART);

		stateMachine.initState = awake;
	}
	void Update()
	{
		stateMachine.Update();
	}
	public void StartStateMachine()
	{
		if (stateMachine != null)
		{
			stateMachine.Start();
			stateMachine.Send((int)EventType.AWAKE);
		}
	}
	public void StopStateMachine()
	{
		if (stateMachine != null)
			stateMachine.Stop();
	}
	protected void OnStop()
	{
		SceneMng.OnDelInterObj -= OnDelInterObj;
		if(OnAutoDartStateUpdate != null)
			OnAutoDartStateUpdate(false);
	}
	protected void OnStart()
	{
		SceneMng.OnDelInterObj += OnDelInterObj;
		if(OnAutoDartStateUpdate != null)
			OnAutoDartStateUpdate(true);
	}
	protected void OnDelInterObj(ObjectType _type,int _id)
	{
		if(_type == ObjectType.MOB)
		{
			MonsterInfo info = GameCenter.sceneMng.GetMyDartInfo();
			if(info == null)
				StopStateMachine();
		}
	}

	/// <summary>
	/// 跳转到临时自主操控
	/// </summary>
	public void GoSelfCtrl()
	{
		stateMachine.Send((int)EventType.SELF_CTRL);
	}
	#region 启动部分
	protected virtual void EnterAwakeState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
		if (thisPlayer == null)
		{
			thisPlayer = this.gameObject.GetComponent<MainPlayer>();
		}
		if (thisPlayer == null)
		{
			Debug.LogError("找不到主玩家组件！");
		}
		else
		{
			commandMng = thisPlayer.commandMng;
		}
	}


	protected virtual void UpdateAwakeState(fsm.State _curState)
	{
		//如果一切就绪，那么跳转到运镖状态
		if (thisPlayer != null && !thisPlayer.isDummy && GameCenter.curGameStage != null && GameCenter.sceneMng != null)
		{
			if(stateMachine != null)
				stateMachine.Send((int)EventType.FINDDART);
		}
	}

	protected virtual void ExitAwakeState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
	}
	#endregion

	#region 寻找运镖
	protected virtual void EnterFindDartState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
		MonsterInfo dartInfo = GameCenter.sceneMng.GetMyDartInfo();
		if(dartInfo != null)
		{
			Monster dart = GameCenter.curGameStage.GetMOB(dartInfo.ServerInstanceID);
			if(dart != null)
			{
				CurDart = dart;
			}else//可能因为视野切割看不见镖车
			{
				GameCenter.activityMng.C2S_ReqDartPos(dartInfo.RankLevel == MobRankLevel.DAILYDART?DartType.DailyDart:DartType.GuildDart);
			}
		}else
		{
			StopStateMachine();
		}
	}


	protected virtual void UpdateFindDartState(fsm.State _curState)
	{
//		if(!commandMng.HasCommand())
//		{
		if(CurDart != null)
		{
			commandMng.CancelCommands();
			if(stateMachine != null)
				stateMachine.Send((int)EventType.AUTODART);
		}else
		{
			if(Time.frameCount % 1000 == 0)
			{
				GameCenter.activityMng.C2S_ReqDartPos(CurMonsterInfo.RankLevel == MobRankLevel.DAILYDART?DartType.DailyDart:DartType.GuildDart);
			}
		}
//		}
	}

	protected virtual void ExitFindDartState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
	}
	#endregion

	#region 自动运镖
	protected virtual void EnterAutoDartState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
		if(CurDart != null)
		{
			Command_TraceTarget traceTarget = new Command_TraceTarget();
			traceTarget.target = CurDart;
			traceTarget.updatePathDeltaTime = 1.0f;
			commandMng.PushCommand(traceTarget);
		}else
		{
			if(stateMachine != null)
				stateMachine.Send((int)EventType.FINDDART);
		}
	}


	protected virtual void UpdateAutoDartState(fsm.State _curState)
	{
		if(!commandMng.HasCommand() && CurDart != null)
		{
			float sqr = (thisPlayer.transform.position - CurDart.transform.position).sqrMagnitude;
			if (sqr >= followRange * followRange)
			{
				Command_MoveTo movetoCMD = new Command_MoveTo();
				movetoCMD.destPos = Utils.GetRandomPos(CurDart.transform);
				movetoCMD.maxDistance = 2;
				thisPlayer.commandMng.PushCommand(movetoCMD);
			}
		}
	}

	protected virtual void ExitAutoDartState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
	}
	#endregion

	#region 玩家临时自主操作状态 
	protected virtual void EnterSelfCtrlState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
	}


	protected virtual void UpdateSelfCtrlState(fsm.State _curState)
	{
		if (thisPlayer != null && !thisPlayer.commandMng.HasCommand() && !PlayerInputListener.isDragingRockerItem)//如果自主命令都已经执行完毕，重新回到战斗
		{
			if(stateMachine != null)
				stateMachine.Send((int)EventType.AUTODART);
		}
	}

	protected virtual void ExitSelfCtrlState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{

	}	
	#endregion
}
