//==============================================
//作者：黄洪兴
//日期：2016/6/18
//用途：充值管理类
//=================================================



using UnityEngine;
using System.Collections;
using st.net.NetBase;
using System;
using System.Collections.Generic;

public class RechargeMng
{
    /// <summary>
    /// 充值类型列表
    /// </summary>
    public List<RechargeRef> RechargeItemList
	{
		get
		{
			List<RechargeRef> needList = new List<RechargeRef>();
			List<RechargeRef> rechargeList = ConfigMng.Instance.RechargeRefList;
			for (int i = 0,max=rechargeList.Count; i < max; i++) {
#if UNITY_IOS
                if(GameCenter.instance.isIOS_JB)
                {
                    if (rechargeList[i] != null && !rechargeList[i].isIOS)
				    {
					    needList.Add(rechargeList[i]);
				    }
                }else
                {
				    if(rechargeList[i] != null && rechargeList[i].isIOS)
				    {
					    needList.Add(rechargeList[i]);
				    }
                }
#else
                if (rechargeList[i] != null && !rechargeList[i].isIOS)
				{
					needList.Add(rechargeList[i]);
				}
				#endif
			}
			return needList;
		}
	}
	/// <summary>
	/// 充值返利奖励数
	/// </summary>
	public int TestRechargeRewardDiamo = 0;
	public int TestRechargeRewardVipExp = 0;

    /// <summary>
    ///出售物品改变事件
    /// </summary>
    public Action OnSellEqChange;

    #region 构造
    /// <summary>
    /// 返回该管理类的唯一实例
    /// </summary>
    /// <returns></returns>
    public static RechargeMng CreateNew(MainPlayerMng _main)
    {
        if (_main.rechargeMng == null)
        {
            RechargeMng RechargeMng = new RechargeMng();
            RechargeMng.Init(_main);
            return RechargeMng;
        }
        else
        {
            _main.rechargeMng.UnRegist(_main);
            _main.rechargeMng.Init(_main);
            return _main.rechargeMng;
        }
    }
    /// <summary>
    /// 注册
    /// </summary>
    protected virtual void Init(MainPlayerMng _main)
    {
		MsgHander.Regist(0xDA01, S2C_OnGotOrderID);
		MsgHander.Regist(0xD819,S2C_OnGotTestChargeData);
    }
    /// <summary>
    /// 注销
    /// </summary>
    protected virtual void UnRegist(MainPlayerMng _main)
    {
		MsgHander.UnRegist(0xDA01, S2C_OnGotOrderID);
		MsgHander.UnRegist(0xD819,S2C_OnGotTestChargeData);
		TestRechargeRewardDiamo = 0;
		TestRechargeRewardVipExp = 0;
    }
    #endregion

    #region 通信S2C
	protected RechargeRef CurRechargeRef = null;
    protected string orderId = string.Empty;


    /// <summary>
    /// 获取冲值订单号
    /// </summary>
    protected void S2C_OnGotOrderID(Pt _pt)
    {
		pt_reply_order_da01 msg = _pt as pt_reply_order_da01;
		if (msg != null && CurRechargeRef != null)
        {
			//Debug.Log("订单号  ==   " + msg.order_id + "   充值ID = " + (int)CurRechargeRef.id);
			if(GameCenter.instance.isPlatform)
			{
                orderId = msg.order_id;
                LynSdkManager.Instance.UsrPayment(orderId, GameCenter.loginMng.Login_ID.ToString(), CurRechargeRef.id,
					CurRechargeRef.chargeName,CurRechargeRef.chargeAmount,CurRechargeRef.chargeDiamond);
			}
        }
    }
	/// <summary>
	/// 获取充值返利数据
	/// </summary>
	protected void S2C_OnGotTestChargeData(Pt _info)
	{
		//Debug.Log("S2C_OnGotTestChargeData");
		pt_recharge_benefit_result_d819 pt = _info as pt_recharge_benefit_result_d819;
		if(pt != null)
		{
			TestRechargeRewardDiamo = pt.diamo;
			TestRechargeRewardVipExp = pt.vipexp;
		}
	}

    protected void S2C_OnPaySuccess(Pt _pt)
    {
        pt_recharge_succ_d80a msg = _pt as pt_recharge_succ_d80a;
        if (msg != null)
        {
            int payId = (int)msg.type;
            RechargeRef rechargeRef = ConfigMng.Instance.GetRechargeRef(payId);
            if (rechargeRef != null)
            {
                //if(GameCenter.instance.isDataEyePattern)DCVirtualCurrency.paymentSuccess(orderId, "", rechargeRef.chargeAmount, "CNY", "普通");
            }
            else
            {
                Debug.LogError("找不到ID为:" + payId + "的RechargeRef");
            }
        }
    }
    #endregion

    #region C2S

    /// <summary>
    /// 向服务器请求：请求充值订单号（充值的id）
    /// </summary>
	public void C2S_RequestRecharge(RechargeRef _curInfo)
    {
		if(_curInfo == null)
		{
			Debug.LogError("充值数据为空!");
			return;
		}
		CurRechargeRef = _curInfo;
		//Debug.Log("C2S_RequestRecharge:" + _curInfo.id);
		pt_req_order_da00 msg = new pt_req_order_da00();
		msg.type = (uint)_curInfo.id;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 向服务器请求：请求充值订单号（充值的id,用于红利充值 add by zsy）
    /// </summary>
    public void C2S_RequestRecharge(int _rechargeId)
    {
        CurRechargeRef = ConfigMng.Instance.GetRechargeRef(_rechargeId);
        pt_req_order_da00 msg = new pt_req_order_da00();
        msg.type = (uint)_rechargeId;
        NetMsgMng.SendMsg(msg);
    }

	/// <summary>
	/// 请求充值返利奖励
	/// </summary>
	public void C2S_ReqTestChargeData()
	{
		//Debug.Log("C2S_ReqTestChargeData");
		pt_req_recharge_benefit_d818 msg = new pt_req_recharge_benefit_d818();
		NetMsgMng.SendMsg(msg);
	}

	/// <summary>
	/// 领取充值返利奖励
	/// </summary>
	public void C2S_ReqTestChargeReward()
	{
		//Debug.Log("C2S_ReqTestChargeReward");
		pt_req_get_recharge_benefit_d820 msg = new pt_req_get_recharge_benefit_d820();
		NetMsgMng.SendMsg(msg);
	}
    #endregion

    #region 辅助逻辑

   



    #endregion
}