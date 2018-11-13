//==============================
//作者：龙英杰
//日期：2015/09/22
//用途：随从表静态配置
//==============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntourageRefTable : AssetTable
{
    public List<EntourageRef> infoList = new List<EntourageRef>();
}


[System.Serializable]
public class EntourageRef
{
    /// <summary>
    /// 佣兵名
    /// </summary>
    public string petname;
    /// <summary>
    /// 佣兵头像
    /// </summary>
    public string headIcon;
    /// <summary>
    /// 佣兵介绍
    /// </summary>
    public string petintro;
    /// <summary>
    /// 对应原画
    /// </summary>
    public string pic;
    /// <summary>
    /// 骨骼名称
    /// </summary>
    public string boneName;
    /// <summary>
    /// 骨骼资源
    /// </summary>
    public string res_name;
    /// <summary>
    /// ID
    /// </summary>
    public int id;
    /// <summary>
    /// 佣兵基础战力
    /// </summary>
    public int baseFighting;
    /// <summary>
    /// 需求魂石ID
    /// </summary>
    public int needSoulId;
    /// <summary>
    /// 需求魂石数量
    /// </summary>
    public int needSoulNum;
    /// <summary>
    /// 佣兵星级
    /// </summary>
    public int star;
    ///// <summary>
    ///// 魂石物品ID
    ///// </summary>
    //public int itemID;
    ///// <summary>
    ///// 佣兵品级
    ///// </summary>
    //public int quality;
    /// <summary>
    /// 佣兵类型
    /// </summary>
    public int type;
    /// <summary>
    /// 每日免费出战次数
    /// </summary>
    public int freeBattleTimes;
    /// <summary>
    /// 普攻ID
    /// </summary>
    public int normalSkill;
    /// <summary>
    /// 怪物预览ID
    /// </summary>
    public int previewsMonsterId;
    /// <summary>
    /// 每秒步伐多远
    /// </summary>
    public float paceSpeed;
    /// <summary>
    /// 移动速度
    /// </summary>
    public float basemoveSpd;
    ///// <summary>
    ///// 跟随距离
    ///// </summary>
    //public float followRange;
    ///// <summary>
    ///// PVP距离
    ///// </summary>
    //public float pvpRange;
    ///// <summary>
    ///// PVE距离
    ///// </summary>
    //public float pveRange;
    ///// <summary>
    ///// 传送距离
    ///// </summary>
    //public float teleportRange;
    /// <summary>
    /// 预览缩放
    /// </summary>
    public float previewScale;
    /// <summary>
    /// 模型缩放
    /// </summary>
    public float modelScale;
    /// <summary>
    /// 名字高度
    /// </summary>
    public float nameHeigiht;
    /// <summary>
    /// 跟随距离
    /// </summary>
    public float followRange;
    /// <summary>
    /// 停止距离
    /// </summary>
    public float stopRange;
    /// <summary>
    /// 传送距离
    /// </summary>
    public float teleportRange;
    /// <summary>
    /// PVE攻击距离
    /// </summary>
    public float pveAtkRange;
    /// <summary>
    /// PVE主人攻击距离
    /// </summary>
    public float pveMasterAtkRange;
    /// <summary>
    /// PVE回归距离
    /// </summary>
    public float pveReturnRange;
    /// <summary>
    /// PVP攻击距离
    /// </summary>
    public float pvpAttackRange;
    /// <summary>
    /// PVP主任攻击距离
    /// </summary>
    public float pvpMasterAtkRange;
    /// <summary>
    /// PVP回归距离
    /// </summary>
    public float pvpDefenseRange;
    /// <summary>
    /// 前置冷却列表
    /// </summary>
    public List<int> cdList = new List<int>();
    /// <summary>
    /// 佣兵技能列表
    /// </summary>
    public List<int> skillList = new List<int>();
    /// <summary>
    /// 装备列表
    /// </summary>
    public List<int> equipList = new List<int>();
    /// <summary>
    /// 模型资源
    /// </summary>
    public List<int> modelList = new List<int>();
    /// <summary>
    /// 属性
    /// </summary>
    public List<EntourageProp> propList = new List<EntourageProp>();
    /// <summary>
    /// 预览位置
    /// </summary>
    public Vector3 previewPostion;
    /// <summary>
    /// 预览旋转
    /// </summary>
    public Vector3 previewRotation;
}


[System.Serializable]
public class EntourageProp {
    public ActorPropertyTag tag;
    //public int baseValue;
    public int growsValue;
}