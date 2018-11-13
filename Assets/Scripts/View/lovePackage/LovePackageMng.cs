//======================================================
//作者:朱素云
//日期:2016/7/11
//用途:爱心大礼包管理类
//======================================================
using UnityEngine;
using System.Collections;
using st.net.NetBase;

public class LovePackageMng
{
    #region 数据 
    /// <summary>
    /// 领取首冲后关闭界面打开爱心奖励
    /// </summary>
    public bool isCloseFirstBonus = false;
    protected bool isOpen = false;
    /// <summary>
    /// 是否开启爱心奖励功能
    /// </summary>
    public bool isOpenLoveReward
    {
        set
        {
            if (isOpen != value)
            { 
                isOpen = value;
                if (OnOpenLoveRechargeUpdate != null)
                    OnOpenLoveRechargeUpdate();
            }
        }
        get
        {
            return isOpen;
        }
    }
    protected int recharge = 0;
    /// <summary>
    /// 玩家充值额度
    /// </summary>
    public int rechargeVal 
    {
        get
        {
            return recharge;
        }
        protected set
        {
            if (recharge != value)
            {
                recharge = value;
                if (OnRechargeUpdate != null)
                    OnRechargeUpdate();
            }
        }
    }
    /// <summary>
    /// 充值阶段
    /// </summary>
    public int stage = 1;
    /// <summary>
    /// 玩家充值额度变化事件
    /// </summary>
    public System.Action OnRechargeUpdate;
    /// <summary>
    /// 爱心奖励功能开启事件
    /// </summary>
    public System.Action OnOpenLoveRechargeUpdate;

    #endregion

    #region 构造
    public static LovePackageMng CreateNew()
    {
        if (GameCenter.lovePackageMng == null)
        {
            LovePackageMng lovePackageMng = new LovePackageMng();
            lovePackageMng.Init();
            return lovePackageMng;
        }
        else
        {
            GameCenter.lovePackageMng.UnRegist();
            GameCenter.lovePackageMng.Init();
            return GameCenter.lovePackageMng;
        }
    }

    protected void Init()
    {
        MsgHander.Regist(0xD806, S2C_GetPlayerRecharge);
        MsgHander.Regist(0xD808, S2C_GetLoveRewared);
    }

    protected void UnRegist()
    {
        MsgHander.UnRegist(0xD806, S2C_GetPlayerRecharge);
        MsgHander.UnRegist(0xD808, S2C_GetLoveRewared);
        rechargeVal = 0;
        isCloseFirstBonus = false;
        stage = 1;
        isOpen = false;
    }
    /// <summary>
    /// 爱心奖励红点
    /// </summary>
    public void SetLoveRedRemind()
    { 
        int prof = GameCenter.mainPlayerMng.MainPlayerInfo.Prof;
        LoveSpreeRef love = ConfigMng.Instance.GetLoveSpreeRef( prof, stage);
        bool isRed = false;
        if (love != null)
        {
            if (rechargeVal >= love.money)
            {
                isRed = true;
            }
        }
        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.LOVEREWARD, isRed);
    } 
    #endregion

    #region S2C
    /// <summary>
    /// 获取玩家充值额度
    /// </summary> 
    protected void S2C_GetPlayerRecharge(Pt pt) 
    {
        pt_love_reward_d806 msg = pt as pt_love_reward_d806;
        if (msg != null)
        {
            rechargeVal = msg.diamo;
            stage = msg.stage;
            SetLoveRedRemind(); 
        } 
        if (OnRechargeUpdate != null) OnRechargeUpdate();
    }
    /// <summary>
    /// 获得领取奖励关闭爱心奖励界面
    /// </summary> 
    protected void S2C_GetLoveRewared(Pt pt) 
    {
        pt_love_reward_list_d808 msg = pt as pt_love_reward_list_d808;
        if (msg != null)
        {
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
            int prof = GameCenter.mainPlayerMng.MainPlayerInfo.Prof; 
            LoveSpreeRef nextLove = ConfigMng.Instance.GetLoveSpreeRef(prof, stage + 1);
            if (nextLove == null)
                GameCenter.lovePackageMng.isOpenLoveReward = false;
        } 
    }

    #endregion


    #region C2S 
    /// <summary>
    /// 请求充值数和阶段
    /// </summary>
    public void C2S_ReqGetLoveInfo()
    {
        pt_love_reward_request_d805 msg = new pt_love_reward_request_d805(); 
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求领取爱心奖励
    /// </summary>
    public void C2S_ReqTakeReward(int _id)
    { 
        pt_love_reward_ok_d807 msg = new pt_love_reward_ok_d807();
        msg.id = _id;
        NetMsgMng.SendMsg(msg);
    }
    #endregion
}
