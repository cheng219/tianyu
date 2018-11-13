//==============================================
//作者：邓成
//日期：2016/2/29
//用途：背包界面
//=================================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackPackWnd : GUIBase
{
    #region UI控件对象
    public BackpackPageUI backpackPageUI;

    /// <summary>
    /// 关闭按钮
    /// </summary>
    public UIButton CloseButton;

	public GameObject batchUseGo;
	public UIButton btnBatchUse;
	public UIInput batchUseNum;
	public UILabel labDes;
	public UIButton btnAdd;
	public UIButton btnReduce;
	public UIButton btnMaxNum;
	public ItemUI batchUseItem;
    /// <summary>
    /// 整理按钮
    /// </summary>
    public UIButton ArrangeButton;
	/// <summary>
	/// 黄金数
	/// </summary>
	public UILabel labGold;
	/// <summary>
	/// 银票数
	/// </summary>
	public UILabel labSilverTicket;
	/// <summary>
	/// 铜币数
	/// </summary>
	public UILabel labCopperCoin;
	/// <summary>
	/// 绑定铜币数
	/// </summary>
	public UILabel labBindCopperCoin;

    protected ItemShowUIType curShowUIType = ItemShowUIType.NONE;
    #endregion
    void Awake()
    {
        if (ArrangeButton != null) UIEventListener.Get(ArrangeButton.gameObject).onClick = OnClickArrangeBtn;
    }

    void Start()
    { 
    
    }

    protected override void OnOpen()
    {
        base.OnOpen();
		ToolTipMng.ShowEquipmentModel = false;
		RefreshCoin();
        InitProgress();
    }
	void InitProgress()
	{
        int initPage = 1;
        curShowUIType = GameCenter.inventoryMng.CurShowUIType;
        if (curShowUIType == ItemShowUIType.NORMALBAG && GameCenter.inventoryMng.CanOpenSlot && !GameCenter.noviceGuideMng.StartGuide && !GameCenter.inventoryMng.HaveShowOpenSlot)
        {
            initPage = GameCenter.inventoryMng.BagOpenCount / SystemSettingMng.PER_PAGE_BAG_NUM + 1;
            GameCenter.inventoryMng.HaveShowOpenSlot = true;
        }
        else
        {
            initPage = 1;
        }
        List<EquipmentInfo> backpackItems = new List<EquipmentInfo>(GameCenter.inventoryMng.RealBackpackDictionary.Values);
        backpackItems.Sort(SortEquipment);
        if (backpackPageUI != null)
        {
            backpackPageUI.SetUITypeForShop(curShowUIType);
            backpackPageUI.Init(initPage, backpackItems);
        }
        switch (curShowUIType)
        { 
            case ItemShowUIType.NONE:
            case ItemShowUIType.NORMALBAG:
            case ItemShowUIType.GUILDSHOPBAG:
            case ItemShowUIType.MARKETBAG:
            case ItemShowUIType.TRADEBAG:
                if (ArrangeButton != null) ArrangeButton.gameObject.SetActive(true);
                break;
            default:
                if (ArrangeButton != null) ArrangeButton.gameObject.SetActive(false);
                break;
        }
	}
	protected override void OnClose ()
	{
		base.OnClose ();
		ToolTipMng.ShowEquipmentModel = true;
	}
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
			GameCenter.inventoryMng.OnBackpackUpdate += UpdateAllItems;
			GameCenter.inventoryMng.OnEquipUpdate += UpdateAllItems;//为了一个装备战力提升的箭头而刷新
			GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += UpdateAllItems;
			GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += RefreshCoin;
			GameCenter.inventoryMng.OnBackpackItemUpdate += UpdateItems;
			//GameCenter.inventoryMng.OnOpenBatchUseWndEvent += OpenBatchUseWnd;
            GameCenter.buyMng.OnBatchSellEvent += RefreshBatchSell;
            GameCenter.tradeMng.OnTradeItemUpdate += RefreshTradeBag;
        }
        else
        {
			GameCenter.inventoryMng.OnBackpackUpdate -= UpdateAllItems;
			GameCenter.inventoryMng.OnEquipUpdate -= UpdateAllItems;
			GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= UpdateAllItems;
			GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= RefreshCoin;
			GameCenter.inventoryMng.OnBackpackItemUpdate -= UpdateItems;
			//GameCenter.inventoryMng.OnOpenBatchUseWndEvent -= OpenBatchUseWnd;
            GameCenter.buyMng.OnBatchSellEvent -= RefreshBatchSell;
            GameCenter.tradeMng.OnTradeItemUpdate -= RefreshTradeBag;
        }
    }

	void OpenBatchUseWnd(EquipmentInfo _eq)
	{
        //if(_eq.can)
        //GameCenter.uIMng.SwitchToUI(GUIType.IMMEDIATEUSE,GUIType.BACKPACKWND);
		//int useNum = 0;
		//if(batchUseGo != null && btnBatchUse != null && batchUseNum != null && int.TryParse(batchUseNum.value,out useNum))
		//{
		//	batchUseGo.SetActive(true);
  //          batchUseNum.value = "1";
		//	UIEventListener.Get(btnBatchUse.gameObject).onClick = (x)=>
		//	{
		//		if(int.TryParse(batchUseNum.value,out useNum) && useNum > _eq.StackCurCount)
		//		{
		//			GameCenter.messageMng.AddClientMsg("数量不足!");
		//			return;
		//		}
		//		GameCenter.inventoryMng.C2S_UseItems(_eq,useNum);
		//		batchUseGo.SetActive(false);
		//	};
		//}
		//if(batchUseItem != null)batchUseItem.FillInfo(_eq);
		//if(labDes != null)labDes.text = _eq.Description.Replace("\\n","\n");
		//if(btnAdd != null && batchUseNum != null && int.TryParse(batchUseNum.value,out useNum))
		//{
		//	UIEventListener.Get(btnAdd.gameObject).onClick = (x)=>
		//	{
		//		if(useNum < _eq.StackCurCount)
		//			batchUseNum.value = (++useNum).ToString();
		//	};
		//}
			
		//if(btnReduce != null && batchUseNum != null && int.TryParse(batchUseNum.value,out useNum))
		//{
		//	UIEventListener.Get(btnReduce.gameObject).onClick = (x)=>
		//	{
		//		if(useNum > 0)batchUseNum.value = (--useNum).ToString();
		//	};
		//}
		//if(btnMaxNum != null && batchUseNum != null )UIEventListener.Get(btnMaxNum.gameObject).onClick = (x)=>
		//{
		//	if(batchUseNum != null)batchUseNum.value = _eq.StackCurCount.ToString();
		//	useNum = _eq.StackCurCount;
		//};
	}
	/// <summary>
	/// 刷新金钱显示
	/// </summary>
	protected void RefreshCoin()
	{
		if(labGold != null)
			labGold.text = GameCenter.mainPlayerMng.MainPlayerInfo.DiamondCountText;
		if(labSilverTicket != null)
			labSilverTicket.text = GameCenter.mainPlayerMng.MainPlayerInfo.BindDiamondCountText;
		if(labCopperCoin != null)
			labCopperCoin.text = GameCenter.mainPlayerMng.MainPlayerInfo.UnBindCoinCountText;
		if(labBindCopperCoin != null)
			labBindCopperCoin.text = GameCenter.mainPlayerMng.MainPlayerInfo.BindCoinCountText;
	}
	protected void RefreshCoin(ActorBaseTag tag,ulong _value,bool flag)
	{
        if (tag == ActorBaseTag.UnBindCoin || tag == ActorBaseTag.BindCoin || tag == ActorBaseTag.Diamond || tag == ActorBaseTag.BindDiamond)
		{
			RefreshCoin();
		}
	}

    /// <summary>
    /// 创建所有物品
    /// </summary>
    protected void RefreshItems()
    {
		List<EquipmentInfo> backpackItems = new List<EquipmentInfo>(GameCenter.inventoryMng.RealBackpackDictionary.Values);
		backpackItems.Sort(SortEquipment);
        if (backpackPageUI != null) backpackPageUI.UpdateItems(backpackItems);
    }

	protected void UpdateAllItems(ActorBaseTag _tag,ulong val,bool _state)
	{
		if(_tag == ActorBaseTag.Level)
		{
			RefreshItems();
		}
	}
	/// <summary>
	/// 刷新所有物品
	/// </summary>
	protected void UpdateAllItems()
	{
        RefreshItems();
	}
	/// <summary>
	/// 背包更新单个数据
	/// </summary>
	protected void UpdateItems(int pos,EquipmentInfo eq)
	{
        RefreshItems();
	}
    /// <summary>
    /// 批量出售刷新
    /// </summary>
    /// <param name="_batchSell"></param>
    protected void RefreshBatchSell(bool _batchSell)
    {
        if (backpackPageUI != null) backpackPageUI.RefreshForBatchSell(_batchSell);
    }
    /// <summary>
    /// 交易物品时刷新背包
    /// </summary>
    protected void RefreshTradeBag(LockUpdateType _lockUpdateType)
    {
        if (curShowUIType != ItemShowUIType.TRADEBAG) return;
        if (_lockUpdateType == LockUpdateType.UPDATEMY || _lockUpdateType == LockUpdateType.TAKEOUR)
        {
            RefreshItems();
        }
    }
    #region 控件事件
	/// <summary>
	/// 点击整理按钮
	/// </summary>
	/// <param name="obj"></param>
	void OnClickArrangeBtn(GameObject obj)
	{
		GameCenter.inventoryMng.C2S_ArrangeBag();
	}

    #endregion
	public int SortEquipment(EquipmentInfo eq1,EquipmentInfo eq2)
	{
		if(eq1.Postion > eq2.Postion)
			return 1;
		if(eq1.Postion < eq2.Postion) 
			return -1;
		return 0;
	}
}
