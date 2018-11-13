//=============================
//作者：易睿
//日期：2015/09/28
//用途：场景物品静态配置数据
//=============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneItemRefTable : AssetTable
{

    public List<SceneItemRef> infoList = new List<SceneItemRef>();
}

[System.Serializable]
public class SceneItemRef
{
    /// <summary>
    /// ID
    /// </summary>
    public int id;
    /// <summary>
    /// 开启消耗时间，用于点击、碰撞
    /// </summary>
    public int openTime;
    /// <summary>
    /// 名字高度
    /// </summary>
    public float nameHeight;
    /// <summary>
    /// 场景物品消失时间
    /// </summary>
    public float actionTimes;
    /// <summary>
    /// 名称
    /// </summary>
    public string name;
    /// <summary>
    /// 描述
    /// </summary>
    public string dec;
    /// <summary>
    /// 模型资源名字
    /// </summary>
    public string assetName;
    /// <summary>
    /// UI展示文字
    /// </summary>
    public string actionIngDec;
    /// <summary>
    /// 玩家互动动作,用于点击、碰撞
    /// </summary>
    public string playerAction;
    /// <summary>
    /// 物品的常规动作
    /// </summary>
    public string idleAction;
    /// <summary>
    /// 物品的休闲动作
    /// </summary>
    public string relaxAction;
    /// <summary>
    /// 物品被互动时的动作,用于点击、碰撞、攻击
    /// </summary>
    public string actionIng;
    /// <summary>
    /// 物品被互动特效,用于点击、碰撞、攻击
    /// </summary>
    public string actionIngDisresName;
    /// <summary>
    /// 物品被互动声音,用于点击、碰撞、攻击
    /// </summary>
    public string actionIngSoundRes;
    /// <summary>
    /// 物品消失动作,用于点击、碰撞、攻击
    /// </summary>
    public string action;
    /// <summary>
    /// 物品消失特效,用于点击、碰撞、攻击
    /// </summary>
    public string actionDisresName;
    /// <summary>
    /// 物品消失声音,用于点击、碰撞、攻击
    /// </summary>
    public string actionSoundRes;
    /// <summary>
    /// 玩家后续动作
    /// </summary>
    public string afterAction;
    /// <summary>
    /// 阻挡碰撞体积
    /// </summary>
    public Vector3 modelVec;
    /// <summary>
    /// 触发方式
    /// </summary>
    public TouchType touchType;
    /// <summary>
    /// 触发后展示UI类型
    /// </summary>
    public ActionUIType actionUItype;
    /// <summary>
    /// 死亡后结果
    /// </summary>
    public DeadResult deadResult;
    /// <summary>
    /// 场景物品所携带的特效
    /// </summary>
    public List<SceneItemEffect> sceneItemEffect = new List<SceneItemEffect>();
}


/// <summary>
/// 触发方式
/// </summary>
public enum TouchType
{ 
    NONE,
    /// <summary>
    /// 无触发（纯阻挡）
    /// </summary>
    BLOCK,
    /// <summary>
    /// 可被点击
    /// </summary>
    TOUCH,
    /// <summary>
    /// 可被碰撞
    /// </summary>
    COLLIDE,
    /// <summary>
    /// 可被攻击
    /// </summary>
    ATK,
}
/// <summary>
/// 触发后展示UI类型
/// </summary>
public enum ActionUIType
{ 
    /// <summary>
    /// 进度条
    /// </summary>
    PROGRESSBAR,
    /// <summary>
    /// 血条
    /// </summary>
    LIFEBAR,
    /// <summary>
    /// 自己冒泡
    /// </summary>
    SELFBUBBLE,
    /// <summary>
    /// 目标冒泡
    /// </summary>
    TARGETBUBBLE,
}
/// <summary>
/// 死亡后结果
/// </summary>
public enum DeadResult
{ 
    /// <summary>
    /// 死亡后消失
    /// </summary>
    DISPPEAR,
    /// <summary>
    /// 以死亡动作保留
    /// </summary>
    REMAIN,
}

[System.Serializable]
public class SceneItemEffect
{
    public string pointName;
    public string effectName;


    public SceneItemEffect(string _pointName, string _effectName)
    {
        this.pointName = _pointName;
        this.effectName = _effectName;

    }

    public SceneItemEffect()
    {
    }

}