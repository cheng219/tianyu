//============================
//作者：何明军
//日期：2016/3/23
//用途：无尽挑战，竞技场，副本，暂停，活动大厅系统数据
//============================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;
using System.Text;

public class EndLessTrialsMng  {
	#region 构造
	public static EndLessTrialsMng CreateNew(){
		if (GameCenter.endLessTrialsMng == null)
		{
			EndLessTrialsMng endLessTrialsMng = new EndLessTrialsMng();
			endLessTrialsMng.Init();
			return endLessTrialsMng;
		}
		else
		{
			GameCenter.endLessTrialsMng.UnRegist();
			GameCenter.endLessTrialsMng.Init();
			return GameCenter.endLessTrialsMng;
		}
	} 

	protected void Init(){
		MsgHander.Regist(0xD435,S2C_AllEndList);
		MsgHander.Regist(0xD437,S2C_AllEndReWard);
		MsgHander.Regist(0xD449,S2C_EndLessItemUpdate);
		MsgHander.Regist(0xD451,S2C_SweepRewardAll);
		
		//MsgHander.Regist(0xD453,S2C_CopyItemList);
		//MsgHander.Regist(0xD455,S2C_BuyCopyInNum);
		//MsgHander.Regist(0xD457,S2C_CopyInSceneStar);
		//MsgHander.Regist(0xD460,S2C_CopyItemTeams);
		
		//MsgHander.Regist(0xD465,S2C_ReqCopyInTeamClose);
		//MsgHander.Regist(0xD463,S2C_ReqCopyInChange);
		//MsgHander.Regist(0xD466,S2C_ReqCopyInPerpare);
		//MsgHander.Regist(0xD467,S2C_ReqSettlementRewardData);
		//MsgHander.Regist(0xD469,S2C_ReqSettlementFlopData);
		//MsgHander.Regist(0xD470,S2C_ReqOpenCopyForce);
		
		//MsgHander.Regist(0xD485,S2C_ArenaServerDataInfo);
        //MsgHander.Regist(0xD491, S2C_ReqArenaRewardData);
        //MsgHander.Regist(0xD489,S2C_ArenaServerReward);
		//MsgHander.Regist(0xD47a,S2C_GameStop);
		//MsgHander.Regist(0xD715,S2C_ActivityDataInfo);
		//MsgHander.Regist(0xD716,S2C_MagicTowers);
		
		//MsgHander.Regist(0xD746,S2C_InvitationPlayer);
		//MsgHander.Regist(0xD749,S2C_CopyInFriend);
		//MsgHander.Regist(0xD753,S2C_ReqOpenCoppyTime);
		
		//MsgHander.Regist(0xD756,S2C_FairyDomainToprotect);
		//MsgHander.Regist(0xD771,S2C_TeamAddInvitationPlayer);
	}

	protected void UnRegist(){
		//IsGameStop = false;
		isFastClearance = false;
		MsgHander.UnRegist(0xD435,S2C_AllEndList);
		MsgHander.UnRegist(0xD437,S2C_AllEndReWard);
		MsgHander.UnRegist(0xD449,S2C_EndLessItemUpdate);
		MsgHander.UnRegist(0xD451,S2C_SweepRewardAll);
		
		//MsgHander.UnRegist(0xD453,S2C_CopyItemList);
		//MsgHander.UnRegist(0xD455,S2C_BuyCopyInNum);
		//MsgHander.UnRegist(0xD457,S2C_CopyInSceneStar);
		//MsgHander.UnRegist(0xD460,S2C_CopyItemTeams);
		
		//MsgHander.UnRegist(0xD465,S2C_ReqCopyInTeamClose);
		//MsgHander.UnRegist(0xD463,S2C_ReqCopyInChange);
		//MsgHander.UnRegist(0xD466,S2C_ReqCopyInPerpare);
		//MsgHander.UnRegist(0xD467,S2C_ReqSettlementRewardData);
		//MsgHander.UnRegist(0xD469,S2C_ReqSettlementFlopData);
		//MsgHander.UnRegist(0xD470,S2C_ReqOpenCopyForce);
		
		//MsgHander.UnRegist(0xD485,S2C_ArenaServerDataInfo);
		//MsgHander.UnRegist(0xD491,S2C_ReqArenaRewardData);
		//MsgHander.UnRegist(0xD489,S2C_ArenaServerReward);
		//MsgHander.UnRegist(0xD47a,S2C_GameStop);
		//MsgHander.UnRegist(0xD715,S2C_ActivityDataInfo);
		//MsgHander.UnRegist(0xD716,S2C_MagicTowers);
		
		//MsgHander.UnRegist(0xD746,S2C_InvitationPlayer);
		//MsgHander.UnRegist(0xD749,S2C_CopyInFriend);
		//MsgHander.UnRegist(0xD753,S2C_ReqOpenCoppyTime);
		//MsgHander.UnRegist(0xD756,S2C_FairyDomainToprotect);
		//MsgHander.UnRegist(0xD771,S2C_TeamAddInvitationPlayer);
        
        endDIc.Clear();
        //copyDic.Clear();
        //copyTeams.Clear();
        //openCopyForceTip = false;
        //CopySettlementDataInfo = null;
        //ArenaServerDataInfo = null;
        //activityDic.Clear();
		sweepListItem.Clear();
		//friendmsg.Clear();
		curId = 1;
        isShowEndlessResetTip = true;
        //copyGroupID = 0;
        //againCopyID=0;
        //againSceneID = 0;
        //lcopyGroupRef = null;
        //openEndless = false;
    }

	#endregion
	#region 无尽挑战
	#region 事件, 变量与访问器
    /// <summary>
    /// 是否显示重置确认框
    /// </summary>
    public bool isShowEndlessResetTip = true;
    protected int restRefreshTime = 0;
    /// <summary>
    /// 剩余刷新次数
    /// </summary>
    public int RestRefreshTime
    {
        get
        {
            return restRefreshTime;
        }
        protected set
        {
            if (restRefreshTime != value)
            { 
                restRefreshTime = value;
                if (OnRestRefreshTimeUpdate != null)
                    OnRestRefreshTimeUpdate();
            }
        }
    }
	/// <summary>
	/// 无尽挑战数据
	/// </summary>
	Dictionary<int,EndLessTrialsDataInfo> endDIc = new Dictionary<int, EndLessTrialsDataInfo>();
	int curId = 1;
	/// <summary>
	/// 当前章节
	/// </summary>
	public int CurChapterID{
		get{
			if(curId > ConfigMng.Instance.GetChapterRefTable().Count){
				curId = ConfigMng.Instance.GetChapterRefTable().Count;
			}
			return curId > 0?curId:1;
		}
		set{
			curId = value;
			if(OnCurChapterUpdate != null)OnCurChapterUpdate();
		}
	}
    /// <summary>
    /// 剩余刷新次数更新
    /// </summary>
    public System.Action OnRestRefreshTimeUpdate;
	/// <summary>
	/// 当前章节变化
	/// </summary>
	public System.Action OnCurChapterUpdate;
	/// <summary>
	/// 关卡数据变更
	/// </summary>
	public System.Action OnCurChapterItemUpdate;
    /// <summary>
    /// 星星数量变更
    /// </summary>
	public System.Action OnCurChapterStarUpdate;
    /// <summary>
    /// 扫荡奖励
    /// </summary>
	public System.Action OnSweepRewardAll;
	/// <summary>
	/// 获取星星数量
	/// </summary>
	public int GetTotalStar(){
		int num = 0;
		//if(endDIc.ContainsKey(CurChapterID)){
        foreach (EndLessTrialsDataInfo info in endDIc.Values)
        {
            num += info.itemsList.Count;//每关固定一颗星
        }
		return num;
	}
    public int GetTotalStar(int chapterID)
    {
        int num = 0;
        if (endDIc.ContainsKey(chapterID))
        {
            foreach (EndLessTrialsDataInfo.EndLessTrialsItemData info in endDIc[chapterID].itemsList.Values)
            {
                num += info.star;
            }
        }
        return num;
    }

    /// <summary>
    /// 获取数据
    /// </summary>
    public EndLessTrialsDataInfo.EndLessTrialsItemData GetItemData(int itemID){
		if(endDIc.ContainsKey(CurChapterID)){
			if(endDIc[CurChapterID].itemsList.ContainsKey(itemID)){
				return endDIc[CurChapterID].itemsList[itemID];
			}
		}
		return null;
	}
    /// <summary>
    /// 无尽试炼的红点计数(可领取的星级奖励红点总计)
    /// </summary>
    int endlessRedNum = 0;
    public int EndlessRedNum
    {
        get
        {
            return endlessRedNum;
        }
        set
        {
            endlessRedNum = value;
        }
    }
    ///// <summary>
    ///// 结算之后是否打开一下无尽试炼(无尽入口的红点计算是在界面中进行的)
    ///// </summary>
    //bool openEndless = false;
    //public bool OpenEndless
    //{
    //    get
    //    {
    //        return openEndless;
    //    }
    //    set
    //    {
    //        openEndless = value;
    //    }
    //}
    /// <summary>
    /// 获得前置关卡是否开启
    /// </summary>
    public bool GetItemDataFront(int frontId){
		if(endDIc.ContainsKey(CurChapterID)){//本章节里查找
			if(endDIc[CurChapterID].itemsList.ContainsKey(frontId)){
				return true;
			}
		}
		if(endDIc.ContainsKey(CurChapterID - 1)){//上个章节里查找
			if(endDIc[CurChapterID - 1].itemsList.ContainsKey(frontId)){
				return true;
			}
		}
		return false;
	}
	/// <summary>
	/// 是否可以扫荡
	/// </summary>
	public bool IsSweepingEndless{
		get{
			foreach(EndLessTrialsDataInfo data in endDIc.Values){
				foreach(EndLessTrialsDataInfo.EndLessTrialsItemData info in data.itemsList.Values){
					if(info.star >= 1){
						return true;
					}
				}
			}
			GameCenter.messageMng.AddClientMsg(266);
			return false;
		}
	}
	/// <summary>
	/// 获得关卡是否开启
	/// </summary>
	public bool GetItemDataCurFront(int dataID){
		CheckPointRef refData = ConfigMng.Instance.GetCheckPointRef(dataID);
		return refData == null ? false : GetItemDataFront(refData.frontGate);
	}
	
	/// <summary>
	/// 关卡击杀了没有
	/// </summary>
	public bool GetItemDataIsKill(int id){
		foreach(EndLessTrialsDataInfo data in endDIc.Values){
			if(data.itemsList.ContainsKey(id)){
				return data.itemsList[id].enter > 0;
			}
		}
		return false;
	}
	int GetNextNum = 0;
	/// <summary>
	/// 遍历找到可打的下个关卡
	/// </summary>
	public int GetNextItemData(int sceneID){
        CheckPointRef chekRef = ConfigMng.Instance.GetFrontGateCheckPoint(sceneID);
        int id = chekRef == null ? 0 : chekRef.id;
		if(!GetItemDataIsKill(id)){
			GetNextNum = 0;
			return id;
		}else{
			if(GetNextNum > 600){
				Debug.LogError("遍历次数600！未避免死循环中断遍历，请策划检查数据。");
				GetNextNum = 0;
				return 0;
			}
			GetNextNum ++;
			return GetNextItemData(id);
		}
		//GetNextNum = 0;
		//return 0;
	}
	/// <summary>
	/// 获取星星的领取状态
	/// </summary>
	public bool GetStarReward(int _chapterID,int _star){
		if(endDIc.ContainsKey(_chapterID)){
			return endDIc[_chapterID].starsList.ContainsKey(_star) ? endDIc[_chapterID].starsList[_star].receive : false;
		}
		return false;
	}
	
	/// <summary>
	/// 是否第一次通关无尽
	/// </summary>
	public bool IsFastClearance{
		get {
			return isFastClearance;
		}
		set{
			if(isFastClearance != value){
				isFastClearance = value;
			}
		}
	}
	bool isFastClearance = false;
    /// <summary>
    /// 是否是无尽最后一关
    /// </summary>
    public bool isLastChapter
    {
        get
        { 
            int curScene = GameCenter.mainPlayerMng.MainPlayerInfo.SceneID;
            int scene = GetNextItemData(curScene);
            return scene == 0 ? true : false;
        }
    }
	/// <summary>
	/// 下一关
	/// </summary>
	public void NextEnd(){
		int curScene = GameCenter.mainPlayerMng.MainPlayerInfo.SceneID;
		int scene = GetNextItemData(curScene);
		int chapterID = 0;
		foreach(ChapterRef _data in ConfigMng.Instance.GetChapterRefTable().Values){
			if(_data.allLevels.Contains(scene)){
				chapterID = _data.id;
				break ;
			}
		}
		C2S_InEndItem(chapterID,scene);
	}
	
	#endregion
	
	#region  协议接受
	void S2C_AllEndList(Pt _info){
		pt_endless_pass_list_d435 info = _info as pt_endless_pass_list_d435;
        if (info != null)
        {
            RestRefreshTime = info.reset_num;
            int id = 0;
            for (int i = 0; i < info.endless_list.Count; i++)
            {
                st.net.NetBase.endless_list data = info.endless_list[i];
                id = (int)data.chpter_id;
                if (endDIc.ContainsKey(id))
                {
                    endDIc[id].Update(data);
                }
                else
                {
                    endDIc[id] = new EndLessTrialsDataInfo(data);
                }
                if (id == 1)
                {
                    isFastClearance = false;
                }
            }

            ChapterRef chapterRef = ConfigMng.Instance.GetChapterRefData(id);
            if (chapterRef != null)
            {
                if (endDIc[id].itemsList.Count >= chapterRef.allLevels.Count)
                {
                    CurChapterID = id + 1;
                }
                else
                {
                    CurChapterID = id;
                }
                //GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.ENDLESSTRIAL,true);
            }
            CountRedPoint();
            if (OnCurChapterUpdate != null) OnCurChapterUpdate();
        }
	}
	void S2C_EndLessItemUpdate(Pt _info){
		pt_update_endless_pass_d449 info = _info as pt_update_endless_pass_d449;
		if(endDIc.ContainsKey(info.chapter_id)){
			if(info.chapter_id == 1 && endDIc[info.chapter_id].itemsList.Count == 0){
				isFastClearance = true;
			}
			if(endDIc[info.chapter_id].itemsList.ContainsKey(info.pass_id)){
				endDIc[info.chapter_id].itemsList[info.pass_id].star = info.star_num;
				endDIc[info.chapter_id].itemsList[info.pass_id].enter = info.state;
				endDIc[info.chapter_id].itemsList[info.pass_id].time = info.time;
			}else{
				endDIc[info.chapter_id].itemsList[info.pass_id] = new EndLessTrialsDataInfo.EndLessTrialsItemData();
				endDIc[info.chapter_id].itemsList[info.pass_id].star = info.star_num;
				endDIc[info.chapter_id].itemsList[info.pass_id].enter = info.state;
				endDIc[info.chapter_id].itemsList[info.pass_id].time = info.time;
			}
		}else{
			endDIc[info.chapter_id] = new EndLessTrialsDataInfo();
			endDIc[info.chapter_id].id = info.chapter_id;
			if(info.chapter_id == 1 && endDIc[info.chapter_id].itemsList.Count == 0){
				isFastClearance = true;
			}
			endDIc[info.chapter_id].itemsList[info.pass_id] = new EndLessTrialsDataInfo.EndLessTrialsItemData();
			endDIc[info.chapter_id].itemsList[info.pass_id].star = info.star_num;
			endDIc[info.chapter_id].itemsList[info.pass_id].enter = info.state;
			endDIc[info.chapter_id].itemsList[info.pass_id].time = info.time;
            //endDIc[info.chapter_id].starsList[info.pass_id].receive
        }
        CountRedPoint();
        if (OnCurChapterItemUpdate != null)OnCurChapterItemUpdate();
	}

	void S2C_AllEndReWard(Pt _info){
		pt_stra_reward_list_d437 info = _info as pt_stra_reward_list_d437;
		if(endDIc.ContainsKey(info.chapter_id)){
			if(endDIc[info.chapter_id].starsList.ContainsKey(info.star_id)){
				endDIc[info.chapter_id].starsList[info.star_id].receive = info.state ==1;
                Debug.Log("endDIc[info.chapter_id].starsList[info.star_id].receive:"+ endDIc[info.chapter_id].starsList[info.star_id].receive);
			}else{
				endDIc[info.chapter_id].starsList[info.star_id] = new EndLessTrialsDataInfo.EndLessTrialsStarData();
				endDIc[info.chapter_id].starsList[info.star_id].receive = info.state ==1;
                Debug.Log("endDIc[info.chapter_id].starsList[info.star_id].receive:" + endDIc[info.chapter_id].starsList[info.star_id].receive);

            }
		}else{
			endDIc[info.chapter_id] = new EndLessTrialsDataInfo();
			endDIc[info.chapter_id].id = info.chapter_id;
			endDIc[info.chapter_id].starsList[info.star_id] = new EndLessTrialsDataInfo.EndLessTrialsStarData();
			endDIc[info.chapter_id].starsList[info.star_id].receive = info.state ==1;
        }
        CountRedPoint();
        if (OnCurChapterStarUpdate != null)OnCurChapterStarUpdate();
	}
	//扫荡
	void S2C_SweepRewardAll(Pt _info){
		pt_copy_sweep_list_d451 info = _info as pt_copy_sweep_list_d451;
		sweepListItem.Clear();
		Dictionary<uint,ItemValue> itemList = new Dictionary<uint,ItemValue>();
		itemList[3] = new ItemValue(3,0);
		itemList[5] = new ItemValue(5,0);
		itemList[7] = new ItemValue(7,0);
        for (int i = 0; i < info.copy_sweep.Count; i++)
        {
            st.net.NetBase.copy_sweep_list item = info.copy_sweep[i];
			if(itemList.ContainsKey(item.type)){
				itemList[item.type].count += (int)item.num;
			}else{
				itemList[item.type] = new ItemValue((int)item.type, (int)item.num);
			}
        }
		sweepListItem = new List<ItemValue>(itemList.Values);
        if (OnSweepRewardAll != null)
        {
            OnSweepRewardAll();
        }
        CountRedPoint();
    }
	#endregion

	#region  协议发送
    /// <summary>
    /// 请求重置无尽数据
    /// </summary>
    public void C2S_ResetEndlessInfo()
    { 
        pt_req_reset_endless_c150 info = new pt_req_reset_endless_c150();
        NetMsgMng.SendMsg(info);
    }
	/// <summary>
	/// 请求无尽数据
	/// </summary>
	public void C2S_EndList(){
		pt_req_endless_list_d433 info = new pt_req_endless_list_d433();
		NetMsgMng.SendMsg(info);
	}
	/// <summary>
	/// 请求进入关卡
	/// </summary>
	public void C2S_InEndItem(int _id,int _itemid){
		pt_req_challenge_endless_d434 info = new pt_req_challenge_endless_d434();
		info.chapter_id = _id;
		info.pass_id = _itemid;
		NetMsgMng.SendMsg(info);
	}
	/// <summary>
	/// 请求无尽星星奖励
	/// </summary>
	public void C2S_EndReward(int _id,int idList){ 
		pt_req_get_star_reward_d436 info = new pt_req_get_star_reward_d436();
		info.chapter_id = _id;
		info.star_id = idList;
		NetMsgMng.SendMsg(info);
	}
	/// <summary>
	/// 扫荡关卡
	/// </summary>
	public int sweepCopyID;
	public SweepType sweepType;
	public enum SweepType{
		COPY = 1,
		EndLess = 2,
	}
	/// <summary>
	/// 扫荡奖励
	/// </summary>
	public List<ItemValue> sweepListItem = new List<ItemValue>();
	/// <summary>
	/// 扫荡，（_id{1=单人，2=无尽},idList{无尽=0，单人=副本ID}）
	/// </summary>
	public void C2S_SweepReward(int _id,int idList){
        //if(SweepType.COPY ==  (SweepType)_id){
        //    using (var data = copyDic.GetEnumerator()){
        //        while(data.MoveNext()){
        //            if(data.Current.Value.copyScenes.ContainsKey(idList) && data.Current.Value.num <= 0){
        //                GameCenter.messageMng.AddClientMsg(244);
        //                return ;
        //            }
        //        }
        //    }
        //}
		pt_req_copy_sweep_d450 info = new pt_req_copy_sweep_d450();
		info.scene_type = idList;
		info.state = _id;
		sweepType = (SweepType)_id;
		sweepCopyID = idList;
		NetMsgMng.SendMsg(info);
	}
    #endregion
    #region 计算是否有奖励可以领取
    public void CountRedPoint()
    {
        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.ENDLESSTRIAL, false);
        bool needRedTip = false;
        FDictionary chapterRefTable = ConfigMng.Instance.GetChapterRefTable();
        foreach (ChapterRef _data in chapterRefTable.Values)
        {
            if (endDIc.ContainsKey(_data.id))
            {
                int starNum = GetTotalStar(_data.id);
                //Debug.Log("_data.id:" + _data.id + ",starNum:" + starNum);
                for (int i = 0, length = _data.rewardData.Count; i < length; i++)
                {
                    ChapterReward reward = _data.rewardData[i];
                    bool rewardState = GameCenter.endLessTrialsMng.GetStarReward(_data.id, reward.starNum);
                    //Debug.Log("rewardState:" + rewardState + ",starNum:" + reward.starNum);
                    needRedTip = (starNum >= reward.starNum) && !rewardState;
                    if (needRedTip)
                        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.ENDLESSTRIAL, needRedTip);
                    else
                    {

                    }
                }
            }
        }
        //Debug.Log("needRedTip:" + needRedTip);
        }
    }
    #endregion
    #endregion



    #region 单人与多人副本

    //#region 副本跳转
    ///// <summary>
    ///// 当前选择的副本
    ///// </summary>
    //public OneCopySType CurSelectOneCopyType = OneCopySType.NONE;
    ///// <summary>
    ///// 进入副本界面，选中副本
    ///// </summary>
    //public void OpenCopyWndSelected(SubGUIType _type, OneCopySType _oneCopyType)
    //{
    //    CurSelectOneCopyType = _oneCopyType;
    //    GameCenter.uIMng.SwitchToSubUI(_type);
    //}
    //#endregion

    //   #region 事件, 变量与访问器

    //   Dictionary<int,CopyInItemDataInfo> copyDic = new Dictionary<int,CopyInItemDataInfo>();
    ///// <summary>
    ///// 多人副本数据
    ///// </summary>
    //public Dictionary<int,CopyInItemDataInfo> CopyDic{
    //	get{
    //		return copyDic;
    //	}
    //}
    //Dictionary<int,CopySceneTeamPlayerInfo> copyTeams = new Dictionary<int,CopySceneTeamPlayerInfo>();
    //   /// <summary>
    //   /// 多人副本组队数据
    //   /// </summary>
    //public Dictionary<int,CopySceneTeamPlayerInfo> CopyTeams{
    //	get{
    //		return copyTeams;
    //	}
    //}
    //CopysType copyType = CopysType.ONESCOPY;
    ///// <summary>
    ///// 1=单人副本,2=多人副本,3= 镇魔塔
    ///// </summary>
    //public enum CopysType{
    //	ONESCOPY = 1,
    //	TWOSCOPY = 2,
    //	MAGICTOWER = 3,
    //}
    ///// <summary>
    ///// 副本类型
    ///// </summary>
    //public CopysType CopyType{
    //	get{
    //		return copyType;
    //	}
    //	set{
    //		if(copyType != value){
    //			copyType = (CopysType)value;
    //			if(OnCopyTypeChange != null)OnCopyTypeChange();
    //		}
    //	}
    //}
    ///// <summary>
    ///// 当前副本组ID，共跳转到副本界面并选中某个副本组
    ///// </summary>
    //public int copyGroupID = 0;

    ///// <summary>
    ///// 化身数据
    ///// </summary>
    //public List<OtherPlayerInfo> OpenInvitationPlayerData{
    //	get{
    //		List<OtherPlayerInfo> Data = new List<OtherPlayerInfo>();
    //		for(int i=0;i<openInvitationPlayerData.Count;i++){
    //			if(!copyTeams.ContainsKey(openInvitationPlayerData[i].ServerInstanceID)){
    //				Data.Add(openInvitationPlayerData[i]);
    //			}
    //		}
    //		return Data;
    //	}
    //}
    //List<OtherPlayerInfo> openInvitationPlayerData = new List<OtherPlayerInfo>();
    //public OtherPlayerInfo GetInvitationPlayerData(int id){
    //	for(int i=0;i<openInvitationPlayerData.Count;i++){
    //		if(id == openInvitationPlayerData[i].ServerInstanceID){
    //			return openInvitationPlayerData[i];
    //		}
    //	}
    //	return null;
    //}
    ///// <summary>
    ///// 多人准备数据变化
    ///// </summary>
    //public System.Action OnSelectChange;
    ///// <summary>
    ///// 化身数据变化
    ///// </summary>
    //public System.Action OpenInvitationPlayer;
    ///// <summary>
    ///// 副本类型变化
    ///// </summary>
    //public System.Action OnCopyTypeChange;
    ///// <summary>
    ///// 多人界面队员数据
    ///// </summary>
    //public System.Action<int> OnCopyItemTeamData;
    ///// <summary>
    ///// 副本数据变化
    ///// </summary>
    //public System.Action OnCopyItemChange;
    ///// <summary>
    ///// 多人界面关闭
    ///// </summary>
    //public System.Action OnCopyItemUIClose;
    ///// <summary>
    ///// 镇魔塔开启
    ///// </summary>
    //public System.Action<int,int> OnMagicTowerUIOpen;
    //   /// <summary>
    //   /// 镇魔塔点击招募队友
    //   /// </summary>
    //   public System.Action OnClickAddFreiend;
    //   /// <summary>
    //   /// 是否是镇魔塔招募队友
    //   /// </summary>
    //   public bool isMagicTowrAddFri = false;
    //   public bool isClickAddFri = false;
    //#endregion

    ////    #region 副本和扫荡 快捷购买
    ////    /// <summary>
    ////    /// 获取相应Vip等级和购买次数所需的元宝
    ////    /// </summary>
    ////    int GetDiamoNum(int _buyNum, CopyGroupRef _data)
    ////    {
    ////        if (_data == null) return 0;
    ////        VIPRef refData = ConfigMng.Instance.GetVIPRef(GameCenter.mainPlayerMng.VipData.vLev);
    ////        CopyTimes times = null;
    ////        if (refData != null)
    ////        {
    ////            for (int i = 0; i < refData.copyPurchasetimes.Count; i++)
    ////            {
    ////                times = refData.copyPurchasetimes[i];
    ////                if (times.copyID == _data.id && times.copyTimes > 0)
    ////                {
    ////                    break;
    ////                }
    ////            }
    ////        }
    ////        //Debug.Log("该VIP一共有多少购买次数 ：" + times.copyTimes + " 还剩几次购买 : " + _buyNum + "  当前是第几次购买 : " + (times.copyTimes - _buyNum + 1));
    ////        //times.copyTimes(该VIP一共有多少次购买次数，配表读取)
    ////        //_buyNum(还剩多少次购买次数，服务端记录)
    ////        //setpId(本次是第几次购买)
    ////        int setpId = times.copyTimes - _buyNum + 1;
    ////        StepConsumptionRef stepConsumptionRef = ConfigMng.Instance.GetStepConsumptionRef(setpId);
    ////        return stepConsumptionRef != null ? stepConsumptionRef.copyNumber[0].count : 5;
    ////    }
    ////    public CopyGroupRef lcopyGroupRef = null;
    ////    /// <summary>
    ////    /// 弹出快捷购买提示
    ////    /// </summary>
    ////    public void PopTip(CopyInItemDataInfo _serData, CopyGroupRef _data, int _type, bool _isSweep)
    ////    {
    ////        if (_serData == null || _data == null) return;
    ////        if (_serData.buyNum > 0)//可购买次数大于0，弹出快捷购买次数
    ////        {
    ////            int needGold = GetDiamoNum(_serData.buyNum, _data);
    ////            MessageST msg = new MessageST();
    ////            msg.messID = 499;
    ////            msg.words = new string[1] { needGold.ToString() };
    ////            msg.delYes = delegate
    ////            {
    ////                //元宝是否充足
    ////                if (GameCenter.mainPlayerMng.MainPlayerInfo.DiamondCount >= (ulong)needGold)
    ////                {
    ////                    if (_isSweep)//是否是扫荡
    ////                    {
    ////                        GameCenter.endLessTrialsMng.C2S_BuyCopyInItem(_data.id, 1);
    ////                        GameCenter.endLessTrialsMng.C2S_SweepReward(1, _type);
    ////                    }
    ////                    else
    ////                    {
    ////                        GameCenter.endLessTrialsMng.C2S_BuyCopyInItem(_data.id, 1);
    ////                        GameCenter.endLessTrialsMng.C2S_ToCopyItem(_data.id, _type);
    ////                    }
    ////                }
    ////                else
    ////                {
    ////                    MessageST mst = new MessageST();
    ////                    mst.messID = 137;
    ////                    mst.delYes = delegate
    ////                    {
    ////                        GameCenter.uIMng.ReleaseGUI(GUIType.SWEEPCARBON);
    ////                        // 跳转到充值界面
    ////                        GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
    ////                    };
    ////                    GameCenter.messageMng.AddClientMsg(mst);
    ////                }
    ////            };
    ////            GameCenter.messageMng.AddClientMsg(msg);
    ////        }
    ////        else//可购买次数小于0跳转到充值界面
    ////        {
    ////            MessageST msg = new MessageST();
    ////            msg.messID = 500;
    ////            msg.delYes = delegate
    ////            {
    ////                GameCenter.uIMng.ReleaseGUI(GUIType.SWEEPCARBON);
    ////                GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
    ////            };
    ////            GameCenter.messageMng.AddClientMsg(msg);
    ////        }
    ////    }
    ////    #endregion
    ////    /// <summary>
    ////	/// 1=单人副本,2=多人副本(红点显示)
    ////	/// </summary>
    ////    public bool IsTipCopyTypeRedShow(int type)
    ////    {
    ////        foreach (CopyInItemDataInfo data in copyDic.Values)
    ////        {
    ////            CopyGroupRef refdata = ConfigMng.Instance.GetCopyGroupRef(data.id);
    ////            if (refdata != null && refdata.sort == type && data.num > 0 && refdata.lv <= GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel)
    ////            {
    ////                if (refdata.sort == 1)
    ////                {
    ////                    //int funcType = 0;
    ////                    FunctionType funcType = FunctionType.None;
    ////                    switch (refdata.id)
    ////                    {
    ////                        case 1: funcType = FunctionType.COPY; break;
    ////                        case 2: funcType = FunctionType.DOUSHUAIASGARD; break;
    ////                        case 3: funcType = FunctionType.FIVEMANOR; break;
    ////                        case 5: funcType = FunctionType.TRUEORFALSEMONKEY; break;
    ////                        case 6: funcType = FunctionType.WHITEPURGATORY; break;
    ////                    }
    ////                    if (GameCenter.mainPlayerMng.FunctionIsOpen(funcType))
    ////                        return true;
    ////                }
    ////                else
    ////                    return true;
    ////            }
    ////        }
    ////        return false;
    ////	}
    ////    /// <summary>
    ////    /// 副本进入按钮红点显示
    ////    /// </summary>
    ////	void SetAllCopyRedPoint(){
    ////		foreach(CopyInItemDataInfo data in copyDic.Values){
    ////            CopyGroupRef refdata = ConfigMng.Instance.GetCopyGroupRef(data.id);
    ////            if (refdata != null && data.num > 0 && refdata.lv <= GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel)
    ////            {
    ////                if (refdata.sort == 1)
    ////                {
    ////                    FunctionType funcType = FunctionType.None;
    ////                    switch (refdata.id)
    ////                    {
    ////                        case 1: funcType = FunctionType.COPY; break;
    ////                        case 2: funcType = FunctionType.DOUSHUAIASGARD; break;
    ////                        case 3: funcType = FunctionType.FIVEMANOR; break;
    ////                        case 5: funcType = FunctionType.TRUEORFALSEMONKEY; break;
    ////                        case 6: funcType = FunctionType.WHITEPURGATORY; break;
    ////                    }
    ////                    if (GameCenter.mainPlayerMng.FunctionIsOpen(funcType))
    ////                    {
    ////                        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.COPY, true);
    ////                        return;
    ////                    }
    ////                }
    ////                else
    ////                {
    ////                    GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.COPY, true);
    ////                    return;
    ////                }
    ////            }
    ////		}
    ////		GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.COPY,false);
    ////	}

    ////	/// <summary>
    ////	/// 好友数据
    ////	/// </summary>
    ////	public List<OtherPlayerInfo> GetOtherPlayerInfoS(){
    ////		List<OtherPlayerInfo> lists = new List<OtherPlayerInfo>();
    ////		OtherPlayerData playerEty = null;
    ////        //===========fix list禁止用foreach，应用for
    ////        for (int i = 0; i < GameCenter.friendsMng.friendList.Count; i++)
    ////        {
    ////			if(GameCenter.friendsMng.friendList[i].IsOnline){
    ////	            FriendsInfo info = GameCenter.friendsMng.friendList[i];
    ////	            playerEty = new OtherPlayerData();
    ////	            playerEty.serverInstanceID = info.configId;
    ////	            playerEty.name = info.name;
    ////	            playerEty.prof = info.prof;
    ////	            playerEty.baseValueDic[ActorBaseTag.FightValue] = (ulong)info.fight;
    ////				playerEty.baseValueDic[ActorBaseTag.Level] = (ulong)info.lev;
    ////				lists.Add(new OtherPlayerInfo(playerEty));
    ////			}
    ////        }
    ////            return lists;
    ////	}
    ////	/// <summary>
    ////	/// 显示选中的化身在多人准备界面
    ////	/// </summary>
    ////	public void AddInvitationToCopyTeams(List<int> uIds){
    ////		for(int j=0;j<uIds.Count;j++){
    ////			copyTeams[uIds[j]] = new CopySceneTeamPlayerInfo(uIds[j],1,true);
    ////		}
    ////		List<int> uIdall = new List<int>();
    ////		foreach(CopySceneTeamPlayerInfo data in copyTeams.Values){
    ////			if(data.isAvatar){
    ////				uIdall.Add(data.pId);
    ////			}
    ////		}
    ////		C2S_InCopyInvitationPlayer(uIdall,true);
    ////		if(OnSelectChange != null)OnSelectChange();
    ////	}
    ////	/// <summary>
    ////	/// 移除化身数据
    ////	/// </summary>
    ////	public void ReMoveInvitationToCopyTeams(int uId){
    ////		CopySceneTeamPlayerInfo pInfo = null;
    ////		if(copyTeams.TryGetValue(uId,out pInfo)){
    ////			if(pInfo.isAvatar)copyTeams.Remove(uId);
    ////		}
    ////		List<int> uIds = new List<int>();
    ////		uIds.Add(uId);
    ////		C2S_InCopyInvitationPlayer(uIds,false);
    ////		if(OnSelectChange != null)OnSelectChange();
    ////	}

    ////	void ClearInviTations(){
    ////		List<OtherPlayerInfo> clearIds = new List<OtherPlayerInfo>();
    ////		for (int i = 0; i < openInvitationPlayerData.Count; i++)
    ////		{
    ////			if(!copyTeams.ContainsKey(openInvitationPlayerData[i].ServerInstanceID)){
    ////				clearIds.Add(openInvitationPlayerData[i]);
    ////			}
    ////		}
    ////		for (int i = 0; i < clearIds.Count; i++)
    ////		{
    ////			openInvitationPlayerData.Remove(clearIds[i]);
    ////		}
    ////	}

    ////	int againCopyID=0,againSceneID = 0;
    ////	/// <summary>
    ////	/// 单人副本再次挑战
    ////	/// </summary>
    ////	public void AgainToCopyItem(){
    ////		if(copyDic.ContainsKey(againCopyID) && copyDic[againCopyID].num > 0){
    ////			C2S_ToCopyItem(againCopyID,againSceneID);
    ////		}else {
    ////			GameCenter.messageMng.AddClientMsg(245);
    ////		}
    ////	}
    ////	/// <summary>
    ////	/// 多人准备队伍解散
    ////	/// </summary>
    ////	public void TeamDissolve(){
    ////		CopyTeams.Clear();
    ////		if(OnCopyItemUIClose != null)OnCopyItemUIClose();
    ////	}
    ////	/// <summary>
    ////	/// 多人准备队员退出
    ////	/// </summary>
    ////	public void TeammateOut(int _teammateId){
    ////		if(CopyTeams.ContainsKey(_teammateId)){
    ////			CopyTeams.Remove(_teammateId);
    ////			if(OnSelectChange != null)OnSelectChange();
    ////		}
    ////	}

    ////	#region  协议接受
    ////	//打开镇魔塔准备界面
    ////	void S2C_MagicTowers(Pt _info){
    ////		pt_quell_demon_win_d716 info = _info as pt_quell_demon_win_d716;
    ////		if (info != null)
    ////		{
    ////			if (OnMagicTowerUIOpen != null) OnMagicTowerUIOpen(info.integral, info.time);
    ////		}
    ////	}
    ////	/// <summary>
    ////	/// 副本数据
    ////	/// </summary>
    ////	void S2C_CopyItemList(Pt _info){
    ////		        //Debug.logger.Log("S2C_CopyItemList");
    ////		pt_single_many_copy_info_d453 info = _info as pt_single_many_copy_info_d453;
    ////		if(info == null)return ;
    ////		for (int i = 0; i < info.single_many_list.Count; i++)
    ////		{
    ////			st.net.NetBase.single_many_list data = info.single_many_list[i];
    ////            //Debug.Log("副本ID  " + data.copy_id +"副本剩余挑战次数  " + data.challenge_num + "可购买次数  " + data.buy_num);
    ////			if (copyDic.ContainsKey((int)data.copy_id))
    ////			{
    ////				copyDic[(int)data.copy_id].UpdateData(data);
    ////			}
    ////			else
    ////			{
    ////				//                Debug.logger.Log("add dungeon " + data.copy_id);
    ////				copyDic[(int)data.copy_id] = new CopyInItemDataInfo(data);
    ////			}
    ////		}
    ////		FDictionary copyGroupRefTable = ConfigMng.Instance.CopyGroupRefTable();
    ////		foreach(CopyGroupRef refData in copyGroupRefTable.Values){
    ////			if (!copyDic.ContainsKey(refData.id))
    ////			{
    ////				copyDic[refData.id] = new CopyInItemDataInfo(refData.id);
    ////			}
    ////		}
    ////		SetAllCopyRedPoint();
    ////		if(OnCopyTypeChange != null)OnCopyTypeChange();
    ////	}
    ////	/// <summary>
    ////	/// 副本购买次数
    ////	/// </summary>
    ////	void S2C_BuyCopyInNum(Pt _info){
    ////		pt_update_single_num_d455 info = _info as pt_update_single_num_d455;
    ////		if(info == null)return ;
    ////		if(copyDic.ContainsKey(info.copy_id)){
    ////			copyDic[info.copy_id].UpdateData(info.copy_id,info.challenge_num,info.buy_num);
    ////		}else{
    ////			copyDic[info.copy_id] = new CopyInItemDataInfo();
    ////			copyDic[info.copy_id].UpdateData(info.copy_id,info.challenge_num,info.buy_num);
    ////		}
    ////		SetAllCopyRedPoint();
    ////		if(OnCopyItemChange != null)OnCopyItemChange();
    ////	}
    ////	/// <summary>
    ////	/// 副本星级
    ////	/// </summary>
    ////	void S2C_CopyInSceneStar(Pt _info){
    ////		pt_uptate_single_many_star_d457 info = _info as pt_uptate_single_many_star_d457;
    ////		if(info == null)return ;
    ////		if(copyDic.ContainsKey(info.copy_id)){
    ////			if(copyDic[info.copy_id].copyScenes.ContainsKey(info.scene_type)){
    ////				copyDic[info.copy_id].copyScenes[info.scene_type].UpdateData(info.star_num);
    ////			}else{
    ////				copyDic[info.copy_id].copyScenes[info.scene_type] = new CopyInItemDataInfo.CopySceneData(info.copy_id,info.star_num);
    ////			}
    ////		}
    ////		if(OnCopyItemChange != null)OnCopyItemChange();
    ////	}

    ////	/// <summary>
    ////	/// 通知队员化身进入多人准备
    ////	/// </summary>
    ////	void S2C_TeamAddInvitationPlayer(Pt _info){
    ////		pt_update_recruit_robot_to_member_d771 info = _info as pt_update_recruit_robot_to_member_d771;
    ////        //Debug.Log("pt_update_recruit_robot_to_member_d771   " + info.recruit_uid_list.Count);
    ////		if(info == null)return ;
    ////		int len = info.recruit_uid_list.Count;
    ////		if(len <= 0){
    ////			List<int> uIdall = new List<int>();
    ////			foreach(CopySceneTeamPlayerInfo data in copyTeams.Values){
    ////				if(data.isAvatar){
    ////					uIdall.Add(data.pId);
    ////				}
    ////			}
    ////			for(int j=0;j<uIdall.Count;j++){
    ////				copyTeams.Remove(uIdall[j]);
    ////			}
    ////		}
    ////		for(int j=0;j<len;j++){
    ////			copyTeams[info.recruit_uid_list[j]] = new CopySceneTeamPlayerInfo(info.recruit_uid_list[j],1,true);
    ////		}
    ////		if(OnSelectChange != null)OnSelectChange();
    ////	}

    ////	//待招募化身数据列表返回
    ////	void S2C_InvitationPlayer(Pt _info){

    ////		pt_update_recruit_robot_list_d746 info = _info as pt_update_recruit_robot_list_d746;
    ////		if(info == null)return ;
    ////		ClearInviTations();
    ////		OtherPlayerData other = null;
    ////        for (int i = 0; i < info.recruit_robot_list.Count; i++)
    ////        {
    ////            st.net.NetBase.recruit_robot_list data = info.recruit_robot_list[i];
    ////            other = new OtherPlayerData();
    ////            other.serverInstanceID = data.uid;
    ////            other.name = data.name;
    ////            other.prof = data.prof;
    ////            other.baseValueDic[ActorBaseTag.Level] = (ulong)data.lev;
    ////            other.baseValueDic[ActorBaseTag.FightValue] = (ulong)data.fight_score;
    ////			OtherPlayerInfo otherInfo = new OtherPlayerInfo(other);
    ////			if(!openInvitationPlayerData.Contains(otherInfo))openInvitationPlayerData.Add(otherInfo);
    ////        }
    ////		if (OpenInvitationPlayer != null && GameCenter.teamMng.isInTeam && GameCenter.teamMng.isLeader) OpenInvitationPlayer();
    ////	}

    ////	/// <summary>
    ////	/// 队员进入多人准备
    ////	/// </summary>
    ////	void S2C_CopyItemTeams(Pt _info){
    //////		copyTeams.Clear();
    ////		pt_many_copy__member_challengenum_d460 info = _info as pt_many_copy__member_challengenum_d460;
    ////        Debug.Log("pt_many_copy__member_challengenum_d460  " + info.member_challengenum.Count);
    ////		if(info == null)return ;
    ////		if(copyDic.ContainsKey(info.copy_id)){
    ////			copyDic[info.copy_id].curCopyScene = info.copy_type;
    ////		}else{
    ////			copyDic[info.copy_id] = new CopyInItemDataInfo();
    ////			copyDic[info.copy_id].id = info.copy_id;
    ////			copyDic[info.copy_id].curCopyScene = info.copy_type;
    ////		}
    ////        for (int i = 0; i < info.member_challengenum.Count; i++)
    ////        {
    ////            st.net.NetBase.member_challengenum_list data = info.member_challengenum[i];
    ////			if(copyTeams.Count >= 3)continue;
    ////			if(!copyTeams.ContainsKey((int)data.uid)){
    ////				copyTeams[(int)data.uid] = new CopySceneTeamPlayerInfo(data);
    ////			}else{
    ////				copyTeams[(int)data.uid].pNum = (int)data.challenge_num;
    ////				copyTeams[(int)data.uid].isPerpare = data.prepare==1;
    ////			}
    ////        }
    ////		SetAllCopyRedPoint();
    ////        if (OnCopyItemTeamData != null) OnCopyItemTeamData(info.copy_id);
    ////		if(OnCopyItemChange != null)OnCopyItemChange();
    ////	}
    ////	/// <summary>
    ////	/// 队员多人准备关闭
    ////	/// </summary>
    ////	void S2C_ReqCopyInTeamClose(Pt _info){
    ////		//pt_update_quit_many_copy_ui_d465 info = _info as pt_update_quit_many_copy_ui_d465;
    ////		GameCenter.teamMng.InvitationTeammateOut();
    ////		copyTeams.Clear();
    ////		if(OnCopyItemUIClose != null)OnCopyItemUIClose();
    ////	}
    ////	/// <summary>
    ////	/// 副本难度变更
    ////	/// </summary>
    ////	void S2C_ReqCopyInChange(Pt _info){
    ////		pt_update_many_copy_difficulty_d463 info = _info as pt_update_many_copy_difficulty_d463;
    ////		if(copyDic.ContainsKey((int)info.copy_id)){
    ////			copyDic[(int)info.copy_id].curCopyScene = (int)info.copy_type;
    ////		}
    ////		if(OnCopyItemChange != null)OnCopyItemChange();
    ////	}
    ////	/// <summary>
    ////	/// 队员是否准备
    ////	/// </summary>
    ////	void S2C_ReqCopyInPerpare(Pt _info){
    ////		pt_update_prepare_state_d466 info = _info as pt_update_prepare_state_d466;
    ////		for(int i =0;i< info.update_prepare.Count;i++){
    ////			if(copyTeams.ContainsKey((int)info.update_prepare[i].uid))copyTeams[(int)info.update_prepare[i].uid].UpdateData(info.update_prepare[i].uid,info.update_prepare[i].prepare);
    ////		}
    ////		if(OnCopyItemChange != null)OnCopyItemChange();
    ////	}
    ////	// 邀请好友询问队列
    ////	static List<int> friendmsg = new List<int>();

    ////	/// <summary>
    ////	/// 邀请好友询问
    ////	/// </summary>
    ////	void S2C_CopyInFriend(Pt _info){
    ////		pt_ans_recruit_friend_d749 info = _info as pt_ans_recruit_friend_d749;
    ////		if(info == null)return;
    ////		if(GameCenter.teamMng.isInTeam)return ;
    ////		if(friendmsg.Contains(info.uid)){
    ////			return ;
    ////		}
    ////		friendmsg.Add(info.uid);
    ////		MessageST msg = new MessageST();
    ////		msg.messID = 251;
    ////		msg.words = new string[3]{info.name,ConfigMng.Instance.GetLevelDes(info.lev),info.fight_score.ToString()};
    ////		msg.delYes = delegate {
    ////			C2S_CopyInFriendReturn(info.uid,1);
    ////		};
    ////		msg.delNo = delegate {
    ////			C2S_CopyInFriendReturn(info.uid,0);
    ////		};
    ////		GameCenter.messageMng.AddClientMsg(msg);
    ////	}
    ////	#endregion

    ////	#region 协议发送

    ////	/// <summary>
    ////	/// 多人准备好友邀请回应
    ////	/// </summary>
    ////	public void C2S_CopyInFriendReturn(int _pid,int _answer){
    ////		if(GameCenter.teamMng.isInTeam){
    ////			GameCenter.messageMng.AddClientMsg(404);
    ////			return ;
    ////		}
    ////		pt_ask_recruit_friend_reply_d750  info = new pt_ask_recruit_friend_reply_d750();
    ////		info.ans_uid = _pid;
    ////		info.ans_state = _answer;
    ////		NetMsgMng.SendMsg(info);
    ////		if(friendmsg.Contains(_pid))friendmsg.Remove(_pid);
    ////	}

    ////	/// <summary>
    ////	/// 多人准备好友邀请
    ////	/// </summary>
    ////	public void C2S_ReqCopyInFriend(int uid){
    ////		if(copyTeams.Count >= 3)return ;
    ////		pt_req_recruit_friend_d747  info = new pt_req_recruit_friend_d747();
    ////		info.oth_uid = uid;
    ////		NetMsgMng.SendMsg(info);
    ////	}

    ////	/// <summary>
    ////	/// 多人准备
    ////	/// </summary>
    ////	public void C2S_ReqCopyInPerpare(uint perpare){
    ////		pt_member_click_prepare_d461  info = new pt_member_click_prepare_d461();
    ////		info.state = perpare;
    ////		NetMsgMng.SendMsg(info);
    ////	}
    ////	/// <summary>
    ////	/// 多人副本组里副本改变
    ////	/// </summary>
    ////	public void C2S_ReqCopyInChange(int id,int type){
    ////		pt_change_many_copy_difficulty_d462  info = new pt_change_many_copy_difficulty_d462();
    ////		info.copy_id = (uint)id;
    ////		info.copy_type = (uint)type;
    ////		NetMsgMng.SendMsg(info);
    ////	}
    ////	/// <summary>
    ////	/// 多人准备界面关闭(0为队员点击关闭 ，1为队长点击关闭)
    ////	/// </summary>
    ////	public void C2S_ReqCopyInTeamClose(int _state){
    ////		pt_req_quit_many_copy_ui_d464  info = new pt_req_quit_many_copy_ui_d464();
    ////        info.state = _state;
    ////		NetMsgMng.SendMsg(info);
    ////	}
    ////	/// <summary>
    ////	/// 多人进副本或者镇魔塔
    ////	/// </summary>
    ////	public void C2S_ReqCopyInTeamData(int id,int type){
    ////		copyTeams.Clear();
    ////		pt_req_team_many_copy_d459  info = new pt_req_team_many_copy_d459();
    ////		info.copy_id = id;
    ////		info.copy_type = type;
    ////		NetMsgMng.SendMsg(info);
    ////	}
    ////	/// <summary>
    ////	/// 副本数据
    ////	/// </summary>
    ////	public void C2S_ReqCopyItemList(){
    ////        //Debug.logger.Log("C2S_ReqCopyItemList");
    ////		pt_req_single_many_info_d452  info = new pt_req_single_many_info_d452();
    ////		NetMsgMng.SendMsg(info);
    ////	}

    ////	/// <summary>
    ////	/// 进副本
    ////	/// </summary>
    ////	public void C2S_ToCopyItem(int id,int type){
    ////		if(CopyType == CopysType.ONESCOPY){
    ////			againCopyID = id;
    ////			againSceneID = type;
    ////		}
    ////		pt_req_fly_single_many_copy_d456 info = new pt_req_fly_single_many_copy_d456();
    ////		info.seq = NetMsgMng.CreateNewSerializeID();
    ////		info.copy_id = id;
    ////		info.scene_type = type;
    ////		NetMsgMng.SendMsg(info);
    ////	}
    ////	/// <summary>
    ////	/// 购买副本次数
    ////	/// </summary>
    ////	public void C2S_BuyCopyInItem(int id,int num){
    ////		pt_req_buy_single_num_d454 info = new pt_req_buy_single_num_d454();
    ////		info.copy_id = id;
    ////		info.buy_num = num;
    ////		NetMsgMng.SendMsg(info);
    ////	}
    ////	/// <summary>
    ////	/// 招募化身
    ////	/// </summary>
    ////	public void C2S_InvitationPlayer(){
    ////		pt_recruit_robot_info_d744 info = new pt_recruit_robot_info_d744();
    ////		NetMsgMng.SendMsg(info);
    ////	}

    ////	/// <summary>
    ////	/// 选中的化身
    ////	/// </summary>
    ////	public void C2S_InCopyInvitationPlayer(List<int> uIds,bool isAdd){
    ////		pt_recruit_robot_member_d745 info = new pt_recruit_robot_member_d745();
    ////		info.add_or_remove = isAdd ? 0 : 1;
    ////		info.recruit_robot_member = uIds;
    ////		NetMsgMng.SendMsg(info);
    ////	}
    ////    /// <summary>
    ////    /// 镇魔塔发送世界招募信息
    ////    /// </summary>
    ////    public void C2S_ReqWoldRecruit()
    ////    {
    ////        pt_leader_req_world_recruit_d790 info = new pt_leader_req_world_recruit_d790();
    ////        NetMsgMng.SendMsg(info);
    ////    }
    ////	#endregion


    #endregion

    //	#region 副本结算
    //    /// <summary>
    //    /// 副本失败
    //    /// </summary>
    //	public bool openCopyForceTip = false;
    //	/// <summary>
    //	/// 副本失败事件
    //	/// </summary>
    //	public System.Action OnOpenCopyForce;
    //	/// <summary>
    //	/// 打开结算
    //	/// </summary>
    //	public System.Action OnOpenCopySettlement;
    //	/// <summary>
    //	/// 打开结算
    //	/// </summary>
    //	public System.Action OnOpenCopySettlementFlop;
    //	/// <summary>
    //	/// 打开结算
    //	/// </summary>
    //	public System.Action OnOpenArenaSettlement;

    //	/// <summary>
    //	/// 结算前等待捡东西
    //	/// </summary>
    //	public System.Action OnOpenCoppyTime;
    //	/// <summary>
    //	/// 结算数据
    //	/// </summary>
    //	public CopySettlementDataInfo CopySettlementDataInfo = new CopySettlementDataInfo();
    //	/// <summary>
    //	/// 时间格式
    //	/// </summary>
    //	public string ItemTime(int time,bool isNow = false){
    //		if(isNow){
    //			DateTime date = GameHelper.ToChinaTime(new DateTime(1970,1,1)).AddSeconds((double)time);
    //			//TimeSpan sp = DateTime.Now - date;
    //            TimeSpan sp = GameCenter.instance.CurServerTime - date;
    //			if(sp.Days > 7){
    //				return ConfigMng.Instance.GetUItext(41);
    //			}else if(sp.Days >= 1 && sp.Days <= 7){
    //				return ConfigMng.Instance.GetUItext(40,new string[1]{sp.Days.ToString()});
    //			}else if(sp.Hours >=1 && sp.Days < 1){
    //				return ConfigMng.Instance.GetUItext(39,new string[1]{sp.Hours.ToString()});
    //			}else if(sp.Minutes >= 1){
    //				return ConfigMng.Instance.GetUItext(38,new string[1]{sp.Minutes.ToString()});
    //			}else if(sp.Seconds < 60){
    //				return ConfigMng.Instance.GetUItext(37);
    //			}
    //			return string.Empty;
    //		}

    //		int tmp = time;
    //		int s = tmp%60;
    //		tmp /= 60;
    //		int m = tmp%60;
    //		int h = tmp/60;
    //		if(h>0)
    //		{
    //			return string.Format("{0:D2}:{1:D2}:{2:D2}",h,m,s);
    //		}
    //		else
    //		{
    //			return string.Format("{0:D2}:{1:D2}",m,s);
    //		}
    //		//return "0:0";
    //	}

    //	#region 协议
    //	void S2C_ReqArenaRewardData(Pt _info){
    //		pt_pk_win_d491 info = _info as pt_pk_win_d491;
    //		CopySettlementDataInfo = new CopySettlementDataInfo();
    //		CopySettlementDataInfo.state = info.state;
    //		CopySettlementDataInfo.rank = info.rank;
    //		CopySettlementDataInfo.upRank = info.up_rank;
    //		CopySettlementDataInfo.showKo = info.cd_state <= 0;
    //        for (int i = 0; i < info.reward.Count; i++)
    //        {
    //            st.net.NetBase.reward_list data = info.reward[i];
    //            CopySettlementDataInfo.items.Add(new EquipmentInfo((int)data.type, (int)data.num, EquipmentBelongTo.PREVIEW));
    //        }
    //        if (OnOpenArenaSettlement != null) OnOpenArenaSettlement();
    //	}

    //	void S2C_ReqOpenCoppyTime(Pt _info){
    //		//pt_copy_pick_item_time_d753 info = _info as pt_copy_pick_item_time_d753;

    //		if(OnOpenCoppyTime != null)OnOpenCoppyTime();
    //	}

    //	void S2C_ReqOpenCopyForce(Pt _info){
    ////		pt_copy_loser_d470 info = new pt_copy_loser_d470();
    //		openCopyForceTip = true;
    //		if(OnOpenCopyForce != null)OnOpenCopyForce();
    //	}

    //	void S2C_ReqSettlementRewardData(Pt _info){
    //		pt_win_list_d467 info = _info as pt_win_list_d467;
    //		CopySettlementDataInfo = new CopySettlementDataInfo();
    //		CopySettlementDataInfo.star = info.star_num;
    //		CopySettlementDataInfo.time = info.time;
    //        for (int i = 0; i < info.reward_list.Count; i++)
    //        {
    //            st.net.NetBase.reward_list data = info.reward_list[i];
    //            CopySettlementDataInfo.items.Add(new EquipmentInfo((int)data.type, (int)data.num, EquipmentBelongTo.PREVIEW));
    //        }
    //        if (OnOpenCopySettlement != null) OnOpenCopySettlement();
    //	}

    //	void S2C_ReqSettlementFlopData(Pt _info){
    //		pt_lucky_brand_list_d469 info = _info as pt_lucky_brand_list_d469;
    //		CopySettlementDataInfo = new CopySettlementDataInfo();
    //		CopySettlementDataInfo.clickFlop = info.bradn_id;

    //		CopySettlementDataInfo.flopItems[(int)info.bradn_id] = new EquipmentInfo((int)info.brand_reward[0].type,(int)info.brand_reward[0].num,EquipmentBelongTo.PREVIEW);
    //		int id = 1;
    //        for (int i = 0; i < info.lucky_brand.Count; i++)
    //        {
    //            st.net.NetBase.lucky_brand_list data = info.lucky_brand[i];
    //            if (id == info.bradn_id)
    //            {
    //                id++;
    //            }
    //            CopySettlementDataInfo.flopItems[id] = new EquipmentInfo((int)data.type, (int)data.num, EquipmentBelongTo.PREVIEW);
    //            id++;
    //        }
    //        if (OnOpenCopySettlementFlop != null) OnOpenCopySettlementFlop();
    //	}
    //	/// <summary>
    //	/// 结算开牌
    //	/// </summary>
    //	public void C2S_ReqSettlementFlop(int brandId){
    //		pt_req_open_lucky_brand_d468  info = new pt_req_open_lucky_brand_d468();
    //		info.brand_id = brandId;
    //		NetMsgMng.SendMsg(info);
    //	}
    //	/// <summary>
    //	/// 通知服务端东西捡完了
    //	/// </summary>
    //	public void C2S_CoppyOver(){
    //		pt_req_item_pick_over_d754 info = new pt_req_item_pick_over_d754();
    //		NetMsgMng.SendMsg(info);
    //	}
    //	/// <summary>
    //	/// 退出
    //	/// </summary>
    //	public void C2S_OutCopy(){
    //		GameCenter.teamMng.InvitationTeammateOut();
    //		pt_req_copy_out_d471 info = new pt_req_copy_out_d471();
    //		NetMsgMng.SendMsg(info);
    //	}

    //	public void OutCopyWnd()
    //	{
    //		MessageST mst = new MessageST();
    //		mst.messID = 44;
    //		mst.delYes = (x)=>
    //		{
    //			C2S_OutCopy();
    //		};
    //		GameCenter.messageMng.AddClientMsg(mst);
    //	}
    //	#endregion


    //#endregion

    //#region 竞技场
    //   /// <summary>
    //   /// 竞技场数据
    //   /// </summary>
    //public ArenaServerDataInfo ArenaServerDataInfo;
    ///// <summary>
    ///// 竞技场数据事件
    ///// </summary>
    //public System.Action OnArenaServerDataInfo;
    ///// <summary>
    ///// 竞技场奖励领取数据
    ///// </summary>
    //public System.Action OnArenaServerReward;

    //   /// <summary>
    //   /// 是否显示竞技场次数用光提示
    //   /// </summary>
    //   public bool ShowAreaTimeTip = true;

    //void S2C_ArenaServerDataInfo(Pt _info){
    //	pt_pk_info_d485 info = _info as pt_pk_info_d485; 
    //       if (info != null)
    //       {
    //           //Debug.Log("S2C_ArenaServerDataInfo d485  " + info.rank + "  , surplus_time : " + info.surplus_time + "   ,  num : " + info.challenge_num);
    //           ArenaServerDataInfo = new ArenaServerDataInfo(info);
    //       }
    //       GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.ARENA, ArenaServerDataInfo != null && ArenaServerDataInfo.challenge_num > 0 && ArenaServerDataInfo.surplus_time <= 0);
    //       if (ArenaServerDataInfo.surplus_time > 0)
    //       {
    //           GameCenter.instance.arenTime = Time.time;
    //           GameCenter.instance.IsSetArenRed = true;
    //           GameCenter.instance.arenSurPlusTime = ArenaServerDataInfo.surplus_time;
    //       }
    //       if (OnArenaServerDataInfo != null)
    //       {
    //           OnArenaServerDataInfo();
    //       }
    //}
    //   public bool ArenRed()
    //   {
    //       return ArenaServerDataInfo != null ? ArenaServerDataInfo.challenge_num > 0 : false;
    //   }
    //void S2C_ArenaServerReward(Pt _info){
    //	pt_update_rank_reward_d489 info = _info as pt_update_rank_reward_d489;
    //	ArenaServerDataInfo.state = info.state;
    //	if(OnArenaServerReward != null){
    //		OnArenaServerReward();
    //	}
    //}
    ///// <summary>
    ///// 请求竞技场数据
    ///// </summary>
    //public void C2S_RepArenaServer(){ 
    //	pt_req_usr_pk_info_d483 info = new pt_req_usr_pk_info_d483();
    //	NetMsgMng.SendMsg(info);
    //}
    ///// <summary>
    ///// 请求竞技场奖励数据
    ///// </summary>
    //public void C2S_RepArenaReward(){
    //	pt_req_get_rank_reward_d487 info = new pt_req_get_rank_reward_d487();
    //	NetMsgMng.SendMsg(info);
    //}
    ///// <summary>
    ///// 请求PK对象
    ///// </summary>
    //public void C2S_RepArenaKill(int uid){
    //	pt_req_pk_challenge_d490 info = new pt_req_pk_challenge_d490();
    //	info.challenge_uid = uid;
    //	NetMsgMng.SendMsg(info);
    //}
    //   /// <summary>
    //   /// 请求购买竞技场次数
    //   /// </summary>
    //   public void C2S_ReqBuyChallengeTimes()
    //   {
    //       pt_req_buy_challenge_times_d798 info = new pt_req_buy_challenge_times_d798();
    //       NetMsgMng.SendMsg(info);
    //   }
    //#endregion

    //#region 暂停功能
    //const int OneCopyGameStopTotalNum = 3;//单人副本暂停总次数
    ///// <summary>
    ///// 单人副本暂停次数
    ///// </summary>
    //public int gameStopNum = 0;
    ///// <summary>
    ///// 暂停事件
    ///// </summary>
    //public System.Action OnGameStop;
    //public bool IsGameStop = false;
    //void S2C_GameStop(Pt _info){
    //	pt_copy_pause_d47a info = _info as pt_copy_pause_d47a;
    //	IsGameStop = info.state == 1;
    //	Time.timeScale = info.state == 1 ? 0 : 1;
    //	if(CopyType == CopysType.ONESCOPY && info.state == 0)gameStopNum++;
    //	if(OnGameStop != null)OnGameStop();
    //}
    ///// <summary>
    ///// 请求暂停
    ///// </summary>
    //public  void C2S_GameStop(byte stege){
    //	if(gameStopNum >= OneCopyGameStopTotalNum && CopyType == CopysType.ONESCOPY){
    //		GameCenter.messageMng.AddClientMsg(317);
    //		return ;
    //	}
    //	pt_copy_pause_d47a info = new pt_copy_pause_d47a();
    //	info.state = stege;
    //	NetMsgMng.SendMsg(info);
    //}
    //#endregion

    #region 活动大厅功能
    ///// <summary>
    ///// 提前5分钟提示
    ///// </summary>
    //public const int ShowTime = 300;
    //public Dictionary<int,ActivityDataInfo> activityDic = new Dictionary<int, ActivityDataInfo>();
    //public ActivityDataInfo GetActivityDataInfo(int id){
    //	if(activityDic.ContainsKey(id)){
    //		return activityDic[id];
    //	}
    //	return null;
    //}
    ///// <summary>
    ///// return {1=未开，2=开启，3=已结束}
    ///// </summary>
    //public ActivityState GetActivityState(ActivityType type){
    //	if(activityDic.ContainsKey((int)type)){
    //		return activityDic[(int)type].State;
    //	}
    //	return ActivityState.NOTATTHE;
    //}
    ///// <summary>
    ///// 活动还有多久结束
    ///// </summary>
    //public int GetActivityTime(ActivityType type){
    //	if(activityDic.ContainsKey((int)type)){
    //		if(activityDic[(int)type].State == ActivityState.ONGOING){
    //			int atime = activityDic[(int)type].UpDateTime;
    //			return atime > 0 ? atime : 0;
    //		}
    //	}
    //	return 0;
    //}

    ///// <summary>
    ///// 活动还有多久开始,活动已结束时返回0
    ///// </summary>
    //public int GetActivityStartTime(int id){
    //	if(activityDic.ContainsKey(id)){
    //		if(activityDic[id].State == ActivityState.NOTATTHE){
    //			return activityDic[id].UpDateTime;
    //		}
    //	}
    //	return 0;
    //}
    //   /// <summary>
    //   /// 是否是直接跳转过去(从别的界面跳转的，直接选中某个活动的将该活动排在第一个)
    //   /// </summary>
    //   //public bool isGoToSelect = false;
    //public ActivityType CurSeleteType = ActivityType.NONE;
    ///// <summary>
    ///// 打开时选择某个活动
    ///// </summary>
    //public void OpenStartSeleteActivity(ActivityType type){
    //       //isGoToSelect = true;
    //	CurSeleteType = type;
    //	GameCenter.uIMng.SwitchToUI(GUIType.ATIVITY);
    //       //ChooseActivity(type);
    //}

    //enum ButtonType{
    //	FengShen =1,
    //	ToPoint=2,
    //	GUI=3,
    //	subGUI,
    //	fly,
    //	ToNpc,
    //}
    ///// <summary>
    ///// 按钮功能
    ///// </summary>
    //public void GoActivityButtonFunc(ActivityButtonRef refdata,int id){
    //	if(refdata == null){return ;}
    //	if(!activityDic.ContainsKey(id)){return ;}
    //	if(!activityDic[id].ActivityLev){
    //		GameCenter.messageMng.AddClientMsg(13);
    //		return ;
    //	}

    //	ActivityType type = (ActivityType)id;
    //       //=============fix 完全没必要每次判断都进行一次强制转换，可以先声明一个变量，强制转换一次，然后拿该变量去判断
    //	if(type == ActivityType.FAIRYAUBONFIRE || type == ActivityType.FAIRYAUSHIPMENTDART 
    //		|| type == ActivityType.FAIRYAUSIEGE || type == ActivityType.FAIRYDOMAINTOPROTECT){
    //		if(!GameCenter.mainPlayerMng.MainPlayerInfo.HavaGuild){
    //			GameCenter.messageMng.AddClientMsg(235);
    //			return ;
    //		}
    //	}

    //	if(type == ActivityType.DAILYTRANSPORTDART){
    //           GameCenter.uIMng.SwitchToUI(GUIType.NONE);
    //		GameCenter.activityMng.C2S_ReqDartPos(DartType.DailyDart);
    //		return ;
    //	}

    //	if(type == ActivityType.FAIRYAUSHIPMENTDART){
    //		GameCenter.activityMng.C2S_ReqDartPos(DartType.GuildDart);
    //		return ;
    //	}
    //	if(type == ActivityType.FAIRYAUBONFIRE)
    //	{
    //		GameCenter.activityMng.C2S_FlyMyGuildFire();
    //		return;
    //	}
    //	if(type == ActivityType.UNDERBOSS)
    //	{
    //		BossChallengeWnd.OpenAndGoWndByType(BossChallengeWnd.ToggleType.UnderBoss);
    //		return;
    //	}
    //       if (activityDic[id].State != ActivityState.ONGOING)
    //       {
    //           GameCenter.messageMng.AddClientMsg(173);
    //           return;
    //       }
    //       if (type == ActivityType.RAIDERARK)
    //       {
    //           //Debug.Log("跳转夺宝奇兵界面！！！");
    //           GameCenter.activityMng.C2S_FlyRaiderArk();
    //           return;
    //       }

    //       //=============fix 没有使用枚举，而是使用魔数========
    //	if(refdata.type == (int)ButtonType.GUI){
    //		GameCenter.uIMng.SwitchToUI((GUIType)Enum.Parse(typeof(GUIType),refdata.pageId));
    //	}else if(refdata.type == (int)ButtonType.FengShen){
    //		GameCenter.activityMng.C2S_FlyFengShen(id);
    //	}else if(refdata.type == (int)ButtonType.ToPoint){//寻路点
    //		GameCenter.taskMng.TraceToScene(refdata.mapId,new Vector3(refdata.mapXY[0],0,refdata.mapXY[1]));
    //		GameCenter.curMainPlayer.GoTraceSearchTreasure();
    //	}else if(refdata.type == (int)ButtonType.subGUI){
    //		GameCenter.uIMng.SwitchToSubUI((SubGUIType)Enum.Parse(typeof(SubGUIType),refdata.pageId));
    //	}else if(refdata.type == (int)ButtonType.fly){//飞副本，
    //		GameCenter.mainPlayerMng.C2S_Fly_Pint(refdata.mapId,refdata.mapXY[0],refdata.mapXY[1]);
    //	}else if(refdata.type == (int)ButtonType.ToNpc){//寻路NPC
    //		GameCenter.taskMng.PublicTraceToNpc(refdata.mapId);
    //		GameCenter.curMainPlayer.GoTraceSearchTreasure();
    //	}
    //}
    ///// <summary>
    ///// 每天凌晨更新活动数据
    ///// </summary>
    //public void InTheMorningUpdateData(){
    //	foreach(ActivityDataInfo data in activityDic.Values){
    //		data.StateUpdateTime();
    //	}
    //	if(OnActivityDataInfo != null)OnActivityDataInfo();
    //}
    ///// <summary>
    ///// 活动数据变化事件
    ///// </summary>
    //public System.Action OnActivityDataInfo;
    //void S2C_ActivityDataInfo(Pt _info){
    //	pt_update_activity_info_d715 info = _info as pt_update_activity_info_d715;
    //	activity_list data = null;
    //	for(int i =0;i<info.activity_list.Count;i++){
    //		data = info.activity_list[i];
    //		if(activityDic.ContainsKey(data.id)){
    //			activityDic[data.id].Update(data);
    //		}else{
    //			activityDic[data.id] = new ActivityDataInfo(data);
    //		}
    //	}
    //	FDictionary datalist = ConfigMng.Instance.GetActivityList();
    //	foreach(ActivityListRef refdata in datalist.Values){
    //		if(!activityDic.ContainsKey(refdata.id)){
    //			activityDic[refdata.id] = new ActivityDataInfo(refdata.id);
    //		}
    //	}
    //	if(OnActivityDataInfo != null)OnActivityDataInfo();
    //}
    //void S2C_FairyDomainToprotect(Pt _info){
    //       //Debug.Log("接收协议pt_activity_guild_guard_time_d756");
    //	pt_activity_guild_guard_time_d756 info = _info as pt_activity_guild_guard_time_d756;
    //	if(activityDic.ContainsKey(info.act_id)){
    //		activityDic[info.act_id].Update(info.start_state,info.surplus_time);
    //	}
    //       if (OnActivityDataInfo != null) OnActivityDataInfo();
    //}
    //   #region 将从外界跳转过去的选中的活动设为第一个，解决活动界面左面看不到该活动的BUG(暂时还没确定是否这样做)
    //   //private List<ActivityDataInfo> newActList = new List<ActivityDataInfo>();
    //   ///// <summary>
    //   ///// 跳转到的活动选为第一个
    //   ///// </summary>
    //   //public void ChooseActivity(ActivityType _type)
    //   //{
    //   //    newActList.Clear();
    //   //    refActList.Clear();
    //   //    dic.Clear();
    //   //    foreach (ActivityListRef refdata in ConfigMng.Instance.GetActivityList().Values)
    //   //    {
    //   //        ActivityDataInfo data = GameCenter.endLessTrialsMng.GetActivityDataInfo(refdata.id);
    //   //        if (refdata.id != (int)_type)
    //   //            refActList.Add(data);
    //   //        else
    //   //            newActList.Add(data);
    //   //    }
    //   //    refActList.Sort(GameCenter.endLessTrialsMng.SortActivity);
    //   //    for (int i = 0; i < refActList.Count; i++)
    //   //    {
    //   //        newActList.Add(refActList[i]);
    //   //    }
    //   //    for (int i = 0; i < newActList.Count; i++)
    //   //    {
    //   //        dic[(int)newActList[i].ID] = ConfigMng.Instance.GetActivityListRef((int)newActList[i].ID);
    //   //    }
    //   //}

    //   //public Dictionary<int, ActivityListRef> dic = new Dictionary<int, ActivityListRef>();
    //   //private List<ActivityDataInfo> refActList = new List<ActivityDataInfo>();
    //   ///// <summary>
    //   ///// 排序
    //   ///// </summary>
    //   //public void SortActivity()
    //   //{
    //   //    refActList.Clear();
    //   //    dic.Clear();
    //   //    foreach (ActivityListRef refdata in ConfigMng.Instance.GetActivityList().Values)
    //   //    {
    //   //        ActivityDataInfo data = GameCenter.endLessTrialsMng.GetActivityDataInfo(refdata.id);
    //   //        refActList.Add(data);
    //   //    }
    //   //    refActList.Sort(GameCenter.endLessTrialsMng.SortActivity);
    //   //    for (int i = 0; i < refActList.Count; i++)
    //   //    {
    //   //        dic[(int)refActList[i].ID] = ConfigMng.Instance.GetActivityListRef((int)refActList[i].ID);
    //   //    }
    //   //}
    //   #endregion
    //   /// <summary>
    //  /// 活动排序 进行中--未开始--已结束
    //  /// </summary>
    //   public int SortActivity(ActivityDataInfo info1, ActivityDataInfo info2)
    //   {
    //       int state1 = 0;
    //       int state2 = 0;
    //       switch (info1.State)
    //       {
    //           case ActivityState.ONGOING:
    //               state1 = 1;
    //               break;
    //           case ActivityState.NOTATTHE:
    //               state1 = 2;
    //               break;
    //           case ActivityState.HASENDED:
    //               state1 = 3;
    //               break;
    //       }
    //       switch (info2.State)
    //       {
    //           case ActivityState.ONGOING:
    //               state2 = 1;
    //               break;
    //           case ActivityState.NOTATTHE:
    //               state2 = 2;
    //               break;
    //           case ActivityState.HASENDED:
    //               state2 = 3;
    //               break;
    //       }
    //       if (state1 > state2)//先按活动状态排序(进行中-未开始-已结束)
    //           return 1;
    //       if (state1 < state2)
    //           return -1;
    //       if (info1.SortNum > info2.SortNum)//状态相同按ListNum排序
    //           return 1;
    //       if (info1.SortNum < info2.SortNum)
    //           return -1;
    //       return 0;
    //   }
    ///// <summary>
    ///// 活动数据
    ///// </summary>
    //public  void C2S_ActivityDataInfo(){
    //	pt_req_activity_info_d714 info = new pt_req_activity_info_d714();
    //	NetMsgMng.SendMsg(info);
    //}
    #endregion
//}
///// <summary>
///// 副本类型
///// </summary>
//public enum OneCopySType
//{
//    NONE = 0,
//    NAIHEQIAO = 1,
//    DOUSHUAIXIANGONG = 2,
//    WUZHUANGGUAN = 3,
//    MONKEY = 5,
//    BAIGUHANYU = 6,
//    TWONAIHEQIAO = 7,
//    TWOBAIGUHANYU = 8,
//}








