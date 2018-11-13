//====================================================
//作者: 黄洪兴
//日期：2016/05/4
//用途：在线奖励数据层对象
//======================================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 在线奖励服务端数据
/// </summary>
public class OnlineRewardItemServerData
{
	public int id;
	public int remainTime;
	public int receiveTime;
	public bool canGet=false;
	public bool isGeted=false;

}
/// <summary>
/// 在线奖励数据
/// </summary>
public class OnlineRewardItemInfo
{
	#region 服务端数据
	OnlineRewardItemServerData onlineRewardItemData;
	#endregion
	#region 静态配置数据
	OnlineRewardRef onlineRewardRef = null;
	public OnlineRewardRef OnlineRewardRef
	{
		get
		{
			if (onlineRewardRef != null) return onlineRewardRef;
			onlineRewardRef = ConfigMng.Instance.GetOnlineRewardRef(onlineRewardItemData.id);
			return onlineRewardRef;
		}
	}
	#endregion

	#region 构造
	public OnlineRewardItemInfo(int id)
	{
        onlineRewardItemData = new OnlineRewardItemServerData();
        onlineRewardItemData.id = id;
        onlineRewardItemData.remainTime = -1;
        onlineRewardItemData.receiveTime = -1;


	}



    public OnlineRewardItemInfo(pt_update_online_reward_d763 info)
	{
        onlineRewardItemData = new OnlineRewardItemServerData();
        onlineRewardItemData.id = info.reward_id;
        onlineRewardItemData.receiveTime = (int)Time.time;
        onlineRewardItemData.remainTime = info.remain_time;
	}
	#endregion

	#region 访问器
	/// <summary>
	/// 奖励ID
	/// </summary>
	public int ID
	{
		get { return onlineRewardItemData.id; }
	}
	/// <summary>
	/// 是否可领取
	/// </summary>
	/// <value><c>true</c> if this instance is foreve; otherwise, <c>false</c>.</value>
    //public bool CanGet = false;
	/// <summary>
	/// 是否领取过
	/// </summary>
	/// <value><c>true</c> if this instance is put; otherwise, <c>false</c>.</value>
    //public bool IsGeted=false;

	/// <summary>
	/// 剩余时间
	/// </summary>
	/// <value>The remain time.</value>
	public int RemainTime
	{
		get{ return onlineRewardItemData.remainTime; }

	}
		
	/// <summary>
	/// 收到数据的时间
	/// </summary>
	/// <value>The remain time.</value>
	public int ReceiveTime
	{
		get{ return onlineRewardItemData.receiveTime; }

	}

	EquipmentInfo rewardItem;
	public EquipmentInfo RewardItem
	{
		get {
            if (OnlineRewardRef != null)
            {
                if (OnlineRewardRef.item.Count == 1)
                {
                    return new EquipmentInfo(OnlineRewardRef.item[0].eid, OnlineRewardRef.item[0].count, EquipmentBelongTo.PREVIEW);
                }
                else
                {
                    Debug.LogError("在线奖励配表有问题！！");
                    return null;
                }
            }
            else
            {
                Debug.LogError("在线奖励配表有问题！！");
                return null;
            }
		}
	}






	#endregion
}
