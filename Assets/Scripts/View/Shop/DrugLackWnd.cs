//==================================
//作者：黄洪兴
//日期：2016/9/21
//用途：药品不足界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrugLackWnd : GUIBase
{

    public ItemUI item;
    public ItemUI itemTwo;
    public UILabel itemName;

    public GameObject CloseBtn;
    public GameObject goShopBtn;
    public GameObject goRechargeBtn;
    public GameObject OpenAutoUseBtn;
    //MainPlayerInfo mainPlayerInfo = null;

    void Awake()
    {
        if (CloseBtn != null)
            UIEventListener.Get(CloseBtn).onClick = delegate { BtnClose(); };
        mutualExclusion = true;
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        if (goShopBtn != null)
            UIEventListener.Get(goShopBtn).onClick += GoShopWnd;
        if (goRechargeBtn != null)
            UIEventListener.Get(goRechargeBtn).onClick += GoRechargeWnd;
        if (OpenAutoUseBtn != null)
            UIEventListener.Get(OpenAutoUseBtn).onClick += OpenAutoUseItem;
        Refresh();


    }
    protected override void OnClose()
    {
        base.OnClose();
        if (goShopBtn != null)
            UIEventListener.Get(goShopBtn).onClick -= GoShopWnd;
        if (goRechargeBtn != null)
            UIEventListener.Get(goRechargeBtn).onClick -= GoRechargeWnd;
        if (OpenAutoUseBtn != null)
            UIEventListener.Get(OpenAutoUseBtn).onClick -= OpenAutoUseItem;
        GameCenter.shopMng.CurLackHPItemInfo = null;
        GameCenter.shopMng.CurLackMPItemInfo = null;
        GameCenter.shopMng.ShowedHPDrugBtn = false;
        GameCenter.shopMng.ShowedMPDrugBtn = false;

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










    void Refresh()
    {
        if (GameCenter.shopMng.CurLackHPItemInfo == null && GameCenter.shopMng.CurLackMPItemInfo == null)
        {
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
            return;
        }
        if (GameCenter.shopMng.CurLackHPItemInfo != null && GameCenter.shopMng.CurLackMPItemInfo == null)
        {
            if (item != null)
            {
                item.FillInfo(GameCenter.shopMng.CurLackHPItemInfo);
                item.gameObject.transform.localPosition = new Vector3(20,50);
                item.gameObject.SetActive(true);
            }
            if (itemTwo != null)
                itemTwo.gameObject.SetActive(false);
            if (itemName != null)
            {
                itemName.text = "[00ff00]生命药水[-]";
            }
        }
        else if (GameCenter.shopMng.CurLackHPItemInfo == null && GameCenter.shopMng.CurLackMPItemInfo != null)
        {

            if (item != null)
            {
                item.FillInfo(GameCenter.shopMng.CurLackMPItemInfo);
                item.gameObject.transform.localPosition = new Vector3(20, 50);
                item.gameObject.SetActive(true);
            }
            if (itemTwo != null)
                itemTwo.gameObject.SetActive(false);
            if (itemName != null)
            {
                itemName.text = "[00ff00]魔法药水[-]";
            }

        }
        else if (GameCenter.shopMng.CurLackHPItemInfo != null && GameCenter.shopMng.CurLackMPItemInfo != null)
        {
            if (item != null)
            {
                item.FillInfo(GameCenter.shopMng.CurLackHPItemInfo);
                item.gameObject.transform.localPosition = new Vector3(-14, 50);
                item.gameObject.SetActive(true);
            }
            if (itemTwo != null)
            {
                itemTwo.FillInfo(GameCenter.shopMng.CurLackMPItemInfo);
                itemTwo.gameObject.transform.localPosition = new Vector3(80, 50);
                itemTwo.gameObject.SetActive(true);
            }
            if (itemName != null)
            {
                itemName.text = "[00ff00]生命药水[-], [00ff00]魔法药水[-]";
            }

        }


        if (GameCenter.vipMng.VipData.vLev < 1)
        {
            if (goRechargeBtn != null)
                goRechargeBtn.SetActive(true);
            if (OpenAutoUseBtn != null)
                OpenAutoUseBtn.SetActive(false);

        }
        else
        {
            if (goRechargeBtn != null)
                goRechargeBtn.SetActive(false);
            if (OpenAutoUseBtn != null)
                OpenAutoUseBtn.SetActive(true);
        }





    }


    void GoShopWnd(GameObject _go)
    {
        BtnClose();
		GameCenter.uIMng.SwitchToUI(GUIType.SHOPWND);
    }

    void GoRechargeWnd(GameObject _go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
    }


    void OpenAutoUseItem(GameObject _go)
    {
        if (GameCenter.vipMng.VipData.vLev > 0)
        GameCenter.systemSettingMng.IsAutoBuy = true;
        BtnClose();
    }
    void BtnClose()
    {
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
    }





}
