
/// <summary>
/// add 何明军
/// VIP 数据.
/// 2016/6/22
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VIPDataInfo {
	/// <summary>
	/// 等级
	/// </summary>
	public int vLev;
	/// <summary>
	/// 经验
	/// </summary>
	public int vExp;
	/// <summary>
	/// 已经领取奖励
	/// </summary>
	public List<int> vipReward = new List<int>();
	
	public VIPDataInfo (pt_vip_info_d329 info){
		vLev = info.vip_lev;
		vExp = info.vip_exp;
		vipReward = info.already_get_reward_lev_list;
	} 
	
	VIPRef refData;
	/// <summary>
	/// 静态数据
	/// </summary>
	public VIPRef RefData{
		get{
			if(refData == null){
				return ConfigMng.Instance.GetVIPRef(vLev);
			}
			if(refData != null && refData.id != vLev){
				return ConfigMng.Instance.GetVIPRef(vLev);
			}
			return refData;
		}
	}
	/// <summary>
	/// 每日运镖的总次数
	/// </summary>
	public int DartNum{
		get{
			return RefData != null ? RefData.Dart_num:0;
		}
	}
	/// <summary>
	/// 是否满级
	/// </summary>
	public bool IsFullLevel{
		get{
			return vLev >= ConfigMng.Instance.GetVIPRefTable().Count - 1;
		}
	}

    /// <summary>
    /// 可铸魂次数
    /// </summary>
    public int CastSoulNum
    {
        get
        {
            return RefData != null ? RefData.cast_soul_num : 0;
        }
    }
    /// <summary>
    /// 挂机副本购买怪物的次数
    /// </summary>
    public int HangUpMaxBuyTimes
    {
        get
        {
            return RefData != null ? RefData.hook_times : 0;
        }
    }
}
