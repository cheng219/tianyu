//==================================
//作者：黄洪兴
//日期：2016/4/12
//用途：仙盟商店主界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuildShopWnd : GUIBase {


	public GameObject grid;
	public UIScrollView uicroll;

	public GameObject sellBtn;
    /// <summary>
    /// 贡献
    /// </summary>
    public UILabel contribute;

	/// <summary>
	/// 商品
	/// </summary>
	public GuildShopItemContainer ItemContainer;
	public GameObject CloseBtn;

	void Awake()
	{
        if (CloseBtn != null) UIEventListener.Get(CloseBtn).onClick = BtnClose;
		mutualExclusion = true;
	}
	protected override void OnOpen()
	{
		base.OnOpen();
		RefreshItems ();
        GameCenter.inventoryMng.OpenBackpack(ItemShowUIType.GUILDSHOPBAG);
	}
	protected override void OnClose()
	{
		base.OnClose();
        GameCenter.uIMng.ReleaseGUI(GUIType.BACKPACKWND);
	}
	protected override void HandEvent(bool _bind)
	{
		base.HandEvent(_bind);
		if (_bind)
		{
			GameCenter.guildShopMng.OnItemBuyedNumUpdate += RefreshItems;
            GameCenter.mainPlayerMng.OnBaseValueChange += RefreshContribute;
		}
		else
		{
			GameCenter.guildShopMng.OnItemBuyedNumUpdate -= RefreshItems;
            GameCenter.mainPlayerMng.OnBaseValueChange -= RefreshContribute;
		}
	}


    void RefreshContribute()
    {
        if (contribute != null)
            contribute.text = GameCenter.mainPlayerMng.MainPlayerInfo.GuildContribution.ToString();
    }


	void RefreshItems()
	{
        DestroyItem();
        if (contribute != null)
            contribute.text = GameCenter.mainPlayerMng.MainPlayerInfo.GuildContribution.ToString();
		ItemContainer.RefreshItems(GameCenter.guildShopMng.GuildShopItemDic);
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
        if (GameCenter.guildMng.NeedOpenGuildWnd)
            GameCenter.uIMng.SwitchToUI(GUIType.GUILDMAIN);
        else
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
	}



	void SellItem(GameObject obj)
	{
		//	GameCenter.buyMng.OpenBuyWnd (GUIType.SHOPWND);	
	}


}
