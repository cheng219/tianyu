//===========================
//作者：龙英杰
//日期：2016/2/2
//用途：考古点静态配置
//===========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArcheologyPointRefTable : AssetTable
{
    public List<ArcheologyPointRef> infoList = new List<ArcheologyPointRef>();
}


[System.Serializable]
public class ArcheologyPointRef
{
    /// <summary>
    /// ID
    /// </summary>
    public int id;
    /// <summary>
    /// 宝箱等级
    /// </summary>
    public int lv;
    /// <summary>
    /// 场景ID
    /// </summary>
    public int sceneId;
    /// <summary>
    /// 宝箱ID
    /// </summary>
    public int rewardBoxId;
    /// <summary>
    /// 刷怪几率
    /// </summary>
    public float enemyRate;
    /// <summary>
    /// 坐标
    /// </summary>
    public Vector3 coordinate;
    /// <summary>
    /// 怪物组
    /// </summary>
    public List<int> enemyGroupList = new List<int>();

}
