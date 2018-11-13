//============================
//作者：唐源
//日期：2017/4/10
//用途：宝藏活动管理类(GM后台配置的充值活动)
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;
using System.Text;
public class TreasureTroveMng{
    #region 构造
    public static TreasureTroveMng CreateNew()
    {
        if (GameCenter.treasureTroveMng == null)
        {
            TreasureTroveMng treasureTroveMng = new TreasureTroveMng();
            treasureTroveMng.Init();
            return treasureTroveMng;
        }
        else
        {
            GameCenter.treasureTroveMng.UnRegister();
            GameCenter.treasureTroveMng.Init();
            return GameCenter.treasureTroveMng;
        }
    }
    /// <summary>
    /// 初始化(注册监听)
    /// </summary>
    protected void Init()
    {
        MsgHander.Regist(0xD971, S2C_GetTreasureInfo);
        MsgHander.Regist(0xD975, S2C_GetTreasurebigPrize);
        MsgHander.Regist(0xD973, S2C_GetRreasureRankReward);
        //MsgHander.Regist(0xD601, S2C_GetRreasurePlayerRank);
        MsgHander.Regist(0xD979, S2C_GetOpenTreasureActivity);
        MsgHander.Regist(0xD977, S2C_GetTreasureRewardInfo);
        isOpen = false;
    }
    /// <summary>
    /// 注销
    /// </summary>
    protected void UnRegister()
    {
        MsgHander.UnRegist(0xD971, S2C_GetTreasureInfo);
        MsgHander.UnRegist(0xD975, S2C_GetTreasurebigPrize);
        MsgHander.UnRegist(0xD973, S2C_GetRreasureRankReward);
        //MsgHander.UnRegist(0xD601, S2C_GetRreasurePlayerRank);
        MsgHander.UnRegist(0xD979, S2C_GetOpenTreasureActivity);
        MsgHander.UnRegist(0xD977, S2C_GetTreasureRewardInfo);
        priceOne = 0;
        priceTwo = 0;
        rewardOne = null;
        rewardTwo = null;
        redPoint = false;
        isOpen = false;
        rewardList.Clear();
        treasureRewardList.Clear();
    }
    #endregion
    #region 数据
    #region 活动是否开启
    private bool isOpen = false;
    public bool IsOpen
    {
        get
        {
            return isOpen;
        }
        set
        {
            if (isOpen != value)
            {
                if (isOpen && value == false)
                {
                    MessageST mst = new MessageST();
                    mst.messID = 427;
                    mst.delYes = (x) =>
                        {
                            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                        };
                    GameCenter.messageMng.AddClientMsg(mst);
                }
                isOpen = value;
                if (treasureOpen != null)
                    treasureOpen();
            }
        }
    }
    #endregion 
    #region 宝藏活动预览
    /// <summary>
    /// 充值价位(两个充值价位)
    /// </summary>
    private int priceOne;
    public int PriceOne
    {
        get
        {
            return priceOne;
        }
    }
    private int priceTwo;
    public int PriceTwo
    {
        get
        {
            return priceTwo;
        }
    }
    /// <summary>
    /// 充值之后必定获得的奖励(对应两个价位选择 包含类型 数量)
    /// </summary>
    private ItemValue rewardOne = new ItemValue();
    public ItemValue RewardOne
    {
        get
        {
            return rewardOne;
        }
    }
    private ItemValue rewardTwo = new ItemValue();
    public ItemValue RewardTwo
    {
        get
        {
            return rewardTwo;
        }
    }
    /// <summary>
    /// 奖励列表
    /// </summary>
    private List<EquipmentInfo> rewardList = new List<EquipmentInfo>();
    public List<EquipmentInfo> GetReward()
    {
        return rewardList;
    }
    /// <summary>
    /// 开启宝藏奖励的链表
    /// </summary>
    public List<EquipmentInfo> treasureRewardList = new List<EquipmentInfo>();

    #endregion
    #region 宝藏活动全民赢大奖
    /// <summary>
    /// 红点
    /// </summary>
    private bool redPoint;
    public bool RedPoint
    {
        get
        {
            return redPoint;
        }
    }
    /// <summary>
    /// 活动剩余时间
    /// </summary>
    private int time;
    public int Time
    {
        get
        {
            return time;
        }
    }
    /// <summary>
    /// 宝藏开启次数
    /// </summary>
    private int times;
    public int Times
    {
        get
        {
            return times;
        }
    }
    private List<st.net.NetBase.treasure_times_reward> rewardInfo = new List<st.net.NetBase.treasure_times_reward>();
    public List<st.net.NetBase.treasure_times_reward> GetRewardInfo()
    {
        return rewardInfo;
    }
    #endregion
    #region 宝藏活动至尊豪礼
    /// <summary>
    /// 至尊豪礼活动时间
    /// </summary>
    private int rankTime;
    public int RankTime
    {
        get
        {
            return rankTime;
        }
    }
    /// <summary>
    /// 至尊排行奖励列表
    /// </summary>
    private List<List<ItemValue>> rankRewardList = new List<List<ItemValue>>();
    public List<List<ItemValue>> GetRankReward()
    {
        return rankRewardList;
    }
    /// <summary>
    /// 至尊排行奖励列表描述
    /// </summary>
    private List<string> rankRewardDes = new List<string>();
    public List<string> GetRankRewardDes()
    {
        return rankRewardDes;
    }
    /// <summary>
    /// 玩家自身的排行名次
    /// </summary>
    private int rank;
    public int Rank
    {
        get
        {
            return rank;
        }
    }
    /// <summary>
    /// 玩家自身的开启次数
    /// </summary>
    private int openTimes;
    public int OpenTimes
    {
        get
        {
            return openTimes;
        }
    }
    /// <summary>
    /// 至尊排行数据
    /// </summary>
    private List<st.net.NetBase.rank_info_base> rankList = new List<rank_info_base>();
    public List<st.net.NetBase.rank_info_base> GetrankList()
    {
        return rankList;
    }
    #endregion
    #endregion
    #region 事件
    /// <summary>
    /// 获取褒奖奖励
    /// </summary>
    public Action OnGetTreasureRewardUpdate;
    /// <summary>
    /// 宝藏活动开启事件
    /// </summary>
    public Action treasureOpen;
    /// <summary>
    /// 更新宝藏活动预览数据事件
    /// </summary>
    public Action updateTreasurePreview;
    /// <summary>
    /// 更新宝藏全民赢大奖数据事件
    /// </summary>
    public Action updateTreasurebigPrize;
    /// <summary>
    /// 更新至尊豪礼数据事件
    /// </summary>
    public Action updateTreasureRank;
    /// <summary>
    /// 更新至尊豪礼排行数据事件
    /// </summary>
    public Action updateTreasureRankInfo;
    ///// <summary>
    ///// 收到至尊排行榜信息协议
    ///// </summary>
    //public System.Action<pt_ranklist_d601> GetTreasureRank;
    #endregion

    #region 协议
    #region S2C协议
    /// <summary>
    /// 宝藏活动是否开启
    /// </summary>
    private void S2C_GetOpenTreasureActivity(Pt _pt)
    {
        //Debug.Log("宝藏活动是否开启：" + isOpen);
        pt_treasure_base_info_d979 info = _pt as pt_treasure_base_info_d979;
        if(info !=null)
        {
            IsOpen = (int)info.rest_time > 0 ? true : false;
            Debug.Log("宝藏活动是否开启："+ isOpen);
        }
    }
    /// <summary>
    /// 宝藏活动预览数据
    /// </summary>
    private void S2C_GetTreasureInfo(Pt _pt)
    {
        //Debug.Log("宝藏活动预览数据");
        rewardList.Clear();
        pt_reply_treasure_info_d971 info = _pt as pt_reply_treasure_info_d971;
        if (info != null)
        {
            priceOne =(int)info.price1;
            priceTwo = (int)info.price2;
            rewardOne = new ItemValue((int)info.reward1_item_type, (int)info.reward1_item_amount);
            //Debug.Log("rewardOne:"+ rewardOne.eid);
            rewardTwo = new ItemValue((int)info.reward2_item_type, (int)info.reward2_item_amount);
            for(int i=0,len = info.reward_info.Count;i<len;i++)
            {
                rewardList.Add(new EquipmentInfo((int)info.reward_info[i].item_id, (int)info.reward_info[i].item_num, EquipmentBelongTo.PREVIEW));
            }
            if (updateTreasurePreview != null)
            {
                updateTreasurePreview();
            }
        }
    }
    /// <summary>
    /// 宝藏活动奖励链表
    /// </summary>
    private void S2C_GetTreasureRewardInfo(Pt _pt)
    {
        //Debug.Log("S2C_GetTreasureRewardInfo");
        treasureRewardList.Clear();
        pt_reply_treasure_lottery_d977 info = _pt as pt_reply_treasure_lottery_d977;
        if (info != null)
        { 
            for (int i = 0, len = info.reward_info.Count; i < len; i++)
            {
                //for (int num = 0, max = (int)info.reward_info[i].item_num; num < max; num++)
                //{
                    treasureRewardList.Add(new EquipmentInfo((int)info.reward_info[i].item_id, (int)info.reward_info[i].item_num, EquipmentBelongTo.PREVIEW));
                //}
            }
            if (OnGetTreasureRewardUpdate != null)
            {
                OnGetTreasureRewardUpdate();
            }
        }
        C2S_ReqTreasurebigPrize();
    }

    /// <summary>
    /// 宝藏活动全民赢大奖数据
    /// </summary>
    private void S2C_GetTreasurebigPrize(Pt _pt)
    {
        //Debug.Log("收到全民赢大奖");
        redPoint = false;
        pt_reply_treasure_times_reward_info_d975 info = _pt as pt_reply_treasure_times_reward_info_d975;
        if(info!=null)
        {
            time = (int)info.rest_time;
            times = (int)info.times;
            rewardInfo = info.reward_info;
            IsOpen = (int)info.rest_time > 0 ? true : false;
            Debug.Log("宝藏活动是否开启：" + isOpen);
            for(int i=0,count = info.reward_info.Count;i<count;i++)
            {
                if(info.reward_info[i].status==0&& times>(int)info.reward_info[i].need_times)
                {
                    redPoint = true;
                }
            }
            if (updateTreasurebigPrize != null)
            {
                //Debug.Log("redPoint:"+ redPoint);
                updateTreasurebigPrize();
            }
        }
    }
    /// <summary>
    /// 宝藏活动至尊豪礼奖励数据
    /// </summary>
    private void S2C_GetRreasureRankReward(Pt _pt)
    {
        rankRewardList.Clear();
        pt_reply_treasure_rank_reward_info_d973 info = _pt as pt_reply_treasure_rank_reward_info_d973;
        if(info!=null)
        {
            rankTime = (int)info.rest_time;
            IsOpen = (int)info.rest_time > 0 ? true : false;
            //Debug.Log("宝藏活动是否开启：" + isOpen);
            for(int i=0,len = info.reward_info.Count;i<len;i++)
            {
                List<ItemValue> list = new List<ItemValue>();
                for (int j = 0, count = info.reward_info[i].reward_info.Count; j < count;j++)
                {
                    list.Add(new ItemValue((int)info.reward_info[i].reward_info[j].item_id, (int)info.reward_info[i].reward_info[j].item_num));
                }
                rankRewardList.Add(list);
                rankRewardDes.Add(info.reward_info[i].desc);
            }
        }
        if(updateTreasureRank!=null)
        {
            updateTreasureRank();
        }
    }
    /// <summary>
    /// 宝藏活动至尊豪礼玩家排行数据
    /// </summary>
    public void S2C_GetRreasurePlayerRank(Pt _pt)
    {
        //Debug.Log("S2C_GetRreasurePlayerRank");
        pt_ranklist_d601 info = _pt as pt_ranklist_d601;
        //Debug.Log("排名info:" + info.rank + ",info.type:" + info.type);
        if (info != null && info.type == (byte)15)
        {
            rank = info.rank;
            rankList = info.ranklist;
            openTimes = info.value2;
            //for (int i = 0; i < info.ranklist.Count; i++)
            //{
            //    Debug.Log("排行榜玩家：" + info.ranklist[i].name);
            //}
        }
        if(updateTreasureRankInfo!=null)
        {
            updateTreasureRankInfo();
        }
    }
    #endregion
    #region C2S协议
    /// <summary>
    /// 请求宝藏活动预览数据
    /// </summary>
    public void C2S_ReqTreasurePreviewInfo()
    {
        //Debug.Log("C2S_ReqTreasurePreviewInfo");
        pt_req_treasure_info_d970 msg = new pt_req_treasure_info_d970();
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 开启宝藏的请求协议
    /// </summary>
    public void C2S_ReqOpenTreasureOnce(int _type)
    {
        pt_req_treasure_lottery_d976 msg = new pt_req_treasure_lottery_d976();
        msg.type = (byte)_type;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求全民赢大奖的数据
    /// </summary>
    public void C2S_ReqTreasurebigPrize()
    {
        //Debug.Log("C2S_ReqTreasurebigPrize");
        pt_req_treasure_times_reward_info_d974 msg = new pt_req_treasure_times_reward_info_d974();
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求全民赢大奖的奖励
    /// </summary>
    public void C2S_ReqTreasurebigPrizeReward(uint _id)
    {
        //Debug.Log("pt_req_get_treasure_reward_d978:");
        pt_req_get_treasure_reward_d978 msg = new pt_req_get_treasure_reward_d978();
        msg.id = _id;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求至尊豪礼的奖励
    /// </summary>
    public void C2S_ReqTreasureRankReward()
    {
        pt_req_treasure_rank_reward_info_d972 msg = new pt_req_treasure_rank_reward_info_d972();
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求至尊豪礼排行
    /// </summary>
    public void C2S_ReqTreasureRank(int _type,int _page)
    {
        //Debug.Log("请求至尊豪礼排行");
        pt_ranklist_d600 msg = new pt_ranklist_d600();
        msg.type = (byte)_type;
        msg.page = (byte)_page;
        NetMsgMng.SendMsg(msg);
    }
    #endregion
    #endregion
}
