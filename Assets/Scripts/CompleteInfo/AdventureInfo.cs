using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
///冒险数据类
/// </summary>
public class AdventureInfo {

    
}

/// <summary>
///冒险类型
/// </summary>
public enum AdventureType 
{
    None,
    /// <summary>
    ///考古1
    /// </summary>
    SearchTreasure,
    /// <summary>
    ///悬赏2
    /// </summary>
    Ring,
    /// <summary>
    ///阵营3
    /// </summary>
    CampReward,
    /// <summary>
    ///英雄挑战4
    /// </summary>
    HeroKiller,
}