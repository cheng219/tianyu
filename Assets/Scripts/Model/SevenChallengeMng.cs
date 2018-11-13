//============================
//作者：唐源
//日期：2017/4/15
//用途：七日挑战管理类
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;
using System.Text;
public class SevenChallengeMng
{
    #region 构造
    public static SevenChallengeMng CreateNew()
    {
        if (GameCenter.sevenChallengeMng == null)
        {
            SevenChallengeMng sevenChallengeMng = new SevenChallengeMng();
            sevenChallengeMng.Init();
            return sevenChallengeMng;
        }
        else
        {
            GameCenter.sevenChallengeMng.UnRegister();
            GameCenter.sevenChallengeMng.Init();
            return GameCenter.sevenChallengeMng;
        }
    }
    /// <summary>
    /// 初始化(注册监听)
    /// </summary>
    protected void Init()
    {
        MsgHander.Regist(0xC131,S2C_GetSevenChallengeAll);
        MsgHander.Regist(0xC132, S2C_GetSevenChallengeSigle);
        isOpenSevenChallenge = false;
        isShowRedPoint = false;
    }
    /// <summary>
    /// 注销
    /// </summary>
    protected void UnRegister()
    {
        MsgHander.UnRegist(0xC131, S2C_GetSevenChallengeAll);
        MsgHander.UnRegist(0xc132, S2C_GetSevenChallengeSigle);
        rewardSevenChallenge.Clear();
        listInfo.Clear();
        listSingleInfo.Clear();
    }

    #endregion
    #region 数据
    /// <summary>
    /// 是否开放七日挑战功能(7日任务完成之后关闭窗口)
    /// </summary>
    private bool isOpenSevenChallenge;
    public bool OpenSevenChallenge
    {
        get
        {
            return isOpenSevenChallenge;
        }
    }
    /// <summary>
    /// 显示红点
    /// </summary>
    private bool isShowRedPoint;
    public bool ShowRedPoint
    {
        get
        {
            return isShowRedPoint;
        }
    }
    /// <summary>
    /// 当前挑战的天数
    /// </summary>
    /// <summary>
    private int curDay;
    public int CurDay
    {
        get
        {
            return curDay;
        }
    }
    /// <summary>
    /// 当前挑战系列任务完成的次数
    /// </summary>
    /// <summary>
    private int curTimes;
    public int CurTimes
    {
        get
        {
            return curTimes;
        }
    }
    /// 七天挑战的展示界面数据
    /// </summary>
    public List<seven_day_target_list> listInfo = new List<seven_day_target_list>();
    /// <summary>
    /// 七天挑战某天的显示数据
    /// </summary>
    public List<st.net.NetBase.single_day_info> listSingleInfo = new List<st.net.NetBase.single_day_info>();
    /// <summary>
    /// 当天奖励是否可以领取的字典
    /// </summary>
    public Dictionary<int, bool> rewardSevenChallenge = new Dictionary<int, bool>();
    #endregion
    #region 事件
    /// <summary>
    /// 七日挑战数据更新事件
    /// </summary>
    public Action updateSevenChallengeData;
    /// <summary>
    /// 更新具体的哪一天的数据
    /// </summary>
    public Action<int> updateSevenChallengeSingleUI;
    #endregion
    #region 协议
    #region C2S协议
    /// <summary>
    /// 请求七日目标数据的协议(发参数(1,0)表示请求外层的数据,(2，对应的天数)请求内层的数据)
    /// </summary>
    public void C2S_ReqSevenChallengeInfo(int _type,int _state)
    {
        //Debug.Log("发起获取七日挑战数据的请求");
        pt_req_senven_day_targe_info_c130 msg  = new pt_req_senven_day_targe_info_c130();
        msg.req_type = (uint)_type;
        msg.req_state = (uint)_state;
        NetMsgMng.SendMsg(msg);
    }
    #endregion
    #region S2C协议
    public void S2C_GetSevenChallengeAll(Pt _pt)
    {
        //Debug.Log("得到七日目标7天数据");
        pt_seven_day_target_rewards_info_c131 info = _pt as pt_seven_day_target_rewards_info_c131;

        if(info!=null)
        {
            listInfo.Clear();
            listInfo = info.seven_day_target;
            //Debug.Log("得到七日目标7天数据:" + listInfo.Count);
            rewardSevenChallenge.Clear();
            //listInfo.Sort();
            isOpenSevenChallenge = true;
            isShowRedPoint = false;
            for (int i=0;i<info.seven_day_target.Count;i++)
            {
                int state = (int)info.seven_day_target[i].reward_state;
                if (!rewardSevenChallenge.ContainsKey((int)info.seven_day_target[i].day_id))
                {
                    bool _sta = state == 1 ? true : false;
                    rewardSevenChallenge.Add((int)info.seven_day_target[i].day_id,_sta);
                }
                if (state == 1)
                    continue;
                else
                {
                    if ((int)info.seven_day_target[i].finish_num == 7)
                    {
                        //Debug.Log("第"+(i+1)+"天完成了："+ info.seven_day_target[i].finish_num+"次");
                        isShowRedPoint = true;
                    }
                }
                //Debug.Log("第几" + info.seven_day_target[i].day_id+"天，完成次数" + info.seven_day_target[i].finish_num+ ",是否已经领取奖励" + info.seven_day_target[i].reward_state);
            }
            if (updateSevenChallengeData != null)
            {
                updateSevenChallengeData();
            }
        }
    }
    public void S2C_GetSevenChallengeSigle(Pt _pt)
    {
        //Debug.Log("得到七日目标某一天的数据");
        curDay = 0;
        curTimes = 0; 
        pt_seven_day_single_target_c132 info = _pt as pt_seven_day_single_target_c132;
        if (info != null)
        {
            listSingleInfo.Clear();
            listSingleInfo = info.single_day_info;
            //Debug.Log("info.single_day_info.Count:" + info.single_day_info.Count);
            //for(int i=0,count = info.single_day_info.Count; i< count;i++)
            //{
            //    Debug.Log("任务：" + info.single_day_info[i].task_id + ",次数：" + info.single_day_info[i].task_num);
            //}
            curDay = (int)info.days;
            curTimes = (int)info.finish_num;
            //Debug.Log("curDay:" + curDay);
            //Debug.Log("curTimes:" + curTimes);
            if (updateSevenChallengeSingleUI != null)
            {
                updateSevenChallengeSingleUI(curDay);
            }
        }
    }
    public void UpdateState()
    {
        //if()
    }
    #endregion
    #endregion
}

