//====================================
//作者：吴江
//日期：2015/5/16
//用途：玩家数据层基对象（Info结尾的类名都为数据层对象，包含 服务端数据  客户端静态数据   访问器 三部分）
//=====================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using st.net.NetBase;



public class PlayerBaseData : ActorData
{
    public string guildName = string.Empty;
    public string name;
    public int vipLev;
	/// <summary>
	/// 衣服时装ID
	/// </summary>
	public int clothesFashionID;
    /// <summary>
    /// 武器时装ID
    /// </summary>
    public int weaponFashionID;
	/// <summary>
	/// 法宝ID(物品表ID)
	/// </summary>
	public int magicWeaponID = 0;
	/// <summary>
	/// 翅膀ID(物品表ID)
	/// </summary>
	public int wingID;
    /// <summary>
    /// 称号
    /// </summary>
    public int titleID = -1;
    public int lastLoginTime;
	/// <summary>
	/// 杀戮值
	/// </summary>
	public int slaValue = 0;
	/// <summary>
	/// 装备信息
	/// </summary>
	public List<EquipmentInfo> realEquipmentInfo = new List<EquipmentInfo>();
	/// <summary>
	/// 技能列表,其他玩家  by邓成
	/// </summary>
	public List<SkillInfo> skillList = new List<SkillInfo>();
    /// <summary>
    /// 其他玩家的最小套装强化等级(12件装备没穿满时为0)
    /// </summary>
    public int smallStrenLev;
    public PlayerBaseData()
    {
    }

    public PlayerBaseData(create_usr_info _info)
    {
        serverInstanceID = (int)_info.id;
        name = _info.name;
        baseValueDic[ActorBaseTag.Level] = _info.level;
        prof = (int)_info.prof;
		lastLoginTime = (int)_info.create_time;
        equipTypeList.Clear();
        
    }
    //by 贺丰
    public PlayerBaseData(pt_usr_info_equi_d123 _info)
    {
        serverInstanceID = (int)_info.pid;
        name = _info.name;
        guildName = _info.guild_name;
        baseValueDic[ActorBaseTag.Level] = _info.lev;
        prof = (int)_info.prof;
        titleID = (int)_info.title;
        equipTypeList.Clear();
        for (int i = 0; i < _info.item_list.Count; i++)
        {
            equipTypeList.Add((int)_info.item_list[i].type);
        }
    }
	/// <summary>
	/// 查看其他玩家
	/// </summary>
	/// <param name="_info">Info.</param>
	public PlayerBaseData(pt_look_usr_list_d712 _info)
	{
        vipLev = _info.vip_lev;
		serverInstanceID = _info.uid;
		name = _info.name;
		guildName = _info.guild_name;
		baseValueDic[ActorBaseTag.Level] = (ulong)_info.lev;
		prof = _info.prof;
        //clothesFashionID = _info.model_clothes_id;
		equipTypeList.Clear();
		realEquipmentInfo.Clear();
		for (int i = 0; i < _info.target_equip_list.Count; i++)
		{
			equipTypeList.Add((int)_info.target_equip_list[i].type);
			//Debug.Log(_info.target_equip_list[i].type+","+_info.target_equip_list[i].lev);
			realEquipmentInfo.Add(new EquipmentInfo(_info.target_equip_list[i],EquipmentBelongTo.PREVIEW));
		}
		for (int i = 0,max = _info.model_clothes_id.Count; i < max; i++) {
			if(_info.model_clothes_id[i] != 0)
				equipTypeList.Add(_info.model_clothes_id[i]);
		}
        if (_info.wing_id > 0)
        {
            WingRef data = ConfigMng.Instance.GetWingRef(_info.wing_id, _info.wing_lev);
            if (data != null)
            {
                equipTypeList.Add(data.itemui);
            }
        }
		for (int i = 0,max=_info.target_property.Count; i < max; i++) {
			propertyValueDic[(ActorPropertyTag)(int)_info.target_property[i].type] = (int)_info.target_property[i].data;
		}
		skillList.Clear();
		for (int i = 0,max=_info.target_skill.Count; i < max; i++) {
			//Debug.Log("skill:"+_info.target_skill[i].skill_id+",lev:"+_info.target_skill[i].skill_lev);
			skillList.Add(new SkillInfo(_info.target_skill[i]));
		}
		//propertyValueDic[ActorPropertyTag.SLA] = _info.slaughter;
		propertyValueDic[ActorPropertyTag.LUCKY] = _info.luck_num;

		baseValueDic[ActorBaseTag.FightValue] = (ulong)_info.battle;
        baseValueDic[ActorBaseTag.SLAVALUE] = (ulong)_info.slaughter;
	}
    /// <summary>
    /// 排行榜玩家模型显示
    /// </summary>
    /// <param name="_info"></param>
    public PlayerBaseData(pt_req_look_rank_usrinfo_d773 _info)
    {
        serverInstanceID = _info.uid;
        name = _info.name;
        baseValueDic[ActorBaseTag.Level] = (ulong)_info.lev;
        prof = _info.prof;
        //clothesFashionID = _info.model_clothes_id;
        //Debug.Log("获取排行榜其他玩家信息 " + _info.name + "职业" + _info.prof);
        equipTypeList.Clear();
        for (int i = 0; i < _info.target_equip_list.Count; i++)
        {
            equipTypeList.Add((int)_info.target_equip_list[i].type);
			realEquipmentInfo.Add(new EquipmentInfo(_info.target_equip_list[i],EquipmentBelongTo.PREVIEW));
        }
        if (_info.wing_id > 0)
        {
            WingRef data = ConfigMng.Instance.GetWingRef(_info.wing_id, _info.wing_lev);
            if (data != null)
            {
                equipTypeList.Add(data.itemui);
            }
        }
        for (int i = 0, max = _info.model_clothes_id.Count; i < max; i++)
        {
            if (_info.model_clothes_id[i] != 0)
                equipTypeList.Add(_info.model_clothes_id[i]);
        }
    }
	/// <summary>
	/// 城主
	/// </summary>
	/// <param name="_info"></param>
	public PlayerBaseData(pt_guild_storm_city_ui_info_d730 _info)
	{
		guildName = _info.guild_name;
		name = _info.castellan;
		prof = _info.prof;
		equipTypeList.Clear();
		if (_info.wing_id > 0)
		{
			WingRef data = ConfigMng.Instance.GetWingRef(_info.wing_id, _info.wing_lev);
			if (data != null)
			{
				equipTypeList.Add(data.itemui);
			}
		}
		if(_info.magic_weapon_id > 0)
		{
			RefineRef rr = ConfigMng.Instance.GetRefineRef(_info.magic_weapon_id, _info.magic_strength_lev, _info.magic_strength_star);
			if (rr != null)
			{
				magicWeaponID = rr.model;
			}
		}
		for (int i = 0, max = _info.model_id.Count; i < max; i++)
		{
			if (_info.model_id[i] != 0)
				equipTypeList.Add(_info.model_id[i]);
		}
	}
}



/// <summary>
/// 玩家数据层基对象（Info结尾的类名都为数据层对象，包含 服务端数据  客户端静态数据   访问器 三部分） by吴江
/// </summary>
public class PlayerBaseInfo : ActorInfo
{

    protected new PlayerBaseData serverData
    {
        get
        {
            return base.serverData as PlayerBaseData;
        }
        set
        {
            base.serverData = value;
        }
    }


    protected int mountId = 0;

    protected MountInfo curMountInfo = null;

	protected MercenaryInfo curPetInfo = null;
	/// <summary>
	/// 当前宠物(其他玩家信息中)
	/// </summary>
	/// <value>The current pet list.</value>
	public MercenaryInfo CurPetInfo
	{
		get
		{
			return curPetInfo;
		}
	}

    protected PlayerBaseInfo()
    {
    }


    public PlayerBaseInfo(create_usr_info _serverData)
    {
        serverData = new PlayerBaseData(_serverData);
        refData = ConfigMng.Instance.GetPlayerConfig((int)serverData.prof);
        ProcessServerData(serverData);
    }


    public PlayerBaseInfo(PlayerBaseData _data)
    {
        serverData = _data;
        ProcessServerData(serverData);
    }

    /// <summary>
    /// 创建角色构造
    /// </summary>
    /// <param name="_createSettingRef"></param>
	public PlayerBaseInfo(int _prof, int _instanceID,bool onlyForCreate = false)
    {
        serverData = new PlayerBaseData();
        serverData.serverInstanceID = _instanceID;
        serverData.prof = _prof;

       // CreateSettingRef createSettingRef = ConfigMng.Instance.GetCreateSettingRef(serverData.prof);
		if(!onlyForCreate)//创角不需要显示默认装备
		{
			PlayerConfig playerConfig = ConfigMng.Instance.GetPlayerConfig(serverData.prof);
			if (playerConfig != null)
	        {
				for (int i = 0; i < playerConfig.defaultEquipList.Count; i++)
	            {
					serverData.equipTypeList.Add(playerConfig.defaultEquipList[i]);
	            }
	        }
	        ProcessServerData(serverData);
		}
    }
		
	/// <summary>
	/// 攻城战城主
	/// </summary>
	public PlayerBaseInfo(pt_guild_storm_city_ui_info_d730 guild_city)
	{
		serverData = new PlayerBaseData(guild_city);
		ProcessServerData(serverData);
	}



    /// <summary>
    /// 客户端静态配置引用 by 吴江
    /// </summary>
    protected PlayerConfig refData = null;

    /// <summary>
    /// 客户端静态配置引用 by 吴江
    /// </summary>
    protected PlayerConfig RefData
    {
        get
        {
            if (refData == null || refData.id != serverData.prof)
            {
                refData = ConfigMng.Instance.GetPlayerConfig((int)serverData.prof);
            }
            return refData;
        }
    }


    protected bool isInSafetyArea = false;
    /// <summary>
    /// 是否在安全区域内  by吴江
    /// </summary>
    public bool IsInSafetyArea
    {
        get
        {
            return isInSafetyArea;
        }
        set
        {
            if (isInSafetyArea != value)
            {
                isInSafetyArea = value;
				if(OnSafetyAreaUpdate != null)
					OnSafetyAreaUpdate(value);
            }
        }
    }
    /// <summary>
    /// 进出安全区的事件 by吴江
    /// </summary>
    public System.Action<bool> OnSafetyAreaUpdate;


    /// <summary>
    /// 穿/脱装备
    /// </summary>
    /// <param name="_eid"></param>
    public void UpdateEquipment(int _eid,bool _equip)
    {
        if (_equip)
        {
            EquipmentInfo eq = new EquipmentInfo(_eid, EquipmentBelongTo.EQUIP);
            //Debug.logger.Log(eq.ShortUrl + " , " + eq.ShowType + " , " + eq.FightWeaponPointName);
            switch (eq.Family)
            {
                case EquipmentFamily.COSMETIC:
                    cosmeticDictionary[eq.Slot] = eq;
                    break;

                default:
                    equipmentDictionary[eq.Slot] = eq;
                    break;
            }
        }
        else
        {
			EquipmentRef equipmentRef = ConfigMng.Instance.GetEquipmentRef(_eid);
			if (equipmentRef != null)
            {
				switch (equipmentRef.family)
                {
                    case EquipmentFamily.COSMETIC:
						cosmeticDictionary[equipmentRef.slot] = null;
                        break;

                    default:
						equipmentDictionary[equipmentRef.slot] = null;
                        break;
                }
            }
        }
        UpdateCurShowEquipments();
        if (OnEquipUpdate != null)
        {
            OnEquipUpdate();
        }
    }
	/// <summary>
	/// 穿脱翅膀or法宝,只更新数据(其他玩家) by邓成
	/// </summary>
	public void Update(EquipSlot _slot, int _eid,bool _use)
	{
		if(_use)//穿
		{
			EquipmentInfo info = new EquipmentInfo(_eid,EquipmentBelongTo.PREVIEW);
			cosmeticDictionary[_slot] = info;
		}else//其他玩家脱翅膀发的信息是0
		{
			cosmeticDictionary[_slot] = null;
		}
	}

        /// <summary>
    /// 穿/脱装备 by吴江
    /// </summary>
    /// <param name="_data"></param>
    public void Update(List<st.net.NetBase.equip_id_state_list> equip_id_state_list)
    {

        for (int i = 0; i < equip_id_state_list.Count; i++)
        {
            if (equip_id_state_list[i].equip_state > 0)//穿
            {
                EquipmentInfo eq = new EquipmentInfo((int)equip_id_state_list[i].equip_id, EquipmentBelongTo.EQUIP);
                switch (eq.Family)
                {
                    case EquipmentFamily.COSMETIC:
                        cosmeticDictionary[eq.Slot] = eq;
                        break;

                    default:
                        equipmentDictionary[eq.Slot] = eq;
                        break;
                }
            }
            else
            {
                EquipmentRef refData = ConfigMng.Instance.GetEquipmentRef((int)equip_id_state_list[i].equip_id);
                if (refData != null)
                {
                    switch (refData.family)
                    {
                        case EquipmentFamily.COSMETIC:
                            cosmeticDictionary[refData.slot] = null;
                            break;

                        default:
                            equipmentDictionary[refData.slot] = null;
                            break;
                    }
                }
            }
        }

        UpdateCurShowEquipments();
        if (OnEquipUpdate != null)
        {
            OnEquipUpdate();
        }
    }

    /// <summary>
    /// 更新信息 by 贺丰
    /// </summary>
    public void UpdateInfo(pt_usr_info_property_d124 _pt)
    {
        serverData.propertyValueDic[ActorPropertyTag.DOD] = (int)_pt.dod;
        serverData.propertyValueDic[ActorPropertyTag.HIT] = (int)_pt.hit;
        serverData.propertyValueDic[ActorPropertyTag.HPLIMIT] = (int)_pt.limit_hp;
        serverData.propertyValueDic[ActorPropertyTag.MOVESPD] = (int)_pt.moveSpd;
        serverData.propertyValueDic[ActorPropertyTag.MPLIMIT] = (int)_pt.limit_mp;
        serverData.propertyValueDic[ActorPropertyTag.TOUGH] = (int)_pt.tough;

        serverData.baseValueDic[ActorBaseTag.CurHP] = _pt.cur_hp;
        serverData.baseValueDic[ActorBaseTag.CurMP] = _pt.cur_mp;
        serverData.baseValueDic[ActorBaseTag.Exp] = _pt.exp;
        serverData.baseValueDic[ActorBaseTag.FightValue] = _pt.fighting;
    }

	/// <summary>
	/// 其他玩家坐骑、宠物更新
	/// </summary>
	/// <param name="_pt">Point.</param>
	public void UpdateInfo(pt_look_pet_ride_info_d713 _pt)
	{
        if (_pt != null)
        {
            if (_pt.ride_skin_id != 0)
            {
                curMountInfo = new MountInfo(new MountData(_pt.ride_skin_id, _pt.ride_lev), this, true);
            }
            else if (_pt.ride_type != 0)//没有坐骑
            {
                curMountInfo = new MountInfo(new MountData(_pt.ride_type, _pt.ride_lev), this);
            }
            if (_pt.target_pet_list.Count > 0)
            {
                curPetInfo = new MercenaryInfo(_pt.target_pet_list[0], GameCenter.mainPlayerMng.MainPlayerInfo);
            }
        }
	}

	/// <summary>
	/// 衣服时装更新
	/// </summary>
	/// <param name="_titleID"></param>
	public void UpdateClothesFashion(int _fashionID)
	{
		if (serverData.clothesFashionID != _fashionID)
		{
            serverData.clothesFashionID = _fashionID;
            if (OnClothesFashionUpdate != null)
			{
                OnClothesFashionUpdate();
			}
		}
	}

    /// <summary>
    /// 武器时装更新
    /// </summary>
    /// <param name="_titleID"></param>
    public void UpdateWeaponFashion(int _fashionID)
    {
        if (serverData.weaponFashionID != _fashionID)
        {
            serverData.weaponFashionID = _fashionID;
            if (OnWeaponFashionUpdate != null)
            {
                OnWeaponFashionUpdate();
            }
        }
    }



    /// <summary>
    /// 称号更新
    /// </summary>
    /// <param name="_titleID"></param>
    public void UpdateTitle(int _titleID)
    {
        if (serverData.titleID != _titleID)
        {
            serverData.titleID = _titleID;
            if (OnTitleUpdate != null)
            {
                OnTitleUpdate();
            }
        }
    }
    /// <summary>
    /// 最后登录时间
    /// </summary>
    public int GetLastLoginTime
    {
        get { return serverData.lastLoginTime; }
    }
    /// <summary>
    /// 公会名字更新
    /// </summary>
    /// <param name="_titleID"></param>
    public void UpdateGuildName(string _guildName)
    {
        if (serverData.guildName != _guildName)
        {
            serverData.guildName = _guildName;
            if (onGuildNameUpdate != null)
            {
                onGuildNameUpdate(_guildName);
            }
        }
    }
    /// <summary>
    /// 玩家名字更新
    /// </summary>
    /// <param name="_name"></param>
    public virtual void UpdateName(string _name)
    {
        if (serverData.name != _name)
        {
            serverData.name = _name;
            if (OnNameUpdate != null)
                OnNameUpdate(_name);
        }
    }

    public void UpdateMoveSpeed()
    {
        if (OnPropertyUpdate != null)
        {
            OnPropertyUpdate(ActorPropertyTag.MOVESPD, (long)Movespd, false);
        }
    }

    /// <summary>
    /// 强化特效更新
    /// </summary>
    /// <param name="_smallLev"></param>
    public void UpdateStrengEffect(int _smallLev)
    {
        if (serverData.smallStrenLev != _smallLev)
        {
            serverData.smallStrenLev = _smallLev;
            if (OnEquipStrengEffectUpdate != null)
            {
                OnEquipStrengEffectUpdate();
            }
        }
    }
    public System.Action OnWeaponFashionUpdate;
	public System.Action OnClothesFashionUpdate;
    public System.Action OnTitleUpdate;
    public System.Action OnEquipStrengEffectUpdate;

    public virtual void ChangeValue(st.net.NetBase.property _value)
    {
        ulong ulongVal = (ulong)_value.data;
        if (_value.sort == 1)
        {
            serverData.propertyValueDic[(ActorPropertyTag)_value.type] = _value.data;
            //Debug.Log("ActorPropertyTag:" + (ActorPropertyTag)_value.type + ",value:" + _value.data);
            if (OnPropertyUpdate != null)
            {
                OnPropertyUpdate((ActorPropertyTag)_value.type, _value.data, false);
            }
        }
        else if (_value.sort == 2)
        {
            serverData.baseValueDic[(ActorBaseTag)_value.type] = ulongVal;
            if (OnBaseUpdate != null)
            {
                OnBaseUpdate((ActorBaseTag)_value.type, ulongVal, false);
            }
			if((ActorBaseTag)_value.type == ActorBaseTag.Camp)
			{
				SetCamp((int)_value.data);
			}
        }
    }

    public virtual void ChangeValue(st.net.NetBase.property64 _value)
    {
        if (_value.sort == 1)
        {
            serverData.propertyValueDic[(ActorPropertyTag)_value.type] = (int)_value.data;
            if (OnPropertyUpdate != null)
            {
                OnPropertyUpdate((ActorPropertyTag)_value.type, (int)_value.data, false);
            }
        }
        else if (_value.sort == 2)
        {
            serverData.baseValueDic[(ActorBaseTag)_value.type] = (ulong)_value.data;
            if (OnBaseUpdate != null)
            {
                OnBaseUpdate((ActorBaseTag)_value.type, (ulong)_value.data, false);
            }
			if((ActorBaseTag)_value.type == ActorBaseTag.Camp)
			{
				SetCamp((int)_value.data);
			}
        }
    }

	/// <summary>
	/// 阵营改变  by吴江
	/// </summary>
	/// <param name="_newCamp"></param>
	public void SetCamp(int _newCamp)
	{
		serverData.camp = _newCamp;
		if (OnCampUpdate != null)
		{
			OnCampUpdate(_newCamp);
		}
	}
    /// <summary>
    /// 当前的星级特效
    /// </summary>
    public void UpdateStarEffect()
    {
        //curStarEffect = string.Empty;
        //int curStart = 0;
        //foreach (EquipmentInfo item in equipmentDictionary.Values)
        //{
        //    curStart += item.Star;
        //}
        //StarPropertyRef sr = ConfigMng.Instance.GetStarPropertyRef(curStart);
        //if (sr != null)
        //{
        //    curStarEffect = sr.effect;
        //}
    }

    /// <summary>
    /// 复活（原地复活需要修改玩家坐标）
    /// </summary>
    /// <param name="_info"></param>
    /// <param name="_type"></param>
    public void Reborn(pt_revive_d205 _info)
    {
        serverData.startPosX = _info.x;
        serverData.startPosY = _info.y;
        serverData.startPosZ = _info.z;
        IsAlive = true;
    }


    protected override void ProcessServerData(ActorData _data)
    {
        base.ProcessServerData(_data);
        List<EquipmentInfo> curEquipList = new List<EquipmentInfo>();
		if(RefData != null)
		{
			for (int i = 0; i < RefData.defaultEquipList.Count; i++)
			{
				EquipmentInfo eq = new EquipmentInfo(RefData.defaultEquipList[i], EquipmentBelongTo.PREVIEW);
				DefaultDictionary[eq.Slot] = eq;
			}
		}
        if (_data.equipTypeList != null && _data.equipTypeList.Count > 0)
        {
            for (int i = 0; i < _data.equipTypeList.Count; i++)
            {
                EquipmentInfo eq = new EquipmentInfo(_data.equipTypeList[i], EquipmentBelongTo.PREVIEW);
                curEquipList.Add(eq);
            }
        }
        UpadateEquipments(curEquipList);
    }


    /// <summary>
    /// 职业变化的事件
    /// </summary>
    public Action<int> OnProfUpdate;

    /// <summary>
    /// 名字变化的事件
    /// </summary>
    public Action<string> OnNameUpdate;
	
	/// <summary>
	/// 玩家的状态模式改变（普通 ，雇佣兵）
	/// </summary>
	public Action<PlayerPlayMode> OnPlayerPlayModeUpdate;

    public Action OnMountUpdate;
    public Action<bool,bool> OnMountRideStateUpdate;
    public Action<SustainRef> onStartCollectEvent;
    /// <summary>
    /// 玩家结束采集事件  
    /// </summary>
    public new System.Action onEndCollectEvent;


    public void StartCollect(SustainRef _info)
    {
        if (onStartCollectEvent != null)
            onStartCollectEvent(_info);
    }
    public void EndCollect()
    {
        if (onEndCollectEvent != null)
            onEndCollectEvent();
    }

    #region 访问器
	/// <summary>
	/// 创建角色显示的豪华装备 by 何明军
	/// </summary>
	public Dictionary<EquipSlot,EquipmentInfo> CreateItemList
	{
		get
		{
			Dictionary<EquipSlot,EquipmentInfo> itemList = new Dictionary<EquipSlot, EquipmentInfo>();
			if(RefData == null)return itemList;
			for(int i=0;i<RefData.create_player_res.Count;i++){
				if(RefData.create_player_res[i] != 0){
					EquipmentInfo itemInfo = new EquipmentInfo(RefData.create_player_res[i],EquipmentBelongTo.PREVIEW);
					itemList.Add(itemInfo.Slot,itemInfo);
				}
			}
			return itemList;
		}
	}
/// <summary>
/// 半身像
/// </summary>
	public string IconHalf
	{
		get
		{
			return RefData == null ? string.Empty : RefData.icon_half;
		}
	}

	public int CreateAbilityID
	{
		get
		{
			return RefData == null ? 0 : RefData.display_action;
		}
	}

    public string IconName
    {
        get
        {
            return RefData == null ? string.Empty : RefData.res_head_Icon;
        }
    }
    /// <summary>
    /// 动画资源标准移动速度
    /// </summary>
    public override float AnimationMoveSpeedBase
    {
        get
        {
            return RefData == null ? 5.0f : RefData.paceSpeed;
        }
    }

    /// <summary>
    /// 工会名称 by吴江
    /// </summary>
    public string GuildName
    {
        get
        {
            return serverData.guildName;
        }
    }
	/// <summary>
	/// pk模式
	/// </summary>
	public PkMode CurPkMode
	{
		get
		{
			int pk = (int)(serverData.baseValueDic.ContainsKey(ActorBaseTag.PKMODE)?serverData.baseValueDic[ActorBaseTag.PKMODE]:0);
			return (PkMode)pk;
		}
	}
	
	/// <summary>
	/// pk模式名字
	/// </summary>
	public string PkModeName
	{
		get
		{
			switch(CurPkMode)
			{
			case PkMode.PKMODE_ALL:
                    return ConfigMng.Instance.GetUItext(215);
			case PkMode.PKMODE_GUILD:
                    return ConfigMng.Instance.GetUItext(216);
			case PkMode.PKMODE_TEAM:
                    return ConfigMng.Instance.GetUItext(217);
			case PkMode.PKMODE_PEASE:
                    return ConfigMng.Instance.GetUItext(218);
			case PkMode.PKMODE_JUSTICE:
                    return ConfigMng.Instance.GetUItext(219);
            case PkMode.PKMODE_CAMP:
                    return ConfigMng.Instance.GetUItext(220);
			}
			return string.Empty;
		}
	}
	/// <summary>
	/// 杀戮等级(用于红名显示)
	/// </summary>
	/// <value>The sla level.</value>
	public int SlaLevel
	{
		get
		{
			return (int)(serverData.baseValueDic.ContainsKey(ActorBaseTag.SLALEVEL)?serverData.baseValueDic[ActorBaseTag.SLALEVEL]:0);
		}
	}

	/// <summary>
	/// 是否有公会
	/// </summary>
	public bool HavaGuild
	{
		get
		{
			return !string.IsNullOrEmpty(GuildName);
		}
	}

    /// <summary>
    /// 模型根节点与质心的高度差
    /// </summary>
    public float CenterDiff
    {
        get
        {
            return RefData == null ? 0 : RefData.centerDiff;
        }
    }

    /// <summary>
    /// 名字
    /// </summary>
    public new string Name
    {
        get
        {
            return serverData.name;
        }
    }
    /// <summary>
    /// 预览坐标
    /// </summary>
    public Vector3 PreviewPos
    {
        get
        {
            return RefData == null ? Vector3.zero : RefData.previewPscale;
        }
    }
    /// <summary>
    /// 预览朝向
    /// </summary>
    public Vector3 PreviewRotation
    {
        get
        {
            return RefData == null ? Vector3.zero : RefData.previewRscale;
        }
    }

    /// <summary>
    /// 预览时的缩放比
    /// </summary>
    public float PreviewScale
    {
        get
        {
            return RefData == null ? 1 : RefData.preview_scale;
        }
    }

    /// <summary>
    /// 称号名称
    /// </summary>
    public string TitleName
    {
        get
        {
            if (serverData.titleID <= 0) return string.Empty;
            TitleRef titleRef = ConfigMng.Instance.GetTitlesRef(serverData.titleID);
            if (titleRef == null) return string.Empty;
            return titleRef.name;
        }
    }
    /// <summary>
    /// 称号图片
    /// </summary>
    public string TitleIcon
    {
        get
        {
            if (serverData.titleID <= 0) return string.Empty;
            TitleRef titleRef = ConfigMng.Instance.GetTitlesRef(serverData.titleID);
            if (titleRef == null) return string.Empty;
            return titleRef.icon;
        }
    }

    /// <summary>
    /// 衣服时装ID
    /// </summary>
    public int ClothesFashionID
    {
        get
        {
            if (serverData.clothesFashionID <= 0)
            {
                return 0;
            }
            return serverData.clothesFashionID;
        }
    }

    /// <summary>
    /// 武器时装ID
    /// </summary>
    public int WeaponFashionID
    {
        get
        {
            if (serverData.weaponFashionID <= 0)
            {
                return 0;
            }
            return serverData.weaponFashionID;
        }
    }

    public int SmallStrenLev
    {
        get
        {
            return serverData.smallStrenLev;
        }
    }

    public void UpdateMount(MountInfo _mount)
    {
        if (_mount != null)
        {
            curMountInfo = _mount;
            if (OnMountUpdate != null)
            {
                OnMountUpdate();
            }
        }
    }
	/// <summary>
	/// 玩家身上穿的装备数据
	/// </summary>
	public List<EquipmentInfo> RealEquipmentInfoList
	{
		get
		{
			return (serverData != null?serverData.realEquipmentInfo:new List<EquipmentInfo>());
		}
	}

    /// <summary>
    /// 玩家等级  by吴江
    /// </summary>
    public ulong Level
    {
        get
        {
            if (serverData != null && serverData.baseValueDic.ContainsKey(ActorBaseTag.Level))
            {
				return serverData.baseValueDic[ActorBaseTag.Level];
            }
            return 0;
        }
    }
    /// <summary>
    /// VIP等级
    /// </summary>
    public int VipLev
    {
        get
        {
            if (GameCenter.mainPlayerMng != null && ServerInstanceID == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
            {
                if (GameCenter.vipMng != null && GameCenter.vipMng.VipData != null)
                    return GameCenter.vipMng.VipData.vLev;
            }
            else
            {
                return serverData.vipLev;
            }
            return 0;
        }
    }
	
	public string LevelDes
	{
		get
		{
			AttributeRef attributeRef = ConfigMng.Instance.GetAttributeRef(Level > 0 ? (int)Level : 1);
			if(attributeRef.reborn > 0){
				return ConfigMng.Instance.GetUItext(12,new string[2]{attributeRef.reborn.ToString(),attributeRef.display_level.ToString()});
			}else{
				return ConfigMng.Instance.GetUItext(13,new string[1]{attributeRef.display_level.ToString()});
			}
//			return "";
		}
	}

    public ulong CurExp
    {
        get
        {
            if (serverData.baseValueDic.ContainsKey(ActorBaseTag.Exp))
            {
                return serverData.baseValueDic[ActorBaseTag.Exp];
            }
            return 0;
        }
    }
    /// <summary>
    /// 最大经验值
    /// </summary>
    public ulong MaxExp
    {
        get
        {
			return (ulong)ConfigMng.Instance.GetAttributeRef((int)Level).max_exp;
        }
    }
    /// <summary>
    /// 战力值 
    /// </summary>
    public int FightValue
    {
        get
        {
            if (serverData.baseValueDic.ContainsKey(ActorBaseTag.FightValue))
            {
				return (int)(serverData.baseValueDic[ActorBaseTag.FightValue]);
            }
            return 0;
        }
    }
	/// <summary>
	/// 是否处在反击状态中  by邓成
	/// </summary>
	public bool IsCounterAttack
	{
		get
		{
			if (serverData.baseValueDic.ContainsKey(ActorBaseTag.CounterAttack))
			{
				return (serverData.baseValueDic[ActorBaseTag.CounterAttack] == 1);
			}
			return false;
		}
	}

    /// <summary>
    /// 暴击 
    /// </summary>
    public int Crit
    {
        get
        {
            if (serverData.propertyValueDic.ContainsKey(ActorPropertyTag.CRI))
            {
                return serverData.propertyValueDic[ActorPropertyTag.CRI];
            }
            return 0;
        }
    }
    /// <summary>
    /// 闪避 
    /// </summary>
    public int Dodge
    {
        get
        {
            if (serverData.propertyValueDic.ContainsKey(ActorPropertyTag.DOD))
            {
                return serverData.propertyValueDic[ActorPropertyTag.DOD];
            }
            return 0;
        }
    }   
    /// <summary>
    /// 命中 
    /// </summary>
    public int Hit
    {
        get
        {
            if (serverData.propertyValueDic.ContainsKey(ActorPropertyTag.HIT))
            {
                return serverData.propertyValueDic[ActorPropertyTag.HIT];
            }
            return 0;
        }
    }
    /// <summary>
    /// 移动速度  by 贺丰 这样的属性是作为所有玩家的属性，所以写在这里
    /// </summary>
    public int Movespd
    {
        get
        {
            if (serverData.propertyValueDic.ContainsKey(ActorPropertyTag.MOVESPD))
            {
                return serverData.propertyValueDic[ActorPropertyTag.MOVESPD];
            }
            return 0;
        }
    }
    /// <summary>
    /// 韧性
    /// </summary>
    public int Tough
    {
        get
        {
            if (serverData.propertyValueDic.ContainsKey(ActorPropertyTag.TOUGH))
            {
                return serverData.propertyValueDic[ActorPropertyTag.TOUGH];
            }
            return 0;
        }
    }
	/// <summary>
	/// 杀戮值
	/// </summary>
	/// <value>The killing value.</value>
	public int KillingValue
	{
		get
		{
			if (serverData.baseValueDic.ContainsKey(ActorBaseTag.SLAVALUE))
			{
				return (int)serverData.baseValueDic[ActorBaseTag.SLAVALUE];
			}
			return 0;
		}
	}

	public string KillingValueName
	{
		get
		{
			switch(SlaLevel)//玩家名字的颜色由杀戮值决定
			{
			case 1:
				return ConfigMng.Instance.GetUItext(221);
			case 2:
                return ConfigMng.Instance.GetUItext(222);
			case 3:
                return ConfigMng.Instance.GetUItext(223);
			case 4:
                return ConfigMng.Instance.GetUItext(224);
			case 5:
                return ConfigMng.Instance.GetUItext(225);
			default:
                return ConfigMng.Instance.GetUItext(221);
			}
		}
	}
	/// <summary>
	/// 幸运值
	/// </summary>
	/// <value>The killing value.</value>
	public int LuckyValue
	{
		get
		{
			if (serverData.propertyValueDic.ContainsKey(ActorPropertyTag.LUCKY))
			{
				return serverData.propertyValueDic[ActorPropertyTag.LUCKY];
			}
			return 0;
		}
	}
	/// <summary>
	/// 攻击上下限
	/// </summary>
	/// <value>The minimum attack.</value>
	public string AttackStr{
		get{
			int atk = 0;
			if (serverData.propertyValueDic.ContainsKey(ActorPropertyTag.ATK))
			{
				atk = serverData.propertyValueDic[ActorPropertyTag.ATK];
			}
			return (MinAttack + atk)+"-"+(MaxAttack + atk);
		}
	}
	/// <summary>
	/// 攻击下限
	/// </summary>
	/// <value>The minimum attack.</value>
	public int MinAttack
	{
		get
		{
			if (serverData.propertyValueDic.ContainsKey(ActorPropertyTag.ATKDOWN))
			{
				return serverData.propertyValueDic[ActorPropertyTag.ATKDOWN];
			}
			return 0;
		}
	}
	/// <summary>
	/// 攻击上限
	/// </summary>
	/// <value>The minimum attack.</value>
	public int MaxAttack
	{
		get
		{
			if (serverData.propertyValueDic.ContainsKey(ActorPropertyTag.ATKUP))
			{
				return serverData.propertyValueDic[ActorPropertyTag.ATKUP];
			}
			return 0;
		}
	}
	/// <summary>
	/// 防御上下限
	/// </summary>
	/// <value>The minimum attack.</value>
	public string DefStr{
		get{
			int def = 0;
			if (serverData.propertyValueDic.ContainsKey(ActorPropertyTag.DEF))
			{
				def = serverData.propertyValueDic[ActorPropertyTag.DEF];
			}
			return (MinDef + def)+"-"+(MaxDef + def);
		}
	}
	/// <summary>
	/// 防御下限
	/// </summary>
	/// <value>The minimum attack.</value>
	public int MinDef
	{
		get
		{
			if (serverData.propertyValueDic.ContainsKey(ActorPropertyTag.DEFDOWN))
			{
				return serverData.propertyValueDic[ActorPropertyTag.DEFDOWN];
			}
			return 0;
		}
	}
	/// <summary>
	/// 攻击上限
	/// </summary>
	/// <value>The minimum attack.</value>
	public int MaxDef
	{
		get
		{
			if (serverData.propertyValueDic.ContainsKey(ActorPropertyTag.DEFUP))
			{
				return serverData.propertyValueDic[ActorPropertyTag.DEFUP];
			}
			return 0;
		}
	}

    /// <summary>
    /// 静态配置速度 by吴江
    /// </summary>
    public int StaticSpeed
    {
        get
        {
			return RefData == null?0:RefData.baseMoveSpd;
        }
    }

    /// <summary>
    /// 骨骼名称
    /// </summary>
    public override string Bone_Name
    {
        get
        {
            return RefData.bone;
        }
    }

    /// <summary>
    /// 名字高度
    /// </summary>
    public new float NameHeight
    {
        get
        {
            return RefData == null ? 5 : RefData.name_height;
        }
    }

    /// <summary>
    /// 当前骑乘的坐骑数据
    /// </summary>
    public MountInfo CurMountInfo
    {
        get
        {
            return curMountInfo;
        }
    }

	public List<SkillInfo> CurSkillList
	{
		get
		{
			return serverData.skillList;
		}
	}


    /// <summary>
    /// 头像图片名
    /// </summary>
    public string HeadIconName
    {
        get
        {
            return RefData == null ? string.Empty : RefData.res_head_Icon;
        }
    }

    #endregion


}
