//==============================================
//作者：黄洪兴
//日期：2016/5/12
//用途：复活管理类
//=================================================



using UnityEngine;
using System.Collections;
using st.net.NetBase;
using System;
using System.Collections.Generic;

public class ResurrectionMng
{




    public int GotTime;
    public int ReviveTime;
    /// <summary>
    /// 复活信息
    /// </summary>
    public pt_usr_die_info_update_d737 ResurrectionInfo;

    /// <summary>
    /// 死亡复活类型
    /// </summary>
    public int ResurrectionType=0;




    private int dieNum;
    /// <summary>
    /// 死亡次数
    /// </summary>
    public int DieNum
    {
        get
        {
            return dieNum;
        }
    }
	/// <summary>
	/// 是否是天罚死亡
	/// </summary>
	public bool HeavenDead = false;

    /// <summary>
    /// 获得死亡信息事件
    /// </summary>
    public Action OnGetDieInfo;
	/// <summary>
	/// 天罚死亡时间
	/// </summary>
	public Action OnShowHeavenDeadEvent;

    #region 构造
    /// <summary>
    /// 返回该管理类的唯一实例
    /// </summary>
    /// <returns></returns>
    public static ResurrectionMng CreateNew(MainPlayerMng _main)
    {
        if (_main.resurrectionMng == null)
        {
            ResurrectionMng ResurrectionMng = new ResurrectionMng();
            ResurrectionMng.Init(_main);
            return ResurrectionMng;
        }
        else
        {
            _main.resurrectionMng.UnRegist(_main);
            _main.resurrectionMng.Init(_main);
            return _main.resurrectionMng;
        }
    }
    /// <summary>
    /// 注册
    /// </summary>
    protected virtual void Init(MainPlayerMng _main)
    {
        MsgHander.Regist(0xD737, S2C_GetDieInfo);
		MsgHander.Regist(0xD789, S2C_HeavenDead);
    }
    /// <summary>
    /// 注销
    /// </summary>
    protected virtual void UnRegist(MainPlayerMng _main)
    {
        MsgHander.UnRegist(0xD737, S2C_GetDieInfo);
		MsgHander.UnRegist(0xD789, S2C_HeavenDead);
    }
    #endregion

    #region 通信S2C


   

    /// <summary>
    /// 获得死亡信息
    /// </summary>
    /// <param name="_pt"></param>
    protected void S2C_GetDieInfo(Pt _pt)
    {
        ResurrectionType = 0;
        pt_usr_die_info_update_d737 pt = _pt as pt_usr_die_info_update_d737;
        if (pt!=null)
        {
           // Debug.Log("收到免费复活次数为" + pt.revive_num);
            ResurrectionInfo = pt;
            dieNum = pt.revive_num;
            if (GameCenter.systemSettingMng.IsAutoResurrection)
            {
                C2S_AskAutoResurrection();
            }
            if (pt.count_down != 0)
            {
                GotTime =(int)Time.time;
                ReviveTime = pt.count_down;
            }
            if (pt.kill_name != null && pt.kill_name != string.Empty)
            {
                //if (GameCenter.systemSettingMng.IsAutoResurrection && GameCenter.inventoryMng.GetNumberByType(2600020)>0)
                //{
                //    GameCenter.resurrectionMng.C2S_AskResurrection(2, 0);
                //}
                //else
                //{
                    if (pt.drop_item != 0)
                    {
                        ResurrectionType = 2;
                    }
                    else
                    {
                        ResurrectionType = 3;
                    }
                    if (GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneType == SceneType.SCUFFLEFIELD)
                    {
                        if (GameCenter.friendsMng.allFriendDic.ContainsKey(4))
                        {
                            if (GameCenter.friendsMng.allFriendDic[4].ContainsKey(pt.kill_uid))
                            {
                                if (OnDefeatRecordUpdate != null) OnDefeatRecordUpdate();
                            }
                        }
                    }
                //}
            }
            else
            {
                SceneRef scene = ConfigMng.Instance.GetSceneRef(pt.type);
                if (scene != null)
                { 
                    if (!ConfigMng.Instance.CanUseItemReborn(scene.reborn_type))
                    {
                        if (scene.reborn_type == 7) ResurrectionType = 7;
                        else ResurrectionType = 4;
                    }
                    else
                    {
                        //if (GameCenter.systemSettingMng.IsAutoResurrection && GameCenter.inventoryMng.GetNumberByType(2600020) > 0)
                        //{
                        //    GameCenter.resurrectionMng.C2S_AskResurrection(2, 0);
                        //}
                        //else
                        //{
                            ResurrectionType = 1;
                        //}
                    }

                    //if (ConfigMng.Instance.GetRebornRef(ConfigMng.Instance.GetSceneRef(pt.type).reborn_type).Special)
                    //{
                    //    ResurrectionType = 1;
                    //}
                }
                else
                {
                    Debug.LogError("死亡协议发过来的协议中场景ID没有找到相应配置   by黄洪兴");
                    return;
                }
            }
           // dieNum = pt.revive_num;
            //Debug.Log("复活类型" + ResurrectionType);
            GameCenter.uIMng.SwitchToUI(GUIType.RESURRECTION);
            if (OnGetDieInfo != null)
            {
                OnGetDieInfo();
            }
        }
    }
	/// <summary>
	/// 天罚致死
	/// </summary>
	protected void S2C_HeavenDead(Pt _info)
	{
		pt_wrath_of_heaven_d789 pt = _info as pt_wrath_of_heaven_d789;
		if(pt != null)
		{
			HeavenDead = true;
		}
	}

    #endregion

    #region C2S
    /// <summary>
    /// 请求复活
    /// </summary>
    /// <param name="num"></param>
    public void C2S_AskResurrection(int _type, int _quick_buy)
    {
        pt_req_revive_d738 msg = new pt_req_revive_d738();
        msg.action = _type;
        msg.quick_buy = _quick_buy;
        NetMsgMng.SendMsg(msg);
       // Debug.Log("请求复活，类型为" + _type);
    }

    public void C2S_AskAutoResurrection()
    {
        pt_update_auto_revive_d785 msg = new pt_update_auto_revive_d785();
        msg.auto_revive = 1;
        NetMsgMng.SendMsg(msg);
       // Debug.Log("请求自动复活");

    }


    #endregion


    #region 战败记录
    public System.Action OnDefeatRecordUpdate;
 
    #endregion

}