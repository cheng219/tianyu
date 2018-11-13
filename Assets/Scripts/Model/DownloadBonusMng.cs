//==============================================
//作者：朱素云
//日期：2016/10/26
//用途：下载奖励管理类
//=================================================



using UnityEngine;
using System.Collections;
using st.net.NetBase;
using System;
using System.Collections.Generic;

public class DownloadBonusMng 
{


	/// <summary>
	///交易成功的事件
	/// </summary>
	public Action OnTrade;
    public DownLoadProType curDownLoadProType = DownLoadProType.LOADSTART; 
	#region 构造
	/// <summary>
	/// 返回该管理类的唯一实例 
	/// </summary>
	/// <returns></returns>
	public static DownloadBonusMng CreateNew(MainPlayerMng _main)
	{
		if (_main.downloadBonusMng == null)
		{
			DownloadBonusMng DownloadBonusMng = new DownloadBonusMng();
			DownloadBonusMng.Init(_main);
			return DownloadBonusMng;
		}
		else
		{
			_main.downloadBonusMng.UnRegist(_main);
			_main.downloadBonusMng.Init(_main);
			return _main.downloadBonusMng;
		}
	}
	/// <summary>
	/// 注册
	/// </summary>
	protected virtual void Init(MainPlayerMng _main)
	{
        MsgHander.Regist(0xD822, S2C_OnGetDownReward);

		//MsgHander.Regist(0xD372, S2C_OnGetRedeemItemList);
		//		MsgHander.Regist(0xD401, S2C_OnGetUseSkillList);
		//GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += ChangeAutoUseSkill;
	}
	/// <summary>
	/// 注销
	/// </summary>
	protected virtual void UnRegist(MainPlayerMng _main)
	{
        MsgHander.UnRegist(0xD822, S2C_OnGetDownReward);
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
    /// <summary>
    /// 获取下载奖励  0、成功；   1、token失效 已领取。  2、内部异常
    /// </summary>
    protected void S2C_OnGetDownReward(Pt _pt)
    {
        pt_download_reward_result_d822 msg = _pt as pt_download_reward_result_d822;
        if (msg != null)
        { 
        
        }
    }
	#endregion

	#region C2S	
    /// <summary>
    /// 请求领取下载奖励
    /// </summary>
    public void C2S_ReqGetDownReward(string _str)
    {
        pt_req_download_reward_d821 msg = new pt_req_download_reward_d821();
        msg.sdktoken = _str;
        NetMsgMng.SendMsg(msg);
    }


	#endregion

	#region 辅助逻辑




	#endregion
}
/// <summary>
/// 下载奖励过程
/// </summary>
public enum DownLoadProType
{ 
    /// <summary>
    /// 开始下载
    /// </summary>
    LOADSTART = 0,
    /// <summary>
    /// 下载中
    /// </summary>
    LOADON = 1,
    /// <summary>
    /// 下载完成
    /// </summary>
    LOADOVER = 2,
}