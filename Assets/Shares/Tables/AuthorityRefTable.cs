//==========================
//作者：龙英杰
//日期：2015/12/12
//用途：公会权限静态配置
//==========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AuthorityRefTable : AssetTable
{
    public List<AuthorityRef> infoList = new List<AuthorityRef>();
}

[System.Serializable]
public class AuthorityRef
{
    /// <summary>
    /// id
    /// </summary>
    public int id;
    /// <summary>
    /// 职位
    /// </summary>
    public string position;
    /// <summary>
    /// 邀请成员
    /// </summary>
    public int invitation;
    /// <summary>
    /// 建设科技
    /// </summary>
    public int development;
    /// <summary>
    /// 提出成员
    /// </summary>
    public int kickOut;
    /// <summary>
    /// 通过申请
    /// </summary>
    public int approval;
    /// <summary>
    /// 解散公会
    /// </summary>
    public int dissolution;
    /// <summary>
    /// 开启副本
    /// </summary>
    public int openRaid;
    /// <summary>
    /// 退出公会
    /// </summary>
    public int quit;
    /// <summary>
    /// 提升会长
    /// </summary>
    public int president;
    /// <summary>
    /// 提升副会长
    /// </summary>
    public int vicePresident;
    /// <summary>
    /// 提升长老
    /// </summary>
    public int elders;
    /// <summary>
    /// 降为成员
    /// </summary>
    public int member;
    /// <summary>
    /// 降职长老
    /// </summary>
    public int downElders;
    /// <summary>
    /// 捐献资源
    /// </summary>
    public int donate;
    /// <summary>
    /// 科技加成
    /// </summary>
    public int technologyAddition;
    /// <summary>
    /// 修改公告
    /// </summary>
    public int notice;

}
