//==================================
//作者：黄洪兴
//日期：2016/7/9
//用途：复活管理类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class RebornMng
{

    #region 数据
    protected int successTime = 0;
    /// <summary>
    /// 收到复活成功的时间
    /// </summary>
    public int SuccessTime
    {
        get
        {
            return successTime;
        }
    }
    protected int rebornTimes = -1;
    /// <summary>
    /// 当日复活次数
    /// </summary>
    public int RebornTimes
    {
        get
        {
            return rebornTimes;
        }
    }
    MainPlayerInfo mainPlayerInfo = null;


    #endregion

    #region 构造
    public  static RebornMng CreateNew(MainPlayerMng _main)
    {
        if (_main.rebornMng == null)
        {
            RebornMng rebornMng = new RebornMng();
            rebornMng.Init(_main);
            return rebornMng;
        }
        else
        {
            _main.rebornMng.UnRegist();
            _main.rebornMng.Init(_main);
            return _main.rebornMng;
        }
    }

    void Init(MainPlayerMng _main)
    {
        mainPlayerInfo = _main.MainPlayerInfo;
        MsgHander.Regist(0xD205, S2C_Reborn);
        MsgHander.Regist(0xD207, S2C_GetRebornTime);
    }
    void UnRegist()
    {
        MsgHander.UnRegist(0xD205, S2C_Reborn);
        MsgHander.UnRegist(0xD207, S2C_GetRebornTime);

    }
    #endregion

    #region 委托
    public System.Action<int> OnRebornEvent;

    #endregion

    #region 协议

    #region S2C
    /// <summary>
    /// 服务端确认复活
    /// </summary>
    /// <param name="_pt"></param>
    protected void S2C_Reborn(Pt _pt)
    {
        pt_revive_d205 msg = _pt as pt_revive_d205;
        int id = (int)msg.revive_uid;

        if (id == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
        {
            //if(msg.revive_sort == (int)RebornType.REBORNCURMAP)
            //successTime = (int)Time.time;
            mainPlayerInfo.Reborn(msg);
            if (OnRebornEvent != null)
                OnRebornEvent((int)msg.revive_sort);
        }
        else
        {
            OtherPlayerInfo opc = GameCenter.sceneMng.GetOPCInfo(id);
            if (opc != null)
            {
                opc.Reborn(msg);
            }
        }


    }
    /// <summary>
    /// 获得复活次数
    /// </summary>
    /// <param name="_pt"></param>
    protected void S2C_GetRebornTime(Pt _pt)
    {
        pt_revive_times_d207 info = _pt as pt_revive_times_d207;
        rebornTimes = (int)info.times;
    }
    #endregion

    #region C2S
    /// <summary>
    /// 请求复活
    /// </summary>
    /// <param name="_sort"></param>
    /// <param name="_action"></param>
    public void C2S_AskReborn(int _sort,int _action)
    {
        pt_action_int_d003 msg = new pt_action_int_d003();
        msg.action = _action;
        msg.data = _sort;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求当日复活次数
    /// </summary>
    public void C2S_GetRebornTimes()
    {
        pt_action_d002 msg = new pt_action_d002();
        msg.action = 18;
        NetMsgMng.SendMsg(msg);
    }
    #endregion


    #endregion

    #region 辅助逻辑
    #endregion
}
/// <summary>
/// 复活方式
/// </summary>
public enum RebornType
{ 
    /// <summary>
    /// 回安全点复活
    /// </summary>
    REBORNTOCITY =1,
    /// <summary>
    /// 当前地图原地复活
    /// </summary>
    REBORNCURMAP =2,
    /// <summary>
    /// 副本里复活点复活
    /// </summary>
    REBORNCURPOINT =3,
}
