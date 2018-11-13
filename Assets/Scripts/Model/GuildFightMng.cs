//==============================================
//作者：黄洪兴
//日期：2016/5/12
//用途：公会战管理类
//=================================================



using UnityEngine;
using System.Collections;
using st.net.NetBase;
using System;
using System.Collections.Generic;

public class GuildFightMng
{



    public List<GuildFightRankItemInfo> ScoreList = new List<GuildFightRankItemInfo>();
    public string Champion;
    public List<GuildFightItemInfo> GuildListOne = new List<GuildFightItemInfo>();
    public List<GuildFightItemInfo> GuildListTwo = new List<GuildFightItemInfo>();
    public List<GuildFightItemInfo> GuildListThree = new List<GuildFightItemInfo>();

    /// <summary>
    /// 胜利工会的名字
    /// </summary>
    public string vectorGuildName;
    public bool isGuildFight = false;
    /// <summary>
    /// 当前获得的是第几届公会战
    /// </summary>
    public int GuildFightIndex=0;
    /// <summary>
    /// 公会战举办届数
    /// </summary>
    public int GuildFightNum=0;
    public RemainTime remainTime;

    public bool isOpen=false;
    public bool canGo=false;


    /// <summary>
    /// 公会战剩余时间更新事件
    /// </summary>
    public Action OnGetGuildFightTimeUpdate;
    /// <summary>
    /// 公会战积分信息更新事件
    /// </summary>
    public Action OnGetGuildFightRankUpdate;
    /// <summary>
    /// 获得了工会战信息
    /// </summary>
    public Action OnGetGuildFightInfo;
    /// <summary>
    /// 公会战届数变化事件
    /// </summary>
    public Action OnGuildFightNumUpdate;

    #region 构造
    /// <summary>
    /// 返回该管理类的唯一实例
    /// </summary>
    /// <returns></returns>
    public static GuildFightMng CreateNew(MainPlayerMng _main)
    {
        if (_main.guildFightMng == null)
        {
            GuildFightMng GuildFightMng = new GuildFightMng();
            GuildFightMng.Init(_main);
            return GuildFightMng;
        }
        else
        {
            _main.guildFightMng.UnRegist(_main);
            _main.guildFightMng.Init(_main);
            return _main.guildFightMng;
        }
    }
    /// <summary>
    /// 注册
    /// </summary>
    protected virtual void Init(MainPlayerMng _main)
    {
        MsgHander.Regist(0xD557, S2C_GetGuildFightInfo);
        MsgHander.Regist(0xD556, S2C_GetGuildFightScore);
        MsgHander.Regist(0xD558, S2C_GetGuildFightNum);
        MsgHander.Regist(0xD695, S2C_GetGuildFightRemainTime);
        MsgHander.Regist(0xD694, S2C_GetGuildFightEnd);
        //GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += ChangeAutoUseSkill;
    }
    /// <summary>
    /// 注销
    /// </summary>
    protected virtual void UnRegist(MainPlayerMng _main)
    {
        MsgHander.UnRegist(0xD557, S2C_GetGuildFightInfo);
        MsgHander.UnRegist(0xD556, S2C_GetGuildFightScore);
        MsgHander.UnRegist(0xD558, S2C_GetGuildFightNum);
        MsgHander.UnRegist(0xD695, S2C_GetGuildFightRemainTime);
        MsgHander.UnRegist(0xD694, S2C_GetGuildFightEnd);
        //GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= ChangeAutoUseSkill;
        isGuildFight = false;
        vectorGuildName = string.Empty;
    }
    #endregion

    #region 通信S2C

    /// <summary>
    /// 获得公会战结束协议
    /// </summary>
    /// <param name="_pt"></param>
    public void S2C_GetGuildFightEnd(Pt _pt)
    {
        pt_guild_battle_state_d694 pt = _pt as pt_guild_battle_state_d694;
        if(pt!=null)
        {
            isGuildFight = true;
            if (pt.state == 1)
            {
                //MessageST mst = new MessageST();
                //mst.messID = 332;
                //mst.delYes = delegate
                //{
                //    GameCenter.endLessTrialsMng.C2S_OutCopy(); 
                //};
                //GameCenter.messageMng.AddClientMsg(mst);
                //Debug.Log("1胜利工会名  " + pt.win_guild_name);
                vectorGuildName = pt.win_guild_name;
                GameCenter.uIMng.SwitchToUI(GUIType.ACTIVITYBALANCE);
            }
            else if (pt.state == 2)
            {
                //MessageST mst = new MessageST();
                //mst.messID = 333;
                //mst.delYes = delegate
                //{
                //    GameCenter.endLessTrialsMng.C2S_OutCopy();
                //};
                //GameCenter.messageMng.AddClientMsg(mst);
                //Debug.Log("2胜利工会名  " + pt.win_guild_name);
                vectorGuildName = pt.win_guild_name;
                GameCenter.uIMng.SwitchToUI(GUIType.ACTIVITYBALANCE);
            }

        }

        //Debug.Log("收到公会战结算消息");

    }





    /// <summary>
    /// 获得公会战剩余时间
    /// </summary>
    /// <param name="_pt"></param>
    public void S2C_GetGuildFightRemainTime(Pt _pt)
    {
        pt_guild_battle_rest_time_d695 pt = _pt as pt_guild_battle_rest_time_d695;
        if (pt != null)
        {
            RemainTime t = new RemainTime(pt.time, Time.time);
            remainTime = t;
            //Debug.Log("收到公会战剩余时间"+t.remainTime);
            if (OnGetGuildFightTimeUpdate != null)
                OnGetGuildFightTimeUpdate();

        }

    }



    /// <summary>
    /// 获得公会战信息
    /// </summary>
    /// <param name="_pt"></param>
    protected void S2C_GetGuildFightInfo(Pt _pt)
    {
        pt_guild_battle_info_ex_d557 pt = _pt as pt_guild_battle_info_ex_d557;
        Champion = string.Empty;
        GuildListOne.Clear();
        GuildListTwo.Clear();
        GuildListThree.Clear();
        GuildFightIndex = 0;
        if(pt!=null)
        {
            isOpen = false;
            canGo = pt.state != 1;
            GuildFightIndex = pt.index;
            if (pt.champion != null && pt.champion != string.Empty)
            {
                Champion = pt.champion;
            }
            if (pt.guild_battle_group_one != null)
            {
                for (int i = 0; i < pt.guild_battle_group_one.Count; i++)
                {
                   // GuildListOne[pt.guild_battle_group_one[i].name] = pt.guild_battle_group_one[i].state;
                 //   GuildListOne.Add();
                  GuildListOne.Add(new GuildFightItemInfo(pt.guild_battle_group_one[i].name,pt.guild_battle_group_one[i].state));
                  if(!isOpen)
                    isOpen = true;
                    //Debug.Log("第一组工会战名字" + pt.guild_battle_group_one[i].name);
                }
            }
            if (pt.guild_battle_group_two != null)
            {
                for (int i = 0; i < pt.guild_battle_group_two.Count; i++)
                {
                    //GuildListTwo[pt.guild_battle_group_two[i].name] = pt.guild_battle_group_two[i].state;
                    GuildListTwo.Add(new GuildFightItemInfo(pt.guild_battle_group_two[i].name, pt.guild_battle_group_two[i].state));
                    if (!isOpen)
                        isOpen = true;
                    //Debug.Log("第二组工会战名字" + pt.guild_battle_group_two[i].name);
                }
            }
            if (pt.guild_battle_group_three!= null)
            {
                for (int i = 0; i < pt.guild_battle_group_three.Count; i++)
                {
                    //GuildListThree[pt.guild_battle_group_three[i].name] = pt.guild_battle_group_three[i].state;
                    GuildListThree.Add(new GuildFightItemInfo(pt.guild_battle_group_three[i].name, pt.guild_battle_group_three[i].state));
                    if (!isOpen)
                        isOpen = true;
                    //Debug.Log("第三组工会战名字" + pt.guild_battle_group_three[i].name);
                }
            }

            if (OnGetGuildFightInfo != null)
                OnGetGuildFightInfo();

            //Debug.Log("获得公会战信息,届数为" + pt.index + "state为" + pt.state + " 冠军  " + pt.champion);
        }

    }

    /// <summary>
    /// 获得公会战举办届数
    /// </summary>
    /// <param name="_pt"></param>
    protected void S2C_GetGuildFightNum(Pt _pt)
    {
        pt_guild_battle_index_d558 pt = _pt as pt_guild_battle_index_d558;
        if(pt!=null)
        {
            if (pt.index_list != null)
            {
                GuildFightNum = pt.index_list.Count;
            }
            else
            {
                GuildFightNum = 0;

            }

            if (OnGuildFightNumUpdate != null)
                OnGuildFightNumUpdate();
        }

        //Debug.Log("收到公会战举办届数" + GuildFightNum);
    }


    /// <summary>
    /// 获得公会战积分信息
    /// </summary>
    /// <param name="_pt"></param>
    protected void S2C_GetGuildFightScore(Pt _pt)
    {
        pt_guild_battle_integer_d556 pt = _pt as pt_guild_battle_integer_d556;
        if(pt!=null)
        {
            ScoreList.Clear();
            if (pt.guild_battle_info!=null)
                for (int i = 0; i < pt.guild_battle_info.Count; i++)
            {
                ScoreList.Add(new GuildFightRankItemInfo(pt.guild_battle_info[i].uid, pt.guild_battle_info[i].name, pt.guild_battle_info[i].integer));
                //Debug.Log("ID" + pt.guild_battle_info[i].uid + "名字" + pt.guild_battle_info[i].name + "积分" + pt.guild_battle_info[i].integer);
            }

            if (OnGetGuildFightRankUpdate != null)
                OnGetGuildFightRankUpdate();

        }


        //Debug.Log("获得公会战积分信息");

    }



    #endregion

    #region C2S
    /// <summary>
    /// 请求公会战届数
    /// </summary>
    /// <param name="num"></param>
    public void C2S_AskGuildFightNum()
    {
        pt_req_guild_battle_index_d559 msg = new pt_req_guild_battle_index_d559();
        NetMsgMng.SendMsg(msg);
        //Debug.Log("请求公会战届数");
    }


    /// <summary>
    /// 请求公会战信息
    /// </summary>
    public void C2S_AskGuildFightInfo(int _index)
    {
        pt_req_guild_battle_info_d560 msg = new pt_req_guild_battle_info_d560();
        msg.index = _index;
        NetMsgMng.SendMsg(msg);
        //Debug.Log("请求第" + msg.index + "届公会信息");


    }

    /// <summary>
    /// 请求进入公会战
    /// </summary>
    public void C2S_AskEnterGuildFight()
    {
        pt_req_enter_guild_battle_d562 msg = new pt_req_enter_guild_battle_d562();
        NetMsgMng.SendMsg(msg);
        //Debug.Log("请求进入公会战");


    }


    #endregion

    #region 辅助逻辑


    #endregion
}

public class GuildFightItemInfo
{
    public string name;
    public int state;

    public GuildFightItemInfo(string _name,int _state)
    {
        this.name = _name;
        this.state = _state;
    }
    public GuildFightItemInfo()
    {
    }
}


public class RemainTime
{
    /// <summary>
    /// 剩余的时间
    /// </summary>
    public int remainTime;
    /// <summary>
    /// 收到时当前的时间
    /// </summary>
    public float getTime;
    public RemainTime(int _remainTime,float _getTime)
    {
        this.remainTime = _remainTime;
        this.getTime = _getTime;
    }

    public RemainTime()
    {

    }

}


public class GuildFightRankItemInfo
{
    public int id;
    public string name;
    public int score;

    public GuildFightRankItemInfo(int _id, string _name, int _score)
    {
        this.id = _id;
        this.name = _name;
        this.score = _score;
    }
    public GuildFightRankItemInfo()
    {
    }
}