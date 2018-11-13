//======================================================
//作者：吴江
//日期：2015/5/10
//用途：游戏当前场景内的所有基本对象极其派生对象的【对象管理类】
//======================================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;

/// <summary>
/// 游戏当前场景内的所有基本对象极其派生对象的【对象管理类】 by 吴江
/// </summary>
public class SceneMng
{

    protected int sceneID = -1;

    protected SceneType sceneType = SceneType.NONE;

    protected int enterSceneSerlizeID = -1;
    public int EnterSceneSerlizeID
    {
        get
        {
            return enterSceneSerlizeID;
        }
        protected set
        {
            enterSceneSerlizeID = value;
        }
    }

    protected Vector2 safetyAreaCenter = Vector2.zero;
    public Vector2 SafetyAreaCenter
    {
        get
        {
            return safetyAreaCenter;
        }
    }
    protected int safetyAreaRadius = 0;
    public int SafetyAreaRadius
    {
        get
        {
            return safetyAreaRadius;
        }
    }
    protected bool hasSafetyArea = false;
    public bool HasSafetyArea
    {
        get
        {
            return hasSafetyArea;
        }
    }
    protected bool hasAreaBuff = false;
    /// <summary>
    /// 是否有区域型buff
    /// </summary>
    public bool HasAreaBuff
    {
        get { return hasAreaBuff; }
    }
    protected List<AreaBuffRef> areaBuffList = new List<AreaBuffRef>();
    /// <summary>
    /// 区域型buff的数据
    /// </summary>
    public List<AreaBuffRef> AreaBuffList
    {
        get { return areaBuffList; }
    }
	protected Vector3 wayPoint = Vector3.zero;
	/// <summary>
	/// 场景里面的路点
	/// </summary>
	public Vector3 WayPoint
	{
		get
		{
			return wayPoint;
		}
	}
	public System.Action OnWayPointUpdateEvent;

    //public Action OnBaseValueChange;
    #region 构造
    public static SceneMng CreateNew()
    { 
        if (GameCenter.sceneMng == null)
        {
            SceneMng sceneMng = new SceneMng();
            sceneMng.Init();
            return sceneMng;
        }
        else
        {
            GameCenter.sceneMng.UnRegist();
            GameCenter.sceneMng.Init();
            return GameCenter.sceneMng;
        }
    }
    /// <summary>
    /// 注册，构造
    /// </summary>
    protected void Init()
    {
        EnterSceneSerlizeID = 0;
        MsgHander.Regist(0xB106, S2C_ChangeSceneBack);
        MsgHander.Regist(0xC001, S2C_ObjectMove);
        MsgHander.Regist(0xC002, S2C_OnSceneAddObject);
		MsgHander.Regist(0xD810,S2C_OnSceneOPCUpdate);
        MsgHander.Regist(0xC003, S2C_OnSceneDeleteObject);
        MsgHander.Regist(0xC011, S2C_AddDropItem);
        MsgHander.Regist(0xC012, S2C_PickUpDropItem);
        MsgHander.Regist(0xC013, S2C_TransformPlayer);
        MsgHander.Regist(0xC014, S2C_AddBallisticCurve);
        MsgHander.Regist(0xC015, S2C_AddTrap);
        MsgHander.Regist(0xC016, S2C_DeleteBallisticCurve);
        MsgHander.Regist(0xC017, S2C_DeleteTrap);
        MsgHander.Regist(0xC018, S2C_PlayerTeamChange);
        MsgHander.Regist(0xC020, S2C_OnSceneHideObject);
        MsgHander.Regist(0xC01B, S2C_OnMonsterChangeOwner);
        //MsgHander.Regist(0xD01D, S2C_OnBaseValueChange);
        MsgHander.Regist(0xD101, S2C_ActorUpdateEquip);
        //MsgHander.Regist(0xD202, S2C_ObjTalking);
        MsgHander.Regist(0xD203, S2C_StartCollect);
        MsgHander.Regist(0xD204, S2C_StopCollect);
        MsgHander.Regist(0xD20B, S2C_OnOtherPlayerJump);
        MsgHander.Regist(0xD222, S2C_OnPlayerHeadTextUpdate);
        MsgHander.Regist(0xC010, S2C_UpdateMonsterOwner);
		MsgHander.Regist(0xC00A,S2C_OnWayPointUpdate);
        if (GameCenter.mainPlayerMng != null && GameCenter.mainPlayerMng.MainPlayerInfo != null)
        {
            SceneRef sr = ConfigMng.Instance.GetSceneRef(GameCenter.mainPlayerMng.MainPlayerInfo.SceneID);
            if (sr != null)
            {
                safetyAreaCenter = sr.safeAreaPoint;
                safetyAreaRadius = sr.safeAreaRadius;
                hasSafetyArea = safetyAreaCenter != Vector2.zero && safetyAreaRadius > 0;
            }
            areaBuffList = ConfigMng.Instance.GetAreaBuffRefBySceneID(GameCenter.mainPlayerMng.MainPlayerInfo.SceneID);
            hasAreaBuff = areaBuffList.Count > 0;
        }
    }
    /// <summary>
    /// 注销，清理
    /// </summary>
    protected void UnRegist()
    {
        MsgHander.UnRegist(0xB106, S2C_ChangeSceneBack);
        MsgHander.UnRegist(0xC001, S2C_ObjectMove);
        MsgHander.UnRegist(0xC002, S2C_OnSceneAddObject);
		MsgHander.UnRegist(0xD810,S2C_OnSceneOPCUpdate);
        MsgHander.UnRegist(0xC003, S2C_OnSceneDeleteObject);
        MsgHander.UnRegist(0xC011, S2C_AddDropItem);
        MsgHander.UnRegist(0xC012, S2C_PickUpDropItem);
        MsgHander.UnRegist(0xC013, S2C_TransformPlayer);
        MsgHander.UnRegist(0xC014, S2C_AddBallisticCurve);
        MsgHander.UnRegist(0xC015, S2C_AddTrap);
        MsgHander.UnRegist(0xC016, S2C_DeleteBallisticCurve);
        MsgHander.UnRegist(0xC017, S2C_DeleteTrap);
        MsgHander.UnRegist(0xC018, S2C_PlayerTeamChange);
        MsgHander.UnRegist(0xC020, S2C_OnSceneHideObject);
        MsgHander.UnRegist(0xC01B, S2C_OnMonsterChangeOwner);
        //MsgHander.UnRegist(0xD01D, S2C_OnBaseValueChange);
        MsgHander.UnRegist(0xD101, S2C_ActorUpdateEquip);
        //MsgHander.UnRegist(0xD202, S2C_ObjTalking);
        MsgHander.UnRegist(0xD203, S2C_StartCollect);
        MsgHander.UnRegist(0xD204, S2C_StopCollect);
        MsgHander.UnRegist(0xD20B, S2C_OnOtherPlayerJump);
        MsgHander.UnRegist(0xD222, S2C_OnPlayerHeadTextUpdate);
        MsgHander.UnRegist(0xC010, S2C_UpdateMonsterOwner);
		MsgHander.UnRegist(0xC00A,S2C_OnWayPointUpdate);
        EnterSucceed = false;
        opcInfoDictionary.Clear();
        npcInfoDictionary.Clear();
        mobInfoDictionary.Clear();
        entourageInfoDictionary.Clear();
        sceneItemInfoDictionary.Clear();
        abilityBallisticCurveInfoDictionary.Clear();
        trapInfoDictionary.Clear();
        dropItemDictionary.Clear();
        modelInfoDictionary.Clear();
		wayPoint = Vector3.zero;//路点重置
        EnterSceneSerlizeID = 0;
    }
    #endregion

    #region 数据
    /// <summary>
    /// 成功进入场景，所有加载结束，可以开始行动  by吴江
	/// 从默认true改为false by 何明军
    /// </summary>
	protected bool enterSucceed = false;
    /// <summary>
    /// 成功进入场景，所有加载结束，可以开始行动  by吴江
    /// </summary>
    public bool EnterSucceed
    {
        get { return enterSucceed; }
        protected set
        {
            if (enterSucceed != value)
            {
                enterSucceed = value;
                if (SceneStateChange != null)
                {
                    SceneStateChange(enterSucceed);
                }
            }
        }
    }
    /// <summary>
    /// 场景状态切换
    /// </summary>
    public System.Action<bool> SceneStateChange;

    /// <summary>
    /// 是否正在切换场景，切换场景的时候不能发送上马协议 add by zsy
    /// </summary>
    public bool isChangingScene = true;

    #region 场景内对象数据
    #region 其他玩家
    protected FDictionary opcInfoDictionary = new FDictionary();
    /// </summary>
    /// 其他玩家数据 by吴江
    /// </summary>
    public FDictionary OPCInfoDictionary
    {
        get { return opcInfoDictionary; }
    }
    /// <summary>
    /// 获取指定ID的其他玩家数据对象 by吴江
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public OtherPlayerInfo GetOPCInfo(int _id)
    {
        if (opcInfoDictionary.ContainsKey(_id))
        {
            return opcInfoDictionary[_id] as OtherPlayerInfo;
        }
        return null;
    }
    /// <summary>
    /// 获取指定name的其他玩家数据对象  
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public OtherPlayerInfo GetOPCInfo(string name)
    {
        foreach (OtherPlayerInfo info in opcInfoDictionary.Values)
        {
            if (info.Name == name)
                return info;
        }
        return null;
    }
    /// <summary>
    /// 其他玩家数据列表变化的事件 by吴江
    /// </summary>
    public static Action OnOPCInfoListUpdate;
    #endregion
    #region 雕像
    protected FDictionary modelInfoDictionary = new FDictionary();
    /// </summary>
    /// 雕像数据 by吴江
    /// </summary>
    public FDictionary ModelInfoDictionary
    {
        get { return modelInfoDictionary; }
    }
    /// <summary>
    /// 获取指定ID的雕像数据对象 by吴江
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public OtherPlayerInfo GetModelInfo(int _id)
    {
        if (modelInfoDictionary.ContainsKey(_id))
        {
            return modelInfoDictionary[_id] as OtherPlayerInfo;
        }
        return null;
    }
    /// <summary>
    /// 获取指定name的雕像数据对象  
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public OtherPlayerInfo GetModelInfo(string name)
    {
        foreach (OtherPlayerInfo info in modelInfoDictionary.Values)
        {
            if (info.Name == name)
                return info;
        }
        return null;
    }
    /// <summary>
    /// 雕像数据列表变化的事件 by吴江
    /// </summary>
    public static Action OnModelInfoListUpdate;
    #endregion
    #region NPC
    protected FDictionary npcInfoDictionary = new FDictionary();
    /// </summary>
    /// 其他玩家数据 by吴江
    /// </summary>
    public FDictionary NPCInfoDictionary
    {
        get { return npcInfoDictionary; }
    }
    /// <summary>
    /// 获取指定ID的NPC数据对象 by吴江
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public NPCInfo GetNPCInfo(int _id)
    {
        if (npcInfoDictionary.ContainsKey(_id))
        {
            return npcInfoDictionary[_id] as NPCInfo;
        }
        return null;
    }
    /// <summary>
    /// NPC数据列表变化的事件 by吴江
    /// </summary>
    public static Action OnNPCInfoListUpdate;
    #endregion
    #region 怪物
    protected FDictionary mobInfoDictionary = new FDictionary();
    /// </summary>
    /// 其他玩家数据 by吴江
    /// </summary>
    public FDictionary MobInfoDictionary
    {
        get { return mobInfoDictionary; }
    }
    /// <summary>
    /// 获取指定ID的怪物数据对象 by吴江
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public MonsterInfo GetMobInfo(int _id)
    {
        if (mobInfoDictionary.ContainsKey(_id))
        {
            return mobInfoDictionary[_id] as MonsterInfo;
        }
        return null;
    }
	/// <summary>
	/// 获取当前玩家的or公会的镖车信息 by邓成
	/// </summary>
	public MonsterInfo GetMyDartInfo()
	{
		MonsterInfo info = null;
		foreach (var item in mobInfoDictionary.Values) 
		{
			info = item as MonsterInfo;
			if(info != null)
			{
				switch( info.RankLevel)
				{
				case MobRankLevel.DAILYDART:
					if(info.DartOwnerID == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID) 
						return info;
					break;
				case MobRankLevel.GUILDDART:
					if(GameCenter.guildMng.MyGuildInfo != null && info.DartOwnerID == GameCenter.guildMng.MyGuildInfo.GuildId) 
						return info;
					break;
				}
			}
		}
		return null;
	}
	/// <summary>
	/// 每日镖车出来的时候,自动跟随(仙盟运镖不用)  by邓成
	/// </summary>
	public void AutoDailyDart(MonsterInfo _monsterInfo)
	{
		if(_monsterInfo != null &&  _monsterInfo.RankLevel == MobRankLevel.DAILYDART && _monsterInfo.DartOwnerID == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
		{
			GameCenter.curMainPlayer.GoAutoDart();
		}
	}

    /// <summary>
    /// NPC数据列表变化的事件 by吴江
    /// </summary>
    public static Action OnMobInfoListUpdate;
    public static Action<int> OnMobChange;
    #endregion
    #region 随从
    protected FDictionary entourageInfoDictionary = new FDictionary();
    /// </summary>
    /// 其他玩家的随从数据 by吴江
    /// </summary>
    public FDictionary EntourageInfoDictionary
    {
        get { return entourageInfoDictionary; }
    }
    /// <summary>
    /// 获取指定ID的随从数据对象 by吴江
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public MercenaryInfo GetEntourageInfo(int _id)
    {
        if (entourageInfoDictionary.ContainsKey(_id))
        {
            return entourageInfoDictionary[_id] as MercenaryInfo;
        }
        return null;
    }
    /// <summary>
    /// 随从数据列表变化的事件 by吴江
    /// </summary>
    public static Action OnEntourageInfoListUpdate;
    #endregion
    #region 场景物品
    protected FDictionary sceneItemInfoDictionary = new FDictionary();
    /// </summary>
    /// 场景物品数据 by吴江
    /// </summary>
    public FDictionary SceneItemInfoDictionary
    {
        get { return sceneItemInfoDictionary; }
    }
    /// <summary>
    /// 获取指定ID的场景物品数据对象 by吴江
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public SceneItemInfo GetSceneItemInfo(int _id)
    {
        if (sceneItemInfoDictionary.ContainsKey(_id))
        {
            return sceneItemInfoDictionary[_id] as SceneItemInfo;
        }
        return null;
    }
    /// <summary>
    /// 场景物品数据列表变化的事件 by吴江
    /// </summary>
    public static Action OnSceneItemInfoListUpdate;
    #endregion
    #region 弹道
    protected FDictionary abilityBallisticCurveInfoDictionary = new FDictionary();
    /// </summary>
    /// 弹道数据 by吴江
    /// </summary>
    public FDictionary AbilityBallisticCurveInfoDictionary
    {
        get { return abilityBallisticCurveInfoDictionary; }
    }
    /// <summary>
    /// 获取指定ID的弹道数据对象 by吴江
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public AbilityBallisticCurveInfo GetAbilityBallisticCurveInfo(int _id)
    {
        if (abilityBallisticCurveInfoDictionary.ContainsKey(_id))
        {
            return abilityBallisticCurveInfoDictionary[_id] as AbilityBallisticCurveInfo;
        }
        return null;
    }
    /// <summary>
    /// 弹道表现数据变化的事件 by吴江
    /// </summary>
    public static Action<int> OnAbilityBallisticCurveUpdate;
    #endregion
    #region 陷阱
    protected FDictionary trapInfoDictionary = new FDictionary();
    /// </summary>
    /// 陷阱数据 by吴江
    /// </summary>
    public FDictionary TrapInfoDictionary
    {
        get { return trapInfoDictionary; }
    }
    /// <summary>
    /// 获取指定ID的陷阱数据对象 by吴江
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public TrapInfo GetTrapInfo(int _id)
    {
        if (trapInfoDictionary.ContainsKey(_id))
        {
            return trapInfoDictionary[_id] as TrapInfo;
        }
        return null;
    }
    /// <summary>
    /// 陷阱表现数据变化的事件 by吴江
    /// </summary>
    public static Action OnTrapInfoDicUpdate;
    #endregion
    #region 掉落物品
    protected FDictionary dropItemDictionary = new FDictionary();
    public FDictionary DropItemDictionary
    {
        get
        {
            return dropItemDictionary;
        }
    }

    public DropItemInfo GetDropItemInfo(int _instanceID)
    {
        if (dropItemDictionary.ContainsKey(_instanceID))
        {
            return dropItemDictionary[_instanceID] as DropItemInfo;
        }
        return null;
    }

    public List<DropItemInfo> GetDropItemInfoList(int _oid)
    {
        List<DropItemInfo> list = new List<DropItemInfo>();
        foreach (DropItemInfo item in dropItemDictionary.Values)
        {
            if (item.DropFromServerInstanceID == _oid)
            {
                list.Add(item);
            }
        }
        return list;
    }



    /// <summary>
    /// 获得场景内属于对应ID对象的掉落物品列表 by黄洪兴
    /// </summary>
    /// <param name="_oid"></param>
    /// <returns></returns>
    public List<DropItemInfo> GetDropItemInfoListByOwner(int _oid)
    {
        List<DropItemInfo> list = new List<DropItemInfo>();
        foreach (DropItemInfo item in dropItemDictionary.Values)
        {
			if(item.OwnerID.Contains((uint)_oid))
            {
				DropItem dropItem = GameCenter.curGameStage.GetDropItem(item.ServerInstanceID);
				if(dropItem != null)
            		list.Add(item);
            }
        }
        return list;
    }

    /// <summary>
    /// 获得场景内属于主玩家象且符合自动拾取条件的物品列表 by黄洪兴
    /// </summary>
    /// <param name="_oid"></param>
    /// <returns></returns>
    public List<DropItemInfo> GetDropItemInfoListByLimit(MainPlayer _player)
    {
        List<DropItemInfo> list = new List<DropItemInfo>();
        List<DropItemInfo> targeList = new List<DropItemInfo>();
        foreach (DropItemInfo item in dropItemDictionary.Values)
        {
            if (item.OwnerID.Contains((uint)_player.id)&&item.DropSort!=506)
            {
                DropItem dropItem = GameCenter.curGameStage.GetDropItem(item.ServerInstanceID);
                if (dropItem != null)
                {
                    float distance = Vector3.Distance(_player.transform.position, dropItem.transform.position);
                    if(distance<15f)
                    list.Add(item);
                }
            }
        }
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Quality >= GameCenter.systemSettingMng.PickModel || list[i].DropSort > 12)
            {
                targeList.Add(list[i]);
            }
        }
        return targeList;
    }



    public static Action OnDropInfoListUpdate;
    public static Action OnAddDropItem;
    public static Action<DropItemInfo> OnDropItemEvent;
    #endregion
    #endregion
    /// <summary>
    /// 对象被删除的事件 by吴江
    /// </summary>
    public static Action<ObjectType, int> OnDelInterObj;
    /// <summary>
    /// 对象冒泡说话的事件
    /// </summary>
    public static Action<int, int> OnObjTalking;
    #endregion

    #region 协议传输
    #region S2C
    /// <summary>
    /// 成功切换场景
    /// </summary>
    /// <param name="_cmd"></param>
    protected void S2C_ChangeSceneBack(Pt _info)
    { 
        isChangingScene = false;
        GameCenter.instance.IsReConnecteding = false;
        GameCenter.curGameStage.myLastIP = Network.player.ipAddress;
        EnterSucceed = true;
        GameCenter.msgLoackingMng.UpdateSerializeList(EnterSceneSerlizeID, false);
        EnterSceneSerlizeID = 0;

        pt_scene_info_b106 pt = _info as pt_scene_info_b106;
        if (!GameCenter.mainPlayerMng.HasApplySubData)
        {
            GameCenter.mainPlayerMng.ApplySubData();
        }
        sceneID = (int)pt.scene;
        SceneRef refData = ConfigMng.Instance.GetSceneRef(sceneID);
        if (refData != null)
        {
            sceneType = refData.sort; 
        }
        InitSceneNPCInstances(sceneID);
        //重新切换场景请求一下宝箱数据 校准宝箱的开启时间
        GameCenter.royalTreasureMng.C2S_ReqRoyalBoxList();
    }
    /// <summary>
    /// 场景内新增对象 by吴江
    /// </summary>
    protected void S2C_OnSceneAddObject(Pt _info)
    {
        pt_scene_add_c002 pt = _info as pt_scene_add_c002;
        if (pt != null)
        {
            bool isInFight = sceneType != SceneType.CITY;
            //其他玩家 by吴江
            int count = pt.ply_list.Count;
            if (count > 0)
            {
                FDictionary equipInfoList = new FDictionary();
                for (int i = 0; i < pt.usr_equip_list.Count; i++)
                {
                    if (!equipInfoList.ContainsKey((int)pt.usr_equip_list[i].uid))
                    {
                        equipInfoList.Add((int)pt.usr_equip_list[i].uid, new List<int>());
                    }
//                    List<int> list = equipInfoList[(int)pt.usr_equip_list[i].uid] as List<int>;
//                    if (list != null) list.Add((int)pt.usr_equip_list[i].equip_id);
                }

                for (int i = 0; i < count; i++)
                {
                    scene_ply info = pt.ply_list[i];
                    if (!opcInfoDictionary.ContainsKey((int)info.pid))
                    {
						OtherPlayerInfo playerInfo = new OtherPlayerInfo(info, equipInfoList[(int)info.pid] as List<int>, isInFight);
                        //Debug.Log("AddObject:" + info.pid + ",speed:" + info.move_speed + ",ride_state:" + info.ride_state);
						opcInfoDictionary[(int)info.pid] = playerInfo;
						if(playerInfo.SlaLevel == 3)//红名玩家入场提示
							GameCenter.messageMng.AddClientMsg(432,new string[1]{playerInfo.Name});
                    }
                    else
                    {
                        OtherPlayerInfo opcInfo = opcInfoDictionary[(int)info.pid] as OtherPlayerInfo;
                        if (opcInfo != null)
                        {
                            opcInfo.UpdateHide(false, info.x, info.z);
                            OtherPlayer opc = GameCenter.curGameStage.GetOtherPlayer((int)info.pid);
                            if (opc != null)
                            {
                                opc.OnHideUpdate();
                            }
                            opcInfo.Update(equipInfoList[(int)info.pid] as List<int>, info);
                            //Debug.Log("UpdateObject:" + info.pid + ",speed:" + info.move_speed + ",ride_state:" + info.ride_state);
                        }
                    }
                }
                if (OnOPCInfoListUpdate != null)
                {
                    OnOPCInfoListUpdate();
                }
            }
            //怪物 by吴江
            count = pt.monster_list.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    scene_monster info = pt.monster_list[i];
                    if (!mobInfoDictionary.ContainsKey((int)info.mid))
                    {
						MonsterInfo monsterInfo = new MonsterInfo(info);
						mobInfoDictionary[(int)info.mid] = monsterInfo;
						AutoDailyDart(monsterInfo);
                    }
                    else
                    {
                        MonsterInfo mInfo = mobInfoDictionary[(int)info.mid] as MonsterInfo;
                        if (mInfo != null)
                        {
                            mInfo.UpdateHide(false, info.x, info.z);
                            Monster mob = GameCenter.curGameStage.GetMOB((int)info.mid);
                            if (mob != null)
                            {
                                mob.OnHideUpdate();
                            }
                            mInfo.Update(info);
                        }
                    }
                }
                if (OnMobInfoListUpdate != null)
                {
                    OnMobInfoListUpdate();
                }
            }
            count = pt.item_list.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    scene_item info = pt.item_list[i];
                    if (!sceneItemInfoDictionary.ContainsKey((int)info.iid))
                    {
                        sceneItemInfoDictionary[(int)info.iid] = new SceneItemInfo(info);
                    }
                    else
                    {
                        SceneItemInfo sInfo = sceneItemInfoDictionary[(int)info.iid] as SceneItemInfo;
                        if (sInfo != null)
                        {
                            sInfo.UpdateHide(false, info.x, info.z);
                            SceneItem item = GameCenter.curGameStage.GetSceneItem((int)info.iid);
                            if (item != null)
                            {
                                item.OnHideUpdate();
                            }
                            sInfo.Update(info);
                        }
                    }
                }
                if (OnSceneItemInfoListUpdate != null)
                {
                    OnSceneItemInfoListUpdate();
                }
            }
            count = pt.entourage_list.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    scene_entourage info = pt.entourage_list[i]; 
                    if ((int)info.owner == GameCenter.curMainPlayer.id) continue;
                    if (!entourageInfoDictionary.ContainsKey((int)info.eid))
                    {
                        entourageInfoDictionary[(int)info.eid] = new MercenaryInfo(info, isInFight);
                    }
                    else
                    {
                        MercenaryInfo eInfo = entourageInfoDictionary[(int)info.eid] as MercenaryInfo;
                        if (eInfo != null)
                        {
                            eInfo.UpdateHide(false, info.x, info.z);
                            OtherEntourage oe = GameCenter.curGameStage.GetOtherEntourage((int)info.eid);
                            if (oe != null)
                            {
                                oe.OnHideUpdate();
                            }
                            eInfo.Update(info);
                        }
                    }
                }
                if (OnEntourageInfoListUpdate != null)
                {
                    OnEntourageInfoListUpdate();
                }
            }
            count = pt.model_list.Count;
            if (count > 0)
            {
				
                for (int i = 0; i < count; i++)
                {
                    scene_model info = pt.model_list[i];
                    if (!modelInfoDictionary.ContainsKey((int)info.id))
                    {
                        modelInfoDictionary[(int)info.id] = new OtherPlayerInfo(info);
                    }
                    else
                    {
                        OtherPlayerInfo opcInfo = modelInfoDictionary[(int)info.id] as OtherPlayerInfo;
                        if (opcInfo != null)
                        {
                            opcInfo.UpdateHide(false, info.x, info.z);
                            Model opc = GameCenter.curGameStage.GetObject(ObjectType.Model, (int)info.id) as Model;
                            if (opc != null)
                            {
                                opc.OnHideUpdate();
                            }
                        }
                    }
                }
                if (OnModelInfoListUpdate != null)
                {
                    OnModelInfoListUpdate();
                }
			}
            count = pt.drop_item_list.Count;
            for (int i = 0; i < count; i++)
            {
                DropItemInfo info = new DropItemInfo(pt.drop_item_list[i], (int)pt.drop_item_list[i].from_id);
                dropItemDictionary.Add((int)pt.drop_item_list[i].id, info);
                if (OnDropItemEvent != null)
                {
                    OnDropItemEvent(info);
                }
            }
            if (OnDropInfoListUpdate != null)
            {
                OnDropInfoListUpdate();
            }
            if (count > 0)
            {
                if (OnAddDropItem != null)
                    OnAddDropItem();
            }
        }
    }


	/// <summary>
	/// 场景里面其他玩家信息变化
	/// </summary>
	/// <param name="_info">Info.</param>
	protected void S2C_OnSceneOPCUpdate(Pt _info)
	{
		pt_other_ply_info_d810 pt = _info as pt_other_ply_info_d810;
		if(pt != null)
		{
			for (int i = 0,max=pt.ply_list.Count; i < max; i++) {
				scene_ply info = pt.ply_list[i];
				if (!opcInfoDictionary.ContainsKey((int)info.pid))
				{
					Debug.LogError("服务端更新不在当前场景的玩家信息!");
				}
				else
				{
					OtherPlayerInfo opcInfo = opcInfoDictionary[(int)info.pid] as OtherPlayerInfo;
					if (opcInfo != null)
					{
						opcInfo.UpdateHide(false, info.x, info.z);
						OtherPlayer opc = GameCenter.curGameStage.GetOtherPlayer((int)info.pid);
						if (opc != null)
						{
							opc.OnHideUpdate();
						}
						opcInfo.Update(info);
                        //Debug.Log("OPCUpdate:" + info.pid + ",speed:" + info.move_speed+",name:"+info.name);
					}
				}
			}

            int count = pt.entourage_list.Count;
            if (count > 0)
            {
                bool isInFight = sceneType != SceneType.CITY;
                for (int i = 0; i < count; i++)
                {
                    scene_entourage info = pt.entourage_list[i];
                    //Debug.Log("id : " + info.eid + "  , petName : " + info.pet_name + "   , ownerid : " + info.owner + "   , ownerName : " + info.owner_name);
                    if ((int)info.owner == GameCenter.curMainPlayer.id) continue;
                    if (!entourageInfoDictionary.ContainsKey((int)info.eid))
                    {
                        entourageInfoDictionary[(int)info.eid] = new MercenaryInfo(info, isInFight);
                    }
                    else
                    {
                        MercenaryInfo eInfo = entourageInfoDictionary[(int)info.eid] as MercenaryInfo;
                        if (eInfo != null)
                        {
                            eInfo.UpdateHide(false, info.x, info.z);
                            OtherEntourage oe = GameCenter.curGameStage.GetOtherEntourage((int)info.eid);
                            if (oe != null)
                            {
                                oe.OnHideUpdate();
                            }
                            eInfo.Update(info);
                        }
                    }
                }
            }
		}
	}
    /// <summary>
    /// 场景对象离开视野,挂起
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_OnSceneHideObject(Pt _info)
    {
        pt_scene_hide_c020 pt = _info as pt_scene_hide_c020;
        if (pt != null)
        {
            for (int i = 0; i < pt.hide_list.Count; i++)
            {
                int instanceID = (int)pt.hide_list[i].id;
                ObjectType type = (ObjectType)pt.hide_list[i].sort;
                ActorInfo act = null;
                switch (type)
                {
                    case ObjectType.Player:
                        act = GetOPCInfo(instanceID);
                        break;
                    case ObjectType.MOB:
                        act = GetMobInfo(instanceID);
                        break;
                    case ObjectType.SceneItem:
                        act = GetSceneItemInfo(instanceID);
                        break;
                    case ObjectType.Entourage:
                        act = GetEntourageInfo(instanceID);
                        break;
                    case ObjectType.Model:
                        act = GetModelInfo(instanceID);
                        break;
					case ObjectType.DropItem:
						act = GetDropItemInfo(instanceID);
						break;
                    default:
                        break;
                }
                if (act != null) act.UpdateHide(true, act.ServerPos.x, act.ServerPos.y);
            }
        }
    }
    /// <summary>
    /// 场景内删除对象 by吴江
    /// </summary>
    protected void S2C_OnSceneDeleteObject(Pt _info)
    {
        pt_scene_dec_c003 pt = _info as pt_scene_dec_c003;
        if (pt != null)
        {
            int id = (int)pt.oid;
            if (mobInfoDictionary.ContainsKey(id))
            {
                MonsterInfo info = mobInfoDictionary[id] as MonsterInfo;
                if (info.IsAlive)
                {
                    Debug.Log("Catch:后台在怪物未死亡的情况下试图删除怪物! " + info.ServerInstanceID);
                }
                mobInfoDictionary.Remove(id);
                if (OnDelInterObj != null)
                {
                    OnDelInterObj(ObjectType.MOB, info.ServerInstanceID);
                }
            }
            else if (opcInfoDictionary.ContainsKey(id))
            {
                OtherPlayerInfo info = opcInfoDictionary[id] as OtherPlayerInfo;
                opcInfoDictionary.Remove(id);
                if (OnDelInterObj != null)
                {
                    OnDelInterObj(ObjectType.Player, info.ServerInstanceID);
                }
            }
            else if (sceneItemInfoDictionary.ContainsKey(id))
            {
                sceneItemInfoDictionary.Remove(id);
                if (OnDelInterObj != null)
                {
                    OnDelInterObj(ObjectType.SceneItem, id);
                }
            }
            else if (entourageInfoDictionary.ContainsKey(id))
            {
                entourageInfoDictionary.Remove(id);
                if (OnDelInterObj != null)
                {
                    OnDelInterObj(ObjectType.Entourage, id);
                }
            }
            else if (modelInfoDictionary.ContainsKey(id))
            {
                modelInfoDictionary.Remove(id);
                if (OnDelInterObj != null)
                {
                    OnDelInterObj(ObjectType.Model, id);
                }
			}else if(dropItemDictionary.ContainsKey(id))
			{
                dropItemDictionary.Remove(id);
                if (OnDelInterObj != null)
                {
					OnDelInterObj(ObjectType.DropItem,id);
                }
			}
        }
    }
    /// <summary>
    /// 怪物改变从属
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_OnMonsterChangeOwner(Pt _info)
    {
        pt_monster_affiliation_c01b pt = _info as pt_monster_affiliation_c01b;
        if (pt != null)
        {
            for (int i = 0; i < pt.monster_affiliation.Count; i++)
            {
                st.net.NetBase.monster_affiliation a = pt.monster_affiliation[i];
                MonsterInfo m = GetMobInfo((int)a.monster_id);
                if (m != null)
                {
                    m.UpdateOwner((int)a.owner_id);
                }
            }
        }
    }
    /// <summary> 
    /// 场景内对象移动 by吴江
    /// </summary>
    /// <param name="_cmd"></param>
    protected void S2C_ObjectMove(Pt _info)
    {
        pt_scene_move_c001 msg = _info as pt_scene_move_c001;
        if (msg == null) return;
        SmartActor obj = null;
        switch ((ObjectType)msg.obj_sort)
        {
            case ObjectType.Player:
                obj = GameCenter.curGameStage.GetObject(ObjectType.Player, (int)msg.oid) as SmartActor;
                break;
            case ObjectType.MOB:
                obj = GameCenter.curGameStage.GetObject(ObjectType.MOB, (int)msg.oid) as SmartActor;
                break;
            case ObjectType.Entourage:
                obj = GameCenter.curGameStage.GetObject(ObjectType.Entourage, (int)msg.oid) as SmartActor;
                break;
        }
        if (obj == null)
        {
            GameSys.Log("找不到ID为:" + msg.oid + "的场景对象!");
            return;
        }
        List<Vector3> path = new List<Vector3>();
        float x = msg.point_list[0].x;
      //  float y = msg.point_list[0].y;
        float z = msg.point_list[0].z;
        float dir = msg.dir;
        bool isPathMove = msg.is_path_move == (byte)1;


        Vector3 clientCurPos = obj.transform.position;
        float distance = Mathf.Max(Mathf.Abs(clientCurPos.x - x), Mathf.Abs(clientCurPos.z - z));
        obj.CancelAbilityMove(isPathMove);
      //  Debug.Log("移动协议:" + obj.name +  "前台当前位置: " + obj.transform.position + "后台当前位置：" + x + " , " + z + " , 路径长度 = " + msg.point_list.Count);
        if (distance > 2.0f  || (distance > 0.5f && msg.point_list.Count == 0))
        {
            //Debug.LogError("捕获移动异常！");
            GameCenter.curGameStage.PlaceGameObjectFromServer(obj, x, z, (int)dir);
        }
        else
        {
            obj.FaceTo(dir);
        }
        for (int i = 0; i < msg.point_list.Count; i++)
        {
            Vector3 pos = new Vector3(msg.point_list[i].x, msg.point_list[i].y, msg.point_list[i].z);
            pos = ActorMoveFSM.LineCast(pos, false);
            path.Add(pos);
        }
        //string ddd = string.Empty;
        //for (int i = 0; i < path.Count; i++)
        //{
        //    ddd += ("(" + path[i].x + "," + path[i].y + "," + path[i].z + ")");
        //}
      //  Debug.logger.Log("S2C_ObjectMove " + ddd + " , isPathmove = " + (msg.is_path_move == (byte)1));
        obj.MoveTo(path.ToArray(), dir, true);
    }
    /// <summary> 
    /// 场景内其它玩家队伍状态改变 by吴江
    /// </summary>
    /// <param name="_cmd"></param>
    protected void S2C_PlayerTeamChange(Pt _info)
    {
        pt_scene_usr_team_chg_c018 msg = _info as pt_scene_usr_team_chg_c018;
        if (msg == null) return;
        OtherPlayerInfo info = GetOPCInfo((int)msg.uid);
        if (info != null)
        {
            info.Update(msg);
        }
    }
    /// <summary>
    /// 场景内其他玩家的阵营改变
    /// </summary>
    /// <param name="_info"></param>
//    protected void S2C_PlayerCampChange(Pt _info)
//    {
//        
//        pt_update_camp_d21d msg = _info as pt_update_camp_d21d;
//        if (msg == null) return;
//        OtherPlayerInfo info = GetOPCInfo((int)msg.uid);
//        if (info != null)
//        {
//            info.SetCamp(msg.camp);
//        }
//    }

    /// <summary>
    /// 玩家更新装备 by吴江
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_ActorUpdateEquip(Pt _info)
    {
        pt_reloading_d101 msg = _info as pt_reloading_d101;
        if (msg != null)
        {
            if (msg.uid == GameCenter.curMainPlayer.id)
            {
                GameCenter.mainPlayerMng.MainPlayerInfo.Update(msg.equip_id_state_list);
            }
            else
            {
                OtherPlayerInfo opc = GetOPCInfo((int)msg.uid);
                if (opc != null)
                {
                    opc.Update(msg.equip_id_state_list);
                }
            }
        }
    }
    /// <summary>
    /// 增加掉落物品
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_AddDropItem(Pt _info)
    {
        pt_scene_drop_c011 msg = _info as pt_scene_drop_c011;
        if (msg == null) return;
        for (int i = 0; i < msg.drop_list.Count; i++)
        {
            DropItemInfo info = new DropItemInfo(msg.drop_list[i], (int)msg.oid);
            dropItemDictionary.Add((int)msg.drop_list[i].id, info);
            if (OnDropItemEvent != null)
            {
                OnDropItemEvent(info);
            }
        }
        if (OnDropInfoListUpdate != null)
        {
            OnDropInfoListUpdate();
        }
        if (OnAddDropItem != null)
        {
            OnAddDropItem();
        }
    }
    /// <summary>
    /// 删除掉落物品
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_PickUpDropItem(Pt _info)
    {
        pt_scene_pickup_c012 msg = _info as pt_scene_pickup_c012;
        if (msg == null) return;
        for (int i = 0; i < msg.pickup_list.Count; i++)
        {
            if (dropItemDictionary.ContainsKey((int)msg.pickup_list[i].id))
            {
                if (msg.pickup_list[i].deleted_state == 0)
                {
                    DropItemInfo info = dropItemDictionary[(int)msg.pickup_list[i].id] as DropItemInfo;
                    if (info != null)
                    {
                        info.IsTimeOut = true;
                    }
					DropItem dropItem = GameCenter.curGameStage.GetDropItem((int)msg.pickup_list[i].id);
					if (dropItem != null )//&& dropItem.OwnerID.Contains((uint)GameCenter.curMainPlayer.id))
					{
						GameCenter.curMainPlayer.DoPickUpEffect(dropItem.transform.position);
					}
                }
//                dropItemDictionary.Remove((int)msg.pickup_list[i].id);//应后台需求,清空对象操作放到c003中做 by邓成
//                if (OnDelInterObj != null)
//                {
//                    OnDelInterObj(ObjectType.DropItem, (int)msg.pickup_list[i].id);
//                }
            }
        }
    }
    /// <summary>
    /// 玩家传送
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_TransformPlayer(Pt _info)
    {
        pt_scene_transform_c013 msg = _info as pt_scene_transform_c013;
        if (msg != null)
        {
            string animaName = string.Empty;
            SceneItemDisRef instance = ConfigMng.Instance.GetSceneItemDisRef((int)msg.type);
            if (instance != null)
            {
                SceneItemRef refData = ConfigMng.Instance.GetSceneItemRef(instance.sceneItemId);
                if (refData != null)
                {
                    animaName = refData.afterAction;
                }
            }
            switch ((ObjectType)msg.obj_sort)
            {
                case ObjectType.Unknown:
                    break;
                case ObjectType.Player:
                    if ((int)msg.oid == GameCenter.curMainPlayer.id)
                    {
                        GameCenter.curMainPlayer.CancelCommands();
                        GameCenter.curMainPlayer.CancelAbility();
                        GameCenter.curMainPlayer.LerpToPos(new Vector3(msg.x, msg.y, msg.z), msg.time, animaName);
                    }
                    else
                    {
                        OtherPlayer player = GameCenter.curGameStage.GetOtherPlayer((int)msg.oid);
                        if (player != null)
                        {
                            player.CancelAbility();
                            player.CancelAbilityMove(true);
                            player.LerpToPos(new Vector3(msg.x, msg.y, msg.z), msg.time, animaName);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
    /// <summary>
    /// 增加弹道表现 by吴江
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_AddBallisticCurve(Pt _info)
    {
        if (!EnterSucceed) return;
         pt_scene_add_arrow_c014 msg = _info as pt_scene_add_arrow_c014;
         if (msg != null)
         {
             bool hasNew = false;
             for (int i = 0; i < msg.list.Count; i++)
             {
                 AbilityBallisticCurveInfo info = new AbilityBallisticCurveInfo(msg.list[i]);
                 if ((info.StartPos - GameCenter.curMainPlayer.transform.position).sqrMagnitude > 40.0f * 40.0f) //40码以外距离的实时弹道就不显示了  by吴江
                 {
                     continue;
                 }
                 hasNew = true;
                 abilityBallisticCurveInfoDictionary[info.InstanceID] = info;
             }
             if (OnAbilityBallisticCurveUpdate != null && hasNew)
             {
                 OnAbilityBallisticCurveUpdate(-1);
             }
         }
    }
    /// <summary>
    /// 删除弹道表现 by吴江
    /// </summary>
    /// <param name="_cmd"></param>
    protected void S2C_DeleteBallisticCurve(Pt _info)
    {
        pt_scene_delete_arrow_c016 msg = _info as pt_scene_delete_arrow_c016;
        if (msg != null)
        {
            if (abilityBallisticCurveInfoDictionary.ContainsKey((int)msg.aid))
            {
                abilityBallisticCurveInfoDictionary.Remove((int)msg.aid);
                if (OnAbilityBallisticCurveUpdate != null)
                {
                    OnAbilityBallisticCurveUpdate((int)msg.aid);
                }
            }
        }
    }
    /// <summary>
    /// 增加弹道表现 by吴江
    /// </summary>
    /// <param name="_info"></param>
    public void C2C_AddBallisticCurve(AbilityInstance _info)
    {
        if (!EnterSucceed) return;
        if (_info.IsDummy) return;
        AbilityBallisticCurveInfo info = new AbilityBallisticCurveInfo(_info);
        abilityBallisticCurveInfoDictionary[info.InstanceID] = info;
        if (OnAbilityBallisticCurveUpdate != null)
        {
            OnAbilityBallisticCurveUpdate(-1);
        }
    }
    /// <summary>
    /// 删除弹道表现 by吴江
    /// </summary>
    /// <param name="_cmd"></param>
    public void C2C_OnDeleteAbilityBallisticLockCurve(int _id)
    {
        int id = _id;
        if (abilityBallisticCurveInfoDictionary.ContainsKey(id))
        {
            AbilityBallisticCurveInfo info = abilityBallisticCurveInfoDictionary[id] as AbilityBallisticCurveInfo;
            if (info != null && info.IsLock)
            {
                info.OnReached();
            }
            abilityBallisticCurveInfoDictionary.Remove(id);
        }
        if (OnAbilityBallisticCurveUpdate != null)
        {
            OnAbilityBallisticCurveUpdate(id);
        }
    }
    /// <summary>
    /// 清除所有的弹道数据 by吴江
    /// </summary>
    public void C2C_CleanAllAbilityBallisticCurve()
    {
        if (OnAbilityBallisticCurveUpdate != null)
        {
            foreach (int id in abilityBallisticCurveInfoDictionary.Keys)
            {
                OnAbilityBallisticCurveUpdate(id);
            }
        }
        abilityBallisticCurveInfoDictionary.Clear();
    }
    /// <summary>
    /// 添加陷阱 by吴江
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_AddTrap(Pt _info)
    {
        pt_scene_add_trap_c015 msg = _info as pt_scene_add_trap_c015;
        if (msg != null)
        {
            for (int i = 0; i < msg.list.Count; i++)
            {
                trapInfoDictionary[(int)msg.list[i].did] = new TrapInfo(msg.list[i]);
            }
            if (OnTrapInfoDicUpdate != null)
            {
                OnTrapInfoDicUpdate();
            }
        }
    }
    /// <summary>
    /// 删除陷阱 by吴江
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_DeleteTrap(Pt _info)
    {
        pt_scene_delete_trap_c017 msg = _info as pt_scene_delete_trap_c017;
        if (trapInfoDictionary.ContainsKey((int)msg.tid))
        {
            trapInfoDictionary.Remove((int)msg.tid);
            if (OnDelInterObj != null)
            {
                OnDelInterObj(ObjectType.Trap, (int)msg.tid);
            }
        }
    }
    /// <summary>
    /// 对象冒泡说话
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_ObjTalking(Pt _info)
    {
        pt_say_notify_d202 msg = _info as pt_say_notify_d202;
        if (msg != null)
        {
            if (OnObjTalking != null)
            {
                OnObjTalking((int)msg.target_id, (int)msg.say_id);
            }
        }
    }
    /// <summary>
    /// 确认开始读条
    /// </summary>
    protected void S2C_StartCollect(Pt _info)
    {
        pt_progress_bar_begin_d203 msg = _info as pt_progress_bar_begin_d203;
        if (msg == null) return;
        SustainRef item = ConfigMng.Instance.GetSustainRef((int)msg.type);
        if(item == null) return;
        if (msg.oid == GameCenter.curMainPlayer.id)
        {
            GameCenter.mainPlayerMng.MainPlayerInfo.StartCollect(item);
        }
        else if (GetOPCInfo((int)msg.oid) != null)
        {
            OtherPlayerInfo opc = GetOPCInfo((int)msg.oid);
            if (opc == null) return;
            opc.StartCollect(item);
        }
        else
        {
            MonsterInfo monster = GetMobInfo((int)msg.oid);
            if (monster == null) return;
            monster.StartCollect(item);
            
        }
    }

    /// <summary>
    /// 停止读条
    /// </summary>
    protected void S2C_StopCollect(Pt _info)
    {
        pt_progress_bar_end_d204 msg = _info as pt_progress_bar_end_d204;
        if (msg == null) return;
        if (msg.oid == GameCenter.curMainPlayer.id)
        {
            GameCenter.mainPlayerMng.MainPlayerInfo.EndCollect();
			GameCenter.mainPlayerMng.IsWaitTouchSceneItemMsg = false;
        }
        else if (GetOPCInfo((int)msg.oid) != null)
        {
            OtherPlayerInfo opc = GetOPCInfo((int)msg.oid);
            if (opc == null) return;
            opc.EndCollect();
        }
        else
        {
            MonsterInfo monster = GetMobInfo((int)msg.oid);
            if (monster == null) return;
            monster.EndCollect();
        }
    }

    /// <summary>
    /// Boss最高伤害者 add 鲁家旗
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_UpdateMonsterOwner(Pt _info)
    {
        pt_update_monster_owner_c010 msg = _info as pt_update_monster_owner_c010;
        if (msg != null)
        {
          if(OnMobChange != null)
              OnMobChange((int)msg.owner_id);
        }
    }

    /// <summary>
    /// 场景中其他玩家跳跃
    /// </summary>
    protected void S2C_OnOtherPlayerJump(Pt _info)
    {
        pt_scene_jump_d20b msg = _info as pt_scene_jump_d20b;
        if (msg == null) return;
        OtherPlayerInfo opc = GetOPCInfo((int)msg.pid);
        if (opc == null) return;
        opc.Jump();
        
    }
    /// <summary>
    /// 属性发生改变的协议
    /// </summary>
    /// <param name="_info"></param>
    //protected void S2C_OnBaseValueChange(Pt _info)
    //{
    //    pt_update_base_d01d pt = _info as pt_update_base_d01d;
    //    if (pt != null)
    //    {
    //        int instanceID = (int)pt.uid;
    //        PlayerBaseInfo pInfo = null;
    //        if (GameCenter.curMainPlayer != null && instanceID == GameCenter.curMainPlayer.id)
    //        {
    //            pInfo = GameCenter.mainPlayerMng.MainPlayerInfo;
    //        }
    //        else
    //        {
    //           pInfo = GetOPCInfo(instanceID);
    //        }
    //        if(pInfo != null)
    //        {
    //            for (int i = 0; i < pt.property_list.Count; i++)
    //            {
    //               pInfo.ChangeValue(pt.property_list[i]);
    //            }
    //            for (int i = 0; i < pt.property64_list.Count; i++)
    //            {
    //                pInfo.ChangeValue(pt.property64_list[i]);
    //            }
    //        }
    //        if (OnBaseValueChange != null)
    //            OnBaseValueChange();

    //    }



    //}
    /// <summary>
    /// 玩家头顶信息更新
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_OnPlayerHeadTextUpdate(Pt _info)
    {
        Debug.Log("--玩家头顶信息更新--");
        pt_update_scene_usr_data_d222 msg = _info as pt_update_scene_usr_data_d222;
        if (msg != null)
        {
            OtherPlayerInfo opc = GetOPCInfo((int)msg.uid);
            if (opc != null)
            {
                switch ((HeadtextUpdateType)msg.sort)
                {
                    case HeadtextUpdateType.Military:
                        Debug.Log("--Military--:" + (int)msg.idata);
                        if(opc.OnBaseUpdate!=null)
                        opc.OnBaseUpdate(ActorBaseTag.MilitaryLv, msg.idata, false);
                        break;
                    case HeadtextUpdateType.GuildName:
                        Debug.Log("--GuildName--:" + msg.sdata);
                        opc.UpdateGuildName(msg.sdata);
                        break;
                    case HeadtextUpdateType.Name:
                        break;
                    default:
                        break;
                }
            }
        }
    }

    
	protected void S2C_OnWayPointUpdate(Pt _info)
	{
		pt_scene_route_c00a pt = _info as pt_scene_route_c00a;
		if(pt != null)
		{
			wayPoint = new Vector3(pt.x,pt.y,pt.z);
			Debug.Log("S2C_OnWayPointUpdate:"+wayPoint);
			if(OnWayPointUpdateEvent != null)
				OnWayPointUpdateEvent();
		}
	}
	#endregion

    #region C2S
    /// <summary>
    /// 成功进入场景 by吴江
    /// </summary>
    public void C2S_EnterSceneSucceed()
    {
        EnterSucceed = false;
        pt_load_scene_finish_b005 msg = new pt_load_scene_finish_b005();
        msg.scene = (uint)GameCenter.mainPlayerMng.MainPlayerInfo.SceneID;
        EnterSceneSerlizeID = (int)NetMsgMng.CreateNewSerializeID();
        GameCenter.msgLoackingMng.UpdateSerializeList(EnterSceneSerlizeID, true);
        GameCenter.uIMng.GenGUI(GUIType.PANELLOADING, true);
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 拾取物品
    /// </summary>
    /// <param name="_itemServerData"></param>
    public void C2S_PickUpDropItem(int _serverInstanceID, bool _decompose)
    {
        pt_scene_pickup_c012 msg = new pt_scene_pickup_c012();
        msg.pickup_list = new List<pickup_des>();
        pickup_des des = new pickup_des();
        des.id = (uint)_serverInstanceID;
        des.state = _decompose ? (uint)1 : 0;
        msg.pickup_list.Add(des);
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 进入到特殊的区域,通知后台添加buff by邓成
    /// </summary>
    /// <param name="_areaID"></param>
    public void C2S_EnterSpecialArea(int _areaID,bool _enter)
    {
        //Debug.Log("C2S_EnterSpecialArea _areaID:" + _areaID);
        pt_req_mountain_flames_add_buff_c116 msg = new pt_req_mountain_flames_add_buff_c116();
        msg.id = _areaID;
        msg.in_and_out = _enter ? 1 : 2;
        NetMsgMng.SendMsg(msg);
    }
    #endregion
    #endregion

    #region 辅助逻辑

    public static Action OnCleanAll;

    public void CleanAll()
    {
        opcInfoDictionary.Clear();
        npcInfoDictionary.Clear();
        mobInfoDictionary.Clear();
        entourageInfoDictionary.Clear();
        sceneItemInfoDictionary.Clear();
        abilityBallisticCurveInfoDictionary.Clear();
        trapInfoDictionary.Clear();
        dropItemDictionary.Clear();
        modelInfoDictionary.Clear();
        if (OnCleanAll != null)
        {
            OnCleanAll();
        }
    }

    public void UpdateSceneNPCInstance()
    {
        int sceneID = GameCenter.curGameStage.SceneID;
        FDictionary dic = ConfigMng.Instance.GetSceneNPCRefTable;
        List<int> npcList = new List<int>();
        bool hasAddNPC = false;
        foreach (SceneNPCRef npc in dic.Values)
        {
            if (!npc.issmartNPC)
            {
                if (!npcList.Contains(npc.id))
                {
                    npcList.Add(npc.id);
                }
                continue;
            }
            foreach (var item in npc.actionConfig)
            {
                NPCAIRef ai = ConfigMng.Instance.GetNPCAIRef(item);
                if (ai != null)
                {
                    if (ai.scene == sceneID)
                    {
                        TaskInfo task = GameCenter.taskMng.GetTaskInfo(ai.task);
                        if (task != null)
                        {
                            if (task.SerializeID > ai.startStep && task.SerializeID < ai.overStep)
                            {
                                if (!npcList.Contains(npc.id))
                                {
                                    npcList.Add(npc.id);
                                }
                                if (!npcInfoDictionary.ContainsKey(npc.id))
                                {
                                    npcInfoDictionary.Add(npc.id, new NPCInfo(npc, ai));
                                    hasAddNPC = true;
                                }
                                continue;
                            }
                            if (task.SerializeID == ai.startStep && CheckState(true, task.TaskState, (TaskStateType)ai.startStepTime))
                            {
                                if (!npcList.Contains(npc.id))
                                {
                                    npcList.Add(npc.id);
                                }
                                if (!npcInfoDictionary.ContainsKey(npc.id))
                                {
                                    npcInfoDictionary.Add(npc.id, new NPCInfo(npc, ai));
                                    hasAddNPC = true;
                                }
                                continue;
                            }
                            if (task.SerializeID == ai.overStep && CheckState(false, task.TaskState, (TaskStateType)ai.overStepTime))
                            {
                                if (!npcList.Contains(npc.id))
                                {
                                    npcList.Add(npc.id);
                                }
                                if (!npcInfoDictionary.ContainsKey(npc.id))
                                {
                                    npcInfoDictionary.Add(npc.id, new NPCInfo(npc, ai));
                                    hasAddNPC = true;
                                }
                                continue;
                            }
                        }
                    }
                }
            }
        }
        List<int> needDelete = new List<int>();
        foreach (int id in npcInfoDictionary.Keys)
        {
            if (!npcList.Contains(id))
            {
                needDelete.Add(id);
            }
        }
        for (int i = 0; i < needDelete.Count; i++)
        {
            npcInfoDictionary.Remove(needDelete[i]);
            if (OnDelInterObj != null)
            {
                OnDelInterObj(ObjectType.NPC, needDelete[i]);
            }
        }
        if (hasAddNPC && OnNPCInfoListUpdate != null)
        {
            OnNPCInfoListUpdate();
        }
    }

	/// <summary>
	/// 设置场景动画的玩家信息（增加删除），在结束场景动画的时候一定要对这些数据进行处理  by吴江
	/// </summary>
	public bool SetSceneAnimaOPCInfo(bool _add, OtherPlayerInfo _info)
	{
		if (_add)
		{
			if (!OPCInfoDictionary.ContainsKey(_info.ServerInstanceID))
			{
				OPCInfoDictionary[_info.ServerInstanceID] = _info;
				if (OnOPCInfoListUpdate != null)
				{
					OnOPCInfoListUpdate();
				}
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			if (OPCInfoDictionary.ContainsKey(_info.ServerInstanceID))
			{
				OPCInfoDictionary[_info.ServerInstanceID] = null;
				OPCInfoDictionary.Remove(_info.ServerInstanceID);
				if (OnOPCInfoListUpdate != null)
				{
					OnOPCInfoListUpdate();
				}
				return true;
			}
			else
			{
				return false;
			}
		}
	}


	/// <summary>
	/// 设置场景动画的NPC信息（增加删除），在结束场景动画的时候一定要对这些数据进行处理  by吴江
	/// </summary>
	public bool SetSceneAnimaNPCInfo(bool _add,NPCInfo _info)
	{
		if (_add)
		{
			if (!NPCInfoDictionary.ContainsKey(_info.ServerInstanceID))
			{
				NPCInfoDictionary[_info.ServerInstanceID] = _info;
				if (OnNPCInfoListUpdate != null)
				{
					OnNPCInfoListUpdate();
				}
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			if (NPCInfoDictionary.ContainsKey(_info.ServerInstanceID))
			{
				NPCInfoDictionary[_info.ServerInstanceID] = null;
				NPCInfoDictionary.Remove(_info.ServerInstanceID);
				if (OnNPCInfoListUpdate != null)
				{
					OnNPCInfoListUpdate();
				}
				return true;
			}
			else
			{
				return false;
			}
		}
	}

	/// <summary>
	/// 设置场景动画的怪物信息（增加删除），在结束场景动画的时候一定要对这些数据进行处理  by吴江
	/// </summary>
	public bool SetSceneAnimaMonsterInfo(bool _add, MonsterInfo _info)
	{
		if (_add)
		{
			if (!MobInfoDictionary.ContainsKey(_info.ServerInstanceID))
			{
				MobInfoDictionary[_info.ServerInstanceID] = _info;
				if (OnMobInfoListUpdate != null)
				{
					OnMobInfoListUpdate();
				}
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			if (MobInfoDictionary.ContainsKey(_info.ServerInstanceID))
			{
				MobInfoDictionary[_info.ServerInstanceID] = null;
				MobInfoDictionary.Remove(_info.ServerInstanceID);
				if (OnMobInfoListUpdate != null)
				{
					OnMobInfoListUpdate();
				}
				return true;
			}
			else
			{
				return false;
			}
		}
	}    

	public void CleanSceneAnimObject(List<MonsterInfo> _mobList,List<NPCInfo> _npcList,List<OtherPlayerInfo> _opcList)
	{
		foreach (var item in _mobList)
		{
			if (MobInfoDictionary.ContainsKey(item.ServerInstanceID))
			{
				MobInfoDictionary[item.ServerInstanceID] = null;
				MobInfoDictionary.Remove(item.ServerInstanceID);
			}
		}
		_mobList.Clear();
		if (OnMobInfoListUpdate != null)
		{
			OnMobInfoListUpdate();
		}

		foreach (var item in _npcList)
		{
			if (NPCInfoDictionary.ContainsKey(item.ServerInstanceID))
			{
				NPCInfoDictionary[item.ServerInstanceID] = null;
				NPCInfoDictionary.Remove(item.ServerInstanceID);
			}
		}
		_npcList.Clear();
		if (OnOPCInfoListUpdate != null)
		{
			OnOPCInfoListUpdate();
		}

		foreach (var item in _opcList)
		{
			if (OPCInfoDictionary.ContainsKey(item.ServerInstanceID))
			{
				OPCInfoDictionary[item.ServerInstanceID] = null;
				OPCInfoDictionary.Remove(item.ServerInstanceID);
			}
		}
		_opcList.Clear();
		if (OnOPCInfoListUpdate != null)
		{
			OnOPCInfoListUpdate();
		}
	}

    public void InitSceneNPCInstances(int _sceneID)
    {
        npcInfoDictionary.Clear();
        FDictionary dic = ConfigMng.Instance.GetSceneNPCRefTable;
        foreach (SceneNPCRef npc in dic.Values)
        {
            foreach (var item in npc.actionConfig)
            {
                NPCAIRef ai = ConfigMng.Instance.GetNPCAIRef(item);
                if (ai != null)
                {
                    if (ai.scene == _sceneID)
                    {
						if (!npcInfoDictionary.ContainsKey(npc.id))
                        {
                            npcInfoDictionary.Add(npc.id, new NPCInfo(npc, ai));
                        }
//                        TaskInfo task = GameCenter.taskMng.GetTaskInfo(ai.task);
//                        if (task != null)
//                        {
//                            if (task.SerializeID > ai.startStep && task.SerializeID < ai.overStep)
//                            {
//                                if (!npcInfoDictionary.ContainsKey(npc.id))
//                                {
//                                    npcInfoDictionary.Add(npc.id, new NPCInfo(npc, ai));
//                                }
//                                continue;
//                            }
//                            if (task.SerializeID == ai.startStep && CheckState(true, task.TaskState, (TaskStateType)ai.startStepTime))
//                            {
//                                if (!npcInfoDictionary.ContainsKey(npc.id))
//                                {
//                                    npcInfoDictionary.Add(npc.id, new NPCInfo(npc, ai));
//                                }
//                                continue;
//                            }
//                            if (task.SerializeID == ai.overStep && CheckState(false, task.TaskState, (TaskStateType)ai.overStepTime))
//                            {
//                                if (!npcInfoDictionary.ContainsKey(npc.id))
//                                {
//                                    npcInfoDictionary.Add(npc.id, new NPCInfo(npc, ai));
//                                }
//                                continue;
//                            }
//                        }
                    }
                }
            }
        }
        if (npcInfoDictionary.Count > 0 && OnNPCInfoListUpdate != null)
        {
            OnNPCInfoListUpdate();
        }
    }

    protected bool isInBuffArea = false;
    protected int enterBuffAreaId = 0;
    public void EnterBuffArea(int _areaBuffID, bool _enter, int _tip)
    {
        if (isInBuffArea != _enter)
        {
            if (_enter)//进buff区域
            {
                if (_tip > 0) GameCenter.messageMng.AddClientMsg(_tip);
                enterBuffAreaId = _areaBuffID;
                isInBuffArea = true;
                C2S_EnterSpecialArea(_areaBuffID,true);
                //Debug.Log("进入buff区域areaBuffID:" + _areaBuffID);
            }
            else
            {
                if (enterBuffAreaId != 0 && enterBuffAreaId == _areaBuffID)//出buff区域
                {
                    if (_tip > 0) GameCenter.messageMng.AddClientMsg(_tip);
                    enterBuffAreaId = 0;
                    isInBuffArea = false;
                    C2S_EnterSpecialArea(_areaBuffID,false);//退出的时候这样发
                    //Debug.Log("退出buff区域areaBuffID:" + _areaBuffID);
                }
            }
        }
    }
    #endregion


    public enum HeadtextUpdateType
    {
        /// <summary>
        /// 军衔
        /// </summary>
        Military = 1,
        /// <summary>
        /// 工会名称 
        /// </summary>
        GuildName = 2,
        /// <summary>
        /// 名字
        /// </summary>
        Name = 3,
        /// <summary>
        /// 称号 
        /// </summary>
        Titile = 4,
    }


    protected bool CheckState(bool _start, TaskStateType _curState, TaskStateType _needState)
    {
        if (_start)
        {
            switch (_needState)
            {
                case TaskStateType.Process:
                    return _curState != TaskStateType.UnTake;
                case TaskStateType.UnTake:
                    return true;
                case TaskStateType.Finished:
                    return _curState == TaskStateType.Finished;
            }
        }
        else
        {
            switch (_needState)
            {
                case TaskStateType.Process:
                    return _curState != TaskStateType.Finished;
                case TaskStateType.UnTake:
                    return _curState == TaskStateType.UnTake;
                case TaskStateType.Finished:
                    return true;
            }
        }
        return false;
    }
}