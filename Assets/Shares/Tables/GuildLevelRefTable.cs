//==========================
//作者：龙英杰
//日期：2015/12/12
//用途：公会表静态配置
//==========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuildLevelRefTable : AssetTable
{
    public List<GuildLevelRef> infoList = new List<GuildLevelRef>();
}


[System.Serializable]
public class GuildLevelRef
{
    /// <summary>
    /// 公会等级
    /// </summary>
    public int level;
    /// <summary>
    /// 公会升级经验
    /// </summary>
    public int experience;
    /// <summary>
    /// 公会最大人数
    /// </summary>
    public int maxPlayers;

}

