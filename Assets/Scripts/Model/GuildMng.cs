//==================================
//作者：邓成
//日期：2016/7/5
//用途：公会管理类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class GuildMng
{
	/// <summary>
	/// 公会成员最大扩充次数
	/// </summary>
	public const int MEMBER_EXPAND_MAX_TIMES = 10;
	public const int MEMBER_EXPAND_PER_COUNT = 2;

    #region 数据
	Dictionary<int,GuildData> guildDic = new Dictionary<int,GuildData>();
    /// <summary>
    /// 公会列表
    /// </summary>
	public Dictionary<int,GuildData> GuildDic
    {
        get
        {
            return guildDic;
        }
    }
	List<GuildMemberData> guildMemberList = new List<GuildMemberData>();
    /// <summary>
    /// 公会成员列表
    /// </summary>
	public List<GuildMemberData> GuildMemberList
    {
        get
        {
			return guildMemberList;
        }
    }

	public bool haveViceChairman = false;
    
    /// <summary>
    /// 新成员验证列表
    /// </summary>
	public List<GuildMemberData> newMemberList = new List<GuildMemberData>();
	/// <summary>
	/// 自动接受入会申请的开关
	/// </summary>
	public bool AutoAgreeToggle = false;
	/// <summary>
	/// 自动接受入会的要求(战力)
	/// </summary>
	public int AutoAgreeValue = 0;
	/// <summary>
	/// 捐献的金币上限
	/// </summary>
	//public int DonateLimitCoin = 100000;
	/// <summary>
	/// 捐献的元宝上限
	/// </summary>
	//public int DonateLimitDiamond = 100;
	/// <summary>
	/// 是否需要发送捐献消息到仙盟(功能修改默认为true)
	/// </summary>
	//public bool NeedSendDonateMsg = false;

    /// <summary>
    /// 剩余捐赠次数
    /// </summary>
    public int restDonateTimes = 0;
    /// <summary>
    /// 最大次数
    /// </summary>
    public int maxDonateTime =3;

	public List<guild_log> guildLogList = new List<guild_log>();

    protected string notice = string.Empty;
    /// <summary>
    /// 公会公告
    /// </summary>
    public string Notice
    {
        get { return notice; }
    }

    protected string invitePlayerName = string.Empty;
    /// <summary>
    /// 邀请我加入他仙盟的玩家名字
    /// </summary>
    public string InvitePlayerName
    {
        get
        {
            return invitePlayerName;
        }
    }
    protected string inviteGuildName = string.Empty;
    /// <summary>
    /// 邀请我加的仙盟的名字
    /// </summary>
    public string InviteGuildName
    {
        get
        {
            return inviteGuildName;
        }
    }

	#region 仓库
	/// <summary>
	/// 仓库物品数据集
	/// </summary>
	protected Dictionary<int, EquipmentInfo> storageDictionary = new Dictionary<int, EquipmentInfo>();
	/// <summary>
	/// 仓库物品数据集
	/// </summary>
	public Dictionary<int, EquipmentInfo> WareHouseDictionary
	{
		get { return storageDictionary; }
	}
	/// <summary>
	/// 仓库物品数据集,包含空格
	/// </summary>
	protected Dictionary<int,EquipmentInfo> realStorageDictionary = new Dictionary<int,EquipmentInfo>();
	/// <summary>
	/// 仓库物品数据集,包含空格
	/// </summary>
	public Dictionary<int,EquipmentInfo> RealStorageDictionary
	{
		get { return realStorageDictionary; }
	}
	/// <summary>
	/// 入会申请列表
	/// </summary>
	public List<guild_check_out_item_ask_list> ApplyList = new List<guild_check_out_item_ask_list>();
	/// <summary>
	/// 公会仓库日志列表
	/// </summary>
	public List<guild_items_log_list> storageLogList = new List<guild_items_log_list>();
	/// <summary>
	/// 仓库物品数据获得
	/// </summary>
	public System.Action OnGotStorageData;
	/// <summary>
	/// 仓库数据发生变化
	/// </summary>
	public System.Action OnStorageItemUpdate;
	#endregion

    /// <summary>
    /// 获取我的公会信息
    /// </summary>
    protected GuildInfo myGuildInfo = null;
    public GuildInfo MyGuildInfo
    {
        get { return myGuildInfo; }
    }
	/// <summary>
	/// 自己是不是会长
	/// </summary>
	public bool isMyPresident
	{
		get
		{
			return myGuildInfo != null?myGuildInfo.PresidentName.Equals(GameCenter.mainPlayerMng.MainPlayerInfo.Name):false;
		}
	}
    /// <summary>
    /// 自己是不是副会长
    /// </summary>
    public bool isMyViceChairman
    {
        get
        {
            return myGuildInfo != null ? myGuildInfo.MyPosition.Equals(GuildMemPosition.VICECHAIRMAN):false;
        }
    }
	/// <summary>
	/// 公会成员数
	/// </summary>
	public int MemberCount
	{
		get
		{
			return myGuildInfo != null?myGuildInfo.MemberNum:0;
		}
	}
    /// <summary>
    /// 关闭仙盟商店or仙盟争霸后是否需要打开仙盟界面
    /// </summary>
    public bool NeedOpenGuildWnd = true;

    protected MainPlayerInfo mainPlayerInfo = null;
    /// <summary>
    /// 是否领取过仙盟活跃奖励
    /// </summary>
    /// <param name="_livelyRewardId"></param>
    /// <returns></returns>
    public bool HaveGotLivelyReward(int _livelyRewardId)
    {
        if (guildLivelyData != null)
        {
            if (guildLivelyData.reward_list.Contains((uint)_livelyRewardId))
                return true;
        }
        return false;
    }

    public int CurLivelyCount
    {
        get
        {
            return guildLivelyData == null ? 0 : (int)guildLivelyData.liveness_guild;
        }
    }
	#endregion

    #region 构造
    public static GuildMng CreateNew(MainPlayerMng _main)
    {
        if (_main.guildMng == null)
        {
            GuildMng guildMng = new GuildMng();
            guildMng.Init(_main);
            return guildMng;
        }
        else
        {
            _main.guildMng.UnRegist();
            _main.guildMng.Init(_main);
            return _main.guildMng;
        }
    }


    protected void Init(MainPlayerMng _main)
    {
        mainPlayerInfo = _main.MainPlayerInfo;
        MsgHander.Regist(0xD501, S2C_GetGuildList);
		MsgHander.Regist(0xD379, S2C_OnGotStorageData);
		MsgHander.Regist(0xD506, S2C_OnGotChangeStorageData);
		MsgHander.Regist(0xD392, S2C_GuildLog);
		MsgHander.Regist(0xD503, S2C_GetNewMemberList);
		MsgHander.Regist(0xD380, S2C_GuildInfo);
		MsgHander.Regist(0xD381, S2C_GetGuildMember);
		MsgHander.Regist(0xD382, S2C_GuildNameUpdate);
		MsgHander.Regist(0xD524, S2C_OnGetItemApplyList);
		MsgHander.Regist(0xD525, S2C_GetStorageLogList);
		MsgHander.Regist(0xD394, S2C_UpdateSalaryState);
		MsgHander.Regist(0xD517, S2C_UpdateAutoReplyState);
		MsgHander.Regist(0xD527, S2C_GuildName);
        MsgHander.Regist(0xD50A, S2C_DonateLimitData);
        MsgHander.Regist(0xD50C, S2C_DonateLimitInfo);
        MsgHander.Regist(0xD138, S2C_GuildNotice);
        MsgHander.Regist(0xC124, S2C_ReqGuildResult);
        MsgHander.Regist(0xD50E, S2C_GotGuildLivelyData);
        MsgHander.Regist(0xD51A, S2C_OnGotLivelyRewardResult);
    }
    protected void UnRegist()
    {
		MsgHander.UnRegist(0xD501, S2C_GetGuildList);
		MsgHander.UnRegist(0xD379, S2C_OnGotStorageData);
		MsgHander.UnRegist(0xD506, S2C_OnGotChangeStorageData);
		MsgHander.UnRegist(0xD392, S2C_GuildLog);
		MsgHander.UnRegist(0xD380, S2C_GuildInfo);
		MsgHander.UnRegist(0xD503, S2C_GetNewMemberList);
        MsgHander.UnRegist(0xD381, S2C_GetGuildMember);
		MsgHander.UnRegist(0xD382, S2C_GuildNameUpdate);
		MsgHander.UnRegist(0xD524, S2C_OnGetItemApplyList);
		MsgHander.UnRegist(0xD525, S2C_GetStorageLogList);
		MsgHander.UnRegist(0xD394, S2C_UpdateSalaryState);
		MsgHander.UnRegist(0xD517, S2C_UpdateAutoReplyState);
		MsgHander.UnRegist(0xD527, S2C_GuildName);
        MsgHander.UnRegist(0xD50A, S2C_DonateLimitData);
        MsgHander.UnRegist(0xD50C, S2C_DonateLimitInfo);
        MsgHander.UnRegist(0xD138, S2C_GuildNotice);
        MsgHander.UnRegist(0xC124, S2C_ReqGuildResult);
        MsgHander.UnRegist(0xD50E, S2C_GotGuildLivelyData);
        MsgHander.UnRegist(0xD51A, S2C_OnGotLivelyRewardResult);
        guildDic.Clear();
		guildMemberList.Clear();
		haveViceChairman = false;
        restDonateTimes = 0;
        maxDonateTime = 3;
    }
    #endregion


	protected int CompareMember(GuildMemberData mem1,GuildMemberData mem2)
	{
		if((int)mem1.memberPosition > (int)mem2.memberPosition)
		{
			return -1;
		}else if((int)mem1.memberPosition < (int)mem2.memberPosition)
		{
			return 1;
		}else
		{
			return (mem2.fightValue - mem1.fightValue);
		}
	}

    #region 委托
    public System.Action OnGetGuildListEvent;//公会
    public System.Action OnUpdateMemberEvent;//成员
    public System.Action OnUpdateNoticeEvent;//公告
    public System.Action OnGetNewMemberEvent;//新成员
    public System.Action OnGetPublicEvent;//公共信息
	public System.Action OnGetGuildLogEvent;//公会日志
	public System.Action OnGetApplyItemListEvent;//取出申请列表
	public System.Action OnGetStorageLogListEvent;//取出申请列表
	public System.Action OnUpdateSalaryStateEvent;//刷新俸禄状态
	public System.Action OnUpdateAutoStateEvent;//更新自动审核状态
	public System.Action OnUpdateMemberMaxCountEvent;//更新公会人数上限
    public System.Action OnGetReqGuildMessageEvent;//收到仙盟邀请消息
    public System.Action OnGuildDonateTimeEvent;//仙盟捐献次数刷新
    public System.Action OnGotGuildLivelyDataEvent;//仙盟活跃数据
    public System.Action OnGotGuildLivelyRewardEvent;//仙盟活跃奖励
    #endregion


    #region 协议

    #region S2C
    /// <summary>
    /// 获取公会列表
    /// </summary>
    /// <param name="pt"></param>
    protected void S2C_GetGuildList(Pt pt)
    {
		//Debug.Log("S2C_GetGuildList");
		pt_guild_list_d501 guildList = pt as pt_guild_list_d501;
        if (guildList != null)
        {
            guildDic.Clear();
			for (int i = 0,max=guildList.all_guild_info.Count; i < max; i++) {
				GuildData data = new GuildData(guildList.all_guild_info[i]);
				guildDic[data.guildId] = data;
			}
            if (OnGetGuildListEvent != null)
				OnGetGuildListEvent();
        }
    }
    /// <summary>
    /// 获取公会成员信息
    /// </summary>
    /// <param name="pt"></param>
    protected void S2C_GetGuildMember(Pt pt)
    {
		//Debug.Log("S2C_GetGuildMember");
		pt_guild_members_info_d381 info = pt as pt_guild_members_info_d381;
        if (info != null)
        {
			guildMemberList.Clear();
			haveViceChairman = false;
			for (int i = 0,max=info.memeber_info_list.Count; i < max; i++) {
				GuildMemberData data = new GuildMemberData(info.memeber_info_list[i]);
				guildMemberList.Add(data);
				if(data.memberPosition == GuildMemPosition.VICECHAIRMAN)
					haveViceChairman = true;
			}
			guildMemberList.Sort(CompareMember);
            if (OnUpdateMemberEvent != null)
                OnUpdateMemberEvent();
        }
    }
    /// <summary>
    /// 获取公共信息
    /// </summary>
    /// <param name="pt"></param>
    protected void S2C_GuildInfo(Pt pt)
    {
		//Debug.Log("S2C_GuildInfo");
		pt_guild_info_d380 info = pt as pt_guild_info_d380;
		if (info.lev == 0) return;
        if (info != null)
        {
            GuildData data = new GuildData(info);
            if (myGuildInfo == null)
            {
                GuildInfo guildInfo = new GuildInfo(data);
                myGuildInfo = guildInfo;
            }
            else
            {
                myGuildInfo.UpdateServerData(data);
            }
            if (OnGetPublicEvent != null)
                OnGetPublicEvent();
        }
    }
	/// <summary>
	/// 获取公会日志
	/// </summary>
	/// <param name="pt">Point.</param>
	protected void S2C_GuildLog(Pt pt)
	{
		//Debug.Log("S2C_GuildLog");
		pt_guild_log_info_d392 pt_guild_log = pt as pt_guild_log_info_d392;
		if(pt_guild_log != null)
		{
			guildLogList.Clear();
			guildLogList = pt_guild_log.logs;
            guildLogList.Sort(CompareGuildLog);
		}
		if(OnGetGuildLogEvent != null)
			OnGetGuildLogEvent();
	}
    int CompareGuildLog(guild_log log1,guild_log log2)
    {
        if (log1.time > log2.time)
            return -1;
        if (log1.time < log2.time)
            return 1;
        return 0;
    }
	/// <summary>
	/// 服务端仓库数据返回 by邓成
	/// </summary>
	/// <param name="_info"></param>
	protected void S2C_OnGotStorageData(Pt _info)
	{
		//Debug.Log("S2C_OnGotStorageData");
		pt_guild_item_info_d379 pt_item = _info as pt_guild_item_info_d379;
		if (pt_item != null)
		{
			int count = pt_item.guld_items.Count;
			storageDictionary.Clear();
			realStorageDictionary.Clear();
			for (int i = 0; i < SystemSettingMng.MAX_GUILD_STORAGE_NUM; i++)
			{
				if(i<count)//仓库中非空格的数据
				{
					EquipmentInfo info = new EquipmentInfo(pt_item.guld_items[i]);
					if(info.StackCurCount == 0)
						info = new EquipmentInfo(pt_item.guld_items[i],EquipmentInfo.EmptyType.EMPTY,EquipmentBelongTo.STORAGE);
					if (!storageDictionary.ContainsKey((int)info.InstanceID))
					{
						storageDictionary[(int)info.InstanceID] = info;
					}
					realStorageDictionary[info.Postion] = info;
				}
			}
			//空格子
			int len = pt_item.emptys.Count;
			for(int i = 0; i < len; i++)
			{
				realStorageDictionary[(int)pt_item.emptys[i]] = new EquipmentInfo((int)(pt_item.emptys[i]),EquipmentInfo.EmptyType.EMPTY,EquipmentBelongTo.STORAGE);//空格物品
			}
		}
		if(OnGotStorageData != null)
			OnGotStorageData();
	}
	/// <summary>
	/// 公会仓库变化返回 by邓成
	/// </summary>
	/// <param name="_info"></param>
	protected void S2C_OnGotChangeStorageData(Pt _info)
	{
		pt_guild_item_chg_d506 pt_guild_item_chg = _info as pt_guild_item_chg_d506;
		if (pt_guild_item_chg != null)
		{
			int count = pt_guild_item_chg.guild_item.Count;
			if (count >= 0)
			{
				for (int i = 0; i < count; i++)
				{
					EquipmentInfo info = new EquipmentInfo( pt_guild_item_chg.guild_item[i]);
                    //Debug.Log("info.pos:" + info.Postion + ",info.count:" + info.StackCurCount);
					if(info.StackCurCount == 0)
						info = new EquipmentInfo(pt_guild_item_chg.guild_item[i],EquipmentInfo.EmptyType.EMPTY,EquipmentBelongTo.STORAGE);
					storageDictionary[info.InstanceID] = info;
					realStorageDictionary[info.Postion] = info;
				}
			}
            if (OnGotStorageData != null)
                OnGotStorageData();
		}
	}
	/// <summary>
	/// 获取申请列表
	/// </summary>
	protected void S2C_OnGetItemApplyList(Pt pt)
	{
		//Debug.Log("S2C_OnGetItemApplyList");
		pt_guild_check_out_item_ask_list_d524 pt_applyList = pt as pt_guild_check_out_item_ask_list_d524;
		if(pt_applyList != null)
		{
			ApplyList.Clear();
			ApplyList = pt_applyList.ask_list;
			if(OnGetApplyItemListEvent != null)
				OnGetApplyItemListEvent();
		}
	}

	protected void S2C_GetStorageLogList(Pt pt)
	{
		//Debug.Log("S2C_GetStorageLogList");
		pt_guild_items_log_d525 pt_log = pt as pt_guild_items_log_d525;
		if(pt_log != null)
		{
			storageLogList.Clear();
			storageLogList = pt_log.guild_item_logs;
			if(OnGetStorageLogListEvent != null)
				OnGetStorageLogListEvent();
		}
	}

    /// <summary>
    /// 获取新成员验证列表
    /// </summary>
    /// <param name="pt"></param>
    protected void S2C_GetNewMemberList(Pt pt)
    {
        newMemberList.Clear();
		//Debug.Log("S2C_GetNewMemberList:d503");
		pt_ask_join_list_d503 info = pt as pt_ask_join_list_d503;
        if (info != null)
        {
			for (int i = 0; i < info.ask_join_info.Count; i++)
            {
				GuildMemberData data = new GuildMemberData(info.ask_join_info[i]);
                newMemberList.Add(data);
            }
			AutoAgreeToggle = (info.open_state == 1);
			AutoAgreeValue = info.fight_score;
            if (OnGetNewMemberEvent != null)
                OnGetNewMemberEvent();
        }

    }
    /// <summary>
    /// 获取公告
    /// </summary>
    /// <param name="pt"></param>
    protected void S2C_GuildNotice(Pt pt) 
    {
        pt_guild_notice_d138 info = pt as pt_guild_notice_d138;
        if (info != null)
        {
            notice = info.data;
            if (myGuildInfo != null)
                myGuildInfo.UpdateNotice(info.data);
            if (OnUpdateNoticeEvent != null)
                OnUpdateNoticeEvent();
        }
    }
	/// <summary>
	/// 更新公会名字、公告(创建、退出、加入公会时接受)
	/// </summary>
	/// <param name="pt">Point.</param>
	protected void S2C_GuildNameUpdate(Pt pt)
	{ 
		pt_req_creat_guild_d382 pt_create = pt as pt_req_creat_guild_d382;
        //Debug.Log("S2C_GuildNameUpdate ： " + pt_create.name);
        if (pt_create != null)
        {
            if (mainPlayerInfo != null)
                mainPlayerInfo.UpdateGuildName(pt_create.name);
            if (OnUpdateNoticeEvent != null)
                OnUpdateNoticeEvent();
            C2S_ReqLivelyData();
            C2S_GuildDonateTimes();//请求仙盟捐献次数
            GameCenter.guildSkillMng.C2S_AskGuildSkillList();
        }  
	}
	/// <summary>
	/// 获取捐赠信息
	/// </summary>
	/// <param name="_pt">Point.</param>
	protected void S2C_DonateLimitData(Pt _pt)
	{ 
        pt_guild_contribute_result_d50a info = _pt as pt_guild_contribute_result_d50a;
		if(info != null)
		{
            restDonateTimes = maxDonateTime - info.times;
            //Debug.Log("S2C_DonateLimitData: time:" + info.times + ",type:" + info.type);  
			//#0#捐献了 #1# 个 #2#
            GuildDonateRef guildDonateRef = ConfigMng.Instance.GetGuildDonateRef(info.type);
            if (guildDonateRef != null)
            {
                if (guildDonateRef.cost.Count > 0)
                {
                    EquipmentInfo eqinfo = new EquipmentInfo(guildDonateRef.cost[0].eid, EquipmentBelongTo.PREVIEW);
                    string msg = ConfigMng.Instance.GetUItext(45, new string[3] { GameCenter.mainPlayerMng.MainPlayerInfo.Name, guildDonateRef.cost[0].count.ToString(), eqinfo.ItemName });
                    GameCenter.chatMng.SendMsgByType(ChatInfo.Type.Guild, msg);
                } 
            }
            GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.GUILDDONATE, restDonateTimes > 0);
            if (OnGuildDonateTimeEvent != null) OnGuildDonateTimeEvent();
		}
	}

    protected void S2C_DonateLimitInfo(Pt _pt)
    {
        pt_guild_contribute_info_d50c info = _pt as pt_guild_contribute_info_d50c;
        if (info != null)
        { 
            maxDonateTime = info.max_times;
            restDonateTimes = info.max_times - info.times;
            GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.GUILDDONATE, restDonateTimes > 0);
            if (OnGuildDonateTimeEvent != null) OnGuildDonateTimeEvent();
        }
    }  


    /// <summary>
    /// 公会名字的更新
    /// </summary>
    /// <param name="_pt"></param>
    protected void S2C_GuildName(Pt _pt)
    {
		pt_guild_name_chg_d527 info = _pt as pt_guild_name_chg_d527;
        if (info != null)
        {
			//Debug.Log("S2C_GuildName:"+info.name);
			mainPlayerInfo.UpdateGuildName(info.name);
        }
        if (info == null || string.IsNullOrEmpty(info.name))
        {
            GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.GUILDACTIVE, false);
            GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.GUILDDONATE, false);
            GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.GUILDSKILL, false);
        }
    }
    /// <summary>
    /// 收到别人邀请消息
    /// </summary>
    protected void S2C_ReqGuildResult(Pt _info)
    {
        //Debug.Log("S2C_ReqGuildResult");
        pt_return_guild_ask_c124 pt = _info as pt_return_guild_ask_c124;
        if (pt != null)
        {
            invitePlayerName = pt.ask_name;
            inviteGuildName = pt.ask_guild_name;
        }
        if (OnGetReqGuildMessageEvent != null)
            OnGetReqGuildMessageEvent();
    }

	/// <summary>
	/// 更新工资状态
	/// </summary>
	protected void S2C_UpdateSalaryState(Pt pt)
	{
		pt_req_guild_salary_d394 pt_salary = pt as pt_req_guild_salary_d394;
		if(pt_salary != null)
		{
			GameCenter.messageMng.AddClientMsg(388);
			if(myGuildInfo != null)
				myGuildInfo.UpdateSalary(true);
			if(OnUpdateSalaryStateEvent != null)
				OnUpdateSalaryStateEvent();
		}
	}

	protected void S2C_UpdateAutoReplyState(Pt pt)
	{
		//Debug.Log("S2C_UpdateAutoReplyState");
		pt_leader_req_add_all_receive_state_d517 pt_auto = pt as pt_leader_req_add_all_receive_state_d517;
		if(pt_auto != null)
		{
			AutoAgreeToggle = (pt_auto.open_state == 1);
			AutoAgreeValue = pt_auto.fight_score;
			if(OnUpdateAutoStateEvent != null)
				OnUpdateAutoStateEvent();
		}
	}

	protected void S2C_AddMemberCountResult(Pt _info )
	{
		pt_guild_expand_result_d817 pt = _info as pt_guild_expand_result_d817;
		if(pt != null)
		{
			Debug.Log("S2C_AddMemberCountResult:"+pt.result);
		}
	}

    public pt_guild_liveness_info_d50e guildLivelyData = null;
    /// <summary>
    /// 是否有可领取的活跃奖励
    /// </summary>
    public bool HaveRewardCanGet
    {
        get
        {
            if (guildLivelyData == null) return false;
            FDictionary livelyReward = ConfigMng.Instance.GetGuildLivelyRewardRefTable();
            foreach (GuildLivelyRewardRef reward in livelyReward.Values)
            {
                if (reward.need <= CurLivelyCount && !guildLivelyData.reward_list.Contains((uint)reward.id))
                {
                    return true;
                }
            }
            return false;
        }
    }
    protected void S2C_GotGuildLivelyData(Pt _info)
    {
        //Debug.Log("S2C_GotGuildLivelyData");
        pt_guild_liveness_info_d50e pt = _info as pt_guild_liveness_info_d50e;
        if (pt != null)
        {
            guildLivelyData = pt;
            guildLivelyData.member_info_list.Sort(CompareLivelyRank);
            if (OnGotGuildLivelyDataEvent != null)
                OnGotGuildLivelyDataEvent();
        }
        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.GUILDACTIVE, HaveRewardCanGet);
    }

    protected void S2C_OnGotLivelyRewardResult(Pt _info)
    {
        pt_guild_liveness_reward_succ_d51a pt = _info as pt_guild_liveness_reward_succ_d51a;
        if (pt != null)
        {
            //Debug.Log("S2C_OnGotLivelyRewardResult:" + pt.reward_id);
            if (guildLivelyData != null && !guildLivelyData.reward_list.Contains(pt.reward_id))
                guildLivelyData.reward_list.Add(pt.reward_id);
        }
        if (OnGotGuildLivelyRewardEvent != null)
            OnGotGuildLivelyRewardEvent();
        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.GUILDACTIVE,HaveRewardCanGet);
    }
    #endregion

    int CompareLivelyRank(guild_liveness_member_info data1,guild_liveness_member_info data2)
    {
        if (data1.liveness > data2.liveness)
            return -1;
        if (data1.liveness < data2.liveness)
            return 1;
        return 0;
    }

    #region 已修改
    /// <summary>
    /// 请求公会列表 1036
    /// </summary>
	public void C2S_GuildList(int page)
    {
		//Debug.Log("C2S_GuildList");
		pt_req_guild_list_d500 msg = new pt_req_guild_list_d500();
		msg.page = page;
        NetMsgMng.SendMsg(msg);
    }
	/// <summary>
	/// 申请入会
	/// </summary>
	/// <param name="guildList">Guild list.</param>
	public void C2S_JoinGuild(List<int> guildList)
	{
		//Debug.Log("C2S_JoinGuild:guildList:"+guildList.Count);
		pt_req_join_guild_d502 msg = new pt_req_join_guild_d502();
		msg.id_list = guildList;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 申请入会
	/// </summary>
	/// <param name="guildList">Guild list.</param>
	public void C2S_JoinGuild(int guildID)
	{
		//Debug.Log("C2S_JoinGuild:"+guildID);
		List<int> list = new List<int>();
		list.Add(guildID);
		pt_req_join_guild_d502 msg = new pt_req_join_guild_d502();
		msg.id_list = list;
		NetMsgMng.SendMsg(msg);
	}
    /// <summary>
    /// 邀请加入仙盟
    /// </summary>
    /// <param name="_pid"></param>
    public void C2S_ReqJoinGuild(int _pid)
    {
        //Debug.Log("C2S_ReqJoinGuild");
        if (GameCenter.mainPlayerMng.MainPlayerInfo.HavaGuild)
        {
            pt_ask_join_guild_c123 msg = new pt_ask_join_guild_c123();
            msg.target_uid = (uint)_pid;
            NetMsgMng.SendMsg(msg);
        }
        else
        {
            GameCenter.messageMng.AddClientMsg(534);
        }
    }
    /// <summary>
    /// 回复仙盟邀请
    /// </summary>
    public void C2S_ApplyGuildInvite(bool _agree)
    {
        //Debug.Log("C2S_ApplyGuildInvite");
        pt_ask_join_guild_answer_c125 msg = new pt_ask_join_guild_answer_c125();
        msg.answer = _agree?0:1;
        NetMsgMng.SendMsg(msg);
    }

    /// <summary>
	/// 请求创建公会createType=1材料,2元宝
    /// </summary>
	public void C2S_CreateNewGuild(string _name,string notice,int createType)
    {
		//Debug.Log("C2S_CreateNewGuild:"+_name+",notice:"+notice+",createType:"+createType);
		if(!GameCenter.loginMng.CheckBadWord(_name+notice))
		{
			return;
		}
		if(_name.Contains(" "))
		{
			GameCenter.messageMng.AddClientMsg("不能含有空格!");
			return;
		}
		pt_req_creat_guild_d382 msg = new pt_req_creat_guild_d382();
		msg.name = _name;
		msg.purpose = notice;
		msg.creat_type = createType;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 退出公会
    /// </summary>
	public void C2S_OutGuild()
    {
		//Debug.Log("C2S_OutGuild");
		pt_req_quilt_guild_d513 msg = new pt_req_quilt_guild_d513();
        NetMsgMng.SendMsg(msg);
    }
	/// <summary>
	/// 踢出公会
	/// </summary>
	public void C2S_FourceOutGuild(int memID)
	{
		//Debug.Log("C2S_FourceOutGuild");
		pt_req_kick_guild_member_d509 msg = new pt_req_kick_guild_member_d509();
		msg.tagert_id = memID;
		NetMsgMng.SendMsg(msg);
	}
    /// <summary>
    /// 回复加入公会的申请
    /// </summary>
	protected void C2S_ReplyJoin(List<int> memList,int action)
    {
		//Debug.Log("C2S_ReplyJoin ");
		pt_req_agree_join_guild_d504 msg = new pt_req_agree_join_guild_d504();
		msg.uid_list = memList;
		msg.action = action;
        NetMsgMng.SendMsg(msg);
    }
	/// <summary>
	/// 回复加入公会的申请
	/// </summary>
	public void C2S_ReplyJoin(int memID,int action)
	{
		//Debug.Log("C2S_ReplyJoin:"+memID+",action:"+action);
		List<int> memList = new List<int>();
		memList.Add(memID);
		C2S_ReplyJoin(memList,action);
	}
	/// <summary>
	/// 一键接受
	/// </summary>
	public void C2S_ReplyJoin()
	{
		//Debug.Log("C2S_ReplyJoin all");
		List<int> memList = new List<int>();
		for (int i = 0,max=newMemberList.Count; i < max; i++) 
		{
			memList.Add(newMemberList[i].memberID);
		}
		C2S_ReplyJoin(memList,1);
	}
	/// <summary>
	/// 自动接收
	/// </summary>
	public void C2S_AutoReplyJoin(bool open,int minValue)
	{
		//Debug.Log("C2S_AutoReplyJoin: open " + open+",minValue:"+minValue);
		pt_leader_req_add_all_receive_state_d517 msg = new pt_leader_req_add_all_receive_state_d517();
		msg.open_state = open?1:0;
		msg.fight_score = minValue;
		NetMsgMng.SendMsg(msg);
	}

	/// <summary>
	/// 拒绝全部
	/// </summary>
	public void C2S_RefruseJoin()
	{
		//Debug.Log("C2S_RefruseJoin");
		pt_leader_req_clear_ask_join_guild_list_d519 msg = new pt_leader_req_clear_ask_join_guild_list_d519();
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 请求申请仙盟的成员信息
	/// </summary>
	public void C2S_GuildJoinList()
	{
		//Debug.Log("C2S_GuildJoinList");
		pt_req_ask_join_list_d505 msg = new pt_req_ask_join_list_d505();
		NetMsgMng.SendMsg(msg);
	}
    /// <summary>
    /// 请求公会成员信息
    /// </summary>
    public void C2S_GuildMemberList()
    {
		//Debug.Log("C2S_GuildMemberList");
		pt_req_guild_member_info_d384 msg = new pt_req_guild_member_info_d384();
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 公会公共信息
    /// </summary>
    public void C2S_GuildInfo()
    {
		pt_req_guild_info_d383 msg = new pt_req_guild_info_d383();
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求移动仙盟仓库物品
    /// </summary>
    public void C2S_MoveStorageItem(int _action,int id)
    {
		//Debug.Log("C2S_MoveStorageItem:"+id);
		pt_req_move_guild_item_d385 msg = new pt_req_move_guild_item_d385();
        msg.action = _action;
		msg.id = id;
        NetMsgMng.SendMsg(msg);


    }
	public void C2S_MemCheckOutItem(int itemID)
	{
		//Debug.Log("C2S_MemCheckOutItem:"+itemID);
		pt_normal_member_req_check_out_guild_item_d521 msg = new pt_normal_member_req_check_out_guild_item_d521();
		//normalMsg.action = _action;
		msg.item_id = itemID;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 操作玩家的审核
	/// </summary>
	/// <param name="uid">玩家ID</param>
	/// <param name="itemID">物品ID</param>
	/// <param name="action">操作类型</param>
	public void C2S_ApplyCheckOutItem(int uid,int itemID,int action)
	{
		//Debug.Log("C2S_ApplyCheckOutItem uid:"+uid+"itemID:"+itemID+",action:"+((action==1)?"同意":"拒绝"));
		pt_leader_req_check_out_item_action_d518 msg = new pt_leader_req_check_out_item_action_d518();
		msg.target_uid = uid;
		msg.item_id = itemID;
		msg.action_type = action;
		NetMsgMng.SendMsg(msg);
	}

	/// <summary>
	/// 请求仙盟仓库物品
	/// </summary>
	public void C2S_ReqStorageItemList()
	{
		//Debug.Log("C2S_ReqStorageItemList");
		pt_req_guild_items_d386 msg = new pt_req_guild_items_d386();
		NetMsgMng.SendMsg(msg);
	}

	public void C2S_ArrangeStorageItem()
	{
		//Debug.Log("C2S_ArrangeStorageItem");
		pt_leader_req_clear_guild_items_d520 msg = new pt_leader_req_clear_guild_items_d520();
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 请求取物品申请列表
	/// </summary>
	public void C2S_ReqItemApplyList()
	{
		pt_leader_req_guild_item_action_ask_list_d523 msg = new pt_leader_req_guild_item_action_ask_list_d523();
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 请求公会仓库日志
	/// </summary>
	public void C2S_ReqStorageLogList(int page)
	{
		//Debug.Log("C2S_ReqStorageLogList:"+page);
		pt_req_guild_items_log_d526 msg = new pt_req_guild_items_log_d526();
		msg.page = page;
		NetMsgMng.SendMsg(msg);
	}

    /// <summary>
    /// 职位变更目标ID，目标职位
    /// </summary>
    public void C2S_PositionChange(int id,int position)
    {
		//Debug.Log("C2S_PositionChange:"+id+"pos:"+position);
		pt_req_change_guild_member_d510 msg = new pt_req_change_guild_member_d510();
		msg.target_id = id;
		msg.pos = position;
        NetMsgMng.SendMsg(msg);
    }
	/// <summary>
	/// 请求公会日志
	/// </summary>
	public void C2S_ReqGuildLog(int _page)
	{
		//Debug.Log("C2S_ReqGuildLog:"+_page);
		pt_req_guild_log_d393 msg = new pt_req_guild_log_d393();
		msg.page = _page;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 请求领取俸禄
	/// </summary>
	public void C2S_ReqGetGuildSalary()
	{
		//Debug.Log("C2S_ReqGetGuildSalary");
		pt_req_guild_salary_d394 msg = new pt_req_guild_salary_d394();
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 仙盟捐献
	/// </summary>
	public void C2S_GuildDonate(int _id)
	{
        //Debug.Log("C2S_GuildDonate _id:" + _id);
		pt_req_contribute_to_guild_d507 msg = new pt_req_contribute_to_guild_d507();
        msg.type = (byte)_id;
		NetMsgMng.SendMsg(msg);
	}

    /// <summary>
    /// 请求仙盟捐献次数
    /// </summary>
    public void C2S_GuildDonateTimes()
    { 
        pt_req_guild_contribute_info_d50b msg = new pt_req_guild_contribute_info_d50b(); 
        NetMsgMng.SendMsg(msg);
    }

	public void C2S_ChangeGuildNotice(string notice)
	{
		//Debug.Log("C2S_ChangeGuildNotice :"+notice);
		if(!GameCenter.loginMng.CheckBadWord(notice))
		{
			return;
		}
		pt_leader_req_change_guild_purpose_d522 msg = new pt_leader_req_change_guild_purpose_d522();
		msg.purpose = notice;
		NetMsgMng.SendMsg(msg);
	}

	public void C2S_ExpandMemberCount(bool isQuickBuy)
	{
		//Debug.Log("C2S_ExpandMemberCount");
		pt_req_guild_expand_d816 msg = new pt_req_guild_expand_d816();
		if(myGuildInfo != null)
		{
			msg.guild_id = myGuildInfo.GuildId;
			msg.expand_num = myGuildInfo.MemberExpandTimes;
			msg.quick = isQuickBuy?1:0;
			NetMsgMng.SendMsg(msg);
		}else
		{
			Debug.LogError("myGuildInfo is null");
		}
	}

    public void C2S_ReqLivelyData()
    {
        //Debug.Log("C2S_ReqLivelyData");
        pt_req_guild_liveness_info_d50d msg = new pt_req_guild_liveness_info_d50d();
        NetMsgMng.SendMsg(msg);
    }

    public void C2S_ReqLivelyReward(int rewardId)
    {
        //Debug.Log("C2S_ReqLivelyReward");
        pt_req_guild_liveness_reward_d50f msg = new pt_req_guild_liveness_reward_d50f();
        msg.reward_id = (uint)rewardId;
        NetMsgMng.SendMsg(msg);
    }
    #endregion

    #endregion

}
/// <summary>
/// 公会成员职位
/// </summary>
public enum GuildMemPosition
{ 
    /// <summary>
    /// 成员
    /// </summary>
    MEMBER =0,
    /// <summary>
	/// 长老(官员)
    /// </summary>
    ELDER=1,
    /// <summary>
    /// 副会长
    /// </summary>
    VICECHAIRMAN=2,
    /// <summary>
    /// 会长
    /// </summary>
    CHAIRMAN=3,
}

public class GuildLivelyInfo
{
    protected st.net.NetBase.guild_liveness_task_info serverData = null;

    protected GuildLivelyRef refData = null;
    public GuildLivelyRef RefData
    {
        get
        {
            if (refData == null)
            {
                refData = ConfigMng.Instance.GetGuildlivelyRefByID(ID);
            }
            if (refData == null)
            {
                Debug.LogError("找不到ID为:" + ID + "的仙盟活跃数据!");
            }
            return refData;
        }
    }

    public int ID = 0;

    /// <summary>
    /// 名字
    /// </summary>
    public string Name
    {
        get
        {
            return RefData == null ? string.Empty : RefData.name;
        }
    }
    /// <summary>
    /// 描述
    /// </summary>
    public string Des
    {
        get
        {
            return RefData == null ? string.Empty : RefData.Des;
        }
    }
    /// <summary>
    /// 完成次数
    /// </summary>
    public int FinishTimes
    {
        get
        {
            return serverData == null ? 0 : (int)serverData.counter;
        }
    }
    /// <summary>
    /// 需求次数
    /// </summary>
    public int TotalTimes
    {
        get
        {
            if (RefData != null && RefData.task_condition.Count > 0)
            {
                return RefData.task_condition[0].number;
            }
            return 0;
        }
    }
    /// <summary>
    /// 已完成
    /// </summary>
    public bool IsFinished
    {
        get
        {
            return FinishTimes >= TotalTimes;
        }
    }
    /// <summary>
    /// 几级界面
    /// </summary>
    public int UISort
    {
        get
        {
            return RefData == null ? 0 : RefData.uinum;
        }
    }
    /// <summary>
    /// 界面枚举字符串
    /// </summary>
    public string UIType
    {
        get
        {
            return RefData == null ? string.Empty : RefData.guitype;
        }
    }

    /// <summary>
    /// 完成该项可获得的活跃度值
    /// </summary>
    public int StaticLivelyCount
    {
        get
        {
            return RefData == null ? 0 : RefData.rewardlive;
        }
    }
    /// <summary>
    /// 该项每日必做贡献的活跃度值,没完成及没贡献
    /// </summary>
    public int CurLivelyCount
    {
        get
        {
            if (IsFinished)
            {
                return StaticLivelyCount;
            }
            return 0;
        }
    }

    /// <summary>
    /// 没有后台数据,暂用
    /// </summary>
    public GuildLivelyInfo(int _mustDoID)
    {
        ID = _mustDoID;
    }
    public GuildLivelyInfo(st.net.NetBase.guild_liveness_task_info _data)
    {
        serverData = _data;
        ID = (int)serverData.task_id;
    }
    public GuildLivelyInfo(GuildLivelyInfo _info)
    {
        serverData = _info.serverData;
        ID = _info.ID;
    }
}

