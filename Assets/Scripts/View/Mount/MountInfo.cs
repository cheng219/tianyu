//====================================
//3D  ： 吴江
//界面： 朱素云
//日期：2016/4/25
//用途: 坐骑数据层对象（Info结尾的类名都为数据层对象，包含 服务端数据  客户端静态数据   访问器 三部分）
//=====================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class MountData : ActorData
{
    public int configID; 
    public bool isRiding = false;
    public bool hasInit = false;
    public int fightValue;
    public int lev = 0;
    public int curChangeId = 0;//0为无幻化状态
    public int skinReminTime = 0;//幻化剩余时间
    public int state;//4为当前化形的坐骑
     
	public MountData(int mountId,int mountLv)
	{
		configID = mountId;
        lev = mountLv;
		isRiding = false;
	}
    public MountData(int mountId, int mountLv, int changeId)
    {
        configID = mountId;
        lev = mountLv;
        curChangeId = changeId; 
    }
    public MountData(st.net.NetBase.ride_list _msg, int _lev)
    {
        configID = _msg.ride_id;
        isRiding = _msg.ride_state == 1;
        state = _msg.ride_state;
        lev = _lev;
    } 
    public MountData(pt_update_ride_lev_d439 _msg)
    {
        configID = (int)_msg.ride_id;
        lev = (int)_msg.state;
    }

    public void Update(st.net.NetBase.ride_list _msg)
    {
        configID = (int)_msg.ride_id;
        isRiding = _msg.ride_state == 1;
        state = _msg.ride_state;
    }

    public void Update(pt_update_ride_state_d440 _msg)
    {
        configID = (int)_msg.ride_id;
        isRiding = _msg.state == 1; 
    }

    public void Update(pt_update_ride_lev_d439 _msg)
    {
        configID = (int)_msg.ride_id;
        lev = (int)_msg.state;
    } 
    public MountData()
    {
       
    }

    #region 幻化

    public MountData(st.net.NetBase.skin_base_info _msg)
    {
        configID = (int)_msg.skin_id;
        curChangeId = (int)_msg.skin_state;
        skinReminTime = (int)_msg.remain_time;
        isRiding = _msg.skin_state == 1;
    }
    public void Update(st.net.NetBase.skin_base_info _msg)
    {
        configID = (int)_msg.skin_id;
        curChangeId = (int)_msg.skin_state;
        skinReminTime = (int)_msg.remain_time;
        isRiding = _msg.skin_state == 1;
    }

    #endregion
}


public class MountInfo : ActorInfo
{

    public MountInfo()
    {

    }

    /// <summary>
    /// 服务端数据
    /// </summary>
    protected new MountData serverData
    {
        get { return base.serverData as MountData; }
        set
        {
            base.serverData = value;
        }
    } 
    /// <summary>
    /// 是否被服务端校正过数据
    /// </summary>
    protected bool hasInited = false;

    protected PlayerBaseInfo ownerInfo;

    /// <summary>
    /// 以服务端数据和PlayerBaseInfo构造MountInfo
    /// </summary> 
    public MountInfo(MountData _actorData, PlayerBaseInfo _info)
    {
        hasInited = true;
        serverData = _actorData;
        ownerInfo = _info;
        if (RefData != null)
        {
            previewItem = new EquipmentInfo(RefData.itemID, 1, EquipmentBelongTo.PREVIEW);
        }
        if (ownerInfo != null && _actorData.isRiding)
        { 
            ownerInfo.UpdateMount(this);
        }
        ProcessLevEffect();
    }
    public MountInfo(MountData _actorData, PlayerBaseInfo _info, bool _isSkin)
    {
        hasInited = true;
        serverData = _actorData;
        ownerInfo = _info;
        if (RefData != null)
        {
            previewItem = new EquipmentInfo(RefData.itemID, 1, EquipmentBelongTo.PREVIEW);
        } 
        ProcessLevEffect();
    }
    /// <summary>
    /// 以客户端数据构造MountInfo，取管理类里面的等级，特效需要
    /// </summary> 
    public MountInfo(MountRef _mountRef)
    {
        refData = _mountRef;
        serverData = new MountData();
        hasInited = false;
        serverData.configID = _mountRef.mountId;
        serverData.lev = GameCenter.newMountMng.CurLev;
        serverData.isRiding = false;
        AssetName = _mountRef.mountModel;//模型资源 
        if (RefData != null)
        {
            previewItem = new EquipmentInfo(RefData.itemID, 1, EquipmentBelongTo.PREVIEW);
        }
        ProcessLevEffect();
    } 
    /// <summary>
    /// 其他玩家的坐骑构造
    /// </summary>
    /// <param name="_msg"></param>
    public MountInfo(scene_ply _msg,PlayerBaseInfo _belongto)
    {
        //Debug.Log(" MountInfo " + "  rideType : " + _msg.ride_type + "  ,rideState : " + _msg.ride_state + "  , skin : " + _msg.currskin + "  , ridelev : " + _msg.ride_lev); 
        ownerInfo = _belongto;
        serverData = new MountData();
        serverData.configID = (int)_msg.ride_type;
        if (_msg.currskin != 0) serverData.configID = (int)_msg.currskin;
        serverData.isRiding = _msg.ride_state == (byte)1;
        serverData.lev = _msg.ride_lev;
        if (OnRideStateUpdate != null)
        {
            OnRideStateUpdate(serverData.isRiding);
        }
        if (ownerInfo.OnMountRideStateUpdate != null)
        {
            ownerInfo.OnMountRideStateUpdate(serverData.isRiding, true);
        }
        UpdateMountEffect();
    }

    /// <summary>
    /// 坐骑更新
    /// </summary> 
    public void Update(st.net.NetBase.ride_list _msg)
    { 
        serverData.Update(_msg);
        //ownerInfo.UpdateMount(this);
    }
    
    /// <summary>
    /// 上下马   1 是坐骑(1 上 0 下) 2是皮肤(2 穿 3 脱) add
    /// </summary>
    /// <param name="_msg"></param>
    public void Update(pt_update_ride_state_d440 _msg)
    {
        serverData.Update(_msg);
        //ownerInfo.UpdateMount(this);
    }


    /// <summary>
    /// 坐骑等级更新
    /// </summary> 
    public void Update(pt_update_ride_lev_d439 _msg)
    {
        serverData.Update(_msg);
        UpdateMountEffect();
        //ownerInfo.UpdateMount(this);
    }


    /// <summary>
    /// 骑乘状态或者坐骑装备变化(其他人)
    /// </summary>
    /// <param name="_msg"></param>
    public void Update(scene_ply _msg)
    {
        //Debug.Log("mountID:" + _msg.ride_type + ",Skin:" + _msg.currskin + ",ride:" + _msg.ride_state + "  ,ridelev : " + _msg.ride_lev);
        bool changeRide = (_msg.ride_state == (byte)1) != serverData.isRiding;
        bool changeModel = (serverData.curChangeId != _msg.currskin) || (serverData.configID != _msg.ride_type);
        {
            serverData.configID = (int)_msg.ride_type;
        }
        if (_msg.currskin != 0) serverData.configID = (int)_msg.currskin;
        serverData.isRiding = _msg.ride_state == (byte)1;
        serverData.lev = _msg.ride_lev;
        if (changeRide)
        {
            if (OnRideStateUpdate != null)
            {
                OnRideStateUpdate(serverData.isRiding);
            }
            if (ownerInfo.OnMountRideStateUpdate != null)
            {
                ownerInfo.OnMountRideStateUpdate(serverData.isRiding, true);
            }
        }
        if (changeModel)
        {
            ownerInfo.UpdateMount(this); 
        }
        UpdateMountEffect();
    }

    #region 委托
    /// <summary>
    /// 骑乘状态变化的事件
    /// </summary>
    public System.Action<bool> OnRideStateUpdate; 
    /// <summary>
    /// 坐骑装备变化事件
    /// </summary>
    public System.Action<int> OnEffectChangeEvent;
    ///// <summary>
    ///// 坐骑幻化事件
    ///// </summary>
    //public System.Action OnChange;
    ///// <summary>
    ///// 坐骑皮肤数量变化事件
    ///// </summary>
    //public System.Action OnMoungSkinEvent;

    #endregion

    /// <summary>
    /// 客户端静态数据
    /// </summary>
    protected MountRef refData = null;
    protected MountRef RefData
    {
        get
        {
            if (refData == null || refData.mountId !=ConfigID)
            {

                refData = ConfigMng.Instance.GetMountRef(serverData.configID);
            }
            return refData;
        }
    } 

    protected RidePropertyRef mountPropertyRefData = null;
    protected RidePropertyRef MountPropertyRefData
    {
        get
        {
            if (mountPropertyRefData == null || mountPropertyRefData.level != serverData.lev)
            {
                if (serverData.lev <= (ConfigMng.Instance.RidePropertyRefTable.Count - 1))
                    mountPropertyRefData = ConfigMng.Instance.GetMountPropertyRef(serverData.lev > 0 ? serverData.lev : 1);
                else
                    mountPropertyRefData = ConfigMng.Instance.GetMountPropertyRef(ConfigMng.Instance.RidePropertyRefTable.Count - 1);
            }
            return mountPropertyRefData;
        }
    }

    
    #region 幻化  
    /// <summary>
    /// 幻化更新
    /// </summary> 
    public void Update(st.net.NetBase.skin_base_info _msg)
    {
        serverData.Update(_msg);
        //ownerInfo.UpdateMount(this);
    }
    public void SetCurMount()
    {
        if (ownerInfo != null)
        {
            //Debug.Log(" ********    设置坐骑  " + this.ConfigID);
            ownerInfo.UpdateMount(this); 
        }
    }

	public void UpdateRide()
	{
		if (OnRideStateUpdate != null)
		{
			OnRideStateUpdate(serverData.isRiding);
		}
		if (ownerInfo.OnMountRideStateUpdate != null)
		{
			ownerInfo.OnMountRideStateUpdate(serverData.isRiding, true);
		}
	}
    #endregion


    #region 访问器
    /// <summary>
    /// 坐骑配置表索引ID
    /// </summary>
    public int ConfigID
    {
        get
        { 
            return serverData.configID;
        }
    }
    /// <summary>
    /// 坐骑等级
    /// </summary>
    public int Lev
    {
        get
        {
            return serverData.lev;
        } 
    }

    /// <summary>
    /// 坐骑特效列表
    /// </summary>
    public List<MountEffect> MountEffectList
    {
        get
        {
            return RefData == null ? new List<MountEffect>() : RefData.mountEffectList;
        }
    } 
    /// <summary>
    /// 皮肤使用剩余时间
    /// </summary>
    public int SkinRemainTime
    {
        get
        {
            return serverData == null ? 0 : (int)serverData.skinReminTime;
        }
    } 
    /// <summary>
    /// 解锁当前坐骑需要的等级
    /// </summary>
    public int GetMountNeedLev
    {
        get
        {
            return RefData != null ? RefData.level : 0;
        }
    }
    /// <summary>
    /// 下一级解锁的坐骑id
    /// </summary>
    public int NextLevId
    {
        get
        {
            return RefData != null ? RefData.nextLevelId : 0;
        }
    }
    /// <summary>
    /// 骑乘挂点
    /// </summary>
    public string SeatPointName
    {
        get
        {
            return RefData != null ? RefData.riderPoint : string.Empty;
        }
    }
    /// <summary>
    /// 坐骑名
    /// </summary>
    public string MountName
    {
        get
        {
            return RefData != null ? RefData.mountName : string.Empty;
        }
    }
    /// <summary>
    /// 阶级名
    /// </summary>
    public string LevName
    {
        get
        {
            return MountPropertyRefData != null ? MountPropertyRefData.name : string.Empty;
        }
    }
    /// <summary>
    /// 升级成功的概率
    /// </summary>
    public int Chance
    {
        get
        { 
            return MountPropertyRefData != null ? MountPropertyRefData.chance : 0;
        }
    }
    /// <summary>
    /// 坐骑的物品ID 
    /// </summary>
    public int ItemID
    {
        get
        {
            return RefData != null ? RefData.itemID : 0;
        }
    }
    /// <summary>
    /// 速度
    /// </summary>
    public int Speed
    {
        get
        {
            return MountPropertyRefData != null ? MountPropertyRefData.speed : 0;
        }
    }
    /// <summary>
    /// 生命
    /// </summary>
    public int Hp
    {
        get
        {
            return MountPropertyRefData != null ? MountPropertyRefData.hp : 0;
        }
    }
    /// <summary>
    /// 攻击
    /// </summary>
    public int Att
    {
        get
        {
            return MountPropertyRefData != null ? MountPropertyRefData.att : 0;
        }
    }
    /// <summary>
    /// 防御
    /// </summary>
    public int Def
    {
        get
        {
            return MountPropertyRefData != null ? MountPropertyRefData.def : 0;
        }
    }
    /// <summary>
    /// 暴击
    /// </summary>
    public int Cri
    {
        get
        {
            return MountPropertyRefData != null ? MountPropertyRefData.cri : 0;
        }
    }
    /// <summary>
    /// 韧性
    /// </summary>
    public int During
    {
        get
        {
            return MountPropertyRefData != null ? MountPropertyRefData.duc : 0;
        }
    }
    /// <summary>
    /// 命中
    /// </summary>
    public int Hit
    {
        get
        {
            return MountPropertyRefData != null ? MountPropertyRefData.hit : 0;
        }
    }
    /// <summary>
    /// 闪避
    /// </summary>
    public int Dge
    {
        get
        {
            return MountPropertyRefData != null ? MountPropertyRefData.dge : 0;
        }
    }
    /// <summary>
    /// 坐骑升级物品
    /// </summary>
    public List<ItemValue> Item
    {
        get
        {
            return MountPropertyRefData != null ? MountPropertyRefData.item : null;
        }
    }
    /// <summary>
    /// 所有者的服务端的唯一索引ID
    /// </summary>
    public int OwnerID
    {
        get
        {
            return ownerInfo == null ? -1 : ownerInfo.ServerInstanceID;
        }
    }
    protected int oldMountID = 0;
    protected int oldChangeID = -1;
	protected bool oldIsRiding = false;
    /// <summary>
    /// 当前使用的幻化坐骑ID 0 为 未幻化
    /// </summary>
    public int CurChangeID
    {
        set
        {
            serverData.curChangeId = value;
        }
        get
        {
            return serverData.curChangeId;
        }
    }

    /// <summary>
    /// 行走碰撞类型，是否检测高低起伏
    /// </summary>
    public RecastType MoveRecastType
    {
        get
        {
            return RefData == null ? RecastType.NONE : RefData.recastType;
        }
    }


    /// <summary>
    /// 是否正在骑乘中
    /// </summary>
    public bool IsRiding
    { 
        get
        {
            return serverData.isRiding;
        }
    }
    /// <summary>
    /// 坐骑状态 4 为当前化形的坐骑
    /// </summary>
    public int MountState
    {
        get
        {
            return serverData.state;
        }
    }

    /// <summary>
    /// 骨骼特效列表
    /// </summary>
    public List<BoneEffectRef> BoneEffectList
    {
        get
        { 
            return boneEffectList;
        }
    }

    protected List<BoneEffectRef> boneEffectList = new List<BoneEffectRef>();
    /// <summary>
    /// 当前坐骑的升级进度
    /// </summary>
    public float CurExpProcess
    {
        get
        {
            return Mathf.Min(1.0f, serverData.baseValueDic[ActorBaseTag.Exp] / (float)CurLevelNeedExp);
        }
    }
    /// <summary>
    /// 坐骑当前经验值
    /// </summary>
	public ulong curExp
    {
        get
        {
            return serverData.baseValueDic[ActorBaseTag.Exp];
        }
    }

    /// <summary>
    /// 坐骑当前等级需要的经验值
    /// </summary>
    public int CurLevelNeedExp
    {
        get
        {
            return RefData == null ? 0 : RefData.explain;
        }
    }

    /// <summary>
    /// 数据是否已经经过服务端修正
    /// </summary>
    public bool HasInitFromServer
    {
        get
        {
            return serverData.hasInit;
        }
    }
    /// <summary>
    /// 前足点
    /// </summary>
    public Vector3 frontPoint
    {
        get
        {
            return RefData == null ? Vector3.zero : RefData.frontPoint;
        }
    }
    /// <summary>
    /// 后足点
    /// </summary>
    public Vector3 BehindPoint
    {
        get
        {
            return RefData == null ? Vector3.zero : RefData.behindPoint;
        }
    }

    /// <summary>
    /// 名字
    /// </summary>
    public new string Name
    {
        get
        {
            if (RefData != null)
            {
                return RefData.mountName;
            }
            return string.Empty;
        }
    }
    /// <summary>
    /// 坐骑描述
    /// </summary>
    public string Description
    {
        get
        {
            return RefData == null ? string.Empty : RefData.text;
        }
    }
    protected EquipmentInfo previewItem;
    /// <summary>
    /// 对应物品
    /// </summary>
    public EquipmentInfo PreviewItem
    {
        get
        {
            return previewItem;
        }
    }
    ///// <summary>
    ///// 预览大小
    ///// </summary>
    //public float PreviewScale
    //{
    //    get
    //    {
    //        return RefData == null ? 1 : RefData.previewRscale;
    //    }
    //}
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
    public Vector3 PreviewRot
    {
        get
        { 
            return RefData == null ? Vector3.zero :  RefData.previewRscale;
        }
    }
    public int oldLevel = 0;
    /// <summary>
    /// 名字高度差值
    /// </summary>
    public float NameHightDiff
    {
        get
        {
            return RefData == null ? 0 : Mathf.Max(0, RefData.addNameHigh);
        }
    }
    protected string assetName = string.Empty;
    /// <summary>
    /// 资源名称
    /// </summary>
    public string AssetName
    {
        get 
        {
            return  RefData != null ? RefData.mountModel : string.Empty;
        }
        set
        {
            assetName = value;
        }
    }

    /// <summary>
    /// 资源路径
    /// </summary>
    public string AssetURL
    {
        get
        {
            return RefData == null ? string.Empty : AssetMng.GetPathWithExtension(RefData.mountModel, AssetPathType.PersistentDataPath);
        }
    }


    /// <summary>
    /// 是否正在移动状态中
    /// </summary>
    protected bool isMoving;
    /// <summary>
    /// 是否正在移动状态中
    /// </summary>
    public bool IsMoving
    {
        get
        {
            return isMoving;
        }
        set
        {
            if (isMoving != value)
            {
                isMoving = value;
            }
        }
    }
    /// <summary>
    /// 坐骑的最大等级
    /// </summary>
    public int MaxLevel
    {
        get
        {
            return RefData == null ? 0 : 100;// RefData.mountMaxLv;---暂无
        }
    }
    /// <summary>
    /// 是否还能升级
    /// </summary>
    public bool CanLevelUp
    {
        get
        {
            if (RefData == null) return false;
            if (GameCenter.inventoryMng.GetNumberByType(FeedID) > 0)
            {
                return (serverData.baseValueDic[ActorBaseTag.Level] < ownerInfo.Level && serverData.baseValueDic[ActorBaseTag.Level] < (ulong)MaxLevel);
            }
            return false;
        }
    } 
    /// <summary>
    /// 战力
    /// </summary>
    public int FightValue
    {
        get
        {
            return serverData.fightValue;
        }
    }
    /// <summary>
    /// 坐骑id
    /// </summary>
    public int FeedID
    {
        get
        {
            return RefData != null ? RefData.feedID:0;
        }
    }
    /// <summary>
    /// 骑乘挂点
    /// </summary>
    public string RidePoint
    {
        get
        {
            return RefData != null ? RefData.riderPoint : string.Empty;
        }
    }
    /// <summary>
    /// 坐骑内型 1 坐骑 2 幻化
    /// </summary>
    public MountType MountKind
    {
        get
        {
            return RefData != null ? (MountType)RefData.kind : MountType.NONE;
        }
    }

    protected void ProcessLevEffect()
    {
        boneEffectList.Clear();
        //Debug.Log("  ProcessLevEffect  " + ConfigID + "  lev : " + Lev); 
        for (int i = 0; i < MountEffectList.Count; i++)
        {
            MountEffect item = MountEffectList[i];
            if (MountKind == MountType.SKINLIST)
            { 
                boneEffectList.Add(new BoneEffectRef(item.boneName, item.effectName)); 
            }
            else
            {
                if ((ConfigID - 1) * 9 + item.effectLev <= Lev)
                {
                    boneEffectList.Add(new BoneEffectRef(item.boneName, item.effectName));
                }
            }
        } 
        if (OnEffectChangeEvent != null)
        {
            OnEffectChangeEvent(Lev);
        }
    } 
	/// <summary>
	/// 切场景重新播放特效
	/// </summary>
	public void ReShowLevEffect()
	{
		ProcessLevEffect();
	}
    protected void UpdateMountEffect()
    {
        if (MountKind == MountType.SKINLIST)
        {
            ProcessLevEffect();
        }
        else
        {
            for (int i = 0; i < MountEffectList.Count; i++)
            {
                MountEffect item = MountEffectList[i];
                if ((ConfigID - 1) * 9 + item.effectLev <= Lev)
                {
                    ProcessLevEffect();
                }
            }
        }
    }
    #endregion


}
