//===============================
//作者：邓成
//日期：2016/7/7
//用途：每日必做管理类
//===============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class DailyMustDoMng{
	/// <summary>
	/// 每日必做数据变化事件
	/// </summary>
	public System.Action OnDailyMustDoUpdateEvent;
    /// <summary>
    /// 活跃度奖励领取变化时间
    /// </summary>
    public System.Action OnDailyMustDoStateUpdateEvent;
	/// <summary>
	/// 当前每日必做数据
	/// </summary>
	public Dictionary<int,DailyMustDoInfo> CurLivelyDic= new Dictionary<int,DailyMustDoInfo>();
    /// <summary>
    /// bool表示是否领取
    /// </summary>
    public Dictionary<int, bool> MustDoStateDic = new Dictionary<int, bool>();
    /// <summary>
    /// 当前活跃度值
    /// </summary>
    public int CurLivelyCount = 0;

    public int TotalLivelyCount = 250;

	#region 构造
	public static DailyMustDoMng CreateNew()
	{
		if(GameCenter.dailyMustDoMng == null)
		{
			GameCenter.dailyMustDoMng = new DailyMustDoMng();
			GameCenter.dailyMustDoMng.Init();
			return GameCenter.dailyMustDoMng;
		}else
		{
			GameCenter.dailyMustDoMng.UnRegister();
			GameCenter.dailyMustDoMng.Init();
			return GameCenter.dailyMustDoMng;
		}
	}
	void Init()
	{
		MsgHander.Regist(0xD692,S2C_GotMustDoData);
        MsgHander.Regist(0xC103, S2C_GotMustDoState);
        //FDictionary mustDoItems = ConfigMng.Instance.GetLivelyRefTable();
        //TotalLivelyCount = 0;
        //foreach (LivelyRef lively in mustDoItems.Values)
        //{
        //    TotalLivelyCount += lively.rewardlive;
        //}
	}
	void UnRegister()
	{
		MsgHander.UnRegist(0xD692,S2C_GotMustDoData);
        MsgHander.UnRegist(0xC103, S2C_GotMustDoState);
		CurLivelyDic.Clear();
	}
	#endregion

	#region 协议
	public void C2S_ReqMustDoData()
	{
		//Debug.Log("C2S_ReqMustDoData");
		pt_req_liveness_state_d693 msg = new pt_req_liveness_state_d693();
		NetMsgMng.SendMsg(msg);
	}
	public void C2S_ReqGetMustDoReward(int _mustDoID)
	{
		//Debug.Log("C2S_ReqGetMustDoReward");
		pt_req_get_live_ness_reward_d691 msg = new pt_req_get_live_ness_reward_d691();
		msg.live_ness_id = _mustDoID;
		NetMsgMng.SendMsg(msg);
	}

    public void C2S_ReqGetLivelyReward(int _livelyID)
    {
        Debug.Log("C2S_ReqGetLivelyReward:" + _livelyID);
        pt_req_liveness_reward_c102 msg = new pt_req_liveness_reward_c102();
        msg.reward_id = _livelyID;
        NetMsgMng.SendMsg(msg);
    }

	protected void S2C_GotMustDoData(Pt _info)
	{
		pt_liveness_state_d692 pt = _info as pt_liveness_state_d692;
		if(pt != null)
		{
			//Debug.Log("S2C_GotMustDoData:"+pt.liveness_list.Count);
			for (int i = 0,max=pt.liveness_list.Count; i < max; i++) {
				//Debug.Log("liveness:"+pt.liveness_list[i].id+",state:"+(DailyMustDoInfo.RewardState)pt.liveness_list[i].reward_state);
				DailyMustDoInfo doInfo = new DailyMustDoInfo(pt.liveness_list[i]);
				CurLivelyDic[pt.liveness_list[i].id] = doInfo;
			}
			if(OnDailyMustDoUpdateEvent != null)
				OnDailyMustDoUpdateEvent();
            CountLivelyAmount();
            if (OnDailyMustDoStateUpdateEvent != null)//计算完活跃度之后也可能需刷新活跃度奖励显示
                OnDailyMustDoStateUpdateEvent();
		}
	}

    protected void S2C_GotMustDoState(Pt _info)
    {
        pt_update_liveness_reward_c103 pt = _info as pt_update_liveness_reward_c103;
        if (pt != null)
        {
            //Debug.Log("S2C_GotMustDoState");
            bool needRedTip = false;
            for (int i = 0,length=pt.liveness_reward.Count; i < length; i++)
            {
                MustDoStateDic[pt.liveness_reward[i].reward_id] = (pt.liveness_reward[i].state == 1);
                //Debug.Log("S2C_GotMustDoState id:" + pt.liveness_reward[i].reward_id + ",state:" + pt.liveness_reward[i].state);
                LivelyRewardRef reward = ConfigMng.Instance.GetLivelyRewardRef(pt.liveness_reward[i].reward_id);
                if (reward != null && pt.liveness_reward[i].state == 0 && reward.need <= CurLivelyCount)
                {
                    needRedTip = true;   
                }
            }
            GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.DAILYMUSTDO, needRedTip);
            if (OnDailyMustDoStateUpdateEvent != null)
                OnDailyMustDoStateUpdateEvent();
        }

    }
	#endregion

	protected void CountLivelyAmount()
	{
		List<DailyMustDoInfo> livelyList = new List<DailyMustDoInfo>(CurLivelyDic.Values);
        CurLivelyCount = 0;
		for (int i = 0,max=livelyList.Count; i < max; i++) {
            DailyMustDoInfo info = livelyList[i];
            if (info.MustDoState == DailyMustDoInfo.RewardState.CANGET || info.MustDoState == DailyMustDoInfo.RewardState.HAVEGOT)
                CurLivelyCount += livelyList[i].CurLivelyCount;
		}
        //Debug.Log("CurLivelyCount:" + CurLivelyCount);
	}

	public List<DailyMustDoInfo> GetDailyMustDoItemByType(MustDoType _type)
	{
		Dictionary<int,DailyMustDoInfo> livelyDic = new Dictionary<int,DailyMustDoInfo>();
		FDictionary livelyRefTable = ConfigMng.Instance.GetLivelyRefTable();
		foreach(var item in livelyRefTable.Values)
		{
			LivelyRef livelyRef = item as LivelyRef;
			if(livelyRef != null && livelyRef.type == (int)_type)
			{
				DailyMustDoInfo doInfo = null;
				if(CurLivelyDic.ContainsKey(livelyRef.id))
				{
					doInfo = new DailyMustDoInfo(CurLivelyDic[livelyRef.id]);//有后台数据
				}else
				{
					doInfo = new DailyMustDoInfo(livelyRef.id);//没有后台数据
				}
				if(livelyDic.ContainsKey(doInfo.Sort))
				{
					livelyDic[doInfo.Sort].UpdateBySort(doInfo);//同类型的只显示一个
				}else
				{
					livelyDic[doInfo.Sort] = doInfo;
				}
			}
		}
		List<DailyMustDoInfo> livelyList = new List<DailyMustDoInfo>(livelyDic.Values);
		livelyList.Sort(SortMustDoInfo);
		return livelyList;
	}

	protected int SortMustDoInfo(DailyMustDoInfo doInfo1,DailyMustDoInfo doInfo2)
	{
        int state1 = (int)doInfo1.MustDoState;
        int state2 = (int)doInfo2.MustDoState;
		if(state1 < state2)//未完成的排前面
			return -1;
		if(state1 > state2)
			return 1;
		if(doInfo1.ID > doInfo2.ID)//状态相同按ID排序
			return 1;
		if(doInfo1.ID < doInfo2.ID)
			return -1;
		return 0;
	}
}
public class DailyMustDoInfo
{
	protected st.net.NetBase.liveness_info serverData = null;

	protected LivelyRef refData = null;
	public LivelyRef RefData
	{
		get
		{
			if(refData == null)
			{
				refData = ConfigMng.Instance.GetlivelyRefByID(ID);
			}
			if(refData == null)
			{
				Debug.LogError("找不到ID为:"+ID+"的每日必做数据!");
			}
			return refData;
		}
	}

	public int ID = 0;
	public RewardState MustDoState
	{
		get
		{
			if(serverData == null)
				return RewardState.CANTGET;
			return (RewardState)serverData.reward_state;
		}
	}
	/// <summary>
	/// 名字
	/// </summary>
	public string Name
	{
		get
		{
			return RefData==null?string.Empty:RefData.name;
		}
	}
	/// <summary>
	/// 描述
	/// </summary>
	public string Des
	{
		get
		{
			return RefData==null?string.Empty:RefData.Des;
		}
	}
	/// <summary>
	/// 图标图片名字
	/// </summary>
	public string IconName
	{
		get
		{
			return RefData==null?string.Empty:RefData.Icon;
		}
	}
	/// <summary>
	/// 奖励
	/// </summary>
	public EquipmentInfo RewardItem
	{
		get
		{
			return RefData==null?null:new EquipmentInfo(RefData.reward,EquipmentBelongTo.PREVIEW);
		}
	}
	/// <summary>
	/// 完成次数
	/// </summary>
	public int FinishTimes
	{
		get
		{
			return serverData == null?0:serverData.condtion_num;
		}
	}
	/// <summary>
	/// 需求次数
	/// </summary>
	public int TotalTimes
	{
		get
		{
			if(RefData != null && RefData.task_condition.Count > 0)
			{
				return RefData.task_condition[0].number;
			}
			return 0;
		}
	}
	/// <summary>
	/// 已完成
	/// </summary>
	public bool IsFinished
	{
		get
		{
			return FinishTimes >= TotalTimes;
		}
	}
	/// <summary>
	/// 每日必做类型
	/// </summary>
	public MustDoType CurMustDoType
	{
		get
		{
			return RefData ==null?MustDoType.NONE:(MustDoType)RefData.type;
		}
	}
	/// <summary>
	/// 几级界面
	/// </summary>
	public int UISort
	{
		get
		{
			return RefData ==null?0:RefData.uinum;
		}
	}
	/// <summary>
	/// 界面枚举字符串
	/// </summary>
	public string UIType
	{
		get
		{
			return RefData ==null?string.Empty:RefData.guitype;
		}
	}

	public string ButtonName
	{
		get
		{
			switch(MustDoState)
			{
			case RewardState.CANGET:
			case RewardState.CANTGET:
				return "领取";
			case RewardState.HAVEGOT:
				return "已领取";
			}
			return "领取";
		}
	}
    /// <summary>
    /// 完成该项可获得的活跃度值
    /// </summary>
    public int StaticLivelyCount
    {
        get
        {
            return RefData == null ? 0 : RefData.rewardlive;
        }
    }
    /// <summary>
    /// 该项每日必做贡献的活跃度值,没完成及没贡献
    /// </summary>
    public int CurLivelyCount
    {
        get
        {
            if (MustDoState == RewardState.CANGET || MustDoState == RewardState.HAVEGOT)
            {
                return StaticLivelyCount;
            }
            return 0;
        }
    }

	public int Sort
	{
		get
		{
			return RefData==null?0:RefData.sort;
		}
	}

	public void UpdateBySort(DailyMustDoInfo doInfo)
	{
		if(doInfo.Sort == Sort)
		{
			if((this.MustDoState == RewardState.CANGET && doInfo.MustDoState == RewardState.CANGET)
				|| (this.MustDoState == RewardState.CANTGET && doInfo.MustDoState == RewardState.CANTGET))
			{
				if(TotalTimes > doInfo.TotalTimes)//都是可领取or不可领取状态,显示完成需求小的
				{
					serverData = doInfo.serverData;
					ID = doInfo.ID;
					refData = null;
				}
			}else if(this.MustDoState == RewardState.HAVEGOT && doInfo.MustDoState == RewardState.HAVEGOT)
			{
				if(TotalTimes < doInfo.TotalTimes)//都是已领取状态,显示完成需求大的
				{
					serverData = doInfo.serverData;
					ID = doInfo.ID;
					refData = null;
				}
			}else if(doInfo.MustDoState == RewardState.CANGET)
			{
				//哪个能领取显示哪个
				serverData = doInfo.serverData;
				ID = doInfo.ID;
				refData = null;
			}else if(this.MustDoState == RewardState.CANGET)
			{
				//不变
			}else
			{
				if(TotalTimes < doInfo.TotalTimes)//显示完成需求大的
				{
					serverData = doInfo.serverData;
					ID = doInfo.ID;
					refData = null;
				}
			}
		}
	}

	public enum RewardState
	{
		NONE,
		/// <summary>
		/// 不可领取
		/// </summary>
		CANTGET = 1,
		/// <summary>
		/// 可领取
		/// </summary>
		CANGET = 2,
		/// <summary>
		/// 已领取
		/// </summary>
		HAVEGOT = 3,
	}
	/// <summary>
	/// 没有后台数据,暂用
	/// </summary>
	public DailyMustDoInfo(int _mustDoID)
	{
		ID = _mustDoID;
	}
	public DailyMustDoInfo(st.net.NetBase.liveness_info _data)
	{
		serverData = _data;
		ID = serverData.id;
	}
	public DailyMustDoInfo(DailyMustDoInfo _info)
	{
		serverData = _info.serverData;
		ID = _info.ID;
	}
}
public enum MustDoType
{
	NONE,
    /// <summary>
    /// 活动
    /// </summary>
    ACTIVITY,
	/// <summary>
	/// 提升
	/// </summary>
	UPGRADE,
}
