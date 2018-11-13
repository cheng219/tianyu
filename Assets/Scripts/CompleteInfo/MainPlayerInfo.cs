//====================================
//作者：吴江
//日期：2015/5/29
//用途：主玩家数据层对象（Info结尾的类名都为数据层对象，包含 服务端数据  客户端静态数据   访问器 三部分）
//=====================================




using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;


public class MainPlayerData : PlayerBaseData
{

    public int sceneID;
    public int guideID;
    public int vipLev;
	public int guildID;

    public MainPlayerData(pt_usr_info_b102 _info)
    {
        serverInstanceID = (int)_info.id;
        name = _info.name;
        prof = (int)_info.prof;
        camp = (int)_info.camp;
        guildName = _info.guild_name;
		titleID=(int)_info.title_id;
        vipLev = (int)_info.vip_lev;
		guildID = (int)_info.guild_id;
        RefineRef rr = ConfigMng.Instance.GetRefineRef(_info.magic_weapon_id, _info.magic_strength_lev, _info.magic_strength_star);
        if (rr != null)
        {
            magicWeaponID = rr.model;
        }
		slaValue = (int)_info.sla;
        for (int i = 0; i < _info.property_list.Count; i++)
        {
            propertyValueDic[(ActorPropertyTag)_info.property_list[i].type] = (int)_info.property_list[i].data;
        }
		baseValueDic[ActorBaseTag.SLAVALUE] = (ulong)slaValue;
        baseValueDic[ActorBaseTag.Level] = _info.level;
        baseValueDic[ActorBaseTag.CurHP] = _info.cur_hp;
        baseValueDic[ActorBaseTag.CurMP] = _info.cur_mp;
        baseValueDic[ActorBaseTag.FightValue] = (ulong)_info.fiight_score;
        baseValueDic[ActorBaseTag.Exp] = _info.exp;
		for (int i = 0,max=_info.model_clothes_id.Count; i < max; i++) {
			equipTypeList.Add((int)_info.model_clothes_id[i]);
		}
		//_info.equip_id_list包含玩家装备及时装中的武器
        for (int i = 0; i < _info.equip_id_list.Count; i++)
        {
            equipTypeList.Add((int)_info.equip_id_list[i]);
        }
        for (int i = 0; i < _info.resource_list.Count; i++)
        {
            baseValueDic[(ActorBaseTag)_info.resource_list[i].resource_type] = _info.resource_list[i].resource_val;
        }
    }
}



/// <summary>
/// 主玩家数据层对象（Info结尾的类名都为数据层对象，包含 服务端数据  客户端静态数据   访问器 三部分） by吴江
/// </summary>
public class MainPlayerInfo : PlayerBaseInfo
{

    /// <summary>
    /// 服务端数据
    /// </summary>
    protected new MainPlayerData serverData
    {
        get { return base.serverData as MainPlayerData; }
        set
        {
            base.serverData = value;
        }
    }
	
	 /// <summary>
    /// 战斗属性发生改变的事件,抛出差值
    /// </summary>
    public System.Action<ActorPropertyTag, int> OnPropertyDiffUpdate;
	
	/// <summary>
    /// 基础属性数据发生改变的事件,抛出差值
    /// </summary>
    public System.Action<ActorBaseTag, int,bool> OnBaseDiffUpdate;
    /// <summary>
    /// 构造  by吴江
    /// </summary>
    /// <param name="_actorData"></param>
    public MainPlayerInfo(pt_usr_info_b102 _info)
    {
        serverData = new MainPlayerData(_info);
        if (_info.cur_hp <= 0) //add by黄洪兴 角色有可能死亡状态登录
            isAlive = false;
        List<int> defaultEquipList = RefData.defaultEquipList;
        foreach (var item in defaultEquipList)
        {
            EquipmentInfo eq = new EquipmentInfo(item, EquipmentBelongTo.EQUIP);
            if (defaultDictionary.ContainsKey(eq.Slot))
            {
                GameSys.LogError(ConfigMng.Instance.GetUItext(213));
            }
            defaultDictionary[eq.Slot] = eq;
        }

        ProcessServerData(serverData);
        loginTime = Time.time;
    }

	/// <summary>
	/// 转生兑换次数
	/// </summary>
	public uint reinNum = 0;
	
        public MainPlayerData GetServerData()
    {
        return serverData;
    }


    public void EnterScene(pt_req_load_scene_b104 _info)
    {
        serverData.sceneID = (int)_info.scene;
		SceneID = serverData.sceneID;
        serverData.startPosX = _info.x;
        serverData.startPosY = _info.y;
        serverData.startPosZ = _info.z;
    }

    public override void ChangeValue(st.net.NetBase.property _value)
    {
        int diff = 0;
		//Debug.Log("_value.sort:"+_value.sort+",_value.type:"+_value.type+",_value.data:"+_value.data);
        ulong ulongVal = (ulong)_value.data;
        if (_value.sort == 1)
        {
            if (serverData.propertyValueDic.ContainsKey((ActorPropertyTag)_value.type))
                diff = (int)(_value.data - serverData.propertyValueDic[(ActorPropertyTag)_value.type]);
            serverData.propertyValueDic[(ActorPropertyTag)_value.type] = _value.data;
			if (OnPropertyDiffUpdate != null && diff != 0)
            {
                OnPropertyDiffUpdate((ActorPropertyTag)_value.type, diff);
            }
            if (OnPropertyUpdate != null)
            {
                OnPropertyUpdate((ActorPropertyTag)_value.type, _value.data, false);
            }
        }
        else if (_value.sort==2)
        {
            ///这里涉及到一个经验值计算  如果一次获得经验值导致升级多次 则需要把之前的经验累加上
            if (serverData.baseValueDic.ContainsKey((ActorBaseTag)_value.type))
            {
                diff = (int)(ulongVal - serverData.baseValueDic[(ActorBaseTag)_value.type]);
            }
            else
            {
                diff = (int)(_value.data);
            }
            if ((ActorBaseTag)_value.type==ActorBaseTag.Level)
            {
                //Accumulationexp = AddAccumulationExp(Level, (int)_value.data , CurExp);
				uint oldLevel = 0;
				if(serverData.baseValueDic.ContainsKey(ActorBaseTag.Level))
				{
					oldLevel = (uint)serverData.baseValueDic[ActorBaseTag.Level];
				}
                serverData.baseValueDic[(ActorBaseTag)_value.type] = ulongVal;
				if (OnBaseDiffUpdate != null && diff != 0)
                {
                    OnBaseDiffUpdate((ActorBaseTag)_value.type, diff,false);
                }
                if (OnBaseUpdate != null)
                {
                    OnBaseUpdate((ActorBaseTag)_value.type, ulongVal, false);
                }
				if (GameCenter.instance.isPlatform && serverData.baseValueDic.ContainsKey(ActorBaseTag.Level) && oldLevel != _value.data)
                {
                    int lev = (int)serverData.baseValueDic[ActorBaseTag.Level];
                    LynSdkManager.Instance.UsrLevelUp(lev);
                    //if(GameCenter.instance.isDataEyePattern) DCAccount.setLevel(lev);
                }
            }else
            {
                serverData.baseValueDic[(ActorBaseTag)_value.type] = ulongVal;
                if (OnBaseDiffUpdate != null)
                {
                    OnBaseDiffUpdate((ActorBaseTag)_value.type, diff, false);
                }
                if (OnBaseUpdate != null)
                {
                    OnBaseUpdate((ActorBaseTag)_value.type, ulongVal, false);
                }
				if((ActorBaseTag)_value.type == ActorBaseTag.Camp)
				{
					SetCamp(_value.data);
				}
                if ((ActorBaseTag)_value.type == ActorBaseTag.SLAVALUE && diff > 0)
                {
                    GameCenter.messageMng.AddClientMsg(493);
                }
            }
        }
    }

    /// <summary>
    /// 积累的经验值 因为协议中发送的都是当前经验值 所以升级的时候经验值 要计算上之前升级的经验
    /// </summary>
    //private ulong Accumulationexp = 0;
    /// <summary>
    /// 计算oldexp 从 oldlev 升级到 lev的经验
    /// </summary>
//    ulong AddAccumulationExp(int oldlev, int lev, ulong oldexp)
//    {
//        ulong exp = 0;
//        for (int i = oldlev; i < lev; i++)
//        {
//            if (i == oldlev)
//				exp += (ulong)ConfigMng.Instance.GetAttributeRef(i).max_exp - oldexp;
//            else
//				exp += (ulong)ConfigMng.Instance.GetAttributeRef(i).max_exp;
//        }
//        return exp;
//    }

    /// <summary>
    /// 阵营改变  by吴江
    /// </summary>
    /// <param name="_newCamp"></param>
    public new void SetCamp(int _newCamp)
    {
        serverData.camp = _newCamp;
        if (OnBaseUpdate != null && serverData.baseValueDic.ContainsKey(ActorBaseTag.MilitaryLv))
            OnBaseUpdate(ActorBaseTag.MilitaryLv, serverData.baseValueDic[ActorBaseTag.MilitaryLv], false);
        if (OnCampUpdate != null)
        {
            OnCampUpdate(_newCamp);
        }
    }

    public override void UpdateName(string _name)
    {
        base.UpdateName(_name);
        if (GameCenter.mercenaryMng != null && GameCenter.mercenaryMng.curMercernaryInfo != null)//更新宠物的从属名字
            GameCenter.mercenaryMng.curMercernaryInfo.UpdateOwnerName(_name);
    }

    /// <summary>
    /// 清空buff，在切换场景时和死亡时使用
    /// </summary>
    public override void CleanBuff()
    {
        base.CleanBuff();
        if (GameCenter.abilityMng != null)
        {
            //GameCenter.abilityMng.CleanLockState();
        }
    }
	
	#region 隐藏掉
	/// <summary>
	/// 整套时装更换
	/// </summary>
	/// <param name="_info"></param>
	//public void UpdateCosmetic(CosmeticInfo _info)
	//{
	//    cosmeticDictionary.Clear();
	//    if (_info == null)
	//    {
	//        actorData.cosmeticID = -1;
	//    }
	//    else
	//    {
	//        actorData.cosmeticID = _info.ID;
	//        foreach (var eq in _info.EquipList.Values)
	//        {
	//            cosmeticDictionary[eq.Slot] = eq;
	//        }
	//    }
	//    UpdateCurShowEquipments();
	//    if (OnEquipUpdate != null)
	//    {
	//        OnEquipUpdate();
	//    }
	//}
	
    /// <summary>
    /// 穿/脱装备 by吴江
    /// </summary>
    /// <param name="_data"></param>
//    public void Update(ServerData.EquipmentData _data)
//    {
//        EquipmentInfo eq = new EquipmentInfo(_data, EquipmentBelongTo.EQUIP);
//        bool takeOff = eq.StackCurCount == 0;
//        if (eq.Family == EquipmentFamily.COSMETIC)
//        {
//            if (cosmeticDictionary.ContainsKey(eq.Slot) && cosmeticDictionary[eq.Slot] != null)
//            {
//                if (cosmeticDictionary[eq.Slot].InstanceID == eq.InstanceID)
//                {
//                    cosmeticDictionary[eq.Slot].Update(_data);
//                    eq = cosmeticDictionary[eq.Slot];
//                }
//            }
//            cosmeticDictionary[eq.Slot] = takeOff ? null : eq;
//        }
//        else if(eq.Family != EquipmentFamily.GEM)
//        {
//            if (equipmentDictionary.ContainsKey(eq.Slot) && equipmentDictionary[eq.Slot] != null)
//            {
//                if (equipmentDictionary[eq.Slot].InstanceID == eq.InstanceID)
//                {
//                    equipmentDictionary[eq.Slot].Update(_data);
//                    eq = equipmentDictionary[eq.Slot];
//                }
//            }
//            equipmentDictionary[eq.Slot] = takeOff ? null : eq;
//        }else
//        {
//            gemDictionary[_data.pos] = takeOff ? null : eq;
////			if(OnCurShowEquipUpdate != null)//更新身上装备的宝石数据
////				OnCurShowEquipUpdate(EquipSlot.gem);
//        }
////		if(eq.Family != EquipmentFamily.GEM)
//        UpdateCurShowEquipments();
//        if (OnEquipUpdate != null)
//        {
//            OnEquipUpdate();
//        }
//    }

    /// <summary>
    /// 复活。目前的游戏机制是回主城直接复活。因此没走服务端验证。下个项目不能这样 。 by吴江
    /// </summary>
    //public void ReLive()
    //{
    //    actorData.baseDictionary[ActorBaseTag.CurHP] = actorData.propertyDictionary[ActorPropertyTag.MaxHP];
    //    actorData.baseDictionary[ActorBaseTag.CurMP] = actorData.propertyDictionary[ActorPropertyTag.MaxMP];
    //    IsAlive = true;
    //}


    //#region 访问器
    ///// <summary>
    ///// 静态配置 by吴江
    ///// </summary>
    //protected override PlayerConfig RefData
    //{
    //    get
    //    {
    //        if (actorData == null)
    //        {
    //            GameSys.LogError("该对象没有服务端数据！");
    //            return null;
    //        }
    //        if (actorData.newbieState == ServerData.NewbieState.Newbie)//新手玩家有临时职业
    //        {
    //            int newbieProf = Prof;
    //            if (refData == null || refData.id != newbieProf)
    //            {
    //                refData = ConfigMng.Instance.GetPlayerConfig(newbieProf);
    //            }
    //        }
    //        else
    //        {
    //            if (refData == null || refData.id != (int)actorData.prof)
    //            {
    //                refData = ConfigMng.Instance.GetPlayerConfig((int)actorData.prof);
    //            }
    //        }
    //        return refData;
    //    }
    //}
	#endregion

    /// <summary>
    /// 引导ID
    /// </summary>
    public int GuideStep
    {
        get { return serverData.guideID; }
    }
    /// <summary>
    /// 登陆完成获得的VIP等级,用于中间件EnterGame数据采集,没有同步VIP变化,其他地方勿用
    /// </summary>
    public int VipLevel
    {
        get
        {
            return serverData.vipLev; 
        }
    }

	public int GuildID
	{
		get
		{ 
			return serverData.guildID; 
		}
	}

    /// <summary>
    /// 属性作为资源 by 贺丰  别用这个接口拿经验和货币, by邓成
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public int GetIDToResource(ActorBaseTag tag)
    {
        if (serverData.baseValueDic.ContainsKey(tag))
        {
			return (int)serverData.baseValueDic[tag];
        }
        return 0;
    }


	protected int sceneID = 0;
    /// <summary>
    /// 当前主角所在的场景ID by吴江
    /// </summary>
    public int SceneID
    {
        get
        {
			return sceneID;
        }
		set
		{
			if(sceneID != value)
			{
				LastSceneID = sceneID;
				sceneID = value;
			}
		}
    }
	protected int lastSceneID = 0;
	/// <summary>
	/// 主角上一个场景ID
	/// </summary>
	public int LastSceneID
	{
		get
		{
			return lastSceneID;
		}
		set
		{
			if(lastSceneID != value)
			{
				lastSceneID = value;
			}
		}
	}
    /// <summary>
    /// 记录一下创角时间，用于解决弹出模型UIBUG
    /// </summary>
    public float loginTime;

	/// <summary>
	/// 当前主角所在的场景(先判断是否为null) by邓成
	/// </summary>
	public SceneRef CurSceneRef
	{
		get
		{
			SceneRef sceneRef = ConfigMng.Instance.GetSceneRef(SceneID);
			return sceneRef;
		}
	}
    /// <summary>
    /// 当前场景是否可以使用药品
    /// </summary>
    public bool IsCanUseDrug
    {
        get
        {
            return CurSceneRef != null ? CurSceneRef.useMedicItem == 1 : false;
        }
    }
	/// <summary>
	/// 是否显示暂停按钮 by 何明军
	/// </summary>
	public bool IsShowStop{
		get{
			return CurSceneRef != null ? CurSceneRef.suspend == 1 : false;
		}
	}
	
	/// <summary>
	/// 是否可以切换PK模式 by 何明军
	/// </summary>
	public bool IsUpdatePkMode{
		get{
			return CurSceneRef != null ? CurSceneRef.pk_mode == 0 : false;
		}
	}

	public SceneType CurSceneType
	{
		get
		{
			if(CurSceneRef != null)
				return CurSceneRef.sort;
			return SceneType.NONE;
		}
	}
	/// <summary>
	/// 当前场景UI类型
	/// </summary>
	/// <value>The type of the current scene user interface.</value>
	public SceneUiType CurSceneUiType
	{
		get
		{
		if(CurSceneRef != null)
			return (SceneUiType)CurSceneRef.uiType;
		return SceneUiType.NONE;
		}
	}

    /// <summary>
    /// 当前蓝量 by吴江
    /// </summary>
    public new int CurMP
    {
        get
        {
			return (int)serverData.baseValueDic[ActorBaseTag.CurMP];
        }
    }
    /// <summary>
    /// 当前蓝量字符串
    /// </summary>
    public string CurMPText
    {
        get
        {
            if (CurMP >= 1000000)
            {
                return Math.Round(CurMP / 10000f, 0).ToString() + ConfigMng.Instance.GetUItext(180);
            }
            else if (CurMP >= 10000)
            {
                return Math.Round(CurMP / 10000f, 1).ToString() + ConfigMng.Instance.GetUItext(180);
            }
            else
            {
                return CurMP.ToString();
            }
        }
    }  
	/// <summary>
	/// 总共的钱
	/// </summary>
    public ulong TotalCoinCount
	{
		get
		{
			return UnBindCoinCount+BindCoinCount;
		}
	}
    /// <summary>
    /// 总共的钱字符串
    /// </summary>
    public string TotalCoinCountText
    {
        get
        {
            //if (TotalCoinCount >= 1000000)
            //{
            //    return Math.Round(TotalCoinCount / 10000f, 0).ToString() + "万";
            //}
            //else if (TotalCoinCount >= 10000)
            //{
            //    return Math.Round(TotalCoinCount / 10000f, 1).ToString() + "万";
            //}
            //else
            //{
                return TotalCoinCount.ToString();
            //}

        }
    }  
    /// <summary>
    /// 未绑定铜币
    /// </summary>
    public ulong UnBindCoinCount
    {
        get
        {
            if (serverData.baseValueDic.ContainsKey(ActorBaseTag.UnBindCoin))
            {
                return serverData.baseValueDic[ActorBaseTag.UnBindCoin];
            }
            return 0;
        }
    }
    /// <summary>
    /// 未绑定铜币字符串
    /// </summary>
    public string UnBindCoinCountText
    {
        get
        {
            //if (UnBindCoinCount >= 1000000)
            //{
            //    return Math.Round(UnBindCoinCount / 10000f, 0).ToString() + "万";
            //}
            //else if (UnBindCoinCount >= 10000)
            //{
            //    return Math.Round(UnBindCoinCount / 10000f, 1).ToString() + "万";
            //}
            //else
            //{
                return UnBindCoinCount.ToString();
            //}

        }
    } 
	/// <summary>
	/// 已绑定铜币
	/// </summary>
	public ulong BindCoinCount
	{
		get
		{
			if (serverData.baseValueDic.ContainsKey(ActorBaseTag.BindCoin))
			{
				return serverData.baseValueDic[ActorBaseTag.BindCoin];
			}
			return 0;
		}
	}
    /// <summary>
    /// 已绑定铜币字符串
    /// </summary>
    public string BindCoinCountText
    {
        get
        {
            //if (BindCoinCount >= 1000000)
            //{
            //    return Math.Round(BindCoinCount / 10000f, 0).ToString() + "万";
            //}
            //else if (BindCoinCount >= 10000)
            //{
            //    return Math.Round(BindCoinCount / 10000f, 1).ToString() + "万";
            //}
            //else
            //{
                return BindCoinCount.ToString();
            //}

        }
    }
    /// <summary>
    /// 总共的钱
    /// </summary>
    public ulong TotalDiamondCount
    {
        get
        {
            return BindDiamondCount + DiamondCount;
        }
    }
    /// <summary>
    /// 元宝
    /// </summary>
    public ulong DiamondCount
    {
        get
        {
            if (serverData.baseValueDic.ContainsKey(ActorBaseTag.Diamond))
            {
				return serverData.baseValueDic[ActorBaseTag.Diamond];
            }
            return 0;
        }
    }
    /// <summary>
    /// 元宝字符串
    /// </summary>
    public string DiamondCountText
    {
        get
        {
            //if (DiamondCount >= 1000000) 
            //{
            //    return Math.Round(DiamondCount / 10000f, 0).ToString() + "万";
            //}
            //else  if (DiamondCount >= 10000)
            //{
            //    return Math.Round(DiamondCount / 10000f, 1).ToString() + "万";
            //}
            //else {
                return DiamondCount.ToString();
            //}
         
        }
    }  
    /// <summary>
    /// 礼金
    /// </summary>
    public ulong BindDiamondCount
    {
        get
        {
            if (serverData.baseValueDic.ContainsKey(ActorBaseTag.BindDiamond))
            {
                return serverData.baseValueDic[ActorBaseTag.BindDiamond];
            }
            return 0;
        }
    }
    /// <summary>
    /// 真元
    /// </summary>
    public ulong RealYuanCount
    {
        get
        {
            if (serverData.baseValueDic.ContainsKey(ActorBaseTag.REALYUAN))
            {
                return serverData.baseValueDic[ActorBaseTag.REALYUAN];
            }
            return 0;
        }
    }
    /// <summary>
    /// 礼金字符串
    /// </summary>
    public string BindDiamondCountText
    {
        get
        {
            //if (BindDiamondCount >= 1000000)
            //{
            //    return Math.Round(BindDiamondCount / 10000f, 0).ToString() + "万";
            //}
            //else if (BindDiamondCount >= 10000)
            //{
            //    return Math.Round(BindDiamondCount / 10000f, 1).ToString() + "万";
            //}
            //else
            //{
                return BindDiamondCount.ToString();
            //}
        }
    }  
	/// <summary>
	/// 当前修为,转生资源
	/// </summary>
	public int ReliveRes
	{
		get
		{
			if (serverData.baseValueDic.ContainsKey(ActorBaseTag.Fix))
			{
				return (int)serverData.baseValueDic[ActorBaseTag.Fix];
			}
			return 0;
		}
	}
	/// <summary>
	/// 当前悟性,技能资源
	/// </summary>
	public int SkillRes
	{
		get
		{
			if (serverData.baseValueDic.ContainsKey(ActorBaseTag.SkillRes))
			{
				return (int)serverData.baseValueDic[ActorBaseTag.SkillRes];
			}
			return 0;
		}
	}
	/// <summary>
	/// 当前灵气,飞升低等资源
	/// </summary>
	public int LowFlyUpRes
	{
		get
		{
			if (serverData.baseValueDic.ContainsKey(ActorBaseTag.LowFlyUpRes))
			{
				return (int)serverData.baseValueDic[ActorBaseTag.LowFlyUpRes];
			}
			return 0;
		}
	}
	/// <summary>
	/// 当前仙气,飞升低等资源
	/// </summary>
	public int HighFlyUpRes
	{
		get
		{
			if (serverData.baseValueDic.ContainsKey(ActorBaseTag.HighFlyUpRes))
			{
				return (int)serverData.baseValueDic[ActorBaseTag.HighFlyUpRes];
			}
			return 0;
		}
	}
	/// <summary>
	/// 当前功勋,功勋商店资源
	/// </summary>
	public int Exploit
	{
		get
		{
			if (serverData.baseValueDic.ContainsKey(ActorBaseTag.Exploit))
			{
				return (int)serverData.baseValueDic[ActorBaseTag.Exploit];
			}
			return 0;
		}
	}
	/// <summary>
	/// 当前声望,声望商店资源
	/// </summary>
	public int Repuutation
	{
		get
		{
			if (serverData.baseValueDic.ContainsKey(ActorBaseTag.Repuutation))
			{
				return (int)serverData.baseValueDic[ActorBaseTag.Repuutation];
			}
			return 0;
		}
	}
	/// <summary>
	/// 当前积分,积分商店资源
	/// </summary>
	public int Integral
	{
		get
		{
			if (serverData.baseValueDic.ContainsKey(ActorBaseTag.Integral))
			{
				return (int)serverData.baseValueDic[ActorBaseTag.Integral];
			}
			return 0;
		}
	}
	/// <summary>
	/// 当前VIP经验
	/// </summary>
	public int VIPExp
	{
		get
		{
            if (GameCenter.vipMng.VipData != null)
                return GameCenter.vipMng.VipData.vExp;
            else
                return 0;
		}
	}
	/// <summary>
	/// 当前仙盟贡献
	/// </summary>
	public int GuildContribution
	{
		get
		{
			if (serverData.baseValueDic.ContainsKey(ActorBaseTag.GuildContribution))
			{
				return (int)serverData.baseValueDic[ActorBaseTag.GuildContribution];
			}
			return 0;
		}
	}
    /// <summary>
    /// 发起攻击的最大距离 by吴江
    /// </summary>
    public float AttackDistance
    {
        get { return RefData != null ? (RefData.atk_dis - 0.5f) : float.MaxValue; } 
    }

    /// <summary>
    /// 当前血量
    /// </summary>
    public new int CurHP
    {
        get
        {
			return (int)serverData.baseValueDic[ActorBaseTag.CurHP];
        }
    }
    /// <summary>
    /// 当前血量字符串
    /// </summary>
    public string CurHPText
    {
        get
        {
            if (CurHP >= 1000000)
            {
                return Math.Round(CurHP / 10000f, 0).ToString() + ConfigMng.Instance.GetUItext(180);
            }
            else if (CurHP >= 10000)
            {
                return Math.Round(CurHP / 10000f, 1).ToString() + ConfigMng.Instance.GetUItext(180);
            }
            else
            {
                return CurHP.ToString();
            }
        }
    }  
    /// <summary>
    /// 当前等级,查表数值
    /// </summary>
    public new int Level
    {
        get
        {
			AttributeRef attributeRef = ConfigMng.Instance.GetAttributeRef(CurLevel);
			return attributeRef == null?0:attributeRef.display_level;
        }
    }
	/// <summary>
	/// 是否可以转生
	/// </summary>
	public bool IsRien{
		get{
			AttributeRef refData = ConfigMng.Instance.GetAttributeRef(CurLevel);
			if(refData == null)return false;
			SuperLifeRef data = ConfigMng.Instance.GetSuperLifeRef(refData.reborn);
			if(data.need_lev <= CurLevel && data.superExp <= ReliveRes){
				return true;
			}
			return false;
		}
	}
	
	/// <summary>
	/// 当前等级,直接数值
	/// </summary>
	public int CurLevel
	{
		get
		{
			return (int)serverData.baseValueDic[ActorBaseTag.Level];
		}
	}
    /// <summary>
    /// 当前战力
    /// </summary>
    public int CurFightVal
    {
        get
        {
            if (serverData.baseValueDic.ContainsKey(ActorBaseTag.FightValue))
            {
				return (int)serverData.baseValueDic[ActorBaseTag.FightValue];
            }
            return 0;
        }
    }
    /// <summary>
    /// 最大经验值
    /// </summary>
    public new ulong MaxExp
    {
        get
        {
			return (ulong)ConfigMng.Instance.GetAttributeRef((int)serverData.baseValueDic[ActorBaseTag.Level]).max_exp;
        }
    }
    public override string CurStarEffect
    {
        get
        {
            return base.CurStarEffect;
        }
    }
}

	
	