//=========================
//作者：龙英杰
//日期：2016/2/15
//用途：新手引导静态配置表
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewGuideRefTable : AssetTable
{
    public List<NewGuideRef> infoList = new List<NewGuideRef>();
}

[System.Serializable]
public class NewGuideRef
{
    /// <summary>
    /// 
    /// </summary>
    public int id;
    /// <summary>
    /// 锁屏，不锁屏
    /// </summary>
    public int islock;
    /// <summary>
    /// 引导步骤
    /// </summary>
    public int guildstep;
    /// <summary>
    /// 下一步ID
    /// </summary>
    public int nextId;
    /// <summary>
    /// 
    /// </summary>
    public int type;
    /// <summary>
    /// 窗口ID
    /// </summary>
    public int widgetID;
    /// <summary>
    /// 
    /// </summary>
    public int expression;
    /// <summary>
    /// 方向
    /// </summary>
    public int direction;
    /// <summary>
    /// 说明
    /// </summary>
    public string text;
    /// <summary>
    /// 触发等级
    /// </summary>
    public int level;
    /// <summary>
    /// 任务ID
    /// </summary>
    public int taskID;
    /// <summary>
    /// 任务步骤
    /// </summary>
    public int taskStep;
    ///// <summary>
    ///// 
    ///// </summary>
    //public Vector2 point;
    ///// <summary>
    ///// 
    ///// </summary>
   // public Vector2 range; 
} 