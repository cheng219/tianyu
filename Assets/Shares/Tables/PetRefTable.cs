//=========================
//作者：龙英杰
//日期：2015/11/10
//用途：宠物静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PetRefTable : AssetTable
{
    public List<PetRef> infoList = new List<PetRef>();
}


[System.Serializable]
public class PetRef
{
    /// <summary>
    /// 宠物ID
    /// </summary>
    public int id;
    /// <summary>
    /// 宠物对应物品
    /// </summary>
    public int petItemId;
    /// <summary>
    /// 宠物名字
    /// </summary>
    public string petName;
    /// <summary>
    /// 预览的时候，模型缩放
    /// </summary>
    public float previewScale;
    /// <summary>
    /// 预览的时候，模型位置
    /// </summary>
    public Vector3 previewPostion = Vector3.zero;
    /// <summary>
    /// 预览的时候，模型旋转角度
    /// </summary>
    public Vector3 previewRotation = Vector3.zero;
    /// <summary>
    /// buff1
    /// </summary>
    public int buffOne;
    /// <summary>
    /// buff2
    /// </summary>
    public int buffTwo;
    /// <summary>
    /// 怪物预览ID
    /// </summary>
    public int previewsMonsterId;
    /// <summary>
    /// buff1说明
    /// </summary>
    public string buffOneDes;
    /// <summary>
    /// buff2说明
    /// </summary>
    public string buffTwoDes;
    /// <summary>
    /// 宠物说明
    /// </summary>
    public string petDes;
    /// <summary>
    /// 骨骼名
    /// </summary>
    public string boneName;
    /// <summary>
    /// 骨骼资源
    /// </summary>
    public string petBone;
    /// <summary>
    /// 基础速度
    /// </summary>
    public float baseMoveSpd;
    /// <summary>
    /// 步伐距离
    /// </summary>
    public float paceSpeed;
    /// <summary>
    /// 跟随距离
    /// </summary>
    public float followRange;
    /// <summary>
    /// 拾取范围
    /// </summary>
    public float collectRange;
    /// <summary>
    /// 休闲表演时间，宠物进入idle状态指定时间后，会播放scream动作，然后再次进入idle状态
    /// </summary>
    public float idleTimes;
    /// <summary>
    /// 模型缩放比例
    /// </summary>
    public float modelScale;
    /// <summary>
    /// 休闲状态的表演动作
    /// </summary>
    public string funAction;
    /// <summary>
    /// 外形装备
    /// </summary>
    public List<int> petModelList = new List<int>();

}
