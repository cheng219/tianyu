//===========================
//作者：龙英杰
//日期：2016/2/2
//用途：考古怪静态配置
//===========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArcheologyEnemyRefTable : AssetTable
{
    public List<ArcheologyEnemyRef> infoList = new List<ArcheologyEnemyRef>();
}


[System.Serializable]
public class ArcheologyEnemyRef
{
    /// <summary>
    /// ID
    /// </summary>
    public int id;
    /// <summary>
    /// 怪物名字
    /// </summary>
    public string enemyName;
    /// <summary>
    /// 怪物组
    /// </summary>
    public List<int> enemyGroupList = new List<int>();

}
