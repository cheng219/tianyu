//=============================
//作者：龙英杰
//日期：2015/7/15
//用途：弹道静态配置数据
//=============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArrowRefTable : AssetTable
{

    public List<ArrowRef> infoList = new List<ArrowRef>();
}

[System.Serializable]
public class ArrowRef
{
    /// <summary>
    /// ID
    /// </summary>
    public int arrowId;
    /// <summary>
    /// 名称
    /// </summary>
    public string name;
    /// <summary>
    /// 推进速度
    /// </summary>
    public int speed;
    /// <summary>
    /// 
    /// </summary>
    public int add_speed;
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
    /// 最大距离
    /// </summary>
    public int max_dis;
    /// <summary>
    /// 弹道特效描述
    /// </summary>
    public string effect_res;
    /// <summary>
    /// 弹道陷阱音效
    /// </summary>
    public string soundArrowRes;
    /// <summary>
    /// 完成特效
    /// </summary>
    public string finisheffect;
    /// <summary>
    /// 预加载对应职业ID
    /// </summary>
    public int player;
    /// <summary>
    /// 最小距离
    /// </summary>
    public int min_dis;
    /// <summary>
    /// 弹道飞行高度：锁定弹道不受此值影响非锁定弹道，为0则贴地面飞行 非0则在指定高度飞行，float
    /// </summary>
    public float arrowFlyHeight;
    /// <summary>
    /// 影响范围
    /// </summary>
    public ImpactArea impactArea;
    /// <summary>
    /// 弹道飞行类型
    /// </summary>
    public ArrowFlyType arrowFlyType;
    /// <summary>
    ///   范围参数  外径|内径|角度|长|宽|上高|下高
    /// </summary>
    public List<float> areaParaList = new List<float>();
}
