//====================================
//作者：吴江
//日期：2015/5/22
//用途：基本活动物体数据层对象（Info结尾的类名都为数据层对象，包含 服务端数据  客户端静态数据   访问器 三部分）
//=====================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using st.net.NetBase;


/// <summary>
/// 活动对象的服务端动态数据 by吴江 
/// </summary>
public class ActorData
{
    public int serverInstanceID;
    public int prof;
    public int camp;
    public float startPosX;
    public float startPosY;
    public float startPosZ;
    public int dir;
    public List<int> equipTypeList = new List<int>();
    public bool isInFight = true;
    public bool isHide = false;
    public int starCount = 0;

    public int cosmeticID;
    public Dictionary<ActorPropertyTag, int> propertyValueDic = new Dictionary<ActorPropertyTag, int>();
    public Dictionary<ActorBaseTag, ulong> baseValueDic = new Dictionary<ActorBaseTag, ulong>();
}


/// <summary>
/// 活动对象的完整数据 by吴江
/// </summary>
public class ActorInfo {

     //<summary>
     //服务端数据
     //</summary>
    protected ActorData serverData = null;


     //<summary>
     //构造
     //</summary>
     //<param name="_actorData"></param>
    public ActorInfo(ActorData _actorData)
    {
        serverData = _actorData;
       //isAlive = CurHP > 0;
    }

    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="_actorData"></param>
    public ActorInfo(ActorData _actorData,bool _isActor)
    {
        isActor = _isActor;
        serverData = _actorData;
        //isAlive = CurHP > 0;
    }

    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="_actorData"></param>
    protected ActorInfo()
    {
    }


    /// <summary>
    /// 承受技能后果导致数据改变  by吴江
    /// </summary>
    /// <param name="_abilityResult"></param>
    public virtual void Update(st.net.NetBase.skill_effect _abilityResult)
    {
        DefResultType defType = (DefResultType)_abilityResult.def_sort;
        switch (defType)
        {
            //case DefResultType.DEF_SORT_DIE:
            //    serverData.baseValueDic[ActorBaseTag.CurHP] = (int)_abilityResult.cur_hp;
            //    if (OnBaseUpdate != null)
            //    {
            //        OnBaseUpdate(ActorBaseTag.CurHP, serverData.baseValueDic[ActorBaseTag.CurHP], true);
            //    }
            //    IsAlive = false;
            //    break;
            case DefResultType.DEF_SORT_UNTREAT:
            case DefResultType.DEF_SORT_NO:
            case DefResultType.DEF_SORT_STIFLE:
            case DefResultType.DEF_SORT_KICK2:
            case DefResultType.DEF_SORT_KICK:
            case DefResultType.DEF_SORT_NOKICKDOWN:
            case DefResultType.DEF_SORT_NOSTIFLE:
            case DefResultType.DEF_SORT_NOKICK:
            case DefResultType.DEF_SORT_TREAT:
                serverData.baseValueDic[ActorBaseTag.CurHP] = _abilityResult.cur_hp;
                if (this.OnBaseUpdate != null)
                {
					this.OnBaseUpdate(ActorBaseTag.CurHP, serverData.baseValueDic[ActorBaseTag.CurHP],true);
                }
                if (serverData.baseValueDic[ActorBaseTag.CurHP] <= 0)
                {
                    IsAlive = false;
                }
                break;
            case DefResultType.DEF_SORT_ADDMP:
                serverData.baseValueDic[ActorBaseTag.CurMP] += _abilityResult.demage;
                if (OnBaseUpdate != null)
                {
                    OnBaseUpdate(ActorBaseTag.CurMP, serverData.baseValueDic[ActorBaseTag.CurMP], true);
                }
                break;
            case DefResultType.DEF_SORT_DELMP:
                serverData.baseValueDic[ActorBaseTag.CurMP] -= _abilityResult.demage;
                if (OnBaseUpdate != null)
                {
                    OnBaseUpdate(ActorBaseTag.CurMP, serverData.baseValueDic[ActorBaseTag.CurMP], true);
                }
                break;
        }
    }

    /// <summary>
    /// 挂起状态变化
    /// </summary>
    /// <param name="_hide"></param>
    public void UpdateHide(bool _hide, float _x, float _z)
    {
        serverData.startPosX = _x;
        serverData.startPosZ = _z;
        if (serverData.isHide != _hide)
        {
            serverData.isHide = _hide;
            if (OnHideUpdate != null)
            {
                OnHideUpdate();
            }
        }
    }
	/// <summary>
	/// 花车巡游过程中的隐藏玩家的buff
	/// </summary>
	public void UpdateHide(bool _hide)
	{
		serverData.isHide = _hide;
	}

    public System.Action OnHideUpdate;

    /// <summary>
    /// 是否活着
    /// </summary>
    protected bool isAlive = true;

    /// <summary>
    /// 为true代表这个对象只存在与过场动画中，不是真实的对象
    /// </summary>
    protected bool isActor = false;
    /// <summary>
    /// 为true代表这个对象只存在与过场动画中，不是真实的对象
    /// </summary>
    public bool IsActor
    {
        get { return isActor; }
    }

    protected Vector3 serverPos = Vector3.zero;
    public Vector3 ServerPos
    {
        get
        {
            serverPos.x = serverData.startPosX;
            serverPos.y = serverData.startPosY;
            serverPos.z = serverData.startPosZ;
            return serverPos;
        }
        set
        {
            serverPos = value;
            serverData.startPosX = serverPos.x;
            serverData.startPosY = serverPos.y;
            serverData.startPosZ = serverPos.z;
        }
    }

    public int RotationY
    {
        get
        {
            return (int)serverData.dir;
        }
    }


    public int Prof
    {
        get
        {
            return (int)serverData.prof;
        }
    }

    /// <summary>
    /// buff列表
    /// </summary>
    protected FDictionary buffList = new FDictionary();
    /// <summary>
    /// buff列表
    /// </summary>
    public FDictionary BuffList
    {
        get
        {
            return buffList;
        }
    }

    /// <summary>
    /// 是否隐藏挂起 by吴江
    /// </summary>
    public bool IsHide
    {
        get
        {
            return serverData.isHide;
        }
    }


    /// <summary>
    /// 是否在战斗中
    /// </summary>
    public bool IsInFight
    {
        get
        {
            return serverData.isInFight;
        }

        set
        {
            if (serverData.isInFight != value)
            {
                serverData.isInFight = value;
                if (OnFightStateUpdate != null)
                {
                    OnFightStateUpdate(serverData.isInFight);
                }
            }
        }
    }

    public System.Action<bool> OnFightStateUpdate;
    #region 装备相关
    /// <summary>
    /// 默认装备的装备列表 by吴江
    /// </summary>
    protected Dictionary<EquipSlot, EquipmentInfo> defaultDictionary = new Dictionary<EquipSlot, EquipmentInfo>();
    /// <summary>
    /// 默认装备的装备列表 by吴江
    /// </summary>
    public Dictionary<EquipSlot, EquipmentInfo> DefaultDictionary
    {
        get { return defaultDictionary; }
    }
    /// <summary>
    /// 当前装备的装备列表  by吴江
    /// </summary>
    protected Dictionary<EquipSlot, EquipmentInfo> equipmentDictionary = new Dictionary<EquipSlot, EquipmentInfo>();
    /// <summary>
    /// 当前装备的装备列表  by吴江
    /// </summary>
    public Dictionary<EquipSlot, EquipmentInfo> EquipmentDictionary
    {
        get { return equipmentDictionary; }
    }

    /// <summary>
    /// 符合的套装数 by 龙英杰
    /// </summary>
    /// <param name="suitList"></param>
    /// <returns></returns>
    public int SuitNumb(List<int> suitList)
    {
        int numb = 0;
        foreach (EquipmentInfo item in equipmentDictionary.Values)
        {
            if (suitList.Contains(item.EID))
            {
                numb++;
            }
        }
        return numb;
    }
	public string ProfName{
		get{
			PlayerConfig player = ConfigMng.Instance.GetPlayerConfig(Prof);
			return player == null ? string.Empty : player.name;
		}
	}

    /// <summary>
    /// 当前的星级特效
    /// </summary>
    protected string curStarEffect = string.Empty;
    /// <summary>
    /// 当前的星级特效
    /// </summary>
    public virtual string CurStarEffect
    {
        get
        {
            //if (serverData.starCount > 0)
            //{
            //    StarPropertyRef curRef = null;
            //    int starLv = ConfigMng.Instance.GetCurStarNumLv(serverData.starCount);
            //    curRef = ConfigMng.Instance.GetStarPropertyRef(starLv);
            //    curStarEffect = curRef == null ? string.Empty : curRef.effect;
            //}
            return curStarEffect;
        }

    }
    /// <summary>
    /// 当前装备的时装列表  by吴江
    /// </summary>
    protected Dictionary<EquipSlot, EquipmentInfo> cosmeticDictionary = new Dictionary<EquipSlot, EquipmentInfo>();
    /// <summary>
    /// 当前装备的时装列表  by吴江
    /// </summary>
    public Dictionary<EquipSlot, EquipmentInfo> CosmeticDictionary
    {
        get { return cosmeticDictionary; }
    }
    /// <summary>
    /// 当前要展示的装备列表  by吴江
    /// </summary>
    protected Dictionary<EquipSlot, EquipmentInfo> curShowDictionary = new Dictionary<EquipSlot, EquipmentInfo>();
    /// <summary>
    /// 当前要展示的装备列表  by吴江
    /// </summary>
    public Dictionary<EquipSlot, EquipmentInfo> CurShowDictionary
    {
        get { return curShowDictionary; }
    }
    /// <summary>
    /// 初始化装备数据 by吴江
    /// </summary>
    /// <param name="_data"></param>
    protected virtual void ProcessServerData(ActorData _data)
    {
    }
    /// <summary>
    /// 检查该部位的时装是否需要显示 by吴江
    /// </summary>
    /// <param name="_slot"></param>
    /// <param name="_serverData"></param>
    /// <returns></returns>
    protected virtual bool CheckCosmeticState(EquipSlot _slot, ActorData _serverData)
    {
        return true;
    }
    /// <summary>
    /// 更新玩家当前穿的装备  by吴江
    /// </summary>
    /// <param name="_equipList"></param>
    protected void UpadateEquipments(List<EquipmentInfo> _equipList)
    {
        cosmeticDictionary.Clear();
        equipmentDictionary.Clear();
        curShowDictionary.Clear();

        foreach (var item in _equipList)
        {
            if (item.Family == EquipmentFamily.COSMETIC)
            {
                cosmeticDictionary[item.Slot] = item;
            }
            else
            {
                equipmentDictionary[item.Slot] = item;
            }
        }

        UpdateCurShowEquipments();
    }
    /// <summary>
    /// 更新玩家当前应该展示的装备  by吴江
    /// </summary>
    /// <param name="_equipList"></param>
    protected void UpdateCurShowEquipments()
    {
        foreach (EquipSlot item in Enum.GetValues(typeof(EquipSlot)))
        {
			if((int)item >= (int)EquipSlot.count)continue;//排除掉没模型的孔位  by邓成
            EquipmentInfo eq = curShowDictionary.ContainsKey(item) ? curShowDictionary[item] : null;
            if (CheckCosmeticState(item, serverData) && cosmeticDictionary.ContainsKey(item) && cosmeticDictionary[item] != null && cosmeticDictionary[item].HasModel()) //如果该部位有时装并且需要显示
            {
                curShowDictionary[item] = cosmeticDictionary[item];
            }
            else
            {
                if (equipmentDictionary.ContainsKey(item) && equipmentDictionary[item] != null && equipmentDictionary[item].HasModel()) //如果有装备
                {
                    curShowDictionary[item] = equipmentDictionary[item];
                }
                else //显示默认外形
                {
                    if (defaultDictionary.ContainsKey(item))
                    {
                        curShowDictionary[item] = defaultDictionary[item];
                    }
                    else
                    {
                        curShowDictionary[item] = null;
                    }
                }
            }
            if (curShowDictionary[item] != null)
            {
                if (!curShowDictionary[item].HasModel())
                {
                    if (defaultDictionary.ContainsKey(item))
                    {
                        curShowDictionary[item] = defaultDictionary[item];
                    }
                }
            }
            if (curShowDictionary[item] != eq && OnCurShowEquipUpdate != null)
            {
                OnCurShowEquipUpdate(item);
            }
        }
    }
    /// <summary>
    /// 当前需要展示的装备发生变化的事件 by吴江
    /// </summary>
    public System.Action<EquipSlot> OnCurShowEquipUpdate;
    /// <summary>
    /// 当前的装备发生变化的事件 by吴江
    /// </summary>
    public System.Action OnEquipUpdate;
    /// <summary>
    /// 骨骼名称 by吴江
    /// </summary>
    public virtual string Bone_Name
    {
        get
        {
            return string.Empty;
        }
    }
    #endregion

    /// <summary>
    /// 被技能影响 by吴江
    /// </summary>
    /// <param name="_info"></param>
    public virtual void BeInfluencedByOther(AbilityInstance _instance, st.net.NetBase.skill_effect _result)
    {
        if (IsActor) return;
        unFinishedAbilityResultList.Add(new AbilityResultInfo(_instance, _result));
    }

    /// <summary>
    /// 等待处理的发生在自己身上的技能后果 by吴江
    /// </summary>
    public List<AbilityResultInfo> unFinishedAbilityResultList = new List<AbilityResultInfo>();

    public int MaxHP
    {
        get
        {
            if (serverData.propertyValueDic.ContainsKey(ActorPropertyTag.HPLIMIT))
            {
                return serverData.propertyValueDic[ActorPropertyTag.HPLIMIT];
            }
            return 0;
        }
    }
    /// <summary>
    /// 最大血量字符串
    /// </summary>
    public string MaxHPText
    {
        get
        {
            if (MaxHP >= 1000000)
            {
                return Math.Round(MaxHP / 10000f, 0).ToString() + ConfigMng.Instance.GetUItext(180);
            }
            else if (MaxHP >= 10000)
            {
                return Math.Round(MaxHP / 10000f, 1).ToString() + ConfigMng.Instance.GetUItext(180);
            }
            else
            {
                return MaxHP.ToString();
            }
        }
    }  
    public int MaxMP
    {
        get
        {
            if (serverData.propertyValueDic.ContainsKey(ActorPropertyTag.MPLIMIT))
            {
                return serverData.propertyValueDic[ActorPropertyTag.MPLIMIT];
            }
            return 0;
        }
    }
    /// <summary>
    /// 最大蓝量字符串
    /// </summary>
    public string MaxMPText
    {
        get
        {
            if (MaxMP >= 1000000)
            {
                return Math.Round(MaxMP / 10000f, 0).ToString() + ConfigMng.Instance.GetUItext(180);
            }
            else if (MaxMP >= 10000)
            {
                return Math.Round(MaxMP / 10000f, 1).ToString() + ConfigMng.Instance.GetUItext(180);
            }
            else
            {
                return MaxMP.ToString();
            }
        }
    }
    public ulong CurHP
    {
        get
        {
            if (serverData.baseValueDic.ContainsKey(ActorBaseTag.CurHP))
            {
                return serverData.baseValueDic[ActorBaseTag.CurHP];
            }
            return 0;
        }
    }
    /// <summary>
    /// 最大血量字符串
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
    public ulong CurMP
    {
        get
        {
            if (serverData.baseValueDic.ContainsKey(ActorBaseTag.CurMP))
            {
                return serverData.baseValueDic[ActorBaseTag.CurMP];
            }
            return 0;
        }
    }

    /// <summary>
    /// 是否活着
    /// </summary>
    public bool IsAlive
    {
        get { return isAlive; }
        set
        {
            if (isAlive != value)
            {
                isAlive = value;
                if (!isAlive)
                {
					CleanBuffByDead();
                }
                if (OnAliveUpdate != null)
                {
                    OnAliveUpdate(isAlive);
                }
            }
        }
    }


    /// <summary>
    /// 清空buff，在切换场景时使用
    /// </summary>
    public virtual void CleanBuff()
    {
        if (OnBuffUpdate != null)
        {
            foreach (BuffInfo item in buffList.Values)
            {
				if(item.MapCleanType == BuffMapCleanType.CLEAN)
				{
					OnBuffUpdate(item.BuffTypeID, false);
				}	
            }
        }
		FDictionary tempBuffList = new FDictionary();
		foreach (BuffInfo item in buffList.Values)
		{
			if(item.MapCleanType != BuffMapCleanType.CLEAN)
			{
				tempBuffList[item.BuffTypeID] = item;
			}	
		}
		buffList = tempBuffList;
//        if (OnCleanBuff != null)
//        {
//            OnCleanBuff();
//        }
    }

	public virtual void CleanBuffByReconnect()
	{
		if (OnBuffUpdate != null)
		{
			foreach (BuffInfo item in buffList.Values)
			{
				OnBuffUpdate(item.BuffTypeID, false);
			}
		}
		buffList.Clear();
        if (OnCleanBuff != null)
        {
            OnCleanBuff();
        }
	}
	/// <summary>
	/// 死亡的时候只清除减益buff   buff由后台控制,不需要前台修改 by邓成
	/// </summary>
	public virtual void CleanBuffByDead()
	{
//		if (OnBuffUpdate != null)
//		{
//			foreach (BuffInfo item in buffList.Values)
//			{
//				if(item.CurBuffAttrType == BuffAttrType.DOWN)
//				{
//					OnBuffUpdate(item.BuffTypeID, false);
//				}	
//			}
//		}
//		FDictionary tempBuffList = new FDictionary();
//		foreach (BuffInfo item in buffList.Values)
//		{
//			if(item.CurBuffAttrType != BuffAttrType.DOWN)
//			{
//				tempBuffList[item.BuffTypeID] = item;
//			}	
//		}
//		buffList = tempBuffList;
	}

    public virtual void CleanUnFinishedAbilityResult()
    {
        unFinishedAbilityResultList.Clear();
    }


    public void Update(BuffInfo _buff, int _buffID, int _id)
    {
        if (!isAlive) return; //如果怪物已经死亡,拒绝后台的状态通知
        if (_buff == null)//删buff
        {
            if (_id == serverData.serverInstanceID && buffList.ContainsKey(_buffID))
            {
                if (OnBuffUpdate != null)
                {
                    OnBuffUpdate(_buffID, false);
                }
                buffList.Remove(_buffID);
            }
        }
        else//加buff
        {
            if (buffList.ContainsKey(_buff.BuffTypeID))
            {
                if (OnBuffUpdate != null)
                {
                    OnBuffUpdate(_buff.BuffTypeID, false);
                    buffList.Remove(_buff.BuffTypeID);
                }
            }
            buffList[_buff.BuffTypeID] = _buff;
            if (OnBuffUpdate != null)
            {
                OnBuffUpdate(_buff.BuffTypeID, true);
            }
        }
    }

	/// <summary>
	/// 设置生死状态
	/// </summary>
	/// <param name="_alive"></param>
	public void SetAlive(bool _alive)
	{
		IsAlive = _alive;
	}

    /// <summary>
    /// 获取buff信息 by吴江
    /// </summary>
    /// <param name="_buffID"></param>
    /// <returns></returns>
    public BuffInfo GetBuffInfo(int _buffID)
    {
        if (buffList.ContainsKey(_buffID))
        {
            return buffList[_buffID] as BuffInfo;
        }
        return null;
    }


    public System.Action<int, bool> OnBuffUpdate;

    public System.Action OnCleanBuff;



    /// <summary>
    /// 基本属性发生改变的事件
    /// </summary>
    public System.Action<ActorBaseTag, ulong,bool> OnBaseUpdate;

    /// <summary>
    /// 战斗属性发生改变的事件
    /// </summary>
    public System.Action<ActorPropertyTag, long, bool> OnPropertyUpdate;
    /// <summary>
    /// 阵营发生改变的事件 
    /// </summary>
    public System.Action<int> OnCampUpdate;
	/// <summary>
	/// 公会名字信息变更事件
	/// </summary>
	public System.Action<string> onGuildNameUpdate;
	/// <summary>
	/// 玩家结束采集事件  
	/// </summary>
	public System.Action<int,int> onEndCollectEvent;

    /// <summary>
    /// 生存状态发生改变的事件
    /// </summary>
    public System.Action<bool> OnAliveUpdate;

    #region 访问器
    /// <summary>
    /// 服务端唯一实例ID
    /// </summary>
    public int ServerInstanceID
    {
        get
        {
            return serverData.serverInstanceID;
        }
    }


    public string Name
    {
        get
        {
            return string.Empty;
        }
    }

    protected float colliderRadius = 1.0f;
    public float ColliderRadius
    {
        get
        {
            return colliderRadius;
        }
    }

    /// <summary>
    /// 动画资源标准移动速度
    /// </summary>
    public virtual float AnimationMoveSpeedBase
    {
        get
        {
            return 5.0f;
        }
    }

    /// <summary>
    /// 互动音效 
    /// </summary>
    public string ActionIngSoundRes
    {
        get
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// 结束死亡音效 
    /// </summary>
    public string EndSoundRes
    {
        get
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// 名字高度 by吴江
    /// </summary>
    public float NameHeight
    {
        get
        {
            return 2.0f;
        }
    }

    /// <summary>
    /// 阵营ID
    /// </summary>
    public int Camp
    {
        get
        {
            return serverData.camp;
        }

    }

    /// <summary>
    /// 模型大小
    /// </summary>
    public virtual float ModelScale
    {
        get
        {
            return 1.0f;
        }
    }

    #endregion


}