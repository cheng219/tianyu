//====================================================
//作者: 黄洪兴
//日期：2016/4/14
//用途：仙盟技能的数据层对象
//======================================================




using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GuildSkillServerData
{
	public int id;

}

/// <summary>
/// 商店商品数据层对象 
/// </summary>
public class GuildSkillInfo 
{
	#region 服务端数据 
	GuildSkillServerData guildSkillData;
	#endregion

	#region 静态配置数据 
	GuildSkillRef guildSkillRef = null;
	public GuildSkillRef GuildSkillRef
	{
		get
		{
			if (guildSkillRef != null) return guildSkillRef;
			guildSkillRef = ConfigMng.Instance.GetGuildSkillRef(guildSkillData.id);
			return guildSkillRef;
		}
	}
	#region 构造 
	public GuildSkillInfo(int _id)
	{
		guildSkillData = new GuildSkillServerData ();
		guildSkillData.id = _id;

	}

	#endregion

	#region 访问器
	/// <summary>
	/// 技能ID
	/// </summary>
	public int ID
	{	
		get { return guildSkillData.id; }
	}
	/// <summary>
	/// 技能名字
	/// </summary>
	public string Name
	{
		get { 
			
			return GuildSkillRef.name;
		}
	}
	/// <summary>
	/// 文本描述
	/// </summary>
	public string Des
	{
		get { 

			return GuildSkillRef.des;
		}
	}

	/// <summary>
	/// 技能图标
	/// </summary>
	public string Icon
	{
		get { 

			return GuildSkillRef.icon;
		}
	}

	/// <summary>
	/// 技能等级
	/// </summary>
	public int Lev
	{
		get
		{
			return GuildSkillRef.lev;
		}
	}
	/// <summary>
	/// 技能属性
	/// </summary>
	/// <value>The amount.</value>
	public List<AttributePair> Attrs
	{
		get{
			return GuildSkillRef.attrs;
		}
	}

	/// <summary>
	/// 公会等级前提
	/// </summary>
	public int Need1
	{
		get
		{
			return GuildSkillRef.need1;
		}
	}
	/// <summary>
	/// 贡献度前提
	/// </summary>
	public int Need2
	{
		get
		{
			return GuildSkillRef.need2;
		}
	}
	/// <summary>
	/// 技能等级
	/// </summary>
	public int Cost
	{
		get
		{
			return GuildSkillRef.cost;
		}
	}
	/// <summary>
	/// 战力
	/// </summary>
	public int Gs
	{
		get
		{
			return GuildSkillRef.gs;
		}
	}
    /// <summary>
    /// 可以升级
    /// </summary>
    public bool CanUpgrade
    {
        get
        {
            if (GameCenter.guildMng.MyGuildInfo != null && GameCenter.guildMng.MyGuildInfo.GuildLv >= Need1 && GameCenter.mainPlayerMng.MainPlayerInfo.GuildContribution >= Cost)
            {
                return true;
            }
            return false;
        }
    }
	#endregion
	#endregion
}
