//====================
//作者：鲁家旗
//日期：2016/5/10
//用途：首冲大礼管理类
//====================
using UnityEngine;
using System.Collections;
using st.net.NetBase;
public class FirstChargeBonusMng
{
    #region 数据
    /// <summary>
    /// 领取奖励状态
    /// </summary>
    public int firstChargeBonusStates = 0;
    /// <summary>
    /// 领取奖励后抛出事件
    /// </summary>
    public System.Action OnFirstChargeBonus;
    #endregion

    #region 构造
    public static FirstChargeBonusMng CreateNew()
    {
        if (GameCenter.firstChargeBonusMng == null)
        {
            FirstChargeBonusMng firstChargeBonus = new FirstChargeBonusMng();
            firstChargeBonus.Init();
            return firstChargeBonus;
        }
        else
        {
            GameCenter.firstChargeBonusMng.UnRegist();
            GameCenter.firstChargeBonusMng.Init();
            return GameCenter.firstChargeBonusMng;
        }
    }
    void Init()
    {
        MsgHander.Regist(0xF002, S2C_GetFirstChageInfo);
    }
    void UnRegist()
    {
        MsgHander.UnRegist(0xF002, S2C_GetFirstChageInfo);
        firstChargeBonusStates = 0;
    }
    #endregion

    #region 协议
    #region S2C
    protected void S2C_GetFirstChageInfo(Pt _msg)
    {
        pt_ret_firstChargeRewardInfo_f002 msg = _msg as pt_ret_firstChargeRewardInfo_f002;
        if (msg != null)
        {
            firstChargeBonusStates = msg.reward_info;
            GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.FIRSTCHARGE, msg.reward_info == 1);
        }
        if (OnFirstChargeBonus != null)
        {
            OnFirstChargeBonus();
        }
    }
    #endregion

    #region C2S
    /// <summary>
    /// 请求获取首冲列表
    /// </summary>
    public void C2S_ReqGetFirstChargeInfo(int _action)
    {
        pt_action_d002 msg = new pt_action_d002();
        msg.action = _action;
        NetMsgMng.SendMsg(msg);
    }
    #endregion
    #endregion
}
/// <summary>
/// 首冲下行协议返回状态
/// </summary>
public enum FirstChargeState
{ 
    /// <summary>
    /// 没有完成首充
    /// </summary>
    NOTCHARGE = 0,
    /// <summary>
    /// 完成首充没有领奖
    /// </summary>
    HAVECHARGENOTGET = 1,
    /// <summary>
    /// 完成首充并领奖
    /// </summary>
    HAVECHARGEGET = 2,
}
