//=========================
//作者：龙英杰
//日期：2016/1/27
//用途：怪物分布表静态配置
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterDistributionRefTable : AssetTable
{
    public List<MonsterDistributionRef> infoList = new List<MonsterDistributionRef>();
}


[System.Serializable]
public class MonsterDistributionRef
{
    /// <summary>
    /// id
    /// </summary>
    public int id;
    /// <summary>
    /// 怪物名字
    /// </summary>
    public string monsterName;
    /// <summary>
    /// 怪物类型名
    /// </summary>
    public string desName;
    /// <summary>
    /// 怪物ID
    /// </summary>
    public int monsterId;
    /// <summary>
    /// 怪物数目
    /// </summary>
    public int monsterNum;
    /// <summary>
    /// 场景ID
    /// </summary>
    public int sceneId;

    /// <summary>
    /// 怪物类型 1为怪物 2为场景物品
    /// </summary>
    public int refreshObjType;
    /// <summary>
    /// 坐标
    /// </summary>
    public Vector3 position;

}
