//=====================================
//作者:黄洪兴
//日期:2016/3/22
//用途:仙盟商店商品组件
//========================================


using UnityEngine;
using System.Collections;
/// <summary>
/// 仙盟商店商品UI组件
/// </summary>
public class GuildShopItemUI : MonoBehaviour
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
	/// 限购数量
	/// </summary>
	public UILabel restriction;
	/// <summary>
	/// 限购显示
	/// </summary>
	public UILabel restrictionObj;


	public GameObject buyObj;
	#endregion 
	#region 数据
	/// <summary>
	/// 选择事件
	/// </summary>
	public System.Action<GuildShopItemUI> OnSelectEvent = null;
	/// <summary>
	/// 当前填充的数据
	/// </summary>
	public GuildShopItemInfo guildShopIteminfo;
	//	protected SkillInfo oldSkillinfo;  //for upgrade effect -by ms
	#endregion
	// Use this for initialization
	void Start () {
        //UIEventListener.Get (buyObj).onClick -= BuyItem;
        //UIEventListener.Get (buyObj).onClick += BuyItem;
		//		UIEventListener.Get(useBtn).onClick -= UseTitle;
		//		UIEventListener.Get(useBtn).onClick += UseTitle;

	}


    void OnEnable()
    {
        if(buyObj!=null)
        UIEventListener.Get(buyObj).onClick += BuyItem;
    }
    void OnDisable()
    {
        if (buyObj != null)
        UIEventListener.Get(buyObj).onClick -= BuyItem;
    }


	void  BuyItem(GameObject obj)
	{

		if (guildShopIteminfo == null)
			return;
        //if (guildShopIteminfo.Amount - guildShopIteminfo.BuyedNum<=0)
        //{
        //    GameCenter.messageMng.AddClientMsg(167);
        //}
        if (guildShopIteminfo != null)
        {
                GameCenter.buyMng.CurPrice = guildShopIteminfo.Price;
                GameCenter.buyMng.id = guildShopIteminfo.ID;
                GameCenter.buyMng.priceType = 16;
                GameCenter.buyMng.restrictionNum = guildShopIteminfo.Amount - guildShopIteminfo.BuyedNum;
                if (guildShopIteminfo.Item != null)
                {
                    ToolTipMng.ShowEquipmentTooltip(guildShopIteminfo.Item, ItemActionType.StoreBuy, ItemActionType.None, ItemActionType.None, ItemActionType.None, this.gameObject);
                }
        }
        //GameCenter.buyMng.id = guildShopIteminfo.ID;
        //GameCenter.buyMng.CurPrice = guildShopIteminfo.Price;
        //GameCenter.buyMng.priceType = 16;
        //GameCenter.buyMng.OpenBuyWnd(guildShopIteminfo.Item, BuyType.GUILDSHOP, (guildShopIteminfo.Amount - guildShopIteminfo.BuyedNum));
		//		GameCenter.titleMng.ChooseTitle = titleinfo;
		//		GameCenter.titleMng.UpdateTitle ();

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
	public void FillInfo(GuildShopItemInfo _info)
	{
		if (_info == null) {
			guildShopIteminfo = null;
			return;
		} 
		else {
			guildShopIteminfo = _info;

			//			oldSkillinfo = skillinfo;
		}
		RefreshShopItem ();
	}
	/// <summary>
	/// 刷新表现
	/// </summary>
	public void RefreshShopItem()
	{
		if(itemName!=null)
            itemName.text = guildShopIteminfo.Item.ItemStrColor + guildShopIteminfo.Item.ItemName;
		if (itemUI != null)itemUI.FillInfo (guildShopIteminfo.Item);
		if(price!=null)
			price.text = guildShopIteminfo.Price.ToString ();
		if(restriction!=null){
			if (guildShopIteminfo.Amount != 0) {
				restriction.text = (guildShopIteminfo.Amount - guildShopIteminfo.BuyedNum).ToString ();
			} else {
				restriction.gameObject.SetActive (false);
				restrictionObj.gameObject.SetActive (false);
			}
			}
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
