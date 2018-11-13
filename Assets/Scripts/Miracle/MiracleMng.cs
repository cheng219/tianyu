//==================================
//作者：李邵南
//日期：2017/3/12
//用途：奇缘系统管理类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;

public class MiracleMng
{
    #region 数据
    /// <summary>
    /// 剩余的时间
    /// </summary>
    public int restTime;
    /// <summary>
    /// 目标对象名字
    /// </summary>
    public string targetName;
    /// <summary>
    /// 目标战力
    /// </summary>
    public uint scoreTarget;
    /// <summary>
    /// 本身战力
    /// </summary>
    public uint scoreSelf; 
    /// <summary>
    /// 目标玩家Id,0表示假数据，1表示真数据
    /// </summary>
    public int playerID;
    /// <summary>
    /// 奇缘系统状态，1是可接，2是已经接取，3是成功，4是已完成
    /// </summary>
    public AccessState miracleStatus = AccessState.NONE;
    /// <summary>
    /// 职业
    /// </summary>
    public int profInfo;
    /// <summary>
    /// 奖励物品信息
    /// </summary>
    public EquipmentInfo info;
    /// <summary>
    /// 获胜奖励物品信息
    /// </summary>
    public EquipmentInfo info1;
    /// <summary>
    /// 奇缘信息变化事件
    /// </summary>
    public System.Action OnMiracleDataUpdateEvent;
    /// <summary>
    /// 奇缘入口信息变化事件
    /// </summary>
    public System.Action OnMiracleEnterUpdateEvent;
    #endregion

    #region 构造
    public static MiracleMng CreateNew()
    {
        if (GameCenter.miracleMng == null)
        {
            MiracleMng newMiracle = new MiracleMng();
            newMiracle.Init();
            return newMiracle;
        }
        else
        {
            GameCenter.miracleMng.UnRegist();
            GameCenter.miracleMng.Init();
            return GameCenter.miracleMng;
        }
    }
    void Init()
    {
        MsgHander.Regist(0xD950, S2C_ReplyMiracleData);
    }
    void UnRegist()
    {
        MsgHander.UnRegist(0xD950, S2C_ReplyMiracleData);
        miracleStatus = AccessState.NONE;
    }
    #endregion

    #region 协议
    #region S2C
    protected void S2C_ReplyMiracleData(Pt _msg)
    {
        pt_hidden_task_info_d950 msg = _msg as pt_hidden_task_info_d950;
        if (msg != null) 
        {
            restTime = (int)msg.rest_time + (int)Time.realtimeSinceStartup;
            scoreSelf = msg.score_self;
            //目标玩家ID
            playerID = (int)msg.uid;
            targetName = msg.name;
            scoreTarget = msg.score_target;
            //奇缘系统枚举状态
            miracleStatus = (AccessState)msg.status;
            //玩家职业
            profInfo = msg.prof;
            //奖励信息获取
            info = new EquipmentInfo(5, 5000,EquipmentBelongTo.PREVIEW);
            info1 = new EquipmentInfo(2200027, 1, EquipmentBelongTo.PREVIEW);
            if (OnMiracleDataUpdateEvent != null)
                OnMiracleDataUpdateEvent();
            if (OnMiracleEnterUpdateEvent != null)
                OnMiracleEnterUpdateEvent();
            if (miracleStatus == AccessState.ACHIEVE)
            {
                GameCenter.uIMng.SwitchToUI(GUIType.NONE);
            }
        }
    }
    #endregion
    #region C2S
    /// <summary>
    /// 请求接任务数据
    /// </summary>
    public void C2S_ReqRoyalMiracleList()
    {
        pt_hidden_task_accept_d951 msg = new pt_hidden_task_accept_d951();
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求领取奖励
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_type"></param>
    public void C2S_ReqGetReward()
    {
        pt_hidden_task_finish_d952 msg = new pt_hidden_task_finish_d952();
        NetMsgMng.SendMsg(msg);
    }
    #endregion
    #endregion
}
#region 枚举变量
//奇缘系统的状态
public enum AccessState
{
    NONE,
    /// <summary>
    /// 可接
    /// </summary>
    ACCESS = 1,
    /// <summary>
    /// 已接受
    /// </summary>
    ACCEPTED = 2,
    /// <summary>
    /// 成功
    /// </summary>
    SUCCEED=3,
    /// <summary>
    /// 完成
    /// </summary>
    ACHIEVE=4
}
#endregion