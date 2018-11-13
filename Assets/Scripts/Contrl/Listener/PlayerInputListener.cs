//=============================
//作者：吴江
//日期：2015/5/15
//用途：玩家输入收听类
//=======================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInputListener : MonoBehaviour {


    public static Camera mainCamera;


    public class PointData
    {
        public Vector3 pos;
        public RaycastHit hit;

        public static PointData creatPointData(Vector3 pos, LayerMask Hitlayer)
        {

            if (UICamera.Raycast(pos) || PlayerInputListener.mainCamera == null)//检测是否按到了UI控件
            {
                return null;
            }


            Ray _ray = PlayerInputListener.mainCamera.ScreenPointToRay(pos);

            RaycastHit _hit;
            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, Hitlayer.value))
            {
                PointData d = new PointData();
                d.pos = pos;
                d.hit = _hit;
                return d;
            }
            return null;
        }

        public override string ToString()
        {

            return "Pos=" + pos + "  HitObject " + hit.collider.gameObject;
        }
    }

    public enum LockType
    {
        /// <summary>
        /// 没有锁定
        /// </summary>
        NONE,
        /// <summary>
        /// 正在播放动画
        /// </summary>
        SCENE_ANIM_PROCESS,
        /// <summary>
        /// 自己已经播放完动画,但是在等待队友
        /// </summary>
        SCENE_ANIM_WAIT_MATE,
        /// <summary>
        /// 限制移动的buff(恐惧)
        /// </summary>
        CTRL_BUFF,
    }


    [System.NonSerialized]
    public MainPlayer target = null;

    public LayerMask Hitlayer = -1;

    public PointData[] pointDatas = null;

    protected List<LockType> curLockTypeList = new List<LockType>();


    public void AddLockType(LockType _type)
    {
        curLockTypeList.Add(_type);
    }

    public void RemoveLockType(LockType _type)
    {
        curLockTypeList.Remove(_type);
    }

    void Start()
    {
        mainCamera = GameCenter.cameraMng.mainCamera;
        Hitlayer.value = LayerMng.mouseInputLayerMask;
        target = GetComponent<MainPlayer>();
    }




    protected void TouchPoint()
    {
        if (Input.touchCount == 0)
            return;
        if (Input.touchCount > 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved) //这个手势是缩放手势
            {
                return;
            }
        }
        List<PointData> list = new List<PointData>();
        for (int i = 0; i < Input.touchCount; ++i)
        {
            Touch input = Input.GetTouch(i);
            bool unpressed = (input.phase == TouchPhase.Canceled) || (input.phase == TouchPhase.Ended);
            if (!unpressed)//只处理触摸弹起的事件
                continue;
            PointData d = PointData.creatPointData(new Vector3(input.position.x, input.position.y, 0), Hitlayer.value);
            if (d != null)
            {
                list.Add(d);
            }
        }
        if (list.Count > 0)
        {
            pointDatas = list.ToArray();
            list.Clear();
            list = null;
        }
    }

    protected void MousePoint()
    {
        if (Input.GetMouseButtonUp(0))
        {
			PointData data = PointData.creatPointData(Input.mousePosition, Hitlayer.value);
			if (data != null)
			{
				pointDatas = new PointData[] { data };
			}
        }
    }
    /// <summary>
    /// 清理所有的操作锁定
    /// </summary>
    public void CleanLock()
    {
        curLockTypeList.Clear();
    }

    /// <summary>
    /// 检查当前是否锁定操作
    /// </summary>
    /// <returns></returns>
    public bool CheckLock()
    {
        if (curLockTypeList.Count == 0) return true;
        switch (curLockTypeList[0])
        {
            case LockType.NONE:
                return true;
            case LockType.SCENE_ANIM_PROCESS:
                return false;
            case LockType.SCENE_ANIM_WAIT_MATE:
               // GameCenter.messageMng.AddClientMsg(111);
                return false;
            case LockType.CTRL_BUFF:
                GameCenter.messageMng.AddClientMsg(58);
                return false;
            default:
                return true;
        }

    }

    private void doPointDatas()
    {
        if (pointDatas == null)
        {
            return;
        }
        if (pointDatas.Length == 0)
        {
            return;
        }

        if (!CheckLock())
        {
            pointDatas = null;
            return;
        }
        UIMng.isClosingWnd = false; 
        foreach (PointData d in pointDatas)
        {
            if (d != null)
            {
                GameObject hitObj = d.hit.collider.gameObject;
                InteractiveObject interactive = hitObj.GetComponent<InteractiveObject>();
                if (interactive != null)
                {
                    switch (interactive.typeID)
                    {
                        case ObjectType.Unknown:
                            break;
                        case ObjectType.Player:
                              OnClickOPC(interactive as OtherPlayer);
                            break;
                        case ObjectType.MOB:
                              OnClickMonster(interactive as Monster);
                            break;
                        case ObjectType.NPC:
                              OnClickNpc(interactive as NPC);
                            break;
                        case ObjectType.DropItem:
                            OnClickDropItem(interactive as DropItem);
                            break;
                        case ObjectType.FlyPoint:
                              OnClickFlyPoint(interactive as FlyPoint);
                            break;
                        case ObjectType.SceneItem:
                              OnClickSceneItem(interactive as SceneItem);
                            break;
                        case ObjectType.Entourage:
                            OnClickEntourage(interactive as EntourageBase);
                            break;
                        case ObjectType.Model:
                            OnClickModel(interactive as Model);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    OnClickTerrain(d);
                }
            }
        }
        pointDatas = null;
    }

    protected bool hasKeyboardMoving = false;

    /// <summary>
    /// 过场动画内双击事件
    /// </summary>
    public System.Action<Vector2> OnSceneAnimaDoubleClickEvent;


    /// <summary>
    /// 过场动画内单击事件
    /// </summary>
    public System.Action<Vector2> OnSceneAnimaSingleClickEvent;

    void OnGUI()
    {
        //if (fsm.SceneAnimStage.IsWatching)
        //{
        //    Event click = Event.current;
        //    if (click != null && click.type == EventType.MouseDown)
        //    {
        //        if (click.clickCount == 2)
        //        {
        //            if (OnSceneAnimaDoubleClickEvent != null)
        //            {
        //                OnSceneAnimaDoubleClickEvent(click.mousePosition);
        //            }
        //        }
        //        else if (click.clickCount == 1)
        //        {
        //            if (OnSceneAnimaSingleClickEvent != null)
        //            {
        //                if (OnSceneAnimaSingleClickEvent != null) OnSceneAnimaSingleClickEvent(click.mousePosition);
        //            }
        //        }
        //    }
        //}
    }

    int time = 0;
    int lastTime = -1;
    void Update()
    {

        //if (!fsm.SceneAnimStage.IsWatching)
        //{
        if (!UIMng.isClosingWnd && !UICamera.isOverUI && !hasKeyboardMoving)//解决主界面点击穿透的BUG  by 易睿
        {
            if (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer)
            {
                TouchPoint();
            }
            else
            {
                MousePoint();
            }
        }
        else
        {
            pointDatas = null;
            UIMng.isClosingWnd = false;
        }

        //}
        //else
        //{
        //}
        doPointDatas();
        if (Application.platform == RuntimePlatform.Android ||
       Application.platform == RuntimePlatform.IPhonePlayer)
        {
            //Debug.Log("进入省电模式的判断");
            if (Input.touchCount > 0)
            {
                time = SystemSettingMng.POWERSAVING_TIME + (int)Time.time;
                if (GameCenter.systemSettingMng.OnPowerSavingEvent != null)
                {
                    GameCenter.systemSettingMng.OnPowerSavingEvent();
                }
            }

        }
        else
        {
            if (Input.anyKeyDown)
            {
                time = SystemSettingMng.POWERSAVING_TIME + (int)Time.time;
                if (GameCenter.systemSettingMng.OnPowerSavingEvent != null)
                {
                    GameCenter.systemSettingMng.OnPowerSavingEvent();
                }
            }

        }
        if (time - (int)Time.time < SystemSettingMng.POWERSAVING_TIME && time - (int)Time.time >= 0)
        {
            if (lastTime != (int)Time.time)
            {
                int showTime;
                showTime = time - (int)Time.time;
                int curTime = showTime % SystemSettingMng.POWERSAVING_TIME;
                if (curTime == 0)
                {
                    if (!GameCenter.systemSettingMng.IsPowerSaving)
                    {
                        //GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                        //Debug.Log("进入省电模式");
                        GameCenter.uIMng.GenGUI(GUIType.POWERSAVING, true);
                    }
                }
            }
        }
        lastTime = (int)Time.time;

        //if (Input.acceleration.x > 0 || Input.acceleration.y > 0 || Input.acceleration.z > 0)
        //{
        //    NGUIDebug.Log("重力加速度 x =" + Input.acceleration.x + " , y = " + Input.acceleration.y + " , z = " + Input.acceleration.z);
        //    if (!target.isRigidity && !target.IsProtecting)
        //    {
        //        target.Jump();
        //    }
        //}

        //if (Input.gyro.gravity.sqrMagnitude > 0)
        //{
        //    NGUIDebug.Log("陀螺仪重力加速度 = " + Input.gyro.gravity);
        //    if (!target.isRigidity && !target.IsProtecting)
        //    {
        //        target.Jump();
        //    }
        //}

        if (GameCenter.instance.isDevelopmentPattern)
        {
            if (Input.GetKeyDown(KeyCode.B))  //影响键盘输入  注释掉 
            {
                //target.AbilityTest();
            }
            //测试用 by吴江
            //if ( Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Z))  //影响键盘输入  注释掉 
            //{
            //    target.BuffTest(1, true);
            //}
            //if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.X))  //影响键盘输入  注释掉 
            //{
            //    target.BuffTest(33,true);
            //}
            //if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.C))  //影响键盘输入  注释掉 
            //{
            //    target.BuffTest(34, true);
            //}
            //if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.V))  //影响键盘输入  注释掉 
            //{
            //    target.BuffTest(36, true);
            //}
            //if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Z))  //影响键盘输入  注释掉 
            //{
            //    target.BuffTest(1, false);
            //}
            //if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.X))  //影响键盘输入  注释掉 
            //{
            //    target.BuffTest(33, false);
            //}
            //if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.C))  //影响键盘输入  注释掉 
            //{
            //    target.BuffTest(34, false);
            //}
            //if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.V))  //影响键盘输入  注释掉 
            //{
            //    target.BuffTest(36, false);
            //}

            //if (Input.GetKey(KeyCode.I))//测试 试炼场 by龙英杰
            //{
            //    GameCenter.uIMng.SwitchToUI(GUIType.WILDBOSS);
            //}

            //if (Input.GetKey(KeyCode.P))//测试每日活动LZR
            //{

            //  //  GameCenter.uIMng.SwitchToUI(GUIType.DailyActivity);
            //    GameCenter.uIMng.SwitchToUI(GUIType.RANDCHEST);
            //}
            if (Input.GetKey(KeyCode.O))//测试每日活动LZR
            {
               // GameCenter.uIMng.SwitchToUI(GUIType.CAMPJOIN);
             //   GameCenter.uIMng.SwitchToUI(GUIType.MAILBOX);
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
             //   GameCenter.sceneAnimMng.PushSceneAnima(GameCenter.instance.cur_Text_SceneAnima_ID);
            }


            //if(Input.GetKeyDown(KeyCode.Alpha1))
            //{
            //    SkillWnd wnd = GameCenter.uIMng.GetGui<SkillWnd>();
            //    if (wnd != null)
            //    {
            //        wnd.OnClickSkillBtnActon(0);
            //    }
            //}
            //else if (Input.GetKeyDown(KeyCode.Alpha2))
            //{
            //    SkillWnd wnd = GameCenter.uIMng.GetGui<SkillWnd>();
            //    if (wnd != null)
            //    {
            //        wnd.OnClickSkillBtnActon(1);
            //    }
            //}
            //else if (Input.GetKeyDown(KeyCode.Alpha3))
            //{
            //    SkillWnd wnd = GameCenter.uIMng.GetGui<SkillWnd>();
            //    if (wnd != null)
            //    {
            //        wnd.OnClickSkillBtnActon(2);
            //    }
            //}
            //else if (Input.GetKeyDown(KeyCode.Alpha4))
            //{
            //    SkillWnd wnd = GameCenter.uIMng.GetGui<SkillWnd>();
            //    if (wnd != null)
            //    {
            //        wnd.OnClickSkillBtnActon(3);
            //    }
            //}

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                GameCenter.curMainPlayer.ChangeTarget();
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                
                //GameCenter.uIMng.SwitchToUI(GUIType.CAMPMAIN);
                //GameCenter.campMng.C2S_AskCampActivity(CampAction.ACTIVITY);
                //  GameCenter.mainPlayerMng.protectMinerMng.FinalData = new FinalSettlemenInfo();
                //GameCenter.uIMng.SwitchToUI(GUIType.MAILBOX);
            }
            //if (Input.GetKey(KeyCode.L))
            //{
            //    GameCenter.chapterRewardMng.Set();
            //}
            if (Input.GetKey(KeyCode.Space))
            {
                target.Jump();
            }
            if (!isDragingRockerItem)
            {
                hasKeyboardMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ||
                    Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A);
                if (hasKeyboardMoving)
                {
                    Vector3 vWS = Vector3.zero;
                    Vector3 vAD = Vector3.zero;

                    Transform mainCamTrasform = GameCenter.cameraMng.mainCamera.transform;
                    if (Input.GetKey(KeyCode.W))
                    {
                        vWS += mainCamTrasform.forward;
                    }
                    if (Input.GetKey(KeyCode.S))
                    {
                        vWS -= mainCamTrasform.forward;
                    }

                    if (Input.GetKey(KeyCode.D))
                    {
                        vAD += mainCamTrasform.right;
                    }
                    if (Input.GetKey(KeyCode.A))
                    {
                        vAD -= mainCamTrasform.right;
                    }

                    if (!target.isRigidity)
                    {
                        target.CancelAbility(true);
                    }

                    target.AttakType = MainPlayer.AttackType.NONE;
                    target.CancelCommands();

                    Vector3 dir = vWS + vAD;
                    dir.y = 0.0f;
                    dir.Normalize();

                    if (dir == Vector3.zero)
                    {
                        target.StopMovingTowards();
                    }
                    else
                    {
                        target.MoveTowards(dir);
                    }
                }
                else if (target.isMovingTowards)
                {
                    target.StopMovingTowards();
                }
            }
        }


    }


	public void OnClickTerrain(PointData pointData)
	{
        if (target.GetMoveFSM().isMoveLocked)
        {
           // GameCenter.messageMng.AddClientMsg(199);//提示玩家，正在晕眩中，不能移动 by吴江
            return;
        }
        NavMeshHit navHit = new NavMeshHit();
        Vector3 destPos = Vector3.zero;
		if (NavMesh.SamplePosition(pointData.hit.point, out navHit, 10.0f, NavMesh.AllAreas))
        {
            destPos = navHit.position;
        }
        else
        {
            destPos = pointData.hit.point;
        }
        OnClickTerrain(destPos);
	}

    /// <summary>
    /// 是否正在拖拽摇杆移动
    /// </summary>
    public static bool isDragingRockerItem = false;

    public void OnClickTerrain(Vector3 _hitpos)
    {
        if (target.GetMoveFSM().isMoveLocked)
        {
            // GameCenter.messageMng.AddClientMsg(199);//提示玩家，正在晕眩中，不能移动 by吴江
            return;
        }
        if (isDragingRockerItem)
        {
            isDragingRockerItem = false;
            return;
        }
        //target.AttakType = MainPlayer.AttackType.NONE;
        //target.GoNormal();
		if (target.CurFSMState == MainPlayer.EventType.AI_FIGHT_CTRL || target.CurFSMState == MainPlayer.EventType.AI_DART_CTRL) //应策划要求，自动战斗时的点击地面，不能直接跳转到普通，而只是临时执行。
        {
            target.BreakAutoFight();
        }else
		{
			target.GoNormal();
		}
        if (target.CurTarget != null)
        {
            if (GameCenter.curGameStage is CityStage)
            {
                if (target.CurTarget is OtherPlayer || target.CurTarget is NPC)
                {
                    target.CurTarget = null;
                }
            }
            else
            {
                if (target.CurTarget is NPC)
                {
                    target.CurTarget = null;
                }
            }
        }
		Command_MoveTo cmdMoveTo = new Command_MoveTo();
		cmdMoveTo.destPos = _hitpos;
		cmdMoveTo.maxDistance = 0.5f;
		target.CancelCommands();
		target.commandMng.PushCommand(cmdMoveTo);

        GameCenter.spawner.SpawnIndicator(_hitpos, Quaternion.identity);
		GameCenter.uIMng.OpenFuncShowMenu(false);
    }


    protected void OnClickFlyPoint(FlyPoint _flyPoint)
    {
        if (_flyPoint == null || target == null) return;
        target.GoNormal();
        target.CurClickFlyPoint = _flyPoint;

        if (isDragingRockerItem)
        {
            return;
        }

        target.CancelCommands();
        Command_MoveTo cmdMoveTo = new Command_MoveTo();
        cmdMoveTo.destPos = _flyPoint.gameObject.transform.position;
        cmdMoveTo.maxDistance = 0f;
        target.commandMng.PushCommand(cmdMoveTo);

        //Command_FlyPoint cmdFly = new Command_FlyPoint();
        //cmdFly.type = _flyPoint.RefData.id;
        //GameCenter.curMainPlayer.commandMng.PushCommand(cmdFly);

    }

    protected void OnClickDropItem(DropItem _dropitem)
    {
        if (_dropitem == null || _dropitem == null) return;
        target.GoNormal();


        if (isDragingRockerItem)
        {
            return;
        }

        target.CancelCommands();
        Command_MoveTo cmdMoveTo = new Command_MoveTo();
        cmdMoveTo.destPos = _dropitem.gameObject.transform.position;
        cmdMoveTo.maxDistance = 0f;
        target.commandMng.PushCommand(cmdMoveTo);
    }

    protected void OnClickEntourage(EntourageBase _entourage)
    {
//        if (_entourage == null || target == null) return;
//        if (_entourage.isDead) return;
//        target.CurTarget = _entourage;
//        if (isDragingRockerItem)
//        {
//            return;
//        }

//        if (PlayerAutoFightFSM.IsEnemy(_entourage))
//        {
//            target.CancelCommands();

//            target.SetAiFightTarget(_entourage);
//            if (target.CurFSMState == MainPlayer.EventType.AI_FIGHT_CTRL) //应策划要求，自动战斗时的点击，不能直接跳转到普通，而只是临时执行。
//            {
//                target.BreakAutoFight();
//            }
////            else
////            {
////                target.GoNormalFight();
////            }
//        }
    }

    protected void OnClickModel(Model _model)
    {
        if (_model == null || target == null) return;
        target.CurTarget = _model;
        target.GoNormal();

        if (isDragingRockerItem)
        {
            return;
        }

        //  target.autoAttak = MainPlayer.AttackType.NONE;
        target.CancelCommands();
        Command_MoveTo cmdMoveTo = new Command_MoveTo();
        Vector3 npcPos = _model.gameObject.transform.position;
        Vector3 myPos = target.transform.position;
        cmdMoveTo.destPos = npcPos - Vector3.Normalize(npcPos - myPos) * 2.0f;

        cmdMoveTo.maxDistance = 0f;
        target.commandMng.PushCommand(cmdMoveTo);

        CommandTriggerTarget cmdMoveToTarget = new CommandTriggerTarget();
        cmdMoveToTarget.minDistance = 3.5f;
        cmdMoveToTarget.target = _model;
        target.commandMng.PushCommand(cmdMoveToTarget);
    }

    protected void OnClickSceneItem(SceneItem _sceneItem)
    {
        if (_sceneItem == null || target == null) return;
        if (target.CurFSMState == MainPlayer.EventType.AI_FIGHT_CTRL) //应策划要求，自动战斗时的点击，不能直接跳转到普通，而只是临时执行。
        {
            target.BreakAutoFight();
        }
        else
        {
            target.GoNormal();
        }
        target.CancelCommands();
        Command_MoveTo cmdMoveTo = new Command_MoveTo();
        cmdMoveTo.destPos = _sceneItem.gameObject.transform.position;
        cmdMoveTo.maxDistance = 0f;
        target.commandMng.PushCommand(cmdMoveTo);

        switch (_sceneItem.IsTouchType)
        {
            case TouchType.TOUCH:
				GameCenter.curMainPlayer.CurTarget = _sceneItem;
                CommandTriggerTarget trigCmd = new CommandTriggerTarget();
                trigCmd.target = _sceneItem;
                target.commandMng.PushCommand(trigCmd);
                break;
            default:
                break;
        }
    }


    protected void OnClickNpc(NPC _npc)
    {
        if (_npc == null || target == null) return;
        target.CurTarget = _npc;
        target.GoNormal();

        if (isDragingRockerItem)
        {
            return;
        }

      //  target.autoAttak = MainPlayer.AttackType.NONE;
        target.CancelCommands();
        Command_MoveTo cmdMoveTo = new Command_MoveTo();
        Vector3 npcPos = _npc.gameObject.transform.position;
        Vector3 myPos = target.transform.position;
        cmdMoveTo.destPos = npcPos - Vector3.Normalize(npcPos - myPos) * 2.0f;

        cmdMoveTo.maxDistance = 0f;
        target.commandMng.PushCommand(cmdMoveTo);

        CommandTriggerTarget cmdMoveToTarget = new CommandTriggerTarget();
        cmdMoveToTarget.minDistance = 3.5f;
        cmdMoveToTarget.target = _npc;
        target.commandMng.PushCommand(cmdMoveToTarget);

    }
    // <summary>
    // 点击开始采集
    // </summary>
    //protected void OnClickCollectItem(CollectItem collectItem)
    //{
    //    if (collectItem == null || target == null) return;
    //    if(GameCenter.sceneMng.isCollecting && GameCenter.sceneMng.curCollectId == collectItem.id)
    //        return;
    //    if (target.CurFSMState == MainPlayer.EventType.AI_FIGHT_CTRL) //应策划要求，自动战斗时的点击，不能直接跳转到普通，而只是临时执行。
    //    {
    //        target.BreakAutoFight();
    //    }else
    //    {
    //        target.GoNormal();
    //    }
    //    target.commandMng.CancelCommands();
    //    target.CurTarget = collectItem;
    //    target.SetAiFightTarget(collectItem);
    //    target.SetInCombat(true);
    //    target.autoAttak = MainPlayer.AttackType.FIGHT;
    //    collectItem.BeClick();
    //}

    protected void OnClickMonster(Monster _mob)
    {
        if (_mob == null || target == null) return;
        if (_mob.isDead) return;   
        if (isDragingRockerItem)
        {
            return;
        }
		target.CurTarget = _mob;
		target.SetAiFightTarget(_mob);
		target.CancelCommands();
		if (target.CurFSMState == MainPlayer.EventType.AI_FIGHT_CTRL) //应策划要求，自动战斗时的点击，不能直接跳转到普通，而只是临时执行。
		{
			target.BreakAutoFight();
		}
		else
		{
			target.HitTargetOnce(_mob);
		}
    }

    protected void OnClickOPC(OtherPlayer _opc)
    {
        if (_opc == null || target == null) return;
        if (_opc.isDead) return;

        target.CurTarget = _opc;
		target.SetAiFightTarget(_opc);
        if (isDragingRockerItem)
        {
            return;
        }

        SceneType curType = GameCenter.curGameStage.SceneType;
        if (curType != SceneType.CITY)
        {
            if (target.CurFSMState == MainPlayer.EventType.AI_FIGHT_CTRL) //应策划要求，自动战斗时的点击，不能直接跳转到普通，而只是临时执行。
            {
                target.BreakAutoFight();
            }
            else
            {
                target.GoNormal();
            }
            target.CancelCommands();
           // target.SetAiFightTarget(_opc);
           // target.autoAttak = MainPlayer.AttackType.FIGHT;
        }
        else
        {
            target.CancelCommands();
            Command_MoveTo cmdMoveTo = new Command_MoveTo();
            Vector3 npcPos = _opc.gameObject.transform.position;
            Vector3 myPos = target.transform.position;
            cmdMoveTo.destPos = npcPos - Vector3.Normalize(npcPos - myPos) * 2.0f;

            cmdMoveTo.maxDistance = 0.8f;
            target.commandMng.PushCommand(cmdMoveTo);
        }
    }


}
