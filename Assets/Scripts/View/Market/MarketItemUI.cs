//=====================================
//作者:黄洪兴
//日期:2016/4/19
//用途:市场大分页UI
//========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// 市场大分页UI组件
/// </summary>
public class MarketItemUI : MonoBehaviour
{
	#region 控件数据
	/// <summary>
	/// 物品UI
	/// </summary>
	public ItemUI itemUI;
	/// <summary>
	/// 拍卖物品的名字
	/// </summary>
	public UILabel itemName;
	/// <summary>
	/// 拍卖物品的等级
	/// </summary>
	public UILabel itemLev;
	/// <summary>
	/// 拍卖物品的单价
	/// </summary>
	public UILabel itemUnitPrice;
	/// <summary>
	/// 拍卖物品的总价
	/// </summary>
	public UILabel itemTotalPrice;
	public UISprite coinSprite;
	public UISprite coinSpriteTwo;
    public UISprite diamoSprite;
    public UISprite diamoSpriteTwo;
    public UILabel remainTime;

    /// <summary>
    /// 续期按钮
    /// </summary>
    public GameObject renewalObj;
	/// <summary>
	/// 购买按钮
	/// </summary>
	public GameObject buyItemObj;
	/// <summary>
	/// 取回按钮
	/// </summary>
	public GameObject getBackObj;
	#endregion 
	#region 数据
	/// <summary>
	/// 选择事件
	/// </summary>
	public System.Action<MarketItemUI> OnSelectEvent = null;
	/// <summary>
	/// 当前填充的数据
	/// </summary>
	public MarketItemInfo marketItemInfo;
	//	protected SkillInfo oldSkillinfo;  //for upgrade effect -by ms
	#endregion
	// Use this for initialization
	void Start () {

		if(buyItemObj!=null)
		{
			UIEventListener.Get (buyItemObj).onClick -= BuyMarketItem;
			UIEventListener.Get(buyItemObj).onClick += BuyMarketItem;
		}
		if(getBackObj!=null)
		{
			UIEventListener.Get (getBackObj).onClick -= GetBackMarketItem;
			UIEventListener.Get(getBackObj).onClick += GetBackMarketItem;
		}

	}

	void  BuyMarketItem(GameObject obj)
	{
		GameCenter.marketMng.CurAuctionItem = marketItemInfo;
		if(GameCenter.marketMng.OnAuctionItem!=null)
		GameCenter.marketMng.OnAuctionItem ();
	}

	void GetBackMarketItem(GameObject obj)
	{
        GameCenter.marketMng.C2S_GetBackMarketItem(marketItemInfo.EID);
//		Debug.Log ("取回拍卖物品");
	}


	/// <summary>
	/// 填充数据
	/// </summary>
	/// <param name="_info"></param>
	public void FillInfo(MarketItemInfo _info)
	{
		if (_info == null) {
			marketItemInfo = null;
			return;
		} 
		else {
			marketItemInfo = _info;

		}
		RefreshMarketItem ();
	}
	/// <summary>
	/// 刷新表现
	/// </summary>
	public void RefreshMarketItem()
	{

		if (itemUI != null) itemUI.FillInfo (marketItemInfo.EquipmentInfo);
		if (itemName != null) itemName.text=marketItemInfo.EquipmentInfo.ItemName;
        if (itemLev != null) itemLev.text = ConfigMng.Instance.GetLevelDes(marketItemInfo.EquipmentInfo.LV);
		if (itemUnitPrice != null) itemUnitPrice.text=marketItemInfo.Price.ToString();
		if (itemTotalPrice != null) itemTotalPrice.text=(marketItemInfo.Price*marketItemInfo.Nums).ToString();
        if (remainTime != null)
        {
            if (marketItemInfo.RemainTime > 0)
            {
                remainTime.text = (marketItemInfo.RemainTime / 3600).ToString() + 
                    ConfigMng.Instance.GetUItext(304)+ ((marketItemInfo.RemainTime % 3600) / 60).ToString() + 
                    ConfigMng.Instance.GetUItext(305) + ((marketItemInfo.RemainTime % 3600) % 60).ToString() + ConfigMng.Instance.GetUItext(306);
            }
            else
            {
                remainTime.text = "";
            }

        }
        if (renewalObj != null)
        {
            renewalObj.SetActive(!(marketItemInfo.RemainTime > 0));
        }

        if (marketItemInfo.PriceType == 1)
        {

            if (coinSprite != null) coinSprite.gameObject.SetActive(true);
            if (coinSpriteTwo != null) coinSpriteTwo.gameObject.SetActive(true);
            if (diamoSprite != null) diamoSprite.gameObject.SetActive(false);
            if (diamoSpriteTwo != null) diamoSpriteTwo.gameObject.SetActive(false);
        }
        else
        {
            if (coinSprite != null) coinSprite.gameObject.SetActive(false);
            if (coinSpriteTwo != null) coinSpriteTwo.gameObject.SetActive(false);
            if (diamoSprite != null) diamoSprite.gameObject.SetActive(true);
            if (diamoSpriteTwo != null) diamoSpriteTwo.gameObject.SetActive(true);
        }

	}

	void RefreshMarketPage()
	{
	}




}
