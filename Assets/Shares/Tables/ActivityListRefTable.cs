//=========================
//作者：黄洪兴
//日期：2016/04/25
//用途：活动大厅静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ActivityListRefTable : AssetTable
{
	public List<ActivityListRef> infoList = new List<ActivityListRef>();
}


[System.Serializable]
public class ActivityListRef
{
	public	int id;
	public ActivityType ID{
		get{
			return (ActivityType)id;
		}
	}
	public int type;
	public string name;
	public string icon;
	/// <summary>
	/// 循环模式
	/// </summary>
	public int looptype;
	/// <summary>
	/// 循环天数
	/// </summary>
	public List<int> loopday=new List<int>();
	/// <summary>
	/// 循环时间
	/// </summary>
	public List<ClockTimeRegion> looptime=new List<ClockTimeRegion>();
	/// <summary>
	/// 服务端相关代码
	/// </summary>
	public string script;
	/// <summary>
	/// 最大次数
	/// </summary>
	public int times;
	/// <summary>
	/// 等级限制
	/// </summary>
	public int level;
	public string typeres;
	public string nameready;
	public string namestart;
	public string nameend;
	public string title;
	public List<int> rewarditem=new List<int> ();
	public string rewardres;
	public string res;
	public List<int> buttontype = new List<int> ();
    public int mainInterfaceButton;
    public int listNum;
}
[System.Serializable]
public class ClockTimeRegion
{
	public List<int> start;
	public List<int> end;
	public  ClockTimeRegion(List<int> _start ,List<int> _end)
	{
		this.start = _start;
		this.end = _end;
	}
}

public enum ActivityUIType{
	NONE = 0,
	/// <summary>
	/// 正常
	/// </summary>
	Normal,
	/// <summary>
	/// 不在活动大厅界面显示
	/// </summary>
	NoInActivityWnd,
	/// <summary>
	/// 时间有服务端控制的
	/// </summary>
	ServerActivityTime,
}

public enum ActivityState{
	NONE = 0,
	/// <summary>
	/// 未开始
	/// </summary>
	NOTATTHE,
	/// <summary>
	/// 进行中
	/// </summary>
	ONGOING,
	/// <summary>
	/// 已结束
	/// </summary>
	HASENDED,
}

public enum ActivityType{
	NONE = 0,
	/// <summary>
	/// 神圣晶石	
	/// </summary>
	HOLYSPAR,
	/// <summary>
	/// 封神之战
	/// </summary>
	SEALOFTHE,
	/// <summary>
	/// 每日运镖
	/// </summary>
	DAILYTRANSPORTDART,
	/// <summary>
	/// 仙盟篝火
	/// </summary>
	FAIRYAUBONFIRE,
	/// <summary>
	/// 武道会
	/// </summary>
	BUDOWILL,
	/// <summary>
	/// 仙域守护
	/// </summary>
	FAIRYDOMAINTOPROTECT,
	/// <summary>
	/// 怪物攻城
	/// </summary>
	MONSTERSIEGE,
	/// <summary>
	/// 荒野奇袭
	/// </summary>
	THEWILDSURPRISE,
	/// <summary>
	/// 仙盟运镖
	/// </summary>
	FAIRYAUSHIPMENTDART,
	/// <summary>
	/// 仙盟攻城
	/// </summary>
	FAIRYAUSIEGE,
	/// <summary>
	/// 地宫BOSS
	/// </summary>
	UNDERBOSS,
    /// <summary>
    /// 仙盟战
    /// </summary>
    FAIRYAFIGHT,
    /// <summary>
    /// 夺宝奇兵
    /// </summary>
    RAIDERARK,
    /// <summary>
    /// 火焰山战场
    /// </summary>
    BATTLEFAGIHT,
}


