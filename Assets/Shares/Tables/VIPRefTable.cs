//=========================
//作者：黄洪兴
//日期：2016/3/8
//用途：VIP静态配置表
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class VIPRefTable : AssetTable
{
	public List<VIPRef> infoList = new List<VIPRef>();
}

[System.Serializable]
public class VIPRef
{
	public int id;
	public int exp;
	public string prize;
	public string show;

    /// <summary>
    /// 铸魂次数
    /// </summary>
    public int cast_soul_num;
	/// <summary>
	/// 副本购买次数
	/// </summary>
    public List<CopyTimes> copyPurchasetimes = new List<CopyTimes>();

	public List<ItemValue> items = new List<ItemValue>();
	
	public void SetItems(){
		items.Clear();
		string[] str = prize.Split('|');
		ItemValue item = null;
		for(int i =0,len=str.Length;i<len;i++){
			string[] strlist = str[i].Split(',');
			item = new ItemValue(Convert.ToInt32(strlist[0]),Convert.ToInt32(strlist[1]));
			items.Add(item);
		}
	}


    /// <summary>
    /// 是否开启随身仓库
    /// </summary>
    public bool storehouseAccess;

    /// <summary>
    /// 仓库开放的格子数
    /// </summary>
    public int warehouse_num;


    /// <summary>
    /// 免费复活次数
    /// </summary>
    public int reliveTime;


    ///// <summary>
    ///// 受到伤害减免百分比  10000=100%
    ///// </summary>
    //public int hurt_reduce;

    /// <summary>
    /// 每日运镖的次数
    /// </summary>
    public int Dart_num;

    /// <summary>
    /// 环式任务一件完成是否开启
    /// </summary>
    public int Ring_task;


    /// <summary>
    /// 是否开启里*火云洞
    /// </summary>
    public bool Li_huoyun;

    /// <summary>
    /// 试练任务每天增加的次数
    /// </summary>
    public int Test_task;
    /// <summary>
    /// 挂机副本购买怪物的次数
    /// </summary>
    public int hook_times;
    /// <summary>
    /// BOSS副本购买的次数
    /// </summary>
    public int boss_times;
    ///// <summary>
    ///// 击杀怪物的经验加成  10000=100%
    ///// </summary>
    //public int EXP_addition;


    /// <summary>
    /// 开启背包格子速度的事件加速比 10000=100%
    /// </summary>
    public int bag_time;

    /// <summary>
    /// 是否免除竞技场挑战CD
    /// </summary>
    public bool arena_CD;

    /// <summary>
    /// 是否开启挂机自动买药
    /// </summary>
    public bool automatic_buy;

    /// <summary>
    /// 是否开启免费传送
    /// </summary>
    public bool Free_transfer;

    /// <summary>
    /// 是否可在商店购买珍贵道具
    /// </summary>
    public bool Shopping_mall;

    /// <summary>
    /// 是否开启免费扫荡副本功能
    /// </summary>
    public bool Mopping_free;
    /// <summary>
    /// 无尽重置次数
    /// </summary>
    public int endlessNum;
    /// <summary>
    /// 环任务刷新总次数
    /// </summary>
    public int ringRefreshNum;
    /// <summary>
    /// 试练任务刷新总次数
    /// </summary>
    public int trailRefreshNum;
}

[System.Serializable]
public class CopyTimes
{
    public int copyID;
    public int copyTimes;
    public CopyTimes(int _id, int _times)
    {
        copyID = _id;
        copyTimes = _times;
    }


}
