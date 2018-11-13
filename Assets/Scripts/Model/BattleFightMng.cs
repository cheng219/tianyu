//======================================================
//作者:朱素云
//日期:2017/2/16
//用途:火焰山战场管理类
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class BattleFightMng
{
    /// <summary>
    /// 战场频道
    /// </summary>
    protected int channel = 0;
    public int Channel
    {
        get
        {
            return channel;
        }
        protected set
        {
            if (channel != value)
            {
                channel = value;
                if (OnChannelUpdate != null)
                    OnChannelUpdate();
            }
        }
    }
    protected int goBackCD = 0;
    /// <summary>
    /// 回城CD =0 可以点击回城 >0回城CD
    /// </summary>
    public int GoBackCD
    {
        get
        {
            return goBackCD;
        }
        protected set
        {
            if (goBackCD != value)
            {
                goBackCD = value;
                if (OnGoBackUpdate != null)
                    OnGoBackUpdate();
            }
        }
    }
    /// <summary>
    /// 战场排行
    /// </summary>
    public List<mountain_flames_rank> rankListInfo = new List<mountain_flames_rank>(); 
    /// <summary>
    /// 结算数据
    /// </summary>
    public SettlementData settlementData = null;
    /// <summary>
    /// 仙界战士状态
    /// </summary>
    public BattleCampInfo failyState = null;
    /// <summary>
    /// 妖界战士状态
    /// </summary>
    public BattleCampInfo demonState = null;
    /// <summary>
    /// 战场积分
    /// </summary>
    public List<mountain_amount_score> CampScore = new List<mountain_amount_score>();

    private int countDown = 0;
    public int CountDown
    {
        get
        {
            return countDown;
        }
        set
        {
            if (countDown != value)
            {
                countDown = value;
                if (OnCountDownUpdate != null) OnCountDownUpdate();
            }
        }
    }
    public System.Action OnCountDownUpdate;
    #region 构造
    /// <summary>
    /// 返回该管理类的唯一实例
    /// </summary>
    /// <returns></returns>
    public static BattleFightMng CreateNew()
    {
        if (GameCenter.battleFightMng == null)
        {
            BattleFightMng battleFightMng = new BattleFightMng();
            battleFightMng.Init();
            return battleFightMng;
        }
        else
        {
            GameCenter.battleFightMng.UnRegist();
            GameCenter.battleFightMng.Init();
            return GameCenter.battleFightMng;
        }
    }
    /// <summary>
    /// 注册
    /// </summary>
    protected virtual void Init()
    {
        MsgHander.Regist(0xC111, S2C_GetBattleSettlement);
        MsgHander.Regist(0xC109, S2C_UpdateBattleRank);
        MsgHander.Regist(0xC110, S2C_UpdateBattleInfo);
        MsgHander.Regist(0xC114, S2C_UpdateFightersInfo);
        MsgHander.Regist(0xC113, S2C_UpdateGoBackInfo);
        MsgHander.Regist(0xC115, S2C_GetBattleChannel);
        MsgHander.Regist(0xC149, S2C_GetBattleCountDown);
    }
    /// <summary>
    /// 注销
    /// </summary>
    protected virtual void UnRegist()
    {
        MsgHander.UnRegist(0xC111, S2C_GetBattleSettlement);
        MsgHander.UnRegist(0xC109, S2C_UpdateBattleRank);
        MsgHander.UnRegist(0xC110, S2C_UpdateBattleInfo);
        MsgHander.UnRegist(0xC114, S2C_UpdateFightersInfo);
        MsgHander.UnRegist(0xC113, S2C_UpdateGoBackInfo);
        MsgHander.UnRegist(0xC115, S2C_GetBattleChannel);
        MsgHander.UnRegist(0xC149, S2C_GetBattleCountDown);
        settlementData = null; 
        failyState = null;
        demonState = null;
        goBackCD = 0;
        channel = 0;
        CountDown = 0;
        CampScore.Clear();
        rankListInfo.Clear();
    }
    #endregion

    #region 事件 

    /// <summary>
    /// 战场排行更新
    /// </summary>
    public System.Action OnBattleRankUpdate;

    /// <summary>
    /// 战场阵营更新
    /// </summary>
    public System.Action OnBattleCampUpdate;

    /// <summary>
    /// 更新回城状态
    /// </summary>
    public System.Action OnGoBackUpdate;

    /// <summary>
    /// 更新战士们的状态1 仙界 2 妖界
    /// </summary>
    public System.Action<int> OnFightersUpdate;

    /// <summary>
    /// 战场频道更新
    /// </summary>
    public System.Action OnChannelUpdate;
     
    #endregion

    #region S2C
    /// <summary>
    /// 倒计时
    /// </summary> 
    private void S2C_GetBattleCountDown(Pt _pt)
    { 
        pt_mountain_of_flames_count_time_c149 pt = _pt as pt_mountain_of_flames_count_time_c149;
        if (pt != null)
        { 
            CountDown = pt.count_time;
        }
    }

    /// <summary>
    /// 获取火焰山战场结算数据
    /// </summary>
    private void S2C_GetBattleSettlement(Pt _pt)
    { 
        pt_mountain_flames_win_c111 pt = _pt as pt_mountain_flames_win_c111;
        if (pt != null)
        {
            //Debug.Log("  获取火焰山战场结算数据 ");
            if (settlementData == null)
            {
                settlementData = new SettlementData(pt);
            }
            else
            {
                settlementData.Update(pt);
            }
            GameCenter.uIMng.GenGUI(GUIType.BATTLEFIELDSETTMENT, true);
        } 
    }
    /// <summary>
    /// 更新火焰山战场排行数据
    /// </summary>
    private void S2C_UpdateBattleRank(Pt _pt)
    { 
        pt_update_mountain_flames_rank_c109 pt = _pt as pt_update_mountain_flames_rank_c109;
        if (pt != null)
        { 
            rankListInfo.Clear();

            rankListInfo = pt.mountain_flames_rank;
            rankListInfo.Sort(SortBattleInfo);

            if (OnBattleRankUpdate != null) OnBattleRankUpdate();
        } 
    }
    /// <summary>
    /// 更新火焰山战场积分信息
    /// </summary>
    private void S2C_UpdateBattleInfo(Pt _pt)
    { 
        pt_update_mountain_flames_score_c110 pt = _pt as pt_update_mountain_flames_score_c110;
        if (pt != null)
        { 
            CampScore.Clear();

            CampScore = pt.mountain_amount_score;

            if (OnBattleCampUpdate != null) OnBattleCampUpdate();
        }
    }

    /// <summary>
    /// 更新回城状态
    /// </summary>
    private void S2C_UpdateGoBackInfo(Pt _pt)
    { 
        pt_update_back_city_time_c113 pt = _pt as pt_update_back_city_time_c113;
        if (pt != null)
        { 
            GoBackCD = pt.surplus; 
        }
    }

    /// <summary>
    /// 更新将的战斗状态
    /// </summary>
    private void S2C_UpdateFightersInfo(Pt _pt)
    { 
        pt_update_general_state_c114 pt = _pt as pt_update_general_state_c114;
        
        if (pt != null)
        {
            if (failyState == null)
            {
                failyState = new BattleCampInfo(pt.fairyland_general);
                if (OnFightersUpdate != null) OnFightersUpdate(1);
            }
            else
            {
                failyState.Update(pt.fairyland_general);
                if (OnFightersUpdate != null) OnFightersUpdate(1);
            }
            if (demonState == null)
            {
                demonState = new BattleCampInfo(pt.demon_general);
                if (OnFightersUpdate != null) OnFightersUpdate(2);
            }
            else
            {
                demonState.Update(pt.demon_general);
                if (OnFightersUpdate != null) OnFightersUpdate(2);
            }  
        }
    }

    /// <summary>
    /// 第一次进入火焰山活动打开评分说明界面
    /// </summary>
    //private void S2C_OpenComentDesWnd(Pt _pt)
    //{
    //    pt_update_back_city_time_c113 pt = _pt as pt_update_back_city_time_c113;
    //    if (pt != null)
    //    {
    //        GameCenter.uIMng.GenGUI(GUIType.BATTLECOMENTDES,true);
    //    }
    //}

    /// <summary>
    /// 战场频道
    /// </summary>
    private void S2C_GetBattleChannel(Pt _pt)
    { 
        pt_update_battleground_id_c115 pt = _pt as pt_update_battleground_id_c115;
        if (pt != null)
        {
            //Debug.Log("战场频道 :   " + pt.battleground_id);
            Channel = pt.battleground_id;
        }
    }
    #endregion

    #region C2S

    /// <summary>
    /// 请求进入火焰山战场
    /// </summary>
    public void C2S_ReqFlyBattleFeild() 
    { 
        pt_req_fly_mountain_of_flames_c108 msg = new pt_req_fly_mountain_of_flames_c108();
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求回城
    /// </summary>
    public void C2S_ReqGoBack() 
    { 
        pt_req_back_to_city_c112 msg = new pt_req_back_to_city_c112();
        NetMsgMng.SendMsg(msg);
    } 
    #endregion

    #region 火焰山积分排序 
    protected int SortBattleInfo(mountain_flames_rank _data1, mountain_flames_rank _data2)
    {
        if (_data1.score > _data2.score)
        {
            return -1;
        }
        if (_data1.score < _data2.score)
        {
            return 1;
        }
        return 0;
    } 
    #endregion
}
 
/// <summary>
/// 火焰山阵营数据
/// </summary>
public class BattleCampInfo
{ 
    /// <summary>
    /// 主帅状态-1交战 0 无状态 >0复活中
    /// </summary>
    public int generalState; 
    /// <summary>
    /// 上将复活CD
    /// </summary>
    public int upGeneralStateCD; 
    /// <summary>
    /// 下将复活CD
    /// </summary>
    public int downGeneralStateCD;

    public BattleCampInfo(List<general_list> _general)
    {
        for (int i = 0, max = _general.Count; i < max; i++)
        {
            if (_general[i].general_type == 1)
            {
                generalState = _general[i].battle_state; 
            }
            else if (_general[i].general_type == 2)
            {
                upGeneralStateCD = _general[i].battle_state; 
            }
            else if (_general[i].general_type == 3)
            {
                downGeneralStateCD = _general[i].battle_state; 
            }
        }
    }
    public void Update(List<general_list> _general)
    {
        for (int i = 0, max = _general.Count; i < max; i++)
        {
            if (_general[i].general_type == 1)
            {
                generalState = _general[i].battle_state; 
            }
            else if (_general[i].general_type == 2)
            {
                upGeneralStateCD = _general[i].battle_state; 
            }
            else if (_general[i].general_type == 3)
            {
                downGeneralStateCD = _general[i].battle_state; 
            }
        }
    }
}


/// <summary>
/// 火焰山结算数据
/// </summary>
public class SettlementData
{
    /// <summary>
    /// 1 胜利 0 失败 -1平局
    /// </summary>
    public int fightState;
    /// <summary>
    /// 仙界战斗结果
    /// </summary>
    public List<mountain_flames_win> failyData = new List<mountain_flames_win>();
    /// <summary>
    /// 妖界战斗结果
    /// </summary>
    public List<mountain_flames_win> demonData = new List<mountain_flames_win>();

    public SettlementData(pt_mountain_flames_win_c111 _pt)
    {
        fightState = _pt.win_state;
        failyData = _pt.fairyland_list;
        demonData = _pt.demon_list;
        failyData.Sort(CompareList);
        demonData.Sort(CompareList);
    }
    public void Update(pt_mountain_flames_win_c111 _pt)
    {
        fightState = _pt.win_state;
        failyData = _pt.fairyland_list;
        demonData = _pt.demon_list;
        failyData.Sort(CompareList);
        demonData.Sort(CompareList);
    }

    int CompareList(mountain_flames_win item1,mountain_flames_win item2)
    {
        if (item1.amount_score > item2.amount_score)
        {
            return -1;
        }
        if (item1.amount_score < item2.amount_score)
        {
            return 1;
        }
        return 0;
    }
}