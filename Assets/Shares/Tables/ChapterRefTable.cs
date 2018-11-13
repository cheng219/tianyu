//=========================
//作者：黄洪兴
//日期：2016/03/25
//用途：无尽试练静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChapterRefTable : AssetTable
{
	public List<ChapterRef> infoList = new List<ChapterRef>();
}


[System.Serializable]
public class ChapterRef
{

	public	int id;

	public string name;

	/// <summary>
	/// 所属关卡
	/// </summary>
	public List<int> allLevels=new List<int>();
	/// <summary>
	/// 奖励
	/// </summary>
	public List<ChapterReward> rewardData=new List<ChapterReward>();
/// <summary>
/// 章节图片
/// </summary>
	public string icon;
}
[System.Serializable]
public class ChapterReward
{
	/// <summary>
	/// 星级对应的星数
	/// </summary>
	public int starNum;
	/// <summary>
	/// 对应的奖励
	/// </summary>
	public List<ItemValue> reward;
    /// <summary>
    /// 奖励描述
    /// </summary>
    public string rewardDes;
	public ChapterReward(int _star,List<ItemValue> _reward, string _des)
	{
		this.starNum = _star;
		this.reward = _reward;
        this.rewardDes = _des;
	}


}


