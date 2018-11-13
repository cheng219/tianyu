//====================
//作者：鲁家旗
//日期：2016/4/29
//用途：七天奖励管理类
//====================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
public class SevenDayMng
{
    #region 数据
    /// <summary>
    /// 七天奖励数据
    /// </summary>
    public Dictionary<int, SevendDayData> sevendDic = new Dictionary<int, SevendDayData>();
    /// <summary>
    /// 领取奖励后抛出事件
    /// </summary>
    public System.Action OnRewardChange;
    /// <summary>
    /// 第几天
    /// </summary>
    public int day = 1;

    //protected bool isFirst = true;
    public bool notGetAll = true;
    #endregion

    #region 构造
    public static SevenDayMng CreateNew()
    {
        if (GameCenter.sevenDayMng == null)
        {
            SevenDayMng sevenDayMng = new SevenDayMng();
            sevenDayMng.Init();
            return sevenDayMng;
        }
        else
        {
            GameCenter.sevenDayMng.UnRegist();
            GameCenter.sevenDayMng.Init();
            return GameCenter.sevenDayMng;
        }
    }
    void Init()
    {
        MsgHander.Regist(0xF001, S2C_GetSevenDayRewardInfo);
    }
    void UnRegist()
    {
        MsgHander.UnRegist(0xF001, S2C_GetSevenDayRewardInfo);
        sevendDic.Clear();
        day = 1;
        //GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= RefreshSevenOpen;
    }
    #endregion

    #region 协议
    #region S2C
    protected void S2C_GetSevenDayRewardInfo(Pt _msg)
    {
        pt_ret_sevenDayRewardsInfo_f001 msg = _msg as pt_ret_sevenDayRewardsInfo_f001;
        if (msg != null)
        {
            day = msg.day;
            if (day > 7) day = 7;
            //Debug.Log("七天奖励数量   " + msg.rewards_info.Count);
            for (int i = 0; i < msg.rewards_info.Count; i++)
            {
                sevenDayReward data = msg.rewards_info[i];
                if (sevendDic.ContainsKey((int)data.type))
                {
                    SevendDayData info = sevendDic[(int)data.type];
                    if (info != null)
                        info.Update(data);
                }
                else
                {
                    SevendDayData info = new SevendDayData(data);
                    sevendDic[info.Type] = info;
                }
            }
            //if (isFirst && GameCenter.mainPlayerMng != null && GameCenter.mainPlayerMng.MainPlayerInfo != null)
            //{
            //    GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= RefreshSevenOpen;
            //    GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += RefreshSevenOpen;
            //    isFirst = false;
            //}
            RefreshSevenOpen();
            //红点显示
            SetRedPoint();
        }
        if (OnRewardChange != null)
        {
            OnRewardChange();
        }
    }
    #endregion

    #region C2S
    /// <summary>
    /// 请求获得七天列表
    /// </summary>
    public void ReqGetSevenday()
    {
        pt_action_d002 msg = new pt_action_d002();
        msg.action = 2001;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求领取奖励 1是vip奖励 0是普通奖励
    /// </summary>
    public void ReqGetReward(int _day,bool _vip)
    {
        pt_action_two_int_d012 msg = new pt_action_two_int_d012();
        msg.action = 2002;
        msg.data_One = _day;
        msg.data_Two = _vip ? 1 : 0;
        NetMsgMng.SendMsg(msg);
    }
    #endregion
    #endregion

    #region 辅助逻辑
    /// <summary>
    /// 登录时是否弹出七天奖励
    /// </summary>
    /// <returns></returns>
    public bool IsSevendDayTrue()
    {
        if (GameCenter.mainPlayerMng != null && GameCenter.mainPlayerMng.MainPlayerInfo != null && GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel < 14) 
            return false;
        bool isRedPoint = false;
        for (int i = 1; i <= day; i++)
        {
            if (sevendDic.ContainsKey(i))
            {
                if (BoolVip(i) && sevendDic[i].Vip == 0)//vip等级达到，vip奖励没领取
                {
                    isRedPoint = true;
                    break;
                }
                else
                    isRedPoint = false;
            }
            else
            {
                isRedPoint = true;
                break;
            }
        }
        return isRedPoint;
    }
    /// <summary>
    /// vip等级是否达到
    /// </summary>
    /// <param name="_day"></param>
    /// <returns></returns>
    public bool BoolVip(int _day)
    {
        SevenDaysRef data = ConfigMng.Instance.GetSevenDayRef(GameCenter.mainPlayerMng.MainPlayerInfo.Prof, _day);
        int vipLimit = _day;
        if (data != null)
            vipLimit = data.vipLimit;
        if (GameCenter.vipMng.VipData.vLev >= vipLimit)
        {
            return true;
        }
        else
            return false;
    }
    /// <summary>
    /// 是否显示红点
    /// </summary>
    /// <returns></returns>
    public void SetRedPoint()
    {
        bool isRedPoint = false;
        for (int i = 1; i <= day; i++)
        {
            if (sevendDic.ContainsKey(i))
            {
                if (BoolVip(i) && sevendDic[i].Vip == 0)//vip等级达到，vip奖励没领取
                {
                    isRedPoint = true;
                    break;
                }
                else
                    isRedPoint = false;
            }
            else
            {
                isRedPoint = true;
                break;
            }
        }
        if (GameCenter.openServerRewardMng.wdfTaroatData != null)
        {
            GameCenter.openServerRewardMng.SetTaroatRedRemind();
        }
        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.SEVENDAYS, isRedPoint);
    }
    /// <summary>
    /// 七天奖励的开启，十级后开启
    /// </summary>
    //public void RefreshSevenOpen(ActorBaseTag tag, ulong value, bool _fromAbility)
    //{
    //    if (tag == ActorBaseTag.Level)
    //    {
    //        if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel >= 20) return;
    //        if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel < 14)
    //            GameCenter.mainPlayerMng.SetServerActiveOpen(FunctionType.SEVENDAYS, false);
    //        else
    //        {
    //            GameCenter.mainPlayerMng.SetServerActiveOpen(FunctionType.SEVENDAYS, true);
    //        }
    //    }
    //}
    /// <summary>
    /// 所有奖励领取完，隐藏
    /// </summary>
    public void RefreshSevenOpen()
    {
        if (sevendDic.Count < 7) return;
        foreach (SevendDayData data in sevendDic.Values)
        {
            if (data.Vip == 0)
            {
                notGetAll = true;
                break;
            }
            else
                notGetAll = false;
        }
        GameCenter.mainPlayerMng.SetServerActiveOpen(FunctionType.SEVENDAYS, notGetAll);
    }
    
    #endregion
}
public class SevendDayData
{
    protected sevenDayReward serverdata;
    /// <summary>
    /// 通过服务端数据构造
    /// </summary>
    /// <param name="_data"></param>
    public SevendDayData(sevenDayReward _data)
    {
        serverdata = _data;
    }
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="_data"></param>
    public void Update(sevenDayReward _data)
    {
        serverdata.type = _data.type;
        serverdata.normal = _data.normal;
        serverdata.vip = _data.vip;
    }
    /// <summary>
    /// 天数
    /// </summary>
    public int Type
    {
        get
        {
            return (int)serverdata.type;
        }
    }
    /// <summary>
    /// 普通奖励领取状况
    /// </summary>
    public int Normal
    {
        get
        {
            return serverdata.normal;
        }
    }
    /// <summary>
    /// vip奖励领取状况
    /// </summary>
    public int Vip
    {
        get
        {
            return serverdata.vip;
        }
    }
}
public enum RewardGetState
{ 
    /// <summary>
    /// 没有领取奖励
    /// </summary>
    NOTGETREWARD = 0,
    /// <summary>
    /// 已经领取奖励
    /// </summary>
    GETREWARD = 1,
}