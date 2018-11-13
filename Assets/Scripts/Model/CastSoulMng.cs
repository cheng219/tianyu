//==============================================
//作者：黄洪兴
//日期：2016/4/1
//用途：铸魂管理类
//=================================================



using UnityEngine;
using System.Collections;
using st.net.NetBase;
using System;
using System.Collections.Generic;

public class CastSoulMng 
{
    protected int curSoulNum = 0;
    /// <summary>
    /// 当前铸魂次数
    /// </summary>
    public int CurSoulNum
    {
        get
        {
            return curSoulNum;
        }
        set
        {
            if (curSoulNum != value)
            { 
                curSoulNum = value;
                if (UpdateSoulReward != null)
                    UpdateSoulReward();
            }
        }
    }
    protected int curSoulRewardId = 0;
    /// <summary>
    /// 当前可以领取的铸魂进度奖励
    /// </summary>
    public int CurSoulRewardId
    {
        get
        {
            return curSoulRewardId;
        }
        set
        { 
                curSoulRewardId = value;
                if (UpdateSoulReward != null)
                    UpdateSoulReward(); 
        }
    }

	/// <summary>
	/// 普通铸魂次数
	/// </summary>
	public Dictionary<int,int> CommonCastSoulNum = new Dictionary <int ,int> ();
    /// <summary>
    /// 高级铸魂次数
    /// </summary>
	public Dictionary<int,int> AdvancedCastSoulNum = new Dictionary <int ,int> ();



    public Action OnCastSoulCrit;
	/// <summary>
	/// 铸魂次数更新事件
	/// </summary>
	public Action UpdateSoulNum;
    /// <summary>
    /// 铸魂奖励更新
    /// </summary>
    public Action UpdateSoulReward;
 
	#region 构造
	/// <summary>
	/// 返回该管理类的唯一实例
	/// </summary>
	/// <returns></returns>
	public static CastSoulMng CreateNew(MainPlayerMng _main)
	{
		if (_main.castSoulMng == null)
		{
			CastSoulMng CastSoulMng = new CastSoulMng();
			CastSoulMng.Init(_main);
			return CastSoulMng;
		}
		else
		{
			_main.castSoulMng.UnRegist(_main);
			_main.castSoulMng.Init(_main);
			return _main.castSoulMng;
		}
	}
	/// <summary>
	/// 注册
	/// </summary>
	protected virtual void Init(MainPlayerMng _main)
	{
        MsgHander.Regist(0xD777, S2C_GetAllSoulCrit);
		MsgHander.Regist(0xD446, S2C_GetAllSoulNum);
		MsgHander.Regist(0xD447, S2C_GetUpdateSoulNum);
        MsgHander.Regist(0xC101, S2C_GetUpdateSoulReward);
		C2S_AskSoulNum ();
		//		MsgHander.Regist(0xD401, S2C_OnGetUseSkillList);
		//GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += ChangeAutoUseSkill;
	}
	/// <summary>
	/// 注销
	/// </summary>
	protected virtual void UnRegist(MainPlayerMng _main)
	{
        MsgHander.UnRegist(0xD777, S2C_GetAllSoulCrit);
		MsgHander.UnRegist(0xD446, S2C_GetAllSoulNum);
		MsgHander.UnRegist(0xD447, S2C_GetUpdateSoulNum);
        MsgHander.UnRegist(0xC101, S2C_GetUpdateSoulReward);
        ResetData();
		//		MsgHander.UnRegist(0xD401, S2C_OnGetUseSkillList);
		//GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= ChangeAutoUseSkill;
	}
	#endregion

	#region 通信S2C

    /// <summary>
    /// 铸魂暴击特效
    /// </summary>
    /// <param name="_pt"></param>
    private void S2C_GetAllSoulCrit(Pt _pt)
    {
        pt_cast_soul_crit_d777 pt = _pt as pt_cast_soul_crit_d777;
        if (pt != null)
        {
            if (OnCastSoulCrit != null)
                OnCastSoulCrit();
        }
        //Debug.Log ("获得铸魂次数列表");
    }



	/// <summary>
	/// 请求所有种类魂的剩余次数
	/// </summary>
	/// <param name="_pt">Point.</param>
	private  void S2C_GetAllSoulNum(Pt _pt)
	{
		pt_cast_soul_info_d446 pt = _pt as  pt_cast_soul_info_d446;
		if (pt != null)
		{
			CommonCastSoulNum.Clear ();
			AdvancedCastSoulNum.Clear ();
            CurSoulNum = pt.cur_cast_soul; 
			for (int i = 0; i < pt.base_soul.Count; i++) {
				CommonCastSoulNum [(int)pt.base_soul [i].id] =(int) pt.base_soul [i].num;
			}
			for (int i = 0; i < pt.advanced_soul.Count; i++) {
				AdvancedCastSoulNum [(int)pt.advanced_soul [i].id] =(int) pt.advanced_soul [i].num;
			} 
		} 

		if (UpdateSoulNum != null) {

			UpdateSoulNum ();
		}
		//Debug.Log ("获得铸魂次数列表");
	}
	/// <summary>
	/// 单个魂种类剩余次数更新
	/// </summary>
	/// <param name="_pt">Point.</param>
	private  void S2C_GetUpdateSoulNum(Pt _pt)
	{
		pt_update_cast_soul_num_d447 pt = _pt as  pt_update_cast_soul_num_d447;
		if(pt != null)
		{
            int remainNum = 0;

			if (pt.state == 1) {
				CommonCastSoulNum [pt.soul_id] = pt.surplus_num; 
			}
			if (pt.state == 2) {
				AdvancedCastSoulNum [pt.soul_id] = pt.surplus_num; 
			}

            using (var e = AdvancedCastSoulNum.GetEnumerator())
            {
                while (e.MoveNext())
                { 
                    remainNum += e.Current.Value;
                }
            }
            using (var e = CommonCastSoulNum.GetEnumerator())
            {
                while (e.MoveNext())
                { 
                    remainNum += e.Current.Value;
                }
            }
             
            int allSoulNum = (GameCenter.vipMng.VipData.CastSoulNum * 5) * 2;//普通 + 高级
             
            CurSoulNum = allSoulNum - remainNum;
		}
		if (UpdateSoulNum != null) {

			UpdateSoulNum ();
		}
		//Debug.Log ("获得单个铸魂次数更新");
	}

    /// <summary>
    /// 获取当前可领取铸魂类型奖励id
    /// </summary>
    /// <param name="_pt"></param>
    private void S2C_GetUpdateSoulReward(Pt _pt)
    {
        pt_update_cast_soul_reward_c101 pt = _pt as pt_update_cast_soul_reward_c101;
        if (pt != null)
        {
            //Debug.Log("获取当前已经领取铸魂类型奖励 c101 : " + pt.get_reward_id);
            CurSoulRewardId = pt.get_reward_id;
        } 
    }

	#endregion

	#region C2S	
	/// <summary>
	/// 获得所有铸魂次数
	/// </summary>
	/// <param name="_pt">Point.</param>
	public  void C2S_AskSoulNum()
	{
		pt_req_soul_info_d444 msg = new pt_req_soul_info_d444();
		NetMsgMng.SendMsg(msg);

		//Debug.Log ("发送请求铸魂次数协议");
	}
	/// <summary>
	/// 铸魂
	/// </summary>
	/// <param name="_pt">Point.</param>
	public  void C2S_CastSoul(int type,int state)
	{
		pt_req_cast_soul_d445 msg = new pt_req_cast_soul_d445();
		msg.soul_id = type;
		msg.state = state;
		NetMsgMng.SendMsg(msg);
        //if (firstTime)
        //{
        //    firstTime = false;
        //    GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.CASTINGSOUL, false);
        //}
		//Debug.Log ("发送铸魂协议");
	}

    /// <summary>
    /// 请求领取铸魂进度奖励
    /// </summary>
    /// <param name="_id"></param>
    public void C2S_AskSoulReward()
    {
        //Debug.Log(" 请求领取铸魂进度奖励  " + (CurSoulRewardId + 1));
        pt_req_cast_soul_reward_c100 msg = new pt_req_cast_soul_reward_c100();
        msg.reward_id = CurSoulRewardId + 1;
        NetMsgMng.SendMsg(msg); 
    }
	#endregion

	#region 辅助逻辑
     /// <summary>
    /// 重置数据
    /// </summary>
    void ResetData()
    { 
        CommonCastSoulNum.Clear();
        AdvancedCastSoulNum.Clear(); 
        curSoulNum = 0;
        curSoulRewardId = 0;
    }

    //public  bool GetCastSoulRed()
    //{
    //    if (isOpened) return false;
    //    //if (GameCenter.mainPlayerMng==null||GameCenter.mainPlayerMng.VipData == null || GameCenter.mainPlayerMng.VipData.RefData == null)
    //    //    return false;
    //    //int canUseNum = GameCenter.mainPlayerMng.VipData.RefData.cast_soul_num;
    //    using (var e = CommonCastSoulNum.GetEnumerator())
    //   {
    //       while (e.MoveNext())
    //       {
    //           if (e.Current.Value > 0)
    //               return true;
    //       }

    //   }

    //    using (var e = AdvancedCastSoulNum.GetEnumerator())
    //    {
    //        while (e.MoveNext())
    //        {
    //            if (e.Current.Value > 0)
    //                return true;
    //        }

    //    }
    //    return false;

    //}



	#endregion
}
public enum SoulType
{
	NONE,
	FIRE=1,//火之魂
	ICE=2,//冰之魂
	WATER=3,//水之魂
	WIND=4,//风之魂
	THUNDER=5,//雷之魂
}