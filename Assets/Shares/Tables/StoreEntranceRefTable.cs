//=========================
//作者：龙英杰
//日期：2015/11/19
//用途：商店入口静态配置
//=========================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StoreEntranceRefTable : AssetTable
{
    public List<StoreEntranceRef> infoList = new List<StoreEntranceRef>();
}


[System.Serializable]
public class StoreEntranceRef
{
    /// <summary>
    /// 入口ID
    /// </summary>
    public int storeEntranceId;
    /// <summary>
    /// 入口描述
    /// </summary>
    public string des;
    /// <summary>
    /// 该入口包含的商店组
    /// </summary>
    public List<int> storeGroupList = new List<int>();

}
