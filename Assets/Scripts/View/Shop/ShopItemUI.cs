//=====================================
//作者:黄洪兴
//日期:2016/3/22
//用途:商品组件
//========================================


using UnityEngine;
using System.Collections;
/// <summary>
/// 商品UI组件
/// </summary>
public class ShopItemUI : MonoBehaviour
{
	#region 控件数据
	/// <summary>
	/// 商品名字
	/// </summary>
	public UILabel itemName;
	/// <summary>
	/// 商品的图片
	/// </summary>
	public ItemUI itemUI;
	/// <summary>
	/// 商品价格
	/// </summary>
	public UILabel price;
	/// <summary>
	/// 商品价格对应的图片
	/// </summary>
	public UISprite priceSprite;


	public GameObject buyObj;
	#endregion 
	#region 数据
	/// <summary>
	/// 选择事件
	/// </summary>
	public System.Action<ShopItemUI> OnSelectEvent = null;
	/// <summary>
	/// 当前填充的数据
	/// </summary>
	public ShopItemInfo shopIteminfo;
	/// <summary>
	/// 当前填充的城内商店数据
	/// </summary>
	protected CityShopRef cityShopRef;

	public UILabel labLimitCount;

	protected int curLimitCount = -1;
	#endregion
	// Use this for initialization
	void Start () {
//		UIEventListener.Get(useBtn).onClick -= UseTitle;
//		UIEventListener.Get(useBtn).onClick += UseTitle;

	}

    void OnEnable()
    {
        if (buyObj != null)
        UIEventListener.Get(buyObj).onClick += BuyItem;
    }
    void OnDisable()
    {
        if (buyObj != null)
        UIEventListener.Get(buyObj).onClick -= BuyItem;
    }



	void  BuyItem(GameObject obj)
	{
		if (shopIteminfo == null && cityShopRef == null)
			return;
		if(shopIteminfo != null)
		{
			if (shopIteminfo.soleID == 0) 
			{
				GameCenter.buyMng.CurPrice = shopIteminfo.Price;
				GameCenter.buyMng.id = shopIteminfo.ID;
				GameCenter.buyMng.priceType = shopIteminfo.BuyWay.EID;
				if (shopIteminfo.Item != null)
				{
					ToolTipMng.ShowEquipmentTooltip(shopIteminfo.Item, ItemActionType.StoreBuy, ItemActionType.None, ItemActionType.None, ItemActionType.None, this.gameObject);
				}
			}else
			{
				GameCenter.buyMng.id = shopIteminfo.soleID;
				if (shopIteminfo.Item != null)
				{
					ToolTipMng.ShowEquipmentTooltip(shopIteminfo.Item, ItemActionType.Redeem, ItemActionType.None, ItemActionType.None, ItemActionType.None, this.gameObject);
				}
			}
		}
		if(cityShopRef != null)
		{
			GameCenter.buyMng.CurPrice = cityShopRef.price;
			GameCenter.buyMng.id = cityShopRef.id;
			GameCenter.buyMng.priceType = cityShopRef.priceType;
			GameCenter.buyMng.restrictionNum = curLimitCount;
			EquipmentInfo equip = new EquipmentInfo(cityShopRef.itemID,EquipmentBelongTo.SHOPWND);
			if (equip != null)
			{
				ToolTipMng.ShowEquipmentTooltip(equip, ItemActionType.CityShopBuy, ItemActionType.None, ItemActionType.None, ItemActionType.None, this.gameObject);
			}
		}
	}

	void UseTitle(GameObject obj)
	{
//		if (titleinfo.IsOwn) {
//			if (GameCenter.titleMng.CurUseTitle == titleinfo) {
//
//				GameCenter.titleMng.C2S_UseTitle (titleinfo.ID, 0);
//			} else {
//				GameCenter.titleMng.C2S_UseTitle (titleinfo.ID, 1);
//			}
//		} else {
//			GameCenter.messageMng.AddClientMsg ("该称号未获得");
//		}
//		GameCenter.titleMng.ChooseTitle = titleinfo;
//		GameCenter.titleMng.UpdateTitle ();
//
	}


	/// <summary>
	/// 填充数据
	/// </summary>
	/// <param name="_info"></param>
	public void FillInfo(ShopItemInfo _info)
	{
		if (_info == null) {
			shopIteminfo = null;
			return;
		} 
		else {
			shopIteminfo = _info;

			//			oldSkillinfo = skillinfo;
		}
		RefreshShopItem ();
	}

	public void FillInfo(CityShopRef shop)
	{
		//CityShopRef shop = ConfigMng.Instance.GetShopRefByID(item.id);
		cityShopRef = shop;
		int limitCount = GameCenter.activityMng.GetCityShopLimitCount(shop.id);
		curLimitCount = (limitCount != -1)?limitCount:shop.limitCount;
		EquipmentInfo equip = new EquipmentInfo(shop.itemID,EquipmentBelongTo.SHOPWND);
		if(labLimitCount != null)labLimitCount.text = curLimitCount.ToString();
		if(itemName!=null)
			itemName.text = equip.ItemName;
		if(price!=null)
			price.text = shop.price.ToString ();
        if (priceSprite != null)
        { 
            priceSprite.spriteName = GameHelper.GetCoinIconByType(shop.priceType);
        }
        if (priceSprite != null)
        {
            priceSprite.MakePixelPerfect();
        }
		if(itemUI!=null)
			itemUI.FillInfo(equip);
	}
	/// <summary>
	/// 刷新表现
	/// </summary>
	public void RefreshShopItem()
	{
		if(itemName!=null)
            itemName.text = shopIteminfo.Item.ItemStrColor + shopIteminfo.Item.ItemName;
		if(price!=null&&shopIteminfo.soleID==0)
		price.text = shopIteminfo.Price.ToString ();
        if (price != null && shopIteminfo.soleID != 0)
        {
            price.text = (shopIteminfo.Item.Price * shopIteminfo.nums).ToString();
        }
        if (priceSprite != null && shopIteminfo.soleID == 0)
        {
            priceSprite.spriteName = shopIteminfo.BuyIcon; 
        }
        if (priceSprite != null && shopIteminfo.soleID != 0)
        {
            priceSprite.spriteName = "Icon_tongbisuo";
        }
        if (priceSprite != null)
        {
            priceSprite.MakePixelPerfect();
        }
		if(itemUI!=null)
			itemUI.FillInfo(shopIteminfo.Item);
		//		Debug.Log ("此时角色穿戴的称号为"+GameCenter.titleMng.CurUseTitle.ID);
//		if (titleinfo != null) {
//			titleName.text = titleinfo.Name;
//			chooseObj.SetActive (isChoose);
//			useObj.SetActive (isUse);
//		} else {
//
//		}

	}
}
