//==============================================
//作者：黄洪兴
//日期：2016/4/19
//用途：市场管理类
//=================================================



using UnityEngine;
using System.Collections;
using st.net.NetBase;
using System;
using System.Collections.Generic;

public class MarketMng 
{
	/// <summary>
	/// 大分页数据
	/// </summary>
	public List<MarketTypeInfo> MarketTypes=new List<MarketTypeInfo>();

	/// <summary>
	/// 拍卖的物品数据
	/// </summary>
    public Dictionary<int, List<MarketItemInfo>> MarketItems = new Dictionary<int, List<MarketItemInfo>>();

	/// <summary>
	/// 我的拍卖的物品数据
	/// </summary>
    public Dictionary<int, List<MarketItemInfo>> MyMarketItems = new Dictionary<int, List<MarketItemInfo>>();
	/// <summary>
	/// 当前购买方式
	/// </summary>
	public int buyType=0;
	/// <summary>
	/// 当前选择上架的物品
	/// </summary>
	public EquipmentInfo CurMarketItem;
	/// <summary>
	/// 当前选择购买的物品
	/// </summary>
	public MarketItemInfo CurAuctionItem;
    /// <summary>
    /// 当前的界面
    /// </summary>
    public int CurWnd=1;

    public string marketPageUI;


	public  int type=0;
	public  int page=0;
	public  int lev=0;
	public  int quality=0;
	public  int price=1;
	public  int moneyType=0;
    public int marketPage = 1;
    public int targetPage = 1;

	/// <summary>
	///选择大分页事件
	/// </summary>
	public Action<int,bool> OnChooseType;
	/// <summary>
	/// 筛选条件变化
	/// </summary>
	public Action OnLimitChange;
	/// <summary>
	/// 拍卖物品
	/// </summary>
	public Action OnAuctionItem;

    /// <summary>
    /// 拍卖物品数据变化
    /// </summary>
    public Action OnMarketItemUpdate;

    /// <summary>
    /// 我的拍卖物品数据变化
    /// </summary>
    public Action OnMyMarketItemUpdate;


    public Action OnMarketItemFeedBack;

    public bool RefreshScrollView = true;

	#region 构造
	/// <summary>
	/// 返回该管理类的唯一实例
	/// </summary>
	/// <returns></returns>
	public static MarketMng CreateNew(MainPlayerMng _main)
	{
		if (_main.marketMng == null)
		{
			MarketMng MarketMng = new MarketMng();
			MarketMng.Init(_main);
			return MarketMng;
		}
		else
		{
			_main.marketMng.UnRegist(_main);
			_main.marketMng.Init(_main);
			return _main.marketMng;
		}
	}
	/// <summary>
	/// 注册
	/// </summary>
	protected virtual void Init(MainPlayerMng _main)
	{
        for (int i = 0; i < ConfigMng.Instance.AllMarketTypeItem.Count; i++)
        {
            MarketTypes.Add(new MarketTypeInfo(i + 1));
        }
        
		InitLimit (0);
        MsgHander.Regist(0xD550, S2C_GetMarketItem);
        MsgHander.Regist(0xD555, S2C_GetMarketItemFeedBack);
		//GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += ChangeAutoUseSkill;
	}
	/// <summary>
	/// 注销
	/// </summary>
	protected virtual void UnRegist(MainPlayerMng _main)
	{
        MsgHander.UnRegist(0xD550, S2C_GetMarketItem);
        MsgHander.UnRegist(0xD555, S2C_GetMarketItemFeedBack);
		//GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= ChangeAutoUseSkill;
        ResetData();
	}


        /// <summary>
    /// 重置数据
    /// </summary>
    void ResetData()
    {
        MarketTypes.Clear();
        MarketItems.Clear();
        MyMarketItems.Clear();
        buyType = 0;
        CurMarketItem = null;
        CurAuctionItem = null;
    }

	#endregion

	#region 通信S2C
    

    /// <summary>
    /// 获得拍卖物品数据
    /// </summary>
    /// <param name="_pt"></param>
    protected void S2C_GetMarketItem(Pt _pt)
    {
        pt_shelve_items_info_d550 pt = _pt as pt_shelve_items_info_d550;
        if (pt!= null)
        {
            if (marketPage != (int)pt.page)
                RefreshScrollView = false;
            if (pt.shelve_item_list.Count>0)
            marketPage = (int)pt.page;
            if (pt.type == 1)
            {
                //MarketItems.Clear();
                List<MarketItemInfo> List = new List<MarketItemInfo>();
                for (int i = 0; i < pt.shelve_item_list.Count; i++)
                {
                    MarketItemInfo info = new MarketItemInfo(pt.shelve_item_list[i]);
                    List.Add(info);
                }
                MarketItems[(int)pt.page] = List;
                if (OnMarketItemUpdate != null && targetPage == (int)pt.page )
                {
                    OnMarketItemUpdate();
                }
                RefreshScrollView = true;
            //    Debug.Log("收到拍卖行数据");
            }
            if (pt.type == 2)
            {
                //MyMarketItems.Clear();
                List<MarketItemInfo> List = new List<MarketItemInfo>();
                for (int i = 0; i < pt.shelve_item_list.Count; i++)
                {
                    MarketItemInfo info = new MarketItemInfo(pt.shelve_item_list[i]);
                    List.Add(info);
                }
                MyMarketItems[(int)pt.page] = List;
                if (OnMyMarketItemUpdate != null && targetPage == marketPage)
                {
                    OnMyMarketItemUpdate();
                }
                RefreshScrollView = true;
             //   Debug.Log("收到我的拍卖行数据");
            }
        }

       // Debug.Log("收到数据后页数为" + marketPage);
    }


    protected void S2C_GetMarketItemFeedBack(Pt _pt)
    {
       // Debug.Log("收到拍卖行反馈");
        pt_shelve_item_chg_d555 pt = _pt as pt_shelve_item_chg_d555;
        if (pt != null)
        {
            if (OnMarketItemFeedBack!=null)
            {
                OnMarketItemFeedBack();
            }

        }


    }
    


	#endregion

	#region C2S	
    /// <summary>
    /// 按照搜索条件请求拍卖物品数据
    /// </summary>
    /// <param name="num"></param>
	public void C2S_AskMarketItem(int _marketPage)
	{
        //Debug.Log("请求的数据页数为"+_marketPage);
        if (_marketPage == 1)
        {
            MarketItems.Clear();
            MyMarketItems.Clear();
        }
        pt_req_shelve_items_d549 msg = new pt_req_shelve_items_d549();
        string sort = string.Empty;
        if (type > 0)
        {
            MarketRef marketRef = ConfigMng.Instance.GetMarketTypeRef(type);
            if (page > 0)
            {
//				Debug.Log(marketRef.Paessage.Count+":"+type+":"+page);
                string[] str = marketRef.Paessage[page - 1].Split(',');
                if (marketRef.Page == "sort")
                {
                    for (int i = 0; i < str.Length; i++)
                    {
                        msg.sort.Add(Convert.ToInt32(str[i]));
                        msg.prof = 0;
                        sort = sort + str[i];
                    }
                }
                if (marketRef.Page == "req_prof")
                {
                    for (int i = 0; i < marketRef.Tabmessage.Count; i++)
                    {
                        string[] s = marketRef.Tabmessage[i].Split(',');
                        for (int j = 0; j < s.Length; j++)
                        {
                            msg.sort.Add(Convert.ToInt32(s[j]));
                            sort = sort + s[j].ToString();
                        }
                        //				msg.sort.Add(Convert.ToInt32(marketRef.Tabmessage[i]));
                        //                  sort = sort+marketRef.Tabmessage[i].ToString();

                    }
                    msg.prof = (byte)Convert.ToInt32(marketRef.Paessage[page - 1]);
                }
            }
            else
            {
                for (int i = 0; i < marketRef.Tabmessage.Count; i++)
                {
					string[] s = marketRef.Tabmessage[i].Split (',');
					for (int j = 0; j <s.Length; j++) {
						msg.sort.Add(Convert.ToInt32(s[j]));
						sort = sort+s[j].ToString();
					}
	//				msg.sort.Add(Convert.ToInt32(marketRef.Tabmessage[i]));
  //                  sort = sort+marketRef.Tabmessage[i].ToString();

                }
            }

        }
       
                msg.lev = (uint)lev;
                msg.color = (byte)quality;
                msg.currency = (byte)moneyType;
                msg.index = (byte)price;
                targetPage = _marketPage;
                for (int i = 0; i < _marketPage; i++)
                {
                    msg.page = (uint)(i+1);
                    NetMsgMng.SendMsg(msg);   
                }
               // Debug.Log("请求拍卖行数据，SORT为"+sort+"等级限制为"+lev+"品质限制为"+quality+"货币限制为"+moneyType+"升序降序为"+price+"页数为"+marketPage+"职业为"+msg.prof);
	}
    /// <summary>
    /// 购买物品
    /// </summary>
    public void C2S_AuctionMarketItem(int _id,int _num)
    {
        pt_req_buy_shelve_item_d551 msg = new pt_req_buy_shelve_item_d551();
        msg.id = (uint)_id;
        msg.num = (uint)_num;
        NetMsgMng.SendMsg(msg);
       // Debug.Log("请求购买拍卖物品ID"+_id+"数量"+_num);
    }

    /// <summary>
    /// 下架物品
    /// </summary>
    public void C2S_GetBackMarketItem(int _id)
    {
        pt_req_dump_shelve_item_d552 msg = new pt_req_dump_shelve_item_d552();
        msg.id = (uint)_id;
        NetMsgMng.SendMsg(msg);
       // Debug.Log("请求下架物品ID"+_id);

    }

    /// <summary>
    /// 请求我上架的物品数据
    /// </summary>
    public void C2S_AskMyMarketItem()
    {
        pt_req_get_my_shelve_item_info_d553 msg = new pt_req_get_my_shelve_item_info_d553();
        NetMsgMng.SendMsg(msg);
      //  Debug.Log("请求我上架的物品数据");
    }


    public void C2S_PutawayMarketItem(int _id, int _price, int _moneyType,int _num,int _broadcast)
    {
        pt_req_ground_shelve_item_d554 msg = new pt_req_ground_shelve_item_d554();
        msg.id =(uint) _id;
        msg.broadcast=(byte)_broadcast;
        msg.num = (uint)_num;
        msg.price = (uint)_price;
        msg.resource=(byte)_moneyType;
        NetMsgMng.SendMsg(msg);
      //  Debug.Log("请求上架物品ID" + _id + "价格" + _price + "货币种类" + msg.resource + "数量" + _num + "是否使用喇叭" + _broadcast);
    }


		
	#endregion

	#region 辅助逻辑

	public void OpenPutawayWnd(EquipmentInfo _info)
	{
		CurMarketItem = _info;
		GameCenter.uIMng.SwitchToUI (GUIType.PUTAWAY, GUIType.MARKET);
		
	}

	public void InitMarketTypeInfo()
	{
		for (int i = 0; i < MarketTypes.Count; i++) {
			MarketTypes [i].ShowPage = false;
		}
	}

	/// <summary>
	/// 按照条件筛选拍卖的物品
	/// </summary>
	public void ScreenOutMarketItems(int _type,int _page,int _lev,int _quality,int _price,int _moneyType)
	{
	}

	/// <summary>
	/// 设置筛选条件
	/// </summary>
	/// <param name="_class">Class.</param>
	/// <param name="_num">Number.</param>
    public 	void InitLimit(int _class,int _num=0)
	{
		if (_class == 0) {
			type = 0;
			page = 0;
			lev = 0;
			quality = 0;
			price = 1;
			moneyType = 0;
			marketPage = 1;
		}
		switch(_class)
		{
		case 1:type = _num;page = 0;break;
		case 2:page = _num;break;
		case 3:lev = _num;break;
		case 4:quality = _num;break;
		case 5:price = _num;break;
		case 6:moneyType = _num;break;
		default: break;
		}

		if (OnLimitChange != null)
			OnLimitChange ();
	}


	#endregion
}