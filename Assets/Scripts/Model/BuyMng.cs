//==============================================
//作者：黄洪兴
//日期：2016/4/1
//用途：物品出售管理类
//=================================================



using UnityEngine;
using System.Collections;
using st.net.NetBase;
using System;
using System.Collections.Generic;

public class BuyMng 
{
	public List<int> SellEqList = new List<int> ();

    /// <summary>
    /// 限购数量
    /// </summary>
    public int restrictionNum=0;

    public int CurPrice;
	/// <summary>
	/// 当前需要购买的物品数据
	/// </summary>
	public EquipmentInfo curEq;
	/// <summary>
	/// 当前购买方式
	/// </summary>
    public BuyType buyType;
	public  int id;
	/// <summary>
	/// 物品数量 仅用于物品回购或者拍卖
	/// </summary>
	public  int nums=0;
	/// <summary>
	/// 价格种类
	/// </summary>
	public int priceType;
	/// <summary>
	///出售物品改变事件
	/// </summary>
	public Action OnSellEqChange;

    public Action<bool> OnBatchSellEvent;

	#region 构造
	/// <summary>
	/// 返回该管理类的唯一实例
	/// </summary>
	/// <returns></returns>
	public static BuyMng CreateNew(MainPlayerMng _main)
	{
		if (_main.buyMng == null)
		{
			BuyMng BuyMng = new BuyMng();
			BuyMng.Init(_main);
			return BuyMng;
		}
		else
		{
			_main.buyMng.UnRegist(_main);
			_main.buyMng.Init(_main);
			return _main.buyMng;
		}
	}
	/// <summary>
	/// 注册
	/// </summary>
	protected virtual void Init(MainPlayerMng _main)
	{
		//		MsgHander.Regist(0xD100, S2C_OnGetSkillList);
		//		MsgHander.Regist(0xD401, S2C_OnGetUseSkillList);
		//GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += ChangeAutoUseSkill;
	}
	/// <summary>
	/// 注销
	/// </summary>
	protected virtual void UnRegist(MainPlayerMng _main)
	{
		//		MsgHander.UnRegist(0xD100, S2C_OnGetSkillList);
		//		MsgHander.UnRegist(0xD401, S2C_OnGetUseSkillList);
		//GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= ChangeAutoUseSkill;
	}
	#endregion

	#region 通信S2C


	#endregion

	#region C2S	
	public void C2S_AskBuyItem(int num)
	{
		pt_buy_action_d371 msg = new pt_buy_action_d371();
		msg.id = id;
		msg.num = (uint)num;
		msg.action = (int)buyType;
		NetMsgMng.SendMsg(msg);
		//Debug.Log ("发送购买物品协议"+id+":"+num+":"+buyType);
	}

    public void C2S_AskBuyItem(int num,int _buyType)
    {
        pt_buy_action_d371 msg = new pt_buy_action_d371();
        msg.id = id;
        msg.num = (uint)num;
        msg.action = _buyType;
        NetMsgMng.SendMsg(msg);
        //Debug.Log ("发送购买物品协议"+id+":"+num+":"+buyType);
    }

	public void C2S_AskSellItem(List<int> eqList)
	{
		pt_req_sell_d376 msg = new pt_req_sell_d376();
		msg.id_list = eqList;
		NetMsgMng.SendMsg(msg);
		//Debug.Log ("发送批量出售协议");
	}


	#endregion

	#region 辅助逻辑

    public void OpenBuyWnd(EquipmentInfo _eq, BuyType _buyType,int _restrictionNum=0)
	{
        restrictionNum = _restrictionNum;
		nums = 0;
		if (_eq != null)
		curEq = _eq;
		buyType = _buyType;
		GameCenter.uIMng.GenGUI(GUIType.BUYWND,true);

	}

    /// <summary>
    /// 打开购买物品界面
    /// </summary>
    /// <param name="_buyEquip"></param>
    public void OpenBuyWnd(EquipmentInfo _buyEquip)
    {
        if (_buyEquip == null) return;
        MallRef mallRef = ConfigMng.Instance.GetMallRefByEID(_buyEquip.EID);
        if (mallRef != null)
        {
            id = mallRef.id;
            CurPrice = mallRef.price;
            priceType = mallRef.buyWay;
            OpenBuyWnd(new EquipmentInfo(_buyEquip.EID, EquipmentBelongTo.PREVIEW), BuyType.MALL);
        }
    }

    public void AskRedeem(EquipmentInfo _eq, GUIType _type, BuyType _buyType, int _nums)
	{
		nums = 0;
		if (_eq != null)
		curEq = _eq;
		nums = _nums;
		buyType = _buyType;
      //  GameCenter.uIMng.GenGUI(GUIType.BUYWND, true);

	}
    /// <summary>
    /// 开始批量出售
    /// </summary>
    /// <param name="_batchSell"></param>
    public void BatchSell(bool _batchSell)
    {
        if (OnBatchSellEvent != null)
            OnBatchSellEvent(_batchSell);
    }

	#endregion

}
public enum BuyType
{
    NONE,
    /// <summary>
    /// 商城
    /// </summary>
    MALL = 1,
    /// <summary>
    /// 商店
    /// </summary>
    SHOP = 2,
    /// <summary>
    /// 回购
    /// </summary>
    REDEEM=3,
	/// <summary>
	/// 城内商店 by邓成
	/// </summary>
	CITYSHOP = 4,


    /// <summary>
    /// 公会商店
    /// </summary>
    GUILDSHOP = 5,
    /// <summary>
    /// 开服贺礼
    /// </summary>
    OPENSERVER,





}