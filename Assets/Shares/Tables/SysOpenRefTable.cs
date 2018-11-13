//===========================
//作者：龙英杰
//日期：2016/2/1
//用途：系统开启静态配置
//===========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SysOpenRefTable : AssetTable
{
    public List<SysOpenRef> infoList = new List<SysOpenRef>();
}


[System.Serializable]
public class SysOpenRef
{
    /// <summary>
    /// ID
    /// </summary>
    public int id;
    /// <summary>
    /// 系统名字
    /// </summary>
    public string sysName;
    /// <summary>
    /// 图标
    /// </summary>
    public string icon;
    /// <summary>
    /// 字体在图集中的名字
    /// </summary>
    public string font;
    /// <summary>
    /// 系统ID
    /// </summary>
    public int sysId;
    /// <summary>
    /// 开启任务
    /// </summary>
    public int openTask;
    /// <summary>
    /// 开启等级
    /// </summary>
    public int openLvl;
    /// <summary>
    /// 标签
    /// </summary>
    public int tab;
    /// <summary>
    /// 任务状态：UnTake = 2    /// 未接Process = 1   /// 已接受未完成 Finished = 3  /// 已完成未交
    /// </summary>
    public int step;
    /// <summary>
    /// 引导ID
    /// </summary>
    public int guildId;
    /// <summary>
    /// 提醒任务ID
    /// </summary>
    public int noticeTask;
    /// <summary>
    /// 提醒等级
    /// </summary>
    public int noticeLevel;
    /// <summary>
    /// 飞向位置
    /// </summary>
    public int position;
   // public Vector3 position;

}
