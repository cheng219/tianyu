//==============================================
//作者：黄洪兴
//日期：2016/4/5
//用途：商城管理类
//=================================================



using UnityEngine;
using System.Collections;
using st.net.NetBase;
using System;
using System.Collections.Generic;

public class NewMallMng 
{
    /// <summary>
    /// 当前需要打开的界面类型
    /// </summary>
    public MallItemType CurMallType;
	/// <summary>
	/// 限购商品列表
	/// </summary>
	public Dictionary<int, MallItemInfo> RestrictionMallItemDic = new Dictionary<int, MallItemInfo>();
	/// <summary>
	/// 变强商品列表
	/// </summary>
	public Dictionary<int, MallItemInfo> StrengthenMallItemDic = new Dictionary<int, MallItemInfo>();
	/// <summary>
	/// 灵兽商品列表
	/// </summary>
	public Dictionary<int, MallItemInfo> PetMallItemDic = new Dictionary<int, MallItemInfo>();
	/// <summary>
	/// 日常商品列表
	/// </summary>
	public Dictionary<int, MallItemInfo> DailyMallItemDic = new Dictionary<int, MallItemInfo>();
	/// <summary>
	/// 时装商品列表
	/// </summary>
	public Dictionary<int, MallItemInfo> FashionMallItemDic = new Dictionary<int, MallItemInfo>();
	/// <summary>
	/// 礼金商品列表
	/// </summary>
	public Dictionary<int, MallItemInfo> CashMallItemDic = new Dictionary<int, MallItemInfo>();
	/// <summary>
	/// 商品列表
	/// </summary>
	public Dictionary<int, MallItemInfo> MallItemDic = new Dictionary<int, MallItemInfo>();

	/// <summary>
	///交易成功的事件
	/// </summary>
	public Action OnTrade;
	/// <summary>
	/// 获得购买物品次数事件
	/// </summary>
	public Action OnGetBuyNums;


	#region 构造
	/// <summary>
	/// 返回该管理类的唯一实例 by 贺丰
	/// </summary>
	/// <returns></returns>
	public static NewMallMng CreateNew(MainPlayerMng _main)
	{
		if (_main.newMallMng == null)
		{
			NewMallMng NewMallMng = new NewMallMng();
			NewMallMng.Init(_main);
			return NewMallMng;
		}
		else
		{
			_main.newMallMng.UnRegist(_main);
			_main.newMallMng.Init(_main);
			return _main.newMallMng;
		}
	}
	/// <summary>
	/// 注册
	/// </summary>
	protected virtual void Init(MainPlayerMng _main)
	{
		Dictionary<int,MallRef> all = ConfigMng.Instance.AllMallItem;
        using (var e = all.GetEnumerator())
        {
            while (e.MoveNext())
            {
                MallItemDic[e.Current.Key] = new MallItemInfo(e.Current.Key);
            }
        }
		C2S_AskBuyItemNum ();
        CurMallType = MallItemType.RESTRICTION;
		MsgHander.Regist(0xD373, S2C_OnGetBuyNums);
	}
	/// <summary>
	/// 注销
	/// </summary>
	protected virtual void UnRegist(MainPlayerMng _main)
	{
		//371出售  372 回购 373 购买次数   375 请求
		MsgHander.UnRegist(0xD373, S2C_OnGetBuyNums);
		MallItemDic.Clear();
        RestrictionMallItemDic.Clear();
        StrengthenMallItemDic.Clear();
        PetMallItemDic.Clear();
        DailyMallItemDic.Clear();
        FashionMallItemDic.Clear();
        CashMallItemDic.Clear();
	}
	#endregion

	#region 通信S2C
	/// <summary>
	/// 获得已经购买过商品的次数
	/// </summary>
	/// <param name="_pt">Point.</param>
	private void S2C_OnGetBuyNums(Pt _pt)
	{
		pt_shop_items_d373 pt = _pt as pt_shop_items_d373;
		if (pt != null) {

			for (int i = 0; i < pt.shop_items.Count; i++) {
				//Debug.Log ("收到商品ID"+(int)pt.shop_items [i].id+"的次数为"+(int)pt.shop_items [i].num);
				if (pt.shop_items [i].from == 1) {
					MallItemDic [(int)pt.shop_items [i].id].buyedNum = (int)pt.shop_items [i].num;
					//Debug.Log ("已经修改商城类型的商品" + pt.shop_items [i].id + "购买次数为" + (int)pt.shop_items [i].num);
				}
				if (pt.shop_items [i].from == 5) {
					//Debug.Log ("已经修改公会商店类型的商品" + pt.shop_items [i].id + "购买次数为" + (int)pt.shop_items [i].num);
					GameCenter.guildShopMng.GuildShopItemDic[(int)pt.shop_items [i].id].BuyedNum=(int)pt.shop_items [i].num;
					if (GameCenter.guildShopMng.OnItemBuyedNumUpdate != null)
						GameCenter.guildShopMng.OnItemBuyedNumUpdate ();	
				}
					
			}

		}

		if (OnGetBuyNums!=null) {
			OnGetBuyNums ();
		}


		//Debug.Log ("收到已经购买过商品的次数的协议");



	}


	#endregion

	#region C2S	
	/// <summary>
	/// 请求物品购买次数的协议
	/// </summary>
	/// <param name="type">Type.</param>
	/// <param name="state">State.</param>
	public  void C2S_AskBuyItemNum()
	{
		pt_req_shop_items_d375 msg = new pt_req_shop_items_d375();
		NetMsgMng.SendMsg(msg);

		// Debug.Log ("发送请求商品购买次数协议");
		
	}


	#endregion

	#region 辅助逻辑
	/// <summary>
	/// 将商品分类
	/// </summary>
	public  void ClassifyMallItem()
	{
		StrengthenMallItemDic.Clear ();
		PetMallItemDic.Clear ();
		DailyMallItemDic.Clear ();
		FashionMallItemDic.Clear ();
		CashMallItemDic.Clear();

        using (var e = MallItemDic.GetEnumerator())
        {
            while (e.MoveNext())
            {

                switch (e.Current.Value.Type)
                {
                    case 0: if (e.Current.Value.Prof == 0 || e.Current.Value.Prof == GameCenter.mainPlayerMng.MainPlayerInfo.Prof) { RestrictionMallItemDic[e.Current.Value.ID] = e.Current.Value; } break;
                    case 1: if (e.Current.Value.Prof == 0 || e.Current.Value.Prof == GameCenter.mainPlayerMng.MainPlayerInfo.Prof) { StrengthenMallItemDic[e.Current.Value.ID] = e.Current.Value; } break;
                    case 2: if (e.Current.Value.Prof == 0 || e.Current.Value.Prof == GameCenter.mainPlayerMng.MainPlayerInfo.Prof) { PetMallItemDic[e.Current.Value.ID] = e.Current.Value; } break;
                    case 3: if (e.Current.Value.Prof == 0 || e.Current.Value.Prof == GameCenter.mainPlayerMng.MainPlayerInfo.Prof) { DailyMallItemDic[e.Current.Value.ID] = e.Current.Value; } break;
                    case 4: if (e.Current.Value.Prof == 0 || e.Current.Value.Prof == GameCenter.mainPlayerMng.MainPlayerInfo.Prof) { FashionMallItemDic[e.Current.Value.ID] = e.Current.Value; } break;
                    case 5: if (e.Current.Value.Prof == 0 || e.Current.Value.Prof == GameCenter.mainPlayerMng.MainPlayerInfo.Prof) { CashMallItemDic[e.Current.Value.ID] = e.Current.Value; } break;
                    default:
                        break;
                }

            }
        }

	}


	public void OpenWndByType(MallItemType _type)
    {
        //Debug.Log("CurMallType:" + CurMallType+",TYPE:"+_type);
        CurMallType = _type;
        //OpenMall();
        GameCenter.uIMng.SwitchToUI(GUIType.NEWMALL);
    }

	public void OpenMall()
	{
		UIMng uiMng = GameCenter.uIMng;
		if(uiMng.CurOpenType == GUIType.NONE)
			GameCenter.uIMng.SwitchToUI(GUIType.NEWMALL);
		else
			GameCenter.uIMng.GenGUI(GUIType.NEWMALL,true);
	}

	#endregion
}
public enum MallItemType
{
    /// <summary>
    /// 限购
    /// </summary>
	RESTRICTION,
	/// <summary>
	/// 变强
	/// </summary>
	STRENGTHEN = 1,
	/// <summary>
	/// 灵兽
	/// </summary>
	PET = 2,
	/// <summary>
	/// 日常
	/// </summary>
	DAILY= 3,
	/// <summary>
	/// 时装
	/// </summary>
	FASHION= 4,
	/// <summary>
	/// 礼金
	/// </summary>
	CASH= 5,

}
