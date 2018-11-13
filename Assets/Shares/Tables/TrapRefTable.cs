//=====================================
//作者：龙英杰
//日期：2015/7/15
//用途：陷阱静态配置
//==========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrapRefTable : AssetTable
{
    public List<TrapRef> infoList = new List<TrapRef>();
}


[System.Serializable]
public class TrapRef
{
    /// <summary>
    /// ID
    /// </summary>
    public int trapId;
    /// <summary>
    /// 名称
    /// </summary>
    public string name;
    /// <summary>
    /// 总时间
    /// </summary>
    public int trapAllTime;
    /// <summary>
    /// 陷阱生效时间
    /// </summary>
    public int trapEffectTime;
    /// <summary>
    /// 目标数量
    /// </summary>
    public int targetNum;
    /// <summary>
    /// 最大生效次数
    /// </summary>
    public int maxEffect;
    /// <summary>
    /// 每个人最大生效次数
    /// </summary>
    public int oneMaxEffect;
    /// <summary>
    /// 多久进行一次计算
    /// </summary>
    public int perTime;
    /// <summary>
    /// 陷阱特效描述
    /// </summary>
    public string effectRes;
    /// <summary>
    /// 弹道音效
    /// </summary>
    public string soundTrapRes;
    /// <summary>
    /// 影响范围
    /// </summary>
    public ImpactArea impactArea;
    /// <summary>
    ///   范围参数  外径|内径|角度|长|宽|上高|下高
    /// </summary>
    public List<float> areaParaList = new List<float>();
}