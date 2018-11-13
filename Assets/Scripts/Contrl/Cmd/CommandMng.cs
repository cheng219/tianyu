//====================================
//作者：吴江
//日期：2015/5/22
//用途：对象的命令管理器
//====================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 对象的命令管理器 by吴江
/// </summary>
public class CommandMng {

    Actor actor = null;
    List<ActorCommand> cmdList = new List<ActorCommand> ();

    /// <summary>
    /// 构造 by吴江
    /// </summary>
    /// <param name="_actor"></param>
    public CommandMng ( Actor _actor ) {
        actor = _actor;
    }


    /// <summary>
    /// 执行 by吴江
    /// </summary>
    public void Tick () {
        if ( cmdList.Count == 0 )
            return;

        //
        ActorCommand curCommand = cmdList[0];
        bool finished = curCommand.Exec(actor);
        if ( finished ) {
            if (cmdList.Count > 0) {
                if (ReferenceEquals(cmdList[0], curCommand)) {
                    cmdList.RemoveAt(0);
                }
                else {
                    // Exec完后，命令可能发生改变 by吴江
                    cmdList.Remove(curCommand);
                }
            }
        }
    }


    /// <summary>
    /// 取消所有命令 by吴江
    /// </summary>
    public void CancelCommands () {
        if ( cmdList.Count > 0 ) {
            cmdList[0].OnCancel(actor);
            cmdList.Clear(); 
        }
    }

    /// <summary>
    /// 取出命令队列中的第一个 by吴江
    /// </summary>
    /// <returns></returns>
    public ActorCommand PopCommand () { 
        if (cmdList.Count > 0) {
            var cmd = cmdList[0];
            cmdList.RemoveAt(0); 
            return cmd;
        }
        return null;
    }

    /// <summary>
    /// 压入一个命令到命令队列的末尾 by吴江
    /// </summary>
    /// <param name="_cmd"></param>
    public void PushCommand ( ActorCommand _cmd ) {
        cmdList.Add(_cmd);
    } 


    /// <summary>
    /// 压入一个命令到命令队列的开头 by吴江
    /// </summary>
    /// <param name="_cmd"></param>
    public void InsertCommand ( ActorCommand _cmd ) {
        cmdList.Insert(0, _cmd);
    } 

    /// <summary>
    /// 当前的命令 by吴江
    /// </summary>
    /// <returns></returns>
    public ActorCommand CurrentCommand () { 
        if ( cmdList.Count > 0 ) 
            return cmdList[0];
        return null;
    }

    /// <summary>
    /// 是否有命令 by吴江
    /// </summary>
    /// <returns></returns>
    public bool HasCommand () {
        return cmdList.Count > 0;
    }
}


public class ActorCommand {
    //public virtual void OnCollision (Collider _collider) { }
    public virtual bool Exec ( Actor _actor ) { return true; }
    public virtual void OnCancel(Actor _actor) { }
}



public class Command_MoveTo : ActorCommand {
    public Vector3 destPos = Vector3.zero;
    public Vector3[] path = null;
    public float maxDistance = -1.0f;
    public bool ignoreY = false;

    //float firstTimeTick = 0;
    bool firstTime = true;


    public override bool Exec(Actor _actor)
    {
        if (GameCenter.curGameStage == null || !GameCenter.sceneMng.EnterSucceed)
        {
            return false;
        }
        //如果目标在使用技能，返回false ，等他用完 by吴江
        SmartActor sa = _actor as SmartActor;
        if (sa.isRigidity) //如果在僵直
        {
            return false;
        }
        if (sa != null && (sa.isCasting || sa.isCastingAttackEffect)) //如果在使用技能
        {
            sa.CancelAbility();
        }

        if (path == null)
        {
            path = GameStageUtility.StartPath(_actor.transform.position, destPos);

            if (path == null)
            {
                GameSys.Log("寻路失败!");
                _actor.StopMovingTo();
                MainPlayer player= _actor as MainPlayer;
                if (player != null)
                {
                    if (player.CannotMoveTo != null)
                        player.CannotMoveTo();
                }
                return true;
            }
            else
            {
                destPos = path[path.Length - 1];
            }
        }

        //检查是否超出最远距离 by吴江
        if (maxDistance > 0.0f)
        {
            Vector3 delta = destPos - _actor.transform.position;
            if (ignoreY) delta.y = 0.0f;
            if (delta.sqrMagnitude <= (maxDistance * maxDistance))
            {
                _actor.StopMovingTo();
                return true;
            }
        }


        if (firstTime)
        {
            if (_actor.isMoveLocked == false)
            {
                // firstTimeTick = Time.realtimeSinceStartup;
                firstTime = false;
                _actor.MoveTo(path, maxDistance, false);
            }

            return false;
        }

        if (maxDistance <= 0.0f)
        {
            Vector3 delta = destPos - _actor.transform.position;
            if (ignoreY) delta.y = 0.0f;
            if (delta.sqrMagnitude < (0.1 * _actor.CurRealSpeed * 0.1 * _actor.CurRealSpeed))
            {
                var player = _actor as SmartActor;
                if (player != null)
                {
                    _actor.StopMovingTo();
                }
                return true;
            }
        }

        if (_actor.IsMoving)
        {
            return false;
        }
        else
        {
            path = GameStageUtility.StartPath(_actor.transform.position, destPos);
            _actor.MoveTo(path, maxDistance, false);

            if (path == null || path.Length == 0)
            {
                return true;
            }
        }
        return false;
    }

}


public class Command_AutoPick : ActorCommand
{
    public Vector3 destPos = Vector3.zero;
    public MainPlayer player;
    public Vector3[] path = null;
    List<DropItemInfo> targetList = new List<DropItemInfo>();
    public List<DropItemInfo> cannotList = new List<DropItemInfo>();

    protected List<DropItemInfo> FilterDropItems
    {
        get
        {
            List<DropItemInfo> realList = new List<DropItemInfo>();
            List<DropItemInfo> itemList = GameCenter.sceneMng.GetDropItemInfoListByLimit(player);
            for (int i = 0, length = itemList.Count; i < length; i++)
            {
                if (!cannotList.Contains(itemList[i]))
                    realList.Add(itemList[i]);
            }
            return realList;
        }
    }

    protected DropItemInfo GetClosestDropItem(List<DropItemInfo> _targetList)
    {
        Vector3 curPos = GameCenter.curMainPlayer.transform.position;
        Vector3 targetPos = Vector3.zero;
        float distance = 0;
        DropItemInfo dropInfo = null;
        for (int i = 0,length=_targetList.Count; i < length; i++)
        {
            targetPos = ActorMoveFSM.LineCast(new Vector2(targetList[i].ServerPos.x,targetList[i].ServerPos.y), true);
            Vector3[] path = GameStageUtility.StartPath(targetPos, curPos);
            if (path != null)
            {
                float tempDistance = path.Length == 2 ? Vector3.Distance(targetPos, curPos) : path.CountPathDistance();
                if (distance == 0)
                {
                    distance = tempDistance;
                    dropInfo = _targetList[i];
                }
                else
                {
                    if (tempDistance < distance)
                    {
                        distance = tempDistance;
                        dropInfo = _targetList[i];
                    }
                }
            }
        }
        if (dropInfo != null) return dropInfo;
        return _targetList.Count > 0 ? _targetList[0] : null;
    }

    public override bool Exec(Actor _actor)
    {
        targetList = FilterDropItems;
        if (targetList.Count < 1)
        {
            return true;//所有物品都拾取完毕,结束命令
        }
        if (GameCenter.inventoryMng != null && GameCenter.inventoryMng.IsBagFull)
        {
            return true;//背包已满结束拾取
        }
        DropItemInfo targetOne = GetClosestDropItem(targetList);
        path = GameStageUtility.StartPath(_actor.transform.position, ActorMoveFSM.LineCast(new Vector3(targetOne.ServerPos.x, 0, targetOne.ServerPos.y), true));
        if (path == null)
        {
            //掩码里的物品,加入黑名单
            if (!cannotList.Contains(targetOne))
                cannotList.Add(targetOne);
            if (player != null && !player.blackDropItemList.Contains(targetOne))
                player.blackDropItemList.Add(targetOne);
        }
        else
        {
            destPos = path[path.Length - 1];
        }
        if (!_actor.IsMoving)
        {
            if (player.isRigidity)
            {
                return false;
            }
            Vector3 delta = destPos - _actor.transform.position;
            if (delta.sqrMagnitude <= (0.3 * 0.3))
            {
                DropItem dropItem = GameCenter.curGameStage.GetDropItem(targetOne.ServerInstanceID);
                if (dropItem != null && dropItem.HasShowName)
                {
                    //尝试拾取的物品,不管拾取是否成功,加入黑名单
                    if (!cannotList.Contains(targetOne))
                        cannotList.Add(targetOne);
                    if (player != null && !player.blackDropItemList.Contains(targetOne))
                        player.blackDropItemList.Add(targetOne);
                }
                return false;
            }
            if (GameCenter.sceneMng.GetDropItemInfo(targetOne.ServerInstanceID) == null)
            {
                return true;
            }
            _actor.MoveTo(path, 0.3f, false);
        }
        return false;
    }
}







public class Command_FlyTo : ActorCommand
{
	public int targetScene = 0;
	public Vector3 targetPos = Vector3.zero;
	public int targetID = 0;

	public float minDistance = 2.0f;

	protected bool firstTime = true;

	public override bool Exec (Actor _actor)
	{
		if(firstTime)
		{
			if (targetScene == 0)
			{
				GameCenter.mainPlayerMng.C2S_Fly_Pint(GameCenter.mainPlayerMng.MainPlayerInfo.SceneID, (int)targetPos.x, (int)targetPos.z);
			}
			else
			{
				GameCenter.mainPlayerMng.C2S_Fly_Pint(targetScene, (int)targetPos.x, (int)targetPos.z);
			}
			firstTime = false;
		}
		if(targetScene != 0 && targetScene != GameCenter.mainPlayerMng.MainPlayerInfo.SceneID)
		{
			return false;
		}
		if((targetScene != 0 && targetScene == GameCenter.mainPlayerMng.MainPlayerInfo.SceneID && GameCenter.sceneMng.EnterSucceed) || targetScene == 0)
		{
			Vector3 delta = targetPos - _actor.transform.position;
			delta.y = 0f;
			if(targetID == 0)
			{
				if(delta.sqrMagnitude < minDistance*minDistance)
				{
                    GameCenter.curMainPlayer.PlayFlyOverEffect();
					return true;
				}
			}else
			{
				if(delta.sqrMagnitude < minDistance*minDistance)
				{
					InteractiveObject obj = GameCenter.curGameStage.GetInterActiveObj(targetID);
					if(obj != null && !obj.isDummy)
					{
						switch(obj.typeID)
						{
						case ObjectType.NPC:
							NPC npc = obj as NPC;
							GameCenter.curMainPlayer.CurTarget = npc;
							if (npc != null)
							{
								npc.BeClick();
							}
							NPCTypeRef npcType = ConfigMng.Instance.GetNPCTypeRef(npc.id);
							if(npcType.function1 != 0){
								NewFunctionRef newFunction = ConfigMng.Instance.GetNewFunctionRef(npcType.function1);
								if(GameCenter.mainPlayerMng.FunctionIsOpen((FunctionType)newFunction.Function_type)){
									if(newFunction.NPC_UI_type == 1){
										GameCenter.uIMng.SwitchToUI((GUIType)Enum.Parse(typeof(GUIType), newFunction.NPC_UI_name));
									}else if(newFunction.NPC_UI_type == 2){
										GameCenter.uIMng.SwitchToSubUI((SubGUIType)Enum.Parse(typeof(SubGUIType), newFunction.NPC_UI_name));
									}
								}
								else{
									GameCenter.uIMng.SwitchToUI(GUIType.NPCDIALOGUE);
								}
							}else if(npcType.function2 != 0){
								NewFunctionRef newFunction = ConfigMng.Instance.GetNewFunctionRef(npcType.function2);
								if(GameCenter.mainPlayerMng.FunctionIsOpen((FunctionType)newFunction.Function_type)){
									if(newFunction.NPC_UI_type == 1){
										GameCenter.uIMng.SwitchToUI((GUIType)Enum.Parse(typeof(GUIType), newFunction.NPC_UI_name));
									}else if(newFunction.NPC_UI_type == 2){
										GameCenter.uIMng.SwitchToSubUI((SubGUIType)Enum.Parse(typeof(SubGUIType), newFunction.NPC_UI_name));
									}
								}else{
									GameCenter.uIMng.SwitchToUI(GUIType.NPCDIALOGUE);
								}
							}else{
								GameCenter.uIMng.SwitchToUI(GUIType.NPCDIALOGUE);//走到NPC身边，打开NPC对话UI 
							}
							break;
						}
                        GameCenter.curMainPlayer.PlayFlyOverEffect();
						return true;
					}
				}
			}
		}
		return false;
	}
}

public class Command_SceneItem : ActorCommand
{
    //public int type = -1;

    //public override bool Exec(Actor _actor)
    //{
    //    SceneItemInfo info = GameCenter.sceneMng.GetItemInfo(type);
    //    if (info == null)
    //    {
    //        GameSys.LogError("取不到触发器的信息！命令执行中断!");
    //        return true;
    //    }

    //    SceneItem item = GameCenter.curGameStage.GetTriggerItem(type);
    //    if (item == null)
    //    {
    //        GameSys.LogError("取不到触发器的表现层对象！命令执行中断!");
    //        return true;
    //    }

    //    MainPlayer p = _actor as MainPlayer;
    //    if (p == null) return true;
    //    Vector3 diff = p.transform.position - item.transform.position;
    //    if (diff.sqrMagnitude > info.Scale * info.Scale)
    //    {
    //        p.MoveTo(item.transform.position);
    //        return false;
    //    }
    //    else
    //    {
    //        GameCenter.sceneMng.C2S_ActiveSceneItem(type);
    //    }
    //    return true;
    //}

    //public override void OnCancel()
    //{

    //}
}

public class Command_FlyPoint : ActorCommand
{
    public int type = -1;

    public override bool Exec(Actor _actor)
    {
        FlyPointRef flyPointRef = ConfigMng.Instance.GetFlyPointRef(type);
        if (flyPointRef == null)
        {
            GameSys.LogError("取不到传送点的配置信息！命令执行中断!");
            return true;
        }

        MainPlayer p = _actor as MainPlayer;
        if (p == null) return true;
        FlyPoint fp = GameCenter.curGameStage.GetObject(ObjectType.FlyPoint, type) as FlyPoint;
        FlyPoint.OnMainPlayerTriggerFlyPoint(p, fp);
        return true;
    }

}



public class Command_TraceTarget : ActorCommand
{
    public InteractiveObject target = null;
    public float minDistance = 1.0f;
    public bool ignoreY = true;
    public AbilityInstance abilityInstance = null;
    public float updatePathDeltaTime = 0.1f;

    int id = -1;
    bool firstTime = true;
    protected Vector3 destPos = Vector3.zero;
    protected Vector3 DestPos
    {
        get
        {
            return destPos;
        }
        set
        {
            destPos = value;
        }
    }
    protected Vector3 lastRealPos = Vector3.zero;
    ObjectType typeID = ObjectType.Unknown;
    bool dummyWaitForLoad = true;
    float updatePathTime = 0;

    protected bool haveReqDownRide = false;

    public override bool Exec(Actor _actor)
    {
        if (target == null)
        {
            return true;
        }
        if (target.isDummy)
        { // 当目标为null时，这里不能返回true，必须等目标创建
            id = target.id;
            typeID = target.typeID;
            dummyWaitForLoad = true;
        }
        if (GameCenter.curGameStage == null || !GameCenter.sceneMng.EnterSucceed)
        {
            return false;
        }
        if (dummyWaitForLoad)
        {
            if (target == null)
            {
                target = GameCenter.curGameStage.GetObject(typeID, id);
                dummyWaitForLoad = false;
            }
        }

        Actor targetActor = target as Actor;
        if (targetActor != null && targetActor.isDead)
            return true;


        SmartActor smartActor = (_actor as SmartActor);
        if (smartActor != null)
        {
            if (smartActor.isRigidity) return false;
            if (smartActor.isDead) return true;
        }

        Vector3 targetPos = target.transform.position;
        Vector3 diff = targetPos - _actor.transform.position;
        bool keepMoving = false;

        if (abilityInstance != null)
        {
            keepMoving = !abilityInstance.CanPushAbilityCommand(target);//如果到了可以施放技能的距离，则停止移动，开始施放技能
            if (!keepMoving)
            {
                _actor.StopMovingTo();
                _actor.FaceToNoLerp(diff);

                MainPlayer player = _actor as MainPlayer;//释放技能前也需要下马
                if (player != null && player.actorInfo != null && player.actorInfo.CurMountInfo != null && player.actorInfo.CurMountInfo.IsRiding)
                {
                    if (haveReqDownRide == false)
                    {
                        GameCenter.newMountMng.C2S_ReqRideMount(ChangeMount.DOWNRIDE, player.actorInfo.CurMountInfo.ConfigID, MountReqRideType.AUTO);
                        haveReqDownRide = true;
                    }
                    return false;
                }
                return true;
            }
        }
        if (diff.sqrMagnitude <= minDistance * minDistance)
        {
            _actor.StopMovingTo();
            _actor.FaceToNoLerp(diff);
            return true;
        }

        DestPos = targetPos;// -Vector3.Normalize(diff) * 2.0f;
        if (firstTime)
        {    // 这里确保在移动中，防止自动挂机卡住
            firstTime = false;
            Vector3[] path = GameStageUtility.StartPath(_actor.transform.position, DestPos);
            _actor.MoveTo(path);
            return false;
        }
        else
        {
            if (!_actor.IsMoving)
            {
                Vector3[] path = GameStageUtility.StartPath(_actor.transform.position, DestPos);
                if (path == null)
                {
                    _actor.ChangeTarget();
                    return true;
                }
                else
                {
                    _actor.MoveTo(path);
                }
                return false;
            }

            if (Time.time >= updatePathTime && updatePathDeltaTime > 0.0f)
            {
                Vector3[] path = GameStageUtility.StartPath(_actor.transform.position, DestPos);
                _actor.MoveTo(path); // 定位新的目标的位置
                updatePathTime = Time.time + updatePathDeltaTime;
            }
        }

        return false;
    }
}


/// <summary>
/// 走到任务节点目标后的操作 by吴江
/// </summary>
public class CommandTriggerTaskTarget : ActorCommand
{
    public InteractiveObject target = null;
    public float minDistance = 1.0f;

    public override bool Exec(Actor _actor)
    {
        if (target == null)
        {
            return true;
        }

        if (target.isDummy)
        {
            return false;
        }
        if (GameCenter.curGameStage == null || !GameCenter.sceneMng.EnterSucceed)
        {
            return false;
        }


        Vector3 targetPos = target.transform.position;
        Vector3 diff = _actor.transform.position - targetPos;

        if (diff.sqrMagnitude <= minDistance * minDistance)
        {
            target.InteractionSound();
            switch (target.typeID)
            {
                case ObjectType.NPC:
                    NPC npc = target as NPC;
                    GameCenter.curMainPlayer.CurTarget = npc;
                    //if(!GameCenter.taskMng.IsOpenDialog)
                    GameCenter.uIMng.SwitchToUI(GUIType.NPCDIALOGUE);
                    //GameCenter.taskMng.SetNPCDialogActive(false);
                    break;
                case ObjectType.FlyPoint:
                    FlyPoint fly = target as FlyPoint;
                    MainPlayer mp = _actor as MainPlayer;
                    if (mp != null && fly != null)
                    {
                        mp.CancelCommands();
                        Command_MoveTo cmdMoveTo = new Command_MoveTo();
                        cmdMoveTo.destPos = fly.gameObject.transform.position;
                        cmdMoveTo.maxDistance = 0f;
                        mp.commandMng.PushCommand(cmdMoveTo);


                        //Command_FlyPoint cmdFly = new Command_FlyPoint();
                        //cmdFly.type = fly.RefData.id;
                        //GameCenter.curMainPlayer.commandMng.PushCommand(cmdFly);
                    }
                    break;
                case ObjectType.SceneItem:
                    SceneItem sceneitem = target as SceneItem;
                    if (sceneitem != null && sceneitem.IsTouchType == TouchType.TOUCH)
                    {
                        if (GameCenter.mainPlayerMng.MainPlayerInfo.CurMountInfo != null && GameCenter.mainPlayerMng.MainPlayerInfo.CurMountInfo.IsRiding)//下坐骑
                        {
                            GameCenter.newMountMng.C2S_ReqRideMount(ChangeMount.DOWNRIDE, GameCenter.mainPlayerMng.MainPlayerInfo.CurMountInfo.ConfigID, MountReqRideType.AUTO);
                        }
                    }
                    break;
            }
            return true;
        }
        return false;
    }
}

/// <summary>
/// 走到终点执行操作
/// </summary>
public class CommandTriggerTarget : ActorCommand
{
    public InteractiveObject target = null;
    public float minDistance = 1.5f;

    public override bool Exec(Actor _actor)
    {
        if (target == null)
        {
            return true;
        }

        if (target.isDummy)
        {
            return false;
        }
        if (GameCenter.curGameStage == null || !GameCenter.sceneMng.EnterSucceed)
        {
            return false;
        }

        Vector3 targetPos = target.transform.position;
        Vector3 diff = _actor.transform.position - targetPos;

        if (diff.sqrMagnitude <= minDistance * minDistance)
        {
			GameCenter.curMainPlayer.CurTarget = target;
            target.InteractionSound();
            switch (target.typeID)
            {
                case ObjectType.Unknown:
                    break;
                case ObjectType.Player:
                    break;
                case ObjectType.PreviewPlayer:
                    break;
                case ObjectType.Mount:
                    break;
                case ObjectType.MOB:
                    break;
                case ObjectType.NPC:
                    if (GameCenter.uIMng.HaveBigWnd())
                    {
                        return true;
                    }
                    List<NPC> npcs = GameCenter.curGameStage.GetNPCs();
                    for (int i = 0; i < npcs.Count; i++)
                    {
                        npcs[i].CancelBubble();
                    }
                    NPC npc = target as NPC;
					
                    if (npc != null)
                    {
                        npc.BeClick();
                    }
					NPCTypeRef npcType = ConfigMng.Instance.GetNPCTypeRef(npc.id);
					if(npcType.function1 != 0){
						NewFunctionRef newFunction = ConfigMng.Instance.GetNewFunctionRef(npcType.function1);
						if(GameCenter.mainPlayerMng.FunctionIsOpen((FunctionType)newFunction.Function_type)){
							if(newFunction.NPC_UI_type == 1){
								GameCenter.uIMng.SwitchToUI((GUIType)Enum.Parse(typeof(GUIType), newFunction.NPC_UI_name));
							}else if(newFunction.NPC_UI_type == 2){
								GameCenter.uIMng.SwitchToSubUI((SubGUIType)Enum.Parse(typeof(SubGUIType), newFunction.NPC_UI_name));
							}
						}
						else{
							GameCenter.uIMng.SwitchToUI(GUIType.NPCDIALOGUE);
						}
					}else if(npcType.function2 != 0){
						NewFunctionRef newFunction = ConfigMng.Instance.GetNewFunctionRef(npcType.function2);
						if(GameCenter.mainPlayerMng.FunctionIsOpen((FunctionType)newFunction.Function_type)){
							if(newFunction.NPC_UI_type == 1){
								GameCenter.uIMng.SwitchToUI((GUIType)Enum.Parse(typeof(GUIType), newFunction.NPC_UI_name));
							}else if(newFunction.NPC_UI_type == 2){
								GameCenter.uIMng.SwitchToSubUI((SubGUIType)Enum.Parse(typeof(SubGUIType), newFunction.NPC_UI_name));
							}
						}else{
							GameCenter.uIMng.SwitchToUI(GUIType.NPCDIALOGUE);
						}
					}else{
						GameCenter.uIMng.SwitchToUI(GUIType.NPCDIALOGUE);//走到NPC身边，打开NPC对话UI 
					}
                    break;
                case ObjectType.SceneItem:
                    SceneItem sceneitem = target as SceneItem;
                    if (sceneitem != null && sceneitem.IsTouchType == TouchType.TOUCH)
                    {
                        if (GameCenter.mainPlayerMng.MainPlayerInfo.CurMountInfo != null && GameCenter.mainPlayerMng.MainPlayerInfo.CurMountInfo.IsRiding)//下坐骑
                        {
                            GameCenter.newMountMng.C2S_ReqRideMount(ChangeMount.DOWNRIDE, GameCenter.mainPlayerMng.MainPlayerInfo.CurMountInfo.ConfigID, MountReqRideType.AUTO);
                        }
                    }
                    if (GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType == SceneUiType.RAIDERARK)
                    {
                        if (GameCenter.activityMng.rewardId != 0)//已携带宝珠错误提示
                        {
                            GameCenter.messageMng.AddClientMsg(456);
                            return true;
                        }
                        if (Time.time > 15 && Time.time - GameCenter.activityMng.coldGetTime < 15)
                        {
                            MessageST msg = new MessageST();
                            msg.messID = 459;
                            msg.words = new string[1] { (15 - (int)(Time.time - GameCenter.activityMng.coldGetTime)).ToString() };
                            GameCenter.messageMng.AddClientMsg(msg);
                            return true;
                        }
                    }
                    if (sceneitem != null) sceneitem.BeClicked();
                    GameCenter.mainPlayerMng.C2S_TouchSceneItem(target.id);
                    break;
                case ObjectType.Entourage:
                    break;
                case ObjectType.DropItem:
                    break;
                case ObjectType.FlyPoint:
                    break;
                case ObjectType.CGObject:
                    break;
                case ObjectType.Trap:
                    break;
                case ObjectType.Model:
                    //Model model = target as Model;
//                    if (model.Camp == GameCenter.mainPlayerMng.MainPlayerInfo.Camp)//走到雕像 如果阵营相同 请求能否膜拜并开窗
//                    GameCenter.campMng.C2S_CanWorship();
					GameCenter.uIMng.SwitchToUI(GUIType.NPCMORSHIP);
                    break;
                default:
                    break;
            }
            return true;
        }
        return false;
    }
}


public class Command_CastAbilityOn : ActorCommand
{
    public AbilityInstance abilityInstance = null;
    public InteractiveObject target = null;
    public bool forceNeedWalkable = false;

    public override bool Exec(Actor _actor)
    {
        SmartActor smartActor = (_actor as SmartActor);

        if (abilityInstance.thisSkillMode == SkillMode.NORMALSKILL && smartActor.IsProtecting)
        {
            return false;
        }
        if (smartActor == null || smartActor.isRigidity)
        {
            return false;
        }
        if (abilityInstance == null)
        {
            return true;
        }

        if (GameCenter.curGameStage == null || !GameCenter.sceneMng.EnterSucceed)
        {
            return false;
        }

        if (smartActor.isDead) return true;//如果自己死了，什么也不干了


        if (target == null)
        {
            if (abilityInstance.NeedTarget)
            {
                return true;
            }
            smartActor.UseAbility(abilityInstance);//没目标或者目标死了，直接放技能，策划的要求  by吴江
            return true;
        }


        Actor targetActor = target as Actor;
        if (targetActor != null && targetActor.isDead) //没目标或者目标死了，直接放技能，策划的要求  by吴江
        {
            smartActor.UseAbility(abilityInstance);
            return true;
        }
        //==========修改,非普通攻击的技能可以空放!
        if (abilityInstance.thisSkillMode == SkillMode.NORMALSKILL)
        {
			if (!abilityInstance.NeedTarget || abilityInstance.CanUseFor(target))
            {
                smartActor.UseAbility(abilityInstance);
                return true;
            }
            else
            {
                PlayerBase p = smartActor as PlayerBase;
                if (p != null)
                {
                    p.FaceTo(target.transform.position);
                    //p.UseAbility(p.LoseHitAbility);
                }
                return true;
            }
        }
        else
        {
            smartActor.UseAbility(abilityInstance);
        }
        return true;
    }

    public override void OnCancel(Actor _actor)
    {
        base.OnCancel(_actor);
        MainPlayer m = _actor as MainPlayer;
        if (m != null)
        {
            if (m.curTryUseAbility == abilityInstance)
            {
                m.curTryUseAbility = null;
            }
        }
    }
}



public class Command_CheckIfDistanceValid : ActorCommand {
    public Vector3 targetPosition = Vector3.zero;
    public float minDistance = 0.0f;
    public bool ignoreY = true;

    public override bool Exec ( Actor _actor ) { 
        // check if we stop
        Vector3 delta = targetPosition - _actor.transform.position;
        if ( ignoreY ) delta.y = 0.0f;
        if ( delta.sqrMagnitude <= (minDistance * minDistance) ) {
            return true;
        }

        return false;
    }
}

