//=====================================
//作者:黄洪兴
//日期:2016/4/6
//用途:商城商品组件
//========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 商城商品UI组件
/// </summary>
public class MallItemUI : MonoBehaviour
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
	/// 商品而外价格
	/// </summary>
	public UILabel otherPrice;


	public GameObject buyBtn;
	/// <summary>
	/// 标签
	/// </summary>
	public UISprite tab;
	/// <summary>
	/// 第二个标签
	/// </summary>
	public UISprite otherTab;
	/// <summary>
	/// VIP限购标识
	/// </summary>
	public UILabel VIP;
	/// <summary>
	/// 限购
	/// </summary>
	public UILabel restriction;
	#endregion 
	#region 数据
	/// <summary>
	/// 选择事件
	/// </summary>
	public System.Action<MallItemUI> OnSelectEvent = null;
	/// <summary>
	/// 当前填充的数据
	/// </summary>
	public MallItemInfo mallIteminfo;
	//	protected SkillInfo oldSkillinfo;  //for upgrade effect -by ms
	#endregion
	// Use this for initialization
	void Start () {
        //UIEventListener.Get(buyBtn).onClick -= BuyItem;
        //UIEventListener.Get(buyBtn).onClick += BuyItem;
		//		UIEventListener.Get(chooseBtn).onClick += ChooseTitle;
		//		UIEventListener.Get(useBtn).onClick -= UseTitle;
		//		UIEventListener.Get(useBtn).onClick += UseTitle;

	}


    void OnEnable()
    {
        if (buyBtn!=null)
        UIEventListener.Get(buyBtn).onClick += BuyItem;
    }
    void OnDisable()
    {
        if (buyBtn != null)
        UIEventListener.Get(buyBtn).onClick -= BuyItem;
    }


	void BuyItem(GameObject obj)
	{
		if (mallIteminfo == null)
			return;
        if (mallIteminfo.Amount !=0&& mallIteminfo.Amount - mallIteminfo.buyedNum <= 0)
        {
			GameCenter.messageMng.AddClientMsg(167);
			return;
		}
        GameCenter.buyMng.id = mallIteminfo.ID;
		GameCenter.buyMng.CurPrice = mallIteminfo.Price;
		GameCenter.buyMng.priceType = mallIteminfo.BuyWay.EID;
        if (mallIteminfo.Amount != 0 && mallIteminfo.Amount - mallIteminfo.buyedNum > 0)
        {
            GameCenter.buyMng.OpenBuyWnd(mallIteminfo.Item, BuyType.MALL, mallIteminfo.Amount - mallIteminfo.buyedNum);
        }
        else
        {
            GameCenter.buyMng.OpenBuyWnd(mallIteminfo.Item, BuyType.MALL);
        }
		//		GameCenter.titleMng.ChooseTitle = titleinfo;
		//		GameCenter.titleMng.UpdateTitle ();

	}
		


	/// <summary>
	/// 填充数据
	/// </summary>
	/// <param name="_info"></param>
	public void FillInfo(MallItemInfo _info)
	{
		if (_info == null) {
			mallIteminfo = null;
			return;
		} 
		else {
			mallIteminfo = _info;

			//			oldSkillinfo = skillinfo;
		}
		RefreshMallItem ();
	}
	/// <summary>
	/// 刷新表现
	/// </summary>
	public void RefreshMallItem()
	{

        if (itemName != null)
        {
            
            itemName.text =mallIteminfo.Item.ItemStrColor+ mallIteminfo.Item.ItemName;
        }
		if (itemUI != null)itemUI.FillInfo (mallIteminfo.Item);
		if (tab != null) {
            if (mallIteminfo.OriginalPrice !=null&& mallIteminfo.OriginalPrice != string.Empty && mallIteminfo.OriginalPrice != "" & mallIteminfo.OriginalPrice != "0")
            {
				if (price != null)
					price.text = mallIteminfo.OriginalPrice;
				if (otherPrice != null)
                    otherPrice.text = mallIteminfo.NowPrice;
			} else {
				if (price != null)
                    price.text = mallIteminfo.NowPrice;
				if (otherPrice != null)
					otherPrice.text = "";
			}
			if (mallIteminfo.Tab.Count > 0 && otherTab != null && tab != null) 
            {
                tab.gameObject.SetActive(false);
                otherTab.gameObject.SetActive(false);
				if (mallIteminfo.Tab.Count == 2) {
					tab.gameObject.SetActive (true);
					tab.spriteName = mallIteminfo.Tab [0];
					otherTab.gameObject.SetActive (true);
					otherTab.spriteName = mallIteminfo.Tab [1];
				}
                if (mallIteminfo.Tab.Count == 1)
                {
                    if (mallIteminfo.Tab[0] == "0")
                    {
                        tab.gameObject.SetActive(false);
                        otherTab.gameObject.SetActive(false);
                    }
                    else
                    {
                        tab.gameObject.SetActive(true);
                        tab.spriteName = mallIteminfo.Tab[0];
                    }
                }
			}

		}
		if(VIP!=null)
		{
			if (mallIteminfo.Vip_level == 0) {
				VIP.text = "";
			} else {
				VIP.text="vip"+mallIteminfo.Vip_level+ConfigMng.Instance.GetUItext(330);
			}
		}
		if (restriction != null) {
			if (mallIteminfo.Amount == 0) {
				restriction.text = "";
			} else {
                restriction.text = ConfigMng.Instance.GetUItext(331) + (mallIteminfo.Amount - mallIteminfo.buyedNum).ToString() + "/" + mallIteminfo.Amount.ToString();
//				Debug.Log ("商品"+mallIteminfo.ID+"的已购买次数为"+mallIteminfo.buyedNum);
			}
		}
			
//		price.text = shopIteminfo.Price.ToString ();
//		priceSprite.spriteName = shopIteminfo.BuyWay.IconName;
//		itemSprite.spriteName = shopIteminfo.Item.IconName;
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
