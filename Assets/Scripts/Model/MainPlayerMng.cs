//==================================
//作者：吴江
//日期：2015/5/21
//用途：主玩家的数据和通信管理类
//=================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;


/// <summary>
/// 主玩家的数据和通信管理类 by吴江
/// </summary>
public class MainPlayerMng
{

    /// <summary>
    /// 更换主玩家的事件
    /// </summary>
    public static System.Action OnCreateNew;

    protected bool isWaitTouchSceneItemMsg = false;
    /// <summary>
    /// 是否在等待采集请求
    /// </summary>
    public bool IsWaitTouchSceneItemMsg
    {
        get
        {
            if (isWaitTouchSceneItemMsg)
            {
                if (Time.time - startTouchTime >= SystemSettingMng.TIME_OUT_LIMIT)
                {
                    isWaitTouchSceneItemMsg = false;
                }
            }
            return isWaitTouchSceneItemMsg;
        }
		set
		{
			isWaitTouchSceneItemMsg = value;
		}
    }

    protected float startTouchTime = 0;

    #region 构造
    public static MainPlayerMng CreateNew(MainPlayerInfo _mainplayerInfo)
    {
        if (GameCenter.mainPlayerMng == null)
        {
            MainPlayerMng mainPlayerMng = new MainPlayerMng();
            mainPlayerMng.Init(_mainplayerInfo);
            return mainPlayerMng;
        }
        else
        {
            GameCenter.mainPlayerMng.UnRegist();
            GameCenter.mainPlayerMng.Init(_mainplayerInfo);
            return GameCenter.mainPlayerMng;
        }
    }


    protected void Init(MainPlayerInfo _mainplayerInfo)
    {
        mainPlayerInfo = _mainplayerInfo;
		functionList.Clear();
        hasApplySubData = false;
        MsgHander.Regist(0xB107, S2C_OnChangePos);
        MsgHander.Regist(0xB104, S2C_OnLoginInGame);
       // MsgHander.Regist(0xD204, S2C_CollectResult);
        MsgHander.Regist(0xD01D, S2C_OnBaseValueChange);
		//MsgHander.Regist(0xD329, S2C_VIPResult);
		MsgHander.Regist(0xD442, S2C_ReinNum);
		MsgHander.Regist(0xB105,S2C_CurServerTime);
		//MsgHander.Regist(0xD779,S2C_ServerStartGuide);
        MsgHander.Regist(0xC105, S2C_UpdateFuncReward);
        //物品管理类
        inventoryMng = InventoryMng.CreateNew(this);
        GameCenter.inventoryMng = inventoryMng;
        //宠物管理类
        //petMng = PetMng.CreateNew(this);
        //GameCenter.petMng = petMng;
        //新排行榜管理类
        newRankingMng = NewRankingMng.CreateNew();
        GameCenter.newRankingMng = newRankingMng;
        //法宝管理类
        magicWeaponMng = MagicWeaponMng.CreateNew(this);
        GameCenter.magicWeaponMng = magicWeaponMng;
        //成就管理类
        achievementMng = AchievementMng.CreateNew(this);
        GameCenter.achievementMng = achievementMng;
        //七天奖励管理类
        sevenDayMng = SevenDayMng.CreateNew();
        GameCenter.sevenDayMng = sevenDayMng;
        //首冲大礼管理类
        firstChargeBonusMng = FirstChargeBonusMng.CreateNew();
        GameCenter.firstChargeBonusMng = firstChargeBonusMng;
        //翅膀管理类
        wingMng = WingMng.CreateNew(this);
        GameCenter.wingMng = wingMng;

        rebornMng = RebornMng.CreateNew(this);
        GameCenter.rebornMng = rebornMng;

//		sceneAnimMng = SceneAnimMng.CreateNew();
//		GameCenter.sceneAnimMng = sceneAnimMng;

        //随从管理类
        mercenaryMng = MercenaryMng.CreateNew();
        GameCenter.mercenaryMng = mercenaryMng;

        newMountMng = NewMountMng.CreateNew();
        GameCenter.newMountMng = newMountMng;

        guildMng = GuildMng.CreateNew(this);
        GameCenter.guildMng = guildMng;

		dailyMustDoMng = DailyMustDoMng.CreateNew();
		GameCenter.dailyMustDoMng = dailyMustDoMng;

        taskMng = TaskMng.CreateNew(this);
        GameCenter.taskMng = taskMng;

        teamMng = TeamMng.CreateNew();
        GameCenter.teamMng = teamMng;
        // 好友管理类 by朱素云
        friendsMng = FriendsMng.CreateNew();
        GameCenter.friendsMng = friendsMng;
        // 等级奖励管理类 by朱素云
        rankRewardMng = RankRewardMng.CreateNew();
        GameCenter.rankRewardMng = rankRewardMng;
        // 修行管理类 by朱素云
        practiceMng = PracticeMng.CreateNew();
        GameCenter.practiceMng = practiceMng;
        // 仙侣管理类 by朱素云
        coupleMng = CoupleMng.CreateNew();
        GameCenter.coupleMng = coupleMng;
        // 周卡管理类 by朱素云
        weekCardMng = WeekCardMng.CreateNew();
        GameCenter.weekCardMng = weekCardMng;
        // 爱心礼包管理类 by朱素云
        lovePackageMng = LovePackageMng.CreateNew();
        GameCenter.lovePackageMng = lovePackageMng;
        // 结义管理类 by朱素云
        swornMng = SwornMng.CreateNew();
        GameCenter.swornMng = swornMng;
        //副本选择管理类
        dungeonMng = DungeonMng.CreateNew();
        GameCenter.dungeonMng = dungeonMng;
        // 火焰山战场管理类 by朱素云
        battleFightMng = BattleFightMng.CreateNew();
        GameCenter.battleFightMng = battleFightMng;




        //技能升级管理类
        skillMng = SkillMng.CreateNew(ref skillMng);
        GameCenter.skillMng = skillMng;

        //称号管理类 by 贺丰
        titleMng = TitleMng.CreateNew(ref titleMng);
        GameCenter.titleMng = titleMng;
         
        //邮箱管理类
		mailBoxMng = XXMailMng.CreateNew();
        GameCenter.mailBoxMng = mailBoxMng;
		
		endLessTrialsMng = EndLessTrialsMng.CreateNew();
		GameCenter.endLessTrialsMng = endLessTrialsMng;

        //聊天管理类
        chatMng = ChatMng.CreateNew();
        GameCenter.chatMng = chatMng; 

		
//        GameCenter.taskMng.updateSingleTask += TaskOpenFunction;
		GameCenter.taskMng.OnTaskGuideUpdateEvent += TaskProcessOpenFunction;
		GameCenter.taskMng.OnTaskFinishedGuideUpdateEvent += TaskFinishedOpenFunction;

		equipmentTraningMng = EquipmentTrainingMng.CreateNew();
		GameCenter.equipmentTrainingMng = equipmentTraningMng;

		//时装管理类 
		fashionMng = FashionMng.CreateNew(this);
		GameCenter.fashionMng = fashionMng;

		//商店管理类 
		shopMng = ShopMng.CreateNew(this);
		GameCenter.shopMng = shopMng;
		//仙盟商店管理类 
		guildShopMng = GuildShopMng.CreateNew(this);
		GameCenter.guildShopMng = guildShopMng;
		//仙盟技能管理类 
		guildSkillMng = GuildSkillMng.CreateNew(this);
		GameCenter.guildSkillMng = guildSkillMng;

		//下载管理类 
		downloadBonusMng =DownloadBonusMng.CreateNew(this);
		GameCenter.downloadBonusMng = downloadBonusMng;


		//商城管理类 
		newMallMng = NewMallMng.CreateNew(this);
		GameCenter.newMallMng= newMallMng;


		//物品购买
		buyMng = BuyMng.CreateNew (this);
		GameCenter.buyMng = buyMng;

        //交易
        tradeMng = TradeMng.CreateNew(this);
        GameCenter.tradeMng = tradeMng;


        //充值管理
        rechargeMng = RechargeMng.CreateNew(this);
        GameCenter.rechargeMng = rechargeMng;

		//市场
		marketMng = MarketMng.CreateNew (this);
		GameCenter.marketMng = marketMng;

		//铸魂
		castSoulMng = CastSoulMng.CreateNew (this);
		GameCenter.castSoulMng = castSoulMng;

		//在线奖励
		onlineRewardMng = OnlineRewardMng.CreateNew (this);
		GameCenter.onlineRewardMng = onlineRewardMng;

        //仙域争霸
        guildFightMng = GuildFightMng.CreateNew(this);
        GameCenter.guildFightMng = guildFightMng;

        //精彩活动
        wdfActiveMng = WdfActiveMng.CreateNew();
        GameCenter.wdfActiveMng = wdfActiveMng;

        //开服贺礼
        openServerRewardMng = OpenServerRewardMng.CreateNew(this);
        GameCenter.openServerRewardMng = openServerRewardMng;
        
        //复活
        resurrectionMng = ResurrectionMng.CreateNew(this);
        GameCenter.resurrectionMng = resurrectionMng;
        //小助手
        littleHelperMng = LittleHelperMng.CreateNew(this);
        GameCenter.littleHelperMng = littleHelperMng;

        //藏宝阁管理类
        treasureHouseMng = TreasureHouseMng.CreateNew();
        GameCenter.treasureHouseMng = treasureHouseMng;

        //皇室宝箱管理类
        royalTreasureMng = RoyalTreasureMng.CreateNew();
        GameCenter.royalTreasureMng = royalTreasureMng;

        //奇缘系统管理类
        newMiracleMng = MiracleMng.CreateNew();
        GameCenter.miracleMng = newMiracleMng;

        //二冲系统管理类
        newTwoChargeMng = TwoChargeMng.CreateNew();
        GameCenter.twoChargeMng = newTwoChargeMng;
       

		bossChallengeMng = BossChallengeMng.CreateNew();
		GameCenter.bossChallengeMng = bossChallengeMng;

		activityMng = ActivityMng.CreateNew();
		GameCenter.activityMng = activityMng;
        //单人副本多人副本的管理类
        duplicateMng = DuplicateMng.CreateNew();
        GameCenter.duplicateMng = duplicateMng;
        //竞技场管理类
        arenaMng = ArenaMng.CreateNew();
        GameCenter.arenaMng = arenaMng;
        //新手引导管理类
        noviceGuideMng = NoviceGuideMng.CreateNew();
        GameCenter.noviceGuideMng = noviceGuideMng;

        //离线经验管理类
        offLineRewardMng = OffLineRewardMng.CreateNew();
        GameCenter.offLineRewardMng = offLineRewardMng;
        //VIP管理类
        vipMng = VipMng.CreateNew();
        GameCenter.vipMng = vipMng;
        //宝藏活动的管理类
        treasureTroveMng = TreasureTroveMng.CreateNew();
        GameCenter.treasureTroveMng = treasureTroveMng;
        //七日挑战管理类
        sevenChallengeMng = SevenChallengeMng.CreateNew();
        GameCenter.sevenChallengeMng = sevenChallengeMng;
        //走马灯 by hmj
        GameCenter.uIMng.GenGUI(GUIType.MERRYGOROUND,true); 
		//MsgHander.Regist(0xD804,S2C_GuideSeqencingList);
		MsgHander.Regist(0xD786,S2C_ServerStartTime);
		MsgHander.Regist(0xD70A,S2C_StartCameraFocus);
		MsgHander.Regist(0xD70B,S2C_EndCameraFocus);
		
		IsUpdateQuestionList = true;
		serverStartTiem = DateTime.Now;
    }


    protected void UnRegist()
    {
        MsgHander.UnRegist(0xB107, S2C_OnChangePos);
        MsgHander.UnRegist(0xB104, S2C_OnLoginInGame);
     //   MsgHander.UnRegist(0xD204, S2C_CollectResult);
		//MsgHander.UnRegist(0xD329, S2C_VIPResult);
		MsgHander.UnRegist(0xD442, S2C_ReinNum);
		MsgHander.UnRegist(0xB105,S2C_CurServerTime); 
		//MsgHander.UnRegist(0xD804,S2C_GuideSeqencingList);
		//MsgHander.UnRegist(0xD779,S2C_ServerStartGuide);
		MsgHander.UnRegist(0xD786,S2C_ServerStartTime);
		MsgHander.UnRegist(0xD70A,S2C_StartCameraFocus);
		MsgHander.UnRegist(0xD70B,S2C_EndCameraFocus);
        MsgHander.UnRegist(0xC105, S2C_UpdateFuncReward);
        MsgHander.UnRegist(0xD01D, S2C_OnBaseValueChange);
//        GameCenter.taskMng.updateSingleTask -= TaskOpenFunction;
		GameCenter.taskMng.OnTaskGuideUpdateEvent -= TaskProcessOpenFunction;
		GameCenter.taskMng.OnTaskFinishedGuideUpdateEvent -= TaskFinishedOpenFunction;
		//vipData = null;
		questionList.Clear();
		IsUpdateQuestionList = false;
		functionList.Clear();
		functionSequence = 0;
		pkModelTipShow = false;
        curGetRewardStep = 1;
    }
    #endregion

    #region 数据
    /// <summary>
    /// 是否提示消耗普通神元丹用于转生
    /// </summary>
    public bool ShowUseNormalGoldPillTip = true;
    /// <summary>
    /// 是否提示消耗高级神元丹用于转生
    /// </summary>
    public bool ShowUseSeniorGoldPillTip = true;


	/// <summary>
	/// pk模式提示是否显示
	/// </summary>
	public bool pkModelTipShow = false;
	
	DateTime serverStartTiem;
	
	/// <summary>
	/// 距离开服第几天还差多少时间
	/// </summary>
	public int ServerStartTiem(int _day,DateTime _curTime){
		if(_curTime == null)_curTime = GameCenter.instance.CurServerTime;
		if(_curTime == null){return -1;}
		int seconds = (int)(_curTime - serverStartTiem).TotalSeconds;
		int totalSeconds = _day * (int)GameCenter.instance.BeforeDawnTime;
		if(seconds > totalSeconds){
			return -1;
		}else{
			return totalSeconds - seconds;
		}
	}
	
	//#region VIP 
	///// <summary>
	///// vip 数据更新
	///// </summary>
	//public System.Action OnVIPDataUpdate;
	///// <summary>
	///// vip 数据
	///// by 何明军
	///// </summary>
	//public VIPDataInfo VipData{
	//	get{
	//		return vipData;
	//	}
	//}
	//VIPDataInfo vipData;
	//#endregion
    /// <summary>
    /// 其他依赖项子数据是否已经向服务端申请 by吴江
    /// </summary>
    protected bool hasApplySubData = false;
    public bool HasApplySubData
    {
        get { return hasApplySubData; }
    }
    /// <summary>
    /// 主玩家的数据 by吴江
    /// </summary>
    protected MainPlayerInfo mainPlayerInfo = null;
    /// <summary>
    /// 主玩家的数据 by吴江
    /// </summary>
    public MainPlayerInfo MainPlayerInfo
    {
        get { return mainPlayerInfo; }
    }
	
	#region Mng
    /// <summary>
    /// 物品数据管理类对象 by吴江
    /// </summary>
    public InventoryMng inventoryMng = null;
      /// <summary>
    /// 任务数据管理类对象 by吴江
    /// </summary>
    public TaskMng taskMng = null;
    /// <summary>
    /// 队伍管理类
    /// </summary>
    public TeamMng teamMng = null;
    /// <summary>
    /// 法宝管理类
    /// </summary>
    public MagicWeaponMng magicWeaponMng = null;
    /// <summary>
    /// 排行榜管理类
    /// </summary>
    public NewRankingMng newRankingMng = null;
    /// <summary>
    /// 成就管理类
    /// </summary>
    public AchievementMng achievementMng = null;
    /// <summary>
    /// 七天奖励管理类
    /// </summary>
    public SevenDayMng sevenDayMng = null;
    /// <summary>
    /// 首冲大礼管理类
    /// </summary>
    public FirstChargeBonusMng firstChargeBonusMng = null;
    /// <summary>
    /// 复活管理类
    /// </summary>
    public RebornMng rebornMng = null;

//	public SceneAnimMng sceneAnimMng;
    /// <summary>
    /// 公会
    /// </summary>
    public GuildMng guildMng = null;
	/// <summary>
	/// 每日必做
	/// </summary>
	public DailyMustDoMng dailyMustDoMng = null;
    /// <summary>
    /// 随从管理类
    /// </summary>
    public MercenaryMng mercenaryMng = null;
    /// <summary>
    /// 坐骑管理类
    /// </summary>
    public NewMountMng newMountMng = null;
    /// <summary>
    /// 技能升级管理类 by 贺丰
    /// </summary>
    public SkillMng skillMng = null;
    /// <summary>
    /// 好友管理类 by朱素云
    /// </summary>
    public FriendsMng friendsMng = null;
    /// <summary>
    /// 等级奖励管理类 by朱素云
    /// </summary>
    public RankRewardMng rankRewardMng = null; 
    /// <summary>
    /// 修行管理类 by朱素云
    /// </summary>
    public PracticeMng practiceMng = null; 
    /// <summary>
    /// 仙侣管理类 by朱素云
    /// </summary>
    public CoupleMng coupleMng = null;
    /// <summary>
    /// 周卡管理类 by朱素云
    /// </summary>
    public WeekCardMng weekCardMng = null;
    /// <summary>
    /// 结义管理类 by朱素云
    /// </summary>
    public SwornMng swornMng = null;
    /// <summary>
    /// 爱心礼包管理类 by 朱素云
    /// </summary>
    public LovePackageMng lovePackageMng = null;
    /// <summary>
    /// 火焰山战场管理类by朱素云
    /// </summary>
    public BattleFightMng battleFightMng = null;
    /// <summary>
    /// 副本选择管理类 by龙英杰
    /// </summary>
    public DungeonMng dungeonMng = null;
	/// <summary>
	/// 时装管理类
	/// </summary>
	public  FashionMng fashionMng=null;
    /// <summary>
    /// 翅膀管理类
    /// </summary>
    public WingMng wingMng = null;

	/// <summary>
	/// 仙盟技能
	/// </summary>
	public  GuildSkillMng guildSkillMng=null;

	/// <summary>
	/// 下载奖励
	/// </summary>
	public  DownloadBonusMng downloadBonusMng=null;

	/// <summary>
	/// 仙盟商店
	/// </summary>
	public  GuildShopMng guildShopMng=null;
	/// <summary>
	/// 商店
	/// </summary>
	public  ShopMng shopMng=null;
	/// <summary>
	/// 商城
	/// </summary>
	public  NewMallMng newMallMng=null;
	/// <summary>
	/// 物品购买
	/// </summary>
	public BuyMng buyMng=null;
	/// <summary>
	/// 交易
	/// </summary>
    public TradeMng tradeMng = null;
    /// <summary>
    /// 充值管理
    /// </summary>
    public RechargeMng rechargeMng = null;
	/// <summary>
	/// 市场
	/// </summary>
	public MarketMng marketMng=null;
	/// <summary>
	/// 铸魂
	/// </summary>
	public CastSoulMng castSoulMng=null;
	/// <summary>
	/// 在线奖励
	/// </summary>
	public OnlineRewardMng onlineRewardMng=null;
    /// <summary>
	/// 仙域争霸
	/// </summary>
    public GuildFightMng guildFightMng = null;
    /// <summary>
    /// 精彩活动
    /// </summary>
    public WdfActiveMng wdfActiveMng = null;
    /// <summary>
    /// 开服贺礼
    /// </summary>
    public OpenServerRewardMng openServerRewardMng = null;
    /// <summary>
	/// 复活
	/// </summary>
    public ResurrectionMng resurrectionMng = null;
    /// <summary>
	/// 小助手
	/// </summary>
    public LittleHelperMng littleHelperMng = null;

    /// <summary>
    /// 称号管理类 by 贺丰
    /// </summary>
    public TitleMng titleMng = null;

    /// <summary>
    /// 藏宝阁管理类
    /// </summary>
    public TreasureHouseMng treasureHouseMng = null;

    /// <summary>
    /// 皇室宝箱管理类
    /// </summary>
    public RoyalTreasureMng royalTreasureMng = null;
    /// <summary>
    /// 奇缘系统管理类
    /// </summary>
    public MiracleMng newMiracleMng = null;
    /// <summary>
    /// 二冲系统管理类
    /// </summary>
    public TwoChargeMng newTwoChargeMng = null;
	/// <summary>
	/// Boss挑战管理类
	/// </summary>
	public static BossChallengeMng bossChallengeMng;


	public static ActivityMng activityMng;
     
    /// <summary>
    /// 邮箱管理类
    /// </summary>
	public static XXMailMng mailBoxMng;
    /// <summary>
    /// 无尽挑战管理类
    /// </summary>
    public static EndLessTrialsMng endLessTrialsMng = null;
    /// <summary>
    /// 单人副本多人副本管理类
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
	public static EquipmentTrainingMng equipmentTraningMng;
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
    /// 宝藏活动管理类
    /// </summary>
    public static TreasureTroveMng treasureTroveMng;
    /// <summary>
    /// 七日目标管理类
    /// </summary>
    public static SevenChallengeMng sevenChallengeMng;
    #endregion
    /// <summary>
    /// 记录主玩家最后进入的副本场景ID
    /// </summary>
    public int lastSceneID = -1;

    #region 功能开启 by 何明军
    /// <summary>
    /// 是否是第一次登陆（合成红点只在登陆时判定一次）
    /// </summary>
    public bool isFirstOpenBackSynUI = true;
    /// <summary>
    /// 开启功能,功能按钮红点数据
    /// </summary>
	private FDictionary functionList = new FDictionary();
	
	int functionSequence = 0;
	/// <summary>
	/// 功能开启进度
	/// </summary>
	public int FunctionSequence{
		get{
			return functionSequence;
		}
        set
        {
            functionSequence = value;
        }
	}
	bool startGuide = false;
	/// <summary>
	/// 是否在进行引导
	/// </summary>
	public bool StartGuide{
		get {
			return startGuide;
		}
	}
	//bool startGuide = false;
	///// <summary>
	///// 是否在进行引导
	///// </summary>
	//public bool StartGuide{
	//	get {
	//		return startGuide;
	//	}
	//}
 //   /// <summary>
 //   /// 新手引导进入的副本ID
 //   /// </summary>
 //   public int newFunctionCopyId = 0;
    public TaskInfo guildTask; 
    /// <summary>
    /// 后台活动开启状态更新事件
    /// </summary>
	public Action<FunctionDataInfo, bool> UpdateServerOpen;
 //   /// <summary>
 //   /// 更新新功能事件
 //   /// </summary>
	//public Action<FunCtionDataInfo> UpdateFunctionData;
	/// <summary>
	/// 更新功能红点事件
	/// </summary>
	public Action<FunctionDataInfo> UpdateFunctionRed;
	///// <summary>
	///// 开启引导事件
	///// </summary>
	//public Action<OpenNewFunctionGuideRef> UpdateGuideData;
    /// <summary>
    /// 当前领奖阶段
    /// </summary>
    public int curGetRewardStep = 1;
    /// <summary>
    /// 功能预告领取奖励
    /// </summary>
    public Action<TaskType> UpdateFunctionReward;
    /// <summary>
    /// 设置后台活动开启 by黄洪兴
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_isOpen"></param>
    public void SetServerActiveOpen(FunctionType _type, bool _isOpen)
    {
		FunctionDataInfo funcInfo = null;
		if(functionList.ContainsKey((int)_type)){
			funcInfo = functionList[(int)_type] as FunctionDataInfo;
            funcInfo.Update(_isOpen);
		}else{
            funcInfo = new FunctionDataInfo(_type, _isOpen, FunctionDataInfo.FunctionControlType.PROGRAMCONTROL);
			functionList[(int)_type] = funcInfo;
		}
		if (UpdateServerOpen != null && funcInfo != null)
        {
            UpdateServerOpen(funcInfo, _isOpen);
            //Debug.Log("后台活动开启更新");
        }

    }
	
	void TaskProcessOpenFunction(TaskInfo task)
    {
		OpenFunction(task,true);
	}
	
	void TaskFinishedOpenFunction(TaskInfo task)
    {
		OpenFunction(task,false);
	}
	void OpenFunction(TaskInfo task,bool taskNew)
    {
		if(task.FuncSequence.Count == 0 || task.TaskState == TaskStateType.ENDED || task.TaskState == TaskStateType.UnTake)return ;
		for(int i=0;i<task.FuncSequence.Count;i++)
        {
			OpenNewFunctionRef refData = ConfigMng.Instance.GetOpenNewFunctionRef(task.FuncSequence[i]);
			if(refData == null){
				Debug.Log("任务task="+task.Task+"step="+task.Step+"的开启功能id。在功能表中没有找到数据");
				continue ;
			}
			if(refData.open_conditions == 2 && taskNew){
				continue;
			}
			if(refData.open_conditions == 1 && !taskNew){
				continue;
			}
            guildTask = task;
			FunctionDataInfo funcInfo  = null;
			if(refData.func_type <= 0)
            {//没有与功能相关的引导或者其他处理
				funcInfo  = new FunctionDataInfo(refData);
			}else
            {
				if(functionList.ContainsKey(refData.func_type))
                {
					funcInfo = functionList[refData.func_type] as FunctionDataInfo;
				} 
			}
			if(funcInfo!= null)
            {
				funcInfo.Update(true);
				if(GameCenter.noviceGuideMng.UpdateFunctionData != null)
                    GameCenter.noviceGuideMng.UpdateFunctionData(funcInfo);//开启多个功能时，只支持第一个功能有开启表现
				if(funcInfo.Seqencing != functionSequence)
                {
					functionSequence = funcInfo.Seqencing;
					GameCenter.noviceGuideMng.C2S_GuideSeqencing(funcInfo.Seqencing);
				}
			}
		}
	}
	///// <summary>
	///// 开启指引
	///// </summary>
	//public void OpenGuide(int _type,int _step = 1){
	//	OpenNewFunctionGuideRef guideData = ConfigMng.Instance.GetOpenNewFunctionGuideRef(_type,_step);
	//	if(guideData == null){
	//		Debug.LogError("没有找到Type = "+_type+",Step = "+_step+"的指引数据！");
	//		return ;
	//	}
	//	OpenGuide(guideData);
	//}
	///// <summary>
	///// 开启指引
	///// </summary>
	//public void OpenGuide(OpenNewFunctionGuideRef _guideData)
 //   {
	//	if(_guideData == null)return ;
	//	if(!startGuide)
 //           startGuide = true;
	//	if(UpdateGuideData != null)
 //           UpdateGuideData(_guideData);
	//}
	///// <summary>
	///// 指引结束
	///// </summary>
	//public void OverGuide()
 //   {
	//	if(startGuide)startGuide = false;
	//}
	/// <summary>
	///  获得功能
	/// </summary
	public FunctionDataInfo GetFunctionData(FunctionType _func)
    {
		if(functionList.ContainsKey((int)_func)){
			return functionList[(int)_func] as FunctionDataInfo;
		}
		return null;
	}
    /// <summary>
    /// 获取某个功能是否显示红点
    /// </summary>
    public bool GetFunctionIsRed(FunctionType _func)
    {
        if (functionList.ContainsKey((int)_func))
        {
            FunctionDataInfo data = functionList[(int)_func] as FunctionDataInfo;
            return data != null ? data.FuncBtnRed : false;
        }
        return false;
    }

	/// <summary>
	/// 设置功能相关红点显示
	/// </summary>
	public void SetFunctionRed(FunctionType _func,bool isRed)
    {
		FunctionDataInfo funcInfo = null;
		if(functionList.ContainsKey((int)_func))
        {
			funcInfo = functionList[(int)_func] as FunctionDataInfo;
			if(funcInfo != null && isRed != funcInfo.FuncBtnRed){
				funcInfo.FuncBtnRed = isRed;
			}
		}
        else
        {
			OpenNewFunctionRef refData = ConfigMng.Instance.GetOpenNewFunctionRef((int)_func,mainPlayerInfo.Prof);
			if(refData == null){
				funcInfo = new FunctionDataInfo(_func);
				funcInfo.FuncBtnRed = isRed;
				functionList[(int)_func] = funcInfo;
			}else{
				funcInfo = new FunctionDataInfo(refData);
				funcInfo.FuncBtnRed = isRed;
				functionList[refData.func_type] = funcInfo;
			}
		}
		
	}
	/// <summary>
	/// 获取功能开启状态
	/// </summary>
	public bool FunctionIsOpen(FunctionType _func)
    {
		if(functionList.ContainsKey((int)_func))
        {
			FunctionDataInfo funcInfo = functionList[(int)_func] as FunctionDataInfo;
			if(funcInfo != null)return funcInfo.IsOpon;
		}else
        {
			Debug.LogError("功能Type ="+(int)_func+" 没有数据");
		}
		return true;
	}
	/// <summary>
	/// 初始化新功能开启数据
	/// </summary>
	public void InitFunctionData(){
        //functionList.Clear();
        //Debug.Log("InitFunctionData()");
		FDictionary dic = ConfigMng.Instance.OpenNewFunctionRefTable();
		FunctionDataInfo funcInfo = null;
		foreach(OpenNewFunctionRef data in dic.Values){
			if(data.prof > 0 && data.prof != mainPlayerInfo.Prof)continue;
			if(data.func_type > 0){
				if(!functionList.ContainsKey(data.func_type)){
					funcInfo = new FunctionDataInfo(data);
					functionList[data.func_type] = funcInfo;
				}else{
					funcInfo = functionList[data.func_type] as FunctionDataInfo;
					funcInfo.Update();
				}
                //Debug.Log("funcInfo.Type:" + funcInfo.Type);
			}
			
		}
	}
	//protected void S2C_GuideSeqencingList(Pt _info)
	//{
	//	pt_new_function_open_aready_d804 info = _info as pt_new_function_open_aready_d804;
	//	if(info != null){
	//		functionSequence = info.openlists;
	//	}
		
	//	InitFunctionData();
	//}
	///// <summary>
	///// 指引进度
	///// </summary>
	//public void C2S_GuideSeqencing(int seqencing){
	//	pt_new_function_open_d803 info = new pt_new_function_open_d803();
	//	info.id = seqencing;
	//	NetMsgMng.SendMsg(info);
	//}
    
    #endregion 

	#region 客服 by 何明军
	/// <summary>
	/// 是否要更新客服数据列表
	/// </summary>
	public bool IsUpdateQuestionList = false;
	/// <summary>
	/// 客服数据更新
	/// </summary>
	public System.Action OnQuestionsReplyUpdate;
	/// <summary>
	/// 客服数据请求更新事件
	/// </summary>
	public System.Action<QuestionsReplyInfo> OnQuestionsReplyRequest;
	Dictionary<int,QuestionsReplyInfo> questionList = new Dictionary<int,QuestionsReplyInfo>();
	/// <summary>
	/// 客服数据
	/// by 何明军
	/// </summary>
	public List<QuestionsReplyInfo> QuestionList{
		get{
			return new List<QuestionsReplyInfo>(questionList.Values);
		}
	}
	/// <summary>
	/// 更新回复
	/// </summary>
	public void UpdateQuestion(int _id,string _replyDes){
		if(!questionList.ContainsKey(_id)){
			Debug.LogError("没有找到ID="+_id+"的客服数据");
		}
		questionList[_id].Update(_replyDes);
	}
	/// <summary>
	/// 更新评价
	/// </summary>
	public void UpdateQuestion(int _id,int _evaluation){
		if(!questionList.ContainsKey(_id)){
			Debug.LogError("没有找到ID="+_id+"的客服数据");
		}
		questionList[_id].Update(_evaluation);
	}
	/// <summary>
	/// 增加客服上报数据
	/// </summary>
	public void AddQuestion(QuestionsReplyInfo _info){
		if(!questionList.ContainsKey(_info.ID)){
			questionList[_info.ID] = _info;
		}else{
			Debug.LogError("ID="+_info.ID+"的客服数据唯一ID重复");
		}
	}
	#endregion
	
	
    #endregion

    #region S2C
    /// <summary>
    /// 玩家详细数据接受（不一定是主玩家，其他管理类也有监听。这里只判断是否主玩家，如果是主玩家，则更新主玩家数据） by吴江
    /// </summary>
    /// <param name="_cmd"></param>
    //protected void S2C_GotMainInfo(Cmd _cmd)
    //{
    //    ServerData.AskMainPlayerData data = new ServerData.AskMainPlayerData(_cmd);
    //    if (mainPlayerInfo == null) return;
    //    if (mainPlayerInfo.ID != data.id) return;
    //    mainPlayerInfo.Update(data);
    //}


    /// <summary>
    /// 根据服务端发来的坐标传送主角
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_OnChangePos(Pt _info)
    {
        pt_scene_tele_b107 pt = _info as pt_scene_tele_b107;
        if (pt == null) return;
        GameCenter.curMainPlayer.ChangePos(pt);
    }
   

    protected void S2C_OnLoginInGame(Pt _info)
    {
        pt_req_load_scene_b104 pt = _info as pt_req_load_scene_b104;
        if (GameCenter.sceneMng != null)
        {
            GameCenter.sceneMng.isChangingScene = true;//切换场景此时不能发送上马协议
        }
        mainPlayerInfo.EnterScene(pt);
        SceneRef sceneRef = ConfigMng.Instance.GetSceneRef((int)pt.scene);
        if (sceneRef != null)
        {
            switch (sceneRef.sort)
            {
                case SceneType.CITY:
                    GameCenter.instance.GoInitCity();//如果是主城
                    break;
                case SceneType.DUNGEONS:
                case SceneType.PEACEFIELD:
                case SceneType.SCUFFLEFIELD:
                case SceneType.CAMPFIELD:
                case SceneType.ENDLESS:
                case SceneType.MULTIPLE:
                case SceneType.ARENA:
                    GameCenter.instance.GoInitDungeon();//如果是地下城
                    break;
            }
        }
    }

    ///// <summary>
    ///// 采集结果 
    ///// </summary>
    //protected void S2C_CollectResult(Pt _info)
    //{
    //    pt_progress_bar_end_d204 msg = _info as pt_progress_bar_end_d204;
    //    if (msg == null) return;
    //    if (msg.oid == GameCenter.curMainPlayer.id)
    //    {
    //        MainPlayerInfo.EndCollect();
    //    }
    //    else if (GameCenter.sceneMng.GetOPCInfo((int)msg.oid) != null)
    //    {
    //        OtherPlayerInfo opc = GameCenter.sceneMng.GetOPCInfo((int)msg.oid);
    //        if (opc == null) return;
    //        opc.EndCollect();
    //    }
    //    else
    //    {
    //        MonsterInfo monster = GameCenter.sceneMng.GetMobInfo((int)msg.oid);
    //        if (monster == null) return;
    //        monster.EndCollect(); ;
    //    }
    //    isWaitTouchSceneItemMsg = false;
    //}
	
	
	///// <summary>
	/////  add 何明军
	///// S2s the c VIP 数据.
	///// </summary>
	///// <param name="info">Info.</param>
	//void S2C_VIPResult(Pt info){
	//	pt_vip_info_d329 msg = info as pt_vip_info_d329;
	//	if (msg == null) return;
	//	vipData = new VIPDataInfo(msg);
	//	if(OnVIPDataUpdate != null){
	//		OnVIPDataUpdate();
	//	}
	//}
	
	void S2C_ReinNum(Pt info){
		pt_update_fly_up_num_d442 msg = info as pt_update_fly_up_num_d442;
		if(msg != null){
			mainPlayerInfo.reinNum = msg.num;
			if(mainPlayerInfo.OnBaseUpdate != null){
				mainPlayerInfo.OnBaseUpdate(ActorBaseTag.CurMP,1,false);
			}
		} 
	}
	
	void S2C_CurServerTime(Pt info){
		pt_sync_time_b105 msg = info as pt_sync_time_b105;
		if(msg != null){
//			double time = (double)(msg.time - (int)Time.time);
			GameCenter.instance.CurServerTime = GameHelper.ToChinaTime(new DateTime(1970,1,1)).AddSeconds((double)msg.time);
			if(activityMng != null)activityMng.InTheMorningUpdateData();
		}
	}
	
	//void S2C_ServerStartGuide(Pt info){
	//	pt_update_guidance_d779 msg = info as pt_update_guidance_d779;
	//	if(msg != null){
	//		OpenGuide(msg.guidance_id,1);
	//	}
	//}
	
	void S2C_ServerStartTime(Pt info){
		pt_update_server_starttime_d786 msg = info as pt_update_server_starttime_d786;
		if(msg != null){
			serverStartTiem = GameHelper.ToChinaTime(new DateTime(1970,1,1)).AddSeconds((double)msg.start_time);
		}
	}

	protected void S2C_StartCameraFocus(Pt _info)
	{
        //Debug.Log("S2C_StartCameraFocus");
		pt_camera_follow_d70a pt = _info as pt_camera_follow_d70a;
		if(pt != null)
		{
			int instanceID = (int)pt.id;
			InteractiveObject obj = GameCenter.curGameStage.GetInterActiveObj(instanceID);
			if(obj != null)
			{
				GameCenter.cameraMng.FocusOn(obj,0);
				GameCenter.curMainPlayer.inputListener.AddLockType(PlayerInputListener.LockType.SCENE_ANIM_PROCESS);
				GameCenter.cameraMng.LockUICamera(true);
				GameCenter.uIMng.ShowMain(false);
                GameCenter.uIMng.ReleaseGUI(GUIType.MARRIAGE);
                GameCenter.uIMng.CloseAllNoAnimation();
			}else
			{
				Debug.Log("未找到目标花车:"+instanceID);
			}
		}
	}

	protected void S2C_EndCameraFocus(Pt _info)
	{
		pt_cancel_camera_follow_d70b pt = _info as pt_cancel_camera_follow_d70b;
		if(pt != null)
		{
			if(GameCenter.curMainPlayer != null)
			{
				GameCenter.cameraMng.FocusOn(GameCenter.curMainPlayer,0);
				GameCenter.curMainPlayer.inputListener.RemoveLockType(PlayerInputListener.LockType.SCENE_ANIM_PROCESS);
				GameCenter.cameraMng.LockUICamera(false);
				GameCenter.uIMng.ShowMain(true);
			}
		}
	}

    /// <summary>
    /// 功能预告领奖返回
    /// </summary>
    public void S2C_UpdateFuncReward(Pt _msg)
    {
        pt_update_function_start_reward_c105 msg = _msg as pt_update_function_start_reward_c105;
        //Debug.Log("pt_update_function_start_reward_c105 ");
        if (msg != null)
        {
            curGetRewardStep = msg.get_reward_id + 1;
        }
        if (UpdateFunctionReward != null)
        {
            UpdateFunctionReward(TaskType.Main);
        }
    }
    public Action OnBaseValueChange;
    /// <summary>
    /// 属性发生改变的协议
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_OnBaseValueChange(Pt _info)
    {
        pt_update_base_d01d pt = _info as pt_update_base_d01d;
        if (pt != null)
        {
            int instanceID = (int)pt.uid; 
            PlayerBaseInfo pInfo = null;
            if (GameCenter.mainPlayerMng.mainPlayerInfo != null && instanceID == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
            {
                pInfo = GameCenter.mainPlayerMng.MainPlayerInfo;
            }
            else
            { 
                if (GameCenter.sceneMng != null) pInfo = GameCenter.sceneMng.GetOPCInfo(instanceID);
            } 
            if (pInfo != null)
            {
                for (int i = 0; i < pt.property_list.Count; i++)
                {
                    pInfo.ChangeValue(pt.property_list[i]);
                }
                for (int i = 0; i < pt.property64_list.Count; i++)
                {
                    pInfo.ChangeValue(pt.property64_list[i]);
                }
            }
            if (OnBaseValueChange != null)
                OnBaseValueChange();

        }



    }
    #endregion

    #region C2S
	/// <summary>
	/// 请求服务端同步时间
	/// </summary>
	public void C2S_ServerTime(){
		pt_now_time_request_d823 msg = new pt_now_time_request_d823();
		NetMsgMng.SendMsg(msg);
	}
	
	/// <summary>
	/// 转生请求（1=转生，2兑换，3高级兑换）
	/// </summary>
	/// <param name="stege">Stege.</param>
	public void C2S_ReinState(uint stege){
		pt_req_usr_fly_up_d438 msg= new pt_req_usr_fly_up_d438();
		msg.state = stege;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 设置阵营模式
	/// </summary>
	public void C2S_SetCampMode(PkMode mode)
	{
		Debug.Log("C2S_SetCampMode:"+mode);
		pt_set_pk_mode_c013 msg = new pt_set_pk_mode_c013();
		msg.mode = (byte)(int)mode;
		NetMsgMng.SendMsg(msg);
	}

	/// <summary>
	/// 转生请求（今日兑换次数）
	/// </summary>
	/// <param name="stege">Stege.</param>
	public void C2S_ReinNum(int stege){
		pt_req_fly_up_info_d441 msg= new pt_req_fly_up_info_d441();
		NetMsgMng.SendMsg(msg);
	}
	///// <summary>
	///// VIP奖励领取 by 何明军
	///// </summary>
	///// <param name="_instanceID"></param>
	//public void C2S_VIPRewarde(int _instanceID)
	//{
	//	pt_req_vip_reward_d328 msg = new pt_req_vip_reward_d328();
	//	msg.vip_lev = _instanceID;
	//	NetMsgMng.SendMsg(msg);
	//}

    /// <summary>
    /// 启动场景物品
    /// </summary>
    /// <param name="_instanceID"></param>
    public void C2S_TouchSceneItem(int _instanceID)
    {
        isWaitTouchSceneItemMsg = true;
        startTouchTime = Time.time;
        pt_scene_item_c019 msg = new pt_scene_item_c019();
        msg.target_id = (uint)_instanceID;
        NetMsgMng.SendMsg(msg);
    }

    //public void C2S_ReqMainPlayerProperty()
    //{
    //    if (mainPlayerInfo == null)
    //    {
    //        GameSys.LogError("主玩家数据为空！无法向服务端索取属性信息");
    //        return;
    //    }
    //    Cmd cmd = new Cmd(MsgHander.PT_ACCEPT_MAININF);
    //    cmd.write_int(mainPlayerInfo.ID);
    //    NetMsgMng.SendCMD(cmd);
    //}

    /// <summary>
    /// 主玩家进入场景的消息推送 by吴江
    /// </summary>
    /// <param name="scene"></param>
    public void C2S_LoginInGame()
    {
        pt_usr_enter_scene_b003 msg = new pt_usr_enter_scene_b003();
        NetMsgMng.SendMsg(msg);

    }


    /// <summary>
    /// 主玩家移动的消息推送
    /// </summary>
    /// <param name="des"></param>
    public void C2S_Move(ObjectType _type, int _instanceID, Vector3 _curPos, Vector3[] _path,bool _isPathMove, int _dir = 0)
    {
        pt_scene_move_c001 msg = new pt_scene_move_c001();
        msg.oid = (uint)_instanceID;
        msg.dir = _dir;
        msg.obj_sort = (uint)_type;
        msg.seq = NetMsgMng.CreateNewUnLockSerializeID();
        msg.is_path_move = _isPathMove ? (byte)1 : (byte)0;
        if (_path != null)
        {
            for (int i = 0; i < _path.Length; i++)
            {
                msg.point_list.Add(Vector2Point(_path[i]));
            }
        }
        else
        {
            msg.point_list.Add(Vector2Point(_curPos));
        }
//		for (int i = 0,max=msg.point_list.Count; i < max; i++) 
//		{
//			Debug.Log("C2S_Move:"+msg.point_list[i].x+","+msg.point_list[i].y+","+msg.point_list[i].z+",count:"+msg.point_list.Count);
//		}
        NetMsgMng.SendMsg(msg);
    }


    public void C2S_Jump()
    {
        pt_req_jump_c01a msg = new pt_req_jump_c01a();
        NetMsgMng.SendMsg(msg);
    }


    /// <summary>
    /// 主玩家传送场景的消息推送
    /// </summary>
    /// <param name="_flypointID"></param>
	public void C2S_Fly(int _flypointID,int x=0,int y=0)
    {
        //Debug.Log("C2S_Fly");
        pt_scene_fly_by_fly_point_c004 msg = new pt_scene_fly_by_fly_point_c004();
        msg.fly_point_id = _flypointID;
		msg.fly_x = x;
		msg.fly_y = y;
        NetMsgMng.SendMsg(msg);
    }

	/// <summary>
	/// 开始播放传送特效
	/// </summary>
	public bool isStartingFlyEffect = false;

    /// <summary>
    /// 主玩家传送指定场景的指定地点 by黄洪兴
    /// </summary>
    public void C2S_Fly_Pint(int _sceneType, int x = 0, int y = 0)
    {
        pt_scene_fly_by_fly_point_c004 msg = new pt_scene_fly_by_fly_point_c004();
        msg.fly_point_id =0;
        msg.fly_scene_type = _sceneType;
        msg.fly_x = x;
        msg.fly_y = y;
        NetMsgMng.SendMsg(msg);
    }
	
	/// <summary>
	/// 主玩家传送指定副本 by 何明军
	/// </summary>
	public void C2STaskFly(int _sceneType)
	{
        //Debug.Log("主玩家传送指定副本  " + _sceneType);
		pt_req_task_fly_d686 msg = new pt_req_task_fly_d686();
		msg.scene = _sceneType;
		NetMsgMng.SendMsg(msg);
	}
    /// <summary>
    /// 请求领取预告奖励
    /// </summary>
    public void C2S_ReqGetFuncReward(int _id)
    {
        //Debug.Log("请求领取奖励  " + _id);
        pt_req_function_start_reward_c104 msg = new pt_req_function_start_reward_c104();
        msg.reward_id = _id;
        NetMsgMng.SendMsg(msg);
    }

    #endregion


    #region 辅助逻辑
    /// <summary>
    /// 获取主玩家的其他依赖项子数据（如背包物品，邮件，任务等）
    /// </summary>
    public void ApplySubData()
    {
        hasApplySubData = true;
		GameCenter.instance.PingListNum.Clear();
        GameCenter.taskMng.C2S_ReqTaskList();
        //GameCenter.myFriendMng.C2S_RequestFriends(MyFriendActionType.AskForFriends);
        GameCenter.skillMng.C2S_SkillReq();
        GameCenter.inventoryMng.C2S_AskForBackPackData();
		GameCenter.inventoryMng.C2S_AskForEquipData();
        GameCenter.titleMng.C2S_AskTitle(); // add by 贺丰
        GameCenter.rebornMng.C2S_GetRebornTimes();
        GameCenter.magicWeaponMng.C2S_RequestGetMagic();//法宝
        GameCenter.mercenaryMng.C2S_ReqMercenaryDataList();//请求宠物列表  
        GameCenter.newMountMng.C2S_GetMountList(MountType.SKINLIST);//请求幻化列表
        GameCenter.newMountMng.C2S_GetMountList(MountType.MOUNTLIST);//请求坐骑列表
        GameCenter.friendsMng.C2S_ReqFriendsList();
        GameCenter.friendsMng.C2S_ReqFriendsList(4);//请求仇人链表
        //GameCenter.rankRewardMng.C2S_ReqGetLevRewardInfo(RewardType.LEVREWARD);
		GameCenter.endLessTrialsMng.C2S_EndList();
        //GameCenter.endLessTrialsMng.C2S_RepArenaServer();
        GameCenter.arenaMng.C2S_RepArenaServer();
        //GameCenter.endLessTrialsMng.C2S_ReqCopyItemList();
        GameCenter.duplicateMng.C2S_ReqCopyItemList();
        GameCenter.practiceMng.C2S_ReinState(practiceType.PRACRICE);
		mailBoxMng.C2S_MailList();
        GameCenter.wingMng.C2S_RequestGetWing();//翅膀
		GameCenter.guildMng.C2S_GuildInfo();
        GameCenter.lovePackageMng.C2S_ReqGetLoveInfo();//请求充值数和充值阶段 
        GameCenter.twoChargeMng.C2S_ReqGetTwoChargeInfo();//二冲请求充值数和充值阶段
        GameCenter.weekCardMng.C2S_ReqGetWeekInfo();
        GameCenter.weekCardMng.C2S_ReqGetLoginBonusInfo();
        GameCenter.firstChargeBonusMng.C2S_ReqGetFirstChargeInfo(2003);//请求首冲数据
        GameCenter.sevenDayMng.ReqGetSevenday();//请求七天数据
		GameCenter.dailyMustDoMng.C2S_ReqMustDoData();//请求每日必做数据
        GameCenter.rankRewardMng.C2S_ReqGetLevRewardInfo(RewardType.LEVREWARD);//请求等级奖励 
        GameCenter.practiceMng.C2S_ReinState(practiceType.SOARING);
        GameCenter.treasureHouseMng.C2S_ReqGetHouse();//藏宝阁临时仓库
        GameCenter.treasureHouseMng.C2S_ReqTreasureRecord();
		GameCenter.bossChallengeMng.C2S_ReqChallengeBossList();//请求BOSS列表
        GameCenter.newMountMng.C2S_ReqMountEquipList();//请求骑装数据
		GameCenter.rechargeMng.C2S_ReqTestChargeData();//请求充值返利数据
        GameCenter.royalTreasureMng.C2S_ReqRoyalBoxList();//请求宝箱数据
        GameCenter.guildMng.C2S_GuildDonateTimes();//请求仙盟捐献次数
        GameCenter.guildMng.C2S_ReqLivelyData();//请求仙盟活跃数据
        GameCenter.miracleMng.C2S_ReqRoyalMiracleList();//请求奇缘数据
        GameCenter.taskMng.C2S_GetRingTaskPrograss();
        GameCenter.activityMng.C2S_ReqHangUpCoppyData(1);//请求挂机副本数据,界面红点显示
     }


    public point3 Vector2Point(Vector3 _v3)
    {
        point3 p = new point3();
        p.x = _v3.x;
        p.y = _v3.y;
        p.z = _v3.z;
        return p;
    }
    #endregion
}
public enum PkMode
{
	NONE,
	/// <summary>
	/// 和平模式
	/// </summary>
	PKMODE_PEASE = 1,
	/// <summary>
	/// 全体模式
	/// </summary>
	PKMODE_ALL = 2,
	/// <summary>
	/// 队伍模式
	/// </summary>
	PKMODE_TEAM = 3,
	/// <summary>
	/// 仙盟模式
	/// </summary>
	PKMODE_GUILD = 4,
	/// <summary>
	/// 善恶模式
	/// </summary>
	PKMODE_JUSTICE = 5,
    /// <summary>
    /// 阵营模式
    /// </summary>
    PKMODE_CAMP = 6,
}
