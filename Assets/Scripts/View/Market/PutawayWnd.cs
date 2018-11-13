//==================================
//作者：黄洪兴
//日期：2016/4/25
//用途：物品上架界面类
//=================================

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PutawayWnd : GUIBase {

	public  ItemUI itemUI;
	public UILabel itemName;
	public GameObject closeBtn;
	public GameObject putawayBtn;
	public GameObject cencelBtn;
	public UIToggle noticeToggle;
	public UIToggle coinToggle;
	public UIToggle silverToggel;
	public UIInput putawayNum;
	public UIInput putawayPrice;
	public UILabel putawayTotal;
	//MainPlayerInfo mainPlayerInfo = null;
	int num;
    EventDelegate edt;
	void Awake()
	{
  
	}
	protected override void OnOpen()
	{
		base.OnOpen();
		Refresh ();
        RefreshPrice();
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
            edt = new EventDelegate(this, "RefreshPrice");
			if(putawayBtn!=null)	UIEventListener.Get(putawayBtn).onClick += PutawayItem;
			if(closeBtn!=null)	UIEventListener.Get(closeBtn).onClick += CloseThis;
			if(cencelBtn!=null)	UIEventListener.Get(cencelBtn).onClick += CloseThis;
            if (putawayPrice != null) putawayPrice.onChange.Add(edt);
            if (putawayNum != null) putawayNum.onChange.Add(edt);
		}
		else
		{
			if(putawayBtn!=null)	UIEventListener.Get(putawayBtn).onClick -= PutawayItem;
			if(closeBtn!=null)	UIEventListener.Get(closeBtn).onClick -= CloseThis;
			if(cencelBtn!=null)	UIEventListener.Get(cencelBtn).onClick -= CloseThis;
            if (putawayPrice != null) putawayPrice.onChange.Remove(edt);
            if (putawayNum != null) putawayNum.onChange.Remove(edt);
		}
	}
    //void Update()
    //{
		
    //}

    void RefreshPrice()
    {
        if (putawayNum == null || putawayPrice == null) 
            return;
        if (putawayNum.value != string.Empty && putawayNum.value != null && putawayPrice.value != string.Empty && putawayPrice.value != null)
        {
            if (Convert.ToInt32(putawayNum.value) > GameCenter.marketMng.CurMarketItem.StackCurCount)
            {
                putawayNum.value = GameCenter.marketMng.CurMarketItem.StackCurCount.ToString();
            }
            if (Convert.ToInt32(putawayNum.value) <= 0)
            {
                putawayNum.value = "1";
            }
            if (Convert.ToInt32(putawayPrice.value) <= 0)
            {
                putawayPrice.value = "1";
            }
            if (Convert.ToInt32(putawayPrice.value) >= 99999999)
            {
                putawayPrice.value = "99999999";
            }
            if (putawayTotal != null && putawayPrice.value != string.Empty && !putawayNum.value.Contains("-") && !putawayPrice.value.Contains("-") && putawayPrice.value != "0" && putawayNum.value != "0")
            {
                putawayTotal.text = (Convert.ToInt32(putawayNum.value) * Convert.ToInt32(putawayPrice.value)).ToString();
            }

        }
        else
        {
            putawayTotal.text = putawayPrice.value;
        }

    }


	/// <summary>
	/// 刷新数量
	/// </summary>
	void Refresh()
	{
		if (itemUI != null)itemUI.FillInfo (GameCenter.marketMng.CurMarketItem);
		if(itemName!=null)itemName.text = GameCenter.marketMng.CurMarketItem.ItemName;
		if (putawayNum != null)putawayNum.value = "1";
		if (putawayPrice != null)putawayPrice.value = "0";
	}
	void CloseThis(GameObject obj)
	{
		GameCenter.uIMng.CloseGUI (GUIType.PUTAWAY);	
	}
	void AddNum()
	{

	}
	void CutNum(GameObject obj)
	{
	}
	void AddTenNum(GameObject obj)
	{
	}
	void PutawayItem(GameObject obj)
	{
        if (putawayTotal != null && putawayPrice != null && putawayNum != null)
        { 
            int total = 0;
            if(!int.TryParse(putawayTotal.text,out total) || total <= 0)
                return;
            //Debug.Log("上架物品数量为" + putawayNum.value + "价格为" + putawayPrice.value + "是否使用喇叭为" + noticeToggle.value);
            int price = 0;
            int num = 0;
            if (int.TryParse(putawayPrice.value, out price) && int.TryParse(putawayNum.value, out num))
            {
                GameCenter.marketMng.C2S_PutawayMarketItem(GameCenter.marketMng.CurMarketItem.InstanceID, price, coinToggle.value == true ? 1 : 2, num, noticeToggle.value == true ? 1 : 0);
            }
            GameCenter.uIMng.CloseGUI(GUIType.PUTAWAY);
        }
		
	}

}
