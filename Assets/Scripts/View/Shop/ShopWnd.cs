//==================================
//作者：黄洪兴
//日期：2016/3/24
//用途：商店主界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopWnd : GUIBase {

    public UILabel resNum;
    public UILabel targeName;
    public UILabel resName;
	public GameObject grid;
	public UIScrollView uicroll;

	public GameObject sellBtn;


    public GameObject GoWndBtn;
    public UILabel GoWndBtnName;
	//public BackPackSubWnd backpackwnd;
    /// <summary>
    /// 批量出售选择
    /// </summary>
    public UIButton batchSellButton;
    /// <summary>
    /// 批量出售选择取消
    /// </summary>
    public UIButton batchSellOffButton;
    /// <summary>
    /// 确定批量出售
    /// </summary>
    public UIButton batchSellOKButton;

	/// <summary>
	/// 分类页签
	/// </summary>
	public UIToggle[] toggles = new UIToggle[7];
	/// <summary>
	/// 商品
	/// </summary>
	public ShopItemContainer ItemContainer;
	public GameObject CloseBtn;

	private ShopItemType curShopItemType;
	//MainPlayerInfo mainPlayerInfo = null;

	void Awake()
	{
		if (CloseBtn != null)UIEventListener.Get (CloseBtn).onClick = BtnClose;
        if (batchSellOKButton != null) UIEventListener.Get(batchSellOKButton.gameObject).onClick += BatchSellOK;
        if (batchSellButton != null) UIEventListener.Get(batchSellButton.gameObject).onClick += BatchSell;
        if (batchSellOffButton != null) UIEventListener.Get(batchSellOffButton.gameObject).onClick += BatchSellOff;
        if (sellBtn != null) UIEventListener.Get(sellBtn).onClick += SellItem;
        if (GoWndBtn != null) UIEventListener.Get(GoWndBtn).onClick += GoWnd;
		mutualExclusion = true;
	}
	protected override void OnOpen()
	{
		base.OnOpen();
        GameCenter.inventoryMng.OpenBackpack(ItemShowUIType.SHOPBAG);
        GameCenter.shopMng.ClassifyShopItem();
        toggles[0].value = false;
        toggles[(int)GameCenter.shopMng.CurType].value = true;
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i] != null) UIEventListener.Get(toggles[i].gameObject).onClick = ClickToggleEvent;
        }
        ClickToggleEvent(toggles[(int)GameCenter.shopMng.CurType].gameObject);
	}
	protected override void OnClose()
	{
		base.OnClose();
		//if(backpackwnd != null)backpackwnd.CloseUI();
        GameCenter.uIMng.ReleaseGUI(GUIType.BACKPACKWND);

        GameCenter.shopMng.CurType = ShopItemType.NORMAL;
        GameCenter.buyMng.SellEqList.Clear();
	}
	protected override void HandEvent(bool _bind)
	{
		base.HandEvent(_bind);
		if (_bind)
		{
			GameCenter.shopMng.OnRedeemUpdate += RefreshRedeemWnd;
            GameCenter.mainPlayerMng.OnBaseValueChange += RefeshRes;
            GameCenter.buyMng.OnSellEqChange += ShowSellBtn;
		}
		else
		{
			GameCenter.shopMng.OnRedeemUpdate -= RefreshRedeemWnd;
            GameCenter.mainPlayerMng.OnBaseValueChange -= RefeshRes;
            GameCenter.buyMng.OnSellEqChange -= ShowSellBtn;
		}
	}


    protected UIToggle lastChangeToggle = null;
    protected void ClickToggleEvent(GameObject go)
    {
        UIToggle toggle = go.GetComponent<UIToggle>();
        if (toggle != lastChangeToggle)
        {
            OpenWndByType();
        }
        if (toggle != null && toggle.value) lastChangeToggle = toggle;
    }


	void OpenWndByType()
	{
		ShopItemType _type = GameCenter.shopMng.CurType;
		for (int i = 0; i < toggles.Length; i++)
		{
			if (toggles[i].value)
			{
				_type = (ShopItemType)(i);
				break;
			}
		}
		RefreshItems (_type);

	}



    void RefeshRes()
    {
        int resNameID = 0;
        int targetNameID = 0;
        int resNumID = 0;
        string resTatolNum = string.Empty;
        bool GoWndBtnActive = false;
        switch (curShopItemType)
        {
            case ShopItemType.EXPLOIT:
                resNameID = 55;
                targetNameID = 58;
                resNumID = 61;
                resTatolNum = GameCenter.mainPlayerMng.MainPlayerInfo.Exploit.ToString();
                GoWndBtnActive = true;
                break;
            case ShopItemType.REPUTATION:
                resNameID = 56;
                targetNameID = 59;
                resNumID = 62;
                resTatolNum = GameCenter.mainPlayerMng.MainPlayerInfo.Repuutation.ToString();
                GoWndBtnActive = true;
                break;
            case ShopItemType.SCORES:
                resNameID = 57;
                targetNameID = 60;
                resNumID = 63;
                resTatolNum = GameCenter.mainPlayerMng.MainPlayerInfo.Integral.ToString();
                GoWndBtnActive = true;
                break;
            case ShopItemType.CASH:
                resNameID = 134;
                targetNameID = 135;
                resNumID = 136;
                resTatolNum = GameCenter.mainPlayerMng.MainPlayerInfo.RealYuanCount.ToString();
                GoWndBtnActive = true;
                break;
            default:
                resNameID = 0;
                targetNameID = 0;
                resNumID = 0;
                resTatolNum = string.Empty;
                GoWndBtnActive = false;
                break;
        }
        if (resName != null)
        {
            string st = ConfigMng.Instance.GetUItext(resNameID);
            if (st != null)
                resName.text = st;
        }
        if (targeName != null)
        {
            string st = ConfigMng.Instance.GetUItext(targetNameID);
            if (st != null)
                targeName.text = st;
        }
        if (resNum != null)
        {
            string st = ConfigMng.Instance.GetUItext(resNumID);
            if (st != null)
                resNum.text = st + resTatolNum.ToString();
        }
        if (GoWndBtn != null)
        {
            GoWndBtn.SetActive(GoWndBtnActive);
        }
    }


  void RefreshItems(ShopItemType _type)
	{
		curShopItemType = _type;
        DestroyItem();
		switch ((int)_type) {
            case 0: ItemContainer.RefreshItems(GameCenter.shopMng.RedeemShopItemDic); uicroll.ResetPosition(); break;
            case 1: ItemContainer.RefreshItems(GameCenter.shopMng.NormalShopItemDic); uicroll.ResetPosition(); break;
            case 2: ItemContainer.RefreshItems(GameCenter.shopMng.EquipmentShopItemDic); uicroll.ResetPosition(); break;
            case 3: ItemContainer.RefreshItems(GameCenter.shopMng.ExploitShopItemDic); uicroll.ResetPosition(); break;
            case 4: ItemContainer.RefreshItems(GameCenter.shopMng.ReputationShopItemDic); uicroll.ResetPosition(); break;
            case 5: ItemContainer.RefreshItems(GameCenter.shopMng.ScoresShopItemDic); uicroll.ResetPosition(); break;
            case 6: ItemContainer.RefreshItems(GameCenter.shopMng.CashShopItemDic); uicroll.ResetPosition(); break;
		default:	break;
		}

        RefeshRes();
	}

	void DestroyItem()
	{
		if (grid != null) 
        {
            grid.transform.DestroyChildren();
		}
	}



	void BtnClose(GameObject go)
	{
	//	GameCenter.uIMng.CloseGUI(GUIType.SHOPWND);
		UIMng uiMng = GameCenter.uIMng;
		if(uiMng.CurOpenType == GUIType.SHOPWND)
		{
			uiMng.SwitchToUI(GUIType.NONE); 
		}else
		{
			uiMng.CloseGUI(GUIType.SHOPWND);
		}
	}

	/// <summary>
	/// 跳转至购回界面
	/// </summary>
	void RefreshRedeemWnd()
	{
        lastChangeToggle = null;
		RefreshItems ((ShopItemType)0);
        toggles[0].value = true;
	}


	void SellItem(GameObject obj)
	{
	//	GameCenter.buyMng.OpenBuyWnd (GUIType.SHOPWND);	
	}

    void GoWnd(GameObject obj)
    {

        switch (curShopItemType)
        {
            case ShopItemType.REDEEM:
                break;
            case ShopItemType.NORMAL:
                break;
            case ShopItemType.EQUIPMENT:
                break;
            case ShopItemType.EXPLOIT:
                GameCenter.activityMng.OpenStartSeleteActivity(ActivityType.SEALOFTHE);
                break;
            case ShopItemType.REPUTATION:
                GameCenter.activityMng.OpenStartSeleteActivity(ActivityType.MONSTERSIEGE);
                break;
            case ShopItemType.SCORES:
                GameCenter.uIMng.SwitchToUI(GUIType.MagicTowerWnd);
                break;
            case ShopItemType.CASH:
                GameCenter.uIMng.SwitchToUI(GUIType.DAILYMUSTDO);
                break;
            default:
                break;
        }


        //	GameCenter.buyMng.OpenBuyWnd (GUIType.SHOPWND);	
    }

    /// <summary>
    /// 显示出售按钮
    /// </summary>
    protected void ShowSellBtn()
    {
        if (GameCenter.buyMng.SellEqList.Count > 0)
        {

            if (batchSellOKButton != null) batchSellOKButton.gameObject.SetActive(true);
            if (batchSellOffButton != null) batchSellOffButton.gameObject.SetActive(false);

        }
        else
        {
            if (batchSellOKButton != null) batchSellOKButton.gameObject.SetActive(false);
            if (batchSellOffButton != null) batchSellOffButton.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 确定批量出售
    /// </summary>
    /// <param name="obj">Object.</param>
    void BatchSellOK(GameObject obj)
    {
        GameCenter.buyMng.C2S_AskSellItem(GameCenter.buyMng.SellEqList);
        GameCenter.buyMng.BatchSell(false);
        if (batchSellOKButton != null) batchSellOKButton.gameObject.SetActive(false);
        if (batchSellOffButton != null) batchSellOffButton.gameObject.SetActive(false);
        if (batchSellButton != null) batchSellButton.gameObject.SetActive(true);
    }


    /// <summary>
    /// 点击批量出售按钮
    /// </summary>
    /// <param name="obj"></param>
    void BatchSell(GameObject obj)
    {
        GameCenter.buyMng.BatchSell(true);
        if (batchSellButton != null) batchSellButton.gameObject.SetActive(false);
        if (batchSellOffButton != null) batchSellOffButton.gameObject.SetActive(true);
    }
    /// <summary>
    /// 点击取消批量出售按钮
    /// </summary>
    /// <param name="obj"></param>
    void BatchSellOff(GameObject obj)
    {
        GameCenter.buyMng.BatchSell(false);
        if (batchSellOffButton != null) batchSellOffButton.gameObject.SetActive(false);
        if (batchSellButton != null) batchSellButton.gameObject.SetActive(true);
    }

}
