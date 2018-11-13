//====================
//作者：鲁家旗
//日期：2016/4/19
//用途：排行榜数据层
//====================
using UnityEngine;
using System.Collections;
using st.net.NetBase;

public class NewRankingInfo 
{
   
    #region 服务端数据
    protected rank_info_base serverdata = null;
    #endregion

    #region 构造
    /// <summary>
    /// 服务端数据来构造
    /// </summary>
    public NewRankingInfo(rank_info_base _data)
    {
        serverdata = _data;
    }
    #endregion

    #region 访问器
    /// <summary>
    /// 玩家ID
    /// </summary>
    public int PlayerId
    {
        get
        {
            return serverdata == null ? 0 : (int)serverdata.id;
        }
    }
    /// <summary>
    /// 玩家名
    /// </summary>
    public string PlayerName
    {
        get
        {
            return serverdata == null? string.Empty : serverdata.name;
        }
    }
    /// <summary>
    /// 玩家战力 10000
    /// </summary>
    public int Fighting
    {
        get
        {
            return serverdata == null ? 0 : serverdata.value1;
        }
    }
    /// <summary>
    /// 玩家等级 3转40级
    /// </summary>
    public string Lev
    {
        get
        {
            return serverdata == null ? string.Empty : ConfigMng.Instance.GetLevelDes(serverdata.value1);
        }
    }
    /// <summary>
    /// 宠物战力 1000
    /// </summary>
    public int PetFighting
    {
        get
        {
            return serverdata == null ? 0 : serverdata.value1;
        }
    }
    /// <summary>
    /// 坐骑品阶 3阶6级
    /// </summary>
    public string MountLev
    {
        get
        {
            RidePropertyRef rideProperty = ConfigMng.Instance.GetMountPropertyRef(serverdata.value1);
            return rideProperty == null ? string.Empty : rideProperty.name;
        }
    }
    /// <summary>
    /// 无尽篇章 第二章第三关
    /// </summary>
    public string Endless
    {
        get
        {
            CheckPointRef checkPoint = ConfigMng.Instance.GetCheckPointRef(serverdata.value1);
            return checkPoint==null ? string.Empty :checkPoint.name;
        }
    }
    /// <summary>
    /// 仙盟等级 3
    /// </summary>
    public int GuildLev
    {
        get
        {
            return serverdata == null ? 0 : serverdata.value1;
        }
    }
    /// <summary>
    /// 仙盟战力 10000
    /// </summary>
    public int GuildFighting
    {
        get
        {
            return serverdata == null ? 0 : serverdata.value2;
        }
    }
    /// <summary>
    /// 翅膀等阶 魔瞳之翼
    /// </summary>
    public string WingName
    {
        get
        {
            WingRef wing = ConfigMng.Instance.GetWingRef(serverdata.value1,serverdata.value2);
            return wing == null ? string.Empty : wing.name;
        }
    }
    /// <summary>
    /// 送花数量
    /// </summary>
    public int ToFlowerNum
    {
        get
        { 
            return serverdata == null ? 0 : serverdata.value1;
        }
    }
    /// <summary>
    /// 收花数量
    /// </summary>
    public int FlowerNum
    {
        get
        {
            return serverdata == null ? 0 : serverdata.value1;
        }
    }
    /// <summary>
    /// 击杀普通玩家数
    /// </summary>
    public int KillPeople
    {
        get
        {
            return serverdata == null ? 0 : serverdata.value1;
        }
    }
    /// <summary>
    /// 击杀恶人数
    /// </summary>
    public int KillWickedPreson
    {
        get
        {
            return serverdata == null ? 0 : serverdata.value1;
        }
    }
    #endregion
}
