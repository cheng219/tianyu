//===============================
//作者：邓成
//日期：2016/5/9
//用途：活动管理类
//===============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;

public class ActivityMng{

	#region 事件


    /// <summary>
    /// 获得其他仙盟篝火信息事件
    /// </summary>
    public Action OnGoTOtherBonfireInfo;

    public Action OnGoTGotApplyInfo;
    /// <summary>
    /// 武道会排名更新事件
    /// </summary>
    public Action BudokaiRankUpdate;
    /// <summary>
    /// 武道战斗日志更新事件
    /// </summary>
    public Action BudokaiLogUpdate;
    /// <summary>
    /// 武道会匹配到对手的事件
    /// </summary>
    public Action OnGotBudokaiOpponentInfo;
	/// <summary>
	/// 获得神圣水晶详细信息时间
	/// </summary>
	public Action OnGotHolyInfoEvent;
	/// <summary>
	/// 获得升级or降级提示事件
	/// </summary>
	public Action OnGotUpDownStateEvent;
	/// <summary>
	/// 获取仙域守护活动信息(开启状态)
	/// </summary>
	public Action OnGotProtectActivityEvent;
	/// <summary>
	/// 获取仙域守护排行榜
	/// </summary>
	public Action OnGotProtectActivityRankEvent;
	/// <summary>
	/// 获取攻城战信息事件
	/// </summary>
	public Action OnGotGuildSiegeInfoEvent;
	/// <summary>
	/// 攻城战申请攻城列表变化事件
	/// </summary>
	public Action OnGotGuildSiegeApplyListEvent;
	/// <summary>
	/// 攻城战城内商店列表更新事件
	/// </summary>
	public Action OnGotGuildCityStoreListEvent;
	/// <summary>
	/// 仙盟运镖排行更新事件
	/// </summary>
	public Action OnGotGuildDartRankListEvent;
    /// <summary>
    /// 活动大厅活动数据变化事件
    /// </summary>
    public System.Action OnActivityDataInfo;
    /// <summary>
    /// 地宫魔王被消灭事件
    /// </summary>
    public System.Action OnBossDestory;
    /// <summary>
    /// 地宫魔王被消灭的后台时间
    /// </summary>
    public int BossBekilled;
    #endregion

    #region 数据
    /// <summary>
    /// 神圣水晶数据
    /// </summary>
    public pt_reply_holy_crystal_info_d611 CurHolyCrystalInfo = null;
   /// <summary>
   /// 仙域守护数据
   /// </summary>
	public List<guild_guard_rank> ProtectActivityRankList = new List<guild_guard_rank>();
	/// <summary>
	/// 仙域守护是否开启
	/// </summary>
	public bool isProtectActivityOpen = false;
	/// <summary>
	/// 仙域守护开启难度
	/// </summary>
	public int ProtectActivityOpenValue = 0;
	/// <summary>
	/// 仙盟运镖排行
	/// </summary>
	public List<rank_info_base> GuildDartRankList = new List<rank_info_base>();

	/// <summary>
	/// 攻城战盟主信息
	/// </summary>
	public PlayerBaseInfo GuildSiegeCastellan = null;
	public bool HadRepplySiege = false;
	public List<req_apply_list> GuildSiegeApplyList = new List<req_apply_list>();
	/// <summary>
	/// 攻城战城内商店数据
	/// </summary>
	public List<astrict_item_list> GuildCityShopList = new List<astrict_item_list>();
    #region 武道会数据

    public bool isFirstTime = true;
    public bool IsWin=false;
    public string OpponentName;
    public int RemainTime;
    public int GetTime;
    public List<budo_log_list> LogList=new List<budo_log_list> ();
    public Dictionary<int, List<BudokaiRankInfo>> BudokaiRankInfoDic=new Dictionary<int,List<BudokaiRankInfo>>();
    public BudokaiRankInfo PlayerRankInfo;
    public int CurRankPage
    {
        get
        {
            return curRankPage;
        }
        set
        {
            if (value < 1)
            {
                curRankPage = 1;
            }
            else if (value > 5)
            {
				curRankPage = 5;
            }
            else
            {
                curRankPage = value;
            }
        }
    }
    public bool NeverHint=false;
    public bool Apply=false;
    public bool IsOpen=false;

	List<int> shour=new List<int>();
	List<int> smin=new List<int>();
	List<int> ehour=new List<int>();
	List<int> emin=new List<int>();
    int curRankPage=1;
    #endregion

    #region 仙盟篝火数据
    /// <summary>
    /// 其他仙盟篝火数据
    /// </summary>
    public List<other_bonfire_list> OtherBonfireList = new List<other_bonfire_list>();


   
    #endregion

    #region 活动结算
    /// <summary>
    /// 活动结算的奖励
    /// </summary>
    public List<reward_list> rewardList = new List<reward_list>();
    /// <summary>
    /// 活动结算状态
    /// </summary>
    public int activeState = 0;
    /// <summary>
    /// 是否是攻城战
    /// </summary>
    public bool isGuildStormCity = false;
    /// <summary>
    /// 攻城战获胜名字
    /// </summary>
    public string vctorName = string.Empty;
    #endregion

    #region 夺宝奇兵
    /// <summary>
    /// 奖励宝箱ID
    /// </summary>
    public int rewardId = 0;
    /// <summary>
    /// 采集宝珠结束事件
    /// </summary>
    public System.Action OnStopCollect;
    /// <summary>
    /// 宝珠数据更新事件
    /// </summary>
    public System.Action OnUpdateJewelry;
    /// <summary>
    /// 活动结算事件
    /// </summary>
    public System.Action<List<reward_list>> OnGetReward;
    /// <summary>
    /// 采集到的宝珠数据
    /// </summary>
    public List<jewelry_list> jewelryList = new List<jewelry_list>();
    /// <summary>
    /// 拾取冷却时间
    /// </summary>
    public float coldGetTime = 0;
    #endregion
    #region 活动大厅功能数据
    /// <summary>
    /// 需要提示的数据
    /// </summary>
    public Dictionary<int,ActivityListRef> ActivityOnGoingList = new Dictionary<int ,ActivityListRef>();
    /// <summary>
    /// 活动正在进行中，进行活动提示
    /// </summary>
    public System.Action OnActivityOnGoing;
    /// <summary>
    /// 活动结束事件
    /// </summary>
    public System.Action OnActivityOver;
    /// <summary>
    /// 已经提示过的数据
    /// </summary>
    public Dictionary<int, ActivityListRef> haveTipDic = new Dictionary<int, ActivityListRef>();
    /// <summary>
    /// 提前5分钟提示
    /// </summary>
    public const int ShowTime = 300;
    public Dictionary<int, ActivityDataInfo> activityDic = new Dictionary<int, ActivityDataInfo>();
    public ActivityDataInfo GetActivityDataInfo(int id)
    {
        if (activityDic.ContainsKey(id))
        {
            return activityDic[id];
        }
        return null;
    }
    /// <summary>
    /// return {1=未开，2=开启，3=已结束}
    /// </summary>
    public ActivityState GetActivityState(ActivityType type)
    {
        if (activityDic.ContainsKey((int)type))
        {
            return activityDic[(int)type].State;
        }
        return ActivityState.NOTATTHE;
    }
    /// <summary>
    /// 活动还有多久结束
    /// </summary>
    public int GetActivityTime(ActivityType type)
    {
        if (activityDic.ContainsKey((int)type))
        {
            if (activityDic[(int)type].State == ActivityState.ONGOING)
            {
                int atime = activityDic[(int)type].UpDateTime;
                return atime > 0 ? atime : 0;
            }
        }
        return 0;
    }
    /// <summary>
    /// 活动还有多久开始,活动已结束时返回0
    /// </summary>
    public int GetActivityStartTime(int id)
    {
        if (activityDic.ContainsKey(id))
        {
            if (activityDic[id].State == ActivityState.NOTATTHE)
            {
                return activityDic[id].UpDateTime;
            }
        }
        return 0;
    }
    /// <summary>
    /// 是否是直接跳转过去(从别的界面跳转的，直接选中某个活动的将该活动排在第一个)
    /// </summary>
    //public bool isGoToSelect = false;
    public ActivityType CurSeleteType = ActivityType.NONE;
    /// <summary>
    /// 打开时选择某个活动
    /// </summary>
    public void OpenStartSeleteActivity(ActivityType type)
    {
        //isGoToSelect = true;
        CurSeleteType = type;
        GameCenter.uIMng.SwitchToUI(GUIType.ATIVITY);
        //ChooseActivity(type);
    }

    enum ButtonType
    {
        FengShen = 1,
        ToPoint = 2,
        GUI = 3,
        subGUI,
        fly,
        ToNpc,
    }
    /// <summary>
    /// 按钮功能
    /// </summary>
    public void GoActivityButtonFunc(ActivityButtonRef refdata, int id)
    {
        if (refdata == null) { return; }
        if (!activityDic.ContainsKey(id)) { return; }
        if (!activityDic[id].ActivityLev)
        {
            GameCenter.messageMng.AddClientMsg(13);
            return;
        }

        ActivityType type = (ActivityType)id;
        //=============fix 完全没必要每次判断都进行一次强制转换，可以先声明一个变量，强制转换一次，然后拿该变量去判断
        if (type == ActivityType.FAIRYAUBONFIRE || type == ActivityType.FAIRYAUSHIPMENTDART
            || type == ActivityType.FAIRYAUSIEGE || type == ActivityType.FAIRYDOMAINTOPROTECT)
        {
            if (!GameCenter.mainPlayerMng.MainPlayerInfo.HavaGuild)
            {
                GameCenter.messageMng.AddClientMsg(235);
                return;
            }
        }

        if (type == ActivityType.BATTLEFAGIHT) 
        {
            GameCenter.battleFightMng.C2S_ReqFlyBattleFeild();
            return;
        }

        if (type == ActivityType.DAILYTRANSPORTDART)
        {
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
            GameCenter.activityMng.C2S_ReqDartPos(DartType.DailyDart);
            return;
        }

        if (type == ActivityType.FAIRYAUSHIPMENTDART)
        {
            GameCenter.activityMng.C2S_ReqDartPos(DartType.GuildDart);
            return;
        }
        if (type == ActivityType.FAIRYAUBONFIRE)
        {
            GameCenter.activityMng.C2S_FlyMyGuildFire();
            return;
        }
        if (type == ActivityType.UNDERBOSS)
        {
            BossChallengeWnd.OpenAndGoWndByType(BossChallengeWnd.ToggleType.UnderBoss);
            return;
        }
        if (activityDic[id].State != ActivityState.ONGOING)
        {
            GameCenter.messageMng.AddClientMsg(173);
            return;
        }
        if (type == ActivityType.RAIDERARK)
        {
            //Debug.Log("跳转夺宝奇兵界面！！！");
            GameCenter.activityMng.C2S_FlyRaiderArk();
            return;
        }

        //=============fix 没有使用枚举，而是使用魔数========
        if (refdata.type == (int)ButtonType.GUI)
        {
            GameCenter.uIMng.SwitchToUI((GUIType)Enum.Parse(typeof(GUIType), refdata.pageId));
        }
        else if (refdata.type == (int)ButtonType.FengShen)
        {
            GameCenter.activityMng.C2S_FlyFengShen(id);
        }
        else if (refdata.type == (int)ButtonType.ToPoint)
        {//寻路点
            GameCenter.taskMng.TraceToScene(refdata.mapId, new Vector3(refdata.mapXY[0], 0, refdata.mapXY[1]));
            GameCenter.curMainPlayer.GoTraceSearchTreasure();
        }
        else if (refdata.type == (int)ButtonType.subGUI)
        {
            GameCenter.uIMng.SwitchToSubUI((SubGUIType)Enum.Parse(typeof(SubGUIType), refdata.pageId));
        }
        else if (refdata.type == (int)ButtonType.fly)
        {//飞副本，
            GameCenter.mainPlayerMng.C2S_Fly_Pint(refdata.mapId, refdata.mapXY[0], refdata.mapXY[1]);
        }
        else if (refdata.type == (int)ButtonType.ToNpc)
        {//寻路NPC
            GameCenter.taskMng.PublicTraceToNpc(refdata.mapId);
            GameCenter.curMainPlayer.GoTraceSearchTreasure();
        }
    }
    /// <summary>
    /// 每天凌晨更新活动数据
    /// </summary>
    public void InTheMorningUpdateData()
    {
        foreach (ActivityDataInfo data in activityDic.Values)
        {
            data.StateUpdateTime();
        }
        if (OnActivityDataInfo != null) OnActivityDataInfo();
    }
    /// <summary>
    /// 活动排序 进行中--未开始--已结束
    /// </summary>
    public int SortActivity(ActivityDataInfo info1, ActivityDataInfo info2)
    { 
        int state1 = 0;
        int state2 = 0;
        switch (info1.State)
        {
            case ActivityState.ONGOING:
                state1 = 2;
                break;
            case ActivityState.NOTATTHE:
                state1 = 3;
                break;
            case ActivityState.HASENDED:
                state1 = 4;
                break;
        }
        if (CurSeleteType != ActivityType.NONE)//将选中的活动放在最前面
        {
            if (CurSeleteType == info1.ID)
            {
                state1 = 1;
            }
        }
        switch (info2.State)
        {
            case ActivityState.ONGOING:
                state2 = 2;
                break;
            case ActivityState.NOTATTHE:
                state2 = 3;
                break;
            case ActivityState.HASENDED:
                state2 = 4;
                break;
        }
        if (CurSeleteType != ActivityType.NONE)
        {
            if (CurSeleteType == info2.ID)
            {
                state2 = 1;
            }
        }
        if (state1 > state2)//先按活动状态排序(进行中-未开始-已结束)
            return 1;
        if (state1 < state2)
            return -1;
        if (info1.SortNum > info2.SortNum)//状态相同按ListNum排序
            return 1;
        if (info1.SortNum < info2.SortNum)
            return -1;
        return 0;
    }

    #endregion
    /// <summary>
    /// 地宫boss是否被击杀(被击杀关闭活动)
    /// </summary>
    /// <returns></returns>
    bool underBossBeKilled;
   public bool UnderBoss
   {
        get
        {
            return underBossBeKilled;
        }
   }
    #endregion


    public static ActivityMng CreateNew()
	{
		if(GameCenter.activityMng != null)
		{
			GameCenter.activityMng.UnRegist();
			GameCenter.activityMng.Init();
			return GameCenter.activityMng;
		}else
		{
			GameCenter.activityMng = new ActivityMng();
			GameCenter.activityMng.Init();
			return GameCenter.activityMng;
		}
	}
	void Init()
	{
        MsgHander.Regist(0xD741, S2C_GotOtherBonfireInfo);
        MsgHander.Regist(0xD736, S2C_GotBudokaiResult);
		MsgHander.Regist(0xD611,S2C_GotHolyInfo);
		MsgHander.Regist(0xD614,S2C_GotDartPos);
		MsgHander.Regist(0xD615,S2C_StartDartResult);
		MsgHander.Regist(0xD616,S2C_EndDartResult);
		MsgHander.Regist(0xD723,S2C_GotUpDownState);
        MsgHander.Regist(0xD717, S2C_GotBudokaiOpponentInfo);
        MsgHander.Regist(0xD720, S2C_GotBudokaiLogInfo);
       // MsgHander.Regist(0xD601, S2C_GotBudokaiRankInfo);
        MsgHander.Regist(0xD721, S2C_GotApplyInfo);
		MsgHander.Regist(0xD726, S2C_GotProtectActivityInfo);
		MsgHander.Regist(0xD727, S2C_GotProtectActivityRank);
		MsgHander.Regist(0xD730,S2C_GotGuildSiegeInfo);
		MsgHander.Regist(0xD731,S2C_GotGuildSiegeApplyList);
		MsgHander.Regist(0xD733,S2C_GotGuildSiegeItemList);
        MsgHander.Regist(0xD728, S2C_ActivityBalance);
        MsgHander.Regist(0xD778, S2C_ActivityStormCity);
        MsgHander.Regist(0xD794, S2C_GetJewelryList);
        MsgHander.Regist(0xD796, S2C_GetJewerlyFinish);
        MsgHander.Regist(0xD715, S2C_ActivityDataInfo);
        MsgHander.Regist(0xD756, S2C_FairyDomainToprotect);
        MsgHander.Regist(0xC136,S2C_GetHungUpCoppyResult);
        MsgHander.Regist(0xC135, S2C_GetHungUpWndResult);
        if (ConfigMng.Instance.GetActivityListRef(5) != null)
        {
            for (int i = 0; i < ConfigMng.Instance.GetActivityListRef(5).looptime.Count; i++)
            {
                shour.Add(ConfigMng.Instance.GetActivityListRef(5).looptime[i].start[0]);
                smin.Add(ConfigMng.Instance.GetActivityListRef(5).looptime[i].start[1]);
                ehour.Add(ConfigMng.Instance.GetActivityListRef(5).looptime[i].end[0]);
                emin.Add(ConfigMng.Instance.GetActivityListRef(5).looptime[i].end[1]);
            }
        }

        isFirstTime = PlayerPrefs.HasKey("BUDOKAI_TIME") ? PlayerPrefs.GetInt("BUDOKAI_TIME")!=DateTime.Now.DayOfYear : true;
	}
	void UnRegist()
	{
        MsgHander.UnRegist(0xD741, S2C_GotOtherBonfireInfo);
        MsgHander.UnRegist(0xD736, S2C_GotBudokaiResult);
		MsgHander.UnRegist(0xD611,S2C_GotHolyInfo);
		MsgHander.UnRegist(0xD614,S2C_GotDartPos);
		MsgHander.UnRegist(0xD615,S2C_StartDartResult);
		MsgHander.UnRegist(0xD616,S2C_EndDartResult);
		MsgHander.UnRegist(0xD723,S2C_GotUpDownState);
        MsgHander.UnRegist(0xD717, S2C_GotBudokaiOpponentInfo);
        MsgHander.UnRegist(0xD720, S2C_GotBudokaiLogInfo);
       // MsgHander.UnRegist(0xD601, S2C_GotBudokaiRankInfo);
        MsgHander.UnRegist(0xD721, S2C_GotApplyInfo);
		MsgHander.UnRegist(0xD726, S2C_GotProtectActivityInfo);
		MsgHander.UnRegist(0xD727, S2C_GotProtectActivityRank);
		MsgHander.UnRegist(0xD730,S2C_GotGuildSiegeInfo);
		MsgHander.UnRegist(0xD731,S2C_GotGuildSiegeApplyList);
		MsgHander.UnRegist(0xD733,S2C_GotGuildSiegeItemList);
        MsgHander.UnRegist(0xD728, S2C_ActivityBalance);
        MsgHander.UnRegist(0xD778, S2C_ActivityStormCity);
        MsgHander.UnRegist(0xD794, S2C_GetJewelryList);
        MsgHander.UnRegist(0xD796, S2C_GetJewerlyFinish);
        MsgHander.UnRegist(0xD715, S2C_ActivityDataInfo);
        MsgHander.UnRegist(0xD756, S2C_FairyDomainToprotect);
        MsgHander.UnRegist(0xC136, S2C_GetHungUpCoppyResult);
        MsgHander.UnRegist(0xC135, S2C_GetHungUpWndResult);
        coldGetTime = 0;
        rewardId = 0;
        activityDic.Clear();
        isAutoButMonsterNum = false;
        HangUpCoppyExpCount = 0;
        hangUpCoppyRemainMonsterNum = 0;
        HangUpRemainBuyTimes = 0;
    }
    void Update()
    {
        if (Time.frameCount % 100 == 0)
        {
           for (int i = 0; i < shour.Count; i++)
			{
                if (System.DateTime.Now.Hour == shour[i] || (System.DateTime.Now.Hour > shour[i]&&System.DateTime.Now.Hour<ehour[i]))
               {
                   if (System.DateTime.Now.Minute >= smin[i])
                       IsOpen = true;
               }
                if (System.DateTime.Now.Hour >= ehour[i])
               {
                   if (System.DateTime.Now.Minute >= emin[i])
                   {
                       IsOpen = false;
                   }
               }
			 
			}
           if (IsOpen)
               Apply = false;  
        }

    }

	#region 神圣水晶 by邓成
	/// <summary>
	/// 请求神圣水晶活动信息
	/// </summary>
	public void C2S_ReqHolyInfo()
	{
		//Debug.Log("C2S_ReqHolyInfo");
		pt_req_holy_crystal_info_d610 msg = new pt_req_holy_crystal_info_d610();
		NetMsgMng.SendMsg(msg);
	}

	protected void S2C_GotHolyInfo(Pt _info)
	{
		//Debug.Log("S2C_GotHolyInfo");
		pt_reply_holy_crystal_info_d611 pt = _info as pt_reply_holy_crystal_info_d611;
		if(pt != null)
		{
			CurHolyCrystalInfo = pt;
			if(OnGotHolyInfoEvent != null)
				OnGotHolyInfoEvent();
		}
	}
	#endregion

	#region 运镖 by邓成
	/// <summary>
	/// 开始运镖,type青铜1、白银2、黄金3  
	/// </summary>
	public void C2S_StartDart(int type,DartType activityType)
	{
		//Debug.Log("C2S_StartDart type:"+type+",activityType:"+activityType);
		pt_req_start_cart_escort_d612 msg = new pt_req_start_cart_escort_d612();
		msg.cart_type = (byte)type;
		msg.type = (byte)(int)activityType;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 请求镖车位置
	/// </summary>
	public void C2S_ReqDartPos(DartType type)
	{
		//Debug.Log("C2S_ReqDartPos activityType:"+type);
		pt_req_cart_pos_d613 msg = new pt_req_cart_pos_d613();
		msg.type = (byte)(int)type;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 请求运镖排行榜
	/// </summary>
	public void C2S_ReqGuildDartRank()
	{
		//Debug.Log("C2S_ReqGuildDartRank");
		pt_ranklist_d600 msg = new pt_ranklist_d600();
		msg.type = 14;
		msg.page = 1;
		NetMsgMng.SendMsg(msg);
	}


	protected void S2C_GotDartPos(Pt _info)
	{
		//Debug.Log("S2C_GotDartPos");
		pt_reply_cart_pos_d614 pt = _info as pt_reply_cart_pos_d614;
		if(pt != null)
		{
			//Debug.Log("pos scene:"+pt.scene+",x:"+pt.x+",z:"+pt.z);
			if(!GameCenter.curMainPlayer.GoAutoDart())
			{
				GameCenter.curMainPlayer.StopForNextMove();
				GameCenter.curMainPlayer.GoTraceTarget((int)pt.scene,(int)pt.x,(int)pt.z);
			}
		}
	}
	/// <summary>
	/// 开始运镖之后的反馈
	/// </summary>
	protected void S2C_StartDartResult(Pt _info)
	{
		//Debug.Log("S2C_StartDartResult");
		pt_reply_start_cart_escort_d615 pt = _info as pt_reply_start_cart_escort_d615;
		if(pt != null)
		{
			GameCenter.uIMng.SwitchToUI(GUIType.NONE);//用SwitchToUI解决关闭界面,不显示主界面的bug
		}
	}

	protected void S2C_EndDartResult(Pt _info)
	{
		//Debug.Log("S2C_EndDartResult");
		pt_cart_escort_succ_d616 pt = _info as pt_cart_escort_succ_d616;
		if(pt != null)
		{
			MessageST mst = new MessageST();
			mst.messID = 200;
			mst.delYes = (x)=>
			{
			//	GameCenter.mainPlayerMng.C2S_Fly_Pint(100012,64,228);
				Command_FlyTo flyTo = new Command_FlyTo();
				flyTo.targetScene = 100012;
				flyTo.targetPos = ActorMoveFSM.LineCast(new Vector3(61,0,212),false);
				flyTo.targetID = 500026;
				GameCenter.curMainPlayer.commandMng.PushCommand(flyTo);
			};
			GameCenter.messageMng.AddClientMsg(mst);
		}
	}
	#endregion

    #region  武道会 by黄洪兴

    /// <summary>
    /// 请求武道会信息或者排队或者匹配状态
    /// </summary>
    /// <param name="_type"></param>
    public void C2S_AskBudokai(int _type)
    {
        //Debug.Log("C2S_AskBudokai");
        pt_req_budo_page_info_d719 msg = new pt_req_budo_page_info_d719();
        msg.type = _type;
        NetMsgMng.SendMsg(msg);
    }

    /// <summary>
    /// 请求传出副本
    /// </summary>
    /// <param name="_type"></param>
    public void C2S_AskOutBudokai()
    {
       // Debug.Log("C2S_AskOutBudokai");
        pt_req_copy_out_d471 msg = new pt_req_copy_out_d471();
        NetMsgMng.SendMsg(msg);
    }




    /// <summary>
    /// 选择开始战斗的方式
    /// </summary>
    /// <param name="_type"></param>
    public void C2S_EnterBudokai(int _type)
    {
        //Debug.Log("C2S_EnterBudokai");
        pt_req_budo_match_state_d718 msg = new pt_req_budo_match_state_d718();
        msg.state = (uint)_type;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求武道会排行信息
    /// </summary>
    /// <param name="_type"></param>
    public void C2S_AskBudokaiRank(int _page)
    {
        //Debug.Log("C2S_AskBudokaiRank");
        pt_ranklist_d600 msg = new pt_ranklist_d600();
        for (int i = 1; i < _page+1; i++)
        {
            msg.type = 13;
            msg.page = (byte)i;
            NetMsgMng.SendMsg(msg); 
        }
    }


    /// <summary>
    /// 获得匹配到的对手信息
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_GotBudokaiOpponentInfo(Pt _info)
    {
        //Debug.Log("S2C_GotBudokaiOpponentInfo");
        pt_update_budo_match_info_d717 pt = _info as pt_update_budo_match_info_d717;
        if (pt != null)
        {
            OpponentName = pt.name;
            RemainTime = (int)pt.time;
            GetTime = (int)Time.time;
            GameCenter.uIMng.SwitchToUI(GUIType.BUDOKAIMATCHING);
            if (OnGotBudokaiOpponentInfo != null)
            OnGotBudokaiOpponentInfo();
        }
    }

    /// <summary>
    /// 获得日志信息
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_GotBudokaiLogInfo(Pt _info)
    {
        //Debug.Log("S2C_GotBudokaiLogInfo");
        pt_update_budo_log_list_d720 pt = _info as pt_update_budo_log_list_d720;
        if (pt != null)
        {
            LogList.Clear();
            LogList = pt.budo_log_list;
            if (BudokaiLogUpdate != null)
                BudokaiLogUpdate();
           // Debug.Log("收到日志长度为" + pt.budo_log_list.Count);
        }
    }


    /// <summary>
    /// 获得排行榜信息
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_GotBudokaiRankInfo(pt_ranklist_d601 pt)
    {
        //pt_ranklist_d601 pt = _info as pt_ranklist_d601;
        //Debug.Log("S2C_GotBudokaiRankInfo,type为" + pt.type);
        if (pt != null&&pt.type==(byte)13)
        {
            if (BudokaiRankInfoDic.ContainsKey(pt.page))
            {
                BudokaiRankInfoDic[pt.page].Clear();
            }
            List<BudokaiRankInfo> list = new List<BudokaiRankInfo>();
            for (int i = 0; i < pt.ranklist.Count; i++)
            {
                BudokaiRankInfo info = new BudokaiRankInfo((pt.page - 1) * 20 + i+1, pt.ranklist[i].name, pt.ranklist[i].value1, pt.ranklist[i].value2);
                list.Add(info);
            }
            BudokaiRankInfoDic[pt.page]=list;
            //Debug.Log("收到排行榜的长度" + pt.ranklist.Count);
            PlayerRankInfo = new BudokaiRankInfo(pt.rank, GameCenter.mainPlayerMng.MainPlayerInfo.Name, pt.value1, pt.value2);
            if (BudokaiRankUpdate != null)
                BudokaiRankUpdate();
        }

    }

	public void GotGuildDartRankList(pt_ranklist_d601 pt)
	{
		if(pt != null && pt.type == (byte)14)
		{
			//Debug.Log("GuildDartRankList count:"+GuildDartRankList.Count);
			GuildDartRankList.Clear();
			GuildDartRankList = pt.ranklist;
			if(OnGotGuildDartRankListEvent != null)
				OnGotGuildDartRankListEvent();
		}
	}
    /// <summary>
    /// 获得报名状态
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_GotApplyInfo(Pt _info)
    {
       // Debug.Log("S2C_GotApplyInfo");
        pt_update_budo_apply_d721 pt = _info as pt_update_budo_apply_d721;
        if (pt != null)
        {
        if ( pt.state == 2)
        {
            Apply = true;
         //   Debug.Log("获得报名状态  报名成功");
        }
        else
        {
            Apply = false;
          //  Debug.Log("获得报名状态 当前没有报名");
        }
        if (OnGoTGotApplyInfo != null)
            OnGoTGotApplyInfo();
        }

    }




    /// <summary>
    /// 副本结算
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_GotBudokaiResult(Pt _info)
    {
        //Debug.Log("S2C_GotBudokaiResult");
        pt_budo_win_d736 pt = _info as pt_budo_win_d736;
        if (pt != null)
        {
            IsWin = pt.val == 1;
            
        }
        GameCenter.uIMng.SwitchToUI(GUIType.BUDOKAIEND);
    }


    public void OnOpenBudokaiWnd()
    {
        GameCenter.newRankingMng.OnGetRankingInfo += S2C_GotBudokaiRankInfo;
    }
    public void OnCloseBudokaiWnd()
    {
        GameCenter.newRankingMng.OnGetRankingInfo -= S2C_GotBudokaiRankInfo;
    }



    #endregion


	#region 封神战 by邓成
	public bool isUp = false;
	public int countDownTime = 0;
	/// <summary>
	/// 传送封神地
	/// </summary>
	public void C2S_FlyFengShen(int id)
	{
		//Debug.Log("C2S_FlyFengShen");
		pt_req_fly_activity_copy_d722 msg = new pt_req_fly_activity_copy_d722();
		msg.actid = id;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 升级降级提示
	/// </summary>
	/// <param name="_info">Info.</param>
	protected void S2C_GotUpDownState(Pt _info)
	{
		//Debug.Log("S2C_GotUpDownState");
		pt_update_fengshen_up_down_d723 pt = _info as pt_update_fengshen_up_down_d723;
		if(pt != null)
		{
			//Debug.Log("S2C_GotUpDownState:"+pt.state +",countDown:" +pt.count_down);
			isUp = (pt.state == 1);
			countDownTime = pt.count_down;
			if(OnGotUpDownStateEvent != null)
				OnGotUpDownStateEvent();
		}
	}
	#endregion

    #region 挂机副本
    protected int hangUpCoppyRemainMonsterNum = 0;
    public int HangUpCoppyRemainMonsterNum
    {
        get
        {
            return hangUpCoppyRemainMonsterNum;
        }
        set
        {
            if (hangUpCoppyRemainMonsterNum != value)
            {
                hangUpCoppyRemainMonsterNum = value;
                if (hangUpCoppyRemainMonsterNum == 0 && IsAutoButMonsterNum)//剩余怪物0,自动购买
                {
                    C2S_ReqHangUpCoppyData(2);
                }
            }
        }
    }
    /// <summary>
    /// 挂机副本获得的经验值
    /// </summary>
    public int HangUpCoppyExpCount = 0;
    protected bool isAutoButMonsterNum = false;
    /// <summary>
    /// 是否自动购买怪物
    /// </summary>
    public bool IsAutoButMonsterNum
    {
        get
        {
            return isAutoButMonsterNum;
        }
        set
        {
            if (isAutoButMonsterNum != value)
            {
                isAutoButMonsterNum = value;
                if (isAutoButMonsterNum && hangUpCoppyRemainMonsterNum == 0)//打开自动购买时,数量为0则发送购买协议
                {
                    C2S_ReqHangUpCoppyData(2);
                }
            }
        }
    }
    /// <summary>
    /// 剩余购买次数
    /// </summary>
    public int HangUpRemainBuyTimes = 0;

    public System.Action OnHanguUpCoppyDataUpdateEvent;


    public void C2S_FlyHangUpCoppy(int _sceneId)
    {
        //Debug.Log("C2S_ReqHangUpCoppyData _sceneId:" + _sceneId);
        pt_req_fly_on_hook_copy_c140 msg = new pt_req_fly_on_hook_copy_c140();
        msg.fly_scene = (uint)_sceneId;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// _type: 1请求界面数据 2购买挂机副本的怪物
    /// </summary>
    public void C2S_ReqHangUpCoppyData(int _type)
    {
        //Debug.Log("C2S_ReqHangUpCoppyData type:" + _type);
        pt_req_on_hook_c134 msg = new pt_req_on_hook_c134();
        msg.state = (uint)_type;
        NetMsgMng.SendMsg(msg);
    }

    protected void S2C_GetHungUpCoppyResult(Pt _pt)
    {
        //Debug.Log("S2C_GetHungUpCoppyResult");
        pt_update_on_hook_c136 msg = _pt as pt_update_on_hook_c136;
        if (msg != null)
        {
            HangUpCoppyExpCount = msg.on_hook_exp;
            HangUpCoppyRemainMonsterNum = msg.surplus_monster;
            if (OnHanguUpCoppyDataUpdateEvent != null)
                OnHanguUpCoppyDataUpdateEvent();
        }
    }

    protected void S2C_GetHungUpWndResult(Pt _pt)
    {
        //Debug.Log("S2C_GetHungUpWndResult");
        pt_on_hook_ui_info_c135 msg = _pt as pt_on_hook_ui_info_c135;
        if (msg != null)
        {
            HangUpRemainBuyTimes = msg.surplus_buy_num;
            HangUpCoppyRemainMonsterNum = msg.surplus_monster;
            GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.HANGUPCOPPY, HangUpCoppyRemainMonsterNum > 0);
            if (OnHanguUpCoppyDataUpdateEvent != null)
                OnHanguUpCoppyDataUpdateEvent();
        }
    }
    #endregion

    #region 仙域守护
    /// <summary>
	/// 开启仙域守护
	/// </summary>
	public void C2S_OpenProtectActivity(int difficult)
	{
		//Debug.Log("C2S_OpenProtectActivity:"+difficult);
		if(difficult == 0)
		{
			GameCenter.messageMng.AddClientMsg("请选择难度");
			return;
		}
		pt_req_guild_guard_something_d726 msg = new pt_req_guild_guard_something_d726();
		msg.type = 2;
		msg.val = difficult;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 进入仙域守护
	/// </summary>
	public void C2S_EnterProtectActivity()
	{
        //Debug.Log("C2S_EnterProtectActivity:" + ProtectActivityOpenValue);
        if (ProtectActivityOpenValue == 0)
		{
			GameCenter.messageMng.AddClientMsg("请选择难度");
			return;
		}
		pt_req_guild_guard_something_d726 msg = new pt_req_guild_guard_something_d726();
		msg.type = 3;
        msg.val = ProtectActivityOpenValue;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 仙域守护信息
	/// </summary>
	public void C2S_ReqProtectActivityInfo()
	{
		//Debug.Log("C2S_ReqProtectActivityInfo");
		pt_req_guild_guard_something_d726 msg = new pt_req_guild_guard_something_d726();
		msg.type = 1;
		msg.val = 0;
		NetMsgMng.SendMsg(msg);
	}

	/// <summary>
	/// 请求排行榜
	/// </summary>
	public void C2S_ReqProtectActivityRank()
	{
		//Debug.Log("C2S_ReqProtectActivityRank");
		pt_req_guild_guard_something_d726 msg = new pt_req_guild_guard_something_d726();
		msg.type = 4;
		msg.val = 0;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 排行榜返回
	/// </summary>
	/// <param name="_info">Info.</param>
	protected void S2C_GotProtectActivityRank(Pt _info)
	{
		//Debug.Log("S2C_GotProtectActivityRank");
		pt_uptate_guild_guard_rank_d727 pt = _info as pt_uptate_guild_guard_rank_d727;
		if(pt != null)
		{
			ProtectActivityRankList = pt.guild_guard_rank;
			if(OnGotProtectActivityRankEvent != null)
				OnGotProtectActivityRankEvent();
		}
	}
	/// <summary>
	/// 获取开启状态
	/// </summary>
	/// <param name="_info">Info.</param>
	protected void S2C_GotProtectActivityInfo(Pt _info)
	{
		
		pt_req_guild_guard_something_d726 pt = _info as pt_req_guild_guard_something_d726;
		if(pt != null)
		{
			//Debug.Log("S2C_GotProtectActivityInfo:"+pt.type +","+pt.val);
			isProtectActivityOpen = (pt.type == 1);
			ProtectActivityOpenValue = pt.val;
			if(OnGotProtectActivityEvent != null)
				OnGotProtectActivityEvent();
		}
	}
	#endregion

	#region 攻城战
	/// <summary>
	/// 获取城内商店物品的限购次数
	/// </summary>
	public int GetCityShopLimitCount(int itemId)
	{
		for (int i = 0,max=GuildCityShopList.Count; i < max; i++) {
			if(GuildCityShopList[i].id == itemId)
				return GuildCityShopList[i].astrict_num;
		}
		return -1;
	}


	public void C2S_ReqGuildSiegeInfo()
	{
		//Debug.Log("C2S_ReqGuildSiegeInfo");
		pt_req_guild_storm_city_d729 msg = new pt_req_guild_storm_city_d729();
		msg.type = 1;
		NetMsgMng.SendMsg(msg);
	}

	public void C2S_ReqGuildSiegeApplyList()
	{
		//Debug.Log("C2S_ReqGuildSiegeApplyList");
		pt_req_guild_storm_city_d729 msg = new pt_req_guild_storm_city_d729();
		msg.type = 2;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 加入攻城战
	/// </summary>
	public void C2S_ReqJoinGuildSiege()
	{
		//Debug.Log("C2S_ReqJoinGuildSiege");
		pt_req_guild_storm_city_d729 msg = new pt_req_guild_storm_city_d729();
		msg.type = 5;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 宣告攻城战
	/// </summary>
	public void C2S_ReqApplyGuildSiege()
	{
		//Debug.Log("C2S_ReqApplyGuildSiege");
		pt_req_guild_storm_city_d729 msg = new pt_req_guild_storm_city_d729();
		msg.type = 4;
		NetMsgMng.SendMsg(msg);
	}

	/// <summary>
	/// 请求城内商店数据
	/// </summary>
	public void C2S_ReqGuildSiegeStoreInfo()
	{
		//Debug.Log("C2S_ReqGuildSiegeStore");
		pt_req_guild_storm_city_d729 msg = new pt_req_guild_storm_city_d729();
		msg.type = 3;
		NetMsgMng.SendMsg(msg);
	}

	/// <summary>
	/// 请求购买
	/// </summary>
	public void C2S_BuyGuildSiegeStoreItem(int itemID,int _num)
	{
		//Debug.Log("C2S_BuyGuildSiegeStoreItem");
		pt_req_buy_astrict_item_d732 msg = new pt_req_buy_astrict_item_d732();
		msg.item_id = itemID;
		msg.item_num = _num;
		NetMsgMng.SendMsg(msg);
	}

	/// <summary>
	/// 获取攻城战界面信息
	/// </summary>
	protected void S2C_GotGuildSiegeInfo(Pt _info)
	{
		pt_guild_storm_city_ui_info_d730 pt = _info as pt_guild_storm_city_ui_info_d730;
		if(pt != null)
		{
			//Debug.Log("S2C_GotGuildSiegeInfo:"+pt.castellan +","+pt.guild_name);
			GuildSiegeCastellan = new PlayerBaseInfo(pt);
			HadRepplySiege = (pt.apply_state == 1);
			if(OnGotGuildSiegeInfoEvent != null)
				OnGotGuildSiegeInfoEvent();
		}
	}

	protected void S2C_GotGuildSiegeApplyList(Pt _info)
	{
		pt_guild_storm_city_apply_list_d731 pt = _info as pt_guild_storm_city_apply_list_d731;
		if(pt != null)
		{
			//Debug.Log("S2C_GotGuildSiegeApplyList:"+pt.req_apply_list.Count);
			GuildSiegeApplyList = pt.req_apply_list;
			if(OnGotGuildSiegeApplyListEvent != null)
				OnGotGuildSiegeApplyListEvent();
		}
	}

	protected void S2C_GotGuildSiegeItemList(Pt _info)
	{
		pt_update_guild_astrict_list_d733 pt = _info as pt_update_guild_astrict_list_d733;
		if(pt != null)
		{
			//Debug.Log("S2C_GotGuildSiegeItemList:"+pt.astrict_item_list.Count);
			GuildCityShopList = pt.astrict_item_list;
			if(OnGotGuildCityStoreListEvent != null)
				OnGotGuildCityStoreListEvent();
		}
	}
	#endregion

	#region 仙盟篝火

    /// <summary>
    /// 请求飞仙盟篝火副本
    /// </summary>
    /// <param name="_type"></param>
    public void C2S_FlyBonfire(int _type ,int _targe)
    {
        //Debug.Log("C2S_FlyBonfire");
        pt_req_fly_guild_bonfire_d739 msg=new pt_req_fly_guild_bonfire_d739();
        msg.type = _type;
        msg.val = _targe;
        NetMsgMng.SendMsg(msg);
    }
	/// <summary>
	/// 飞到自己的公会篝火
	/// </summary>
	public void C2S_FlyMyGuildFire()
	{
		//Debug.Log("C2S_FlyMyGuildFire");
		pt_req_fly_guild_bonfire_d739 msg=new pt_req_fly_guild_bonfire_d739();
		msg.type = 1;
		msg.val = 0;
		NetMsgMng.SendMsg(msg);
	}

    /// <summary>
    /// 请求其他仙盟篝火副本信息
    /// </summary>
    public void C2S_AskOtherBonfireInfo()
    {
        //Debug.Log("C2S_AskOtherBonfireInfo");
        pt_req_look_other_guild_bonfire_d740 msg= new pt_req_look_other_guild_bonfire_d740();
        NetMsgMng.SendMsg(msg);

    }
 
    
    /// <summary>
    /// 获得其他仙盟篝火信息
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_GotOtherBonfireInfo(Pt _info)
    {

        //Debug.Log("S2C_GotOtherBonfireInfo");
        pt_other_guild_bonfire_info_d741 pt = _info as pt_other_guild_bonfire_info_d741;
        if(pt!=null)
        {
            OtherBonfireList.Clear();
            if(pt.other_bonfire_list.Count>0)
            {
                OtherBonfireList = pt.other_bonfire_list;
                for (int i = 0; i < pt.other_bonfire_list.Count; i++)
                {
                    Debug.Log("公会名字" + pt.other_bonfire_list[i].guild_name);
                }
            }
            if (OnGoTOtherBonfireInfo != null)
            {
                OnGoTOtherBonfireInfo();
            }

        }

    }






	#endregion

	#region 膜拜
	/// <summary>
	/// 膜拜
	/// </summary>
	public void C2S_Morship()
	{
		pt_req_worship_d620 msg = new pt_req_worship_d620();
		NetMsgMng.SendMsg(msg);
	}
	#endregion

    #region 活动结算
    /// <summary>
    /// 仙盟篝火和仙域守护活动结算
    /// </summary>
    /// <param name="_msg"></param>
    public void S2C_ActivityBalance(Pt _msg)
    {
        pt_activity_game_over_d728 msg = _msg as pt_activity_game_over_d728;
        if (msg != null)
        {
            isGuildStormCity = false;
            rewardList = msg.activity_reward;
            activeState = msg.state;
            if (GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType == SceneUiType.RAIDERARK)
            {
                if (OnGetReward != null)
                    OnGetReward(msg.activity_reward);
            }
            else
                GameCenter.uIMng.SwitchToUI(GUIType.ACTIVITYBALANCE);
        }
    }
    /// <summary>
    /// 攻城战活动结算
    /// </summary>
    /// <param name="_msg"></param>
    public void S2C_ActivityStormCity(Pt _msg)
    {
        pt_guild_storm_city_over_d778 msg = _msg as pt_guild_storm_city_over_d778;
        {
            if (msg != null)
            {
                isGuildStormCity = true;
                if (msg.win_guildname != null)
                {
                    vctorName = msg.win_guildname;
                }
                else
                    vctorName = "GM军团";
                GameCenter.uIMng.SwitchToUI(GUIType.ACTIVITYBALANCE);
            }
        }
    }
    #endregion

    #region  夺宝奇兵
    /// <summary>
    /// 得到采集到的宝珠相关信息
    /// </summary>
    /// <param name="_msg"></param>
    public void S2C_GetJewelryList(Pt _msg)
    {
        pt_update_jewelry_list_d794 msg = _msg as pt_update_jewelry_list_d794;
        if (msg != null)
        {
            jewelryList = msg.jewelry_list;
            GameCenter.activityMng.rewardId = 0;
            //for (int i = 0; i < msg.jewelry_list.Count; i++)
            //{
            //    Debug.Log("采集到的宝珠ID  " + msg.jewelry_list[i].jewelry_id + "  数量  " + msg.jewelry_list[i].num);
            //}
        }
        if (OnUpdateJewelry != null)
            OnUpdateJewelry();
    }
    /// <summary>
    /// 采集完成
    /// </summary>
    public void S2C_GetJewerlyFinish(Pt _msg)
    {
        pt_gather_jewelry_finish_d796 msg = _msg as pt_gather_jewelry_finish_d796;
        if (msg != null)
        {
            //Debug.Log("采集完成宝珠ID  " + msg.jewelry_type);
            switch (msg.jewelry_type)
            {
                case 0: rewardId = 0; break;//断线重连，清除宝珠
                case 1: rewardId = 4000001; break;
                case 2: rewardId = 4000002; break;
                case 3: rewardId = 4000003; break;
                case 4:
                    Debug.Log("玩家拾取紫色人参果");
                    rewardId = 4000004;
                    MessageST message = new MessageST();
                    message.messID = 460;
                    message.words = new string[1] { GameCenter.mainPlayerMng.MainPlayerInfo.Name+"111" };
                    //GameCenter.messageMng.AddClientMsg(message);
                    //GameCenter.messageMng.AddClientMsg(460);
                    break;
            }
            if (OnStopCollect != null && rewardId != 0)
                OnStopCollect();
            coldGetTime = Time.time;
        }
    }
    /// <summary>
    /// 请求飞夺宝奇兵场景
    /// </summary>
    public void C2S_FlyRaiderArk()
    {
        pt_req_fly_snatch_soldier_d793 msg = new pt_req_fly_snatch_soldier_d793();
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求领取奖励
    /// </summary>
    public void C2S_ReqGetReward()
    {
        pt_get_snatch_soldier_reward_d795 msg = new pt_get_snatch_soldier_reward_d795();
        NetMsgMng.SendMsg(msg);
    }
    #endregion
    #region 活动大厅
    /// <summary>
    /// 请求活动大厅数据
    /// </summary>
    public void C2S_ActivityDataInfo()
    {
        pt_req_activity_info_d714 info = new pt_req_activity_info_d714();
        NetMsgMng.SendMsg(info);
    }
    void S2C_ActivityDataInfo(Pt _info)
    {
        pt_update_activity_info_d715 info = _info as pt_update_activity_info_d715;
        activity_list data = null;
        for (int i = 0; i < info.activity_list.Count; i++)
        {
            data = info.activity_list[i];
            if (activityDic.ContainsKey(data.id))
            {
                activityDic[data.id].Update(data);
            }
            else
            {
                activityDic[data.id] = new ActivityDataInfo(data);
            }
        }
        FDictionary datalist = ConfigMng.Instance.GetActivityList();
        foreach (ActivityListRef refdata in datalist.Values)
        {
            //Debug.Log("活动大厅数据" + refdata.name+"");
            if (!activityDic.ContainsKey(refdata.id))
            {
                activityDic[refdata.id] = new ActivityDataInfo(refdata.id);
                //Debug.Log("活动大厅数据去构造活动数据"+ activityDic[refdata.id].State);
            }
        }
        if (OnActivityDataInfo != null) OnActivityDataInfo();
    }
    void S2C_FairyDomainToprotect(Pt _info)
    {
        pt_activity_guild_guard_time_d756 info = _info as pt_activity_guild_guard_time_d756;
        //Debug.Log("接收协议pt_activity_guild_guard_time_d756");
        if (activityDic.ContainsKey(info.act_id))
        {
            //Debug.Log("info.act_id:" + info.act_id+",info.start_state:" + info.start_state);
            if (activityDic[info.act_id].ID == ActivityType.UNDERBOSS && info.start_state == 1)
                activityDic[info.act_id].UnderBossBeKilled();//地宫BOSS被击杀更新
            else
                activityDic[info.act_id].Update(info.start_state, info.surplus_time);
        }
        if (OnActivityDataInfo != null) OnActivityDataInfo();
        //地宫魔王被消灭之后
    }
    #endregion
    
}
