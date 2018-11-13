//==============================================
//作者：黄洪兴
//日期：2016/4/12
//用途：仙盟商店管理类
//=================================================



using UnityEngine;
using System.Collections;
using st.net.NetBase;
using System;
using System.Collections.Generic;

public class GuildShopMng 
{

	/// <summary>
	/// 商品列表
	/// </summary>
	public Dictionary<int, GuildShopItemInfo> GuildShopItemDic = new Dictionary<int, GuildShopItemInfo>();

	/// <summary>
	///交易成功的事件
	/// </summary>
	public Action OnTrade;

	/// <summary>
	///购买次数更新事件
	/// </summary>
	public Action OnItemBuyedNumUpdate;

	#region 构造
	/// <summary>
	/// 返回该管理类的唯一实例 by 贺丰
	/// </summary>
	/// <returns></returns>
	public static GuildShopMng CreateNew(MainPlayerMng _main)
	{
		if (_main.guildShopMng == null)
		{
			GuildShopMng GuildShopMng = new GuildShopMng();
			GuildShopMng.Init(_main);
			return GuildShopMng;
		}
		else
		{
			_main.guildShopMng.UnRegist(_main);
			_main.guildShopMng.Init(_main);
			return _main.guildShopMng;
		}
	}
	/// <summary>
	/// 注册
	/// </summary>
	protected virtual void Init(MainPlayerMng _main)
	{
		Dictionary<int,GuildShopRef> all = ConfigMng.Instance.AllGuildShopItem;
        using (var e = all.GetEnumerator())
        {
            while (e.MoveNext())
            {
                GuildShopItemDic[e.Current.Key] = new GuildShopItemInfo(e.Current.Value.id);
            }
        }
		//MsgHander.Regist(0xD372, S2C_OnGetRedeemItemList);
		//		MsgHander.Regist(0xD401, S2C_OnGetUseSkillList);
		//GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += ChangeAutoUseSkill;
	}
	/// <summary>
	/// 注销
	/// </summary>
	protected virtual void UnRegist(MainPlayerMng _main)
	{

		//371出售  372 回购 373 购买次数   375 请求
	//	MsgHander.UnRegist(0xD372, S2C_OnGetRedeemItemList);
		//		MsgHander.UnRegist(0xD100, S2C_OnGetSkillList);
		//		MsgHander.UnRegist(0xD401, S2C_OnGetUseSkillList);
		//GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= ChangeAutoUseSkill;
	}
	#endregion

	#region 通信S2C
	/// <summary>
	/// 获得可以购回的物品
	/// </summary>
	/// <param name="_info">Info.</param>
	private void S2C_OnGetRedeemItemList(Pt _pt)
	{
		



	}


	#endregion

	#region C2S	




	#endregion

	#region 辅助逻辑




	#endregion
}
