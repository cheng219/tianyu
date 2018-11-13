//=====================================
//作者:吴江
//日期:2015/9/11
//用途:物品格子组件
//========================================



using UnityEngine;
using System.Collections;

public class ItemUI : MonoBehaviour
{
	[HideInInspector]public ItemShowUIType showUIType = ItemShowUIType.NONE;
    #region 控件数据
	public UISprite itemIcon;//将UISpriteEx修改成UISprite,解决拖动背包部分图标显示不出的bug
    public UILabel itemName;
    public UILabel itemCount;
    public UILabel strengthenLv;
	public UISprite betterFlag;
    public UISprite qualitybox;
    public UISprite lowerFlag;
    public UISprite isNewFlag;
	public UISprite lightFlag;
    public UILabel putMat;
    public bool isShowPutMat = false;
    /// <summary>
    /// 禁用标识
    /// </summary>
    public GameObject recycleForbid;
	/// <summary>
	/// 是否是绑定物品
	/// </summary>
	public GameObject NotBind;
	/// <summary>
	/// 紫色装备强化+8特效
	/// </summary>
	public GameObject strengthPurpleFx;
	/// <summary>
	/// 橙色装备强化+8特效
	/// </summary>
	public GameObject strengthOrangeFx;

	public bool lightFlagEnable = false;//修改预制上的值,决定是否显示(ItemContainer动态生成的不好控制)
    /// <summary>
    /// 坐骑幻化
    /// </summary>
    public UISprite mountChange;
	public GameObject cantUse;
	/// <summary>
	/// 红色遮罩(挡住不能交易、不能放入公会仓库等)
	/// </summary>
	public GameObject redMask;
	/// <summary>
	/// 职业遮罩
	/// </summary>
	public UISprite profMask;
    /// <summary>
    /// 当前是否锁定操作
    /// </summary>
    public GameObject lockFlag;
	/// <summary>
	/// 当前是否保护操作
	/// </summary>
	public GameObject protectFlag;
	/// <summary>
	/// 锁定物品栏、可解锁物品栏、转化中物品栏
	/// </summary>
	public GameObject lockEmptyItem;
	public GameObject canUnlockItem;
	public GameObject lockCdItem;
	public TweenFill lockCd;
    /// <summary>
    /// 交易中的标识
    /// </summary>
    public GameObject tradeFlag;

    /// <summary>
    /// 使用CD
    /// </summary>
    public GameObject useCDObj;
    public UILabel useCDtime;
    public UISprite useCDSpite;
	/// <summary>
	/// 是否批量选择
	/// </summary>
	public  bool showBatch= false;
	/// <summary>
	/// 批量选择
	/// </summary>
	public GameObject batchObj;
    /// <summary>
    /// 操作事件监听组件
    /// </summary>
    protected UIEventListener eventListener;
    /// <summary>
    /// tooltip展示碰撞依据控件，如果为空则默认为碰撞自身 by吴江
    /// </summary>
    protected GameObject tooltipTrigger = null;
    /// <summary>
    /// 点击控件时是否展示物品信息
    /// </summary>
    protected bool showTooltip = true;
    protected bool showDetails = false;
    /// <summary>
    /// 原始图片
    /// </summary>
	[HideInInspector]
    public string originIconName = string.Empty;
    /// <summary>
    /// 点击控件时是否展示物品信息
    /// </summary>
    public bool ShowTooltip
    {
        get { return showTooltip; }
        set { showTooltip = value; }
    }
    /// <summary>
    /// 物品堆叠数量为0时,是否显示不可用状态
    /// </summary>
    public bool showZeroCount = false;
    /// <summary>
    /// 控件序号
    /// </summary>
    public int pos = 0;
    /// <summary>
    /// 武器幸运等级
    /// </summary>
    public GameObject luckyGo;

    protected ItemActionType leftAction = ItemActionType.None;
    protected ItemActionType middleAction = ItemActionType.None;
    protected ItemActionType rightAction = ItemActionType.None;
    protected ItemActionType otherAction = ItemActionType.None;
    #endregion 

    #region 引用数据
    /// <summary>
    /// 当前要表现的物品数据 by吴江
    /// </summary>
    protected EquipmentInfo equipmentInfo;
    /// <summary>
    /// 当前要表现的物品数据 by吴江
    /// </summary>
    public EquipmentInfo EQInfo
    {
        get { return equipmentInfo; }
    }
    /// <summary>
    /// 当前格子内物品是否为空 by吴江
    /// </summary>
    public bool IsEmtpy
    {
        get
        {
            return equipmentInfo == null;
        }
    }


    public System.Action<ItemUI> OnSelectEvent = null;

    /// <summary>
    /// 获取格子内物品数据引用 by吴江
    /// </summary>
    /// <param name="_obj"></param>
    /// <returns></returns>
    public static EquipmentInfo GetEqInfo(GameObject _obj)
    {
        ItemUI itemUI = _obj.GetComponent<ItemUI>();
        if (itemUI == null)
        {
            Debug.LogError("请在控件对象上加上<ItemUI>组件!!");
            return null;
        }
        return itemUI.EQInfo;
    }
    #endregion

    #region Unity流程

    void OnDestroy()
    {
        if (equipmentInfo != null)
        {
            equipmentInfo.OnPropertyUpdate -= Refresh;
        }
    }
    #endregion

    #region 表现操作

    /// <summary>
    /// 刷新冷却时间
    /// </summary>
    void RefreshUseCD()
    {
        if (GameCenter.inventoryMng.UseCD==null||equipmentInfo == null || equipmentInfo.CDInfo == null)
            return;
            if (GameCenter.inventoryMng.UseCD.ContainsKey(equipmentInfo.CDInfo.id))
            {
                if (GameCenter.inventoryMng.UseCD[equipmentInfo.CDInfo.id] != 0)
                {
                    float f = Time.time - GameCenter.inventoryMng.UseCD[equipmentInfo.CDInfo.id];
                    float s = f / (equipmentInfo.CDInfo.time / 1000);
//                   Debug.Log(f);
                    if (s >= 1)
                    {
                        if (useCDObj != null)
                            useCDObj.SetActive(false);
                        GameCenter.inventoryMng.UseCD[equipmentInfo.CDInfo.id] = 0;
                        CancelInvoke("RefreshUseCD");

                    }
                    else
                    {
                        if (useCDObj != null)
                            useCDObj.SetActive(true);
                        if (useCDSpite != null)
                            useCDSpite.fillAmount = 1 - s;
                        if (useCDtime != null)
                            useCDtime.text = ((int)(equipmentInfo.CDInfo.time / 1000 - f) + 1).ToString();
                    }
                }
                else
                {
                    if (useCDObj != null && useCDObj.activeSelf)
                        useCDObj.SetActive(false);
                    CancelInvoke("RefreshUseCD");

                }
            }
        else
        {
            if (useCDObj != null && useCDObj.activeSelf)
            {
                useCDObj.SetActive(false);
                CancelInvoke("RefreshUseCD");
            }
        }

    }


    /// <summary>
    /// 往格子内填充物品数据 by吴江
    /// </summary>
    /// <param name="_info"></param>
    public void FillInfo(EquipmentInfo _info)
    {
        if (this != null) UIEventListener.Get(gameObject).onClick -= ClickItemEvent;
		if(this != null)UIEventListener.Get(gameObject).onClick += ClickItemEvent;
        if (equipmentInfo == _info)
        {
            RefreshNewState();
            RefreshCanUse();
            //RefreshBetter();
			RefreshNotBind();
            ShowRedMask();
        }
        if (equipmentInfo != null) equipmentInfo.OnPropertyUpdate -= Refresh;
       
        equipmentInfo = _info;
        if (equipmentInfo != null) equipmentInfo.OnPropertyUpdate += Refresh;
      
       
        if (equipmentInfo != null)
        {
			if (equipmentInfo.BelongTo == EquipmentBelongTo.BACKPACK || equipmentInfo.BelongTo == EquipmentBelongTo.EQUIP)
			{
	            switch (equipmentInfo.Family)
	            {
	                case EquipmentFamily.CONSUMABLES:
	                case EquipmentFamily.MOUNT:
	                case EquipmentFamily.PET:
					case EquipmentFamily.POTION:
					case EquipmentFamily.CHANGE:
					case EquipmentFamily.GEM:
					case EquipmentFamily.COSMETIC:
					case EquipmentFamily.MATERIAL:
					if(equipmentInfo.CanDiscard)
						SetActionBtn(ItemActionType.TryToUse, ItemActionType.TryToDestory, ItemActionType.Flaunt);
					else//不能丢弃 显示两个按钮
						SetActionBtn(ItemActionType.TryToUse, ItemActionType.Flaunt, ItemActionType.None);
	                    break;
					case EquipmentFamily.WEAPON:
                        SetActionBtn(ItemActionType.NormalLeft, ItemActionType.NormalMiddle, ItemActionType.NormalRight, ItemActionType.BLESSING);
                        break;
					case EquipmentFamily.ARMOR:
					case EquipmentFamily.JEWELRY:
                        SetActionBtn(ItemActionType.NormalLeft, ItemActionType.NormalMiddle, ItemActionType.NormalRight);
						break;
                    case EquipmentFamily.MOUNTEQUIP:
                        if (equipmentInfo.BelongTo == EquipmentBelongTo.BACKPACK)
						    SetActionBtn(ItemActionType.NormalLeft, ItemActionType.NormalMiddle, ItemActionType.NormalRight);
                        else//骑装界面只显示炫耀
                            SetActionBtn(ItemActionType.Flaunt, ItemActionType.None, ItemActionType.None);
						break;
	                default:
                        SetActionBtn(ItemActionType.None, ItemActionType.None, ItemActionType.None, ItemActionType.None);
	                    break;
				}
			}
			if(equipmentInfo.Family == EquipmentFamily.GEM && GameCenter.uIMng.CurOpenType == GUIType.EQUIPMENTTRAINING)
			{
				if(equipmentInfo.BelongTo == EquipmentBelongTo.BACKPACK)
                    SetActionBtn(ItemActionType.Inlay, ItemActionType.Synthetic, ItemActionType.None, ItemActionType.None);//装备培养界面的宝石镶嵌
				if(equipmentInfo.BelongTo == EquipmentBelongTo.EQUIP)
                    SetActionBtn(ItemActionType.UnInlay, ItemActionType.Synthetic, ItemActionType.None, ItemActionType.None);//装备培养界面的宝石卸下
			}
            if (equipmentInfo.BelongTo == EquipmentBelongTo.PREVIEW)
            {
                SetActionBtn(ItemActionType.None, ItemActionType.None, ItemActionType.None, ItemActionType.None);
            }
            if (equipmentInfo.BelongTo == EquipmentBelongTo.WAREHOUSE)
            {
                SetActionBtn(ItemActionType.TakeOutStorage, ItemActionType.None, ItemActionType.None, ItemActionType.None);
            }
            if (equipmentInfo.BelongTo == EquipmentBelongTo.TRADEBOX)
            {
                SetActionBtn(ItemActionType.TAKEOUT, ItemActionType.None, ItemActionType.None, ItemActionType.None);
            }
            switch (showUIType)
            {
                case ItemShowUIType.SHOPBAG:
                case ItemShowUIType.GUILDSHOPBAG:
                    SetActionBtn(ItemActionType.StoreSell, ItemActionType.None, ItemActionType.None, ItemActionType.None);
                    break;
                case ItemShowUIType.MARKETBAG:
                    SetActionBtn(ItemActionType.Putaway, ItemActionType.None, ItemActionType.None, ItemActionType.None);
                    break;
                case ItemShowUIType.TRADEBAG:
                    bool isTrade = GameCenter.tradeMng.IsInTradeBox(equipmentInfo.InstanceID);
                    SetActionBtn(isTrade?ItemActionType.None:ItemActionType.Trade, ItemActionType.None, ItemActionType.None, ItemActionType.None);
                    break;
            }
        }
        if (equipmentInfo != null)
        {
            if (equipmentInfo.CDInfo != null)
            {
                if (equipmentInfo.BelongTo == EquipmentBelongTo.BACKPACK && equipmentInfo.CDInfo.id != 0)
                {
                    CancelInvoke("RefreshUseCD");
                    InvokeRepeating("RefreshUseCD", 0.0f, 1.0f);
                }
            }
        }
        Refresh();
    }

    /// <summary>
    /// 获得详细信息后再次刷新
    /// </summary>
    public void GetItemDetailData(EquipmentInfo _info) 
    {

        equipmentInfo = _info;
        // 这个方法会执行两次，做一下屏蔽
        if (!equipmentInfo.IsGotSeverdata) FillInfo(_info);
        
    }

    protected void RefreshNewState()
    {
        if (isNewFlag != null)
        {
            isNewFlag.gameObject.SetActive(equipmentInfo != null && equipmentInfo.isNew);
        }
    }

    protected void RefreshCanUse()
    {
        if (equipmentInfo != null && equipmentInfo.BelongTo != EquipmentBelongTo.EQUIP && equipmentInfo != null && itemIcon != null)
        {
            if (cantUse != null)
            {
                cantUse.SetActive(equipmentInfo.CheckUse(GameCenter.mainPlayerMng.MainPlayerInfo) == false);//不可用表现修改
            }
        }
    }
	/// <summary>
	/// 非绑定标识
	/// </summary>
	protected void RefreshNotBind()
	{
		if(NotBind != null)
		{
            if (equipmentInfo != null)
            {
				NotBind.SetActive(equipmentInfo.IsBind);
            }
            else
            {
                NotBind.SetActive(false);
            }
		}
	}
    public void SetBindByClient(bool isNotBind)
    {
        if (NotBind != null)
        {
            NotBind.SetActive(!isNotBind);
        }
    }


    protected void RefreshBetter()
    {
        if (lowerFlag != null)
        {
            if (equipmentInfo == null || equipmentInfo.Family != EquipmentFamily.COSMETIC)
            {
                lowerFlag.gameObject.SetActive(false);
                return;
            }
            if (equipmentInfo.CheckClass(GameCenter.curMainPlayer.Prof) && equipmentInfo.BelongTo != EquipmentBelongTo.EQUIP)
            {
                lowerFlag.gameObject.SetActive(equipmentInfo.Slot != EquipSlot.None
                    && GameCenter.inventoryMng.EquipDictionary.ContainsKey(equipmentInfo.Slot)
                    && equipmentInfo.IsLower(GameCenter.inventoryMng.EquipDictionary[equipmentInfo.Slot]));
            }
            else
            {
                lowerFlag.gameObject.SetActive(false);
            }
        }
    }

    protected void Refresh()
    {
		if(this == null)return;
        if (equipmentInfo == null)
        {
            CleanData();
            RefreshNewState();
            return;
        }
        if (equipmentInfo.CurEmptyType != EquipmentInfo.EmptyType.NONE)
        {
            switch (equipmentInfo.CurEmptyType)
            {
                case EquipmentInfo.EmptyType.EMPTY:
                    FillEmptyItem();
                    return;
                case EquipmentInfo.EmptyType.LOCK:
                    FillLockItem();
                    return;
                case EquipmentInfo.EmptyType.CANUNLOCK:
                    FillCanUnLockItem();
                    return;
                case EquipmentInfo.EmptyType.CDLOCK:
                    FillCdItem();
                    return;
            }
        }
        else
        {
            if (lockEmptyItem != null) lockEmptyItem.SetActive(false);
            if (canUnlockItem != null) canUnlockItem.SetActive(false);
            if (lockCdItem != null) lockCdItem.SetActive(false);
        }
		//加上物品命名,新手引导需要用到,之前是UIItemWidget_+Icon 避免配置的修改 by邓成
		if(this != null && gameObject != null)
			gameObject.name = new System.Text.StringBuilder().Append("UIItemWidget_").Append(equipmentInfo.IconName).ToString();
        if (itemIcon != null)
        {
            itemIcon.spriteName = equipmentInfo.IconName;
            itemIcon.gameObject.SetActive(true);
            //itemIcon.IsGray = equipmentInfo.IsGray ? UISpriteEx.ColorGray.Gray : UISpriteEx.ColorGray.normal;
        }
        else
        {
            GameSys.LogError(ConfigMng.Instance.GetUItext(195));
        }
        RefreshNewState();
        RefreshCanUse();
        //RefreshBetter();
		RefreshNotBind();
		ShowRedMask();

        // 用于坐骑幻化显示 by 易睿
        if (mountChange != null)
        {
            if (equipmentInfo.Family == EquipmentFamily.MOUNT)
            {
                mountChange.gameObject.SetActive(equipmentInfo.IsChangeMount);
            }
            else
            {
                mountChange.gameObject.SetActive(false);
            }
        }

        if (itemCount != null)
        {
            ////物品的可叠加数大于1,并且数量大于0时才显示数量标记
            if ((equipmentInfo.StackMaxCount > 1 && equipmentInfo.StackCurCount > 0) || equipmentInfo.StackCurCount > 1)
            {
                itemCount.text = equipmentInfo.StackCurCount >= 100000 ? (equipmentInfo.StackCurCount / 10000).ToString() + "万" : equipmentInfo.StackCurCount.ToString();
                itemCount.gameObject.SetActive(true);
            }
            else if (equipmentInfo.StackCurCount == 0 && showZeroCount)
            {
                itemCount.text = "";
                itemCount.gameObject.SetActive(true);
                if (cantUse != null)
                    cantUse.SetActive(true);
            }
            else
            {
                itemCount.text = string.Empty;
                itemCount.gameObject.SetActive(false);
            }
        }
        if (strengthenLv != null)
        {
            if (equipmentInfo.UpgradeLv > 0 && equipmentInfo.Family != EquipmentFamily.MOUNTEQUIP)
            {
				strengthenLv.text = "+" + equipmentInfo.UpgradeLv.ToString();
				strengthenLv.gameObject.SetActive(true);
            }
            else
            {
                strengthenLv.text = string.Empty;
				strengthenLv.gameObject.SetActive(false);
            }
        }
        
		if(strengthPurpleFx != null && strengthOrangeFx != null  )
		{
			strengthPurpleFx.SetActive(equipmentInfo.UpgradeLv >= 8 && equipmentInfo.Quality == 4);
			strengthOrangeFx.SetActive(equipmentInfo.UpgradeLv >= 8 && equipmentInfo.Quality == 5);
		}
		if(betterFlag != null)
		{
			if(equipmentInfo.Slot != EquipSlot.None && equipmentInfo.Family != EquipmentFamily.GEM && equipmentInfo.NeedProf == GameCenter.mainPlayerMng.MainPlayerInfo.Prof)
			{
				betterFlag.enabled = equipmentInfo.IsBetterSlot(equipmentInfo);
			}else
			{
				betterFlag.enabled = false;
			}
		}
        if (qualitybox != null)
        {
			qualitybox.enabled = equipmentInfo.QualityBox != string.Empty;
            qualitybox.color = equipmentInfo.ItemColor;
        }
		if(profMask != null)
		{
			bool profNo = equipmentInfo.IsEquip && !equipmentInfo.CheckClass(GameCenter.mainPlayerMng.MainPlayerInfo.Prof);
			bool levelNo = equipmentInfo.IsEquip && equipmentInfo.UseReqLevel > GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel;
			profMask.enabled = profNo || levelNo;
		}
		if(lightFlagEnable)//稀有道具获得时显示闪光效果 by邓成
			ShowLightFlag();
		if (gameObject.GetComponent<BoxCollider>() != null)
			gameObject.GetComponent<BoxCollider>().enabled = true;
		gameObject.SetActive(true);
        if (batchObj != null)//by黄洪兴 是否显示批量选中
        {
            if (showBatch && equipmentInfo.CanSell)
            {
                batchObj.SetActive(true); 
            }
            else
            {
                batchObj.SetActive(false);
            }
        }
        if (showUIType == ItemShowUIType.MARKETBAG)
        {
            if (equipmentInfo.IsBind)
            {
                if (cantUse != null)
                    cantUse.SetActive(true);
            }
        }
        if (showUIType == ItemShowUIType.TRADEBAG)
        {
            if (tradeFlag != null && GameCenter.tradeMng != null)
            {
                tradeFlag.SetActive(GameCenter.tradeMng.IsInTradeBox(equipmentInfo.InstanceID));
            }
        }
        if (luckyGo != null)
        {
            if (equipmentInfo.LuckyLv >= 1)
            {
                luckyGo.SetActive(true);
                luckyGo.GetComponentInChildren<UILabel>().text = equipmentInfo.LuckyLv.ToString();
            }
            else
                luckyGo.SetActive(false);
        }
		ShowName();
        ShowPutMat();
    }
    /// <summary>
    /// 清理格子内数据和表现 by吴江
    /// </summary>
    public void CleanData()
    {

      //  if (equipmentInfo == null) return;  add . 直接传个空的物品 要把所有的子组件隐藏
        equipmentInfo = null;
		CleanUi();
    }
	protected void CleanUi()
	{
		if (batchObj != null) batchObj.gameObject.SetActive(false);
		if (itemIcon != null) itemIcon.gameObject.SetActive(false);
		if (itemName != null) itemName.gameObject.SetActive(false);//text = "";
		if (itemCount != null) itemCount.gameObject.SetActive(false);
		if (strengthenLv != null) strengthenLv.gameObject.SetActive(false);
		if(betterFlag != null)betterFlag.enabled = false;
		if(redMask != null)redMask.SetActive(false);
		if(profMask != null)profMask.enabled = false;
		if (lockFlag != null) lockFlag.SetActive(false);
		if (lowerFlag != null) lowerFlag.gameObject.SetActive(false);
		if (qualitybox != null) qualitybox.enabled = false;
		if(cantUse != null)cantUse.SetActive(false);
		if(protectFlag != null)protectFlag.SetActive(false);
		if(lightFlag != null)lightFlag.gameObject.SetActive(false);
		if (mountChange != null) mountChange.gameObject.SetActive(false);
		if(NotBind != null)NotBind.SetActive(false);
		if(lockEmptyItem != null)lockEmptyItem.SetActive(false);
		if(canUnlockItem != null)canUnlockItem.SetActive(false);
		if(lockCdItem != null)lockCdItem.SetActive(false);
        if (useCDObj != null) useCDObj.SetActive(false);
        if (tradeFlag != null)tradeFlag.SetActive(false);
		if(strengthPurpleFx != null && strengthOrangeFx != null  )
		{
			strengthPurpleFx.SetActive(false);
			strengthOrangeFx.SetActive(false);
		}
        if (luckyGo != null) luckyGo.SetActive(false);
		if (gameObject.GetComponent<BoxCollider>() != null)
			gameObject.GetComponent<BoxCollider>().enabled = false;
	}
	/// <summary>
	/// 清理格子内数据并隐藏
	/// </summary>
	public void CleanData2Disable()
	{
		equipmentInfo = null;
		gameObject.SetActive(false);
	}
    /// <summary>
    /// 需要显示名字时调用
    /// </summary>
    public void ShowName()
    {
        if (itemName != null)
        {
            itemName.text = equipmentInfo.ItemStrColor + equipmentInfo.ItemName;//_info.IconName改为_info.ItemName
            itemName.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 需要显示放入材料的label时显示
    /// </summary>
    public void ShowPutMat()
    {
        if (isShowPutMat)
        {
            if (putMat != null) putMat.gameObject.SetActive(true);
        }
        else
        {
            if (putMat != null) putMat.gameObject.SetActive(false); 
        }
    }
    /// <summary>
    /// 回收禁用
    /// </summary>
    /// <param name="_bool"></param>
    public void RecycleForbid(bool _bool)
    {
        if (recycleForbid != null)
        {
            recycleForbid.SetActive(_bool);
        }
    }
	/// <summary>
	/// 紫色或者橙色显示Light效果
	/// </summary>
	public void ShowLightFlag()
	{
		if(lightFlag != null )
		{
			if(equipmentInfo.Quality == 4)
			{
				lightFlag.color = new Color(1f,0f,1f,1f);
				lightFlag.gameObject.SetActive(true);
			}
			else if(equipmentInfo.Quality == 5)
			{
				lightFlag.color = new Color(1f,125f/255f,0f,1f);
				lightFlag.gameObject.SetActive(true);
			}else
			{
				lightFlag.gameObject.SetActive(false);
			}
		}
			
	}
    public void SetEmtpy()
    {
        if (itemIcon != null) itemIcon.spriteName = originIconName;
		if (qualitybox != null) qualitybox.enabled = false;
		if(protectFlag != null)protectFlag.SetActive(false);
		if(lightFlag != null)lightFlag.gameObject.SetActive(false);
    }

    /// <summary>
    /// 设置为空格子 by黄洪兴
    /// </summary>
    public void SetEmpty()
    {
        equipmentInfo = null;
        if (itemIcon != null) itemIcon.spriteName = originIconName;
		if (qualitybox != null) qualitybox.enabled = false;
        if (protectFlag != null) protectFlag.SetActive(false);
        if (lightFlag != null) lightFlag.gameObject.SetActive(false);
        if (NotBind != null) NotBind.gameObject.SetActive(false);// by 黄洪兴  设为空时影藏绑定图标
        if (itemCount != null) itemCount.gameObject.SetActive(false);// by 黄洪兴  设为空时影藏数量图标
        if (itemName != null) itemName.gameObject.SetActive(false);
    }

    public void SetInvalid()
    {
        itemIcon.spriteName = "Icon_Lock";

    }

    public void DataUpdate()
    {
        FillInfo(equipmentInfo);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
	/// <summary>
//	/// 隐藏之后,去掉选中效果
//	/// </summary>
//	void OnDisable()
//	{
//	}
	/// <summary>
	/// 勾选批量出售
	/// </summary>
     public void OnBatchSell()
	{
		if (batchObj != null) {
			if (batchObj.GetComponent<UIToggle> ().value) {
				GameCenter.buyMng.SellEqList.Add (equipmentInfo.InstanceID);
				if(GameCenter.buyMng.OnSellEqChange!=null)
				GameCenter.buyMng.OnSellEqChange ();
			} else {
				GameCenter.buyMng.SellEqList.Remove (equipmentInfo.InstanceID);
				if(GameCenter.buyMng.OnSellEqChange!=null)
				GameCenter.buyMng.OnSellEqChange ();
			}
		}
	}
	protected bool showRedMask = false;
	/// <summary>
	/// 显示红色遮罩
	/// </summary>
	protected void ShowRedMask()
	{
		bool needMask = false;
		GuildStorageWnd storage = GameCenter.uIMng.GetGui<GuildStorageWnd>();
        bool maskForUi = (storage != null);//公会仓库、显示红色遮罩
		if(equipmentInfo != null)
		{
			needMask = maskForUi && equipmentInfo.BelongTo == EquipmentBelongTo.BACKPACK &&  (equipmentInfo.Slot == EquipSlot.None 
				|| equipmentInfo.Family == EquipmentFamily.GEM || equipmentInfo.IsBind || equipmentInfo.HasInlayGem);
		}
        if (GameCenter.uIMng.CurOpenType == GUIType.TRADE || GameCenter.uIMng.CurOpenType == GUIType.MARKET)//by黄洪兴
        {
            if (equipmentInfo != null && equipmentInfo.BelongTo == EquipmentBelongTo.BACKPACK)
            {
                if (equipmentInfo.IsBind)
                {
                    needMask=true;
                }
            }
        }
		if(redMask != null)redMask.SetActive(needMask);
		showRedMask = needMask;
	}

    /// <summary>
    /// 禁用效果
    /// </summary>
    public void Forbidden(bool _bool)
    {
        GetComponent<BoxCollider>().enabled = !_bool;
        RecycleForbid(_bool);
    }

    public void SetActionBtn(ItemActionType _left, ItemActionType _middle, ItemActionType _right)
    {
        leftAction = _left;
        middleAction = _middle;
        rightAction = _right;
    }
    public void SetActionBtn(ItemActionType _left, ItemActionType _middle, ItemActionType _right, ItemActionType _other)
    {
        leftAction = _left;
        middleAction = _middle;
        rightAction = _right;
        otherAction = _other;
    }



    /// <summary>
    /// 展示详细信息
    /// </summary>
    /// <param name="obj"></param>
    protected void OnShowTooltip(GameObject obj)
    {

        if (showUIType == ItemShowUIType.MARKETBAG || showUIType == ItemShowUIType.TRADEBAG)
        {
            if (equipmentInfo.IsBind)
            {
                GameCenter.messageMng.AddClientMsg(326);
                return;
            }
        }
		if(showRedMask)//by 邓成
		{
			return;
		}
        if (equipmentInfo!=null&&equipmentInfo.CDInfo != null)
        {
            if (equipmentInfo.BelongTo == EquipmentBelongTo.BACKPACK)
            {
                if (GameCenter.inventoryMng.UseCD.ContainsKey(equipmentInfo.CDInfo.id))
                {

                    if (GameCenter.inventoryMng.UseCD[equipmentInfo.CDInfo.id] != 0)
                    {
                        GameCenter.messageMng.AddClientMsg(259);
                        return;
                    }

                }
            }
        }

        if (equipmentInfo != null && showTooltip)
        {
			StorageWnd storageWnd = GameCenter.uIMng.GetGui<StorageWnd>();
			GuildStorageWnd guildStorageWnd = GameCenter.uIMng.GetGui<GuildStorageWnd>();
			if(storageWnd != null ||  guildStorageWnd != null)//当前打开的是仓库,屏蔽所有按钮.只有取出or放入
			{
				if(equipmentInfo.BelongTo == EquipmentBelongTo.BACKPACK)
					ToolTipMng.ShowEquipmentTooltip(equipmentInfo, ItemActionType.PutInStorage, ItemActionType.None, ItemActionType.None, ItemActionType.None, tooltipTrigger == null ? this.gameObject : tooltipTrigger);
				else if(equipmentInfo.BelongTo == EquipmentBelongTo.STORAGE)
					ToolTipMng.ShowEquipmentTooltip(equipmentInfo, ItemActionType.TakeOutStorage, ItemActionType.None, ItemActionType.None, ItemActionType.None, tooltipTrigger == null ? this.gameObject : tooltipTrigger);
			}
            else
			{
				ToolTipMng.ShowEquipmentTooltip(equipmentInfo, leftAction, middleAction, rightAction, otherAction, tooltipTrigger == null ? this.gameObject : tooltipTrigger);
			}
        }

        if (equipmentInfo != null && equipmentInfo.BelongTo == EquipmentBelongTo.EQUIP)
            GameCenter.inventoryMng.CurSelectEquip = equipmentInfo;
        ///能使用的物品(非装备)用于使用预览 by 唐源
        if (equipmentInfo != null && equipmentInfo.CanUse)
            GameCenter.inventoryMng.CurSelectInventory = equipmentInfo;//如果当前选中的物品是可以使用的

        if (OnSelectEvent != null)
        {
            OnSelectEvent(this);
        }
    }

	protected void ClickItemEvent(GameObject go)
	{
		if (equipmentInfo == null) return;
		switch(equipmentInfo.CurEmptyType)
		{
		case EquipmentInfo.EmptyType.NONE:
			OnShowTooltip(go);
			break;
		case EquipmentInfo.EmptyType.EMPTY:
			break;
		case EquipmentInfo.EmptyType.CANUNLOCK:
			if (equipmentInfo.BelongTo == EquipmentBelongTo.BACKPACK)GameCenter.inventoryMng.C2S_UnlockBagSlot(equipmentInfo.Postion);
			//GameCenter.messageMng.AddClientMsg("解锁可解锁的格子POS:"+equipmentInfo.Postion);
			break;
		case EquipmentInfo.EmptyType.CDLOCK:
		case EquipmentInfo.EmptyType.LOCK:
			if (equipmentInfo.BelongTo == EquipmentBelongTo.BACKPACK)GameCenter.inventoryMng.C2S_ReqUnlockBagCost(equipmentInfo.Postion);
			//GameCenter.messageMng.AddClientMsg("解锁pos:"+equipmentInfo.Postion+",之前的所有格子");
			break;
		}
	}
    /// <summary>
    /// 快捷回收
    /// </summary>
    public void OnRecycle()
    {
        if (equipmentInfo == null) return;
    }
    #endregion

	#region 仙侠中,锁定物品栏、可解锁物品栏、转化中物品栏 by邓成
	protected void FillEmptyItem()
	{
		CleanUi();
		SetEmtpy();
		if (gameObject.GetComponent<BoxCollider>() != null)
			gameObject.GetComponent<BoxCollider>().enabled = true;
	}
	public void FillLockItem()
	{
		CleanUi();
		SetEmtpy();
		if(lockEmptyItem != null)lockEmptyItem.SetActive(true);
		if (gameObject.GetComponent<BoxCollider>() != null)
			gameObject.GetComponent<BoxCollider>().enabled = true;
	}
	public void FillCanUnLockItem()
	{
		CleanUi();
		SetEmtpy();
		if(canUnlockItem != null)canUnlockItem.SetActive(true);
		if (gameObject.GetComponent<BoxCollider>() != null)
			gameObject.GetComponent<BoxCollider>().enabled = true;
	}
	public void FillCdItem()
	{
		CleanUi();
		SetEmtpy();
		if(lockCdItem != null)lockCdItem.SetActive(true);
		if (gameObject.GetComponent<BoxCollider>() != null)
			gameObject.GetComponent<BoxCollider>().enabled = true;
		if(lockCd != null)
		{
			int restTime = GameCenter.inventoryMng.BagCdTime;
			int totalTime = GameCenter.inventoryMng.BagCdTotalTime;
			lockCd.ResetToBeginning();
			lockCd.from = (float)restTime / (float)totalTime;
			lockCd.duration = restTime;
			lockCd.enabled = true;
			EventDelegate.Remove(lockCd.onFinished,FillCanUnLockItem);
			EventDelegate.Add(lockCd.onFinished,FillCanUnLockItem);
		}
	}
	#endregion

    #region 创建与删除

    /// <summary>
    /// 释放自身 by吴江
    /// </summary>
    public void Release()
    {
        if (equipmentInfo != null) equipmentInfo = null;
        DestroyImmediate(this.gameObject);
        Destroy(this);
    }


    /// <summary>
    /// 创建一个新的itemUI by吴江
    /// </summary>
    /// <param name="_parent"></param>
    /// <param name="_index"></param>
    /// <param name="_tooltipTrigger"></param>
    /// <returns></returns>
    public static ItemUI CreatNew(UIExGrid _parent, int _index,GameObject _tooltipTrigger = null)
    {
        if (_parent == null)
        {
            GameSys.LogError("父grid组件为空！如果没有父控件，请使用合适的重载方法！");
            return null;
        }
        if (_index < 0)
        {
            GameSys.LogError("位置序数不能为负数！");
            return null;
        }
		Object prefab = null;
        if (prefab == null)
        {
            prefab = exResources.GetResource(ResourceType.GUI, "Item_icon/Item_icon");
        }
        if (prefab == null)
        {
            GameSys.LogError("找不到预制：Item_icon/Item_icon");
            return null;
        }
        GameObject obj = Instantiate(prefab) as GameObject;
        Transform parentTransf = _parent.gameObject.transform;
        obj.transform.parent = parentTransf;
		obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        prefab = null;
        ItemUI itemUI = obj.GetComponent<ItemUI>();
        if (itemUI == null)
        {
            GameSys.LogError("预制上找不到组件：<ItemUI>");
            return null;
        }
        itemUI.pos = _index;
        _parent.AddSolt(obj.transform, _index);
         //itemUI.tooltipTrigger = _tooltipTrigger;
        return itemUI;
    }



    /// <summary>
    /// 创建一个新的itemUI by吴江
    /// </summary>
    /// <param name="_parent"></param>
    /// <param name="_index"></param>
    /// <param name="_tooltipTrigger"></param>
    /// <returns></returns>
    public static ItemUI CreatNew(Transform _parent, Vector3 _localPos, Vector3 _localScale, bool _showToolTip = true)
    {
        if(_parent == null)
        {
            return null;
        }
		Object prefab = null;
        if (prefab == null)
        {
            prefab = exResources.GetResource(ResourceType.GUI, "Item_icon/Item_icon");
        }
        if (prefab == null)
        {
            GameSys.LogError("找不到预制：Item_icon/Item_icon");
            return null;
        }
        GameObject obj = Instantiate(prefab) as GameObject;
        obj.transform.parent = _parent;
        obj.transform.localPosition = _localPos;
        obj.transform.localScale = _localScale;
        ItemUI itemUI = obj.GetComponent<ItemUI>();
        if (itemUI == null)
        {
            GameSys.LogError("预制上找不到组件：<ItemUI>");
            return null;
        }
        itemUI.ShowTooltip = _showToolTip;
        prefab = null;
        return itemUI;
    }

	/// <summary>
	/// 创建一个新的itemUI,没有Load的过程 by邓成
	/// </summary>
	public static ItemUI CreatNewByPrefab(UIExGrid _parent, int _index,GameObject _itemPrefab)
	{
		if (_parent == null)
		{
			GameSys.LogError("父grid组件为空！如果没有父控件，请使用合适的重载方法！");
			return null;
		}
		if (_index < 0)
		{
			GameSys.LogError("位置序数不能为负数！");
			return null;
		}
		GameObject obj = Instantiate(_itemPrefab) as GameObject;
		Transform parentTransf = _parent.gameObject.transform;
		obj.transform.parent = parentTransf;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale = Vector3.one;
		ItemUI itemUI = obj.GetComponent<ItemUI>();
		if (itemUI == null)
		{
			GameSys.LogError("预制上找不到组件：<ItemUI>");
			return null;
		}
		itemUI.pos = _index;
		_parent.AddSolt(obj.transform, _index);
		//itemUI.tooltipTrigger = _tooltipTrigger;
		return itemUI;
	}
    #endregion



}
