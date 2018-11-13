//============================
//作者：何明军
//日期：2016/3/23
//用途：副本结算系统数据
//============================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CopySettlementDataInfo{
	/// <summary>
	/// 结算物品
	/// </summary>
	public List<EquipmentInfo> items = new List<EquipmentInfo>();
    /// <summary>
    /// 队伍奖励
    /// </summary>
    public List<EquipmentInfo> teamItems = new List<EquipmentInfo>();
	/// <summary>
	/// 无尽星星
	/// </summary>
	public int star;
	/// <summary>
	/// 无尽时间
	/// </summary>
	public int time;
	/// <summary>
	/// 翻牌ID
	/// </summary>
	public int clickFlop;
	/// <summary>
	/// 翻牌物品
	/// </summary>
	public FDictionary flopItems = new FDictionary();

	/// <summary>
	/// 竞技场结果
	/// </summary>
	public int state;
	/// <summary>
	/// 竞技场排名
	/// </summary>
	public int rank;
	/// <summary>
	/// 竞技场排名上升
	/// </summary>
	public int upRank;
	/// <summary>
	/// 显示KO
	/// </summary>
	public bool showKo = true;
    /// <summary>
    /// 副本id
    /// </summary>
    public int coppyId;
    /// <summary>
    /// 击杀BOSS数
    /// </summary>
    public int bossCount;
}
