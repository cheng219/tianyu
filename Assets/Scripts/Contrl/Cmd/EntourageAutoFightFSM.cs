//==============================================
//作者：吴江
//日期：2015/9/14
//用途：玩家自动战斗的AI状态机
//==============================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;



/// <summary>
/// 主角的随从的自动战斗的AI状态机  by吴江
/// </summary>
public class EntourageAutoFightFSM : FSMBase
{
    #region 数据  by吴江
    protected MercenaryInfo actorInfo;
    /// <summary>
    /// 从属主人
    /// </summary>
    protected MainPlayer owner = null;
    /// <summary>
    /// 自身对象
    /// </summary>
    protected MainEntourage thisPlayer = null;
    /// <summary>
    /// 当前目标
    /// </summary>
    protected SmartActor thisTarget = null;
    /// <summary>
    /// 最后的普通攻击
    /// </summary>
    protected AbilityInstance lastNormalAbilityInstance = null;
    /// </summary>
    /// 跟随距离
    /// </summary>
    public float followRange = 5.0f;
    /// <summary>
    /// 停止跟随的距离 by吴江
    /// </summary>
    public float stopFollowRange = 1.0f;
    /// <summary>
    /// 强制传送距离 by吴江
    /// </summary>
    public float teleportRange = 40.0f;
    /// <summary>
    /// PVE自主攻击距离距离 by吴江
    /// </summary>
    public float pveAtkRange = 10.0f;
    /// <summary>
    /// PVE主人攻击的协助的距离 by吴江
    /// </summary>
    public float pveMasterAtkRange = 15.0f;
    /// <summary>
    /// PVE回归的距离 by吴江
    /// </summary>
    public float pveReturnRange = 20.0f;
    /// <summary>
    /// PVP自主攻击的距离 by吴江
    /// </summary>
    public float pvpAttackRange = 10.0f;
    /// <summary>
    /// PVP主人发起攻击的协助的距离 by吴江
    /// </summary>
    public float pvpMasterAtkRange = 15.0f;
    /// <summary>
    /// PVP的被动防御的范围 by吴江
    /// </summary>
    public float pvpDefenseRange = 20.0f;
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
    /// <summary>
    /// 当前的状态机状态
    /// </summary>
    protected EventType curEventType = EventType.AWAKE;

    protected float startNoTargetTime = 0f;
    protected bool startCheakNoTarget = false;
    /// <summary>
    /// 命令管理类 
    /// </summary>
    public CommandMng commandMng;
    #endregion

    #region 构造  by吴江
    public void Init(MercenaryInfo _actorInfo)
    {
        if (_actorInfo == null) return;
        actorInfo = _actorInfo;
        followRange = _actorInfo.FollowRange;
        stopFollowRange = _actorInfo.StopFollowRange;
        pveAtkRange = _actorInfo.PveAtkRange;
        pveMasterAtkRange = _actorInfo.PveMasterAtkRange;
        pveReturnRange = _actorInfo.PveReturnRange;
        pvpAttackRange = _actorInfo.PvpAttackRange;
        pvpDefenseRange = _actorInfo.PvpDefenseRange;
        pvpMasterAtkRange = _actorInfo.PvpMasterAtkRange;
        teleportRange = _actorInfo.TeleportRange;
        InitTalkWaitDic();
    }
    #endregion 

    #region UNITY  by吴江
    void OnEnable()
    {
    //    GameCenter.inventoryMng.OnBackpackAddItem += OnAddItem;
    }

    void OnDisable()
    {
    //    GameCenter.inventoryMng.OnBackpackAddItem -= OnAddItem;
    }

    void Update()
    {
        if (stateMachine != null)
        {
            if (Time.frameCount % 10 == 0)
            {
                stateMachine.Update();
                if (startCheakNoTarget && Time.time - startNoTargetTime > 30f)
                {
                    TryTalk(EntourageTalkType.NOTARGET);
                    startNoTargetTime = Time.time;
                }
            }
        }
    }

    void OnDestroy()
    {
        GameCenter.abilityMng.OnMainPlayerUseAbility -= OnMainPlayerUseAbility;
        GameCenter.abilityMng.OnMainPlayerBeHit -= OnMainPlayerBeHit;
    }
    #endregion

    #region 状态机  by吴江
    /// <summary>
    /// 状态枚举 by吴江
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// 启动
        /// </summary>
        AWAKE = fsm.Event.USER_FIELD + 1,
        /// <summary>
        /// 锁定目标
        /// </summary>
        TARGET,
        /// <summary>
        /// 追杀主人pve目标 第三优先
        /// </summary>
        PVE_TRACE,
        /// <summary>
        /// 追杀打主人的pve目标 第四优先
        /// </summary>
        PVE_DEFENSE,
        /// <summary>
        /// PVE自主战斗 第六优先
        /// </summary>
        PVE_FIGHT,
        /// <summary>
        /// pvp战斗,追杀主人目标  第一优先
        /// </summary>
        PVP_FIGHT_TRACE,
        /// <summary>
        /// pvp战斗,追杀攻击主人的目标 第二优先
        /// </summary>
        PVP_FIGHT_DEFENSE,
        /// <summary>
        /// PVP自主战斗 第五优先
        /// </summary>
        PVP_FIGHT,
        /// <summary>
        /// 跟随主人
        /// </summary>
        FLOW,
        /// <summary>
        /// 传送状态
        /// </summary>
        TELEPORT,
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

    protected override void InitStateMachine()
    {
        fsm.State awake = new fsm.State("awake", stateMachine);
        awake.onEnter += EnterAwakeState;
        awake.onExit += ExitAwakeState;
        awake.onAction += UpdateAwakeState;



        fsm.State pveFight = new fsm.State("pveFight", stateMachine);
        pveFight.onEnter += EnterPVEFightState;
        pveFight.onExit += ExitPVEFightState;
        pveFight.onAction += UpdatePVEFightState;



        fsm.State pveTraceFight = new fsm.State("pveTraceFight", stateMachine);
        pveTraceFight.onEnter += EnterPVETraceFightState;
        pveTraceFight.onExit += ExitPVETraceFightState;
        pveTraceFight.onAction += UpdatePVETraceFightState;


        fsm.State pveDefenseFight = new fsm.State("pveDefenseFight", stateMachine);
        pveDefenseFight.onEnter += EnterPVEDefenseFightState;
        pveDefenseFight.onExit += ExitPVEDefenseFightState;
        pveDefenseFight.onAction += UpdatePVEDefenseFightState;

        fsm.State pvpFight = new fsm.State("pvpFight", stateMachine);
        pvpFight.onEnter += EnterPVPFightState;
        pvpFight.onExit += ExitPVPFightState;
        pvpFight.onAction += UpdatePVPFightState;

        fsm.State pvpTraceFight = new fsm.State("pvpTraceFight", stateMachine);
        pvpTraceFight.onEnter += EnterPVPTraceFightState;
        pvpTraceFight.onExit += ExitPVPTraceFightState;
        pvpTraceFight.onAction += UpdatePVPTraceFightState;

        fsm.State pvpDefenseFight = new fsm.State("pvpDefenseFight", stateMachine);
        pvpDefenseFight.onEnter += EnterPVPDefenseFightState;
        pvpDefenseFight.onExit += ExitPVPDefenseFightState;
        pvpDefenseFight.onAction += UpdatePVPDefenseFightState;

        fsm.State flow = new fsm.State("flow", stateMachine);
        flow.onEnter += EnterFlowState;
        flow.onExit += ExitFlowState;
        flow.onAction += UpdateFlowState;

        fsm.State teleport = new fsm.State("teleport", stateMachine);
        teleport.onEnter += EnterTelePortState;
        teleport.onExit += ExitTelePortState;
        teleport.onAction += UpdateTelePortState;


        //各状态之间的可跳转关系 by吴江
        awake.Add<fsm.EventTransition>(flow, (int)EventType.FLOW);


        flow.Add<fsm.EventTransition>(teleport, (int)EventType.TELEPORT);
        flow.Add<fsm.EventTransition>(pvpTraceFight, (int)EventType.PVP_FIGHT_TRACE);

        teleport.Add<fsm.EventTransition>(flow, (int)EventType.FLOW);

        pvpTraceFight.Add<fsm.EventTransition>(pvpDefenseFight, (int)EventType.PVP_FIGHT_DEFENSE);
        pvpTraceFight.Add<fsm.EventTransition>(teleport, (int)EventType.TELEPORT);

        pvpDefenseFight.Add<fsm.EventTransition>(pveTraceFight, (int)EventType.PVE_TRACE);
        pvpDefenseFight.Add<fsm.EventTransition>(pvpTraceFight, (int)EventType.PVP_FIGHT_TRACE);
        pvpDefenseFight.Add<fsm.EventTransition>(teleport, (int)EventType.TELEPORT);

        pveTraceFight.Add<fsm.EventTransition>(pveDefenseFight, (int)EventType.PVE_DEFENSE);
        pveTraceFight.Add<fsm.EventTransition>(pvpTraceFight, (int)EventType.PVP_FIGHT_TRACE);
        pveTraceFight.Add<fsm.EventTransition>(pvpDefenseFight, (int)EventType.PVP_FIGHT_DEFENSE);
        pveTraceFight.Add<fsm.EventTransition>(teleport, (int)EventType.TELEPORT);

        pveDefenseFight.Add<fsm.EventTransition>(pvpFight, (int)EventType.PVP_FIGHT);
        pveDefenseFight.Add<fsm.EventTransition>(pvpTraceFight, (int)EventType.PVP_FIGHT_TRACE);
        pveDefenseFight.Add<fsm.EventTransition>(pvpDefenseFight, (int)EventType.PVP_FIGHT_DEFENSE);
        pveDefenseFight.Add<fsm.EventTransition>(pveTraceFight, (int)EventType.PVE_TRACE);
        pveDefenseFight.Add<fsm.EventTransition>(teleport, (int)EventType.TELEPORT);

        pvpFight.Add<fsm.EventTransition>(pveFight, (int)EventType.PVE_FIGHT);
        pvpFight.Add<fsm.EventTransition>(pvpTraceFight, (int)EventType.PVP_FIGHT_TRACE);
        pvpFight.Add<fsm.EventTransition>(pvpDefenseFight, (int)EventType.PVP_FIGHT_DEFENSE);
        pvpFight.Add<fsm.EventTransition>(pveTraceFight, (int)EventType.PVE_TRACE);
        pvpFight.Add<fsm.EventTransition>(pveDefenseFight, (int)EventType.PVE_DEFENSE);
        pvpFight.Add<fsm.EventTransition>(teleport, (int)EventType.TELEPORT);

        pveFight.Add<fsm.EventTransition>(flow, (int)EventType.FLOW);
        pveFight.Add<fsm.EventTransition>(pvpTraceFight, (int)EventType.PVP_FIGHT_TRACE);
        pveFight.Add<fsm.EventTransition>(pvpDefenseFight, (int)EventType.PVP_FIGHT_DEFENSE);
        pveFight.Add<fsm.EventTransition>(pveTraceFight, (int)EventType.PVE_TRACE);
        pveFight.Add<fsm.EventTransition>(pveDefenseFight, (int)EventType.PVE_DEFENSE);
        pveFight.Add<fsm.EventTransition>(pvpFight, (int)EventType.PVP_FIGHT);
        pveFight.Add<fsm.EventTransition>(teleport, (int)EventType.TELEPORT);

        stateMachine.initState = awake;
    }

    #region 启动部分 by吴江
    protected virtual void EnterAwakeState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (thisPlayer == null)
        {
            thisPlayer = this.gameObject.GetComponent<MainEntourage>();
        }
        if (owner == null)
        {
            owner = GameCenter.curMainPlayer;
        }
        if (thisPlayer == null)
        {
            GameSys.LogError("找不到组件！");
        }
        else
        {
            commandMng = thisPlayer.commandMng;
        }
        GameCenter.abilityMng.OnMainPlayerUseAbility += OnMainPlayerUseAbility;
        GameCenter.abilityMng.OnMainPlayerBeHit += OnMainPlayerBeHit;
        curEventType = EventType.AWAKE;
        startNoTargetTime = Time.time;
        startCheakNoTarget = true;
    }

    protected virtual void UpdateAwakeState(fsm.State _curState)
    {
        if (thisPlayer != null && !thisPlayer.isDummy && GameCenter.curGameStage != null && GameCenter.sceneMng != null)
        {
            stateMachine.Send((int)EventType.FLOW);
        }
    }

    protected virtual void ExitAwakeState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        TryTalk(EntourageTalkType.CALL);
    }
    #endregion

    #region 跟随状态 by吴江
    protected virtual void EnterFlowState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (thisPlayer.isDead) return;
        //TryTalk(EntourageTalkType.IDLE);
        thisTarget = null;
        curEventType = EventType.FLOW;
    }

    protected virtual void UpdateFlowState(fsm.State _curState)
    {
        if (thisPlayer.isDead) return;
        if (CheckNeedTeleport()) return; 
        if (thisPlayer != null && !thisPlayer.commandMng.HasCommand())//如果自主命令都已经执行完毕，重新回到战斗
        {
            float sqr = (thisPlayer.transform.position - owner.transform.position).sqrMagnitude;
            if (sqr >= stopFollowRange * stopFollowRange)
            {
                Command_MoveTo movetoCMD = new Command_MoveTo();
                movetoCMD.destPos = Utils.GetRandomPos(owner.transform);
                thisPlayer.commandMng.PushCommand(movetoCMD);
            }
            else
            {
                stateMachine.Send((int)EventType.PVP_FIGHT_TRACE);
            }
        }
    }

    protected virtual void ExitFlowState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
    }
    #endregion

    #region 传送状态 by吴江
    protected virtual void EnterTelePortState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (thisPlayer.isDead) return;
        thisTarget = null;
        //TryTalk(EntourageTalkType.IDLE);
        curEventType = EventType.TELEPORT;
        thisPlayer.commandMng.CancelCommands();
        if (thisPlayer.IsMoving)
        {
            thisPlayer.StopMovingTo();
        };
        thisPlayer.CancelAbility();
        thisPlayer.InitPos();
        stateMachine.Send((int)EventType.FLOW);
    }

    protected virtual void UpdateTelePortState(fsm.State _curState)
    {

    }

    protected virtual void ExitTelePortState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {

    }
    #endregion

    #region 自主PVE战斗部分 by吴江
    protected SmartActor lastPveTarget;

    protected virtual void EnterPVEFightState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (thisPlayer.isDead) return;
        //TryTalk(EntourageTalkType.ATTACK);
        curEventType = EventType.PVE_FIGHT;
        thisPlayer.commandMng.CancelCommands();
        if (thisPlayer.IsMoving)
        {
            thisPlayer.StopMovingTo();
        }
    }

    protected virtual void UpdatePVEFightState(fsm.State _curState)
    {
        if (thisPlayer.isDead) return;
        TryFight(EventType.PVE_FIGHT, EventType.FLOW, pveReturnRange);
    }

    protected virtual void ExitPVEFightState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
    }
    #endregion

    #region PVE追杀战斗部分 by吴江
    protected SmartActor lastPveTraceTarget;

    protected virtual void EnterPVETraceFightState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (thisPlayer.isDead) return;
        //TryTalk(EntourageTalkType.ATTACK);
        curEventType = EventType.PVE_TRACE;
        thisPlayer.commandMng.CancelCommands();
        if (thisPlayer.IsMoving)
        {
            thisPlayer.StopMovingTo();
        }
    }

    protected virtual void UpdatePVETraceFightState(fsm.State _curState)
    {
        if (thisPlayer.isDead) return;
        TryFight(EventType.PVE_TRACE, EventType.PVE_DEFENSE,pveMasterAtkRange);
    }

    protected virtual void ExitPVETraceFightState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {

    }
    #endregion

    #region PVE守护战斗部分 by吴江
    protected SmartActor lastPveDefenseTarget;

    protected virtual void EnterPVEDefenseFightState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (thisPlayer.isDead) return;
        //TryTalk(EntourageTalkType.ATTACK);
        curEventType = EventType.PVE_DEFENSE;
        thisPlayer.commandMng.CancelCommands();
        if (thisPlayer.IsMoving)
        {
            thisPlayer.StopMovingTo();
        }
    }

    protected virtual void UpdatePVEDefenseFightState(fsm.State _curState)
    {
        if (thisPlayer.isDead) return;
        TryFight(EventType.PVE_DEFENSE, EventType.PVP_FIGHT, pveReturnRange);
    }

    protected virtual void ExitPVEDefenseFightState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {

    }
    #endregion


    #region 自主PVP战斗部分 by吴江
    protected SmartActor lastPvpTarget;

    protected virtual void EnterPVPFightState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (thisPlayer.isDead) return;
        //TryTalk(EntourageTalkType.ATTACK);
        curEventType = EventType.PVP_FIGHT;
        thisPlayer.commandMng.CancelCommands();
        if (thisPlayer.IsMoving)
        {
            thisPlayer.StopMovingTo();
        }
    }

    protected virtual void UpdatePVPFightState(fsm.State _curState)
    {
        if (thisPlayer.isDead) return;
        TryFight(EventType.PVP_FIGHT, EventType.PVE_FIGHT, pvpAttackRange);
    }

    protected virtual void ExitPVPFightState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {

    }
    #endregion

    #region PVP追杀战斗部分 by吴江
    protected SmartActor lastPvpTraceTarget;

    protected virtual void EnterPVPTraceFightState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (thisPlayer.isDead) return;
        //TryTalk(EntourageTalkType.ATTACK);
        curEventType = EventType.PVP_FIGHT_TRACE;
        thisPlayer.commandMng.CancelCommands();
        if (thisPlayer.IsMoving)
        {
            thisPlayer.StopMovingTo();
        }
    }


    protected virtual void UpdatePVPTraceFightState(fsm.State _curState)
    {
        if (thisPlayer.isDead) return;
        TryFight(EventType.PVP_FIGHT_TRACE, EventType.PVP_FIGHT_DEFENSE,pvpMasterAtkRange);
    }

    protected virtual void ExitPVPTraceFightState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {

    }
    #endregion

    #region PVP守护战斗部分 by吴江
    protected SmartActor lastPvpDefenseTarget;

    protected virtual void EnterPVPDefenseFightState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (thisPlayer.isDead) return;
        //TryTalk(EntourageTalkType.ATTACK);
        curEventType = EventType.PVP_FIGHT_DEFENSE;
        thisPlayer.commandMng.CancelCommands();
        if (thisPlayer.IsMoving)
        {
            thisPlayer.StopMovingTo();
        }
    }


    protected virtual void UpdatePVPDefenseFightState(fsm.State _curState)
    {
        if (thisPlayer.isDead) return;
        TryFight(EventType.PVP_FIGHT_DEFENSE, EventType.PVE_TRACE, pvpDefenseRange);

    }

    protected virtual void ExitPVPDefenseFightState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {

    }
    #endregion
    #endregion

    #region 锁怪部分 by吴江
    /// <summary>
    /// 检测是否需要强制传送了
    /// </summary>
    /// <returns></returns>
    protected bool CheckNeedTeleport()
    {
        if ((thisPlayer.transform.position - owner.transform.position).sqrMagnitude > teleportRange * teleportRange)
        {
            stateMachine.Send((int)EventType.TELEPORT);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 尝试战斗 by吴江
    /// </summary>
    /// <param name="_curType">当前状态</param>
    /// <param name="_nextType">下一顺位的状态</param>
    protected void TryFight(EventType _curType, EventType _nextType, float _checkRange)
    {
        if (CheckNeedTeleport())
        {
            return;
        }
		if (lastNormalAbilityInstance != null && !lastNormalAbilityInstance.HasServerConfirm && !lastNormalAbilityInstance.IsTringEnd)
        {
            return;
        }
        if ((thisPlayer.transform.position - owner.transform.position).sqrMagnitude > _checkRange * _checkRange)
        {
            stateMachine.Send((int)_nextType);
            return;
        }
        if (thisTarget == null || thisTarget.isDead)
        {
            thisTarget = TryTarget(_curType);
        }
        if (thisTarget == null || thisTarget.isDead)
        {
            stateMachine.Send((int)_nextType);
        }
        else
        {
            if (IsUseLoseHitAbility) return;
            if (thisTarget != null)
            {
                TryTalk(EntourageTalkType.ATTACK);
                if (!thisPlayer.isRigidity && !thisPlayer.IsProtecting && !commandMng.HasCommand())
                {
                    if (lastNormalAbilityInstance != null && !lastNormalAbilityInstance.HasServerConfirm)
                    {
                        Debug.LogError("Catch: 捕捉到上一次普通攻击尚未获得后台确认，前台即发动下一次普通攻击！");
                    }
                    AbilityInstance instance = thisPlayer.abilityMng.GetNextEntourageAbility(thisTarget);
					//Debug.Log("TryUseAbility:"+(instance==null?"null":instance.AbilityName)+",Time:"+Time.realtimeSinceStartup);
					if(instance != null)
					{
	                    thisPlayer.TryUseAbility(instance, true);
	                    lastNormalAbilityInstance = instance;
                        startNoTargetTime = Time.time;
					}
                }
            }
        }
        return;
    }
    /// <summary>
    /// 尝试锁定目标 by吴江
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    protected SmartActor TryTarget(EventType _type)
    {
        float distance = 0;
        switch (_type)
        {
            case EventType.PVE_TRACE:
                if (pveMasterAtkRange <= 0) return null;
                if (lastPveTraceTarget != null && lastPveTraceTarget.isDead) lastPveTraceTarget = null;
                if (lastPveTraceTarget != null && (owner.transform.position - lastPveTraceTarget.transform.position).sqrMagnitude <= pveMasterAtkRange * pveMasterAtkRange)
                {
                    if (ConfigMng.Instance.GetRelationType(thisPlayer.Camp, lastPveTraceTarget.Camp, GameCenter.curGameStage.SceneType) == RelationType.NO_ATTAK || lastPveTraceTarget.typeID != ObjectType.MOB)
                    {
                        lastPveTraceTarget = null;
                    }
                    return lastPveTraceTarget;
                }
                else
                {
                    lastPveTraceTarget = null;
                    return null;
                }
            case EventType.PVE_DEFENSE:
                if (pveReturnRange <= 0) return null;
                if (lastPveDefenseTarget != null && lastPveDefenseTarget.isDead) lastPveDefenseTarget = null;
                if (lastPveDefenseTarget != null && (owner.transform.position - lastPveDefenseTarget.transform.position).sqrMagnitude <= pveReturnRange * pveReturnRange)
                {
                    if (ConfigMng.Instance.GetRelationType(thisPlayer.Camp, lastPveDefenseTarget.Camp, GameCenter.curGameStage.SceneType) == RelationType.NO_ATTAK || lastPveDefenseTarget.typeID != ObjectType.MOB)
                    {
                        lastPveDefenseTarget = null;
                    }
                    return lastPveDefenseTarget;
                }
                else
                {
                    lastPveDefenseTarget = null;
                    return null;
                }
            case EventType.PVE_FIGHT:
                if (pveAtkRange <= 0 || pveReturnRange <= 0) return null;
                if (lastPveTarget != null && lastPveTarget.isDead) lastPveTarget = null;
                if (lastPveTarget != null && (owner.transform.position - lastPveTarget.transform.position).sqrMagnitude <= pveReturnRange * pveReturnRange)
                {
                    if (ConfigMng.Instance.GetRelationType(thisPlayer.Camp, lastPveTarget.Camp, GameCenter.curGameStage.SceneType) == RelationType.NO_ATTAK || lastPveTarget.typeID != ObjectType.MOB)
                    {
                        lastPveTarget = null;
                    }
                }
                else
                {
                    lastPveTarget = null;
                }
                if (lastPveTarget == null)
                {
                    lastPveTarget = GameCenter.curGameStage.GetClosestMob(owner, ref distance);  //找到距离主人最近的怪物。  
                    if (distance > pveAtkRange)
                    {
                        lastPveTarget = null;
                    }
                }
                return lastPveTarget;
            case EventType.PVP_FIGHT_TRACE:
                if (pvpMasterAtkRange <= 0) return null;
                if (lastPvpTraceTarget != null && lastPvpTraceTarget.isDead) lastPvpTraceTarget = null;
                if (lastPvpTraceTarget != null && (owner.transform.position - lastPvpTraceTarget.transform.position).sqrMagnitude <= pvpMasterAtkRange * pvpMasterAtkRange)
                {
                    if ((lastPvpTraceTarget.typeID != ObjectType.Player && lastPvpTraceTarget.typeID != ObjectType.Entourage))
                    {
                        lastPvpTraceTarget = null;
                    }
                    return lastPvpTraceTarget;
                }
                else
                {
                    lastPvpTraceTarget = null;
                    return null;
                }
            case EventType.PVP_FIGHT_DEFENSE:
                if (pvpDefenseRange <= 0) return null;
                if (lastPvpDefenseTarget != null && lastPvpDefenseTarget.isDead) lastPvpDefenseTarget = null;
                if (lastPvpDefenseTarget != null && (owner.transform.position - lastPvpDefenseTarget.transform.position).sqrMagnitude <= pvpDefenseRange * pvpDefenseRange)
                {
                    if ((lastPvpDefenseTarget.typeID != ObjectType.Player && lastPvpDefenseTarget.typeID != ObjectType.Entourage))
                    {
                        lastPvpDefenseTarget = null;
                    }
                    return lastPvpDefenseTarget;
                }
                else
                {
                    lastPvpDefenseTarget = null;
                    return null;
                }
            case EventType.PVP_FIGHT:
                if (pvpAttackRange <= 0) return null;
                if (lastPvpTarget != null && lastPvpTarget.isDead) lastPvpTarget = null;
                if (lastPvpTarget != null && (owner.transform.position - lastPvpTarget.transform.position).sqrMagnitude <= pvpAttackRange * pvpAttackRange)
                {
                    if ((lastPvpTarget.typeID != ObjectType.Player && lastPvpTarget.typeID != ObjectType.Entourage))
                    {
                        lastPvpTarget = null;
                    }
                }
                else
                {
                    lastPvpTarget = null;
                }
                if (lastPvpTarget == null)
                {
                    lastPvpTarget = GameCenter.curGameStage.GetClosestPlayer(owner, RelationType.AUTOMATEDATTACKS, ref distance);
                    if (distance > pveAtkRange)
                    {
                        lastPvpTarget = null;
                        distance = 0;
                        lastPvpTarget = GameCenter.curGameStage.GetClosestEntourage(owner, RelationType.AUTOMATEDATTACKS, ref distance);
                        if (distance > pveAtkRange)
                        {
                            lastPvpTarget = null;
                        }
                    }
                }
                return lastPvpTarget;
            default:
                return null;
        }
    }
    /// <summary>
    /// 当主主角使用技能成功时
    /// </summary>
    protected void OnMainPlayerUseAbility()
    {
        SmartActor target = owner.CurTarget as SmartActor;
        if (target == null) return;
        if (PlayerAutoFightFSM.IsEnemy(target))
        {
            switch (target.typeID)
            {
                case ObjectType.Entourage:
                case ObjectType.Player:
                    if (lastPvpTraceTarget != target)
                    {
                        lastPvpTraceTarget = target;
                        stateMachine.Send((int)EventType.PVP_FIGHT_TRACE);
                    }
                    break;
                case ObjectType.MOB:
                    if (lastPveTraceTarget != target)
                    {
                        lastPveTraceTarget = target;
                        stateMachine.Send((int)EventType.PVE_TRACE);
                    }
                    break;
                default:
                    break;
            }
        }
    }
    /// <summary>
    /// 主角被打事件 by吴江
    /// </summary>
    /// <param name="_fromUser"></param>
    protected void OnMainPlayerBeHit(SmartActor _fromUser)
    {
        if (_fromUser == null) return;
        switch (_fromUser.typeID)
        {
            case ObjectType.Entourage:
            case ObjectType.Player:
                lastPvpDefenseTarget = _fromUser;
                stateMachine.Send((int)EventType.PVP_FIGHT_DEFENSE);
                break;
            case ObjectType.MOB:
                lastPveDefenseTarget = _fromUser;
                stateMachine.Send((int)EventType.PVE_DEFENSE);
                break;
            default:
                return;
        }
    }

    public void ChangeTarget()
    {
        float distance = 0;
        if (thisTarget != null && !thisTarget.isDead)
        {
            thisTarget = GameCenter.curGameStage.GetAnotherMob(thisTarget.id);
        }
        else
        {
            thisTarget = GameCenter.curGameStage.GetClosestMob(thisPlayer, ref distance);
        }
    }

    #endregion

    #region 说话部分  by吴江
    protected Dictionary<EntourageTalkType, EntourageTalkCheck> curTalkWaitDic = new Dictionary<EntourageTalkType, EntourageTalkCheck>();

    protected void InitTalkWaitDic()
    {
        foreach (EntourageTalkType item in Enum.GetValues(typeof(EntourageTalkType)))
        {
			PoPoPetRef refData = ConfigMng.Instance.GetPoPoPetRef(actorInfo.PetId, item);
            if (refData != null)
            {
                //Debug.Log("actorInfo.PetId:" + actorInfo.PetId + "item:" + item + ",popID:" + refData.poPoId);
                curTalkWaitDic[item] = new EntourageTalkCheck(refData);
            }
            else
            {
            //    Debug.LogError(item + "随从泡泡说话初始化失败!");
            }
        }
    }
    /// <summary>
    /// 尝试冒泡说话
    /// </summary>
    /// <param name="_type"></param>
    public void TryTalk(EntourageTalkType _type)
    {
        if (thisPlayer.headTextCtrl == null) return;
        if (curTalkWaitDic.ContainsKey(_type))
        {
            EntourageTalkCheck check = curTalkWaitDic[_type];
            if (check.CanTalk)
            {
                check.Reset();
                PoPoRef bub = check.CurTalkRefData;
                if (bub != null)
                {
                    thisPlayer.headTextCtrl.SetBubble(bub.content, bub.time);
                }
            }
        }
    }

    protected void OnAddItem(EquipmentInfo _eq)
    {
        if (_eq == null) return;
        //if (_eq.Quality == 4)
        //{
        //    TryTalk(EntourageTalkType.REWARDEPIC);
        //}
        //else if (_eq.Quality == 5)
        //{
        //    TryTalk(EntourageTalkType.REWARDLEGEND);
        //}
    }
    #endregion
}


public class EntourageTalkCheck
{

    public EntourageTalkCheck(PoPoPetRef _refData)
    {
        refData = _refData;
        popRef = ConfigMng.Instance.GetPoPoRef(refData.poPoId);
        if (_refData.type == (int)EntourageTalkType.CALL)
        {
            curWaitTime = 0;
        }
        else
        {
            if (popRef != null)
                curWaitTime = popRef.time;// UnityEngine.Random.Range(5, popRef.time);
        }
        startTime = Time.time;
    }

    protected PoPoPetRef refData;
    protected PoPoRef popRef;
    public float curWaitTime;
    public float startTime;


    public bool CanTalk
    {
        get
        {
            return Time.time - startTime >= curWaitTime && randomTrue;
        }
    }

    protected bool randomTrue
    {
        get
        {
            if (popRef != null)
            {
                int pro = popRef.probability/100;
                System.Random random = new System.Random();
                return random.Next(1, 101) <= pro;
            }
            return false;
        }
    }

    public PoPoRef CurTalkRefData
    {
        get
        {
            return popRef;
        }
    }

    public void Reset()
    {
        startTime = Time.time;
        curWaitTime = UnityEngine.Random.Range(5, popRef.time);
    }
}