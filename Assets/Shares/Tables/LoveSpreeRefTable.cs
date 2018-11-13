//=========================
//作者：黄洪兴
//日期：2016/05/10
//用途：首冲爱心静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoveSpreeRefTable : AssetTable
{
    public List<LoveSpreeRef> infoList = new List<LoveSpreeRef>();
}


[System.Serializable]
public class LoveSpreeRef
{

    public int id;
    /// <summary>
    /// 职业ID
    /// </summary>
    public int prof;
    /// <summary>
    /// 奖励ID
    /// </summary>
    public List<ItemValue> reward = new List<ItemValue>();

    /// <summary>
    /// 阶段
    /// </summary>
    public int stage;
    /// <summary>
    ///  领奖充值元宝
    /// </summary>
    public int money;



}


