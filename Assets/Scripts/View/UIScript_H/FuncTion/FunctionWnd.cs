//======================================================
//作者:何明军
//日期:2016/7/6
//用途:功能开启，引导界面
//======================================================
//======================================================
//作者:唐源
//日期:2017/3/2 (维护)
//用途:功能开启
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FunctionWnd : GUIBase {
    #region 数据
    /// <summary>
    /// 一个子窗口作为新功能开启的弹出框
    /// </summary>
    public FunctionOpenWnd mainFunc;
    /// <summary>
    /// 下一功能开启的新功能类型
    /// </summary>
	FunctionType nextFunc = FunctionType.None;
    /// <summary>
    /// 功能开启数据
    /// </summary>
    protected FunctionDataInfo funcInfo;

    #region 获得可以使用的物品提示
    protected GameObject prefab = null;
    protected List<GameObject> equipTipList = new List<GameObject>();
    protected bool isEquip;
    protected int depath = 35;
    protected List<EquipmentInfo> EquipTip
    {
        get
        {
            return GameCenter.inventoryMng.equipTip;
        }
    }

    private Dictionary<int, EquipmentInfo> tipDic = new Dictionary<int, EquipmentInfo>();
    #endregion

    #region 获得新成就提示
    protected GameObject acheievePrefab = null;
    protected List<GameObject> achieveTipList = new List<GameObject>();
    #endregion
    #endregion
    #region Unity函数
    void Awake(){
		mutualExclusion = false;
		Layer = GUIZLayer.TIP + 580;
		this.transform.localPosition = Vector3.zero;
		nextFunc = FunctionType.None;
	}
    #endregion
    #region OnOpen 刷新
    protected override void OnOpen()
    {
        base.OnOpen();
        //默认隐藏掉功能开启的子窗口并关闭掉新手引导窗口
        CloseAll();
        //窗口开启的判断一下红点
        //CheckBackPackSynthesis();
        //特殊引导
        SpecificGuidelines();
        //刷新背包统一检测相关功能红点
        BackpackUpdate();
        //装备可培养红点
        EquipUpdate();
        //技能红点
        InitData();
        /// <summary>
        ///获得可用物品时候创建一个提示
        /// </summary>
        if (GameCenter.inventoryMng != null)
        {
            for (int i = 0; i < EquipTip.Count; i++)
            {
                CreateEquipTip(EquipTip[i]);
            }
        }
		//CheckBackPackSynthesis();
		//SpecificGuidelines();
        BackpackUpdate();
        EquipUpdate();
        InitData();
        GameCenter.duplicateMng.OnOpenCopyForce = delegate
        {
            GameCenter.uIMng.SwitchToUI(GUIType.FORCE);
        };
        GameCenter.duplicateMng.OnCopyItemUIClose = delegate
        {
            GameCenter.uIMng.ReleaseGUI(GUIType.COPYMULTIPLEWND);
        };
	}
	
	protected override void OnClose ()
	{
		base.OnClose ();
        CancelInvoke("HideEquipTip");
        CancelInvoke("DestoryAchieveGo");
        CancelInvoke("Refresh");
        CancelInvoke("OpenFlyTipWnd");
    }
    #endregion
    #region 事件句柄
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            if (GameCenter.mainPlayerMng != null)
            {
                GameCenter.noviceGuideMng.UpdateFunctionData += UpdateFunctionData;
                GameCenter.noviceGuideMng.UpdateGuideData += UpdateGuideData;
                GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += RefreshBaseDate;
                GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseDiffUpdate += DoFightValueTip;
            }
            InventoryMng.onAddEquipForTip += CreateEquipTipNotSame;
            InventoryMng.onDelEquipForTip += DestroyTip;
            if (GameCenter.achievementMng != null) GameCenter.achievementMng.OnGetNewAchievement += CreateAchievementTip;
            if (GameCenter.inventoryMng != null) GameCenter.inventoryMng.OnBackpackUpdate += BackpackUpdate;
            if (GameCenter.inventoryMng != null) GameCenter.inventoryMng.OnEquipUpdate += EquipUpdate;
            GameCenter.noviceGuideMng.OnSpecilGuild += OnSpecilGuild;
            GameCenter.wingMng.OnShowWingModel += OnShowWingModel;
            GameCenter.newMountMng.OnGetNewMountUpdate += OnShowMountModel;
            GameCenter.mercenaryMng.OnGetNewPetUpdate += OnShowPetModel;
            GameCenter.duplicateMng.OnCopyItemTeamData += OnCopyItemTeamData;
            GameCenter.duplicateMng.OnOpenCopySettlement += OnOpenCopySettlement;
            GameCenter.endLessTrialsMng.OnSweepRewardAll = OnSweepRewardAll;
            GameCenter.inventoryMng.OnEquipUpdate += DestoryLowerEquip;
        }
        else
        {
            if (GameCenter.mainPlayerMng != null)
            {
                GameCenter.noviceGuideMng.UpdateFunctionData -= UpdateFunctionData;
                GameCenter.noviceGuideMng.UpdateGuideData -= UpdateGuideData;
                GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= RefreshBaseDate;
                GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseDiffUpdate -= DoFightValueTip;
            }
            InventoryMng.onAddEquipForTip -= CreateEquipTipNotSame;
            InventoryMng.onDelEquipForTip -= DestroyTip;
            if (GameCenter.achievementMng != null) GameCenter.achievementMng.OnGetNewAchievement -= CreateAchievementTip;
            if (GameCenter.inventoryMng != null) GameCenter.inventoryMng.OnBackpackUpdate -= BackpackUpdate;
            if (GameCenter.inventoryMng != null) GameCenter.inventoryMng.OnEquipUpdate -= EquipUpdate;
            GameCenter.noviceGuideMng.OnSpecilGuild -= OnSpecilGuild;
            GameCenter.wingMng.OnShowWingModel -= OnShowWingModel;
            GameCenter.newMountMng.OnGetNewMountUpdate -= OnShowMountModel;
            GameCenter.mercenaryMng.OnGetNewPetUpdate -= OnShowPetModel;
            GameCenter.duplicateMng.OnCopyItemTeamData -= OnCopyItemTeamData;
            GameCenter.duplicateMng.OnOpenCopySettlement -= OnOpenCopySettlement;
            GameCenter.inventoryMng.OnEquipUpdate -= DestoryLowerEquip;
        }
    }
    #endregion
    #region 引导与功能开启的相关提示
    #region 战力数值提示
    void DoFightValueTip(ActorBaseTag _arg1, int _value, bool _fromAbility)
    {
        if (_value == 0) return;
        if (_arg1 == ActorBaseTag.FightValue)
        {
            MessageST mess = new MessageST();
            if (_value >= 0)
                mess.messID = 28;
            else
            {
                mess.messID = 29;
            }
            mess.words = new string[1];
            mess.words[0] = Mathf.Abs(_value).ToString();
            GameCenter.messageMng.AddClientMsg(mess);
        }
        else if (_arg1 == ActorBaseTag.Exp)
        {

            MessageST mess = new MessageST();
            mess.messID = 19;
            mess.words = new string[] { _value.ToString() };
            GameCenter.messageMng.AddClientMsg(mess);
        }
    }
    #endregion
    #region 获得可以使用的物品和新成就提示
    /// <summary>
    /// 当玩家30级 或者 40级时检查背包是否有更好的装备，进行提示
    /// </summary>
    void CreateEquipTipList()
    {
        foreach (EquipmentInfo info in GameCenter.inventoryMng.GetBackpackEquipDic().Values)
        {
            if (!tipDic.ContainsKey(info.EID))
            {
                CreateEquipTip(info);
                tipDic[info.EID] = info;
            }
        }
    }
    void CreateEquipTipNotSame(EquipmentInfo _info)
    {
        if (!EquipTip.Contains(_info))
        {
            CreateEquipTip(_info);
        }
    }

    /// <summary>
    /// 获得可用物品时，创建提示
    /// </summary>
    void CreateEquipTip(EquipmentInfo _info)
    {
        if (GameCenter.openServerRewardMng.isRotate) return;
        if (GameCenter.uIMng.CurOpenType == GUIType.SPRITEANIMAL)//使用宠物蛋融合的时候不提示
        {
            if (_info.ActionType == EquipActionType.activate_animal)
            {
                return;
            }
        }
        if (prefab == null)
        {
            prefab = exResources.GetResource(ResourceType.GUI, "mainUI/Main_ItemTips") as GameObject;
        }
        if (prefab == null)
        {
            Debug.Log("找不到相关预制！");
            return;
        }
        MainPlayerInfo info = GameCenter.mainPlayerMng.MainPlayerInfo;
        if (_info.OldSort >= 1 && _info.OldSort <= 12)
        {
            isEquip = _info.IsBetterSlot(_info);
        }
        else
        {
            isEquip = true;
        }
        if (_info.AttentionType == GoodsAttentionType.YES && (_info.NeedProf == info.Prof || _info.NeedProf == 0) && _info.UseReqLevel <= info.CurLevel && isEquip)
        {
            depath++;
            GameObject go = Instantiate(prefab) as GameObject;
            go.transform.parent = this.gameObject.transform;
            go.transform.localPosition = prefab.transform.localPosition;
            go.transform.localScale = Vector3.one;
            UIPanel uiPanel = go.GetComponent<UIPanel>();
            if (uiPanel != null)
            {
                uiPanel.depth = depath;
            }
            go.SetActive(true);
            equipTipList.Add(go);
            EquipTipUI equipTipUI = go.GetComponent<EquipTipUI>();
            if (equipTipUI != null)
                equipTipUI.SetEquipTipInfo(_info, depath, equipTipList);
            if (!EquipTip.Contains(_info))
            {
                EquipTip.Add(_info);
            }
            CancelInvoke("HideEquipTip");
            Invoke("HideEquipTip", 30.0f);
        }
        prefab = null;
    }
    void HideEquipTip()
    {
        for (int i = 0; i < EquipTip.Count; i++)
        {
            if (i == EquipTip.Count - 1)
            {
                EquipmentInfo info = EquipTip[i];
                EquipTip.Remove(info);
                GameObject go = equipTipList[i];
                equipTipList.Remove(go);
                DestroyImmediate(go);
                Invoke("HideEquipTip", 30.0f);
            }
        }
    }
    /// <summary>
    /// 物品被使用删除提示
    /// </summary>
    void DestroyTip(int _id)
    {
        for (int i = 0; i < EquipTip.Count; i++)
        {
            EquipmentInfo info = EquipTip[i];
            if (info.InstanceID == _id)
            {
                EquipTip.Remove(info);
                GameObject go = equipTipList[i];
                equipTipList.Remove(go);
                DestroyImmediate(go);
            }
        }
    }

    void DestoryLowerEquip()
    {
        int count = EquipTip.Count;
        while(count > 0)
        {
            EquipmentInfo info = EquipTip[count - 1];
            EquipmentInfo equip = GameCenter.inventoryMng.GetEquipFromEquipDicBySlot(info.Slot);
            if (equip != null && equip.IsBetter(info))//和身上的做比较
            {
                EquipTip.Remove(info);
                GameObject go = equipTipList[count - 1];
                equipTipList.Remove(go);
                DestroyImmediate(go);
            }
            count--;
        }
    }
    /// <summary>
    /// 获得新成就时，创建提示
    /// </summary>
    /// <param name="_data"></param>
    void CreateAchievementTip(AchievementData _data)
    {
        if (acheievePrefab == null)
        {
            acheievePrefab = exResources.GetResource(ResourceType.GUI, "mainUI/Achievement") as GameObject;
        }
        if (acheievePrefab == null)
        {
            Debug.Log("找不到相关预制！");
            return;
        }
        GameObject achieveGo = Instantiate(acheievePrefab) as GameObject;
        achieveGo.transform.parent = this.gameObject.transform;
        achieveGo.transform.localPosition = acheievePrefab.transform.localPosition;
        acheievePrefab = null;
        achieveGo.transform.localScale = Vector3.one;
        achieveGo.SetActive(true);
        AchievementTip achievementTip = achieveGo.GetComponent<AchievementTip>();
        if (achievementTip != null)
            achievementTip.SetAchievementTip(_data);
        achieveTipList.Add(achieveGo);
        CancelInvoke("DestoryAchieveGo");
        Invoke("DestoryAchieveGo", 5.0f);
    }
    void DestoryAchieveGo()
    {
        for (int i = 0; i < achieveTipList.Count; i++)
        {
            if (i == achieveTipList.Count - 1)
            {
                GameObject go = achieveTipList[i];
                achieveTipList.Remove(go);
                DestroyImmediate(go);
                Invoke("DestoryAchieveGo", 5.0f);
            }
        }
    }
    #endregion
    #endregion
    #region 开启特殊引导
    void SpecificGuidelines(){
        //Debug.Log("打开特殊引导");
        TaskInfo task = GameCenter.taskMng.GetMainTaskInfo();
		int sceneID = GameCenter.curGameStage.SceneID;
        //判断主线任务和任务步骤以及场景和是否已经创建角色确定弹出欢迎界面
		if(task != null && task.Task == 1 && task.Step == 1 && sceneID == 100011){
            if (GameCenter.loginMng.isCreatePlayer)
            {
                GameCenter.uIMng.SwitchToUI(GUIType.WELCOME);
                GameCenter.loginMng.isCreatePlayer = false;
            }
			else 
                GameCenter.noviceGuideMng.OpenGuide(100024,1);//任务引导点击执行任务
			return ;
		}
		//红名小王子 引导PK
		if(sceneID == 160007){
			GameCenter.curMainPlayer.GoHoldOn();
			GameCenter.curMainPlayer.StopForCheckMsg();
//			GameCenter.curMainPlayer.GoAutoFight();
			GameCenter.noviceGuideMng.OpenGuide(100023,1);
			GameCenter.uIMng.SwitchToSubUI(SubGUIType.PKTIP);
			return ;
		}
        //花果山场景 挂机引导
        else if(sceneID == 160005){
			GameCenter.curMainPlayer.GoHoldOn();
			GameCenter.curMainPlayer.StopForCheckMsg();
//			GameCenter.curMainPlayer.GoAutoFight();
			GameCenter.noviceGuideMng.OpenGuide(100021,1);
//			isOpenGuide = false;
//			GameCenter.endLessTrialsMng.OnOpenCoppyTime -= OpenCoppyTime;
//			GameCenter.endLessTrialsMng.OnOpenCoppyTime += OpenCoppyTime;
			return ;
		}
		SceneRef sceneRef = ConfigMng.Instance.GetSceneRef(sceneID);
		if(GameCenter.endLessTrialsMng.IsFastClearance && (sceneRef.sort == SceneType.PEACEFIELD || sceneRef.sort == SceneType.CAMPFIELD || sceneRef.sort == SceneType.SCUFFLEFIELD)){
			GameCenter.endLessTrialsMng.IsFastClearance = false;
			GameCenter.noviceGuideMng.OpenGuide(100018,1);
			return ;
		}
	}
    #region 普通攻击和技能特殊引导
    OpenNewFunctionGuideRef guid;
    void OnSpecilGuild(OpenNewFunctionGuideRef _data)
    {
        //Debug.Log("普通攻击和技能特殊引导");
        guid = _data;
        if (_data.type == 100001 || _data.type == 100002)
        {
            GameCenter.curMainPlayer.CancelCommands();
            if (GameCenter.curMainPlayer.CurFSMState != MainPlayer.EventType.AI_DART_CTRL && GameCenter.curMainPlayer.CurFSMState != MainPlayer.EventType.AI_FIGHT_CTRL)
            {
                GameCenter.curMainPlayer.GoAutoFight();
            }
        }
        if (funcInfo != null && funcInfo.SceneID > 0)//指引完毕如果有传送点，则传送
        {
            CloseAll();
            GameCenter.mainPlayerMng.C2STaskFly(funcInfo.SceneID);
        }
    }
    //void Refresh()
    //{
    //    if (GameCenter.mainPlayerMng.guildTask.TaskState != TaskStateType.Process) return;
    //    GameCenter.mainPlayerMng.OpenGuide(guid);
    //}
    #endregion
    /*
	void OpenCoppyTime(){
		SceneMng.OnDelInterObj -= UpdateCheckCoppy;
		SceneMng.OnDelInterObj += UpdateCheckCoppy;
		SceneMng.OnDropItemEvent -= UpdateCheckDropItem;
		SceneMng.OnDropItemEvent += UpdateCheckDropItem;
	}

	void UpdateCheckDropItem(DropItemInfo info){
		if(GameCenter.sceneMng.GetDropItemInfoListByOwner(GameCenter.curMainPlayer.id).Count <=0){
			SceneMng.OnDelInterObj -= UpdateCheckCoppy;
			SceneMng.OnDropItemEvent -= UpdateCheckDropItem;
			if(!isOpenGuide){
				isOpenGuide = true;
				GameCenter.mainPlayerMng.OpenGuide(100022,1);
			}
		}
	}

	void UpdateCheckCoppy(ObjectType type, int id){
		if(type == ObjectType.DropItem && GameCenter.sceneMng.GetDropItemInfoListByOwner(GameCenter.curMainPlayer.id).Count <=0){
			SceneMng.OnDelInterObj -= UpdateCheckCoppy;
			SceneMng.OnDropItemEvent -= UpdateCheckDropItem;
			if(!isOpenGuide){
				isOpenGuide = true;
				GameCenter.mainPlayerMng.OpenGuide(100022,1);
			}
		}
	}*/
	#endregion
	
	#region 人物属性变化与背包数据变化
	void RefreshBaseDate(ActorBaseTag tag, ulong value,bool _fromAbility)
	{
		switch (tag)
		{
		case ActorBaseTag.Level:
			GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.REIN,GameCenter.mainPlayerMng.MainPlayerInfo.IsRien);
            GameCenter.rankRewardMng.RereshRankRewardWnd();
            //升到20级弹出升级UI
            if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel == 21)
            {
                //GameCenter.uIMng.SwitchToUI(GUIType.UPTIPUI);
                CancelInvoke("OpenFlyTipWnd");
                Invoke("OpenFlyTipWnd",1f);
            }
            //当玩家30级或者40级时进行提示背包中是否有更好的装备
            if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel == 30 || GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel == 40)
            {
                CreateEquipTipList();
            }
			break;
		case ActorBaseTag.Fix:
			GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.REIN,GameCenter.mainPlayerMng.MainPlayerInfo.IsRien);
			break;
        case ActorBaseTag.BindCoin:
            GameCenter.wingMng.SetWingRedPoint();
            GameCenter.magicWeaponMng.SetMagicRedPoint();
            break;
        case ActorBaseTag.UnBindCoin:
            GameCenter.wingMng.SetWingRedPoint();
            GameCenter.magicWeaponMng.SetMagicRedPoint();
            break;
        case ActorBaseTag.GuildContribution:
            GameCenter.guildSkillMng.SetSkillRed();
            break;
		}

	}
    void InitData()
    {
        if (GameCenter.guildSkillMng != null)
            GameCenter.guildSkillMng.SetSkillRed();
    }
	/// <summary>
	/// 背包数据变化，统一检测相关功能红点a
	/// </summary>
	void BackpackUpdate(){
		//CheckBackPackSynthesis();
        if (GameCenter.wingMng != null)
            GameCenter.wingMng.SetWingRedPoint();
        if (GameCenter.newMountMng != null)
            GameCenter.newMountMng.SetRedRemind();
        if (GameCenter.mercenaryMng != null)
            GameCenter.mercenaryMng.SetRedRemind();
        if (GameCenter.magicWeaponMng != null)
            GameCenter.magicWeaponMng.SetMagicRedPoint();
		if(GameCenter.equipmentTrainingMng != null)
			GameCenter.equipmentTrainingMng.SetRedTipState();
        if (GameCenter.newMountMng != null)
            GameCenter.newMountMng.SetRedTipState();
	}
	/// <summary>
	/// 身上装备发生变化
	/// </summary>
	void EquipUpdate()
	{
		if(GameCenter.equipmentTrainingMng != null)
			GameCenter.equipmentTrainingMng.SetRedTipState();
	}
	/// <summary>
	/// 合成红点
	/// </summary>
    //void CheckBackPackSynthesis(){
    //    if (!GameCenter.mainPlayerMng.isFirstOpenBackSynUI) return;
    //    FDictionary synths = ConfigMng.Instance.GetBlendRefTable();
    //    foreach(BlendRef refa in synths.Values){
    //        int count = GameCenter.inventoryMng.GetNumberByType(refa.needItems[0].eid);
    //        if(refa.needItems.Count > 0 && count/(refa.needItems.Count * refa.needItems[0].count) > 0){
    //            GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.SYNTHETIC,true);
    //            return ;
    //        }
    //    }
    //    GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.SYNTHETIC,false);
    //}
    /// <summary>
    /// 技能红点
    /// </summary>
    //void InitData()
    //{
    //    if (GameCenter.guildSkillMng != null)
    //        GameCenter.guildSkillMng.SetSkillRed();
    #endregion
    #region 功能开启与指引交互逻辑
    void UpdateGuideData(OpenNewFunctionGuideRef _data){
        //Debug.Log("功能开启与指引");
        if (_data != null && _data.close){
			GameCenter.uIMng.SwitchToSubUI(SubGUIType.NONE);
			GameCenter.uIMng.SwitchToUI(GUIType.NONE);
			GameCenter.uIMng.ShowSkill(true);
			GameCenter.uIMng.ShowMenu(false);
            GameCenter.uIMng.ShowMapMenu(true);
		}
		CloseAll();
		GameCenter.curMainPlayer.GoNormal();
		//if(mainGuide != null && !mainGuide.gameObject.activeSelf){
			if(_data == null)return ;
			GameCenter.noviceGuideMng.RefData = _data;
			EventDelegate.Remove(GameCenter.noviceGuideMng.OnGuideOver,OnGuideOver);
			EventDelegate.Add(GameCenter.noviceGuideMng.OnGuideOver, OnGuideOver);
        //mainGuide.OpenUI();
            GameCenter.uIMng.GenGUI(GUIType.NEWGUID,true);
	}
    /// <summary>
    /// 引导结束的时候抛出事件更新功能
    /// </summary>
	void OnGuideOver(){
        //Debug.Log("结束引导");
		if(nextFunc != FunctionType.None){
			FunctionDataInfo _funcInfo = GameCenter.mainPlayerMng.GetFunctionData(nextFunc);
            if (_funcInfo != null)// && _funcInfo.IsOpon)
            {
                _funcInfo.Update(true);
                GameCenter.noviceGuideMng.UpdateFunctionData(_funcInfo);
                //Debug.Log("更新功能");
            }
		}
	}
    /// <summary>
    /// 刷新功能数据
    /// </summary>
    /// <param name="_funcInfo"></param>
	void UpdateFunctionData(FunctionDataInfo _funcInfo){
		nextFunc = FunctionType.None;
		if(_funcInfo.VipLev > GameCenter.vipMng.VipData.vLev){
			return ;
		}
        funcInfo = _funcInfo;
        if (_funcInfo.SceneID > 0 && _funcInfo.FunSubGUIType == SubGUIType.NONE && _funcInfo.FunGUIType == GUIType.NONE)
        {
            CloseAll();
            GameCenter.noviceGuideMng.newFunctionCopyId = _funcInfo.SceneID;
            GameCenter.uIMng.SwitchToUI(GUIType.NEWFUNCTIONTIPUI);
            return;
        }
		if(_funcInfo.FunSubGUIType == SubGUIType.FUNCTIONOPEN){
			if(mainFunc != null && !mainFunc.gameObject.activeSelf){
				CloseAll();
				mainFunc.FuncInfo = _funcInfo;
				nextFunc = _funcInfo.NextFunc;
				mainFunc.OpenUI();
			}
			return ;
		}
		if(_funcInfo.FunSubGUIType == SubGUIType.GUIDEOPEN){
			if(_funcInfo.GuideData == null)return ;
			GameCenter.noviceGuideMng.OpenGuide(_funcInfo.GuideData);
			return ;
		}
		if(_funcInfo.FunGUIType != GUIType.NONE){
			CloseAll();
			GameCenter.uIMng.SwitchToUI(_funcInfo.FunGUIType);
		}else if(_funcInfo.FunSubGUIType != SubGUIType.NONE){
			CloseAll();
			GameCenter.uIMng.SwitchToSubUI(_funcInfo.FunSubGUIType);
		}
        if (_funcInfo.FunGUIType == GUIType.NONE && _funcInfo.FunSubGUIType == SubGUIType.NONE && _funcInfo.GuideData != null)
        {
            GameCenter.noviceGuideMng.OpenGuide(_funcInfo.GuideData);
            nextFunc = _funcInfo.NextFunc;
        }
	}
    #endregion
    #region 辅助逻辑
    void CloseAll(){
        //Debug.logger.Log("隐藏两个子窗口");
        //if(mainGuide != null && mainGuide.gameObject.activeSelf)mainGuide.CloseUI();
        GameCenter.uIMng.ReleaseGUI(GUIType.NEWGUID);
		if(mainFunc != null && mainFunc.gameObject.activeSelf)mainFunc.CloseUI();
	}
    //非会员试用到期
    void OpenFlyTipWnd()
    {
        //非VIP才弹
        if (GameCenter.vipMng.VipData == null || GameCenter.vipMng.VipData.vLev <= 0)
        {
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
            //传送试用到期弹窗
            GameCenter.uIMng.GenGUI(GUIType.UPTIPUI, true);
        }
    }
    #endregion 
    #region 激活翅膀时弹出展示模型窗口
    /// <summary>
    /// 激活翅膀弹出展示模型UI
    /// </summary>
    /// <param name="_wingId"></param>
    void OnShowWingModel(WingInfo _wingInfo)
    {
        if (Time.time - GameCenter.mainPlayerMng.MainPlayerInfo.loginTime < 10) return;//在创建主角之后十秒内 不做任何操作
        GameCenter.wingMng.modelType = ModelType.WING;
        GameCenter.wingMng.needShowWingInfo = _wingInfo;
        GameCenter.wingMng.isNotShowTrialWingModel = true;
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
        GameCenter.uIMng.GenGUI(GUIType.SHOWMODELUI, true);
    }
    /// <summary>
    /// 坐骑激活时弹出模型UI
    /// </summary>
    void OnShowMountModel(MountInfo _mountInfo, ModelType _type)
    { 
        if (Time.time - GameCenter.mainPlayerMng.MainPlayerInfo.loginTime < 10) return;
        GameCenter.wingMng.modelType = _type;
        GameCenter.wingMng.needShowMountInfo = _mountInfo;
        GameCenter.wingMng.isNotShowTrialWingModel = true;
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
        GameCenter.uIMng.GenGUI(GUIType.SHOWMODELUI, true);
    }
    /// <summary>
    /// 宠物激活时弹出模型UI
    /// </summary>
    void OnShowPetModel(MercenaryInfo _petInfo)
    {
        if (Time.time - GameCenter.mainPlayerMng.MainPlayerInfo.loginTime < 10) return;
        GameCenter.wingMng.modelType = ModelType.PET;
        GameCenter.wingMng.needShowPetInfo = _petInfo;
        GameCenter.wingMng.isNotShowTrialWingModel = true;
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
        GameCenter.uIMng.GenGUI(GUIType.SHOWMODELUI, true);
    }
    #endregion

    #region 多人界面队员数据 副本结算 扫荡奖励
    void OnCopyItemTeamData(int copyGroupID)
    {
        //if(GameCenter.uIMng.CurOpenType == GUIType.COPYMULTIPLEWND){
        //    return ;
        //} 
        if (GameCenter.teamMng.isInTeam)
        {
            if (!GameCenter.teamMng.isLeader)
            {
                if (GameCenter.duplicateMng.isMagicTowrAddFri)//镇魔塔招募队友直接进入界面，不弹提示
                {
                    GameCenter.duplicateMng.copyGroupID = copyGroupID;
                    GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                    GameCenter.uIMng.SwitchToUI(GUIType.COPYMULTIPLEWND);
                    GameCenter.duplicateMng.isMagicTowrAddFri = false;
                }
                else
                {
                    MessageST mst = new MessageST();
                    mst.messID = 166;
                    mst.delYes = delegate
                    {
                        if (GameCenter.teamMng.isInTeam)
                        {
                            GameCenter.duplicateMng.copyGroupID = copyGroupID;
                            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                            GameCenter.uIMng.SwitchToUI(GUIType.COPYMULTIPLEWND);
                        }
                        else
                            GameCenter.messageMng.AddClientMsg(78);
                    };
                    mst.delNo = delegate
                    {
                        GameCenter.teamMng.C2S_TeamOut();
                    };
                    GameCenter.messageMng.AddClientMsg(mst);
                }
            }
            else
            {
                GameCenter.duplicateMng.copyGroupID = copyGroupID;
                if (GameCenter.duplicateMng.CopyType != DuplicateMng.CopysType.MAGICTOWER)
                {
                    GameCenter.uIMng.SwitchToUI(GUIType.COPYMULTIPLEWND, GUIType.COPYINWND);
                }
                else
                {
                    GameCenter.uIMng.SwitchToUI(GUIType.COPYMULTIPLEWND, GUIType.MagicTowerWnd);
                }
            }
        }
    }

    void OnOpenCopySettlement()
    {
        GameCenter.uIMng.GenGUI(GUIType.COPYWIN, true);
    }
    void OnSweepRewardAll()
    {
        if (GameCenter.endLessTrialsMng.sweepType == EndLessTrialsMng.SweepType.EndLess) GameCenter.uIMng.SwitchToUI(GUIType.SWEEPCARBON, GUIType.ENDLESSWND);
        if (GameCenter.endLessTrialsMng.sweepType == EndLessTrialsMng.SweepType.COPY) GameCenter.uIMng.SwitchToUI(GUIType.SWEEPCARBON, GUIType.COPYINWND);
    }
    #endregion
}
