//==============================================
//作者：黄洪兴
//日期：2016/3/24
//用途：在线奖励管理类
//=================================================



using UnityEngine;
using System.Collections;
using st.net.NetBase;
using System;
using System.Collections.Generic;

public class OnlineRewardMng 
{
	/// <summary>
	/// 在线奖励数据
	/// </summary>
	public List<OnlineRewardItemInfo> OnlineRewardItem = new List<OnlineRewardItemInfo> ();
     
    public OnlineRewardItemInfo TrueCurInfo;

    private bool isGetedAll=false;
    /// <summary>
    /// 是否领取全部在线奖励
    /// </summary>
    public bool IsGetedAll
    {
        get
        {
            return isGetedAll;
        }
    }

    /// <summary>
    /// 当前抽中的奖励
    /// </summary>
    public int curLottryNum = -1;

    /// <summary>
    /// 当前领取在线奖励id
    /// </summary>
	private int curRewardItem=-1;

	/// <summary>
	/// 当前查看的奖励
	/// </summary>
	public int CurRewardItem
	{
		get{
			return curRewardItem;
		}

		set{
            if (value < 0)
            { 
                curRewardItem = -1;
            }
            else
            {
                if (value >= OnlineRewardItem.Count)
                {
                    curRewardItem = OnlineRewardItem.Count - 1;
                }
                else
                {
                    curRewardItem = value;
                }
            }
		}
	}

    /// <summary>
    /// 已经领取过的奖励id
    /// </summary>
    public List<int> alreadyTakeReward = new List<int>();


	/// <summary>
	/// 收到在线奖励事件
	/// </summary>
	public Action OnGetOnlineRewardInfo;
    public Action<int> OnBginTotateUpdate;

	#region 构造
	/// <summary>
	/// 返回该管理类的唯一实例 
	/// </summary>
	/// <returns></returns>
	public static OnlineRewardMng CreateNew(MainPlayerMng _main)
	{
		if (_main.onlineRewardMng == null)
		{
			OnlineRewardMng OnlineRewardMng = new OnlineRewardMng();
			OnlineRewardMng.Init(_main);
			return OnlineRewardMng;
		}
		else
		{
			_main.onlineRewardMng.UnRegist(_main);
			_main.onlineRewardMng.Init(_main);
			return _main.onlineRewardMng;
		}
	}
	/// <summary>
	/// 注册
	/// </summary>
	protected virtual void Init(MainPlayerMng _main)
	{

        GetAllOnlineReward();
        MsgHander.Regist(0xD763, S2C_GetOnlineRewardItemInfo);
        MsgHander.Regist(0xC107, S2C_OnlineRewardLottery);
        C2S_AskOnlineRewardInfo(2);
		//GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += ChangeAutoUseSkill;
	}
	/// <summary>
	/// 注销
	/// </summary>
	protected virtual void UnRegist(MainPlayerMng _main)
	{

        MsgHander.UnRegist(0xD763, S2C_GetOnlineRewardItemInfo);
        MsgHander.UnRegist(0xC107, S2C_OnlineRewardLottery);
        ResetData();
	}
	#endregion

	#region 通信S2C


    public void S2C_GetOnlineRewardItemInfo(Pt _pt)
    {
        pt_update_online_reward_d763 pt = _pt as pt_update_online_reward_d763;
         if (pt != null)
         {
             GetAllOnlineReward();
             alreadyTakeReward.Clear(); 
             alreadyTakeReward = pt.reward_id_list;
             //Debug.Log(" d763 :  " + (pt.reward_id-1) + "   ,time : " + pt.remain_time + "     allCount : " + OnlineRewardItem.Count + "    ,alreadyTakeCount : " + pt.reward_id_list.Count);
             CurRewardItem = pt.reward_id - 1;
             if (pt.reward_id_list.Count >= OnlineRewardItem.Count)
             {
                 isGetedAll = true;
                 //for (int i = 0; i < OnlineRewardItem.Count; i++)
                 //{
                 //       OnlineRewardItem[i].IsGeted = true;
                 //}
                 if (OnGetOnlineRewardInfo != null)
                     OnGetOnlineRewardInfo();
                 return;

             }
            
             OnlineRewardItemInfo Info = new OnlineRewardItemInfo(pt); 
             OnlineRewardItem[pt.reward_id - 1] = Info;
             TrueCurInfo = Info;
             //for (int i = 0; i < OnlineRewardItem.Count; i++)
             //{
             //    if (i < curRewardItem)
             //        OnlineRewardItem[i].IsGeted = true;
                 
             //}
             if (OnGetOnlineRewardInfo != null)
                 OnGetOnlineRewardInfo();
             if (pt.remain_time > 0)
             {
                 GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.ONLINEREWARDS, false);
             }
             else
             {
                 GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.ONLINEREWARDS, true);
             }

         } 
    }

    public void S2C_OnlineRewardLottery(Pt _pt) 
    { 
        pt_online_choujiang_id_c107 pt = _pt as pt_online_choujiang_id_c107;
        if (pt != null)
        { 
            curLottryNum = pt.reward_id;
            if (OnBginTotateUpdate != null) OnBginTotateUpdate(curLottryNum);
            //if (pt.reward_id >= OnlineRewardItem.Count)  这里注释掉,不然每次抽到双倍符就出错了 by邓成
            //{
            //    isGetedAll = true; 
            //    if (OnGetOnlineRewardInfo != null)
            //        OnGetOnlineRewardInfo();
            //    return;

            //} 
        }
    }

	#endregion

	#region C2S	

    /// <summary>
    /// 请求在线奖励信息
    /// </summary>
    public void C2S_AskOnlineRewardInfo(int _type)
    {
        pt_req_lev_reward_info_d760 msg = new pt_req_lev_reward_info_d760();
        msg.reward_type = _type;
        NetMsgMng.SendMsg(msg);
        //Debug.Log("请求在线奖励数据");
    }

    /// <summary>
    /// 请求领取在线奖励type:抽奖0/领奖1
    /// </summary>
    public void C2S_AskGetOnlineReward(int _id)
    {
        //Debug.Log(" 请求领取在线奖励  :  " + _id); 
        pt_req_get_online_reward_d764 msg = new pt_req_get_online_reward_d764();
        msg.reward_id = _id;
        NetMsgMng.SendMsg(msg);
    }

	#endregion

	#region 辅助逻辑

     /// <summary>
    /// 重置数据
    /// </summary>
    void ResetData()
    {
        OnlineRewardItem.Clear();
        TrueCurInfo = null; 
        isGetedAll = false;
        curRewardItem = -1;
        curLottryNum = -1;
        alreadyTakeReward.Clear();
    }


    void GetAllOnlineReward()
    {
        OnlineRewardItem.Clear();
        List<OnlineRewardRef> list = new List<OnlineRewardRef>();
        list = ConfigMng.Instance.GetAllOnlineRewardRef();
        for (int i = 0; i < list.Count; i++)
        {
            OnlineRewardItem.Add(new OnlineRewardItemInfo(list[i].id));
        }

    }

	#endregion
}

