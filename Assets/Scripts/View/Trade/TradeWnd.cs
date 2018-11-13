//==================================
//作者：黄洪兴
//日期：2016/6/22
//用途：交易主界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TradeWnd : GUIBase
{
    public GameObject allLock;
    public GameObject CloseBtn;
    public GameObject LockBtn;
    public UISpriteEx LockBtnSprite;
    public GameObject TradeBtn;
    public UISpriteEx TradeBtnSprite;

    public GameObject OtherLock;
    public GameObject OtherTradeObj;
    public GameObject MyLock;
    public GameObject MyTradeObj;
    public UILabel OtherName;
    public UILabel MyName;
    public List<ItemUI> OtherItems = new List<ItemUI>();
    public List<ItemUI> MyItems = new List<ItemUI>();


    void Awake()
    {
        mutualExclusion = true;
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        GameCenter.inventoryMng.OpenBackpack(ItemShowUIType.TRADEBAG);
        if (CloseBtn != null)UIEventListener.Get(CloseBtn).onClick = BtnClose;
        if (LockBtn != null)UIEventListener.Get(LockBtn).onClick = BtnLock;
        if (TradeBtn != null)UIEventListener.Get(TradeBtn).onClick = BtnTrade;
        Refesh();
    }
    protected override void OnClose()
    {
        base.OnClose();
        GameCenter.uIMng.ReleaseGUI(GUIType.BACKPACKWND);
        GameCenter.tradeMng.ReSet();
    }
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            GameCenter.tradeMng.OnTradeItemUpdate += RefeshLock;
            GameCenter.tradeMng.OnTradeLockUpdate += ShowTradeBtn;
            GameCenter.tradeMng.OnTradeTargetReply += RefreshTradeObj;
        }
        else
        {
            GameCenter.tradeMng.OnTradeItemUpdate -= RefeshLock;
            GameCenter.tradeMng.OnTradeLockUpdate -= ShowTradeBtn;
            GameCenter.tradeMng.OnTradeTargetReply -= RefreshTradeObj;
        }
    }

    void Refesh()
    {
        if (OtherName != null)
            OtherName.text = GameCenter.tradeMng.TradeTargetName+ConfigMng.Instance.GetUItext(352);
        if (MyName != null)
            MyName.text = GameCenter.mainPlayerMng.MainPlayerInfo.Name + ConfigMng.Instance.GetUItext(352);
        if (MyLock != null)
        {
            MyLock.gameObject.SetActive(GameCenter.tradeMng.TradeMyLockState);
        }
        if (OtherLock != null)
        {
            OtherLock.gameObject.SetActive(GameCenter.tradeMng.TradeTargetLockState);
        }
        for (int i = 0,max = OtherItems.Count; i < max; i++)
        {
            OtherItems[i].FillInfo(null);
        }
        if (GameCenter.tradeMng.TradeTargetItems.Count > 0)
        {
            ItemUI item;
            for (int i = 0; i < GameCenter.tradeMng.TradeTargetItems.Count; i++)
            {
                item = OtherItems[i];
                if (item != null)
                    item.FillInfo(GameCenter.tradeMng.TradeTargetItems[i]);
            }
        }
        for (int i = 0; i < MyItems.Count; i++)
        {
            MyItems[i].FillInfo(null);

        }
        if (GameCenter.tradeMng.TradeMyItems.Count > 0)
        {
            ItemUI item;
            for (int i = 0; i < GameCenter.tradeMng.TradeMyItems.Count; i++)
            {
                item = MyItems[i];
                if (item != null)
                {
                    item.FillInfo(GameCenter.tradeMng.TradeMyItems[i]);
                    //item.SetActionBtn(ItemActionType.None, ItemActionType.None, ItemActionType.TAKEOUT);
                }
            }
        }

    }

    void RefeshLock(LockUpdateType _type)
    { 
        if (MyLock != null)
        {
            MyLock.gameObject.SetActive(GameCenter.tradeMng.TradeMyLockState);
        }
        if (OtherLock != null)
        {
            OtherLock.gameObject.SetActive(GameCenter.tradeMng.TradeTargetLockState);
        }
        if (GameCenter.tradeMng.TradeTargetItems.Count > 0)
        {
            ItemUI item;
            for (int i = 0; i < GameCenter.tradeMng.TradeTargetItems.Count; i++)
            {
                item = OtherItems[i];
                if (item != null)
                    item.FillInfo(GameCenter.tradeMng.TradeTargetItems[i]);
            }
        }
        if (GameCenter.tradeMng.TradeMyItems.Count > 0)
        {
            ItemUI item;
            for (int i = 0; i < GameCenter.tradeMng.TradeMyItems.Count; i++)
            {
                item = MyItems[i];
                if (item != null)
                {
                    item.FillInfo(GameCenter.tradeMng.TradeMyItems[i]);
                    //item.SetActionBtn(ItemActionType.None, ItemActionType.None, ItemActionType.TAKEOUT);
                }
            }
        }
        if (_type == LockUpdateType.UPDATEMY)
        {

        }

        if (_type == LockUpdateType.TAKEOUR)
        {
            Refesh();
        }

    }
    void BtnTrade(GameObject go)
    {
        GameCenter.tradeMng.ConfirmTrade();
        GameCenter.messageMng.AddClientMsg(327);
        if (TradeBtn != null)
        {
            BoxCollider box = TradeBtn.GetComponent<BoxCollider>();
            if (box != null)
                box.enabled = false;
        }
        if (TradeBtnSprite != null)
        {
            TradeBtnSprite.IsGray = UISpriteEx.ColorGray.Gray;
        }
        if (MyTradeObj != null)
            MyTradeObj.SetActive(true);
    }


    void BtnLock(GameObject go)
    {
        if (MyLock != null)
            MyLock.gameObject.SetActive(true);
        if (allLock != null)
            allLock.SetActive(true);
        if (LockBtn != null)
        {
            BoxCollider box = LockBtn.GetComponent<BoxCollider>();
            if (box != null)
                box.enabled = false;
        }
        if (LockBtnSprite != null)
        {
            LockBtnSprite.IsGray = UISpriteEx.ColorGray.Gray;
        }
        GameCenter.tradeMng.LockTrade();
    }

    void ShowTradeBtn()
    {
        if (GameCenter.tradeMng.TradeTargetLockState && GameCenter.tradeMng.TradeMyLockState)
        {
            if (TradeBtn != null)
            {
                BoxCollider box = TradeBtn.GetComponent<BoxCollider>();
                if (box != null)
                    box.enabled = true;
            }
            if (TradeBtnSprite != null)
            {
                TradeBtnSprite.IsGray = UISpriteEx.ColorGray.normal;
            }
        }
        if (OtherLock != null)
        {
            OtherLock.gameObject.SetActive(GameCenter.tradeMng.TradeTargetLockState);
        }

    }


    void RefreshTradeObj()
    {
        if (OtherTradeObj != null)
            OtherTradeObj.SetActive(true);
    }

    void BtnClose(GameObject go)
    {
        GameCenter.tradeMng.C2S_CancelTrade();
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
    }
     
}
