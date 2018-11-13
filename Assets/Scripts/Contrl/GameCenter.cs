//====================================================
//作者：吴江
//日期：2015/5/19
//用途：整个游戏的控制中心
//======================================================




using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 整个游戏的控制中心 by 吴江
/// </summary>
public class GameCenter : Game
{
    #region 数据
    /// <summary>
    /// 整个游戏的控制中心的唯一实例 by 吴江
    /// </summary>
    public static new GameCenter instance = null;


    #region DEBUG
    /// <summary>
    /// 是否在工程界面渲染坐标网格 by吴江
    /// </summary>
    public bool showSector = false;
    /// <summary>
    /// 是否在工程界面打印当前坐标网格内的对象数量 by吴江
    /// </summary>
    public bool showSectorObjectCount = false;

    /// <summary>
    /// 是否在屏幕上打印服务端消息 by吴江
    /// </summary>
    public bool screen_Debug_Log_S2C = false;

    /// <summary>
    /// 是否在编辑器上打印服务端消息 by吴江
    /// </summary>
    public bool editor_Debug_Log_S2C = false;

    /// <summary>
    /// 是否在屏幕上打印客户端推送的消息 by吴江
    /// </summary>
    public bool screen_Debug_Log_C2S = false;

    /// <summary>
    /// 是否在编辑器上打客户端推送的消息 by吴江
    /// </summary>
    public bool editor_Debug_Log_C2S = false;

    /// <summary>
    /// 当前策划要测试的过场动画
    /// </summary>
    public int cur_Text_SceneAnima_ID = 0;

    /// <summary>
    /// 是否可强行跳过过场动画
    /// </summary>
    public bool forceTo_BreakSceneAnima = false;

    /// <summary>
    /// 聊天喇叭使用CD
    /// </summary>
    public float chatHornTime = 20f;
	/// <summary>
	/// 是否开启引导
	/// </summary>
	public bool NewFunctionLock = false;
    #endregion
    protected bool isSetWorldChatTime = false;
    public bool IsSetWorldChatTime
    {
        get
        {
            return isSetWorldChatTime;
        }
        set
        {
            if (isSetWorldChatTime != value)
            {
                isSetWorldChatTime = value;
                if (isSetWorldChatTime)
                {
                    worldChatColdTime = Time.time;
                }
            }
        }
    }
    /// <summary>
    /// 世界聊天频率不能超过20秒
    /// </summary>
    public float worldChatColdTime = 0;

    /// <summary>
    /// 竞技场冷却倒计时
    /// </summary>
    public int arenSurPlusTime = 0;
    public float arenTime = 0;
    /// <summary>
    /// 竞技场红点
    /// </summary>
    bool isSetArenRed = false;
    public bool IsSetArenRed
    {
        get
        {
            return isSetArenRed;
        }
        set
        {
            if (isSetArenRed != value)
            {
                isSetArenRed = value;
                if (isSetArenRed)
                {
                    arenTime = Time.time;
                }
            }
        }
    }

    bool isChatHorn = false;
    /// <summary>
    /// 聊天喇叭是否能使用
    /// </summary>
    public bool IsChatHorn
    {
        get
        {
            return isChatHorn;
        }
        set
        {
            if (isChatHorn != value)
            {
                isChatHorn = value;
                if (isChatHorn)
                {
                    chatHornStartTime = Time.time;
                }
            }
        }
    }
    /// <summary>
    /// 聊天喇叭CD几时起始时间
    /// </summary>
    float chatHornStartTime = 0f;

    //TO DO：要把这些对象全部弄进对象池，避免重复创建和删除 by吴江
    [System.NonSerialized]
    public GameObject dummyMobPrefab;
    [System.NonSerialized]
    public GameObject dummyNpcPrefab;
    [System.NonSerialized]
    public GameObject dummyOpcPrefab;

    #region 全局管理类
    /// <summary>
    /// 游戏运行的mono实例,用于挂载游戏逻辑状态机的对象 by 吴江
    /// </summary>
    protected static GameObject stageObj = null;
    /// <summary>
    /// 摄像机管理类唯一实例 by 吴江
    /// </summary>
    public static CameraMng cameraMng = null;
    /// <summary>
    /// 界面管理类唯一实例 by 吴江
    /// </summary>
    public static UIMng uIMng = null;
    /// <summary>
    /// 登陆管理类唯一实例 by 吴江
    /// </summary>
    public static LoginMng loginMng = null;
    /// <summary>
    /// 网络连接管理类唯一实例 by 吴江
    /// </summary>
    public static NetMsgMng netMsgMng = null;
    /// <summary>
    /// 主玩家管理类唯一实例 by吴江
    /// </summary>
    public static MainPlayerMng mainPlayerMng = null;
    /// <summary>
    /// 声音播放管理类唯一实例 by吴江
    /// </summary>
    public static SoundMng soundMng = null;
    /// <summary>
    /// 对象池管理器唯一实例 by吴江
    /// </summary>
    public static Spawner spawner = null;
    /// <summary>
    /// 错误提示管理类
    /// </summary>
    public static MessageInstanceMng messageMng = null;

	/// <summary>
	/// 通用说明弹窗
	/// </summary>
	public static DescriptionMng descriptionMng;
    /// <summary>
    /// 模型预览管理类 by吴江
    /// </summary>
    public static PreviewManager previewManager = null;

    public static MsgLoackingMng msgLoackingMng = null;

    public static SystemSettingMng systemSettingMng = null;

    public static TimeMng timeMng = null;
    #endregion

    #region 跟随场景的管理类
    /// <summary>
    /// 当前游戏运行逻辑  by吴江
    /// </summary>
    public static GameStage curGameStage = null;
    /// <summary>
    /// 当前游戏场景管理类唯一实例 by 吴江
    /// </summary>
    public static SceneMng sceneMng = null;
    #endregion

    #region 跟随玩家的管理类
	/// <summary>
	/// 时装管理类
	/// </summary>
	public static FashionMng fashionMng;
    /// <summary>
    /// 技能管理类
    /// </summary>
    public static AbilityMng abilityMng;
    /// <summary>
    /// 物品管理类
    /// </summary>
    public static InventoryMng inventoryMng;
    /// <summary>
    /// 任务管理类
    /// </summary>
    public static TaskMng taskMng;
    /// <summary>
    /// 复活管理类
    /// </summary>
    public static RebornMng rebornMng;
    /// <summary>
    /// 公会管理类
    /// </summary>
    public static GuildMng guildMng;
	/// <summary>
	/// 每日必做管理类
	/// </summary>
	public static DailyMustDoMng dailyMustDoMng;
    /// <summary>
    /// 好友管理类 by朱素云
    /// </summary>
    public static FriendsMng friendsMng;
    /// <summary>
    /// 等级奖励管理类 by朱素云
    /// </summary>
    public static RankRewardMng rankRewardMng; 
    /// <summary>
    /// 修行管理类 by朱素云
    /// </summary>
    public static PracticeMng practiceMng; 
    /// <summary>
    /// 仙侣管理类 by朱素云
    /// </summary>
    public static CoupleMng coupleMng;
    /// <summary>
    /// 周卡管理类 by朱素云
    /// </summary>
    public static WeekCardMng weekCardMng; 
    /// <summary>
    /// 结义管理类 by朱素云
    /// </summary>
    public static SwornMng swornMng;
    /// <summary>
    /// 随从管理类 by朱素云
    /// </summary>
    public static MercenaryMng mercenaryMng;
    /// <summary>
    /// 坐骑管理类 by朱素云
    /// </summary>
    public static NewMountMng newMountMng;
    /// <summary>
    /// 爱心礼包管理类 by朱素云
    /// </summary>
    public static LovePackageMng lovePackageMng;
    /// <summary>
    /// 火焰山战场管理类 by朱素云
    /// </summary>
    public static BattleFightMng battleFightMng;
    /// <summary>
    /// 副本管理类
    /// </summary>
    public static DungeonMng dungeonMng;
    /// <summary>
    /// 队伍管理类
    /// </summary>
    public static TeamMng teamMng;

    /// <summary>
    /// 技能升级管理类
    /// </summary>
    public static SkillMng skillMng;
	/// <summary>
	/// 仙盟商店管理类
	/// </summary>
	public static GuildShopMng guildShopMng;
	/// <summary>
	/// 仙盟技能管理类
	/// </summary>
	public static GuildSkillMng guildSkillMng;

	/// <summary>
	/// 下载奖励管理类
	/// </summary>
	public static DownloadBonusMng downloadBonusMng;


	/// <summary>
	/// 商店管理类
	/// </summary>
	public static ShopMng shopMng;
	/// <summary>
	/// 商城管理类
	/// </summary>
	public static NewMallMng newMallMng;


	/// <summary>
	/// 市场
	/// </summary>
	public static MarketMng marketMng;

	/// <summary>
	/// 物品购买
	/// </summary>
	public static BuyMng buyMng;
	/// <summary>
	/// 交易
	/// </summary>
    public static TradeMng tradeMng;
    /// <summary>
    /// 充值管理
    /// </summary>
    public static RechargeMng rechargeMng;
	/// <summary>
	/// 铸魂
	/// </summary>
	public static CastSoulMng castSoulMng;
	/// <summary>
	/// 在线奖励
	/// </summary>
	public static OnlineRewardMng onlineRewardMng;
    /// <summary>
    /// 仙域争霸
    /// </summary>
    public static GuildFightMng guildFightMng;
    /// <summary>
    /// 精彩活动
    /// </summary>
    public static WdfActiveMng wdfActiveMng;
    /// <summary>
    /// 开服贺礼
    /// </summary>
    public static OpenServerRewardMng openServerRewardMng;
    
    /// <summary>
    /// 复活
    /// </summary>
    public static ResurrectionMng resurrectionMng;
    /// <summary>
    /// 小助手
    /// </summary>
    public static LittleHelperMng littleHelperMng;

    /// <summary>
    /// 称号管理类 by 贺丰
    /// </summary>
    public static TitleMng titleMng;
     
    /// <summary>
    /// 邮箱管理类
    /// </summary>
	public static XXMailMng mailBoxMng;
	
	/// <summary>
	/// 无尽挑战，竞技场，副本，暂停，活动大厅管理类
	/// </summary>
	public static EndLessTrialsMng endLessTrialsMng;
    /// <summary>
    /// 副本管理类
    /// </summary>
    public static DuplicateMng duplicateMng;
    /// <summary>
    /// 竞技场管理类
    /// </summary>
    public static ArenaMng arenaMng;
    /// <summary>
    /// 聊天管理类
    /// </summary>
    public static ChatMng chatMng; 
	/// <summary>
	/// 装备培养管理类
	/// </summary>
	public static EquipmentTrainingMng equipmentTrainingMng;


    /// <summary>
    /// 法宝管理类
    /// </summary>
    public static MagicWeaponMng magicWeaponMng;

    /// <summary>
    /// 成就管理类
    /// </summary>
    public static AchievementMng achievementMng;

    /// <summary>
    /// 七天奖励管理类
    /// </summary>
    public static SevenDayMng sevenDayMng;

    /// <summary>
    /// 首冲大礼管理类
    /// </summary>
    public static FirstChargeBonusMng firstChargeBonusMng;

    /// <summary>
    /// 排行榜管理类
    /// </summary>
    public static NewRankingMng newRankingMng;

    /// <summary>
    /// 翅膀管理类
    /// </summary>
    public static WingMng wingMng;

    /// <summary>
    /// 藏宝阁管理类
    /// </summary>
    public static TreasureHouseMng treasureHouseMng;

	/// <summary>
	/// Boss挑战管理类
	/// </summary>
	public static BossChallengeMng bossChallengeMng;
	/// <summary>
	/// 活动管理类
	/// </summary>
	public static ActivityMng activityMng;
    /// <summary>
    /// 皇室宝箱管理类
    /// </summary>
    public static RoyalTreasureMng royalTreasureMng;
    /// <summary>
    /// 奇缘系统管理类
    /// </summary>
    public static MiracleMng miracleMng;
    /// <summary>
    /// 新手引导管理类
    /// </summary>
    public static NoviceGuideMng noviceGuideMng;
    /// <summary>
    /// 离线经验管理类
    /// </summary>
    public static OffLineRewardMng offLineRewardMng;
    /// <summary>
    /// VIP管理类
    /// </summary>
    public static VipMng vipMng;
    /// <summary>
    /// 二冲管理类
    /// </summary>
    public static TwoChargeMng twoChargeMng;
    /// <summary>
    /// 宝藏活动管理类
    /// </summary>
    public static TreasureTroveMng treasureTroveMng;
    /// <summary>
    /// 七日目标管理类
    /// </summary>
    public static SevenChallengeMng sevenChallengeMng;
    #endregion
    /// <summary>
    /// 当前主玩家的表现层对象（因为主玩家切换场景的时候不销毁，因此主玩家无法完全从属GameStage，因此在游戏中保持一份引用。但创建仍然由GameStage来执行） by吴江
    /// </summary>
    public static MainPlayer curMainPlayer;
    /// <summary>
    /// 当前的主玩家随从表现层对象（因为主玩家切换场景的时候不销毁，因此主玩家无法完全从属GameStage，因此在游戏中保持一份引用。但创建仍然由GameStage来执行） by吴江
    /// </summary>
    public static MainEntourage curMainEntourage;
    #endregion

    #region UNITY
    void Start()
    {
        //Debug.Log("陀螺仪是否可用 " + SystemInfo.supportsGyroscope);
        instance = this;
        this.gameObject.transform.position = Vector3.zero;
        stateMachine.Start();
        if (isPlatform)
        {
			LynSdkManager.Instance.InitSdk();
            YvVoiceSdk.YvVoiceInit();
            //if (isDataEyePattern) DCAgent.getInstance().initWithAppIdAndChannelId("appid", "channelId");//初始化DataEye,参数与配置中不一致则以配置参数为准
        }
        if (!SystemSettingMng.HasAutoSetQuality)
        {
            InvokeRepeating("UpdateFPS", 0.0f, 1.0f);
        }
		curServerTime = DateTime.Now;
    }


	protected int frames = 0;
    public int FPS
	{
		get{
			return fps;
		}
	}
    protected int fps = 0;
	protected float lastCountTime = 0;
	protected float blockTimes=0;
	protected float fpsNums=0;


	protected void UpdateFPS()
	{
        if (GameCenter.curMainPlayer != null)
        {
            fps = (int)(frames / (Time.time - lastCountTime));
            //Debug.Log("此时的帧数为" + fps);
            lastCountTime = Time.time;
            frames = 0;
            if (systemSettingMng.IntelligentMode)
            {
                fpsNums += fps;
                ++blockTimes;
                if (blockTimes % 10 == 0)
                {
                    if ((fpsNums / blockTimes) <= 10 && !systemSettingMng.FluencyMode)
                    {
                        systemSettingMng.OnFluencyModel();
                        //Debug.Log("帧数过低自动设置为低配显示");
                    }

                    blockTimes = 0;
                    fpsNums = 0;
                }
            }
        }
	}


    new void Update()
    {
		++frames;
        base.Update();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SdkExit();
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            NetMsgMng.ConectClose();
        }
        if (IsChatHorn)
        {
            if (Time.time - chatHornStartTime > chatHornTime)
            {
                chatHornStartTime = 0;
                IsChatHorn = false;
            }
        }
        if (IsSetArenRed)
        {
            if (Time.time - arenTime > arenSurPlusTime)
            {
                arenTime = 0;
                if (GameCenter.mainPlayerMng != null && GameCenter.endLessTrialsMng != null)
                    GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.ARENA, GameCenter.arenaMng.ArenRed());
                IsSetArenRed = false;
            }
        }
        if (IsSetWorldChatTime)
        {
            if (Time.time - worldChatColdTime > 6)
            {
                IsSetWorldChatTime = false;
            }
        }
        if (isDevelopmentPattern)
        {
            UpdateTimeScale();
        }
    }


    void UpdateTimeScale()
    {
        if (Input.GetKey(KeyCode.Minus))
        {
            Time.timeScale = Mathf.Max(Time.timeScale - 0.01f, 0.0f);
        }
        else if (Input.GetKey(KeyCode.Equals))
        {
            Time.timeScale = Mathf.Min(Time.timeScale + 0.01f, 10.0f);
        }

        if (Input.GetKey(KeyCode.Alpha0))
        {
            Time.timeScale = 0.0f;
        }
        else if (Input.GetKey(KeyCode.Alpha9))
        {
            Time.timeScale = 1.0f;
        }
    }


    public void SdkExit()
    {
		GameExit();
    }
    void OnExitResult(string result)
    {
        //	NGUIDebug.Log("OnExitResult result:"+result);
        //if (GameCenter.instance.isDataEyePattern) DCAccount.logout();
        GameExit();
    }

    void OnDisable()
    {
        NetMsgMng.ConectClose();
    }
    #endregion



    /// <summary>
    /// 注册消息监听
    /// </summary>
    void ResgistMsg()
    {

    }

    #region FSM 状态机控制部分
    /// <summary>
    /// 游戏状态枚举 by吴江
    /// </summary>
    public enum EventType
    {
        UPDATEASSET = fsm.Event.USER_FIELD + 1,
        AWAKE,
        PLATFORMLOGIN,
        INIT_CONFIG,
        LOGIN,
        WAIT_CONNECT,
        SELECTCHAR,
        CREATCHAR,
        ENTER_CITY,
        RUN_CITY,
        ENTER_DUNGEON,
        RUN_DUNGEON,
        LOAD_GAME,
        RUN_GAME,

        ENTER_ARENA,
        RUN_ARENA,
    }
    /// <summary>
    /// 初始化状态机 by吴江
    /// </summary>
    protected override void InitStateMachine()
    {
        fsm.State updateAsset = new fsm.State("updateAsset", stateMachine);
        updateAsset.onEnter += EnterUpdateAssetState;
        updateAsset.onExit += ExitUpdateAssetState;
        updateAsset.onAction += UpdateAssetState;

        fsm.State awake = new fsm.State("awake", stateMachine);
        awake.onEnter += EnterAwakeState;
        awake.onExit += ExitAwakeState;
        awake.onAction += UpdateAwakeState;


        fsm.State initConfig = new fsm.State("initConfig", stateMachine);
        initConfig.onEnter += EnterInitConfigState;
        initConfig.onExit += ExitInitConfigState;
        initConfig.onAction += UpdateInitConfigState;

        fsm.State platformLogin = new fsm.State("platformLogin", stateMachine);
        platformLogin.onEnter += EnterPlatformLoginState;
        platformLogin.onExit += ExitPlatformLoginState;

        fsm.State login = new fsm.State("login", stateMachine);
        login.onEnter += EnterLoginState;
        login.onExit += ExitLoginState;
        login.onAction += UpdateLoginState;

        fsm.State waitConnect = new fsm.State("waitConnect", stateMachine);
        waitConnect.onEnter += EnterWaitConnectState;
        waitConnect.onExit += ExitWaitConnectState;
        waitConnect.onAction += UpdateWaitConnectState;

//        fsm.State selectChar = new fsm.State("selectChar", stateMachine);
//        selectChar.onEnter += EnterSelectCharState;
//        selectChar.onExit += ExitSelectCharState;
//        selectChar.onAction += UpdateSelectCharState;


        fsm.State createChar = new fsm.State("createChar", stateMachine);
        createChar.onEnter += EnterCreateCharState;
        createChar.onExit += ExitCreateCharState;
        createChar.onAction += UpdateCreateCharState;


        fsm.State enterCity = new fsm.State("enterCity", stateMachine);
        enterCity.onEnter += EnterInitCityState;
        enterCity.onExit += ExitInitCityState;
        enterCity.onAction += UpdateInitCityState;


        fsm.State runCity = new fsm.State("runCity", stateMachine);
        runCity.onEnter += EnterRunCityState;
        runCity.onExit += ExitRunCityState;
        runCity.onAction += UpdateRunCityState;


        ///竞技场的增加 .
        fsm.State enterArana = new fsm.State("enterArana", stateMachine);
        enterArana.onEnter += EnterInitArenaState;
        enterArana.onExit += ExitInitArenaState;
        enterArana.onAction += UpdateInitArenaState;

        fsm.State runArana = new fsm.State("runArana", stateMachine);
        //  runArana.onEnter += EnterRunAranaState;
        //  runArana.onExit += ExitRunAranaState;
        runArana.onAction += UpdateRunArenaState;
        /// end 竞技场


        fsm.State enterDungeon = new fsm.State("enterDungeon", stateMachine);
        enterDungeon.onEnter += EnterInitDungeonState;
        enterDungeon.onExit += ExitInitDungeonState;
        enterDungeon.onAction += UpdateInitDungeonState;



        fsm.State runDungeon = new fsm.State("runDungeon", stateMachine);
        runDungeon.onEnter += EnterRunDungeonState;
        runDungeon.onExit += ExitRunDungeonState;
        runDungeon.onAction += UpdateRunDungeonState;



        awake.Add<fsm.EventTransition>(updateAsset, (int)EventType.UPDATEASSET);
        awake.Add<fsm.EventTransition>(initConfig, (int)EventType.INIT_CONFIG);

        updateAsset.Add<fsm.EventTransition>(initConfig, (int)EventType.INIT_CONFIG);

        initConfig.Add<fsm.EventTransition>(login, (int)EventType.LOGIN);
        initConfig.Add<fsm.EventTransition>(platformLogin, (int)EventType.PLATFORMLOGIN);


        platformLogin.Add<fsm.EventTransition>(login, (int)EventType.LOGIN);

		//        login.Add<fsm.EventTransition>(selectChar, (int)EventType.SELECTCHAR);//西游没有选角  by 何明军
        login.Add<fsm.EventTransition>(createChar, (int)EventType.CREATCHAR);
        login.Add<fsm.EventTransition>(platformLogin, (int)EventType.PLATFORMLOGIN);
		
		login.Add<fsm.EventTransition>(enterDungeon, (int)EventType.ENTER_DUNGEON);//西游没有选角，登录直接进游戏 by 何明军
		login.Add<fsm.EventTransition>(enterCity, (int)EventType.ENTER_CITY);//西游没有选角，登录直接进游戏 by 何明军

		//        selectChar.Add<fsm.EventTransition>(enterCity, (int)EventType.ENTER_CITY); by 何明军
//        selectChar.Add<fsm.EventTransition>(createChar, (int)EventType.CREATCHAR);
//        selectChar.Add<fsm.EventTransition>(enterDungeon, (int)EventType.ENTER_DUNGEON);
//        selectChar.Add<fsm.EventTransition>(login, (int)EventType.LOGIN); //选择角色到登录

        login.Add<fsm.EventTransition>(waitConnect, (int)EventType.WAIT_CONNECT);


        waitConnect.Add<fsm.EventTransition>(login, (int)EventType.LOGIN);
        waitConnect.Add<fsm.EventTransition>(createChar, (int)EventType.CREATCHAR);
        waitConnect.Add<fsm.EventTransition>(enterCity, (int)EventType.ENTER_CITY);
        waitConnect.Add<fsm.EventTransition>(enterDungeon, (int)EventType.ENTER_DUNGEON);



		//        createChar.Add<fsm.EventTransition>(selectChar, (int)EventType.SELECTCHAR);//西游没有选角
        createChar.Add<fsm.EventTransition>(enterCity, (int)EventType.ENTER_CITY);
		createChar.Add<fsm.EventTransition>(login, (int)EventType.LOGIN);
        createChar.Add<fsm.EventTransition>(enterDungeon, (int)EventType.ENTER_DUNGEON);//登录到地下城
        createChar.Add<fsm.EventTransition>(waitConnect, (int)EventType.WAIT_CONNECT);

        enterCity.Add<fsm.EventTransition>(runCity, (int)EventType.RUN_CITY);
        enterCity.Add<fsm.EventTransition>(updateAsset, (int)EventType.UPDATEASSET);//进主城时,跳转更新 
		enterCity.Add<fsm.EventTransition>(login, (int)EventType.LOGIN);//断线重连跳转登陆

        runCity.Add<fsm.EventTransition>(enterCity, (int)EventType.ENTER_CITY);
        runCity.Add<fsm.EventTransition>(enterDungeon, (int)EventType.ENTER_DUNGEON);
        runCity.Add<fsm.EventTransition>(enterArana, (int)EventType.ENTER_ARENA);// 跳转到竞技场
        runCity.Add<fsm.EventTransition>(login, (int)EventType.LOGIN);//主城到登录跳转
		//        runCity.Add<fsm.EventTransition>(selectChar, (int)EventType.SELECTCHAR);//主城跳转到选择角色列表   //西游没有选角
        runCity.Add<fsm.EventTransition>(updateAsset, (int)EventType.UPDATEASSET);//在主城时,跳转更新 
        runCity.Add<fsm.EventTransition>(waitConnect, (int)EventType.WAIT_CONNECT);

        enterDungeon.Add<fsm.EventTransition>(runDungeon, (int)EventType.RUN_DUNGEON);
		enterDungeon.Add<fsm.EventTransition>(login, (int)EventType.LOGIN);//断线重连跳转登陆

        enterArana.Add<fsm.EventTransition>(runArana, (int)EventType.RUN_ARENA);
		enterArana.Add<fsm.EventTransition>(login, (int)EventType.LOGIN);//断线重连跳转登陆

        runArana.Add<fsm.EventTransition>(enterCity, (int)EventType.ENTER_CITY);
        runArana.Add<fsm.EventTransition>(login, (int)EventType.LOGIN);
        runArana.Add<fsm.EventTransition>(enterArana, (int)EventType.ENTER_ARENA);//添加竞技场到竞技场的跳转 add by吴江
        runArana.Add<fsm.EventTransition>(enterDungeon, (int)EventType.ENTER_DUNGEON);//添加竞技场到副本的跳转 add by吴江

        runDungeon.Add<fsm.EventTransition>(enterCity, (int)EventType.ENTER_CITY);
        runDungeon.Add<fsm.EventTransition>(enterDungeon, (int)EventType.ENTER_DUNGEON);
        runDungeon.Add<fsm.EventTransition>(login, (int)EventType.LOGIN);
        runDungeon.Add<fsm.EventTransition>(enterArana, (int)EventType.ENTER_ARENA);// 跳转到竞技场
        runDungeon.Add<fsm.EventTransition>(waitConnect, (int)EventType.WAIT_CONNECT);

        stateMachine.initState = awake;
    }

    #region 更新部分
    protected virtual void EnterUpdateAssetState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        Destroy(stageObj.GetComponent<GameStage>());
        curGameStage = stageObj.AddComponent<UpdateAssetStage>();
        Debug.Log("EnterUpdateAssetState");
    }
    protected virtual void ExitUpdateAssetState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        Debug.Log("ExitUpdateAssetState");
        XluaMng.instance.InitXlua();
    }

    protected virtual void UpdateAssetState(fsm.State _curState)
    {

    }
    #endregion

    #region 启动部分 by吴江
    protected virtual void EnterAwakeState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        //Debug.Log("EnterAwakeState");
        if (!isDevelopmentPattern)
        {
            Application.targetFrameRate = 30;//限制最多每秒计算30帧
        }
        Caching.CleanCache();
        LayerMng.Init();
        if (stageObj == null)
        {
            stageObj = new GameObject();
            stageObj.transform.parent = this.transform;
            stageObj.transform.localPosition = Vector3.zero;
            stageObj.name = "Stage";
        }


        GameObject assetLoadObj = new GameObject();
        assetLoadObj.transform.parent = this.transform;
        assetLoadObj.transform.localPosition = Vector3.zero;
        assetLoadObj.name = "AssetLoad";
        AssetMng assetMng = assetLoadObj.AddComponent<AssetMng>();
        assetMng.Init();

        if(isDevelopmentPattern)XluaMng.instance.InitXlua();//开发模式在这里初始化(AssetMng创建之后),非开发模式在资源更新之后

        if (systemSettingMng == null) systemSettingMng = SystemSettingMng.CreateNew();
        //声音管理器 
        soundMng = SoundMng.CreateNew();

        ////把客户端所有的非法字符加载进来。
        //BadWordChecker.loadFromResources("badwords");

        GameObject.DontDestroyOnLoad(this.gameObject);
        if (netMsgMng == null) netMsgMng = NetMsgMng.CreateNew("192.168.1.185", 8000);

        cameraMng = this.gameObject.GetComponent<CameraMng>();
        if (cameraMng == null) Debug.LogError("can't find cameraMng, please check it!");
        uIMng = this.gameObject.GetComponent<UIMng>();
        if (uIMng == null) Debug.LogError("can't find uiMng, please check it!");

        spawner = this.gameObject.GetComponent<Spawner>();
        if (spawner == null) spawner = this.gameObject.AddComponent<Spawner>();

        if (loginMng == null) loginMng = LoginMng.CreateNew();

        if (abilityMng == null) abilityMng = AbilityMng.CreateNew();
        if (GameCenter.messageMng == null) messageMng = MessageInstanceMng.CreateNew();
		if(descriptionMng == null)descriptionMng = DescriptionMng.CreateNew();

        if (previewManager == null) previewManager = PreviewManager.CreateNew();
        if (msgLoackingMng == null) msgLoackingMng = MsgLoackingMng.CreateNew();//

        if (timeMng != null) timeMng = TimeMng.CreateNew();

        ResgistMsg();

        //UI部分
        if (isDevelopmentPattern)
        {
            stateMachine.Send((int)EventType.INIT_CONFIG);
        }
        else
        {
            //GameCenter.uIMng.SwitchToUI(GUIType.UPDATEASSET);
            stateMachine.Send((int)EventType.UPDATEASSET);
        }

    }

    protected virtual void ExitAwakeState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        originQaulity = GameCenter.systemSettingMng.CurRendererQuality;
        GameCenter.systemSettingMng.SetQualitySettings(SystemSettingMng.RendererQuality.LOW);
    }

    protected virtual void UpdateAwakeState(fsm.State _curState)
    {
    }
    #endregion

    #region 初始化数据配置状态 by吴江
    protected virtual void EnterInitConfigState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        Resources.UnloadUnusedAssets();
        ConfigMng configMng = ConfigMng.Instance;
		
		#region InitTable
        configMng.InitPetDataRefTable();//zsy
        configMng.InitNewPetRefTable();
        configMng.InitPetSkillComposeRefTable();
        configMng.InitPetSkillNumRefTable();
        configMng.InitPetSkillRefTable();
        configMng.InitMountRefTable();
        configMng.InitMountPropertyRefTable();
        configMng.InitSkinPropertyRefTable();
        configMng.InitSpecialRefTable();//zsy
        configMng.InitWeddingRefTable();//zsy
        configMng.InitTokenLevRefTable();//zsy
        configMng.InitSwornRefTable();//zsy
        configMng.InitFlyUpRefTable();//zsy
        configMng.InitStyliteRefTable();//zsy
        configMng.InitStyliteMoneyRefTable();//zsy
        configMng.InitEverydayRewardRefTable();//zsy
        configMng.InitStarTypeRefTable();//zsy
        configMng.InitTitleTypeRefTable();//zsy
        configMng.InitloveSpreeRefTable();//zsy
        configMng.InitWeekCardRefTable();//zsy
        configMng.InitFashionLevelRefTable();//zsy
        configMng.InitFlyExRefTableRefTable();//zsy
        configMng.InitDownloadBonusRefTable();//zsy
        configMng.InitWeddingCoppyRefTable();//zsy
        configMng.InitCastsoulRewardRefTable();//zsy
        configMng.InitUISkipRefTable();//zsy
        configMng.InitBattleFieldRefTable();//zsy
        configMng.InitDialogueRefTable();//zsy
        configMng.InitCornucopiaRefTable();//zsy
        configMng.InitDividendRefTable();//zsy
        configMng.InitGuildDonateRefTable();//zsy
        configMng.InitTaskRingRewardRefTable();//zsy
        configMng.InitBattleSettlementBonusRefTable();//zsy

        configMng.InitArrowRefTable();
        configMng.InitPlayerConfigRefTable();
        configMng.InitSceneRefTable();
        configMng.InitSceneItemRefTable();
        configMng.InitSceneItemDisRefTable();
        configMng.InitUILevelConfigRefTable();
        configMng.InitEquipmentRefTable();
        configMng.InitSceneNPCRefTable();
        configMng.InitNPCActionRefTable();
        configMng.InitNPCAIRefTable();
        configMng.InitNPCTypeRefTable();
        configMng.InitFlyPointRefTable();
        configMng.InitSkillMainRefTable();
        configMng.InitSkillRuneRefTable();//add by 贺丰
        configMng.InitSkillMainLvRefTable();//add by 贺丰
        configMng.InitSkillPerformanceRefTable();
        configMng.InitSkillBuffRefTable();//add by 龙英杰
        configMng.InitSkillLvDataRefTable();
		configMng.InitOpenNewFunctionRefTable();//by 何明军
		configMng.InitOpenNewFunctionGuideRefTable();//by 何明军
		configMng.InitChatTemplatesRefTable();//by 何明军
        configMng.InitMonsterTable();
        configMng.InitMonsterDistributionRef();//add by 龙英杰
        configMng.InitRelationRefTable();
        configMng.InitPassiveSkillRefTable();//add by 龙英杰
        configMng.InitTaskConfigTable();
        configMng.InitTrapRefTable();
        configMng.InitTitleRefTable();//add by 贺丰

        configMng.InitAttributeTypeRefTable();//add by 易睿
        configMng.InitServerMSGRefTable();//add by 易睿
        configMng.InitMallRefTable();//add by 黄洪兴
        configMng.InitNPCRefTable();//add by 易睿
        configMng.InitTipsRefTable();
        configMng.InitMountRefTable();//add by 易睿

        configMng.InitMagicWeaponRefTabel();//ljq
        configMng.InitRefineRefTable();//ljq
        configMng.InitAddSoulRefTable();//ljq
        configMng.InitWingRefTabel();//ljq
        configMng.InitTreasureHouseRefTable();//ljq
        configMng.InitRewardGroudRefTable();//ljq
        configMng.InitRewardGroudMemberRefTable();//ljq
        configMng.InitDescriptionRefTable();//ljq
        configMng.InitAchieveTypeRefTable();//ljq
        configMng.InitAchievementRefTable();//ljq
        configMng.InitSevenDayRefTable();//ljq
        configMng.InitFirstChargeRefTable();//ljq
        configMng.InitStrengSuitRefTable();//ljq
        configMng.InitCdKeyRefTable();//ljq
        configMng.InitRoyalBoxRefTable();
        configMng.InitPushedReftable();
        
        configMng.InitLevelRewardRefTable();//zsy
		
		configMng.InitStepConsumptionRefTable();///何明军
		configMng.InitBlendRefTable();///何明军
		configMng.InitVIPRefTable();///何明军
		configMng.InitUITextRefTable();
		configMng.InitChapterRefTable();///何明军
		configMng.InitCheckPointRefTable();///何明军
		configMng.InitNameRefTable();
		configMng.InitLineRefTable();
		configMng.InitSuperLifeRefTable();
		configMng.InitAttributeRefTable();
		configMng.InitCopyGroupRefTable();
		configMng.InitCopyRefTable();
		configMng.InitArenaRankRefTable();
		configMng.InitActivityButtonRefTable();
		configMng.InitActivityListRefTable();
		configMng.InitNewFunctionRefTable();
		configMng.InitSystemMailRefTable();

        configMng.InitOffLineRewardRefTable();//by 唐源
        configMng.InitSevenChallengeTaskRefTable();//by 唐源
        configMng.InitSevenChallengeRewardRefTable();
        configMng.InitTwoChargeRefTable();//by 李邵南
        #region 仙侠新添
        configMng.InitStrengthAttrRefTable();
		configMng.InitStrengthRefTable();
        configMng.InitStrengthDesRefTable();
        configMng.InitInlayOpenRefTable();
		configMng.InitWashConsumeRefTable();
		configMng.InitWashValueRefTable();
		configMng.InitOrangeRefineRefTable();
		configMng.InitPromoteRefTable();
		configMng.InitInheritLuckyRefTable();
		configMng.InitGuildRefTable();
		configMng.InitGlogRefTable();
		configMng.InitBossRefTable();
		configMng.InitCityShopRefTable();
		configMng.InitResolveRefTable();
		configMng.InitResolveLevelRefTable();
		configMng.InitEquipmentSetRefTable();
		configMng.InitTaskSurroundRewardRefTable();
		configMng.InitLivelyRefTable();
        configMng.InitLivelyRewardRefTable();
		configMng.InitTowerRefTable();
		configMng.InitUILabelTranslationRefTable();
        configMng.InitMountEquipQualityRefTabel();
        configMng.InitMountEquQualityAttributeRefTable();
        configMng.InitMountStrenConsumeRefTable();
        configMng.InitMountStrenLevRefTable();
        configMng.InitMountSuitRefTable();
        configMng.InitMountEquipMaxLvTable();
        configMng.InitPoPoRefTable();
        configMng.InitPoPoPetRefTable();
        configMng.InitAreaBuffRefTable();
        configMng.InitGuildLivelyRefTable();
        configMng.InitGuildLivelyRewardRefTable();

		configMng.InitFashionRefTable ();//add by 黄洪兴
		configMng.InitShopRefTable ();//add by 黄洪兴
		configMng.InitGuildShopRefTable ();//add by 黄洪兴
		configMng.InitGuildSkillRefTable ();//add by 黄洪兴
		configMng.InitCastSoulRefTable(); //黄洪兴
		configMng.InitCastSoulTimeRefTable();//黄洪兴
		configMng.InitMarketTypeRefTable ();//add by 黄洪兴
		configMng.InitOnlineRewardRefTable();//add by 黄洪兴
        configMng.InitSustainRefTable();//add by 黄洪兴
        configMng.InitLittleHelperRefTable();//add by 黄洪兴
        configMng.InitLittleHelperTypeRefTable();//add by 黄洪兴
        configMng.InitAutoItemRefTable();//add by 黄洪兴
        configMng.InitRechargeRefTable();//add by黄洪兴
        configMng.InitNewFunctionHintsRefTable(); //add by黄洪兴
        configMng.InitRebornRefTable();//add by黄洪兴
		#endregion
		#endregion
		
        GameCenter.uIMng.GenGUI(GUIType.MESSAGE, true);
		
        ResgistMsg();
        //	NGUIDebug.Log("EnterInitConfigState");
    }

    protected virtual void ExitInitConfigState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        //	NGUIDebug.Log("ExitInitConfigState");
    }

    protected virtual void UpdateInitConfigState(fsm.State _curState)
    {
        if (ConfigMng.Instance.Pendings == 0)
        {
            stateMachine.Send((int)EventType.LOGIN);
        }
    }
    #endregion

    #region  平台登陆部分
    protected virtual void EnterPlatformLoginState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        Resources.UnloadUnusedAssets();
        if (isPlatform && !isSwitchAccount)
            LynSdkManager.Instance.UsrLogin(this.gameObject.name, "OnLoginResult");
    }
    protected virtual void ExitPlatformLoginState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {

    }
    void OnLoginResult(string result)
    {
        if (GameCenter.curGameStage is LoginStage)
        {
            hasPlatformLogin = true;
            GameCenter.msgLoackingMng.UpdateSerializeList(platformLoginSeralizeID, false);
            platformLoginSeralizeID = 0;
            string[] ss = result.Split(',');
            string deviceId = ss[0];
            string deviceNo = ss[1];
            string userId = ss[2];
            string userName = ss[3];
            string source = ss[4];
            string token = ss[5];

            GameCenter.loginMng.SDKUserName = userName;
            GameCenter.loginMng.SDKUserID = userId;
            LitJson.JsonData data = new LitJson.JsonData();
            data["DeviceID"] = deviceId;
            data["DeviceNO"] = deviceNo;
            data["UserID"] = userId;
            data["UserName"] = userName;
            data["SourceID"] = source;
            data["Token"] = token;

            GameCenter.loginMng.Login_Name = data.ToJson();
            GameCenter.loginMng.Login_Word = "";
            GameCenter.uIMng.SwitchToSubUI(SubGUIType.SELECTSERVER);
        }
    }

    protected bool canInitMD5Url = false;
    public bool CanInitMd5Url
    {
        get { return canInitMD5Url; }
    }
    /// <summary>
    /// 收到中间件这个消息才可以去获取MD5资源地址
    /// </summary>
    /// <param name="msg"></param>
    public void OnMd5UrlInited(string msg)
    {
        Debug.Log("OnMd5UrlInited");
        canInitMD5Url = true;
    }

    /// <summary>
    /// 切换账号返回登录 by 易睿
    /// </summary>
    /// <param name="_userID"></param>
    /// <param name="_userName"></param>
    protected void OnSwitchUserResult(string _user)
    {
        GoPassWord();
    }
    #endregion

    #region 登录部分 by吴江
    protected bool hasPlatformLogin = false;
    public bool HasPlatformLogin
    {
        get
        {
            return hasPlatformLogin;
        }
    }
	[HideInInspector]public int platformLoginSeralizeID = 0;
    protected SystemSettingMng.RendererQuality originQaulity = SystemSettingMng.RendererQuality.HIGHT;
    protected virtual void EnterLoginState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        hasPlatformLogin = false;
		GameCenter.systemSettingMng.SetQualitySettings(SystemSettingMng.RendererQuality.HIGHT);
        uIMng.ReleaseGUI(GUIType.AUTO_RECONNECT);
        uIMng.ReleaseGUI(GUIType.RECONNECT);
        uIMng.ReleaseGUI(GUIType.UPDATEASSET);
        cameraMng.BlackCoverAll(true);
        //if (curMainPlayer != null)
        //{
        //    Destroy(curMainPlayer);
        //}
        if (curGameStage != null)
        {
            curGameStage.UnRegistAll();
            DestroyImmediate(curGameStage, true);
            curGameStage = null;
        }
        curGameStage = stageObj.AddComponent<LoginStage>();
        soundMng.AutoPlayBGM();
    }

    protected virtual void ExitLoginState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        //cameraMng.BlackCoverAll(false);
        if (_to.name != "waitConnect")
        {
            ExitLoginState();
        }
        else
        {
            OnExitWaitConnect = ExitLoginState;
        }
    }

    protected void ExitLoginState()
    {
        uIMng.ReleaseGUI(GUIType.LOGIN);
        soundMng.StopPlayBGM();
    }

    protected virtual void UpdateLoginState(fsm.State _curState)
    {
        NetUpdate();
    }
    #endregion

    #region 等待连接部分 by吴江
    public enum WaitConnectType
    {
        normal,
        autoReconnect,
        playerRecconnect,
    }
    protected float enterConnectTime = 0;
    protected float connectWaitTime = 10.0f;
    protected bool hasFail = false;
    protected int connectSerlizedID = 0;
    protected float startConnectTime = 0;

    public System.Action OnExitWaitConnect = null;
    protected virtual void EnterWaitConnectState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        hasFail = false;
        enterConnectTime = Time.time;
        connectSerlizedID = (int)NetMsgMng.CreateNewSerializeID();
        msgLoackingMng.UpdateSerializeList(connectSerlizedID, true, true);
    }

    protected virtual void ExitWaitConnectState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        msgLoackingMng.UpdateSerializeList(connectSerlizedID, false, true);
        connectSerlizedID = 0;
        if (OnExitWaitConnect != null)
        {
            OnExitWaitConnect();
            OnExitWaitConnect = null;
        }
    }

    protected virtual void UpdateWaitConnectState(fsm.State _curState)
    {
        if (!hasFail && Time.time - enterConnectTime >= connectWaitTime)
        {
            hasFail = true;
            msgLoackingMng.UpdateSerializeList(connectSerlizedID, false, true);
            connectSerlizedID = 0;
            //NGUIDebug.Log("SwitchToUI(GUIType.RECONNECT) ,导致close 2");
            GameCenter.uIMng.SwitchToUI(GUIType.RECONNECT);
            if (mainPlayerMng != null && mainPlayerMng.MainPlayerInfo.Level == 1)
            {
                LynSdkManager.Instance.ReportConnectionLose("0", "1级时有掉线重连!");
            }
            LynSdkManager.Instance.ReportConnectionLose("0", "断开排队服务器,连接游戏服务器时超时!开始连接时间:" + enterConnectTime + ", 超时判断时间:" + Time.time);
        }
        else
        {
            NetUpdate();
        }
    }
    #endregion

    #region 选角部分 by吴江 //西游没有选角功能，by 何明军
    protected void EnterSelectCharState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        Resources.UnloadUnusedAssets();
        Destroy(stageObj.GetComponent<GameStage>());
        curGameStage = stageObj.AddComponent<CharacterSelectStage>();
        curGameStage.sceneMng = SceneMng.CreateNew();
        sceneMng = curGameStage.sceneMng;
    }

    protected void ExitSelectCharState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        soundMng.StopPlayBGM();
        GameCenter.systemSettingMng.SetQualitySettings(originQaulity);
        //CharacterSelectStage stage = curGameStage as CharacterSelectStage;
        //if (stage != null) stage.Stop();
        //if (_to.name == "createChar")
        //{
        //    cameraMng.BlackCoverAll(true);
        //}
    }

    protected void UpdateSelectCharState(fsm.State _curState)
    {
        NetUpdate(false);
    }
    #endregion


    #region 创角部分 by吴江
    protected void EnterCreateCharState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        curGameStage.UnRegistAll();
        curGameStage = null;
        Destroy(stageObj.GetComponent<GameStage>());
        curGameStage = stageObj.AddComponent<CharacterCreateStage>();
        curGameStage.sceneMng = SceneMng.CreateNew();
        sceneMng = curGameStage.sceneMng;
		soundMng.AutoPlayBGM(); 
    }

    protected void ExitCreateCharState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        if (_to.name != "waitConnect")
        {
            ExitCreateCharState();
        }
        else
        {
            OnExitWaitConnect = ExitCreateCharState;
        }
    }

    protected void ExitCreateCharState()
    {
        GameCenter.systemSettingMng.SetQualitySettings(originQaulity);
        CharacterCreateStage stage = curGameStage as CharacterCreateStage;
        if (stage != null) stage.Stop();
    }

    protected void UpdateCreateCharState(fsm.State _curState)
    {
        NetUpdate(false);
    }
    #endregion

    #region 主城部分 by吴江
    protected virtual void EnterInitCityState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        uIMng.ReleaseGUI(GUIType.AUTO_RECONNECT);
        uIMng.ReleaseGUI(GUIType.RECONNECT);

        if (curMainPlayer != null) curMainPlayer.CurTarget = null;
        if (sceneMng != null && curGameStage != null && curGameStage.SceneID == mainPlayerMng.MainPlayerInfo.SceneID && IsReConnecteding)
        {
            sceneMng.C2S_EnterSceneSucceed();
            PlayGameStage ps = curGameStage as PlayGameStage;
            if (ps != null)
            {
                ps.ExitReconnect();
            }
            GoRunCity();
        }
        else
        {
            if (curMainPlayer != null) curMainPlayer.CurTarget = null;
            dungeonMng = DungeonMng.CreateNew();

            uIMng.SwitchToUI(GUIType.LOADING);
            curGameStage.UnRegistAll();
            DestroyImmediate(curGameStage, true);
            curGameStage = null;
            sceneMng = SceneMng.CreateNew();
            curGameStage = stageObj.AddComponent<CityStage>();
            curGameStage.sceneMng = sceneMng;
            cameraMng.BlackCoverAll(true);
        }
    }

    protected virtual void ExitInitCityState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
		GameCenter.systemSettingMng.SetQualitySettings(originQaulity);
    }

    protected virtual void UpdateInitCityState(fsm.State _curState)
    {
        NetUpdate();
    }


    protected virtual void EnterRunCityState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        soundMng.AutoPlayBGM();
    }

    protected virtual void ExitRunCityState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        soundMng.StopPlayBGM();
    }

    protected virtual void UpdateRunCityState(fsm.State _curState)
    {
        NetUpdate();
    }
    #endregion

    #region 竞技场 部分 add 

    /// <summary>
    /// 进入竞技场 add .
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    /// <param name="_event"></param>
    protected virtual void EnterInitArenaState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        uIMng.ReleaseGUI(GUIType.RECONNECT);
        //GameCenter.mainPlayerMng.MainPlayerInfo.SceneFightingType = SceneMarkType.arena;//所有之前 其他逻辑参照

        GameCenter.uIMng.SwitchToUI(GUIType.LOADING);
        if (curMainPlayer != null) curMainPlayer.CurTarget = null;
        //Destroy(stageObj.GetComponent<GameStage>());
        //curGameStage = stageObj.AddComponent<ArenaStage>();
        curGameStage.sceneMng = SceneMng.CreateNew();
		dungeonMng = DungeonMng.CreateNew();
        sceneMng = curGameStage.sceneMng;
        soundMng.AutoPlayBGM();

    }

    protected virtual void ExitInitArenaState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
    }

    protected virtual void UpdateInitArenaState(fsm.State _curState)
    {
        NetUpdate();
    }

    protected virtual void UpdateRunArenaState(fsm.State _curState)
    {
        NetUpdate();
    }

    #endregion

    #region 地下城部分 by吴江
    protected virtual void EnterInitDungeonState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        uIMng.ReleaseGUI(GUIType.AUTO_RECONNECT);
        uIMng.ReleaseGUI(GUIType.RECONNECT);

        if (curMainPlayer != null) curMainPlayer.CurTarget = null;
        if (sceneMng != null && curGameStage != null && curGameStage is PlayGameStage && curGameStage.SceneID == mainPlayerMng.MainPlayerInfo.SceneID && IsReConnecteding)
        {
            sceneMng.C2S_EnterSceneSucceed();
            PlayGameStage ps = curGameStage as PlayGameStage;
            if (ps != null)
            {
                ps.ExitReconnect();
            }
            GoRunDungeon();
        }
        else
        {
            if (curMainPlayer != null) curMainPlayer.CurTarget = null;
            dungeonMng = DungeonMng.CreateNew();

            uIMng.SwitchToUI(GUIType.LOADING);
            curGameStage.UnRegistAll();
            DestroyImmediate(curGameStage, true);
            curGameStage = null;
            sceneMng = SceneMng.CreateNew();
            curGameStage = stageObj.AddComponent<DungeonStage>();
            curGameStage.sceneMng = sceneMng;
        }
    }

    protected virtual void ExitInitDungeonState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
    }

    protected virtual void UpdateInitDungeonState(fsm.State _curState)
    {
        NetUpdate();
    }


    protected virtual void EnterRunDungeonState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        soundMng.AutoPlayBGM();
    }

    protected virtual void ExitRunDungeonState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        soundMng.StopPlayBGM();
    }

    protected virtual void UpdateRunDungeonState(fsm.State _curState)
    {
        NetUpdate();
    }
    #endregion


    /// <summary>
    /// 切换到输入帐号密码的登录状态 by吴江
    /// </summary>
    public void GoPassWord()
    {
        if (msgLoackingMng != null)
        {
            msgLoackingMng.CleanSerializeList();
        }
        stateMachine.Send((int)EventType.LOGIN);
    }

    /// <summary>
    /// 切换到加载数据配置的状态 by吴江
    /// </summary>
    public void GoInitConfig()
    {
        stateMachine.Send((int)EventType.INIT_CONFIG);
    }

    /// <summary>
    /// 切换到选择角色状态 by吴江
    /// </summary>
    public void GoSelectChar()
    {
        stateMachine.Send((int)EventType.SELECTCHAR);
    }

    /// <summary>
    /// 切换到创建角色状态 by吴江
    /// </summary>
    public void GoCreatChar()
    {
        stateMachine.Send((int)EventType.CREATCHAR);
    }

    /// <summary>
    /// 切换到初始化主城状态 by吴江
    /// </summary>
    public void GoInitCity()
    {
        stateMachine.Send((int)EventType.ENTER_CITY);
    }

    /// <summary>
    /// 切换到运行主城的状态 by吴江
    /// </summary>
    public void GoRunCity()
    {
        stateMachine.Send((int)EventType.RUN_CITY);
    }

    /// <summary>
    /// 切换到初始化地下城的状态 by吴江
    /// </summary>
    public void GoInitDungeon()
    {
        stateMachine.Send((int)EventType.ENTER_DUNGEON);
    }

    /// <summary>
    /// 切换到运行地下城的状态 by吴江
    /// </summary>
    public void GoRunDungeon()
    {
        stateMachine.Send((int)EventType.RUN_DUNGEON);
    }


    /// <summary>
    /// 切换到初始化竞技场的状态 .
    /// </summary>
    public void GoInitArenaFight()
    {
        stateMachine.Send((int)EventType.ENTER_ARENA);
    }

    /// <summary>
    /// 进入竞技场 add .
    /// </summary>
    public void GotoArenaFight()
    {
        stateMachine.Send((int)EventType.RUN_ARENA);
    }
    /// <summary>
    /// 进入到更新界面 
    /// </summary>
    public void GotoUpdate()
    {
        stateMachine.Send((int)EventType.UPDATEASSET);
    }

    public void GoWaitConnect()
    {
        stateMachine.Send((int)EventType.WAIT_CONNECT);
    }

    public void GoPlatformLogin()
    {
        stateMachine.Send((int)EventType.PLATFORMLOGIN);
    }
    /// <summary>
    /// 是否是切换账号,切换账号不需要自行调用SDK登陆接口  by邓成
    /// </summary>
    [HideInInspector]public bool isSwitchAccount = false;
    /// <summary>
    /// 切换账号
    /// </summary>
    public void SwitchAccount()
    {
        if (isPlatform)
        {
            isSwitchAccount = true;
            GoPassWord();
            LynSdkManager.Instance.UsrLogout1();
        }
        else
        {
            GoPassWord();
        }
    }
    #endregion


    #region Gizmos
    /// <summary>
    /// 判断一个对象是否有dummy缓存 by吴江
    /// </summary>
    /// <param name="_objectType"></param>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static bool IsDummyDestroyed(ObjectType _objectType, int _id)
    {
        return (GameCenter.curGameStage == null || GameCenter.curGameStage.GetObject(_objectType, _id) == null);
    }

    /// <summary>
    /// 渲染一些逻辑对象在工程窗口，用于研发时的检测 by吴江
    /// </summary>
    void OnDrawGizmos()
    {
        if (!isDevelopmentPattern) return;
        if (curGameStage != null && curGameStage.sceneMng != null && mainPlayerMng != null)
        {
            float y = 0.5f;
            GameStage.Sector playerSector = null;
            if (curMainPlayer != null)
            {
                y = curMainPlayer.transform.position.y;
                playerSector = curMainPlayer.curSector;

                Gizmos.color = Color.green;
                DrawPath(curMainPlayer.GetMoveFSM());

                //DrawAbility(curMainPlayer.curTryUseAbility);
            }
            if (curMainEntourage != null)
            {
                DrawPath(curMainEntourage.GetMoveFSM());
            }
            Gizmos.color = Color.blue;
            if (showSector) curGameStage.DrawSector(y + 0.4f);
            if (showSectorObjectCount) curGameStage.DrawSectorObjectCountAt(playerSector, y + 0.5f);



            //Gizmos.color = Color.red;
            //List<Monster> mobList = curGameStage.GetMobs();
            //foreach (var item in mobList)
            //{
            //    DrawPath(item.GetMoveFSM());
            //}


        }
    }

    //protected void DrawAbility(AbilityInstance _instance)
    //{
    //    if (_instance == null || _instance.Scope == null || _instance.Scope.Length < 1) return;
    //    int[] _rangeData = _instance.Scope;
    //    int type = _rangeData[0];
    //    switch (type)
    //    {
    //        case 1://扇形，点距离在半径内，与两半径夹角小于等于，半径夹角	
    //            if (_rangeData.Length < 3) return;//如果数据配置错误
    //            int _r = 2 * _rangeData[2];
    //            Gizmos.DrawSphere(curMainPlayer.transform.position, _r);
    //            return;
    //        case 5:
    //        case 3://矩形，点在矩形四个坐标中
    //            if (_rangeData.Length < 3) return;//如果数据配置错误
    //            int _w = 2 * _rangeData[1];
    //            int _h = 2 * _rangeData[2];
    //            Gizmos.DrawCube(curMainPlayer.transform.position + _h * curMainPlayer.transform.forward, new Vector3(_w, 1, _h));
    //            return;
    //        case 4://圆，与玩家点，距离小于半径
    //            if (_rangeData.Length < 2) return;//如果数据配置错误
    //            int _r3 = 2 * _rangeData[1];
    //            Gizmos.DrawSphere(curMainPlayer.transform.position, _r3);
    //            return;
    //    }
    //}

    /// <summary>
    /// 渲染一个活动对象的当前寻路路线 by吴江
    /// </summary>
    /// <param name="_actor"></param>
    protected void DrawPath(ActorMoveFSM _actor)
    {
        if (_actor == null) return;
        if (_actor.path != null)
        {
            Vector3 lastPos = Vector3.zero;
            if (_actor.nextPathPoint <= 0)
            {
                lastPos = _actor.transform.position;
            }
            foreach (var item in _actor.path)
            {
                if (lastPos != Vector3.zero)
                {
                    Gizmos.DrawLine(lastPos, item);
                }
                Gizmos.DrawSphere(item, 0.5f);
                lastPos = item;
            }
        }
    }

	public void ShowNotice()
	{
		StartCoroutine(LoadNotice(true));
	}
	public void SetNotice()
	{
		StartCoroutine(LoadNotice(false));
	}
	IEnumerator LoadNotice(bool _show)
	{
        string sourceId = "0";
        if(GameCenter.instance.isPlatform)
            sourceId = LynSdkManager.Instance.GetSourceId();
        string time = (System.DateTime.Now.Ticks/10000).ToString();
        string sign = string.Format(SystemSettingMng.NOTICE_HTTP_ADDRESS_PARAMETER, sourceId, time);
        string signResult = GameHelper.SignString(sign);
        //Debug.Log("NOTICE signResult:" + signResult);
        WWW www = new WWW(string.Format(SystemSettingMng.NOTICE_HTTP_ADDRESS, sourceId, time, signResult));
		yield return www;
		LitJson.JsonData json = new LitJson.JsonData();
		json = LitJson.JsonMapper.ToObject(www.text);
		if (www.text.Contains("state"))
		{
            int status = (int)json["state"];
			if (status == 1)
			{
				string loginNotice = (string)json["data"];
				if(descriptionMng != null)
				{
					if(_show)
						descriptionMng.ShowLoginNotice(loginNotice);
					else
						descriptionMng.SetNotice(loginNotice);
				}
			}
			else if (status == -1)
			{
				Debug.Log("参数错误");
			}
			else if (status == 0)
			{
				Debug.Log("暂无数据");
			}
		}
	}
    #endregion

    /// <summary>
    /// 游戏退出 add .
    /// </summary>
    public void GameExit()
    {
        if (isPlatform)
        {
            LynSdkManager.Instance.UsrLogout(); 
            DateTime newServerTime = GameCenter.instance.CurServerTime;
            DateTime endTime = newServerTime.AddSeconds(36000);//离线十小时后发送推送消息
            string time = string.Format("{0:D2}:{1:D2}:{2:D2}", endTime.Hour, endTime.Minute, endTime.Second);
            GameCenter.messageMng.SendPushInfo(3, 3, time);
        }
        else
        {
            Application.Quit();
            NetMsgMng.ConectClose();//关掉net
        }
        //#if ANDROID
        //                DCAgent.onKillProcessOrExit();
        //#endif
    }


    #region 作弊命令
    protected bool openGodMsg = false;
    protected string godMsgStr = string.Empty;
    protected bool openCameraEditor = false;

    IEnumerator Test()
    { 
        long dateTime = DateTime.Now.Ticks / 10000;
        string sign = string.Format(SystemSettingMng.PAGE_SERVER_HTTP_ADDRESS_PARAMETER, 1,202, dateTime, "5.6.5");
        string signResult = GameHelper.SignString(sign);
        Debug.Log("PAGE_SERVER signResult:" + signResult);
		string wwwText =  string.Format(SystemSettingMng.PAGE_SERVER_HTTP_ADDRESS, 1, 202, dateTime, "5.6.5", signResult);

		//NGUIDebug.Log("wwwText" + wwwText);
		WWW www = new WWW(wwwText);
		yield return www;
        if (www.isDone)
        {
            Debug.Log("www.text:"+www.text);
        }
    }

    void OnGUI()
    {
        //if (GUI.Button(new Rect(200, 100, 50, 50), "攻城"))
        //{
        //    /*GameCenter.uIMng.SwitchToUI(GUIType.GUILDSIEGE);
        //    Debug.Log("CurfocusTask.NeedChatWithNpc:" + GameCenter.taskMng.CurfocusTask.NeedChatWithNpc(50004));
        //    GameCenter.uIMng.SwitchToSubUI(SubGUIType.EQUIPMENTWASH);
        //    GameCenter.endLessTrialsMng.OpenCopyWndSelected(SubGUIType.BCopyTypeOne, OneCopySType.DOUSHUAIXIANGONG);*/
        //    StartCoroutine(Test());
        //}
        //if (GUI.Button(new Rect(100, 100, 50, 50), "封神"))
        //{
            //GameCenter.activityMng.C2S_FlyFengShen();
            //GameCenter.endLessTrialsMng.OpenStartSeleteActivity(ActivityType.SEALOFTHE);
        //}
        //if (!isDevelopmentPattern) return;
        //if (curMainPlayer == null) return;
        //if (Input.GetKey(KeyCode.F2) && !openGodMsg)
        //{
        //    //Input.eatKeyPressOnTextFieldFocus = false;
        //    openGodMsg = true;
        //    godMsgStr = "!flymap 100000";
        //}
        //if (Input.GetKey(KeyCode.Return) && openGodMsg)
        //{
        //    openGodMsg = false;
        //    if (godMsgStr.Length > 2 && godMsgStr.StartsWith("!"))
        //    {
        //        ServerData.ChatMessageSendData st = new ServerData.ChatMessageSendData();
        //        st.Channel = 0;
        //        st.Content = "" + godMsgStr;
        //        st.ReceiverName = "";
        //        chatMng.C2S_messagesend(st);
        //    }
        //}
        //if (openGodMsg)
        //{
        //    godMsgStr = GUI.TextField(new Rect(Screen.width / 2.0f - 150, Screen.height / 2.0f - 10, 300, 20), godMsgStr);
        //}



        if (!isDevelopmentPattern) return;
        if (curMainPlayer == null) return;
        if (Input.GetKeyUp(KeyCode.F2) && !openGodMsg)
        {
            //Input.eatKeyPressOnTextFieldFocus = false;
            openGodMsg = true;
            godMsgStr = "!res 1 100000";
            openCameraEditor = false;
        }
        if (Input.GetKey(KeyCode.Return) && openGodMsg)
        {
            openGodMsg = false;
            if (godMsgStr.Length > 2)
            {
				pt_req_chat_to_world_d318 msg = new pt_req_chat_to_world_d318();
				msg.content = godMsgStr;
                msg.target_name = string.Empty;
                NetMsgMng.SendMsg(msg);
            }
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            
        }

        if (openGodMsg)
        {
            godMsgStr = GUI.TextField(new Rect(Screen.width / 2.0f - 150, Screen.height / 2.0f - 10, 300, 20), godMsgStr);
        }

        if (Input.GetKeyUp(KeyCode.F3) && !openCameraEditor)
        {
            openCameraEditor = true;
            openGodMsg = false;
        }

        if (Input.GetKey(KeyCode.Return) && openCameraEditor)
        {
            openCameraEditor = false;
        }

        if (openCameraEditor)
        {
            GUI.Label(new Rect(Screen.width / 2.0f - 230, Screen.height / 2.0f - 55, 60, 20), "镜头距离:");
            cameraMng.currentCtrl.distance = GUI.HorizontalSlider(new Rect(Screen.width / 2.0f - 150, Screen.height / 2.0f - 50, 300, 20), cameraMng.currentCtrl.distance, 10, 50);

            GUI.Label(new Rect(Screen.width / 2.0f - 230, Screen.height / 2.0f - 10, 60, 20), "镜头X:");
            cameraMng.currentCtrl.x = GUI.HorizontalSlider(new Rect(Screen.width / 2.0f - 150, Screen.height / 2.0f - 10, 300, 20), cameraMng.currentCtrl.x, -180, 180);


            GUI.Label(new Rect(Screen.width / 2.0f - 230, Screen.height / 2.0f + 35, 60, 20), "镜头Y:");
            cameraMng.currentCtrl.y = GUI.HorizontalSlider(new Rect(Screen.width / 2.0f - 150, Screen.height / 2.0f + 35, 300, 20), cameraMng.currentCtrl.y, -180, 180);
        }

    }
    #endregion

    #region 网络相关
    /// <summary>
    /// 网络是否断开
    /// </summary>
    bool isConnected = false;
    /// <summary>
    /// 网络是否断开
    /// </summary>
    public bool IsConnected
    {
        get
        {
            return isConnected;
        }
        protected set
        {
            if (isConnected != value)
            {
                isConnected = value;
                if (isConnected)
                {
                    if (loginMng != null)
                    {
                        loginMng.IsActiveDisconnection = false;
                    }
                }
                if (OnConnectStateChange != null)
                {
                    OnConnectStateChange(isConnected);
                }
            }
        }
    }

    /// <summary>
    /// 是否正在重新连接
    /// </summary>
    protected bool isReConnecteding = false;
    /// <summary>
    /// 是否正在重新连接
    /// </summary>
    public bool IsReConnecteding
    {
        get
        {
            return isReConnecteding;
        }
        set
        {
            isReConnecteding = value;
        }
    }

    /// <summary>
    /// 流畅ping值
    /// </summary>
    public const int pingSmooth = 200;
    /// <summary>
    /// 中等ping值
    /// </summary>
    public const int pingOrdinary = 500;
    /// <summary>
    /// 延迟ping值
    /// </summary>
    public const int pingMax = 1000;
    /// <summary>
    /// 断网ping值
    /// </summary>
    public const int pingOff = 20000;

    public const int pingMaxNum = 15;

    List<bool> pingListNum = new List<bool>();
    public List<bool> PingListNum
    {
        get
        {
            return pingListNum;
        }
    }
    protected long pingTime = 10;
    /// <summary>
    /// ping网络的总时间
    /// </summary>
    public long PingTime
    {
        get
        {
            return pingTime;
        }
        set
        {
            pingTime = value;
            pingListNum.Clear();
        }
    }


    protected long netDelayTime = 10;
    public long NetDelayTime
    {
        get
        {
            return netDelayTime;
        }
        protected set
        {
            netDelayTime = value;
        }
    }

    //int pingDegress = 0;
    /// <summary>
    /// 开始ping网络的时间
    /// </summary>
    protected long pingStartTiem = 0;
    public long PingStartTime
    {
        get
        {
            return pingStartTiem;
        }
        set
        {
            pingStartTiem = value;
            if (pingListNum.Count == pingMaxNum)
            {
                int sceneID = 0;
                PlayGameStage stage = GameCenter.curGameStage as PlayGameStage;
                if (stage != null)
                {
                    sceneID = stage.SceneID;
                    GameCenter.uIMng.SwitchToUI(GUIType.AUTO_RECONNECT);
                }
                else
                {
                    //NGUIDebug.Log("SwitchToUI(GUIType.RECONNECT) ,导致close 4");
                    // GameCenter.uIMng.SwitchToUI(GUIType.RECONNECT);
                }
                LynSdkManager.Instance.ReportConnectionLose(sceneID.ToString(), "ping超过" + pingMaxNum + "次,没有受到任何回应!判断为断线,打开断线重连界面!");
                GameCenter.uIMng.ReleaseGUI(GUIType.PANELLOADING);
            }
            if (pingListNum.Count < pingMaxNum)
            {
                pingListNum.Add(true);
                //Debug.Log("pingListNum.Count    " + pingListNum.Count);
                if (pingListNum.Count == 5)
                {
                    NetMsgMng.ConectClose();
                    //Debug.Log("打开弱网界面");
                    //GameCenter.uIMng.SwitchToUI(GUIType.WEAKNETWORK);
                    //GameCenter.messageMng.AddClientMsg(56, (x) =>
                    //{
                    //    GameCenter.instance.IsReConnecteding = false;
                    //    GameCenter.instance.GoPassWord();
                    //    if (GameCenter.mainPlayerMng != null && GameCenter.mainPlayerMng.MainPlayerInfo != null)
                    //    {
                    //        GameCenter.mainPlayerMng.MainPlayerInfo.CleanBuff();//清除buff
                    //    }
                    //});
                }
            }
        }
    }

    /// <summary>
    /// 连接状态发生变化的事件
    /// </summary>
    public static Action<bool> OnConnectStateChange;

    /// <summary>
    /// 0通讯忍耐时间
    /// </summary>
    public float pingWarnningTime = 2f;
    float curPingNetSendStartTime = 0;

    protected bool lastFrameHasPing = false;

    protected void NetUpdate(bool _needPing = true)
    {
        MsgHander.Update();
        IsConnected = NetCenter.Connected;
        if (IsConnected)
        {
            if (NetCenter.LastConnectedPing > 0)//如果有收到任何协议则NetCenter.LastConnectedPing清零
            {
                curPingNetSendStartTime += Time.deltaTime;
                if (curPingNetSendStartTime - NetCenter.LastConnectedPing > pingWarnningTime)
                {
                    if (Time.frameCount % 30 == 0)
                    {
                        curPingNetSendStartTime = 0;

                        loginMng.C2S_Ping();
                        lastFrameHasPing = true;
                    }
                }
                else
                {
                    if (Time.frameCount % 150 == 0)
                    {
                        loginMng.C2S_Ping();
                        lastFrameHasPing = true;
                    }
                }
            }
            else
            {
                if (PingListNum.Count > 0)
                {
                    PingListNum.Clear();
                }
                if (lastFrameHasPing)
                {
                    NetDelayTime = (NetDelayTime * 4 + PingTime) / 5;
                    lastFrameHasPing = false;
                }
                else
                {
                    if (Time.frameCount % 150 == 0)
                    {
                        loginMng.C2S_Ping();
                        lastFrameHasPing = true;
                    }
                }
                curPingNetSendStartTime = 0;
            }
        }
    }
    #endregion

    public static Action OnServerTimeUpdate;
	const uint beforeDawnTime = 86400;
	/// <summary>
	/// 一天的总秒数
	/// </summary>
	public uint BeforeDawnTime{
		get {
			return beforeDawnTime;
		}
	}
	DateTime curServerTime;
	int CurRealtimeSinceStartup = 0;
	DateTime newServerTime;
	/// <summary>
	/// 当前服务器真实时间 by 何明军
	/// </summary>
	public DateTime CurServerTime{
		get{
			newServerTime = curServerTime.AddSeconds((int)Time.realtimeSinceStartup - CurRealtimeSinceStartup);
			return newServerTime;
		}
		set{
			CurRealtimeSinceStartup = (int)Time.realtimeSinceStartup;
			curServerTime = value;
			if(OnServerTimeUpdate != null)OnServerTimeUpdate();
		}
	}
	/// <summary>
	/// 服务器同步时间点的总秒数 by 何明军
	/// </summary>
	public int ServerTimePointSecond{
		get{
			return curServerTime.Hour * 3600 + curServerTime.Minute * 60 + curServerTime.Second - CurRealtimeSinceStartup;
		}
	}
	/// <summary>
	/// 当前服务器真实时间 的总秒数 by 何明军
	/// </summary>
	public int CurServerTimeSecond{
		get{
			DateTime date = CurServerTime;
			return date.Hour * 3600 + date.Minute * 60 + date.Second;
		}
	}
    

}