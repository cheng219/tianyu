//===============================
//作者：邓成
//日期：2016/7/7
//用途：副本管理类
//===============================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;

public class DungeonMng {

	 #region 数据

    protected int dungeonTime;
    /// <summary>
    /// 副本中，战斗剩余时间
    /// </summary>
    public int DungeonTime
    {
        get { return dungeonTime; }
        set { dungeonTime = value; }
    }

	protected int bridgeNpcID = 0;
	/// <summary>
	/// 断魂桥的NPC
	/// </summary>
	public int BridgeNpcID
	{
		get
		{
			return bridgeNpcID;
		}
	}
	public System.Action OnBridgeNPCUpdateEvent;
	protected List<call_boss_list> bridgeBossList = new List<call_boss_list>();
	/// <summary>
	/// 断魂桥能召唤的BOSS
	/// </summary>
	public List<call_boss_list> BridgeBossList
	{
		get{return bridgeBossList;}
		set{bridgeBossList = value;}
	}
	public System.Action OnBridgeBossListUpdateEvent;

	public int DungeonStartTime = 0;
    /// <summary>
    /// 副本中，战斗剩余时间变化的事件
    /// </summary>
    public System.Action OnDungeonTimeUpdate;

    protected int dungeonWave;
    /// <summary>
    /// 经验试炼中的怪物波数
    /// </summary>
    public int DungeonWave
    {
		get { return dungeonWave; }
		set { dungeonWave = value; }
    }

	protected int dungeonMaxWave;
	/// <summary>
	/// 经验试炼中的怪物最大波数
	/// </summary>
	public int DungeonMaxWave
	{
		get { return dungeonMaxWave; }
		set { dungeonMaxWave = value; }
	}

    /// <summary>
    ///波数变化的事件
    /// </summary>
    public System.Action OnDGWaveUpdate;

	public int DungeonLayer = 0;
	public int DungeonMaxLayer = 0;
    public int totalSatr = 0;
    public int totalDropInteger = 0;
	/// <summary>
	//层数变化的事件
	/// </summary>
	public System.Action OnDGLayerUpdate;
	/// <summary>
	/// 镇魔塔层数的奖励领取状态
	/// </summary>
	public bool HaveGotTowerReward = false;
	public System.Action OnTowerRewardStateEvent;
    /// <summary>
    /// 累计星星总数变化
    /// </summary>
    public System.Action OnTowerTotalStarEvent;
    /// <summary>
    /// 累计掉落奖励积分
    /// </summary>
    public System.Action OnTowerTotalDropEvent;
	public System.Action OnStarTimeUpdateEvent;

	public int DungeonMonsterNum = 0;
	/// <summary>
	//剩余怪物数变化的事件
	/// </summary>
	public System.Action OnDGMonsterUpdate;

	public int DungeonPetTime;
	public string DungeonPetName;
	/// <summary>
	//丁丁被烤熟时间变化的事件
	/// </summary>
	public System.Action OnDGPetTimeUpdate;

	public int DungeonReliveTimes;
	/// <summary>
	//复活次数变化的事件
	/// </summary>
	public System.Action OnDGReliveTimesUpdate;

	public List<boss_count> DungeonKillMonster = new List<boss_count>();
	/// <summary>
	//杀怪变化的事件
	/// </summary>
	public System.Action OnDGKillMonsterUpdate;
	/// <summary>
	/// 最大连斩
	/// </summary>
	public int MaxEvenKill = 0;
	/// <summary>
	/// 当前最大连斩
	/// </summary>
	protected int CurMaxEvenKill = 0;
	/// <summary>
	/// 当前连斩
	/// </summary>
	public int CurEvenKill = 0;
	/// <summary>
	/// 击杀间隔时间
	/// </summary>
	protected int killDiffTime = 0;
	protected int previousTime = 0;

	public int CurDGScore = 0;
	public int MaxDGScore = 0;
	/// <summary>
	//分数变化的事件
	/// </summary>
	public System.Action OnDGScoreUpdate;

	public int DungeonDeadTimes = 0;
	public int DungeonMaxDeadTimes = 0;
	/// <summary>
	//死亡次数变化的事件
	/// </summary>
	public System.Action OnDGDeadTimesUpdate;
	/// <summary>
	/// 公会篝火获得的经验
	/// </summary>
	public int GuildFireExp = 0;
	public int GuildFireExpPercent = 0;
	/// <summary>
	/// 篝火中的晶石HP
	/// </summary>
	public int GuildFireStoneCurHp = 0;
	public int GuildFireStoneMaxHp = 0;
	public string GuildFireCurGuildName = string.Empty;
	public System.Action OnGuildFireUpdateEvent;
	public System.Action OnGuildFireNameUpdateEvent;

	protected int guildProtectResult;
    /// <summary>
    /// 用来记录副本结果：1为胜利0为失败
    /// </summary>
    public int GuildProtectResult
    {
		get { return guildProtectResult; }
		set { guildProtectResult = value; }
    }
    /// <summary>
    /// 仙盟守护结果
    /// </summary>
	public System.Action OnGuildProtectResultUpdate;

	#region 仙盟城战
	public int WestStoneID = 0;
	public int EastStoneID = 0;
	public int NorthStoneID = 0;
	public GuildSiegeState guildSiegeState = GuildSiegeState.Fighting;
	public int GuildSiegeWaitTime = 0;
	public System.Action<int,int,int> OnGuildSiegeStoneUpdate;
	public System.Action OnGuildSiegeStateUpdate;
	#endregion

    #region  仙侣副本
    protected int coupleId = 0;
    /// <summary>
    /// 仙侣副本诗句id
    /// </summary>
    public int coupleCopyId
    {
        get
        {
            return coupleId;
        }
        protected set
        {
            if (coupleId != value)
            {
                coupleId = value;
                if (OnGetCouplePoemId != null)
                    OnGetCouplePoemId();
            }
        }
    }
    public string coupleReplaceChar = string.Empty; 
    /// <summary>
    /// 仙侣集齐诗句任务变化事件
    /// </summary>
    public System.Action OnCoupleCopyUpdate;
    public System.Action OnGetCouplePoemId;
    #endregion

     #endregion

    #region 构造
    /// <summary>
    /// 返回一个全新的管理类对象实例
    /// </summary>
    /// <returns></returns>
    public static DungeonMng CreateNew()
    {
        if (GameCenter.dungeonMng == null)
        {
            DungeonMng dungeonMng = new DungeonMng();
            dungeonMng.Init();
            return dungeonMng;
        }
        else
        {
            GameCenter.dungeonMng.UnRegist();
            GameCenter.dungeonMng.Init();
            return GameCenter.dungeonMng;
        }
    }


    /// <summary>
    /// 初始化（包含协议注册）
    /// </summary>
    protected void Init()
    {
        MsgHander.Regist(0xD492, S2C_GotDGTime);
		MsgHander.Regist(0xD493, S2C_GotDGWaveNum);
		MsgHander.Regist(0xD497, S2C_GotDGLayerNum);
		MsgHander.Regist(0xD496, S2C_GotDGMonsterNum);
		MsgHander.Regist(0xD498, S2C_GotDGPetTime);
		MsgHander.Regist(0xD499, S2C_GotDGReliveTimes);
		MsgHander.Regist(0xD700, S2C_GotDGKillMonster);
		MsgHander.Regist(0xD724, S2C_GotDGScore);
		MsgHander.Regist(0xD734,S2C_GotDGDeadTimes);

		MsgHander.Regist(0xD772, S2C_GotTowerRewardState);
		MsgHander.Regist(0xD495,S2C_GotBridgeBossCallInfo);
		MsgHander.Regist(0xD768,S2C_GotGuildFireExp);
		MsgHander.Regist(0xD769,S2C_GotGuildFireHp);
		MsgHander.Regist(0xD770,S2C_GotGuildFireExpPercent);
		MsgHander.Regist(0xD728,S2C_GotGuildProtectResult);
		MsgHander.Regist(0xD780,S2C_GotBridgeNpcID);
		MsgHander.Regist(0xD781,S2C_GotGuildSiegeState);
		MsgHander.Regist(0xD782,S2C_GotGuildSiegeStone);
        MsgHander.Regist(0xD791, S2C_GetCouplePoemId);
        MsgHander.Regist(0xD792, S2C_GetCouplePoem);
        MsgHander.Regist(0xC144, S2C_GetAllSatr);
        MsgHander.Regist(0xC145, S2C_GetDropRewardInteger);
    }
    /// <summary>
    /// 注销（包含清空和还原数据）
    /// </summary>
    protected void UnRegist()
    {
        MsgHander.UnRegist(0xD492, S2C_GotDGTime);
		MsgHander.UnRegist(0xD493, S2C_GotDGWaveNum);
		MsgHander.UnRegist(0xD497, S2C_GotDGLayerNum);
		MsgHander.UnRegist(0xD496, S2C_GotDGMonsterNum);
		MsgHander.UnRegist(0xD498, S2C_GotDGPetTime);
		MsgHander.UnRegist(0xD499, S2C_GotDGReliveTimes);
		MsgHander.UnRegist(0xD700, S2C_GotDGKillMonster);
		MsgHander.UnRegist(0xD724, S2C_GotDGScore);
		MsgHander.UnRegist(0xD734,S2C_GotDGDeadTimes);

		MsgHander.UnRegist(0xD772, S2C_GotTowerRewardState);
		MsgHander.UnRegist(0xD495,S2C_GotBridgeBossCallInfo);
		MsgHander.UnRegist(0xD768,S2C_GotGuildFireExp);
		MsgHander.UnRegist(0xD769,S2C_GotGuildFireHp);
		MsgHander.UnRegist(0xD770,S2C_GotGuildFireExpPercent);
		MsgHander.UnRegist(0xD728,S2C_GotGuildProtectResult);
		MsgHander.UnRegist(0xD780,S2C_GotBridgeNpcID);
		MsgHander.UnRegist(0xD781,S2C_GotGuildSiegeState);
		MsgHander.UnRegist(0xD782,S2C_GotGuildSiegeStone);
        MsgHander.UnRegist(0xD791, S2C_GetCouplePoemId);
        MsgHander.UnRegist(0xD792, S2C_GetCouplePoem);
        MsgHander.UnRegist(0xC144, S2C_GetAllSatr);
        MsgHander.UnRegist(0xC145, S2C_GetDropRewardInteger);
		ClearData();
    }

	void ClearData()
	{
		dungeonTime = 0;
		bridgeNpcID = 0;
		bridgeBossList.Clear();
		DungeonStartTime = 0;
		dungeonWave = 0;
		dungeonMaxWave = 0;
		DungeonLayer = 0;
		DungeonMaxLayer = 0;
		HaveGotTowerReward = false;
		DungeonMonsterNum = 0;
		DungeonPetTime = 0;
		DungeonReliveTimes = 0;
		MaxEvenKill = 0;
		CurEvenKill = 0;
		killDiffTime = 0;
		previousTime = 0;
		CurDGScore = 0;
		MaxDGScore = 0;
		DungeonDeadTimes = 0;
		DungeonMaxDeadTimes = 0;
		GuildFireExp = 0;
		GuildFireExpPercent = 0;
		GuildFireStoneCurHp = 0;
		GuildFireStoneMaxHp = 0;
		GuildFireCurGuildName = string.Empty;
		DungeonKillMonster.Clear();
		WestStoneID = 0;
		EastStoneID = 0;
		NorthStoneID = 0;
		guildSiegeState = GuildSiegeState.Fighting;
		GuildSiegeWaitTime = 0;
        totalDropInteger = 0;
        totalSatr = 0;
	}
    #endregion


    #region 协议
    #region S2C 服务端发往客户端的协议处理

    /// <summary>
	/// 服务端返回： 副本中，战斗剩余时间 by邓成
    /// </summary>
	protected void S2C_GotDGTime(Pt _info)
    {
		pt_update_copy_time_d492 pt = _info as pt_update_copy_time_d492;
        if (pt != null)
        {
			//Debug.Log("S2C_GotDGTime=============" + (int)pt.copy_time);
			dungeonTime = (int)pt.copy_time;
			DungeonStartTime = (int)Time.realtimeSinceStartup;
            if (OnDungeonTimeUpdate != null)
            {
                OnDungeonTimeUpdate();
            }
        }
    }
    /// <summary>
    /// 服务端返回： 怪物波数  by邓成
    /// </summary>
    public void S2C_GotDGWaveNum(Pt _info)
    {
		pt_update_monster_wave_num_d493 pt = _info as pt_update_monster_wave_num_d493;
        if (pt != null)
        {
			//Debug.Log("S2C_GotDGWaveNum=============" + (int)pt.wave_num);
			dungeonWave = (int)pt.wave_num;
			dungeonMaxWave = pt.max_wave;
			if(GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType == SceneUiType.TOWER)
			{
				DungeonStartTime = (int)Time.realtimeSinceStartup;//镇魔塔三星时间重置
				if(OnStarTimeUpdateEvent != null)
					OnStarTimeUpdateEvent();
			}
            if (OnDGWaveUpdate != null)
            {
                OnDGWaveUpdate();
            }
        }
    }
   	/// <summary>
   	/// 挑战层数  by邓成
   	/// </summary>
	public void S2C_GotDGLayerNum(Pt _info)
	{
		pt_update_copy_tier_d497 pt = _info as pt_update_copy_tier_d497;
		if(pt != null)
		{
			//Debug.Log("S2C_GotDGLayerNum=============" + (int)pt.tier);
			DungeonLayer = pt.tier;
			DungeonMaxLayer = pt.max_tier;
			if(OnDGLayerUpdate != null)
				OnDGLayerUpdate();
		}
	}

	/// <summary>
	/// 剩余怪物数  by邓成
	/// </summary>
	public void S2C_GotDGMonsterNum(Pt _info)
	{
		pt_update_monster_die_num_d496 pt = _info as pt_update_monster_die_num_d496;
		if(pt != null)
		{
			//Debug.Log("S2C_GotDGMonsterNum=============" + (int)pt.monster_num);
			DungeonMonsterNum = pt.monster_num;
			if(OnDGMonsterUpdate != null)
				OnDGMonsterUpdate();
		}
	}
	/// <summary>
	/// 丁丁被烤熟时间
	/// </summary>
	/// <param name="_info">Info.</param>
	public void S2C_GotDGPetTime(Pt _info)
	{
		pt_update_ding_rescue_time_d498 pt = _info as pt_update_ding_rescue_time_d498;
		if(pt != null)
		{
			//Debug.Log("S2C_GotDGPetTime=============" + (int)pt.rescue_time+",name:"+pt.dd_name);
			DungeonPetTime = pt.rescue_time;
			DungeonPetName = pt.dd_name;
			if(OnDGPetTimeUpdate != null)
				OnDGPetTimeUpdate();
		}
	}
	/// <summary>
	/// 副本复活次数 by邓成
	/// </summary>
	public void S2C_GotDGReliveTimes(Pt _info)
	{
		pt_update_copy_revive_num_d499 pt = _info as pt_update_copy_revive_num_d499;
		if(pt != null)
		{
			//Debug.Log("S2C_GotDGReliveTimes=============" + (int)pt.revive_num);
			DungeonReliveTimes = pt.revive_num;
			if(OnDGReliveTimesUpdate != null)
				OnDGReliveTimesUpdate();
		}
	}
	/// <summary>
	/// 副本死亡次数
	/// </summary>
	/// <param name="_info">Info.</param>
	public void S2C_GotDGDeadTimes(Pt _info)
	{
		pt_update_die_num_d734 pt = _info as pt_update_die_num_d734;
		if(pt != null)
		{
			//Debug.Log("S2C_GotDGDeadTimes===:" + (int)pt.cur_die_num+","+pt.max_die_num);
			DungeonDeadTimes = pt.cur_die_num;
			DungeonMaxDeadTimes = pt.max_die_num;
			if(OnDGDeadTimesUpdate != null)
				OnDGDeadTimesUpdate();
		}
	}

	/// <summary>
	/// 副本杀怪名字和数量及总数 by邓成
	/// </summary>
	public void S2C_GotDGKillMonster(Pt _info)
	{
		pt_update_copy_boss_count_d700 pt = _info as pt_update_copy_boss_count_d700;
		if(pt != null)
		{
			//Debug.Log("S2C_GotDGKillMonster=============" + (int)pt.boss_count.Count);
			DungeonKillMonster = pt.boss_count;
			if(DungeonKillMonster.Count > 0 && DungeonKillMonster[0].cur_num > 0)
			{
				killDiffTime = (int)Time.realtimeSinceStartup - previousTime;
				//Debug.Log("killDiffTime:"+killDiffTime+",previousTime:"+previousTime+",pt.boss_count:"+DungeonKillMonster[0].cur_num);
				if(killDiffTime < 30)
				{
					CurEvenKill += 1;
					if(CurMaxEvenKill <= CurEvenKill)
						CurMaxEvenKill = CurEvenKill;
				}else
				{
					if(previousTime == 0)
					{
						CurEvenKill = 1;//杀第一只怪的时候,且游戏时间大于30s
						CurMaxEvenKill = CurEvenKill;
					}else//重置
					{
						CurEvenKill = 1;
						CurMaxEvenKill = 1;	
					}
				}
				if(MaxEvenKill <= CurMaxEvenKill)
					MaxEvenKill = CurMaxEvenKill;
				previousTime = (int)Time.realtimeSinceStartup;
			}
			if(OnDGKillMonsterUpdate != null)
				OnDGKillMonsterUpdate();
		}
	}
	/// <summary>
	/// 断魂桥召唤助手信息
	/// </summary>
	public void S2C_GotBridgeBossCallInfo(Pt _info)
	{
		pt_update_call_boss_d495 pt = _info as pt_update_call_boss_d495;
		if(pt != null)
		{
			BridgeBossList = pt.call_boss_list;
			//Debug.Log("S2C_GotBridgeBossCallInfo:"+BridgeBossList.Count);
			if(OnBridgeBossListUpdateEvent != null)
				OnBridgeBossListUpdateEvent();
		}
	}

	protected void S2C_GotBridgeNpcID(Pt _info)
	{
		pt_update_guard_hp_d780 pt = _info as pt_update_guard_hp_d780;
		if(pt != null)
		{
			//Debug.Log("S2C_GotBridgeNpcID:"+pt.guard_npc_id);
			bridgeNpcID = pt.guard_npc_id;
			if(OnBridgeNPCUpdateEvent != null)
				OnBridgeNPCUpdateEvent();
		}
	}

	/// <summary>
	/// 副本分数 by邓成
	/// </summary>
	public void S2C_GotDGScore(Pt _info)
	{
		pt_update_copy_score_d724 pt = _info as pt_update_copy_score_d724;
		if(pt != null)
		{
			//Debug.Log("S2C_GotDGScore=============" + (int)pt.cur_score);
			CurDGScore = pt.cur_score;
			MaxDGScore = pt.max_score;
			if(OnDGScoreUpdate != null)
				OnDGScoreUpdate();
		}
	}
	/// <summary>
	/// 获取镇魔塔当前层数的奖励领取状态
	/// </summary>
	public void S2C_GotTowerRewardState(Pt _info)
	{
		pt_update_quell_demon_tier_reward_d772 pt = _info as pt_update_quell_demon_tier_reward_d772;
		if(pt != null)
		{
			//Debug.Log("S2C_GotTowerRewardState state:"+HaveGotTowerReward);
			HaveGotTowerReward = (pt.reward_state == 1);
			if(OnTowerRewardStateEvent != null)
				OnTowerRewardStateEvent();
		}
	}

	public void S2C_GotGuildFireExp(Pt _info)
	{
		pt_update_bonfire_exp_d768 pt = _info as pt_update_bonfire_exp_d768;
		if(pt != null)
		{
			//Debug.Log("S2C_GotGuildFireExp=============" + (int)pt.amount_exp);
			GuildFireExp = pt.amount_exp;
			if(OnGuildFireUpdateEvent != null)
				OnGuildFireUpdateEvent();
		}
	}
	public void S2C_GotGuildFireHp(Pt _info)
	{
		pt_update_jingshi_hp_d769 pt = _info as pt_update_jingshi_hp_d769;
		if(pt != null)
		{
			//Debug.Log("S2C_GotGuildFireHp=============" + pt.cur_guild_name);
			GuildFireStoneCurHp = pt.cur_jingshi_hp;
			GuildFireStoneMaxHp = pt.max_jingshi_hp;
			string oldGuildName = GuildFireCurGuildName;
			GuildFireCurGuildName = pt.cur_guild_name;
			if(OnGuildFireNameUpdateEvent != null && !GuildFireCurGuildName.Equals(oldGuildName))//防止更新血量的时候也抛事件
				OnGuildFireNameUpdateEvent();
			if(OnGuildFireUpdateEvent != null)
				OnGuildFireUpdateEvent();
		}
	}
	public void S2C_GotGuildFireExpPercent(Pt _info)
	{
		pt_update_jingshi_exp_addition_d770 pt = _info as pt_update_jingshi_exp_addition_d770;
		if(pt != null)
		{
			//Debug.Log("S2C_GotGuildFireExpPercent=============" + (int)pt.exp_addition);
			GuildFireExpPercent = pt.exp_addition;
			if(OnGuildFireUpdateEvent != null)
				OnGuildFireUpdateEvent();
		}
	}

	protected void S2C_GotGuildProtectResult(Pt _info)
	{
		//Debug.Log("S2C_GotGuildProtectResult");
		pt_activity_game_over_d728 pt = _info as pt_activity_game_over_d728;
		if(pt != null)
		{
			GuildProtectResult = pt.state;
			if(OnGuildProtectResultUpdate != null)
				OnGuildProtectResultUpdate();
		}
	}

	protected void S2C_GotGuildSiegeStone(Pt _info)
	{
		pt_guild_storm_city_shuijing_d782 pt = _info as pt_guild_storm_city_shuijing_d782;
		if(pt != null)
		{
			//Debug.Log("S2C_GotGuildSiegeStone:"+pt.shuijing_type+","+pt.cur_hp+","+pt.max_hp);
			if(OnGuildSiegeStoneUpdate != null)
				OnGuildSiegeStoneUpdate(pt.shuijing_type,pt.cur_hp,pt.max_hp);
		}
	}

	protected void S2C_GotGuildSiegeState(Pt _info)
	{
		
		pt_guild_storm_city_portal_d781 pt = _info as pt_guild_storm_city_portal_d781;
		if(pt != null)
		{
			guildSiegeState = (GuildSiegeState)pt.start_state;
			GuildSiegeWaitTime = pt.surplus_time + (int)Time.realtimeSinceStartup;
			//Debug.Log("S2C_GotGuildSiegeState guildSiegeState:"+guildSiegeState+",pt.surplus_time:"+pt.surplus_time);
			if(OnGuildSiegeStateUpdate != null)
				OnGuildSiegeStateUpdate();
		}
	}

    protected void S2C_GetCouplePoem(Pt _msg)
    {
        pt_updata_poetry_d792 msg = _msg as pt_updata_poetry_d792;
        if (msg != null)
        {
            //Debug.Log("d792 coupleChar :   " + msg.poetry);
            coupleReplaceChar = msg.poetry;
            if (OnCoupleCopyUpdate != null) OnCoupleCopyUpdate();
        }
    }

    protected void S2C_GetCouplePoemId(Pt _msg)
    {
        pt_update_companion_copy_id_d791 msg = _msg as pt_update_companion_copy_id_d791;
        if (msg != null)
        {
            //Debug.Log("d791 coupleId :   " + msg.id);
            coupleCopyId = msg.id; 
        }
    }
    /// <summary>
    /// 镇魔塔累计获取的星星数
    /// </summary> 
    protected void S2C_GetAllSatr(Pt _msg)
    { 
        pt_update_quell_demon_star_c144 msg = _msg as pt_update_quell_demon_star_c144;
        if (msg != null)
        {
            totalSatr = msg.star_num;
            if (OnTowerTotalStarEvent != null) OnTowerTotalStarEvent();
        }
    }
    /// <summary>
    /// 镇魔塔累计获取的掉落奖励积分
    /// </summary> 
    protected void S2C_GetDropRewardInteger(Pt _msg)
    {
        pt_update_quell_demon_drop_out_c145 msg = _msg as pt_update_quell_demon_drop_out_c145;
        if (msg != null)
        {
            totalDropInteger = msg.drop_out_num;
            if (OnTowerTotalDropEvent != null) OnTowerTotalDropEvent();
        }
    }
    #endregion

    #region  C2S 客户端发往服务端的协议

    #region 副本匹配
    /// <summary>
    /// 请求匹配
    /// </summary>
    public void C2S_RequestMatch(int _id)
    {
        //Debug.Log("请求匹配" + _id);
        pt_req_match_d214 msg = new pt_req_match_d214();
        msg.id = (uint)_id;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求：在匹配队列中（副本选择界面）取消匹配
    /// </summary>
    public void C2S_MatchQueueCancle(int _id)
    {
        pt_action_int_d003 msg = new pt_action_int_d003();
        msg.action = 21;
        //Debug.Log("难度ID=========" + _id);
        msg.data = _id;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求：在匹配界面中等待进入副本时 取消匹配
    /// </summary>
    public void C2S_MatchReadyWndCancle()
    {
        //Debug.Log("取消匹配");
        pt_action_d002 msg = new pt_action_d002();
        msg.action = 22;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求：在匹配界面中等待进入副本时 点击准备
    /// </summary>
    public void C2S_MatchReadyWndReady()
    {
        //Debug.Log("请求：在匹配界面中等待进入副本时 点击准备");
        pt_action_d002 msg = new pt_action_d002();
        msg.action = 23;
        NetMsgMng.SendMsg(msg);
    }

    #endregion
	/// <summary>
	/// 请求断魂桥的BOSS支援
	/// </summary>
	public void C2S_ReqCallBridgeBoss(int bossId)
	{
		//Debug.Log("C2S_ReqCallBridgeBoss");
		pt_req_call_boss_d494 msg = new pt_req_call_boss_d494();
		msg.boss_id = bossId;
		NetMsgMng.SendMsg(msg);
	}
    #endregion
    #endregion

}
/// <summary>
/// 攻城战副本进行状态
/// </summary>
public enum GuildSiegeState
{
    /// <summary>
	/// 战斗中(传送门未开启)
    /// </summary>
    Fighting = 0,
    /// <summary>
    /// 传送门已开启
    /// </summary>
    Open = 1,
    /// <summary>
	/// 传送门已关闭
    /// </summary>
    Close = 2,
}

public enum DungeonType
{
	NONE,
	/// <summary>
	/// 断魂桥
	/// </summary>
	BRIDGE = 1,
	/// <summary>
	/// 无量圣地
	/// </summary>
	HOLYLAND = 2,
	/// <summary>
	/// 灵兽岛
	/// </summary>
	PETLAND = 3,
	/// <summary>
	/// 死亡荒漠
	/// </summary>
	DESERT = 4,
	/// <summary>
	/// 寒冰炼狱
	/// </summary>
	ICECOPPY = 5,
	/// <summary>
	/// 无尽
	/// </summary>
	ENDLESS = 6,
	/// <summary>
	/// 竞技场
	/// </summary>
	ARENA = 7,
	/// <summary>
	/// 镇魔塔
	/// </summary>
	TOWER = 8,
	/// <summary>
	/// 仙侣副本
	/// </summary>
	XIANLV = 9,
}

