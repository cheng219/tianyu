//===================================
//作者：邓成
//日期: 2016/7/5
//用途：玩家物品管理类
//=====================================



using UnityEngine;
using System.Collections;
using st.net.NetBase;
using System;
using System.Collections.Generic;


public class InventoryMng 
{
    #region 构造
    /// <summary>
    /// 构造单例
    /// </summary>
    /// <param name="_father"></param>
    /// <returns></returns>
    public static InventoryMng CreateNew(MainPlayerMng _father)
    {
        if (_father == null)
        {
            GameSys.LogError("物品管理类必须从属于一个玩家");
            return null;
        }
        if (_father.inventoryMng == null)
        {
            InventoryMng inventoryMng = new InventoryMng();
            inventoryMng.belongTo = _father;
            inventoryMng.actorInfo = _father.MainPlayerInfo;
            inventoryMng.Init(_father);
            return inventoryMng;
        }
        else
        {
            _father.inventoryMng.UnRegist(_father);
            _father.inventoryMng.belongTo = _father;
            _father.inventoryMng.actorInfo = _father.MainPlayerInfo;
            _father.inventoryMng.Init(_father);
            return _father.inventoryMng;
        }
    }
    /// <summary>
    /// 注册
    /// </summary>
    protected void Init(MainPlayerMng _father)
    {
		MsgHander.Regist(0xD302, S2C_GetDecomposeResult);
		MsgHander.Regist(0xD007, S2C_OnGotBackPackData);
		MsgHander.Regist(0xD130, S2C_OnGotBagSlotData);
		MsgHander.Regist(0xD319,S2C_OnGotStorageData);
		MsgHander.Regist(0xD322, S2C_OnGotChangePackData);
		MsgHander.Regist(0xD323, S2C_OnGotChangeStorageData);
		MsgHander.Regist(0xD351, S2C_OnGotEquipData);
		MsgHander.Regist(0xD696,S2C_OnGotUnlockBagCost);

        MsgHander.Regist(0xD108, S2C_OnGotBoxItem);
        MsgHander.Regist(0xD136, S2C_GetUsingItem);
		MsgHander.Regist(0xD313,S2C_GetSynthesisResult);
        MsgHander.Regist(0xD815, S2C_GetSealEquResult);
        MsgHander.Regist(0xC122, S2C_GetRenameCardResult);
    }
    /// <summary>
    /// 注销
    /// </summary>
    protected void UnRegist(MainPlayerMng _father)
    {
		MsgHander.UnRegist(0xD007, S2C_OnGotBackPackData);
		MsgHander.UnRegist(0xD302, S2C_GetDecomposeResult);
		MsgHander.UnRegist(0xD130, S2C_OnGotBagSlotData);
		MsgHander.UnRegist(0xD319,S2C_OnGotStorageData);
        MsgHander.UnRegist(0xD322, S2C_OnGotChangePackData);
		MsgHander.UnRegist(0xD323, S2C_OnGotChangeStorageData);
		MsgHander.UnRegist(0xD351, S2C_OnGotEquipData);
		MsgHander.UnRegist(0xD696,S2C_OnGotUnlockBagCost);

        MsgHander.UnRegist(0xD108, S2C_OnGotBoxItem);

        MsgHander.UnRegist(0xD136, S2C_GetUsingItem);
		MsgHander.UnRegist(0xD313,S2C_GetSynthesisResult);
        MsgHander.UnRegist(0xD815, S2C_GetSealEquResult);
        MsgHander.UnRegist(0xC122, S2C_GetRenameCardResult);
        EquipDictionary.Clear();
		BackPackDictionary.Clear();
		RealBackpackDictionary.Clear();
		RealStorageDictionary.Clear();
		WareHouseDictionary.Clear();
		HaveShowOpenSlot = false;
        DontTipHighQualityForDecompose = false;
    }
    #endregion

    #region 数据

    /// <summary>
    /// 所从属的玩家
    /// </summary>
    protected MainPlayerMng belongTo;
    /// <summary>
    /// 本次注册所引用的主玩家信息
    /// </summary>
    protected MainPlayerInfo actorInfo = null;
    /// <summary>
    /// 是否正在整理背包中
    /// </summary>
    protected bool isTidyDirty = false;
    /// <summary>
    /// 分解时是否提示高品质装备
    /// </summary>
    public bool DontTipHighQualityForDecompose = false;

    /// <summary>
    /// 冷却中的物品
    /// </summary>
    public Dictionary<int, float> UseCD = new Dictionary<int, float>();


	#region 背包
    public ItemShowUIType CurShowUIType = ItemShowUIType.NONE;
    /// <summary>
    /// 打开背包
    /// </summary>
    /// <param name="_curShowUIType"></param>
    public void OpenBackpack(ItemShowUIType _curShowUIType)
    {
        CurShowUIType = _curShowUIType;
        GameCenter.uIMng.GenGUI(GUIType.BACKPACKWND, true);
    }

    /// <summary>
    /// 背包物品数据集
    /// </summary>
    protected Dictionary<int, EquipmentInfo> backPackDictionary = new Dictionary<int, EquipmentInfo>();
    /// <summary>
    /// 背包物品数据集
    /// </summary>
    public Dictionary<int, EquipmentInfo> BackPackDictionary
    {
        get { return backPackDictionary; }
    }
	/// <summary>
	/// 背包物品数据集,包含空格
	/// </summary>
	protected Dictionary<int,EquipmentInfo> realBackpackDictionary = new Dictionary<int,EquipmentInfo>();
	/// <summary>
	/// 背包物品数据集,包含空格
	/// </summary>
	public Dictionary<int,EquipmentInfo> RealBackpackDictionary
	{
		get { return realBackpackDictionary; }
	}
	/// <summary>
	/// 背包物品数据发生变化的事件(物品变化、格子变化、登陆获取数据)
	/// </summary>
	public System.Action OnBackpackUpdate;
	/// <summary>
	/// 背包数据发生变化(格子,物品信息)
	/// </summary>
	public System.Action<int,EquipmentInfo> OnBackpackItemUpdate;
	/// <summary>
	/// 身上装备物品数据发生变化的事件
	/// </summary>
	public System.Action OnEquipItemUpdate;
    /// <summary>
    /// 前一次加载的强化特效
    /// </summary>
    private string OldEffect=" ";
    /// <summary>
    /// 物品的实例ID(改名卡使用需要二次弹窗而在新开的窗口使用改名需要发服务端实例ID)
    /// </summary>
    private int instanceID = 0;
    public int InstanceID
    {
        get
        {
            return instanceID;
        }
        set
        {
            instanceID = value;
        }
    }
    #endregion

    #region 仓库
    /// <summary>
    /// 仓库物品数据集
    /// </summary>
    protected Dictionary<int, EquipmentInfo> storageDictionary = new Dictionary<int, EquipmentInfo>();
	/// <summary>
	/// 仓库物品数据集
	/// </summary>
	public Dictionary<int, EquipmentInfo> WareHouseDictionary
	{
		get { return storageDictionary; }
	}
	/// <summary>
	/// 仓库物品数据集,包含空格
	/// </summary>
	protected Dictionary<int,EquipmentInfo> realStorageDictionary = new Dictionary<int,EquipmentInfo>();
	/// <summary>
	/// 仓库物品数据集,包含空格
	/// </summary>
	public Dictionary<int,EquipmentInfo> RealStorageDictionary
	{
		get { return realStorageDictionary; }
	}
	/// <summary>
	/// 仓库物品数据获得
	/// </summary>
	public System.Action OnGotStorageData;
	/// <summary>
	/// 仓库数据发生变化(位置,物品信息)
	/// </summary>
	public System.Action<int,EquipmentInfo> OnStorageItemUpdate;
	#endregion
	/// <summary>
	/// 分解成功事件
	/// </summary>
	public System.Action OnGetDecomposeResult;
	
	/// <summary>
	/// 合成成功事件
	/// </summary>
	public System.Action OnGetSynthesisResult;
    /// <summary>
    /// 封印成功事件
    /// </summary>
    public System.Action<EquipmentInfo> OnGetSealEquResult;
    /// <summary>
    /// 请求得到详细数据
    /// </summary>
    public System.Action<EquipmentInfo> OnUpdateDetailItem;
    /// <summary>
    /// 使用物品的返回事件 用于带模型的物品展示 by 易睿
    /// </summary>
    public System.Action<int> OnGetItemWithModelEvent;

    /// <summary>
    /// 背包整理的缓存数据 by吴江
    /// </summary>
    protected List<ItemUI> tidyList = null;
    /// <summary>
    /// 可以使用的物品(用于批量使用打开弹窗的信息预览)
    /// </summary>
    protected EquipmentInfo curSelectInventory = null;
    public EquipmentInfo CurSelectInventory
    {
        set
        {
            curSelectInventory = value;
        }
        get
        {
            return curSelectInventory;
        }
    }
    protected EquipmentInfo curSelectEquip = null;
    public EquipmentInfo CurSelectEquip
    {
        set 
        { 
            curSelectEquip = value;
            if (OnSelectEquip != null)
            {
                OnSelectEquip(curSelectEquip);
            }
        }
        get 
        {
            if (curSelectEquip!=null && backPackDictionary.ContainsKey(curSelectEquip.InstanceID))
                curSelectEquip = backPackDictionary[curSelectEquip.InstanceID];
            return curSelectEquip; 
        }
    }
    /// <summary>
    /// 根据ID获取背包数据
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public EquipmentInfo GetEquipInfoByID(int ID)
    {
        if (backPackDictionary.ContainsKey(ID))
            return backPackDictionary[ID];
        else
            return null;
    }
	/// <summary>
	/// 获取背包中的装备(用于预览)
	/// </summary>
	public Dictionary<int, EquipmentInfo> GetBackpackEquipDic()
	{
		Dictionary<int, EquipmentInfo> dic = new Dictionary<int, EquipmentInfo>();
		foreach(EquipmentInfo info in backPackDictionary.Values)
		{
			if(info.IsEquip && info.CurEmptyType == EquipmentInfo.EmptyType.NONE && info.StackCurCount != 0)
				dic[info.InstanceID] = info;
		}
		return dic;
	}

	/// <summary>
	/// 获取背包中的装备(用于预览)
	/// </summary>
	public Dictionary<int, EquipmentInfo> GetBackpackEquipAndMountEquipDic()
	{
		Dictionary<int, EquipmentInfo> dic = new Dictionary<int, EquipmentInfo>();
		foreach(EquipmentInfo info in backPackDictionary.Values)
		{
			if((info.IsEquip || info.Family == EquipmentFamily.MOUNTEQUIP) && info.CurEmptyType == EquipmentInfo.EmptyType.NONE && info.StackCurCount != 0)
				dic[info.InstanceID] = info;
		}
		return dic;
	}

	/// <summary>
	/// 获取身上装备
	/// </summary>
	public EquipmentInfo GetEquipFromEquipDicByID(int _id)
	{
		foreach(EquipmentInfo info in EquipDictionary.Values)
		{
			if(info.InstanceID == _id)
				return info;
		}
		return null;
	}
	/// <summary>
	/// 穿装备的任务引导
	/// </summary>
	/// <param name="_slot">Slot.</param>
	public void GuideEquipmentTask(EquipSlot _slot)
	{
		Dictionary<int,EquipmentInfo> equipDic = GetBackpackEquipDic();
		using(var item = equipDic.GetEnumerator())
		{
			while(item.MoveNext())
			{
				if(item.Current.Value.Slot == _slot)
				{
					onAddEquipForTip(item.Current.Value);
					break;
				}
			}
		}
	}
	/// <summary>
	/// 获取身上装备
	/// </summary>
	public EquipmentInfo GetEquipFromEquipDicBySlot(EquipSlot slot)
	{
		Dictionary<EquipSlot,EquipmentInfo> equipDic = GetPlayerEquipDic();
		if(equipDic.ContainsKey(slot))
			return equipDic[slot];
		return null;
	}

	/// <summary>
	/// 获取背包中的宝石
	/// </summary>
	public List<EquipmentInfo> GetGemList()
	{
		List<EquipmentInfo> gemList = new List<EquipmentInfo>();
		foreach(EquipmentInfo info in BackPackDictionary.Values)
		{
			if(info.Family == EquipmentFamily.GEM && info.StackCurCount != 0)//排除已经删除掉的物品(数量为0)
				gemList.Add(info);
		}
		return gemList;
	}
	/// <summary>
	/// 身上所有装备
	/// </summary>
	public Dictionary<int, EquipmentInfo> GetPlayerEquipList()
	{
		Dictionary<int, EquipmentInfo> dic = new Dictionary<int, EquipmentInfo>();
		foreach(EquipmentInfo info in equipDictionary.Values)
		{
			if(info.Slot != EquipSlot.None && info.Family != EquipmentFamily.GEM && info.CurEmptyType == EquipmentInfo.EmptyType.NONE && info.StackCurCount != 0)
				dic[info.InstanceID] = info;
		}
		return dic;
	}
    /// <summary>
    /// 身上所有装备
    /// </summary>
    public List<EquipmentInfo> GetPlayerEquList()
    {
        List<EquipmentInfo> list = new List<EquipmentInfo>();
        foreach (EquipmentInfo info in equipDictionary.Values)
        {
            if (info.Slot != EquipSlot.None && info.Family != EquipmentFamily.GEM && info.CurEmptyType == EquipmentInfo.EmptyType.NONE && info.StackCurCount != 0)
                list.Add(info);
        }
        return list;
    }
	/// <summary>
	/// 身上所有装备
	/// </summary>
	public Dictionary<EquipSlot,EquipmentInfo> GetPlayerEquipDic()
	{
		Dictionary<EquipSlot, EquipmentInfo> dic = new Dictionary<EquipSlot, EquipmentInfo>();
		foreach(EquipmentInfo info in equipDictionary.Values)
		{
			if(info.Slot != EquipSlot.None && info.Family != EquipmentFamily.GEM && info.CurEmptyType == EquipmentInfo.EmptyType.NONE && info.StackCurCount != 0)
				dic[info.Slot] = info;
		}
		return dic;
	}
	/// <summary>
	/// 获取镶嵌的相应等级宝石的数量
	/// </summary>
	public int GetPlayerInlayGemNumByLv(int gemLv)
	{
		int gemNum = 0;
		foreach(EquipmentInfo info in equipDictionary.Values)//没有直接用GetPlayerEquipDic(),直接用还是要多遍历一次
		{
			if(info.Slot != EquipSlot.None && info.Family != EquipmentFamily.GEM && info.CurEmptyType == EquipmentInfo.EmptyType.NONE 
				&& info.StackCurCount != 0)
			{
				foreach(pos_des gem in info.InlayGemDic.Values)//没有先用info.HasInlayGem,直接用可能多遍历一次
				{
					if(gem.type != 0)
					{
						EquipmentRef equip = ConfigMng.Instance.GetEquipmentRef(gem.type);
                        if (equip != null && equip.gemLevel >= gemLv)
							gemNum++;
					}
				}
			}
		}
		return gemNum;
	}
	/// <summary>
	/// 获取玩家身上响应品质的洗练属性条数(目前只需求橙色)
	/// </summary>
	public int GetWashAttrNumByQuality()
	{
		int washNum = 0;
		foreach(EquipmentInfo info in equipDictionary.Values)//没有直接用GetPlayerEquipDic(),直接用还是要多遍历一次
		{
			if(info.Slot != EquipSlot.None && info.Family != EquipmentFamily.GEM && info.CurEmptyType == EquipmentInfo.EmptyType.NONE 
				&& info.StackCurCount != 0)
			{
				EquipmentWashValueRef washValue = null;
				if(info.EquOne != 0)
				{
					washValue = ConfigMng.Instance.GetEquipmentWashValueRefByID(info.EquOne);
					if(washValue != null && washValue.att_quality >= 5)
						washNum++;
				}
				if(info.EquTwo != 0)
				{
					washValue = ConfigMng.Instance.GetEquipmentWashValueRefByID(info.EquTwo);
					if(washValue != null && washValue.att_quality >= 5)
						washNum++;
				}
				if(info.EquThree != 0)
				{
					washValue = ConfigMng.Instance.GetEquipmentWashValueRefByID(info.EquThree);
					if(washValue != null && washValue.att_quality >= 5)
						washNum++;
				}
				if(info.EquFour != 0)
				{
					washValue = ConfigMng.Instance.GetEquipmentWashValueRefByID(info.EquFour);
					if(washValue != null && washValue.att_quality >= 5)
						washNum++;
				}	
			}
		}
		return washNum;
	}

    /// <summary>
    /// 宝箱获得物品列表
    /// </summary>
    protected List<EquipmentInfo> boxGotItems = new List<EquipmentInfo>();
    public List<EquipmentInfo> BoxGotItems
    {
        get { return boxGotItems; }
    }

    /// <summary>
    /// 随机宝箱获得物品列表
    /// </summary>
    public List<EquipmentInfo> randomChestList = new List<EquipmentInfo>();

    /// <summary>
    /// 身上已经装备的物品的数据集 
    /// </summary>
    protected Dictionary<EquipSlot, EquipmentInfo> equipDictionary = new Dictionary<EquipSlot, EquipmentInfo>();
    /// <summary>
    /// 身上已经装备的物品的数据集 
    /// </summary>
    public Dictionary<EquipSlot, EquipmentInfo> EquipDictionary
    {
        get { return equipDictionary; }
    }
    /// <summary>
    /// 获取宝箱物品
    /// </summary>
    public System.Action GotBoxItems;
    /// <summary>
    /// 判断背包是否已满
    /// </summary>
    public bool IsBagFull
    {
        get 
        {
			return (BagOpenCount <= BackPackDictionary.Count); 
        }
    }
    /// <summary>
    /// 背包等级
    /// </summary>
    protected int baglv = 0;
    /// <summary>
    /// 背包等级访问器
    /// </summary>
    public int Baglv
    {
        get { return baglv; }
    }
	/// <summary>
	/// 背包开启格子数
	/// </summary>
	protected int bagOpenCount = 0;
	/// <summary>
	/// 背包开启格子数访问器
	/// </summary>
	public int BagOpenCount
	{
		get { return bagOpenCount; }
	}
	/// <summary>
	/// 背包可开启格子位置
	/// </summary>
	protected int firstNeedOpenPos = 0;
	protected int bagCdTime = 0;
	public int BagCdTime
	{
		get{return bagCdTime;}
		set
		{
			bagCdTime = value;
			GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.MAINBACK,CanOpenSlot);
			if(CanOpenSlot && bagCdTime != value)
			{
				HaveShowOpenSlot = false;
			}
		}
	}
	public int BagCdTotalTime = 3600;
	/// <summary>
	/// 背包格子可开启
	/// </summary>
	public bool CanOpenSlot
	{
		get
		{
			return (BagCdTime == 0) && (BagOpenCount < SystemSettingMng.MAX_BAG_NUM);
		}
	}

	public bool HaveShowOpenSlot = false;

	/// <summary>
	/// 仓库开启格子数
	/// </summary>
	protected int storageOpenCount = 0;
	/// <summary>
	/// 仓库开启格子数访问器
	/// </summary>
	public int WareHouseOpenCount
	{
		get { return storageOpenCount; }
	}
    /// <summary>
    /// 装备物品发生变化的事件
    /// </summary>
    public System.Action OnEquipUpdate;
    /// <summary>
    /// 来源的物品为副本获取或者礼包、任务、邮件,此类物品需要提示 
    /// </summary>
	static public System.Action<EquipmentInfo> onAddEquipForTip;
    /// <summary>
    /// 用来保存一份需要提示的物品信息，用于切换场景后还需要继续提示
    /// </summary>
    public List<EquipmentInfo> equipTip = new List<EquipmentInfo>();
    /// <summary>
    /// 装备在提示时，物品被销毁事件 
    /// </summary>
    static public System.Action<int> onDelEquipForTip;
    /// <summary>
    /// 使用物品时,打开显示使用物品需求的界面 
    /// </summary>
    public System.Action<EquipmentInfo> OnOpenUseConditionWnd;
    /// <summary>
    /// 选择装备中的物品
    /// </summary>
    public System.Action<EquipmentInfo> OnSelectEquip;
    /// <summary>
    /// 快捷购买总价格
    /// </summary>
    protected int TotalPrice = 0;
    /// <summary>
    /// 当前穿戴的武器数据
    /// </summary>
    public EquipmentInfo curEquWeapon;
    #endregion

    #region 协议数据
    #region S2C
	/// <summary>
	/// 服务端身上装备数据返回 by邓成
	/// </summary>
	/// <param name="_info"></param>
	protected void S2C_OnGotEquipData(Pt _info)
	{
		pt_equ_info_d351 pt_equ_info_d351 = _info as pt_equ_info_d351;
		if(pt_equ_info_d351 != null)
		{
			int count = pt_equ_info_d351.equ_list.Count;
			for (int i = 0; i < count; i++) 
			{
				EquipmentInfo info = new EquipmentInfo(pt_equ_info_d351.equ_list[i]);
				if(!equipDictionary.ContainsKey(info.Slot))
					equipDictionary[info.Slot] = info;
				else
					equipDictionary[info.Slot].Update(pt_equ_info_d351.equ_list[i],EquipmentBelongTo.EQUIP);
			}
			if(OnEquipItemUpdate != null)
				OnEquipItemUpdate();
			if(OnEquipUpdate != null)
				OnEquipUpdate();
		}
	}
    /// <summary>
	/// 服务端背包数据返回(强烈建议后台只在请求背包数据后返回,更新背包数据用另一条协议) by邓成
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_OnGotBackPackData(Pt _info)
    {
        pt_item_info_d007 pt_item_info_d007Info = _info as pt_item_info_d007;
        if (pt_item_info_d007Info != null)
        {
			bagOpenCount = (int)pt_item_info_d007Info.can_use_bags;
			firstNeedOpenPos = (int)pt_item_info_d007Info.unlock_bags_num;
			BagCdTime = (int)pt_item_info_d007Info.rest_time;
            int count = pt_item_info_d007Info.item_list.Count;
            backPackDictionary.Clear();
			realBackpackDictionary.Clear();
			for (int i = 0; i < SystemSettingMng.MAX_BAG_NUM; i++)
            {
				if(i<count)//背包中非空格的数据
				{
					EquipmentInfo info = new EquipmentInfo(pt_item_info_d007Info.item_list[i]);
					if (!backPackDictionary.ContainsKey((int)info.InstanceID))
					{
						backPackDictionary[(int)info.InstanceID] = info;
						realBackpackDictionary[info.Postion] = info;
					}else
					{
						if(info.StackCurCount == 0)
						{
							if(onDelEquipForTip != null)
								onDelEquipForTip(info.InstanceID);
							backPackDictionary.Remove(info.InstanceID);
							realBackpackDictionary[info.Postion] = new EquipmentInfo(info.Postion,EquipmentInfo.EmptyType.EMPTY,EquipmentBelongTo.BACKPACK);
						}else
						{
							backPackDictionary[info.InstanceID].Update(pt_item_info_d007Info.item_list[i],EquipmentBelongTo.BACKPACK);
							realBackpackDictionary[info.Postion] = backPackDictionary[info.InstanceID];
						}
					}
				}
				if(i >= bagOpenCount)//未解锁的格子
				{
					if(i == bagOpenCount)
					{
						if(bagCdTime == 0)
							realBackpackDictionary[i+1] = new EquipmentInfo(i+1,EquipmentInfo.EmptyType.CANUNLOCK,EquipmentBelongTo.BACKPACK);//可解锁的空格
						else
							realBackpackDictionary[i+1] = new EquipmentInfo(i+1,EquipmentInfo.EmptyType.CDLOCK,EquipmentBelongTo.BACKPACK);//CD中的空格
						continue;
					}
					realBackpackDictionary[i+1] = new EquipmentInfo(i+1,EquipmentInfo.EmptyType.LOCK,EquipmentBelongTo.BACKPACK);//锁定空格物品
				}
            }
			//空格子
			int len = pt_item_info_d007Info.empty_bags.Count;
			for(int i = 0; i < len; i++)
			{
				realBackpackDictionary[(int)pt_item_info_d007Info.empty_bags[i]] = new EquipmentInfo((int)(pt_item_info_d007Info.empty_bags[i]),EquipmentInfo.EmptyType.EMPTY,EquipmentBelongTo.BACKPACK);//空格物品
			}
        }
        if (OnBackpackUpdate != null)
        {
            OnBackpackUpdate();
        }
    }

	/// <summary>
	/// 服务端仓库数据返回 by邓成
	/// </summary>
	/// <param name="_info"></param>
	protected void S2C_OnGotStorageData(Pt _info)
	{
		pt_store_house_info_d319 pt_store_house_info_d319 = _info as pt_store_house_info_d319;

		if (pt_store_house_info_d319 != null)
		{
			storageOpenCount = (int)pt_store_house_info_d319.can_use_bags;
			//Debug.Log("storageOpenCount:"+storageOpenCount);
			int count = pt_store_house_info_d319.item_list.Count;
			storageDictionary.Clear();
			realStorageDictionary.Clear();
			for (int i = 0; i < SystemSettingMng.MAX_STORAGE_NUM; i++)
			{
				if(i<count)//仓库中非空格的数据
				{
					EquipmentInfo info = new EquipmentInfo(pt_store_house_info_d319.item_list[i]);
					if (!storageDictionary.ContainsKey((int)info.InstanceID))
					{
						storageDictionary[(int)info.InstanceID] = info;
					}
					realStorageDictionary[info.Postion] = info;
				}
				if(i >= storageOpenCount)//未解锁的格子
				{
					int pos = i+1001;
					realStorageDictionary[pos] = new EquipmentInfo(pos,EquipmentInfo.EmptyType.LOCK,EquipmentBelongTo.STORAGE);//锁定空格物品
				}
			}
			//空格子
			int len = pt_store_house_info_d319.empty_bags.Count;
			for(int i = 0; i < len; i++)
			{
				realStorageDictionary[(int)pt_store_house_info_d319.empty_bags[i]] = new EquipmentInfo((int)(pt_store_house_info_d319.empty_bags[i]),EquipmentInfo.EmptyType.EMPTY,EquipmentBelongTo.STORAGE);//空格物品
			}
		}
		if(OnGotStorageData != null)
			OnGotStorageData();
	}
    /// <summary>
	/// 服务端背包变化返回 (背包数据变化)by邓成
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_OnGotChangePackData(Pt _info)
    {
		pt_item_chg_d322 pt_item_chg_d322 = _info as pt_item_chg_d322;
		if (pt_item_chg_d322 != null)
        {
			int count = pt_item_chg_d322.item_list.Count;
            if (count >= 0)
            {
                for (int i = 0; i < count; i++)
                {
					EquipmentInfo info = new EquipmentInfo( pt_item_chg_d322.item_list[i]);
                    //Debug.Log("pos:" + info.Postion + ",instance:" + info.InstanceID + ",name:" + info.ItemName + ",num:" + info.StackCurCount);
					if (!backPackDictionary.ContainsKey((int)info.InstanceID))
					{
						backPackDictionary[(int)info.InstanceID] = info;
						realBackpackDictionary[info.Postion] = info;
						if(onAddEquipForTip != null)//获得物品提示
							onAddEquipForTip(info);
					}else
					{
						if(info.StackCurCount == 0)
						{
							if(onDelEquipForTip != null)
								onDelEquipForTip(info.InstanceID);
							backPackDictionary.Remove(info.InstanceID);
							realBackpackDictionary[info.Postion] = new EquipmentInfo(info.Postion,EquipmentInfo.EmptyType.EMPTY,EquipmentBelongTo.BACKPACK);
						}else
						{
							backPackDictionary[info.InstanceID].Update(pt_item_chg_d322.item_list[i],EquipmentBelongTo.BACKPACK);
							realBackpackDictionary[info.Postion] = backPackDictionary[info.InstanceID];
                            if (onAddEquipForTip != null)//获得物品提示
                                onAddEquipForTip(backPackDictionary[info.InstanceID]);
						}
					}
                }
            }
        }
		if (OnBackpackUpdate != null)
		{
			OnBackpackUpdate();
		}
    }
	/// <summary>
	/// 背包格子变化,仅用于解锁后返回
	/// </summary>
	/// <param name="_info">Info.</param>
	protected void S2C_OnGotBagSlotData(Pt _info)
    {
		pt_bags_chg_d130 info = _info as pt_bags_chg_d130;
        if (info!=null)
        {
			for(int i=firstNeedOpenPos;i<(int)info.unlock_bags_num;i++)
			{
				EquipmentInfo eq = new EquipmentInfo(i,EquipmentInfo.EmptyType.EMPTY,EquipmentBelongTo.BACKPACK);
				realBackpackDictionary[i] = eq;
				if(OnBackpackItemUpdate != null)
					OnBackpackItemUpdate(i,eq);//将锁定的格子变成已开启的空格
			}
			bagOpenCount = (int)info.can_use_bags;
			firstNeedOpenPos = (int)info.unlock_bags_num;
			BagCdTime = (int)info.rest_time;
			BagCdTotalTime = (int)info.static_rest_time;
			EquipmentInfo cdEq = new EquipmentInfo(firstNeedOpenPos,EquipmentInfo.EmptyType.CDLOCK,EquipmentBelongTo.BACKPACK);
			realBackpackDictionary[firstNeedOpenPos] = cdEq;
			if(OnBackpackItemUpdate != null)
				OnBackpackItemUpdate(firstNeedOpenPos,cdEq);
        }
        if (OnBackpackUpdate != null)
        {
            OnBackpackUpdate();
        }
    }

	/// <summary>
	/// 服务端仓库变化返回 by邓成
	/// </summary>
	/// <param name="_info"></param>
	protected void S2C_OnGotChangeStorageData(Pt _info)
	{
		pt_store_house_item_chg_d323 pt_store_house_item_chg_d323 = _info as pt_store_house_item_chg_d323;
		if (pt_store_house_item_chg_d323 != null)
		{
			int count = pt_store_house_item_chg_d323.item_list.Count;
			//Debug.Log("S2C_OnGotChangeStorageData count :"+count);
			if (count >= 0)
			{
				for (int i = 0; i < count; i++)
				{
					EquipmentInfo info = new EquipmentInfo( pt_store_house_item_chg_d323.item_list[i]);
					if(info.StackCurCount == 0)
						info = new EquipmentInfo(info.Postion,EquipmentInfo.EmptyType.EMPTY,EquipmentBelongTo.STORAGE);
					storageDictionary[info.InstanceID] = info;
					realStorageDictionary[info.Postion] = info;
				}
			}
            if (OnGotStorageData != null)
                OnGotStorageData();
		}
	}
	/// <summary>
	/// 获取到解锁背包格子的消耗
	/// </summary>
	protected void S2C_OnGotUnlockBagCost(Pt _info)
	{
		pt_update_bags_info_d696 pt = _info as pt_update_bags_info_d696;
		if(pt != null)
		{
			
			int time = pt.time;
			if(time <= 0)
			{
				C2S_UnlockBagSlot(CurUnlockBagSlotPos);
			}else
			{
				int diamo = pt.sycee;
				int exp = pt.exp;
				//Debug.Log("S2C_OnGotUnlockBagCost:"+time+",diamo:"+diamo+",exp:"+exp);
				string timeStr = string.Format("{0:D2}:{1:D2}:{2:D2}",time/3600,(time/60)%60,time%60);

				MessageST mst = new MessageST();
				mst.messID = 281;
				mst.words = new string[]{timeStr,diamo.ToString(),exp.ToString()};
				mst.delYes = (x)=>
				{
					C2S_UnlockBagSlot(CurUnlockBagSlotPos);
				};
				GameCenter.messageMng.AddClientMsg(mst);
			}
		}
	}

    protected void S2C_OnGotBoxItem(Pt _info)
    {
        pt_open_box_get_item_d108 _pt = _info as pt_open_box_get_item_d108;
        boxGotItems.Clear();
        foreach (var item in _pt.item_list)
        {
            EquipmentInfo info = new EquipmentInfo((int)item.item_id, (int)item.item_num, EquipmentBelongTo.PREVIEW);
            boxGotItems.Add(info);
        }
        RoyalTreasureWnd wnd = GameCenter.uIMng.GetGui<RoyalTreasureWnd>();
        if (wnd != null && boxGotItems.Count > 0)
        {
            GameCenter.royalTreasureMng.OnGetRoyalReward(boxGotItems);
        }
        else if (boxGotItems.Count>0)
        {
		//	GameCenter.messageMng.AddClientMsg(boxGotItems,67);
			GameCenter.uIMng.GenGUI(GUIType.BOXREWARD,true);
        }
    }


  

    /// <summary>
    /// 详细数据获得
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_OnGotEquipDetail(Pt _info)
    {
       
        pt_item_info_return_d129 _pt = _info as pt_item_info_return_d129;
        if (_pt != null)
        {
            for (int i = 0; i < _pt.item_list.Count; i++)
            {
                EquipmentInfo info = new EquipmentInfo(_pt.item_list[i],true);

                if (OnUpdateDetailItem != null)
                    OnUpdateDetailItem(info);
            }
        }
    }
    /// <summary>
    /// 获取使用的物品信息（预览模型用） by 易睿
    /// </summary>
    protected void S2C_GetUsingItem(Pt _pt)
    {
        pt_item_model_d136 info = _pt as pt_item_model_d136;
        if (info != null)
        {
            //GameCenter.uIMng.SwitchToUI(GUIType.NEWTHINGSGET);
            if (OnGetItemWithModelEvent != null)
                OnGetItemWithModelEvent(info.item_id);
           
        }
    }
	/// <summary>
	/// 分解结果返回
	/// </summary>
	protected void S2C_GetDecomposeResult(Pt _pt)
	{
		//Debug.Log("S2C_GetDecomposeResult:"+Time.realtimeSinceStartup);
//		pt_decompose_result_d302 info = _pt as pt_decompose_result_d302;
		if(OnGetDecomposeResult != null)
			OnGetDecomposeResult();
	}
	
	/// <summary>
	/// 合成结果返回  add 何明军
	/// </summary>
	protected void S2C_GetSynthesisResult(Pt _pt)
	{
		//Debug.Log("OnGetSynthesisResult");
//		pt_compose_result_d313 info = _pt as pt_compose_result_d313;
		if(OnGetSynthesisResult != null)
			OnGetSynthesisResult();
	}
    /// <summary>
    /// 封印结果返回
    /// </summary>
    /// <param name="_pt"></param>
    protected void S2C_GetSealEquResult(Pt _pt)
    {
        pt_bind_result_d815 info = _pt as pt_bind_result_d815;
        if (info != null)
        {
            EquipmentInfo data = null;
            if (GetPlayerEquipList().ContainsKey((int)info.id))
                data = GetPlayerEquipList()[(int)info.id];
            else if (backPackDictionary.ContainsKey((int)info.id))
                data = backPackDictionary[(int)info.id];
            if (OnGetSealEquResult != null && data != null)
                OnGetSealEquResult(data);
        }
    }
    /// <summary>
    /// 改名卡结果返回
    /// </summary>
    /// <param name="_pt"></param>
    protected void S2C_GetRenameCardResult(Pt _pt)
    {
        pt_update_new_name_c122 info = _pt as pt_update_new_name_c122;
        if(info!=null)
        {
            GameCenter.messageMng.AddClientMsg(532);
            GameCenter.mainPlayerMng.MainPlayerInfo.UpdateName(info.new_name);
            GameCenter.uIMng.ReleaseGUI(GUIType.RENAMECARD);
        }
    }
    #endregion
    #region C2S
    #region 仙侠新添
    /// <summary>
    /// 合成 add 何明军
    /// </summary>
    public void C2S_SynthesisItem(int id)
	{
		pt_req_compose_d311 msg = new pt_req_compose_d311();
		msg.id = id;
		NetMsgMng.SendMsg(msg);

	}

	/// <summary>
	/// 全部合成 add 何明军
	/// </summary>
	public void C2S_SynthesisItemAll(int id,int num)
	{
		pt_req_compose_all_d312 msg = new pt_req_compose_all_d312();
		msg.id = id;
		msg.times = (uint)num;
		NetMsgMng.SendMsg(msg);
	}
	
    /// <summary>
    /// 向服务端请求背包物品数据  by 邓成
    /// </summary>
    public void C2S_AskForBackPackData()
    {
		pt_req_item_info_d317 msg = new pt_req_item_info_d317();
        NetMsgMng.SendMsg(msg);
    }
	/// <summary>
	/// 向服务端请求仓库物品数据  by 邓成
	/// </summary>
	public void C2S_AskForStorageData()
	{
		pt_req_store_house_info_d320 msg = new pt_req_store_house_info_d320();
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 向服务端请求身上装备数据  by 邓成
	/// </summary>
	public void C2S_AskForEquipData()
	{
		pt_req_equips_d352 msg = new pt_req_equips_d352();
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 移动物品(放入仓库)  by 邓成
	/// </summary>
	public void C2S_AskPutInStorage(uint _instanceId)
	{
		pt_req_check_out_item_pos_d321 msg = new pt_req_check_out_item_pos_d321();
		msg.action = 1;
		msg.id = _instanceId;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 移动物品(仓库取出)  by 邓成
	/// </summary>
	public void C2S_AskTakeOutStorage(uint _instanceId)
	{
		pt_req_check_out_item_pos_d321 msg = new pt_req_check_out_item_pos_d321();
		msg.action = 2;
		msg.id = _instanceId;
		NetMsgMng.SendMsg(msg);
	}
    /// <summary>
    /// 整理背包 by 邓成
    /// </summary>
    public void C2S_ArrangeBag()
    {
		pt_req_clear_bags_d300 msg = new pt_req_clear_bags_d300();
        NetMsgMng.SendMsg(msg);
    }
	/// <summary>
	/// 整理仓库 by 邓成
	/// </summary>
	public void C2S_ArrangeStorage()
	{
		pt_req_clear_store_house_d326 msg = new pt_req_clear_store_house_d326();
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 解锁背包格子,pos格子位置
	/// </summary>
	public void C2S_UnlockBagSlot(int pos)
	{
	//	Debug.Log("C2S_UnlockBagSlot:"+pos);
		pt_add_bags_d131 msg = new pt_add_bags_d131();
		msg.add_bags = (uint)pos;
		NetMsgMng.SendMsg(msg);
	}
	protected int CurUnlockBagSlotPos = 0;
	/// <summary>
	/// 请求解锁格子的消耗
	/// </summary>
	public void C2S_ReqUnlockBagCost(int pos)
	{
	//	Debug.Log("C2S_ReqUnlockBagCost:"+pos);
		CurUnlockBagSlotPos = pos;
		pt_req_update_bags_info_d697 msg = new pt_req_update_bags_info_d697();
		msg.num = pos;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 分解装备
	/// </summary>
	/// <param name="equipList">Equip list.</param>
	public void C2S_DecompositionEquip(List<uint> equipList)
	{
		pt_req_item_decompose_d301 msg = new pt_req_item_decompose_d301();
		msg.item_id_list = equipList;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 使用多个物品
	/// </summary>
	/// <param name="_info"></param>
	/// <param name="_num"></param>
	public void C2S_UseItems(EquipmentInfo _info,int _num)
	{
	//	Debug.Log("C2S_UseItems:"+_info.InstanceID + ",num:"+_num);
		pt_req_use_item_d316 msg = new pt_req_use_item_d316();
		msg.id = _info.InstanceID;
		msg.num = _num;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 使用单个物品
	/// </summary>
	/// <param name="_info"></param>
	public void C2S_UseItem(EquipmentInfo _info)
	{
		if (!VerifyCD(_info))
			return;
		pt_req_use_item_d316 msg = new pt_req_use_item_d316();
		msg.id = _info.InstanceID;
		msg.num = 1;
		NetMsgMng.SendMsg(msg);
		UpDateUseCD(_info);
	}
	/// <summary>
	/// 穿装备
	/// </summary>
	/// <param name="_info"></param>
	public void C2S_ReplaceItem(EquipmentInfo _info)
	{
		//Debug.Log("C2S_ReplaceItem:"+_info.InstanceID);
		pt_req_equ_item_d315 msg = new pt_req_equ_item_d315();
		msg.id = _info.InstanceID;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 销毁物品
	/// </summary>
	public void C2S_DiscardItem(EquipmentInfo _info)
	{
		//Debug.Log("C2S_DiscardItem:"+_info.ItemName);
		pt_req_destroy_item_d314 msg = new pt_req_destroy_item_d314();
		msg.id = _info.InstanceID;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 卸下
	/// </summary>
	/// <param name="_id"></param>
	public void C2S_TakeOff(int _id)
	{
		//Debug.Log("C2S_TakeOff:"+_id);
		pt_req_dump_equip_d350 msg = new pt_req_dump_equip_d350();
		msg.id = _id;
		NetMsgMng.SendMsg(msg);
	}
    /// <summary>
    /// 使用改名卡
    /// </summary>
    /// <param name="_info"></param>
    public void C2S_UseRenameCard(int _id,string _name)
    {
        pt_change_name_c121 msg = new pt_change_name_c121();
        msg.id = _id;
        msg.new_name = _name;
        NetMsgMng.SendMsg(msg);
        //Debug.Log("改名");
    }
    #endregion
    /// <summary>
    /// 请求详细物品数据
    /// </summary>
    /// <param name="_id"></param>
    public void C2S_AskDetail(int _id)
    {
        
        pt_action_int_d003 msg = new pt_action_int_d003();
        msg.action = 1082;
        msg.data = _id;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 祝福油的快捷购买
    /// </summary>
    public void C2S_AskBlessWeapon(int _type)
    {
        pt_quick_buy_d787 msg = new pt_quick_buy_d787();
        msg.buy_item = 1;
        msg.quick_buy_type = _type;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 封印或解除封印
    /// </summary>
    public void C2S_AskSealEqu(int _id,int _type)
    {
        pt_req_bind_d814 msg = new pt_req_bind_d814();
        msg.id = _id;
        msg.type = _type;
        NetMsgMng.SendMsg(msg);
    }
    #endregion
    #endregion

    #region 辅助代码
    /// <summary>
    /// 获取物品信息根据type  by黄洪兴
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public EquipmentInfo GetEquipByType(int _type)
    {
        EquipmentInfo eq=null;
        int num = 0;
        foreach (var item in backPackDictionary)
        {
            if (item.Value.EID == _type)
            {
                if(num==0)
                {
                num = item.Value.StackCurCount;
                eq = item.Value;
                }else
				{
                    if (num > item.Value.StackCurCount)
                    {
                        num = item.Value.StackCurCount;
                        eq = item.Value;
                    }
                }
            }
        }
        return eq;
    }


    bool VerifyCD(EquipmentInfo _info)
    {

        if (_info.isOnCD)
        {
            GameCenter.messageMng.AddClientMsg(259);
            return false;
        }
        return true;

    }


     void UpDateUseCD(EquipmentInfo _eq)
    {
        if (_eq.CDInfo != null)
        {
            if (_eq.CDInfo.id != 0)
            {
                UseCD[_eq.CDInfo.id]=Time.time;
            }

        }
    }



    /// <summary>
    /// 获取物品数量
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public int GetNumberByType(int _type)
    {
        int num = 0;
        foreach (var item in backPackDictionary)
        {
            if (item.Value.EID == _type)
            {
                num += item.Value.StackCurCount;
            }
        }
        return num;
    }
    /// <summary>
    /// 对比数据增加提示
    /// </summary>
    /// <param name="_list"></param>
    private void AddGotEquipTip(EquipmentInfo info)
    {
        string[] mess = new string[2];
        mess[0] = string.Empty;
        mess[1] = string.Empty;
        if (!backPackDictionary.ContainsKey(info.InstanceID))
        {
            mess[0] = info.ItemStrColor + info.ItemName;
            mess[1] = info.StackCurCount.ToString();
        }
        else
        {
            if (backPackDictionary[info.InstanceID].StackCurCount < info.StackCurCount)
            {
                mess[0] = info.ItemStrColor + info.ItemName;
                mess[1] = (info.StackCurCount - backPackDictionary[info.InstanceID].StackCurCount).ToString();
            }
        }

        if (mess[0] != string.Empty)
        {
            EquipmentInfo eqInfo = new EquipmentInfo(info.EID, int.Parse(mess[1]), EquipmentBelongTo.PREVIEW);
            GameCenter.messageMng.AddClientMsg(eqInfo,67);
        }
    }
    /// <summary>
    /// 快捷购买消耗物品
    /// </summary>
    public int QuickBuyConsume(List<EquipmentInfo> _consumeList)
    {
		TotalPrice = 0;
        for (int i = 0; i < _consumeList.Count; i++)
        {
			TotalPrice = TotalPrice + (int)Mathf.Ceil(( _consumeList[i].DiamondPrice * (float)_consumeList[i].StackCurCount));
        }
        return TotalPrice;
    }
    /// <summary>
    /// 快捷购买消耗物品
    /// </summary>
    public int QuickBuyConsume(Dictionary<int, EquipmentInfo> _consumeDic)
    {
        foreach (EquipmentInfo info in _consumeDic.Values)
        {
            TotalPrice = TotalPrice + (int)info.DiamondPrice * info.StackCurCount;
        }
        return TotalPrice;
    }

	public Action<EquipmentInfo> OnOpenBatchUseWndEvent;
	public void OpenBatchUseWnd(EquipmentInfo _eq)
	{
        //if(OnOpenBatchUseWndEvent != null)
        //	OnOpenBatchUseWndEvent(_eq);
    }
    /// <summary>
    /// 播放强化特效
    /// </summary>
    public void PlayEquStrengEffectName(List<EquipmentInfo> _list,FXCtrl _fxCtrl)
    {
        if (_list.Count < 12)
            _fxCtrl.ClearStrengthEffect();
        else
        {
            _list.Sort(SortEquUpdarLev);
            List<StrengthenSuitRef> suitList= ConfigMng.Instance.GetStrengSuitRefList(2);
            for (int i = 0; i < suitList.Count; i++)
            {
                if (i + 1 < suitList.Count && _list[0].UpgradeLv >= suitList[i].str_Lev && _list[0].UpgradeLv < suitList[i + 1].str_Lev)
                {
                    //Debug.Log("_list[0].ItemName:" + _list[0].ItemName);
                    if (!OldEffect.Equals(suitList[i].effects))
                    {
                        _fxCtrl.DoStrengthEffect(suitList[i].effects);
                        OldEffect = suitList[i].effects;
                    }
                    return;
                }
                else if (i + 1 == suitList.Count && _list[0].UpgradeLv >= suitList[i].str_Lev)
                {
                    if (!OldEffect.Equals(suitList[i].effects))
                    {
                        _fxCtrl.DoStrengthEffect(suitList[i].effects);
                        OldEffect = suitList[i].effects;
                    }
                    //Debug.Log("_list[0].ItemName:" + _list[0].ItemName);
                    return;
                }
            }
        }
    }
    /// <summary>
    /// 播放强化特效
    /// </summary>
    public void PlayEquStrengEffectName(int _lev, FXCtrl _fxCtrl)
    {
        List<StrengthenSuitRef> suitList = ConfigMng.Instance.GetStrengSuitRefList(2);
        for (int i = 0; i < suitList.Count; i++)
        {
            if (i + 1 < suitList.Count && _lev >= suitList[i].str_Lev && _lev < suitList[i + 1].str_Lev)
            {
                _fxCtrl.DoStrengthEffect(suitList[i].effects);
                return;
            }
            else if (i + 1 == suitList.Count && _lev>= suitList[i].str_Lev)
            {
                _fxCtrl.DoStrengthEffect(suitList[i].effects);
                return;
            }
        }
    }
    /// <summary>
    /// 排序(强化等级)
    /// </summary>
    /// <param name="_info1"></param>
    /// <param name="_info2"></param>
    /// <returns></returns>
    int SortEquUpdarLev(EquipmentInfo _info1,EquipmentInfo _info2)
    {
        if (_info1.UpgradeLv > _info2.UpgradeLv)
        {
            return 1;
        }
        if (_info1.UpgradeLv < _info2.UpgradeLv)
        {
            return -1;
        }
        return 0;
    }

    /// <summary>
    /// 根据uitype跳转到指定界面 add by zsy
    /// </summary>
    /// <param name="_id"></param>
    public void SkipWndById(int _id)
    {
        UISkipRef uISkipRef = ConfigMng.Instance.GetUISkipRef(_id);
        if (uISkipRef != null)
        {
            int uilev = uISkipRef.Level;
            int num = uISkipRef.num;

            //Debug.Log(" SetUrlToUIByWndId    id : " + wndId + "   , uilev : " + uilev + "  , num : " + num);

            if (uilev == 0)//为0代表可以直接跳转
            {
                if (num == 1)
                {
                    GUIType uiType = (GUIType)Enum.Parse(Type.GetType("GUIType"), (string)uISkipRef.type, true);
                    GameCenter.uIMng.SwitchToUI(uiType);
                }
                else if (num == 2)
                {
                    SubGUIType subUiType = (SubGUIType)Enum.Parse(Type.GetType("SubGUIType"), (string)uISkipRef.type);
                    GameCenter.uIMng.SwitchToSubUI(subUiType);
                }
                else
                {
                    Debug.LogError("界面跳转表中num配置错误，uiLev为0时代表可以直接跳转到1，2级界面");
                }
            }
            else if (uilev == 1)//跳转到一级界面的页签
            {
                GUIType uiType = (GUIType)Enum.Parse(Type.GetType("GUIType"), (string)uISkipRef.type, true);
                switch (uiType)
                {
                    case GUIType.SHOPWND:
                        GameCenter.shopMng.OpenWndByType((ShopItemType)num);
                        break;
                    case GUIType.NEWMALL:
                        GameCenter.newMallMng.OpenWndByType((MallItemType)num);
                        break;
                    case GUIType.ATIVITY:
                        GameCenter.activityMng.OpenStartSeleteActivity((ActivityType)num);
                        break;
                    default:
                        Debug.LogError("跳转到某一级界面的分页，但是界面跳转表中uiLev配置错误");
                        break;
                }
            }
            else if (uilev == 2)//跳转到二级界面的页签
            {
                SubGUIType subUiType = (SubGUIType)Enum.Parse(Type.GetType("SubGUIType"), (string)uISkipRef.type);
                if (subUiType == SubGUIType.BCopyTypeOne || subUiType == SubGUIType.BCopyType)
                {
                    GameCenter.duplicateMng.OpenCopyWndSelected(subUiType, (OneCopySType)num);
                }
                else
                {
                    GameCenter.uIMng.SwitchToSubUI(subUiType);
                }
            }
        }
        else
        {
            Debug.LogError("界面跳转表中uiLev配置错误");
        }
    }


    #endregion
}


/// <summary>
/// 排序比较
/// </summary>
public class ItemTidyComparer : IComparer<ItemTidyInstance>
{
    static ItemTidyComparer instance;
    public static ItemTidyComparer Instance
    {
        get
        {
            if (instance == null) instance = new ItemTidyComparer();
            return instance;
        }
    }
    ItemTidyComparer()
    {
    }
    public int Compare(ItemTidyInstance _x, ItemTidyInstance _y)
    {
        EquipmentInfo x = _y.EqInfo;
        EquipmentInfo y = _x.EqInfo;

        int ret = x.Slot.CompareTo(y.Slot);
        if (ret != 0) return ret;

        ret = x.UseReqLevel.CompareTo(y.UseReqLevel);
        if (ret != 0) return ret;

        ret = x.Quality.CompareTo(y.Quality);
        if (ret != 0) return ret;

        ret = _y.tryMergeCount.CompareTo(_x.tryMergeCount);//这里要以合并以后的数量计算 
        if (ret != 0) return ret;

        ret = x.InstanceID.CompareTo(y.InstanceID);

        return ret;
    }
}

/// <summary>
/// 排序比较临时数据
/// </summary>
public class ItemTidyInstance
{
    /// <summary>
    /// 原装备引用
    /// </summary>
    protected EquipmentInfo eqInfo;

    /// <summary>
    /// 原装备引用
    /// </summary>
    public EquipmentInfo EqInfo
    {
        get { return eqInfo; }
    }

    public ItemTidyInstance(EquipmentInfo _eq)
    {
        eqInfo = _eq;
        tryChangePos = _eq.Postion;
    }


    /// <summary>
    /// 整理后的临时数量
    /// </summary>
    public int tryMergeCount;
    /// <summary>
    /// 整理后的临时位置
    /// </summary>
    public int tryChangePos;

}
