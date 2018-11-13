//==================================
//作者：黄洪兴
//日期：2016/4/18
//用途：市场主界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MarketWnd : GUIBase {

	public GameObject noneMyMarketItem;
	public GameObject myItemGrid;
	public GameObject itemGrid;
	public GameObject typeGrid;
	public GameObject limitBackObj;
	public UIScrollView uicroll;
	public GameObject marketObj;
	public GameObject marketBtn;
	public GameObject auctionObj;
    public FlipOver flipOver;

	/// <summary>
	/// 大分页
	/// </summary>
	public MarketTypeContainer MarketTypeContainer;
	/// <summary>
	/// 拍卖的物品
	/// </summary>
	public MarketItemContainer MarketItemContainer;
	/// <summary>
	/// 我的拍卖的物品
	/// </summary>
	public MarketItemContainer MyMarketItemContainer;

	public GameObject MyMarketItemBtn;
	public GameObject MyMarketItemObj;
	public GameObject CloseBtn;
	/// <summary>
	/// 等级限制Obj
	/// </summary>
	public GameObject lvLimitObj;
	/// <summary>
	/// 等级限制
	/// </summary>
	public GameObject lvLimit;
	/// <summary>
	/// 品质限制Obj
	/// </summary>
	public GameObject qualityLimitObj;
	/// <summary>
	/// 品质限制
	/// </summary>
	public GameObject qualityLimit;
	/// <summary>
	/// 价格限制Obj
	/// </summary>
	public GameObject priceLimitObj;
	/// <summary>
	/// 价格限制
	/// </summary>
	public GameObject priceLimit;
	/// <summary>
	/// 货币限制Obj
	/// </summary>
	public GameObject moneyTypeLimitObj;
	/// <summary>
	/// 货币限制
	/// </summary>
	public GameObject moneyTypeLimit;


	public UILabel levLabel;
	public UILabel qualityLabel;
	public UILabel priceLabel;
	public UILabel moneyTypeLabel;

    /// <summary>
    /// 市场上架
    /// </summary>
    public UIButton putawayButton;
    /// <summary>
    /// 关闭市场上架
    /// </summary>
    public UIButton putawayOffButton;
    public GameObject putawayObj;

	void Awake()
	{
		if (CloseBtn != null)UIEventListener.Get (CloseBtn).onClick = BtnClose;
        if (putawayOffButton != null) UIEventListener.Get(putawayOffButton.gameObject).onClick += OffPutaway;
        if (putawayButton != null) UIEventListener.Get(putawayButton.gameObject).onClick += OnPutaway;
		mutualExclusion = true;
		if(noneMyMarketItem!=null)
		noneMyMarketItem.SetActive (false);
	}
	protected override void OnOpen()
	{
		base.OnOpen();
		if (marketBtn != null)UIEventListener.Get (marketBtn).onClick += OnClickMarket;
		if (limitBackObj != null)UIEventListener.Get (limitBackObj).onClick += OpenClickBack;
		if (lvLimit != null)UIEventListener.Get (lvLimit).onClick += OnLvLimitObj;
		if (qualityLimit != null)UIEventListener.Get (qualityLimit).onClick += OnQualityLimitObj;
		if (priceLimit != null)UIEventListener.Get (priceLimit).onClick += OnPriceLimitObj;
		if (moneyTypeLimit != null)UIEventListener.Get (moneyTypeLimit).onClick += OnMoneyTypeLimitObj;
		if (MyMarketItemBtn != null)UIEventListener.Get (MyMarketItemBtn).onClick += OnClickMyMarket;
        GameCenter.marketMng.InitLimit(0);
        GameCenter.marketMng.C2S_AskMarketItem(GameCenter.marketMng.marketPage);
		RefreshAll ();
	}
	protected override void OnClose()
	{
		base.OnClose();
		if (marketBtn != null)UIEventListener.Get (marketBtn).onClick -= OnClickMarket;
        if (limitBackObj != null) UIEventListener.Get(limitBackObj).onClick -= OpenClickBack;
		if (lvLimit != null)UIEventListener.Get (lvLimit).onClick -= OnLvLimitObj;
		if (qualityLimit != null)UIEventListener.Get (qualityLimit).onClick -= OnQualityLimitObj;
		if (priceLimit != null)UIEventListener.Get (priceLimit).onClick -= OnPriceLimitObj;
		if (moneyTypeLimit != null)UIEventListener.Get (moneyTypeLimit).onClick -= OnMoneyTypeLimitObj;
		if (MyMarketItemBtn != null)UIEventListener.Get (MyMarketItemBtn).onClick -= OnClickMyMarket;
	}
	protected override void HandEvent(bool _bind)
	{
		base.HandEvent(_bind);
		if (_bind)
		{
		//	SwitchToSubWnd(SubGUIType.SUBBACKPACK);
            GameCenter.marketMng.OnChooseType += UpdateTypes;
            GameCenter.marketMng.OnLimitChange += RefreshLimit;
			GameCenter.marketMng.OnAuctionItem += AuctionItem;
			GameCenter.marketMng.OnMyMarketItemUpdate += RefreshMyItems;
			GameCenter.marketMng.OnMarketItemUpdate += RefreshItems;
            GameCenter.marketMng.OnMarketItemFeedBack += OnMarketItemFeedBack;
		}
		else
		{
            GameCenter.marketMng.OnChooseType -= UpdateTypes;
            GameCenter.marketMng.OnLimitChange -= RefreshLimit;
			GameCenter.marketMng.OnAuctionItem -= AuctionItem;
			GameCenter.marketMng.OnMyMarketItemUpdate -= RefreshMyItems;
            GameCenter.marketMng.OnMarketItemFeedBack -= OnMarketItemFeedBack;
            GameCenter.marketMng.OnMarketItemUpdate -= RefreshItems;
		}
	}
	void AuctionItem()
	{
        if (auctionObj != null)
        {
            auctionObj.SetActive(true);
            AuctionWnd wnd = auctionObj.GetComponent<AuctionWnd>();
            if (wnd != null)
                auctionObj.GetComponent<AuctionWnd>().Refresh();
        }
	}


    void UpdateTypes(int _id, bool _b)
    {
        if (MarketTypeContainer != null)
            MarketTypeContainer.UpdateItems(_id, _b);


    }

	void RefreshTypes()
	{
		DestroyType ();
		if(MarketTypeContainer!=null)
		MarketTypeContainer.RefreshItems(GameCenter.marketMng.MarketTypes);
	}

	void DestroyType()
	{
		if (typeGrid != null) {
            typeGrid.transform.DestroyChildren();
		}
	}

	void RefreshItems()
	{
		DestroyItem ();
		if (MarketItemContainer != null) {
			MarketItemContainer.isMyMarketItem = false;
			MarketItemContainer.RefreshItems (GameCenter.marketMng.MarketItems);
		}
        if (flipOver != null)
        {
            if (GameCenter.marketMng.RefreshScrollView)
            {
                UIScrollView ScrollView = flipOver.gameObject.GetComponent<UIScrollView>();
                if (ScrollView != null)
                {
                    ScrollView.ResetPosition();
                    flipOver.canFlip = true;
                }
            }
        }
	}
	void DestroyItem()
	{
        if (itemGrid != null)
        {
            itemGrid.transform.DestroyChildren();
        }
	}

	void RefreshMyItems()
	{
		DestroyMyItem ();
		if (MyMarketItemContainer != null) {
			MyMarketItemContainer.isMyMarketItem = true;
			if (GameCenter.marketMng.MyMarketItems.Count == 0) {
				noneMyMarketItem.SetActive (true);
				return;
			}
			MyMarketItemContainer.RefreshItems (GameCenter.marketMng.MyMarketItems);
			noneMyMarketItem.SetActive (false);
		}
	}
	void DestroyMyItem()
	{
		if (myItemGrid != null) {
            myItemGrid.transform.DestroyChildren();
		}
	}

	void RefreshLimit()
	{
		if(levLabel!=null)
		switch(GameCenter.marketMng.lev)
		{
		case 0:levLabel.text=ConfigMng.Instance.GetUItext(307);break;
        case 20: levLabel.text = ConfigMng.Instance.GetUItext(316); break;
        case 30: levLabel.text = ConfigMng.Instance.GetUItext(317); break;
        case 40: levLabel.text = ConfigMng.Instance.GetUItext(318); break;
        case 56: levLabel.text = ConfigMng.Instance.GetUItext(319); break;
        case 78: levLabel.text = ConfigMng.Instance.GetUItext(320); break;
        case 100: levLabel.text= ConfigMng.Instance.GetUItext(321); break;
        case 122: levLabel.text = ConfigMng.Instance.GetUItext(322); break;
        case 144: levLabel.text = ConfigMng.Instance.GetUItext(323); break;
        case 166: levLabel.text = ConfigMng.Instance.GetUItext(324); break;
		default: break;
		}
		if(qualityLabel!=null)
		switch(GameCenter.marketMng.quality)
		{
            case 0: qualityLabel.text = ConfigMng.Instance.GetUItext(307); break;
            case 2: qualityLabel.text = ConfigMng.Instance.GetUItext(312); break;
            case 3: qualityLabel.text = ConfigMng.Instance.GetUItext(313); break;
            case 4: qualityLabel.text = ConfigMng.Instance.GetUItext(314); break;
            case 5: qualityLabel.text = ConfigMng.Instance.GetUItext(315); break;
		default: break;
		}
		if(priceLabel!=null)
		switch(GameCenter.marketMng.price)
		{
            case 0: priceLabel.text = ConfigMng.Instance.GetUItext(307); break;
            case 1: priceLabel.text = ConfigMng.Instance.GetUItext(310); break;
            case 2: priceLabel.text = ConfigMng.Instance.GetUItext(311); break;
		default: break;
		}
		if(moneyTypeLabel!=null)
		switch(GameCenter.marketMng.moneyType)
		{
            case 0: moneyTypeLabel.text = ConfigMng.Instance.GetUItext(307); break;
            case 1: moneyTypeLabel.text = ConfigMng.Instance.GetUItext(308); break;
            case 2: moneyTypeLabel.text = ConfigMng.Instance.GetUItext(309); break;
		default: break;
		}



	}

    void OnMarketItemFeedBack()
    {
        if (GameCenter.marketMng.CurWnd == 1)
        {
            GameCenter.marketMng.C2S_AskMarketItem(GameCenter.marketMng.marketPage);
        }
        if (GameCenter.marketMng.CurWnd == 2)
        {
            GameCenter.marketMng.C2S_AskMyMarketItem();
        }

    }


	void RefreshAll()
	{


		RefreshTypes ();
		//RefreshItems ();
		//RefreshMyItems ();
		RefreshLimit ();
	}

	void BtnClose(GameObject go)
    {
		GameCenter.uIMng.SwitchToUI(GUIType.NONE);
		GameCenter.marketMng.InitMarketTypeInfo();
	}

    /// <summary>
    /// 变为市场上架物品
    /// </summary>
    /// <param name="obj">Object.</param>
    public void OnPutaway(GameObject obj)
    {
        GameCenter.inventoryMng.OpenBackpack(ItemShowUIType.MARKETBAG);
        if (putawayObj != null) putawayObj.transform.position = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// 取消上架物品
    /// </summary>
    /// <param name="obj">Object.</param>
    public void OffPutaway(GameObject obj)
    {
        GameCenter.uIMng.ReleaseGUI(GUIType.BACKPACKWND);
        if (putawayObj != null) putawayObj.transform.position = new Vector3(0, 700, 0);
    }

	void OnLvLimitObj(GameObject obj)
	{
		lvLimitObj.SetActive (!lvLimitObj.activeSelf);
		if (limitBackObj != null)
			limitBackObj.SetActive (true);
	}
	void OnQualityLimitObj(GameObject obj)
	{
		qualityLimitObj.SetActive (!qualityLimitObj.activeSelf);
		if (limitBackObj != null)
			limitBackObj.SetActive (true);	
	}
	void OnPriceLimitObj(GameObject obj)
	{
		priceLimitObj.SetActive (!priceLimitObj.activeSelf);
		if (limitBackObj != null)
			limitBackObj.SetActive (true);
	}
	void OnMoneyTypeLimitObj(GameObject obj)
	{
		moneyTypeLimitObj.SetActive (!moneyTypeLimitObj.activeSelf);
		if (limitBackObj != null)
			limitBackObj.SetActive (true);	
	}
	void OpenClickBack(GameObject obj)
	{
		moneyTypeLimitObj.SetActive (false);
		priceLimitObj.SetActive (false);
		qualityLimitObj.SetActive (false);
		lvLimitObj.SetActive (false);
		limitBackObj.SetActive (false);
	}
		
	void OnClickMarket(GameObject obj)
	{
		if(marketObj!=null)marketObj.SetActive (true);
		if(MyMarketItemObj!=null)MyMarketItemObj.SetActive (false);
        GameCenter.marketMng.C2S_AskMarketItem(GameCenter.marketMng.marketPage);
		//DestroyItem ();
        GameCenter.marketMng.CurWnd = 1;
	}
	void OnClickMyMarket(GameObject obj)
	{
		if(marketObj!=null)marketObj.SetActive (false);
		if(MyMarketItemObj!=null)MyMarketItemObj.SetActive (true);
		GameCenter.marketMng.C2S_AskMyMarketItem ();
		DestroyMyItem ();
        GameCenter.marketMng.CurWnd = 2;
	}


}
