//====================================================
//作者: 黄洪兴
//日期：2016/5/10
//用途：武道会排名数据层对象
//======================================================




using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BudokaiRankServerData
{
    public int rank;
    public string name;
    public int power;
    public int score;

}

/// <summary>
/// 武道会排名数据层对象 
/// </summary>
public class BudokaiRankInfo
{
    #region 服务端数据
    BudokaiRankServerData budokaiRankServerData;
    #endregion

    #region 静态配置数据
    #region 构造
    public BudokaiRankInfo(int _rank, string _name , int _power,int _score)
    {
        budokaiRankServerData=new BudokaiRankServerData();
        budokaiRankServerData.rank=_rank;
        budokaiRankServerData.name=_name;
        budokaiRankServerData.power=_power;
        budokaiRankServerData.score=_score;
    }
    #endregion

    #region 访问器
    /// <summary>
    /// 排名
    /// </summary>
    public int Rank
    {
        get { return budokaiRankServerData.rank; }
    }

    /// <summary>
    /// 名字
    /// </summary>
    public string Name
    {
        get { return budokaiRankServerData.name; }
    }
    /// <summary>
    /// 战斗力
    /// </summary>
    public int Power
    {
        get { return budokaiRankServerData.power; }
    }
    /// <summary>
    /// 积分
    /// </summary>
    public int Score
    {
        get { return budokaiRankServerData.score; }
    }
  


    #endregion
    #endregion
}
