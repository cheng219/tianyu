//==============================================
//作者：吴江
//日期：2015/9/14
//用途：玩家自动战斗的AI状态机
//==============================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/// <summary>
/// 玩家自动战斗的AI状态机 
/// </summary>
public class PlayerAutoFightFSM : FSMBase
{


	public static System.Action<bool> OnAutoFightStateUpdate;

	protected MainPlayer thisPlayer = null;
	protected SmartActor thisTarget = null;
	protected AbilityInstance lastNormalAbilityInstance = null;
	/// <summary>
	/// 是否正在空放普通攻击
	/// </summary>
	public bool IsUseLoseHitAbility
	{
		get
		{
			return false;
			//  if (animFSM == null) return false;
			// return animFSM.CurAbilityID == actorInfo.HitLoseAbilityID;
		}
	}

	protected float autoFightDistance = 15f;

	public CommandMng commandMng;

	protected Vector3 startFightPos = Vector3.zero;

	/// <summary>
	/// 状态枚举 by吴江
	/// </summary>
	public enum EventType
	{
		AWAKE = fsm.Event.USER_FIELD + 1,
		TARGET,
		FIGHT,
		AUTOFIGHT,//托管
		SELF_CTRL,
		COLLECT,
	}



	public void StartStateMachine()
	{
		if (stateMachine != null)
		{
			stateMachine.Start();
			stateMachine.Send((int)EventType.AWAKE);
		}
		if (thisPlayer != null)
		{
			startFightPos = thisPlayer.transform.position;
		}
	}



	public void StopStateMachine()
	{
		startFightPos = Vector3.zero;
		if (stateMachine != null)
			stateMachine.Stop();
	}

	protected override void InitStateMachine()
	{
		fsm.State awake = new fsm.State("awake", stateMachine);
		awake.onEnter += EnterAwakeState;
		awake.onExit += ExitAwakeState;
		awake.onAction += UpdateAwakeState;

		fsm.State target = new fsm.State("target", stateMachine);
		target.onEnter += EnterTargetState;
		target.onExit += ExitTargetState;
		target.onAction += UpdateTargetState;


		fsm.State fight = new fsm.State("fight", stateMachine);
		fight.onEnter += EnterFightState;
		fight.onExit += ExitFightState;
		fight.onAction += UpdateFightState;

		fsm.State collect = new fsm.State("collect", stateMachine);
		collect.onEnter += EnterCollectState;
		collect.onExit += ExitCollectState;
		collect.onAction += UpdateCollectState;

		fsm.State autoFight = new fsm.State("autoFight", stateMachine);
		autoFight.onEnter += EnterAutoFightState;
		autoFight.onExit += ExitAutoFightState;
		autoFight.onAction += UpdateAutoFightState;

		fsm.State selfCtrl = new fsm.State("selfCtrl", stateMachine);
		selfCtrl.onEnter += EnterSelfCtrlState;
		selfCtrl.onExit += ExitSelfCtrlState;
		selfCtrl.onAction += UpdateSelfCtrlState;

		stateMachine.onStop = OnStop;
        stateMachine.onStart = OnStart;


		//各状态之间的可跳转关系 by吴江
		awake.Add<fsm.EventTransition>(target, (int)EventType.TARGET);
		awake.Add<fsm.EventTransition>(selfCtrl, (int)EventType.SELF_CTRL);
		awake.Add<fsm.EventTransition>(fight, (int)EventType.FIGHT);
		awake.Add<fsm.EventTransition>(autoFight, (int)EventType.AUTOFIGHT);
		awake.Add<fsm.EventTransition>(collect, (int)EventType.COLLECT);

		target.Add<fsm.EventTransition>(fight, (int)EventType.FIGHT);
		target.Add<fsm.EventTransition>(autoFight, (int)EventType.AUTOFIGHT);
		target.Add<fsm.EventTransition>(selfCtrl, (int)EventType.SELF_CTRL);
		target.Add<fsm.EventTransition>(collect, (int)EventType.COLLECT);

		fight.Add<fsm.EventTransition>(target, (int)EventType.TARGET);
		fight.Add<fsm.EventTransition>(selfCtrl, (int)EventType.SELF_CTRL);
		fight.Add<fsm.EventTransition>(autoFight, (int)EventType.AUTOFIGHT);

		autoFight.Add<fsm.EventTransition>(target,(int)EventType.TARGET);
		autoFight.Add<fsm.EventTransition>(fight, (int)EventType.FIGHT);
		autoFight.Add<fsm.EventTransition>(selfCtrl, (int)EventType.SELF_CTRL);

		selfCtrl.Add<fsm.EventTransition>(target, (int)EventType.TARGET);
		selfCtrl.Add<fsm.EventTransition>(fight, (int)EventType.FIGHT);
		selfCtrl.Add<fsm.EventTransition>(autoFight, (int)EventType.AUTOFIGHT);
		selfCtrl.Add<fsm.EventTransition>(collect, (int)EventType.COLLECT);

		collect.Add<fsm.EventTransition>(selfCtrl, (int)EventType.SELF_CTRL);



		stateMachine.initState = awake;
	}


	void Update()
	{
		stateMachine.Update();
	}

	#region 启动部分 by吴江
	protected virtual void EnterAwakeState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
        curEventType = EventType.AWAKE;
		if(GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef != null)
			autoFightDistance = (float)GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef.autofight_distance;
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
			thisTarget = thisPlayer.CurTarget as SmartActor;
		}
		mainTaskStateType = CheckMainTaskStateType();
		startFightPos = thisPlayer.transform.position;
	}


	protected virtual void UpdateAwakeState(fsm.State _curState)
	{
		//如果一切就绪，那么跳转到锁怪状态 by吴江
		if (thisPlayer != null && !thisPlayer.isDummy && GameCenter.curGameStage != null && GameCenter.sceneMng != null)
		{
			if (CheckTask() && thisPlayer.AttakType == MainPlayer.AttackType.COMPLETE_TASK)
			{
				stateMachine.Send((int)EventType.COLLECT);
			}
			else
			{
				// thisPlayer.commandMng.CancelCommands();//终止之前的所有行为 by吴江
				if (thisTarget == null || !IsEnemy(thisTarget))
				{
					stateMachine.Send((int)EventType.TARGET);
				}
				else
				{
					switch (thisPlayer.AttakType)
					{
					case MainPlayer.AttackType.NONE:
						stateMachine.Stop();
						break;
					case MainPlayer.AttackType.NORMALFIGHT:
                        thisPlayer.GoNormal();
						break;
					case MainPlayer.AttackType.AUTOFIGHT:
					case MainPlayer.AttackType.COMPLETE_TASK:
						stateMachine.Send((int)EventType.AUTOFIGHT);
						break;
					default:
						break;
					}
				}
			}
		}
	}

	protected virtual void ExitAwakeState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
	}
	#endregion


	#region 锁怪部分 by吴江

	protected virtual void EnterTargetState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
        curEventType = EventType.TARGET;
		thisTarget = null;
		float distance = 0;
		curTarTaskInfo = GameCenter.taskMng.CurfocusTask;

		SceneRef sceneRef = GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef;
		if(sceneRef != null && sceneRef.pk_mode != 0)//强制玩家切换PK模式的场景里,才锁定玩家 by邓成
		{
			thisTarget = GameCenter.curGameStage.GetClosestPlayer(thisPlayer, RelationType.AUTOMATEDATTACKS, ref distance);
			if (distance > autoFightDistance)
			{
				thisTarget = null;
			}
		}else
		{
			thisTarget = GameCenter.curGameStage.GetClosestMob(thisPlayer, ref distance);
			if (distance > autoFightDistance)
			{
				thisTarget = null;
			}
			if (curTarTaskInfo != null && thisTarget != null)
			{
				Monster mob = thisTarget as Monster;
				if (mob == null)
				{
					thisTarget = null;
				}
//				else if (curTarTaskInfo.TargetMosterID != mob.ConfigID)   //这里需要TargetMosterID和ConfigID
//				{
//					thisTarget = null;
//				}
			}
            if (thisTarget != null)//自动战斗避开BOSS  BY黄洪兴
            {
                Monster mo = thisTarget as Monster;
                if (mo != null && GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef != null)
                {
                    if (GameCenter.systemSettingMng.IsHideBoss && mo.actorInfo.IsBoss && GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef.sort == SceneType.SCUFFLEFIELD)
                        thisTarget = null;

                }
            }
		}
		if (thisTarget == null)
		{
			ChangeTarget();
		}
	}


	protected virtual void UpdateTargetState(fsm.State _curState)
	{
		if (Time.frameCount % 10 == 0)
		{
			if (thisTarget != null)
			{
				thisPlayer.CurTarget = thisTarget;
				switch (thisPlayer.AttakType)
				{
				case MainPlayer.AttackType.NONE:
					stateMachine.Stop();
					break;
				case MainPlayer.AttackType.NORMALFIGHT:
					stateMachine.Send((int)EventType.FIGHT);
					break;
				case MainPlayer.AttackType.AUTOFIGHT:
				case MainPlayer.AttackType.COMPLETE_TASK:
					stateMachine.Send((int)EventType.AUTOFIGHT);
					break;
				default:
					break;
				}
			}
			else
			{
				ChangeTarget();
			}
		}
	}

	protected virtual void ExitTargetState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
	}
	#endregion


	#region 玩家临时自主操作状态 by吴江
	protected virtual void EnterSelfCtrlState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
        curEventType = EventType.SELF_CTRL;
	}


	protected virtual void UpdateSelfCtrlState(fsm.State _curState)
	{
		if (thisPlayer != null && !thisPlayer.commandMng.HasCommand() && !PlayerInputListener.isDragingRockerItem)//如果自主命令都已经执行完毕，重新回到战斗
		{
            startFightPos = thisPlayer.transform.position;
			switch (thisPlayer.AttakType)
			{
			case MainPlayer.AttackType.NONE:
				stateMachine.Stop();
				break;
			case MainPlayer.AttackType.NORMALFIGHT:
				thisPlayer.GoNormal();
				break;
			case MainPlayer.AttackType.AUTOFIGHT:
			case MainPlayer.AttackType.COMPLETE_TASK:
				stateMachine.Send((int)EventType.AUTOFIGHT);
				break;
			default:
				break;
			}
		}
	}

	protected virtual void ExitSelfCtrlState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{

	}	
	#endregion

	#region 战斗部分 by吴江

	protected virtual void EnterFightState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
        curEventType = EventType.FIGHT;
	}


	protected virtual void UpdateFightState(fsm.State _curState)
	{
		if (thisTarget == null || thisTarget.isDead)
		{
			GameCenter.curMainPlayer.GoNormal();
		}
		else
		{
			if (IsUseLoseHitAbility) return;
			if (thisTarget != null)
			{
				if (!thisPlayer.isRigidity && !thisPlayer.IsProtecting && !commandMng.HasCommand())
				{
					if (thisTarget.typeID == ObjectType.SceneItem)
					{
						Command_MoveTo cmdMoveTo = new Command_MoveTo();
						cmdMoveTo.destPos = thisTarget.transform.position;
						cmdMoveTo.maxDistance = 0f;
						commandMng.PushCommand(cmdMoveTo);
					}
					if (IsEnemy(thisTarget))
					{
						if (thisTarget.typeID == ObjectType.MOB)
						{
							Monster mob = thisTarget as Monster;
							if (mob != null && mob.isDead)
							{
								thisTarget = null;
								return;
							}
						}
						else if (thisTarget.typeID == ObjectType.Player)
						{
							OtherPlayer opc = thisTarget as OtherPlayer;
							if (opc != null && opc.isDead)
							{
								thisTarget = null;
								return;
							}
						}
						if (thisPlayer.AttakType == MainPlayer.AttackType.NORMALFIGHT)
						{
							if (lastNormalAbilityInstance != null && !lastNormalAbilityInstance.HasServerConfirm)
							{
								Debug.LogError(lastNormalAbilityInstance.AbilityName +  " Catch: 捕捉到上一次攻击尚未获得后台确认，前台即发动下一次普通攻击！");
							}
							AbilityInstance instance = thisPlayer.abilityMng.GetNextDefaultAbility(thisTarget);
							thisPlayer.TryUseAbility(instance, true);
							lastNormalAbilityInstance = instance;
						}
						else if (thisPlayer.AttakType == MainPlayer.AttackType.AUTOFIGHT)
						{
							stateMachine.Send((int)EventType.AUTOFIGHT);
						}
					}
				}
			}
		}
	}

	protected virtual void ExitFightState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{

	}
	#endregion

	#region 采集部分 by吴江
	protected SceneItem curTarSceneItem;
	protected TaskInfo curTarTaskInfo;

	protected SceneItem GetTarSceneItem()
	{
		TaskInfo taskInfo = GameCenter.taskMng.CurfocusTask;
		if(taskInfo == null)return null;
		TaskConditionType conditionType = taskInfo.ConditionRefList[0].sort;
		if (conditionType != TaskConditionType.CollectSceneItem && conditionType != TaskConditionType.CollectAnyItem) return null;
		List<SceneItem> sceneItems = GameCenter.curGameStage.GetSceneItems();
		float distance = float.MaxValue;
		SceneItem tarSceneItem = null;
		for (int i = 0,max=sceneItems.Count; i < max; i++) 
		{
			SceneItem item = sceneItems[i];
			if(item.IsTouchType != TouchType.TOUCH || item.isDummy)continue;
			if(conditionType == TaskConditionType.CollectSceneItem && item.ConfigID == taskInfo.ConditionRefList[0].data)
			{
				float sqr = (thisPlayer.transform.position - item.transform.position).sqrMagnitude;
				if(sqr < distance)
				{
					distance = sqr;
					tarSceneItem = item;
				}
			}
			if(conditionType == TaskConditionType.CollectAnyItem)
			{
				float sqr = (thisPlayer.transform.position - item.transform.position).sqrMagnitude;
				if(sqr < distance)
				{
					distance = sqr;
					tarSceneItem = item;
				}
			}
		}
		return tarSceneItem;
	}


	protected TaskStateType mainTaskStateType = TaskStateType.UnTake;

	protected TaskStateType CheckMainTaskStateType()
	{
		Dictionary<int, TaskInfo> dic = GameCenter.taskMng.GetTaskDic(TaskType.Main);
		if (dic.Count > 0)
		{
			foreach (TaskInfo item in dic.Values)
			{
				return item.TaskState;
			}
		}
		return TaskStateType.UnTake;
	}

	/// <summary>
	/// 检查任务是不是需要采集
	/// </summary>
	protected bool CheckTask()
	{
		curTarTaskInfo = GameCenter.taskMng.CurfocusTask;
		//SceneType sceneType = GameCenter.curGameStage.SceneType;
		if(curTarTaskInfo == null || curTarTaskInfo.ConditionRefList.Count <= 0)return false;
		TaskConditionType conditionType = curTarTaskInfo.ConditionRefList[0].sort;
		if ((conditionType != TaskConditionType.CollectSceneItem && conditionType != TaskConditionType.CollectAnyItem)
			|| curTarTaskInfo.ProgressConditionList[0] >= curTarTaskInfo.ConditionRefList[0].number)
		{
			return false;
		}
		return true;
	}

	protected virtual void EnterCollectState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
        curEventType = EventType.COLLECT;
		curTarSceneItem = null;
		if (!CheckTask())
		{
			if (curTarTaskInfo != null && curTarTaskInfo.TaskState == TaskStateType.Finished)
			{
				thisPlayer.GoTraceTask(curTarTaskInfo);
			}
			else
			{
				thisPlayer.GoNormal();
			}
			return;
		}
	}


	protected virtual void UpdateCollectState(fsm.State _curState)
	{
		if (!CheckTask())
		{
			thisPlayer.GoNormal();
			return;
		}
		if (curTarSceneItem == null || curTarSceneItem.isDead)
		{
			curTarSceneItem = GetTarSceneItem();
		}
		else if (!thisPlayer.commandMng.HasCommand() && !thisPlayer.IsCollecting && !GameCenter.mainPlayerMng.IsWaitTouchSceneItemMsg)
		{
			thisPlayer.CurTarget = curTarSceneItem;
			Command_MoveTo moveCmd = new Command_MoveTo();
			moveCmd.destPos = curTarSceneItem.transform.position;
			thisPlayer.commandMng.PushCommand(moveCmd);

			CommandTriggerTarget trigCmd = new CommandTriggerTarget();
			trigCmd.target = curTarSceneItem;
			thisPlayer.commandMng.PushCommand(trigCmd);
		}
	}

	protected virtual void ExitCollectState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
		curTarSceneItem = null;
	}
	#endregion

	#region 托管自动战斗
	protected AbilityInstance lastAbilityInstance = null;

    protected bool isInAutoFIght = false;

    protected EventType curEventType = EventType.AWAKE;
	public bool IsInAutoFIght
	{
		get
		{
            return isInAutoFIght;
		}
	}

    public bool NotFighting
    {
        get
        {
            return (curEventType == EventType.AWAKE && curEventType == EventType.SELF_CTRL);
        }
    }

	protected virtual void EnterAutoFightState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
        isInAutoFIght = true;
        curEventType = EventType.AUTOFIGHT;
		if (thisTarget == null)
		{
			stateMachine.Send((int)EventType.TARGET);
		}
	}


	protected virtual void UpdateAutoFightState(fsm.State _curState)
	{
		if (thisTarget == null || thisTarget.isDead)
		{
            //OnAutoPick();
			TaskStateType curTaskStateType = CheckMainTaskStateType();
			if (GameCenter.curGameStage.SceneType != SceneType.DUNGEONS && curTaskStateType != mainTaskStateType && curTaskStateType == TaskStateType.Finished)
			{
				thisPlayer.GoTraceTask(curTarTaskInfo);
				return;
			}
			else if (!commandMng.HasCommand())
			{
				stateMachine.Send((int)EventType.TARGET);//怪死了直接进入寻怪状态,去掉回起始点的设定  TARGET状态中有回路点的设定 By邓成
			}
		}
		else
		{
			if (IsUseLoseHitAbility) return;
			if (thisTarget != null)
			{
                if (!thisPlayer.isRigidity && !thisPlayer.IsProtecting && !commandMng.HasCommand())
				{
					if (thisTarget.typeID == ObjectType.SceneItem)
					{
						Command_MoveTo cmdMoveTo = new Command_MoveTo();
						cmdMoveTo.destPos = thisTarget.transform.position;
						cmdMoveTo.maxDistance = 0f;
						commandMng.PushCommand(cmdMoveTo);
					}
					if (IsEnemy(thisTarget))
					{
						if (thisTarget.typeID == ObjectType.MOB)
						{
							Monster mob = thisTarget as Monster;
							if (mob != null && mob.isDead)
							{
								thisTarget = null;
								return;
							}
						}
						else if (thisTarget.typeID == ObjectType.Player)
						{
							OtherPlayer opc = thisTarget as OtherPlayer;
							if (opc != null && opc.isDead)
							{
								thisTarget = null;
								return;
							}
						}
						switch (thisPlayer.AttakType)
						{
						case MainPlayer.AttackType.AUTOFIGHT:
						case MainPlayer.AttackType.COMPLETE_TASK:
							AbilityInstance instance = GameCenter.skillMng.GetAbilityRandom();
							if (instance == null || instance.RestCD > 0)
							{
								if (lastAbilityInstance != null && lastAbilityInstance.thisSkillMode == SkillMode.NORMALSKILL && thisPlayer.IsProtecting)
								{
									return;
								}
								if (lastNormalAbilityInstance != null && !lastNormalAbilityInstance.HasServerConfirm)
								{
									Debug.LogError(lastNormalAbilityInstance.AbilityName + " Catch: 捕捉到上一次攻击尚未获得后台确认，前台即发动下一次普通攻击！");
								}
								instance = thisPlayer.abilityMng.GetNextDefaultAbility(thisTarget);
								lastNormalAbilityInstance = instance;
							}
							else
							{
								instance.ResetResult(thisTarget);
								instance.SetActor(thisPlayer, thisTarget);
							}
							if (instance != null)
							{
								lastAbilityInstance = instance;
								thisPlayer.TryUseAbility(instance, true);
							}
							break;
						default:
							stateMachine.Send((int)EventType.FIGHT);
							break;
						}
					}
				}
			}
		}
	}

	protected virtual void ExitAutoFightState(fsm.State _from, fsm.State _to, fsm.Event _event)
	{
        isInAutoFIght = false;
	}	
	#endregion


    protected void OnStart()
    {
        isInAutoFIght = true;
        if (OnAutoFightStateUpdate != null)
        {
            OnAutoFightStateUpdate(true);
        }
    }

	protected void OnStop()
	{
        isInAutoFIght = false;
        curEventType = EventType.AWAKE;
		if (OnAutoFightStateUpdate != null)
		{
			OnAutoFightStateUpdate(false);
		}
	}

	public void ChangeTarget()
	{
		float distance = 0;
		thisTarget = GameCenter.curGameStage.GetAnotherSmartActor(thisTarget == null ? -1 : thisTarget.id,thisPlayer.transform.position);//startFightPos == Vector3.zero ? thisPlayer.transform.position : startFightPos);  //这里需要加
		if (thisTarget == null)
		{
			thisTarget = GameCenter.curGameStage.GetClosestMob(thisPlayer, ref distance);
			if (distance > autoFightDistance)
			{
				thisTarget = null;
			}
            if (thisTarget != null)//自动战斗避开BOSS  BY黄洪兴
            {
                Monster mo = thisTarget as Monster;
                if (mo != null&&GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef!=null)
                {
                    if (GameCenter.systemSettingMng.IsHideBoss && mo.actorInfo.IsBoss && GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef.sort==SceneType.SCUFFLEFIELD)
                        thisTarget = null;

                }
            }
           
		}
		if (thisTarget != null)
		{
			thisPlayer.CurTarget = thisTarget;
		}else
		{
            if(!commandMng.HasCommand())
            {
                List<DropItemInfo> dropItmes = GameCenter.sceneMng.GetDropItemInfoListByLimit(thisPlayer);
                if (dropItmes.Count > 1)
                {
                    thisPlayer.OnDropItem();
                }
                else
                {
                    Vector3 wayPoint = GameCenter.sceneMng.WayPoint;//找不到怪物则寻找路点
                    if ((thisPlayer.transform.position - wayPoint).sqrMagnitude > 2f)
                    {
                        if (wayPoint.x != 0 || wayPoint.z != 0)
                        {
                            Command_MoveTo moveto = new Command_MoveTo();
                            moveto.destPos = wayPoint;
                            moveto.maxDistance = 0f;
                            GameCenter.curMainPlayer.commandMng.PushCommand(moveto);
                        }
                    }
                }
            }
		}
	}




	/// <summary>
	/// 手动选中的怪物
	/// </summary>
	public void SetThisTarget(SmartActor _actor)
	{
		if (thisTarget != null && _actor != null && thisTarget.id == _actor.id)
		{
			return;
		}
		thisTarget = _actor;
	}
	/// <summary>
	/// 跳转到临时自主操控
	/// </summary>
	public void GoSelfCtrl()
	{
		stateMachine.Send((int)EventType.SELF_CTRL);
	}

	public void UnRegist()
	{
		thisTarget = null;
        foreach (AbilityInstance item in GameCenter.skillMng.abilityDic.Values)
        {
            item.ResetTarget(null);
        }
	}

	/// <summary>
	/// 是否为敌人 by吴江
	/// </summary>
	/// <param name="_smartActor"></param>
	/// <returns></returns>
	public static bool IsEnemy(InteractiveObject _obj)
	{
		if (_obj == null) return false;
		if (GameCenter.curGameStage is CityStage) return false;//主城不能打，没敌人
		switch (_obj.typeID)
		{
		case ObjectType.Player:
			OtherPlayer op = _obj as OtherPlayer;
			if (op == null) return false;
            if (GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType == SceneUiType.BATTLEFIGHT)//阵营模式
			    return ConfigMng.Instance.GetRelationType(GameCenter.curMainPlayer.Camp, op.Camp, GameCenter.curGameStage.SceneType) != RelationType.NO_ATTAK;
			if(GameCenter.curMainPlayer.CurPkMode == PkMode.PKMODE_ALL)
				return true; 
			if(GameCenter.curMainPlayer.CurPkMode == PkMode.PKMODE_TEAM && !GameCenter.teamMng.CheckTeamMate(op.id))
				return true;//队伍模式,不在一个队伍则是敌人
			if(GameCenter.curMainPlayer.CurPkMode == PkMode.PKMODE_GUILD && (string.IsNullOrEmpty(op.GuildName) || string.IsNullOrEmpty(GameCenter.curMainPlayer.GuildName) || !op.GuildName.Equals(GameCenter.curMainPlayer.GuildName)))
				return true;//公会模式下,自己无公会or对方无公会or不同的公会则是敌人
			if(GameCenter.curMainPlayer.CurPkMode == PkMode.PKMODE_JUSTICE && (op.SlaSevel == 3 || op.SlaSevel == 2 || op.IsCounterAttack))
				return true;//3是魔头,IsCounterAttack表示该玩家恶意了你
			return false;//和平模式
		case ObjectType.MOB:
			Monster mob = _obj as Monster;
			if (mob == null) return false;
			if(mob.actorInfo.RankLevel == MobRankLevel.DAILYDART)
			{
				if(mob.actorInfo.DartOwnerID == GameCenter.curMainPlayer.id)
					return false;
				OtherPlayer opc = GameCenter.curGameStage.GetOtherPlayer(mob.actorInfo.DartOwnerID);
				if (opc == null)
				{
					if(GameCenter.curMainPlayer.CurPkMode == PkMode.PKMODE_PEASE)
						return false;
					return true;//无人镖车可以打吧
				}
				if(GameCenter.curMainPlayer.CurPkMode == PkMode.PKMODE_ALL)
					return true;
				if(GameCenter.curMainPlayer.CurPkMode == PkMode.PKMODE_TEAM && !GameCenter.teamMng.CheckTeamMate(opc.id))
					return true;//队伍模式,不在一个队伍则是敌人
				if(GameCenter.curMainPlayer.CurPkMode == PkMode.PKMODE_GUILD && (string.IsNullOrEmpty(opc.GuildName) || string.IsNullOrEmpty(GameCenter.curMainPlayer.GuildName) || !opc.GuildName.Equals(GameCenter.curMainPlayer.GuildName)))
					return true;//公会模式下,自己无公会or对方无公会or不同的公会则是敌人
				if(GameCenter.curMainPlayer.CurPkMode == PkMode.PKMODE_JUSTICE && (opc.SlaSevel == 3 || opc.SlaSevel == 2))
					return true;//3是魔头
				return false;//和平模式
			}else if(mob.actorInfo.RankLevel == MobRankLevel.GUILDDART)
			{
				if(GameCenter.curMainPlayer.CurPkMode != PkMode.PKMODE_PEASE && GameCenter.curMainPlayer.CurPkMode != PkMode.PKMODE_JUSTICE)
				{
					if(GameCenter.guildMng.MyGuildInfo != null && mob.actorInfo.DartOwnerID == GameCenter.guildMng.MyGuildInfo.GuildId)
						return false;//不能打自己公会的镖车
					return true;
				}
				return false;//和平模式和善恶模式不能打镖车
			}
			return ConfigMng.Instance.GetRelationType(GameCenter.curMainPlayer.Camp, mob.Camp, GameCenter.curGameStage.SceneType) != RelationType.NO_ATTAK;
		case ObjectType.Entourage:
			EntourageBase Entourage = _obj as EntourageBase;
			if (Entourage.Owner != null && Entourage.Owner == GameCenter.curMainPlayer)
			{
				return false;
			}
			else
			{
				return ConfigMng.Instance.GetRelationType(GameCenter.curMainPlayer.Camp, Entourage.Camp, GameCenter.curGameStage.SceneType) != RelationType.NO_ATTAK;
			}
		case ObjectType.NPC:
			return false;
		case ObjectType.FlyPoint:
			return false;
		default:
			return false;
		}
	}
}
