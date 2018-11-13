//==================================
//作者：黄洪兴
//日期：2016/3/15
//用途：时装管理类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;



public class FashionMng
{
    /// <summary>
    /// 时装等级
    /// </summary>
    public int fashionLev = 0;

    /// <summary>
    /// 时装经验
    /// </summary>
    public int fashionExp = 0;

	/// <summary>
	/// 已经拥有的时装集合
	/// </summary>
	public List<FashionInfo> ownFashionList=new List<FashionInfo>();


    /// <summary>
    /// 临时时装集合
    /// </summary>
    public List<FashionInfo> tempFashionList = new List<FashionInfo>();

	/// <summary>
	/// 衣服时装列表
	/// </summary>
	public Dictionary<int,FashionInfo> clothesFashionDic=new Dictionary<int,FashionInfo>();
    public List<FashionInfo> clothesFashionList = new List<FashionInfo>();
	/// <summary>
	/// 武器时装列表
	/// </summary>
	public Dictionary<int,FashionInfo> weaponFashionDic= new Dictionary<int,FashionInfo>();

    public List<FashionInfo> weaponFashionList = new List<FashionInfo>();
	/// <summary>
	/// 所有时装的列表
	/// </summary>
	public Dictionary<int,FashionInfo> fashionDic=new Dictionary<int,FashionInfo>();


    public Dictionary<int, FashionInfo> cacheFashionDic = new Dictionary<int, FashionInfo>();
    /// <summary>
    /// 所有时装增加的属性
    /// </summary>
    public List<AttributePair> AllAttrNum=new List<AttributePair>() ;


    /// <summary>
    /// 当前需要打开的界面类型
    /// </summary>
    public FashionWndType CurFashionWndType=FashionWndType.CLOTHES;


    private FashionInfo inUseClothesFashion;
	/// <summary>
	/// 当前穿戴的衣服时装
	/// </summary>
	/// <value>The in use fashion.</value>
	public FashionInfo InUseClothesFashion
	{
        get { return inUseClothesFashion; }
		set 
		{
            if (inUseClothesFashion != value)
			{
                inUseClothesFashion = value;
                if (inUseClothesFashion != null)
                {
                    GameCenter.mainPlayerMng.MainPlayerInfo.UpdateClothesFashion(inUseClothesFashion.FashionID);
				} else {
                    GameCenter.mainPlayerMng.MainPlayerInfo.UpdateClothesFashion(0);
				}
			}
		}
	}


    private FashionInfo inUseWeaponFashion;
    /// <summary>
    /// 当前穿戴的武器时装
    /// </summary>
    /// <value>The in use fashion.</value>
    public FashionInfo InUseWeaponFashion
    {
        get { return inUseWeaponFashion; }
        set
        {
            if (inUseWeaponFashion != value)
            {
                inUseWeaponFashion = value;
                if (inUseWeaponFashion != null)
                {
                    GameCenter.mainPlayerMng.MainPlayerInfo.UpdateWeaponFashion(inUseWeaponFashion.FashionID);
                }
                else
                {
                    GameCenter.mainPlayerMng.MainPlayerInfo.UpdateWeaponFashion(0);
                }
            }
        }
    }

    private FashionInfo curTargetFashion;
    /// <summary>
    /// 当前需要操作的时装
    /// </summary>
    public FashionInfo CurTargetFashion
    {
        get
        {
            return curTargetFashion;
        }
        set
        {
            curTargetFashion = value;
			if (CurTargetFashion != null && fashionDic.ContainsKey(CurTargetFashion.FashionID))
            {
                    curTargetFashion.SetOwn(fashionDic[curTargetFashion.FashionID].IsOwn);
            }
        }
    }


    bool isFirstTime=true;

	#region 构造
	public static FashionMng CreateNew(MainPlayerMng _main)
	{
		if (_main.fashionMng == null)
		{
			FashionMng FashionMng = new FashionMng();
			FashionMng.Init(_main);
			return FashionMng;
		}
		else
		{
			_main.fashionMng.UnRegist(_main);
			_main.fashionMng.Init(_main);
			return _main.fashionMng;
		}
	}



	/// <summary>
	/// 注册
	/// </summary>
	protected void Init(MainPlayerMng _main)
	{
		MsgHander.Regist (0xD412, S2C_GetFashionList);
		MsgHander.Regist (0xD414, S2C_PutFashion);
        MsgHander.Regist(0xD788, S2C_UpdateFashionLev);
		//GetAllFashion ();
		C2S_AskFashionDic ();
	}
	/// <summary>
	/// 注销
	/// </summary>
	protected void UnRegist(MainPlayerMng _main)
	{
		MsgHander.UnRegist(0xD412, S2C_GetFashionList);
		MsgHander.UnRegist(0xD414, S2C_PutFashion);
        MsgHander.UnRegist(0xD788, S2C_UpdateFashionLev);
        ResetData();

	}
		
	#endregion
	/// <summary>
	/// 时装列表更新事件
	/// </summary>
	public Action OnUpdateFashionList;
	/// <summary>
	/// 时装穿戴事件
	/// </summary>
	public Action OnUpdateFashion;

    public Action OnChangeTargetFashion;
    //FashionInfo firstClothesInfo;
    //FashionInfo firstWeaponInfo;
    /// <summary>
    /// 时装等级更新
    /// </summary>
    public Action OnUpdateFashionLev;
	#region S2C 通信

	/// <summary>
	/// 获取所有的时装
	/// </summary>
	/// <param name="_pt"></param>
	protected void S2C_GetFashionList(Pt _pt)
	{
		pt_model_clothes_list_d412 pt = _pt as pt_model_clothes_list_d412;
        //Debug.Log("收到时装协议");
		if (pt != null) {
             
            fashionLev = pt.model_lev;
            fashionExp = pt.model_exp;

            if (fashionDic.Count < 1)
            {
                GetAllFashion();
            }
            RefreshTemporaryFashion();
			if (fashionDic != null) {
				//GetAllTitle ();
				ownFashionList.Clear ();
                tempFashionList.Clear();
                AllAttrNum.Clear();
                InUseClothesFashion = null;
                InUseWeaponFashion = null;
			}
            for (int i = 0; i < pt.model_list.Count; i++)
            {
                RemainTime time = null;
                if (pt.model_list[i].remain_time > 0)
                {
                    time = new RemainTime(pt.model_list[i].remain_time, Time.time);
                }
                FashionInfo fashion = new FashionInfo(pt.model_list[i].model_id, pt.model_list[i].own_state, pt.model_list[i].put_state, time);
                if (!isFirstTime)
                {
                    if (!cacheFashionDic.ContainsKey(fashion.FashionID))
                    {
                        if (fashion.FashionType == 1)
                            CurFashionWndType = FashionWndType.WEAPON;
                        if (fashion.FashionType == 2)
                            CurFashionWndType = FashionWndType.CLOTHES;
                        GameCenter.uIMng.SwitchToSubUI(SubGUIType.SUBFASHION);
                    }
                }
                fashionDic[fashion.FashionID] = fashion;
                if (pt.model_list[i].put_state == 1)
                {
                    if (fashion.FashionType == 1)
                        InUseWeaponFashion = fashion;
                    if (fashion.FashionType == 2)
                        InUseClothesFashion = fashion;
                }
                if (pt.model_list[i].own_state == 1)
                {
                    ownFashionList.Add(fashion);
                }
                if (fashion.FashionType == 1)
                {
                    weaponFashionDic[fashion.FashionID] = fashion;
                    if (fashion.RemainTime != null)
                    {
                        if (fashion.TempID != 0)
                        {
                            if (weaponFashionDic.ContainsKey(fashion.TempID))
                                weaponFashionDic.Remove(fashion.TempID);
                        }
                    }
                    else
                    {
                        if (fashion.ForeverTempID != 0)
                        {
                            if (weaponFashionDic.ContainsKey(fashion.ForeverTempID))
                                weaponFashionDic.Remove(fashion.ForeverTempID);
                        }
                    }
                }
                if (fashion.FashionType == 2)
                {
                    clothesFashionDic[fashion.FashionID] = fashion;
                    if (fashion.RemainTime != null)
                    {
                        if (fashion.TempID != 0)
                        {
                            if (clothesFashionDic.ContainsKey(fashion.TempID))
                                clothesFashionDic.Remove(fashion.TempID);
                        }
                    }
                    else
                    {
                        if (fashion.ForeverTempID != 0)
                        {
                            if (clothesFashionDic.ContainsKey(fashion.ForeverTempID))
                                clothesFashionDic.Remove(fashion.ForeverTempID);
                        }
                    }
                }
                if (pt.model_list[i].remain_time > 0)
                {
                    tempFashionList.Add(fashion);
                   // Debug.Log ("时装列表发来的时间为"+pt.model_list [i].remain_time);
                }
               // Debug.Log("时装列表的长度为" + pt.model_list.Count + "时装ID为" + pt.model_list[i].model_id);

                cacheFashionDic[fashion.FashionID] = fashion;
            }

            if (ownFashionList.Count > 0)
            {
                for (int i = 0; i < ownFashionList.Count; i++)
                {
                    for (int j = 0; j < ownFashionList[i].Attribute.Count; j++)
                    { 
                        ActorPropertyTag act = (ActorPropertyTag)ownFashionList[i].Attribute[j].eid;
                            AllAttrNum.Add(new AttributePair(act, ownFashionList[i].Attribute[j].count));
                    } 
                }
            }
            if (tempFashionList.Count > 0)
            {
                for (int i = 0; i < tempFashionList.Count; i++)
                {
                    for (int j = 0; j < tempFashionList[i].Attribute.Count; j++)
                    { 
                        ActorPropertyTag act = (ActorPropertyTag)tempFashionList[i].Attribute[j].eid;
                            AllAttrNum.Add(new AttributePair(act, tempFashionList[i].Attribute[j].count));
                    }
                }
            }

		}

        if (isFirstTime)
        {
            isFirstTime = false;
        }
        SortFashion();
		if (OnUpdateFashionList != null) {
			OnUpdateFashionList ();
		}
        //Debug.Log("此时的衣服时装ID" + GameCenter.mainPlayerMng.MainPlayerInfo.GetServerData().clothesFashionID + "武器ID" + GameCenter.mainPlayerMng.MainPlayerInfo.GetServerData().weaponFashionID);
	}
	/// <summary>
	/// 获得穿上的时装
	/// </summary>
	/// <param name="_pt"></param>
	protected void S2C_PutFashion(Pt _pt)
	{
		pt_updata_model_clothes_d414 pt = _pt as pt_updata_model_clothes_d414;
        if (pt != null)
        {
            RemainTime time = null;
            if (pt.time > 0)
            {
                time = new RemainTime(pt.time, Time.time);
            }
            FashionInfo F = new FashionInfo(pt.model_id);
            if (pt.put_state == 1)
            {
                FashionInfo fashion = new FashionInfo(pt.model_id, 1, pt.put_state, time);
                if (fashion.FashionType == 1)
                    InUseWeaponFashion = fashion;
                if (fashion.FashionType == 2)
                    InUseClothesFashion = fashion;
            }
            if (pt.put_state == 0)
            {
                if (F.FashionType == 1)
                    InUseWeaponFashion = null;
                if (F.FashionType == 2)
                    InUseClothesFashion = null;
            }
            GameCenter.mainPlayerMng.MainPlayerInfo.UpdateEquipment(F.Item, (pt.put_state == 1)); //eid为时装的物品表ID
        }
        //if (OnUpdateFashion != null) {
        //    OnUpdateFashion ();
        //}
        if (OnChangeTargetFashion != null)
            OnChangeTargetFashion();
	}

    /// <summary>
    /// 更新时装等级和经验
    /// </summary>
    protected void S2C_UpdateFashionLev(Pt _pt)
    {
        pt_update_model_lev_exp_d788 pt = _pt as pt_update_model_lev_exp_d788;
        if (pt != null)
        { 
            fashionLev = pt.lev; 
            fashionExp = pt.exp;
            if (OnUpdateFashionLev != null) OnUpdateFashionLev();
        }
    }
	#endregion

	#region C2S通信
	/// <summary>
	/// 请求时装列表
	/// </summary>
	public void C2S_AskFashionDic()
	{
		pt_req_model_clothes_d416 msg = new pt_req_model_clothes_d416();
		NetMsgMng.SendMsg(msg);
        //Debug.Log("请求时装数据");
	}

	/// <summary>
	/// 穿脱时装 0脱时装 1穿时装 2将时装变为永久
	/// </summary>
	public void C2S_AskPutFashion(int id,int state)
	{
		pt_req_put_model_d413 msg = new pt_req_put_model_d413();
		msg.model_id = id;
		msg.state = state;
		NetMsgMng.SendMsg(msg);
	}

	#endregion


    /// <summary>
    /// 重置数据
    /// </summary>
    void ResetData()
    {
        ownFashionList.Clear();
        tempFashionList.Clear();
        clothesFashionDic.Clear();
        weaponFashionDic.Clear();
        fashionDic.Clear();
        cacheFashionDic.Clear();
        AllAttrNum.Clear();
        CurFashionWndType = FashionWndType.CLOTHES;
        inUseClothesFashion = null;
        inUseWeaponFashion = null;
        CurTargetFashion = null;
        isFirstTime = true;
        fashionLev = 0;
        fashionExp = 0;
    }

	/// <summary>
	/// 获取配置的所有时装
	/// </summary>
    public 	void GetAllFashion()
	{
		fashionDic.Clear();
		weaponFashionDic.Clear ();
		clothesFashionDic.Clear ();
		List<FashionRef> list = ConfigMng.Instance.FashionList ();
		for (int i = 0; i < list.Count; i++)
		{
            if (list[i] == null)
                continue;
			FashionInfo info = new FashionInfo(list[i].id);
			fashionDic[info.FashionID] = info;
            if (list[i].type == 1 && list[i].prof==GameCenter.mainPlayerMng.MainPlayerInfo.Prof)
            {
                if (info.Time==0)
				weaponFashionDic[info.FashionID]= info;
			}
            if (list[i].type == 2 && list[i].prof == GameCenter.mainPlayerMng.MainPlayerInfo.Prof)
            {
                if (info.Time == 0)
				clothesFashionDic[info.FashionID]= info;
			}
		}
        SetCurFashionInfo();
	}

   public  void SetCurFashionInfo()
    {
        //Debug.Log("修改TYPE:" + CurFashionWndType);
        switch (CurFashionWndType)
        {
            case FashionWndType.CLOTHES: if (clothesFashionList.Count>0) CurTargetFashion = clothesFashionList[0]; break;
            case FashionWndType.WEAPON: if (weaponFashionList.Count>0) CurTargetFashion = weaponFashionList[0]; break;
            default: if (clothesFashionList.Count > 0) CurTargetFashion = clothesFashionList[0]; break;
        }
        //Debug.Log("修改TYPE:" + CurFashionWndType + ":" + CurTargetFashion.FashionID + ":" + firstClothesInfo.FashionID + ":" + firstWeaponInfo.FashionID);
    }

    /// <summary>
    /// 打开时装称号界面
    /// </summary> 
   public void OpenFinshionTitleWnd()
   {
       GameCenter.fashionMng.CurFashionWndType = FashionWndType.TITLE;
       GameCenter.uIMng.SwitchToSubUI(SubGUIType.SUBFASHION);
   }

    void RefreshTemporaryFashion()
    {
        List<int> targetList=new List<int>();
        using (var e = fashionDic.GetEnumerator())
        {
            while(e.MoveNext())
            {
                if (e.Current.Value.FashionRef.time != 0)
                {
                    targetList.Add(e.Current.Value.FashionID);
                }
            }
        }
        for (int i = 0; i < targetList.Count; i++)
        {
            if (fashionDic.ContainsKey(targetList[i]))
            fashionDic[targetList[i]] = new FashionInfo(targetList[i]);
        }

    }


    /// <summary>
    /// 变为永久
    /// </summary>
    public void ToFover()
    {
        if (CurTargetFashion != null)
        {
            GameCenter.fashionMng.C2S_AskPutFashion(CurTargetFashion.FashionID, 2);
        }
    }


    /// <summary>
    /// 时装排序
    /// </summary>
    void SortFashion()
    {
        weaponFashionList.Clear();
        clothesFashionList.Clear();
        using (var e = weaponFashionDic.GetEnumerator())
        {
            while (e.MoveNext())
            {
                if (e.Current.Value.IsPut)
                {
                    weaponFashionList.Add(e.Current.Value);
                }
            }

        }
        using (var e = weaponFashionDic.GetEnumerator())
        {
            while (e.MoveNext())
            {
                if ((e.Current.Value.IsOwn || e.Current.Value.RemainTime != null) && !e.Current.Value.IsPut && !weaponFashionList.Contains(e.Current.Value))
                    weaponFashionList.Add(e.Current.Value);
            }

        }
        using (var e = weaponFashionDic.GetEnumerator())
        {
            while (e.MoveNext())
            {
                if (!weaponFashionList.Contains(e.Current.Value))
                    weaponFashionList.Add(e.Current.Value);
            }

        }
        using (var e = clothesFashionDic.GetEnumerator())
        {
            while (e.MoveNext())
            {
                if (e.Current.Value.IsPut)
                {
                    clothesFashionList.Add(e.Current.Value);
                }
            }

        }
        using (var e = clothesFashionDic.GetEnumerator())
        {
            while (e.MoveNext())
            {
                if ((e.Current.Value.IsOwn || e.Current.Value.RemainTime != null) && !e.Current.Value.IsPut && !clothesFashionList.Contains(e.Current.Value))
                {
                    clothesFashionList.Add(e.Current.Value);
                }
            }

        }
        using (var e = clothesFashionDic.GetEnumerator())
        {
            while (e.MoveNext())
            {
                if (!clothesFashionList.Contains(e.Current.Value))
                {
                    clothesFashionList.Add(e.Current.Value);
                }
            }

        }

        SetCurFashionInfo();
    }



}

public enum FashionWndType
{
    /// 衣服
    /// </summary>
    CLOTHES = 0,
    /// <summary>
    /// 武器
    /// </summary>
    WEAPON = 1,
    /// <summary>
    /// 称号
    /// </summary>
    TITLE = 2,

}

