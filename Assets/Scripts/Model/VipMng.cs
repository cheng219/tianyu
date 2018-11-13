//======================================================
//作者：唐源
//日期：2017/3/14
//用途：vip管理
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;
public class VipMng{
    #region 构造
    public static VipMng CreateNew()
    {
        if (GameCenter.vipMng!= null)
        {
            GameCenter.vipMng.UnRegister();
            GameCenter.vipMng.Init();
            return GameCenter.vipMng;
        }
        else
        {
            VipMng vipMng = new VipMng();
            GameCenter.vipMng = vipMng;
            GameCenter.vipMng.Init();
            return GameCenter.vipMng;
        }
    }
    /// <summary>
    /// 初始化注册
    /// </summary>
    void Init()
    {
        MsgHander.Regist(0xD329, S2C_VIPResult);
        MsgHander.Regist(0xD80B, S2C_RechargeFlag);
    }
    /// <summary>
    /// 注销
    /// </summary>
    void UnRegister()
    {
        MsgHander.UnRegist(0xD329, S2C_VIPResult);
        MsgHander.UnRegist(0xD80B, S2C_RechargeFlag);
        vipData = null;
        rechargeFlagDic.Clear();
    }
    #endregion
    #region 数据
    VIPDataInfo vipData;
    #endregion
    #region 事件
    /// <summary>
    /// vip 数据更新事件
    /// </summary>
    public System.Action OnVIPDataUpdate;
    /// <summary>
    /// 充值提示标志更新
    /// </summary>
    public System.Action OnRechargeFlageUpdate;
    #endregion
    #region 访问器
    public VIPDataInfo VipData
    {
        get
        {
            return vipData;
        }
    }
    public Dictionary<int, bool> rechargeFlagDic = new Dictionary<int, bool>();
    #endregion
    #region 协议
    #region C2S协议
    /// <summary>
    /// VIP奖励领取 by 唐源
    /// </summary>
    /// <param name="_instanceID"></param>
    public void C2S_VIPRewarde(int _instanceID)
    {
        pt_req_vip_reward_d328 msg = new pt_req_vip_reward_d328();
        msg.vip_lev = _instanceID;
        NetMsgMng.SendMsg(msg);
    }
    #endregion
    #region S2C协议
/// <summary>
    ///  add 唐源
    /// S2s the c VIP 数据.
    /// </summary>
    /// <param name="info">Info.</param>
    void S2C_VIPResult(Pt info)
    {
        pt_vip_info_d329 msg = info as pt_vip_info_d329;
        if (msg == null) return;
        vipData = new VIPDataInfo(msg);
        if (OnVIPDataUpdate != null)
        {
            OnVIPDataUpdate();
        }
    } 
    protected void S2C_RechargeFlag(Pt _pt)
    {
        pt_recharge_flag_d80b pt = _pt as pt_recharge_flag_d80b;
        if (pt != null)
        {  
            for (int i = 0; i < pt.flag_list.Count;i++ )
            {
                rechargeFlagDic[pt.flag_list[i].type] = pt.flag_list[i].flag == 1; 
            }
            if (OnRechargeFlageUpdate != null)
                OnRechargeFlageUpdate();
        }
    }
#endregion
    #endregion

}
