///////////////////////////////////////////////////////////////////////////////
// 作者：吴江
// 日期：2015/5/22
// 用途：地下城平台的状态机
///////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using UnityEngine.SceneManagement;

/// <summary>
/// 地下城平台的状态机  by吴江
/// </summary>
public class DungeonStage : PlayGameStage
{


    Dictionary<int, AbilityBallisticCurve> abilityBallisticCurveDic = new Dictionary<int, AbilityBallisticCurve>();


    #region 状态机

    protected override void InitStateMachine()
    {
        base.InitStateMachine();

        fsm.State awake = new fsm.State("awake", stateMachine);
        awake.onEnter += EnterAwakeState;
        awake.onAction += UpdateAwakeState;

        fsm.State newbieGuide = new fsm.State("newbieGuide", stateMachine);
        newbieGuide.onEnter += EnterNewbieGuideState;
        newbieGuide.onAction += UpdateNewbieGuideState;
        newbieGuide.onExit += ExitNewbieGuideState;


        fsm.State load = new fsm.State("load", stateMachine);
        load.onEnter += EnterLoadtate;
        load.onAction += UpdateLoadState;
        load.onExit += ExitLoadState;

        reconnectState = new fsm.State("reconnectState", stateMachine);
        reconnectState.onEnter += EnterReconnectState;
        reconnectState.onAction += UpdateReconnectState;
        reconnectState.onExit += ExitReconnectState;

        fsm.State run = new fsm.State("run", stateMachine);
        run.onEnter += EnterRunState;
        run.onAction += UpdateRunState;
        run.onExit += ExitRunState;

        fsm.State blackProcess = new fsm.State("blackProcess", stateMachine);
        blackProcess.onEnter += EnterBlackProcesstate;
        blackProcess.onAction += UpdateBlackProcesState;
        blackProcess.onExit += ExitBlackProcesState;


        awake.Add<fsm.EventTransition>(load, (int)EventType.LOAD);
        awake.Add<fsm.EventTransition>(newbieGuide, (int)EventType.NEWBIE_GUIDE);
        newbieGuide.Add<fsm.EventTransition>(load, (int)EventType.LOAD);
        newbieGuide.Add<fsm.EventTransition>(run, (int)EventType.RUN);
        load.Add<fsm.EventTransition>(run, (int)EventType.RUN);
		load.Add<fsm.EventTransition>(awake, (int)EventType.AWAKE);//加载的时候可能掉线重连
		load.Add<fsm.EventTransition>(newbieGuide, (int)EventType.NEWBIE_GUIDE);
        //run.Add<fsm.EventTransition>(sceneAnimState, (int)EventType.SCENE_ANIMATION);
        //sceneAnimState.Add<fsm.EventTransition>(run, (int)EventType.RUN);
        blackProcess.Add<fsm.EventTransition>(run, (int)EventType.RUN);
        //blackProcess.Add<fsm.EventTransition>(sceneAnimState, (int)EventType.SCENE_ANIMATION);
        blackProcess.Add<fsm.EventTransition>(load, (int)EventType.LOAD);
        //sceneAnimState.Add<fsm.EventTransition>(blackProcess, (int)EventType.BLACK_PROCESS);
        run.Add<fsm.EventTransition>(blackProcess, (int)EventType.BLACK_PROCESS);
        load.Add<fsm.EventTransition>(blackProcess, (int)EventType.BLACK_PROCESS);
        //load.Add<fsm.EventTransition>(reconnectState, (int)EventType.RECONNECT);

        reconnectState.Add<fsm.EventTransition>(run, (int)EventType.RUN);

        run.Add<fsm.EventTransition>(reconnectState, (int)EventType.RECONNECT);
        load.Add<fsm.EventTransition>(reconnectState, (int)EventType.RECONNECT);

        stateMachine.initState = awake;
    }

    /// <summary>
    /// 进入启动状态 by吴江
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    /// <param name="_event"></param>
    protected void EnterAwakeState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        myLastIP = Network.player.ipAddress;
        GameCenter.OnConnectStateChange += ConnectStateChange;
        SceneManager.LoadSceneAsync("DungeonStage");
        SceneID = (int)GameCenter.mainPlayerMng.MainPlayerInfo.SceneID;

        GameCenter.cameraMng.MainCameraActive(false);
        GameCenter.cameraMng.PreviewCameraActive(false);
    }

    /// <summary>
    /// 更新启动状态 by吴江
    /// </summary>
    /// <param name="_curState"></param>
    protected void UpdateAwakeState(fsm.State _curState)
    {
        if (SceneManager.GetActiveScene().name == "DungeonStage")
        {
                stateMachine.Send((int)EventType.LOAD);
        }
    }


    /// <summary>
    /// 进入新手登录的原画播放状态
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    /// <param name="_event"></param>
    protected void EnterNewbieGuideState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
    }

    protected void UpdateNewbieGuideState(fsm.State _curState)
    {
    }


    /// <summary>
    /// 退出新手加载状态 by吴江
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    /// <param name="_event"></param>
    protected virtual void ExitNewbieGuideState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
    }

    /// <summary>
    /// 进入加载状态 by吴江
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    /// <param name="_event"></param>
    protected void EnterLoadtate(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        GameCenter.cameraMng.MainCameraActive(true);
        GameCenter.cameraMng.BlackCoverAll(true);
		//隐藏弹出窗口
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
		GameCenter.uIMng.ReleaseGUI(GUIType.COPYMULTIPLEWND);
        GameCenter.uIMng.ReleaseGUI(GUIType.GUILDFIGHT);
		//加载场景
        if (_from.name == "awake")
        {
            DoStartLoad(sceneID);
        }
    }

    /// <summary>
    /// 更新加载状态 by吴江
    /// </summary>
    /// <param name="_curState"></param>
    protected void UpdateLoadState(fsm.State _curState)
    {
        if (loadSceneTaskList.Count == 0)
        {
            if (!GameCenter.instance.IsReConnecteding && !NetCenter.Connected && GameCenter.messageMng != null && GameCenter.loginMng.CurConnectServerType == LoginMng.ConnectServerType.Game)
            {
                stateMachine.Send((int)EventType.RECONNECT);
            }
            else
            {
                stateMachine.Send((int)EventType.RUN);
            }
        }
    }

    /// <summary>
    /// 推出加载状态 by吴江
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    /// <param name="_event"></param>
    protected virtual void ExitLoadState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        Regist();
		if(!(GameCenter.curGameStage is LoginStage))//解决:断线重连强制退出Load状态,回不到登陆界面 by邓成
		{
			sceneMng.C2S_EnterSceneSucceed();
			GameCenter.instance.GoRunDungeon();
		}
    }



    /// <summary>
    /// 进入黑屏状态 by吴江
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    /// <param name="_event"></param>
    protected void EnterBlackProcesstate(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        GameCenter.cameraMng.BlackCoverAll(true);
    }

    /// <summary>
    /// 更新黑屏状态 by吴江
    /// </summary>
    /// <param name="_curState"></param>
    protected void UpdateBlackProcesState(fsm.State _curState)
    {
    }

    /// <summary>
    /// 退出黑屏状态 by吴江
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    /// <param name="_event"></param>
    protected virtual void ExitBlackProcesState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        GameCenter.cameraMng.BlackCoverAll(false);
    }

    /// <summary>
    /// 加载结束，进入游戏运行状态 by吴江
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    /// <param name="_event"></param>
    protected void EnterRunState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (_from.name != "load" && _from.name != "newbieGuide")
        {
            GameCenter.cameraMng.BlackCoverAll(false);
        }
        SystemSettingMng.CullingShow = true;
		LoadSceneEffects();
		//NPC
        BuildNPCs();
        BuildMonsters();
        BuildTraps();
        BuildOPCs();
        BuildItems();
        RefreshAbilityBallisticCurve(-1);

		
		GameCenter.mainPlayerMng.lastSceneID = sceneID;// 

        //if (GameCenter.sceneAnimMng.RestAnimCount > 0)
        //{
        //    stateMachine.Send((int)EventType.SCENE_ANIMATION);
        //}
        if (_from.name != "load" && _from.name != "newbieGuide")
        {
           // GameCenter.uIMng.GenGUI(GUIType.MAINDUNGEON, true);
        }
//		if(_from.name == "sceneAnimState")
//		{
//			OnSceneStateChange(true);
//		}
        alreadyHasMob = false;

    }



    /// <summary>
    /// 上一次判断，主玩家规定范围内是否有敌对怪物对象
    /// </summary>
    protected bool alreadyHasMob;
    /// <summary>
    /// 更新运行状态 by吴江
    /// </summary>
    /// <param name="_curState"></param>
    protected void UpdateRunState(fsm.State _curState)
    {
        CheckWifi();
    }

    protected void ExitRunState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        GameCenter.uIMng.ReleaseGUI(GUIType.LITTLEMAP);
        GameCenter.uIMng.ReleaseGUI(GUIType.TASK);
        GameCenter.uIMng.ReleaseGUI(GUIType.MAINFIGHT);
		GameCenter.uIMng.ReleaseGUI(GUIType.MAINCOPPY);
		GameCenter.uIMng.ReleaseGUI(GUIType.GUILDACTIVITYCOPPY);
    }
    #endregion

    void OnDisable()
    {
        UnRegist(); 
        GameCenter.uIMng.ReleaseGUI(GUIType.LITTLEMAP);
        GameCenter.uIMng.ReleaseGUI(GUIType.TASK);
        GameCenter.uIMng.ReleaseGUI(GUIType.MAINFIGHT);
		GameCenter.uIMng.ReleaseGUI(GUIType.MAINCOPPY);
		GameCenter.uIMng.ReleaseGUI(GUIType.GUILDACTIVITYCOPPY);
        if (GameCenter.curMainPlayer != null)
        {
            GameCenter.curMainPlayer.CurTarget = null;
        }
        sceneMng.C2C_CleanAllAbilityBallisticCurve();
        foreach (var item in abilityBallisticCurveDic.Values)
        {
            if (item != null)
            {
                item.needDespawed = true;
                item.OnDespawned();
            }
        }
        abilityBallisticCurveDic.Clear();
		GameCenter.uIMng.ReleaseGUI(GUIType.FUNCTION);
    }


    protected override void Regist()
    {
        base.Regist();
        SceneMng.OnAbilityBallisticCurveUpdate += RefreshAbilityBallisticCurve;
		GameCenter.OnConnectStateChange -= ConnectStateChange;
    }

    public override void UnRegist()
    {
        base.UnRegist();
        sceneMng.C2C_CleanAllAbilityBallisticCurve();
        SceneMng.OnAbilityBallisticCurveUpdate -= RefreshAbilityBallisticCurve;

    }
    /// <summary>
    /// 刷新技能弹道（依赖对象池，必须在对象池初始化结束以后使用） by吴江
    /// </summary>
    /// <param name="_id">删除的弹道ID，如果为负数则这次变化为增加弹道</param>
    protected void RefreshAbilityBallisticCurve(int _id)
    {
        if (_id < 0)//增加弹道 by吴江
        {
            FDictionary dataList = sceneMng.AbilityBallisticCurveInfoDictionary;
            foreach (int item in dataList.Keys)
            {
                if (!abilityBallisticCurveDic.ContainsKey(item))
                {
                    //从对象池中取出 by吴江
                    AbilityBallisticCurveInfo info = dataList[item] as AbilityBallisticCurveInfo;
                    if (info != null)
                    {
                        abilityBallisticCurveDic[item] = GameCenter.spawner.SpawnAbilityBallisticCurve(info);
                    }
                    else
                    {
                        Debug.LogError(item + " 弹道数据为空!");
                    }
                }
            }
        }
        else//删除弹道 by吴江
        {
            if (abilityBallisticCurveDic.ContainsKey(_id))
            {
                //归还给对象池 by吴江
                abilityBallisticCurveDic[_id].needDespawed = true;
                abilityBallisticCurveDic.Remove(_id);
            }
        }
    }

    protected override void OnSceneStateChange(bool _succeed)
    {
        
        if (_succeed)
        {
			SceneUiType sceneUiType = GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType;//场景sort换来换去,UI显示用UIType 
			switch(sceneUiType)
			{
			case SceneUiType.GUILDPROTECT: 
				GameCenter.uIMng.SwitchToUI(GUIType.GUILDACTIVITYCOPPY);
				GameCenter.uIMng.SwitchToUI(GUIType.LITTLEMAP);
				break;
            case SceneUiType.GUILDWAR:
                GameCenter.uIMng.SwitchToUI(GUIType.GUILDACTIVITYCOPPY);
                break;
            case SceneUiType.BATTLEFIGHT://火焰山
                if (GameCenter.mainPlayerMng.MainPlayerInfo.CurMountInfo != null && GameCenter.mainPlayerMng.MainPlayerInfo.CurMountInfo.IsRiding)//下坐骑
                {
                    GameCenter.newMountMng.C2S_ReqRideMount(ChangeMount.DOWNRIDE, GameCenter.mainPlayerMng.MainPlayerInfo.CurMountInfo.ConfigID, MountReqRideType.AUTO);
                }
                GameCenter.uIMng.SwitchToUI(GUIType.GUILDACTIVITYCOPPY); 
                break;
			case SceneUiType.ARENA:
			case SceneUiType.BUDOKAI:
			case SceneUiType.BRIDGE:
			case SceneUiType.DESERT:
			case SceneUiType.HOLYLAND:
			case SceneUiType.PETLAND:
			case SceneUiType.ICE:
			case SceneUiType.XIANLV:
			case SceneUiType.TOWER:
			case SceneUiType.SEALBOSS:
			case SceneUiType.ENDLESS:
				GameCenter.uIMng.SwitchToUI(GUIType.MAINCOPPY);
				break;
			case SceneUiType.GODSWAR:
            case SceneUiType.BOSSCOPPY:
				GameCenter.uIMng.SwitchToUI(GUIType.MAINCOPPY);
				GameCenter.uIMng.SwitchToUI(GUIType.LITTLEMAP);
				break;
			case SceneUiType.UNDERBOSS://地宫BOSS只显示了任务和队伍
				GameCenter.uIMng.SwitchToUI(GUIType.TASK);
				GameCenter.taskMng.SetCurSelectToggle(TaskTeamWnd.ToggleType.TEAM);
				break;
			case SceneUiType.RONGELAND:
                GameCenter.taskMng.SetCurSelectToggle(TaskTeamWnd.ToggleType.TASK);
				//GameCenter.bossChallengeMng.C2S_ReqChallengeBossList();//进入熔恶之地,请求BOSS列表
				GameCenter.uIMng.SwitchToUI(GUIType.TASK);
				GameCenter.uIMng.SwitchToUI(GUIType.LITTLEMAP);
                break;
			case SceneUiType.LIRONGELAND:
				GameCenter.taskMng.SetCurSelectToggle(TaskTeamWnd.ToggleType.BOSS);
				GameCenter.bossChallengeMng.C2S_ReqChallengeBossList();//进入熔恶之地,请求BOSS列表
				GameCenter.uIMng.SwitchToUI(GUIType.TASK);
				GameCenter.uIMng.SwitchToUI(GUIType.LITTLEMAP);
				break;
			case SceneUiType.GUILDFIRE:
				GameCenter.taskMng.SetCurSelectToggle(TaskTeamWnd.ToggleType.GUILDFIRE);
				GameCenter.uIMng.SwitchToUI(GUIType.TASK);
				GameCenter.uIMng.SwitchToUI(GUIType.LITTLEMAP);
				break;
			case SceneUiType.GUILDSIEGE:
				GameCenter.taskMng.SetCurSelectToggle(TaskTeamWnd.ToggleType.GUILDSIEGE);
				GameCenter.uIMng.SwitchToUI(GUIType.TASK);
				GameCenter.uIMng.SwitchToUI(GUIType.LITTLEMAP);
				break;
			case SceneUiType.NEWBIEMAP:
                GameCenter.uIMng.SwitchToUI(GUIType.MAINCOPPY);
				GameCenter.uIMng.SwitchToUI(GUIType.LITTLEMAP);
				break;
            case SceneUiType.RAIDERARK:
                //夺宝奇兵界面不能骑马
                if (GameCenter.mainPlayerMng.MainPlayerInfo.CurMountInfo != null && GameCenter.mainPlayerMng.MainPlayerInfo.CurMountInfo.IsRiding)//下坐骑
                {
                    GameCenter.newMountMng.C2S_ReqRideMount(ChangeMount.DOWNRIDE, GameCenter.mainPlayerMng.MainPlayerInfo.CurMountInfo.ConfigID, MountReqRideType.AUTO);
                }
                if (!GameCenter.teamMng.isInTeam)
                {
                    GameCenter.uIMng.SwitchToUI(GUIType.MAINCOPPY);
                    GameCenter.uIMng.SwitchToUI(GUIType.LITTLEMAP);
                }
                else
                    GameCenter.messageMng.AddClientMsg(457);
				break;
            case SceneUiType.HANGUPCOPPYFIRSTFLOOR:
            case SceneUiType.HANGUPCOPPYSECONDFLOOR:
                GameCenter.taskMng.SetCurSelectToggle(TaskTeamWnd.ToggleType.HANGUPCOPPY);
				GameCenter.uIMng.SwitchToUI(GUIType.TASK);
                GameCenter.uIMng.SwitchToUI(GUIType.LITTLEMAP);
                break;
			default:
				GameCenter.taskMng.SetCurSelectToggle(TaskTeamWnd.ToggleType.TASK);
				GameCenter.uIMng.SwitchToUI(GUIType.TASK);
				GameCenter.uIMng.SwitchToUI(GUIType.LITTLEMAP);
				break;
			}
            GameCenter.uIMng.SwitchToUI(GUIType.MAINFIGHT);
//            if (!GameCenter.mainPlayerMng.MainPlayerInfo.IsJoinedCamp && GameCenter.mainPlayerMng.IsOpenFun(FunctionType.Camp))
//            {
//                GameCenter.uIMng.SwitchToUI(GUIType.CAMPJOIN);
//            }
        }
        base.OnSceneStateChange(_succeed);
    }


}
