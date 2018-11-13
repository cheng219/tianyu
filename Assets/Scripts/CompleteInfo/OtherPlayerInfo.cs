//====================================
//作者：吴江
//日期：2015/7/7
//用途：其他玩家数据层对象（Info结尾的类名都为数据层对象，包含 服务端数据  客户端静态数据   访问器 三部分）
//=====================================




using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;


public class OtherPlayerData : PlayerBaseData
{

    public int sceneID;
    public int teamID;
    public int leaderID;
	public OtherPlayerData(){
		
	}

    public OtherPlayerData(st.net.NetBase.scene_ply _info)
    {
        serverInstanceID = (int)_info.pid;
        name = _info.name;
        guildName = _info.guildName;
        baseValueDic[ActorBaseTag.Level] = _info.level;
        prof = (int)_info.prof;
        startPosX = _info.x;
        startPosY = _info.y;
        startPosZ = _info.z;
        dir = (int)_info.dir;
        camp = (int)_info.camp;
        teamID = (int)_info.team_id;
        titleID = (int)_info.title;
        leaderID = (int)_info.team_leader;
        smallStrenLev = _info.strenthen_min_lev;
		equipTypeList.Clear();
		if(_info.magic_weapon_id > 0)
		{
			RefineRef rr = ConfigMng.Instance.GetRefineRef(_info.magic_weapon_id, _info.magic_strength_lev, _info.magic_strength_star);
			if (rr != null)
			{
				magicWeaponID = rr.model;
			}
		}
		if (_info.wing_id > 0)
		{
			WingRef data = ConfigMng.Instance.GetWingRef(_info.wing_id, _info.wing_lev);
			if (data != null)
			{
				equipTypeList.Add(data.itemui);
			}
		}
		for (int i = 0,max=_info.model_clothes_id.Count; i < max; i++) {
			equipTypeList.Add((int)_info.model_clothes_id[i]);
		}

        propertyValueDic[ActorPropertyTag.HPLIMIT] = (int)_info.max_hp;
		propertyValueDic[ActorPropertyTag.MOVESPD] = (int)_info.move_speed;
        baseValueDic[ActorBaseTag.CurHP] = _info.hp;
        baseValueDic[ActorBaseTag.FightValue] = _info.fighting;
		baseValueDic[ActorBaseTag.SLALEVEL] = _info.pk_level;
		baseValueDic[ActorBaseTag.CounterAttack] = (uint)_info.counter_status;
		//Debug.Log("_info.move_speed:"+_info.move_speed);
    }

    public OtherPlayerData(st.net.NetBase.scene_model _info)
    {
        serverInstanceID = (int)_info.id;
        name = _info.name;
        prof = (int)_info.prof;
        startPosX = _info.x;
        startPosY = _info.y;
        startPosZ = _info.z;
        dir = (int)_info.dir;
        camp = (int)_info.camp;
        equipTypeList.Clear();
        for (int i = 0; i < _info.equip_list.Count; i++)
        {
            equipTypeList.Add((int)_info.equip_list[i]);
        }
    }
    /// <summary>
    /// 玩家出视野,再进视野,需更新信息
    /// </summary>
    /// <param name="_info"></param>
    public void Update(st.net.NetBase.scene_ply _info)
    {
        serverInstanceID = (int)_info.pid;
        name = _info.name;
        guildName = _info.guildName;
        baseValueDic[ActorBaseTag.Level] = _info.level;
        prof = (int)_info.prof;
        startPosX = _info.x;
        startPosY = _info.y;
        startPosZ = _info.z;
        dir = (int)_info.dir;
        camp = (int)_info.camp;
        teamID = (int)_info.team_id;
        titleID = (int)_info.title;
        leaderID = (int)_info.team_leader;
        smallStrenLev = _info.strenthen_min_lev;

        propertyValueDic[ActorPropertyTag.HPLIMIT] = (int)_info.max_hp;
        propertyValueDic[ActorPropertyTag.MOVESPD] = (int)_info.move_speed;
        baseValueDic[ActorBaseTag.CurHP] = _info.hp;
        baseValueDic[ActorBaseTag.FightValue] = _info.fighting;
        baseValueDic[ActorBaseTag.SLALEVEL] = _info.pk_level;
        baseValueDic[ActorBaseTag.CounterAttack] = (uint)_info.counter_status;
    }
}



/// <summary>
/// 其他玩家数据层对象（Info结尾的类名都为数据层对象，包含 服务端数据  客户端静态数据   访问器 三部分） by吴江
/// </summary>
public class OtherPlayerInfo : PlayerBaseInfo
{

    /// <summary>
    /// 服务端数据
    /// </summary>
    protected new OtherPlayerData serverData
    {
        get { return base.serverData as OtherPlayerData; }
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
    public System.Action<ActorBaseTag, int> OnBaseDiffUpdate;
    /// <summary>
    /// 构造  by吴江
    /// </summary>
    /// <param name="_actorData"></param>
    public OtherPlayerInfo(st.net.NetBase.scene_ply _info, List<int> _equipList,bool _isInFight)
    {
        serverData = new OtherPlayerData(_info);

		if(_equipList != null && _equipList.Count > 0)serverData.equipTypeList.AddRange(_equipList);
        serverData.isInFight = _isInFight;
		IsAlive = serverData.baseValueDic.ContainsKey(ActorBaseTag.CurHP)?(serverData.baseValueDic[ActorBaseTag.CurHP] != 0):true;//其他玩家是否死亡和复活

		List<int> defaultEquipList = RefData==null?new List<int>():RefData.defaultEquipList;
        foreach (var item in defaultEquipList)
        {
            EquipmentInfo eq = new EquipmentInfo(item, EquipmentBelongTo.EQUIP);
            if (defaultDictionary.ContainsKey(eq.Slot))
            {
                GameSys.LogError(ConfigMng.Instance.GetUItext(213));
            }
            defaultDictionary[eq.Slot] = eq;
        }
        RefineRef rr = ConfigMng.Instance.GetRefineRef(_info.magic_weapon_id, _info.magic_strength_lev, _info.magic_strength_star);
        if (rr != null)
        {
            serverData.magicWeaponID = rr.model;
            if (serverData.equipTypeList.Contains(serverData.magicWeaponID))
            {
                if (!GameCenter.systemSettingMng.OtherPlayerMagic)
                {
                    serverData.equipTypeList.Remove(serverData.magicWeaponID);
                }
            }
            else
            {
                if (GameCenter.systemSettingMng.OtherPlayerMagic)
                {
                    serverData.equipTypeList.Add(serverData.magicWeaponID);
                }
            }
        }
        WingRef data = null;
        if (_info.wing_id > 0)
        {
            data = ConfigMng.Instance.GetWingRef(_info.wing_id, _info.wing_lev);
        }
        if (data != null)
        {
            serverData.wingID = data.itemui;
            if (serverData.equipTypeList.Contains(serverData.wingID))
            {
                if (!GameCenter.systemSettingMng.OtherPlayerWing)
                {
                    serverData.equipTypeList.Remove(serverData.wingID);
                }
            }
            else
            {
                if (GameCenter.systemSettingMng.OtherPlayerWing)
                {
                    serverData.equipTypeList.Add(serverData.wingID);
                }
            }
        }



        ProcessServerData(serverData);

        if (_info.ride_type > 0)
        {
            curMountInfo = new MountInfo(_info,this);
        }

    }
	public OtherPlayerInfo(OtherPlayerData data)
	{
		serverData = data;
	}

//	public OtherPlayerInfo(SceneAnimActionRef _refData)
//	{
//		if (_refData == null || _refData.values.Count < 4)
//		{
//			GameSys.LogError("参数个数错误,构造玩家信息失败!");
//			return;
//		}
//		serverData = new OtherPlayerData();
//		serverData.serverInstanceID = _refData.targetInstanceID;
//		serverData.prof = _refData.targetConfigID;
//		serverData.startPosX = (int)(_refData.values[0] / 2.0f);
//		serverData.startPosZ = (int)(_refData.values[1] / 2.0f);
//		serverData.dir = _refData.values[2];
//		serverData.camp = _refData.values[3];
//		if (_refData.texts.Count > 0)
//		{
//			serverData.name = _refData.texts[0];
//		}
//		isActor = true;
//		ProcessServerData(serverData);
//	}

    /// <summary>
    /// 构造  by吴江
    /// </summary>
    /// <param name="_actorData"></param>
    public OtherPlayerInfo(st.net.NetBase.scene_model _info)
    {
        serverData = new OtherPlayerData(_info);


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
    }


    public void Update(List<int> _equipList,scene_ply _data)
    {
        serverData.equipTypeList.Clear();
		if(_equipList != null && _equipList.Count > 0)serverData.equipTypeList.AddRange(_equipList);
        
        IsAlive = _data.hp > 0;
        serverData.Update(_data);
        //更新移动速度
        UpdateMoveSpeed();

        UpdateGuildName(_data.guildName);
        //强化特效更新
        UpdateStrengEffect(_data.strenthen_min_lev);
        //坐骑更新
        if (curMountInfo == null)
        {
            if (_data.ride_type != 0)
            {
                curMountInfo = new MountInfo(_data, this);
                UpdateMount(curMountInfo);
            }
        }
        else
        {
            curMountInfo.Update(_data);
        }
        //法宝更新
        if (_data.magic_weapon_id > 0)
        {
            RefineRef rr = ConfigMng.Instance.GetRefineRef(_data.magic_weapon_id, _data.magic_strength_lev, _data.magic_strength_star);
            if (rr != null)
            {
                serverData.magicWeaponID = rr.model;
                if (GameCenter.systemSettingMng.OtherPlayerMagic)
                {
                    serverData.equipTypeList.Add(rr.model);
                }
            }
        }
        //翅膀更新
        if (_data.wing_id > 0)
        {
            WingRef data = ConfigMng.Instance.GetWingRef(_data.wing_id, _data.wing_lev);
            if (data != null)
            {
                serverData.wingID = data.itemui;
                if (GameCenter.systemSettingMng.OtherPlayerWing)
                {
                    serverData.equipTypeList.Add(data.itemui);
                }
            }
        }
        for (int i = 0, max = _data.model_clothes_id.Count; i < max; i++)
        {
            serverData.equipTypeList.Add(_data.model_clothes_id[i]);
        }
        ProcessServerData(serverData);
    }
     
	public void Update(scene_ply _data)
	{
        IsAlive = _data.hp > 0;
        UpdateName(_data.name);//放在serverData.Update(_data);之前
        UpdateGuildName(_data.guildName); //放在serverData.Update(_data);之前
        serverData.Update(_data);
		serverData.equipTypeList.Clear();
        //更新移动速度
        UpdateMoveSpeed();
        //强化特效更新
        UpdateStrengEffect(_data.strenthen_min_lev);
		//坐骑更新
		if (curMountInfo == null)
		{
			if(_data.ride_type != 0)
			{
				curMountInfo = new MountInfo(_data, this);
				UpdateMount(curMountInfo);
			}
		}
		else
		{
			curMountInfo.Update(_data);
		}
		//法宝更新
		if(_data.magic_weapon_id > 0)
		{
			RefineRef rr = ConfigMng.Instance.GetRefineRef(_data.magic_weapon_id, _data.magic_strength_lev, _data.magic_strength_star);
			if (rr != null)
			{
				serverData.magicWeaponID = rr.model;
                if (GameCenter.systemSettingMng.OtherPlayerMagic)
                {
                    serverData.equipTypeList.Add(rr.model);
                }
			}
		}
		//翅膀更新
		if (_data.wing_id > 0)
		{
			WingRef data = ConfigMng.Instance.GetWingRef(_data.wing_id, _data.wing_lev);
			if (data != null)
			{
				serverData.wingID = data.itemui;
                if (GameCenter.systemSettingMng.OtherPlayerWing)
                {
                    serverData.equipTypeList.Add(data.itemui);
                }
			}
		}
		for (int i = 0,max=_data.model_clothes_id.Count; i < max; i++) {
			serverData.equipTypeList.Add(_data.model_clothes_id[i]);
		}
		ProcessServerData(serverData);
	}




    /// <summary>
    /// 刷新翅膀以及法宝显示
    /// </summary>
    public void RefreshWingAndMagic()
    {
            if(GameCenter.systemSettingMng.OtherPlayerMagic)
            {
                if(!serverData.equipTypeList.Contains(serverData.magicWeaponID))
                {
                   serverData.equipTypeList.Add(serverData.magicWeaponID);
                }
            }
            else
            {
                if(serverData.equipTypeList.Contains(serverData.magicWeaponID))
                {
                   serverData.equipTypeList.Remove(serverData.magicWeaponID);
                }
            }
            if (GameCenter.systemSettingMng.OtherPlayerWing)
            {
                if (!serverData.equipTypeList.Contains(serverData.wingID))
                {
                    serverData.equipTypeList.Add(serverData.wingID);
                }

            }
            else
            {
                if (serverData.equipTypeList.Contains(serverData.wingID))
                {
                    serverData.equipTypeList.Remove(serverData.wingID);
                }
            }
        ProcessServerData(serverData);
    }




    /// <summary>
    /// 队伍改变
    /// </summary>
    /// <param name="_data"></param>
    public void Update(pt_scene_usr_team_chg_c018 _data)
    {
        serverData.teamID = (int)_data.team_id;
        serverData.leaderID = (int)_data.team_leader;
    }


    public void Jump()
    {
        if (OnJump != null)
        {
            OnJump();
        }
    }

    public System.Action OnJump;

    /// <summary>
    /// 是否在队伍中
    /// </summary>
    public bool IsInTeam
    {
        get
        {
            return serverData.teamID > 0;
        }
    }

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
    /// 复活。目前的游戏机制是回主城直接复活。因此没走服务端验证。下个项目不能这样 。 by吴江
    /// </summary>
    //public void ReLive()
    //{
    //    actorData.baseDictionary[ActorBaseTag.CurHP] = actorData.propertyDictionary[ActorPropertyTag.MaxHP];
    //    actorData.baseDictionary[ActorBaseTag.CurMP] = actorData.propertyDictionary[ActorPropertyTag.MaxMP];
    //    IsAlive = true;
    //}



    #region 访问器
    /// <summary>
    /// 队伍id
    /// </summary>
    public int TeamID
    {
        get
        {
            return serverData.teamID;
        }
    }

    /// <summary>
    /// 队长ID
    /// </summary>
    public int LeaderID
    {
        get
        {
            return serverData.leaderID;
        }
    }

    public int PetInstanceID
    {
        get
        {
            //return petInfo == null ? -1 : petInfo.ServerInstanceID;
            return 0;//这里是删除魔兽后修改的地方
        }
    }

    #endregion



}

	
	