//=============================
//作者：唐源
//日期：2017/3/10
//用途：离线奖励的管理
//=============================
using UnityEngine;
using st.net.NetBase;
using System.Collections;
using System.Collections.Generic;
public class OffLineRewardMng{
    #region 构造
    public static OffLineRewardMng CreateNew()
    {
        if(GameCenter.offLineRewardMng!=null)
        {
            GameCenter.offLineRewardMng.UnRegister();
            GameCenter.offLineRewardMng.Init();
            return GameCenter.offLineRewardMng;
        }
        else
        {
            OffLineRewardMng offLineRewardMng = new OffLineRewardMng();
            GameCenter.offLineRewardMng = offLineRewardMng;
            GameCenter.offLineRewardMng.Init();
            return GameCenter.offLineRewardMng;
        }
    }
    /// <summary>
    /// 初始化注册
    /// </summary>
    void Init()
    {
        MsgHander.Regist(0xC118, S2C_GetRewardInfo);
        rewardData = new OffLineRewardRef();
    }
    /// <summary>
    /// 注销
    /// </summary>
    void UnRegister()
    {
        MsgHander.UnRegist(0xC118, S2C_GetRewardInfo);
        rewardData = null;
        OffLineTime = 0;
        amountTime = 0;
        IsOpenWnd = false;
    }
    #endregion
    #region 数据
    /// <summary>
    /// 离线修炼的开始时间
    /// </summary>
    private int offLineTime;
    /// <summary>
    /// 离线奖励的累计时间
    /// </summary>
    private int amountTime;
    /// <summary>
    /// 离线奖励的累计时间
    /// </summary>离线奖励的静态配置数据
    private OffLineRewardRef rewardData;
    /// <summary>
    /// 是否开启离线奖励窗口(与七天奖励一样在CharacterWnd脚本运行的时候判断是否打开窗口)
    /// </summary>
    public bool IsOpenWnd = false;
    #endregion
    #region 属性
    public int OffLineTime
    {
        get
        {
            return offLineTime;
        }
        private set
        {
            offLineTime = value;
        }
    }
    public int AmountTime
    {
        get
        {
            return amountTime;
        }
        private set
        {
            amountTime = value;
        }
    }
    public OffLineRewardRef RewardData
    {
        get
        {
            return rewardData;
        }
        private set
        {
            rewardData = value;
        }
    }
    #endregion
    #region 事件
    #endregion
    #region 协议
    #region C2S协议
    /// <summary>
    /// 领取离线奖励的请求协议
    /// </summary>
    public void C2S_RequestReward()
    {
        //Debug.Log("领取离线奖励"); 
        pt_req_get_offline_reward_c117 msg = new pt_req_get_offline_reward_c117();
        NetMsgMng.SendMsg(msg);
    }
    #endregion
    #region S2C协议
    /// <summary>
    /// 离线奖励数据协议(上线后由服务器推送 有奖励就推送 无不推送)
    /// </summary>
    void S2C_GetRewardInfo(Pt _info)
    { 
        pt_offline_reward_info_c118 pt = _info as pt_offline_reward_info_c118;
        if(pt!=null)
        {
            //Debug.Log("收取离线奖励的协议" + pt.offline_reward.Count);
            IsOpenWnd = false;
            if (pt.start_time == 0)
                return;
                OffLineTime = pt.start_time;
            if (pt.amount_time==0)
                return;
            AmountTime = pt.amount_time;
            if(pt.offline_reward.Count > 0)
            {
                //Debug.Log("有离线奖励");
                for (int i = 0; i < pt.offline_reward.Count; i++)
                {
                    ItemValue item = new ItemValue((int)pt.offline_reward[i].type, (int)pt.offline_reward[i].num);
                    rewardData.rewardList.Add(item);
                }
                IsOpenWnd = true;
            }
            else
            {
                Debug.LogError("离线奖励列表为空找郑梧孜");
            }

        }
    }
    #endregion
    #endregion
}
