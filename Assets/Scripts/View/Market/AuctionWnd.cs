//==============================================
//作者：黄洪兴
//日期：2016/4/27
//用途：拍卖购买界面
//=================================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AuctionWnd : SubWnd
{
	#region UI控件对象

	/// <summary>
	/// 关闭按钮
	/// </summary>
	public GameObject CloseButton;
    public UILabel itemDes;
	public ItemUI itemUI;
	public UILabel itemName;
	public UIInput nunInput;
	public GameObject auctionBtn;
    public UILabel price;
    public UILabel total;
    public GameObject addNumBtn;
    public GameObject cutNumBtn;
    public GameObject addTenNumBtn;
    public UISprite priceSprite;
    public UISprite priceSpriteTwo;


    private int num;
	#endregion
	void Awake()
	{
        if (addTenNumBtn != null) UIEventListener.Get(addTenNumBtn).onClick -= AddTenNum;
        if (addNumBtn != null) UIEventListener.Get(addNumBtn).onClick -= AddNum;
        if (cutNumBtn != null) UIEventListener.Get(cutNumBtn).onClick -= CutNum;
        if (CloseButton != null) UIEventListener.Get(CloseButton).onClick -= OnCloseWnd;
        if (auctionBtn != null) UIEventListener.Get(auctionBtn).onClick -= OnAuction;
        if (addTenNumBtn != null) UIEventListener.Get(addTenNumBtn).onClick += AddTenNum;
        if (addNumBtn != null) UIEventListener.Get(addNumBtn).onClick += AddNum;
        if (cutNumBtn != null) UIEventListener.Get(cutNumBtn).onClick += CutNum;
        if (CloseButton != null) UIEventListener.Get(CloseButton).onClick += OnCloseWnd;
        if (auctionBtn != null) UIEventListener.Get(auctionBtn).onClick += OnAuction;
        Refresh();

	}
	protected override void OnOpen()
	{
		base.OnOpen();



	}
	protected override void OnClose ()
	{
		base.OnClose ();
	}
	protected override void HandEvent(bool _bind)
	{
		base.HandEvent(_bind);
		if (_bind)
		{
			
		}
		else
		{
	

		}
	}
    void Update()
    {

        if (nunInput != null && nunInput.value != null && nunInput.value != string.Empty)
        {
            if (Convert.ToInt32(nunInput.value) > GameCenter.marketMng.CurAuctionItem.Nums)
            {
                nunInput.value = GameCenter.marketMng.CurAuctionItem.Nums.ToString();
            }
            if(total!=null)
            {
                total.text = (Convert.ToInt32(nunInput.value) * GameCenter.marketMng.CurAuctionItem.Price).ToString();
            }
        }


    }
		

	public  void Refresh()
	{
        if (itemDes != null) itemDes.text = GameCenter.marketMng.CurAuctionItem.EquipmentInfo.Description;
		if (itemUI != null)
			itemUI.FillInfo (GameCenter.marketMng.CurAuctionItem.EquipmentInfo);
        if (itemName != null) itemName.text = GameCenter.marketMng.CurAuctionItem.EquipmentInfo.ItemName;
        if (nunInput != null) nunInput.value = GameCenter.marketMng.CurAuctionItem.Nums.ToString();
        if (price != null) price.text = GameCenter.marketMng.CurAuctionItem.Price.ToString();
        if (total != null) total.text = (GameCenter.marketMng.CurAuctionItem.Price * GameCenter.marketMng.CurAuctionItem.Nums).ToString();
        if (priceSprite != null && priceSpriteTwo!=null)
        {
            if (GameCenter.marketMng.CurAuctionItem.PriceType == 1)
            {
                priceSprite.spriteName = "Icon_tongbi";
                priceSpriteTwo.spriteName = "Icon_tongbi";
            }
            else
            {
                priceSprite.spriteName = "Icon_gold";
                priceSpriteTwo.spriteName = "Icon_gold";
            }
        }
	}


    void AddTenNum(GameObject _obj)
    {
        num = Convert.ToInt32(nunInput.value);
        num+=10;
        if (num > GameCenter.marketMng.CurAuctionItem.Nums)
        {
            num = GameCenter.marketMng.CurAuctionItem.Nums;
        }
        nunInput.value = num.ToString();
    }



    void AddNum(GameObject _obj)
    {
        num = Convert.ToInt32(nunInput.value);
        num++;
        if (num > GameCenter.marketMng.CurAuctionItem.Nums)
        {
            num = GameCenter.marketMng.CurAuctionItem.Nums;
        }
        nunInput.value = num.ToString();
    }

    void CutNum(GameObject _obj)
    {
        num = Convert.ToInt32(nunInput.value);
        num--;
        if (num <=0 )
        {
            num = 1;
        }
        nunInput.value = num.ToString();
    }

	void OnCloseWnd(GameObject obj)
	{

		this.gameObject.SetActive (false);

	}

	void OnAuction(GameObject obj)
	{

        GameCenter.marketMng.C2S_AuctionMarketItem(GameCenter.marketMng.CurAuctionItem.EID,Convert.ToInt32(nunInput.value));
		this.gameObject.SetActive (false);
	}

}
