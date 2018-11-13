//=========================
//作者：黄洪兴
//日期：2016/03/22
//用途：称号静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TitleRefTable : AssetTable
{
	public List<TitleRef> infoList = new List<TitleRef>();
}


[System.Serializable]
public class TitleRef
{
	/// <summary>
	/// 称号类型
	/// </summary>
	public	int type;
	/// <summary>
	/// 称号名称
	/// </summary>
	public string name;
	/// <summary>
	/// 称号奖励说明
	/// </summary>
	public string des;
	/// <summary>
	/// 获得途径的说明
	/// </summary>
	public string wayDes;
	/// <summary>
	/// 称号对应的图片
	/// </summary>
	public string icon;
	/// <summary>
	/// 判断条件（1大于判断值2小于判断值3等于判断值）
	/// </summary>
	public int judge;
	/// <summary>
	/// 判断的值
	/// </summary>
	public List<int> judgeNum;
	/// <summary>
	/// 条件不符合时是否移除（1移除，2不移除）
	/// </summary>
	public int removeJudge;
	/// <summary>
	/// 增加的属性
	/// </summary>
	public List<ItemValue> attribute=new List<ItemValue>();
	/// <summary>
	/// 时限判定 永久则填0
	/// </summary>
	public int time;
	/// <summary>
	/// 战斗力
	/// </summary>
	public int gs;

    /// <summary>
    /// 称号名字文本
    /// </summary>
    public string nameDes;

}