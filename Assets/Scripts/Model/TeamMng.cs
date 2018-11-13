//====================================================
//作者：吴江
//日期：2015/11/10
//用途：组队管理类
//
//
//修改：贺丰
//日期：2016/1/12
//
//=============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;


public class TeamMenberInfo
{
	//info.baseInfo.robot_state 标记改队员是否化身 by 何明军
    public st.net.NetBase.team_member_list baseInfo;
    public System.Action OnBaseInfoUpdate;

    public TeamMenberInfo(st.net.NetBase.team_member_list _baseInfo)
    {
        baseInfo = _baseInfo;
    }

    public void SetBaseInfo(st.net.NetBase.team_member_list _baseInfo)
    {
        baseInfo = _baseInfo;
        if (OnBaseInfoUpdate != null)
        {
            OnBaseInfoUpdate();
        }
    }
	/// <summary>
	/// 亲密度
	/// </summary>
	/// <value>The close number.</value>
	public int CloseNum
	{
		get
		{
			return baseInfo == null?0:(int)baseInfo.lev;
		}
	}
}

public class TeamMng
{


    public enum TeamState
    {
        FreeJoin,
        ApplyJoin
    }

    protected int teamId = 0;
    /// <summary>
    /// 队伍ID
    /// </summary>
    public int TeamId
    {
        get
        {
            return teamId;
        }
        protected set
        {
            if (teamId != value)
            {
                teamId = value;
            }
        }
    }
    protected int leaderId = 0;
    /// <summary>
    /// 队长ID
    /// </summary>
    public int LeaderId
    {
        get
        {
            return leaderId;
        }
        protected set
        {
            if (leaderId != value)
            {
                leaderId = value;
            }
        }
    }

    protected int teamTargetID = 0;
    /// <summary>
    /// 队伍目标
    /// </summary>
    public int TeamTargetID
    {
        get
        {
            return teamTargetID;
        }
        protected set
        {
            if (teamTargetID != value)
            {
                teamTargetID = value;
            }
        }
    }
    protected int limitLv = 10;
    /// <summary>
    /// 最低等级限定
    /// </summary>
    public int LimitLv
    {
        get
        {
            return limitLv;
        }
        set
        {
            if (limitLv != value)
            {
                limitLv = value;
            }
        }
    }

    protected int limitNumb = 3;
    /// <summary>
    /// 人数限定
    /// </summary>
    public int LimitNumb
    {
        get
        {
            return limitNumb;
        }
        set
        {
            if (limitNumb != value)
            {
                limitNumb = value;
            }
        }
    }
    protected TeamState joinMode = TeamState.ApplyJoin;
    /// <summary>
    /// 加入方式
    /// </summary>
    public TeamState JoinMode
    {
        get
        {
            return joinMode;
        }
        set
        {
            if (joinMode != value)
            {
                joinMode = value;
            }
        }
    }
    /// <summary>
    /// 队伍成员，不包含自己
    /// </summary>
    protected FDictionary teammatesDic = new FDictionary();
    /// <summary>
    /// 队伍成员列表
    /// </summary>
    public FDictionary TeammatesDic
    {
        get
        {
            return teammatesDic;
        }
    }
    /// <summary>
    /// 队伍是否已满
    /// </summary>
    public bool TeamIsFull
    {
        get
        {
			return teammatesDic.Count >= limitNumb;
        }
    }

    /// <summary>
    /// 附近队伍
    /// </summary>
    protected FDictionary nearTeam = new FDictionary();
    /// <summary>
    /// 附近队伍
    /// </summary>
    public FDictionary NearTeam
    {
        get
        {
            return nearTeam;
        }
    }
    /// <summary>
    /// 显示队伍界面时间
    /// </summary>
    public Action OpenTeamWndEvent;

    /// <summary>
    /// 队伍信息变更事件
    /// </summary>
    public Action onTeammateUpdateEvent;
    /// <summary>
    /// 附近队伍信息更新
    /// </summary>
    public Action onUpdateNearTeam;
    /// <summary>
    /// 副本中队友添加好友按钮显示事件
    /// </summary>
    public Action<bool> doAddFriendShowEvent;
    /// <summary>
    /// 玩家在线状态改变的事件
    /// </summary>
    public Action<int, bool> onUpdateOnLineEvent;
    /// <summary>
    /// 更新队友观看剧情动画的状态
    /// </summary>
    public Action<int, bool> onUpdateSceneAnimEvent;
    /// <summary>
    /// 更新队友是否活着的事件
    /// </summary>
    public Action<int, bool> onUpdateAliveEvent;
    /// <summary>
    /// 更新队友名字颜色
    /// </summary>
    public Action<int, bool> onUpdateNameColorEvent;
    /// <summary>
    /// 第一个参数是sceneId,第二个参数1表示进入副本,2表示重新开始
    /// </summary>
    public static Action<int, int> doEnterDungenEvent;
    public static Action onRefuseEnterEvent;

    public Dictionary<int, bool> voiceDic = new Dictionary<int, bool>();

	public MessageST CurTeamMessage = null;
	public Action OnTeamMessageUpdateEvent;

    #region 构造
    /// <summary>
    /// 返回该管理类的唯一实例
    /// </summary>
    /// <returns></returns>
    public static TeamMng CreateNew()
    {
        if (GameCenter.teamMng == null)
        {
            TeamMng teamMng = new TeamMng();
            teamMng.Init();
            return teamMng;
        }
        else
        {
            GameCenter.teamMng.UnRegist();
            GameCenter.teamMng.Init();
            return GameCenter.teamMng;
        }
    }
    /// <summary>
    /// 注册
    /// </summary>
    void Init()
    {
        MsgHander.Regist(0xD022, S2C_TeamMemberResult);
        MsgHander.Regist(0xD023, S2C_TeamMemberUpdate);
        MsgHander.Regist(0xD024, S2C_AskForInvite);
        MsgHander.Regist(0xD026, S2C_AskForJoin);
        MsgHander.Regist(0xD028, S2C_TeamChangeLeader);
        MsgHander.Regist(0xD029, S2C_TeammateOut);
        MsgHander.Regist(0xD030, S2C_TeamDissolve);
        MsgHander.Regist(0xD031, S2C_CancelInvite);
        MsgHander.Regist(0xD032, S2C_CancelJoin);
        MsgHander.Regist(0xE110, S2C_GetNearTeam);
        //MsgHander.Regist(0x3615, S2C_TeamInjury);
    }
    /// <summary>
    /// 注销
    /// </summary>
    void UnRegist()
    {
        MsgHander.UnRegist(0xD022, S2C_TeamMemberResult);
        MsgHander.UnRegist(0xD023, S2C_TeamMemberUpdate);
        MsgHander.UnRegist(0xD024, S2C_AskForInvite);
        MsgHander.UnRegist(0xD026, S2C_AskForJoin);
        MsgHander.UnRegist(0xD028, S2C_TeamChangeLeader);
        MsgHander.UnRegist(0xD029, S2C_TeammateOut);
        MsgHander.UnRegist(0xD030, S2C_TeamDissolve);
        MsgHander.UnRegist(0xD031, S2C_CancelInvite);
        MsgHander.UnRegist(0xD032, S2C_CancelJoin);
        MsgHander.UnRegist(0xE110, S2C_GetNearTeam);
        //MsgHander.UnRegist(0x3615, S2C_TeamInjury);
        teammatesDic.Clear();
        LeaderId = 0;
        TeamId = 0;
        TeamTargetID = 0;
    }
    #endregion

    #region 通信C2S
    //2001,快速组队
    //3,退出队伍
    //2002,改变队伍目标 int 目标
    //1,邀请组队 int 目标id


    /// <summary>
    /// 邀请组队  1,邀请组队 int 目标id
    /// </summary>
    public void C2S_TeamInvite(int _pid)
    {
        Debug.Log("C2S_TeamInvite + " + _pid);
        if (!TeamIsFull)
        {
			pt_req_team_d427 msg = new pt_req_team_d427();
			msg.state = 1;
			msg.uid = _pid;
			NetMsgMng.SendMsg(msg);
			GameCenter.messageMng.AddClientMsg(97);
        }
        else
        {
            GameCenter.messageMng.AddClientMsg(84);
        }
    }
	/// <summary>
	/// 一键组队
	/// </summary>
	public void C2S_TeamInvite(List<int> _pids)
	{
		for (int i = 0,max=_pids.Count; i < max; i++) {
			pt_req_team_d427 msg = new pt_req_team_d427();
			msg.state = 1;
			msg.uid = _pids[i];
			NetMsgMng.SendMsg(msg);
		}
		GameCenter.messageMng.AddClientMsg(97);
	}
    /// <summary>
    /// 申请组队 2004,申请组队  int 目标id
    /// </summary>
    /// <param name="_pid"></param>
    public void C2S_TeamJoin(int _pid)
    {
    }
    /// <summary>
    /// 回应组队邀请
    /// </summary>
    public void C2S_ReplyInvite(int _pid, int _answer)
    {
        pt_team_ans_ask_d025 msg = new pt_team_ans_ask_d025();
        msg.ans_uid = (uint)_pid;
        msg.ans = (uint)_answer;
        NetMsgMng.SendMsg(msg);

    }
    /// <summary>
    /// 回答别人的申请
    /// </summary>
    public void C2S_ReplyJoin(int _pid, int _answer)
    {
        pt_team_ans_req_d027 msg = new pt_team_ans_req_d027();
        msg.ans_uid = (uint)_pid;
        msg.ans = (uint)_answer;
        NetMsgMng.SendMsg(msg);
    }

	public void C2S_CreateTeam()
	{
		Debug.Log("C2S_CreateTeam");
		pt_req_single_team_d758 msg = new pt_req_single_team_d758();
		NetMsgMng.SendMsg(msg);
	}

    /// <summary>
    /// 退出队伍 2006,退出队伍
    /// </summary>
    public void C2S_TeamOut()
    {
		Debug.Log("C2S_TeamOut");
		pt_req_team_d427 msg = new pt_req_team_d427();
		msg.state = 3;
        NetMsgMng.SendMsg(msg);
    }
	
	/// <summary>
	/// 解散队伍
	/// </summary>
	public void C2S_TeamDissolve()
	{
		pt_req_team_d427 msg = new pt_req_team_d427();
		msg.state = 6;
		msg.uid = GameCenter.curMainPlayer.id;
		NetMsgMng.SendMsg(msg);
	}
    /// <summary>
    /// 强制踢人
    /// </summary>
    public void C2S_TeamForceOut(int _pid)
    {
		Debug.Log("C2S_TeamForceOut _pid = " + _pid);
		pt_req_team_d427 msg = new pt_req_team_d427();
		msg.state = 5;
		msg.uid = _pid;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 转让队长
    /// </summary>
    public void C2S_TeamTransLeader(int _pid)
    {
		Debug.Log("C2S_TeamTransLeader:"+_pid);
		pt_req_team_d427 msg = new pt_req_team_d427();
		msg.state = 4;
		msg.uid = _pid;
        NetMsgMng.SendMsg(msg);
    }

    /// <summary>
    /// 获取周围的队伍
    /// </summary>
    public void C2S_NearTeam()
    {
        pt_action_d002 msg = new pt_action_d002();
        msg.action = 8004;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 保存设置
    /// </summary>
    public void C2S_PreserveSetting()
    {
        pt_mdf_team_info_e10e msg = new pt_mdf_team_info_e10e();
        msg.max_plys = (byte)limitNumb;
        msg.min_lev = (byte)limitLv;
        msg.need_verify = (byte)joinMode;
        NetMsgMng.SendMsg(msg);
    }
    #endregion

    #region 通信S2C
    /// <summary>
    /// 获取队伍成员
    /// </summary>
    protected void S2C_TeamMemberResult(Pt _info)
    {
        //Debug.Log("S2C_TeamMemberResult");
        pt_team_info_d022 msg = _info as pt_team_info_d022;
        if (msg != null)
        {
            if (!isInTeam && OpenTeamWndEvent != null)
                OpenTeamWndEvent();//不在队伍中,获得队伍信息 则为新建了队伍
            TeamId = (int)msg.team_id;
            LeaderId = (int)msg.leader_id;
            TeamTargetID = (int)msg.target;
            for (int i = 0; i < msg.team_member_list.Count; i++)
            {
                TeamMenberInfo info = new TeamMenberInfo(msg.team_member_list[i]);
//				if((int)info.baseInfo.uid == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
//					continue;//排除自己
                teammatesDic[(int)info.baseInfo.uid] = info;
                UpdateTeammateColor((int)info.baseInfo.uid, true);//修改颜色
            }
            if (onTeammateUpdateEvent != null)
            {
                onTeammateUpdateEvent();
            }
        }
    }
    /// <summary>
    /// 队伍成员信息改变
    /// </summary>
    protected void S2C_TeamMemberUpdate(Pt _info)
    {
        //Debug.Log("S2C_TeamMemberUpdate");
        pt_team_member_chg_d023 msg = _info as pt_team_member_chg_d023;
        if (msg != null)
        {
            for (int i = 0; i < msg.team_member_list.Count; i++)
            {
                int id = (int)msg.team_member_list[i].uid;
                if (teammatesDic.ContainsKey(id))
                {
                    TeamMenberInfo info = teammatesDic[id] as TeamMenberInfo;
                    if (info != null) info.SetBaseInfo(msg.team_member_list[i]);
                }
                else
                {
                    TeamMenberInfo info = new TeamMenberInfo(msg.team_member_list[i]);
//					if((int)info.baseInfo.uid == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
//						continue;//排除自己
                    teammatesDic[(int)info.baseInfo.uid] = info;
                    UpdateTeammateColor((int)info.baseInfo.uid, true);//修改颜色
                    if (onTeammateUpdateEvent != null)
                        onTeammateUpdateEvent();
                }
            }
        }
    }
    /// <summary>
    /// 队伍解散
    /// </summary>
    protected void S2C_TeamDissolve(Pt _info)
    {
        pt_team_destroy_d030 msg = _info as pt_team_destroy_d030;
        if (msg != null)
        {
			if(teammatesDic.Count <= 1)
				GameCenter.messageMng.AddClientMsg(78);//在队伍界面点击退出队伍,需要提示"解散队伍"
            TeamId = 0;
            LeaderId = 0;
            TeamTargetID = 0;
            UpdateTeammateColor(0, false);
            teammatesDic.Clear();
            //GameCenter.endLessTrialsMng.TeamDissolve();
            GameCenter.duplicateMng.TeamDissolve();
            if (onTeammateUpdateEvent != null)
                onTeammateUpdateEvent();
        }
    }
    /// <summary>
    /// 成员退出
    /// </summary>
    protected void S2C_TeammateOut(Pt _info)
    {
        pt_team_member_leave_d029 msg = _info as pt_team_member_leave_d029;
        if (msg != null)
        {
            int _teammateId = (int)msg.leave_uid;
            if (teammatesDic.ContainsKey(_teammateId))
            {
                string[] str = new string[1];
                TeamMenberInfo info = teammatesDic[_teammateId] as TeamMenberInfo;
                str[0] = info.baseInfo.name;
                GameCenter.messageMng.AddClientMsg(96, str);
                teammatesDic.Remove(_teammateId);
				//移除多人准备数据
				//GameCenter.endLessTrialsMng.TeammateOut(_teammateId);
                GameCenter.duplicateMng.TeammateOut(_teammateId);
            }

            if (onTeammateUpdateEvent != null)
                onTeammateUpdateEvent();
            UpdateTeammateColor(_teammateId, false);//修改颜色
        }
    }
	
    /// <summary>
    /// 转让队长
    /// </summary>
    protected void S2C_TeamChangeLeader(Pt _info)
    {
        pt_team_leader_chg_d028 msg = _info as pt_team_leader_chg_d028;
        if (msg != null)
        {
            LeaderId = (int)msg.new_leader_uid;
            if (onTeammateUpdateEvent != null)
                onTeammateUpdateEvent();
        }
    }
    /// <summary>
    /// 邀请组队
    /// </summary>
    protected void S2C_AskForInvite(Pt _info)
    {
		Debug.Log("S2C_AskForInvite");
        if (isInTeam) return;
        pt_team_ask_d024 msg = _info as pt_team_ask_d024;
        if (msg != null)
        {
            Debug.Log("S2C_AskForInvite " + msg.ask_uid);
            string[] str = new string[1];
            str[0] = msg.ask_usr_name;

			CurTeamMessage = new MessageST();
			CurTeamMessage.messID = 27;
			CurTeamMessage.words = str;
			CurTeamMessage.delYes = (x)=>
			{
				InviteResult((int)msg.ask_uid, true);
			};
			CurTeamMessage.delNo = (y)=>
			{
				InviteResult((int)msg.ask_uid, false);
			};
			if(OnTeamMessageUpdateEvent != null)
				OnTeamMessageUpdateEvent();
        }
    }

    protected MessageST.MessDel InviteResult(int _pid, bool _yes)
    {
        C2S_ReplyInvite(_pid, _yes ? 1 : 0);
		CurTeamMessage = null;
        return null;
    }

    /// <summary>
    /// 取消邀请
    /// </summary>
    protected void S2C_CancelInvite(Pt _info)
    {
        pt_team_ask_cancle_d031 msg = _info as pt_team_ask_cancle_d031;
        if (msg != null)
        {
			Debug.Log("S2C_CancelInvite暂时无用!");
        }
    }
    /// <summary>
    /// 申请组队
    /// </summary>
    protected void S2C_AskForJoin(Pt _info)
    {
        pt_team_req_d026 msg = _info as pt_team_req_d026;
        if (msg != null)
        {
            if (!TeamIsFull && msg.req_lev >= LimitLv)
            {
                if (joinMode == TeamState.ApplyJoin)
                {
                    string[] str = new string[1];
                    str[0] = msg.req_name;
					CurTeamMessage = new MessageST();
					CurTeamMessage.messID = 26;
					CurTeamMessage.words = str;
					CurTeamMessage.delYes = (x)=>
					{
						JoinResult((int)msg.req_uid, true);
					};
					CurTeamMessage.delNo = (y)=>
					{
						JoinResult((int)msg.req_uid, false);
					};
					if(OnTeamMessageUpdateEvent != null)
						OnTeamMessageUpdateEvent();
                }
                else
                {
                    C2S_ReplyJoin((int)msg.req_uid, 1);
                }
            }
            else
            {
                C2S_ReplyJoin((int)msg.req_uid, 0);
            }
        }
    }

    protected MessageST.MessDel JoinResult(int _pid, bool _yes)
    {
        C2S_ReplyJoin(_pid, _yes ? 1 : 0);
		CurTeamMessage = null;
        return null;
    }

    /// <summary>
    /// 取消申请
    /// </summary>
    protected void S2C_CancelJoin(Pt _info)
    {
        pt_team_req_cancle_d032 msg = _info as pt_team_req_cancle_d032;
        if (msg != null)
        {
			Debug.Log("S2C_CancelJoin暂时无用");
        }
    }

    /// <summary>
    /// 获取周围的队伍信息
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_GetNearTeam(Pt _info)
    {
        pt_ret_scene_teams_e110 msg = _info as pt_ret_scene_teams_e110;
        if (msg != null)
        {
            //Debug.logger.Log("msg.team_info.Count = " + msg.team_info.Count);
            nearTeam.Clear();
            if (msg.team_info.Count > 0)
            {
                for (int i = 0; i < msg.team_info.Count; i++)
                {
                    NearTeamInfo info = new NearTeamInfo(msg.team_info[i]);
                    nearTeam[info.teamID] = info;
                }
            }
            if (onUpdateNearTeam != null)
                onUpdateNearTeam();
        }
    }



    public System.Action TeamInjuryEvent;

    /// <summary>
    /// 组队伤害
    /// </summary>
    protected void S2C_TeamInjury(Pt _info)
    {
    }

    public void ClearTeamInjuryList()
    {
    }


    #endregion

    #region 队伍操作

    #endregion

    #region 辅助
	/// <summary>
	/// 移除队伍中的化身数据
	///招募化身逻辑：服务端只记录被招募的化身ID并在进入副本时模拟通知化身加入队伍，服务端化身没有真实的进入队伍。
	/// 故：刷新队伍，刷新化身显示，存储化身数据是客服端维护的。并在副本退出请求时清除队伍中的化身数据。
	/// </summary>
	public void InvitationTeammateOut(){
		List<int> invitations = new List<int>();
		foreach(TeamMenberInfo info in teammatesDic.Values){
			if(info.baseInfo.robot_state == 1)invitations.Add((int)info.baseInfo.uid);
		}
		foreach(int id in invitations){
			teammatesDic.Remove(id);
			if (onTeammateUpdateEvent != null)
				onTeammateUpdateEvent();
			UpdateTeammateColor(id, false);//修改颜色
		}
	}
	
    /// <summary>
    /// 更新队友名字颜色
    /// </summary>
    public void UpdateTeammateColor(int _pid, bool isTeammate)
    {
        if (_pid == 0)//解散队伍或者切换主城是变更颜色
        {
            if (onUpdateNameColorEvent != null)
            {
                foreach (int pid in teammatesDic.Keys)
                {
                    onUpdateNameColorEvent(pid, isTeammate);
                }
            }
        }
        else
        {
            if (onUpdateNameColorEvent != null)
                onUpdateNameColorEvent(_pid, isTeammate);
        }
    }

    /// <summary>
    /// 是否在队伍中
    /// </summary>
    public bool isInTeam
    {
        get
        {
            return (teammatesDic.Count > 0);//teammatesDic包含自己
        }
    }
	/// <summary>
	/// 队伍人数(不包含自己)
	/// </summary>
	/// <value>The teammate count.</value>
	public int TeammateCount
	{
		get
		{
			return teammatesDic.Count;
		}
	}
	/// <summary>
	/// 检查别人是否在队伍中  By邓成
	/// </summary>
	public bool CheckTeamMate(int pid)
	{
		if(isInTeam)
		{
			foreach(TeamMenberInfo mate in teammatesDic.Values)
			{
				if(mate.baseInfo.uid == pid)
					return true;
			}
		}
		return false;
	}

    /// <summary>
    /// 自己是否是队长
    /// </summary>
    public bool isLeader
    {
        get
        {
            return isInTeam && (GameCenter.curMainPlayer.id == LeaderId);// 先判断是否组队
        }
    }
    protected bool isRecruit = true;
    /// <summary>
    /// 是否在招募
    /// </summary>
    public bool IsRecruit
    {
        get { return isRecruit; }
        set
        {
            isRecruit = value;
        }
    }
    /// <summary>
    /// 显示添加好友按钮
    /// </summary>
    public void ShowAddFriendButton()
    {
        if (doAddFriendShowEvent != null)
            doAddFriendShowEvent(true);
    }
    /// <summary>
    /// 设置队友在线状态(断线or正常),_pid玩家ID,online是否正常在线
    /// </summary>
    public void SetOutLineState(int _pid, bool _outline)
    {
        Debug.Log("SetOutLineState ： 掉线角色name =   " + _pid + "  是否掉线 ： " + _outline);
        if (teammatesDic.ContainsKey(_pid))
        {

            if (onUpdateOnLineEvent != null)
                onUpdateOnLineEvent(_pid, _outline);
        }
    }
    #endregion


}
public class NearTeamInfo
{
    public int teamID;
    public int leaderID;
    public int maxPlayer;
    public int fight;
    public List<SingleMember> memberList;
    public NearTeamInfo(st.net.NetBase.team_info _info)
    {
        teamID = (int)_info.tid;
        leaderID = (int)_info.leader_id;
        maxPlayer = (int)_info.max_plys;
        fight = (int)_info.fighting;
        memberList = new List<SingleMember>();
        for (int i = 0; i < _info.members.Count; i++)
        {
            SingleMember singleinfo = new SingleMember(_info.members[i]);
            memberList.Add(singleinfo);
        }
    }
}
public class SingleMember
{
    public int ID;
    public int Level;
    public string Name;
    public SingleMember(st.net.NetBase.member_info _info)
    {
        ID = (int)_info.id;
        Level = (int)_info.lev;
        Name = _info.name;
    }
}