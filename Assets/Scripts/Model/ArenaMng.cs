//============================
//作者：唐源
//日期：2017/2/4
//用途：竞技场管理类
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;
using System.Text;
public class ArenaMng {
    #region 构造
    public static ArenaMng CreateNew()
    {
        if (GameCenter.arenaMng == null)
        {
            ArenaMng arenaMng = new ArenaMng();
            arenaMng.Init();
            return arenaMng;
        }
        else
        {
            GameCenter.arenaMng.UnRegist();
            GameCenter.arenaMng.Init();
            return GameCenter.arenaMng;
        }
    }
    protected void Init()
    {
        MsgHander.Regist(0xD485, S2C_ArenaServerDataInfo);
        MsgHander.Regist(0xD489, S2C_ArenaServerReward);
    }
    protected void UnRegist()
    {
        MsgHander.UnRegist(0xD485, S2C_ArenaServerDataInfo);
        MsgHander.UnRegist(0xD489, S2C_ArenaServerReward);
        ArenaServerDataInfo = null;
    }
    #endregion
    #region 竞技场
    /// <summary>
    /// 竞技场数据
    /// </summary>
    public ArenaServerDataInfo ArenaServerDataInfo;
    /// <summary>
    /// 竞技场数据事件
    /// </summary>
    public System.Action OnArenaServerDataInfo;
    /// <summary>
    /// 竞技场奖励领取数据
    /// </summary>
    public System.Action OnArenaServerReward;

    /// <summary>
    /// 是否显示竞技场次数用光提示
    /// </summary>
    public bool ShowAreaTimeTip = true;

    void S2C_ArenaServerDataInfo(Pt _info)
    {
        pt_pk_info_d485 info = _info as pt_pk_info_d485;
        if (info != null)
        {
            //Debug.Log("S2C_ArenaServerDataInfo d485  " + info.rank + "  , surplus_time : " + info.surplus_time + "   ,  num : " + info.challenge_num);
            ArenaServerDataInfo = new ArenaServerDataInfo(info);
        }
        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.ARENA, ArenaServerDataInfo != null && ArenaServerDataInfo.challenge_num > 0 && ArenaServerDataInfo.surplus_time <= 0);
        if (ArenaServerDataInfo.surplus_time > 0)
        {
            GameCenter.instance.arenTime = Time.time;
            GameCenter.instance.IsSetArenRed = true;
            GameCenter.instance.arenSurPlusTime = ArenaServerDataInfo.surplus_time;
        }
        if (OnArenaServerDataInfo != null)
        {
            OnArenaServerDataInfo();
        }
    }
    public bool ArenRed()
    {
        return ArenaServerDataInfo != null ? ArenaServerDataInfo.challenge_num > 0 : false;
    }
    void S2C_ArenaServerReward(Pt _info)
    {
        pt_update_rank_reward_d489 info = _info as pt_update_rank_reward_d489;
        ArenaServerDataInfo.state = info.state;
        if (OnArenaServerReward != null)
        {
            OnArenaServerReward();
        }
    }
    /// <summary>
    /// 请求竞技场数据
    /// </summary>
    public void C2S_RepArenaServer()
    {
        pt_req_usr_pk_info_d483 info = new pt_req_usr_pk_info_d483();
        NetMsgMng.SendMsg(info);
    }
    /// <summary>
    /// 请求竞技场奖励数据
    /// </summary>
    public void C2S_RepArenaReward()
    {
        pt_req_get_rank_reward_d487 info = new pt_req_get_rank_reward_d487();
        NetMsgMng.SendMsg(info);
    }
    /// <summary>
    /// 请求PK对象
    /// </summary>
    public void C2S_RepArenaKill(int uid)
    {
        pt_req_pk_challenge_d490 info = new pt_req_pk_challenge_d490();
        info.challenge_uid = uid;
        NetMsgMng.SendMsg(info);
    }
    /// <summary>
    /// 请求购买竞技场次数
    /// </summary>
    public void C2S_ReqBuyChallengeTimes()
    {
        pt_req_buy_challenge_times_d798 info = new pt_req_buy_challenge_times_d798();
        NetMsgMng.SendMsg(info);
    }
    #endregion
}
