//======================================================
//作者:朱素云
//日期:2016/7/13
//用途:周卡管理类
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class WeekCardMng
{
    #region 构造
    public static WeekCardMng CreateNew()
    {
        if (GameCenter.weekCardMng == null)
        {
            WeekCardMng weekCardMng = new WeekCardMng();
            weekCardMng.Init();
            return weekCardMng;
        }
        else
        {
            GameCenter.weekCardMng.UnRegist();
            GameCenter.weekCardMng.Init();
            return GameCenter.weekCardMng;
        }
    }

    protected void Init()
    {
        MsgHander.Regist(0xD921, S2C_GetPlayerWeekRecharge);
        MsgHander.Regist(0xD923, S2C_GetWeekRechargeReward);
        MsgHander.Regist(0xD981, S2C_GetLoginBonusInfo);
    } 

    protected void UnRegist()
    {
        MsgHander.UnRegist(0xD921, S2C_GetPlayerWeekRecharge);
        MsgHander.UnRegist(0xD923, S2C_GetWeekRechargeReward);
        MsgHander.UnRegist(0xD981, S2C_GetLoginBonusInfo);
        weekRecharge = 0;
        weekRewardDic.Clear();
        loginBonusData = null; 
        isOpenLoginbonus = true;
        isCanTakeLoginBunus = false;
    }
    /// <summary>
    /// 周卡奖励红点
    /// </summary>
    public void SetWeekRedRemind() 
    {
        bool isRed = false;  
        using (var state = weekRewardDic.GetEnumerator())
        {
            while (state.MoveNext())
            {
                if(state.Current.Value == (int)WeekRewardStatus.CANTAKE)
                { 
                    isRed = true;
                    break;
                } 
            }
        } 
        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.FINANCIAL, isRed);
    }
    #endregion


    #region 理财周卡
    /// <summary>
    /// 周卡充值额度
    /// </summary>
    public int weekRecharge = 0;
    /// <summary>
    /// 领取了的周卡类型和领取状态
    /// </summary>
    public Dictionary<int, int> weekRewardDic = new Dictionary<int, int>();
    /// <summary>
    /// 当前领取的周卡奖励
    /// </summary>
    public weekcard_info reward;
    /// <summary>
    /// 周卡充值变化事件
    /// </summary>
    public System.Action OnWeekRewardUpdate;

    #region S2C
    /// <summary>
    /// 获取玩家周卡充值信息
    /// </summary> 
    protected void S2C_GetPlayerWeekRecharge(Pt pt)
    {
        pt_reply_weekcard_info_d921 msg = pt as pt_reply_weekcard_info_d921;
        if (msg != null)
        {
            weekRecharge = (int)msg.rechrage_amount;
            //Debug.Log("周卡充值额度 ： " + weekRecharge);
            for (int i = 0; i < msg.weekcard_info.Count; i++)
            {
                //Debug.Log("周卡领取信息 周卡类型 ： " + msg.weekcard_info[i].type + "   , 周卡领取状态 ： " + msg.weekcard_info[i].status);
                int type = (int)msg.weekcard_info[i].type;
                weekRewardDic[type] = msg.weekcard_info[i].status;
            }
            SetWeekRedRemind();
        }
        if (OnWeekRewardUpdate != null) OnWeekRewardUpdate();
    }


    /// <summary>
    /// 获取周卡领取奖励
    /// </summary>
    /// <param name="pt"></param>
    protected void S2C_GetWeekRechargeReward(Pt pt)
    {
        pt_reply_get_weekcard_reward_d923 msg = pt as pt_reply_get_weekcard_reward_d923;
        if (msg != null)
        {
            if (weekRewardDic.ContainsKey((int)msg.type))
                weekRewardDic[(int)msg.type] = msg.status;
            SetWeekRedRemind();
        }
        if (OnWeekRewardUpdate != null) OnWeekRewardUpdate();
    }
    #endregion
     
    #region C2S
    /// <summary>
    /// 请求周卡充值信息
    /// </summary>
    public void C2S_ReqGetWeekInfo()
    {
        pt_req_weekcard_info_d920 msg = new pt_req_weekcard_info_d920();
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求领取周卡奖励
    /// </summary>
    public void C2S_ReqTakeWeekReward(int _id)
    { 
        pt_req_get_weekcard_reward_d922 msg = new pt_req_get_weekcard_reward_d922();
        msg.type = (uint)_id;
        NetMsgMng.SendMsg(msg);
    }
    #endregion

    #endregion

    #region 登陆红利

    public WdfActiveTypeData loginBonusData = null;
    public System.Action OnLoginBonusUpdate;
    protected bool isOpenLoginbonus = true;
    public System.Action OnOpenLoginBonusUpdate;
    /// <summary>
    /// 登陆红利是否关闭
    /// </summary>
    public bool isOpenLoginBonus
    {
        get
        {
            return isOpenLoginbonus;
        }
       set
        {
            if (isOpenLoginbonus != value)
            {
                isOpenLoginbonus = value;
                if (OnOpenLoginBonusUpdate != null)
                {
                    OnOpenLoginBonusUpdate();
                }
            }
        }
    }
    /// <summary>
    /// 是否激活红利
    /// </summary>
    public bool isCanTakeLoginBunus = false;
    #region S2C
    /// <summary>
    /// 获取玩家登陆红利信息
    /// </summary> 
    protected void S2C_GetLoginBonusInfo(Pt pt)
    { 
        pt_reply_login_dividend_info_d981 msg = pt as pt_reply_login_dividend_info_d981;
        if (msg != null)
        {  
            if (loginBonusData == null)
            {
                loginBonusData = new WdfActiveTypeData(msg);
            }
            else
            {
                loginBonusData.Update(msg);
            } 
            if (ConfigMng.Instance.GetDividendRefTable().Count <= msg.reward_ids.Count)//奖励领完
            {
                isOpenLoginBonus = false;
            }
            SetLoginBunusRedRemind();
            if (OnLoginBonusUpdate != null) OnLoginBonusUpdate();
        } 
    } 
     
    #endregion

    #region C2S
    /// <summary>
    /// 请求登陆红利信息
    /// </summary>
    public void C2S_ReqGetLoginBonusInfo()
    { 
        pt_req_login_dividend_info_d980 msg = new pt_req_login_dividend_info_d980();
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求领取登陆红利
    /// </summary>
    public void C2S_ReqTakeLoginBonus(int _id)
    {
        pt_req_login_dividend_reward_d982 msg = new pt_req_login_dividend_reward_d982();
        msg.reward_id = (uint)_id;
        NetMsgMng.SendMsg(msg);
    }
    #endregion

    public void SetLoginBunusRedRemind()
    {
        bool isred = false;
        if (loginBonusData != null)
        { 
            for (int i = 0, max= loginBonusData.details.Count; i < max; i++)
            {
                if (loginBonusData.details[i].total_reward_times >= loginBonusData.details[i].reward_times && loginBonusData.details[i].reward_times > 0)
                {
                    isred = true;
                    break;
                }
            }
        } 
        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.LOGINBONUS, isred);
    }

    #endregion
}
/// <summary>
/// 周卡类型
/// </summary>
public enum WeekRewardType
{
    /// <summary>
    /// 特惠周卡
    /// </summary>
    PRIVILEGES = 1,
    /// <summary>
    /// 至尊周卡
    /// </summary>
    SUPERME = 2, 
}
/// <summary>
/// 周卡领取状态
/// </summary>
public enum WeekRewardStatus
{ 
    /// <summary>
    /// 可以领取
    /// </summary>
    CANTAKE = 1,
    /// <summary>
    /// 已经领取
    /// </summary>
    ALREADTAKE = 2,
    /// <summary>
    /// 还不可领取
    /// </summary>
    UNTAKE = 3,
}
