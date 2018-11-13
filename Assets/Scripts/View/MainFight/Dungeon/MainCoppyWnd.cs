//===============================
//作者：邓成
//日期：2016/4/25
//用途：副本显示界面类
//===============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainCoppyWnd : GUIBase {

    #region 队伍和附近的人
    /// <summary>
    /// 队伍按钮
    /// </summary>
    public GameObject teamBtn;
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

    /// <summary>
    /// 当前点击的附近的人
    /// </summary>
    protected OtherPlayerInfo curNearbyPlayerInfo = null;
    /// <summary>
    /// 队员控件实例
    /// </summary>
    public TeamMemberListSingle teamMemberInstance;

    public TeamMemberListSingle[] teamMemberList = new TeamMemberListSingle[2];

    public OtherPlayerListSingle otherListSingleInstance;

    public List<OtherPlayerListSingle> othersList = new List<OtherPlayerListSingle>();
    #endregion

    #region
    public GameObject timeGo;
	public TweenFill timeProgress;//timeProgress.fillAmount = 0.1;
	public UIProgressBar starPb;
	public UILabel labStarDes;
	
	public GameObject copy1v1;
	public GameObject copyNomal;
	#endregion

	#region 单人副本
	public GameObject singleCoppy;//单人副本
	public UILabel coppyName;
	public SubWnd[] coppys;

	public UIButton btnExit;
	public GameObject gameStop;//暂停
	public GameObject gameNoStop;//暂停
	public GameObject gameNoStopBox;//暂停
    public UIButton btnClose;//1V1退出副本

	public UIToggle toggleCoppy;
	public UIToggle toggleOthers;
	#endregion

	#region 多人副本
	public GameObject multipleCoppy;//多人副本
	#endregion

    #region 结算前等待捡东西
    public GameObject coppyTime;//多人副本
	public UITimer coppyDownTime;
	#endregion

    #region 不能组队的副本(一个Toggle的副本)
    public GameObject cantTeamCoppy;
    public UILabel canteamCoppyName;
    #endregion
	void Awake()
	{
		if (teamBtn != null) UIEventListener.Get(teamBtn.gameObject).onClick = OnClickTeamOpenBtn;
		if(dissolveTeamBtn != null)UIEventListener.Get(dissolveTeamBtn).onClick = OnClickDissolveTeamBtn;
		if(fourceOutBtn != null) UIEventListener.Get(fourceOutBtn).onClick = OnClickFourceOutBtn;
		if(giveLeaderBtn != null) UIEventListener.Get(giveLeaderBtn).onClick = OnClickGiveLeaderBtn;
		if(sendMessageBtn != null) UIEventListener.Get(sendMessageBtn).onClick = OnClickChatBtn;
		if(addFriendBtn != null) UIEventListener.Get(addFriendBtn).onClick = OnClickAddFriendBtn;
        if (addGuildBtn != null) UIEventListener.Get(addGuildBtn.gameObject).onClick = OnClickReqGuild;
		if(outTeamBtn != null) UIEventListener.Get(outTeamBtn).onClick = OnClickOutTeamBtn;
		if (joinTeamBtn != null) UIEventListener.Get(joinTeamBtn.gameObject).onClick = OnClickQuickTeamBtn;
		GameObject obj = exResources.GetResource(ResourceType.GUI, "mainUI/TeamInstance") as GameObject;
		if(obj != null)teamMemberInstance = obj.GetComponent<TeamMemberListSingle>();
        obj = null;
        obj = exResources.GetResource(ResourceType.GUI, "") as GameObject;
        if (obj != null) otherListSingleInstance = obj.GetComponent<OtherPlayerListSingle>();
        obj = null;
	}
	#region by 何明军 副本暂停与捡东西
	void Start()
	{
//		Debug.Log("MainCoppyWnd Start");
		if(gameNoStop != null)gameNoStop.SetActive(false);
		if(gameNoStopBox != null)gameNoStopBox.SetActive(false);
		GameCenter.duplicateMng.gameStopNum = 0;
		
		if(gameStop != null){
			gameStop.SetActive(GameCenter.mainPlayerMng.MainPlayerInfo.IsShowStop);
			UIEventListener.Get(gameStop).onClick = delegate {
			if(coppyTime != null && coppyTime.activeSelf){
				GameCenter.messageMng.AddClientMsg(316);
				return ;
			}
			GameCenter.duplicateMng.C2S_GameStop(1);
			};
		}
		if(gameNoStop != null)UIEventListener.Get(gameNoStop).onClick = delegate {
			GameCenter.duplicateMng.C2S_GameStop(0);
		};
		GameCenter.duplicateMng.OnGameStop = delegate {
			bool stop = GameCenter.duplicateMng.IsGameStop;
			for(int i=0;i<coppys.Length;i++){
				if(coppys[i] != null){
					UITimer[] uiTimers = coppys[i].GetComponentsInChildren<UITimer>();
					for(int j=0;j<uiTimers.Length;j++){
						uiTimers[j].StopTime = stop;
					}
				}
			}
			if(timeProgress != null)timeProgress.enabled = !stop;
			if(gameNoStop != null)gameNoStop.SetActive(stop);
			if(gameNoStopBox != null)gameNoStopBox.SetActive(stop);
			if(gameStop != null)gameStop.SetActive(!stop);
		};
		
		if(btnExit != null)UIEventListener.Get(btnExit.gameObject).onClick = ExitCoppy;
        //退出副本
        if (btnClose != null) UIEventListener.Get(btnClose.gameObject).onClick = ExitCoppy;
		
	}
	
	void OpenCoppyTime(){
		if(coppyTime != null)coppyTime.SetActive(true);
		if(coppyDownTime != null){
            SceneUiType uiType = GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType;
            if (uiType == SceneUiType.BOSSCOPPY)
                coppyDownTime.StartIntervalTimer(60);
            else
			    coppyDownTime.StartIntervalTimer(30);
			coppyDownTime.onTimeOut = delegate {
				coppyTime.SetActive(false);
                if (uiType == SceneUiType.NEWBIEMAP)
                    GameCenter.noviceGuideMng.OutNewbieCoppy();
			};
		}
		SceneMng.OnDelInterObj -= UpdateCheckCoppy;
		SceneMng.OnDelInterObj += UpdateCheckCoppy;
		SceneMng.OnDropItemEvent -= UpdateCheckDropItem;
		SceneMng.OnDropItemEvent += UpdateCheckDropItem;
	}
	
	void UpdateCheckDropItem(DropItemInfo info){
		if(GameCenter.sceneMng.DropItemDictionary.Count <= 0){
			if(coppyTime != null)coppyTime.SetActive(false);
			SceneMng.OnDelInterObj -= UpdateCheckCoppy;
			SceneMng.OnDropItemEvent -= UpdateCheckDropItem;
            if (GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType == SceneUiType.NEWBIEMAP)
                GameCenter.noviceGuideMng.OutNewbieCoppy();
            else
			    GameCenter.duplicateMng.C2S_CoppyOver();
		}
	}
	
	void UpdateCheckCoppy(ObjectType type, int id){
        if (type == ObjectType.DropItem && GameCenter.sceneMng.DropItemDictionary.Count <= 0)
        {
			SceneMng.OnDelInterObj -= UpdateCheckCoppy;
			SceneMng.OnDropItemEvent -= UpdateCheckDropItem;
            if (GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType == SceneUiType.NEWBIEMAP)
                GameCenter.noviceGuideMng.OutNewbieCoppy();
            else
			    GameCenter.duplicateMng.C2S_CoppyOver();
		}
	}
	#endregion
	protected override void OnOpen ()
	{
		base.OnOpen ();
		ShowCoppyWnd();
		if(coppyTime != null)coppyTime.SetActive(false);
	}
	protected override void OnClose ()
	{
		base.OnClose ();
        if (coppys == null) return;
        CloseAllWnd();
	}
	protected override void HandEvent (bool _bind)
	{
		base.HandEvent (_bind);
		if(_bind)
		{
			GameCenter.dungeonMng.OnDungeonTimeUpdate += ShowStar;
			GameCenter.dungeonMng.OnStarTimeUpdateEvent += ShowStar;
			GameCenter.teamMng.onTeammateUpdateEvent += OnTeammateUpdateEvent;
			GameCenter.duplicateMng.OnOpenCoppyTime += OpenCoppyTime;
			GameCenter.duplicateMng.OnOpenCopySettlement += StopTimer;
            GameCenter.teamMng.OpenTeamWndEvent += ShowTeamWnd;
            SceneMng.OnOPCInfoListUpdate += RefreshOthers;
            SceneMng.OnDelInterObj += RefreshOthersByOutline;
		}else
		{
			GameCenter.dungeonMng.OnDungeonTimeUpdate -= ShowStar;
			GameCenter.dungeonMng.OnStarTimeUpdateEvent -= ShowStar;
			GameCenter.teamMng.onTeammateUpdateEvent -= OnTeammateUpdateEvent;
			GameCenter.duplicateMng.OnOpenCoppyTime -= OpenCoppyTime;
			GameCenter.duplicateMng.OnOpenCopySettlement -= StopTimer;
            GameCenter.teamMng.OpenTeamWndEvent -= ShowTeamWnd;
            SceneMng.OnOPCInfoListUpdate -= RefreshOthers;
            SceneMng.OnDelInterObj -= RefreshOthersByOutline;
		}
	}
    void CloseAllWnd()
    {
        for (int i = 0, max = coppys.Length; i < max; i++)
        {
            if (coppys[i] != null)
                coppys[i].CloseUI();//关闭所有界面
        }
    }

	void ShowCoppyWnd()
	{
		if(coppys == null)return;
        CloseAllWnd();
		if(toggleOthers != null)toggleOthers.gameObject.SetActive(false);
		if(toggleCoppy != null)toggleCoppy.gameObject.SetActive(false);
		switch(GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType)
		{
		case SceneUiType.ARENA://竞技场
		case SceneUiType.BUDOKAI:
			if(copy1v1 != null)copy1v1.SetActive(true);
			if(copyNomal != null)copyNomal.SetActive(false);
			break;
		case SceneUiType.GUILDPROTECT://仙域守护
		case SceneUiType.GUILDWAR://帮派战
        case SceneUiType.BATTLEFIGHT://火焰山战场
		case SceneUiType.GUILDFIRE://仙盟篝火
		case SceneUiType.RONGELAND://熔恶之地
		case SceneUiType.LIRONGELAND://里熔恶之地
			Debug.LogError("已经移到其他地方,显示不对!检查场景配置表");
			break;
		case SceneUiType.SEALBOSS://封印BOSS
        case SceneUiType.NEWBIEMAP:
			if(toggleCoppy != null)
			{
				toggleCoppy.gameObject.SetActive(true);
				toggleCoppy.value = true;
			}
			if(copy1v1 != null)copy1v1.SetActive(false);
			if(copyNomal != null)copyNomal.SetActive(true);
			singleCoppy.SetActive(true);
			multipleCoppy.SetActive(false);
            cantTeamCoppy.SetActive(false);
			if(timeGo != null)timeGo.SetActive(false);
			break;
		case SceneUiType.UNDERBOSS:
			if(toggleOthers != null)
			{
				toggleOthers.gameObject.SetActive(true);
				toggleOthers.value = true;
			}
			if(copy1v1 != null)copy1v1.SetActive(false);
			if(copyNomal != null)copyNomal.SetActive(true);
			singleCoppy.SetActive(true);
			multipleCoppy.SetActive(false);
            cantTeamCoppy.SetActive(false);
			if(timeGo != null)timeGo.SetActive(false);
			if(gameStop != null)gameStop.SetActive(false);
			break;
		case SceneUiType.GODSWAR:
        case SceneUiType.BOSSCOPPY:
			if(toggleCoppy != null)
			{
				toggleCoppy.gameObject.SetActive(true);
				toggleCoppy.value = true;
			}
			if(copy1v1 != null)copy1v1.SetActive(false);
			if(copyNomal != null)copyNomal.SetActive(true);
			singleCoppy.SetActive(true);
			multipleCoppy.SetActive(false);
            cantTeamCoppy.SetActive(false);
			if(timeGo != null)timeGo.SetActive(false);
			GameCenter.uIMng.GenGUI(GUIType.LITTLEMAP,true);//封神之地要小地图,不要星级
			break;
        case SceneUiType.RAIDERARK:
            if(toggleCoppy != null)
			{
				toggleCoppy.gameObject.SetActive(true);
				toggleCoppy.value = true;
			}
			if(copy1v1 != null)copy1v1.SetActive(false);
			if(copyNomal != null)copyNomal.SetActive(true);
			singleCoppy.SetActive(false);
			multipleCoppy.SetActive(false);
            cantTeamCoppy.SetActive(true);
			if(timeGo != null)timeGo.SetActive(false);
			GameCenter.uIMng.GenGUI(GUIType.LITTLEMAP,true);//夺宝奇兵要小地图,不要星级
			break;
        case SceneUiType.ENDLESS:
            if (timeGo != null) timeGo.SetActive(false);
            break;
		default://单人副本
			if(toggleCoppy != null)
			{
				toggleCoppy.gameObject.SetActive(true);
				toggleCoppy.value = true;
			}
			if(copy1v1 != null)copy1v1.SetActive(false);
			if(copyNomal != null)copyNomal.SetActive(true);
			singleCoppy.SetActive(true);
			multipleCoppy.SetActive(false);
            cantTeamCoppy.SetActive(false);
			if(timeGo != null)timeGo.SetActive(true);
			break;
		}
		int uiType = (int)GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType;
		if(coppys != null && coppys.Length >= uiType && GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType != SceneUiType.NONE && coppys[uiType-1] != null)
		{
			coppys[uiType-1].OpenUI();//打开当前副本对应界面
		}
		if(coppyName != null)coppyName.text = GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef == null?string.Empty:GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef.name;
        if (canteamCoppyName != null) canteamCoppyName.text = GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef == null ? string.Empty : GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef.name;
	}
	void ExitCoppy(GameObject go)
	{
		GameCenter.duplicateMng.OutCopyWnd();
	}
	void ShowStar()
	{
		int threeStarSecond = 0;
		int twoStarSecond = 0;
		MainPlayerInfo mainPlayerInfo = GameCenter.mainPlayerMng.MainPlayerInfo;
		SceneRef sceneRef = mainPlayerInfo.CurSceneRef;
		bool isTowerScene = false;
		if(sceneRef != null)
		{
			//镇魔塔三星时间特殊
			if(mainPlayerInfo.CurSceneUiType == SceneUiType.TOWER)
			{
				isTowerScene = true;
				int layer = GameCenter.dungeonMng.DungeonWave;
				int sceneID = sceneRef.id;
				TowerRef towerRef = ConfigMng.Instance.GetTowerRefByLayer(layer,sceneID);
				if(towerRef != null)
				{
					threeStarSecond = towerRef.Time3;
					twoStarSecond = towerRef.Time2;
				}
			}else
			{
				threeStarSecond = sceneRef.starTime3;
				twoStarSecond = sceneRef.starTime2;
			}
			int actionTime = sceneRef.dropRemainTimes - GameCenter.dungeonMng.DungeonTime;//副本已经进行的时间
			int realTime = (int)Time.realtimeSinceStartup - GameCenter.dungeonMng.DungeonStartTime;//接到协议到此处的时间差
			int realActionTime = actionTime + realTime;
			Debug.Log("threeStarSecond:"+threeStarSecond+",twoStarSecond:"+twoStarSecond+",realActionTime:"+realActionTime+",realTime:"+realTime+",sceneRef.dropRemainTimes:"+sceneRef.dropRemainTimes);
			float fromPoint = 1.0f;
			float durationTime = 0f;
			bool startWithThreeSatr = false;
			bool startWithTwoSatr = false;
			if(isTowerScene)
			{ 
				startWithThreeSatr = realTime < threeStarSecond - 1;
				startWithTwoSatr = realTime < threeStarSecond + twoStarSecond -1;
                if (startWithThreeSatr)
                { 
                    durationTime = threeStarSecond - realTime;
                }
                else if (startWithTwoSatr)
                { 
                    durationTime = threeStarSecond + twoStarSecond - realTime;
                }
				fromPoint = 1.0f;
			}else
			{
				if(realActionTime < threeStarSecond)//从三星开始计时(断线重连)
				{
					startWithThreeSatr = true;
					fromPoint = (float)(threeStarSecond - realActionTime)/(float)threeStarSecond;
					durationTime = (float)(threeStarSecond - realActionTime);
				}
				if(!startWithThreeSatr && realActionTime < threeStarSecond+twoStarSecond)//从二星开始计时
				{
					startWithTwoSatr = true;
					fromPoint = (float)(twoStarSecond + threeStarSecond - realActionTime)/(float)twoStarSecond;
					durationTime = (float)(twoStarSecond + threeStarSecond - realActionTime);
				}
			}
			if(startWithThreeSatr)//-1防止TweenFill比Time.realtimeSinceStartup快一点点导致bug
			{
				ShowStar(3);
				if(labStarDes != null)labStarDes.text = threeStarSecond + ConfigMng.Instance.GetUItext(298);
				if(timeProgress != null)
				{
					timeProgress.ResetToBeginning();
					timeProgress.from = fromPoint;
					timeProgress.duration = durationTime;
					timeProgress.AddOnFinished(()=>
						{
							ShowStar();
						});
					timeProgress.enabled = true;
				}
			}else if(startWithTwoSatr)//-1防止TweenFill比Time.realtimeSinceStartup快一点点导致bug
			{
				ShowStar(2);
                if (labStarDes != null) labStarDes.text = twoStarSecond + ConfigMng.Instance.GetUItext(299);
				if(timeProgress != null)
				{
					timeProgress.ResetToBeginning();
					timeProgress.from = fromPoint;
					timeProgress.duration = durationTime;
					timeProgress.AddOnFinished(()=>
						{
							ShowStar();
						});
					timeProgress.enabled = true;
				}
			}else
			{
				ShowStar(1);
                if (labStarDes != null) labStarDes.text = ConfigMng.Instance.GetUItext(300);
			}
		}
	}
	void ShowStar(int starNum)
	{
		if(starPb != null)
		{
			starPb.value = (float)starNum/3f;
		}
	}


	void StopTimer()
	{
		if(timeGo != null)timeGo.SetActive(false);
		if(timeProgress != null)timeProgress.enabled = false;
	}

	#region 队伍操作
	protected void OnClickTeamOpenBtn(GameObject _obj)
	{
		RefreshTeam();
	}
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
			GameCenter.teamMng.C2S_TeamOut();
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
        if (othersParent != null) othersParent.SetActive(false);
        if (teamParent != null) teamParent.gameObject.SetActive(false);
        if (GameCenter.teamMng.isInTeam)
        {
            joinTeamBtn.SetActive(false);
            FDictionary memberDic = GameCenter.teamMng.TeammatesDic;
            if (memberDic.Count == 1)//队伍中只有自己
            {
                if (dissolveTeamBtn != null) dissolveTeamBtn.SetActive(true);
            }
            else
            {
                if (teamParent != null) teamParent.gameObject.SetActive(true);
                if (dissolveTeamBtn != null) dissolveTeamBtn.SetActive(false);
                int index = 0;
                foreach (TeamMenberInfo item in memberDic.Values)
                {
                    if (item.baseInfo.uid == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
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
            if (dissolveTeamBtn != null) dissolveTeamBtn.SetActive(false);
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
    void RefreshOthersByOutline(ObjectType objType, int id)
    {
        if (objType == ObjectType.Player)
            RefreshOthers();
    }
	#endregion

    protected void OnClickNearbyPlayerBtn(GameObject go)
	{
        curNearbyPlayerInfo = UIEventListener.Get(go).parameter as OtherPlayerInfo;
        if (btnParent != null) btnParent.gameObject.SetActive(true);
	}

    void ShowTeamWnd()
    {
        if (teamBtn != null)
        {
            teamBtn.SendMessage("OnClick");
        }
    }
}
