//===============================
//日期：2016/4/29
//作者：鲁家旗
//用途描述:成就管理类
//===============================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
public class AchievementMng
{
    #region 数据
    /// <summary>
    /// 当前玩家所到的成就
    /// </summary>
    public FDictionary curhaveAchieve = new FDictionary();
    /// <summary>
    /// 所有的成就等级
    /// </summary>
    public FDictionary achieveNumDic = new FDictionary();
    /// <summary>
    /// 没领取的成就
    /// </summary>
    public FDictionary achievementTogRed = new FDictionary();
    /// <summary>
    /// 获取到总的成就数量后抛出事件
    /// </summary>
    public System.Action OnGetAchievementNum;
    /// <summary>
    /// 成就领取后抛出事件
    /// </summary>
    public System.Action OnAchievementUpdate;
    /// <summary>
    /// 获得新成就后抛出事件
    /// </summary>
    public System.Action<AchievementData> OnGetNewAchievement;
    /// <summary>
    /// 当前成就数量
    /// </summary>
    public int currentAchievementNum = 0;
    /// <summary>
    /// 是否显示红点
    /// </summary>
    protected bool redPoint = false;
    protected bool isGetAchieve = false;
    /// <summary>
    /// 是否收到成就信息
    /// </summary>
    public bool IsGetAchieve
    {
        get
        {
            return isGetAchieve;
        }
        set
        {
            isGetAchieve = value;
        }
    }
    #endregion

    #region 构造
    public static AchievementMng CreateNew(MainPlayerMng _father)
    {
        if (_father.achievementMng == null)
        {
            AchievementMng achievementMng = new AchievementMng();
            achievementMng.Init();
            return achievementMng;
        }
        else
        {
            _father.achievementMng.UnRegist();
            _father.achievementMng.Init();
            return _father.achievementMng;
        }
    }

    void Init()
    {
        MsgHander.Regist(0xD766, S2C_AchievementReachNum);
        MsgHander.Regist(0xD767, S2C_AchievementRewardState);
        MsgHander.Regist(0xD776, S2C_AchievementRedPoint);
        MsgHander.Regist(0xD783, S2C_AchievementToggleRed);
    }
    void UnRegist()
    {
        MsgHander.UnRegist(0xD766, S2C_AchievementReachNum);
        MsgHander.UnRegist(0xD767, S2C_AchievementRewardState);
        MsgHander.UnRegist(0xD776, S2C_AchievementRedPoint);
        MsgHander.UnRegist(0xD783, S2C_AchievementToggleRed);
        achieveNumDic.Clear();
        currentAchievementNum = 0;
        curhaveAchieve.Clear();
        redPoint = false;
        achievementTogRed.Clear();
        isGetAchieve = false;
    }
    #endregion

    #region 协议
    #region S2C 
    /// <summary>
    /// 获得已经达成成就的数量
    /// </summary>
    protected void S2C_AchievementReachNum(Pt _msg)
    {
        pt_update_achievement_reach_num_d766 msg = _msg as pt_update_achievement_reach_num_d766;
        if (msg != null)
        {
            achieveNumDic.Clear();
            for (int i = 0; i < msg.achievement_reach.Count; i++)
            {
                achievement_reach data = msg.achievement_reach[i];
                if (!achieveNumDic.ContainsKey(data.type))
                {
                    achieveNumDic[data.type] = data.amount;
                }
            }
        }
        if (OnGetAchievementNum != null)
        {
            OnGetAchievementNum();
        }
    }
    /// <summary>
    /// 获得成就达到的奖励领取状态
    /// </summary>
    protected void S2C_AchievementRewardState(Pt _msg)
    {
        pt_update_achievement_reward_d767 msg = _msg as pt_update_achievement_reward_d767;
        if (msg != null)
        {
            isGetAchieve = true;
            for (int i = 0; i < msg.achievement_reward.Count; i++)
            {
                achievement_reward data = msg.achievement_reward[i];
                if (achievementTogRed.ContainsKey(data.id) && data.state == 1)
                    achievementTogRed.Remove(data.id);
                AchievementRef achievementRef = ConfigMng.Instance.GetAchievementRef(data.id);
                currentAchievementNum = data.num;
                if (curhaveAchieve.ContainsKey(msg.achievement_reward[i].id))
                {
                    AchievementData info = curhaveAchieve[data.id] as AchievementData;
                    if (info != null)
                        info.Update(data);
                    //SetRedPoint();
                }
                else if (data.new_or_old == 1)//新达成成就
                {
                    AchievementData info = new AchievementData(data);
                    curhaveAchieve[info.AchieveId] = info;
                    if (OnGetNewAchievement != null)
                    {
                        OnGetNewAchievement(info);
                    }
                    SetRedPoint();
                }
                else if (data.state == 1)//退出游戏重进已领取的成就
                {
                    AchievementData info = new AchievementData(data);
                    curhaveAchieve[info.AchieveId] = info;
                }
                else if (data.id >= 41 && data.id <= 55)//退出游戏重进，已达成的成就，但是没有领取
                {
                    if (data.num >= 1)
                    {
                        AchievementData info = new AchievementData(data);
                        curhaveAchieve[info.AchieveId] = info;
                    }
                }
                else if (achievementRef != null && achievementRef.judgeNum2 != 0)
                {
                    if (data.num >= achievementRef.judgeNum2)
                    {
                        AchievementData info = new AchievementData(data);
                        curhaveAchieve[info.AchieveId] = info;
                    }
                }
                else if (achievementRef != null && data.num >= achievementRef.judgeNum1)
                {
                    AchievementData info = new AchievementData(data);
                    curhaveAchieve[info.AchieveId] = info;
                }
            }
        }
        if (OnAchievementUpdate != null)
        {
            OnAchievementUpdate();
        }
    }
    /// <summary>
    /// 是否显示红点
    /// </summary>
    /// <param name="_msg"></param>
    protected void S2C_AchievementRedPoint(Pt _msg)
    {
        pt_update_red_dot_d776 msg = _msg as pt_update_red_dot_d776;
        if (msg != null)
        {
            switch (msg.type)
            {
                //成就红点
                case 1: if (GameCenter.mainPlayerMng != null) GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.ACHIEVEMENT, msg.state == 1); break;
                //首冲红点
                case 2: if (GameCenter.mainPlayerMng != null) GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.FIRSTCHARGE, msg.state == 1); break;
                //开服活动红点
                case 3:
                    {
                        GameCenter.wdfActiveMng.C2S_AskAllActivitysInfo();
                        break;
                        //if (GameCenter.mainPlayerMng != null) GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.OPENACTIVE, msg.state == 1); break;
                    }
                case 5:
                    GameCenter.weekCardMng.isCanTakeLoginBunus = true;
                    GameCenter.weekCardMng.C2S_ReqGetLoginBonusInfo();
                    break;
            }
        }
    }
    /// <summary>
    /// 退出游戏后，没领取的成就，进入游戏显示红点
    /// </summary>
    protected void S2C_AchievementToggleRed(Pt _msg)
    {
        pt_achievement_red_dot_list_d783 msg = _msg as pt_achievement_red_dot_list_d783;
        if (msg != null)
        {
            achievementTogRed.Clear();
            for (int i = 0; i < msg.red_dot_list.Count; i++)
            {
                red_dot_list data = msg.red_dot_list[i];
                achievementTogRed[data.type] = data.state;
            }
            if (GameCenter.mainPlayerMng != null) GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.ACHIEVEMENT, achievementTogRed.Count != 0);
        }
    }
    #endregion

    #region C2S
    /// <summary>
    /// 请求获得成就信息
    /// </summary>
    public void C2S_ReqGetAchievementInfo(int _type)
    {
        //Debug.Log("请求获得成就信息" + _type);
        pt_req_achievement_info_d765 msg = new pt_req_achievement_info_d765();
        msg.type = _type;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求领取奖励
    /// </summary>
    /// <param name="_id"></param>
    public void C2S_ReqGetReward(int _id)
    {
        //Debug.Log("请求领取奖励 " + _id);
        pt_req_get_achievement_reward_d775 msg = new pt_req_get_achievement_reward_d775();
        msg.achievement_id = _id;
        NetMsgMng.SendMsg(msg);
    }
    #endregion

    #endregion

    #region 辅助逻辑
    /// <summary>
    /// 设置红点
    /// </summary>
    public void SetRedPoint()
    {
        if (curhaveAchieve.Count == 0) return;
        foreach (AchievementData data in curhaveAchieve.Values)
        {
            if (!data.RewardState)
            {
                redPoint = true;
                break;
            }
            else
                redPoint = false;
        }
        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.ACHIEVEMENT, redPoint);
    }
    /// <summary>
    /// 排序
    /// </summary>
    public List<AchievementData> GetAchievementDataList(int _type)
    {
        AchieveTypeRef typeData = ConfigMng.Instance.GetAchieveTypeRef(_type);
        List<AchievementData> list = new List<AchievementData>();
        if (typeData != null)
        {
            AchievementData achievementData = null;
            for (int i = 0; i < typeData.numId.Count; i++)
            {
                if (curhaveAchieve.ContainsKey(typeData.numId[i]))
                {
                    achievementData = curhaveAchieve[typeData.numId[i]] as AchievementData;
                }
                else
                {
                    achievementData = new AchievementData(typeData.numId[i]);
                }
                list.Add(achievementData);
            }
        }
        list.Sort(SortAchievementData);
        return list;
    }
    protected int SortAchievementData(AchievementData info1, AchievementData info2)
    {
        int state1 = 0;
        int state2 = 0;
        switch (info1.AchievementState)
        {
            case AchievementRewardState.CANGET:
                state1 = 1;
                break;
            case AchievementRewardState.CANTGET:
                state1 = 2;
                break;
            case AchievementRewardState.HAVEGOT:
                state1 = 3;
                break;
        }
        switch (info2.AchievementState)
        {
            case AchievementRewardState.CANGET:
                state2 = 1;
                break;
            case AchievementRewardState.CANTGET:
                state2 = 2;
                break;
            case AchievementRewardState.HAVEGOT:
                state2 = 3;
                break;
        }
        if (state1 > state2)//先按奖励状态排序(可领取-不能领取-已领取)
            return 1;
        if (state1 < state2)
            return -1;
        if (info1.ID > info2.ID)//状态相同按ID排序
            return 1;
        if (info1.ID < info2.ID)
            return -1;
        return 0;
    }
    #endregion
}
public class AchievementData
{
    protected achievement_reward serverData;
    /// <summary>
    /// 通过服务端数据构造
    /// </summary>
    /// <param name="_data"></param>
    public AchievementData(achievement_reward _data)
    {
        serverData = _data;
        ID = serverData.id;
    }
    public int ID = 0;
    /// <summary>
    /// 静态配置构造
    /// </summary>
    public AchievementData(int _id)
    {
        ID = _id;
    }
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="_data"></param>
    public void Update(achievement_reward _data)
    {
        serverData.id = _data.id;
        serverData.num = _data.num;
        serverData.state = _data.state;
        serverData.new_or_old = _data.new_or_old;
    }
    /// <summary>
    /// ID
    /// </summary>
    public int AchieveId
    {
        get
        {
            return serverData.id;
        }
    }
    /// <summary>
    /// 当前成就数据
    /// </summary>
    public int AchievementNum
    {
        get
        {
            return serverData.num;
        }
    }
    public AchievementRewardState AchievementState
    {
        get
        {
            if (serverData == null)
                return AchievementRewardState.CANTGET;
            return (AchievementRewardState)serverData.state;
        }
    }
    /// <summary>
    /// 当前成就领取状态
    /// </summary>
    public bool RewardState
    {
        get
        {
            return serverData.state == 0 ? false : true;
        }
    }
    /// <summary>
    /// 是否是新成就
    /// </summary>
    public bool IsNewAchievement
    {
        get
        {
            return serverData.new_or_old == 1 ? true : false;
        }
    }
}
public enum AchievementRewardState
{
    NONE,
    /// <summary>
    /// 可领取
    /// </summary>
    CANGET = 0,
    /// <summary>
    /// 已领取
    /// </summary>
    HAVEGOT = 1,
    /// <summary>
    /// 不可领取
    /// </summary>
    CANTGET = 2,
}