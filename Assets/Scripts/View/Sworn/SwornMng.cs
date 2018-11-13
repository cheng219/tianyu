//==================================
//作者：朱素云
//日期：2016/5/4
//用途：结义管理类
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class SwornMng
{
    #region 数据
    protected OpenType open = OpenType.NONE;
    /// <summary>
    /// 是否自动打开结义界面
    /// </summary>
    public OpenType isOpenSworn
    {
        get
        {
            return open;
        } 
        set
        {
            if (open != value)
            {
                open = value;
                if (null != open)
                {
                    if (OnOpenSwornUpdate != null)
                        OnOpenSwornUpdate();
                }
            } 
        }
    }
    /// <summary>
    /// 结义任务链表
    /// </summary>
    public List<TaskInfo> swornTask
    {
        get
        { 
			return GameCenter.taskMng.GetAllTaskDic(TaskType.Daily);
        }
    }
    /// <summary>
    /// 当前结义数据
    /// </summary>
    public SwornData data = null;
    /// <summary>
    /// 完成结义任务与奖励领取(积分点和领取奖励状态1已领0未领)
    /// </summary>
    public List<brother_reward_info> isTakeAwared = new List<brother_reward_info>();
    /// <summary>
    /// 任务进度（任务id,完成的任务字典（任务id和任务step））
    /// </summary> 
    public Dictionary<int, Dictionary<int, int>> taskProgress = new Dictionary<int, Dictionary<int, int>>(); 
    #endregion 

    /// <summary>
    /// 结义数据变化事件
    /// </summary>
    public System.Action OnSwornListUpdata; 
    /// <summary>
    /// 打开结义界面事件
    /// </summary>
    public System.Action OnOpenSwornUpdate;

    #region 构造
    public static SwornMng CreateNew()
    {
        if (GameCenter.swornMng == null)
        {
            SwornMng swornMng = new SwornMng();
            swornMng.Init();
            return swornMng;
        }
        else
        {
            GameCenter.swornMng.UnRegist();
            GameCenter.swornMng.Init();
            return GameCenter.swornMng;
        }
    }

    protected void Init()
    {
        MsgHander.Regist(0xD540, S2C_GetSwornWndInfo);
        MsgHander.Regist(0xD543, S2C_GetRewardInfo);
        MsgHander.Regist(0xD547, S2C_BrokeUp);
        MsgHander.Regist(0xD690, S2C_GetSwornBrother);
    }

    protected void UnRegist()
    {
        MsgHander.UnRegist(0xD540, S2C_GetSwornWndInfo);
        MsgHander.UnRegist(0xD543, S2C_GetRewardInfo);
        MsgHander.UnRegist(0xD547, S2C_BrokeUp);
        MsgHander.UnRegist(0xD690, S2C_GetSwornBrother);
        data = null;
        isTakeAwared.Clear();
    }
    /// <summary>
    /// 判断是否和某人结义
    /// </summary> 
    public bool IsSwornWithSomeone(int _someoneId)
    {
        if (data != null)
        {
            for (int i = 0; i < data.brothers.Count; i++)
            {
                if (data.brothers[i].uid == _someoneId)
                    return true;
            }
            return false;
        }
        else
            return false;
    }
    /// <summary>
    /// 是否与某某结义的某某名字
    /// 结义成员的等级必须≥30级；
    /// 仙侣之间不能结义；
    /// 双方必须都是没有结义过； 
    /// </summary> 
    public List<string> HisName()
    {
        List<string> list = new List<string>();
        //Debug.Log("队伍人数 ： " + GameCenter.teamMng.TeammateCount);
        if (GameCenter.teamMng.isLeader)
        {
            if (!GameCenter.teamMng.isInTeam || GameCenter.teamMng.TeammateCount <1)
            {
                GameCenter.messageMng.AddClientMsg(344);
            }
            else
            {
                foreach (TeamMenberInfo team in GameCenter.teamMng.TeammatesDic.Values)
                {
                    if (team.baseInfo.uid != GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
                    {
                        list.Add(team.baseInfo.name);
                    }
                }
            }
        }
        else
        {
            GameCenter.messageMng.AddClientMsg(350);
        }
        return list;
    }

    #endregion

    #region S2C
    /// <summary>
    /// 通知结义成功
    /// </summary> 
    protected void S2C_GetSwornBrother(Pt pt)
    {
        pt_sworn_success_d690 msg = pt as pt_sworn_success_d690;
        if (msg != null)
        {
            isOpenSworn = OpenType.SWORN;
        }
    }
    /// <summary>
    /// 获取结义界面信息
    /// </summary>
    protected void S2C_GetSwornWndInfo(Pt pt)
    { 
        pt_wsorn_brother_info_d540 msg = pt as pt_wsorn_brother_info_d540;
        if (msg != null)
        {
            //for (int i = 0; i < msg.brothers_info.Count; i++)
            //{
            //    Debug.Log("   d540结义兄弟id：" + msg.brothers_info[i].uid + "      ，结义任务数：" + swornTask.Count + "  , 积分 ： " + msg.brothers_frendship_integer);
            //}
            if (msg.brothers_info.Count <= 0)
            {
                data = null;
            }
            else
            {
                if (data == null)
                {
                    data = new SwornData(msg);
                }
                else
                {
                    data.Updata(msg);
                }
            }
        }  
        if (OnSwornListUpdata != null) OnSwornListUpdata();
    }
    /// <summary>
    /// 宝箱领取情况和人物完成情况
    /// </summary> 
    protected void S2C_GetRewardInfo(Pt pt)
    {
        pt_brother_reward_info_d543 msg = pt as pt_brother_reward_info_d543;
        if (msg != null)
        {
            isTakeAwared.Clear();
            for (int i = 0; i < msg.reward_list.Count; i++)
            {
                //Debug.Log("积分等级  ：  " + msg.reward_list[i] + "     ， 宝箱是否领取：" + msg.reward_list[i].state);
                isTakeAwared.Add(msg.reward_list[i]);
            }
            for (int i = 0; i < msg.task_finish_list.Count; i++)
            { 
                int id = msg.task_finish_list[i].uid;
                List<sworn_task> list = msg.task_finish_list[i].sworn_task_list;
                Dictionary<int, int> dic = new Dictionary<int,int>();
                //Debug.Log("人物id查看完成了几条结义任务 :    " + id);
                for (int j = 0; j < list.Count; j++)
                {
                    //Debug.Log("     完成的任务id : " + list[j].task_id + "    , 任务step : " + list[j].task_step);
                    dic[list[j].task_id] = list[j].task_step;
                }
                taskProgress[id] = dic;
            }
        }
        if (OnSwornListUpdata != null) OnSwornListUpdata();
    }
    /// <summary>
    /// 分道扬镳成功
    /// </summary>
    protected  void S2C_BrokeUp(Pt pt)
    {
        pt_req_break_brother_d547 msg = pt as pt_req_break_brother_d547;
        if (msg != null)
        {
            data = null;
        }
        if (OnSwornListUpdata != null) OnSwornListUpdata();
    }
    #endregion

    #region C2S
    /// <summary>
    /// 请求结义界面信息
    /// </summary>
    public void C2S_ReqGetSwornInfo()
    {
        pt_req_sworn_friend_info_d541 msg = new pt_req_sworn_friend_info_d541();
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求结义(誓言)
    /// </summary>
    public void C2S_ReqAddBrother(string _oath)
    {
        pt_req_add_brother_d542 msg = new pt_req_add_brother_d542();
        msg.oath = _oath;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求修改誓言
    /// </summary>
    public void C2S_ReqChangeOath(string _oath)
    {
        pt_req_change_oath_d548 msg = new pt_req_change_oath_d548();
        msg.oath = _oath;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求积分奖励 宝箱信息
    /// </summary> 
    public void C2S_ReqBoxReward()  
    {
        pt_req_get_brother_reward_info_d546 msg = new pt_req_get_brother_reward_info_d546(); 
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求领取奖励
    /// </summary>
    public void C2S_ReqTakeBoxReward(int id)
    {
        pt_req_get_brother_reward_d545 msg = new pt_req_get_brother_reward_d545();
        msg.lev = id;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求分道扬镳
    /// </summary>
    public void C2S_ReqBrokeUp()
    { 
        pt_req_break_brother_d547 msg = new pt_req_break_brother_d547();
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求赠送美酒给他
    /// </summary>
    public void C2S_ReqSendWine(int id)
    {
        pt_req_send_drink_to_friend_d544 msg = new pt_req_send_drink_to_friend_d544();
        msg.target_uid = id;
        NetMsgMng.SendMsg(msg);
    }
    #endregion
}
public enum OpenType
{ 
    NONE = 0,
    /// <summary>
    /// 结义
    /// </summary>
    SWORN = 1,
    /// <summary>
    /// 结婚
    /// </summary>
    COUPLE = 2,
}
