//==============================================
//作者：黄洪兴
//日期：2016/3/24
//用途：商店管理类
//=================================================



using UnityEngine;
using System.Collections;
using st.net.NetBase;
using System;
using System.Collections.Generic;

public class ShopMng 
{
	/// <summary>
	/// 积分商品列表
	/// </summary>
	public Dictionary<int, ShopItemInfo> ScoresShopItemDic = new Dictionary<int, ShopItemInfo>();
	/// <summary>
	/// 声望商品列表
	/// </summary>
	public Dictionary<int, ShopItemInfo> ReputationShopItemDic = new Dictionary<int, ShopItemInfo>();
	/// <summary>
	/// 功勋商品列表
	/// </summary>
	public Dictionary<int, ShopItemInfo> ExploitShopItemDic = new Dictionary<int, ShopItemInfo>();
	/// <summary>
	/// 装备商品列表
	/// </summary>
	public Dictionary<int, ShopItemInfo> EquipmentShopItemDic = new Dictionary<int, ShopItemInfo>();
	/// <summary>
	/// 杂货商品列表
	/// </summary>
	public Dictionary<int, ShopItemInfo> NormalShopItemDic = new Dictionary<int, ShopItemInfo>();
	/// <summary>
	/// 商品列表
	/// </summary>
	public Dictionary<int, ShopItemInfo> ShopItemDic = new Dictionary<int, ShopItemInfo>();
	/// <summary>
	/// 可购回的商品列表
	/// </summary>
	public Dictionary<int, ShopItemInfo> RedeemShopItemDic = new Dictionary<int, ShopItemInfo>();
    /// <summary>
    /// 真元商品列表
    /// </summary>
    public Dictionary<int, ShopItemInfo> CashShopItemDic = new Dictionary<int, ShopItemInfo>();

    public ShopItemType CurType = ShopItemType.NORMAL;
	/// <summary>
	///交易成功的事件
	/// </summary>
	public Action OnTrade;

	/// <summary>
	/// 可购回物品变化事件
	/// </summary>
	public Action OnRedeemUpdate;

    public Action ShowDrugBtn;

    /// <summary>
    /// 当前不足需要提示的红药物品
    /// </summary>
    public EquipmentInfo CurLackHPItemInfo=null;
    /// <summary>
    /// 当前不足需要提示的蓝药物品
    /// </summary>
    public EquipmentInfo CurLackMPItemInfo = null;

    protected bool showedHPDrugBtn = false;
    public bool ShowedHPDrugBtn
    {
        set {
            if (showedHPDrugBtn != value)
            {
                showedHPDrugBtn = value;
                if (ShowDrugBtn != null)
                    ShowDrugBtn();
            }
        }
        get
        {
            return showedHPDrugBtn;
        }
    }
    protected bool showedMPDrugBtn = false;
    public bool ShowedMPDrugBtn
    {
        set
        {
            if (showedMPDrugBtn != value)
            {
                showedMPDrugBtn = value;
                if (ShowDrugBtn != null)
                    ShowDrugBtn();
            }
        }
        get
        {
            return showedMPDrugBtn;
        }
    }
	#region 构造
	/// <summary>
	/// 返回该管理类的唯一实例 by 贺丰
	/// </summary>
	/// <returns></returns>
	public static ShopMng CreateNew(MainPlayerMng _main)
	{
		if (_main.shopMng == null)
		{
			ShopMng ShopMng = new ShopMng();
			ShopMng.Init(_main);
			return ShopMng;
		}
		else
		{
			_main.shopMng.UnRegist(_main);
			_main.shopMng.Init(_main);
			return _main.shopMng;
		}
	}
	/// <summary>
	/// 注册
	/// </summary>
	protected virtual void Init(MainPlayerMng _main)
	{
		Dictionary<int,ShopRef> all = ConfigMng.Instance.AllShopItem;
        using (var e = all.GetEnumerator())
        {
            while (e.MoveNext())
            {
                ShopItemDic[e.Current.Key] = new ShopItemInfo(e.Current.Key);
            }
        }
        CurType = ShopItemType.NORMAL;
		MsgHander.Regist(0xD372, S2C_OnGetRedeemItemList);
//		MsgHander.Regist(0xD401, S2C_OnGetUseSkillList);
		//GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += ChangeAutoUseSkill;
	}
	/// <summary>
	/// 注销
	/// </summary>
	protected virtual void UnRegist(MainPlayerMng _main)
	{

		//371出售  372 回购 373 购买次数   375 请求
		MsgHander.UnRegist(0xD372, S2C_OnGetRedeemItemList);
 //		MsgHander.UnRegist(0xD100, S2C_OnGetSkillList);
//		MsgHander.UnRegist(0xD401, S2C_OnGetUseSkillList);
		//GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= ChangeAutoUseSkill;
        RedeemShopItemDic.Clear();
	}
	#endregion

	#region 通信S2C
	/// <summary>
	/// 获得可以购回的物品
	/// </summary>
	/// <param name="_info">Info.</param>
	private void S2C_OnGetRedeemItemList(Pt _pt)
	{
		RedeemShopItemDic.Clear ();
		pt_buy_items_d372 pt = _pt as pt_buy_items_d372;
		if (pt != null) {
			for (int i = 0; i < pt.all_buy_items.Count; i++) {

				RedeemShopItemDic [(int)pt.all_buy_items [i].id] = new ShopItemInfo ((int)pt.all_buy_items [i].id,(int)pt.all_buy_items [i].type,(int)pt.all_buy_items [i].num);
				//Debug.Log ("收到的可购回商品的ID为"+(int)pt.all_buy_items [i].type+"唯一ID为"+(int)pt.all_buy_items [i].id);

			}
			
		}

		if (OnRedeemUpdate != null) {
			OnRedeemUpdate ();
		}


		//Debug.Log ("收到可以购回物品的协议");




	}


	#endregion

	#region C2S	




	#endregion

	#region 辅助逻辑
	/// <summary>
	/// 将商品分类
	/// </summary>
	public  void ClassifyShopItem()
	{
		ScoresShopItemDic.Clear ();
		ReputationShopItemDic.Clear ();
		ExploitShopItemDic.Clear ();
		EquipmentShopItemDic.Clear ();
		NormalShopItemDic.Clear();
        using (var e = ShopItemDic.GetEnumerator())
        {
            while (e.MoveNext())
            {
                switch (e.Current.Value.Type)
                {
                    case 1: if (e.Current.Value.Prof == 0 || e.Current.Value.Prof == GameCenter.mainPlayerMng.MainPlayerInfo.Prof) { NormalShopItemDic[e.Current.Value.ID] = e.Current.Value; } break;
                    case 2: if (e.Current.Value.Prof == 0 || e.Current.Value.Prof == GameCenter.mainPlayerMng.MainPlayerInfo.Prof) { EquipmentShopItemDic[e.Current.Value.ID] = e.Current.Value; } break;
                    case 3: if (e.Current.Value.Prof == 0 || e.Current.Value.Prof == GameCenter.mainPlayerMng.MainPlayerInfo.Prof) { ExploitShopItemDic[e.Current.Value.ID] = e.Current.Value; } break;
                    case 4: if (e.Current.Value.Prof == 0 || e.Current.Value.Prof == GameCenter.mainPlayerMng.MainPlayerInfo.Prof) { ReputationShopItemDic[e.Current.Value.ID] = e.Current.Value; } break;
                    case 5: if (e.Current.Value.Prof == 0 || e.Current.Value.Prof == GameCenter.mainPlayerMng.MainPlayerInfo.Prof) { ScoresShopItemDic[e.Current.Value.ID] = e.Current.Value; } break;
                    case 6: if (e.Current.Value.Prof == 0 || e.Current.Value.Prof == GameCenter.mainPlayerMng.MainPlayerInfo.Prof) { CashShopItemDic[e.Current.Value.ID] = e.Current.Value; } break;
                    default:
                        break;
                }

            }
        }
		//Debug.Log ("长度分别为"+ScoresShopItemDic.Count+":"+ReputationShopItemDic.Count+":"+ExploitShopItemDic.Count+":"+EquipmentShopItemDic.Count+":"+NormalShopItemDic.Count);


	}





    public void OpenWndByType(ShopItemType _type)
    {
        //if (CurType != _type)  这样会导致有时候无法跳转
        //{
            CurType = _type;
            GameCenter.uIMng.SwitchToUI(GUIType.SHOPWND);
        //}
		//OpenShopWnd();
    }

	public void OpenShopWnd()
	{
		UIMng uiMng = GameCenter.uIMng;
		if(uiMng.CurOpenType == GUIType.NONE)
			GameCenter.uIMng.SwitchToUI(GUIType.SHOPWND);
		else
			GameCenter.uIMng.GenGUI(GUIType.SHOPWND,true);
	}
	#endregion
}
public enum ShopItemType
{
	/// 购回
	/// </summary>
	REDEEM = 0,
	/// <summary>
	/// 杂货
	/// </summary>
	NORMAL = 1,
	/// <summary>
	/// 装备
	/// </summary>
	EQUIPMENT = 2,
	/// <summary>
	/// 功勋
	/// </summary>
	EXPLOIT= 3,
	/// <summary>
	/// 声望
	/// </summary>
	REPUTATION= 4,
	/// <summary>
	/// 积分
	/// </summary>
	SCORES= 5,
	/// <summary>
    /// 真元
    /// <summary>
    CASH = 6,
}
