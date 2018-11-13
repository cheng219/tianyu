//==================================
//作者：邓成
//日期：2016/7/5
//用途：公会数据层
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;

public class GuildData
{
    public int guildId = 0;
    public string guildName = "";
    public string presidentName = "";
	public string notice = string.Empty;
    public int memberAmount =0;
	public int expandTimes = 0;//扩充成员次数
	public int totalMemAmount = 0;
    public int guildLevel = 0;
	public int guildExp = 0;
	public int guildRank = 0;
	public int guildFightValue = 0;
	public bool haveGotSalary = false;
	public int myPos = 0;

	public bool haveJoined = false;
	public bool canJoin = false;

    public int state = 0;
    public int totalRes = 0;
    public int donateTime = 0;
	public int guildCamp = 0;

    public GuildData() { }
	/// <summary>
	/// 自己的公会信息
	/// </summary>
	public GuildData(pt_guild_info_d380 info)
    {
		guildId = (int)info.id;
		guildName = info.name;
		guildLevel = (int)info.lev;
		presidentName = info.lead_name;
		memberAmount = info.members;
		guildRank = (int)info.rank;
		guildExp = info.exp;
		notice = info.purpose.ToString();
		myPos = info.pos;
		haveGotSalary = (info.get_salary_state == 1);
		expandTimes = info.expand_num;
    }
	/// <summary>
	/// 公会列表
	/// </summary>
	/// <param name="info"></param>
	public GuildData(guild_list_info info)
    {
		guildId = (int)info.id;
		guildName = info.name;
		presidentName = info.leader;
        guildLevel = info.lev;
		memberAmount = (int)info.cur_member;
		totalMemAmount = (int)info.all_member;
		guildFightValue = info.fight_score;
		haveJoined = (info.ask_join_state == 1);
		guildRank = info.rank;
		canJoin = (!haveJoined && (memberAmount < totalMemAmount));
    }
}

public class GuildMemberData
{
    public int memberID = 0;
    public string memberName = "";
    public int memberLv = 0;
    public GuildMemPosition memberPosition = GuildMemPosition.MEMBER;
    public int donate = 0;
	public int allDonate = 0;
    public int recentTime = 0;
    public int isOnline = 0;//0不在线1在线
	public int fightValue = 0;

    /// <summary>
    /// 公会成员
    /// </summary>
    /// <param name="info"></param>
	public GuildMemberData(guild_member_info info)
    {
		memberID = (int)info.uid;
		memberLv = (int)info.lev;
		memberName = info.name;
		memberPosition = (GuildMemPosition)info.position;
		donate = (int)info.tribute;
		allDonate = info.all_tribute;
		recentTime = info.last_login_time;
		fightValue = info.fight_score;
    }
    /// <summary>
    /// 新成员验证
    /// </summary>
    /// <param name="info"></param>
	public GuildMemberData(ask_join_guild_list info)
    {
        memberID = (int)info.uid;
        memberLv = (int)info.lev;
        memberName = info.name;
		fightValue = info.fight_score;
		isOnline = info.state;
    }
}

public class GuildInfo
{
    #region 数据
	protected GuildData serverData = null;
    public void UpdateServerData(GuildData _data)
    {
		serverData = _data;
    }
    public GuildInfo(GuildData _data)
    {
		serverData = _data;
    }
    /// <summary>
    /// 更新公告
    /// </summary>
    /// <param name="_notice"></param>
    public void UpdateNotice(string _notice)
    {
		if(serverData != null)
			serverData.notice = _notice;
    }
	/// <summary>
	/// 更新名字
	/// </summary>
	/// <param name="_notice"></param>
	public void UpdateName(string name)
	{
		if(serverData != null)
			serverData.guildName = name;
	}

	public void UpdateSalary(bool state)
	{
		if(serverData != null)
			serverData.haveGotSalary = state;
	}
    #endregion


    #region 访问器
	/// <summary>
	/// 公会ID
	/// </summary>
    public int GuildId
    {
        get { return serverData.guildId; }
    }
	/// <summary>
	/// 公会名字
	/// </summary>
    public string GuildName
    {
        get { return serverData.guildName; }
    }
	/// <summary>
	/// 会长名字
	/// </summary>
	public string PresidentName
	{
		get { return serverData.presidentName; }
	}
	/// <summary>
	/// 是否已经领取过工资
	/// </summary>
	public bool haveGotSalary
	{
		get
		{
			return serverData.haveGotSalary;
		}
	}
	/// <summary>
	/// 公会经验
	/// </summary>
    public int GuildExp
    {
        get { return serverData.guildExp; }
    }
	/// <summary>
	/// 公会成员数量
	/// </summary>
    public int MemberNum
    {
        get { return serverData.memberAmount; }
    }
	/// <summary>
	/// 公会扩充人数的次数
	/// </summary>
	public int MemberExpandTimes
	{
		get
		{
			return serverData.expandTimes;
		}
	}

	public int MemberMaxCount
	{
		get
		{
			if(CurGuildRef != null)
			{
				return CurGuildRef.num + MemberExpandTimes*GuildMng.MEMBER_EXPAND_PER_COUNT;
			}else
			{
				return 30 + MemberExpandTimes*GuildMng.MEMBER_EXPAND_PER_COUNT;
			}
		}
	}
	/// <summary>
	/// 公会等级
	/// </summary>
    public int GuildLv
    {
        get { return serverData.guildLevel; }
    }
	/// <summary>
	/// 当前等级公会的静态配置
	/// </summary>
	public GuildRef CurGuildRef
	{
		get
		{
			return ConfigMng.Instance.GetGuildRefByLv(GuildLv);;
		}
	}
	/// <summary>
	/// 公会排行
	/// </summary>
    public int GuildRank
    {
        get { return serverData.guildRank; }
    }
	/// <summary>
	/// 公会公告
	/// </summary>	
    public string Notice
    {
		get { return serverData.notice; }
    }
    /// <summary>
    /// 捐献次数
    /// </summary>
    public int DonateTime
    {
        get { return serverData.donateTime; }
    }
    /// <summary>
    /// 我的公会职位
    /// </summary>
    public GuildMemPosition MyPosition
    {
		get { return (GuildMemPosition)serverData.myPos; }
    }
    #endregion

}