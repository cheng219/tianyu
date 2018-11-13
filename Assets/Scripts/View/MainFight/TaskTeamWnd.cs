//==============================================
//作者：邓成
//日期：2016/3/20
//用途：任务和组队窗口
//==============================================

using UnityEngine;
using AnimationOrTween;
using System.Collections;
using System.Collections.Generic;

public class TaskTeamWnd : GUIBase
{
    #region 队伍和附近的人
    /// 队伍父控件 by吴江
    /// </summary>
    public UIGrid teamParent;
    /// <summary>
    /// 附近的人父控件
    /// </summary>
    public UIGrid otherParent;
    /// <summary>
    /// Table
    /// </summary>
    public UITable teamTable;

    public GameObject othersParent;
    // 组队操作按钮
    public GameObject joinTeamBtn;
    public GameObject dissolveTeamBtn;
    public GameObject teamBtnParent;
    public GameObject fourceOutBtn;
    public GameObject giveLeaderBtn;
    public GameObject sendMessageBtn;
    public GameObject addFriendBtn;
    public GameObject addGuildBtn;
    public GameObject outTeamBtn;
    //附近的人操作按钮
    /// <summary>
    /// 附近的人按钮父控件
    /// </summary>
    public GameObject btnParent;
    /// <summary>
    /// 察看按钮
    /// </summary>
    public UIButton checkBtn;
    /// <summary>
    /// 私聊按钮
    /// </summary>
    public UIButton privetChatBtn;
    /// <summary>
    /// 组队按钮
    /// </summary>
    public UIButton makeTeamBtn;
    /// <summary>
    /// 加为好友按钮
    /// </summary>
    public UIButton addToFriendBtn;
    /// <summary>
    /// 交易按钮
    /// </summary>
    public UIButton tradeBtn;
    /// <summary>
    /// 邮件按钮 
    /// </summary>
    public UIButton mailBtn;
    #endregion


    #region UI控件对象
    /// <summary>
    /// 任务父控件 by吴江
    /// </summary>
    public UIGrid taskParent;
    

    /// <summary>
    /// 当前点击的附近的人
    /// </summary>
    protected OtherPlayerInfo curNearbyPlayerInfo = null;


	/// <summary>
    /// 场景BOSS列表
	/// </summary>
	public UIGrid boss1Parent;
    /// <summary>
    /// 里火云洞BOSS列表
    /// </summary>
    public UIGrid boss2Parent;
    public UITable tableBoss;
    public UIToggle toggle1Boss;
    public UIToggle toggle2Boss;
    public GameObject bossTipGo;
    public UILabel labBossTip;
    public UISprite bossIcon;

	/// <summary>
	/// 仙盟篝火
	/// </summary>
	public GuildFireCoppyWnd guildFireCoppyWnd;
	/// <summary>
	/// 仙盟攻城
	/// </summary>
	public GuildSiegeCoppyWnd guildSiegeCoppyWnd;
    /// <summary>
    /// 挂机副本
    /// </summary>
    public HangUpCoppyWnd hangUpCoppyWnd;
    /// <summary>
    /// 队伍按钮
    /// </summary>
    public GameObject teamBtn;
	/// <summary>
	/// Boss按钮
	/// </summary>
	public GameObject bossBtn;
    

    /// <summary>
    /// 任务按钮
    /// </summary>
    public GameObject taskBtn;

	//public GameObject othersBtn;

    /// <summary>
    /// 队员控件实例
    /// </summary>
    public TeamMemberListSingle teamMemberInstance;

    public TeamMemberListSingle[] teamMemberList = new TeamMemberListSingle[2];

    public TaskListSingle taskListSingleInstance;

    public List<TaskListSingle> taskList = new List<TaskListSingle>();

	public OtherPlayerListSingle otherListSingleInstance;

	public List<OtherPlayerListSingle> othersList = new List<OtherPlayerListSingle>();

	public BossListSingle bossListSingleInstance;

	public List<BossListSingle> bossList = new List<BossListSingle>();

    public UIToggle[] toggles;
    protected UIToggle curTogle;

    protected ItemUI itemUI;
	/// <summary>
	/// 神圣水晶表述
	/// </summary>
	public UILabel labHolyInfo;
	public UITimer holyTimer;

	public enum ToggleType
	{
		/// <summary>
		/// 任务(一张纸图标)
		/// </summary>
		TASK = 0,
		/// <summary>
		/// 仙盟篝火(两把剑图标)
		/// </summary>
		GUILDFIRE = 1,
		/// <summary>
		/// 队伍(两个人图标)
		/// </summary>
		TEAM = 2,
		/// <summary>
		/// 神圣水晶(旗帜)
		/// </summary>
		HOLYSTONE = 3,
		/// <summary>
		/// BOSS列表(一个皇冠)
		/// </summary>
		BOSS = 4,
		/// <summary>
		/// 攻城战
		/// </summary>
		GUILDSIEGE = 5,
        /// <summary>
        /// 挂机副本
        /// </summary>
        HANGUPCOPPY = 6,
	}
    #endregion

    void Awake()
    {
        if (dissolveTeamBtn != null) UIEventListener.Get(dissolveTeamBtn).onClick = OnClickDissolveTeamBtn;
        GameObject obj = exResources.GetResource(ResourceType.GUI, "mainUI/TeamInstance") as GameObject;
		if(obj != null)teamMemberInstance = obj.GetComponent<TeamMemberListSingle>();
        obj = null;


        obj = exResources.GetResource(ResourceType.GUI, "mainUI/TaskInstance") as GameObject;
		if(obj != null)taskListSingleInstance = obj.GetComponent<TaskListSingle>();
        obj = null;

		obj = exResources.GetResource(ResourceType.GUI,"") as GameObject;
		if(obj != null)otherListSingleInstance = obj.GetComponent<OtherPlayerListSingle>();
        obj = null;

        if (toggle1Boss != null) EventDelegate.Add(toggle1Boss.onChange,OnChangeBoss);
        if (toggle2Boss != null) EventDelegate.Add(toggle2Boss.onChange, OnChangeBoss);
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        RefreshTask();
		RefreshHolyInfo();
        SetToggle();
       
    }

    void SetToggle() 
    {
		ToggleType select = GameCenter.taskMng.CurSelectToggle;
		SceneUiType uiType = GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType;
        bool showTask = (select != ToggleType.HOLYSTONE && select != ToggleType.GUILDSIEGE && select != ToggleType.GUILDFIRE && select != ToggleType.HANGUPCOPPY);//任务在神圣水晶活动隐藏
		for (int i = 0; i < toggles.Length; i++)
		{
			ToggleType type = (ToggleType)i;
			if (toggles[i] != null)
			{
				if(type == select)
				{
					toggles[i].value = true;
					toggles[i].gameObject.SetActive(true);
				}else
				{
					toggles[i].value = false;
					toggles[i].gameObject.SetActive(false);
				}
                if (type == ToggleType.TASK) toggles[i].gameObject.SetActive(showTask);
				if(type == ToggleType.TEAM)toggles[i].gameObject.SetActive(true);//组队只根据场景隐藏
				if(type == ToggleType.BOSS)toggles[i].gameObject.SetActive(true);//BOSS列表显示
				if(guildFireCoppyWnd != null)
				{
                    if (select == ToggleType.GUILDFIRE)
                    {
                        guildFireCoppyWnd.CloseUI();
                        guildFireCoppyWnd.OpenUI();
                    }
                    else
                        guildFireCoppyWnd.CloseUI();
				}
				if(guildSiegeCoppyWnd != null)
				{
                    if (select == ToggleType.GUILDSIEGE)
                    {
                        guildSiegeCoppyWnd.CloseUI();
                        guildSiegeCoppyWnd.OpenUI();
                    }
                    else
                    {
                        guildSiegeCoppyWnd.CloseUI();
                    }
				}
                if (hangUpCoppyWnd != null)
                {
                    if (select == ToggleType.HANGUPCOPPY)
                    {
                        hangUpCoppyWnd.CloseUI();
                        hangUpCoppyWnd.OpenUI();
                    }
                    else
                    {
                        hangUpCoppyWnd.CloseUI();
                    }
                }
			}
		}
    }

    public void SetCurToggle(UIToggle _toggle) 
    {
        if (_toggle.value) 
        {
            for (int i = 0; i < toggles.Length; i++)
            {
                if (toggles[i].value)
                {
					GameCenter.taskMng.SetCurSelectToggle((ToggleType)i);
                    break;
                }
            }
        }

    }

    void ShowTeamWnd()
    {
        int teamIndex = (int)ToggleType.TEAM;
        if (toggles != null && toggles.Length > teamIndex)
        {
            if (toggles[teamIndex] != null && toggles[teamIndex].isActiveAndEnabled)
            {
                toggles[teamIndex].value = true;
            }
        }
    }

    protected override void OnClose()
    {
        base.OnClose();
    }


    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            if (checkBtn != null) UIEventListener.Get(checkBtn.gameObject).onClick += OnClickCheckBtn;
            if (privetChatBtn != null) UIEventListener.Get(privetChatBtn.gameObject).onClick += OnClickPrivetChatBtn;
            if (makeTeamBtn != null) UIEventListener.Get(makeTeamBtn.gameObject).onClick += OnClickMakeTeamBtn;
            if (addToFriendBtn != null) UIEventListener.Get(addToFriendBtn.gameObject).onClick += OnClickAddToFriendBtn;
            if (tradeBtn != null) UIEventListener.Get(tradeBtn.gameObject).onClick += OnClickTradeBtn;
            if (mailBtn != null) UIEventListener.Get(mailBtn.gameObject).onClick += OnClickMailBtn;  
            //if (othersBtn != null) UIEventListener.Get(othersBtn.gameObject).onClick += OnClickOtherPlayerOpenBtn;
            if (bossBtn != null) UIEventListener.Get(bossBtn.gameObject).onClick += OnClickBossOpenBtn;
            if (teamBtn != null) UIEventListener.Get(teamBtn.gameObject).onClick += OnClickTeamOpenBtn; 
            if (fourceOutBtn != null) UIEventListener.Get(fourceOutBtn).onClick += OnClickFourceOutBtn;
            if (giveLeaderBtn != null) UIEventListener.Get(giveLeaderBtn).onClick += OnClickGiveLeaderBtn;
            if (sendMessageBtn != null) UIEventListener.Get(sendMessageBtn).onClick += OnClickChatBtn;
            if (addFriendBtn != null) UIEventListener.Get(addFriendBtn).onClick += OnClickAddFriendBtn;
            if (addGuildBtn != null) UIEventListener.Get(addGuildBtn.gameObject).onClick = OnClickReqGuild;
            if (outTeamBtn != null) UIEventListener.Get(outTeamBtn).onClick += OnClickOutTeamBtn;
            if (taskBtn != null) UIEventListener.Get(taskBtn.gameObject).onClick += OnClickTaskOpenBtn;
            if (joinTeamBtn != null) UIEventListener.Get(joinTeamBtn.gameObject).onClick += OnClickQuickTeamBtn;
            GameCenter.teamMng.onTeammateUpdateEvent += OnTeammateUpdateEvent;
            GameCenter.taskMng.OnTaskListUpdate += RefreshTask;
			GameCenter.taskMng.OnTaskGroupUpdate += RefreshTask;
			GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += RefreshTask;
			GameCenter.activityMng.OnGotHolyInfoEvent += RefreshHolyInfo;
			GameCenter.bossChallengeMng.OnGotChallengListEvent += RefreshBoss;
			GameCenter.activityMng.OnActivityDataInfo += CloseHolyWndByActivityEnd;
			SceneMng.OnOPCInfoListUpdate += RefreshOthers;
			SceneMng.OnDelInterObj += RefreshOthersByOutline;
			SceneMng.OnSceneItemInfoListUpdate += RefreshHolyFirstLogin;
			if(GameCenter.curMainPlayer != null)GameCenter.curMainPlayer.onSectorChanged += RefreshHolyInfo;
			GameCenter.taskMng.OnTaskProgressUpdateEvent += OnTaskProgressUpdate;
			GameCenter.noviceGuideMng.UpdateGuideData += CloseTaskGuideByFunction;
            GameCenter.noviceGuideMng.UpdateFunctionData += CloseTaskGuideByFunction;
            GameCenter.teamMng.OpenTeamWndEvent += ShowTeamWnd;
            GameCenter.bossChallengeMng.OnBossReliveEvent += ShowBossTip;
        }
        else
        {
            if (checkBtn != null) UIEventListener.Get(checkBtn.gameObject).onClick -= OnClickCheckBtn;
            if (privetChatBtn != null) UIEventListener.Get(privetChatBtn.gameObject).onClick -= OnClickPrivetChatBtn;
            if (makeTeamBtn != null) UIEventListener.Get(makeTeamBtn.gameObject).onClick -= OnClickMakeTeamBtn;
            if (addToFriendBtn != null) UIEventListener.Get(addToFriendBtn.gameObject).onClick -= OnClickAddToFriendBtn;
            if (tradeBtn != null) UIEventListener.Get(tradeBtn.gameObject).onClick -= OnClickTradeBtn;
            if (mailBtn != null) UIEventListener.Get(mailBtn.gameObject).onClick -= OnClickMailBtn; 
            //if (othersBtn != null) UIEventListener.Get(othersBtn.gameObject).onClick -= OnClickOtherPlayerOpenBtn;
            if (bossBtn != null) UIEventListener.Get(bossBtn.gameObject).onClick -= OnClickBossOpenBtn;
            if (teamBtn != null) UIEventListener.Get(teamBtn.gameObject).onClick -= OnClickTeamOpenBtn; 
            if (fourceOutBtn != null) UIEventListener.Get(fourceOutBtn).onClick -= OnClickFourceOutBtn;
            if (giveLeaderBtn != null) UIEventListener.Get(giveLeaderBtn).onClick -= OnClickGiveLeaderBtn;
            if (sendMessageBtn != null) UIEventListener.Get(sendMessageBtn).onClick -= OnClickChatBtn;
            if (addFriendBtn != null) UIEventListener.Get(addFriendBtn).onClick -= OnClickAddFriendBtn;
            if (outTeamBtn != null) UIEventListener.Get(outTeamBtn).onClick -= OnClickOutTeamBtn;
            if (taskBtn != null) UIEventListener.Get(taskBtn.gameObject).onClick -= OnClickTaskOpenBtn;
            if (joinTeamBtn != null) UIEventListener.Get(joinTeamBtn.gameObject).onClick -= OnClickQuickTeamBtn;
            GameCenter.teamMng.onTeammateUpdateEvent -= OnTeammateUpdateEvent;
            GameCenter.taskMng.OnTaskListUpdate -= RefreshTask;
			GameCenter.taskMng.OnTaskGroupUpdate -= RefreshTask;
			GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= RefreshTask;
			GameCenter.activityMng.OnGotHolyInfoEvent -= RefreshHolyInfo;
			GameCenter.bossChallengeMng.OnGotChallengListEvent -= RefreshBoss;
			GameCenter.activityMng.OnActivityDataInfo -= CloseHolyWndByActivityEnd;
			SceneMng.OnOPCInfoListUpdate -= RefreshOthers;
			SceneMng.OnDelInterObj -= RefreshOthersByOutline;
			SceneMng.OnSceneItemInfoListUpdate -= RefreshHolyFirstLogin;
			if(GameCenter.curMainPlayer != null)GameCenter.curMainPlayer.onSectorChanged -= RefreshHolyInfo;
			GameCenter.taskMng.OnTaskProgressUpdateEvent -= OnTaskProgressUpdate;
			GameCenter.noviceGuideMng.UpdateGuideData -= CloseTaskGuideByFunction;
            GameCenter.noviceGuideMng.UpdateFunctionData -= CloseTaskGuideByFunction;
            GameCenter.teamMng.OpenTeamWndEvent -= ShowTeamWnd;
            GameCenter.bossChallengeMng.OnBossReliveEvent -= ShowBossTip;
        }
    }

	protected List<string> taskProgressTip = new List<string>();
	protected void OnTaskProgressUpdate(string _progressTip)
	{
		taskProgressTip.Add(_progressTip);
		Invoke("InvokeTaskProgress",1.0f);
	}
	protected void InvokeTaskProgress()
	{
		if(taskProgressTip.Count > 0)
		{
			GameCenter.messageMng.AddClientMsg(35,new string[]{taskProgressTip[0]});
			taskProgressTip.RemoveAt(0);
		}
	}

    void OnDisable()
    {
        GameCenter.taskMng.OnTaskListUpdate -= RefreshTask;

    }

    #region 控件事件

    protected void OnClickTaskOpenBtn(GameObject _obj)
    {
        joinTeamBtn.SetActive(false);
        RefreshTask();
    }

    protected void OnClickTeamOpenBtn(GameObject _obj)
    {
        RefreshTeam();
    }

	protected void OnClickOtherPlayerOpenBtn(GameObject _obj)
	{
	//	RefreshOthers();
	}
	void RefreshOthersByOutline(ObjectType objType,int id)
	{
		if(objType == ObjectType.Player)
			RefreshOthers();
	}

	protected void OnClickBossOpenBtn(GameObject _obj)
	{
        GameCenter.bossChallengeMng.C2S_ReqChallengeBossList();
		RefreshBoss();
	}
	#region 队伍操作
	/// <summary>
	/// 快捷组队
	/// </summary>
	/// <param name="_obj"></param>
	protected void OnClickQuickTeamBtn(GameObject _obj)
	{
		GameCenter.teamMng.C2S_CreateTeam();
	}
	protected void OnClickDissolveTeamBtn(GameObject go)
	{
		if(GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneType != SceneType.SCUFFLEFIELD)
		{
			GameCenter.messageMng.AddClientMsg(119);
			return;
		}
		GameCenter.teamMng.C2S_TeamOut();
	}

	protected TeamMenberInfo CurTeamMenberInfo;
    protected void OnClickTeamMemBtn(GameObject _obj)
    {
		if(teamBtnParent != null)teamBtnParent.SetActive(true);
		CurTeamMenberInfo = UIEventListener.Get(_obj).parameter as TeamMenberInfo;
    }
	protected void OnClickFourceOutBtn(GameObject _obj)
	{
		if(GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneType != SceneType.SCUFFLEFIELD)
		{
			GameCenter.messageMng.AddClientMsg(119);
			return;
		}
		if(CurTeamMenberInfo != null)
		{
			GameCenter.teamMng.C2S_TeamForceOut((int)CurTeamMenberInfo.baseInfo.uid);
		}
		if(teamBtnParent != null)teamBtnParent.SetActive(false);
	}
	protected void OnClickGiveLeaderBtn(GameObject _obj)
	{
		if(GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneType != SceneType.SCUFFLEFIELD)
		{
			GameCenter.messageMng.AddClientMsg(119);
			return;
		}
		if(CurTeamMenberInfo != null)
		{
			GameCenter.teamMng.C2S_TeamTransLeader((int)CurTeamMenberInfo.baseInfo.uid);
		}
		if(teamBtnParent != null)teamBtnParent.SetActive(false);
	}
	protected void OnClickChatBtn(GameObject _obj)
	{
		if(CurTeamMenberInfo != null)
		{
			GameCenter.chatMng.OpenPrivateWnd(CurTeamMenberInfo.baseInfo.name);
		}
		if(teamBtnParent != null)teamBtnParent.SetActive(false);
	}
	protected void OnClickAddFriendBtn(GameObject _obj)
	{
		if(CurTeamMenberInfo != null)
		{
			GameCenter.friendsMng.C2S_AddFriend((int)CurTeamMenberInfo.baseInfo.uid);
		}
		if(teamBtnParent != null)teamBtnParent.SetActive(false);
	}

    protected void OnClickReqGuild(GameObject go)
    {
        GameCenter.guildMng.C2S_ReqJoinGuild(GameCenter.curMainPlayer.CurTarget.id);
    }

	protected void OnClickOutTeamBtn(GameObject _obj)
	{
		if(GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneType != SceneType.SCUFFLEFIELD)
		{
			GameCenter.messageMng.AddClientMsg(119);
			return;
		}
		if(CurTeamMenberInfo != null)
		{
			MessageST mst = new MessageST();
			mst.messID = 77;
			mst.delYes = (x)=>
			{
				GameCenter.teamMng.C2S_TeamOut();
			};
			GameCenter.messageMng.AddClientMsg(mst);
		}
		if(teamBtnParent != null)teamBtnParent.SetActive(false);
	}
	private void OnTeammateUpdateEvent()
	{
		OnClickTeamOpenBtn(null);
	}

	protected void RefreshTeam()
	{
		for (int i = 0; i < teamMemberList.Length; i++)
		{
            if (teamMemberList[i] != null) teamMemberList[i].gameObject.SetActive(false);
		}
        for (int i = 0; i < othersList.Count; i++)
        {
            othersList[i].gameObject.SetActive(false);
        }
        if (othersParent != null)othersParent.SetActive(false);
        if (teamParent != null)teamParent.gameObject.SetActive(false);
		if (GameCenter.teamMng.isInTeam)
		{
			joinTeamBtn.SetActive(false);
			FDictionary memberDic = GameCenter.teamMng.TeammatesDic;
			if(memberDic.Count == 1)//队伍中只有自己
			{
				if(dissolveTeamBtn != null)dissolveTeamBtn.SetActive(true);
			}else
			{
                if (teamParent != null) teamParent.gameObject.SetActive(true);
				if(dissolveTeamBtn != null)dissolveTeamBtn.SetActive(false);
				int index = 0;
				foreach (TeamMenberInfo item in memberDic.Values)
				{
					if(item.baseInfo.uid == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
						continue;
                    if (teamMemberList.Length > index && teamMemberList[index] == null)
					{
                        teamMemberList[index] = (teamMemberInstance.CreateNew(teamParent.transform, index));
					}
                    UIEventListener.Get(teamMemberList[index].gameObject).onClick -= OnClickTeamMemBtn;
                    UIEventListener.Get(teamMemberList[index].gameObject).onClick += OnClickTeamMemBtn;
					teamMemberList[index].gameObject.SetActive(true);
					teamMemberList[index].SetInfo(item);
					UIEventListener.Get(teamMemberList[index].gameObject).parameter = item;
					index++;
				}
			}
		}
		else
		{
			joinTeamBtn.SetActive(true);
			if(dissolveTeamBtn != null)dissolveTeamBtn.SetActive(false);
		}
		teamParent.repositionNow = true;
        if (teamTable != null) teamTable.repositionNow = true;
        if (othersParent != null)
        {
            othersParent.SetActive(true);
        }
        RefreshOthers();
	}

    protected void RefreshOthers()
    {
        for (int i = 0; i < othersList.Count; i++)
        {
            othersList[i].gameObject.SetActive(false);
        }
        FDictionary otherList = GameCenter.sceneMng.OPCInfoDictionary;
        int index = 0;
        foreach (OtherPlayerInfo item in otherList.Values)
        {
            if (othersList.Count < index + 1)
            {
                othersList.Add(otherListSingleInstance.CreateNew(otherParent.transform, index));
                UIEventListener.Get(othersList[index].gameObject).onClick -= OnClickNearbyPlayerBtn;
                UIEventListener.Get(othersList[index].gameObject).onClick += OnClickNearbyPlayerBtn;
            }
            UIEventListener.Get(othersList[index].gameObject).parameter = item;
            othersList[index].gameObject.SetActive(true);
            othersList[index].SetInfo(item);
            index++;
        }
        otherParent.repositionNow = true;
        if (teamTable != null) teamTable.repositionNow = true;
    }
	#endregion

    protected void OnClickNearbyPlayerBtn(GameObject go)
	{
        curNearbyPlayerInfo = UIEventListener.Get(go).parameter as OtherPlayerInfo;
        if (btnParent != null) btnParent.gameObject.SetActive(true);
        if (curNearbyPlayerInfo != null)
        {
            OtherPlayer other = GameCenter.curGameStage.GetOtherPlayer(curNearbyPlayerInfo.ServerInstanceID);
            if (GameCenter.curMainPlayer != null && other != null)
                GameCenter.curMainPlayer.CurTarget = other;
        }
	}

	protected void OnClickMoveToMobBtn(GameObject go)
	{
		GameCenter.curMainPlayer.commandMng.CancelCommands();
		BossChallengeData info = UIEventListener.Get(go).parameter as BossChallengeData;
		if(info != null && info.CurBossRef != null)
		{
            if (info.CurBossRef.type != (int)BossChallengeWnd.ToggleType.LiRongEBoss || (info.CurBossRef.type == (int)BossChallengeWnd.ToggleType.LiRongEBoss && GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType == SceneUiType.LIRONGELAND))
            {
                GameCenter.curMainPlayer.GoTraceTarget(info.CurBossRef.sceneID, info.CurBossRef.sceneX, info.CurBossRef.sceneY);
            }
            else
            {
                if (GameCenter.vipMng.VipData != null)
                {
                    if (GameCenter.vipMng.VipData.vLev < 2)
                    {
                        MessageST mst = new MessageST();
                        mst.messID = 496;
                        mst.delYes = (x) =>
                        {
                            GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
                        };
                        GameCenter.messageMng.AddClientMsg(mst);
                    }
                    else
                    {
                        if (!GameCenter.bossChallengeMng.FightLiRongEBossNoTip)
                        {

                            MessageST mst = new MessageST();
                            mst.messID = 497;
                            object[] pa = { 1 };
                            mst.pars = pa;
                            mst.delPars = delegate(object[] ob)
                            {
                                if (ob.Length > 0)
                                {
                                    bool b = (bool)ob[0];
                                    if (b)
                                        GameCenter.bossChallengeMng.FightLiRongEBossNoTip = true;
                                }
                            };
                            mst.delYes = (x) =>
                            {
                                GameCenter.bossChallengeMng.C2S_ChallengeBoss(info.bossID, info.CurBossRef.type);
                            };
                            GameCenter.messageMng.AddClientMsg(mst);
                        }
                        else
                        {
                            GameCenter.bossChallengeMng.C2S_ChallengeBoss(info.bossID, info.CurBossRef.type);
                        }
                    }
                }
            }
		}
	}



    protected void OnClickTaskBtn(GameObject _obj)
    {

        TaskListSingle taskUI = _obj.GetComponent<TaskListSingle>();
        if (taskUI == null) return;
        GameCenter.curMainPlayer.GoNormal();
        TaskInfo info = taskUI.MyTaskInfo;
        if (info == null) return;
        if (info.TaskType == TaskType.Ring)//打开环任务
        {
            GameCenter.taskMng.curRingTaskType = info.StarLevel;
            if (GameCenter.taskMng.GetRingAutoFinish(GameCenter.taskMng.curRingTaskType))
            {
                GameCenter.curMainPlayer.StopForNextMove();
                GameCenter.taskMng.SetRingAutoFinishType(GameCenter.taskMng.curRingTaskType, false);
            } 
            GameCenter.uIMng.SwitchToUI(GUIType.RINGTASK);
            return;
        }
        GameCenter.taskMng.CurfocusTask = info;

        GameCenter.taskMng.TraceToAction(info);
    }

    #region 点击附近的人按钮事件
    /// <summary>
    /// 查看信息
    /// </summary> 
    protected void OnClickCheckBtn(GameObject go)
    {
        if (curNearbyPlayerInfo != null)
        {
            GameCenter.previewManager.C2S_AskOPCPreview(curNearbyPlayerInfo.ServerInstanceID);
        }
    }
    /// <summary>
    /// 私聊
    /// </summary> 
    protected void OnClickPrivetChatBtn(GameObject go)
    {
        if (curNearbyPlayerInfo != null && curNearbyPlayerInfo.Name != string.Empty)
        {
            GameCenter.chatMng.OpenPrivateWnd(curNearbyPlayerInfo.Name);
        }
        else
        {
            Debug.LogError("私聊对象名字为空");
        }
    }
    /// <summary>
    /// 组队
    /// </summary> 
    protected void OnClickMakeTeamBtn(GameObject go)
    {
        if (curNearbyPlayerInfo != null)
        {
            GameCenter.teamMng.C2S_TeamInvite(curNearbyPlayerInfo.ServerInstanceID);
        }
    }
    /// <summary>
    /// 加为好友
    /// </summary> 
    protected void OnClickAddToFriendBtn(GameObject go)
    {
        if (curNearbyPlayerInfo != null)
        {
            GameCenter.friendsMng.C2S_AddFriend(curNearbyPlayerInfo.ServerInstanceID);
        }
    }
    /// <summary>
    /// 交易
    /// </summary> 
    protected void OnClickTradeBtn(GameObject go)
    {
        if (curNearbyPlayerInfo != null)
        {
            GameCenter.tradeMng.C2S_AskTrade(curNearbyPlayerInfo.ServerInstanceID);
        }
    }
    /// <summary>
    /// 邮件
    /// </summary> 
    protected void OnClickMailBtn(GameObject go)
    {
        if (curNearbyPlayerInfo != null)
        {
            GameCenter.mailBoxMng.mailWriteData = new MailWriteData(curNearbyPlayerInfo.Name);
            GameCenter.uIMng.SwitchToSubUI(SubGUIType.BMail);
        }
    }

    #endregion
    #endregion


    #region 辅助逻辑

    protected void ShowBossTip(BossChallengeData _boss)
    {
        if (bossTipGo != null && labBossTip != null && bossIcon != null && _boss != null)
        {
            bossTipGo.SetActive(true);
            if(_boss.CurBossRef != null)
            {
                labBossTip.text = _boss.CurBossRef.bossTip;
                bossIcon.spriteName = _boss.bossIcon;
            }
            CancelInvoke("HideBossTip");
            Invoke("HideBossTip",30f);
        }
    }
    void HideBossTip()
    {
        if (bossTipGo != null) bossTipGo.SetActive(false);
    }

	protected void RefreshBoss()
	{
		for (int i = 0; i < bossList.Count; i++)
		{
			bossList[i].gameObject.SetActive(false);
		}
        List<BossChallengeData> boss1DataList = GameCenter.bossChallengeMng.SceneBossList;
        List<BossChallengeData> boss2DataList = GameCenter.bossChallengeMng.LiRongEBossList;
		int index = 0;
        for (int i = 0, max = boss1DataList.Count; i < max; i++)
        {
            BossChallengeData item = boss1DataList[i];
			if (bossList.Count < index + 1)
			{
                bossList.Add(bossListSingleInstance.CreateNew(boss1Parent.transform, index)); 
			}
			bossList[index].gameObject.SetActive(true);
			bossList[index].SetInfo(item);
			UIEventListener.Get(bossList[index].gameObject).onClick -= OnClickMoveToMobBtn;
			UIEventListener.Get(bossList[index].gameObject).onClick += OnClickMoveToMobBtn;
			UIEventListener.Get(bossList[index].gameObject).parameter = item;
			index++;
		}
        if (boss1Parent != null) boss1Parent.repositionNow = true;
        int indexL = index;
        for (int i = 0, max = boss2DataList.Count; i < max; i++)
        {
            BossChallengeData item = boss2DataList[i];
            if (bossList.Count < indexL + 1)
            {
                bossList.Add(bossListSingleInstance.CreateNew(boss2Parent.transform, indexL));
            }
            bossList[indexL].gameObject.SetActive(true);
            bossList[indexL].SetInfo(item);
            UIEventListener.Get(bossList[indexL].gameObject).onClick -= OnClickMoveToMobBtn;
            UIEventListener.Get(bossList[indexL].gameObject).onClick += OnClickMoveToMobBtn;
            UIEventListener.Get(bossList[indexL].gameObject).parameter = item;
            indexL++;
        }
        if (boss2Parent != null) boss2Parent.repositionNow = true;
        if (tableBoss != null) tableBoss.repositionNow = true;
        SceneUiType uiType = GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType;
        switch (uiType)
        { 
            case SceneUiType.LIRONGELAND:
                if (toggle2Boss != null) toggle2Boss.value = true;
                break;
        }
	}

    void OnChangeBoss()
    {
        if (toggle1Boss != null && toggle2Boss != null)
        {
            if (boss1Parent != null) boss1Parent.transform.localScale = toggle1Boss.value ? Vector3.one : Vector3.zero;//boss1Parent不能隐藏,要做倒计时
            if (boss2Parent != null) boss2Parent.transform.localScale = toggle2Boss.value ? Vector3.one : Vector3.zero;
        }
        if (tableBoss != null) tableBoss.repositionNow = true;
    }

    protected void RefreshTask(TaskDataType dataType,TaskType type)
    {
        RefreshTask();
    }
	protected void RefreshTask(TaskType type)
	{
		RefreshTask();
	}
	protected void RefreshTask(ActorBaseTag _tag,ulong val,bool state)
	{
		if(_tag == ActorBaseTag.Level)
		{
			RefreshTask();
		}
	}

    protected void RefreshTask()
    { 
        for (int i = 0; i < taskList.Count; i++)
        {
            if (taskList[i] != null)
            {
                taskList[i].gameObject.SetActive(false);
            }
        }
        int index = 0;
        index = RefreshTask(TaskType.Main,  index);
		index = RefreshTask(TaskType.Ring,  index);
		index =  RefreshTask(TaskType.Trial,  index);
//        index = RefreshTask(TaskType.Guild,  index);
//        index = RefreshTask(TaskType.Daily,  index);
//        index =RefreshTask(TaskType.Offer, index);
		index =  RefreshTask(TaskType.Special,  index);
        taskParent.repositionNow = true;

    }

    protected int RefreshTask(TaskType _type, int _startIndex)
    {
		TaskMng taskMng = GameCenter.taskMng;
		Dictionary<int, TaskInfo> taskDic = taskMng.GetTaskDic(_type);
        int index = _startIndex; 
        foreach (TaskInfo item in taskDic.Values)
        {
            if (_type == TaskType.Trial && (item.TaskState == TaskStateType.UnTake || item.TaskState == TaskStateType.ENDED)) continue;//未接取的试炼任务不显示(显示特殊任务)
            if (_type == TaskType.Ring && (taskMng.IsShowSpecislRingTask() || taskMng.FinishAllRingTask)) continue;//未接取的环任务不显示(显示特殊任务)
            if (_type == TaskType.Ring && !taskMng.IsShowThisRingTask(item.StarLevel)) continue;
            if(_type == TaskType.Special && !item.SpecialTaskCanShow)continue;//特殊任务显示有等级要求
            if (_type == TaskType.Special && item.ID == 10001 && (!taskMng.IsShowSpecislRingTask() || taskMng.FinishAllRingTask)) continue;//身上有环任务or所有环任务已完成,不显示特殊(环)任务
			if(_type == TaskType.Special && item.ID == 10002 && (taskMng.HaveTrialTask() || (taskMng.FinishAllTrialTask() && GameCenter.taskMng.TrialTaskRestRewardTimes <= 0)))continue;//身上有试炼任务,不显示特殊(试炼)任务
            if (taskList.Count < index + 1)
            {
                TaskListSingle single = taskListSingleInstance.CreateNew(taskParent.transform, index);
                taskList.Add(single);
                taskParent.AddChild(single.transform);
                UIEventListener.Get(taskList[index].gameObject).onClick -= OnClickTaskBtn;
                UIEventListener.Get(taskList[index].gameObject).onClick += OnClickTaskBtn;
            }
  
            if (taskList[index] != null)
            {
                taskList[index].gameObject.SetActive(true);
                taskList[index].MyTaskInfo = item;
                
            }
            index++;
            
        }
        return index;
    }
	/// <summary>
	/// 指引开启关闭任务引导特效
	/// </summary>
	protected void CloseTaskGuideByFunction(OpenNewFunctionGuideRef _data)
	{
        //Debug.Log("指引开启关闭任务引导特效");
		for (int i = 0,max=taskList.Count; i < max; i++) 
		{
			if(taskList[i] != null)taskList[i].ShowFx(false);
		}
	}
    /// <summary>
    /// 功能开启关闭任务引导特效
    /// </summary>
    protected void CloseTaskGuideByFunction(FunctionDataInfo _data)
    {
        //Debug.Log("功能开启关闭任务引导特效");
        for (int i = 0, max = taskList.Count; i < max; i++)
        {
            if (taskList[i] != null) taskList[i].ShowFx(false);
        }
    }
	#region 仙盟篝火活动
    protected void RefreshGuildFire()
    {
		
    }
	#endregion

	#region 神圣水晶活动
	/// <summary>
	/// 活动时间到了,关闭显示界面(目前任一活动变化都会进这里,待修改) by邓成
 	/// </summary>
	void CloseHolyWndByActivityEnd()
	{
		if(GameCenter.activityMng.GetActivityState(ActivityType.HOLYSPAR) == ActivityState.HASENDED)
		{
			CloseHolyWnd();  
		}
	}
	void CloseHolyWnd()
	{
		if(GameCenter.taskMng.CurSelectToggle == ToggleType.HOLYSTONE)
		{
			GameCenter.uIMng.CloseGUI(GUIType.TASK);
			GameCenter.taskMng.SetCurSelectToggle(ToggleType.TASK);
			GameCenter.uIMng.GenGUI(GUIType.TASK,true);
		}
	}
	/// <summary>
	/// 根据距离变化，检测是否到达神圣晶石活动区域
	/// </summary>
	void RefreshHolyInfo(GameStage.Sector sector1,GameStage.Sector sector2)
	{
		if(GameCenter.activityMng.GetActivityState(ActivityType.HOLYSPAR) == ActivityState.ONGOING)
		{
			List<SceneItem> itemList = GameCenter.curGameStage.GetSceneItems();
			SceneItem holyStone = null;
			for (int i = 0,max=itemList.Count; i < max; i++) 
			{
				if(itemList[i].MySceneFunctionType == SceneFunctionType.HOLYSTONE)	
				{
					holyStone = itemList[i];
					break;
				}
			}
			if(holyStone == null)
			{
				CloseHolyWnd();//活动结束
				return;
			}else
			{
				int distance = (int)((GameCenter.curMainPlayer.transform.position - holyStone.transform.position).sqrMagnitude);
				if(distance < 144)//要客户端判断真是醉了(隐藏bug：在0~144范围内选择队伍toggle也会切到神圣晶石显示界面)
				{
					if(GameCenter.taskMng.CurSelectToggle != ToggleType.HOLYSTONE)
					{
						GameCenter.activityMng.C2S_ReqHolyInfo();
						GameCenter.uIMng.CloseGUI(GUIType.TASK);
						GameCenter.taskMng.SetCurSelectToggle(ToggleType.HOLYSTONE);
						GameCenter.uIMng.GenGUI(GUIType.TASK,true);
					}
				}else if(distance < 169 && distance > 144)//要客户端判断真是醉了(隐藏bug：在神在144~169范围内选择队伍toggle也会切到任务显示界面)
				{
					if(GameCenter.taskMng.CurSelectToggle != ToggleType.TASK)
					{
						GameCenter.uIMng.CloseGUI(GUIType.TASK);
						GameCenter.taskMng.SetCurSelectToggle(ToggleType.TASK);
						GameCenter.uIMng.GenGUI(GUIType.TASK,true);
					}
				}
			}
		}
	}
	/// <summary>
	/// 第一次进场景的时候没有移动的时候,不会显示神圣晶石界面
	/// </summary>
	void RefreshHolyFirstLogin()
	{
		if(GameCenter.activityMng.GetActivityState(ActivityType.HOLYSPAR) == ActivityState.ONGOING)
		{
			FDictionary itemList = GameCenter.sceneMng.SceneItemInfoDictionary;
			SceneItem holyStone = null;
			foreach(int key in itemList.Keys) 
			{
				SceneItemInfo itemInfo = itemList[key] as SceneItemInfo;
				if(itemInfo.FunctionType == SceneFunctionType.HOLYSTONE)	
				{
					holyStone = GameCenter.curGameStage.GetSceneItem(itemInfo.ServerInstanceID);
					break;
				}
			}
			if(holyStone != null)
			{
				int distance = (int)((GameCenter.curMainPlayer.transform.position - holyStone.transform.position).sqrMagnitude);
				if(distance < 144)
				{
					if(GameCenter.taskMng.CurSelectToggle != ToggleType.HOLYSTONE)
					{
						GameCenter.activityMng.C2S_ReqHolyInfo();
						GameCenter.uIMng.CloseGUI(GUIType.TASK);
						GameCenter.taskMng.SetCurSelectToggle(ToggleType.HOLYSTONE);
						GameCenter.uIMng.GenGUI(GUIType.TASK,true);
					}
				}
			}
		}
	}
	protected System.Text.StringBuilder builder = new System.Text.StringBuilder();
	void RefreshHolyInfo()
	{
		pt_reply_holy_crystal_info_d611 info = GameCenter.activityMng.CurHolyCrystalInfo;
		if(info == null)return;
		if(holyTimer != null)
		{
			int time = GameCenter.activityMng.GetActivityTime(ActivityType.HOLYSPAR);
			holyTimer.StartIntervalTimer(time);
			//活动结束
			if(time != 0)
			{
				holyTimer.onTimeOut = (x)=>
				{
					CloseHolyWnd();
				};
			}
		}
		builder.Remove(0,builder.Length);
		builder.Append("累计获得经验:").Append(info.total_exp).Append("\n");
		builder.Append("累计采集宝箱数:").Append(info.open_box_times).Append("/5").Append("\n");

		List<OtherPlayer> opcList = GameCenter.curGameStage.GetOtherPlayers();
		SceneItem holyStone = null;
		List<SceneItem> itemList = GameCenter.curGameStage.GetSceneItems();
		for (int i = 0,max=itemList.Count; i < max; i++) {
			SceneItem item = itemList[i];
			if(item.MySceneFunctionType == SceneFunctionType.HOLYSTONE)
			{
				holyStone = item;
				break;
			}
		}
		if(holyStone == null)
		{
			//GameCenter.messageMng.AddClientMsg("没有发现神圣水晶石!");
			CloseHolyWnd();//活动结束
			return;
		}
		int opcCount = 0;
		if((GameCenter.curMainPlayer.transform.position - holyStone.transform.position).sqrMagnitude < 25)
			opcCount = 1;//自己在紫色圈里面
		for (int i = 0,max=opcList.Count; i < max; i++) {
			if((opcList[i].transform.position - holyStone.transform.position).sqrMagnitude < 25)
				opcCount++;
		}

		int addNum = 280 - Mathf.Min(opcCount,11)*10 - (int)Mathf.Floor((Mathf.Min(Mathf.Max(opcCount,11),20)-11)/3)*10 - (int)Mathf.Floor((Mathf.Min(Mathf.Max(opcCount,20),40)-20)/5)*10;
		builder.Append("紫色区域当前人数:").Append(opcCount).Append("\n");
		builder.Append("紫色区域当前经验加成:").Append(addNum).Append("%").Append("\n");
		builder.Append("蓝色区域当前经验加成:100%").Append("\n");
		if(labHolyInfo != null)labHolyInfo.text = builder.ToString();
	}
	/// <summary>
	/// 神圣晶石正在进行中
	/// </summary>
	bool onGoing = false;
	void Update()
	{
		if(Time.frameCount % 300 == 0)
		{
			if(GameCenter.activityMng.GetActivityState(ActivityType.HOLYSPAR) == ActivityState.ONGOING)
			{
				onGoing = true;
				RefreshHolyInfo();
			}
			if(onGoing && GameCenter.activityMng.GetActivityState(ActivityType.HOLYSPAR) == ActivityState.HASENDED)
			{
				onGoing = false;
				CloseHolyWndByActivityEnd();
			}
		}
	}
	#endregion

    #endregion
}
