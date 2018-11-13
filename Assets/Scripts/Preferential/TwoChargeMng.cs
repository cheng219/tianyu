//==================================
//作者：李邵南
//日期：2017/4/6
//用途：二冲活动管理类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;

public class TwoChargeMng
{
    #region 数据
    /// <summary>
    /// 是否开启二冲奖励功能
    /// </summary>
    public bool isCloseTwoCharge = false;
    protected bool isopen = true;
    /// <summary>
    /// 是否开启二冲奖励功能
    /// </summary>
    public bool isOpenTwoCharge
    {
        get
        {
            return isopen;
        }
        protected set
        {
            if (isopen != value)
            {
                isopen = value;
                if (OnOpenTwoChargeUpdate != null)
                    OnOpenTwoChargeUpdate();
            }
        }
    }
    /// <summary>
    /// 二冲奖励功能开启事件
    /// </summary>
    public System.Action OnOpenTwoChargeUpdate;
    /// <summary>
    /// 二冲的领取状态
    /// </summary>
    public int stage;
    private TwoChargeWnd twoChargeWnd;
    #endregion
    #region 构造
    public static TwoChargeMng CreateNew()
    {
        if (GameCenter.twoChargeMng == null)
        {
            TwoChargeMng twoChargeMng = new TwoChargeMng();
            twoChargeMng.Init();
            return twoChargeMng;
        }
        else
        {
            GameCenter.twoChargeMng.UnRegist();
            GameCenter.twoChargeMng.Init();
            return GameCenter.twoChargeMng;
        }
    }
    void Init()
    {
        MsgHander.Regist(0xC133, S2C_GetTwoChargeRewared);
    }
    void UnRegist()
    {
        MsgHander.UnRegist(0xC133, S2C_GetTwoChargeRewared);
        isCloseTwoCharge = false;
        stage = 0;
        isOpenTwoCharge = true;
    }
    #endregion
    #region 协议
    #region S2C
      /// <summary>
     /// 获得领取奖励关闭二冲奖励界面
     /// </summary>
     /// <param name="pt"></param>
    protected void S2C_GetTwoChargeRewared(Pt pt)
    {
        pt_update_second_recharge_reward_c133 msg = pt as pt_update_second_recharge_reward_c133;
        if (msg != null)
        {
            stage = msg.status;
            SetLoveRedRemind();
            if (stage == 1) 
            {
                TwoChargeRef TwoCharge = ConfigMng.Instance.GetTwoChargeRef(1);
                if (TwoCharge == null)
                    GameCenter.twoChargeMng.isOpenTwoCharge = false;
                if (OnOpenTwoChargeUpdate != null)
                    OnOpenTwoChargeUpdate();
            }
            else if (stage == 2)
            {
                isOpenTwoCharge = false;
                //GameCenter.uIMng.ReleaseGUI(GUIType.PRIVILEGE);
                if (twoChargeWnd != null) twoChargeWnd.CloseUI();
            }
        }
    }
    #endregion
    #region C2S
    /// <summary>
    /// 请求充值数和阶段
    /// </summary>
    public void C2S_ReqGetTwoChargeInfo()
    {
        pt_love_reward_request_d805 msg = new pt_love_reward_request_d805();
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求领取奖励
    /// </summary>
    public void C2S_ReqTakeReward()
    {
        pt_get_second_recharge_reward_c126 msg = new pt_get_second_recharge_reward_c126();
        NetMsgMng.SendMsg(msg);
    }
    #endregion
    #endregion
    /// <summary>
    /// 奖励红点
    /// </summary>
    public void SetLoveRedRemind()
    {
        bool isRed = false;
        if (stage != null && stage == 1 )
        {
                isRed = true;
        }
        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.TWOCHARGE, isRed);
    } 
}
