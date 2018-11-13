//==============================================
//作者：黄洪兴
//日期：2016/4/14
//用途：仙盟技能管理类
//=================================================



using UnityEngine;
using System.Collections;
using st.net.NetBase;
using System;
using System.Collections.Generic;

public class GuildSkillMng 
{

	/// <summary>
	/// 技能列表
	/// </summary>
	public Dictionary<int, GuildSkillInfo> GuildSkillDic = new Dictionary<int, GuildSkillInfo>();
	/// <summary>
	/// 技能列表
	/// </summary>
	public List<GuildSkillInfo> GuildSkillList = new List<GuildSkillInfo>();
	/// <summary>
	/// 当前选择的技能
	/// </summary>
	public int CurSkillMark;
	public GuildSkillInfo CurGuildSkill
	{
		get {
            if (GuildSkillList.Count > CurSkillMark)
            {
                return GuildSkillList[CurSkillMark];
            }
            else
                return null;
		}
	}
	/// <summary>
	///技能列表更新事件
	/// </summary>
	public Action OnSkillListUpdate;
	public Action OnCurSkillUpdate;

	#region 构造
	/// <summary>
	/// 返回该管理类的唯一实例 
	/// </summary>
	/// <returns></returns>
	public static GuildSkillMng CreateNew(MainPlayerMng _main)
	{
		if (_main.guildSkillMng == null)
		{
			GuildSkillMng GuildSkillMng = new GuildSkillMng();
			GuildSkillMng.Init(_main);
			return GuildSkillMng;
		}
		else
		{
			_main.guildSkillMng.UnRegist(_main);
			_main.guildSkillMng.Init(_main);
			return _main.guildSkillMng;
		}
	}
	/// <summary>
	/// 注册
	/// </summary>
	protected virtual void Init(MainPlayerMng _main)
	{

		MsgHander.Regist(0xD514, S2C_OnGetGuildSkillList);
		C2S_AskGuildSkillList ();
		//		MsgHander.Regist(0xD401, S2C_OnGetUseSkillList);
		//GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += ChangeAutoUseSkill;
	}
	/// <summary>
	/// 注销
	/// </summary>
	protected virtual void UnRegist(MainPlayerMng _main)
	{
        MsgHander.UnRegist(0xD514, S2C_OnGetGuildSkillList);
		//371出售  372 回购 373 购买次数   375 请求
		//	MsgHander.UnRegist(0xD372, S2C_OnGetRedeemItemList);
		//		MsgHander.UnRegist(0xD100, S2C_OnGetSkillList);
		//		MsgHander.UnRegist(0xD401, S2C_OnGetUseSkillList);
		//GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= ChangeAutoUseSkill;
	}
	#endregion

	#region 通信S2C
	/// <summary>
	/// 获得公会技能列表
	/// </summary>
	/// <param name="_info">Info.</param>
	private void S2C_OnGetGuildSkillList(Pt _pt)
	{
        bool isRed = false;
		GuildSkillDic.Clear ();
		GuildSkillList.Clear ();
		pt_guild_skill_list_d514 pt = _pt as  pt_guild_skill_list_d514;
		if (pt != null) 
        {
			for (int i = 0; i < pt.guild_skills.Count; i++) 
            {
                GuildSkillInfo skillInfo = new GuildSkillInfo (pt.guild_skills [i]);
                GuildSkillDic[pt.guild_skills[i]] = skillInfo;
                GuildSkillList.Add(skillInfo);
                if (skillInfo.CanUpgrade)
                {
                    isRed = true;
                } 
			}
            GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.GUILDSKILL, isRed);
		}
		if (OnSkillListUpdate != null) 
        {
			OnSkillListUpdate ();
		}
	}


	#endregion

	#region C2S	
	/// <summary>
	/// 请求技能列表
	/// </summary>
	public  void C2S_AskGuildSkillList()
	{
		pt_req_guild_skill_d516 msg = new pt_req_guild_skill_d516();
		NetMsgMng.SendMsg(msg);

		//Debug.Log ("发送请求技能列表的协议");

	}
	/// <summary>
	/// 升级技能
	/// </summary>
	public  void C2S_AskSkillUp(int skillid)
	{
		pt_req_guild_skill_update_d515 msg = new pt_req_guild_skill_update_d515();
		msg.skill_id = skillid;
		NetMsgMng.SendMsg(msg);
		//Debug.Log ("发送请求技能升级的协议，技能ID为"+skillid);
	}
	#endregion

    public void SetSkillRed()
    {
        bool isRed = false;
        for (int i = 0,length=GuildSkillList.Count; i < length; i++)
        {
            GuildSkillInfo skillInfo = GuildSkillList[i];
            if (skillInfo != null && skillInfo.CanUpgrade)
            {
                isRed = true;
            } 
        }
        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.GUILDSKILL, isRed);
    }
}
