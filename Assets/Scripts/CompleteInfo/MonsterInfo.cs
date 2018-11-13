//====================================
//作者：吴江
//日期：2015/7/18
//用途: 怪物数据层对象（Info结尾的类名都为数据层对象，包含 服务端数据  客户端静态数据   访问器 三部分）
//=====================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MonsterData : ActorData
{
    public int sceneID;
    public int ownerID;
	public int dartOwnerID;
    public string dartOwnerName;

    public MonsterData(st.net.NetBase.scene_monster _info)
    {
        serverInstanceID = (int)_info.mid;
        prof = (int)_info.type;
        startPosX = _info.x;
        startPosY = _info.y;
        startPosZ = _info.z;
        dir = (int)_info.dir;
        camp = (int)_info.camp;
        ownerID = _info.ownerID;
        dartOwnerName = _info.cart_owner_name;
		dartOwnerID = (int)_info.cart_owner;
        propertyValueDic[ActorPropertyTag.HPLIMIT] = (int)_info.max_hp;
        baseValueDic[ActorBaseTag.CurHP] = _info.hp;
        baseValueDic[ActorBaseTag.Level] = _info.level;
    }

    public void Update(st.net.NetBase.scene_monster _info)
    {
        serverInstanceID = (int)_info.mid;
        baseValueDic[ActorBaseTag.Level] = _info.level;
        prof = (int)_info.type;
        startPosX = _info.x;
        startPosY = _info.y;
        startPosZ = _info.z;
        dir = (int)_info.dir;
        camp = (int)_info.camp;
        ownerID = _info.ownerID;
		dartOwnerID = (int)_info.cart_owner;
        dartOwnerName = _info.cart_owner_name;
        propertyValueDic[ActorPropertyTag.HPLIMIT] = (int)_info.max_hp;
        baseValueDic[ActorBaseTag.CurHP] = _info.hp;
    }

    public MonsterData()
    {
    }
}



public class MonsterInfo : ActorInfo
{


    /// <summary>
    /// 服务端数据
    /// </summary>
    protected new MonsterData serverData
    {
        get { return base.serverData as MonsterData; }
        set
        {
            base.serverData = value;
        }
    }
		
//	public MonsterInfo(SceneAnimActionRef _refData)
//	{
//		serverData = new MonsterData();
//		serverData.serverInstanceID = _refData.targetInstanceID;
//		serverData.prof = _refData.targetConfigID;
//		serverData.startPosX = (int)(_refData.values[0] / 2.0f);
//		serverData.startPosZ = (int)(_refData.values[1] / 2.0f);
//		serverData.dir = _refData.values[2];
//		serverData.camp = _refData.values[3];
//		isActor = true;
//		colliderRadius = RefData.cmodel_r;
//	}

    public MonsterInfo(MonsterData _actorData)
    {
        serverData = _actorData;
		colliderRadius = RefData != null?RefData.cmodel_r:1.0f;

        ProcessServerData(serverData);

    }

    public MonsterInfo(st.net.NetBase.scene_monster _info)
    {
        serverData = new MonsterData(_info);
        colliderRadius = RefData.cmodel_r;

        ProcessServerData(serverData);

    }


    public void Update(st.net.NetBase.scene_monster _info)
    {
        serverData.Update(_info);
        if (OnBaseUpdate != null)
        {
            OnBaseUpdate(ActorBaseTag.CurHP, serverData.baseValueDic[ActorBaseTag.CurHP],false);
        }
    }


    /// <summary>
    /// 限预览用
    /// </summary>
    /// <param name="_refData"></param>
    public MonsterInfo(MonsterRef _refData)
    {
        serverData = new MonsterData();
        serverData.serverInstanceID = -1;
        serverData.prof = _refData.id;
        refData = _refData;
        colliderRadius = _refData.cmodel_r;

        ProcessServerData(serverData);

    }



    //public MonsterInfo(SceneAnimActionRef _refData)
    //{
    //    actorData = new ServerData.MonsterData();
    //    actorData.id = _refData.targetInstanceID;
    //    actorData.type = _refData.targetConfigID;
    //    actorData.serverPosX = (int)(_refData.values[0] / 2.0f);
    //    actorData.serverPosY = (int)(_refData.values[1] / 2.0f);
    //    actorData.dir = _refData.values[2];
    //    actorData.camp = _refData.values[3];
    //    isActor = true;
    //    colliderRadius = RefData.cmodel_r;
    //}

    /// <summary>
    /// 客户端静态数据
    /// </summary>
    protected MonsterRef refData = null;
    protected MonsterRef RefData
    {
        get
        {
            if (refData == null || refData.id != serverData.prof)
            {
                refData = ConfigMng.Instance.GetMonsterRef(serverData.prof);
            }
            return refData;
        }
    }



    protected override void ProcessServerData(ActorData _data)
    {
        List<EquipmentInfo> curEquipList = new List<EquipmentInfo>();
		if(RefData != null)
		{
			for (int i = 0; i < RefData.equipList.Count; i++)
			{
				EquipmentInfo eq = new EquipmentInfo(RefData.equipList[i], EquipmentBelongTo.PREVIEW);
				DefaultDictionary[eq.Slot] = eq;
			}
		}
        UpadateEquipments(curEquipList);
    }



    public void UpdateOwner(int _ownerID)
    {
        if (serverData.ownerID != _ownerID)
        {
            serverData.ownerID = _ownerID;
            if (OnOwnerUpdate != null)
            {
                OnOwnerUpdate();
            }
        }
    }
    public System.Action OnOwnerUpdate;

    public System.Action<SustainRef> onStartCollectEvent;
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

    public int MonsterID
    {
        get
        {
            return serverData.prof;
        }

        
    }

    public new string Name
    {
        get
        {
            if (RefData == null)
            {
                return string.Empty;
            }
            if (IsDart)
            {
                return UIUtil.Str2Str(ConfigMng.Instance.GetUItext(122), new string[] { serverData.dartOwnerName,RefData.name});
            }
            if (RefData != null)
            {
                return RefData.name;
            }
            return string.Empty;
        }
    }
    public bool IsDart
    {
        get
        {
            return (RankLevel == MobRankLevel.DAILYDART || RankLevel == MobRankLevel.GUILDDART);
        }
    }
    /// <summary>
    /// 镖车是否是自己的(或者自己公会的)
    /// </summary>
    public bool IsOwnerDart
    {
        get
        {
            if (RankLevel == MobRankLevel.DAILYDART)
            {
                return (DartOwnerID == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID);
            }
            if (RankLevel == MobRankLevel.GUILDDART)
            {
                if (GameCenter.guildMng.MyGuildInfo == null)
                    return false;
                return (DartOwnerID == GameCenter.guildMng.MyGuildInfo.GuildId);
            }
            return false;
        }
    }

    public int OwnerID
    {
        get
        {
            return serverData.ownerID;
        }
    }
	/// <summary>
	/// 镖车从属,每日镖车属于个人玩家,仙盟镖车属于公会(ID)
	/// </summary>
	public int DartOwnerID
	{
		get
		{
			return serverData.dartOwnerID;
		}
	}

    public int Level
    {
        get { return RefData.lv; }
    }

    public float Hight
    {
        get { return RefData.model_y; }
    }



    /// <summary>
    /// 骨骼名称
    /// </summary>
    public override string Bone_Name
    {
        get
        {
            return RefData.boneName;
        }
    }

    public float NameHight
    {
        get
        {
            return RefData == null ? 0 : RefData.name_height / 100f;
        }
    }

    /// <summary>
    /// 自定义颜色
    /// </summary>
    public Color CustumColor
    {
        get
        {
            return RefData == null ? Color.white : RefData.color;
        }
    }

	public float RingSize
	{
		get
		{
			return RefData == null ? 0 : RefData.displaySize;
		}
	}

    public string DeadEffectName
    {
        get
        {
            return RefData == null ? string.Empty : RefData.dead_action;
        }
    }


    /// <summary>
    /// 模型大小
    /// </summary>
    public override float ModelScale
    {
        get
        {
            return RefData == null ? 1 : RefData.model_scale /10000f;
        }
    }
    /// <summary>
    /// 预览时的缩放比
    /// </summary>
    public float PreviewScale(PreviewConfigType _previewType)
    {
        if (RefData != null)
        {
            switch (_previewType)
            {
                case PreviewConfigType.Dialog:
                    return RefData.preview_scale;
                case PreviewConfigType.Task:
                    return RefData.taskPreviewScale;
                default:
                    return 1;
            }
        }
        return 1;
    }
    /// <summary>
    /// 预览时的相机距离
    /// </summary>
    public Vector3 PreviewPosition(PreviewConfigType _previewType)
    {
        if (RefData != null)
        {
            switch (_previewType)
            {
                case PreviewConfigType.Dialog:
                    return RefData.previewPscale;
                case PreviewConfigType.Task:
                    return RefData.taskpreviewPscale;
                default:
                    return Vector3.zero;
            }
        }
        return Vector3.zero;

    }
        
	/// <summary>
    /// 预览时的相机角度
    /// </summary>
	public Vector3 PreviewRotation(PreviewConfigType _previewType)
    {
        if (RefData != null)
        {
            switch (_previewType)
            {
                case PreviewConfigType.Dialog:
                    return RefData.previewRscale;
                case PreviewConfigType.Task:
                    return RefData.taskpreviewRscale;
                default:
                    return Vector3.zero;
            }
        }
        return Vector3.zero;
    }

    public float StaticSpeed
    {
        get
        {
            return RefData.baseMoveSpd;
        }
    }

    /// <summary>
    /// 怪物级别
    /// </summary>
	public MobRankLevel RankLevel
    {
        get
        {
			return  RefData ==null?MobRankLevel.NORMAL:(MobRankLevel)RefData.rank_level;
        }
    }

    public int NormalAbility
    {
        get
        {
            return RefData ==null ? 0: RefData.skill_public;
        }
    }


    public List<int> SpecialAbility
    {
        get
        {
            return RefData == null ? null : RefData.skill;
        }
    }


    public int ScaleType
    {
        get { return RefData.volume; }
    }


    public string IconName
    {
        get
        {
            return RefData.res;
        }
    }
	/// <summary>
	/// 是否是花车
	/// </summary>
	public bool IsSedan
	{
		get
		{
			return RefData == null ? false : (RankLevel == MobRankLevel.SEDAN);
		}
	}

    /// <summary>
    /// 是否为BOSS
    /// </summary>
    public bool IsBoss
    {
        get
        {
			return RefData == null ? false : (RankLevel == MobRankLevel.BOSS);
        }
    }
    /// <summary>
    /// 是否为精英怪
    /// </summary>
    public bool IsElite
    {
        get
        {
            return RefData == null ? false : (RankLevel == MobRankLevel.ELITE);
        }
    }
    /// <summary>
    /// BOSS有几管血
    /// </summary>
    public int BossHpNum
    {
        get
        {
            return RefData == null ? 1 : RefData.blood_volume;
        }
    }
    /// <summary>
    /// 动画资源标准移动速度
    /// </summary>
    public override float AnimationMoveSpeedBase
    {
        get
        {
            return RefData == null ? base.AnimationMoveSpeedBase : RefData.paceSpeed;
        }
    }

    /// <summary>
    /// 骨骼名称
    /// </summary>
    public string BoneName
    {
        get { return RefData == null ? string.Empty : RefData.boneName; }
    }

    /// <summary>
    /// 出生动画 by吴江
    /// </summary>
    public string BornAnim
    {
        get { return RefData == null ? string.Empty : RefData.bornAnim; }
        
    }
    /// <summary>
    /// 出生特效 by吴江
    /// </summary>
    public string BornEffect
    {
        get { return RefData == null ? string.Empty : RefData.bornEffect; }

    }


    public float DeadDisapearTime
    {
        get
        {
            return RefData == null ? 0.0f : RefData.corpse_remain_time;
        }
    }

    #endregion

    public void Update(MonsterData _actorData)
    {
        serverData = _actorData;
        if (OnPropertyUpdate != null)
        {
            OnPropertyUpdate(ActorPropertyTag.TOTAL, 0,false);
        }
    }

}
