//=========================
//作者：黄洪兴
//日期：2016/04/21
//用途：竞技场排名静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaRankRefTable : AssetTable
{
	public List<ArenaRankRef> infoList = new List<ArenaRankRef>();
}


[System.Serializable]
public class ArenaRankRef
{
	/// <summary>
	///竞技场排名
	/// </summary>
	public	int ranking;
	/// <summary>
	/// 玩家每场比赛胜利获得的奖励，失败奖励直接减半
	/// </summary>
	public List<ItemValue> reward=new List<ItemValue>();


}