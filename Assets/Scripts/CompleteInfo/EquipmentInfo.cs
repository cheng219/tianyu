//====================================================
//作者：吴江
//日期：2015/5/10
//用途：装备物品的数据层对象
//======================================================




using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class EquipmentServerData
{
    public int eid;
    public int Lev;
    public int etype;
    public int num;
    public int epos;
    public int ebind;
    public int equOne;
    public int equTwo;
    public int equThree;
    public int equFour;
	public int strengthExp;
    public int exp_expire_time;
	public int washExp;
	public int lucky_lev;
    public int color;
	public List<st.net.NetBase.pos_des> pos_list = new List<st.net.NetBase.pos_des>();

    public EquipmentServerData()
    {

    }
    public EquipmentServerData(st.net.NetBase.item_des _info)
    {
        eid = (int)_info.id;
        etype = (int)_info.type;
        num = (int)_info.num;
        epos = (int)_info.pos;
        ebind = (int)_info.bind;
        Lev = (int)_info.lev;
        equOne = (int)_info.equOne;
        equTwo = (int)_info.equTwo;
        equThree = (int)_info.equThree;
        equFour = (int)_info.equFour;
		strengthExp = (int)_info.strenthen_exp;
        exp_expire_time = (int)_info.exp_expire_time+(int)Time.time;
		washExp = (int)_info.recast_exp;
		lucky_lev = (int)_info.luck;
		pos_list = _info.pos_list;
        color = (int)_info.color;
    }
	public EquipmentServerData(EquipmentServerData data)
	{
		eid = data.eid;
		etype = data.etype;
		num = data.num;
		epos = data.epos;
		ebind = data.ebind;
		Lev = data.Lev;
		equOne = data.equOne;
		equTwo =data.equTwo;
		equThree = data.equThree;
		equFour = data.equFour;
		strengthExp = data.strengthExp;
        exp_expire_time = data.exp_expire_time;
		washExp = data.washExp;
        lucky_lev = data.lucky_lev;
		pos_list = data.pos_list;
        color = data.color;
	}
}



/// <summary>
/// 装备物品的数据层对象 by吴江
/// </summary>
public class EquipmentInfo
{

    #region 服务端数据 by吴江
    EquipmentServerData equipmentData;
    #endregion

    #region 静态配置数据 by吴江
    EquipmentRef equipmentRef = null;
    protected EquipmentRef EquipmentRef
    {
        get
        {
            if (equipmentRef != null)
			{
				if(equipmentRef.id != equipmentData.etype)//橙炼和升阶之后,只是更新了etype,此时需要更新equipmentRef by邓成
					equipmentRef = ConfigMng.Instance.GetEquipmentRef(equipmentData.etype);
				return equipmentRef;
			}
            equipmentRef = ConfigMng.Instance.GetEquipmentRef(equipmentData.etype);
            return equipmentRef;
        }
    }
    #endregion
    /// <summary>
    /// 装备从属
    /// </summary>
    protected EquipmentBelongTo equipmentBelongTo = EquipmentBelongTo.NONE;
    /// <summary>
    /// 装备分数
    /// </summary>
    protected int gs;
    //装备的基础战力值
    public int EquipmentGs
    {
        get
        {
            return EquipmentRef.gs;
        }
    }
    protected bool isGotSeverdata = false;
    /// <summary>
    ///是否已经获得装备详细信息
    /// </summary>
    public bool IsGotSeverdata 
    {
        get 
        {
            return isGotSeverdata;
        }
    }
    protected bool isChangeMount = false;
    /// <summary>
    /// 是否幻化中
    /// </summary>
    public bool IsChangeMount
    {
        get
        { 
            return isChangeMount;
        }
        set
        {
            isChangeMount = value;
        }
    }

    #region 构造 by吴江
    public EquipmentInfo(int _eid, EquipmentBelongTo _belongTo)
    {
        equipmentData = new EquipmentServerData();
        equipmentData.etype = _eid;
        equipmentBelongTo = _belongTo;
		gs = (EquipmentRef == null?0:EquipmentRef.gs) + equipmentData.Lev;//TO DO:把强化的属性部分引入计算
    }
    public EquipmentInfo(int _eid, int _num,EquipmentBelongTo _belongTo)
    {
        equipmentData = new EquipmentServerData();
        equipmentData.etype = _eid;
        equipmentData.num = _num;
        equipmentBelongTo = _belongTo;
		gs = (EquipmentRef == null?0:EquipmentRef.gs) + equipmentData.Lev;//TO DO:把强化的属性部分引入计算
    }
	/// <summary>
	/// 构造下一强化等级的预览对象
	/// </summary>
	/// <param name="curEquip">Current equip.</param>
	public EquipmentInfo(EquipmentInfo curEquip)
	{
		equipmentData = new EquipmentServerData(curEquip.equipmentData);
		equipmentData.eid = -1;
		equipmentData.Lev = curEquip.MaxPowerLvValue > curEquip.equipmentData.Lev?(curEquip.equipmentData.Lev + 1):curEquip.equipmentData.Lev;
		equipmentBelongTo = EquipmentBelongTo.PREVIEW;
	}
	/// <summary>
	/// 将背包or身上的装备在其他界面显示时,修改equipmentBelongTo
	/// </summary>
	public EquipmentInfo(EquipmentInfo ownEquip,EquipmentBelongTo _belongTo)
	{
		if(ownEquip == null)
		{
			Debug.LogError("请先做空引用判断");
			return;
		}
		equipmentData = new EquipmentServerData(ownEquip.equipmentData);
		equipmentBelongTo = _belongTo;
	}
    /// <summary>
    /// 剥离出一部分数据
    /// </summary>
    public EquipmentInfo(EquipmentInfo ownEquip,int _num, EquipmentBelongTo _belongTo)
    {
        if (ownEquip == null)
        {
            Debug.LogError("请先做空引用判断");
            return;
        }
        equipmentData = new EquipmentServerData(ownEquip.equipmentData);
        equipmentData.num = _num;
        equipmentBelongTo = _belongTo;
    }

	/// <summary>
	/// 升阶、橙练,当前装备变成另外一个装备(用于前台预览)  by邓成
	/// </summary>
	public EquipmentInfo(EquipmentInfo ownEquip,int eid)
	{
		equipmentData = new EquipmentServerData(ownEquip.equipmentData);
		equipmentData.eid = -1;
		equipmentData.etype = eid;
		equipmentBelongTo = EquipmentBelongTo.PREVIEW;
	}

    /// <summary>
    /// 加上实例化ID，请求装备服务端详细信息用
    /// </summary>
    public EquipmentInfo(int _eid, int _instanceID, int _num, EquipmentBelongTo _belongTo)
    {
        equipmentData = new EquipmentServerData();
        equipmentData.etype = _eid;
        equipmentData.num = _num;
        equipmentBelongTo = _belongTo;
        equipmentData.eid = _instanceID;
        gs = (EquipmentRef == null ? 0 : EquipmentRef.gs) + equipmentData.Lev;//TO DO:把强化的属性部分引入计算
    }

    public EquipmentInfo(int _eid, int _num,bool _isGray, EquipmentBelongTo _belongTo)
    {
        equipmentData = new EquipmentServerData();
        equipmentData.etype = _eid;
        equipmentData.num = _num;
        equipmentBelongTo = _belongTo;
        isGray = _isGray;
        gs = (EquipmentRef == null ? 0 : EquipmentRef.gs) + equipmentData.Lev;//TO DO:把强化的属性部分引入计算
    }

	public EquipmentInfo(ItemValue itemValue, EquipmentBelongTo _belongTo)
	{
		equipmentData = new EquipmentServerData();
		equipmentData.etype = itemValue.eid;
		equipmentData.num = itemValue.count;
		equipmentBelongTo = _belongTo;
        gs = (EquipmentRef == null ? 0 : EquipmentRef.gs) + equipmentData.Lev;//TO DO:把强化的属性部分引入计算
	}

    public EquipmentInfo(st.net.NetBase.item_des _info,bool _isGotSeverdata = false)
    {
        equipmentData = new EquipmentServerData(_info);
        isGotSeverdata = _isGotSeverdata;
        if (_info.pos >= 10000)
        {
            equipmentBelongTo = EquipmentBelongTo.EQUIP;
		}else if(_info.pos >= 1000)
		{
			equipmentBelongTo = EquipmentBelongTo.STORAGE;
		}else
		{
			equipmentBelongTo = EquipmentBelongTo.BACKPACK;
		}
     
    }

    public EquipmentInfo(st.net.NetBase.item_des _info, EquipmentBelongTo _belongTo)
    {
        equipmentData = new EquipmentServerData(_info);
        equipmentBelongTo = _belongTo;
    }
	/// <summary>
	/// 装备上面的宝石信息(需要宝石type(用于展示)和宝石pos(用于卸下))  by邓成
	/// </summary>
	public EquipmentInfo(st.net.NetBase.pos_des _info,EquipmentBelongTo _belongTo)
	{
		equipmentData = new EquipmentServerData();
		equipmentData.etype = _info.type;
		equipmentData.epos = _info.pos;
		equipmentBelongTo = _belongTo;
	}

	public EquipmentInfo(st.net.NetBase.item_des _info,EmptyType emptyType,EquipmentBelongTo _belongTo)
	{
		equipmentData = new EquipmentServerData(_info);
		equipmentBelongTo = _belongTo;
		CurEmptyType = emptyType;
	}
	public EquipmentInfo(int pos,EmptyType emptyType,EquipmentBelongTo _belongTo)
	{
		equipmentData = new EquipmentServerData();
		equipmentBelongTo = _belongTo;
		equipmentData.epos = pos;
		CurEmptyType = emptyType;
	}

    /// <summary>
    /// 修改物品从属(一般仅用于做起和时装系统的穿戴标示,除非非常了解这个,否则尽量不要用) by吴江
    /// </summary>
    /// <param name="_belongTo"></param>
    public void SetBelongTo(EquipmentBelongTo _belongTo)
    {
        if (BelongTo != _belongTo)
        {
            BelongTo = _belongTo;
            if (OnPropertyUpdate != null) OnPropertyUpdate();
        }
    }

	public void Update(st.net.NetBase.item_des _info,EquipmentBelongTo _belongTo)
	{
		equipmentData = new EquipmentServerData(_info);
		equipmentBelongTo = _belongTo;
		CurEmptyType = EmptyType.NONE;
		if(OnPropertyUpdate != null)
			OnPropertyUpdate();
	}

	public void Update(st.net.NetBase.item_des _info,EmptyType _emptyType,EquipmentBelongTo _belongTo)
	{
		equipmentData = new EquipmentServerData(_info);
		equipmentBelongTo = _belongTo;
		CurEmptyType = _emptyType;
		if(OnPropertyUpdate != null)
			OnPropertyUpdate();
	}

    #endregion

    #region 访问器
	public enum EmptyType
	{
		NONE,
		/// <summary>
		/// 空物品
		/// </summary>
		EMPTY,
		/// <summary>
		/// 锁定的空物品
		/// </summary>
		LOCK,
		/// <summary>
		/// 可解锁的空物品
		/// </summary>
		CANUNLOCK,
		/// <summary>
		/// 正在CD的空物品
		/// </summary>
		CDLOCK,
	}
	/// <summary>
	/// 空物品类型
	/// </summary>
	public EmptyType CurEmptyType = EmptyType.NONE;
    /// <summary>
    /// 实例化ID by 贺丰
    /// </summary>
    public int InstanceID
    {
        get { return equipmentData.eid; }
    }
    /// <summary>
    /// 配置索引 by吴江
    /// </summary>
    public int EID
    {
        get { return equipmentData.etype; }
    }
    /// <summary>
    /// 物品在容器中的位置 by吴江
    /// </summary>
    public int Postion
    {
        get { return equipmentData.epos; }
    }
    /// <summary>
    /// 物品的等级
    /// </summary>
    public int LV
    {
		get { return equipmentRef == null?0:equipmentRef.req_lv; }
    }
	/// <summary>
	/// 强化等级
	/// </summary>
	public int UpgradeLv
	{
		get { return equipmentData.Lev; }
	}

	public int LuckyLv
	{
		get{return equipmentData.lucky_lev;}
	}
	/// <summary>
	/// 是否是攻击套装
	/// </summary>
	public bool isAttackSuit
	{
		get
		{
			return (Family == EquipmentFamily.JEWELRY) || (Family == EquipmentFamily.WEAPON);
		}
	}
	/// <summary>
	/// 是否是防御套装
	/// </summary>
	public bool isDefenseSuit
	{
		get
		{
			return (Family == EquipmentFamily.ARMOR);
		}
	}

	/// <summary>
	/// 强化经验
	/// </summary>
	public int StrengthExp
	{
		get { return equipmentData.strengthExp;}
	}

    public int StrengthExpTime
    {
        get { return equipmentData.exp_expire_time; }
    }
	/// <summary>
	/// 洗练经验
	/// </summary>
	public int WashExp
	{
		get
		{
			return equipmentData.washExp;
		}
	}
	/// <summary>
	/// 镶嵌的宝石的状态
	/// </summary>
	public Dictionary<int,st.net.NetBase.pos_des> InlayGemDic
	{
		get 
		{
			Dictionary<int,st.net.NetBase.pos_des> gemDic = new Dictionary<int,st.net.NetBase.pos_des>();
			for (int i = 0,max = equipmentData.pos_list.Count; i < max; i++) 
			{
				gemDic[equipmentData.pos_list[i].pos] = equipmentData.pos_list[i];
			}
			return gemDic; 
		}
	}
	/// <summary>
	/// 获取可以镶嵌宝石的位置
	/// </summary>
	public int CanInlayPos
	{
		get
		{
			int inlayPos = 0;
			for (int i = 0,max = equipmentData.pos_list.Count; i < max; i++) 
			{
				if(equipmentData.pos_list[i].type == 0)
				{
				//	Debug.Log("CanInlayPos:"+equipmentData.pos_list[i].pos);
				//	return equipmentData.pos_list[i].pos;
					if(inlayPos != 0)
					{
						inlayPos = Mathf.Min(inlayPos,equipmentData.pos_list[i].pos);
					}else
					{
						inlayPos = equipmentData.pos_list[i].pos;
					}
				}

			}
			return inlayPos;
		}
	}
	/// <summary>
	/// 获取要卸下宝石的位置
	/// </summary>
	public int GetUnstallPos(int _gemType)
	{
		for (int i = 0,max = equipmentData.pos_list.Count; i < max; i++) 
		{
			if(equipmentData.pos_list[i].type == _gemType)
			{
				Debug.Log("GetUnstallPos:"+equipmentData.pos_list[i].pos);
				return equipmentData.pos_list[i].pos;
			}
		}
		return 0;
	}
	/// <summary>
	/// 有无镶嵌宝石(是否有镶嵌属性)
	/// </summary>
	public bool HasInlayGem
	{
		get
		{
			for (int i = 0,max = equipmentData.pos_list.Count; i < max; i++) 
			{
				if(equipmentData.pos_list[i].type != 0)
				{
					return true;
				}
			}
			return false;
		}
	}
    /// <summary>
    /// 第一条属性
    /// </summary>
    public int EquOne
    {
        get { return equipmentData.equOne; }
    }
    /// <summary>
    /// 第二条属性
    /// </summary>
    public int EquTwo
    {
        get { return equipmentData.equTwo; }
    }/// <summary>
    /// 第三条属性
    /// </summary>
    public int EquThree
    {
        get { return equipmentData.equThree; }
    }/// <summary>
    /// 第四条属性
    /// </summary>
    public int EquFour
    {
        get { return equipmentData.equFour; }
    }

	public bool haveHighQualityWashAttr
	{
		get
		{
			if(EquOne != 0)
			{
				EquipmentWashValueRef washValue1 = ConfigMng.Instance.GetEquipmentWashValueRefByID(EquOne);
				if(washValue1 != null && washValue1.att_quality > 3)return true;
			}
			if(EquTwo != 0)
			{
				EquipmentWashValueRef washValue2 = ConfigMng.Instance.GetEquipmentWashValueRefByID(EquTwo);
				if(washValue2 != null && washValue2.att_quality > 3)return true;
			}
			if(EquThree != 0)
			{
				EquipmentWashValueRef washValue3 = ConfigMng.Instance.GetEquipmentWashValueRefByID(EquThree);
				if(washValue3 != null && washValue3.att_quality > 3)return true;
			}
			if(EquFour != 0)
			{
				EquipmentWashValueRef washValue4 = ConfigMng.Instance.GetEquipmentWashValueRefByID(EquFour);
				if(washValue4 != null && washValue4.att_quality > 3)return true;
			}	
			return false;
		}
	}
    /// <summary>
    /// 类型 by吴江
    /// </summary>
    public EquipmentFamily Family
    {
        get { return EquipmentRef == null ? EquipmentFamily.NORMAL : EquipmentRef.family; }
    }

	public bool IsEquip
	{
		get
		{
			return Family == EquipmentFamily.WEAPON || Family == EquipmentFamily.ARMOR || Family == EquipmentFamily.JEWELRY;
		}
	}

    /// <summary>
    /// 骨骼特效列表
    /// </summary>
    public List<BoneEffectRef> BoneEffectList
    {
        get
        {
			return EquipmentRef == null ? new List<BoneEffectRef>() : EquipmentRef.boneEffectList;
        }
    }

    ///// <summary>
    ///// 获取列表
    ///// </summary>
    //public List<int> Access
    //{
    //    get
    //    {
    //        return equipmentRef == null ? new List<int>() : equipmentRef.access;
    //    }
    //}

	public string FamilyName
	{
		get
		{
			switch(Family)
			{
			case EquipmentFamily.WEAPON:
                    return ConfigMng.Instance.GetUItext(142);
			case EquipmentFamily.ARMOR:
                    return ConfigMng.Instance.GetUItext(143);
			case EquipmentFamily.JEWELRY:
                    return ConfigMng.Instance.GetUItext(144);
			case EquipmentFamily.COSMETIC:
                    return ConfigMng.Instance.GetUItext(145);
			case EquipmentFamily.PET:
                    return ConfigMng.Instance.GetUItext(146);
			case EquipmentFamily.POTION:
                    return ConfigMng.Instance.GetUItext(147);
			case EquipmentFamily.MATERIAL:
                    return ConfigMng.Instance.GetUItext(148);
			case EquipmentFamily.TASK:
                    return ConfigMng.Instance.GetUItext(149);
			case EquipmentFamily.GEM:
                    return ConfigMng.Instance.GetUItext(150);
			case EquipmentFamily.MOUNT:
                    return ConfigMng.Instance.GetUItext(151);
            case EquipmentFamily.NORMAL:
                    return ConfigMng.Instance.GetUItext(152);
            case EquipmentFamily.CONSUMABLES:
                    return ConfigMng.Instance.GetUItext(153);
            case EquipmentFamily.MOUNTEQUIP:
                    return ConfigMng.Instance.GetUItext(154);
			default:
				return string.Empty;
			}
		}
	}
    //protected ItemFromType fromType = ItemFromType.None;
    ///// <summary>
    ///// 标识物品从哪儿来 
    ///// </summary>
    //public ItemFromType FromType
    //{
    //    get
    //    {
    //        return fromType;
    //    }
    //}
	/// <summary>
	/// 物品从哪儿来
	/// </summary>
    //public string FromTypeStr
    //{
    //    get
    //    {
    //        switch(FromType)
    //        {
    //        case ItemFromType.DoubleReward:
    //            return "活动";
    //        default:
    //            return string.Empty;
    //        }
    //    }
    //}

    /// <summary>
    /// 穿戴位置 by吴江
    /// </summary>
    public EquipSlot Slot
    {
        get { return EquipmentRef == null ? EquipSlot.None : EquipmentRef.slot; }
    }

	public string SlotName
	{
		get
		{
			switch(Slot)
			{
			case EquipSlot.badge:
                    return ConfigMng.Instance.GetUItext(155);
			case EquipSlot.weapon:
                    return ConfigMng.Instance.GetUItext(142);
			case EquipSlot.necklace:
                    return ConfigMng.Instance.GetUItext(156);
			case EquipSlot.bracers:
                    return ConfigMng.Instance.GetUItext(157);
			case EquipSlot.ring:
                    return ConfigMng.Instance.GetUItext(158);
			case EquipSlot.glove:
                    return ConfigMng.Instance.GetUItext(159);
			case EquipSlot.head:
                    return ConfigMng.Instance.GetUItext(160);
			case EquipSlot.sarmor:
                    return ConfigMng.Instance.GetUItext(161);
			case EquipSlot.body:
                    return ConfigMng.Instance.GetUItext(162);
			case EquipSlot.belt:
                    return ConfigMng.Instance.GetUItext(163);
			case EquipSlot.shoes:
                    return ConfigMng.Instance.GetUItext(164);
			case EquipSlot.special:
                    return ConfigMng.Instance.GetUItext(165);
			case EquipSlot.wing:
                    return ConfigMng.Instance.GetUItext(166);
			case EquipSlot.magicweapon:
                    return ConfigMng.Instance.GetUItext(167);
            case EquipSlot.Headband:
                    return ConfigMng.Instance.GetUItext(168);
            case EquipSlot.Armor:
                    return ConfigMng.Instance.GetUItext(168);
            case EquipSlot.Saddle:
                    return ConfigMng.Instance.GetUItext(169);
            case EquipSlot.Hoofsteel:
                    return ConfigMng.Instance.GetUItext(170);
            case EquipSlot.Toko:
                    return ConfigMng.Instance.GetUItext(171);
            case EquipSlot.Whip:
                    return ConfigMng.Instance.GetUItext(172);
            case EquipSlot.Reins:
                    return ConfigMng.Instance.GetUItext(173);
            case EquipSlot.Ornaments:
                    return ConfigMng.Instance.GetUItext(174);
			}
			return string.Empty;
		}
	}

    /// <summary>
    /// 模型处理方式 by吴江
    /// </summary>
    public ShowType ShowType
    {
        get { return EquipmentRef == null ? ShowType.NO : EquipmentRef.showType; }

    }
    /// <summary>
    /// 强化等级是否达到上限 
    /// </summary>
    /// <value>
    /// The max power lv.
    /// </value>
    //public bool MaxPowerLv
    //{
    //    get
    //    {
    //        return UpGradeLevel >= EquipmentRef.maxPowerLv;
    //    }
    //}
    /// <summary>
    /// 预览缩放大小
    /// </summary>
    public float PreviewScale
    {
        get
        {
            return EquipmentRef == null ? 1.0f : EquipmentRef.previewScale;
        }
    }

    /// <summary>
    /// 属性等级
    /// </summary>
    public int PropLv
    {
        get
        {
            return EquipmentRef == null ? 1 : EquipmentRef.propLv;
        }
    }
    /// <summary>
    /// 是否转职前装备
    /// </summary>
    public bool IsLowProfBelong
    {
        get
        {
            //PlayerConfig config = ConfigMng.Instance.GetPlayerConfig(NeedProf);
            //if (config != null)
            //{
            //    return config.form_ID.Count == 0;
            //}
            return true;
        }
    }

	/// <summary>
	/// 强化上限等级 
	/// </summary>
	public int MaxPowerLvValue{
		get{
			return EquipmentRef != null?EquipmentRef.maxPowerLv:0;
		}
	}

    /// <summary>
    /// 预览时的相机距离 
    /// </summary>
    public Vector3 PreviewPosition
    {
        get
        {
            if (EquipmentRef != null)
            {
                return EquipmentRef.previewPositon;
            }
            return Vector3.zero;
        }

    }

    /// <summary>
    /// 预览时的相机角度
    /// </summary>
    public Vector3 PreviewRotation
    {
        get
        {
            if (EquipmentRef != null)
            {
                return EquipmentRef.previewRotation;
            }
            return Vector3.zero;
        }
    }

    /// <summary>
    /// 战斗中武器挂点名字 by吴江
    /// </summary>
    public string FightWeaponPointName
    {
        get
        {
            return EquipmentRef == null ? string.Empty : EquipmentRef.bPoint;
        }
    }

    /// <summary>
    /// 非战斗中武器挂点名字 by吴江
    /// </summary>
    public string UnFightWeaponPointName
    {
        get
        {
            return EquipmentRef == null ? string.Empty : EquipmentRef.rPoint;
        }
    }
    /// <summary>
    /// 是否为新增物品
    /// </summary>
    public bool isNew
    {
        get
        {
            return false;
        }
        set
        {
        }
    }
    /// <summary>
    /// 物品对应的资源ID（不是资源为0）
    /// </summary>
    public int ServerResID
    {
        get
        {
            return equipmentRef == null ? 0 : equipmentRef.serverResId;
        }
    }
    /// <summary>
    /// 骑装战力计算
    /// </summary>
    protected int MountEquipGS
    {
        get
        {
            int gs = 0;
            if (Family != EquipmentFamily.MOUNTEQUIP)
                return gs;
            MountStrenLevRef mountStrenLev = ConfigMng.Instance.GetMountStrenLevRef(UpgradeLv,Slot);
            if (mountStrenLev != null) gs += mountStrenLev.gs;
            MountEquQualityAttributeRef mountQuality = ConfigMng.Instance.GetMountEquQualityAttributeRef(Quality,Slot);
            if (mountQuality != null) gs += mountQuality.gs;
            return gs;
        }
    }

    /// <summary>
    /// 装备分数
    /// </summary>
    public int GS
    {
        get
        {
			//装备基础战力 + 强化战力+幸运战力 + 洗练战力+镶嵌战力 
            int Gs = 0;
			if(EquipmentRef == null)
				return Gs;
            if (Family == EquipmentFamily.MOUNTEQUIP)
                return MountEquipGS;
			//基础
			Gs += EquipmentRef.gs;
			StrengthenAttrRef strengthRef = ConfigMng.Instance.GetStrengthenAttrRefByLv(UpgradeLv,Slot);
			if(strengthRef != null)
				Gs += strengthRef.GS;
			//幸运
			Gs += LuckyLv * 24;
			//洗练
			EquipmentWashValueRef washValue = null;
			if (equipmentData.equOne != 0)
			{
				washValue = ConfigMng.Instance.GetEquipmentWashValueRefByID(equipmentData.equOne);
				Gs += washValue == null?0:washValue.gs;
			}
            if (equipmentData.equTwo != 0) 
			{
				washValue = ConfigMng.Instance.GetEquipmentWashValueRefByID(equipmentData.equTwo);
				Gs += washValue == null?0:washValue.gs;
			}
			if (equipmentData.equThree != 0) 
			{
				washValue = ConfigMng.Instance.GetEquipmentWashValueRefByID(equipmentData.equThree);
				Gs += washValue == null?0:washValue.gs;
			}
			if (equipmentData.equFour != 0) 
			{
				washValue = ConfigMng.Instance.GetEquipmentWashValueRefByID(equipmentData.equFour);
				Gs += washValue == null?0:washValue.gs;
			}
			//镶嵌
			st.net.NetBase.pos_des gem = null;
			EquipmentRef gemRef = null;
			for (int i = 0,max = equipmentData.pos_list.Count; i < max; i++) 
			{
				gem = equipmentData.pos_list[i];
				if(gem.type != 0)
					gemRef = ConfigMng.Instance.GetEquipmentRef(gem.type);
				if(gemRef != null)
					Gs += gemRef.gs;
			}
            return Gs;
        }
    }
    /// <summary>
    /// 基础评分
    /// </summary>
    public int BaseGS 
    {
        get
        {
            return EquipmentGs;
        }
    }
    /// <summary>
    /// 模型名称 by吴江
    /// </summary>
    public string ModelName
    {
        get { return EquipmentRef == null ? "0" : EquipmentRef.equip_model.ToLower(); }
    }
    /// <summary>
    /// 资源路径 by吴江
    /// </summary>
    public string ShortUrl
    {
		get 
		{ 
			if(EquipmentRef == null)
				return string.Empty;
			return AssetMng.GetPathWithExtension(EquipmentRef.equip_model.ToLower(), AssetPathType.PersistentDataPath); 
		}
    }

    /// <summary>
    /// 穿上后是否改变外形
    /// </summary>
    public bool WillChangeRender
    {
        get
        {
            if (EquipmentRef == null) return false;
            if (EquipmentRef.slot == EquipSlot.None || EquipmentRef.slot >= EquipSlot.count) return false;
            return true;
        }
    }

    /// <summary>
    /// 旧的sort字段 by吴江
    /// </summary>
    public int OldSort
    {
		get { return EquipmentRef==null?0:EquipmentRef.oldSort; }
    }
    /// <summary>
    /// 服务端的唯一实例ID  by吴江
    /// </summary>
    //public int InstanceID
    //{
    //    get { return equipmentData.instanceID; }
    //}
    /// <summary>
    /// 物品的图片名称 by吴江
    /// </summary>
    public string IconName
    {
        get { return EquipmentRef == null ? string.Empty : EquipmentRef.item_res; }
    }
    //public bool IsProtect
    //{
    //    get{return (equipmentData.isProtect == 1);}
    //}

    /// <summary>
    /// 物品名称 by吴江
    /// </summary>
    public string ItemName
    {
		get { return EquipmentRef == null?string.Empty:EquipmentRef.name; }
    }
    /// <summary>
    /// 物品的最大堆叠数量 by吴江
    /// </summary>
    public int StackMaxCount
    {
        get { return EquipmentRef == null ? 0 : EquipmentRef.max; }
    }
    /// <summary>
    /// 物品的当前堆叠数量 by吴江
    /// </summary>
    public int StackCurCount
    {
        get { return equipmentData.num; }
    }
    /// <summary>
    /// 物品当前的强化等级 by吴江
    /// </summary>
    //public int UpGradeLevel
    //{
    //    get { return equipmentData.Lev; }
    //}
    /// <summary>
    /// 物品当前所处的归属 by吴江
    /// </summary>
    public EquipmentBelongTo BelongTo
    {
        set
        {
            equipmentBelongTo = value;
        }
        get { return equipmentBelongTo; }
    }
    /// <summary>
    /// 物品的品质 by吴江
    /// </summary>
    public int Quality
    {
        get 
        {
            if (EquipmentRef == null)
                return 0;
            if(Family != EquipmentFamily.MOUNTEQUIP)
                return EquipmentRef.quality; 
            else
                return equipmentData.color;
        }
    }
	/// <summary>
	/// 物品获取途径
	/// </summary>
	public string EquipGetAddress
	{
		get{return EquipmentRef == null ? "0": EquipmentRef.attention; }
	}
    /// <summary>
    /// 物品出售价格 by 
    /// </summary>
    public int Price
    {
        get
        {
            return EquipmentRef == null ? 0 : EquipmentRef.price;
        }
    }

    /// <summary>
    /// 是否可以回收
    /// </summary>
    public GoodsRecycleType RecycleType
    {
        get 
        {
            return EquipmentRef == null ? GoodsRecycleType.YES : EquipmentRef.recycleType;
        }
    }
    /// <summary>
    /// 物品是否需要获得提示
    /// </summary>
    public GoodsAttentionType AttentionType
    {
        get
        {
            return EquipmentRef == null ? GoodsAttentionType.NO : EquipmentRef.attentionType;
        }
    }
	/// <summary>
	/// 使用该物品后打开的界面
	/// </summary>
	public int OpenUiType
	{
		get
		{
			return EquipmentRef == null ?0:EquipmentRef.openUiType;
		}
	}
    /// <summary>
    /// 是否有模型
    /// </summary>
    public bool HasModel()
    {
        if (EquipmentRef == null) return false;
        bool hasModel = EquipmentRef.equip_model != string.Empty && EquipmentRef.equip_model != "0";
        return hasModel;
    }

    /// <summary>
    /// 钻石购买的价格,为0表示不能用钻石购买.
    /// </summary>
    public float DiamondPrice
    {
        get
        {
			return EquipmentRef == null?0:EquipmentRef.diamonPrice;
        }
    }

    /// <summary>
    /// 物品颜色 by吴江
    /// </summary>
    public Color ItemColor
    {
        get
        {
			if(EquipmentRef == null)
				return Color.white;
            return GetQualityColor(Quality);
        }
    }

    public string ItemStrColor
    {
        get
        {
            if (EquipmentRef == null)
                return "[000000]";
            return ItemColorStr(Quality);

        }
    }

	/// <summary>
	/// 品质框  
	/// </summary>
	public string QualityBox
	{
		get
		{
			if(EquipmentRef == null)
				return string.Empty;
            switch (Quality)
			{
			case 1:
				return "Icon_Quality_White";
			case 2:
				return "Icon_Quality_lv";
			case 3:
				return "Icon_Quality_lan";
			case 4:
				return "Icon_Quality_zi";
			case 5:
				return "Icon_Quality_cheng";
			default:
				return "Icon_Quality_White";
			}
		}
	}
    public string QualityName
    {
        get
        {
            if (EquipmentRef == null)
                return string.Empty;
            return GetQualityName(EquipmentRef.quality);
        }
    }
    /// <summary>
    /// 是否已经绑定 by吴江
    /// </summary>
    public bool IsBind
    {
        get { return equipmentData.ebind == 1; }
    }
    /// <summary>
    /// 绑定规则 by吴江
    /// </summary>
    public EquipmentBindType BindType
    {
        get
        {
			return EquipmentRef == null?EquipmentBindType.UnBind:(EquipmentBindType)EquipmentRef.bind;
        }
    }
    /// <summary>
    /// 使用需求职业  by吴江
    /// </summary>
    public int NeedProf
    {
        get
        {
            return EquipmentRef == null ? -1 : EquipmentRef.req_prof;
        }
    }
    /// <summary>
    /// 是否满足职业需求或者满足转职后职业需求 
    /// </summary>
    //public bool IsNeedProf
    //{
    //    get
    //    {
    //        return NeedProf == 0 || NeedProf == GameCenter.curMainPlayer.Prof
    //            || GameCenter.mainPlayerMng.MainPlayerInfo.FromProf == NeedProf || GameCenter.mainPlayerMng.MainPlayerInfo.UpProf.Contains(NeedProf);
    //    }
    //}
    /// <summary>
    /// 使用需求等级 by吴江
    /// </summary>
    public int UseReqLevel
    {
        get
        {
            return EquipmentRef == null ? 0 : EquipmentRef.req_lv; ;
        }
    }

    /// <summary>
    /// 物品描述
    /// </summary>
    public string Description
    {
        get { return EquipmentRef == null ? string.Empty : EquipmentRef.des; }
    }

    /// <summary>
    /// 能否批量使用
    /// </summary>
    public bool CanUseBatch
    {
        get { return EquipmentRef == null ? false : EquipmentRef.multi_open > 0; }
    }
	/// <summary>
	/// 是否可升阶
	/// </summary>
	public bool CanUpgrade
	{
		get
		{
			if(EquipmentRef == null || Quality < 4 || (EquipmentRef.slot == EquipSlot.None && EquipmentRef.family != EquipmentFamily.GEM))return false;
			PromoteRef promoteRef = ConfigMng.Instance.GetPromoteRefByEid(EID);
			return promoteRef != null;
		}
	}
	/// <summary>
	/// 是否可升阶,且材料足够
	/// </summary>
	public bool RealCanUpgrade
	{
		get
		{
            if (Family == EquipmentFamily.MOUNTEQUIP)
            {
                MountEquQuailtRef upgradeConsumeRef = ConfigMng.Instance.GetMountEquipQualityRef(Quality, Slot);
                if (upgradeConsumeRef == null) return false;
                int maxLev = ConfigMng.Instance.GetMountEquQualityMaxUpgradeLv(Quality);
                if (UpgradeLv != maxLev) return false;//是否达到升品要求
                bool enough = true;
                for (int i = 0, max = upgradeConsumeRef.consume.Count; i < max; i++)
                {
                    GameHelper.GetStringWithBagNumber(upgradeConsumeRef.consume[i], out enough);
                    if (enough == false) return false;
                }
                return enough;
            }
            else
            {
                if (EquipmentRef == null || LV > GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel || Quality < 4 || (EquipmentRef.slot == EquipSlot.None && EquipmentRef.family != EquipmentFamily.GEM)) return false;
                PromoteRef promoteRef = ConfigMng.Instance.GetPromoteRefByEid(EID);
                if (promoteRef == null) return false;
                if ((ulong)promoteRef.coin > GameCenter.mainPlayerMng.MainPlayerInfo.TotalCoinCount) return false;
                EquipmentRef equip = ConfigMng.Instance.GetEquipmentRef(promoteRef.end_item);
                if (equip == null || equip.req_lv > GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel)
                    return false;
                bool enoughMaterial = true;
                for (int i = 0, max = promoteRef.materialItem.Count; i < max; i++)
                {
                    GameHelper.GetStringWithBagNumber(promoteRef.materialItem[i], out enoughMaterial);
                    if (enoughMaterial == false) return false;//任一个材料不足则不足
                }
                return enoughMaterial;
            }
		}
	}

	/// <summary>
	/// 是否可橙炼
	/// </summary>
	public bool CanOrangeRefine
	{
		get
		{
			if(EquipmentRef == null || Quality != 4 || (EquipmentRef.slot == EquipSlot.None && EquipmentRef.family != EquipmentFamily.GEM))return false;
			OrangeRefineRef orangeRefineRef = ConfigMng.Instance.GetOrangeRefineRefByEid(EID);
			return orangeRefineRef != null;
		}
	}
	/// <summary>
	/// 是否可橙炼,且材料足够
	/// </summary>
	public bool RealCanOrangeRefine
	{
		get
		{
			if(EquipmentRef == null || Quality != 4 || (EquipmentRef.slot == EquipSlot.None && EquipmentRef.family != EquipmentFamily.GEM))return false;
			OrangeRefineRef orangeRefineRef = ConfigMng.Instance.GetOrangeRefineRefByEid(EID);
			if(orangeRefineRef == null)return false;
            if ((ulong)orangeRefineRef.coin > GameCenter.mainPlayerMng.MainPlayerInfo.TotalCoinCount) return false;
			bool enoughMaterial = true;
			for (int i = 0,max=orangeRefineRef.materialItem.Count; i < max; i++) 
			{
				GameHelper.GetStringWithBagNumber(orangeRefineRef.materialItem[i],out enoughMaterial);
				if(enoughMaterial == false)return false;//任一个材料不足则不足
			}
			return enoughMaterial;
		}
	}
	/// <summary>
	/// 是否可洗练
	/// </summary>
	public bool CanWash
	{
		get
		{
			if(EquipmentRef == null || (EquipmentRef.slot == EquipSlot.None && EquipmentRef.family != EquipmentFamily.GEM))return false;
			EquipmentWashConsumeRef consumeRef = ConfigMng.Instance.GetEquipmentWashConsumeRefByQuality(Quality);
			return consumeRef != null;
		}
	}
	/// <summary>
	/// 是否可洗练,且材料足够
	/// </summary>
	public bool RealCanWash
	{
		get
		{
            if (EquipmentRef == null || (EquipmentRef.slot == EquipSlot.None && EquipmentRef.family != EquipmentFamily.GEM)) return false;
            EquipmentWashConsumeRef consumeRef = ConfigMng.Instance.GetEquipmentWashConsumeRefByQuality(Quality);
            if (consumeRef == null) return false;
            bool enough = true;
            for (int i = 0, max = consumeRef.consumeItem.Count; i < max; i++)
            {
                GameHelper.GetStringWithBagNumber(consumeRef.consumeItem[i], out enough);
                if (enough == false) return false;//任一个材料不足则不足
            }
            return enough;
		}
	}
	/// <summary>
	/// 是否可强化,且材料足够
	/// </summary>
	public bool RealCanStrength
	{
		get
		{
            if (Family == EquipmentFamily.MOUNTEQUIP)
            {
                MountStrenConsumeRef mountStrenConsume = ConfigMng.Instance.GetMountStrenConsumeRef(UpgradeLv);
                if (mountStrenConsume == null) return false;
                int maxLev = ConfigMng.Instance.GetMountEquQualityMaxUpgradeLv(Quality);
                if(UpgradeLv == maxLev)return false;//是否达到升品要求
                bool enough = true;
                for (int i = 0, max = mountStrenConsume.item.Count; i < max; i++)
                {
                    GameHelper.GetStringWithBagNumber(mountStrenConsume.item[i], out enough);
                    if (enough == false) return false;
                }
                return enough;
            }
            else
            {
                if (EquipmentRef == null || (EquipmentRef.slot == EquipSlot.None && EquipmentRef.family != EquipmentFamily.GEM) || (UpgradeLv >= MaxPowerLvValue)) return false;
                StrengthenRef strengthenRef = ConfigMng.Instance.GetStrengthenRefByLv(UpgradeLv + 1);
                if (strengthenRef == null) return false;
                if ((ulong)strengthenRef.coin > GameCenter.mainPlayerMng.MainPlayerInfo.TotalCoinCount) return false;
                bool enough = true;
                for (int i = 0, max = strengthenRef.items.Count; i < max; i++)
                {
                    GameHelper.GetStringWithBagNumber(strengthenRef.items[i], out enough);
                    if (enough == false) return false;//任一个材料不足则不足
                }
                return enough;
            }
		}
	}

	/// <summary>
	/// 是否可镶嵌
	/// </summary>
	public bool CanInlay
	{
		get
		{
			if(EquipmentRef == null || (EquipmentRef.slot == EquipSlot.None && EquipmentRef.family != EquipmentFamily.GEM))return false;
			return equipmentData.pos_list.Count > 0;
		}
	}
	/// <summary>
	/// 是否可继承(继承中主装备必须有强化or有幸运or有洗练)
	/// </summary>
	public bool CanExtend
	{
		get
		{
			return (UpgradeLv > 0) || (LuckyLv > 0) || (EquOne > 0);
		}
	}

    /// <summary>
    /// 物品属性列表 by吴江  
    /// </summary>
    public List<AttributePair> AttributePairList
    {
        get 
		{ 
			if(EquipmentRef != null)
			{
				return EquipmentRef.attributePairList;
			}
			return null;
		}
    }

	public List<AttributePair> FinalAttributePairList
	{
		get 
		{ 
			if(EquipmentRef != null)
			{
				if(UpgradeLv > 0)
					return StrengthValue;
				else
					return EquipmentRef.attributePairList;
			}
			return null;
		}
	}
	public int GetStrengthValueByTag(ActorPropertyTag tag)
	{
		foreach(AttributePair attr in StrengthValue)
		{
			if(attr.tag == tag)
				return attr.value;
		}
		return 0;
	}

    /// <summary>
    /// 获取任意强化等级的属性
    /// </summary>
    /// <param name="_strengthLevel"></param>
    /// <returns></returns>
	protected List<AttributePair> GetStrengthValue(int _strengthLevel)
    {
        if (Family != EquipmentFamily.MOUNTEQUIP)
        {
            StrengthenAttrRef attrRef = ConfigMng.Instance.GetStrengthenAttrRefByLv(_strengthLevel, Slot);
            return attrRef == null ? new List<AttributePair>() : attrRef.attrs;
        }
        else
        {
            MountStrenLevRef mountStrenLevRef = ConfigMng.Instance.GetMountStrenLevRef(_strengthLevel,Slot);
            MountEquQualityAttributeRef mountEquipQualityAttrRef = ConfigMng.Instance.GetMountEquQualityAttributeRef(Quality,Slot);
            List<AttributePair> attrs = new List<AttributePair>();
            if (mountStrenLevRef == null || mountEquipQualityAttrRef == null)
                return attrs;
            int length = Mathf.Min(mountStrenLevRef.attr.Count,mountEquipQualityAttrRef.attrs.Count);
            for (int i = 0; i < length; i++)
            {
                AttributePair strenAttr = mountStrenLevRef.attr[i];
                for (int j = 0; j < length; j++)
                {
                    AttributePair qualityAttr = mountEquipQualityAttrRef.attrs[j];
                    if (qualityAttr.tag == strenAttr.tag)
                    {
                        attrs.Add(new AttributePair(qualityAttr.tag, qualityAttr.value + strenAttr.value));
                    }
                }
            }
            return attrs;
        }
    }

    /// <summary>
    /// 获取任意品质的属性(仅限骑装)
    /// </summary>
    /// <param name="_strengthLevel"></param>
    /// <returns></returns>
    protected List<AttributePair> GetQualityValue(int _quality)
    {
        MountStrenLevRef mountStrenLevRef = ConfigMng.Instance.GetMountStrenLevRef(UpgradeLv, Slot);
        MountEquQualityAttributeRef mountEquipQualityAttrRef = ConfigMng.Instance.GetMountEquQualityAttributeRef(_quality, Slot);
        List<AttributePair> attrs = new List<AttributePair>();
        if (mountStrenLevRef == null || mountEquipQualityAttrRef == null)
            return attrs;
        int length = Mathf.Min(mountStrenLevRef.attr.Count, mountEquipQualityAttrRef.attrs.Count);
        for (int i = 0; i < length; i++)
        {
            AttributePair strenAttr = mountStrenLevRef.attr[i];
            for (int j = 0; j < length; j++)
            {
                AttributePair qualityAttr = mountEquipQualityAttrRef.attrs[j];
                if (qualityAttr.tag == strenAttr.tag)
                {
                    attrs.Add(new AttributePair(qualityAttr.tag, qualityAttr.value + strenAttr.value));
                }
            }
        }
        return attrs;
    }

	public List<AttributePair> StrengthValue
    {
        get
        {
			return GetStrengthValue(UpgradeLv);
        }
    }
    /// <summary>
    /// 获取下一级强化属性
    /// </summary>
    public List<AttributePair> NextStrengthValue
    {
        get { return GetStrengthValue(UpgradeLv + 1); }
    }
    /// <summary>
    /// 获取下一品质的属性(仅限骑装)
    /// </summary>
    public List<AttributePair> NextQualityValue
    {
        get { return GetQualityValue(Quality+1); }
    }

    /// <summary>
    /// 附加属性数量，来自静态表数据
    /// </summary>
    public int countExtraAttr
    {
        get
        {
            if (equipmentRef != null)
            {
                if (equipmentRef.profNum.IndexOf("3") != -1)
                    return 3;
                if (equipmentRef.profNum.IndexOf("2") != -1)
                    return 2;
                if (equipmentRef.profNum.IndexOf("1") != -1)
                    return 1;
            }
            return 0;
        }
    }

    /// <summary>
    /// 物品能否被使用
    /// </summary>
    public bool CanUse
    {
		get { return EquipmentRef == null ? false : EquipmentRef.action != EquipActionType.none; }
    }

    /// <summary>
    /// 物品能否被销毁
    /// </summary>
    public bool CanDiscard
    {
        get { return EquipmentRef == null ? false : EquipmentRef.discard > 0; }
    }
    /// <summary>
    /// 物品是否锁定（内） by 沙新佳
    /// </summary>
    private bool isLocked = false;
    /// <summary>
    /// 物品是否锁定（外） by 沙新佳
    /// </summary>
    public bool IsLocked
    {
        get
        {
            return isLocked;
        }
        set
        {
            if (isLocked != value)
            {
                isLocked = value;
                if (OnPropertyUpdate != null) OnPropertyUpdate();
            }

        }
    }

    public CDValue CDInfo
    {
        get
        {
            if (EquipmentRef == null)
                return null;
            return EquipmentRef.use_cd;
        }
    }

    /// <summary>
    /// 是否在冷却中
    /// </summary>
    public bool isOnCD
    {
        get
        {
            if (CDInfo == null||CDInfo.id == 0||!GameCenter.inventoryMng.UseCD.ContainsKey(CDInfo.id))
                return false;
            if (GameCenter.inventoryMng.UseCD[CDInfo.id] == 0)
                return false;
            if (Time.time - GameCenter.inventoryMng.UseCD[CDInfo.id] < CDInfo.time/1000)
                return true;
            return false;

        }
    }




    public bool CanSell
    {
        get
        {
            return (EquipmentRef.business != 0);
        }
    }
	/// <summary>
	/// 宝石可升级,且残渣足够  
	/// </summary>
    //public bool GemRealCanLevUp
    //{
    //    get
    //    {
    //        if(Family == EquipmentFamily.GEM)
    //        {
    //            if(UpGradeLevel < 20)
    //            {
    //                GemImproveLevelRef theGem = ConfigMng.Instance.GetNextGemImproveLevelRef(UpGradeLevel+1,Quality);
    //                return (theGem.multy <= GameCenter.mainPlayerMng.MainPlayerInfo.GemItems);
    //            }
    //        }
    //        return false;
    //    }
    //}
	/// <summary>
	/// 可强化,且强化石和钱足够  
	/// </summary>
    //public bool CanRealUpGrade
    //{
    //    get
    //    {
    //        if(CanBeStrenth && MaxPowerLv == false && Family != EquipmentFamily.COSMETIC)
    //        {
    //            StrengtheningInfo info = new StrengtheningInfo();
    //            StrengThening_ConsumeRef consume = ConfigMng.Instance.GetStrengtheningRef(UseReqLevel);
    //            if(consume != null)
    //            {
    //                info.stuffItemId = consume.need_item_id;
    //                info.needMoney = consume.need_coin;
    //            }				
    //            StrengThening_dataRef multData = ConfigMng.Instance.GetStrengtheningMultRef(UpGradeLevel+1, Quality, OldSort);
    //            if(multData != null)
    //            {
    //                info.stuffItemId = multData.need_item_id;
    //                info.stuffItemCount = multData.need_item_num;
    //                info.needMoney = info.needMoney*multData.multy/10000;
    //            }
    //            if(info.needMoney <= GameCenter.mainPlayerMng.MainPlayerInfo.Coin)
    //            {
    //                int count = GameCenter.inventoryMng.GetEqCountByEID(info.stuffItemId,EquipmentBelongTo.BACKPACK);
    //                return info.stuffItemCount <= count;
    //            }
    //        }
    //        return false;
    //    }
    //}
    /// <summary>
    /// 物品使用行为类型
    /// </summary>
    public EquipActionType ActionType
    {
        get { return EquipmentRef == null ? EquipActionType.none : EquipmentRef.action; }
    }

	protected string doubleStr = string.Empty;
	public string DoubleStr
	{
		get
		{
			return doubleStr;
		}
	}

    /// <summary>
    /// 行为参数
    /// </summary>
    public string ActionArg
    {
        get
        {
            return EquipmentRef == null ? "" : EquipmentRef.action_arg;
        }
    }
    /// <summary>
    /// 宠物技能书等级
    /// </summary>
    public int BookLev
    {
        get
        {
            return EquipmentRef == null ? -1 : EquipmentRef.psetSkillLevel;
        }
    }
	/// <summary>
	/// 使用没有前提条件
	/// </summary>
	public bool UseNoCondition
	{
		get
		{
			if(EquipmentRef == null)
				return false;
			foreach(int item in EquipmentRef.conditionsItem)
			{
				return (item == 0);//EquipmentRef.conditionsItem为0,则使用物品没有前提条件
			}
			return true;
		}
	}
    //public List<EquipmentInfo> ConditionItems
    //{
    //    get
    //    {
    //        if(UseNoCondition)
    //            return null;
    //        List<EquipmentInfo> list = new List<EquipmentInfo>();
    //        int len = Mathf.Min(EquipmentRef.conditionsItem.Count,EquipmentRef.conditionsCount.Count);
    //        for(int i=0;i<len;i++)
    //        {
    //            EquipmentInfo temp = new EquipmentInfo(EquipmentRef.conditionsItem[i],EquipmentRef.conditionsCount[0],EquipmentBelongTo.PREVIEW);
    //            list.Add(temp);
    //        }
    //        return list;
    //    }
    //}
    #endregion



    public System.Action OnPropertyUpdate;


    public bool IsBetter(EquipmentInfo _info)
    {
        if (_info == null) return false;
        if (this.CheckClass(GameCenter.mainPlayerMng.MainPlayerInfo.Prof))
        {
            return BaseGS > _info.BaseGS;
        }
        return false;
    }
    public bool IsLower(EquipmentInfo _info)
    {
        if (_info == null) return false;
        if (this.CheckClass(GameCenter.mainPlayerMng.MainPlayerInfo.Prof))
        {
            return BaseGS < _info.BaseGS;
        }
        return false;
    }
	/// <summary>
	/// 与身上同位置的装备对比
	/// </summary>
	public bool IsBetterSlot(EquipmentInfo _info)
	{
        if(_info == null)return false;
        if(_info.Slot == EquipSlot.None)return false;
		if(_info.Family == EquipmentFamily.GEM)return false;
		EquipmentInfo equip = GameCenter.inventoryMng.GetEquipFromEquipDicBySlot(_info.Slot);
        if(equip != null)
            return _info.BaseGS > equip.BaseGS;
		return true;//身上没有装备
	}

    public static string ItemColorStr(int _quality)
    {
        switch (_quality)
        {
            case 1:
                return "[e7ffe8]";
            case 2:
                return "[6ef574]";
            case 3:
                return "[3cb3ff]";
            case 4:
				return "[bd54ff]";
            case 5:
                return "[ff7b2c]";
            default:
				return "[e7ffe8]";
        }
    }


    public static Color GetQualityColor(int _quality)
    {
        switch (_quality)
        {
            case 1:
				return new Color(231f / 255f, 255f / 255f, 232f / 255f);
            case 2:
                return new Color(110f / 255f, 245f/255f, 116f / 255f);
            case 3:
                return new Color(60f/255f, 179f/255, 1f);
            case 4:
                return new Color(189f / 255f, 84f / 255f, 1f);
            case 5:
                return new Color(1.0f, 123f / 255f, 44f / 255f);
            default:
				return new Color(231f / 255f, 255f / 255f, 232f / 255f);
        }
    }


    public static Color GetStarColor(int _star)
    {
        switch ((_star-1)/10)
        {
            case 0:
				return new Color(157f / 255f, 157f / 255f, 157f / 255f);
            case 1:
                return new Color(195f / 255f, 1f, 58f / 255f);//45,255,86
            case 2:
                return new Color(20f / 255f, 225f / 255f, 1f);//0,146,255
            case 3:
                return new Color(198f / 255f, 93f / 255f, 1f);//157,47,193
            case 4:
                return new Color(1.0f, 150f / 255f, 14f / 255f);//255,175,60
            default:
                return Color.gray;
        }
    }

    public static string GetQualityName(int _quality)
    {
        switch (_quality)
        {
            case 1:
                return ConfigMng.Instance.GetUItext(175);
            case 2:
                return ConfigMng.Instance.GetUItext(176);
            case 3:
                return ConfigMng.Instance.GetUItext(177);
            case 4:
                return ConfigMng.Instance.GetUItext(178);
            case 5:
                return ConfigMng.Instance.GetUItext(179);
            default:
                return string.Empty;
        }
    }
    protected bool isGray = false;
    /// <summary>
    /// 是否置灰
    /// </summary>
    public bool IsGray 
    {
        get 
        {
            return isGray;
        }
    }

    /// <summary>
    /// 是否需要请求详细信息
    /// </summary>
    protected bool needDetailed = false;
    public bool NeedDetailed
    {
        set { needDetailed = value; }
        get { return needDetailed; }
    }
}



public enum EquipmentBindType
{
    UnBind = 0,
    LootBind = 1,
    EquipBind = 2,
}

/// <summary>
/// 对于主玩家而言的装备所属（其他玩家身上的装备也属于preview）
/// </summary>
public enum EquipmentBelongTo
{
    NONE,
    EQUIP,
    BACKPACK,
    STORAGE,
    PREVIEW,
    WAREHOUSE,//藏宝阁临时仓库 ljq
    SHOPWND,
    REDEEM,
    GUILDSHOP,//公会商店
    TRADEBOX,//自己交易栏中的
}




public class EquipmentStaticLogic
{
}