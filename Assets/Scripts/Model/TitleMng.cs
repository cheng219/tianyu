//===================================
//作者：黄洪兴
//日期: 2016/1/18
//用途：玩家称号管理类
//=====================================



using UnityEngine;
using System.Collections;
using st.net.NetBase;
using System;
using System.Collections.Generic;

public class TitleMng 
{
    #region 构造
    /// <summary>
    /// 返回该管理类的唯一实例 by 贺丰
    /// </summary>
    /// <returns></returns>
    public static TitleMng CreateNew(ref TitleMng _titleMng)
    {
        if (_titleMng == null)
        {
            TitleMng titleMng = new TitleMng();
            titleMng.Init();
            return titleMng;
        }
        else
        {
            _titleMng.UnRegist();
            _titleMng.Init();
            return _titleMng;
        }
    }
    /// <summary>
    /// 注册
    /// </summary>
    protected virtual void Init()
    {
        MsgHander.Regist(0xD422, S2C_GetTitleList);
		MsgHander.Regist(0xD423, S2C_GetUseTitle);
		GetAllTitle();
    }
    /// <summary>
    /// 注销
    /// </summary>
    protected virtual void UnRegist()
    {
        MsgHander.UnRegist(0xD422, S2C_GetTitleList);
		MsgHander.UnRegist(0xD423, S2C_GetUseTitle);
        ResetData();
    }
    #endregion

    #region 数据



    /// <summary>
    /// 所有称号增加的属性
    /// </summary>
    public List<AttributePair> AllAttrNum = new List<AttributePair>();
	/// <summary>
	/// 排列顺序后的称号数据
	/// </summary>
	public List<TitleInfo> titleList = new List<TitleInfo>();
    /// <summary>
    /// 玩家当前的称号数据
    /// </summary>
    private FDictionary titleDictionary = new FDictionary();
    public FDictionary TitleDictionary
    {
        get { return titleDictionary; }
    }


    public TitleInfo TargetTitle = null;

	/// <summary>
	/// 已经拥有的称号
	/// </summary>
	private List<TitleInfo> OwnTitleDictionary = new List<TitleInfo>();
    /// <summary>
    /// 当前使用的称号
    /// </summary>
	private TitleInfo curUseTitle=null;
    public TitleInfo CurUseTitle
    {
        get { return curUseTitle; }
        set 
        {
            if (curUseTitle != value)
            {
                curUseTitle = value;
				if (curUseTitle != null) {
					GameCenter.mainPlayerMng.MainPlayerInfo.UpdateTitle (curUseTitle.ID);
				} else {
					GameCenter.mainPlayerMng.MainPlayerInfo.UpdateTitle (0);
				}
            }
        }
    }

    private bool isFirstTime=true;

    public TitleInfo chooseTitle;
	/// <summary>
	/// 当前选中的称号
	/// </summary>
    public TitleInfo ChooseTitle
    {
        get
        {
            return chooseTitle;
        }
        set
        {
            chooseTitle = value;
        }
    }



    /// <summary>
    /// 新获得的称号
    /// </summary>
    public TitleInfo NewTitle;

    /// <summary>
    /// 称号数据更新事件
    /// </summary>
    public Action UpdateTitle;

    /// <summary>
    /// 预览称号更新事件
    /// </summary>
    public Action UpDateTargetTitle;
    #endregion
    #region S2C
    /// <summary>
    /// 获取称号列表
    /// </summary>
    private void S2C_GetTitleList(Pt _pt)
    {
		pt_title_list_d422 pt = _pt as pt_title_list_d422; 
        if (pt != null)
        {
            AllAttrNum.Clear();
			OwnTitleDictionary.Clear ();
			//GetAllTitle();
			List<title_base_info_list> list = pt.title_list;
            for (int i = 0; i < list.Count; i++)
            {

                TitleInfo info = new TitleInfo(list[i]); 
                if (!isFirstTime)
                {
                    TitleInfo oldInfo = titleDictionary[info.ID] as TitleInfo;
                    if (oldInfo != null)
                    {
                        if (!oldInfo.IsOwn)
                        {
                            NewTitle = info;
                            GameCenter.curMainPlayer.StopForNextMove();
                            GameCenter.uIMng.GenGUI(GUIType.NEWTITLEMSG,true);
                        }
                    }

                }
                titleDictionary[info.ID] = info;
				OwnTitleDictionary.Add(info);
				if (list [i].put_state == 1) {

					CurUseTitle = (titleDictionary [info.ID])as TitleInfo;
				}
                for (int j = 0; j < info.Attribute.Count; j++)
                {
                    ActorPropertyTag act = (ActorPropertyTag)info.Attribute[j].eid;
                        AllAttrNum.Add(new AttributePair(act, info.Attribute[j].count));
                } 

            }
            if (UpdateTitle != null)
                UpdateTitle();
            if(isFirstTime)
            isFirstTime = false;
        }
		SortTitle ();
        GameCenter.coupleMng.GeTTitleRef();
    }
    /// <summary>
    /// 切换当前使用的称号
    /// </summary>
    private void S2C_GetUseTitle(Pt _pt)
    {
		pt_update_title_d423 pt = _pt as pt_update_title_d423;
        if (pt != null)
        {
			if (pt.state == 0)
            {
                CurUseTitle = null;
            }
            else
            {
				CurUseTitle = (titleDictionary[(int)pt.title_id]) as TitleInfo;
            }
        } 
        if (UpdateTitle != null)
            UpdateTitle();
    }
    #endregion
    #region C2S
    /// <summary>
    /// 请求称号数据
    /// </summary>
    public void C2S_AskTitle()
    {
		pt_req_title_list_d424 msg = new pt_req_title_list_d424();
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 使用称号
    /// </summary>
	public void C2S_UseTitle(int _titleID,int _state)
    {
		pt_req_change_title_d425 msg = new pt_req_change_title_d425();
		msg.title_id =(uint) _titleID;
		msg.state =(uint) _state;
        NetMsgMng.SendMsg(msg);
    }
    #endregion



    
    /// <summary>
    /// 重置数据
    /// </summary>
    void ResetData()
    {
        AllAttrNum.Clear();
        titleList.Clear();
        titleDictionary.Clear();
        OwnTitleDictionary.Clear();
        curUseTitle = null;
        isFirstTime = true;
        ChooseTitle = null;
        NewTitle = null;
    }


	/// <summary>
	/// 称号排序
	/// </summary>
	void SortTitle()
	{
		int a = 0;
		//int b = 0;
		titleList.Clear ();
		if (CurUseTitle != null) {
			titleList.Add (CurUseTitle);
		}


        for (int i = 0; i < OwnTitleDictionary.Count; i++)
        {
            if (CurUseTitle != null)
            {
                if (OwnTitleDictionary[i]!= CurUseTitle)
                {
                    titleList.Add(OwnTitleDictionary[i]);
                    a++;
                }
            }
            else
            {
                titleList.Add(OwnTitleDictionary[i]);
                //Debug.Log ("当前第一个添加的是"+item.Value.Name);
                a++;
            }
            
        }
		foreach (var item in titleDictionary.Values) {
            TitleInfo Info = item as TitleInfo;
            if (Info != null)
            {
                if (!Info.IsOwn)
                {
                    titleList.Add(Info);
                }
            }
		}
	}

    /// <summary>
    /// 获取配置的所有称号
    /// </summary>
    void GetAllTitle()
    {
        titleDictionary.Clear();
        List<TitleRef> list = ConfigMng.Instance.TitlesList();
        for (int i = 0; i < list.Count; i++)
        {
			TitleInfo info = new TitleInfo(list[i].type);
            if (info.ID == 25 || info.ID == 26 || info.ID == 27 || info.ID == 28)
            {
                continue;
            }
            titleDictionary[info.ID] = info;
        }
    }



}
