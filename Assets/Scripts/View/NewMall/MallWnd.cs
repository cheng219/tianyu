//==================================
//作者：黄洪兴
//日期：2015/11/20
//用途：商店主界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MallWnd : GUIBase {


	public GameObject grid;
	/// <summary>
	/// 分类页签
	/// </summary>
	public UIToggle[] toggles = new UIToggle[6];
	/// <summary>
	/// 商品
	/// </summary>
	public MallItemContainer ItemContainer;
	public GameObject CloseBtn;
	public UILabel coin;
	public UILabel otherCoin;
	public UIScrollView uicroll;
	private MallItemType curMallType;
    public GameObject GoRechargeWndBtn;


	//MainPlayerInfo mainPlayerInfo = null;

	void Awake()
	{
		mutualExclusion = true;
		layer = GUIZLayer.TOPWINDOW;
	}
	void Start()
	{

	}
	protected UIToggle lastChangeToggle = null;
	protected void ClickToggleEvent(GameObject go)
	{
		UIToggle toggle = go.GetComponent<UIToggle>();
		if(toggle != lastChangeToggle)
		{
			OpenWndByType();
		}
		if(toggle != null && toggle.value)lastChangeToggle = toggle;
	}
	protected override void OnOpen()
	{
		base.OnOpen(); 
        if (CloseBtn != null) UIEventListener.Get(CloseBtn).onClick = BtnClose;
        toggles[0].value = false;
        toggles[(int)GameCenter.newMallMng.CurMallType].value = true;
        for (int i = 0; i < toggles.Length; i++)
        {
            //EventDelegate.Add(toggles[i].onChange, OpenWndByType);
            if (toggles[i] != null) UIEventListener.Get(toggles[i].gameObject).onClick = ClickToggleEvent;
        }
        GameCenter.newMallMng.ClassifyMallItem();
        //toggles[(int)GameCenter.newMallMng.CurMallType].value = true;
		ClickToggleEvent(toggles[(int)GameCenter.newMallMng.CurMallType].gameObject);
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
			GameCenter.newMallMng.OnGetBuyNums += RefreshCurTypeWnd;
            if (GoRechargeWndBtn != null) UIEventListener.Get(GoRechargeWndBtn).onClick += GoRechargeWnd;
            GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += RefreshCoin;
		}
		else
		{
			GameCenter.newMallMng.OnGetBuyNums -= RefreshCurTypeWnd;
            if (GoRechargeWndBtn != null) UIEventListener.Get(GoRechargeWndBtn).onClick -= GoRechargeWnd;
            GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= RefreshCoin;
		}
	}
	void OpenWndByType()
	{
		MallItemType _type = GameCenter.newMallMng.CurMallType;
		for (int i = 0; i < toggles.Length; i++)
		{
			if (toggles[i].value)
			{
				_type = (MallItemType)(i);
				break;
			}
		}
		RefreshItems (_type);

	}



	void RefreshItems(MallItemType _type)
	{
		//Debug.Log ("进入选择TYPE为"+(int)_type);
		curMallType = _type;
        DestroyItem();
		switch ((int)_type) {
		case 0:ItemContainer.RefreshItems(GameCenter.newMallMng.RestrictionMallItemDic);break;
		case 1:ItemContainer.RefreshItems(GameCenter.newMallMng.StrengthenMallItemDic);break;
		case 2:ItemContainer.RefreshItems(GameCenter.newMallMng.PetMallItemDic);break;
		case 3:ItemContainer.RefreshItems(GameCenter.newMallMng.DailyMallItemDic);break;
		case 4:ItemContainer.RefreshItems(GameCenter.newMallMng.FashionMallItemDic);break;
		case 5:ItemContainer.RefreshItems(GameCenter.newMallMng.CashMallItemDic);break;
		default:	break;
		}
        RefreshCoin();
		if (uicroll != null)
			uicroll.ResetPosition ();

	}

	/// <summary>
	/// 刷新当前页面
	/// </summary>
	void RefreshCurTypeWnd()
	{
        DestroyItem();
		switch ((int)curMallType) {
		case 0:ItemContainer.RefreshItems(GameCenter.newMallMng.RestrictionMallItemDic);break;
		case 1:ItemContainer.RefreshItems(GameCenter.newMallMng.StrengthenMallItemDic);break;
		case 2:ItemContainer.RefreshItems(GameCenter.newMallMng.PetMallItemDic);break;
		case 3:ItemContainer.RefreshItems(GameCenter.newMallMng.DailyMallItemDic);break;
		case 4:ItemContainer.RefreshItems(GameCenter.newMallMng.FashionMallItemDic);break;
		case 5:ItemContainer.RefreshItems(GameCenter.newMallMng.CashMallItemDic);break;
		default:	break;
		}
        RefreshCoin();
	}

    void RefreshCoin()
    {
        if (coin != null)
            coin.text = GameCenter.mainPlayerMng.MainPlayerInfo.DiamondCountText;
        if (otherCoin != null)
            otherCoin.text = GameCenter.mainPlayerMng.MainPlayerInfo.BindDiamondCountText;
    }

    void RefreshCoin(ActorBaseTag _tag,ulong _val,bool _state)
    {
        if (_tag == ActorBaseTag.Diamond || _tag == ActorBaseTag.BindDiamond)
        {
            RefreshCoin();
        }
    }

	void DestroyItem()
	{
        if (grid != null)
        {
            grid.transform.DestroyChildren();
        }
	}

	void BtnClose(GameObject go){
		UIMng uiMng = GameCenter.uIMng;
		if(uiMng.CurOpenType == GUIType.NEWMALL)
		{
			uiMng.SwitchToUI(GUIType.NONE); 
		}else
		{
			uiMng.CloseGUI(GUIType.NEWMALL);
		}
	}



    void GoRechargeWnd(GameObject obj)
	{
        GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
		//	GameCenter.buyMng.OpenBuyWnd (GUIType.SHOPWND);	
	}


}
