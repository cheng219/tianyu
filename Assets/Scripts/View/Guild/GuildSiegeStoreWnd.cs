//===============================
//作者：邓成
//日期：2016/5/18
//用途：攻城战城内商店界面类
//===============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class GuildSiegeStoreWnd : SubWnd {
	public UIToggle[] toggleSort;
	public UILabel labGold;
	public UILabel labTicket;
    public UILabel labBindGold;
	public UIButton btnCharge;
	public UIPanel panelStore;
	public UIButton btnClose;
	public Vector4 positionInfo;

	protected Dictionary<int,ShopItemUI> allItemList = new Dictionary<int, ShopItemUI>();
	protected List<CityShopRef> CurShowItems = new List<CityShopRef>();
	void Awake()
	{
		if(btnClose != null)UIEventListener.Get(btnClose.gameObject).onClick = CloseWnd;
        if (btnCharge != null) UIEventListener.Get(btnCharge.gameObject).onClick = OpenRechargeWnd;
		if(toggleSort != null)
		{
			for (int i = 0,max=toggleSort.Length; i < max; i++) {
                EventDelegate.Remove(toggleSort[i].onChange, FilterItem);
				EventDelegate.Add(toggleSort[i].onChange,FilterItem);
			}
		}
	}
	protected override void OnOpen ()
	{
		base.OnOpen ();
		ShowMainPlayerInfo();
		GameCenter.activityMng.C2S_ReqGuildSiegeStoreInfo();
	}
	protected override void OnClose ()
	{
		base.OnClose ();
		GameCenter.buyMng.restrictionNum = 0;//关闭城战商店,限购数量重置
	}
	protected override void HandEvent (bool _bind)
	{
		base.HandEvent (_bind);
		if(_bind)
		{
			GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += ShowMainPlayerInfo;
			GameCenter.activityMng.OnGotGuildCityStoreListEvent += FilterItem;
		}else
		{
			GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= ShowMainPlayerInfo;
			GameCenter.activityMng.OnGotGuildCityStoreListEvent -= FilterItem;
		}
	}
	void FilterItem()
	{
		int type = 0;
		for (int i = 0,max=toggleSort.Length; i < max; i++) {
			if(toggleSort[i].value)
				type = i+1;
		}
		CurShowItems = ConfigMng.Instance.GetCityShopRefLisyByPage(type);
		ShowStoreItem();
	}
	void ShowStoreItem()
	{
		HideItems();
		List<CityShopRef> rankList = CurShowItems;
		for (int i = 0,max=rankList.Count; i < max; i++) {
			ShopItemUI item = null;
			if(!allItemList.TryGetValue(i,out item))
			{
				GameObject go = Instantiate(exResources.GetResource(ResourceType.GUI,"GuildActivity/CityShopItem")) as GameObject;
				if(go != null)
				{
					item = go.GetComponent<ShopItemUI>();
					if(panelStore != null)go.transform.parent = panelStore.transform;
					go.transform.localScale = Vector3.one;
				}
                go = null;
				allItemList[i] = item;
			}
			item = allItemList[i];
			if(item != null)
			{
				item.gameObject.SetActive(true);
				item.transform.localPosition = new Vector3(positionInfo.x+positionInfo.z*(i%3),positionInfo.y+positionInfo.w*(i/3),0f);
				item.FillInfo(rankList[i]);
			}
		}
	}
	void HideItems()
	{
		foreach (var item in allItemList.Keys) {
			if(allItemList[item] != null)allItemList[item].gameObject.SetActive(false);
		}
	}
	void ShowMainPlayerInfo(ActorBaseTag tag,ulong val,bool state)
	{
		if(tag == ActorBaseTag.Diamond || tag == ActorBaseTag.BindDiamond || tag == ActorBaseTag.REALYUAN)
		{
            ShowMainPlayerInfo();
		}
	}
	void ShowMainPlayerInfo()
	{
        if (labTicket != null) labTicket.text = GameCenter.mainPlayerMng.MainPlayerInfo.RealYuanCount.ToString();
		if(labGold != null)labGold.text = GameCenter.mainPlayerMng.MainPlayerInfo.DiamondCountText;
        if (labBindGold != null) labBindGold.text = GameCenter.mainPlayerMng.MainPlayerInfo.BindDiamondCountText;
	}
	void CloseWnd(GameObject go)
	{
	//	GameCenter.uIMng.SwitchToSubUI(SubGUIType.NONE);
		CloseUI();
	}

    void OpenRechargeWnd(GameObject _go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
    }
}
