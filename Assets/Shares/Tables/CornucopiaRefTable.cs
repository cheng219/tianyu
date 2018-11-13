//=========================
//作者：李邵南
//日期：2017/04/11
//用途：聚宝盆静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CornucopiaRefTable : AssetTable
{
    public List<CornucopiaRef> infoList = new List<CornucopiaRef>();
}

[System.Serializable]
public class CornucopiaRef 
{
    /// <summary>
    /// ID
    /// </summary>
    public int id;
    /// <summary>
    /// 需求的VIP等级
    /// </summary>
    public int vip;
    /// <summary>
    /// 消耗元宝数量
    /// </summary>
    public int consume;
    /// <summary>
    /// 80%获得元宝数量
    /// </summary>
    public int probability80;
    /// <summary>
    /// 20%概率获得的元宝数量
    /// </summary>
    public int probability20;
}
