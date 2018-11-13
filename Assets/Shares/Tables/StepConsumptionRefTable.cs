//=========================
//作者：黄洪兴
//日期：2016/04/5
//用途：阶梯消费静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StepConsumptionRefTable : AssetTable
{
	public List<StepConsumptionRef> infoList = new List<StepConsumptionRef>();
}


[System.Serializable]
public class StepConsumptionRef
{
	/// <summary>
	/// id
	/// </summary>
	public	int id;

	public List<ItemValue> copyNumber=new List<ItemValue> ();
    /// <summary>
    /// 竞技场额外购买次数消耗的元宝数量
    /// </summary>
    public List<ItemValue> areaTime = new List<ItemValue>();
    /// <summary>
    /// boss副本购买次数的消耗
    /// </summary>
    public List<ItemValue> bossAttrCost = new List<ItemValue>();
    /// <summary>
    /// 挂机副本购买次数的消耗
    /// </summary>
    public List<ItemValue> hangUpCoppyTimesCost = new List<ItemValue>();
    /// <summary>
    /// 环任务刷新消耗
    /// </summary>
    public List<ItemValue> ringTaskCost = new List<ItemValue>();
    /// <summary>
    /// 试练任务刷新消耗
    /// </summary>
    public List<ItemValue> trailTaskCost = new List<ItemValue>();
    /// <summary>
    /// 无尽重置消耗
    /// </summary>
    public List<ItemValue> endlessResetCost = new List<ItemValue>();
}