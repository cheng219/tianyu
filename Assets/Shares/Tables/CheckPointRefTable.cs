//=========================
//作者：黄洪兴
//日期：2016/03/28
//用途：无尽试炼关卡静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckPointRefTable : AssetTable
{
	public List<CheckPointRef> infoList = new List<CheckPointRef>();
}


[System.Serializable]
public class CheckPointRef
{

	public	int id;

	public string name;
	/// <summary>
	/// 前置关卡ID
	/// </summary>
	public int frontGate;
	/// <summary>
	/// 推荐战力值
	/// </summary>
	public string fighting;
	/// <summary>
	/// 通关副本给予的奖励
	/// </summary>
	public List<ItemValue> reward=new List<ItemValue> ();
	/// <summary>
	/// 首次通关副本给予的奖励
	/// </summary>
	public List<ItemValue> firstAward=new List<ItemValue> ();
	/// <summary>
	/// 界面X,Y坐标
	/// </summary>
	public Vector2 coordinate;
	/// <summary>
	/// 关卡图
	/// </summary>
	public string icon;
	/// <summary>
	/// 3星时间
	/// </summary>
	public int star3;
	/// <summary>
	/// 2星时间
	/// </summary>
	public int star2;
	/// <summary>
	/// 1星时间
	/// </summary>
	public int star1;
    /// <summary>
    /// 章节ID
    /// </summary>
    public int chapterID;

    /// <summary>
    /// 所属章节
    /// </summary>
    public int Chapter;
    /// <summary>
    /// 关卡名字
    /// </summary>
    public string chapterName;
}