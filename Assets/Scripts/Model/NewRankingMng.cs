//====================
//作者：鲁家旗
//日期：2016/4/19
//用途：排行榜管理类
//====================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class NewRankingMng
{
    #region 数据
    /// <summary>
    /// 排行榜数据
    /// </summary>
    protected Dictionary<int, NewRankingInfo> rankingDic = new Dictionary<int, NewRankingInfo>();
    /// <summary>
    /// 排行榜数据
    /// </summary>
    public Dictionary<int, NewRankingInfo> RankingDic
    { 
        get
        {
            return rankingDic;
        }
    }
    /// <summary>
    /// 当前选中的排行榜人物ID
    /// </summary>
    protected int curChooseRankPlayerId = 0;
    /// <summary>
    /// 当前选中的排行榜人物ID
    /// </summary>
    public int CurChooseRankPlayerId
    {
        get
        {
            return curChooseRankPlayerId;
        }
        set
        {
            curChooseRankPlayerId = value;
        }
    }
    /// <summary>
    /// 自身排名
    /// </summary>
    public int myRank = -1;
    /// <summary>
    /// 自身数据1
    /// </summary>
    public int myValue1 = -1;
    /// <summary>
    /// 自身数据2
    /// </summary>
    public int myValue2 = -1;
    /// <summary>
    /// 排行榜数据发生变化
    /// </summary>
    public System.Action OnRankingUpdate;
    /// <summary>
    /// 排行榜中当前选中的人物信息
    /// </summary>
    public PlayerBaseInfo CurRankPlayerInfo;
    /// <summary>
    /// 点击查看的那个人的ID
    /// </summary>
    public int CurOtherId = 0;

    /// <summary>
    /// 收到排行榜信息协议
    /// </summary>
    public System.Action<pt_ranklist_d601> OnGetRankingInfo;
    /// <summary>
    /// 其他玩家实例
    /// </summary>
    public GameObject otherGo = null;
    /// <summary>
    /// 当前选中的排行榜
    /// </summary>
    public int curChooseRank = 0;
    #endregion

    #region 构造
    /// <summary>
    /// 返回一个全新的排行榜管理类实例
    /// </summary>
    public static NewRankingMng CreateNew()
    {
        if (GameCenter.newRankingMng == null)
        {
            NewRankingMng newRankingMng = new NewRankingMng();
            newRankingMng.Init();
            return newRankingMng;
        }
        else
        {
            GameCenter.newRankingMng.UnRegist();
            GameCenter.newRankingMng.Init();
            return GameCenter.newRankingMng;
        }
    }
    void Init()
    {
        MsgHander.Regist(0xD601, S2C_GetRankList);
    }
    void UnRegist()
    {
        MsgHander.UnRegist(0xD601, S2C_GetRankList);
        rankingDic.Clear();
        curChooseRankPlayerId = 0;
        myRank = -1;
        myValue1 = -1;
        myValue2 = -1;
        CurOtherId = 0;
        CurRankPlayerInfo = null;
        otherGo = null;
    }
    #endregion

    #region 协议
    #region  S2C服务度发往客户端的协议
    protected void S2C_GetRankList(Pt _msg)
    {
        //Debug.Log("接收pt_ranklist_d601 协议！");
        pt_ranklist_d601 msg = _msg as pt_ranklist_d601;
        if (msg != null)
        {
			if(GameCenter.activityMng != null)GameCenter.activityMng.GotGuildDartRankList(msg);
            if (GameCenter.treasureTroveMng != null)GameCenter.treasureTroveMng.S2C_GetRreasurePlayerRank(msg);
            rankingDic.Clear();
            myRank = msg.rank;
            myValue1 = msg.value1;
            myValue2 = msg.value2;
            for (int i = 0; i < msg.ranklist.Count; i++)
            {
                rank_info_base data = msg.ranklist[i];
                if (!rankingDic.ContainsKey((int)data.id))
                {
                    NewRankingInfo info = new NewRankingInfo(data);
                    rankingDic[(int)data.id] = info;
                }
            }

            if (OnGetRankingInfo != null)
            {
                OnGetRankingInfo(msg);
            }
        }
        if (OnRankingUpdate != null)
        {
            OnRankingUpdate();
        }
    }
    #endregion                                                                                                                                                                                                                                                                          
    #region C2S客户端发往服务度的协议
    /// <summary>
    /// 请求排行榜信息
    /// </summary>
    /// <param name="_type"></param>
    public void C2S_ReqGetRank(int _type,int _page)
    {
        pt_ranklist_d600 msg = new pt_ranklist_d600();
        msg.type = (byte)_type;
        msg.page = (byte)_page;
        NetMsgMng.SendMsg(msg);
    }
    #endregion
    #endregion
}

