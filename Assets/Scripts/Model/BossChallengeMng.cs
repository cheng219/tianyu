//===============================
//作者：邓成
//日期：2016/4/27
//用途：挑战BOSS管理类
//===============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class BossChallengeMng {

	/// <summary>
	/// BOSS列表
	/// </summary>
	public Dictionary<int,BossChallengeData> BossChallengeDic = new Dictionary<int, BossChallengeData>();
	/// <summary>
	/// 获取到BOSS列表的事件
	/// </summary>
	public System.Action OnGotChallengListEvent;

    public System.Action<BossChallengeData> OnBossReliveEvent;
    /// <summary>
    /// 挑战里火云洞BOSS不再提示
    /// </summary>
    public bool FightLiRongEBossNoTip = false;

    /// <summary>
    /// 剩余挑战boss副本的次数
    /// </summary>
    public int ChallengeBossCoppyTimes = 0;
    /// <summary>
    /// 剩余购买boss副本的次数
    /// </summary>
    public int RemainBuyBossCoppyTimes = 0;
    /// <summary>
    /// 剩余BOSS数量
    /// </summary>
    public int RemainBossCount = 0;
    /// <summary>
    /// Boss副本增加的属性百分比
    /// </summary>
    public int CurBossCoppyAttrNum = 0;

    public List<st.net.NetBase.boss_copy_list> bossList = new List<boss_copy_list>();

    public System.Action OnBossCoppyDataUpdateEvent;

    /// <summary>
    /// BOSS复活了
    /// </summary>
    /// <param name="_bossChallengeData"></param>
    public void BossRelive(BossChallengeData _bossChallengeData)
    {
        if (OnBossReliveEvent != null) OnBossReliveEvent(_bossChallengeData);
    }

	/// <summary>
	/// 可击杀的BOSS列表
	/// </summary>
	public List<BossChallengeData> CanKillBossList
	{
		get
		{
			List<BossChallengeData> list = new List<BossChallengeData>();
			using(var e = BossChallengeDic.GetEnumerator())
			{
				while(e.MoveNext())
				{
					BossChallengeData boss = e.Current.Value;
					if(boss.CanKill)list.Add(boss);
				}
			}
			return list;
		}
	}
	/// <summary>
	/// 熔恶BOSS列表
	/// </summary>
	/// <value>The rong E boss list.</value>
	public List<BossChallengeData> RongEBossList
	{
		get
		{
			List<BossChallengeData> list = new List<BossChallengeData>();
			using(var e = BossChallengeDic.GetEnumerator())
			{
				while(e.MoveNext())
				{
					BossChallengeData boss = e.Current.Value;
					if(boss.CurBossRef != null && boss.CurBossRef.type == (int)BossChallengeWnd.ToggleType.RongEBoss)
						list.Add(boss);
				}
			}
			list.Sort(CompareByAppearTime);
			return list;
		}
	}

	/// <summary>
	/// 里熔恶BOSS列表
	/// </summary>
	/// <value>The rong E boss list.</value>
	public List<BossChallengeData> LiRongEBossList
	{
		get
		{
			List<BossChallengeData> list = new List<BossChallengeData>();
			using(var e = BossChallengeDic.GetEnumerator())
			{
				while(e.MoveNext())
				{
					BossChallengeData boss = e.Current.Value;
					if(boss.CurBossRef != null && boss.CurBossRef.type == (int)BossChallengeWnd.ToggleType.LiRongEBoss)
						list.Add(boss);
				}
			}
			list.Sort(CompareByAppearTime);
			return list;
		}
	}

    /// <summary>
    /// 场景BOSS列表
    /// </summary>
    /// <value>The rong E boss list.</value>
    public List<BossChallengeData> SceneBossList
    {
        get
        {
            List<BossChallengeData> list = new List<BossChallengeData>();
            using (var e = BossChallengeDic.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    BossChallengeData boss = e.Current.Value;
                    if (boss.CurBossRef != null && boss.CurBossRef.type == (int)BossChallengeWnd.ToggleType.SceneBoss)
                        list.Add(boss);
                }
            }
            list.Sort(CompareByAppearTime);
            return list;
        }
    }

	int CompareByAppearTime(BossChallengeData boss1,BossChallengeData boss2)
	{
		if(boss1.AppearTime > (int)Time.realtimeSinceStartup && boss2.AppearTime > (int)Time.realtimeSinceStartup)//未出现
		{
			if(boss1.CurBossRef.needLevel > boss2.CurBossRef.needLevel)
				return 1;
			if(boss1.CurBossRef.needLevel < boss2.CurBossRef.needLevel)
				return -1;
		}else if(boss1.AppearTime <= (int)Time.realtimeSinceStartup && boss2.AppearTime <= (int)Time.realtimeSinceStartup)//已出现的BOSS按等级排序
		{
			if(boss1.CurBossRef.needLevel > boss2.CurBossRef.needLevel)
				return 1;
			if(boss1.CurBossRef.needLevel < boss2.CurBossRef.needLevel)
				return -1;
		}else//已出现和未出现比较
		{
			if(boss1.AppearTime <= (int)Time.realtimeSinceStartup)
				return -1;
			if(boss2.AppearTime <= (int)Time.realtimeSinceStartup)
				return 1;
		}
		return 0;
	}


	public static BossChallengeMng CreateNew()
	{
		if(GameCenter.bossChallengeMng == null)
		{
			GameCenter.bossChallengeMng = new BossChallengeMng();
			GameCenter.bossChallengeMng.Init();
			return GameCenter.bossChallengeMng;
		}else
		{
			GameCenter.bossChallengeMng.UnRegist();
			GameCenter.bossChallengeMng.Init();
			return GameCenter.bossChallengeMng;
		}
	}
	void Init()
	{
		MsgHander.Regist(0xD701,S2C_GotBossList);
        MsgHander.Regist(0xC138, S2C_OnGotBossCopyWndData);
        MsgHander.Regist(0xC139, S2C_OnGotBossCopyData);
	}
	void UnRegist()
	{
		MsgHander.UnRegist(0xD701,S2C_GotBossList);
        MsgHander.UnRegist(0xC138, S2C_OnGotBossCopyWndData);
        MsgHander.UnRegist(0xC139, S2C_OnGotBossCopyData);
		BossChallengeDic.Clear();
        RemainBossCount = 0;
        RemainBuyBossCoppyTimes = 0;
        ChallengeBossCoppyTimes = 0;
        CurBossCoppyAttrNum = 0;
        bossList.Clear();
	}
	#region S2C
	/// <summary>
	/// BOSS列表获取协议
	/// </summary>
	protected void S2C_GotBossList(Pt _info)
	{
		//Debug.Log("S2C_GotBossList");
		bool noUnderTip = true;
		bool noSceneTip = true;
		bool noSealTip = true;
		bool noRongETip = true;
		bool noLiRongETip = true;
		pt_boss_challenge_list_d701 pt = _info as pt_boss_challenge_list_d701;
		if(pt != null)
		{
			BossChallengeDic.Clear();
			for (int i = 0,max=pt.underground_palace_boss.Count; i < max; i++) {
				BossChallengeData data = new BossChallengeData(pt.underground_palace_boss[i]);
				BossChallengeDic[pt.underground_palace_boss[i].boss_id] = data;
				if(data.CanKill)
				{
					GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.UNDERBOSS,true);
					noUnderTip = false;
				}
			}
			if(noUnderTip)
				GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.UNDERBOSS,false);
			for (int i = 0,max=pt.scene_boss.Count; i < max; i++) {
				BossChallengeData data = new BossChallengeData(pt.scene_boss[i]);
				BossChallengeDic[pt.scene_boss[i].boss_id] = data;
				if(data.CanKill)
				{
					GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.SCENEBOSS,data.CanKill);
					noSceneTip = false;
				}
			}
			if(noSceneTip)
				GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.SCENEBOSS,false);
			for (int i = 0,max=pt.seal_boss.Count; i < max; i++) {
				BossChallengeData data = new BossChallengeData(pt.seal_boss[i]);
				BossChallengeDic[pt.seal_boss[i].boss_id] = data;
				if(data.CanKill && data.CurBossRef.needLevel <= GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel && data.AppearTime <= (int)Time.realtimeSinceStartup)
				{
					GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.SEALBOSS,true);
					noSealTip = false;
				}
			}
			if(noSealTip)
				GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.SEALBOSS,false);
            //for (int i = 0,max=pt.smeltters_boss.Count; i < max; i++) {
            //    BossChallengeData data = new BossChallengeData(pt.smeltters_boss[i]);
            //    BossChallengeDic[pt.smeltters_boss[i].boss_id] = data;
            //    if(data.CanKill)
            //    {
            //        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.RONGEBOSS,true);
            //        noRongETip = false;
            //    }
            //}
            //if(noRongETip)
            //    GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.RONGEBOSS,false);
			for (int i = 0,max=pt.li_smeltters_boss.Count; i < max; i++) {
				BossChallengeData data = new BossChallengeData(pt.li_smeltters_boss[i]);
				BossChallengeDic[pt.li_smeltters_boss[i].boss_id] = data;
				if(data.CanKill)
				{
					GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.LIRONGEBOSS,true);
					noLiRongETip = false;
				}
			}
			if(noLiRongETip)
				GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.LIRONGEBOSS,false);
		}
		if(OnGotChallengListEvent != null)
			OnGotChallengListEvent();
	}

    protected void S2C_OnGotBossCopyWndData(Pt _pt)
    {
        //Debug.Log("S2C_OnGotBossCopyWndData");
        pt_boss_copy_ui_info_c138 info = _pt as pt_boss_copy_ui_info_c138;
        if (info != null)
        {
            ChallengeBossCoppyTimes = info.challenge_num;
            RemainBuyBossCoppyTimes = info.surplus_buy_num;
            GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.RONGEBOSS, ChallengeBossCoppyTimes > 0);
            if (OnBossCoppyDataUpdateEvent != null)
                OnBossCoppyDataUpdateEvent();
        }
    }

    protected void S2C_OnGotBossCopyData(Pt _pt)
    {
        //Debug.Log("S2C_OnGotBossCopyData");
        pt_update_boss_copy_c139 info = _pt as pt_update_boss_copy_c139;
        if (info != null)
        {
            RemainBossCount = info.boss_surplus_num;
            CurBossCoppyAttrNum = info.add_property;
            info.boss_list.Sort(CompareBossList);
            bossList = info.boss_list;
            if (OnBossCoppyDataUpdateEvent != null)
                OnBossCoppyDataUpdateEvent();
        }
    }
	#endregion

	#region C2S
	/// <summary>
	/// 挑战BOSS
	/// </summary>
	public void C2S_ChallengeBoss(int bossID,int bossType)
	{
		pt_req_challenge_seal_boss_d702 msg = new pt_req_challenge_seal_boss_d702();
		msg.boss_id = bossID;
		msg.type = bossType;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 请求BOSS列表
	/// </summary>
	public void C2S_ReqChallengeBossList()
	{
		//Debug.Log("C2S_ReqChallengeBossList");
		pt_req_boss_challenge_info_d703 msg = new pt_req_boss_challenge_info_d703();
		NetMsgMng.SendMsg(msg);
	}
    /// <summary>
    /// type:1购买挑战次数  2购买属性次数
    /// </summary>
    public void C2S_ReqAddBossCoppyData(int type,int times)
    {
        Debug.Log("C2S_ReqAddBossCoppyData type:"+type+",times:"+times);
        pt_req_boss_cpoy_buy_c137 msg = new pt_req_boss_cpoy_buy_c137();
        msg.req_type = (uint)type;
        msg.buy_nums = (byte)times;
        NetMsgMng.SendMsg(msg);
    }
	#endregion

    int CompareBossList(boss_copy_list data1,boss_copy_list data2)
    {
        if (data1.boss_kill_state < data2.boss_kill_state)
            return 1;
        if (data1.boss_kill_state > data2.boss_kill_state)
            return -1;
        BossRef boss1 = ConfigMng.Instance.GetBossRefByID((int)data1.boss_id);
        if (boss1 == null) return 0;
        BossRef boss2 = ConfigMng.Instance.GetBossRefByID((int)data2.boss_id);
        if (boss2 == null) return 0;
        if (boss1.needLevel < boss2.needLevel)
            return -1;
        if (boss1.needLevel < boss2.needLevel)
            return 1;
        return 0;
    }
}
public class BossChallengeData
{
	private boss_challenge boss;

	protected BossRef curBossRef;
	public BossRef CurBossRef
	{
		get
		{
			if(curBossRef == null)
				curBossRef = ConfigMng.Instance.GetBossRefByID(boss.boss_id);
			return curBossRef;
		}
	}

    public string bossIcon
    {
        get
        {
            if (CurBossRef != null)
            {
                MonsterRef mob = ConfigMng.Instance.GetMonsterRef(CurBossRef.monsterId);
                if (mob != null) return mob.res;
            }
            return string.Empty;
        }
    }

	public int bossID
	{
		get
		{
			return boss.boss_id;
		}
	}
	public string bossName
	{
		get
		{
			if(CurBossRef != null)
			{
				int monsterId = CurBossRef.monsterId;
				MonsterRef mob = ConfigMng.Instance.GetMonsterRef(monsterId);
				if(mob != null)
					return mob.name;
			}
			return string.Empty;
		}
	}
	public int Type
	{
		get
		{
			if(CurBossRef != null)
			{
				return CurBossRef.type;
			}
			return 0;
		}
	}

    public int NeedLevel
    {
        get
        {
            return CurBossRef == null ? 0 : CurBossRef.needLevel;
        }
    }
	/// <summary>
	/// 是否可击杀
	/// </summary>
	public bool CanKill
	{
		get
		{
            if (Type == (int)BossChallengeWnd.ToggleType.SealBoss)
                return boss.kill_state == 0 && NeedLevel <= GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel && AppearTime <= (int)Time.realtimeSinceStartup;
			return (boss.kill_state == 0);
		}
	}
    /// <summary>
    /// 是否已经通关
    /// </summary>
    private bool alreadyCarnet;
    public bool AlreadyCarnet
    {
        get
        {
            //Debug.Log("alreadyCarnet=boss.kill_state=" + boss.kill_state);
            if (boss == null) return false;
            return alreadyCarnet = (boss.kill_state == 1);
        }
    }
    protected int appearTime = 0;
	/// <summary>
	/// 出现时间
	/// </summary>
	public int AppearTime
	{
		get
		{
			return appearTime;
		}
		set
		{
			appearTime = value;
		}
	}
	public string KillName
	{
		get
		{
            return boss == null?string.Empty:boss.kill_name;
		}
	}

	public List<ItemValue> rewardList
	{
		get
		{
			List<ItemValue> list = new List<ItemValue>();
			if(CurBossRef != null)
			{
				for (int i = 0,max=CurBossRef.item.Count; i < max; i++) {
					list.Add(new ItemValue(CurBossRef.item[i],1));
				}
			}
			return list;
		}
	}

	public BossChallengeData(boss_challenge boss)
	{
		this.boss = boss;
		AppearTime = boss.surplus_time + (int)Time.realtimeSinceStartup;
	}

    public BossChallengeData(BossRef boss)
    {
        curBossRef = boss;
    }
}
