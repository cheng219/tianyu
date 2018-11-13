//==================================
//作者：朱素云
//日期：2016/5/5
//用途：等级/奖励每日签到管理类
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;

public class RankRewardMng 
{ 
    #region 每日奖励
    /// <summary>
    /// 每日奖励变化事件
    /// </summary>
    public System.Action OnRewardUpdate;
    /// <summary>
    /// 等级奖励变化事件
    /// </summary>
    public System.Action OnLevRewardUpdate;
    /// <summary>
    /// 连续签到的天数（每日奖励中服务端发来的当天可以领取的天数）
    /// </summary>
    public int rewardDay = 0;
    /// <summary>
    /// 连续签到领取的奖励列表
    /// </summary>
    public Dictionary<int, int> rewardDic = new Dictionary<int, int>();
    /// <summary>
    /// 等级奖励链表
    /// </summary>
    public Dictionary<int, int> levRewardDic = new Dictionary<int, int>();
    public List<LevelRewardLevelRef> rewardList = new List<LevelRewardLevelRef>();
    /// <summary>
    /// 是否是第一次登陆
    /// </summary>
    public bool isFirstPlay = false;

    /// <summary>
    /// 小对话框id
    /// </summary>
    public List<DialogueRef> dialogueRef = new List<DialogueRef>();
    #endregion

    #region 构造
    public static RankRewardMng CreateNew()
    {
        if (GameCenter.rankRewardMng == null)
        {
            RankRewardMng rankRewardMng = new RankRewardMng();
            rankRewardMng.Init();
            return rankRewardMng;
        }
        else
        {
            GameCenter.rankRewardMng.UnRegist();
            GameCenter.rankRewardMng.Init();
            return GameCenter.rankRewardMng;
        }
    } 
    
    protected void Init()
    { 
        MsgHander.Regist(0xD743, S2C_GetEverydayRewardList);
        MsgHander.Regist(0xD762, S2C_GetLevRewardList);
        MsgHander.Regist(0xC119, S2C_OpenDialogWnd);
        if (isEverdayRedRemind())
        {
            GameCenter.messageMng.SendPushInfo(1, 2);
        }
    }

    protected void UnRegist()
    { 
        MsgHander.UnRegist(0xD743, S2C_GetEverydayRewardList);
        MsgHander.UnRegist(0xD762, S2C_GetLevRewardList);
        MsgHander.UnRegist(0xC119, S2C_OpenDialogWnd);
        rewardDay = 0;
        rewardDic.Clear();
        levRewardDic.Clear();
        dialogueRef.Clear();
        isFirstPlay = false;
    }
    /// <summary>
    /// 等级奖励界面的红点
    /// </summary> 
    public void RereshRankRewardWnd()
    { 
        bool isOpen = false;
        List<LevelRewardLevelRef> stepDic = ConfigMng.Instance.GetLevelRewardList(GameCenter.mainPlayerMng.MainPlayerInfo.Prof);
        for (int i = 0, max = stepDic.Count; i < max; i++)
        {
            if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel >= stepDic[i].level)
            {
                if (!levRewardDic.ContainsKey(stepDic[i].level))
                {
                    isOpen = true;
                    break;
                }
            }
        } 
        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.LEVELREWARD, isOpen);
    }
    /// <summary>
    /// 设置每日登陆奖励红点
    /// </summary>
    public void SetEveyDayRedRemind()
    {
        bool isOpen = false;
        if (!rewardDic.ContainsKey(rewardDay))
        {  
            isOpen = true; 
        } 
        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.WELFARE, isOpen);
    }
    public bool isEverdayRedRemind()
    { 
        if (!rewardDic.ContainsKey(rewardDay))
        {
            return true;
        }
        return false;
    }

    public void GetRewardList()
    {
        rewardList = ConfigMng.Instance.GetLevelRewardList(GameCenter.mainPlayerMng.MainPlayerInfo.Prof);
        rewardList.Sort(SortRewardInfo); 
    } 

    protected int SortRewardInfo(LevelRewardLevelRef reward1, LevelRewardLevelRef reward2)
    {
        int playerLev = GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel;
        int state1 = 0;
        int state2 = 0;
        //已经领取 
        if (levRewardDic.ContainsKey(reward1.level)) state1 = 3; 
        //可领取
        else if (playerLev >= reward1.level) state1 = 1;
        //不可领取
        else state1 = 2; 
            
        //已经领取 
        if (levRewardDic.ContainsKey(reward2.level)) state2 = 3; 
        //可领取 
        else if (playerLev >= reward2.level)state2 = 1; 
        //不可领取
        else state2 = 2; 
            
        if (state1 > state2)//先按奖励状态排序(可领取-不能领取-已领取)
            return 1;
        if (state1 < state2)
            return -1;
        if (reward1.level > reward2.level)//状态相同按ID排序
            return 1;
        if (reward1.level < reward2.level)
            return -1;
        return 0;
    } 
    #endregion

    #region S2C  
    /// <summary>
    /// 获取每日登陆奖励列表
    /// </summary> 
    protected void S2C_GetEverydayRewardList(Pt _msg)
    {
        pt_everyday_reward_list_d743 msg = _msg as pt_everyday_reward_list_d743;
        if (_msg != null)
        { 
            rewardDic.Clear(); 
            rewardDay = msg.can_get_id;//可以领取的天数 
            isFirstPlay = msg.login_state == 1; 
            //isOpenDailyReward = msg.login_state == 1; 
            for (int i = 0 , max = msg.everyday_reward_list.Count; i < max; i++)
            { 
                int key = (int)msg.everyday_reward_list[i].type;
                int val = (int)msg.everyday_reward_list[i].num;
                if (val == (int)RewardState.TAKED)//字典存储已经领取的奖励
                {
                    rewardDic[key] = val;
                }  
            }
            SetEveyDayRedRemind(); 
        }
        if (OnRewardUpdate != null) OnRewardUpdate(); 
    }

    /// <summary>
    /// 获取等级奖励信息
    /// </summary>
    /// <param name="_msg"></param>
    protected void S2C_GetLevRewardList(Pt _msg)
    {
        pt_update_lev_reward_d762 msg = _msg as pt_update_lev_reward_d762;
        if (_msg != null)
        { 
            for (int i = 0, max = msg.lev_reward_list.Count; i < max; i++)
            { 
                if (!levRewardDic.ContainsKey(msg.lev_reward_list[i]))
                {
                    //Debug.Log(" 领取等级奖励 ：   " + msg.lev_reward_list[i]);
                    levRewardDic[msg.lev_reward_list[i]] = msg.lev_reward_list[i];
                }
            }
            GetRewardList();
            RereshRankRewardWnd();
        }
        if (OnLevRewardUpdate != null) OnLevRewardUpdate();
    }

    /// <summary>
    /// 打开对话框
    /// </summary>
    /// <param name="_msg"></param>
    protected void S2C_OpenDialogWnd(Pt _msg)
    {
        pt_update_little_window_c119 msg = _msg as pt_update_little_window_c119;
        if (_msg != null)
        {
            dialogueRef = ConfigMng.Instance.GetDialogueRef(msg.id);
            if (dialogueRef.Count > 0)
            {
                GameCenter.uIMng.GenGUI(GUIType.DIALOGBOX, true);
            }
            else
            {
                Debug.Log("小对话框找不到id 为" + msg.id + "的数据");
            }
        } 
    }
    #endregion

    #region C2S 
    /// <summary>
    /// 请求领取每日奖励
    /// </summary> 
    public  void C2S_ReqTakeEverydayReward(int _id)
    {
        //Debug.Log("领取当天奖励: " + _id); 
        pt_get_everday_reward_d755 msg = new pt_get_everday_reward_d755();
        msg.reward_id = _id;
        NetMsgMng.SendMsg(msg);
    }

    /// <summary>
    /// 请求获取等级奖励信息
    /// </summary> 
    public void C2S_ReqGetLevRewardInfo(RewardType _id)
    {
        pt_req_lev_reward_info_d760 msg = new pt_req_lev_reward_info_d760();
        msg.reward_type = (int)_id;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求领取等级奖励
    /// </summary> 
    public void C2S_ReqGetLevReward(int _lev)
    {
        //Debug.logger.Log("发送的领取等级 ： " + _lev);
        pt_req_get_lev_reward_d761 msg = new pt_req_get_lev_reward_d761();
        msg.get_lev = _lev;
        NetMsgMng.SendMsg(msg);
    }

    #region 礼包领取
    /// <summary>
    /// 请求领取礼包奖励
    /// </summary>
    /// <param name="_data"></param>
    public void C2S_ReqGetGiftReward(string _data)
    {
        pt_cdkey_d800 msg = new pt_cdkey_d800();
        msg.cdkey = _data;
        NetMsgMng.SendMsg(msg);
    }
    #endregion
    #endregion
}
public enum RewardType
{ 
    /// <summary>
    /// 等级奖励
    /// </summary>
    LEVREWARD = 1,
    /// <summary>
    /// 在线奖励
    /// </summary>
    ONLINEREWARD = 2,
}

/// <summary>
/// 领取状态
/// </summary>
public enum RewardState
{ 
    /// <summary>
    /// 未领取当天奖励
    /// </summary>
    NOTAKE = 0,
    /// <summary>
    /// 已经领取
    /// </summary>
    TAKED = 1, 
}

 
