//==================================
//作者：黄洪兴
//日期：2016/4/1
//用途：物品购买界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuyWnd : GUIBase {


	public GameObject closeBtn;
	public GameObject add;
	public GameObject cut;
	public GameObject addTen;
	public GameObject buyBtn;
	public  ItemUI itemUI;
	public UILabel itemName;
	public UILabel itemDes;
	public UILabel itemPrice;
	public UISprite itemPriceIcon;	
	public UIInput itemNum;
	public UILabel allItemPrice;
	public UISprite allItemPriceIcon;
	private int buyType;
	//MainPlayerInfo mainPlayerInfo = null;
	int num;
	void Awake()
	{

	}
	protected override void OnOpen()
	{
		base.OnOpen();
		num=1;
        if (itemNum != null) itemNum.value = num.ToString();
		if (GameCenter.buyMng.curEq != null)
			itemUI.FillInfo(GameCenter.buyMng.curEq);
		RefreshNum();
	}
	protected override void OnClose()
	{
		base.OnClose();
	}
	protected override void HandEvent(bool _bind)
	{
		base.HandEvent(_bind);
		if (_bind)
		{
			if(closeBtn!=null)	UIEventListener.Get(closeBtn).onClick += CloseThis;
			if(add!=null) UIEventListener.Get(add).onClick += AddNum;
			if(cut!=null) UIEventListener.Get(cut).onClick += CutNum;
			if(addTen!=null) UIEventListener.Get(addTen).onClick += AddTenNum;
			if(buyBtn!=null) UIEventListener.Get(buyBtn).onClick += BuyItem;
            if (itemNum != null) EventDelegate.Add(itemNum.onChange, LimitNum);
		}
		else
		{
			if(closeBtn!=null)	UIEventListener.Get(closeBtn).onClick -= CloseThis;
			if(add!=null) UIEventListener.Get(add).onClick -= AddNum;
			if(cut!=null) UIEventListener.Get(cut).onClick -= CutNum;
			if(addTen!=null) UIEventListener.Get(addTen).onClick -= AddTenNum;
			if(buyBtn!=null) UIEventListener.Get(buyBtn).onClick -= BuyItem;
            if (itemNum != null) EventDelegate.Remove(itemNum.onChange, LimitNum);

		}
	}

	/// <summary>
	/// 刷新数量
	/// </summary>
	void RefreshNum()
	{
		if(itemNum!=null)itemNum.value = num.ToString ();
		if (itemName != null)
			itemName.text = GameCenter.buyMng.curEq.ItemName;
        if (itemPrice != null)
            itemPrice.text = GameCenter.buyMng.CurPrice.ToString();
		if (allItemPrice != null)
			allItemPrice.text = (num * GameCenter.buyMng.CurPrice).ToString();
        if (itemDes != null)
            itemDes.text = GameCenter.buyMng.curEq.Description.Replace("\\n", "\n"); ;
		if (itemPriceIcon != null&&ConfigMng.Instance.GetEquipmentRef (GameCenter.buyMng.priceType)!=null)
			itemPriceIcon.spriteName = GameHelper.GetCoinIconByType(GameCenter.buyMng.priceType);
		if (allItemPriceIcon != null&&ConfigMng.Instance.GetEquipmentRef (GameCenter.buyMng.priceType)!=null)
			allItemPriceIcon.spriteName = GameHelper.GetCoinIconByType(GameCenter.buyMng.priceType);
        
	}
	void CloseThis(GameObject obj)
	{
		GameCenter.uIMng.CloseGUI (GUIType.BUYWND);	
	}
	void AddNum(GameObject obj)
	{
		num++;
        if (GameCenter.buyMng.restrictionNum > 0 && num > GameCenter.buyMng.restrictionNum)
        {
            num = GameCenter.buyMng.restrictionNum;
        }

		RefreshNum();
	}
	void CutNum(GameObject obj)
	{
		if (num > 1) {
			num--;
		} else {
			num = 1;
		}
        if (GameCenter.buyMng.restrictionNum > 0 && num > GameCenter.buyMng.restrictionNum)
        {
            num = GameCenter.buyMng.restrictionNum;
        }
		RefreshNum ();
	}
	void AddTenNum(GameObject obj)
	{
		num=num+10;
        if (GameCenter.buyMng.restrictionNum > 0 && num > GameCenter.buyMng.restrictionNum)
        {
            num = GameCenter.buyMng.restrictionNum;
        }
       // Debug.Log("限购数量" + GameCenter.buyMng.restrictionNum+":"+num);
		RefreshNum ();
	}
	void BuyItem(GameObject obj)
	{
		//Debug.Log ("发送购买协议");
	    if(itemNum!=null)
        {
            int.TryParse(itemNum.value, out num);
        }
		if(GameCenter.buyMng.buyType == BuyType.CITYSHOP)
		{
			GameCenter.activityMng.C2S_BuyGuildSiegeStoreItem(GameCenter.buyMng.id,num);
		}
        else if (GameCenter.buyMng.buyType==BuyType.OPENSERVER)
        {
            GameCenter.openServerRewardMng.C2S_AskBuyOpenServerReward(GameCenter.buyMng.id, num);
        }
        else
		{
			GameCenter.buyMng.C2S_AskBuyItem (num);
		}
		GameCenter.uIMng.CloseGUI (GUIType.BUYWND);	
	}


    void LimitNum()
    {
        if (itemNum != null)
        {
            int limitNum=0;
            if (int.TryParse(itemNum.value, out limitNum))
            {
                if (limitNum <= 0)
                {
                    itemNum.value = "1";
				}else if(GameCenter.buyMng.restrictionNum > 0 && limitNum >= GameCenter.buyMng.restrictionNum)
				{
					itemNum.value = GameCenter.buyMng.restrictionNum.ToString();
				}else if (limitNum > 9999)
                {
                    itemNum.value = "9999";
                }
            }
            else
            {
                itemNum.value = "1";
            }

			int writeNum = 0;
			if(int.TryParse(itemNum.value,out writeNum))
			{
				num = writeNum;
			}
			if (allItemPrice != null)
				allItemPrice.text = (num * GameCenter.buyMng.CurPrice).ToString();
        }

    }

}
