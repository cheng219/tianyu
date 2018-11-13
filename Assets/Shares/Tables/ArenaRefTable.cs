//=========================
//作者：黄洪兴
//日期：2016/04/21
//用途：竞技场静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaRefTable : AssetTable
{
	public List<ArenaRef> infoList = new List<ArenaRef>();
}


[System.Serializable]
public class ArenaRef
{
	/// <summary>
	///玩家等级
	/// </summary>
	public	int level;
	/// <summary>
	/// 玩家每场比赛胜利获得的奖励，失败奖励直接减半
	/// </summary>
	public List<ItemValue> reward=new List<ItemValue>();


}